using DirecLayer;
using DomainLayer.Models;
using MetroFramework;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using zDeclare;

namespace PresenterLayer
{

    public partial class InventoryCount_Posting : MetroForm
    {
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        DataAccessSQL da_sql = new DataAccessSQL();
        DECLARE dc = new DECLARE();
        DataAccess da = new DataAccess();
        DateTime localDate = DateTime.Now;
        bool _updateStatus = false;
        private void InventoryCount_Posting_Resize(object sender, EventArgs e)
        {
            //WindowState == FormWindowState.Maximized;
            Form frm = Application.OpenForms["frmITM_UDF"];
            if (frm != null)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    int udf_width = frm.Width;
                    Size = new Size(MdiParent.ClientSize.Width - udf_width, max_height);
                    WindowState = FormWindowState.Normal;
                    Location = new Point(0, 0);
                }
            }
            else
            { WindowState = FormWindowState.Normal; }
        }

        int max_height = Screen.PrimaryScreen.Bounds.Height - 200;
        int max_width = Screen.PrimaryScreen.Bounds.Width - 100;

        int iColumn = 1, iRow = 0;

        public DataTable dtItems, dtWhs, dtActive;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult result = MetroMessageBox.Show(StaticHelper._MainForm, "UPLOAD TO SAP ?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    UpdateCounts();
                    UploadtoSap();
                    ReshreshAll("1");
                    //if (_updateStatus == true)
                    //{
                    //    string _docEntry = "";
                    //    string docEntry_in = "";

                    //    foreach (DataGridViewRow row1 in dgvItems.Rows)
                    //    {
                    //        if (Convert.ToBoolean(row1.Cells[0].Value))
                    //        { _docEntry += row1.Cells[1].Value + ","; ; }
                    //    }
                    //    docEntry_in = _docEntry.Remove(_docEntry.Length - 1);
                    //    string sdata = "SELECT DISTINCT WhsCode FROM INC1 ";
                    //    sdata += " WHERE HeaderId in (" + docEntry_in + ") ORDER BY WhsCode ASC";

                    //    UpdateDocStatus(docEntry_in);
                    //    ReshreshAll("1");
                    //}
                }





            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }

        }

        void UploadTOTempTable(string query, string DocEntry, int icnt)
        {

            var dtItems = msSql.Get(query);
            int oLimit = 5000;
            int oCounter = 0;
            int ooDocEntry = 1;

            foreach (DataRow x in dtItems.Rows)
            {
                //MessageBox.Show(x["ItemCode"].ToString(), x["WhsCode"].ToString());

                DataTable dt = new DataTable();

                dt = hana.Get($"SELECT InvntItem,ItemCode,frozenFor FROM OITM WHERE InvntItem = 'Y' AND frozenFor = 'N' and ItemCode = '{x["ItemCode"].ToString()}'");
                if (helper.DataTableExist(dt))
                {
                    oCounter = oCounter + 1;
                    if (oCounter == oLimit + 1)
                    { ooDocEntry = ooDocEntry + 1; oCounter = 1; }

                    DECLARE._OINC.Add(new DECLARE.xOINC
                    {
                        oDocEntry = ooDocEntry.ToString(),
                        oWhsCode = x["WhsCode"].ToString(),
                        oItemCode = x["ItemCode"].ToString(),
                        oQty = x["Quantity"].ToString()
                    });
                }
                else
                {
                    var dtday = DateTime.Now;
                    var datenow = dtday.ToString("MMddyyyy");
                    string sdata;
                    sdata = $" INSERT INTO [dbo].[tmpItems] ";
                    sdata += $" (itemcode,whscode,quantity,docdate)";
                    sdata += $" VALUES('{x["ItemCode"].ToString()}','{x["WhsCode"].ToString()}','{x["Quantity"].ToString()}','{datenow}') ";
                    msSql.Execute(sdata);
                }
            }

            dataGridView1.DataSource = DECLARE._OINC.GroupBy(x => x.oDocEntry).ToList();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string b = dtPostingDate.Text;
                var txtMyDate = Convert.ToDateTime(b);
                b = txtMyDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                Dictionary<string, string> dict = new Dictionary<string, string>()
                    {
                        {"CountDate", Convert.ToDateTime(b).ToString("yyyyMMdd")},
                        {"CountTime", txtTime.Text},
                        {"CountingType", "1"},
                        {"SingleCounterType", "ctUser"},
                        {"Reference2", row.Cells[0].Value.ToString()},
                        {"Remarks",  $"Created by EasySAP | Data Transfer : {DateTime.Now} | Powered By : DIREC"},
                    {"U_Remarks",txtRemarks.Text }
                    };

                int countlines = 0;
                foreach (var saplines in DECLARE._OINC.Where(a => a.oDocEntry == row.Cells[0].Value.ToString()))
                {
                    countlines = countlines + 1;
                }

                int iMax = countlines;
                int oCnt = 0;

                List<Dictionary<string, object>> dictLines = new List<Dictionary<string, object>>();

                foreach (var saplines in DECLARE._OINC.Where(a => a.oDocEntry == row.Cells[0].Value.ToString()))
                {
                    var lines = new Dictionary<string, object>();

                    lines.Add("ItemCode", saplines.oItemCode.ToString());
                    lines.Add("WarehouseCode", saplines.oWhsCode.ToString());
                    lines.Add("Counted", "Y");
                    lines.Add("CountedQuantity", saplines.oQty.ToString());

                    oCnt += 1;

                    StaticHelper._MainForm.Progress($"Uploading {saplines.oWhsCode.ToString()} {oCnt} out of {iMax}", oCnt, iMax);

                    dictLines.Add(lines);
                }

                // add to service layer
                string returnvalue = string.Empty;
                var json = DataRepository.JsonBuilder(dict, dictLines, "InventoryCountingLines");
                var serviceLayerAccess = new ServiceLayerAccess();
                bool isPosted = serviceLayerAccess.ServiceLayer_Posting(json, "POST", $"InventoryCountings", "DocumentEntry", out returnvalue, out string val);

                //bool isPosted = SAPHana.SL_AJAX((value) => returnvalue = value, "InventoryCountings", "POST", json.ToString(), "DocEntry");
                if (isPosted)
                {
                    DECLARE._OINC.Clear();
                    dataGridView1.DataSource = null;

                    //New Logic in getting DocNum of transaction 01/15/2020
                    List<string> DocList = DocEntry.Split(',').ToList<string>();
                    string strDocEntry = DocList[icnt].Replace(" ", "");

                    //UpdateDocStatus(ooDocEntry.ToString());
                    UpdateDocStatus(strDocEntry);
                }
                else
                {
                    DECLARE._OINC.Clear();
                    StaticHelper._MainForm.ShowMessage(returnvalue, true);
                }
            }
        }

        void UploadtoSap()
        {
            try
            {
                bool Error = false;
                int countChecked = 0;
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        countChecked += 1;
                    }
                }

                if (countChecked != 0 && txtRemarks.Text != "")
                {
                    string _docEntry = "";
                    string docEntry_in = "";

                    foreach (DataGridViewRow row in dgvItems.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        { _docEntry += row.Cells[1].Value + ","; ; }
                    }
                    docEntry_in = _docEntry.Remove(_docEntry.Length - 1);
                    var sdata = $"SELECT T1.WhsCode FROM OINC T0 INNER JOIN INC1 T1 ON T0.DocEntry = T1.HeaderId WHERE T0.DocStatus IN ('O','R') AND T0.Canceled = 'N' AND T0.DocEntry IN ({docEntry_in}) GROUP BY T1.WhsCode";

                    var dtItems = msSql.Get(sdata);

                    int icnt = 0;
                    foreach (DataRow x in dtItems.Rows)
                    {
                        sdata = "SELECT ItemCode, WhsCode, SUM(Quantity) as Quantity FROM INC1";
                        sdata += " WHERE HeaderId IN(" + docEntry_in + ") AND WhsCode = '" + x["WhsCode"].ToString() + "' ";
                        sdata += " GROUP BY ";
                        sdata += " ItemCode, WhsCode ";
                        sdata += " ORDER BY ItemCode ";
                        
                        UploadTOTempTable(sdata, docEntry_in, icnt);
                        icnt+=1;
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Fill up mandatory fields!", true);
                    _updateStatus = false;
                }
            }

            catch (Exception ex)
            {
                _updateStatus = false;
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        #region AddtoSAP
        //void AddtoSap()
        //{
        //    string _docEntry = "";
        //    string docEntry_in = "";

        //    string q = " SELECT DocEntry,RefNo,ISNULL(Comments,'') as Comments,SapUsername,CAST(DocDate as date) as DocDate ";
        //    q = q + " ,CAST(CountDate as Date) as CountDate ";
        //    q = q + " ,CAST(Time as time) as Time,PreparedBy,NotedBy,Remarks,DocStatus FROM OINC ";
        //    q = q + " WHERE DocStatus IN ('O','R') AND Canceled = 'N'";
        //    q = q + " ORDER BY DocEntry DESC ";

        //    SAPbobsCOM.CompanyService oCS = SAPAccess.oCompany.GetCompanyService();
        //    SAPbobsCOM.InventoryCountingsService oICS = oCS.GetBusinessService(ServiceTypes.InventoryCountingsService);
        //    SAPbobsCOM.InventoryCounting oIC = oICS.GetDataInterface(InventoryCountingsServiceDataInterfaces.icsInventoryCounting);

        //    oIC.CountDate = DateTime.Now;
        //    //oIC.CountingType = CountingTypeEnum.ctSingleCounter;
        //    oIC.SingleCounterType = CounterTypeEnum.ctEmployee;
        //    oIC.SingleCounterID = 23;
        //    //oIC.CountTime = DateTime.Now;
        //    oIC.Remarks = txtRemarks.Text;

        //    foreach (DataGridViewRow row in dgvItems.Rows)
        //    {
        //        if (row.Cells[0].Value != null)
        //        {
        //            if ((bool)row.Cells[0].Value == true)
        //            {
        //                _docEntry += row.Cells[1].Value + ",";
        //            }
        //        }

        //    }
        //    docEntry_in = _docEntry.Remove(_docEntry.Length - 1);

        //    DataTable dtItems = DataAccessSQL.Select(DataAccess.conStr("SAO"), "SELECT ItemCode,WhsCode,Sum(Quantity) as Quantity FROM INC1 " +
        //                                                                   $"WHERE DocEntry in ({docEntry_in}) Group By ItemCode,WhsCode");
        //    foreach (DataRow x in dtItems.Rows)
        //    {
        //        SAPbobsCOM.InventoryCountingLines oICL = oIC.InventoryCountingLines;
        //        SAPbobsCOM.InventoryCountingLine line = oICL.Add();

        //        //Loop Items
        //        line.ItemCode = x["ItemCode"].ToString();
        //        line.WarehouseCode = x["WhsCode"].ToString();
        //        line.Counted = BoYesNoEnum.tYES;
        //        line.CountedQuantity = Convert.ToInt32(x["Quantity"]);
        //    }

        //    //line = oICL.Add();
        //    try
        //    {
        //        SAPbobsCOM.InventoryCountingParams oICP = oICS.Add(oIC);
        //        UpdateDocStatus(docEntry_in);
        //        LoadInvCounts(q);
        //        txtRemarks.Text = "";
        //        frmMain.NotiMsg("Operation completed successfully", Color.Green);
        //    }
        //    catch (Exception ex)
        //    {
        //        frmMain.NotiMsg(ex.Message, Color.Red);
        //    }
        //}
        #endregion

        void LoadInvCounts(string q)
        {
            dgvItems.DataSource = null;

            var dt = msSql.Get(q);
            //add checkbox column
            var col = new DataGridViewCheckBoxColumn();
            col.Name = "Chk";
            col.HeaderText = "";
            col.ReadOnly = false;

            txtTime.Text = localDate.ToString("hh:mmtt", CultureInfo.InvariantCulture).ToString();
            dgvItems.Columns.Clear();
            dgvItems.Columns.Add(col);
            dgvItems.DataSource = dt;
            dataGridLayout(dgvItems);
        }

        public static void dataGridLayout(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[2].ReadOnly = true;
            dgv.Columns[4].ReadOnly = true;
            dgv.Columns[5].ReadOnly = true;
            dgv.Columns[6].ReadOnly = true;
            dgv.Columns[7].ReadOnly = true;
            dgv.Columns[8].ReadOnly = true;
            dgv.Columns[9].ReadOnly = true;
            dgv.Columns[10].ReadOnly = true;
            dgv.Columns[11].ReadOnly = true;
            dgv.Columns[12].ReadOnly = true;
            //dgv.Columns[13].ReadOnly = true;
        }

        void UpdateDocStatus(string DocEntry)
        {

            try
            {
                msSql.Execute($"Update OINC Set DocStatus ='C' Where DocEntry in ({DocEntry})");
            }
            catch (Exception ex) { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "CANCEL TRANSACTION?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        try
                        {

                            var oSettings = Properties.Settings.Default;
                            var oCheck = $"SELECT TOP 1 U_User,U_UpdateCount FROM OHEM WHERE empID = '{EasySAPCredentialsModel.GetEmployeeCode()}' AND U_UpdateCount = 'Y'";

                            var dt1 = hana.Get(oCheck);

                            if (helper.DataTableExist(dt1))
                            {
                                msSql.Execute($"Update OINC Set Canceled ='Y' Where DocEntry in ({dgvItems.Rows[dgvItems.CurrentCell.RowIndex].Cells["DocEntry"].Value.ToString()})");
                                ReshreshAll("1");
                                checker();
                            }
                            else
                            {
                                StaticHelper._MainForm.ShowMessage("Not authorized to cancel document!", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            ReshreshAll("1");
                            checker();
                            StaticHelper._MainForm.ShowMessage(ex.Message, true);
                        }
                    }
                    catch (Exception ex)
                    { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }


        }

        private void dgvItems_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    dgvItems.CurrentCell = dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvItems.Rows[e.RowIndex].Selected = true;
                    dgvItems.Focus();

                    var mousePosition = dgvItems.PointToClient(Cursor.Position);

                    msItemsList.Show(dgvItems, mousePosition);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                // This will check the cell.
                row.Cells[0].Value = checkBox1.Checked.ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            try
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Transaction not be saved?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Dispose();
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }

        }

        public void checker()
        {
            try
            {
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    var chk = (DataGridViewCheckBoxCell)row.Cells[0];
                    if (row.Cells["DocStatus"].Value.ToString() == "R")
                    {
                        chk.Value = true;
                    }
                    else { chk.Value = false; }
                }

                foreach (DataGridViewColumn col in dgvItems.Columns)
                {

                    if (col.HeaderText == "DocStatus" || col.HeaderText == "DocEntry")
                    {
                        col.Visible = false;
                    }
                }


            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }


            try
            {
                string sdata;
                double _TotalQty;
                sdata = " SELECT SUM(T1.Quantity) as qty FROM OINC T0 INNER JOIN INC1 T1 ON T0.DocEntry = T1.HeaderId ";
                sdata = sdata + " WHERE T0.DocStatus IN ('O','R') AND Canceled = 'N' AND ISNULL(T0.WhsCode,'') <> ''";

                var dtcheck = msSql.Get(sdata);

                if (helper.DataTableExist(dtcheck))
                {
                    sdata = helper.ReadDataRow(dtcheck, "qty", "", 0);

                    _TotalQty = Convert.ToDouble(sdata);
                    lblTotalCount.Text = _TotalQty.ToString("#,##0.00");
                }
                else
                {
                    lblTotalCount.Text = "0.00";
                }
            }
            catch (Exception ex) { lblTotalCount.Text = "0.00"; }


        }

        private void InventoryCount_Posting_Shown(object sender, EventArgs e)
        {
            checker();
        }

        private void msItemsList_Opening(object sender, CancelEventArgs e)
        {

        }

        public InventoryCount_Posting()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            msSql = new SAPMsSqlAccess();
            helper = new DataHelper();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "UPDATE TRANSACTION?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var frmInventoryCount = new frmInventoryCount();
                        frmInventoryCount.oMode = "U";
                        frmInventoryCount.oDocEntry = dgvItems.Rows[dgvItems.CurrentCell.RowIndex].Cells["DocEntry"].Value.ToString();
                        FormHelper.ShowForm(frmInventoryCount, true);
                        ReshreshAll("1");
                        checker();
                    }
                    catch (Exception ex)
                    { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }





        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "UPDATE REVIEWS ?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UpdateCounts();
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }

        }

        public void UpdateCounts()
        {
            try
            {
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if ((Boolean)((DataGridViewCheckBoxCell)row.Cells[0]).FormattedValue)
                    {
                        string sdata;
                        sdata = "UPDATE OINC SET DocStatus= 'R',Comments = '" + row.Cells[3].Value.ToString() + "' Where DocEntry = '" + row.Cells[1].Value.ToString() + "'";
                        msSql.Execute(sdata);
                    }
                    else
                    {
                        string oUpdate;
                        oUpdate = "UPDATE OINC SET DocStatus = 'O',Comments = '" + row.Cells[3].Value.ToString() + "' WHERE DocEntry = " + row.Cells[1].Value.ToString() + "";
                        msSql.Execute(oUpdate);
                    }
                }

                StaticHelper._MainForm.ShowMessage("Updated Successfully");
                txtSearch.Text = "";
                ReshreshAll("1");
                checker();
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }


        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvItems.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReshreshAll("2");
            checker();
        }

        private void InventoryCount_Posting_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
            try
            {
                var localDate = DateTime.Now;
                dtPostingDate.Text = localDate.ToString("MM/dd/yyyy");
                txtTime.Text = localDate.ToString("hh:mmtt");
                MaximumSize = new Size(max_width, max_height);
                ReshreshAll("1");
                txtSearch.Focus();

                var oSettings = Properties.Settings.Default;
                var oCheck = $"SELECT TOP 1 U_User,U_UpdateCount FROM OHEM WHERE empID = '{EasySAPCredentialsModel.GetEmployeeCode()}' AND U_UpdateCount = 'Y'";

                var dt1 = hana.Get(oCheck);

                if (helper.DataTableExist(dt1) == false)
                { btnAdd.Enabled = false; btnUpdateRevies.Enabled = false; }

            }
            catch (Exception ex)
            {
                MaximumSize = new Size(max_width, max_height);
                ReshreshAll("1");
                txtSearch.Focus();
            }

            txtSearch.Focus();
        }

        void ReshreshAll(string oMode)
        {
            string q = "";
            switch (oMode)
            {
                case "1":
                    //q = " SELECT T0.DocEntry,T0.RefNo,ISNULL(T0.Comments,'') as Comments,T0.SapUsername,CAST(T0.DocDate as date) as DocDate ";
                    //q = q + " ,CAST(T0.CountDate as Date) as CountDate ";
                    //q = q + " ,CAST(T0.Time as time) as Time,T0.PreparedBy,T0.NotedBy,T0.Remarks,T0.DocStatus ";
                    //q = q + " ,(SELECT SUM(Quantity) as TotalQty FROM INC1 WHERE DocEntry = T0.DocEntry) as TotalQty FROM OINC T0 ";
                    //q = q + " WHERE T0.DocStatus IN ('O','R') AND Canceled = 'N' ORDER BY T0.DocEntry DESC ";

                    q = " SELECT T0.DocEntry,T0.RefNo,ISNULL(T0.Comments,'') as Comments,T0.SapUsername,CAST(T0.DocDate as date) as DocDate ";
                    q = q + " ,(SELECT SUM(Quantity) as TotalQty FROM INC1 WHERE HeaderId = T0.DocEntry) as TotalQty";
                    q = q + " ,T0.WhsCode,T0.PreparedBy,T0.NotedBy,T0.Remarks,T0.DocStatus ";
                    q = q + " ,CAST(T0.CheckedBy as nvarchar(50)) as CheckedBy FROM OINC T0 ";
                    q = q + " WHERE T0.DocStatus IN ('O','R') AND Canceled = 'N' AND ISNULL(T0.WhsCode,'') <> '' ORDER BY T0.DocEntry DESC ";

                    var dt1 = msSql.Get(q);
                    if (helper.DataTableExist(dt1))
                    {
                        LoadInvCounts(q);
                        //FreezeBand(dgvItems.Rows[0]);
                        this.dgvItems.Columns[0].Frozen = true;
                    }
                    else
                    {
                        try
                        {
                            StaticHelper._MainForm.ShowMessage("NO TRANSACTION TO BE POSTED ON SAP", true);
                            this.Close();
                        }
                        catch (Exception ex) { }
                    }

                    break;

                case "2":
                    //q = " SELECT T0.DocEntry,T0.RefNo,ISNULL(T0.Comments,'') as Comments,T0.SapUsername,CAST(T0.DocDate as date) as DocDate ";
                    //q = q + " ,CAST(T0.CountDate as Date) as CountDate ";
                    //q = q + " ,CAST(T0.Time as time) as Time,T0.PreparedBy,T0.NotedBy,T0.Remarks,T0.DocStatus,(SELECT SUM(Quantity) as TotalQty FROM INC1 WHERE DocEntry = T0.DocEntry) as TotalQty FROM OINC T0 ";
                    //q = q + " WHERE T0.DocStatus IN ('O','R') AND (T0.RefNo LIKE '%" + txtSearch.Text + "%' OR T0.SapUsername LIKE '%" + txtSearch.Text + "%')";
                    //q = q + " AND Canceled = 'N' ORDER BY T0.DocEntry DESC ";

                    q = " SELECT T0.DocEntry,T0.RefNo,ISNULL(T0.Comments,'') as Comments,T0.SapUsername,CAST(T0.DocDate as date) as DocDate ";
                    q = q + " ,(SELECT SUM(Quantity) as TotalQty FROM INC1 WHERE HeaderId = T0.DocEntry) as TotalQty";
                    q = q + " ,T0.WhsCode,T0.PreparedBy,T0.NotedBy,T0.Remarks,T0.DocStatus ";
                    q = q + " ,CAST(T0.CheckedBy as nvarchar(50)) as CheckedBy FROM OINC T0 ";
                    q = q + " WHERE T0.DocStatus IN ('O','R') AND Canceled = 'N' AND (T0.RefNo LIKE '%" + txtSearch.Text + "%' OR T0.SapUsername LIKE '%" + txtSearch.Text + "%') ORDER BY T0.DocEntry DESC ";

                    var dtcheck = msSql.Get(q);
                    if (helper.DataTableExist(dtcheck) == false)
                    {
                        StaticHelper._MainForm.ShowMessage("NO REFERENCE AND USER FOUND !", true);
                        ReshreshAll("1");
                    }
                    else
                    {
                        var dt11 = msSql.Get(q);
                        if (helper.DataTableExist(dtcheck))
                        {
                            LoadInvCounts(q);
                            FreezeBand(dgvItems.Rows[0]);
                            //this.dgvItems.Columns[0].Frozen = true;
                        }
                        else
                        {
                            try
                            {
                                StaticHelper._MainForm.ShowMessage("NO TRANSACTION TO BE POSTED ON SAP", true);
                                Close();
                            }
                            catch (Exception ex) { }
                        }
                    }

                    txtSearch.Text = "";
                    txtSearch.Focus();
                    break;
            }
        }

        private static void FreezeBand(DataGridViewBand band)
        {
            band.Frozen = true;
            var style = new DataGridViewCellStyle();
            style.BackColor = Color.WhiteSmoke;
            band.DefaultCellStyle = style;
        }
    }
}

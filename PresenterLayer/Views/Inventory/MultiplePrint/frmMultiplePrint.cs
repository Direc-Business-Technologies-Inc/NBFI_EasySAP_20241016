using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Drawing.Printing;
using System.Windows.Forms.VisualStyles;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DirecLayer;
using PresenterLayer.Helper;
using MetroFramework;
using DomainLayer.Helper;
using DomainLayer.Models;

namespace PresenterLayer.Views
{
    public partial class frmMultiplePrint : MetroForm
    {
        private static int defaultColumn = 1, _rowIndex = 0;
        private static int defaultColumn2 = 1, _rowIndex2 = 0;
        public string objCode;
        public string userType;
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }

        public frmMultiplePrint()
        {
            InitializeComponent();
            var oSettings = Properties.Settings.Default;
            hana = new SAPHanaAccess();
            helper = new DataHelper();
            userType = EasySAPCredentialsModel.GetEmployeeCode();
        }
        
        public void LoadLayout(string userType)
        {
            var query = "SELECT DISTINCT B.SeqName, A.U_LayoutName " +
                           "FROM [@ADPL] A INNER JOIN RPRS B ON A.U_LayoutName = B.SeqName INNER JOIN " +
                           $"PRS1 C ON B.SeqID = C.SeqID WHERE A.U_UserCode = '{userType}'";
            
            var dtSource = hana.Get(query);

            cbDoc.DataSource = dtSource;
            
            cbDoc.DisplayMember = "SeqName";
            
            cbDoc.SelectedIndex = -1;
        }

        public void LoadDetails(string userType)
        {
            try
            {
                txtUser.Text = userType;
                var query = $"SELECT (LEFT(firstName,1) + '. ' + lastName) [Name] FROM OHEM Where U_User = '{userType}'";
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
            }
        }

        public void LoadUserLayout(string userType)//1
        {
            //string query = "SELECT TOP 1 U_LayoutName from [@ADPL] where U_UserCode = '" + txtUser.Text + "'";
            var query = "SELECT U_LayoutName from [@ADPL] where U_UserCode = '" + userType + "'";
            var dt = new DataTable();
            dt = hana.Get(query);
            if (dt.Rows.Count > 0)
            {
                
                var layout_name = helper.ReadDataRow(dt, "U_layoutname", "", 0);
                for (int i = 0; i < cbDoc.Items.Count; i++)
                {
                    string cbname = cbDoc.GetItemText(cbDoc.Items[i]);
                    if (layout_name == cbname)
                    {
                        cbDoc.SelectedIndex = i;
                        break;
                    }

                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("You can't use this module. User has no existing report allocation in SAP. Please contact IT Admin.",true);
            }

        }

        public void UponLoadData()
        {
            var qryTable = "SELECT U_Object from [@ADPL] where U_LayoutName = '" + cbDoc.Text + "'";
            var qry = "SELECT LaytCode from PRS1 T0 inner join RPRS T1 on T0.SeqID = T1.SeqID where T1.SeqID = '" + cbDoc.Text + "'";


            var dt = new DataTable();
            var dt2 = new DataTable();
            
            dt = hana.Get(qryTable);
            dt2 = hana.Get(qry);

            var tblName = "";

            if (dt.Rows.Count > 0)
            {
                
                tblName = helper.ReadDataRow(dt, "U_Object", "", 0);
                txtObjectID.Text = CodetoTable(tblName, "GetName");
                objCode = ObjtoTable(tblName);
            }
            else if (dt2.Rows.Count > 0)
            {
                int objid = Convert.ToInt16(helper.ReadDataRow(hana.Get("SELECT * from RPRS"), "ObjectID", 1, 0));
                
                string rcode = helper.ReadDataRow(hana.Get(qry), "LaytCode", "", 0);
                rcode = rcode.Substring(0, 3);
                tblName = CodetoTable(rcode, "GetName");
                txtObjectID.Text = tblName;
            }
            else

            return;
            
            var query = "SELECT DocNum, U_DRNo [DR No],DocDate [Date], CardName [Customer Name], U_Remarks [Remarks] from O" + CodetoTable(txtObjectID.Text,"GetCode") + " where (U_OrderStatus <> 'Printed' OR U_OrderStatus IS NULL) and U_DRNo <> '-' and U_DRNo  is not null  Order by DocNum desc";

            dgvDoc.Columns.Clear();
            dgvDoc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDoc.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvDoc.MultiSelect = false;
            dgvDoc.RowTemplate.Resizable = DataGridViewTriState.False;
            dgvDoc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDoc.RowHeadersVisible = false;
            dgvDoc.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            dgvDoc.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);

            //===============================HEADER WITH TICKBOX=========================
            var col1 = new DataGridViewCheckBoxColumn();
            var checkheader = new CheckBoxHeaderCell();
            checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
            col1.HeaderCell = checkheader;
            dgvDoc.Columns.Add(col1);
            
            dgvDoc.DataSource = hana.Get(query);
        }

        private void frmMultiplePrint_Load(object sender, EventArgs e)
        {
            LoadDetails(userType);
            LoadLayout(userType);
            LoadUserLayout(userType);
            UponLoadData();
            LoadLayoutandPrinters();
            cbDoc.SelectedIndex = -1;
        }

        public void DefValue()
        {
            foreach (DataGridViewRow row in dgcLayoutPrinter.Rows)
            {
                row.Cells["cmb"].Value = (row.Cells["cmb"] as DataGridViewComboBoxCell).Items[0];
                row.Cells["tb"].Value = "1";
            }
        }



        public void GetDatafromLayout()
        {
            string layoutname = cbDoc.Text;
            //get table name from layout SAP(UDO) [@ADPL]
        }

        public void dtFormat(DataGridView dt)
        {
            //dt.Rows.Clear();
            dt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dt.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dt.MultiSelect = false;
            dt.RowTemplate.Resizable = DataGridViewTriState.False;
            dt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dt.RowHeadersVisible = true;
            dt.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            dt.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);
        }

        #region Tickbox Header

        void checkheader_OnCheckBoxHeaderClick(CheckBoxHeaderCellEventArgs e)
        {
            if (dgvDoc.Rows.Count > 0)
            {
                dgvDoc.BeginEdit(true);
                foreach (DataGridViewRow item in dgvDoc.Rows)
                {
                    item.Cells[0].Value = e.IsChecked;
                }
                dgvDoc.EndEdit();
            }
            if (dgvPrinted.Rows.Count > 0)
            {
                dgvPrinted.BeginEdit(true);
                foreach (DataGridViewRow item in dgvPrinted.Rows)
                {
                    item.Cells[0].Value = e.IsChecked;
                }
                dgvPrinted.EndEdit();
            }
        }

        public class CheckBoxHeaderCellEventArgs : EventArgs
        {
            private bool _isChecked;
            public bool IsChecked
            {
                get { return _isChecked; }
            }

            public CheckBoxHeaderCellEventArgs(bool _checked)
            {
                _isChecked = _checked;

            }

        }

        public delegate void CheckBoxHeaderClickHandler(CheckBoxHeaderCellEventArgs e);

        class CheckBoxHeaderCell : DataGridViewColumnHeaderCell
        {
            Size checkboxsize;
            bool ischecked;
            Point location;
            Point cellboundsLocation;
            CheckBoxState state = CheckBoxState.UncheckedNormal;

            public event CheckBoxHeaderClickHandler OnCheckBoxHeaderClick;

            public CheckBoxHeaderCell()
            {
                location = new Point();
                cellboundsLocation = new Point();
                ischecked = false;
            }

            protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
            {
                /* Make a condition to check whether the click is fired inside a checkbox region */
                Point clickpoint = new Point(e.X + cellboundsLocation.X, e.Y + cellboundsLocation.Y);

                if ((clickpoint.X > location.X && clickpoint.X < (location.X + checkboxsize.Width)) && (clickpoint.Y > location.Y && clickpoint.Y < (location.Y + checkboxsize.Height)))
                {
                    ischecked = !ischecked;
                    if (OnCheckBoxHeaderClick != null)
                    {
                        OnCheckBoxHeaderClick(new CheckBoxHeaderCellEventArgs(ischecked));
                        this.DataGridView.InvalidateCell(this);
                    }
                }
                base.OnMouseClick(e);
            }

            protected override void Paint(Graphics graphics, Rectangle clipBounds,
                 Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle
                advancedBorderStyle, DataGridViewPaintParts paintParts)
            {

                base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState,
               value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);


                checkboxsize = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal);
                location.X = cellBounds.X + (cellBounds.Width / 2 - checkboxsize.Width / 2);
                location.Y = cellBounds.Y + (cellBounds.Height / 2 - checkboxsize.Height / 2);
                cellboundsLocation = cellBounds.Location;

                if (ischecked)
                    state = CheckBoxState.CheckedNormal;
                else
                    state = CheckBoxState.UncheckedNormal;

                CheckBoxRenderer.DrawCheckBox(graphics, location, state);

            }
        }

        #endregion

        private void cbDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            UponLoadData();
            LoadLayoutandPrinters();
        }

        private void dgvDoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dataGridView = (DataGridView)sender;
            var cell = dataGridView[0, e.RowIndex];

            if (cell.Value == null) cell.Value = false;
            //cell.Value = !(bool)cell.Value;

            if ((bool)cell.Value == false)
            {
                cell.Value = true;
                //MessageBox.Show("Checked");

                //string query = ""
            }
            else
            {
                cell.Value = false;
                //MessageBox.Show("Unchecked");
            }
        }

        public void LoadLayoutandPrinters()
        {
            dgcLayoutPrinter.Columns.Clear();

            var dt = new DataTable();
            
            var query = "SELECT T2.DocName [Layout], T0.Printer from PRS1 T0 " +
                           "INNER JOIN RPRS T1 on T0.SeqID = T1.SeqID " +
                           "inner join RDOC T2 on T0.LaytCode = T2.DocCode " + 
                           "where T1.SeqName = '" + cbDoc.Text + "' AND T1.ObjectID ='" + objCode + "'";
 
            dt = hana.Get(query);

            var cmb = new DataGridViewComboBoxColumn();
            cmb.HeaderText = "Select Printer";
            cmb.Name = "cmb";
            
            var tb = new DataGridViewTextBoxColumn();
            tb.HeaderText = "Qty";
            tb.Name = "tb";

            var printingBTN = new DataGridViewButtonColumn();
            printingBTN.HeaderText = "Action";
            printingBTN.Name = "printingBTN";
            printingBTN.Text = "Preview";
            printingBTN.UseColumnTextForButtonValue = true;

            dgcLayoutPrinter.DataSource = dt;
            dgcLayoutPrinter.Columns.Add(tb);
            dgcLayoutPrinter.Columns.Add(printingBTN);

            
            foreach (DataGridViewRow row in dgcLayoutPrinter.Rows)
            {
                row.Cells["tb"].Value = "1";
            }
            
        }
        
        private void setRowNumber(DataGridView dgv)
        {
            int i = 1;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells["SNO"].Value = i;
                i++;
            }
        }


        private static string CodetoTable(string code, string mode)
        {
            string res = "";
            if (mode == "GetCode")
            {
                if (code == "Transfer")
                {
                    res = "WTR";
                }
                else if (code == "Transfer Request")
                {
                    res = "WTQ";
                }
                else if (code == "Sales Invoice")
                {
                    res = "INV";
                }
            }
            else if (mode == "GetName")
            {
                if (code == "WTR")
                {
                    res = "Transfer";
                }
                else if (code == "WTQ")
                {
                    res = "Transfer Request";
                }
                else if (code == "INV")
                {
                    res = "Sales Invoice";
                }
            }

            return res;
        }

        private static string ObjtoTable(string code)
        {
            string res = "";

            if (code == "WTR")
            {
                res = "67";
            }
            else if (code == "WTQ")
            {
                res = "1250000001";
            }
            else if (code == "INV")
            {
                res = "13";
            }

            return res;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure do you want to print ?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //loop 1st all the check document on the left datagrid
                if (btnPrint.Text == "Print")
                {
                    int count = 0;
                    foreach (DataGridViewRow chrow in dgvDoc.Rows)
                    {
                        var ch = new DataGridViewCheckBoxCell();
                        ch = (DataGridViewCheckBoxCell)chrow.Cells[0];
                        if (ch.Value == null)
                            ch.Value = false;
                        if ((bool)ch.Value == true)
                        {
                            count++;
                        }
                    }
                    int counter = 1;
                    if (count != 0)
                    {
                        var res = true;
                        foreach (DataGridViewRow row in dgvDoc.Rows)
                        {

                            var ch1 = new DataGridViewCheckBoxCell();
                            ch1 = (DataGridViewCheckBoxCell)row.Cells[0];
                            if (ch1.Value == null)
                                ch1.Value = false;
                            if ((bool)ch1.Value == true)
                            {
                                var docnum = row.Cells["DocNum"].Value.ToString();
                                
                                var dockey = helper.ReadDataRow(hana.Get("SELECT DocEntry from O" + CodetoTable(txtObjectID.Text, "GetCode") + " where DocNum ='" + docnum + "'"), "DocEntry", "", 0);

                                //printer and layout get details right datagrid
                                foreach (DataGridViewRow row1 in dgcLayoutPrinter.Rows)
                                {

                                    var layoutname = row1.Cells["Layout"].Value.ToString();
                                    var printername = row1.Cells["Printer"].Value.ToString(); //"Microsoft XPS Document Writer";  row1.Cells["Printer"].Value.ToString();
                                    var sqty = row1.Cells["tb"].Value.ToString();
                                    int qty = Convert.ToInt16(sqty);

                                    //printing function

                                    if (Print(dockey, layoutname, qty, printername, txtUser.Text, cbDoc.Text) == true)
                                    {
                                        var qry = "UPDATE O" + CodetoTable(txtObjectID.Text,"GetCode") + " SET U_OrderStatus = 'Printed' where DocNum = '" + docnum + "'";
                                        hana.Execute(qry);

                                        //counter++;
                                        res = true;

                                    }
                                    else
                                    {
                                        StaticHelper._MainForm.ShowMessage("Error on printing function", true);
                                        res = false;
                                    }
                                }
                                //frmMain.Progress("Printing " + counter.ToString() + " out of " + count.ToString() + "document/s.", count);
                                StaticHelper._MainForm.Progress($"Printing {counter.ToString()} out of {count.ToString()} document/s.", counter, count);
                                counter++;
                            }
                        }
                        if (res == true)
                        {
                            UponLoadData();
                            StaticHelper._MainForm.ShowMessage(StaticHelper.OperationMessage);
                            StaticHelper._MainForm.ProgressClear();
                        }

                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Please select document/s to be printed.", true);
                    }
                }
                //end of first if
                else if (btnPrint.Text == "Update")
                {
                    int count = 0;
                    foreach (DataGridViewRow chrow in dgvPrinted.Rows)
                    {
                        var ch = new DataGridViewCheckBoxCell();
                        ch = (DataGridViewCheckBoxCell)chrow.Cells[0];
                        if (ch.Value == null)
                            ch.Value = false;
                        if ((bool)ch.Value == true)
                        {
                            count++;
                        }
                    }

                    if (count != 0)
                    {
                        bool res = false;

                        foreach (DataGridViewRow row in dgvPrinted.Rows)
                        {
                            var ch1 = new DataGridViewCheckBoxCell();
                            ch1 = (DataGridViewCheckBoxCell)row.Cells[0];

                            var cmb1 = new DataGridViewComboBoxCell();
                            cmb1 = (DataGridViewComboBoxCell)row.Cells[1];
                            string value = cmb1.Value.ToString();
                            string docnum = row.Cells["DocNum"].Value.ToString();

                            if (ch1.Value == null)
                                ch1.Value = false;
                            if ((bool)ch1.Value == true)
                            {
                                var query = "UPDATE O" + cbObj.Text + " SET U_OrderStatus = '" + value + "' where DocNum = '" + docnum + "'";
                                
                                if (hana.Execute(query) > 0)
                                {
                                    res = true;
                                    StaticHelper._MainForm.ShowMessage($"{count.ToString()} document/s updated.");
                                }
                            }
                        }
                        if (res == true)
                        {
                            UponLoadData();
                            LoadPrintedDoc(cbObj.Text);
                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Please select document to be printed.", true);
                    }
                }
            }
        }


        public static bool Print(string DocKey, string report, int copies, string printer,string zUserType,string zDocCode)
        {
            var _res = true;
            try
            {
                var cryRpt = new ReportDocument();
                //cryRpt.Load(DAL.Properties.Settings.Default.ReportPath + "Schedule Form" + ".rpt");
                var sys = new SystemSettings();
                var path = AppDomain.CurrentDomain.BaseDirectory + $"\\Crystals\\Forms\\Inv_PackList\\{report}.rpt";
                path = sys.PathExist(path);

                if (System.IO.File.Exists(path) == true)
                {
                    cryRpt.Load(path);
                }
                else
                {
                    

                    var query =  "SELECT T0.U_Path FROM [@ADPL] T0 " + 
                                    "LEFT JOIN RDOC T1 ON T1.DocCode = T0.U_LayoutName " +
                                    $"WHERE (T0.U_UserCode = '{zUserType}' and T0.U_LayoutName = '{zDocCode}')";
                    //temp path for my internal testing.
                    //string localpath = $"E:\\EDSEL\\EasySAP22817\\EASYSAP-rpt\\EASYSAP";
                    //real path came from SAP data
                    var hana = new SAPHanaAccess();
                    var helper = new DataHelper();
                    path = helper.ReadDataRow(hana.Get(query), "U_Path", "", 0) + $"\\{report}.rpt";
                    //path = DataAccess.Search(DataAccess.Select(DataAccess.conStr("HANA"), query), 0, "U_Path");
                    // path = localpath + $"\\{report}.rpt";
                    cryRpt.Load(path);
                }
                
                var crtableLogoninfos = new TableLogOnInfos();
                var crtableLogoninfo = new TableLogOnInfo();
                var crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                //cryRpt.SetDatabaseLogon("SYSTEM", "Sb1@dbsi", "DRIVER={B1CRHPROXY32};SERVERNODE=HANASERVERUAT:30015;DATABASE=SBOLIVE_WBII", "SBOLIVE_WBII");

                cryRpt.SetParameterValue("dockey@", DocKey);
                //crConnectionInfo.ServerName = "DRIVER={B1CRHPROXY32};SERVERNODE=HANASERVERUAT:30015;DATABASE=SBOLIVE_WBII"; //temp only for my local debuggin, if on client please change it.
                var sboCred = new SboCredentials();
                
                crConnectionInfo.ServerName = sboCred.DbServer;
                crConnectionInfo.DatabaseName = sboCred.Database;
                crConnectionInfo.UserID = sboCred.DbUserId;
                crConnectionInfo.Password = sboCred.DbPassword;

                CrTables = cryRpt.Database.Tables;
                foreach (Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                    string status = CrTable.LogOnInfo.ToString();
                }
                //cryRpt.PrintOptions.PrinterName = printer;
                var printerSettings = new PrinterSettings();
                printerSettings.PrinterName = printer;

                //if(cryRpt.PrintOptions.PrinterName == "")
                // {
                //     cryRpt.PrintOptions.PrinterName = printer;
                // }

                var printername = printerSettings.PrinterName;

                //cryRpt.Refresh();
                cryRpt.PrintToPrinter(printerSettings, new PageSettings(),false);
                
            }
            catch(Exception ex)
            {
                _res = false;
            }
            return _res;
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgcLayoutPrinter_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(tb_KeyPress);
            if (dgcLayoutPrinter.CurrentCell.ColumnIndex == 1) //Desired Column
            {
                var tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                }
            }
        }
        
        private void LoadObj()
        {
            cbObj.Items.Clear();
            var query = "SELECT DISTINCT U_Object from [@ADPL]";
            var dt = new DataTable();
            dt = hana.Get(query);
            foreach (DataRow dr in dt.Rows)
            {
                string code = dr["U_Object"].ToString();
                if (code == "WTR")
                {
                    cbObj.Items.Add("Transfer");
                }
                else if (code == "WTQ")
                {
                    cbObj.Items.Add("Transfer Request");
                }
            }
            //cbObj.DataSource = DataAccess.Select(DataAccess.conStr("HANA"), query);
            //cbObj.DisplayMember = "U_Object";
            //cbObj.ValueMember = "U_Object";
            cbObj.SelectedIndex = -1;
        }

        private void LoadPrintedDoc(string tblname)
        {
            var query = "SELECT DocNum, U_DRNo [DR No],DocDate [Date], CardName [Customer Name], U_Remarks [Remarks] from O" + tblname + " where U_OrderStatus = 'Printed' and U_DRNo <> '-' and U_DRNo  is not null  Order by DocNum desc";
            var dt = new DataTable();
            dt = hana.Get(query);

            dgvPrinted.Columns.Clear();
            
            //===============================HEADER WITH TICKBOX=========================
            var col1 = new DataGridViewCheckBoxColumn();
            var checkheader = new CheckBoxHeaderCell();
            checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
            col1.HeaderCell = checkheader;
            dgvPrinted.Columns.Add(col1);
            
            var dgvcmb = new DataGridViewComboBoxColumn();
            dgvcmb.HeaderText = "Status";
            dgvcmb.Items.AddRange("Printed", "Hold");
            dgvPrinted.Columns.Add(dgvcmb);

            foreach (DataGridViewRow row in dgvPrinted.Rows)
            {
                row.Cells[1].Value = (row.Cells[1] as DataGridViewComboBoxCell).Items[0];
            }

            //set cb's index

            dgvPrinted.DataSource = hana.Get(query);
            foreach (DataGridViewRow row in dgvPrinted.Rows)
            {
                row.Cells[1].Value = (row.Cells[1] as DataGridViewComboBoxCell).Items[0];
            }
            dtFormat(dgvPrinted);
            DataGridViewColumn dgc = dgvPrinted.Columns[0];
            dgc.Width = 50;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //check if user has authorization

            if (e.TabPageIndex == 1)
            {
                btnPrint.Text = "Update";
                LoadObj();
                var query = "SELECT U_UpdateCount from OHEM where U_User = '" + txtUser.Text + "'";
                var res = helper.ReadDataRow(hana.Get(query), "U_UpdateCount", "", 0);
                if (res == "N")
                {
                    StaticHelper._MainForm.ShowMessage("User has no authorization on this tab.", true);
                    cbObj.Enabled = false;
                    button1.Enabled = false;
                }
                else
                {
                    //try
                    //{
                    //    LoadPrintedDoc(cbObj.Text);
                    //}
                    //catch
                    //{
                    //    dgvPrinted.Columns.Clear();
                    //    return;
                    //}
                }
            }
            else
            {
                btnPrint.Text = "Print";
                cbObj.Enabled = true;
                button1.Enabled = true;
            }

        }

        private void dgvDoc_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (dgvDoc.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgvDoc.Rows)
                {
                    if (row.Cells[defaultColumn].Value.ToString().ToUpper().StartsWith(textBox5.Text.ToUpper()))
                    {
                        row.Selected = true;
                        _rowIndex = row.Index;
                        dgvDoc.FirstDisplayedScrollingRowIndex = _rowIndex;
                        break;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (dgvPrinted.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgvPrinted.Rows)
                {
                    if (row.Cells[defaultColumn].Value.ToString().ToUpper().StartsWith(textBox1.Text.ToUpper()))
                    {
                        row.Selected = true;
                        _rowIndex = row.Index;
                        dgvPrinted.FirstDisplayedScrollingRowIndex = _rowIndex;
                        break;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                var query = $"SELECT DocNum, U_DRNo [DR No],DocDate [Date], CardName [Customer Name], U_Remarks [Remarks] from  O{CodetoTable(txtObjectID.Text,"GetCode") } where DocDate between '{dtFrom.Value.ToString("yyyy-MM-dd")}' and '{dtTo.Value.ToString("yyyy-MM-dd")}' and  (U_OrderStatus <> 'Printed' OR U_OrderStatus IS NULL) and U_DRNo <> '-' and U_DRNo  is not null and U_PrepBy = '{txtPrepBy.Text}'  Order by DocNum desc";
                dgvDoc.Columns.Clear();
                dgvDoc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDoc.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvDoc.MultiSelect = false;
                dgvDoc.RowTemplate.Resizable = DataGridViewTriState.False;
                dgvDoc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvDoc.RowHeadersVisible = false;
                dgvDoc.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dgvDoc.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);

                //===============================HEADER WITH TICKBOX=========================
                var col1 = new DataGridViewCheckBoxColumn();
                var checkheader = new CheckBoxHeaderCell();
                checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
                col1.HeaderCell = checkheader;
                dgvDoc.Columns.Add(col1);
                dgvDoc.DataSource = hana.Get(query);

            }
            else
            {
                if (cbObj.Text != "" || cbObj.Text != null)
                {
                    var query = $"SELECT DocNum, U_DRNo [DR No],DocDate [Date], CardName [Customer Name], U_Remarks [Remarks] from O{CodetoTable(cbObj.Text,"GetCode")} where DocDate between '{dtFrom.Value.ToString("yyyy-MM-dd")}' and '{dtTo.Value.ToString("yyyy-MM-dd")}' and U_OrderStatus = 'Printed' and U_DRNo <> '-' and U_DRNo  is not null  Order by DocNum desc";
                    var dt = new DataTable();
                    dt = hana.Get(query);

                    dgvPrinted.Columns.Clear();



                    //===============================HEADER WITH TICKBOX=========================
                    var col1 = new DataGridViewCheckBoxColumn();
                    var checkheader = new CheckBoxHeaderCell();
                    checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
                    col1.HeaderCell = checkheader;
                    dgvPrinted.Columns.Add(col1);



                    var dgvcmb = new DataGridViewComboBoxColumn();
                    dgvcmb.HeaderText = "Status";
                    dgvcmb.Items.AddRange("Printed", "Hold");
                    dgvPrinted.Columns.Add(dgvcmb);

                    foreach (DataGridViewRow row in dgvPrinted.Rows)
                    {
                        row.Cells[1].Value = (row.Cells[1] as DataGridViewComboBoxCell).Items[0];
                    }

                    //set cb's index




                    dgvPrinted.DataSource = DataAccess.Select(DataAccess.conStr("HANA"), query);
                    foreach (DataGridViewRow row in dgvPrinted.Rows)
                    {
                        row.Cells[1].Value = (row.Cells[1] as DataGridViewComboBoxCell).Items[0];
                    }
                    dtFormat(dgvPrinted);
                    DataGridViewColumn dgc = dgvPrinted.Columns[0];
                    dgc.Width = 50;
                }
            }




        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //v1.24 7517
            var fS = new frmSearch2();
            fS.oSearchMode = "PREP";
            frmSearch2.Param1 = CodetoTable(txtObjectID.Text, "GetCode");
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                txtPrepBy.Text = fS.oCode;
                LoadData(fS.oCode);
            }
        }
        void LoadData(string PrepBy)
        {
            if (txtObjectID.Text != "" || txtObjectID.Text != null)
            {
                var query = $"SELECT DocNum, U_DRNo [DR No],DocDate [Date], CardName [Customer Name], U_Remarks [Remarks] from O{CodetoTable(txtObjectID.Text, "GetCode")} where (U_OrderStatus <> 'Printed' OR U_OrderStatus IS NULL) and U_DRNo <> '-' and U_DRNo  is not null and U_PrepBy = '{PrepBy}'  Order by DocNum desc";
                dgvDoc.Columns.Clear();
                dgvDoc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDoc.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvDoc.MultiSelect = false;
                dgvDoc.RowTemplate.Resizable = DataGridViewTriState.False;
                dgvDoc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvDoc.RowHeadersVisible = false;
                dgvDoc.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dgvDoc.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);

                //===============================HEADER WITH TICKBOX=========================
                var col1 = new DataGridViewCheckBoxColumn();
                var checkheader = new CheckBoxHeaderCell();
                checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
                col1.HeaderCell = checkheader;
                dgvDoc.Columns.Add(col1);
                dgvDoc.DataSource = DataAccess.Select(DataAccess.conStr("HANA"), query);
            }
        }

        private void dgcLayoutPrinter_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
        
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {

                int count = 0;
                foreach (DataGridViewRow chrow in dgvDoc.Rows)
                {
                    var ch = new DataGridViewCheckBoxCell();
                    ch = (DataGridViewCheckBoxCell)chrow.Cells[0];
                    if (ch.Value == null)
                        ch.Value = false;
                    if ((bool)ch.Value == true)
                    {
                        count++;
                    }
                }

                int counter = e.RowIndex;

                if (count != 0)
                {
                    bool res = true;

                    foreach (DataGridViewRow dgvRow in dgvDoc.Rows)
                    {
                        var row = new DataGridViewCheckBoxCell();
                        row = (DataGridViewCheckBoxCell)dgvRow.Cells[0];

                        if ((bool)row.Value == true)
                        {
                            var docnum = dgvRow.Cells["DocNum"].Value.ToString();
                            var queryString = "SELECT DocEntry FROM O" + CodetoTable(txtObjectID.Text, "GetCode") + " WHERE DocNum =" + docnum;
                            var dockey = helper.ReadDataRow(hana.Get(queryString), "DocEntry", "", 0);
                            
                            if (dgcLayoutPrinter.Rows[e.RowIndex].Cells[1].Value != null && dgcLayoutPrinter.Rows[e.RowIndex].Cells[2].Value != null)
                            {
                                var layout = dgcLayoutPrinter.Rows[e.RowIndex].Cells["Layout"].Value.ToString(); //dgcRow.Cells["Layout"].Value.ToString();
                                var printerName = dgcLayoutPrinter.Rows[e.RowIndex].Cells[1].Value.ToString();
                                int quantity = Convert.ToInt16(dgcLayoutPrinter.Rows[e.RowIndex].Cells[2].Value.ToString());

                                var a = new frmCrystalReports2();

                                a.zDocKey = dockey;
                                a.zLayoutName = layout;
                                a.zQty = quantity;
                                a.zPrinterName = printerName;
                                a.zUserType = txtUser.Text;
                                a.zDocCode = cbDoc.Text;

                                a.moduleType = "PL";
                                a.count = count;
                                a.Show(); 
                            }
                            else
                            {
                                StaticHelper._MainForm.ShowMessage("Select printer or enter quantity please",true);
                            }

                        }
                        else
                        {
                            row.Value = false;
                        }
                    }
                }

            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dgcLayoutPrinter_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadDetails(userType);
            LoadLayout(userType);
            LoadUserLayout(userType);
            UponLoadData();
            LoadLayoutandPrinters();
            cbDoc.SelectedIndex = 0;
        }

        private void frmMultiplePrint_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void dgvPrinted_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
            foreach (DataGridViewRow row in dgvPrinted.Rows)
            {
                row.Cells[1].Value = (row.Cells[1] as DataGridViewComboBoxCell).Items[0];
            }
        }

        private void cbObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbObj.Text == "Transfer")
                {
                    LoadPrintedDoc("WTR");
                }
                else if (cbObj.Text == "Transfer Request")
                {
                    LoadPrintedDoc("WTQ");
                }

            }
            catch
            {
                dgvPrinted.Columns.Clear();
                return;
            }
        }
    }
}

using MetroFramework.Forms;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using DirecLayer;
using PresenterLayer.Helper;
using ServiceLayer.Services;
using MetroFramework;

namespace PresenterLayer
{
    public partial class PackingList_Conssionaire : MetroForm
    {
        int max_width = Screen.PrimaryScreen.Bounds.Width - 220;
        int max_height = Screen.PrimaryScreen.Bounds.Height - 200;
        int iColumn = 1, iRow = 0;
        SAPHanaAccess hana { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        DataHelper helper { get; set; }
        public PackingList_Conssionaire()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            msSql = new SAPMsSqlAccess();
            helper = new DataHelper();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var fS = new frmSearch2();
            fS.oSearchMode = "OPKL";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                txtDRNo.Text = fS.oCode;
                lblDocEntry.Text = fS.oName;
                lblseries.Text = fS.oRate;

                var table = lblseries.Text == "CST" ? "WTR" : "INV";

                var query = $"SELECT DocEntry FROM O{table} Where DocEntry = '{lblDocEntry.Text}'";
                
                txtDocEntry.Text = helper.ReadDataRow(hana.Get(query),"DocEntry","",0);
            }
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            var a = new frmCrystalReports();
            a.type = "Concession";
            a.oDocKey = txtDocEntry.Text;
            a.ShowDialog();
        }

        private void PackingList_Conssionaire_Load(object sender, EventArgs e)
        {
            MaximumSize = new Size(max_width, max_height);
            NumSeries();
            CreateTable();
        }

        void Find(string DocEntry)
        {
            try
            {
                dgvItems.Rows.Clear();

                var query = "SELECT U_Identifier,U_StoreCode,U_StoreName,U_VendorCode,U_VendorName,U_DRNo " +
                               ",U_DeptCode,U_SubDeptCode,U_ClassCode,U_ClassDescr,U_TotalBox,U_BoxBdlNo,U_TotalDrAmt,U_LineEnder,U_Formula " +
                              $"FROM [@PKC1] Where DocEntry = '{DocEntry}' AND U_TotalBox is not null Order By U_BoxBdlNo ASC";
                var dt = hana.Get(query);

                foreach (DataRow row in dt.Rows)
                {
                    var Identifier = row["U_Identifier"].ToString();
                    var VendorCode = row["U_StoreCode"].ToString();
                    var VendorName = row["U_StoreName"].ToString();
                    var BranchCode = row["U_VendorCode"].ToString();
                    var BranchName = row["U_VendorName"].ToString();
                    var Dept = row["U_DeptCode"].ToString();
                    var SubDept = row["U_SubDeptCode"].ToString();
                    var Class = row["U_ClassCode"].ToString();
                    var SubClass = row["U_ClassDescr"].ToString();
                    var DRNo = row["U_DRNo"].ToString();
                    var DocTotal = string.Format("{0:#.##}", row["U_TotalDrAmt"].ToString());
                    var BoxTotal = row["U_TotalBox"].ToString();
                    var BoxNo = row["U_BoxBdlNo"].ToString();
                    var LineEnder = row["U_LineEnder"].ToString();
                    var Formula = row["U_Formula"].ToString();

                    object[] rowdata = { true, BoxNo, Identifier, VendorCode, VendorName, BranchCode, BranchName, DRNo,
                                         Dept, SubDept, Class, SubClass, BoxTotal, BoxNo, DocTotal, LineEnder,Formula };

                    dgvItems.Rows.Add(rowdata);
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        void EditableFields()
        {
            //foreach (DataGridViewColumn col in dgvItems.Columns)
            //{
            //    col.ReadOnly = col.Index != 0 | col.Index != 6 || col.Index != 7 || col.Index != 8 || col.Index != 9 ? true : false;
            //}
        }

        void AddLineItems(int DocEntry, int BoxNumber, string table)
        {
            dgvItems.Rows.Clear();

            List<string[]> row = new List<string[]>();

            try
            {
                var query = "SELECT " +
                ", A.DocNum [CST/AR No.]" +
                ", A.DocTotal " +
                ", A.CardCode " +
                ", (SELECT B.AliasName FROM OCRD B WHERE B.CardCode = A.CardCode) AS VendorCode " +
                ", (SELECT Z.Name FROM [@CMP_INFO] Z Where Z.Code = A.U_CompanyTIN) [CardName] " +
                ", (SELECT B.AddID FROM OCRD B WHERE B.CardCode = A.CardCode) AS BranchCode ,  A.CardName [CardName2]" +
                ", (SELECT Distinct D.U_Dept FROM CPN1 C INNER JOIN CPN2 D ON C.CpnNo = D.CpnNo WHERE C.BpCode = A.CardCode AND D.ItemCode in (B.ItemCode)) U_Dept " +
                ", (SELECT Distinct D.U_SubDept FROM CPN1 C INNER JOIN CPN2 D ON C.CpnNo = D.CpnNo WHERE C.BpCode = A.CardCode AND D.ItemCode in (B.ItemCode)) U_SubDept" +
                ", (SELECT Distinct D.U_Class FROM CPN1 C INNER JOIN CPN2 D ON C.CpnNo = D.CpnNo WHERE C.BpCode = A.CardCode AND D.ItemCode in (B.ItemCode)) U_Class " +
                ", (SELECT (Select Name from [@BRAND] WHERE Code = U_Brand) FROM OITM WHERE ItemCode = B.ItemCode) U_SubClass  " +
                $" FROM O{table} A " +
                $" INNER JOIN {table}1 B ON A.DocEntry = B.DocEntry WHERE A.DocEntry =  '{DocEntry}'";

                var dt = hana.Get(query);

                var dtcount = dt.Rows.Count;

                int count = 1;

                foreach (DataRow x in dt.Rows)
                {

                    string _branchCode, _dept, _subdept, _class;
                    var VendorCode = x["VendorCode"].ToString();
                    var VendorName = x["CardName"].ToString();
                    var BranchCode = x["BranchCode"].ToString();
                    var Dept = x["U_Dept"].ToString();
                    var SubDept = x["U_SubDept"].ToString();
                    var Class = x["U_Class"].ToString();
                    var SubClass = x["U_SubClass"].ToString();
                    var DRNo = x["CST/AR No."].ToString();
                    var DocTotal = string.Format("{0:#.##}", x["DocTotal"].ToString());
                    var storename = x["CardName2"].ToString();
                    var sCardCode = x["CardCode"].ToString();

                    var querySelect = "SELECT Distinct C.U_VendorID FROM OCPN A INNER JOIN CPN1 B ON A.CpnNo = B.CpnNo INNER JOIN CPN2 C ON B.CpnNo = C.CpnNo " +

                        $"WHERE B.BpCode = '{sCardCode}' AND IsNull(C.U_Dept, '') = '{Dept}' AND IsNull(C.U_SubDept, '') = '{SubDept}' AND IsNull(C.U_Class, '') = '{Class}' AND IsNull(C.U_SubClass, '') = '{SubClass}'";

                    var getDataSel = hana.Get(querySelect);

                    string sGetDataSel = "";

                    try
                    {
                        sGetDataSel = getDataSel.Rows[0]["U_VendorID"].ToString();
                    }
                    catch (Exception ex)
                    {
                        sGetDataSel = "";
                    }
                    


                    //branch
                    if (string.IsNullOrEmpty(BranchCode) == true)
                        _branchCode = "";
                    else
                    {
                        int result = 0;
                        var isINt = int.TryParse(BranchCode, out result);

                        if (isINt == true)
                        {
                            //MessageBox.Show("_branchCode");
                            _branchCode = Convert.ToInt32(BranchCode).ToString();
                        }
                        else
                            _branchCode = BranchCode;

                    }
                    //dept
                    if (string.IsNullOrEmpty(Dept) == true)
                        _dept = "";
                    else
                    {
                        var result = 0;
                        var isINt = int.TryParse(Dept, out result);

                        if (isINt == true)
                        {
                            _dept = Convert.ToInt32(Dept).ToString();
                        }
                        else
                            _dept = Dept;
                    }
                    //sub dept
                    if (string.IsNullOrEmpty(SubDept) == true)
                        _subdept = "";
                    else
                    {
                        //MessageBox.Show("_subdept");

                        var result = 0;
                        var isINt = int.TryParse(SubDept, out result);

                        if (isINt == true)
                        {
                            _subdept = Convert.ToInt32(SubDept).ToString();
                        }
                        else
                            _subdept = SubDept;

                    }
                    //sub class
                    if (string.IsNullOrEmpty(Class) == true)
                        _class = "";
                    else
                    {
                        //MessageBox.Show("_class");

                        var result = 0;
                        var isINt = int.TryParse(Class, out result);

                        if (isINt == true)
                        {
                            _class = Convert.ToInt32(Class).ToString();
                        }
                        else
                            _class = Class;
                    }


                    //string Formula = "SCDS" + "," + _branchCode + "," + VendorCode + "," + DRNo + ","
                    //    + _dept + "," + _subdept
                    //    + "," + _class + ", ," + count + "," + DocTotal + "," + "SMEND";



                    
                    var dtCN = hana.Get("SELECT CompnyName FROM OADM");
                    var CompanyName = helper.ReadDataRow(dtCN, "CompnyName", "", 0);

                    var count1 = count.ToString();
                    var SCDS = "SCDS";
                    var SMEND1 = "SMEND";

                    var Formula = $"{SCDS}, {BranchCode}, {VendorName}, {VendorCode}, {CompanyName}, {DRNo}, {Dept}, {SubDept}, {Class}, {SubClass}, {SMEND1}";

                    row.Add(new string[] { "", SCDS, BranchCode, VendorName, VendorCode, CompanyName, DRNo, Dept, SubDept, Class, SubClass, BoxNumber.ToString(), "", DocTotal, SMEND1, Formula, storename, sGetDataSel});

                    count++;
                }
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }

            Application.DoEvents();
            dgvItems.Rows.Clear();
            var grpRow = row.Distinct().GroupBy(x => x[15]).Select(x => x.First()).ToList();
            int footerCount = 1;
            int qweqweqwewqewq = Convert.ToInt32(numericUpDown1.Value);
            for (int i = 0; i < qweqweqwewqewq; i++)
            {
                foreach (var xx in grpRow)
                {
                    //dgvItems.Rows.Add(false, footerCount, xx[1], xx[2], xx[3], xx[4], xx[5], xx[6], xx[7], xx[8], xx[9], xx[10], xx[11], footerCount, xx[13], xx[14], xx[15]);
                    dgvItems.Rows.Add(false, footerCount, xx[1], xx[2], xx[16], xx[17], xx[3], xx[6], xx[7], xx[8], xx[9], xx[10], xx[11], footerCount, xx[13], xx[14], xx[15]);

                    footerCount++;
                }
            }
        }

        private void CreateTable()
        {
            var col0 = new DataGridViewCheckBoxColumn();
            var col1 = new DataGridViewTextBoxColumn();
            var col2 = new DataGridViewTextBoxColumn();
            var col3 = new DataGridViewTextBoxColumn();
            var col4 = new DataGridViewTextBoxColumn();
            var col5 = new DataGridViewTextBoxColumn();
            var col6 = new DataGridViewTextBoxColumn();
            var col7 = new DataGridViewTextBoxColumn();
            var col8 = new DataGridViewTextBoxColumn();
            var col9 = new DataGridViewTextBoxColumn();
            var col10 = new DataGridViewTextBoxColumn();
            var col11 = new DataGridViewTextBoxColumn();
            var col12 = new DataGridViewTextBoxColumn();
            var col13 = new DataGridViewTextBoxColumn();
            var col14 = new DataGridViewTextBoxColumn();
            var col15 = new DataGridViewTextBoxColumn();
            var col16 = new DataGridViewTextBoxColumn();


            col0.ValueType = typeof(bool);
            col0.Name = "Chk";
            col0.HeaderText = "";

            col1.Name = "ID";
            col1.HeaderText = "#";
            col1.ReadOnly = true;

            col2.Name = "Identifier";
            col2.HeaderText = "Identifier";
            col2.ReadOnly = true;

            col3.Name = "StoreCode";
            col3.HeaderText = "Store Code";
            col3.ReadOnly = true;

            col4.Name = "StoreName";
            col4.HeaderText = "Store Name";
            col4.ReadOnly = true;

            col5.Name = "VendorCode";
            col5.HeaderText = "Vendor Code";
            col5.ReadOnly = true;

            col6.Name = "VendorName";
            col6.HeaderText = "Vendor Name";
            col6.ReadOnly = true;

            col7.Name = "CST/AR No.";
            col7.HeaderText = "CST/AR No.";
            col7.ReadOnly = true;

            col8.Name = "DeptCode";
            col8.HeaderText = "Department";
            col8.ReadOnly = true;

            col9.Name = "SubDept";
            col9.HeaderText = "Sub Department";
            col9.ReadOnly = true;

            col10.Name = "ClassCode";
            col10.HeaderText = "Class Code";
            col10.ReadOnly = true;

            col11.Name = "ClassDesc";
            col11.HeaderText = "Class Description";
            col11.ReadOnly = true;

            col12.Name = "TotalBox";
            col12.HeaderText = "Total No. of Carton";
            col12.ReadOnly = true;

            col13.Name = "BoxNo";
            col13.HeaderText = "Carton/BDL No.";
            col13.ReadOnly = true;

            col14.Name = "TotalDR";
            col14.HeaderText = "Total DR Amount";
            col14.ReadOnly = true;

            col15.Name = "LineEnd";
            col15.HeaderText = "Line Ender";
            col15.ReadOnly = true;

            col16.Name = "Formula";
            col16.HeaderText = "Formula";
            col16.ReadOnly = true;

            dgvItems.Columns.Add(col0);
            dgvItems.Columns.Add(col1);
            dgvItems.Columns.Add(col2);
            dgvItems.Columns.Add(col3);
            dgvItems.Columns.Add(col4);
            dgvItems.Columns.Add(col5);
            dgvItems.Columns.Add(col6);
            dgvItems.Columns.Add(col7);
            dgvItems.Columns.Add(col8);
            dgvItems.Columns.Add(col9);
            dgvItems.Columns.Add(col10);
            dgvItems.Columns.Add(col11);
            dgvItems.Columns.Add(col12);
            dgvItems.Columns.Add(col13);
            dgvItems.Columns.Add(col14);
            dgvItems.Columns.Add(col15);
            dgvItems.Columns.Add(col16);

            dataGridLayout(dgvItems);
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            ReloadLineItems();
        }

        void ReloadLineItems()
        {
            if (txtDRNo.Text != "")
            {
                string series = lblseries.Text == "CST" ? "WTR" : "INV";
                AddLineItems(Convert.ToInt32(lblDocEntry.Text), Convert.ToInt32(numericUpDown1.Value), series);
                EditableFields();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0)
            {
                StaticHelper._MainForm.ShowMessage("Please input total no. of Carton.", true);
            }
            else
            {
                List<DataGridViewRow> list = dgvItems.Rows.Cast<DataGridViewRow>().Where(k => (bool)k.Cells[0].Value == true).ToList();

                int cnttrue = 0;
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    DataGridViewCheckBoxCell checkrow = row.Cells[0] as DataGridViewCheckBoxCell;
                    if ((bool)checkrow.Value == true)
                    {
                        cnttrue++;
                    }
                }

                if (numericUpDown1.Value >= cnttrue)
                {
                    AddtoSAP();
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Selected rows. is greater than the Total no. of Carton. Please increase Total no. of Carton.", true);
                }
            }
        }

        public static void dataGridLayout(DataGridView dgv)
        {
            dgv.ReadOnly = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
        }
        void ClearData()
        {
            dgvItems.Rows.Clear();

            txtDRNo.Text = "";
            txtRemarks.Text = "";
            numericUpDown1.Value = 0;

            NumSeries();

        }
        void NumSeries()
        {
            var q = "SELECT (MAX(DocNum) + 1) [DocEntry] FROM [@OPKC]";
            var dt = hana.Get(q);
            txtDocEntry.Text = dt.Rows[0][0].ToString();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            New();
        }

        void New()
        {
            txtDRNo.Text = "";
            txtRemarks.Text = "";
            numericUpDown1.Value = 0;

            dgvItems.Rows.Clear();

            NumSeries();

            btnAdd.Enabled = true;
            numericUpDown1.Enabled = true;
            txtRemarks.Enabled = true;
            pictureBox2.Enabled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            var fS = new frmSearch2();
            fS.oSearchMode = "OPKC";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                txtDocEntry.Text = fS.oName;

                var query = $"SELECT A.DocEntry,A.DocNum [Doc No.],A.U_DRNo [DR No.],A.U_NoofBox [Total No. of Box],Remark [Remarks] FROM [@OPKC] A Where A.DocEntry = '{fS.oCode}'";
                var dt = hana.Get(query);

                if (dt.Rows.Count > 0)
                {
                    txtDocEntry.Text = fS.oCode;
                    txtDRNo.Text = dt.Rows[0]["DR No."].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();

                    int Box = 0;

                    if (dt.Rows[0]["Total No. of Box"].ToString() != "")
                    {
                        Box = Convert.ToInt32(dt.Rows[0]["Total No. of Box"].ToString());
                    }

                    numericUpDown1.Value = Box;
                    lblDocEntry2.Text = fS.oCode;

                    Find(fS.oCode);

                    btnAdd.Enabled = false;
                    txtRemarks.Enabled = false;
                    numericUpDown1.Enabled = false;
                    pictureBox2.Enabled = false;
                }
            }
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int count = dgvItems.Rows.Cast<DataGridViewRow>().ToList().Where(x => (bool)x.Cells[0].Value == true).Count() + 1;

            if (numericUpDown1.Value >= count)
            {

            }
            else
            {

            }
        }

        private void PackingList_Conssionaire_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void PackingList_Conssionaire_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close this Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            { Dispose(); }
            else
            { e.Cancel = true; }
        }

        private void dgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvItems.Columns.Count > 1 && e.ColumnIndex == 0)
            //{
            //    if (Convert.ToBoolean(dgvItems.SelectedRows[0].Cells[0].Value) == false)
            //    {
            //        dgvItems.SelectedRows[0].Cells[0].Value = true;
            //    }
            //    else
            //    {
            //        dgvItems.SelectedRows[0].Cells[0].Value = false;
            //    }
            //}
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ReloadLineItems();
        }

        private void numericUpDown1_Enter(object sender, EventArgs e)
        {
            
        }

        void AddtoSAP()
        {
            var head = new Dictionary<string, string>();

            head.Add("U_DRNo", txtDRNo.Text);
            head.Add("U_NoofBox", numericUpDown1.Value.ToString());
            head.Add("Remark", txtRemarks.Text);


            var dictLines = new List<Dictionary<string, object>>();
            int bdlNumber = 1;
            foreach (DataGridViewRow dt in dgvItems.Rows)
            {
                var lines = new Dictionary<string, object>();

                lines.Add("U_Identifier", dt.Cells[2].Value.ToString());
                lines.Add("U_StoreCode", dt.Cells[3].Value.ToString());
                lines.Add("U_StoreName", dt.Cells[4].Value.ToString());
                lines.Add("U_VendorCode", dt.Cells[5].Value.ToString());
                lines.Add("U_VendorName", dt.Cells[6].Value.ToString());
                lines.Add("U_DRNo", dt.Cells[7].Value.ToString());
                lines.Add("U_DeptCode", dt.Cells[8].Value.ToString());
                lines.Add("U_SubDeptCode", dt.Cells[9].Value.ToString());
                lines.Add("U_ClassCode", dt.Cells[10].Value.ToString());
                lines.Add("U_ClassDescr", dt.Cells[11].Value.ToString());
                lines.Add("U_TotalBox", Convert.ToInt32(dt.Cells[12].Value.ToString()));
                lines.Add("U_BoxBdlNo", bdlNumber); //Convert.ToInt32(dt.Cells[13].Value.ToString())    
                lines.Add("U_TotalDrAmt", Convert.ToDouble(dt.Cells[14].Value.ToString()));
                lines.Add("U_LineEnder", dt.Cells[15].Value.ToString());
                lines.Add("U_Formula", dt.Cells[16].Value.ToString());

                dictLines.Add(lines);
                bdlNumber++;
            }

            var json = DataRepository.JsonBuilder(head, dictLines, "PKC1Collection");

            string returnvalue = string.Empty;

            bool isPosted = false;
            var serviceLayerAccess = new ServiceLayerAccess();

            if (btnAdd.Text == "Add")
            {
                var url = $"OPKC";
                isPosted = serviceLayerAccess.ServiceLayer_Posting(json, "POST", url, "DocEntry", out string output, out string value);
            }
            else
            {
                var url = $"OPKC({txtDocEntry.Text})";
                isPosted = serviceLayerAccess.ServiceLayer_Posting(json, "PATCH", url, "DocEntry", out string output, out string value);
            }

            if (isPosted)
            {
                ClearData();
            }
        }
    }
}

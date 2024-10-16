using DirecLayer;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using PresenterLayer.Views.Main;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PresenterLayer.Views.Tools
{
    public partial class frmDuplicateItemSelection : MetroForm
    {
        public MainForm frmMain { get; set; }
        //public frmBarcodeEncoding frmBarcodeEncoding { get; set; }
        public FrmUnofficialSales frmUnofficialSales { get; set; }
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        public string oSelectedCode { get; set; }
        public string strBPcode, strBarCode;

        int row { get; set; }
        int col { get; set; }

        Thread thread;
        DataTable dt;
        public frmDuplicateItemSelection()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            helper = new DataHelper();
        }


        public frmDuplicateItemSelection([Optional] FrmUnofficialSales frmUnofficialSales)
        {
            InitializeComponent();
            this.frmUnofficialSales = frmUnofficialSales;
            this.frmMain = StaticHelper._MainForm;
        }

        private void frmDuplicateItemSelection_Load(object sender, EventArgs e)
        {
            if (dgvItem.Columns.Count > 0) { dgvItem.Columns.Clear(); }

            DataGridViewTextBoxColumn itemCode = new DataGridViewTextBoxColumn();
            itemCode.Name = "Item No.";
            dgvItem.Columns.Add(itemCode);

            DataGridViewTextBoxColumn itemName = new DataGridViewTextBoxColumn();
            itemName.Name = "Item Description";
            dgvItem.Columns.Add(itemName);

            DataGridViewTextBoxColumn UPC = new DataGridViewTextBoxColumn();
            UPC.Name = "UPC";
            dgvItem.Columns.Add(UPC);

            DataGridViewTextBoxColumn SKU = new DataGridViewTextBoxColumn();
            SKU.Name = "Store Chain SKU";
            dgvItem.Columns.Add(SKU);

            DataGridViewTextBoxColumn SKU2 = new DataGridViewTextBoxColumn();
            SKU2.Name = "12 Digit SKU";
            dgvItem.Columns.Add(SKU2);

            dgvItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItem.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvItem.MultiSelect = false;
            
            var query = string.Format(helper.ReadDataRow(hana.Get(SP.US_BarCodeList), 1, "", 0), strBPcode, strBarCode);
            //string query = "SELECT T1.ItemCode, T3.ItemName, T1.U_BarCode [UPC], T1.U_SKU [SKU], T3.CodeBars [SKU2] FROM CPN2 T1 " +
            //                $" INNER JOIN CPN1 T2 on T1.CpnNo = T2.CpnNo and T2.BpCode = '{strBPcode}' INNER JOIN OITM T3 on T1.ItemCode = T3.ItemCode " +
            //                $" WHERE T1.U_BarCode LIKE '%{strBarCode}%' OR T1.U_SKU LIKE '%{strBarCode}%'";

            dt = DataAccess.Select(DataAccess.conStr("HANA"), query);

            thread = new Thread(new ThreadStart(ExtractData));
            thread.Start();
            dgvItem.Focus();
        }


        private void ExtractData()
        {
            int max = dt.Rows.Count;
            int min = 0;

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    object[] a = { row["ItemCode"].ToString(), row["ItemName"].ToString(), row["UPC"].ToString(), row["SKU"].ToString(), row["SKU2"].ToString() };

                    Invoke(new Action(() => { dgvItem.Rows.Add(a); }));
                    dgvItem.Invoke(new Action(() => {
                        foreach (DataGridViewRow row2 in dgvItem.Rows)
                        {
                            row2.HeaderCell.Value = String.Format("{0}", row2.Index + 1);
                        }

                        dgvItem.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                    }));

                    Thread.Sleep(50);
                    StaticHelper._MainForm.pbStatus.Invoke(new Action(() => { StaticHelper._MainForm.Progress($"Please wait until all data are loaded. ({min}/{max}) ", min, max); }));
                    min++;
                }
                catch (Exception ex) { }
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvItem.Columns.Count > 1)
            {
                foreach (DataGridViewRow x in dgvItem.Rows)
                {
                    if (x.Cells[col].Value != null)
                    {
                        if (x.Cells[col].Value.ToString().ToUpper().StartsWith(txtSearch.Text.ToUpper()))
                        {
                            x.Selected = true;
                            row = x.Index;
                            dgvItem.FirstDisplayedScrollingRowIndex = row;
                            break;
                        }
                        else
                        {
                            x.Selected = false;
                        }
                    }
                }
            }
        }

        private void frmSearchItemSoUn_FormClosing(object sender, FormClosingEventArgs e)
        {
            abortThread();
        }

        void abortThread()
        {
            if (thread.IsAlive)
            {
                thread.Abort();
            }
        }

        private void dgvItem_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            col = e.ColumnIndex;
        }

        private void dgvItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            SelectItem();
        }

        private void frmDuplicateItemSelection_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dgvItem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectItem();
        }

        private void dgvItem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectItem();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            abortThread();
            Dispose();
        }

        private void SelectItem()
        {
            oSelectedCode = dgvItem.SelectedRows[0].Cells[0].Value.ToString();
            //PublicStatic.frmMain.ProgressClear();
            abortThread();
            Dispose();
        }

    }
}

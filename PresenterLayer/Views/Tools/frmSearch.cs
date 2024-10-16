using MetroFramework.Forms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PresenterLayer.Views.Main;

namespace DirecLayer
{
    public partial class frmSearch : MetroForm
    {
        MainForm frmMain;
        
        //missing forms
        //frmItemMasterData frmItemMasterData;
        //frmBarcodePerBP frmBarcodePerBP;
        int iRow = 0;
        public string iColumn;
        public frmSearch(MainForm frmMain)
        {
            InitializeComponent();
            this.frmMain = frmMain;
            ActiveControl = txtSearch;
            gvSetup(gvSearch);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }
            else if (keyData == Keys.Enter)
            { Choose(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        { Dispose(); }

        private void gvSetup(DataGridView dt)
        {
            try
            {
                dt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dt.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dt.MultiSelect = false;
                dt.RowTemplate.Resizable = DataGridViewTriState.False;
                dt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dt.RowHeadersVisible = false;
                dt.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dt.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);
            }
            catch (Exception ex)
            { /*frmMain.NotiMsg(ex.Message, Color.Red);*/ }
        }

        public void SearchData(DataTable dt/*, [Optional]frmItemMasterData frmOITM,[Optional] frmBarcodePerBP frmBarcodePerBP*/)
        {
            if (Tag.ToString() == "STYLE" || Tag.ToString() == "COLOR")
            {
                DataRow oRow;
                iColumn = "Name";
                oRow = dt.NewRow();
                oRow[0] = "Define New";
                oRow[1] = "Define New";
                dt.Rows.Add(oRow);
            }
            else if (Tag.ToString() == "Preview")
            {
                iColumn = "Description";
            }
            gvSearch.DataSource = dt;
            //frmItemMasterData = frmOITM;
            //this.frmBarcodePerBP = frmBarcodePerBP;
            
        }


        void Choose()
        {
            int i = gvSearch.SelectedRows[0].Index;
            if (i < 0)
            { /*frmMain.NotiMsg("No matching records found", Color.Red);*/ }
            else if (gvSearch.Rows[i].Cells[0].Value.ToString() == "Define New")
            {
                DataTable dt = new DataTable();
                dt = ((DataTable)gvSearch.DataSource).Copy();
                frmDefineNew frmDefineNew = new frmDefineNew(frmMain);
                frmDefineNew.MdiParent = frmMain;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[0].ToString() == "Define New")
                    { dr.Delete(); break; }
                }
                frmDefineNew.Text = $"Define New - {Text}";
                frmDefineNew.Tag = Tag;
                frmDefineNew.Show();
                //frmDefineNew.SearchData(dt, frmItemMasterData, this);
            }
            else
            {
                string SearchCode = gvSearch.Rows[i].Cells[0].Value.ToString();
                string SearchName = gvSearch.Rows[i].Cells[1].Value.ToString();
                //if (Tag.ToString() == "STYLE" || Tag.ToString() == "COLOR")
                //{ frmItemMasterData.SearchValue(SearchCode, SearchName); }
                //else if (Tag.ToString() == "OCRD")
                //{ frmItemMasterData.CardCode.Text = SearchCode; }
                //else if (Tag.ToString() == "BarCode")
                { /*frmBarcodePerBP.RefreshData(SearchCode, SearchName);*/ }
                Dispose();
            }
        }

        private void btnCommand_Click(object sender, EventArgs e)
        {
            if (Tag.ToString() == "Preview")
            { Dispose(); }
            else 
            { Choose(); }
        }

        private void gvSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Tag.ToString() != "Preview")
            { Choose(); }
        }

        private void gvSearch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (gvSearch.Rows.Count > 0)
            { iColumn = gvSearch.Columns[e.ColumnIndex].Name; }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (gvSearch.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in gvSearch.Rows)
                {
                    if (row.Cells[iColumn].Value.ToString().ToUpper().Contains(txtSearch.Text.ToUpper()))
                    {
                        row.Selected = true;
                        iRow = row.Index;
                        gvSearch.FirstDisplayedScrollingRowIndex = iRow;
                        break;
                    }
                    else
                    { row.Selected = false; }
                }
            }
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            if (gvSearch.Rows.Count > 0)
            {
                iColumn = gvSearch.Columns[0].Name;
            }
        }

        private void gvSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}

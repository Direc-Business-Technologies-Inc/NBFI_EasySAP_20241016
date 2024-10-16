using MetroFramework.Forms;
using System.Drawing;
using System;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using MetroFramework;
using Context;
using PresenterLayer.Views.Main;
using System.Text;
using PresenterLayer;

namespace DirecLayer
{
    public partial class frmDefineNew : MetroForm
    {
        MainForm frmMain;

        //need master data module
        //frmItemMasterData frmItemMasterData;
        frmSearch frmSearch;
        int iColumn, iRow;
        string iSeries;

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmDefineNew(MainForm frmMain)
        {
            InitializeComponent();
            this.frmMain = frmMain;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        { Dispose(); }

        public void SearchData(DataTable dt/*, [Optional]frmItemMasterData frmOITM*/, [Optional]frmSearch frmSearch)
        {
            //frmItemMasterData = frmOITM;
            this.frmSearch = frmSearch;
            gvDefine.DataSource = dt;
            DataTable dtSeries = new DataTable();
            dtSeries = DataAccess.Select(DataAccess.conStr("HANA"), $"SELECT TOP 1 U_Series FROM [@PR{Tag.ToString()}] ORDER BY CAST(U_Series AS INT) DESC"/*, frmMain*/);
            //dtSeries = DataAccess.Select(DataAccess.conStr("HANA"), $"SELECT TOP 1 Code FROM [@PR{Tag.ToString()}] ORDER BY Code DESC", frmMain);
            
            if (dtSeries.Rows.Count > 0 )
            {
                iSeries = Convert.ToString(Convert.ToInt16(dtSeries.Rows[0]["U_Series"]) + 1);
            }
            else
            {
                switch (Tag.ToString())
                {
                    case "STYLE" :

                        iSeries = "0001";
                        break;

                    case "COLOR" :

                        iSeries = "01";
                        break;
                }
            }

            DefindNewSearch();
            gvSetup(gvDefine);
        }

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
                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                dt.Columns.Add(col);
                dt.Columns[2].Name = "Tag";
                dt.Columns[2].Visible = false;
            }
            catch (Exception ex)
            { /*frmMain.NotiMsg(ex.Message, Color.Red);*/ }
        }

        private void gvDefine_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            iColumn = e.ColumnIndex;
        }

        private void btnCommand_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        void RefreshData()
        {
            gvDefine.DataSource = DataAccess.Select(DataAccess.conStr("HANA"), $"SELECT Code, Name FROM [@PR{Tag.ToString()}] ORDER BY Code"/*, frmMain*/);
            switch (Tag.ToString())
            {
                case "COLOR":
                    //frmItemMasterData.RefreshGV("Colors");
                    break;
                case "STYLE":
                    DataTable dt = new DataTable();
                    dt = DataAccess.Select(DataAccess.conStr("HANA"), $"SELECT Code,Name FROM [@PR{Tag.ToString()}]");
                    //frmSearch.SearchData(dt, frmItemMasterData);
                    break;
            }
            DefindNewSearch();
        }

        private void gvDefine_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            btnCommand.Text = "&Update";
            gvDefine.Rows[gvDefine.CurrentCell.RowIndex].Cells["Tag"].Value = true;
        }
        
        private void gvDefine_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    gvDefine.CurrentCell = gvDefine.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    gvDefine.Rows[e.RowIndex].Selected = true;
                    gvDefine.Focus();

                    var mousePosition = gvDefine.PointToClient(Cursor.Position);

                    ContextMenuStrip.Show(gvDefine, mousePosition);
                }
            }
        }

        private void gvDefine_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (gvDefine.Columns.Contains("Code") && e.Column == gvDefine.Columns["Code"])
            {
                string[] parts1 = e.CellValue1.ToString().Trim().Split('/');
                int a1 = int.Parse(parts1[0].Split(' ')[2]);
                int b1 = int.Parse(parts1[1]);
                int c1 = int.Parse(parts1[2]);
                string[] parts2 = e.CellValue2.ToString().Trim().Split('/');
                int a2 = int.Parse(parts2[0].Split(' ')[2]);
                int b2 = int.Parse(parts2[1]);
                int c2 = int.Parse(parts2[2]);
                e.SortResult = c1.CompareTo(c2);

                // If equal, then compare second value (month?)
                if (e.SortResult == 0)
                    e.SortResult = b1.CompareTo(b2);

                // Finally if still equal, then compare first value
                if (e.SortResult == 0)
                    e.SortResult = a1.CompareTo(a2);

            }
        }

        private void DefindNewSearch()
        {
            if (gvDefine.Rows.Count > 1)
            {
                iRow = gvDefine.Rows.Count - 1;
                gvDefine.CurrentCell = gvDefine.Rows[iRow].Cells[1];
                gvDefine.FirstDisplayedScrollingRowIndex = iRow;
                gvDefine.Rows[iRow].Cells["Code"].Value = iSeries;
            }

        }
    }
}

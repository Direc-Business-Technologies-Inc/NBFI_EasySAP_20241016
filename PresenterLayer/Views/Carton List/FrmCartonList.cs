using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirecLayer._02_Form.MVP.Presenters;
using MetroFramework.Forms;
using DirecLayer._03_Repository;
using PresenterLayer;
using PresenterLayer.Services;
using PresenterLayer.Helper;
using MetroFramework;
using DirecLayer;

namespace PresenterLayer.Views
{
    public partial class FrmCartonList : MetroForm, ICartonList
    {
        private int ColumnIndex = 0;
        private int ColumnIndex2 = 0;

        private string GlobalDocEntry { get; set; }

        public FrmCartonList()
        {
            InitializeComponent();
        }

        public FrmCartonList(string _DocEntry)
        {
            InitializeComponent();

            GlobalDocEntry = _DocEntry;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public DataGridView Table
        {
            get => DgvItem;
        }

        public string DocEntry
        {
            get => TxtDocEntry.Text;
            set => TxtDocEntry.Text = value;
        }

        public Button Request
        {
            get => BtnRequest;
            set => BtnRequest = value;
        }

        public string Remark
        {
            get => TxtRemarks.Text;
            set => TxtRemarks.Text = value;
        }
        public DataTable TableSearch
        {
            set => DgvFindDocument.DataSource = value;
        }
        
        public CartonListPresenter Presenter
        {
            private get;
            set;
        }
        
        private void FrmCartonList_Load(object sender, EventArgs e)
        {
            if (GlobalDocEntry != null)
            {
                Presenter.GetExistingCartonList(GlobalDocEntry);
                TabPreviewItem.TabPages.RemoveAt(1);
                BtnRequest.Text = "Update";
            }
        }

        private void TabPreviewItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TabPreviewItem.SelectedIndex)
            {
                default:
                    Presenter.GetExistingCartonList();
                    DgvFindDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    break;
            }
        }

        private void BtnAddCartonList_Click(object sender, EventArgs e)
        {
            Presenter.ShowExistingCartons();
        }

        private void DgvItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvItem.CurrentRow.Selected = true;
        }

        private void DgvItem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Presenter.GetCartonInformation();
        }

        private void BtnChoose_Click(object sender, EventArgs e)
        {
            Choose();
        }
        
        void Choose()
        {
            if (DgvFindDocument.CurrentRow.Cells[0].Value != null)
            {
                string docEntry = DgvFindDocument.CurrentRow.Cells[0].Value.ToString();

                Presenter.GetExistingCartonList(docEntry);

                TabPreviewItem.SelectedIndex = 0;

                BtnRequest.Text = "Update";
            }
        }

        private void DgvItem_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex = e.ColumnIndex;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            DataRepository.RowSearch(DgvItem, TxtSearch.Text, ColumnIndex);
        }
        
        private void DgvFindDocument_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex2 = e.ColumnIndex;
        }

        private void TxtSearchDocument_TextChanged(object sender, EventArgs e)
        {
            DataRepository.RowSearch(DgvFindDocument, TxtSearchDocument.Text, ColumnIndex2);
        }

        private void BtnRequest_Click(object sender, EventArgs e)
        {
            BtnRequest.Enabled = false;
            if (Presenter.ExecuteRequest(BtnRequest.Text))
            {
                BtnRequest.Text = "Add";
            }

            BtnRequest.Enabled = true;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BtnNewDocument_Click(object sender, EventArgs e)
        {
            Presenter.ClearField(true);
        }

        private void DgvItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    DgvItem.CurrentCell = DgvItem.CurrentRow.Cells[e.ColumnIndex + 1];
                    DgvItem.CurrentRow.Selected = true;
                    DgvItem.Focus();

                    var mousePosition = DgvItem.PointToClient(Cursor.Position);

                    msItems.Show(DgvItem, mousePosition);
                }
            }
            
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = DgvItem.CurrentRow.Index;
            DgvItem.Rows.RemoveAt(index);
        }

        private void DgvFindDocument_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvFindDocument.CurrentRow.Selected = true;
        }

        private void FrmCartonList_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void FrmCartonList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close this document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) != DialogResult.Yes)
            { e.Cancel = true; }
            else { Dispose(); }
        }

        private void DgvFindDocument_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Choose();
        }
    }
}

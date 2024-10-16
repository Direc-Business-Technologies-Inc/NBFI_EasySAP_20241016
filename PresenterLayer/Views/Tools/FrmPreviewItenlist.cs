using System;
using System.Data;
using System.Windows.Forms;
using MetroFramework.Forms;
using PresenterLayer.Helper;

namespace PresenterLayer.Views
{
    public partial class FrmPreviewItenlist : MetroForm, IPreviewItemlist
    {
        private int ColumnIndex = 0;

        private DataTable _dt;

        public FrmPreviewItenlist(DataTable dt)
        {
            InitializeComponent();

            _dt = dt;
        }
        
        public Tuple<DataTable, DataGridView> ItemSource
        {
            get
            {
                DataTable dt = null;
                return Tuple.Create(dt, DgvItems);
            }

            set { DgvItems.DataSource = value.Item1; }
        }

        public PreviewItemPresenter Presenter
        {
            private get;
            set;
        }

        // EVENTS
        
        private void FrmPreviewItenlist_Load(object sender, EventArgs e)
        {
            DgvItems.DataSource = _dt;
        }

        private void DgvItems_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex = e.ColumnIndex;
        }

        private void TxtSearchBox_TextChanged(object sender, EventArgs e)
        {
            if (DgvItems.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in DgvItems.Rows)
                {
                    if (row.Cells[ColumnIndex].Value != null)
                    {
                        if (row.Cells[ColumnIndex].Value.ToString().ToUpper().StartsWith(TxtSearchBox.Text.ToUpper()))
                        {
                            row.Selected = true;
                            DgvItems.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                        else
                        {
                            row.Selected = false;
                        }
                    }
                }
            }
        }

        private void DgvItems_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    try
                    {
                        DgvItems.CurrentRow.Selected = true;
                        DgvItems.Focus();

                        var mousePosition = DgvItems.PointToClient(Cursor.Position);

                        msItems.Show(DgvItems, mousePosition);
                    }
                    catch (Exception ex)
                    {
                        StaticHelper._MainForm.ShowMessage(ex.Message, true);
                    }
                }
            }
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = DgvItems.CurrentRow.Index;

            DgvItems.Rows.RemoveAt(index);

            Presenter.ReturnItems();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}

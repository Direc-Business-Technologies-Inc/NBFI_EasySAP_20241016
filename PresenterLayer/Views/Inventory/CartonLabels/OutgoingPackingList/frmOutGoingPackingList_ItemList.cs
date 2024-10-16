using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using DomainLayer.Models.OutgoingPackingList;

namespace PresenterLayer
{
    public partial class frmOutGoingPackingList_ItemList : MetroForm
    {
        int ColumnIndex = 0;

        public frmOutGoingPackingList_ItemList()
        {
            InitializeComponent();
        }

        private void frmOutGoingPackingList_ItemList_Load(object sender, EventArgs e)
        {
            DgvItems.DataSource = null;
            DgvItems.DataSource = OutgoingPackingListModel.packinglist
                                                          .Where(x =>  x.Status == "N")
                                                          .Select(x => new { x.SortCode, x.ItemCode, x.Description, x.Brand, x.Size, x.Color, x.Available, x.Index })
                                                          .OrderBy(x => x.SortCode).ToList();
            DgvItems.Columns[7].Visible = false;
            DgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void DgvItems_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex = e.ColumnIndex;
        }

        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            if (DgvItems.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in DgvItems.Rows)
                {
                    if (row.Cells[ColumnIndex].Value != null)
                    {
                        if (row.Cells[ColumnIndex].Value.ToString().ToUpper().Contains(txtSearchItem.Text.ToUpper()))
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

        private void DgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvItems.CurrentRow.Selected = true;
        }

        private void DgvItems_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectItems();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SelectItems();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        
        void SelectItems()
        {
            foreach (DataGridViewRow row in DgvItems.SelectedRows)
            {
                int index = Convert.ToInt32(row.Cells["Index"].Value);

                OutgoingPackingListModel.packinglist.Find(x => x.Index == index).Status = "Y";
            }

            Dispose();
        }
    }
}

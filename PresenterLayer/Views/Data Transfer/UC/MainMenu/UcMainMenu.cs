using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PresenterLayer.Views.Main;
using PresenterLayer.Views;
using DomainLayer;

namespace PresenterLayer.Views
{
    public partial class UcMainMenu : UserControl
    {
        frmDT frmDt { get; set; }
        MainForm frmMain { get; set; }

        SAOContext model = new SAOContext();

        public UcMainMenu(frmDT frmDt, MainForm frmMain)
        {
            InitializeComponent();

            this.frmDt = frmDt;
            this.frmMain = frmMain;
        }

        private void UcMainMenu_Load(object sender, EventArgs e)
        {
            if (DgvMapList.ColumnCount <= 0)
            {
                DgvMapList.ColumnCount = 3;

                DgvMapList.Columns[0].Name = "#";
                DgvMapList.Columns[0].Visible = false;
                DgvMapList.Columns[1].Name = "Map Code";
                DgvMapList.Columns[2].Name = "Map Description";

                DgvMapList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            DisplayTemplates();
        }

        private void BtnCreateTemplate_Click(object sender, EventArgs e)
        {
            AddControl(new UcTemplateCreation(frmDt, frmMain));
        }

        private void DgvMapList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var modal = new FrmDt_Modal();
            modal.frmDt = frmDt;
            modal.frmMain = frmMain;
            modal.Id = Convert.ToInt32(DgvMapList.CurrentRow.Cells[0].Value);
            modal.ShowDialog();
        }

        private void DgvMapList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int lastROw = DgvMapList.RowCount - 1;

            if (e.Button == MouseButtons.Right && lastROw != e.RowIndex)
            {
                if (e.RowIndex != -1)
                {
                    DgvMapList.CurrentRow.Selected = true;
                    DgvMapList.Focus();

                    var mousePosition = DgvMapList.PointToClient(Cursor.Position);

                    msItems.Show(DgvMapList, mousePosition);
                }
            }
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRow = DgvMapList.CurrentRow;

            int id = Convert.ToInt32(selectedRow.Cells[0].Value);

            var result = MessageBox.Show($"Are you sure you want to delete {selectedRow.Cells[1].Value.ToString()}", "Data Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                model.dtheader.Where(w => w.MapID == id).Select(s => s).ToList().ForEach(f =>
                {
                    model.Entry(f).State = System.Data.Entity.EntityState.Deleted;
                });

                model.SaveChanges();

                DgvMapList.Rows.RemoveAt(selectedRow.Index);
            }
        }
        
        #region functions 

        void AddControl(Control ctrl)
        {
            frmDt.pnlContainer.Controls.Clear();
            frmDt.pnlContainer.Controls.Add(ctrl);
        }

        void DisplayTemplates()
        {
            foreach (var x in model.dtheader.ToList())
            {
                DgvMapList.Rows.Add(x.MapID, x.MapCode, x.MapDescription);
            }
        }

        #endregion

        private void DgvMapList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvMapList.CurrentRow.Selected = true;
        }

        private void msItems_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}

using MetroFramework.Forms;
using PresenterLayer.Services;
using System;
using System.Windows.Forms;

namespace PresenterLayer.Views
{
    public partial class frmITM_ChooseFromList : MetroForm
    {
        public ITM_ChooseFromList cfl { get; set; }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }
            else if (keyData == Keys.Enter)
            { btnCommand.PerformClick(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmITM_ChooseFromList()
        { InitializeComponent(); }

        private void btnCancel_Click(object sender, EventArgs e)
        { cfl.btnCancel(); }

        private void frmITM_Load(object sender, EventArgs e)
        { cfl.LoadForm(); }

        private void btnCommand_Click(object sender, EventArgs e)
        { cfl.btnCommand(); }
        
        private void dgvChooseFromList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        { cfl.btnCommand(); }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        { cfl.txtSearch_TextChanged(e); }

        private void dgvChooseFromList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        { cfl.dgvSearch_ColumnHeaderMouseClick(e); }
    }
}

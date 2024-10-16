using MetroFramework.Forms;
using PresenterLayer.Services;
using System;
using System.Windows.Forms;

namespace PresenterLayer.Views
{
    public partial class frmITM_Search : MetroForm
    {
        ITM_Preview Search { get; set; }
        public frmItemMasterData imd { get; set; }

        public frmITM_Search()
        {
            InitializeComponent();
        }

        private void frmITM_Search_Load(object sender, EventArgs e)
        {
            ITM_Preview preview = new ITM_Preview();
            Search = preview;
            preview.imd = imd;
            Search.Form_Load();
            dgvSearch.Focus();
        }
        
        private void btnCommand_Click(object sender, EventArgs e)
        { 
            //Search.Form_Close();
        }

        private void dgvSearch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        { Search.ColumnHeaderMouseClick(sender, e); }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        { Search.SearchEngine(dgvSearch, txtSearch); }
    }
}

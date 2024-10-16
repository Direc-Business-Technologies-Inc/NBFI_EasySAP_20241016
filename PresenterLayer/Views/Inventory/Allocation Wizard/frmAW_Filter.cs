using System.Windows.Forms;
using MetroFramework.Forms;
using System;

namespace PresenterLayer
{
    public partial class frmAW_Filter : MetroForm
    {
        public string TableID { get; set; }
        public string Type { get; set; }
        public string sQuery { get; set; }
        AW_Filter_Form filterForm { get; set; }
        public DataGridViewRow dgvdr { get; set; }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Dispose(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmAW_Filter()
        {
            InitializeComponent();
            AW_Filter_Form filterForm = new AW_Filter_Form();
            this.filterForm = filterForm;
            filterForm.filter = this;
        }

        private void frmFilter_Load(object sender, EventArgs e)
        {
            filterForm.sQuery = sQuery;
            filterForm.TableID = TableID;
            filterForm.Type = Type;
            filterForm.dgvdr = dgvdr;
        }

        public void LoadData()
        { filterForm.dgvSetup(dgvSearch, TableID, Type); }

        private void btnCancel_Click(object sender, EventArgs e)
        { Close(); }
        
        private void dgvSearch_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        { filterForm.dgvSearch_CellEndEdit(sender,e); }

        private void btnCommand_Click(object sender, EventArgs e)
        { Close(); }

        private void dgvSearch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        { filterForm.dgvSearch_ColumnHeaderMouseClick(e); }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        { filterForm.txtSearch_TextChanged(e); }

        private void frmAW_Filter_FormClosing(object sender, FormClosingEventArgs e)
        { filterForm.Form_Closing(); }

        private void dgvSearch_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        { filterForm.dgvNumberFormatting(sender,e); }
    }
}

using MetroFramework.Forms;
using System;
using System.Windows.Forms;

namespace PresenterLayer
{
    public partial class frmAW_Find : MetroForm
    {
        AW_Find_Form findForm { get; set; }
        public frmAllocationWizard form { get; set; }
        public string sQuery { get; set; }
        public string TableID { get; set; }
        public string Type { get; set; }
        public DataGridViewRow dgvdr { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Dispose(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmAW_Find()
        {
            InitializeComponent();
            AW_Find_Form findForm = new AW_Find_Form();
            this.findForm = findForm;
            findForm.find = this;
        }

        private void btnCommand_Click(object sender, EventArgs e)
        { findForm.Choose(); }
        
        private void txtSearch_TextChanged(object sender, EventArgs e)
        { findForm.txtSearch_TextChanged(); }

        private void btnCancel_Click(object sender, EventArgs e)
        { Dispose(); }

        private void frmAWFind_Load(object sender, EventArgs e)
        {
            findForm.form = form;
            findForm.dgvdr = dgvdr;
            findForm.sQuery = sQuery;
            findForm.TableID = TableID;
            findForm.Type = Type;
            findForm.dgvSetup(dgvSearch);
        }
        
        private void dgvSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        { btnCommand.PerformClick(); }

        private void dgvSearch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        { findForm.dgvSearch_ColumnHeaderMouseClick(e); }
    }
}

using System;
using MetroFramework.Forms;
using System.Windows.Forms;
using System.Data;

namespace PresenterLayer
{
    public partial class frmAW_DefineNew : MetroForm
    {

        AW_DefineNew_Form definenew { get; set; }
        public frmAllocationWizard form { get; set; }
        public DataTable dt { get; set; }
        public string sCode { get; set; }
        public DataGridView dgv { get; set; }

        public frmAW_DefineNew()
        {
            InitializeComponent();
            AW_DefineNew_Form definenew = new AW_DefineNew_Form();
            this.definenew = definenew;
            definenew.faw = form;
            definenew.definenew = this;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void frmAW_DefineNew_Load(object sender, EventArgs e)
        { definenew.dgvSetup(dt); }

        private void btnCommand_Click(object sender, EventArgs e)
        { definenew.CommandClick(dgv); }

        private void btnCancel_Click(object sender, EventArgs e)
        { Dispose(); }
    }
}

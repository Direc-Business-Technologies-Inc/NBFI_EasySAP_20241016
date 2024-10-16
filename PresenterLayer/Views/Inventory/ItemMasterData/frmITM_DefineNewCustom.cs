using sys = System.Windows.Forms;
using MetroFramework.Forms;
using System;
using PresenterLayer.Services;

namespace PresenterLayer.Views
{
    public partial class frmITM_DefineNewCustom : MetroForm
    {
        ITM_DefineNewCustom definenew { get; set; }

        public frmItemMasterData frmIMD {get;set;}

        protected override bool ProcessCmdKey(ref sys.Message msg, sys.Keys keyData)
        {
            if (keyData == sys.Keys.Escape)
            { Close(); }
            else if (keyData == sys.Keys.Enter)
            { btnCommand.PerformClick(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmITM_DefineNewCustom()
        {
            InitializeComponent();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        { definenew.Form_Close(); }

        private void btnCommand_Click(object sender, EventArgs e)
        { definenew.Form_Add(); }

        private void cbParentCode_SelectedIndexChanged(object sender, EventArgs e)
        { definenew.Form_SelectedIndexChanged(); }

        private void frmITM_DefineNewCutom_Load(object sender, EventArgs e)
        {
            ITM_DefineNewCustom definenew = new ITM_DefineNewCustom();
            this.definenew = definenew;
            definenew.imd = frmIMD;
            definenew.definenew = this;
            definenew.Form_Load();
        }
    }
}

using MetroFramework.Forms;
using PresenterLayer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresenterLayer.Views
{
    public partial class frmITM_DefineNew : MetroForm
    {
        ITM_DefineNew form { get; set; }
        public ITM_ChooseFromList cfl { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Dispose(); }
            else if (keyData == Keys.Enter)
            { btnCommand.PerformClick(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmITM_DefineNew()
        {
            InitializeComponent();
            ITM_DefineNew form = new ITM_DefineNew();
            form.frmITM_DefineNew = this;
            this.form = form;

        }

        public void PopulateData(string sCode, string sName)
        { form.DefindNewSearch(sCode, sName); }

        private void btnCancel_Click(object sender, EventArgs e)
        { form.Form_Cancel(); }

        private void frm_DefineNew_Load(object sender, EventArgs e)
        {
            form.cfl = cfl;
            form.Form_Load();
        }

        private void btnCommand_Click(object sender, EventArgs e)
        { form.AddValue(); }
        
        private void dgvDefine_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        { form.dgv_CellEndEdit(); }
    }
}

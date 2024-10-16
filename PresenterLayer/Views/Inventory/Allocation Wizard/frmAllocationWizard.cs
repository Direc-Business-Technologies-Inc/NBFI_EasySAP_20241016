using MetroFramework.Forms;
using System.Windows.Forms;
using System.Drawing;
using MetroFramework;
using Context;
using System;
using PresenterLayer.Helper;

namespace PresenterLayer
{
    public partial class frmAllocationWizard : MetroForm
    {
        AW_Form form { get; set; }
        AW_Button button { get; set; }
        AW_DataGridview dgv { get; set; }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }
            else if (keyData == (Keys.Alt | Keys.Up))
            { WindowState = FormWindowState.Maximized; }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmAllocationWizard()
        {
            InitializeComponent();

            AW_Form form = new AW_Form();
            this.form = form;
            form.frmAW = this;

            MaximumSize = new Size(form.max_width, form.max_height);

            AW_Button button = new AW_Button();
            this.button = button;
            button.form = this;

            AW_DataGridview dgv = new AW_DataGridview();
            this.dgv = dgv;
            dgv.form = this;
        }

        private void MetroTabControl_SelectedIndexChanged(object sender, EventArgs e)
        { button.SelectedIndexChanged(); }

        private void frmAllocationWizard_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void frmAllocationWizard_FormClosing(object sender, FormClosingEventArgs e)
        { form.Form_Close(e); }

        private void frmAllocationWizard_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            form.Form_Load();
            var s = "sadasdasdas";
            var sa = s.Length.ToString();
        }

        private void dgvCellContentClick(object sender, DataGridViewCellEventArgs e)
        { form.dgvCellContentClick(sender, e); }

        private void navClick(object sender, EventArgs e)
        { form.navClick(sender, e); }

        private void dgvRowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        { form.dgvRowPostPaint(sender, e); }

        private void dgvCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        { form.dgvCellPainting(sender, e); }
        
        private void btnCancel_Click(object sender, EventArgs e)
        { button.btnCancelClick(); }

        private void btnGenerate_Click(object sender, EventArgs e)
        { button.btnGenerateClick(); }

        private void btnPrev_Click(object sender, EventArgs e)
        { button.SelectedTab(false); }

        private void btnNext_Click(object sender, EventArgs e)
        { button.SelectedTab(true); }

        private void btnFinish_Click(object sender, EventArgs e)
        { button.btnFinishClick(); }

        private void btnItemNewParam_Click(object sender, EventArgs e)
        { button.btnDefineNew("ItemSelection"); }

        private void dgvItemOtherParam_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { form.dgvItemOtherParam_CellContentClick(e); }

        private void EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        { form.EditingControlShowing(sender, e); }

        private void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        { form.dgvCellPainting(sender, e); }

        private void MergeFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        { form.dgvMergeFormatting(sender, e); }

        //private void NumberFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{ form.dgvNumberFormatting(sender, e); }
        
        private void NumberCellEndEdit(object sender, DataGridViewCellEventArgs e)
        { form.dgvNumberCellEndEdit(sender, e); }

        private void dgvButtonCellContentClick(object sender, DataGridViewCellEventArgs e)
        { form.dgvButtonCellContentClick(sender, e); }

        private void btnCustNewParam_Click(object sender, EventArgs e)
        { button.btnDefineNew("StoreSelection"); }

        private void dgvCustOtherParam_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { form.dgvCustOtherParam_CellContentClick(e); }

        private void dgvLevels_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        { form.dgvLevels(sender, e); }
        
        private void rbAverage_CheckedChanged(object sender, EventArgs e)
        { form.AverageCheckedChange(rbAverage.Checked); }

        private void dgvSalesCritera_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        { form.dgvSalesCritera(sender, e); }
        
        private void dgvColorFormat(object sender, DataGridViewCellEventArgs e)
        { form.dgvColorFormat(sender, e); }

        private void AutoEdit(object sender, DataGridViewCellEventArgs e)
        { form.AutoEdit(sender,e); }

        private void dgvAddRow(object sender, DataGridViewRowStateChangedEventArgs e)
        { form.RowsAdd(); }

        private void rowChangedValue(object sender, DataGridViewCellEventArgs e)
        { form.rowChangedValue(sender, e); }

        private void dgvSearch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        { form.dgvSearch_ColumnHeaderMouseClick(sender, e); }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        { form.txtSearch_TextChanged(sender, e); }

        private void btnAWRFilter_Click(object sender, EventArgs e)
        { dgv.DataGridView_Load("ApprovedRuns"); }
    }
}
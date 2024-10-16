using Context;
using DirecLayer;
using DomainLayer.Helper;
using DomainLayer.Models;
using DomainLayer.Models.Inventory;
using DomainLayer.Models.Inventory.Allocation_Wizard;
using MetroFramework;
using PresenterLayer;
using PresenterLayer.Helper;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PresenterLayer
{
    class AW_Button
    {
        public frmAllocationWizard form { get; set; }
        private SAPHanaAccess sapHana { get; set; }
        private DataHelper helper { get; set; }

        public AW_Button()
        {
            sapHana = new SAPHanaAccess();
            helper = new DataHelper();
        }
        public void btnGenerateClick()
        {
            if (form.btnGenerate.Text.Equals("Cancel"))
            {
                PublicHelper.isCancel = true;
                Application.DoEvents();
            }
            else
            {
                PublicHelper.isCancel = false;

                AW_Generate generate = new AW_Generate();
                form.btnGenerate.Text = "Cancel";
                Application.DoEvents();
                switch (form.MetroTabControl.SelectedTab.Name)
                {
                    case "tabItemSelection":
                        if (form.rbCreateNewAlloc.Checked)
                        {
                            if (string.IsNullOrEmpty(form.dgvItemOtherParam.Rows[5].Cells["Value"].Value.ToString()) || !SelectionModel.Selection.Where(x => x.Type == "IOP" && x.TableID == "U_ID012").Any())
                            {
                                StaticHelper._MainForm.ShowMessage("You must select up to style.",true);
                                return;
                            }

                        }

                        if (form.dgvItemSelected.Rows.Count > 0)
                        {
                            if (MetroMessageBox.Show(StaticHelper._MainForm, "Generating another selection will clear all selected item(s). Do you want to continue?", LibraryHelper.AssemblyInfo.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                generate.form = form;
                                generate.GenerateItemSelection();
                            }
                        }
                        else
                        {
                            generate.form = form;
                            generate.GenerateItemSelection();
                        }
                        break;
                    case "tabStoreSelection":
                        generate.form = form;
                        generate.GenerateStoreSelection();
                        break;
                    case "tabRanking":
                        generate.form = form;
                        generate.GenerateRanking();
                        break;
                    case "tabSummary":
                        generate.form = form;

                        if (form.rbCreateNewAlloc.Checked && string.IsNullOrEmpty(form.txtRemarks.Text))
                        {
                            StaticHelper._MainForm.ShowMessage("Please fillup Remarks field", true);
                            break;
                        }

                        generate.GenerateUploading();
                        break;
                    default:
                        break;
                }
                if (form.MetroTabControl.SelectedTab.Name.Equals("tabSummary"))
                { form.btnGenerate.Text = (form.rbAllocationApproval.Checked ? "&Approve" : "&Upload"); }
                else { form.btnGenerate.Text = "&Generate"; }
                
                Application.DoEvents();
            }
        }

        public void btnCancelClick()
        { form.Close(); }

        public void SelectedIndexChanged()
        {
            if (form.MetroTabControl.TabCount != 0)
            {
                form.btnGenerate.Text = "&Generate";
                switch (form.MetroTabControl.SelectedTab.Name)
                {
                    case "tabWelcome":
                        form.btnGenerate.Enabled = false;
                        form.btnPrev.Enabled = false;
                        form.btnNext.Enabled = true;
                        form.btnFinish.Enabled = false;
                        break;
                    case "tabScenario":
                        form.btnGenerate.Enabled = false;
                        form.btnPrev.Enabled = true;
                        form.btnNext.Enabled = true;
                        form.btnFinish.Enabled = false;
                        break;
                    case "tabAllocWizRuns":
                        form.btnGenerate.Enabled = false;
                        form.btnPrev.Enabled = true;
                        form.btnNext.Enabled = true;
                        form.btnFinish.Enabled = false;
                        break;
                    case "tabItemSelection":
                        form.btnGenerate.Enabled = true;
                        form.btnPrev.Enabled = true;
                        form.btnNext.Enabled = true;
                        form.btnFinish.Enabled = false;
                        break;
                    case "tabAllocationBase":
                        form.btnGenerate.Enabled = false;
                        form.btnPrev.Enabled = true;
                        form.btnNext.Enabled = true;
                        form.btnFinish.Enabled = false;
                        break;
                    case "tabStoreSelection":
                        form.btnGenerate.Enabled = form.rbCreateNewAlloc.Checked;
                        form.btnPrev.Enabled = true;
                        form.btnNext.Enabled = true;
                        form.btnFinish.Enabled = false;
                        break;
                    case "tabRanking":
                        form.btnGenerate.Enabled = !form.rbAllocationApproval.Checked;
                        form.btnPrev.Enabled = true;
                        form.btnNext.Enabled = true;
                        form.btnFinish.Enabled = false;
                        break;
                    case "tabAllocation":
                        form.btnGenerate.Enabled = true;
                        form.btnPrev.Enabled = true;
                        form.btnNext.Enabled = true;
                        form.btnFinish.Enabled = false;
                        break;
                    case "tabSummary":
                        form.btnGenerate.Enabled = true;
                        form.btnPrev.Enabled = true;
                        form.btnNext.Enabled = false;
                        form.btnFinish.Enabled = false;
                        form.btnGenerate.Text = (form.rbAllocationApproval.Checked ? "&Approve" : "&Upload");
                        form.btnGenerate.Enabled = (form.MetroTabControl.TabPages.Count == 4 && form.tabWIzardRuns.SelectedIndex == 1 ? false : true);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                form.btnGenerate.Enabled = false;
                form.btnPrev.Enabled = false;
                form.btnNext.Enabled = true;
                form.btnFinish.Enabled = false;
            }
        }

        public void SelectedTab(bool isNext)
        {
            switch (form.MetroTabControl.SelectedTab.Name)
            {
                case "tabWelcome":

                    if (isNext)
                    {
                        if (form.MetroTabControl.TabPages.Count == 1)
                        { form.MetroTabControl.TabPages.Add(AW_Form.tabScenario); }

                        Load_ScenarioSelection();
                        form.MetroTabControl.SelectTab(AW_Form.tabScenario);
                    }
                    else { form.MetroTabControl.SelectTab(AW_Form.tabWelcome); }
                    break;

                case "tabScenario":

                    if (isNext)
                    {
                        tabRemove();

                        if (form.rbAllocationApproval.Checked)
                        {
                            if (form.MetroTabControl.TabPages.Count == 2)
                            { form.MetroTabControl.TabPages.Add(AW_Form.tabAllocWizRuns); }

                            Load_AllocWizRuns();
                            form.MetroTabControl.SelectTab(AW_Form.tabAllocWizRuns);
                        }
                        else
                        {
                            if (form.MetroTabControl.TabPages.Count == 2)
                            { form.MetroTabControl.TabPages.Add(AW_Form.tabItemSelection); }

                            Load_ItemSelection();
                            form.MetroTabControl.SelectTab(AW_Form.tabItemSelection);
                        }
                    }
                    else { form.MetroTabControl.SelectTab(AW_Form.tabWelcome); }
                    break;

                case "tabAllocWizRuns":

                    if (isNext)
                    {
                        if (form.MetroTabControl.TabPages.Count == 3)
                        {
                            if (form.tabWIzardRuns.SelectedIndex == 1)
                            { form.MetroTabControl.TabPages.Add(AW_Form.tabSummary); }
                            else
                            { form.MetroTabControl.TabPages.Add(AW_Form.tabRanking); }
                                
                        }

                        AW_Generate generate = new AW_Generate();
                        generate.form = form;

                        if (form.tabWIzardRuns.SelectedIndex == 1)
                        {
                            generate.GenerateAllocatedWizRuns();
                            Load_Summary();
                            generate.AllocatedWizRuns();
                            form.MetroTabControl.SelectTab(AW_Form.tabSummary);
                        }
                        else
                        {
                            Load_Ranking();
                            generate.form = form;
                            generate.GenerateAllocWizRuns();
                            form.MetroTabControl.SelectTab(AW_Form.tabRanking);
                        }

                    }
                    else { form.MetroTabControl.SelectTab(AW_Form.tabScenario); }
                    break;

                case "tabItemSelection":

                    if (isNext)
                    {
                        if (form.dgvItemSelected.Rows.Count > 0)
                        {
                            if (form.rbCreateNewAlloc.Checked)
                            {
                                if (form.MetroTabControl.TabPages.Count == 3)
                                { form.MetroTabControl.TabPages.Add(AW_Form.tabAllocationBase); }
                                Load_AllocationBase();
                                form.MetroTabControl.SelectTab(AW_Form.tabAllocationBase);
                            }
                            else if (form.rbRepeatOrder.Checked)
                            {
                                if (form.MetroTabControl.TabPages.Count == 3)
                                { form.MetroTabControl.TabPages.Add(AW_Form.tabStoreSelection); }

                                Load_StoreSelection();
                                StaticHelper._MainForm.ProgressClear();
                                form.MetroTabControl.SelectTab(AW_Form.tabStoreSelection);
                            }
                        }
                        else { StaticHelper._MainForm.ShowMessage("Please select item first.", true); }
                    }
                    else { form.MetroTabControl.SelectTab(AW_Form.tabScenario); }
                    break;

                case "tabAllocationBase":
                    if (isNext)
                    {
                        if (form.MetroTabControl.TabPages.Count == 4)
                        { form.MetroTabControl.TabPages.Add(AW_Form.tabStoreSelection); }
                        Load_StoreSelection();
                        form.MetroTabControl.SelectTab(AW_Form.tabStoreSelection);
                    }
                    else { form.MetroTabControl.SelectTab(AW_Form.tabItemSelection); }
                    break;

                case "tabStoreSelection":
                    if (isNext)
                    {
                        var proceed = true;
                        if (form.rbCreateNewAlloc.Checked)
                        {
                            if (form.MetroTabControl.TabPages.Count == 5)
                            { form.MetroTabControl.TabPages.Add(AW_Form.tabRanking); }

                            for (int i = 0; i < form.dgvCustSelected.Rows.Count; i++)
                            {
                                var dr = form.dgvCustSelected.Rows[i];
                                if (string.IsNullOrEmpty((dr.Cells["Classification"].Value == null ? "" : dr.Cells["Classification"].Value.ToString())))
                                {
                                    proceed = false;
                                    break;
                                }
                            }

                            //foreach (DataGridViewRow dr in form.dgvCustSelected.Rows)
                            //{
                            //    if (string.IsNullOrEmpty((dr.Cells["Classification"].Value == null ? "" : dr.Cells["Classification"].Value.ToString())))
                            //    {
                            //        proceed = false;
                            //        break;
                            //    }
                            //}
                        }
                        else if (form.rbRepeatOrder.Checked)
                        {
                            if (form.MetroTabControl.TabPages.Count == 4)
                            { form.MetroTabControl.TabPages.Add(AW_Form.tabRanking); }
                        }

                        if (proceed)
                        {
                            try
                            {
                                Load_Ranking();
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                MessageBox.Show(ex.StackTrace);
                            }

                            form.MetroTabControl.SelectTab(AW_Form.tabRanking);
                        }
                        else { StaticHelper._MainForm.ShowMessage("Cannot proceed if you selected a missing classification.", true); }
                    }
                    else
                    {
                        if (form.rbCreateNewAlloc.Checked)
                        { form.MetroTabControl.SelectTab(AW_Form.tabAllocationBase); }
                        else if (form.rbRepeatOrder.Checked)
                        { form.MetroTabControl.SelectTab(AW_Form.tabItemSelection); }
                    }
                    break;

                case "tabRanking":
                    if (isNext)
                    {
                        if (form.dgvAllocation.Rows.Count > 0)
                        {
                            if ((form.rbCreateNewAlloc.Checked && form.MetroTabControl.TabPages.Count == 6) ||
                                (form.rbRepeatOrder.Checked && form.MetroTabControl.TabPages.Count == 5) ||
                                (form.rbAllocationApproval.Checked && form.MetroTabControl.TabPages.Count == 4))
                            { form.MetroTabControl.TabPages.Add(AW_Form.tabSummary); }

                            Load_Summary();
                            StaticHelper._MainForm.ProgressClear();
                            form.MetroTabControl.SelectTab(AW_Form.tabSummary);
                        }
                        else { StaticHelper._MainForm.ShowMessage("Please generate ranking first.", true); }

                    }
                    else
                    {
                        if (form.rbAllocationApproval.Checked)
                        { form.MetroTabControl.SelectTab(AW_Form.tabAllocWizRuns); }
                        else
                        { form.MetroTabControl.SelectTab(AW_Form.tabStoreSelection); }
                    }
                    break;

                case "tabSummary":
                    if (!isNext)
                    {
                        if (form.MetroTabControl.TabPages.Count == 4 && form.tabWIzardRuns.SelectedIndex == 1)
                        { form.MetroTabControl.SelectTab(AW_Form.tabAllocWizRuns); }
                        else
                        { form.MetroTabControl.SelectTab(AW_Form.tabRanking); }
                    }
                    break;

                default:
                    break;
            }
        }

        void tabRemove()
        {
            var model = SelectionModel.Selection.Where(x => x.Type == "MD");

            if (model.Any())
            {
                foreach (var item in model.ToList())
                { SelectionModel.Selection.Remove(item); }
            }
            
            foreach (TabPage item in form.MetroTabControl.TabPages)
            {
                if (item.Name == "tabAllocWizRuns")
                { form.MetroTabControl.TabPages.Remove(AW_Form.tabAllocWizRuns); }

                if (item.Name == "tabItemSelection")
                { form.MetroTabControl.TabPages.Remove(AW_Form.tabItemSelection); }

                if (item.Name == "tabAllocationBase")
                { form.MetroTabControl.TabPages.Remove(AW_Form.tabAllocationBase); }

                if (item.Name == "tabStoreSelection")
                { form.MetroTabControl.TabPages.Remove(AW_Form.tabStoreSelection); }

                if (item.Name == "tabRanking")
                { form.MetroTabControl.TabPages.Remove(AW_Form.tabRanking); }

                if (item.Name == "tabSummary")
                { form.MetroTabControl.TabPages.Remove(AW_Form.tabSummary); }
            }
        }

        void Load_ItemSelection()
        {
            AW_DataGridview dgv = new AW_DataGridview();
            dgv.form = form;

            dgv.DataGridView_Load("Whs");
            dgv.DataGridView_Load("MarketingDocs");
            dgv.DataGridView_Load("ItemOtherParam");

            dgv.DataGridView_Load("ItemSelection");
            dgv.DataGridView_Load("ItemSelected");
        }

        void Load_ScenarioSelection()
        {
            var sQuery = helper.ReadDataRow(sapHana.Get(SP.AW_GetUserAccess), 1, "", 0);
            var UserId = EasySAPCredentialsModel.ESUserId;
            form.rbCreateNewAlloc.Visible =  helper.ReadDataRow(sapHana.Get(string.Format(sQuery, UserId, "1")), 0,"",0) == "1" ? true : false;
            form.rbRepeatOrder.Visible = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, UserId, "2")), 0,"",0) == "2" ? true : false;
            form.rbAllocationApproval.Visible = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, UserId, "3")), 0,"",0) == "3" ? true : false;
        }

        void Load_AllocWizRuns()
        {
            AW_DataGridview dgv = new AW_DataGridview();
            dgv.form = form;

            dgv.DataGridView_Load("AllocWizRuns");
            dgv.DataGridView_Load("ApprovedRuns");
        }

        void Load_AllocationBase()
        {
            AW_DataGridview dgv = new AW_DataGridview();
            dgv.form = form;

            dgv.DataGridView_Load("AllocationBase");
        }

        void Load_StoreSelection()
        {
            AW_DataGridview dgv = new AW_DataGridview();
            dgv.form = form;

            dgv.DataGridView_Load("StoreCriteria");
            dgv.DataGridView_Load("CustOtherParam");
            dgv.DataGridView_Load("CustSelection");
            dgv.DataGridView_Load("CustSelected");
        }

        void Load_Ranking()
        {
            AW_DataGridview dgv = new AW_DataGridview();
            dgv.form = form;

            form.grpSalesHorizon.Enabled = !form.rbAllocationApproval.Checked;
            form.grpValueBased.Enabled = !form.rbAllocationApproval.Checked;

            form.rbTotalSales.Enabled = !form.rbAllocationApproval.Checked;
            form.rbAverage.Enabled = !form.rbAllocationApproval.Checked;
            form.cbAverage.Enabled = !form.rbAllocationApproval.Checked;

            dgv.DataGridView_Load("Levels");
            dgv.DataGridView_Load("SalesCritera");
            dgv.DataGridView_Load("Allocation");
            form.dgvLevels.Enabled = !form.rbAllocationApproval.Checked;
            form.dgvSalesCritera.Enabled = !form.rbAllocationApproval.Checked;
        }

        void Load_Summary()
        {
            AW_DataGridview dgv = new AW_DataGridview();
            dgv.form = form;

            var isCreateNew = form.rbCreateNewAlloc.Checked;

            form.txtRemarks.Enabled = isCreateNew;
            form.dtpDocDate.Enabled = isCreateNew;
            form.dtpDueDate.Enabled = isCreateNew;
            form.dtpTaxDate.Enabled = isCreateNew;
            dgv.DataGridView_Load("Parameters");
            form.dgvParameter.Enabled = isCreateNew;

            dgv.DataGridView_Load("Summary");
        }

        public void btnFinishClick()
        { form.Close(); }

        public void btnDefineNew(string sVal)
        {
            frmAW_DefineNew frm = new frmAW_DefineNew();
            frm.form = form;
            switch (sVal)
            {
                case "ItemSelection":
                    frm.sCode = "IOP";
                    frm.dt = sapHana.Get(SP.AW_IOP);
                    frm.dgv = form.dgvItemOtherParam;
                    break;
                case "StoreSelection":
                    frm.sCode = "COP";
                    frm.dt = sapHana.Get(SP.AW_COP);
                    frm.dgv = form.dgvCustOtherParam;
                    break;
                default:
                    break;
            }
            frm.MdiParent = StaticHelper._MainForm;
            frm.Show();
        }

    }
}

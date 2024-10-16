using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresenterLayer
{
    partial class frmAllocationWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAllocationWizard));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGenerate = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.btnFinish = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.btnPrev = new MetroFramework.Controls.MetroButton();
            this.btnNext = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.MetroTabControl = new MetroFramework.Controls.MetroTabControl();
            this.tabWelcome = new MetroFramework.Controls.MetroTabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabScenario = new MetroFramework.Controls.MetroTabPage();
            this.rbAllocationApproval = new System.Windows.Forms.RadioButton();
            this.rbRepeatOrder = new System.Windows.Forms.RadioButton();
            this.rbCreateNewAlloc = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabAllocWizRuns = new MetroFramework.Controls.MetroTabPage();
            this.tabWIzardRuns = new System.Windows.Forms.TabControl();
            this.tabApprovalRun = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSearchApprovalRuns = new System.Windows.Forms.TextBox();
            this.dgvAllocWizRuns = new System.Windows.Forms.DataGridView();
            this.tabAllocationRun = new System.Windows.Forms.TabPage();
            this.btnAWRFilter = new MetroFramework.Controls.MetroButton();
            this.label16 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpApprovedTo = new System.Windows.Forms.DateTimePicker();
            this.dtpApprovedFrom = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSearchApprovedRuns = new System.Windows.Forms.TextBox();
            this.dgvApprovedRuns = new System.Windows.Forms.DataGridView();
            this.tabItemSelection = new MetroFramework.Controls.MetroTabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvWhs = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnItemNewParam = new System.Windows.Forms.Button();
            this.dgvItemOtherParam = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvMarketingDocs = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvItemSelected = new System.Windows.Forms.DataGridView();
            this.dgvItemSelection = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.navItemBack = new MetroFramework.Controls.MetroButton();
            this.navItemGet = new MetroFramework.Controls.MetroButton();
            this.navItemGetAll = new MetroFramework.Controls.MetroButton();
            this.navItemBackAll = new MetroFramework.Controls.MetroButton();
            this.tabAllocationBase = new MetroFramework.Controls.MetroTabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cmbWorkSheet = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtExcelName = new System.Windows.Forms.TextBox();
            this.dgvAllocationBase = new System.Windows.Forms.DataGridView();
            this.tabStoreSelection = new MetroFramework.Controls.MetroTabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvStoreCriteria = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnCustNewParam = new System.Windows.Forms.Button();
            this.dgvCustOtherParam = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvCustSelection = new System.Windows.Forms.DataGridView();
            this.dgvCustSelected = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.navCustGet = new MetroFramework.Controls.MetroButton();
            this.navCustBack = new MetroFramework.Controls.MetroButton();
            this.navCustBackAll = new MetroFramework.Controls.MetroButton();
            this.navCustGetAll = new MetroFramework.Controls.MetroButton();
            this.tabRanking = new MetroFramework.Controls.MetroTabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rbAverage = new System.Windows.Forms.RadioButton();
            this.rbTotalSales = new System.Windows.Forms.RadioButton();
            this.cbAverage = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dgvLevels = new System.Windows.Forms.DataGridView();
            this.grpSalesHorizon = new System.Windows.Forms.GroupBox();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.grpValueBased = new System.Windows.Forms.GroupBox();
            this.rbQuantity = new System.Windows.Forms.RadioButton();
            this.rbAmount = new System.Windows.Forms.RadioButton();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.dgvSalesCritera = new System.Windows.Forms.DataGridView();
            this.dgvAllocation = new System.Windows.Forms.DataGridView();
            this.tabSummary = new MetroFramework.Controls.MetroTabPage();
            this.dgvParameter = new System.Windows.Forms.DataGridView();
            this.label13 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.dtpTaxDate = new System.Windows.Forms.DateTimePicker();
            this.dtpDueDate = new System.Windows.Forms.DateTimePicker();
            this.dtpDocDate = new System.Windows.Forms.DateTimePicker();
            this.dgvSummary = new System.Windows.Forms.DataGridView();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.MetroTabControl.SuspendLayout();
            this.tabWelcome.SuspendLayout();
            this.tabScenario.SuspendLayout();
            this.tabAllocWizRuns.SuspendLayout();
            this.tabWIzardRuns.SuspendLayout();
            this.tabApprovalRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllocWizRuns)).BeginInit();
            this.tabAllocationRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApprovedRuns)).BeginInit();
            this.tabItemSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWhs)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemOtherParam)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarketingDocs)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemSelection)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabAllocationBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllocationBase)).BeginInit();
            this.tabStoreSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStoreCriteria)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustOtherParam)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustSelected)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tabRanking.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLevels)).BeginInit();
            this.grpSalesHorizon.SuspendLayout();
            this.grpValueBased.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalesCritera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllocation)).BeginInit();
            this.tabSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGenerate);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnFinish);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(20, 584);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 36);
            this.panel1.TabIndex = 0;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Image = null;
            this.btnGenerate.Location = new System.Drawing.Point(10, 4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(129, 29);
            this.btnGenerate.TabIndex = 55;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseSelectable = true;
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(458, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(129, 29);
            this.btnCancel.TabIndex = 54;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinish.Enabled = false;
            this.btnFinish.Image = null;
            this.btnFinish.Location = new System.Drawing.Point(861, 4);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(129, 29);
            this.btnFinish.TabIndex = 53;
            this.btnFinish.Text = "&Finish";
            this.btnFinish.UseSelectable = true;
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.Enabled = false;
            this.btnPrev.Location = new System.Drawing.Point(593, 4);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(5);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(129, 29);
            this.btnPrev.TabIndex = 52;
            this.btnPrev.Text = "< &Back";
            this.btnPrev.UseSelectable = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Image = null;
            this.btnNext.Location = new System.Drawing.Point(728, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(129, 29);
            this.btnNext.TabIndex = 51;
            this.btnNext.Text = "&Next >";
            this.btnNext.UseSelectable = true;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.MetroTabControl);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(20, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1000, 524);
            this.panel2.TabIndex = 1;
            // 
            // MetroTabControl
            // 
            this.MetroTabControl.Controls.Add(this.tabWelcome);
            this.MetroTabControl.Controls.Add(this.tabScenario);
            this.MetroTabControl.Controls.Add(this.tabAllocWizRuns);
            this.MetroTabControl.Controls.Add(this.tabItemSelection);
            this.MetroTabControl.Controls.Add(this.tabAllocationBase);
            this.MetroTabControl.Controls.Add(this.tabStoreSelection);
            this.MetroTabControl.Controls.Add(this.tabRanking);
            this.MetroTabControl.Controls.Add(this.tabSummary);
            this.MetroTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetroTabControl.Location = new System.Drawing.Point(0, 0);
            this.MetroTabControl.Name = "MetroTabControl";
            this.MetroTabControl.SelectedIndex = 3;
            this.MetroTabControl.Size = new System.Drawing.Size(1000, 524);
            this.MetroTabControl.TabIndex = 45;
            this.MetroTabControl.UseSelectable = true;
            this.MetroTabControl.SelectedIndexChanged += new System.EventHandler(this.MetroTabControl_SelectedIndexChanged);
            // 
            // tabWelcome
            // 
            this.tabWelcome.AutoScroll = true;
            this.tabWelcome.Controls.Add(this.label2);
            this.tabWelcome.Controls.Add(this.label1);
            this.tabWelcome.HorizontalScrollbar = true;
            this.tabWelcome.HorizontalScrollbarBarColor = true;
            this.tabWelcome.HorizontalScrollbarHighlightOnWheel = false;
            this.tabWelcome.HorizontalScrollbarSize = 10;
            this.tabWelcome.Location = new System.Drawing.Point(4, 38);
            this.tabWelcome.Name = "tabWelcome";
            this.tabWelcome.Size = new System.Drawing.Size(992, 482);
            this.tabWelcome.TabIndex = 0;
            this.tabWelcome.Text = "Welcome";
            this.tabWelcome.VerticalScrollbar = true;
            this.tabWelcome.VerticalScrollbarBarColor = true;
            this.tabWelcome.VerticalScrollbarHighlightOnWheel = false;
            this.tabWelcome.VerticalScrollbarSize = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(21, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(317, 91);
            this.label2.TabIndex = 3;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Allocation Wizard";
            // 
            // tabScenario
            // 
            this.tabScenario.Controls.Add(this.rbAllocationApproval);
            this.tabScenario.Controls.Add(this.rbRepeatOrder);
            this.tabScenario.Controls.Add(this.rbCreateNewAlloc);
            this.tabScenario.Controls.Add(this.label3);
            this.tabScenario.Controls.Add(this.label4);
            this.tabScenario.HorizontalScrollbarBarColor = true;
            this.tabScenario.HorizontalScrollbarHighlightOnWheel = false;
            this.tabScenario.HorizontalScrollbarSize = 10;
            this.tabScenario.Location = new System.Drawing.Point(4, 38);
            this.tabScenario.Name = "tabScenario";
            this.tabScenario.Size = new System.Drawing.Size(992, 482);
            this.tabScenario.TabIndex = 1;
            this.tabScenario.Text = "Scenario Selection";
            this.tabScenario.VerticalScrollbarBarColor = true;
            this.tabScenario.VerticalScrollbarHighlightOnWheel = false;
            this.tabScenario.VerticalScrollbarSize = 10;
            // 
            // rbAllocationApproval
            // 
            this.rbAllocationApproval.AutoSize = true;
            this.rbAllocationApproval.BackColor = System.Drawing.Color.Transparent;
            this.rbAllocationApproval.Location = new System.Drawing.Point(48, 154);
            this.rbAllocationApproval.Name = "rbAllocationApproval";
            this.rbAllocationApproval.Size = new System.Drawing.Size(115, 17);
            this.rbAllocationApproval.TabIndex = 8;
            this.rbAllocationApproval.Text = "&Allocation approval";
            this.rbAllocationApproval.UseVisualStyleBackColor = false;
            // 
            // rbRepeatOrder
            // 
            this.rbRepeatOrder.AutoSize = true;
            this.rbRepeatOrder.BackColor = System.Drawing.Color.Transparent;
            this.rbRepeatOrder.Location = new System.Drawing.Point(48, 131);
            this.rbRepeatOrder.Name = "rbRepeatOrder";
            this.rbRepeatOrder.Size = new System.Drawing.Size(131, 17);
            this.rbRepeatOrder.TabIndex = 7;
            this.rbRepeatOrder.Text = "&Repeat order approval";
            this.rbRepeatOrder.UseVisualStyleBackColor = false;
            // 
            // rbCreateNewAlloc
            // 
            this.rbCreateNewAlloc.AutoSize = true;
            this.rbCreateNewAlloc.BackColor = System.Drawing.Color.Transparent;
            this.rbCreateNewAlloc.Checked = true;
            this.rbCreateNewAlloc.Location = new System.Drawing.Point(48, 108);
            this.rbCreateNewAlloc.Name = "rbCreateNewAlloc";
            this.rbCreateNewAlloc.Size = new System.Drawing.Size(127, 17);
            this.rbCreateNewAlloc.TabIndex = 6;
            this.rbCreateNewAlloc.TabStop = true;
            this.rbCreateNewAlloc.Text = "Cr&eate new allocation";
            this.rbCreateNewAlloc.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(21, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "You need to choose from the following scenario";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(210, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Scenario Selection";
            // 
            // tabAllocWizRuns
            // 
            this.tabAllocWizRuns.Controls.Add(this.tabWIzardRuns);
            this.tabAllocWizRuns.HorizontalScrollbarBarColor = true;
            this.tabAllocWizRuns.HorizontalScrollbarHighlightOnWheel = false;
            this.tabAllocWizRuns.HorizontalScrollbarSize = 10;
            this.tabAllocWizRuns.Location = new System.Drawing.Point(4, 38);
            this.tabAllocWizRuns.Name = "tabAllocWizRuns";
            this.tabAllocWizRuns.Size = new System.Drawing.Size(992, 482);
            this.tabAllocWizRuns.TabIndex = 8;
            this.tabAllocWizRuns.Text = "Allocation Wizard Runs";
            this.tabAllocWizRuns.VerticalScrollbarBarColor = true;
            this.tabAllocWizRuns.VerticalScrollbarHighlightOnWheel = false;
            this.tabAllocWizRuns.VerticalScrollbarSize = 10;
            // 
            // tabWIzardRuns
            // 
            this.tabWIzardRuns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabWIzardRuns.Controls.Add(this.tabApprovalRun);
            this.tabWIzardRuns.Controls.Add(this.tabAllocationRun);
            this.tabWIzardRuns.Location = new System.Drawing.Point(0, 13);
            this.tabWIzardRuns.Name = "tabWIzardRuns";
            this.tabWIzardRuns.SelectedIndex = 0;
            this.tabWIzardRuns.Size = new System.Drawing.Size(992, 466);
            this.tabWIzardRuns.TabIndex = 7;
            // 
            // tabApprovalRun
            // 
            this.tabApprovalRun.Controls.Add(this.label7);
            this.tabApprovalRun.Controls.Add(this.txtSearchApprovalRuns);
            this.tabApprovalRun.Controls.Add(this.dgvAllocWizRuns);
            this.tabApprovalRun.Location = new System.Drawing.Point(4, 22);
            this.tabApprovalRun.Name = "tabApprovalRun";
            this.tabApprovalRun.Padding = new System.Windows.Forms.Padding(3);
            this.tabApprovalRun.Size = new System.Drawing.Size(984, 440);
            this.tabApprovalRun.TabIndex = 0;
            this.tabApprovalRun.Text = "For Approval Runs";
            this.tabApprovalRun.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 16);
            this.label7.TabIndex = 59;
            this.label7.Text = "Find :";
            // 
            // txtSearchApprovalRuns
            // 
            this.txtSearchApprovalRuns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchApprovalRuns.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchApprovalRuns.Location = new System.Drawing.Point(52, 6);
            this.txtSearchApprovalRuns.Name = "txtSearchApprovalRuns";
            this.txtSearchApprovalRuns.Size = new System.Drawing.Size(516, 22);
            this.txtSearchApprovalRuns.TabIndex = 58;
            this.txtSearchApprovalRuns.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // dgvAllocWizRuns
            // 
            this.dgvAllocWizRuns.AllowUserToAddRows = false;
            this.dgvAllocWizRuns.AllowUserToDeleteRows = false;
            this.dgvAllocWizRuns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAllocWizRuns.BackgroundColor = System.Drawing.Color.White;
            this.dgvAllocWizRuns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllocWizRuns.Location = new System.Drawing.Point(3, 36);
            this.dgvAllocWizRuns.MultiSelect = false;
            this.dgvAllocWizRuns.Name = "dgvAllocWizRuns";
            this.dgvAllocWizRuns.ReadOnly = true;
            this.dgvAllocWizRuns.Size = new System.Drawing.Size(978, 401);
            this.dgvAllocWizRuns.TabIndex = 2;
            this.dgvAllocWizRuns.Tag = "WHS";
            this.dgvAllocWizRuns.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSearch_ColumnHeaderMouseClick);
            this.dgvAllocWizRuns.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRowPostPaint);
            // 
            // tabAllocationRun
            // 
            this.tabAllocationRun.Controls.Add(this.btnAWRFilter);
            this.tabAllocationRun.Controls.Add(this.label16);
            this.tabAllocationRun.Controls.Add(this.label9);
            this.tabAllocationRun.Controls.Add(this.dtpApprovedTo);
            this.tabAllocationRun.Controls.Add(this.dtpApprovedFrom);
            this.tabAllocationRun.Controls.Add(this.label8);
            this.tabAllocationRun.Controls.Add(this.txtSearchApprovedRuns);
            this.tabAllocationRun.Controls.Add(this.dgvApprovedRuns);
            this.tabAllocationRun.Location = new System.Drawing.Point(4, 22);
            this.tabAllocationRun.Name = "tabAllocationRun";
            this.tabAllocationRun.Padding = new System.Windows.Forms.Padding(3);
            this.tabAllocationRun.Size = new System.Drawing.Size(984, 440);
            this.tabAllocationRun.TabIndex = 1;
            this.tabAllocationRun.Text = "Approved Allocation Runs";
            this.tabAllocationRun.UseVisualStyleBackColor = true;
            // 
            // btnAWRFilter
            // 
            this.btnAWRFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAWRFilter.Location = new System.Drawing.Point(916, 7);
            this.btnAWRFilter.Margin = new System.Windows.Forms.Padding(5);
            this.btnAWRFilter.Name = "btnAWRFilter";
            this.btnAWRFilter.Size = new System.Drawing.Size(61, 20);
            this.btnAWRFilter.TabIndex = 65;
            this.btnAWRFilter.Text = "Filte&r";
            this.btnAWRFilter.UseSelectable = true;
            this.btnAWRFilter.Click += new System.EventHandler(this.btnAWRFilter_Click);
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(758, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(12, 16);
            this.label16.TabIndex = 64;
            this.label16.Text = "-";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(575, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 16);
            this.label9.TabIndex = 63;
            this.label9.Text = "Date :";
            // 
            // dtpApprovedTo
            // 
            this.dtpApprovedTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpApprovedTo.CustomFormat = "MM.dd.yyyy";
            this.dtpApprovedTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpApprovedTo.Location = new System.Drawing.Point(776, 7);
            this.dtpApprovedTo.Name = "dtpApprovedTo";
            this.dtpApprovedTo.Size = new System.Drawing.Size(132, 20);
            this.dtpApprovedTo.TabIndex = 62;
            // 
            // dtpApprovedFrom
            // 
            this.dtpApprovedFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpApprovedFrom.CustomFormat = "MM.dd.yyyy";
            this.dtpApprovedFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpApprovedFrom.Location = new System.Drawing.Point(620, 7);
            this.dtpApprovedFrom.Name = "dtpApprovedFrom";
            this.dtpApprovedFrom.Size = new System.Drawing.Size(132, 20);
            this.dtpApprovedFrom.TabIndex = 62;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 16);
            this.label8.TabIndex = 61;
            this.label8.Text = "Find :";
            // 
            // txtSearchApprovedRuns
            // 
            this.txtSearchApprovedRuns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchApprovedRuns.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchApprovedRuns.Location = new System.Drawing.Point(52, 6);
            this.txtSearchApprovedRuns.Name = "txtSearchApprovedRuns";
            this.txtSearchApprovedRuns.Size = new System.Drawing.Size(517, 22);
            this.txtSearchApprovedRuns.TabIndex = 60;
            this.txtSearchApprovedRuns.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // dgvApprovedRuns
            // 
            this.dgvApprovedRuns.AllowUserToAddRows = false;
            this.dgvApprovedRuns.AllowUserToDeleteRows = false;
            this.dgvApprovedRuns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvApprovedRuns.BackgroundColor = System.Drawing.Color.White;
            this.dgvApprovedRuns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApprovedRuns.Location = new System.Drawing.Point(3, 36);
            this.dgvApprovedRuns.MultiSelect = false;
            this.dgvApprovedRuns.Name = "dgvApprovedRuns";
            this.dgvApprovedRuns.ReadOnly = true;
            this.dgvApprovedRuns.Size = new System.Drawing.Size(979, 401);
            this.dgvApprovedRuns.TabIndex = 3;
            this.dgvApprovedRuns.Tag = "WHS";
            this.dgvApprovedRuns.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSearch_ColumnHeaderMouseClick);
            this.dgvApprovedRuns.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRowPostPaint);
            // 
            // tabItemSelection
            // 
            this.tabItemSelection.Controls.Add(this.splitContainer1);
            this.tabItemSelection.HorizontalScrollbarBarColor = true;
            this.tabItemSelection.HorizontalScrollbarHighlightOnWheel = false;
            this.tabItemSelection.HorizontalScrollbarSize = 10;
            this.tabItemSelection.Location = new System.Drawing.Point(4, 38);
            this.tabItemSelection.Name = "tabItemSelection";
            this.tabItemSelection.Size = new System.Drawing.Size(992, 482);
            this.tabItemSelection.TabIndex = 2;
            this.tabItemSelection.Text = "Item Selection";
            this.tabItemSelection.VerticalScrollbarBarColor = true;
            this.tabItemSelection.VerticalScrollbarHighlightOnWheel = false;
            this.tabItemSelection.VerticalScrollbarSize = 10;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(992, 482);
            this.splitContainer1.SplitterDistance = 168;
            this.splitContainer1.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox3, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(992, 168);
            this.tableLayoutPanel3.TabIndex = 52;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.dgvWhs);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(324, 162);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Warehouse";
            // 
            // dgvWhs
            // 
            this.dgvWhs.AllowUserToAddRows = false;
            this.dgvWhs.AllowUserToDeleteRows = false;
            this.dgvWhs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWhs.BackgroundColor = System.Drawing.Color.White;
            this.dgvWhs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWhs.Location = new System.Drawing.Point(6, 19);
            this.dgvWhs.Name = "dgvWhs";
            this.dgvWhs.Size = new System.Drawing.Size(312, 137);
            this.dgvWhs.TabIndex = 1;
            this.dgvWhs.Tag = "WHS";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.btnItemNewParam);
            this.groupBox3.Controls.Add(this.dgvItemOtherParam);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(663, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(326, 162);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Other Parameter";
            // 
            // btnItemNewParam
            // 
            this.btnItemNewParam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemNewParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnItemNewParam.Location = new System.Drawing.Point(6, 135);
            this.btnItemNewParam.Name = "btnItemNewParam";
            this.btnItemNewParam.Size = new System.Drawing.Size(314, 22);
            this.btnItemNewParam.TabIndex = 2;
            this.btnItemNewParam.Tag = "IOP";
            this.btnItemNewParam.Text = "Define Parameter";
            this.btnItemNewParam.UseVisualStyleBackColor = true;
            this.btnItemNewParam.Click += new System.EventHandler(this.btnItemNewParam_Click);
            // 
            // dgvItemOtherParam
            // 
            this.dgvItemOtherParam.AllowUserToAddRows = false;
            this.dgvItemOtherParam.AllowUserToDeleteRows = false;
            this.dgvItemOtherParam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItemOtherParam.BackgroundColor = System.Drawing.Color.White;
            this.dgvItemOtherParam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemOtherParam.Location = new System.Drawing.Point(6, 19);
            this.dgvItemOtherParam.Name = "dgvItemOtherParam";
            this.dgvItemOtherParam.ReadOnly = true;
            this.dgvItemOtherParam.Size = new System.Drawing.Size(314, 117);
            this.dgvItemOtherParam.TabIndex = 1;
            this.dgvItemOtherParam.Tag = "IOP";
            this.dgvItemOtherParam.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemOtherParam_CellContentClick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dgvMarketingDocs);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(333, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 162);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Marketing Documents";
            // 
            // dgvMarketingDocs
            // 
            this.dgvMarketingDocs.AllowUserToAddRows = false;
            this.dgvMarketingDocs.AllowUserToDeleteRows = false;
            this.dgvMarketingDocs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMarketingDocs.BackgroundColor = System.Drawing.Color.White;
            this.dgvMarketingDocs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMarketingDocs.Location = new System.Drawing.Point(6, 19);
            this.dgvMarketingDocs.Name = "dgvMarketingDocs";
            this.dgvMarketingDocs.ReadOnly = true;
            this.dgvMarketingDocs.Size = new System.Drawing.Size(312, 137);
            this.dgvMarketingDocs.TabIndex = 0;
            this.dgvMarketingDocs.Tag = "MD";
            this.dgvMarketingDocs.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvButtonCellContentClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.36842F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.36842F));
            this.tableLayoutPanel1.Controls.Add(this.dgvItemSelected, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvItemSelection, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(992, 310);
            this.tableLayoutPanel1.TabIndex = 55;
            // 
            // dgvItemSelected
            // 
            this.dgvItemSelected.AllowUserToAddRows = false;
            this.dgvItemSelected.AllowUserToDeleteRows = false;
            this.dgvItemSelected.BackgroundColor = System.Drawing.Color.White;
            this.dgvItemSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItemSelected.Location = new System.Drawing.Point(524, 3);
            this.dgvItemSelected.Name = "dgvItemSelected";
            this.dgvItemSelected.Size = new System.Drawing.Size(465, 304);
            this.dgvItemSelected.TabIndex = 59;
            this.dgvItemSelected.Tag = "ItemSelected";
            this.dgvItemSelected.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.NumberCellEndEdit);
            this.dgvItemSelected.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.AutoEdit);
            this.dgvItemSelected.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.rowChangedValue);
            this.dgvItemSelected.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.EditingControlShowing);
            this.dgvItemSelected.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRowPostPaint);
            // 
            // dgvItemSelection
            // 
            this.dgvItemSelection.AllowUserToAddRows = false;
            this.dgvItemSelection.AllowUserToDeleteRows = false;
            this.dgvItemSelection.BackgroundColor = System.Drawing.Color.White;
            this.dgvItemSelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItemSelection.Location = new System.Drawing.Point(3, 3);
            this.dgvItemSelection.Name = "dgvItemSelection";
            this.dgvItemSelection.ReadOnly = true;
            this.dgvItemSelection.Size = new System.Drawing.Size(463, 304);
            this.dgvItemSelection.TabIndex = 57;
            this.dgvItemSelection.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRowPostPaint);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.navItemBack, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.navItemGet, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.navItemGetAll, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.navItemBackAll, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(472, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(46, 304);
            this.tableLayoutPanel2.TabIndex = 58;
            // 
            // navItemBack
            // 
            this.navItemBack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.navItemBack.Location = new System.Drawing.Point(6, 251);
            this.navItemBack.Margin = new System.Windows.Forms.Padding(5);
            this.navItemBack.Name = "navItemBack";
            this.navItemBack.Size = new System.Drawing.Size(34, 29);
            this.navItemBack.TabIndex = 65;
            this.navItemBack.Text = "<";
            this.navItemBack.UseSelectable = true;
            this.navItemBack.Click += new System.EventHandler(this.navClick);
            // 
            // navItemGet
            // 
            this.navItemGet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.navItemGet.Location = new System.Drawing.Point(6, 23);
            this.navItemGet.Margin = new System.Windows.Forms.Padding(5);
            this.navItemGet.Name = "navItemGet";
            this.navItemGet.Size = new System.Drawing.Size(34, 29);
            this.navItemGet.TabIndex = 62;
            this.navItemGet.Text = ">";
            this.navItemGet.UseSelectable = true;
            this.navItemGet.Click += new System.EventHandler(this.navClick);
            // 
            // navItemGetAll
            // 
            this.navItemGetAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.navItemGetAll.Location = new System.Drawing.Point(6, 99);
            this.navItemGetAll.Margin = new System.Windows.Forms.Padding(5);
            this.navItemGetAll.Name = "navItemGetAll";
            this.navItemGetAll.Size = new System.Drawing.Size(34, 29);
            this.navItemGetAll.TabIndex = 63;
            this.navItemGetAll.Text = ">>";
            this.navItemGetAll.UseSelectable = true;
            this.navItemGetAll.Click += new System.EventHandler(this.navClick);
            // 
            // navItemBackAll
            // 
            this.navItemBackAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.navItemBackAll.Location = new System.Drawing.Point(6, 175);
            this.navItemBackAll.Margin = new System.Windows.Forms.Padding(5);
            this.navItemBackAll.Name = "navItemBackAll";
            this.navItemBackAll.Size = new System.Drawing.Size(34, 29);
            this.navItemBackAll.TabIndex = 64;
            this.navItemBackAll.Text = "<<";
            this.navItemBackAll.UseSelectable = true;
            this.navItemBackAll.Click += new System.EventHandler(this.navClick);
            // 
            // tabAllocationBase
            // 
            this.tabAllocationBase.Controls.Add(this.splitContainer4);
            this.tabAllocationBase.HorizontalScrollbarBarColor = true;
            this.tabAllocationBase.HorizontalScrollbarHighlightOnWheel = false;
            this.tabAllocationBase.HorizontalScrollbarSize = 10;
            this.tabAllocationBase.Location = new System.Drawing.Point(4, 38);
            this.tabAllocationBase.Name = "tabAllocationBase";
            this.tabAllocationBase.Size = new System.Drawing.Size(992, 482);
            this.tabAllocationBase.TabIndex = 7;
            this.tabAllocationBase.Text = "Allocation Base";
            this.tabAllocationBase.VerticalScrollbarBarColor = true;
            this.tabAllocationBase.VerticalScrollbarHighlightOnWheel = false;
            this.tabAllocationBase.VerticalScrollbarSize = 10;
            // 
            // splitContainer4
            // 
            this.splitContainer4.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.dataGridView2);
            this.splitContainer4.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainer4.Panel1.Controls.Add(this.cmbWorkSheet);
            this.splitContainer4.Panel1.Controls.Add(this.label15);
            this.splitContainer4.Panel1.Controls.Add(this.label14);
            this.splitContainer4.Panel1.Controls.Add(this.btnOpenFile);
            this.splitContainer4.Panel1.Controls.Add(this.txtExcelName);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.dgvAllocationBase);
            this.splitContainer4.Size = new System.Drawing.Size(992, 482);
            this.splitContainer4.SplitterDistance = 25;
            this.splitContainer4.TabIndex = 5;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(-21, 69);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(500, 0);
            this.dataGridView2.TabIndex = 49;
            this.dataGridView2.Tag = "SC";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(517, 10);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(497, 5);
            this.dataGridView1.TabIndex = 48;
            this.dataGridView1.Tag = "SC";
            this.dataGridView1.Visible = false;
            // 
            // cmbWorkSheet
            // 
            this.cmbWorkSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbWorkSheet.Font = new System.Drawing.Font("Calibri", 10F);
            this.cmbWorkSheet.FormattingEnabled = true;
            this.cmbWorkSheet.Location = new System.Drawing.Point(48, 41);
            this.cmbWorkSheet.Name = "cmbWorkSheet";
            this.cmbWorkSheet.Size = new System.Drawing.Size(432, 23);
            this.cmbWorkSheet.TabIndex = 47;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Calibri", 10F);
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(78)))));
            this.label15.Location = new System.Drawing.Point(-21, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(68, 17);
            this.label15.TabIndex = 46;
            this.label15.Text = "Worksheet";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Calibri", 10F);
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(78)))));
            this.label14.Location = new System.Drawing.Point(-12, 14);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 17);
            this.label14.TabIndex = 42;
            this.label14.Text = "Excel File";
            this.label14.Visible = false;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOpenFile.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenFile.Image")));
            this.btnOpenFile.Location = new System.Drawing.Point(444, 11);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(36, 24);
            this.btnOpenFile.TabIndex = 44;
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Visible = false;
            // 
            // txtExcelName
            // 
            this.txtExcelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtExcelName.Font = new System.Drawing.Font("Calibri", 10F);
            this.txtExcelName.Location = new System.Drawing.Point(48, 11);
            this.txtExcelName.Name = "txtExcelName";
            this.txtExcelName.Size = new System.Drawing.Size(402, 24);
            this.txtExcelName.TabIndex = 43;
            this.txtExcelName.Visible = false;
            // 
            // dgvAllocationBase
            // 
            this.dgvAllocationBase.AllowUserToAddRows = false;
            this.dgvAllocationBase.AllowUserToDeleteRows = false;
            this.dgvAllocationBase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAllocationBase.BackgroundColor = System.Drawing.Color.White;
            this.dgvAllocationBase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllocationBase.Location = new System.Drawing.Point(10, 12);
            this.dgvAllocationBase.Name = "dgvAllocationBase";
            this.dgvAllocationBase.Size = new System.Drawing.Size(973, 428);
            this.dgvAllocationBase.TabIndex = 49;
            this.dgvAllocationBase.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.NumberCellEndEdit);
            this.dgvAllocationBase.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.AutoEdit);
            this.dgvAllocationBase.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.MergeFormatting);
            this.dgvAllocationBase.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.CellPainting);
            this.dgvAllocationBase.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.EditingControlShowing);
            this.dgvAllocationBase.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRowPostPaint);
            // 
            // tabStoreSelection
            // 
            this.tabStoreSelection.BackColor = System.Drawing.Color.Transparent;
            this.tabStoreSelection.Controls.Add(this.splitContainer2);
            this.tabStoreSelection.HorizontalScrollbarBarColor = true;
            this.tabStoreSelection.HorizontalScrollbarHighlightOnWheel = false;
            this.tabStoreSelection.HorizontalScrollbarSize = 10;
            this.tabStoreSelection.Location = new System.Drawing.Point(4, 38);
            this.tabStoreSelection.Name = "tabStoreSelection";
            this.tabStoreSelection.Size = new System.Drawing.Size(992, 482);
            this.tabStoreSelection.TabIndex = 3;
            this.tabStoreSelection.Text = "Branch Selection";
            this.tabStoreSelection.VerticalScrollbarBarColor = true;
            this.tabStoreSelection.VerticalScrollbarHighlightOnWheel = false;
            this.tabStoreSelection.VerticalScrollbarSize = 10;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel5);
            this.splitContainer2.Size = new System.Drawing.Size(992, 482);
            this.splitContainer2.SplitterDistance = 167;
            this.splitContainer2.TabIndex = 3;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.groupBox5, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.groupBox4, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(992, 167);
            this.tableLayoutPanel4.TabIndex = 52;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.dgvStoreCriteria);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(490, 161);
            this.groupBox5.TabIndex = 50;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Store Criteria";
            // 
            // dgvStoreCriteria
            // 
            this.dgvStoreCriteria.AllowUserToAddRows = false;
            this.dgvStoreCriteria.AllowUserToDeleteRows = false;
            this.dgvStoreCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStoreCriteria.BackgroundColor = System.Drawing.Color.White;
            this.dgvStoreCriteria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStoreCriteria.Location = new System.Drawing.Point(6, 19);
            this.dgvStoreCriteria.Name = "dgvStoreCriteria";
            this.dgvStoreCriteria.Size = new System.Drawing.Size(478, 136);
            this.dgvStoreCriteria.TabIndex = 1;
            this.dgvStoreCriteria.Tag = "SC";
            this.dgvStoreCriteria.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvButtonCellContentClick);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.btnCustNewParam);
            this.groupBox4.Controls.Add(this.dgvCustOtherParam);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(499, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(490, 161);
            this.groupBox4.TabIndex = 51;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Other Parameter";
            // 
            // btnCustNewParam
            // 
            this.btnCustNewParam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCustNewParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustNewParam.Location = new System.Drawing.Point(6, 134);
            this.btnCustNewParam.Name = "btnCustNewParam";
            this.btnCustNewParam.Size = new System.Drawing.Size(478, 22);
            this.btnCustNewParam.TabIndex = 2;
            this.btnCustNewParam.Tag = "COP";
            this.btnCustNewParam.Text = "Define new";
            this.btnCustNewParam.UseVisualStyleBackColor = true;
            this.btnCustNewParam.Click += new System.EventHandler(this.btnCustNewParam_Click);
            // 
            // dgvCustOtherParam
            // 
            this.dgvCustOtherParam.AllowUserToAddRows = false;
            this.dgvCustOtherParam.AllowUserToDeleteRows = false;
            this.dgvCustOtherParam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCustOtherParam.BackgroundColor = System.Drawing.Color.White;
            this.dgvCustOtherParam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustOtherParam.Location = new System.Drawing.Point(6, 19);
            this.dgvCustOtherParam.Name = "dgvCustOtherParam";
            this.dgvCustOtherParam.ReadOnly = true;
            this.dgvCustOtherParam.Size = new System.Drawing.Size(478, 116);
            this.dgvCustOtherParam.TabIndex = 1;
            this.dgvCustOtherParam.Tag = "COP";
            this.dgvCustOtherParam.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustOtherParam_CellContentClick);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.36842F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.36842F));
            this.tableLayoutPanel5.Controls.Add(this.dgvCustSelection, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.dgvCustSelected, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(992, 311);
            this.tableLayoutPanel5.TabIndex = 55;
            // 
            // dgvCustSelection
            // 
            this.dgvCustSelection.AllowUserToAddRows = false;
            this.dgvCustSelection.AllowUserToDeleteRows = false;
            this.dgvCustSelection.BackgroundColor = System.Drawing.Color.White;
            this.dgvCustSelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCustSelection.Location = new System.Drawing.Point(3, 3);
            this.dgvCustSelection.Name = "dgvCustSelection";
            this.dgvCustSelection.ReadOnly = true;
            this.dgvCustSelection.Size = new System.Drawing.Size(463, 305);
            this.dgvCustSelection.TabIndex = 49;
            this.dgvCustSelection.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRowPostPaint);
            // 
            // dgvCustSelected
            // 
            this.dgvCustSelected.AllowUserToAddRows = false;
            this.dgvCustSelected.AllowUserToDeleteRows = false;
            this.dgvCustSelected.BackgroundColor = System.Drawing.Color.White;
            this.dgvCustSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCustSelected.Location = new System.Drawing.Point(524, 3);
            this.dgvCustSelected.Name = "dgvCustSelected";
            this.dgvCustSelected.ReadOnly = true;
            this.dgvCustSelected.Size = new System.Drawing.Size(465, 305);
            this.dgvCustSelected.TabIndex = 50;
            this.dgvCustSelected.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRowPostPaint);
            this.dgvCustSelected.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgvAddRow);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.navCustGet, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.navCustBack, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.navCustBackAll, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.navCustGetAll, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(472, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 4;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(46, 305);
            this.tableLayoutPanel6.TabIndex = 51;
            // 
            // navCustGet
            // 
            this.navCustGet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.navCustGet.Location = new System.Drawing.Point(6, 23);
            this.navCustGet.Margin = new System.Windows.Forms.Padding(5);
            this.navCustGet.Name = "navCustGet";
            this.navCustGet.Size = new System.Drawing.Size(34, 29);
            this.navCustGet.TabIndex = 54;
            this.navCustGet.Text = ">";
            this.navCustGet.UseSelectable = true;
            this.navCustGet.Click += new System.EventHandler(this.navClick);
            // 
            // navCustBack
            // 
            this.navCustBack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.navCustBack.Location = new System.Drawing.Point(6, 252);
            this.navCustBack.Margin = new System.Windows.Forms.Padding(5);
            this.navCustBack.Name = "navCustBack";
            this.navCustBack.Size = new System.Drawing.Size(34, 29);
            this.navCustBack.TabIndex = 51;
            this.navCustBack.Text = "<";
            this.navCustBack.UseSelectable = true;
            this.navCustBack.Click += new System.EventHandler(this.navClick);
            // 
            // navCustBackAll
            // 
            this.navCustBackAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.navCustBackAll.Location = new System.Drawing.Point(6, 175);
            this.navCustBackAll.Margin = new System.Windows.Forms.Padding(5);
            this.navCustBackAll.Name = "navCustBackAll";
            this.navCustBackAll.Size = new System.Drawing.Size(34, 29);
            this.navCustBackAll.TabIndex = 52;
            this.navCustBackAll.Text = "<<";
            this.navCustBackAll.UseSelectable = true;
            this.navCustBackAll.Click += new System.EventHandler(this.navClick);
            // 
            // navCustGetAll
            // 
            this.navCustGetAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.navCustGetAll.Location = new System.Drawing.Point(6, 99);
            this.navCustGetAll.Margin = new System.Windows.Forms.Padding(5);
            this.navCustGetAll.Name = "navCustGetAll";
            this.navCustGetAll.Size = new System.Drawing.Size(34, 29);
            this.navCustGetAll.TabIndex = 53;
            this.navCustGetAll.Text = ">>";
            this.navCustGetAll.UseSelectable = true;
            this.navCustGetAll.Click += new System.EventHandler(this.navClick);
            // 
            // tabRanking
            // 
            this.tabRanking.BackColor = System.Drawing.Color.Transparent;
            this.tabRanking.Controls.Add(this.splitContainer3);
            this.tabRanking.HorizontalScrollbarBarColor = true;
            this.tabRanking.HorizontalScrollbarHighlightOnWheel = false;
            this.tabRanking.HorizontalScrollbarSize = 10;
            this.tabRanking.Location = new System.Drawing.Point(4, 38);
            this.tabRanking.Name = "tabRanking";
            this.tabRanking.Size = new System.Drawing.Size(992, 482);
            this.tabRanking.TabIndex = 4;
            this.tabRanking.Text = "Ranking";
            this.tabRanking.VerticalScrollbarBarColor = true;
            this.tabRanking.VerticalScrollbarHighlightOnWheel = false;
            this.tabRanking.VerticalScrollbarSize = 10;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tableLayoutPanel7);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.dgvAllocation);
            this.splitContainer3.Size = new System.Drawing.Size(992, 482);
            this.splitContainer3.SplitterDistance = 167;
            this.splitContainer3.TabIndex = 3;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 5;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.Controls.Add(this.groupBox7, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.groupBox6, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.grpSalesHorizon, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.grpValueBased, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.groupBox8, 4, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(992, 167);
            this.tableLayoutPanel7.TabIndex = 76;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.rbAverage);
            this.groupBox7.Controls.Add(this.rbTotalSales);
            this.groupBox7.Controls.Add(this.cbAverage);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox7.Location = new System.Drawing.Point(597, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(192, 161);
            this.groupBox7.TabIndex = 77;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Total Sales Based On";
            // 
            // rbAverage
            // 
            this.rbAverage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.rbAverage.AutoSize = true;
            this.rbAverage.BackColor = System.Drawing.Color.Transparent;
            this.rbAverage.Location = new System.Drawing.Point(11, 49);
            this.rbAverage.Name = "rbAverage";
            this.rbAverage.Size = new System.Drawing.Size(65, 17);
            this.rbAverage.TabIndex = 72;
            this.rbAverage.Text = "Average";
            this.rbAverage.UseVisualStyleBackColor = false;
            this.rbAverage.CheckedChanged += new System.EventHandler(this.rbAverage_CheckedChanged);
            // 
            // rbTotalSales
            // 
            this.rbTotalSales.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.rbTotalSales.AutoSize = true;
            this.rbTotalSales.BackColor = System.Drawing.Color.Transparent;
            this.rbTotalSales.Checked = true;
            this.rbTotalSales.Location = new System.Drawing.Point(11, 26);
            this.rbTotalSales.Name = "rbTotalSales";
            this.rbTotalSales.Size = new System.Drawing.Size(78, 17);
            this.rbTotalSales.TabIndex = 73;
            this.rbTotalSales.TabStop = true;
            this.rbTotalSales.Text = "Total Sales";
            this.rbTotalSales.UseVisualStyleBackColor = false;
            // 
            // cbAverage
            // 
            this.cbAverage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cbAverage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAverage.Enabled = false;
            this.cbAverage.FormattingEnabled = true;
            this.cbAverage.Items.AddRange(new object[] {
            "Daily",
            "Weekly",
            "Monthly"});
            this.cbAverage.Location = new System.Drawing.Point(30, 72);
            this.cbAverage.Name = "cbAverage";
            this.cbAverage.Size = new System.Drawing.Size(129, 21);
            this.cbAverage.TabIndex = 74;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.Transparent;
            this.groupBox6.Controls.Add(this.dgvLevels);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(192, 161);
            this.groupBox6.TabIndex = 55;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Levels";
            // 
            // dgvLevels
            // 
            this.dgvLevels.AllowUserToAddRows = false;
            this.dgvLevels.AllowUserToDeleteRows = false;
            this.dgvLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLevels.BackgroundColor = System.Drawing.Color.White;
            this.dgvLevels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLevels.Location = new System.Drawing.Point(6, 19);
            this.dgvLevels.Name = "dgvLevels";
            this.dgvLevels.Size = new System.Drawing.Size(180, 136);
            this.dgvLevels.TabIndex = 1;
            this.dgvLevels.Tag = "LVL";
            this.dgvLevels.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLevels_CellEndEdit);
            this.dgvLevels.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRowPostPaint);
            // 
            // grpSalesHorizon
            // 
            this.grpSalesHorizon.BackColor = System.Drawing.Color.Transparent;
            this.grpSalesHorizon.Controls.Add(this.dtpDateTo);
            this.grpSalesHorizon.Controls.Add(this.label6);
            this.grpSalesHorizon.Controls.Add(this.dtpDateFrom);
            this.grpSalesHorizon.Controls.Add(this.label5);
            this.grpSalesHorizon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSalesHorizon.Location = new System.Drawing.Point(201, 3);
            this.grpSalesHorizon.Name = "grpSalesHorizon";
            this.grpSalesHorizon.Size = new System.Drawing.Size(192, 161);
            this.grpSalesHorizon.TabIndex = 54;
            this.grpSalesHorizon.TabStop = false;
            this.grpSalesHorizon.Text = "Sales Horizon";
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpDateTo.CustomFormat = "MM.dd.yyyy";
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTo.Location = new System.Drawing.Point(9, 79);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(163, 20);
            this.dtpDateTo.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Date to :";
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpDateFrom.CustomFormat = "MM.dd.yyyy";
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFrom.Location = new System.Drawing.Point(9, 37);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(163, 20);
            this.dtpDateFrom.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Date from :";
            // 
            // grpValueBased
            // 
            this.grpValueBased.BackColor = System.Drawing.Color.Transparent;
            this.grpValueBased.Controls.Add(this.rbQuantity);
            this.grpValueBased.Controls.Add(this.rbAmount);
            this.grpValueBased.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpValueBased.Location = new System.Drawing.Point(399, 3);
            this.grpValueBased.Name = "grpValueBased";
            this.grpValueBased.Size = new System.Drawing.Size(192, 161);
            this.grpValueBased.TabIndex = 56;
            this.grpValueBased.TabStop = false;
            this.grpValueBased.Text = "Value Based on";
            // 
            // rbQuantity
            // 
            this.rbQuantity.AutoSize = true;
            this.rbQuantity.BackColor = System.Drawing.Color.Transparent;
            this.rbQuantity.Location = new System.Drawing.Point(20, 49);
            this.rbQuantity.Name = "rbQuantity";
            this.rbQuantity.Size = new System.Drawing.Size(64, 17);
            this.rbQuantity.TabIndex = 9;
            this.rbQuantity.Tag = "";
            this.rbQuantity.Text = "Quantity";
            this.rbQuantity.UseVisualStyleBackColor = false;
            // 
            // rbAmount
            // 
            this.rbAmount.AutoSize = true;
            this.rbAmount.BackColor = System.Drawing.Color.Transparent;
            this.rbAmount.Checked = true;
            this.rbAmount.Location = new System.Drawing.Point(20, 26);
            this.rbAmount.Name = "rbAmount";
            this.rbAmount.Size = new System.Drawing.Size(61, 17);
            this.rbAmount.TabIndex = 8;
            this.rbAmount.TabStop = true;
            this.rbAmount.Tag = "";
            this.rbAmount.Text = "Amount";
            this.rbAmount.UseVisualStyleBackColor = false;
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.Transparent;
            this.groupBox8.Controls.Add(this.dgvSalesCritera);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(795, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(194, 161);
            this.groupBox8.TabIndex = 56;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Sales Criteria";
            // 
            // dgvSalesCritera
            // 
            this.dgvSalesCritera.AllowUserToAddRows = false;
            this.dgvSalesCritera.AllowUserToDeleteRows = false;
            this.dgvSalesCritera.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSalesCritera.BackgroundColor = System.Drawing.Color.White;
            this.dgvSalesCritera.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSalesCritera.Location = new System.Drawing.Point(6, 19);
            this.dgvSalesCritera.Name = "dgvSalesCritera";
            this.dgvSalesCritera.Size = new System.Drawing.Size(182, 136);
            this.dgvSalesCritera.TabIndex = 1;
            this.dgvSalesCritera.Tag = "ISC";
            this.dgvSalesCritera.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvSalesCritera_CellBeginEdit);
            this.dgvSalesCritera.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.rowChangedValue);
            // 
            // dgvAllocation
            // 
            this.dgvAllocation.AllowUserToAddRows = false;
            this.dgvAllocation.AllowUserToDeleteRows = false;
            this.dgvAllocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAllocation.BackgroundColor = System.Drawing.Color.White;
            this.dgvAllocation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllocation.Location = new System.Drawing.Point(10, 12);
            this.dgvAllocation.Name = "dgvAllocation";
            this.dgvAllocation.Size = new System.Drawing.Size(967, 286);
            this.dgvAllocation.TabIndex = 49;
            this.dgvAllocation.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.NumberCellEndEdit);
            this.dgvAllocation.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.AutoEdit);
            this.dgvAllocation.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.MergeFormatting);
            this.dgvAllocation.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.CellPainting);
            this.dgvAllocation.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvColorFormat);
            this.dgvAllocation.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.EditingControlShowing);
            // 
            // tabSummary
            // 
            this.tabSummary.BackColor = System.Drawing.Color.Transparent;
            this.tabSummary.Controls.Add(this.dgvParameter);
            this.tabSummary.Controls.Add(this.label13);
            this.tabSummary.Controls.Add(this.txtRemarks);
            this.tabSummary.Controls.Add(this.dtpTaxDate);
            this.tabSummary.Controls.Add(this.dtpDueDate);
            this.tabSummary.Controls.Add(this.dtpDocDate);
            this.tabSummary.Controls.Add(this.dgvSummary);
            this.tabSummary.Controls.Add(this.label12);
            this.tabSummary.Controls.Add(this.label11);
            this.tabSummary.Controls.Add(this.label10);
            this.tabSummary.HorizontalScrollbarBarColor = true;
            this.tabSummary.HorizontalScrollbarHighlightOnWheel = false;
            this.tabSummary.HorizontalScrollbarSize = 10;
            this.tabSummary.Location = new System.Drawing.Point(4, 38);
            this.tabSummary.Name = "tabSummary";
            this.tabSummary.Size = new System.Drawing.Size(992, 482);
            this.tabSummary.TabIndex = 6;
            this.tabSummary.Text = "Summary";
            this.tabSummary.VerticalScrollbarBarColor = true;
            this.tabSummary.VerticalScrollbarHighlightOnWheel = false;
            this.tabSummary.VerticalScrollbarSize = 10;
            // 
            // dgvParameter
            // 
            this.dgvParameter.AllowUserToAddRows = false;
            this.dgvParameter.AllowUserToDeleteRows = false;
            this.dgvParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvParameter.BackgroundColor = System.Drawing.Color.White;
            this.dgvParameter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParameter.Location = new System.Drawing.Point(275, 8);
            this.dgvParameter.Name = "dgvParameter";
            this.dgvParameter.Size = new System.Drawing.Size(266, 72);
            this.dgvParameter.TabIndex = 62;
            this.dgvParameter.Tag = "SMRY";
            this.dgvParameter.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCellContentClick);
            this.dgvParameter.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.AutoEdit);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(662, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 13);
            this.label13.TabIndex = 61;
            this.label13.Text = "Remarks :";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemarks.Location = new System.Drawing.Point(723, 8);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(263, 72);
            this.txtRemarks.TabIndex = 60;
            // 
            // dtpTaxDate
            // 
            this.dtpTaxDate.CustomFormat = "MM.dd.yyyy";
            this.dtpTaxDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTaxDate.Location = new System.Drawing.Point(107, 60);
            this.dtpTaxDate.Name = "dtpTaxDate";
            this.dtpTaxDate.Size = new System.Drawing.Size(139, 20);
            this.dtpTaxDate.TabIndex = 59;
            // 
            // dtpDueDate
            // 
            this.dtpDueDate.CustomFormat = "MM.dd.yyyy";
            this.dtpDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDueDate.Location = new System.Drawing.Point(107, 34);
            this.dtpDueDate.Name = "dtpDueDate";
            this.dtpDueDate.Size = new System.Drawing.Size(139, 20);
            this.dtpDueDate.TabIndex = 59;
            // 
            // dtpDocDate
            // 
            this.dtpDocDate.CustomFormat = "MM.dd.yyyy";
            this.dtpDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDocDate.Location = new System.Drawing.Point(107, 8);
            this.dtpDocDate.Name = "dtpDocDate";
            this.dtpDocDate.Size = new System.Drawing.Size(139, 20);
            this.dtpDocDate.TabIndex = 59;
            // 
            // dgvSummary
            // 
            this.dgvSummary.AllowUserToAddRows = false;
            this.dgvSummary.AllowUserToDeleteRows = false;
            this.dgvSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSummary.BackgroundColor = System.Drawing.Color.White;
            this.dgvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSummary.Location = new System.Drawing.Point(6, 91);
            this.dgvSummary.Name = "dgvSummary";
            this.dgvSummary.ReadOnly = true;
            this.dgvSummary.Size = new System.Drawing.Size(981, 388);
            this.dgvSummary.TabIndex = 58;
            this.dgvSummary.Tag = "Allocated";
            this.dgvSummary.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.AutoEdit);
            this.dgvSummary.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.MergeFormatting);
            this.dgvSummary.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvCellPainting);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 13);
            this.label12.TabIndex = 57;
            this.label12.Text = "Document Date :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 13);
            this.label11.TabIndex = 56;
            this.label11.Text = "Due Date :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 55;
            this.label10.Text = "Posting Date :";
            // 
            // frmAllocationWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(1040, 640);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmAllocationWizard";
            this.Text = "Allocation Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAllocationWizard_FormClosing);
            this.Load += new System.EventHandler(this.frmAllocationWizard_Load);
            this.Resize += new System.EventHandler(this.frmAllocationWizard_Resize);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.MetroTabControl.ResumeLayout(false);
            this.tabWelcome.ResumeLayout(false);
            this.tabWelcome.PerformLayout();
            this.tabScenario.ResumeLayout(false);
            this.tabScenario.PerformLayout();
            this.tabAllocWizRuns.ResumeLayout(false);
            this.tabWIzardRuns.ResumeLayout(false);
            this.tabApprovalRun.ResumeLayout(false);
            this.tabApprovalRun.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllocWizRuns)).EndInit();
            this.tabAllocationRun.ResumeLayout(false);
            this.tabAllocationRun.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApprovedRuns)).EndInit();
            this.tabItemSelection.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWhs)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemOtherParam)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarketingDocs)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemSelection)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabAllocationBase.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllocationBase)).EndInit();
            this.tabStoreSelection.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStoreCriteria)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustOtherParam)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustSelected)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tabRanking.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLevels)).EndInit();
            this.grpSalesHorizon.ResumeLayout(false);
            this.grpSalesHorizon.PerformLayout();
            this.grpValueBased.ResumeLayout(false);
            this.grpValueBased.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalesCritera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllocation)).EndInit();
            this.tabSummary.ResumeLayout(false);
            this.tabSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        public MetroFramework.Controls.MetroTextBox.MetroTextButton btnGenerate;
        public MetroFramework.Controls.MetroButton btnCancel;
        public MetroFramework.Controls.MetroTextBox.MetroTextButton btnFinish;
        public MetroFramework.Controls.MetroButton btnPrev;
        public MetroFramework.Controls.MetroTextBox.MetroTextButton btnNext;
        private Panel panel2;
        public MetroFramework.Controls.MetroTabControl MetroTabControl;
        public MetroFramework.Controls.MetroTabPage tabWelcome;
        public Label label2;
        public Label label1;
        public MetroFramework.Controls.MetroTabPage tabItemSelection;
        private SplitContainer splitContainer1;
        public GroupBox groupBox3;
        public DataGridView dgvItemOtherParam;
        public GroupBox groupBox2;
        public DataGridView dgvWhs;
        public GroupBox groupBox1;
        public DataGridView dgvMarketingDocs;
        public MetroFramework.Controls.MetroTabPage tabScenario;
        public Label label3;
        public Label label4;
        public MetroFramework.Controls.MetroTabPage tabStoreSelection;
        public MetroFramework.Controls.MetroTabPage tabRanking;
        private Button btnItemNewParam;
        private SplitContainer splitContainer2;
        public GroupBox groupBox4;
        public DataGridView dgvCustOtherParam;
        public GroupBox groupBox5;
        public DataGridView dgvStoreCriteria;
        public MetroFramework.Controls.MetroButton navCustGet;
        public MetroFramework.Controls.MetroButton navCustGetAll;
        public MetroFramework.Controls.MetroButton navCustBackAll;
        public MetroFramework.Controls.MetroButton navCustBack;
        public DataGridView dgvCustSelected;
        public DataGridView dgvCustSelection;
        private SplitContainer splitContainer3;
        public DataGridView dgvAllocation;
        public GroupBox groupBox6;
        public DataGridView dgvLevels;
        public GroupBox grpSalesHorizon;
        private Label label6;
        private Label label5;
        public GroupBox grpValueBased;
        public DataGridView dgvSalesCritera;
        public DateTimePicker dtpDateTo;
        public DateTimePicker dtpDateFrom;
        public RadioButton rbQuantity;
        public RadioButton rbAmount;
        public RadioButton rbRepeatOrder;
        public MetroFramework.Controls.MetroTabPage tabSummary;
        public DataGridView dgvSummary;
        private Label label12;
        private Label label11;
        private Label label10;
        public DateTimePicker dtpTaxDate;
        public DateTimePicker dtpDueDate;
        public DateTimePicker dtpDocDate;
        private Label label13;
        public DataGridView dgvParameter;
        public TextBox txtRemarks;
        public RadioButton rbCreateNewAlloc;
        public MetroFramework.Controls.MetroTabPage tabAllocationBase;
        private SplitContainer splitContainer4;
        public DataGridView dataGridView2;
        public DataGridView dataGridView1;
        private ComboBox cmbWorkSheet;
        private Label label15;
        private Label label14;
        private Button btnOpenFile;
        private TextBox txtExcelName;
        public DataGridView dgvAllocationBase;
        public GroupBox groupBox8;
        public RadioButton rbTotalSales;
        public RadioButton rbAverage;
        public ComboBox cbAverage;
        public Button btnCustNewParam;
        private MetroFramework.Controls.MetroTabPage tabAllocWizRuns;
        public RadioButton rbAllocationApproval;
        public DataGridView dgvAllocWizRuns;
        private TableLayoutPanel tableLayoutPanel1;
        public DataGridView dgvItemSelected;
        public DataGridView dgvItemSelection;
        private TableLayoutPanel tableLayoutPanel2;
        public MetroFramework.Controls.MetroButton navItemBack;
        public MetroFramework.Controls.MetroButton navItemGet;
        public MetroFramework.Controls.MetroButton navItemGetAll;
        public MetroFramework.Controls.MetroButton navItemBackAll;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private TableLayoutPanel tableLayoutPanel7;
        private GroupBox groupBox7;
        private TabPage tabApprovalRun;
        private TabPage tabAllocationRun;
        public DataGridView dgvApprovedRuns;
        private Label label7;
        public TextBox txtSearchApprovalRuns;
        private Label label8;
        public TextBox txtSearchApprovedRuns;
        private Label label9;
        public DateTimePicker dtpApprovedTo;
        public DateTimePicker dtpApprovedFrom;
        public TabControl tabWIzardRuns;
        private Label label16;
        public MetroFramework.Controls.MetroButton btnAWRFilter;
    }
}
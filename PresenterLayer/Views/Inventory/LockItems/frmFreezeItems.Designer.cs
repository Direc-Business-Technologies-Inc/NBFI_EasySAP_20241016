namespace PresenterLayer
{
    partial class frmFreezeItems
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbl = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtDocDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDocStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.txtDocEntry = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnAddItems = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtItemSearch = new System.Windows.Forms.TextBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnAddWhs = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearchWhs = new System.Windows.Forms.TextBox();
            this.dgvWhs = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.msItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msWhs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.msCancel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWhs)).BeginInit();
            this.msItems.SuspendLayout();
            this.msWhs.SuspendLayout();
            this.msCancel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.lbl.Location = new System.Drawing.Point(8, 40);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(52, 13);
            this.lbl.TabIndex = 127;
            this.lbl.Text = "End Date";
            // 
            // dtTo
            // 
            this.dtTo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.dtTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(95, 38);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(160, 18);
            this.dtTo.TabIndex = 128;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label7.Location = new System.Drawing.Point(8, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 129;
            this.label7.Text = "Start Date";
            // 
            // dtFrom
            // 
            this.dtFrom.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(95, 14);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(160, 18);
            this.dtFrom.TabIndex = 130;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtDocDate);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtDocStatus);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.txtRemarks);
            this.panel1.Controls.Add(this.txtDocEntry);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.lbl);
            this.panel1.Controls.Add(this.dtFrom);
            this.panel1.Controls.Add(this.dtTo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 437);
            this.panel1.TabIndex = 131;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // txtDocDate
            // 
            this.txtDocDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtDocDate.Location = new System.Drawing.Point(591, 62);
            this.txtDocDate.Name = "txtDocDate";
            this.txtDocDate.ReadOnly = true;
            this.txtDocDate.Size = new System.Drawing.Size(113, 18);
            this.txtDocDate.TabIndex = 318;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label5.Location = new System.Drawing.Point(501, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 317;
            this.label5.Text = "Document Date";
            // 
            // txtDocStatus
            // 
            this.txtDocStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtDocStatus.Location = new System.Drawing.Point(591, 38);
            this.txtDocStatus.Name = "txtDocStatus";
            this.txtDocStatus.ReadOnly = true;
            this.txtDocStatus.Size = new System.Drawing.Size(113, 18);
            this.txtDocStatus.TabIndex = 316;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label4.Location = new System.Drawing.Point(501, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 315;
            this.label4.Text = "Document Status";
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNew.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(17, 406);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(74, 21);
            this.btnNew.TabIndex = 314;
            this.btnNew.Text = "New";
            this.btnNew.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFind.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnFind.ForeColor = System.Drawing.Color.White;
            this.btnFind.Location = new System.Drawing.Point(177, 406);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(74, 21);
            this.btnFind.TabIndex = 313;
            this.btnFind.Text = "Find";
            this.btnFind.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.BackColor = System.Drawing.Color.RoyalBlue;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(411, 42);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(84, 21);
            this.button5.TabIndex = 312;
            this.button5.Text = "Unlock Items";
            this.button5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.BackColor = System.Drawing.Color.RoyalBlue;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(411, 18);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(84, 21);
            this.button4.TabIndex = 311;
            this.button4.Text = "Lock Items";
            this.button4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label11.Location = new System.Drawing.Point(450, 379);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 309;
            this.label11.Text = "Remarks";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemarks.Location = new System.Drawing.Point(503, 376);
            this.txtRemarks.MaxLength = 254;
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(201, 51);
            this.txtRemarks.TabIndex = 310;
            // 
            // txtDocEntry
            // 
            this.txtDocEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtDocEntry.Location = new System.Drawing.Point(591, 15);
            this.txtDocEntry.Name = "txtDocEntry";
            this.txtDocEntry.ReadOnly = true;
            this.txtDocEntry.Size = new System.Drawing.Size(113, 18);
            this.txtDocEntry.TabIndex = 308;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label3.Location = new System.Drawing.Point(501, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 307;
            this.label3.Text = "Document No.";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(97, 406);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(74, 21);
            this.btnAdd.TabIndex = 306;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(257, 406);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 21);
            this.btnCancel.TabIndex = 305;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 90);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(705, 285);
            this.tabControl1.TabIndex = 131;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnAddItems);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtItemSearch);
            this.tabPage1.Controls.Add(this.dgvItems);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(697, 259);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Items";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnAddItems
            // 
            this.btnAddItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItems.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnAddItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnAddItems.ForeColor = System.Drawing.Color.White;
            this.btnAddItems.Location = new System.Drawing.Point(581, 3);
            this.btnAddItems.Name = "btnAddItems";
            this.btnAddItems.Size = new System.Drawing.Size(113, 21);
            this.btnAddItems.TabIndex = 307;
            this.btnAddItems.Text = "Add Items +";
            this.btnAddItems.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddItems.UseVisualStyleBackColor = false;
            this.btnAddItems.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 307;
            this.label1.Text = "Search";
            // 
            // txtItemSearch
            // 
            this.txtItemSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtItemSearch.Location = new System.Drawing.Point(52, 5);
            this.txtItemSearch.Name = "txtItemSearch";
            this.txtItemSearch.Size = new System.Drawing.Size(523, 18);
            this.txtItemSearch.TabIndex = 315;
            this.txtItemSearch.TextChanged += new System.EventHandler(this.txtItemSearch_TextChanged);
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Location = new System.Drawing.Point(3, 28);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.Size = new System.Drawing.Size(691, 227);
            this.dgvItems.TabIndex = 0;
            this.dgvItems.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItems_ColumnHeaderMouseClick);
            this.dgvItems.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItems_RowHeaderMouseClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnAddWhs);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtSearchWhs);
            this.tabPage2.Controls.Add(this.dgvWhs);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(696, 258);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Warehouse";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnAddWhs
            // 
            this.btnAddWhs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddWhs.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnAddWhs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddWhs.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnAddWhs.ForeColor = System.Drawing.Color.White;
            this.btnAddWhs.Location = new System.Drawing.Point(584, 3);
            this.btnAddWhs.Name = "btnAddWhs";
            this.btnAddWhs.Size = new System.Drawing.Size(113, 21);
            this.btnAddWhs.TabIndex = 316;
            this.btnAddWhs.Text = "Add Warehouse +";
            this.btnAddWhs.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddWhs.UseVisualStyleBackColor = false;
            this.btnAddWhs.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label2.Location = new System.Drawing.Point(6, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 317;
            this.label2.Text = "Search";
            // 
            // txtSearchWhs
            // 
            this.txtSearchWhs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchWhs.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtSearchWhs.Location = new System.Drawing.Point(52, 5);
            this.txtSearchWhs.Name = "txtSearchWhs";
            this.txtSearchWhs.Size = new System.Drawing.Size(525, 18);
            this.txtSearchWhs.TabIndex = 318;
            // 
            // dgvWhs
            // 
            this.dgvWhs.AllowUserToAddRows = false;
            this.dgvWhs.AllowUserToDeleteRows = false;
            this.dgvWhs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWhs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWhs.Location = new System.Drawing.Point(3, 28);
            this.dgvWhs.Name = "dgvWhs";
            this.dgvWhs.ReadOnly = true;
            this.dgvWhs.Size = new System.Drawing.Size(693, 214);
            this.dgvWhs.TabIndex = 1;
            this.dgvWhs.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvWhs_RowHeaderMouseClick);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.panel2.Location = new System.Drawing.Point(10, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(737, 1);
            this.panel2.TabIndex = 132;
            // 
            // msItems
            // 
            this.msItems.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteItemsToolStripMenuItem});
            this.msItems.Name = "msItems";
            this.msItems.Size = new System.Drawing.Size(201, 30);
            // 
            // deleteItemsToolStripMenuItem
            // 
            this.deleteItemsToolStripMenuItem.Image = global::PresenterLayer.Properties.Resources.close;
            this.deleteItemsToolStripMenuItem.Name = "deleteItemsToolStripMenuItem";
            this.deleteItemsToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.deleteItemsToolStripMenuItem.Text = "Remove Selected Items";
            this.deleteItemsToolStripMenuItem.Click += new System.EventHandler(this.deleteItemsToolStripMenuItem_Click);
            // 
            // msWhs
            // 
            this.msWhs.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msWhs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.msWhs.Name = "msItems";
            this.msWhs.Size = new System.Drawing.Size(201, 30);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::PresenterLayer.Properties.Resources.close;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(200, 26);
            this.toolStripMenuItem1.Text = "Remove Selected Items";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // msCancel
            // 
            this.msCancel.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msCancel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.msCancel.Name = "msItems";
            this.msCancel.Size = new System.Drawing.Size(115, 30);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = global::PresenterLayer.Properties.Resources.close;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(114, 26);
            this.toolStripMenuItem2.Text = "Cancel";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // frmFreezeItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 517);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmFreezeItems";
            this.Text = "Warehouse Lock";
            this.MaximumSizeChanged += new System.EventHandler(this.frmFreezeItems_MaximumSizeChanged);
            this.Load += new System.EventHandler(this.frmFreezeItems_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmFreezeItems_MouseClick);
            this.Resize += new System.EventHandler(this.frmFreezeItems_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWhs)).EndInit();
            this.msItems.ResumeLayout(false);
            this.msWhs.ResumeLayout(false);
            this.msCancel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridView dgvWhs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtItemSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAddItems;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDocEntry;
        private System.Windows.Forms.ContextMenuStrip msItems;
        private System.Windows.Forms.ToolStripMenuItem deleteItemsToolStripMenuItem;
        private System.Windows.Forms.Button btnAddWhs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearchWhs;
        private System.Windows.Forms.ContextMenuStrip msWhs;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.ContextMenuStrip msCancel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtDocStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDocDate;
        private System.Windows.Forms.Label label5;
    }
}
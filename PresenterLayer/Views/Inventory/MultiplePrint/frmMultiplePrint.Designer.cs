namespace PresenterLayer.Views
{
    partial class frmMultiplePrint
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrepBy = new System.Windows.Forms.TextBox();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPrint = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvDoc = new System.Windows.Forms.DataGridView();
            this.dgcLayoutPrinter = new System.Windows.Forms.DataGridView();
            this.tabPrinted = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbObj = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dgvPrinted = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnRefresh = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbBPList = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtObjectID = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDoc = new System.Windows.Forms.ComboBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPrint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgcLayoutPrinter)).BeginInit();
            this.tabPrinted.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrinted)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBPList)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(8, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(77, 16);
            this.lblTitle.TabIndex = 86;
            this.lblTitle.Text = "Print Layout";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Location = new System.Drawing.Point(-6, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1035, 1);
            this.panel1.TabIndex = 87;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label1.Location = new System.Drawing.Point(490, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 88;
            this.label1.Text = "Prepared by";
            // 
            // txtPrepBy
            // 
            this.txtPrepBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrepBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtPrepBy.Location = new System.Drawing.Point(571, 4);
            this.txtPrepBy.Name = "txtPrepBy";
            this.txtPrepBy.ReadOnly = true;
            this.txtPrepBy.Size = new System.Drawing.Size(143, 18);
            this.txtPrepBy.TabIndex = 89;
            // 
            // dtFrom
            // 
            this.dtFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtFrom.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(767, 4);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(111, 18);
            this.dtFrom.TabIndex = 162;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label10.Location = new System.Drawing.Point(720, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 161;
            this.label10.Text = "From";
            // 
            // dtTo
            // 
            this.dtTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtTo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(767, 28);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(111, 18);
            this.dtTo.TabIndex = 164;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label2.Location = new System.Drawing.Point(720, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 163;
            this.label2.Text = "To";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPrint);
            this.tabControl1.Controls.Add(this.tabPrinted);
            this.tabControl1.Location = new System.Drawing.Point(14, 137);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(879, 281);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPrint
            // 
            this.tabPrint.BackColor = System.Drawing.Color.White;
            this.tabPrint.Controls.Add(this.label7);
            this.tabPrint.Controls.Add(this.textBox5);
            this.tabPrint.Controls.Add(this.splitContainer1);
            this.tabPrint.Location = new System.Drawing.Point(4, 22);
            this.tabPrint.Name = "tabPrint";
            this.tabPrint.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrint.Size = new System.Drawing.Size(871, 255);
            this.tabPrint.TabIndex = 0;
            this.tabPrint.Text = "For print";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label7.Location = new System.Drawing.Point(6, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 172;
            this.label7.Text = "Find";
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.textBox5.Location = new System.Drawing.Point(39, 6);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(816, 18);
            this.textBox5.TabIndex = 83;
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(9, 30);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvDoc);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgcLayoutPrinter);
            this.splitContainer1.Size = new System.Drawing.Size(846, 219);
            this.splitContainer1.SplitterDistance = 498;
            this.splitContainer1.TabIndex = 172;
            // 
            // dgvDoc
            // 
            this.dgvDoc.AllowUserToAddRows = false;
            this.dgvDoc.AllowUserToDeleteRows = false;
            this.dgvDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDoc.BackgroundColor = System.Drawing.Color.Silver;
            this.dgvDoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDoc.Location = new System.Drawing.Point(3, 3);
            this.dgvDoc.Name = "dgvDoc";
            this.dgvDoc.Size = new System.Drawing.Size(492, 213);
            this.dgvDoc.TabIndex = 82;
            this.dgvDoc.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDoc_CellClick);
            this.dgvDoc.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDoc_ColumnHeaderMouseClick);
            // 
            // dgcLayoutPrinter
            // 
            this.dgcLayoutPrinter.AllowUserToAddRows = false;
            this.dgcLayoutPrinter.AllowUserToDeleteRows = false;
            this.dgcLayoutPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgcLayoutPrinter.BackgroundColor = System.Drawing.Color.Silver;
            this.dgcLayoutPrinter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgcLayoutPrinter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgcLayoutPrinter.Location = new System.Drawing.Point(3, 3);
            this.dgcLayoutPrinter.Name = "dgcLayoutPrinter";
            this.dgcLayoutPrinter.Size = new System.Drawing.Size(338, 213);
            this.dgcLayoutPrinter.TabIndex = 164;
            this.dgcLayoutPrinter.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgcLayoutPrinter_CellClick);
            this.dgcLayoutPrinter.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgcLayoutPrinter_CellContentClick);
            this.dgcLayoutPrinter.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgcLayoutPrinter_EditingControlShowing);
            // 
            // tabPrinted
            // 
            this.tabPrinted.Controls.Add(this.label8);
            this.tabPrinted.Controls.Add(this.label6);
            this.tabPrinted.Controls.Add(this.cbObj);
            this.tabPrinted.Controls.Add(this.textBox1);
            this.tabPrinted.Controls.Add(this.dgvPrinted);
            this.tabPrinted.Location = new System.Drawing.Point(4, 22);
            this.tabPrinted.Name = "tabPrinted";
            this.tabPrinted.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrinted.Size = new System.Drawing.Size(871, 255);
            this.tabPrinted.TabIndex = 1;
            this.tabPrinted.Text = "Printed";
            this.tabPrinted.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label8.Location = new System.Drawing.Point(498, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 13);
            this.label8.TabIndex = 173;
            this.label8.Text = "Find";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label6.Location = new System.Drawing.Point(5, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 13);
            this.label6.TabIndex = 172;
            this.label6.Text = "Obj";
            // 
            // cbObj
            // 
            this.cbObj.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.cbObj.FormattingEnabled = true;
            this.cbObj.Location = new System.Drawing.Point(34, 7);
            this.cbObj.Name = "cbObj";
            this.cbObj.Size = new System.Drawing.Size(386, 20);
            this.cbObj.TabIndex = 173;
            this.cbObj.SelectedIndexChanged += new System.EventHandler(this.cbObj_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.textBox1.Location = new System.Drawing.Point(531, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(333, 18);
            this.textBox1.TabIndex = 165;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // dgvPrinted
            // 
            this.dgvPrinted.AllowUserToAddRows = false;
            this.dgvPrinted.AllowUserToDeleteRows = false;
            this.dgvPrinted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPrinted.BackgroundColor = System.Drawing.Color.Silver;
            this.dgvPrinted.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPrinted.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrinted.Location = new System.Drawing.Point(6, 31);
            this.dgvPrinted.Name = "dgvPrinted";
            this.dgvPrinted.Size = new System.Drawing.Size(858, 319);
            this.dgvPrinted.TabIndex = 164;
            this.dgvPrinted.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPrinted_ColumnHeaderMouseClick);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(815, 52);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 21);
            this.button1.TabIndex = 163;
            this.button1.Text = "Search";
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.BtnRefresh);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.pbBPList);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.txtObjectID);
            this.panel2.Controls.Add(this.txtUser);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.cbDoc);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.dtTo);
            this.panel2.Controls.Add(this.txtPrepBy);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.dtFrom);
            this.panel2.Location = new System.Drawing.Point(11, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(895, 452);
            this.panel2.TabIndex = 166;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BtnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRefresh.Location = new System.Drawing.Point(3, 77);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(54, 23);
            this.BtnRefresh.TabIndex = 322;
            this.BtnRefresh.Text = "Refresh";
            this.BtnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pictureBox1.Location = new System.Drawing.Point(693, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(19, 15);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 321;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pbBPList
            // 
            this.pbBPList.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbBPList.Location = new System.Drawing.Point(438, 219);
            this.pbBPList.Name = "pbBPList";
            this.pbBPList.Size = new System.Drawing.Size(19, 15);
            this.pbBPList.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBPList.TabIndex = 320;
            this.pbBPList.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label5.Location = new System.Drawing.Point(294, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 171;
            this.label5.Text = "-";
            // 
            // txtObjectID
            // 
            this.txtObjectID.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtObjectID.Location = new System.Drawing.Point(310, 4);
            this.txtObjectID.Name = "txtObjectID";
            this.txtObjectID.ReadOnly = true;
            this.txtObjectID.Size = new System.Drawing.Size(115, 18);
            this.txtObjectID.TabIndex = 170;
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtUser.Location = new System.Drawing.Point(571, 28);
            this.txtUser.Name = "txtUser";
            this.txtUser.ReadOnly = true;
            this.txtUser.Size = new System.Drawing.Size(143, 18);
            this.txtUser.TabIndex = 168;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label4.Location = new System.Drawing.Point(490, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 167;
            this.label4.Text = "User";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label3.Location = new System.Drawing.Point(9, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 165;
            this.label3.Text = "No.";
            // 
            // cbDoc
            // 
            this.cbDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.cbDoc.FormattingEnabled = true;
            this.cbDoc.Location = new System.Drawing.Point(38, 4);
            this.cbDoc.Name = "cbDoc";
            this.cbDoc.Size = new System.Drawing.Size(250, 20);
            this.cbDoc.TabIndex = 166;
            this.cbDoc.SelectedIndexChanged += new System.EventHandler(this.cbDoc_SelectedIndexChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(826, 490);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(63, 21);
            this.btnPrint.TabIndex = 169;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // frmMultiplePrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 594);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel2);
            this.Name = "frmMultiplePrint";
            this.Load += new System.EventHandler(this.frmMultiplePrint_Load);
            this.Resize += new System.EventHandler(this.frmMultiplePrint_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPrint.ResumeLayout(false);
            this.tabPrint.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgcLayoutPrinter)).EndInit();
            this.tabPrinted.ResumeLayout(false);
            this.tabPrinted.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrinted)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBPList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPrint;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TabPage tabPrinted;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dgvPrinted;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbObj;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbBPList;
        public System.Windows.Forms.DataGridView dgvDoc;
        public System.Windows.Forms.DataGridView dgcLayoutPrinter;
        public System.Windows.Forms.TextBox txtPrepBy;
        public System.Windows.Forms.DateTimePicker dtFrom;
        public System.Windows.Forms.DateTimePicker dtTo;
        public System.Windows.Forms.ComboBox cbDoc;
        public System.Windows.Forms.TextBox txtUser;
        public System.Windows.Forms.TextBox txtObjectID;
        public System.Windows.Forms.Button BtnRefresh;
    }
}
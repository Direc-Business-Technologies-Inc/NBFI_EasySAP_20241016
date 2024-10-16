namespace PresenterLayer.Views
{
    partial class frmBarcodeAll
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.SearchDoc = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_IB = new System.Windows.Forms.CheckBox();
            this.cb_UPC = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.gvPrinter = new System.Windows.Forms.DataGridView();
            this.cbPrinter = new System.Windows.Forms.ComboBox();
            this.gvBarCode = new System.Windows.Forms.DataGridView();
            this.btnCommand = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.tmrPrint = new System.Windows.Forms.Timer(this.components);
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.IsPrinted = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPrinter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBarCode)).BeginInit();
            this.ContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.SearchDoc);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.gvBarCode);
            this.panel1.Controls.Add(this.btnCommand);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(6, 70);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1028, 383);
            this.panel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Location = new System.Drawing.Point(163, 334);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(146, 29);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SearchDoc
            // 
            this.SearchDoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchDoc.Location = new System.Drawing.Point(66, 17);
            this.SearchDoc.Name = "SearchDoc";
            this.SearchDoc.Size = new System.Drawing.Size(498, 25);
            this.SearchDoc.TabIndex = 1;
            this.SearchDoc.TextChanged += new System.EventHandler(this.SearchDoc_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cb_IB);
            this.groupBox1.Controls.Add(this.cb_UPC);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.gvPrinter);
            this.groupBox1.Controls.Add(this.cbPrinter);
            this.groupBox1.Location = new System.Drawing.Point(668, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 353);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Printer Properties";
            // 
            // cb_IB
            // 
            this.cb_IB.AutoSize = true;
            this.cb_IB.Location = new System.Drawing.Point(125, 75);
            this.cb_IB.Name = "cb_IB";
            this.cb_IB.Size = new System.Drawing.Size(134, 21);
            this.cb_IB.TabIndex = 30;
            this.cb_IB.Text = "Inverted Printing";
            this.cb_IB.UseVisualStyleBackColor = true;
            this.cb_IB.CheckedChanged += new System.EventHandler(this.cb_IB_CheckedChanged);
            // 
            // cb_UPC
            // 
            this.cb_UPC.AutoSize = true;
            this.cb_UPC.Location = new System.Drawing.Point(11, 75);
            this.cb_UPC.Name = "cb_UPC";
            this.cb_UPC.Size = new System.Drawing.Size(114, 21);
            this.cb_UPC.TabIndex = 29;
            this.cb_UPC.Text = "UPC Printing";
            this.cb_UPC.UseVisualStyleBackColor = true;
            this.cb_UPC.CheckedChanged += new System.EventHandler(this.cb_UPC_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 28;
            this.label3.Text = "Printers";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(265, 75);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(86, 23);
            this.btnAdd.TabIndex = 27;
            this.btnAdd.Text = "&Add Printer";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gvPrinter
            // 
            this.gvPrinter.AllowUserToAddRows = false;
            this.gvPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvPrinter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvPrinter.Location = new System.Drawing.Point(11, 104);
            this.gvPrinter.Name = "gvPrinter";
            this.gvPrinter.Size = new System.Drawing.Size(340, 243);
            this.gvPrinter.TabIndex = 26;
            this.gvPrinter.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvPrinter_CellContentClick);
            // 
            // cbPrinter
            // 
            this.cbPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrinter.FormattingEnabled = true;
            this.cbPrinter.Location = new System.Drawing.Point(11, 44);
            this.cbPrinter.Name = "cbPrinter";
            this.cbPrinter.Size = new System.Drawing.Size(340, 25);
            this.cbPrinter.TabIndex = 2;
            // 
            // gvBarCode
            // 
            this.gvBarCode.AllowUserToAddRows = false;
            this.gvBarCode.AllowUserToDeleteRows = false;
            this.gvBarCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvBarCode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvBarCode.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvBarCode.Location = new System.Drawing.Point(11, 42);
            this.gvBarCode.Name = "gvBarCode";
            this.gvBarCode.Size = new System.Drawing.Size(651, 284);
            this.gvBarCode.TabIndex = 23;
            this.gvBarCode.Tag = "Colors";
            this.gvBarCode.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvBarCode_CellEndEdit);
            this.gvBarCode.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvBarCode_ColumnHeaderMouseClick);
            this.gvBarCode.DoubleClick += new System.EventHandler(this.gvBarCode_DoubleClick);
            this.gvBarCode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.gvBarCode_PreviewKeyDown);
            // 
            // btnCommand
            // 
            this.btnCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCommand.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCommand.Image = null;
            this.btnCommand.Location = new System.Drawing.Point(11, 334);
            this.btnCommand.Name = "btnCommand";
            this.btnCommand.Size = new System.Drawing.Size(146, 28);
            this.btnCommand.TabIndex = 22;
            this.btnCommand.Text = "&Print";
            this.btnCommand.UseSelectable = true;
            this.btnCommand.UseVisualStyleBackColor = true;
            this.btnCommand.Click += new System.EventHandler(this.btnCommand_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 17);
            this.label5.TabIndex = 24;
            this.label5.Text = "Search :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(575, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 17);
            this.label4.TabIndex = 31;
            this.label4.Text = "Type";
            this.label4.Visible = false;
            // 
            // cbType
            // 
            this.cbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Regular",
            "Markdown"});
            this.cbType.Location = new System.Drawing.Point(576, 30);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(438, 25);
            this.cbType.TabIndex = 30;
            this.cbType.Visible = false;
            // 
            // ContextMenuStrip
            // 
            this.ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.IsPrinted});
            this.ContextMenuStrip.Name = "ContextMenuStrip";
            this.ContextMenuStrip.Size = new System.Drawing.Size(171, 28);
            // 
            // IsPrinted
            // 
            this.IsPrinted.Name = "IsPrinted";
            this.IsPrinted.Size = new System.Drawing.Size(170, 24);
            this.IsPrinted.Text = "Tag as Printed";
            this.IsPrinted.Click += new System.EventHandler(this.IsPrinted_Click);
            // 
            // frmBarcodeAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(1040, 470);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.MinimumSize = new System.Drawing.Size(1040, 470);
            this.Name = "frmBarcodeAll";
            this.Padding = new System.Windows.Forms.Padding(6, 70, 6, 17);
            this.Text = "Price Tag Printing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBarcodeAll_FormClosing);
            this.Load += new System.EventHandler(this.frmBarcodeAll_Load);
            this.Resize += new System.EventHandler(this.frmBarcodePrinting_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPrinter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBarCode)).EndInit();
            this.ContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gvBarCode;
        private System.Windows.Forms.ComboBox cbPrinter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SearchDoc;
        private MetroFramework.Controls.MetroButton btnCancel;
        private System.Windows.Forms.Timer tmrPrint;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem IsPrinted;
        public MetroFramework.Controls.MetroTextBox.MetroTextButton btnCommand;
        private System.Windows.Forms.DataGridView gvPrinter;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.CheckBox cb_UPC;
        private System.Windows.Forms.CheckBox cb_IB;
    }
}
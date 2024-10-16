namespace PresenterLayer.Views
{
    partial class frmBarcodePerBP
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_IB = new System.Windows.Forms.CheckBox();
            this.cb_UPC = new System.Windows.Forms.CheckBox();
            this.txtDocEntry = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbB_DocType = new System.Windows.Forms.ComboBox();
            this.pB_GetDocNum = new System.Windows.Forms.PictureBox();
            this.txtDocNum = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numQty = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbPrinter = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pbStyleName = new System.Windows.Forms.PictureBox();
            this.pbGetBPCode = new System.Windows.Forms.PictureBox();
            this.txtBpName = new System.Windows.Forms.TextBox();
            this.txtBpCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gvPrinter = new System.Windows.Forms.DataGridView();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.btnCommand = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.gvBarCode = new System.Windows.Forms.DataGridView();
            this.txtSearchItem = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pB_GetDocNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStyleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGetBPCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPrinter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBarCode)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnCommand);
            this.panel1.Controls.Add(this.gvBarCode);
            this.panel1.Controls.Add(this.txtSearchItem);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 65);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1189, 416);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cb_IB);
            this.groupBox1.Controls.Add(this.cb_UPC);
            this.groupBox1.Controls.Add(this.txtDocEntry);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cbB_DocType);
            this.groupBox1.Controls.Add(this.pB_GetDocNum);
            this.groupBox1.Controls.Add(this.txtDocNum);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.numQty);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.cbType);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbPrinter);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pbStyleName);
            this.groupBox1.Controls.Add(this.pbGetBPCode);
            this.groupBox1.Controls.Add(this.txtBpName);
            this.groupBox1.Controls.Add(this.txtBpCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.gvPrinter);
            this.groupBox1.Location = new System.Drawing.Point(732, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 378);
            this.groupBox1.TabIndex = 151;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Printer Properties";
            // 
            // cb_IB
            // 
            this.cb_IB.AutoSize = true;
            this.cb_IB.Location = new System.Drawing.Point(125, 223);
            this.cb_IB.Name = "cb_IB";
            this.cb_IB.Size = new System.Drawing.Size(133, 20);
            this.cb_IB.TabIndex = 184;
            this.cb_IB.Text = "Inverted Printing";
            this.cb_IB.UseVisualStyleBackColor = true;
            // 
            // cb_UPC
            // 
            this.cb_UPC.AutoSize = true;
            this.cb_UPC.Location = new System.Drawing.Point(11, 223);
            this.cb_UPC.Name = "cb_UPC";
            this.cb_UPC.Size = new System.Drawing.Size(110, 20);
            this.cb_UPC.TabIndex = 183;
            this.cb_UPC.Text = "UPC Printing";
            this.cb_UPC.UseVisualStyleBackColor = true;
            // 
            // txtDocEntry
            // 
            this.txtDocEntry.Location = new System.Drawing.Point(373, 47);
            this.txtDocEntry.Name = "txtDocEntry";
            this.txtDocEntry.Size = new System.Drawing.Size(58, 23);
            this.txtDocEntry.TabIndex = 182;
            this.txtDocEntry.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 16);
            this.label10.TabIndex = 181;
            this.label10.Text = "Doc Type:";
            // 
            // cbB_DocType
            // 
            this.cbB_DocType.FormattingEnabled = true;
            this.cbB_DocType.Items.AddRange(new object[] {
            "Sales Order",
            "Purchase Order",
            "Inventory Transfer Request",
            "Inventory Transfer"});
            this.cbB_DocType.Location = new System.Drawing.Point(114, 17);
            this.cbB_DocType.Name = "cbB_DocType";
            this.cbB_DocType.Size = new System.Drawing.Size(253, 24);
            this.cbB_DocType.TabIndex = 180;
            // 
            // pB_GetDocNum
            // 
            this.pB_GetDocNum.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pB_GetDocNum.Location = new System.Drawing.Point(340, 48);
            this.pB_GetDocNum.Name = "pB_GetDocNum";
            this.pB_GetDocNum.Size = new System.Drawing.Size(23, 18);
            this.pB_GetDocNum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pB_GetDocNum.TabIndex = 178;
            this.pB_GetDocNum.TabStop = false;
            this.pB_GetDocNum.Click += new System.EventHandler(this.pB_GetDocNum_Click);
            // 
            // txtDocNum
            // 
            this.txtDocNum.Location = new System.Drawing.Point(114, 47);
            this.txtDocNum.Name = "txtDocNum";
            this.txtDocNum.Size = new System.Drawing.Size(253, 23);
            this.txtDocNum.TabIndex = 177;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 16);
            this.label9.TabIndex = 176;
            this.label9.Text = "Doc Num:";
            // 
            // numQty
            // 
            this.numQty.Location = new System.Drawing.Point(114, 192);
            this.numQty.Name = "numQty";
            this.numQty.Size = new System.Drawing.Size(253, 23);
            this.numQty.TabIndex = 175;
            this.numQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 195);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 174;
            this.label5.Text = "Quantity :";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(373, 192);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 173;
            this.btnAdd.Text = "&Add Printer";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Regular",
            "Markdown"});
            this.cbType.Location = new System.Drawing.Point(114, 136);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(253, 24);
            this.cbType.TabIndex = 168;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 16);
            this.label6.TabIndex = 171;
            this.label6.Text = "Type :";
            // 
            // cbPrinter
            // 
            this.cbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrinter.FormattingEnabled = true;
            this.cbPrinter.Location = new System.Drawing.Point(114, 164);
            this.cbPrinter.Name = "cbPrinter";
            this.cbPrinter.Size = new System.Drawing.Size(253, 24);
            this.cbPrinter.TabIndex = 167;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 16);
            this.label3.TabIndex = 172;
            this.label3.Text = "Printer :";
            // 
            // pbStyleName
            // 
            this.pbStyleName.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbStyleName.Location = new System.Drawing.Point(423, 107);
            this.pbStyleName.Name = "pbStyleName";
            this.pbStyleName.Size = new System.Drawing.Size(25, 19);
            this.pbStyleName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbStyleName.TabIndex = 170;
            this.pbStyleName.TabStop = false;
            this.pbStyleName.Visible = false;
            this.pbStyleName.Click += new System.EventHandler(this.pbStyleName_Click);
            // 
            // pbGetBPCode
            // 
            this.pbGetBPCode.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbGetBPCode.Location = new System.Drawing.Point(340, 77);
            this.pbGetBPCode.Name = "pbGetBPCode";
            this.pbGetBPCode.Size = new System.Drawing.Size(23, 18);
            this.pbGetBPCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbGetBPCode.TabIndex = 169;
            this.pbGetBPCode.TabStop = false;
            this.pbGetBPCode.Visible = false;
            this.pbGetBPCode.Click += new System.EventHandler(this.pbStyleCode_Click);
            // 
            // txtBpName
            // 
            this.txtBpName.Location = new System.Drawing.Point(114, 107);
            this.txtBpName.Name = "txtBpName";
            this.txtBpName.ReadOnly = true;
            this.txtBpName.Size = new System.Drawing.Size(334, 23);
            this.txtBpName.TabIndex = 166;
            // 
            // txtBpCode
            // 
            this.txtBpCode.Location = new System.Drawing.Point(114, 76);
            this.txtBpCode.Name = "txtBpCode";
            this.txtBpCode.ReadOnly = true;
            this.txtBpCode.Size = new System.Drawing.Size(253, 23);
            this.txtBpCode.TabIndex = 165;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 163;
            this.label2.Text = "BP Name :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 164;
            this.label1.Text = "BP Code :";
            // 
            // gvPrinter
            // 
            this.gvPrinter.AllowUserToAddRows = false;
            this.gvPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvPrinter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvPrinter.Location = new System.Drawing.Point(6, 249);
            this.gvPrinter.Name = "gvPrinter";
            this.gvPrinter.Size = new System.Drawing.Size(442, 123);
            this.gvPrinter.TabIndex = 161;
            this.gvPrinter.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvPrinter_CellContentClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Location = new System.Drawing.Point(167, 372);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(146, 29);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCommand
            // 
            this.btnCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCommand.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCommand.Image = null;
            this.btnCommand.Location = new System.Drawing.Point(15, 372);
            this.btnCommand.Name = "btnCommand";
            this.btnCommand.Size = new System.Drawing.Size(146, 28);
            this.btnCommand.TabIndex = 9;
            this.btnCommand.Text = "&Print";
            this.btnCommand.UseSelectable = true;
            this.btnCommand.UseVisualStyleBackColor = true;
            this.btnCommand.Click += new System.EventHandler(this.btnCommand_Click);
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
            this.gvBarCode.Location = new System.Drawing.Point(13, 38);
            this.gvBarCode.Name = "gvBarCode";
            this.gvBarCode.Size = new System.Drawing.Size(703, 328);
            this.gvBarCode.TabIndex = 8;
            this.gvBarCode.Tag = "Colors";
            this.gvBarCode.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvBarCode_CellEndEdit);
            this.gvBarCode.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvBarCode_ColumnHeaderMouseClick);
            // 
            // txtSearchItem
            // 
            this.txtSearchItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchItem.Location = new System.Drawing.Point(86, 12);
            this.txtSearchItem.Name = "txtSearchItem";
            this.txtSearchItem.Size = new System.Drawing.Size(630, 23);
            this.txtSearchItem.TabIndex = 7;
            this.txtSearchItem.TextChanged += new System.EventHandler(this.txtSearchItem_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Search Item :";
            // 
            // frmBarcodePerBP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(1209, 492);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(997, 415);
            this.Name = "frmBarcodePerBP";
            this.Padding = new System.Windows.Forms.Padding(10, 65, 10, 11);
            this.Text = "Price Tag Printing Per Document";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBarcodePerPB_FormClosing);
            this.Load += new System.EventHandler(this.frmBarcodePerPB_Load);
            this.Resize += new System.EventHandler(this.frmBarcodePerPB_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pB_GetDocNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStyleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGetBPCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPrinter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBarCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gvBarCode;
        private System.Windows.Forms.TextBox txtSearchItem;
        private System.Windows.Forms.Label label4;
        private MetroFramework.Controls.MetroButton btnCancel;
        public MetroFramework.Controls.MetroTextBox.MetroTextButton btnCommand;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView gvPrinter;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbPrinter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pbStyleName;
        private System.Windows.Forms.PictureBox pbGetBPCode;
        private System.Windows.Forms.TextBox txtBpName;
        private System.Windows.Forms.TextBox txtBpCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbB_DocType;
        private System.Windows.Forms.PictureBox pB_GetDocNum;
        private System.Windows.Forms.TextBox txtDocNum;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDocEntry;
        private System.Windows.Forms.CheckBox cb_IB;
        private System.Windows.Forms.CheckBox cb_UPC;
    }
}
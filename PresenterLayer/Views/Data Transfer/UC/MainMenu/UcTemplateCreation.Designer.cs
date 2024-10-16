namespace PresenterLayer.Views
{
    partial class UcTemplateCreation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtMapDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtMapName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CmbUploadType = new System.Windows.Forms.ComboBox();
            this.BtnFile = new System.Windows.Forms.Button();
            this.btnFindPurchaseOrder = new System.Windows.Forms.Button();
            this.BtnBack = new System.Windows.Forms.Button();
            this.DgvMap = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.CmbWorkSheet = new System.Windows.Forms.ComboBox();
            this.TxtTemplate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DgvExcel = new System.Windows.Forms.DataGridView();
            this.msItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvExcel)).BeginInit();
            this.msItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TxtMapDescription);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.TxtMapName);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.CmbUploadType);
            this.panel1.Controls.Add(this.BtnFile);
            this.panel1.Controls.Add(this.btnFindPurchaseOrder);
            this.panel1.Controls.Add(this.BtnBack);
            this.panel1.Controls.Add(this.DgvMap);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.CmbWorkSheet);
            this.panel1.Controls.Add(this.TxtTemplate);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.DgvExcel);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(15, 2, 3, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1144, 649);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 17);
            this.label1.TabIndex = 344;
            this.label1.Text = "Template: ";
            // 
            // TxtMapDescription
            // 
            this.TxtMapDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMapDescription.Location = new System.Drawing.Point(663, 558);
            this.TxtMapDescription.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtMapDescription.Multiline = true;
            this.TxtMapDescription.Name = "TxtMapDescription";
            this.TxtMapDescription.Size = new System.Drawing.Size(468, 45);
            this.TxtMapDescription.TabIndex = 343;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(660, 538);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 17);
            this.label6.TabIndex = 342;
            this.label6.Text = "Map Description";
            // 
            // TxtMapName
            // 
            this.TxtMapName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMapName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.TxtMapName.Location = new System.Drawing.Point(663, 505);
            this.TxtMapName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtMapName.Name = "TxtMapName";
            this.TxtMapName.Size = new System.Drawing.Size(468, 23);
            this.TxtMapName.TabIndex = 341;
            this.TxtMapName.TextChanged += new System.EventHandler(this.TxtMapName_TextChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(660, 485);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 340;
            this.label5.Text = "Map code";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(804, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 17);
            this.label4.TabIndex = 339;
            this.label4.Text = "Upload Type:";
            // 
            // CmbUploadType
            // 
            this.CmbUploadType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.CmbUploadType.FormattingEnabled = true;
            this.CmbUploadType.Items.AddRange(new object[] {
            "Carton Packing List",
            "Delivery",
            "Inventory Transfer Request",
            "Sales Order",
            "Sales Quotation",
            "Inventory Counting",
            "RAS Report",
            "A/R invoice",
            "A/R Credit Memo",
            "A/R invoice (Group By Name)"});
            this.CmbUploadType.Location = new System.Drawing.Point(907, 53);
            this.CmbUploadType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbUploadType.MaxDropDownItems = 9;
            this.CmbUploadType.Name = "CmbUploadType";
            this.CmbUploadType.Size = new System.Drawing.Size(209, 26);
            this.CmbUploadType.TabIndex = 338;
            // 
            // BtnFile
            // 
            this.BtnFile.BackColor = System.Drawing.Color.RoyalBlue;
            this.BtnFile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.BtnFile.ForeColor = System.Drawing.Color.White;
            this.BtnFile.Location = new System.Drawing.Point(379, 52);
            this.BtnFile.Margin = new System.Windows.Forms.Padding(4);
            this.BtnFile.Name = "BtnFile";
            this.BtnFile.Size = new System.Drawing.Size(104, 26);
            this.BtnFile.TabIndex = 337;
            this.BtnFile.Text = "Browse file";
            this.BtnFile.UseVisualStyleBackColor = false;
            this.BtnFile.Click += new System.EventHandler(this.BtnFile_Click);
            // 
            // btnFindPurchaseOrder
            // 
            this.btnFindPurchaseOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindPurchaseOrder.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnFindPurchaseOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindPurchaseOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindPurchaseOrder.ForeColor = System.Drawing.Color.White;
            this.btnFindPurchaseOrder.Location = new System.Drawing.Point(912, 609);
            this.btnFindPurchaseOrder.Margin = new System.Windows.Forms.Padding(4);
            this.btnFindPurchaseOrder.Name = "btnFindPurchaseOrder";
            this.btnFindPurchaseOrder.Size = new System.Drawing.Size(115, 34);
            this.btnFindPurchaseOrder.TabIndex = 336;
            this.btnFindPurchaseOrder.Text = "Save";
            this.btnFindPurchaseOrder.UseVisualStyleBackColor = false;
            this.btnFindPurchaseOrder.Click += new System.EventHandler(this.btnFindPurchaseOrder_Click);
            // 
            // BtnBack
            // 
            this.BtnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBack.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.BtnBack.ForeColor = System.Drawing.Color.White;
            this.BtnBack.Location = new System.Drawing.Point(1033, 609);
            this.BtnBack.Margin = new System.Windows.Forms.Padding(4);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(99, 34);
            this.BtnBack.TabIndex = 335;
            this.BtnBack.Text = "Back";
            this.BtnBack.UseVisualStyleBackColor = false;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // DgvMap
            // 
            this.DgvMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvMap.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.DgvMap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMap.Location = new System.Drawing.Point(664, 96);
            this.DgvMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvMap.Name = "DgvMap";
            this.DgvMap.RowHeadersWidth = 51;
            this.DgvMap.RowTemplate.Height = 24;
            this.DgvMap.Size = new System.Drawing.Size(468, 386);
            this.DgvMap.TabIndex = 334;
            this.DgvMap.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvMap_CellClick);
            this.DgvMap.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvMap_RowHeaderMouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(503, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 333;
            this.label2.Text = "Worksheet: ";
            // 
            // CmbWorkSheet
            // 
            this.CmbWorkSheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.CmbWorkSheet.FormattingEnabled = true;
            this.CmbWorkSheet.Location = new System.Drawing.Point(593, 53);
            this.CmbWorkSheet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbWorkSheet.Name = "CmbWorkSheet";
            this.CmbWorkSheet.Size = new System.Drawing.Size(179, 26);
            this.CmbWorkSheet.TabIndex = 332;
            this.CmbWorkSheet.SelectedIndexChanged += new System.EventHandler(this.CmbWorkSheet_SelectedIndexChanged);
            // 
            // TxtTemplate
            // 
            this.TxtTemplate.Location = new System.Drawing.Point(92, 52);
            this.TxtTemplate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtTemplate.Multiline = true;
            this.TxtTemplate.Name = "TxtTemplate";
            this.TxtTemplate.Size = new System.Drawing.Size(280, 26);
            this.TxtTemplate.TabIndex = 331;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(3, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 25);
            this.label3.TabIndex = 20;
            this.label3.Text = "Mapping";
            // 
            // DgvExcel
            // 
            this.DgvExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvExcel.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.DgvExcel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvExcel.Location = new System.Drawing.Point(8, 96);
            this.DgvExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvExcel.Name = "DgvExcel";
            this.DgvExcel.ReadOnly = true;
            this.DgvExcel.RowHeadersWidth = 51;
            this.DgvExcel.RowTemplate.Height = 24;
            this.DgvExcel.Size = new System.Drawing.Size(644, 542);
            this.DgvExcel.TabIndex = 0;
            this.DgvExcel.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvExcel_CellDoubleClick);
            this.DgvExcel.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DgvExcel_RowPostPaint);
            // 
            // msItems
            // 
            this.msItems.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteItemsToolStripMenuItem});
            this.msItems.Name = "msItems";
            this.msItems.Size = new System.Drawing.Size(167, 30);
            // 
            // deleteItemsToolStripMenuItem
            // 
            this.deleteItemsToolStripMenuItem.Image = global::PresenterLayer.Properties.Resources.close;
            this.deleteItemsToolStripMenuItem.Name = "deleteItemsToolStripMenuItem";
            this.deleteItemsToolStripMenuItem.Size = new System.Drawing.Size(166, 26);
            this.deleteItemsToolStripMenuItem.Text = "Delete Items";
            this.deleteItemsToolStripMenuItem.Click += new System.EventHandler(this.deleteItemsToolStripMenuItem_Click);
            // 
            // UcTemplateCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UcTemplateCreation";
            this.Size = new System.Drawing.Size(1144, 649);
            this.Load += new System.EventHandler(this.UcTemplateCreation_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvExcel)).EndInit();
            this.msItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView DgvExcel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip msItems;
        private System.Windows.Forms.ToolStripMenuItem deleteItemsToolStripMenuItem;
        private System.Windows.Forms.TextBox TxtMapDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TxtMapName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CmbUploadType;
        private System.Windows.Forms.Button BtnFile;
        private System.Windows.Forms.Button btnFindPurchaseOrder;
        private System.Windows.Forms.Button BtnBack;
        private System.Windows.Forms.DataGridView DgvMap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CmbWorkSheet;
        private System.Windows.Forms.TextBox TxtTemplate;
        private System.Windows.Forms.Label label1;
    }
}

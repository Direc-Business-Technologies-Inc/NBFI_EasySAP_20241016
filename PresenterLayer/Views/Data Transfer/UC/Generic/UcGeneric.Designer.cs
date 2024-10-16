namespace PresenterLayer.Views
{
    partial class UcGeneric
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.CmbSecondColumn = new System.Windows.Forms.ComboBox();
            this.CmbFirstColumn = new System.Windows.Forms.ComboBox();
            this.BtnStatus = new System.Windows.Forms.Button();
            this.TxtMap = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnBack = new System.Windows.Forms.Button();
            this.BtnUpload = new System.Windows.Forms.Button();
            this.TxtLineCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtDocumentCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CmbUploadType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CmbWorkSheet = new System.Windows.Forms.ComboBox();
            this.BtnOpenTemplate = new System.Windows.Forms.Button();
            this.TxtTemplate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DgvExcel = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvExcel)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CmbSecondColumn);
            this.panel1.Controls.Add(this.CmbFirstColumn);
            this.panel1.Controls.Add(this.BtnStatus);
            this.panel1.Controls.Add(this.TxtMap);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.BtnBack);
            this.panel1.Controls.Add(this.BtnUpload);
            this.panel1.Controls.Add(this.TxtLineCount);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.TxtDocumentCount);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.CmbUploadType);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.CmbWorkSheet);
            this.panel1.Controls.Add(this.BtnOpenTemplate);
            this.panel1.Controls.Add(this.TxtTemplate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.DgvExcel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(1144, 649);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // CmbSecondColumn
            // 
            this.CmbSecondColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbSecondColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.CmbSecondColumn.FormattingEnabled = true;
            this.CmbSecondColumn.Items.AddRange(new object[] {
            "Row",
            "Column"});
            this.CmbSecondColumn.Location = new System.Drawing.Point(873, 97);
            this.CmbSecondColumn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbSecondColumn.Name = "CmbSecondColumn";
            this.CmbSecondColumn.Size = new System.Drawing.Size(136, 26);
            this.CmbSecondColumn.TabIndex = 25;
            // 
            // CmbFirstColumn
            // 
            this.CmbFirstColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbFirstColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.CmbFirstColumn.FormattingEnabled = true;
            this.CmbFirstColumn.Items.AddRange(new object[] {
            "Row",
            "Column"});
            this.CmbFirstColumn.Location = new System.Drawing.Point(713, 97);
            this.CmbFirstColumn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbFirstColumn.Name = "CmbFirstColumn";
            this.CmbFirstColumn.Size = new System.Drawing.Size(153, 26);
            this.CmbFirstColumn.TabIndex = 24;
            // 
            // BtnStatus
            // 
            this.BtnStatus.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStatus.Location = new System.Drawing.Point(179, 140);
            this.BtnStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnStatus.Name = "BtnStatus";
            this.BtnStatus.Size = new System.Drawing.Size(75, 38);
            this.BtnStatus.TabIndex = 15;
            this.BtnStatus.Text = "Status";
            this.BtnStatus.UseVisualStyleBackColor = true;
            this.BtnStatus.Click += new System.EventHandler(this.BtnStatus_Click);
            // 
            // TxtMap
            // 
            this.TxtMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.TxtMap.Location = new System.Drawing.Point(137, 23);
            this.TxtMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtMap.Name = "TxtMap";
            this.TxtMap.Size = new System.Drawing.Size(255, 26);
            this.TxtMap.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label6.Location = new System.Drawing.Point(8, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 20);
            this.label6.TabIndex = 13;
            this.label6.Text = "Map :";
            // 
            // BtnBack
            // 
            this.BtnBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBack.Location = new System.Drawing.Point(259, 140);
            this.BtnBack.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(75, 38);
            this.BtnBack.TabIndex = 12;
            this.BtnBack.Text = "Back";
            this.BtnBack.UseVisualStyleBackColor = true;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // BtnUpload
            // 
            this.BtnUpload.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUpload.Location = new System.Drawing.Point(9, 140);
            this.BtnUpload.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnUpload.Name = "BtnUpload";
            this.BtnUpload.Size = new System.Drawing.Size(163, 38);
            this.BtnUpload.TabIndex = 11;
            this.BtnUpload.Text = "Upload";
            this.BtnUpload.UseVisualStyleBackColor = true;
            this.BtnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // TxtLineCount
            // 
            this.TxtLineCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtLineCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.TxtLineCount.Location = new System.Drawing.Point(1016, 97);
            this.TxtLineCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtLineCount.Name = "TxtLineCount";
            this.TxtLineCount.Size = new System.Drawing.Size(88, 26);
            this.TxtLineCount.TabIndex = 10;
            this.TxtLineCount.Text = "0";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(581, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Column end : ";
            // 
            // TxtDocumentCount
            // 
            this.TxtDocumentCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtDocumentCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.TxtDocumentCount.Location = new System.Drawing.Point(713, 62);
            this.TxtDocumentCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtDocumentCount.Name = "TxtDocumentCount";
            this.TxtDocumentCount.Size = new System.Drawing.Size(391, 26);
            this.TxtDocumentCount.TabIndex = 8;
            this.TxtDocumentCount.Text = "1";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(581, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Row end : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(581, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Upload type :";
            // 
            // CmbUploadType
            // 
            this.CmbUploadType.Enabled = false;
            this.CmbUploadType.FormattingEnabled = true;
            this.CmbUploadType.Items.AddRange(new object[] {
            "Carton Packing List",
            "Delivery",
            "Inventory Transfer Request",
            "Inventory Counting",
            "Sales Order",
            "RAS Report",
            "A/R invoice",
            "A/R Credit Memo",
            "A/R invoice (Group By Name)"});
            this.CmbUploadType.Location = new System.Drawing.Point(713, 25);
            this.CmbUploadType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbUploadType.MaxDropDownItems = 9;
            this.CmbUploadType.Name = "CmbUploadType";
            this.CmbUploadType.Size = new System.Drawing.Size(391, 24);
            this.CmbUploadType.TabIndex = 5;
            this.CmbUploadType.SelectedValueChanged += new System.EventHandler(this.CmbUploadType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(8, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Worksheet :";
            // 
            // CmbWorkSheet
            // 
            this.CmbWorkSheet.FormattingEnabled = true;
            this.CmbWorkSheet.Location = new System.Drawing.Point(137, 100);
            this.CmbWorkSheet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbWorkSheet.Name = "CmbWorkSheet";
            this.CmbWorkSheet.Size = new System.Drawing.Size(255, 24);
            this.CmbWorkSheet.TabIndex = 3;
            this.CmbWorkSheet.SelectedIndexChanged += new System.EventHandler(this.CmbWorkSheet_SelectedIndexChanged);
            // 
            // BtnOpenTemplate
            // 
            this.BtnOpenTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOpenTemplate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnOpenTemplate.Location = new System.Drawing.Point(399, 64);
            this.BtnOpenTemplate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnOpenTemplate.Name = "BtnOpenTemplate";
            this.BtnOpenTemplate.Size = new System.Drawing.Size(75, 26);
            this.BtnOpenTemplate.TabIndex = 2;
            this.BtnOpenTemplate.Text = "Browse";
            this.BtnOpenTemplate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnOpenTemplate.UseVisualStyleBackColor = true;
            this.BtnOpenTemplate.Click += new System.EventHandler(this.BtnOpenTemplate_Click);
            // 
            // TxtTemplate
            // 
            this.TxtTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.TxtTemplate.Location = new System.Drawing.Point(137, 62);
            this.TxtTemplate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtTemplate.Name = "TxtTemplate";
            this.TxtTemplate.Size = new System.Drawing.Size(255, 26);
            this.TxtTemplate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(8, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Template :";
            // 
            // DgvExcel
            // 
            this.DgvExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvExcel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvExcel.GridColor = System.Drawing.Color.Gainsboro;
            this.DgvExcel.Location = new System.Drawing.Point(8, 183);
            this.DgvExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvExcel.Name = "DgvExcel";
            this.DgvExcel.RowHeadersWidth = 51;
            this.DgvExcel.RowTemplate.Height = 24;
            this.DgvExcel.Size = new System.Drawing.Size(1128, 457);
            this.DgvExcel.TabIndex = 0;
            this.DgvExcel.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DgvExcel_RowPostPaint);
            // 
            // UcGeneric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UcGeneric";
            this.Size = new System.Drawing.Size(1144, 649);
            this.Load += new System.EventHandler(this.UcGeneric_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvExcel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox CmbSecondColumn;
        private System.Windows.Forms.ComboBox CmbFirstColumn;
        private System.Windows.Forms.Button BtnStatus;
        private System.Windows.Forms.TextBox TxtMap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnBack;
        private System.Windows.Forms.Button BtnUpload;
        private System.Windows.Forms.TextBox TxtLineCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtDocumentCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CmbUploadType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CmbWorkSheet;
        private System.Windows.Forms.Button BtnOpenTemplate;
        private System.Windows.Forms.TextBox TxtTemplate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView DgvExcel;
    }
}

namespace PresenterLayer.Views
{
    partial class UcUDF_delivery
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
            this.TxtVat = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbVatGroup = new System.Windows.Forms.PictureBox();
            this.txtDeliveryDate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtDocDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.CmbDocumentType = new System.Windows.Forms.ComboBox();
            this.TxtDocumentDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DtDocumentDate = new System.Windows.Forms.DateTimePicker();
            this.TxtPostingDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DtPostingDate = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.pbVatGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtVat
            // 
            this.TxtVat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtVat.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TxtVat.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.TxtVat.Location = new System.Drawing.Point(152, 167);
            this.TxtVat.Margin = new System.Windows.Forms.Padding(4);
            this.TxtVat.Multiline = true;
            this.TxtVat.Name = "TxtVat";
            this.TxtVat.ReadOnly = true;
            this.TxtVat.Size = new System.Drawing.Size(225, 23);
            this.TxtVat.TabIndex = 175;
            this.TxtVat.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label1.Location = new System.Drawing.Point(20, 170);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 15);
            this.label1.TabIndex = 174;
            this.label1.Text = "Vat Group";
            this.label1.Visible = false;
            // 
            // pbVatGroup
            // 
            this.pbVatGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbVatGroup.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbVatGroup.Location = new System.Drawing.Point(349, 170);
            this.pbVatGroup.Margin = new System.Windows.Forms.Padding(4);
            this.pbVatGroup.Name = "pbVatGroup";
            this.pbVatGroup.Size = new System.Drawing.Size(25, 18);
            this.pbVatGroup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbVatGroup.TabIndex = 176;
            this.pbVatGroup.TabStop = false;
            this.pbVatGroup.Click += new System.EventHandler(this.pbVatGroup_Click);
            // 
            // txtDeliveryDate
            // 
            this.txtDeliveryDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeliveryDate.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtDeliveryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtDeliveryDate.Location = new System.Drawing.Point(148, 75);
            this.txtDeliveryDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtDeliveryDate.Multiline = true;
            this.txtDeliveryDate.Name = "txtDeliveryDate";
            this.txtDeliveryDate.Size = new System.Drawing.Size(185, 21);
            this.txtDeliveryDate.TabIndex = 179;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label6.Location = new System.Drawing.Point(16, 78);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 15);
            this.label6.TabIndex = 177;
            this.label6.Text = "Delivery Date";
            // 
            // dtDocDate
            // 
            this.dtDocDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtDocDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.dtDocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.dtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDocDate.Location = new System.Drawing.Point(148, 75);
            this.dtDocDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtDocDate.Name = "dtDocDate";
            this.dtDocDate.Size = new System.Drawing.Size(225, 21);
            this.dtDocDate.TabIndex = 178;
            this.dtDocDate.ValueChanged += new System.EventHandler(this.dtDocDate_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label2.Location = new System.Drawing.Point(16, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 15);
            this.label2.TabIndex = 180;
            this.label2.Text = "Document Type";
            // 
            // CmbDocumentType
            // 
            this.CmbDocumentType.FormattingEnabled = true;
            this.CmbDocumentType.Location = new System.Drawing.Point(148, 15);
            this.CmbDocumentType.Name = "CmbDocumentType";
            this.CmbDocumentType.Size = new System.Drawing.Size(225, 24);
            this.CmbDocumentType.TabIndex = 181;
            this.CmbDocumentType.SelectedIndexChanged += new System.EventHandler(this.CmbDocumentType_SelectedIndexChanged);
            // 
            // TxtDocumentDate
            // 
            this.TxtDocumentDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtDocumentDate.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TxtDocumentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.TxtDocumentDate.Location = new System.Drawing.Point(148, 104);
            this.TxtDocumentDate.Margin = new System.Windows.Forms.Padding(4);
            this.TxtDocumentDate.Multiline = true;
            this.TxtDocumentDate.Name = "TxtDocumentDate";
            this.TxtDocumentDate.Size = new System.Drawing.Size(185, 21);
            this.TxtDocumentDate.TabIndex = 184;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label3.Location = new System.Drawing.Point(16, 107);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 15);
            this.label3.TabIndex = 182;
            this.label3.Text = "Document Date";
            // 
            // DtDocumentDate
            // 
            this.DtDocumentDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DtDocumentDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.DtDocumentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.DtDocumentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtDocumentDate.Location = new System.Drawing.Point(148, 104);
            this.DtDocumentDate.Margin = new System.Windows.Forms.Padding(4);
            this.DtDocumentDate.Name = "DtDocumentDate";
            this.DtDocumentDate.Size = new System.Drawing.Size(225, 21);
            this.DtDocumentDate.TabIndex = 183;
            this.DtDocumentDate.ValueChanged += new System.EventHandler(this.DtDocumentDate_ValueChanged);
            // 
            // TxtPostingDate
            // 
            this.TxtPostingDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtPostingDate.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TxtPostingDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.TxtPostingDate.Location = new System.Drawing.Point(148, 46);
            this.TxtPostingDate.Margin = new System.Windows.Forms.Padding(4);
            this.TxtPostingDate.Multiline = true;
            this.TxtPostingDate.Name = "TxtPostingDate";
            this.TxtPostingDate.Size = new System.Drawing.Size(185, 21);
            this.TxtPostingDate.TabIndex = 187;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label4.Location = new System.Drawing.Point(16, 49);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 15);
            this.label4.TabIndex = 185;
            this.label4.Text = "Posting Date";
            // 
            // DtPostingDate
            // 
            this.DtPostingDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DtPostingDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.DtPostingDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.DtPostingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtPostingDate.Location = new System.Drawing.Point(148, 46);
            this.DtPostingDate.Margin = new System.Windows.Forms.Padding(4);
            this.DtPostingDate.Name = "DtPostingDate";
            this.DtPostingDate.Size = new System.Drawing.Size(225, 21);
            this.DtPostingDate.TabIndex = 186;
            this.DtPostingDate.ValueChanged += new System.EventHandler(this.DtPostingDate_ValueChanged);
            // 
            // UcUDF_delivery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.TxtPostingDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DtPostingDate);
            this.Controls.Add(this.TxtDocumentDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DtDocumentDate);
            this.Controls.Add(this.CmbDocumentType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDeliveryDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dtDocDate);
            this.Controls.Add(this.pbVatGroup);
            this.Controls.Add(this.TxtVat);
            this.Controls.Add(this.label1);
            this.Name = "UcUDF_delivery";
            this.Size = new System.Drawing.Size(381, 194);
            this.Load += new System.EventHandler(this.UcUDF_delivery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbVatGroup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbVatGroup;
        public System.Windows.Forms.TextBox TxtVat;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtDeliveryDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtDocDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CmbDocumentType;
        public System.Windows.Forms.TextBox TxtDocumentDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DtDocumentDate;
        public System.Windows.Forms.TextBox TxtPostingDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DtPostingDate;
    }
}


namespace PresenterLayer.Views.Data_Transfer.UC.UDF
{
    partial class UcUDF_creditmemo
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
            this.TxtPostingDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DtPostingDate = new System.Windows.Forms.DateTimePicker();
            this.TxtDocumentDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DtDocumentDate = new System.Windows.Forms.DateTimePicker();
            this.CmbDocumentType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDeliveryDate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtDocDate = new System.Windows.Forms.DateTimePicker();
            this.pbVatGroup = new System.Windows.Forms.PictureBox();
            this.TxtVat = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pbBpCode = new System.Windows.Forms.PictureBox();
            this.txtCardCode = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbVatGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBpCode)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtPostingDate
            // 
            this.TxtPostingDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtPostingDate.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TxtPostingDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.TxtPostingDate.Location = new System.Drawing.Point(106, 33);
            this.TxtPostingDate.Multiline = true;
            this.TxtPostingDate.Name = "TxtPostingDate";
            this.TxtPostingDate.Size = new System.Drawing.Size(140, 18);
            this.TxtPostingDate.TabIndex = 201;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label4.Location = new System.Drawing.Point(7, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 199;
            this.label4.Text = "Posting Date";
            // 
            // DtPostingDate
            // 
            this.DtPostingDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DtPostingDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.DtPostingDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.DtPostingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtPostingDate.Location = new System.Drawing.Point(106, 33);
            this.DtPostingDate.Name = "DtPostingDate";
            this.DtPostingDate.Size = new System.Drawing.Size(170, 18);
            this.DtPostingDate.TabIndex = 200;
            this.DtPostingDate.ValueChanged += new System.EventHandler(this.DtPostingDate_ValueChanged);
            // 
            // TxtDocumentDate
            // 
            this.TxtDocumentDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtDocumentDate.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TxtDocumentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.TxtDocumentDate.Location = new System.Drawing.Point(106, 80);
            this.TxtDocumentDate.Multiline = true;
            this.TxtDocumentDate.Name = "TxtDocumentDate";
            this.TxtDocumentDate.Size = new System.Drawing.Size(140, 18);
            this.TxtDocumentDate.TabIndex = 198;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label3.Location = new System.Drawing.Point(7, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 196;
            this.label3.Text = "Document Date";
            // 
            // DtDocumentDate
            // 
            this.DtDocumentDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DtDocumentDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.DtDocumentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.DtDocumentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtDocumentDate.Location = new System.Drawing.Point(106, 80);
            this.DtDocumentDate.Name = "DtDocumentDate";
            this.DtDocumentDate.Size = new System.Drawing.Size(170, 18);
            this.DtDocumentDate.TabIndex = 197;
            this.DtDocumentDate.ValueChanged += new System.EventHandler(this.DtDocumentDate_ValueChanged);
            // 
            // CmbDocumentType
            // 
            this.CmbDocumentType.FormattingEnabled = true;
            this.CmbDocumentType.Location = new System.Drawing.Point(106, 103);
            this.CmbDocumentType.Margin = new System.Windows.Forms.Padding(2);
            this.CmbDocumentType.Name = "CmbDocumentType";
            this.CmbDocumentType.Size = new System.Drawing.Size(170, 21);
            this.CmbDocumentType.TabIndex = 195;
            this.CmbDocumentType.Visible = false;
            this.CmbDocumentType.SelectedIndexChanged += new System.EventHandler(this.CmbDocumentType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label2.Location = new System.Drawing.Point(7, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 194;
            this.label2.Text = "Document Type";
            this.label2.Visible = false;
            // 
            // txtDeliveryDate
            // 
            this.txtDeliveryDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeliveryDate.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtDeliveryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtDeliveryDate.Location = new System.Drawing.Point(106, 57);
            this.txtDeliveryDate.Multiline = true;
            this.txtDeliveryDate.Name = "txtDeliveryDate";
            this.txtDeliveryDate.Size = new System.Drawing.Size(140, 18);
            this.txtDeliveryDate.TabIndex = 193;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label6.Location = new System.Drawing.Point(7, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 191;
            this.label6.Text = "Delivery Date";
            // 
            // dtDocDate
            // 
            this.dtDocDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtDocDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.dtDocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.dtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDocDate.Location = new System.Drawing.Point(106, 57);
            this.dtDocDate.Name = "dtDocDate";
            this.dtDocDate.Size = new System.Drawing.Size(170, 18);
            this.dtDocDate.TabIndex = 192;
            this.dtDocDate.ValueChanged += new System.EventHandler(this.dtDocDate_ValueChanged);
            // 
            // pbVatGroup
            // 
            this.pbVatGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbVatGroup.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbVatGroup.Location = new System.Drawing.Point(257, 134);
            this.pbVatGroup.Name = "pbVatGroup";
            this.pbVatGroup.Size = new System.Drawing.Size(19, 15);
            this.pbVatGroup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbVatGroup.TabIndex = 190;
            this.pbVatGroup.TabStop = false;
            this.pbVatGroup.Visible = false;
            this.pbVatGroup.Click += new System.EventHandler(this.pbVatGroup_Click);
            // 
            // TxtVat
            // 
            this.TxtVat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtVat.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TxtVat.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.TxtVat.Location = new System.Drawing.Point(109, 132);
            this.TxtVat.Multiline = true;
            this.TxtVat.Name = "TxtVat";
            this.TxtVat.ReadOnly = true;
            this.TxtVat.Size = new System.Drawing.Size(170, 19);
            this.TxtVat.TabIndex = 189;
            this.TxtVat.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label1.Location = new System.Drawing.Point(10, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 188;
            this.label1.Text = "Vat Group";
            this.label1.Visible = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label5.Location = new System.Drawing.Point(7, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 202;
            this.label5.Text = "Customer";
            // 
            // pbBpCode
            // 
            this.pbBpCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbBpCode.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbBpCode.Location = new System.Drawing.Point(254, 10);
            this.pbBpCode.Name = "pbBpCode";
            this.pbBpCode.Size = new System.Drawing.Size(19, 15);
            this.pbBpCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBpCode.TabIndex = 204;
            this.pbBpCode.TabStop = false;
            this.pbBpCode.Click += new System.EventHandler(this.pbBpCode_Click);
            // 
            // txtCardCode
            // 
            this.txtCardCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCardCode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtCardCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtCardCode.Location = new System.Drawing.Point(106, 8);
            this.txtCardCode.Multiline = true;
            this.txtCardCode.Name = "txtCardCode";
            this.txtCardCode.ReadOnly = true;
            this.txtCardCode.Size = new System.Drawing.Size(170, 19);
            this.txtCardCode.TabIndex = 203;
            // 
            // UcUDF_creditmemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbBpCode);
            this.Controls.Add(this.txtCardCode);
            this.Controls.Add(this.label5);
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
            this.Name = "UcUDF_creditmemo";
            this.Size = new System.Drawing.Size(286, 158);
            this.Load += new System.EventHandler(this.UcUDF_invoice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbVatGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBpCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox TxtPostingDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DtPostingDate;
        public System.Windows.Forms.TextBox TxtDocumentDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DtDocumentDate;
        private System.Windows.Forms.ComboBox CmbDocumentType;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtDeliveryDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtDocDate;
        private System.Windows.Forms.PictureBox pbVatGroup;
        public System.Windows.Forms.TextBox TxtVat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pbBpCode;
        public System.Windows.Forms.TextBox txtCardCode;
    }
}

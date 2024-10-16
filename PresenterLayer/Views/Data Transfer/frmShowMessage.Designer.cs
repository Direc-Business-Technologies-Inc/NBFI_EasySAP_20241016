namespace PresenterLayer.Views
{
    partial class frmShowMessage
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
            this.BtnBrowse = new System.Windows.Forms.Button();
            this.TxtFile = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.BtnGenerateExcel = new System.Windows.Forms.Button();
            this.DgvMessages = new System.Windows.Forms.DataGridView();
            this.CmbSelectExcel = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMessages)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBrowse.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnBrowse.Location = new System.Drawing.Point(700, 79);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(113, 30);
            this.BtnBrowse.TabIndex = 7;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.UseVisualStyleBackColor = true;
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // TxtFile
            // 
            this.TxtFile.BackColor = System.Drawing.Color.White;
            this.TxtFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TxtFile.Location = new System.Drawing.Point(33, 79);
            this.TxtFile.Name = "TxtFile";
            this.TxtFile.ReadOnly = true;
            this.TxtFile.Size = new System.Drawing.Size(661, 30);
            this.TxtFile.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(165, 573);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 39);
            this.button2.TabIndex = 5;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // BtnGenerateExcel
            // 
            this.BtnGenerateExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnGenerateExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGenerateExcel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnGenerateExcel.Location = new System.Drawing.Point(33, 573);
            this.BtnGenerateExcel.Name = "BtnGenerateExcel";
            this.BtnGenerateExcel.Size = new System.Drawing.Size(126, 39);
            this.BtnGenerateExcel.TabIndex = 4;
            this.BtnGenerateExcel.Text = "Generate Excel";
            this.BtnGenerateExcel.UseVisualStyleBackColor = true;
            this.BtnGenerateExcel.Click += new System.EventHandler(this.BtnGenerateExcel_Click);
            // 
            // DgvMessages
            // 
            this.DgvMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvMessages.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.DgvMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMessages.Location = new System.Drawing.Point(33, 118);
            this.DgvMessages.Name = "DgvMessages";
            this.DgvMessages.RowTemplate.Height = 24;
            this.DgvMessages.Size = new System.Drawing.Size(780, 433);
            this.DgvMessages.TabIndex = 0;
            // 
            // CmbSelectExcel
            // 
            this.CmbSelectExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.CmbSelectExcel.FormattingEnabled = true;
            this.CmbSelectExcel.Items.AddRange(new object[] {
            "Error",
            "Uploaded"});
            this.CmbSelectExcel.Location = new System.Drawing.Point(33, 35);
            this.CmbSelectExcel.Name = "CmbSelectExcel";
            this.CmbSelectExcel.Size = new System.Drawing.Size(273, 28);
            this.CmbSelectExcel.TabIndex = 2;
            this.CmbSelectExcel.Text = "Error";
            this.CmbSelectExcel.SelectedIndexChanged += new System.EventHandler(this.CmbSelectExcel_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CmbSelectExcel);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.DgvMessages);
            this.panel1.Controls.Add(this.BtnBrowse);
            this.panel1.Controls.Add(this.TxtFile);
            this.panel1.Controls.Add(this.BtnGenerateExcel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 70);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(30, 20, 30, 30);
            this.panel1.Size = new System.Drawing.Size(846, 634);
            this.panel1.TabIndex = 8;
            // 
            // frmShowMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 724);
            this.Controls.Add(this.panel1);
            this.Name = "frmShowMessage";
            this.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
            this.Text = "Upload Status";
            this.Load += new System.EventHandler(this.frmShowMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMessages)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button BtnGenerateExcel;
        private System.Windows.Forms.Button BtnBrowse;
        private System.Windows.Forms.TextBox TxtFile;
        private System.Windows.Forms.DataGridView DgvMessages;
        private System.Windows.Forms.ComboBox CmbSelectExcel;
        private System.Windows.Forms.Panel panel1;
    }
}
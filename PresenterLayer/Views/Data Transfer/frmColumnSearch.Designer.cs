namespace PresenterLayer.Views
{
    partial class frmColumnSearch
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
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvSapFields = new System.Windows.Forms.DataGridView();
            this.cmbOptionType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSapFields)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Calibri", 10F);
            this.txtSearch.Location = new System.Drawing.Point(68, 26);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(284, 28);
            this.txtSearch.TabIndex = 17;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtExcelName_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 10F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(78)))));
            this.label7.Location = new System.Drawing.Point(4, 29);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 21);
            this.label7.TabIndex = 16;
            this.label7.Text = "Search:";
            // 
            // dgvSapFields
            // 
            this.dgvSapFields.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvSapFields.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvSapFields.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSapFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSapFields.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvSapFields.Location = new System.Drawing.Point(10, 122);
            this.dgvSapFields.Margin = new System.Windows.Forms.Padding(4);
            this.dgvSapFields.Name = "dgvSapFields";
            this.dgvSapFields.ReadOnly = true;
            this.dgvSapFields.Size = new System.Drawing.Size(453, 422);
            this.dgvSapFields.TabIndex = 19;
            this.dgvSapFields.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSapFields_CellDoubleClick);
            this.dgvSapFields.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSapFields_ColumnHeaderMouseClick);
            this.dgvSapFields.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvSapFields_PreviewKeyDown);
            // 
            // cmbOptionType
            // 
            this.cmbOptionType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbOptionType.Font = new System.Drawing.Font("Calibri", 10F);
            this.cmbOptionType.FormattingEnabled = true;
            this.cmbOptionType.Location = new System.Drawing.Point(360, 26);
            this.cmbOptionType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbOptionType.Name = "cmbOptionType";
            this.cmbOptionType.Size = new System.Drawing.Size(89, 29);
            this.cmbOptionType.TabIndex = 20;
            this.cmbOptionType.SelectedIndexChanged += new System.EventHandler(this.cmbOptionType_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbOptionType);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(453, 484);
            this.panel1.TabIndex = 21;
            // 
            // frmColumnSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 554);
            this.Controls.Add(this.dgvSapFields);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmColumnSearch";
            this.Padding = new System.Windows.Forms.Padding(10, 60, 10, 10);
            this.Text = "SAP Fields";
            this.Load += new System.EventHandler(this.frmSearchColumns_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSapFields)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvSapFields;
        private System.Windows.Forms.ComboBox cmbOptionType;
        private System.Windows.Forms.Panel panel1;
    }
}
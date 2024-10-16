namespace DirecLayer
{
    partial class frmCrystalReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCrystalReports));
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbFindDocument = new System.Windows.Forms.PictureBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.pbSelectForms = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDocEntry = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFindDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pbFindDocument);
            this.panel3.Controls.Add(this.btnPreview);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.pbSelectForms);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.txtDocEntry);
            this.panel3.Controls.Add(this.txtPath);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(842, 113);
            this.panel3.TabIndex = 1;
            // 
            // pbFindDocument
            // 
            this.pbFindDocument.BackColor = System.Drawing.Color.Transparent;
            this.pbFindDocument.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbFindDocument.Location = new System.Drawing.Point(224, 51);
            this.pbFindDocument.Name = "pbFindDocument";
            this.pbFindDocument.Size = new System.Drawing.Size(19, 15);
            this.pbFindDocument.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFindDocument.TabIndex = 310;
            this.pbFindDocument.TabStop = false;
            this.pbFindDocument.Click += new System.EventHandler(this.pbFindDocument_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnPreview.ForeColor = System.Drawing.Color.White;
            this.btnPreview.Location = new System.Drawing.Point(18, 84);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(77, 24);
            this.btnPreview.TabIndex = 309;
            this.btnPreview.Text = "Preview";
            this.btnPreview.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPreview.UseVisualStyleBackColor = false;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(34, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 298;
            this.label7.Text = "File";
            // 
            // pbSelectForms
            // 
            this.pbSelectForms.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.pbSelectForms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbSelectForms.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbSelectForms.Image = ((System.Drawing.Image)(resources.GetObject("pbSelectForms.Image")));
            this.pbSelectForms.Location = new System.Drawing.Point(363, 13);
            this.pbSelectForms.Name = "pbSelectForms";
            this.pbSelectForms.Size = new System.Drawing.Size(29, 27);
            this.pbSelectForms.TabIndex = 300;
            this.pbSelectForms.UseVisualStyleBackColor = true;
            this.pbSelectForms.Click += new System.EventHandler(this.pbSelectForms_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label10.Location = new System.Drawing.Point(16, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 297;
            this.label10.Text = "Doc No.";
            // 
            // txtDocEntry
            // 
            this.txtDocEntry.BackColor = System.Drawing.Color.White;
            this.txtDocEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtDocEntry.Location = new System.Drawing.Point(78, 49);
            this.txtDocEntry.Name = "txtDocEntry";
            this.txtDocEntry.ReadOnly = true;
            this.txtDocEntry.Size = new System.Drawing.Size(168, 20);
            this.txtDocEntry.TabIndex = 296;
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtPath.Location = new System.Drawing.Point(78, 13);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(280, 21);
            this.txtPath.TabIndex = 299;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(20, 60);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.crystalReportViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(842, 560);
            this.splitContainer1.SplitterDistance = 113;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 2;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.DisplayStatusBar = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowCopyButton = false;
            this.crystalReportViewer1.ShowExportButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowLogo = false;
            this.crystalReportViewer1.ShowParameterPanelButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(842, 444);
            this.crystalReportViewer1.TabIndex = 1;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // frmCrystalReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 640);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmCrystalReports";
            this.Text = "Report Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCrystalReports_FormClosing);
            this.Load += new System.EventHandler(this.frmCrystalReports_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFindDocument)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pbFindDocument;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button pbSelectForms;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDocEntry;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
    }
}
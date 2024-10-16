namespace PresenterLayer.Views
{
    partial class FrmDt_Modal
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DgvMap = new System.Windows.Forms.DataGridView();
            this.TxtUploadType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtMapDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtMapCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnBack = new System.Windows.Forms.Button();
            this.BtnProceed = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMap)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 70);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.63536F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.36464F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(647, 362);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DgvMap);
            this.panel1.Controls.Add(this.TxtUploadType);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.TxtMapDescription);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.TxtMapCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(641, 304);
            this.panel1.TabIndex = 1;
            // 
            // DgvMap
            // 
            this.DgvMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvMap.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.DgvMap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMap.Location = new System.Drawing.Point(6, 101);
            this.DgvMap.Name = "DgvMap";
            this.DgvMap.ReadOnly = true;
            this.DgvMap.RowTemplate.Height = 24;
            this.DgvMap.Size = new System.Drawing.Size(627, 200);
            this.DgvMap.TabIndex = 6;
            // 
            // TxtUploadType
            // 
            this.TxtUploadType.BackColor = System.Drawing.SystemColors.Window;
            this.TxtUploadType.Location = new System.Drawing.Point(488, 11);
            this.TxtUploadType.Name = "TxtUploadType";
            this.TxtUploadType.ReadOnly = true;
            this.TxtUploadType.Size = new System.Drawing.Size(145, 22);
            this.TxtUploadType.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(373, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Upload type :";
            // 
            // TxtMapDescription
            // 
            this.TxtMapDescription.BackColor = System.Drawing.SystemColors.Window;
            this.TxtMapDescription.Location = new System.Drawing.Point(141, 47);
            this.TxtMapDescription.Multiline = true;
            this.TxtMapDescription.Name = "TxtMapDescription";
            this.TxtMapDescription.ReadOnly = true;
            this.TxtMapDescription.Size = new System.Drawing.Size(492, 47);
            this.TxtMapDescription.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Map Description :";
            // 
            // TxtMapCode
            // 
            this.TxtMapCode.BackColor = System.Drawing.SystemColors.Window;
            this.TxtMapCode.Location = new System.Drawing.Point(141, 11);
            this.TxtMapCode.Name = "TxtMapCode";
            this.TxtMapCode.ReadOnly = true;
            this.TxtMapCode.Size = new System.Drawing.Size(184, 22);
            this.TxtMapCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Map Code :";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnBack);
            this.panel2.Controls.Add(this.BtnProceed);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 313);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(641, 46);
            this.panel2.TabIndex = 2;
            // 
            // BtnBack
            // 
            this.BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBack.Location = new System.Drawing.Point(99, 6);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(86, 34);
            this.BtnBack.TabIndex = 4;
            this.BtnBack.Text = "Back";
            this.BtnBack.UseVisualStyleBackColor = true;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // BtnProceed
            // 
            this.BtnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnProceed.Location = new System.Drawing.Point(7, 6);
            this.BtnProceed.Name = "BtnProceed";
            this.BtnProceed.Size = new System.Drawing.Size(86, 34);
            this.BtnProceed.TabIndex = 3;
            this.BtnProceed.Text = "Proceed";
            this.BtnProceed.UseVisualStyleBackColor = true;
            this.BtnProceed.Click += new System.EventHandler(this.BtnProceed_Click);
            // 
            // FrmDt_Modal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 452);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmDt_Modal";
            this.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
            this.Text = "Map data";
            this.Load += new System.EventHandler(this.FrmDt_Modal_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMap)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView DgvMap;
        private System.Windows.Forms.TextBox TxtUploadType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtMapDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtMapCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button BtnBack;
        private System.Windows.Forms.Button BtnProceed;
    }
}
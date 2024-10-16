namespace PresenterLayer
{
    partial class frmItemSelection
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbBPList = new System.Windows.Forms.PictureBox();
            this.pbColorList = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStyleCode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStyleDesc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtColorCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtColorDesc = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.pbSection = new System.Windows.Forms.PictureBox();
            this.txtSection = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBPList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbColorList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSection)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pbBPList);
            this.panel2.Controls.Add(this.pbColorList);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtStyleCode);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.txtStyleDesc);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtColorCode);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtColorDesc);
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.pbSection);
            this.panel2.Controls.Add(this.txtSection);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(20, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(668, 371);
            this.panel2.TabIndex = 222;
            // 
            // pbBPList
            // 
            this.pbBPList.BackColor = System.Drawing.Color.Transparent;
            this.pbBPList.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbBPList.Location = new System.Drawing.Point(225, 9);
            this.pbBPList.Name = "pbBPList";
            this.pbBPList.Size = new System.Drawing.Size(19, 15);
            this.pbBPList.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBPList.TabIndex = 154;
            this.pbBPList.TabStop = false;
            this.pbBPList.Click += new System.EventHandler(this.pbBPList_Click);
            // 
            // pbColorList
            // 
            this.pbColorList.BackColor = System.Drawing.Color.Transparent;
            this.pbColorList.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbColorList.Location = new System.Drawing.Point(225, 52);
            this.pbColorList.Name = "pbColorList";
            this.pbColorList.Size = new System.Drawing.Size(19, 15);
            this.pbColorList.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbColorList.TabIndex = 165;
            this.pbColorList.TabStop = false;
            this.pbColorList.Click += new System.EventHandler(this.pbColorList_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label2.Location = new System.Drawing.Point(7, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 150;
            this.label2.Text = "Style Code";
            // 
            // txtStyleCode
            // 
            this.txtStyleCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtStyleCode.Location = new System.Drawing.Point(124, 8);
            this.txtStyleCode.Name = "txtStyleCode";
            this.txtStyleCode.ReadOnly = true;
            this.txtStyleCode.Size = new System.Drawing.Size(123, 18);
            this.txtStyleCode.TabIndex = 151;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label8.Location = new System.Drawing.Point(7, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 152;
            this.label8.Text = "Style Description";
            // 
            // txtStyleDesc
            // 
            this.txtStyleDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtStyleDesc.Location = new System.Drawing.Point(124, 29);
            this.txtStyleDesc.Name = "txtStyleDesc";
            this.txtStyleDesc.ReadOnly = true;
            this.txtStyleDesc.Size = new System.Drawing.Size(123, 18);
            this.txtStyleDesc.TabIndex = 153;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label3.Location = new System.Drawing.Point(7, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 155;
            this.label3.Text = "Color Code";
            // 
            // txtColorCode
            // 
            this.txtColorCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtColorCode.Location = new System.Drawing.Point(124, 50);
            this.txtColorCode.Name = "txtColorCode";
            this.txtColorCode.ReadOnly = true;
            this.txtColorCode.Size = new System.Drawing.Size(123, 18);
            this.txtColorCode.TabIndex = 156;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label1.Location = new System.Drawing.Point(7, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 157;
            this.label1.Text = "Color Description";
            // 
            // txtColorDesc
            // 
            this.txtColorDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtColorDesc.Location = new System.Drawing.Point(124, 71);
            this.txtColorDesc.Name = "txtColorDesc";
            this.txtColorDesc.ReadOnly = true;
            this.txtColorDesc.Size = new System.Drawing.Size(123, 18);
            this.txtColorDesc.TabIndex = 158;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(7, 95);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(659, 241);
            this.dataGridView1.TabIndex = 160;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(6, 342);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(74, 21);
            this.btnAdd.TabIndex = 161;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(86, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 21);
            this.btnCancel.TabIndex = 162;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label6.Location = new System.Drawing.Point(459, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 164;
            this.label6.Text = "Section";
            // 
            // pbSection
            // 
            this.pbSection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSection.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbSection.Location = new System.Drawing.Point(644, 11);
            this.pbSection.Name = "pbSection";
            this.pbSection.Size = new System.Drawing.Size(19, 15);
            this.pbSection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSection.TabIndex = 173;
            this.pbSection.TabStop = false;
            this.pbSection.Click += new System.EventHandler(this.pbSection_Click);
            // 
            // txtSection
            // 
            this.txtSection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtSection.Location = new System.Drawing.Point(543, 9);
            this.txtSection.Name = "txtSection";
            this.txtSection.ReadOnly = true;
            this.txtSection.Size = new System.Drawing.Size(123, 18);
            this.txtSection.TabIndex = 170;
            // 
            // frmItemSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 451);
            this.Controls.Add(this.panel2);
            this.Name = "frmItemSelection";
            this.Text = "Item Selection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmItemSelection_FormClosing);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBPList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbColorList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pbBPList;
        private System.Windows.Forms.PictureBox pbColorList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStyleCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStyleDesc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtColorCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtColorDesc;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pbSection;
        private System.Windows.Forms.TextBox txtSection;
    }
}
namespace DirecLayer
{
    partial class frmDefineNew
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
            this.btnCommand = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.gvDefine = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvDefine)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCommand
            // 
            this.btnCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCommand.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCommand.Image = null;
            this.btnCommand.Location = new System.Drawing.Point(3, 235);
            this.btnCommand.Name = "btnCommand";
            this.btnCommand.Size = new System.Drawing.Size(146, 27);
            this.btnCommand.TabIndex = 59;
            this.btnCommand.Text = "&OK";
            this.btnCommand.UseSelectable = true;
            this.btnCommand.UseVisualStyleBackColor = true;
            this.btnCommand.Click += new System.EventHandler(this.btnCommand_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Location = new System.Drawing.Point(155, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(146, 27);
            this.btnCancel.TabIndex = 60;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gvDefine
            // 
            this.gvDefine.AllowUserToDeleteRows = false;
            this.gvDefine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvDefine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDefine.Location = new System.Drawing.Point(3, 3);
            this.gvDefine.Name = "gvDefine";
            this.gvDefine.Size = new System.Drawing.Size(554, 226);
            this.gvDefine.TabIndex = 58;
            this.gvDefine.Tag = "Colors";
            this.gvDefine.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvDefine_CellEndEdit);
            this.gvDefine.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvDefine_CellMouseDown);
            this.gvDefine.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvDefine_ColumnHeaderMouseClick);
            this.gvDefine.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.gvDefine_SortCompare);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvDefine);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnCommand);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 270);
            this.panel1.TabIndex = 61;
            // 
            // frmDefineNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(600, 350);
            this.Controls.Add(this.panel1);
            this.Name = "frmDefineNew";
            ((System.ComponentModel.ISupportInitialize)(this.gvDefine)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public MetroFramework.Controls.MetroTextBox.MetroTextButton btnCommand;
        private MetroFramework.Controls.MetroButton btnCancel;
        private System.Windows.Forms.DataGridView gvDefine;
        private System.Windows.Forms.Panel panel1;
    }
}
namespace PresenterLayer.Views
{
    partial class frmItems
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearchItem = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gvItems = new System.Windows.Forms.DataGridView();
            this.btnCommand = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSearchItem);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.gvItems);
            this.panel1.Controls.Add(this.btnCommand);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(916, 542);
            this.panel1.TabIndex = 54;
            // 
            // txtSearchItem
            // 
            this.txtSearchItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchItem.Location = new System.Drawing.Point(107, 6);
            this.txtSearchItem.Name = "txtSearchItem";
            this.txtSearchItem.Size = new System.Drawing.Size(630, 23);
            this.txtSearchItem.TabIndex = 53;
            this.txtSearchItem.TextChanged += new System.EventHandler(this.txtSearchItem_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 16);
            this.label4.TabIndex = 52;
            this.label4.Text = "Search Item :";
            // 
            // gvItems
            // 
            this.gvItems.AllowUserToAddRows = false;
            this.gvItems.AllowUserToDeleteRows = false;
            this.gvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvItems.Location = new System.Drawing.Point(6, 35);
            this.gvItems.Name = "gvItems";
            this.gvItems.Size = new System.Drawing.Size(907, 463);
            this.gvItems.TabIndex = 11;
            this.gvItems.Tag = "Colors";
            this.gvItems.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvItems_CellEndEdit);
            this.gvItems.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvItems_ColumnHeaderMouseClick);
            this.gvItems.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.gvItems_PreviewKeyDown);
            // 
            // btnCommand
            // 
            this.btnCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCommand.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCommand.Image = null;
            this.btnCommand.Location = new System.Drawing.Point(6, 504);
            this.btnCommand.Name = "btnCommand";
            this.btnCommand.Size = new System.Drawing.Size(146, 27);
            this.btnCommand.TabIndex = 51;
            this.btnCommand.Text = "&OK";
            this.btnCommand.UseSelectable = true;
            this.btnCommand.UseVisualStyleBackColor = true;
            this.btnCommand.Click += new System.EventHandler(this.btnCommand_Click);
            // 
            // frmItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(936, 612);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(749, 457);
            this.Name = "frmItems";
            this.Padding = new System.Windows.Forms.Padding(10, 60, 10, 10);
            this.Text = "Items";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmItems_FormClosing);
            this.Load += new System.EventHandler(this.frmItems_Load);
            this.Resize += new System.EventHandler(this.frmItems_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public MetroFramework.Controls.MetroTextBox.MetroTextButton btnCommand;
        private System.Windows.Forms.DataGridView gvItems;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSearchItem;
        private System.Windows.Forms.Label label4;
    }
}
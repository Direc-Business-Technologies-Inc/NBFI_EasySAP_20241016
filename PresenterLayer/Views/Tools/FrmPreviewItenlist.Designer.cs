namespace PresenterLayer.Views
{
    partial class FrmPreviewItenlist
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnClose = new System.Windows.Forms.Button();
            this.TxtSearchBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DgvItems = new System.Windows.Forms.DataGridView();
            this.msItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvItems)).BeginInit();
            this.msItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnClose);
            this.panel1.Controls.Add(this.TxtSearchBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.DgvItems);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 629);
            this.panel1.TabIndex = 1;
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.BtnClose.ForeColor = System.Drawing.Color.White;
            this.BtnClose.Location = new System.Drawing.Point(675, 584);
            this.BtnClose.Margin = new System.Windows.Forms.Padding(4);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(99, 29);
            this.BtnClose.TabIndex = 318;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = false;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // TxtSearchBox
            // 
            this.TxtSearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtSearchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.TxtSearchBox.Location = new System.Drawing.Point(119, 22);
            this.TxtSearchBox.Name = "TxtSearchBox";
            this.TxtSearchBox.Size = new System.Drawing.Size(655, 24);
            this.TxtSearchBox.TabIndex = 2;
            this.TxtSearchBox.TextChanged += new System.EventHandler(this.TxtSearchBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search :";
            // 
            // DgvItems
            // 
            this.DgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvItems.BackgroundColor = System.Drawing.Color.White;
            this.DgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvItems.Location = new System.Drawing.Point(12, 74);
            this.DgvItems.Name = "DgvItems";
            this.DgvItems.RowTemplate.Height = 24;
            this.DgvItems.Size = new System.Drawing.Size(762, 503);
            this.DgvItems.TabIndex = 0;
            this.DgvItems.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvItems_ColumnHeaderMouseClick);
            this.DgvItems.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvItems_RowHeaderMouseClick);
            // 
            // msItems
            // 
            this.msItems.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteItemsToolStripMenuItem});
            this.msItems.Name = "msItems";
            this.msItems.Size = new System.Drawing.Size(167, 30);
            // 
            // deleteItemsToolStripMenuItem
            // 
            this.deleteItemsToolStripMenuItem.Image = global::PresenterLayer.Properties.Resources.close;
            this.deleteItemsToolStripMenuItem.Name = "deleteItemsToolStripMenuItem";
            this.deleteItemsToolStripMenuItem.Size = new System.Drawing.Size(166, 26);
            this.deleteItemsToolStripMenuItem.Text = "Delete Items";
            this.deleteItemsToolStripMenuItem.Click += new System.EventHandler(this.deleteItemsToolStripMenuItem_Click);
            // 
            // FrmPreviewItenlist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 709);
            this.Controls.Add(this.panel1);
            this.Name = "FrmPreviewItenlist";
            this.Text = "Preview Item list";
            this.Load += new System.EventHandler(this.FrmPreviewItenlist_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvItems)).EndInit();
            this.msItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.TextBox TxtSearchBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip msItems;
        private System.Windows.Forms.ToolStripMenuItem deleteItemsToolStripMenuItem;
        public System.Windows.Forms.DataGridView DgvItems;
    }
}
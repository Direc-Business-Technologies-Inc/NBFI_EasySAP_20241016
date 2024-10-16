namespace PresenterLayer.Views
{
    partial class UcMainMenu
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnCreateTemplate = new System.Windows.Forms.Button();
            this.msItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DgvMapList = new System.Windows.Forms.DataGridView();
            this.msItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMapList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(10, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Template map";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Location = new System.Drawing.Point(10, 50);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 4);
            this.panel1.TabIndex = 2;
            // 
            // BtnCreateTemplate
            // 
            this.BtnCreateTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCreateTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCreateTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.BtnCreateTemplate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnCreateTemplate.Location = new System.Drawing.Point(712, 27);
            this.BtnCreateTemplate.Margin = new System.Windows.Forms.Padding(2, 2, 24, 32);
            this.BtnCreateTemplate.Name = "BtnCreateTemplate";
            this.BtnCreateTemplate.Size = new System.Drawing.Size(136, 37);
            this.BtnCreateTemplate.TabIndex = 3;
            this.BtnCreateTemplate.Text = "New Mapping";
            this.BtnCreateTemplate.UseVisualStyleBackColor = true;
            this.BtnCreateTemplate.Click += new System.EventHandler(this.BtnCreateTemplate_Click);
            // 
            // msItems
            // 
            this.msItems.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteItemsToolStripMenuItem});
            this.msItems.Name = "msItems";
            this.msItems.Size = new System.Drawing.Size(157, 52);
            this.msItems.Opening += new System.ComponentModel.CancelEventHandler(this.msItems_Opening);
            // 
            // deleteItemsToolStripMenuItem
            // 
            this.deleteItemsToolStripMenuItem.Image = global::PresenterLayer.Properties.Resources.close;
            this.deleteItemsToolStripMenuItem.Name = "deleteItemsToolStripMenuItem";
            this.deleteItemsToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.deleteItemsToolStripMenuItem.Text = "Delete Items";
            this.deleteItemsToolStripMenuItem.Click += new System.EventHandler(this.deleteItemsToolStripMenuItem_Click);
            // 
            // DgvMapList
            // 
            this.DgvMapList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvMapList.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.DgvMapList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvMapList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMapList.Location = new System.Drawing.Point(10, 78);
            this.DgvMapList.Margin = new System.Windows.Forms.Padding(2);
            this.DgvMapList.Name = "DgvMapList";
            this.DgvMapList.ReadOnly = true;
            this.DgvMapList.Size = new System.Drawing.Size(838, 438);
            this.DgvMapList.TabIndex = 4;
            this.DgvMapList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvMapList_CellClick);
            this.DgvMapList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvMapList_CellDoubleClick);
            this.DgvMapList.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvMapList_RowHeaderMouseClick);
            // 
            // UcMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.DgvMapList);
            this.Controls.Add(this.BtnCreateTemplate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UcMainMenu";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(858, 527);
            this.Load += new System.EventHandler(this.UcMainMenu_Load);
            this.msItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMapList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnCreateTemplate;
        private System.Windows.Forms.ContextMenuStrip msItems;
        private System.Windows.Forms.ToolStripMenuItem deleteItemsToolStripMenuItem;
        private System.Windows.Forms.DataGridView DgvMapList;
    }
}

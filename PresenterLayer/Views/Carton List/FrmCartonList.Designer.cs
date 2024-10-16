namespace PresenterLayer.Views
{
    partial class FrmCartonList
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
            this.TabPreviewItem = new System.Windows.Forms.TabControl();
            this.TabNewDocs = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TxtDocEntry = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnAddCartonList = new System.Windows.Forms.Button();
            this.TxtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnRequest = new System.Windows.Forms.Button();
            this.DgvItem = new System.Windows.Forms.DataGridView();
            this.TxtRemarks = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TabFindDocument = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.BtnChoose = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.DgvFindDocument = new System.Windows.Forms.DataGridView();
            this.TxtSearchDocument = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.BtnNewDocument = new System.Windows.Forms.Button();
            this.msItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabPreviewItem.SuspendLayout();
            this.TabNewDocs.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvItem)).BeginInit();
            this.TabFindDocument.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvFindDocument)).BeginInit();
            this.msItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabPreviewItem
            // 
            this.TabPreviewItem.Controls.Add(this.TabNewDocs);
            this.TabPreviewItem.Controls.Add(this.TabFindDocument);
            this.TabPreviewItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabPreviewItem.Location = new System.Drawing.Point(20, 75);
            this.TabPreviewItem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabPreviewItem.Name = "TabPreviewItem";
            this.TabPreviewItem.SelectedIndex = 0;
            this.TabPreviewItem.Size = new System.Drawing.Size(1005, 693);
            this.TabPreviewItem.TabIndex = 1;
            this.TabPreviewItem.SelectedIndexChanged += new System.EventHandler(this.TabPreviewItem_SelectedIndexChanged);
            // 
            // TabNewDocs
            // 
            this.TabNewDocs.Controls.Add(this.panel1);
            this.TabNewDocs.Location = new System.Drawing.Point(4, 25);
            this.TabNewDocs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabNewDocs.Name = "TabNewDocs";
            this.TabNewDocs.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabNewDocs.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TabNewDocs.Size = new System.Drawing.Size(997, 664);
            this.TabNewDocs.TabIndex = 0;
            this.TabNewDocs.Text = "Document";
            this.TabNewDocs.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TxtDocEntry);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.BtnAddCartonList);
            this.panel1.Controls.Add(this.TxtSearch);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BtnClose);
            this.panel1.Controls.Add(this.BtnRequest);
            this.panel1.Controls.Add(this.DgvItem);
            this.panel1.Controls.Add(this.TxtRemarks);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(991, 660);
            this.panel1.TabIndex = 0;
            // 
            // TxtDocEntry
            // 
            this.TxtDocEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtDocEntry.Location = new System.Drawing.Point(149, 89);
            this.TxtDocEntry.Margin = new System.Windows.Forms.Padding(4);
            this.TxtDocEntry.Name = "TxtDocEntry";
            this.TxtDocEntry.ReadOnly = true;
            this.TxtDocEntry.Size = new System.Drawing.Size(160, 28);
            this.TxtDocEntry.TabIndex = 424;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label2.Location = new System.Drawing.Point(15, 95);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 18);
            this.label2.TabIndex = 423;
            this.label2.Text = "Document Entry";
            // 
            // BtnAddCartonList
            // 
            this.BtnAddCartonList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAddCartonList.BackColor = System.Drawing.Color.White;
            this.BtnAddCartonList.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.BtnAddCartonList.FlatAppearance.BorderSize = 3;
            this.BtnAddCartonList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddCartonList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.BtnAddCartonList.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnAddCartonList.Location = new System.Drawing.Point(827, 84);
            this.BtnAddCartonList.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAddCartonList.Name = "BtnAddCartonList";
            this.BtnAddCartonList.Size = new System.Drawing.Size(147, 33);
            this.BtnAddCartonList.TabIndex = 422;
            this.BtnAddCartonList.Text = "Add Carton";
            this.BtnAddCartonList.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnAddCartonList.UseVisualStyleBackColor = false;
            this.BtnAddCartonList.Click += new System.EventHandler(this.BtnAddCartonList_Click);
            // 
            // TxtSearch
            // 
            this.TxtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TxtSearch.Location = new System.Drawing.Point(149, 31);
            this.TxtSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtSearch.Name = "TxtSearch";
            this.TxtSearch.Size = new System.Drawing.Size(321, 30);
            this.TxtSearch.TabIndex = 421;
            this.TxtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(13, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 29);
            this.label1.TabIndex = 420;
            this.label1.Text = "Search";
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnClose.BackColor = System.Drawing.Color.White;
            this.BtnClose.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BtnClose.FlatAppearance.BorderSize = 3;
            this.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.BtnClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnClose.Location = new System.Drawing.Point(119, 608);
            this.BtnClose.Margin = new System.Windows.Forms.Padding(4);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(99, 33);
            this.BtnClose.TabIndex = 419;
            this.BtnClose.Text = "Close";
            this.BtnClose.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnClose.UseVisualStyleBackColor = false;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnRequest
            // 
            this.BtnRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnRequest.BackColor = System.Drawing.Color.White;
            this.BtnRequest.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.BtnRequest.FlatAppearance.BorderSize = 3;
            this.BtnRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.BtnRequest.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnRequest.Location = new System.Drawing.Point(13, 608);
            this.BtnRequest.Margin = new System.Windows.Forms.Padding(4);
            this.BtnRequest.Name = "BtnRequest";
            this.BtnRequest.Size = new System.Drawing.Size(99, 33);
            this.BtnRequest.TabIndex = 418;
            this.BtnRequest.Text = "Add";
            this.BtnRequest.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnRequest.UseVisualStyleBackColor = false;
            this.BtnRequest.Click += new System.EventHandler(this.BtnRequest_Click);
            // 
            // DgvItem
            // 
            this.DgvItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvItem.BackgroundColor = System.Drawing.Color.White;
            this.DgvItem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DgvItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvItem.Location = new System.Drawing.Point(13, 132);
            this.DgvItem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvItem.Name = "DgvItem";
            this.DgvItem.ReadOnly = true;
            this.DgvItem.RowTemplate.Height = 24;
            this.DgvItem.Size = new System.Drawing.Size(962, 338);
            this.DgvItem.TabIndex = 417;
            this.DgvItem.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvItem_CellClick);
            this.DgvItem.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvItem_CellDoubleClick);
            this.DgvItem.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvItem_ColumnHeaderMouseClick);
            this.DgvItem.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvItem_RowHeaderMouseClick);
            // 
            // TxtRemarks
            // 
            this.TxtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TxtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtRemarks.Location = new System.Drawing.Point(13, 494);
            this.TxtRemarks.Margin = new System.Windows.Forms.Padding(4);
            this.TxtRemarks.Multiline = true;
            this.TxtRemarks.Name = "TxtRemarks";
            this.TxtRemarks.Size = new System.Drawing.Size(519, 106);
            this.TxtRemarks.TabIndex = 414;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label10.Location = new System.Drawing.Point(15, 473);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 18);
            this.label10.TabIndex = 413;
            this.label10.Text = "Remark";
            // 
            // TabFindDocument
            // 
            this.TabFindDocument.Controls.Add(this.panel6);
            this.TabFindDocument.Location = new System.Drawing.Point(4, 25);
            this.TabFindDocument.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabFindDocument.Name = "TabFindDocument";
            this.TabFindDocument.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabFindDocument.Size = new System.Drawing.Size(997, 664);
            this.TabFindDocument.TabIndex = 2;
            this.TabFindDocument.Text = "Find Document";
            this.TabFindDocument.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.BtnChoose);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Controls.Add(this.label20);
            this.panel6.Controls.Add(this.DgvFindDocument);
            this.panel6.Controls.Add(this.TxtSearchDocument);
            this.panel6.Controls.Add(this.label21);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 2);
            this.panel6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(991, 660);
            this.panel6.TabIndex = 420;
            // 
            // BtnChoose
            // 
            this.BtnChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnChoose.BackColor = System.Drawing.Color.White;
            this.BtnChoose.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.BtnChoose.FlatAppearance.BorderSize = 3;
            this.BtnChoose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnChoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.BtnChoose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnChoose.Location = new System.Drawing.Point(19, 610);
            this.BtnChoose.Margin = new System.Windows.Forms.Padding(4);
            this.BtnChoose.Name = "BtnChoose";
            this.BtnChoose.Size = new System.Drawing.Size(107, 34);
            this.BtnChoose.TabIndex = 425;
            this.BtnChoose.Text = "Choose";
            this.BtnChoose.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnChoose.UseVisualStyleBackColor = false;
            this.BtnChoose.Click += new System.EventHandler(this.BtnChoose_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Silver;
            this.panel5.Location = new System.Drawing.Point(20, 66);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(380, 2);
            this.panel5.TabIndex = 424;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label20.Location = new System.Drawing.Point(15, 95);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(75, 25);
            this.label20.TabIndex = 423;
            this.label20.Text = "Search";
            // 
            // DgvFindDocument
            // 
            this.DgvFindDocument.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvFindDocument.BackgroundColor = System.Drawing.Color.White;
            this.DgvFindDocument.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DgvFindDocument.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvFindDocument.Location = new System.Drawing.Point(19, 146);
            this.DgvFindDocument.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvFindDocument.Name = "DgvFindDocument";
            this.DgvFindDocument.ReadOnly = true;
            this.DgvFindDocument.RowTemplate.Height = 24;
            this.DgvFindDocument.Size = new System.Drawing.Size(955, 441);
            this.DgvFindDocument.TabIndex = 422;
            this.DgvFindDocument.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvFindDocument_CellClick);
            this.DgvFindDocument.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvFindDocument_CellDoubleClick);
            this.DgvFindDocument.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvFindDocument_ColumnHeaderMouseClick);
            // 
            // TxtSearchDocument
            // 
            this.TxtSearchDocument.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtSearchDocument.Location = new System.Drawing.Point(125, 94);
            this.TxtSearchDocument.Margin = new System.Windows.Forms.Padding(4);
            this.TxtSearchDocument.Name = "TxtSearchDocument";
            this.TxtSearchDocument.Size = new System.Drawing.Size(275, 28);
            this.TxtSearchDocument.TabIndex = 421;
            this.TxtSearchDocument.TextChanged += new System.EventHandler(this.TxtSearchDocument_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label21.Location = new System.Drawing.Point(11, 25);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(248, 39);
            this.label21.TabIndex = 420;
            this.label21.Text = "Find Document";
            // 
            // BtnNewDocument
            // 
            this.BtnNewDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnNewDocument.BackColor = System.Drawing.Color.White;
            this.BtnNewDocument.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.BtnNewDocument.FlatAppearance.BorderSize = 3;
            this.BtnNewDocument.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnNewDocument.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.BtnNewDocument.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnNewDocument.Location = new System.Drawing.Point(853, 60);
            this.BtnNewDocument.Margin = new System.Windows.Forms.Padding(4);
            this.BtnNewDocument.Name = "BtnNewDocument";
            this.BtnNewDocument.Size = new System.Drawing.Size(147, 33);
            this.BtnNewDocument.TabIndex = 417;
            this.BtnNewDocument.Text = "New Document";
            this.BtnNewDocument.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnNewDocument.UseVisualStyleBackColor = false;
            this.BtnNewDocument.Click += new System.EventHandler(this.BtnNewDocument_Click);
            // 
            // msItems
            // 
            this.msItems.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteItemsToolStripMenuItem});
            this.msItems.Name = "msItems";
            this.msItems.Size = new System.Drawing.Size(163, 28);
            // 
            // deleteItemsToolStripMenuItem
            // 
            this.deleteItemsToolStripMenuItem.Name = "deleteItemsToolStripMenuItem";
            this.deleteItemsToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
            this.deleteItemsToolStripMenuItem.Text = "Delete Items";
            this.deleteItemsToolStripMenuItem.Click += new System.EventHandler(this.deleteItemsToolStripMenuItem_Click);
            // 
            // FrmCartonList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 788);
            this.Controls.Add(this.BtnNewDocument);
            this.Controls.Add(this.TabPreviewItem);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmCartonList";
            this.Padding = new System.Windows.Forms.Padding(20, 75, 20, 20);
            this.Text = "Carton List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCartonList_FormClosing);
            this.Load += new System.EventHandler(this.FrmCartonList_Load);
            this.Resize += new System.EventHandler(this.FrmCartonList_Resize);
            this.TabPreviewItem.ResumeLayout(false);
            this.TabNewDocs.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvItem)).EndInit();
            this.TabFindDocument.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvFindDocument)).EndInit();
            this.msItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabPreviewItem;
        private System.Windows.Forms.TabPage TabNewDocs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnRequest;
        private System.Windows.Forms.DataGridView DgvItem;
        private System.Windows.Forms.TextBox TxtRemarks;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage TabFindDocument;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button BtnChoose;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DataGridView DgvFindDocument;
        private System.Windows.Forms.TextBox TxtSearchDocument;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox TxtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnAddCartonList;
        private System.Windows.Forms.Button BtnNewDocument;
        private System.Windows.Forms.TextBox TxtDocEntry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip msItems;
        private System.Windows.Forms.ToolStripMenuItem deleteItemsToolStripMenuItem;
    }
}
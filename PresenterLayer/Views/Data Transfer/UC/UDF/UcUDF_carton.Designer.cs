namespace PresenterLayer.Views
{
    partial class UcUDF_carton
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTransactionType = new System.Windows.Forms.TextBox();
            this.pbTransactionType = new System.Windows.Forms.PictureBox();
            this.pbTargetWarehouse = new System.Windows.Forms.PictureBox();
            this.txtTargetWarehouse = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pbLastWarehouse = new System.Windows.Forms.PictureBox();
            this.txtLastWarehouse = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbTransactionType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTargetWarehouse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLastWarehouse)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 175;
            this.label1.Text = "Transaction Type";
            // 
            // txtTransactionType
            // 
            this.txtTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTransactionType.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtTransactionType.Location = new System.Drawing.Point(148, 13);
            this.txtTransactionType.Margin = new System.Windows.Forms.Padding(4);
            this.txtTransactionType.Multiline = true;
            this.txtTransactionType.Name = "txtTransactionType";
            this.txtTransactionType.ReadOnly = true;
            this.txtTransactionType.Size = new System.Drawing.Size(225, 23);
            this.txtTransactionType.TabIndex = 176;
            // 
            // pbTransactionType
            // 
            this.pbTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTransactionType.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbTransactionType.Location = new System.Drawing.Point(345, 15);
            this.pbTransactionType.Margin = new System.Windows.Forms.Padding(4);
            this.pbTransactionType.Name = "pbTransactionType";
            this.pbTransactionType.Size = new System.Drawing.Size(25, 18);
            this.pbTransactionType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTransactionType.TabIndex = 177;
            this.pbTransactionType.TabStop = false;
            this.pbTransactionType.Click += new System.EventHandler(this.pbTransactionType_Click);
            // 
            // pbTargetWarehouse
            // 
            this.pbTargetWarehouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTargetWarehouse.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbTargetWarehouse.Location = new System.Drawing.Point(345, 46);
            this.pbTargetWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.pbTargetWarehouse.Name = "pbTargetWarehouse";
            this.pbTargetWarehouse.Size = new System.Drawing.Size(25, 18);
            this.pbTargetWarehouse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTargetWarehouse.TabIndex = 180;
            this.pbTargetWarehouse.TabStop = false;
            this.pbTargetWarehouse.Click += new System.EventHandler(this.pbTargetWarehouse_Click);
            // 
            // txtTargetWarehouse
            // 
            this.txtTargetWarehouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTargetWarehouse.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtTargetWarehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtTargetWarehouse.Location = new System.Drawing.Point(148, 44);
            this.txtTargetWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.txtTargetWarehouse.Multiline = true;
            this.txtTargetWarehouse.Name = "txtTargetWarehouse";
            this.txtTargetWarehouse.ReadOnly = true;
            this.txtTargetWarehouse.Size = new System.Drawing.Size(225, 23);
            this.txtTargetWarehouse.TabIndex = 179;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label2.Location = new System.Drawing.Point(13, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 15);
            this.label2.TabIndex = 178;
            this.label2.Text = "Target Warehouse";
            // 
            // pbLastWarehouse
            // 
            this.pbLastWarehouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLastWarehouse.Image = global::PresenterLayer.Properties.Resources.signs;
            this.pbLastWarehouse.Location = new System.Drawing.Point(345, 74);
            this.pbLastWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.pbLastWarehouse.Name = "pbLastWarehouse";
            this.pbLastWarehouse.Size = new System.Drawing.Size(25, 18);
            this.pbLastWarehouse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLastWarehouse.TabIndex = 183;
            this.pbLastWarehouse.TabStop = false;
            this.pbLastWarehouse.Click += new System.EventHandler(this.pbLastWarehouse_Click);
            // 
            // txtLastWarehouse
            // 
            this.txtLastWarehouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLastWarehouse.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtLastWarehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.txtLastWarehouse.Location = new System.Drawing.Point(148, 72);
            this.txtLastWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastWarehouse.Multiline = true;
            this.txtLastWarehouse.Name = "txtLastWarehouse";
            this.txtLastWarehouse.ReadOnly = true;
            this.txtLastWarehouse.Size = new System.Drawing.Size(225, 23);
            this.txtLastWarehouse.TabIndex = 182;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label3.Location = new System.Drawing.Point(13, 75);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 15);
            this.label3.TabIndex = 181;
            this.label3.Text = "Last Warehouse";
            // 
            // UcUDF_carton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pbLastWarehouse);
            this.Controls.Add(this.txtLastWarehouse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pbTargetWarehouse);
            this.Controls.Add(this.txtTargetWarehouse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pbTransactionType);
            this.Controls.Add(this.txtTransactionType);
            this.Controls.Add(this.label1);
            this.Name = "UcUDF_carton";
            this.Size = new System.Drawing.Size(381, 194);
            ((System.ComponentModel.ISupportInitialize)(this.pbTransactionType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTargetWarehouse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLastWarehouse)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtTransactionType;
        private System.Windows.Forms.PictureBox pbTransactionType;
        private System.Windows.Forms.PictureBox pbTargetWarehouse;
        public System.Windows.Forms.TextBox txtTargetWarehouse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pbLastWarehouse;
        public System.Windows.Forms.TextBox txtLastWarehouse;
        private System.Windows.Forms.Label label3;
    }
}

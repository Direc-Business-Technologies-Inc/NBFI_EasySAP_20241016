namespace PresenterLayer
{
    partial class frmpackinglist_udf
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gvUDF = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gvUDF)).BeginInit();
            this.SuspendLayout();
            // 
            // gvUDF
            // 
            this.gvUDF.AllowUserToAddRows = false;
            this.gvUDF.AllowUserToDeleteRows = false;
            this.gvUDF.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gvUDF.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvUDF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvUDF.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvUDF.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvUDF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvUDF.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvUDF.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gvUDF.Location = new System.Drawing.Point(20, 60);
            this.gvUDF.Margin = new System.Windows.Forms.Padding(4);
            this.gvUDF.MultiSelect = false;
            this.gvUDF.Name = "gvUDF";
            this.gvUDF.ReadOnly = true;
            this.gvUDF.RowHeadersVisible = false;
            this.gvUDF.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gvUDF.Size = new System.Drawing.Size(387, 565);
            this.gvUDF.TabIndex = 3;
            // 
            // frmpackinglist_udf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 645);
            this.Controls.Add(this.gvUDF);
            this.Name = "frmpackinglist_udf";
            this.Load += new System.EventHandler(this.frmpackinglist_udf_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvUDF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView gvUDF;
    }
}
namespace PresenterLayer.Tools
{
    partial class frm_UDF
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Category = new System.Windows.Forms.ComboBox();
            this.dgvUDF = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUDF)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Category, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvUDF, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 30);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.880239F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.11976F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(261, 334);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Category
            // 
            this.Category.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Category.FormattingEnabled = true;
            this.Category.Location = new System.Drawing.Point(3, 3);
            this.Category.Name = "Category";
            this.Category.Size = new System.Drawing.Size(255, 24);
            this.Category.TabIndex = 4;
            // 
            // dgvUDF
            // 
            this.dgvUDF.AllowUserToAddRows = false;
            this.dgvUDF.AllowUserToDeleteRows = false;
            this.dgvUDF.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvUDF.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUDF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUDF.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUDF.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUDF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUDF.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvUDF.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvUDF.Location = new System.Drawing.Point(4, 36);
            this.dgvUDF.Margin = new System.Windows.Forms.Padding(4);
            this.dgvUDF.MultiSelect = false;
            this.dgvUDF.Name = "dgvUDF";
            this.dgvUDF.ReadOnly = true;
            this.dgvUDF.RowHeadersVisible = false;
            this.dgvUDF.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvUDF.Size = new System.Drawing.Size(253, 294);
            this.dgvUDF.TabIndex = 2;
            this.dgvUDF.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUDF_CellClick);
            this.dgvUDF.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUDF_CellContentClick);
            // 
            // frm_UDF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 369);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DisplayHeader = false;
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Movable = false;
            this.Name = "frm_UDF";
            this.Padding = new System.Windows.Forms.Padding(5, 30, 5, 5);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_UDF_FormClosing);
            this.Load += new System.EventHandler(this.frm_UDF_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUDF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.DataGridView dgvUDF;
        private System.Windows.Forms.ComboBox Category;
    }
}
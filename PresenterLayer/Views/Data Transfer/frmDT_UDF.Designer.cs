namespace PresenterLayer.Views
{
    partial class frmDT_UDF
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
            this.gvUDF = new System.Windows.Forms.DataGridView();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUDF)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.gvUDF, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnlContainer, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 60);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.39823F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.60177F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(290, 448);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gvUDF
            // 
            this.gvUDF.AllowUserToAddRows = false;
            this.gvUDF.AllowUserToDeleteRows = false;
            this.gvUDF.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gvUDF.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvUDF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvUDF.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvUDF.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvUDF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvUDF.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvUDF.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gvUDF.Location = new System.Drawing.Point(3, 161);
            this.gvUDF.MultiSelect = false;
            this.gvUDF.Name = "gvUDF";
            this.gvUDF.ReadOnly = true;
            this.gvUDF.RowHeadersVisible = false;
            this.gvUDF.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gvUDF.Size = new System.Drawing.Size(284, 284);
            this.gvUDF.TabIndex = 5;
            this.gvUDF.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvUDF_CellClick);
            this.gvUDF.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvUDF_CellEndEdit_1);
            this.gvUDF.CurrentCellDirtyStateChanged += new System.EventHandler(this.GvUDF_CurrentCellDirtyStateChanged);
            this.gvUDF.Leave += new System.EventHandler(this.GvUDF_Leave);
            // 
            // pnlContainer
            // 
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(2, 2);
            this.pnlContainer.Margin = new System.Windows.Forms.Padding(2);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(286, 154);
            this.pnlContainer.TabIndex = 0;
            // 
            // frmDT_UDF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 524);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Movable = false;
            this.Name = "frmDT_UDF";
            this.Padding = new System.Windows.Forms.Padding(15, 60, 15, 16);
            this.Resizable = false;
            this.Load += new System.EventHandler(this.frmDT_UDF_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvUDF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlContainer;
        public System.Windows.Forms.DataGridView gvUDF;
    }
}
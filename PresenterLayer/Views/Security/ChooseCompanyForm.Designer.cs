using MetroFramework.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace PresenterLayer.Views.Security
{
    partial class ChooseCompanyForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseCompanyForm));
            this.panel1 = new Panel();
            this.btnOK = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.txtSearch = new MetroFramework.Controls.MetroTextBox();
            this.groupBox1 = new GroupBox();
            this.rbDatabase = new MetroFramework.Controls.MetroRadioButton();
            this.rbCompany = new MetroFramework.Controls.MetroRadioButton();
            this.dgvCompanies = new MetroFramework.Controls.MetroGrid();
            this.btnRefresh = new MetroFramework.Controls.MetroButton();
            this.btnChangeUser = new MetroFramework.Controls.MetroButton();
            this.txtPassword = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.txtDatabase = new MetroFramework.Controls.MetroTextBox();
            this.txtUserID = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.panel2 = new Panel();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompanies)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.dgvCompanies);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnChangeUser);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.metroLabel2);
            this.panel1.Controls.Add(this.txtDatabase);
            this.panel1.Controls.Add(this.txtUserID);
            this.panel1.Controls.Add(this.metroLabel5);
            this.panel1.Controls.Add(this.metroLabel3);
            this.panel1.Controls.Add(this.metroLabel1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(5, 60);
            this.panel1.Margin = new Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(590, 295);
            this.panel1.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Image = null;
            this.btnOK.Location = new Point(12, 269);
            this.btnOK.Margin = new Padding(5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(100, 22);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "&OK";
            this.btnOK.UseSelectable = true;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new Point(130, 269);
            this.btnCancel.Margin = new Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(100, 22);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtSearch.CustomButton.Image = null;
            this.txtSearch.CustomButton.Location = new Point(121, 2);
            this.txtSearch.CustomButton.Name = "";
            this.txtSearch.CustomButton.Size = new Size(17, 17);
            this.txtSearch.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtSearch.CustomButton.TabIndex = 1;
            this.txtSearch.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSearch.CustomButton.UseSelectable = true;
            this.txtSearch.CustomButton.Visible = false;
            this.txtSearch.Lines = new string[0];
            this.txtSearch.Location = new Point(446, 205);
            this.txtSearch.MaxLength = 32767;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.ScrollBars = ScrollBars.None;
            this.txtSearch.SelectedText = "";
            this.txtSearch.SelectionLength = 0;
            this.txtSearch.SelectionStart = 0;
            this.txtSearch.ShortcutsEnabled = true;
            this.txtSearch.Size = new Size(141, 22);
            this.txtSearch.TabIndex = 11;
            this.txtSearch.UseSelectable = true;
            this.txtSearch.WaterMarkColor = Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtSearch.WaterMarkFont = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Pixel);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rbDatabase);
            this.groupBox1.Controls.Add(this.rbCompany);
            this.groupBox1.Location = new Point(446, 119);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(141, 80);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Find By:";
            // 
            // rbDatabase
            // 
            this.rbDatabase.AutoSize = true;
            this.rbDatabase.Location = new Point(5, 46);
            this.rbDatabase.Name = "rbDatabase";
            this.rbDatabase.Size = new Size(106, 15);
            this.rbDatabase.TabIndex = 10;
            this.rbDatabase.Tag = "Database Name";
            this.rbDatabase.Text = "Data&base Name";
            this.rbDatabase.UseSelectable = true;
            this.rbDatabase.CheckedChanged += new System.EventHandler(this.rbDatabase_CheckedChanged);
            // 
            // rbCompany
            // 
            this.rbCompany.AutoSize = true;
            this.rbCompany.Checked = true;
            this.rbCompany.Location = new Point(5, 25);
            this.rbCompany.Name = "rbCompany";
            this.rbCompany.Size = new Size(110, 15);
            this.rbCompany.TabIndex = 9;
            this.rbCompany.TabStop = true;
            this.rbCompany.Tag = "Company Name";
            this.rbCompany.Text = "Com&pany Name";
            this.rbCompany.UseSelectable = true;
            this.rbCompany.CheckedChanged += new System.EventHandler(this.rbCompany_CheckedChanged);
            // 
            // dgvCompanies
            // 
            this.dgvCompanies.AllowUserToAddRows = false;
            this.dgvCompanies.AllowUserToDeleteRows = false;
            this.dgvCompanies.AllowUserToResizeRows = false;
            this.dgvCompanies.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.dgvCompanies.BackgroundColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvCompanies.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCompanies.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.dgvCompanies.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dgvCompanies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCompanies.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dgvCompanies.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCompanies.EnableHeadersVisualStyles = false;
            this.dgvCompanies.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            this.dgvCompanies.GridColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvCompanies.Location = new Point(12, 95);
            this.dgvCompanies.Margin = new Padding(5);
            this.dgvCompanies.Name = "dgvCompanies";
            this.dgvCompanies.ReadOnly = true;
            this.dgvCompanies.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            this.dgvCompanies.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCompanies.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCompanies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCompanies.Size = new Size(426, 164);
            this.dgvCompanies.TabIndex = 7;
            this.dgvCompanies.CellClick += new DataGridViewCellEventHandler(this.dgvCompanies_CellClick);
            this.dgvCompanies.SelectionChanged += new System.EventHandler(this.dgvCompanies_SelectionChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnRefresh.Location = new Point(446, 95);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(141, 22);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseSelectable = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnChangeUser
            // 
            this.btnChangeUser.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnChangeUser.Location = new Point(446, 8);
            this.btnChangeUser.Name = "btnChangeUser";
            this.btnChangeUser.Size = new Size(141, 24);
            this.btnChangeUser.TabIndex = 3;
            this.btnChangeUser.Text = "&Change User";
            this.btnChangeUser.UseSelectable = true;
            this.btnChangeUser.Click += new System.EventHandler(this.btnChangeUser_Click);
            // 
            // txtPassword
            // 
            // 
            // 
            // 
            this.txtPassword.CustomButton.Image = null;
            this.txtPassword.CustomButton.Location = new Point(88, 2);
            this.txtPassword.CustomButton.Name = "";
            this.txtPassword.CustomButton.Size = new Size(19, 19);
            this.txtPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtPassword.CustomButton.TabIndex = 1;
            this.txtPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtPassword.CustomButton.UseSelectable = true;
            this.txtPassword.CustomButton.Visible = false;
            this.txtPassword.Lines = new string[0];
            this.txtPassword.Location = new Point(331, 8);
            this.txtPassword.Margin = new Padding(2);
            this.txtPassword.MaxLength = 32767;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.ScrollBars = ScrollBars.None;
            this.txtPassword.SelectedText = "";
            this.txtPassword.SelectionLength = 0;
            this.txtPassword.SelectionStart = 0;
            this.txtPassword.ShortcutsEnabled = true;
            this.txtPassword.Size = new Size(110, 24);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSelectable = true;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.WaterMarkColor = Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPassword.WaterMarkFont = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Pixel);
            // 
            // metroLabel2
            // 
            this.metroLabel2.Location = new Point(235, 7);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new Size(91, 24);
            this.metroLabel2.TabIndex = 11;
            this.metroLabel2.Text = "Password";
            this.metroLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDatabase
            // 
            // 
            // 
            // 
            this.txtDatabase.CustomButton.Image = null;
            this.txtDatabase.CustomButton.Location = new Point(299, 2);
            this.txtDatabase.CustomButton.Name = "";
            this.txtDatabase.CustomButton.Size = new Size(19, 19);
            this.txtDatabase.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtDatabase.CustomButton.TabIndex = 1;
            this.txtDatabase.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtDatabase.CustomButton.UseSelectable = true;
            this.txtDatabase.CustomButton.Visible = false;
            this.txtDatabase.Lines = new string[0];
            this.txtDatabase.Location = new Point(120, 36);
            this.txtDatabase.Margin = new Padding(2);
            this.txtDatabase.MaxLength = 32767;
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.PasswordChar = '\0';
            this.txtDatabase.ReadOnly = true;
            this.txtDatabase.ScrollBars = ScrollBars.None;
            this.txtDatabase.SelectedText = "";
            this.txtDatabase.SelectionLength = 0;
            this.txtDatabase.SelectionStart = 0;
            this.txtDatabase.ShortcutsEnabled = true;
            this.txtDatabase.Size = new Size(321, 24);
            this.txtDatabase.TabIndex = 6;
            this.txtDatabase.UseSelectable = true;
            this.txtDatabase.WaterMarkColor = Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtDatabase.WaterMarkFont = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Pixel);
            // 
            // txtUserID
            // 
            // 
            // 
            // 
            this.txtUserID.CustomButton.Image = null;
            this.txtUserID.CustomButton.Location = new Point(88, 2);
            this.txtUserID.CustomButton.Name = "";
            this.txtUserID.CustomButton.Size = new Size(19, 19);
            this.txtUserID.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtUserID.CustomButton.TabIndex = 1;
            this.txtUserID.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtUserID.CustomButton.UseSelectable = true;
            this.txtUserID.CustomButton.Visible = false;
            this.txtUserID.Lines = new string[0];
            this.txtUserID.Location = new Point(120, 8);
            this.txtUserID.Margin = new Padding(2);
            this.txtUserID.MaxLength = 32767;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.PasswordChar = '\0';
            this.txtUserID.ScrollBars = ScrollBars.None;
            this.txtUserID.SelectedText = "";
            this.txtUserID.SelectionLength = 0;
            this.txtUserID.SelectionStart = 0;
            this.txtUserID.ShortcutsEnabled = true;
            this.txtUserID.Size = new Size(110, 24);
            this.txtUserID.TabIndex = 1;
            this.txtUserID.UseSelectable = true;
            this.txtUserID.WaterMarkColor = Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtUserID.WaterMarkFont = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Pixel);
            // 
            // metroLabel5
            // 
            this.metroLabel5.Location = new Point(12, 71);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new Size(560, 19);
            this.metroLabel5.TabIndex = 7;
            this.metroLabel5.Text = "Companies on Current Server";
            this.metroLabel5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // metroLabel3
            // 
            this.metroLabel3.Location = new Point(12, 41);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new Size(103, 19);
            this.metroLabel3.TabIndex = 6;
            this.metroLabel3.Text = "Database";
            this.metroLabel3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // metroLabel1
            // 
            this.metroLabel1.Location = new Point(12, 12);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new Size(103, 19);
            this.metroLabel1.TabIndex = 4;
            this.metroLabel1.Text = "User ID";
            this.metroLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = Color.Gray;
            this.panel2.Dock = DockStyle.Top;
            this.panel2.Location = new Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(590, 2);
            this.panel2.TabIndex = 3;
            // 
            // ChooseCompanyForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 370);
            this.Controls.Add(this.panel1);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new Size(650, 370);
            this.Name = "ChooseCompanyForm";
            this.Padding = new Padding(5, 60, 5, 15);
            this.Text = "Choose Company";
            this.Load += new System.EventHandler(this.ChooseCompanyForm_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompanies)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        public MetroTextBox.MetroTextButton btnOK;
        public MetroButton btnCancel;
        public MetroTextBox txtSearch;
        private GroupBox groupBox1;
        public MetroRadioButton rbDatabase;
        public MetroRadioButton rbCompany;
        public MetroGrid dgvCompanies;
        public MetroButton btnRefresh;
        public MetroButton btnChangeUser;
        public MetroTextBox txtPassword;
        private MetroLabel metroLabel2;
        public MetroTextBox txtDatabase;
        public MetroTextBox txtUserID;
        private MetroLabel metroLabel5;
        private MetroLabel metroLabel3;
        private MetroLabel metroLabel1;
        private Panel panel2;
    }
}
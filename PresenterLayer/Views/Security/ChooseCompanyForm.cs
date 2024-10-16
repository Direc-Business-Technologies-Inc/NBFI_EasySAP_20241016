using DomainLayer.Helper;
using DomainLayer.Models;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using PresenterLayer.Services.Security;
using PresenterLayer.Views.Main;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PresenterLayer.Views.Security
{
    public partial class ChooseCompanyForm : MetroForm
    {
        ChooseCompanyService _chooseCompanyService { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                if (StaticHelper._MainForm.menuStrip.Enabled)
                { Dispose(); }
            }
            else if (keyData == Keys.Home)
            { FormHelper.ShowForm(new SettingsForm()); }
            else if (keyData == Keys.Enter)
            { btnOK.PerformClick(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public ChooseCompanyForm()
        {
            InitializeComponent();
            _chooseCompanyService = new ChooseCompanyService();
            RegisterObjects();
        }

        private void RegisterObjects()
        {
            IChooseCompany._CompaniesDataGridView = dgvCompanies;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_chooseCompanyService.CancelClick())
            {
                Dispose();
            }
        }

        private void ChooseCompanyForm_Load(object sender, EventArgs e)
        {
            _chooseCompanyService.DataGridViewSetup();
        }

        private void ChangeUser()
        {
            txtUserID.Text = null;
            txtPassword.Text = null;
            txtUserID.Focus();
        }

        private void SelectionCompanyChange()
        {
            _chooseCompanyService.SelectionCompanyChange();
            txtDatabase.Text = IChooseCompany.CompanyName;
        }
        private void dgvCompanies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectionCompanyChange();
        }

        private void dgvCompanies_SelectionChanged(object sender, EventArgs e)
        {
            SelectionCompanyChange();
        }

        private void btnChangeUser_Click(object sender, EventArgs e)
        {
            ChangeUser();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _chooseCompanyService.DataGridViewSetup();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            IChooseCompany.Search = txtSearch.Text;
            _chooseCompanyService.SearchCompany();
            txtDatabase.Text = IChooseCompany.CompanyName;
        }

        private void rbCompany_CheckedChanged(object sender, EventArgs e)
        {
            IChooseCompany.ColumnSearch = 0;
        }

        private void rbDatabase_CheckedChanged(object sender, EventArgs e)
        {
            IChooseCompany.ColumnSearch = 1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            EasySAPCredentialsModel.ESUserId = txtUserID.Text;
            EasySAPCredentialsModel.ESPassword = txtPassword.Text;
            EasySAPCredentialsModel.ESDatabase = txtDatabase.Text;

            CredentialService cred = new CredentialService();
            if (cred.Login(out string sMessage))
            {
                IMainForm._MenuStrip.Enabled = true;
                Dispose();
                StaticHelper._MainForm.Connected();
                StaticHelper._MainForm.ShowMessage(StaticHelper.OperationMessage);
            }
            else
            {
                StaticHelper._MainForm.ShowMessage(sMessage, true);
            }
        }
    }
}

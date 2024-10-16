using DomainLayer.Helper;
using DomainLayer.Models;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using PresenterLayer.Services.Security;
using PresenterLayer.Views.Main;
using ServiceLayer.Services;
using System;
using System.Windows.Forms;

namespace PresenterLayer.Views.Security
{
    public partial class LoginForm : MetroForm
    {
        LoginService _loginService { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Home)
            { FormHelper.ShowForm(new SettingsForm()); }
            else if (keyData == Keys.Enter)
            { btnOK.PerformClick(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public LoginForm()
        {
            InitializeComponent();
            _loginService = new LoginService();
            Text = ILoginForm.CompanyName;
            lblVersion.Text = ILoginForm.Version;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            FormHelper.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            EasySAPCredentialsModel.ESUserId = txtUserID.Text;
            EasySAPCredentialsModel.ESPassword = txtPassword.Text;
            EasySAPCredentialsModel.ESDatabase = txtCompany.Text;

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

        private void btnChooseCompany_Click(object sender, EventArgs e)
        {
            if (_loginService.ChooseCompany())
            {
                Dispose();

            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            _loginService.FormLoad();
            txtCompany.Visible = ILoginForm.CompanyVisibility;
            lblCompany.Visible = ILoginForm.CompanyVisibility;

            SboCredentials sboCred = new SboCredentials();
            txtCompany.Text = sboCred.Database;
        }
    }
}

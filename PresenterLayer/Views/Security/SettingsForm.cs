using MetroFramework.Forms;
using PresenterLayer.Services.Security;
using System;
using System.Windows.Forms;

namespace PresenterLayer.Views.Security
{
    public partial class SettingsForm : MetroForm
    {
        SettingsService _settingsService { get; set; }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Dispose(); }
            else if (keyData == (Keys.Control | Keys.S))
            { btnSave.PerformClick(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public SettingsForm()
        {
            InitializeComponent();
            _settingsService = new SettingsService();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ISettingsForm.ServiceLayer = cbServiceLayer.Text;
            ISettingsForm.DbServer = txtServer.Text;
            ISettingsForm.DbUserId = txtUserID.Text;
            ISettingsForm.DbPassword = txtPassword.Text;

            if (_settingsService.SaveSettings())
            {
                Dispose();
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            _settingsService.FormLoad();

            cbServiceLayer.DataSource = ISettingsForm.CbServiceLayer;
            cbServiceLayer.Text = ISettingsForm.ServiceLayer;
            txtServer.Text = ISettingsForm.DbServer;
            txtUserID.Text = ISettingsForm.DbUserId;
            txtPassword.Text = ISettingsForm.DbPassword;
        }
    }
}

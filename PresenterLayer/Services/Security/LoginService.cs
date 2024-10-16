using DirecLayer;
using DomainLayer.Helper;
using DomainLayer.Models;
using PresenterLayer.Helper;
using PresenterLayer.Views.Security;
using System;
using System.Windows.Forms;

namespace PresenterLayer.Services.Security
{
    public class LoginService
    {
        public LoginService()
        {
            ILoginForm.CompanyName = SystemSettings.Info.Company;
            ILoginForm.Version = $"{SystemSettings.Info.Trademark} {SystemSettings.Info.AssemblyVersion}";
        }

        public void FormLoad()
        {
            checkSettings();
        }

        void checkSettings()
        {
            try
            {
                SboCredentials sboCred = new SboCredentials();

                if (string.IsNullOrEmpty(sboCred.ServiceLayer))
                {
                    FormHelper.ShowForm(new SettingsForm(), true);
                }
                else if (string.IsNullOrEmpty(sboCred.Database) == false)
                {
                    ILoginForm.CompanyVisibility = true;
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        public void Close()
        {
            Application.Exit();
        }

        public bool ChooseCompany()
        {
            var output = false;
            try
            {
                FormHelper.ShowForm(new ChooseCompanyForm());
                output = true;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
           
            return output;
        }
    }
}

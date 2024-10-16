using DirecLayer;
using DomainLayer.Helper;
using DomainLayer.Models;
using InfrastructureLayer.Repository;
using MetroFramework;
using PresenterLayer.Helper;
using PresenterLayer.Views.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace PresenterLayer.Services.Security
{
    public class SettingsService
    {
        public void FormLoad()
        {
            NetworkAddress();
            var sboCred = new SboCredentials();
            ISettingsForm.ServiceLayer = sboCred.ServiceLayer;
            ISettingsForm.DbServer = sboCred.DbServer;
            ISettingsForm.DbUserId = sboCred.DbUserId;
            ISettingsForm.DbPassword = sboCred.DbPassword;
        }

        public void NetworkAddress()
        {
            try
            {
                ISettingsForm.CbServiceLayer = new List<string>();
                IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ipAddress in hostEntry.AddressList)
                {
                    if (ipAddress.AddressFamily.Equals(System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        ISettingsForm.CbServiceLayer.Add(ipAddress.ToString());
                    };
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private List<ConfigModel> SaveList()
        {
            var list = new List<ConfigModel>();
            list.Add(new ConfigModel { Code = "ServiceLayer", Value = ISettingsForm.ServiceLayer });
            list.Add(new ConfigModel { Code = "DbServer", Value = ISettingsForm.DbServer });
            list.Add(new ConfigModel { Code = "DbUserId", Value = ISettingsForm.DbUserId });
            return list;
        }

        public bool SaveSettings()
        {
            var output = false;
            try
            {
                if (MetroMessageBox.Show(StaticHelper._MainForm, "Saving configuration setup?", SystemSettings.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var repo = new SettingsRepository();
                    output = repo.UpdateSettings(SaveList());
                    StaticHelper._MainForm.ShowMessage(StaticHelper.OperationMessage);
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
            return output;
        }

        public string GetReportPath()
        {
            //OFC Path
            SboCredentials sboCreds = new SboCredentials();

           return $"{sboCreds.Reports}\\";

            //return "\\\\hanaserverdbsi\\b1_shf\\AttachmentsPath\\Extensions\\";
            
            //Onsite Path
           // return "\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\";
        }
    }
}

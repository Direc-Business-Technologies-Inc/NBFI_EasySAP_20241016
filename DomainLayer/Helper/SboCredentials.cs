using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirecLayer;

namespace DomainLayer.Helper
{
    public class SboCredentials
    {
        public string ServiceLayer { get; set; }
        public string DbServer { get; set; }
        public string DbUserId { get; set; }
        public string DbPassword { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }       
        public string SLTag { get; set; }
        public string Reports { get; set; }

        public SboCredentials()
        {
            AppConfig appConfig = new AppConfig();
            ServiceLayer = appConfig.AppSettings("ServiceLayer");
            DbServer = appConfig.AppSettings("DbServer");
            DbUserId = appConfig.AppSettings("DbUserId");
            DbPassword = appConfig.AppSettings("DbPassword");
            Database = appConfig.AppSettings("Database");
            SLTag = appConfig.AppSettings("SAPHanaTag");
            UserId = appConfig.AppSettings("SAPUserId");
            Password = appConfig.AppSettings("SAPPassword");
            Reports = appConfig.AppSettings("CrystalPath");
        }
    }
}

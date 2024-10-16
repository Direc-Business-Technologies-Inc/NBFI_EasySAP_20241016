using PresenterLayer.Views.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Inventory
{
    public class PublicHelper
    {
        //public static IMainForm frmMain { get; set; }
        //public static frm_UDF frmUDF { get; set; }
        public static string sRunID { get; set; }
        public static string DtRunID { get; set; }
        public static int oDocEntry { get; set; }
        public static StringBuilder sbCardCode { get; set; } = new StringBuilder();
        public static bool isCancel { get; set; }
        public static Dictionary<string, string> TempUser { get; set; }
        public static string DeliveryCardCode { get; set; }

        public static bool Uploaded { get; set; } = false;
        public static string oEmployeeCode { get; set; }
        public static string oEmployeeName { get; set; }

        public static string oEmployeeCompleteName { get; set; }
        public static string oLicenseID { get; set; }
    }
}

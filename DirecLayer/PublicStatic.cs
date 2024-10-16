
using System.Collections.Generic;
using System.Text;

namespace DirecLayer
{
   public class PublicStatic
    {
        //public static frmMain frmMain { get; set; }
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

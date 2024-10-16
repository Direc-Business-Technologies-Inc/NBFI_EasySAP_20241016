using Context;
using DirecLayer;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

namespace PresenterLayer
{
    public class SboCred
    {
        public static string DBConnection { get; set; }
        
        public static string Server { get; set; }
        public static string SAPHanaTag { get; set; }
        public static string Database { get; set; }
        public static bool IsDIAPI { get; set; }
        public static string DBUserid { get; set; }
        public static string DBPassword { get; set; }
        public static string UserID { get; set; }
        public static string HttpsUrl { get; set; }
        public static string strCurrentServiceURL { get; set; }
        public static string SessionId { get; set; }
        
    }
}
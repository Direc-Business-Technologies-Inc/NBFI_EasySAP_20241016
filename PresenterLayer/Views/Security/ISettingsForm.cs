using System.Collections.Generic;

namespace PresenterLayer.Views.Security
{
    public class ISettingsForm
    {
        public static List<string> CbServiceLayer { get; set; }
        public static string ServiceLayer { get; set; }
        public static string DbServer { get; set; }
        public static string DbUserId { get; set; }
        public static string DbPassword { get; set; }
    }
}

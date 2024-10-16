using PresenterLayer.Views.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace PresenterLayer.Helper
{
    public class StaticHelper
    {
        public static string OperationMessage { get => "Operation completed successfully"; }
        public static MainForm _MainForm { get; set; }
        public static Dictionary<string, string> TempUser { get; set; }
        public static int MessageCountdown { get; set; }

        public static void DownloadStringBuilderAsFile(StringBuilder sb, string fileName)
        {
            try
            {
                // Write StringBuilder contents to file
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(sb.ToString());
                }

                #region Download File To Local Machine
                //// Download file to local machine
                //byte[] fileBytes = File.ReadAllBytes(fileName);
                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.ContentType = "application/octet-stream";
                //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(fileName) + "\"");
                //HttpContext.Current.Response.AddHeader("Content-Length", fileBytes.Length.ToString());
                //HttpContext.Current.Response.BinaryWrite(fileBytes);
                //HttpContext.Current.Response.Flush();
                //HttpContext.Current.Response.End();
                #endregion
            }
            catch (Exception ex)
            {
                _MainForm.ShowMessage(ex.Message, true);
            }
        }
    }

}

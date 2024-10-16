using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class SystemSettings
    {
        public static AssemblyInfo Info { get; set; } = new AssemblyInfo(Assembly.GetEntryAssembly());

        #region SystemTask
        public void EndTask(string sTask)
        {
            try
            {
                foreach (var clsProcess in Process.GetProcesses())
                {
                    string ProcessUserSID = clsProcess.StartInfo.EnvironmentVariables["USERNAME"];
                    string CurrentUser = Environment.UserName;

                    if (ProcessUserSID.Equals(CurrentUser) && (clsProcess.ProcessName.Equals(sTask)))
                    {
                        clsProcess.Kill();
                        break;
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }
        }
        #endregion

        #region Assembly
        public class GetLogonSid
        {
            //The SID structure that identifies the user that is currently associated with the specified object. 
            //If no user is associated with the object, the value returned in the buffer pointed to by lpnLengthNeeded is zero. 
            //Note that SID is a variable length structure. 
            //You will usually make a call to GetUserObjectInformation to determine the length of the SID before retrieving its value.
            private const int UOI_USER_SID = 4;

            //GetUserObjectInformation function
            //Retrieves information about the specified window station or desktop object.
            [DllImport("user32.dll")]
            static extern bool GetUserObjectInformation(IntPtr hObj, int nIndex, [MarshalAs(UnmanagedType.LPArray)] byte[] pvInfo, int nLength, out uint lpnLengthNeeded);


            //GetThreadDesktop function
            //Retrieves a handle to the desktop assigned to the specified thread.
            [DllImport("user32.dll")]
            private static extern IntPtr GetThreadDesktop(int dwThreadId);


            //GetCurrentThreadId function
            //Retrieves the thread identifier of the calling thread.
            [DllImport("kernel32.dll")]
            public static extern int GetCurrentThreadId();

            //ConvertSidToStringSid function
            //The ConvertSidToStringSid function converts a security identifier (SID) to a string format suitable for display, storage, or transmission.
            //To convert the string-format SID back to a valid, functional SID, call the ConvertStringSidToSid function.

            [DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
            static extern bool ConvertSidToStringSid(
                [MarshalAs(UnmanagedType.LPArray)] byte[] pSID,
                out IntPtr ptrSid);


            /// <summary>
            /// The getLogonSid function returns the Logon Session string
            /// </summary>
            /// <returns></returns>
            public static string getLogonSid()
            {
                var sidString = "";
                IntPtr hdesk = GetThreadDesktop(GetCurrentThreadId());
                byte[] buf = new byte[100];
                uint lengthNeeded;
                GetUserObjectInformation(hdesk, UOI_USER_SID, buf, 100, out lengthNeeded);
                IntPtr ptrSid;
                if (!ConvertSidToStringSid(buf, out ptrSid))
                    throw new  Win32Exception();
                try
                { sidString = Marshal.PtrToStringAuto(ptrSid); }
                catch (Exception ex)
                { throw new Exception($"Error : {ex.Message}"); }
                return sidString;
            }

        }

        public class AssemblyInfo
        {
            // The assembly information values.
            public string Title = "", Description = "", Company = "",
                Product = "", Copyright = "", Trademark = "",
                AssemblyVersion = "", FileVersion = "", Guid = "",
                NeutralLanguage = "";
            public bool IsComVisible = false;

            // Constructors.
            public AssemblyInfo()
                : this(Assembly.GetExecutingAssembly())
            {
            }

            public AssemblyInfo(Assembly assembly)
            {
                // Get values from the assembly.
                var titleAttr =
                    GetAssemblyAttribute<AssemblyTitleAttribute>(assembly);
                if (titleAttr != null) Title = titleAttr.Title;

                var assemblyAttr =
                    GetAssemblyAttribute<AssemblyDescriptionAttribute>(assembly);
                if (assemblyAttr != null) Description = assemblyAttr.Description;

                var companyAttr =
                    GetAssemblyAttribute<AssemblyCompanyAttribute>(assembly);
                if (companyAttr != null) Company = companyAttr.Company;

                var productAttr =
                    GetAssemblyAttribute<AssemblyProductAttribute>(assembly);
                if (productAttr != null) Product = productAttr.Product;

                var copyrightAttr =
                    GetAssemblyAttribute<AssemblyCopyrightAttribute>(assembly);
                if (copyrightAttr != null) Copyright = copyrightAttr.Copyright;

                var trademarkAttr =
                    GetAssemblyAttribute<AssemblyTrademarkAttribute>(assembly);
                if (trademarkAttr != null) Trademark = trademarkAttr.Trademark;

                //Version Setup
                AssemblyVersion = $"{assembly.GetName().Version.Major.ToString()}.{assembly.GetName().Version.Minor.ToString()}.{assembly.GetName().Version.Build.ToString()}";

                var fileVersionAttr =
                    GetAssemblyAttribute<AssemblyFileVersionAttribute>(assembly);
                if (fileVersionAttr != null) FileVersion = fileVersionAttr.Version;

                var guidAttr =
                    GetAssemblyAttribute<GuidAttribute>(assembly);
                if (guidAttr != null) Guid = guidAttr.Value;

                var languageAttr =
                    GetAssemblyAttribute<System.Resources.NeutralResourcesLanguageAttribute>(assembly);
                if (languageAttr != null) NeutralLanguage = languageAttr.CultureName;

                var comAttr =
                    GetAssemblyAttribute<ComVisibleAttribute>(assembly);
                if (comAttr != null) IsComVisible = comAttr.Value;
            }

            // Return a particular assembly attribute value.
            public static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
            {
                // Get attributes of this type.
                object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

                // If we didn't get anything, return null.
                if ((attributes == null) || (attributes.Length == 0)) return null;

                // Convert the first attribute value into the desired type and return it.
                return (T)attributes[0];
            }
        }
        #endregion


        #region FolderOptions
        public string PathExist(string sPath)
        {
            try
            {
                var output = sPath.EndsWith(@"\") ? sPath : $@"{sPath}\";

                if (Directory.Exists(output) == false)
                { Directory.CreateDirectory(output); }

                return output;
            }
            catch (Exception)
            {
                return "";
            }
          
        }

        public bool FolderExist(string sPath)
        {
            var output = Directory.Exists(sPath.EndsWith(@"\") ? sPath : $@"{sPath}\");
            return output;
        }

        public FileInfo[] FileList(string sPath, string sFileParameter)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(sPath);
                FileInfo[] output = directoryInfo.GetFiles(sFileParameter);
                return output;
            }
            catch (Exception)
            {
                return null;
            }
           
        }
        #endregion
    }
}

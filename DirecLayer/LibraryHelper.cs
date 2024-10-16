using System;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
//using Direc.Helper;
using DirecLayer;

namespace Context
{
    public class LibraryHelper
    {
        public static  SystemSettings.AssemblyInfo AssemblyInfo = new SystemSettings.AssemblyInfo(Assembly.GetEntryAssembly());
        public static string DataGridViewRowRet(DataGridViewRow dr, string field)
        {
            string ret;
            if (dr.Cells[field].Value == null)
            {
                ret = "";
            }
            else
            {
                ret = dr.Cells[field].Value.ToString();
            }
            return ret;
        }

        public static string DataRowRet(DataRow dr, string FieldName, string Replace)
        {
            string value = "";
            try
            {
                if (dr[FieldName] == DBNull.Value)
                { value = Replace; }
                else
                { value = dr[FieldName].ToString(); }
            }
            catch
            { value = Replace; }
            return value;
        }

        public static string DataTableRet(DataTable dt, int row, string ColName, string newvalue)
        {
            string value;
            try
            {
                value = dt.Rows[row][ColName].ToString();
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    value = newvalue;
                }
            }
            catch
            {
                value = newvalue;
            }
            
            return value.ToString();
        }

        public static string Replace(string StrQuery)
        {
            try
            {
                StrQuery = StrQuery.Replace(";", " ");
                StrQuery = StrQuery.Replace("\r", "");
                StrQuery = StrQuery.Replace("\n", "");
                return StrQuery;
            }
            catch
            { return null; }
        }

        public static bool DataExist(DataTable dt)
        {
            bool ret = true;
            try
            {
                if (dt == null)
                {
                    ret = false;
                }
                else if (dt.Rows.Count <= 0)
                {
                    ret = false;
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        public static bool UpdateConfig(string elemet,string key, string value)
        {
            bool result = true;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            foreach (XmlElement element in xmlDoc.DocumentElement)
            {
                if (element.Name.Equals(elemet))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        if (node.Attributes[0].Value.Equals(key))
                        {
                            node.Attributes[1].Value = value;
                            break;
                        }
                    }
                    break;
                }
            }

            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection(elemet);

            return result;
        }
    }
}

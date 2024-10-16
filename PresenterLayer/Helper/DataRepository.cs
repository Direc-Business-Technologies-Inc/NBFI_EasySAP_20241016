using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DirecLayer;
using System.Windows.Forms;
//using System.Windows.Forms;

namespace PresenterLayer
{
    public class DataRepository
    {
        /// <summary>
        /// Convert List Collection to Data Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        /// <summary>
        /// create json with document lines for service layer
        /// </summary>
        /// <param name="header"></param>
        /// <param name="DocumentLines"></param>
        /// <returns></returns>
        public static StringBuilder JsonBuilder(Dictionary<string, string> header, List<Dictionary<string, object>> DocumentLines, string LineFieldName)
        {
            int currentCount = 1;
            int maxCount = header.Count + DocumentLines.Count;

            char q = '"';

            StringBuilder json = new StringBuilder();

            json.Append("{");

            foreach (var arr in header)
            {
                if (arr.Value == null || string.IsNullOrEmpty(arr.Value.ToString()))
                {
                }
                else
                {
                    json.Append($"{q}{arr.Key}{q}:{q}{arr.Value}{q},");
                }
                

                currentCount++;
            }

            json.Append($"{q}{LineFieldName}{q}: [");

            foreach (var lines in DocumentLines)
            {   
                json.Append("{");
                foreach (var line in lines)
                {
                    if (line.Value == null || string.IsNullOrEmpty(line.Value.ToString()))
                    {
                    }
                    else
                    {
                        json.Append($"{q}{line.Key}{q}:{q}{line.Value}{q},");
                    }
                }
                json.Append("},");

                currentCount++;
            }

            json.Append("]}");

            return json;
        }

        public static StringBuilder JsonHeaderBuilder(Dictionary<string, string> header, List<Dictionary<string, object>> DocumentLines, string LineFieldName)
        {
            int currentCount = 1;
            int maxCount = header.Count + DocumentLines.Count;

            char q = '"';

            StringBuilder json = new StringBuilder();

            json.Append("{");

            foreach (var arr in header)
            {
                if (arr.Value == null || string.IsNullOrEmpty(arr.Value.ToString()))
                {
                }
                else
                {
                    json.Append($"{q}{arr.Key}{q}:{q}{arr.Value}{q},");
                }

                currentCount++;
            }

            json.Append("}");

            return json;
        }

        public static StringBuilder JsonApprovalBuilder(Dictionary<string, string> header, List<Dictionary<string, object>> DocumentLines, string LineFieldName)
        {
            int currentCount = 1;
            int maxCount = header.Count + DocumentLines.Count;

            char q = '"';

            StringBuilder json = new StringBuilder();

            json.Append("{ ");

            json.Append($"{q}Document{q}:");

            json.Append("{ ");

            foreach (var arr in header)
            {
                if (arr.Value == null || string.IsNullOrEmpty(arr.Value.ToString()))
                {
                }
                else
                {
                    json.Append($"{q}{arr.Key}{q}:{q}{arr.Value}{q},");
                }

                currentCount++;
            }

            json.Append($"{q}{LineFieldName}{q}: [");

            foreach (var lines in DocumentLines)
            {
                json.Append("{");
                foreach (var line in lines)
                {
                    if (line.Value == null || string.IsNullOrEmpty(line.Value.ToString()))
                    {
                    }
                    else
                    {
                        json.Append($"{q}{line.Key}{q}:{q}{line.Value}{q},");
                    }
                }
                json.Append("},");

                currentCount++;
            }

            json.Append("]}}}");

            return json;
        }
        
        public static StringBuilder JsonBuilder(Dictionary<string, object> header, List<Dictionary<string, object>> DocumentLines, string LineFieldName)
        {
            char q = '"';

            StringBuilder json = new StringBuilder();

            json.Append("{");

            foreach (var arr in header)
            {
                if (arr.Value == null || string.IsNullOrEmpty(arr.Value.ToString()))
                {
                }
                else
                {
                    json.Append($"{q}{arr.Key}{q}:{q}{arr.Value}{q},");
                }
            }

            json.Append($"{q}{LineFieldName}{q}: [");

            foreach (var lines in DocumentLines)
            {
                json.Append("{");
                foreach (var line in lines)
                {
                    if (line.Value == null || string.IsNullOrEmpty(line.Value.ToString()))
                    {
                    }
                    else
                    {
                        json.Append($"{q}{line.Key}{q}:{q}{line.Value}{q},");
                    }
                }
                json.Append("},");
            }

            json.Append("]}");

            return json;
        }

        public static DataTable GetData(string query)
        {
         
            var Hana = new SAPHanaAccess();
            var queryReturn = Hana.Get(query);
         
            return queryReturn;
        }

        public static List<string> Modal(string searchKey, List<string> Parameters, string title)
        {
            List<string> modalValue = new List<string>();

            frmSearch2 fS = new frmSearch2()
            {
                oSearchMode = searchKey,
                oFormTitle = title
            };

            if (Parameters != null)
            {
                for (int i = 0; Parameters.Count > i; i++)
                {
                    switch (i)
                    {
                        case 0:
                            frmSearch2.Param1 = Parameters[i].ToString();
                            break;

                        case 1:
                            frmSearch2.Param2 = Parameters[i].ToString();
                            break;

                        case 2:
                            frmSearch2.Param3 = Parameters[i].ToString();
                            break;

                        case 3:
                            frmSearch2.Param4 = Parameters[i].ToString();
                            break;

                        case 4:
                            frmSearch2.Param5 = Parameters[i].ToString();
                            break;
                    }
                }
            }

            fS.ShowDialog();

            if (fS.oCode != null)
            {
                modalValue.Add(fS.oCode);
            }

            if (fS.oName != null)
            {
                modalValue.Add(fS.oName);
            }

            if (fS.oRate != null)
            {
                modalValue.Add(fS.oRate);
            }

            return modalValue;
        }

        public static void RowSearch(DataGridView dgv, string keyword, int columnName)
        {
            if (dgv.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[columnName].Value != null)
                    {
                        if (row.Cells[columnName].Value.ToString().ToUpper().StartsWith(keyword.ToUpper()))
                        {
                            row.Selected = true;
                            dgv.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                        else
                        {
                            row.Selected = false;
                        }
                    }
                }
            }
        }
    }
}

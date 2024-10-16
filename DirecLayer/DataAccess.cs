using System;
using Translator;
using System.Data;
using Sap.Data.Hana;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DirecLayer
{

    public class DataAccess
    {

        static TranslatorTool tool = new TranslatorTool();
        static string QueryResult = null;
        static int numOfStatements, numOfErrors;
        public static string search;
        public static string DBConnection, SQLDBConnection;
        public static string oFormTitle { get; set; }
        public static string oSearchMode { get { return search; } set { search = value; } }

        //public static void Reports(string DocEntry, string CardCode, string ObjCode/*, frmMain frmMain*/)
        //{
        //    string xgetpathandname;


        //    xgetpathandname = "SELECT B.U_Path [xpath], B.U_LayoutName [Layout] FROM [@ODPL] A INNER JOIN [@ADPL] B ON A.Code = B.Code" +
        //                       $" WHERE B.U_UserCode = '{SboCred.GetEmployeeCode()}'" +
        //                       $" AND A.U_CardCode = '{CardCode}'" +
        //                       $" AND B.U_Object = '{ObjCode}'";

        //    var dt = new DataTable();
        //    dt = DataAccess.Select(DataAccess.conStr("HANA"), xgetpathandname/*, frmMain*/);

        //    if (dt.Rows.Count > 1)
        //    {
        //        //Show frmReportList
        //        frmReportList a = new frmReportList(frmMain);
        //        a.dt = dt;
        //        a.oDocKey = DocEntry;
        //        a.MdiParent = frmMain;
        //        a.Show();
        //    }
        //    else if (dt.Rows.Count == 1)
        //    {
        //        //Show frmCrystalReport
        //        //frmCrystalReports2 a = new frmCrystalReports2();
        //        a.oDocKey = DocEntry;
        //        a.xcardcode = CardCode;
        //        a.xcode = ObjCode; //.Substring(1);
        //        a.MdiParent = frmMain;
        //        a.Show();
        //    }
        //    else
        //    {
        //        //frmMain.NotiMsg("No reports found", Color.Red);
        //    }
        //}

        public static string conStr(string Connection)
        {
            string str = null;
            switch (Connection)
            {
                case "HANA":
                    str = DBConnection;
                    break;
                case "SQL":
                    str = SQLDBConnection;
                    break;
                case "SAO":
                    str = SQLDBConnection;
                    break;
            }
            return str;
        }

        public static int intConv(DataTable dt, int row, string col)
        { return Convert.ToInt16(dt.Rows[row][col]); }

        public static bool boolConv(DataTable dt, int row, string col)
        { return Convert.ToBoolean(dt.Rows[row][col]); }

        public static string strConv(DataTable dt, int row, string col)
        { return Convert.ToString(dt.Rows[row][col]); }

        //public static int intNull(DataGridViewRow dt, string col, int RetVal, frmMain frmMain)
        //{
        //    try
        //    {
        //        if (dt.Cells[col].Value == null)
        //        { return RetVal; }
        //        else
        //        { return Convert.ToInt32(dt.Cells[col].Value); }
        //    }
        //    catch (Exception ex)
        //    { frmMain.NotiMsg(ex.Message, Color.Red); return 0; }
        //}

        public static string strNull(DataGridViewRow dt, string col, string RetVal/*, frmMain frmMain*/)
        {
            try
            {
                if (dt.Cells[col].Value == null)
                { return RetVal; }
                else
                { return dt.Cells[col].Value.ToString(); }
            }
            catch (Exception ex)
            { /*frmMain.NotiMsg(ex.Message, Color.Red);*/ return ""; }
        }

        public static int ConvertInt(object val, int nullval)
        {
            int _return = Convert.ToInt32(nullval);

            if (val != null)
            {
                _return = Convert.ToInt32(val);
            }
            return _return;
        }


        /// <summary>
        /// Connection String
        /// </summary>
        /// <param name="Server">Server Name or Server IP Address</param>
        /// <param name="User">UserName</param>
        /// <param name="Password">Password</param>
        /// <param name="Database">Database</param>
        /// <returns></returns>
        public static string HANA_conString(string Server, string User, string Password, string Database)
        { return "DRIVER={HDBODBC32};SERVERNODE=" + Server + ";UID=" + User + ";PWD=" + Password + ";CS=" + Database; }

        /// <summary>
        /// Data Select Command
        /// </summary>
        /// <param name="conString">Connection String</param>
        /// <param name="Query">Queries</param>
        /// <param name="owner">Current form name</param>
        /// <returns>DataTable</returns>
        public static DataTable Select(string conString, string Query/*, [Optional]frmMain frmMain*/)
        {
            try
            {
                QueryResult = tool.TranslateQuery(Query, out numOfStatements, out numOfErrors);
                //QueryResult = QueryResult.Contains($"{"@CartonList"}")?QueryResult.Replace($"{"@CartonList"}", "[@CartonList]"): QueryResult;
                using (DataTable dt = new DataTable())
                {
                    using (HanaConnection con = new HanaConnection(conString))
                    {
                        using (HanaCommand cmd = new HanaCommand(replace(QueryResult/*, frmMain*/), con))
                        {
                            HanaDataAdapter da = new HanaDataAdapter(cmd);
                            con.Open();
                            da.Fill(dt);
                            con.Close();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //PublicStatic.frmMain.NotiMsg(ex.Message, Color.Red);
                return null;
            }
        }
        public static DataTable SelectNoConvert(string conString, string Query/*, [Optional]frmMain frmMain)*/)
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    using (HanaConnection con = new HanaConnection(conString))
                    {
                        using (HanaCommand cmd = new HanaCommand(Query, con))
                        {
                            HanaDataAdapter da = new HanaDataAdapter(cmd);
                            con.Open();
                            da.Fill(dt);
                            con.Close();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
                return null;
            }
        }
        /// <summary>
        /// Execute Command (return false if error)
        /// </summary>
        /// <param name="conString">Connection String</param>
        /// <param name="Query">Queries</param>
        /// <param name="owner">Current form name</param>
        /// <returns>false if error</returns>
        public static Boolean Execute(string conString, string Query/*, frmMain frmMain*/)
        {
            Boolean _bool = false;
            QueryResult = tool.TranslateQuery(Query, out numOfStatements, out numOfErrors);
            try
            {
                using (DataTable dt = new DataTable())
                {
                    using (HanaConnection con = new HanaConnection(conString))
                    {
                        HanaCommand cmd = new HanaCommand();
                        cmd = con.CreateCommand();
                        con.Open();
                        cmd.CommandText = replace(QueryResult/*, frmMain*/);
                        cmd.ExecuteNonQuery();
                        _bool = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }
            return _bool;
        }
        /// <summary>
        /// 8- if the datatable has a record
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="owner">Current form name</param>
        /// <returns></returns>
        public static Boolean Exist(DataTable dt/*, frmMain frmMain*/)
        {
            var _bool = new Boolean();
            try
            {
                if (dt.Rows.Count > 0)
                { _bool = true; }
                else
                { _bool = false; }
            }
            catch (Exception ex)
            {/* frmMain.NotiMsg(ex.Message, Color.Red);*/ }
            return _bool;
        }

        /// <summary>
        /// Search specific field w/in query
        /// </summary>
        /// <param name="conString">Connection String</param>
        /// <param name="RowNumber">Row Number</param>
        /// <param name="FieldName">Field Name</param>
        /// <param name="owner">Current form name</param>
        /// <returns></returns>
        public static string Search(DataTable dt, int RowNumber, string FieldName/*, [Optional] frmMain frmMain*/)
        {
            string result = "";
            try
            {
                result = dt.Rows[RowNumber][FieldName].ToString();
            }
            catch (Exception ex)
            { /*frmMain.NotiMsg(ex.Message, Color.Red);*/ }
            return result;
        }

        public static string replace(string StrQuery/*, [Optional]frmMain frmMain*/)
        {
            try
            {
                StrQuery = StrQuery.Replace(";", " ");
                StrQuery = StrQuery.Replace("'", "''");
                //StrQuery = StrQuery.Replace(",", " ");
                //StrQuery = StrQuery;
                //StrQuery = StrQuery.Replace(":", " ");
                //StrQuery = StrQuery.Replace(".", " ");
                //StrQuery = StrQuery.Replace(",", " ");
                //StrQuery = StrQuery;
                StrQuery = StrQuery.Replace("\'", " ");
                StrQuery = StrQuery.Replace("\"", "");
                StrQuery = StrQuery.Replace("\r", "");
                StrQuery = StrQuery.Replace("\n", "");
                return StrQuery;

            }
            catch (Exception ex)
            { /*frmMain.NotiMsg(ex.Message, Color.Red);*/ return null; }
        }


        public static string DataGridReplace(DataGridView gv, int row, string ColName, string newvalue)
        {
            int n;
            bool isNumeric = int.TryParse(ColName, out n);
            object value;

            if (isNumeric == false)
            { value = gv.Rows[row].Cells[ColName].Value; }
            else
            { value = gv.Rows[row].Cells[n].Value; }

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            { return newvalue; }
            else { return value.ToString(); }

        }

        public static string DataTableReplace(DataTable gv, int row, string ColName, string newvalue)
        {
            int n;
            bool isNumeric = int.TryParse(ColName, out n);
            object value;

            if (isNumeric == false)
            { value = gv.Rows[row][ColName]; }
            else
            { value = gv.Rows[row][n]; }

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            { return newvalue; }
            else { return value.ToString(); }

        }
        //public static string SearchData(string HANA_constring, string HANA_query, int RowNumber, string FieldName/*, frmMain frmMain*/)
        //{
        //    string query = tool.TranslateQuery(HANA_query, out numOfStatements, out numOfErrors);
        //    DataTable dt;
        //    string result = "";
        //    try
        //    {
        //        dt = DataAccess.Select(HANA_constring, replace(HANA_query/*, frmMain*/));
        //        result = dt.Rows[RowNumber][FieldName].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        //frmMain.NotiMsg(ex.Message, Color.Red);
        //        result = "";
        //    }
        //    return result;
        //}

        public static string SearchData(string HANA_constring, string HANA_query, int RowNumber, string FieldName)
        {
            string query = tool.TranslateQuery(HANA_query, out numOfStatements, out numOfErrors);
            DataTable dt;
            string result = "";
            try
            {
                dt = Select(HANA_constring, replace(HANA_query));
                result = dt.Rows[RowNumber][FieldName].ToString();
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }
    }

    public class DataAccessSQL
    {
        /// <summary>
        /// Add-on Connection String
        /// </summary>
        /// <returns>AO Connection String</returns>
        public string AOConString()
        {
            string _Server = "", _DBName = "", _User = "", _Password = "";

            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\SQLConfig.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (counter == 0)
                {
                    _Server = line;
                }
                else if (counter == 1)
                {
                    _DBName = line;
                }
                else if (counter == 2)
                {
                    _User = line;
                }
                else if (counter == 3)
                {
                    _Password = line;
                }
                counter++;
            }

            file.Close();
            //replace value w/ My Settings
            return connString(_Server, _DBName, _User, _Password);
            //return connString("localhost", "EasySAP", "sa", "B1Admin");
        }
        /// <summary>
        /// SAP Connection String
        /// </summary>
        /// <returns>SAP Connection String</returns>
        public string SAPConString()
        {
            //replace value w/ My Settings
            return connString("", "", "", "");
        }
        /// <summary>
        /// SET Sql Connection String
        /// </summary>
        /// <param name="ServerName">Server Name</param>
        /// <param name="DBName">Database Name</param>
        /// <param name="SqlUser">Sql Username</param>
        /// <param name="SqlPassword">Sql Password</param>
        /// <returns>Connection String</returns>
        public static string connString(string ServerName, string DBName, string SqlUser, string SqlPassword)
        {
            string conn;
            conn = "Data Source=" + ServerName + ";Initial Catalog=" + DBName + ";User id=" + SqlUser + ";Password=" + SqlPassword + ";";
            return conn;
        }
        /// <summary>
        /// Sql Select Command
        /// </summary>
        /// <param name="ConString">Sql Connection String</param>
        /// <param name="query">Sql queries</param>
        /// <returns></returns>
        public static DataTable Select(string ConString, string query/*, [Optional]frmMain frmMain*/)
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            con.Open();
                            da.Fill(dt);
                            con.Close();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            { /*frmMain.NotiMsg(ex.Message, Color.Red); */return null; }
        }

        /// <summary>
        /// Execute Sql Command
        /// </summary>
        /// <param name="ConString">Sql Connection String</param>
        /// <param name="query">Sql queries</param>
        /// <returns></returns>
        public static Boolean Execute(string ConString, string query/*, [Optional]frmMain frmMain*/)
        {
            Boolean _bool = false;
            try
            {
                using (DataTable dt = new DataTable())
                {
                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd = con.CreateCommand();
                        con.Open();
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                        _bool = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EasySAP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }
            return _bool;
        }

        /// <summary>
        /// Check if record has rows
        /// </summary>
        /// <returns>True if has rows | False if has no rows</returns>
        //public static Boolean Exist(string ConString, string query)
        //{
        //    var _bool = new Boolean();
        //    try
        //    {
        //        var dt = new DataTable();
        //        dt = DataAccessSQL.Select(ConString, query);
        //        if (dt.Rows.Count > 0)//        { _bool = true; }
        //        else
        //        { _bool = false; }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return _bool;
        //}
        public static Boolean Exist(DataTable dt/*, frmMain frmMain*/)
        {
            var _bool = new Boolean();
            try
            {
                if (dt.Rows.Count > 0)
                { _bool = true; }
                else
                { _bool = false; }
            }
            catch (Exception ex)
            { /*frmMain.NotiMsg(ex.Message, Color.Red);*/ }
            return _bool;
        }

        public static string Search(string ConString, string sqlQuery, int RowNumber, string FieldName/*, [Optional]frmMain frmMain*/)
        {
            DataTable dt;
            string result = "";
            try
            {
                dt = DataAccessSQL.Select(ConString, sqlQuery);
                result = dt.Rows[RowNumber][FieldName].ToString();
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
                result = "";
            }
            return result;
        }

        public static string GetData(DataTable dt, int RowNumber, string FieldName)
        {
            string value = "";

            if (dt.Rows[RowNumber][FieldName] == DBNull.Value)
            {
                value = "";
            }
            else
            {
                value = dt.Rows[RowNumber][FieldName].ToString();
            }

            return value;
        }

        public static string GetDataRow(DataRow dt, string FieldName, string Replace)
        {
            string value = "";
            try
            {
                if (dt[FieldName] == DBNull.Value)
                { value = Replace; }
                else
                { value = dt[FieldName].ToString(); }
            }
            catch
            { }
            return value;
        }

        #region blockings
        ///// <summary>
        ///// Transactional Blockings
        ///// </summary>
        ///// <param name="errcode">Error Code</param>
        ///// <param name="result">Error Description</param>
        //public void TransactionBlockings(out int errcode, out string result)
        //{
        //    int x = -1;
        //    string y = "";
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = Select(AOConString(), "sp_RND_Transaction");
        //        if (Convert.ToInt32(dt.Rows[0][0]) == 0)
        //        {
        //            x = 0;
        //            y = "";
        //        }
        //        else
        //        {
        //            x = Convert.ToInt32(dt.Rows[0][0]);
        //            y = Convert.ToString(dt.Rows[0][1]);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        x = -1;
        //        y = ex.Message;
        //    }

        //    errcode = x;
        //    result = y;
        //}
        #endregion

    }//Class Closing 
}

using Sap.Data.Hana;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using Translator;

namespace DirecLayer
{
    public class SAPHanaAccess
    {
        #region ConnectionString
        public string ConnectionString(string sServer,
                                        string sDbUserId,
                                        string sDbPassword,
                                        string sDatabase,
                                        bool bRefresh = false)
        {
            var output = new StringBuilder();

            try
            {
                output.Append("DRIVER={HDBODBC32};");
                output.Append($"SERVERNODE={sServer};");
                output.Append($"UID={sDbUserId};");
                output.Append($"PWD={sDbPassword};");
                output.Append($"CS={sDatabase};");

                if (bRefresh)
                {
                    var app = new AppConfig();
                    app.UpdateConnectionString("SAPHana", output.ToString());
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output.ToString();
        }

        public string ConnectionString(string sServer,
                                        string sDbUserId,
                                        string sDbPassword,
                                        bool bRefresh = false)
        {
            var output = new StringBuilder();

            try
            {
                var app = new AppConfig();
                output.Append("DRIVER={HDBODBC32};");
                output.Append($"SERVERNODE={sServer};");
                output.Append($"UID={sDbUserId};");
                output.Append($"PWD={sDbPassword};");
                output.Append($"CS={app.AppSettings("Database")};");

                if (bRefresh)
                { app.UpdateConnectionString("SAPHana", output.ToString()); }

            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output.ToString();
        }

        public string ConnectionString(string sDatabase, bool bRefresh = false)
        {
            var output = new StringBuilder();

            try
            {
                var app = new AppConfig();
                output.Append("DRIVER={HDBODBC32};");
                output.Append($"SERVERNODE={app.AppSettings("DbServer")};");
                output.Append($"UID={app.AppSettings("DbUserId")};");
                output.Append($"PWD={app.AppSettings("DbPassword")};");
                output.Append($"CS={sDatabase};");

                if (bRefresh)
                { app.UpdateConnectionString("SAPHana", output.ToString()); }
            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output.ToString();
        }

        public string ConnectionString(bool bRefresh = false)
        {
            var output = new StringBuilder();

            try
            {
                var app = new AppConfig();
                output.Append("DRIVER={HDBODBC32};");
                output.Append($"SERVERNODE={app.AppSettings("DbServer")};");
                output.Append($"UID={app.AppSettings("DbUserId")};");
                output.Append($"PWD={app.AppSettings("DbPassword")};");
                output.Append($"CS={app.AppSettings("Database")};");

                if (bRefresh)
                { app.UpdateConnectionString("SAPHana", output.ToString()); }
            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output.ToString();
        }
        #endregion

        #region RESTful Return
        public DataTable Get(string sConnectionString,
                              string sQuery)
        {
            var output = new DataTable();
            try
            {
                var translator = new TranslatorTool();
                var query = translator.TranslateQuery(sQuery, out int numOfStatements, out int numOfErrors);

                using (var dataAdapter = new HanaDataAdapter(query, sConnectionString))
                {
                    using (var dataTable = new DataTable())
                    {
                        dataAdapter.Fill(dataTable);
                        output = dataTable;
                    }
                }

            }
            catch (Exception ex)
            { throw new Exception($"Error : RESTful Return Create {ex.Message}"); }

            return output;
        }

        public DataTable Get(string sQuery)
        {
            var output = new DataTable();
            var query = string.Empty;
            try
            {
            
                try
                {
                   
                    if (sQuery.Contains("||"))
                    {
                        query = sQuery;
                    }
                    else
                    {
                        var translate = new TranslatorTool();
                        query = translate.TranslateQuery(sQuery, out int numOfStatements, out int numOfErrors);
                    }
                }
                catch(Exception ex)
                {
                    query = sQuery;
                }

                if (string.IsNullOrEmpty(query) == false)
                {
                    using (var dataAdapter = new HanaDataAdapter(query, ConnectionString()))
                    {
                        using (var dataTable = new DataTable())
                        {
                            dataAdapter.Fill(dataTable);
                            output = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter($@"C:\ErrorLogs\ConvertedLog.txt"))
                {
                    writer.Write(query);
                }
                throw new Exception($"Error : RESTful Return Create {ex.Message}"); 
            }
            //{ throw new Exception($"{query}"); }

            return output;
        }

        public int Execute(string sConnectionString,
                        string sQuery)
        {
            var output = -999;
            try
            {
                var query = "";
                try
                {
                    var translate = new TranslatorTool();
                    query = translate.TranslateQuery(sQuery, out int numOfStatements, out int numOfErrors);
                }
                catch
                { query = sQuery; }

                using (var connection = new HanaConnection(sConnectionString))
                {
                    using (var command = new HanaCommand(query, connection))
                    {
                        connection.Open();
                        output = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Execute {ex.Message}"); }

            return output;
        }

        public int Execute(string sQuery)
        {
            var output = -999;
            try
            {
                var query = "";
                try
                {
                    var translate = new TranslatorTool();
                    query = translate.TranslateQuery(sQuery, out int numOfStatements, out int numOfErrors);
                }
                catch
                { query = sQuery; }

                using (var connection = new HanaConnection(ConnectionString()))
                {
                    using (var command = new HanaCommand(query, connection))
                    {
                        connection.Open();
                        output = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Execute {ex.Message}"); }

            return output;
        }

        public int Exec(string sQuery)
        {
            var output = -999;
            try
            {
                var query = "";
                try
                {
                    var translate = new TranslatorTool();
                    query = translate.TranslateQuery(sQuery, out int numOfStatements, out int numOfErrors);
                }
                catch
                { query = sQuery; }

                using (var connection = new HanaConnection(ConnectionString()))
                {
                    using (var command = new HanaCommand(query, connection))
                    {
                        connection.Open();
                        output = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Execute {ex.Message}"); }

            return output;
        }


        #endregion
    }
}

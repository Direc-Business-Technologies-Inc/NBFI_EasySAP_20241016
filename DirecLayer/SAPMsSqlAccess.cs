using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DirecLayer
{
    public class SAPMsSqlAccess
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
                var app = new AppConfig();
                output.Append($"Data Source={sServer};");
                output.Append("Persist Security Info=True;");
                output.Append($"User ID={sDbUserId};");
                output.Append($"Password={sDbPassword};");
                output.Append($"Initial Catalog={sDatabase};");
                output.Append("Connection Timeout=0;");

                if (bRefresh)
                { app.UpdateConnectionString("SAPSql", output.ToString()); }
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
                output.Append($"Data Source={sServer};");
                output.Append("Persist Security Info=True;");
                output.Append($"User ID={sDbUserId};");
                output.Append($"Password={sDbPassword};");
                output.Append($"Initial Catalog={app.AppSettings("SqlDatabase")};");
                output.Append("Connection Timeout=0;");

                if (bRefresh)
                { app.UpdateConnectionString("SAPSql", output.ToString()); }
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
                output.Append($"Data Source={app.AppSettings("SqlServer")};");
                output.Append("Persist Security Info=True;");
                output.Append($"User ID={app.AppSettings("SqlUserId")};");
                output.Append($"Password={app.AppSettings("SqlPassword")};");
                output.Append($"Initial Catalog={app.AppSettings("SqlDatabase")};");
                output.Append("Connection Timeout=0;");

                if (bRefresh)
                { app.UpdateConnectionString("SAPSql", output.ToString()); }
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
                using (var dataAdapter = new SqlDataAdapter(sQuery, sConnectionString))
                {
                    using (var dataTable = new DataTable())
                    {
                        dataAdapter.Fill(dataTable);
                        output = dataTable;
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : RESTful Return Get {ex.Message}"); }

            return output;
        }

        public DataTable Get(string sQuery)
        {
            var output = new DataTable();

            try
            {
                using (var dataAdapter = new SqlDataAdapter(sQuery, ConnectionString()))
                {
                    using (var dataTable = new DataTable())
                    {
                        dataAdapter.Fill(dataTable);
                        output = dataTable;
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : RESTful Return Get {ex.Message}"); }

            return output;
        }


        public int Execute(string sConnectionString,
                        string sQuery)
        {
            var output = -999;
            try
            {
                using (var connection = new SqlConnection(sConnectionString))
                {
                    using (var command = new SqlCommand(sQuery, connection))
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
                using (var connection = new SqlConnection(ConnectionString()))
                {
                    using (var command = new SqlCommand(sQuery, connection))
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
                using (var connection = new SqlConnection(ConnectionString()))
                {
                    using (var command = new SqlCommand(sQuery, connection))
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

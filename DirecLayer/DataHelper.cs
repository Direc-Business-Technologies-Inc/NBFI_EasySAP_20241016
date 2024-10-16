using System;
using System.Data;
using System.Windows.Forms;

namespace DirecLayer
{
    public class DataHelper
    {
        #region DataTable
        public bool DataTableExist(DataTable dt)
        {
            var output = false;
            try
            {
                output = dt != null ? (dt.Rows.Count > 0 ? true : false) : false;
            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output;
        }
        #endregion

        #region DataRow
        public string ReadDataRow(DataRow drDataRow,
                                        string sColumnName,
                                        string oReplace)
        {
            var output = "";

            try
            {
                output = drDataRow != null ? drDataRow[sColumnName].ToString() : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public string ReadDataRow(DataTable dtData,
                                string sColumnName,
                                string oReplace,
                                int iRowCount)
        {
            var output = "";

            try
            {
                output = dtData != null ? (DataTableExist(dtData) ? dtData.Rows[iRowCount][sColumnName].ToString() : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public string ReadDataRow(DataTable dtData,
                                int sColumnName,
                                string oReplace,
                                int iRowCount)
        {
            var output = "";

            try
            {
                output = dtData != null ? (dtData.Rows.Count > 0 ? dtData.Rows[iRowCount][sColumnName].ToString() : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public int ReadDataRow(DataRow drDataRow,
                                        string sColumnName,
                                        int oReplace)
        {
            var output = 0;

            try
            {
                output = drDataRow != null ? (int.TryParse(drDataRow[sColumnName].ToString(), out int ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }
        public int ReadDataRow(DataTable dtData,
                                string sColumnName,
                                int oReplace,
                                int iRowCount)
        {
            var output = 0;

            try
            {
                output = dtData != null ? (int.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : 0.ToString()), out int ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error :  ReadDataRow{ex.Message}"); }

            return output;
        }

        public int ReadDataRow(DataTable dtData,
                                int sColumnName,
                                int oReplace,
                                int iRowCount)
        {
            var output = 0;

            try
            {
                output = dtData != null ? (int.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : 0.ToString()), out int ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error :  ReadDataRow{ex.Message}"); }

            return output;
        }
        public double ReadDataRow(DataRow drDataRow,
                                        string sColumnName,
                                        double oReplace)
        {
            var output = 0.0;

            try
            {
                output = drDataRow != null ? (double.TryParse(drDataRow[sColumnName].ToString(), out double ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }
        public double ReadDataRow(DataTable dtData,
                                string sColumnName,
                                double oReplace,
                                int iRowCount)
        {
            var output = 0.0;

            try
            {
                output = dtData != null ? (double.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : 0.ToString()), out double ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public double ReadDataRow(DataTable dtData,
                                int sColumnName,
                                double oReplace,
                                int iRowCount)
        {
            var output = 0.0;

            try
            {
                output = dtData != null ? (double.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : 0.ToString()), out double ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public DateTime ReadDataRow(DataRow drDataRow,
                                        string sColumnName,
                                        DateTime oReplace)
        {
            var output = DateTime.Now;

            try
            {
                output = drDataRow != null ? (DateTime.TryParse(drDataRow[sColumnName].ToString(), out DateTime ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public DateTime ReadDataRow(DataTable dtData,
                                string sColumnName,
                                DateTime oReplace,
                                int iRowCount)
        {
            var output = DateTime.Now;

            try
            {
                output = dtData != null ? (DateTime.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : DateTime.Now.ToString()), out DateTime ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public DateTime ReadDataRow(DataTable dtData,
                                int sColumnName,
                                DateTime oReplace,
                                int iRowCount)
        {
            var output = DateTime.Now;

            try
            {
                output = dtData != null ? (DateTime.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : DateTime.Now.ToString()), out DateTime ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public string DataTableRet(DataTable dt, int row, string ColName, string newvalue)
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
        #endregion

        #region Datagridviewrow

        public string ValidateCells(DataGridViewRow row, string cellValue)
        {
            string result = "";

            try
            {
                string value = row.Cells[cellValue].Value.ToString();

                result = value == "" || value == null ? "" : value;
            }
            catch { }

            return result;
        }

        public static void RowSearch(DataGridView dgv, string keyword, int columnName)
        {
            if (dgv.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[columnName].Value != null)
                    {
                        if (row.Cells[columnName].Value.ToString().ToUpper().Contains(keyword.ToUpper()))
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
        #endregion
    }
}

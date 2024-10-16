using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirecLayer._03_Repository
{
    public class ValidationRepository
    {
        public int ConvertToInt(object value)
        {
            int newValue = value != null ? 0 : Convert.ToInt32(value);

            return newValue;
        }

        public string ConvertToString(object value)
        {
            string newValue = value != null ? string.Empty : value.ToString();

            return newValue;
        }

        public double ConvertToDouble(object value)
        {
            double newValue = value == null || value.ToString() != string.Empty ? 0D :
                Convert.ToDouble(value);

            return Math.Round(newValue, 2);
        }

        public string ValidateCells(DataGridViewRow row, int cellValue)
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

        public string ValidateCells(DataRow row, string cellValue)
        {
            string result = "";

            try
            {
                string value = row[cellValue].ToString();

                result = value == "" || value == null ? "" : value;
            }
            catch { }

            return result;
        }

        public double ValidateCellsDouble(DataGridViewRow row, int cellValue)
        {
            double result = 0;

            try
            {
                string value = row.Cells[cellValue].Value.ToString();

                result = value == "" || value == null ? 0D : Convert.ToDouble(value);
            }
            catch { }

            return Math.Round(result, 2);
        }

        public double ValidateCellsDouble(DataGridViewRow row, string cellValue)
        {
            double result = 0;

            try
            {
                var value = row.Cells[cellValue].Value;

                result = value == null ? 0D : Convert.ToDouble(value);
            }
            catch { }

            return Math.Round(result, 2);
        }

        public double ValidateCellsDouble(DataRow row, string cellValue)
        {
            double result = 0;

            try
            {
                string value = row[cellValue].ToString();

                result = value == "" || value == null ? 0D : Convert.ToDouble(value);
            }
            catch { }

            return Math.Round(result, 2);
        }

        public decimal ValidateCellsDecimal(DataGridViewRow row, int cellValue)
        {
            decimal result = 0;

            try
            {
                string value = row.Cells[cellValue].Value.ToString();

                result = value == "" || value == null ? 0 : Convert.ToDecimal(value);
            }
            catch { }

            return Math.Round(result, 2);
        }

        public decimal ValidateCellsDecimal(DataGridViewRow row, string cellValue)
        {
            decimal result = 0;

            try
            {
                string value = row.Cells[cellValue].Value.ToString();

                result = value == "" || value == null ? 0 : Convert.ToDecimal(value);
            }
            catch { }

            return Math.Round(result, 2);
        }

        public decimal ValidateCellsDecimal(DataRow row, string cellValue)
        {
            decimal result = 0;

            try
            {
                string value = row[cellValue].ToString();

                result = value == "" || value == null ? 0 : Convert.ToDecimal(value);
            }
            catch { }

            return Math.Round(result, 2);
        }
    }
}

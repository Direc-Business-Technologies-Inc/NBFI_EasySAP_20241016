using ExcelDataReader;
using System.Data;
using System.IO;

namespace DirecLayer
{
    public class Excel
    {
        IExcelDataReader reader;

        public delegate void ExcelData(DataSet excelData);

        public bool ReadExcel (ExcelData exceldata, Stream fs, string type)
        {
            bool isExcel = true;
            DataSet ds = new DataSet();

            if (type == ".csv")
            {
                reader = ExcelReaderFactory.CreateCsvReader(fs);
                isExcel = false;
            }
            else
            {
                reader = ExcelReaderFactory.CreateReader(fs);
            }

            ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (a) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }               
            });

            exceldata(ds);

            return isExcel;
        }
        
        public void CloseExcel ()
        {
            reader.Close();
        }
    }
}

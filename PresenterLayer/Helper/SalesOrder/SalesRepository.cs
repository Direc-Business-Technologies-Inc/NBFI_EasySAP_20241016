using DirecLayer;
using PresenterLayer.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresenterLayer;

namespace PresenterLayer.Helper
{
    public class SalesRepository
    {
        SalesAR_generics _generics = new SalesAR_generics();
        SAPHanaAccess Hana = new SAPHanaAccess();
        public DataTable BpInformation(string cardCode)
        {

            string query = "SELECT " +
                            "A.CardCode, " +
                            "A.CardName, " +
                            "A.Address, " +
                            "A.ProjectCod, " +
                            "A.SlpCode, " +
                            "A.ListNum, " +
                            "A.Currency, " +
                            "ECVatGroup, " +
                            "CntctPrsn, " +
                            "(Select Z.SlpName FROM OSLP Z Where Z.SlpCode = A.SlpCode) [SlpName], " +
                            "(SELECT Z.GroupName FROM OCRG Z Where Z.GroupCode = A.GroupCode) [Group], " +
                            $"(SELECT TOP 1 Z.U_Whs FROM CRD1 Z where Z.CardCode = '{cardCode}') [Whs] " +
                            $"FROM OCRD A WHERE A.CardCode = '{cardCode}'";

            return _generics.Select(query);
        }

      
        public string GetDocNum(string DocEntry, string SapTable)
        {
            string strDocNum = "0";

            strDocNum = Hana.Get( $"select DocNum from {SapTable} where DocEntry = '{DocEntry}'").Rows[0]["DocNum"].ToString();

            return strDocNum;
        }

        public string TaxRateValue(string currency)
        {
            string rate = "";

            string query = $"SELECT TOP 1 Rate FROM ORTT where Currency = '{currency}' Order by RateDate Desc";

            DataTable dt = _generics.Select(query);

            if (dt.Rows.Count > 0)
            {
                rate = dt.Rows[0][0].ToString();
            }

            return rate;
        }

        public DataTable CompanyTinValues()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Name");
            dt.Columns.Add("Code");

            dt.Rows.Add(DBNull.Value, DBNull.Value);

            foreach (DataRow row in _generics.Select("SELECT Name, Code FROM [@CMP_INFO]").Rows)
            {
                dt.Rows.Add(row["Name"].ToString(), row["Code"].ToString());
            }

            return dt;
        }

        public DataTable DocumentTypeValues()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Name");
            dt.Columns.Add("Code");

            dt.Rows.Add(DBNull.Value, DBNull.Value);

            foreach (DataRow row in _generics.Select("SELECT Code, Name FROM [@DOC_TYPE]").Rows)
            {
                dt.Rows.Add(row["Name"].ToString(), row["Code"].ToString());
            }

            return dt;
        }
        
    }
}

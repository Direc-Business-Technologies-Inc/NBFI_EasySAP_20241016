using DirecLayer;
using PresenterLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class PurchasingAP_generics
    {
        public static int index = 0; 

        public static int Series { get; set; }

        public static int SlpCode { get; set; }

        public static string Dept { get; set; }

        public static string BudgetDesc { get; set; }
        public static SAPHanaAccess Hana = new SAPHanaAccess();
        public List<string> ModalShow(string parameter, string parameter1, string title)
        {
            List<string> modalValue = new List<string>();

            frmSearch2 fS = new frmSearch2()
            {
                oSearchMode = parameter,
                oFormTitle = title
            };

            if (parameter1 != string.Empty)
            {
                frmSearch2.Param1 = parameter1;
            }

            fS.ShowDialog();

            if (fS.oCode != null)
            {
                modalValue.Add(fS.oCode);
                modalValue.Add(fS.oName);
            }

            return modalValue;
        }

        public List<string> ModalShow(string searchKey, List<string> Parameters, string title)
        {
            List<string> modalValue = new List<string>();

            frmSearch2 fS = new frmSearch2()
            {
                oSearchMode = searchKey,
                oFormTitle = title
            };

            if (Parameters.Count > 0)
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

                if (fS.oName != null)
                {
                    modalValue.Add(fS.oName);
                }
            }

            return modalValue;
        }

        public DataTable Select(string query)
        {
            DataTable dt = Hana.Get( query);

            return dt;
        }

        public List<string> DocumentSeries(int objCode)
        {
            List<string> series = new List<string>();

            DataTable dt = Select($"SELECT T0.SeriesName, T0.Series FROM NNM1 T0 Where T0.ObjectCode = {objCode}");

            foreach (DataRow row in dt.Rows)
            {
                series.Add(row[0].ToString());
                Series = Convert.ToInt32(row[1]);
            }

            return series;
        }

        public string BpCurrency (string cardcode)
        {
            string currency = "";

            string query = $"Select Currency from OCRD A Where A.frozenFor = 'N' AND A.CardType = 'S' AND CardCode = '{cardcode}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                currency = dt.Rows[0]["Currency"].ToString();
            }

            return currency;
        }
         
        public string DocumentNumber(int objCode, string seriesName)
        {
            string docNum = "";

            DataTable dt = Select($"SELECT T0.NextNumber, T0.Series FROM NNM1 T0 Where T0.ObjectCode = {objCode} AND T0.SeriesName = '{seriesName}'");

            if (dt.Rows.Count > 0)
            {
                docNum = dt.Rows[0]["NextNumber"].ToString();
                Series = Convert.ToInt32(dt.Rows[0]["Series"]);
            }

            return docNum;
        }

        public DataTable CompanyTIN()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Name");
            dt.Columns.Add("Code");

            dt.Rows.Add(DBNull.Value, DBNull.Value);

            foreach (DataRow row in Select("SELECT Name, Code FROM [@CMP_INFO]").Rows)
            {
                dt.Rows.Add(row["Name"].ToString(), row["Code"].ToString());
            }
            
            return dt;
        }

        public string RateValue(string currency)
        {
            string rate = "";

            string query = $"SELECT TOP 1 Rate FROM ORTT where Currency = '{currency}' Order by RateDate Desc";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                rate = dt.Rows[0][0].ToString();
            }

            return rate;
        }

        public string ColorName(string code)
        {
            string color = "";

            string query = $"SELECT Distinct Name FROM [@OCLC] WHERE Code = '{code}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                color = dt.Rows[0][0].ToString();
            }

            return color;
        }

        public string StyleName(string code)
        {
            string style = "";

            string query = $"SELECT Distinct U_Style FROM [@OSTL] WHERE U_Code = '{code}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                style = dt.Rows[0][0].ToString();
            }

            return style;
        }

        public List<string> Brand (string ItemCode)
        {
            List<string> brand = new List<string>();

            string query = "SELECT (SELECT Z.Code FROM [@OBND] Z WHERE Z.Code = A.U_ID001) [BrandCode], " +
                           $"(SELECT Z.Name FROM [@OBND] Z WHERE Z.Code = A.U_ID001) [Brand] FROM OITM A Where ItemCode = '{ItemCode}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                brand.Add(dt.Rows[0][0].ToString());
                brand.Add(dt.Rows[0][1].ToString());
            }
            else
            {
                brand.Add(string.Empty);
                brand.Add(string.Empty);
            }

            return brand;
        }

        public string TaxCode (string BpCode)
        {
            string tax = "";

            string query = $"SELECT ECVatGroup FROM OCRD WHERE CardCode = '{BpCode}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                tax = dt.Rows[0][0].ToString();
            }

            return tax;
        }

        public string Uom (string ItemCode)
        {
            string uom = "";

            string query = $"SELECT BuyUnitMsr FROM OITM WHERE ItemCode = '{ItemCode}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                uom = dt.Rows[0][0].ToString();
            }

            return uom;
        }

        public int UomEntry (string Uom)
        {
            int uomEntry = 0;

            string query = $"SELECT UomEntry FROM OUOM Where UomCode = '{Uom}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                uomEntry = Convert.ToInt32(dt.Rows[0][0]);
            }

            return uomEntry;
        }

        public bool IsDraft ()
        {
            bool isTrue = false;

            string query =  $"SELECT Distinct T2.U_NAME FROM OWTM T0 INNER JOIN WTM3 T1 ON T1.WtmCode = T0.WtmCode " +

                            $"INNER JOIN OUSR T2 ON T2.userSign = T0.UserSign WHERE T0.Active = 'Y' AND T1.TransType = 22 AND T2.U_NAME = '{SboCred.UserID}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }

        public List<string> Glaccount (string gl)
        {
            List<string> glList = new List<string>();

            string query = $"SELECT AcctCode, AcctName FROM OACT Where AcctCode = '{gl}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                glList.Add(dt.Rows[0][0].ToString());
                glList.Add(dt.Rows[0][1].ToString());
            }
            else
            {
                glList.Add("");
                glList.Add("");
            }
            return glList;
        }

        public string CompanyCode (string name)
        {
            string code = "";

            string query = $"SELECT Code FROM [@CMP_INFO] Where Name = '{name}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                code = dt.Rows[0][0].ToString();
            }

            return code;
        }

        public string CompanyInfo (string companyTin)
        {
            string input = companyTin;

            string query = $"SELECT Name FROM [@CMP_INFO] WHERE Code = '{companyTin}'";

            DataTable dt = Select(query);

            if (dt.Rows.Count > 0)
            {
                input = dt.Rows[0][0].ToString();
            }

            return input;
        }
    }
}

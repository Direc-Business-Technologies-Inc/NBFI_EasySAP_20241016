using DirecLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PresenterLayer.Helper.SalesOrder
{
    class SOmaintenance
    {
        SAPHanaAccess Hana = new SAPHanaAccess();
        public string SelValue(string SOqrycode, string strDocType, [Optional] string strBPCode, [Optional] string strAddress, [Optional] string strTxtbox)
        {
            string strSelectedQry = "";
            string strSelValue = "";


            string strAddr = string.IsNullOrEmpty(strAddress) ? "" : strAddress;
            string strBPC = string.IsNullOrEmpty(strBPCode) ? "" : strBPCode;

            if (SOqrycode == "series1" && strDocType != "")
            {
                strSelectedQry = "SELECT " +
                                " (Select SeriesName from NNM1 where SeriesName = T1.U_Series and ObjectCode = '17' ) [Value] " +
                                " FROM [@DOC_TYPE] T1 " +
                                $" WHERE T1.Name = '{strDocType}' ";
            }
            else if (SOqrycode == "ARseries" && strDocType != "")
            {
                //strSelectedQry = "SELECT " +
                //                " (Select SeriesName from NNM1 where SeriesName = T1.U_Series and ObjectCode = '13' ) [Value] " +
                //                " FROM [@DOC_TYPE] T1 " +
                //                $" WHERE T1.Name = '{strDocType}' ";
                strSelectedQry = "SELECT " +
                                " U_DocSeries [Value] " +
                                " FROM [@DOC_TYPE] T1 " +
                                $" WHERE T1.Name = '{strDocType}' ";
            }
            else if (SOqrycode == "toWhs1" && strDocType != "")
            {
                strSelectedQry = "SELECT DISTINCT " +
                                " CASE a.U_WhsSource  " +
                                " WHEN 'WHS'  " +
                                " THEN a.U_WhsCode " +
                                " WHEN 'CRD' " +
                                " THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' ";

                if (strAddr != "")
                {
                    strSelectedQry += $" and x.Street = '{ strAddr }' ";
                }

                strSelectedQry += $" and x.CardCode = '{ strBPC }') " +
                " END [Value] " +
                " FROM [@DOC_TYPE] a  " +
                " LEFT JOIN OLCT b on a.U_WhsSource = b.Location " +
                " LEFT JOIN OWHS c on((b.Code <> c.Location) " +
                " or (b.Code = c.Location) )  " +
                " and a.U_WhsSource = 'WHS-LOC' " +
                $" WHERE a.Name = '{ strDocType }' Order by Value ";
            }
            else if (SOqrycode == "toWhs2" && strDocType != "" && strTxtbox != "")
            {
                strSelectedQry = "SELECT Value FROM ( " +
                                "SELECT DISTINCT " +
                                " CASE a.U_WhsSource  " +
                                " WHEN 'WHS'  " +
                                " THEN a.U_WhsCode " +
                                " WHEN 'CRD' " +
                                $" THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' ";

                if (strAddr != "")
                {
                    strSelectedQry += $" and x.Street = '{ strAddr }' ";
                }

                strSelectedQry += $" and x.CardCode = '{ strBPC }') " +
                " END [Value] " +
                " FROM [@DOC_TYPE] a  " +
                " LEFT JOIN OLCT b on a.U_WhsSource = b.Location " +
                " LEFT JOIN OWHS c on((b.Code <> c.Location) " +
                " or (b.Code = c.Location) )  " +
                " and a.U_WhsSource = 'WHS-LOC' " +
                $" WHERE a.Name = '{ strDocType }' ) MT1 where Value = '{strTxtbox}' Order by Value ";
            }
            else if (SOqrycode == "AllowDupItems" && strDocType != "")
            {
                strSelectedQry = "SELECT " +
                                " U_AllowDupItems [Value] " +
                                " FROM [@DOC_TYPE] T1 " +
                                $" WHERE T1.Name = '{strDocType}' ";
            }

            if (strSelectedQry != "" && Hana.Get( strSelectedQry).Rows.Count > 0)
            {
                strSelValue = Hana.Get(strSelectedQry).Rows[0]["Value"].ToString();
            }

            return strSelValue;
        }

        public string GetCompany(string BPcode, string ItemCode)
        {
            string CompanyCode = "";

            string strSelQry = "Select top 1 ISNULL(b.U_Company, '') [Company] " +
                                " from OITM a " +
                                " inner " +
                                " join CPN3 b on a.U_ID001 = ifnull(b.U_Brand, '') or  ifnull(b.U_Brand, '') = '' " +
                                $" inner join CPN1 c on c.BpCode = '{BPcode}' and c.CpnNo = b.CpnNo " +
                                " inner join OCPN d on c.CpnNo = d.CpnNo and d.U_CType = 'VC' " +
                                $" where a.ItemCode = '{ItemCode}'";

            if (Hana.Get( strSelQry).Rows.Count > 0)
            {
                CompanyCode = Hana.Get(strSelQry).Rows[0]["Company"].ToString();
            }

            return CompanyCode;
        }
    }
}

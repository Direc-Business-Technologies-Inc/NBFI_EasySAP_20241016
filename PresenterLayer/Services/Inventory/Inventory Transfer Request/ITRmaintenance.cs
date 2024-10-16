using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DirecLayer;

namespace PresenterLayer.Services.Inventory
{
    class ITRmaintenance
    {
        SAPHanaAccess sapHanaAccess = new SAPHanaAccess();
        public string SelValue(string ITRqrycode, string strTransType, [Optional] string strBPCode, [Optional] string strAddress, [Optional] string strTxtbox)
        {
            string strSelectedQry = "";
            string strSelValue = "";

            string strAddr = string.IsNullOrEmpty(strAddress) ? "" : strAddress;
            string strBPC = string.IsNullOrEmpty(strBPCode) ? "" : strBPCode;
            

            if (ITRqrycode == "series1" && strTransType != "")
            {
                strSelectedQry = "SELECT " +
                                " (Select SeriesName from NNM1 where SeriesName = T1.U_ITRSeries and ObjectCode = '1250000001' ) [Value] " +
                                " FROM [@TRANSFER_TYPE] T1 " +
                                $" WHERE T1.Code = '{strTransType}' ";
            }
            else if (ITRqrycode == "frmWhs1" && strTransType != "")
            {
                strSelectedQry = "SELECT DISTINCT " +
                                " CASE a.U_FillerSource " +
                                " WHEN 'WHS' " +
                                " THEN a.U_Filler " +
                                " WHEN 'WHS-LOC' " +
                                " THEN c.WhsCode " +
                                " WHEN 'CRD' " +
                                $" THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' and Address = '{ strAddr }' and x.CardCode = '{ strBPC }') " +
                                " ELSE '' " +
                                " END [Value] " +
                                " FROM [@TRANSFER_TYPE] a  " +
                                " LEFT JOIN OLCT b on a.U_Filler = b.Location " +
                                " LEFT JOIN OWHS c on(b.Code <> c.Location and a.U_FillerComp = '<>') or(b.Code = c.Location and a.U_FillerComp = '=')  " +
                                $" WHERE a.Code = '{ strTransType }'";
            }
            else if (ITRqrycode == "frmWhs2" && strTransType != "" && strTxtbox != "")
            {
                strSelectedQry = "SELECT Value FROM ( " +
                                " SELECT DISTINCT " +
                                " CASE a.U_FillerSource " +
                                " WHEN 'WHS' " +
                                " THEN a.U_Filler " +
                                " WHEN 'WHS-LOC' " +
                                " THEN c.WhsCode " +
                                " WHEN 'CRD' " +
                                $" THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' and Address = '{ strAddr }' and x.CardCode = '{ strBPC }') " +
                                " ELSE '' " +
                                " END [Value] " +
                                " FROM [@TRANSFER_TYPE] a  " +
                                " LEFT JOIN OLCT b on a.U_Filler = b.Location " +
                                " LEFT JOIN OWHS c on(b.Code <> c.Location and a.U_FillerComp = '<>') or(b.Code = c.Location and a.U_FillerComp = '=')  " +
                                $" WHERE a.Code = '{ strTransType }' ) MT1 where Value = '{strTxtbox}' ";
            }
            else if (ITRqrycode == "toWhs1" && strTransType != "")
            {
                //strSelectedQry = "SELECT DISTINCT " +
                //                " CASE a.U_DestSource  " +
                //                " WHEN 'WHS'  " +
                //                " THEN a.U_Destination " +
                //                " WHEN 'WHS-LOC'  " +
                //                " THEN c.WhsCode " +
                //                " WHEN 'CRD' " +
                //                $" THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' and Address = '{ strAddr }' and x.CardCode = '{ strBPC }') " +
                //                " END [Value] " +
                //                " FROM [@TRANSFER_TYPE] a  " +
                //                " LEFT JOIN OLCT b on a.U_Destination = b.Location " +
                //                " LEFT JOIN OWHS c on((b.Code <> c.Location and a.U_DestComp = '<>') " +
                //                " or (b.Code = c.Location and a.U_DestComp = '=') )  " +
                //                " and a.U_DestSource = 'WHS-LOC' " +
                //                $" WHERE a.U_Destination = '{ strBPC }' OR c.WhsCode = '{ strBPC }' Order by Value ";

                strSelectedQry = "SELECT DISTINCT " +
                                " CASE a.U_DestSource  " +
                                " WHEN 'WHS'  " +
                                " THEN a.U_Destination " +
                                " WHEN 'WHS-LOC'  " +
                                " THEN c.WhsCode " +
                                " WHEN 'CRD' " +
                                $" THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' and Address = '{ strAddr }' and x.CardCode = '{ strBPC }') " +
                                " END [Value] " +
                                " FROM [@TRANSFER_TYPE] a  " +
                                " LEFT JOIN OLCT b on a.U_Destination = b.Location " +
                                " LEFT JOIN OWHS c on((b.Code <> c.Location and a.U_DestComp = '<>') " +
                                " or (b.Code = c.Location and a.U_DestComp = '=') )  " +
                                " and a.U_DestSource = 'WHS-LOC' " +
                                $" WHERE a.Code = '{ strTransType }' Order by Value ";

            }
            else if (ITRqrycode == "toWhs2" && strTransType != "" && strTxtbox != "")
            {
                strSelectedQry = "SELECT Value FROM ( " +
                                "SELECT DISTINCT " +
                                " CASE a.U_DestSource  " +
                                " WHEN 'WHS'  " +
                                " THEN a.U_Destination " +
                                " WHEN 'WHS-LOC'  " +
                                " THEN c.WhsCode " +
                                " WHEN 'CRD' " +
                                $" THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' and Address = '{ strAddr }' and x.CardCode = '{ strBPC }') " +
                                " END [Value] " +
                                " FROM [@TRANSFER_TYPE] a  " +
                                " LEFT JOIN OLCT b on a.U_Destination = b.Location " +
                                " LEFT JOIN OWHS c on((b.Code <> c.Location and a.U_DestComp = '<>') " +
                                " or (b.Code = c.Location and a.U_DestComp = '=') )  " +
                                " and a.U_DestSource = 'WHS-LOC' " +
                                $" WHERE a.Code = '{ strTransType }' ) MT1 where Value = '{strTxtbox}' Order by Value ";
            }
            else if (ITRqrycode == "AllowDupItems" && strTransType != "")
            {
                strSelectedQry = "SELECT " +
                                " U_AllowDupItems [Value] " +
                                " FROM [@TRANSFER_TYPE] T1 " +
                                $" WHERE T1.Code = '{strTransType}' ";
            }

            if (strSelectedQry != "" && sapHanaAccess.Get(strSelectedQry).Rows.Count > 0)
            {
                strSelValue = sapHanaAccess.Get(strSelectedQry).Rows[0]["Value"].ToString();
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

            if (sapHanaAccess.Get(strSelQry).Rows.Count > 0)
            {
                CompanyCode =sapHanaAccess.Get(strSelQry).Rows[0]["Company"].ToString();
                if (CompanyCode != "")
                {
                    CompanyCode =sapHanaAccess.Get($"select Name from [@CMP_INFO] where Code = '{CompanyCode}'").Rows[0]["Name"].ToString();
                }
            }

            return CompanyCode;
        }

        public string GetDocNum(string DocEntry, string SapTable)
        {
            string strDocNum = "0";

            strDocNum = sapHanaAccess.Get($"select DocNum from {SapTable} where DocEntry = '{DocEntry}'").Rows[0]["DocNum"].ToString();

            return strDocNum;
        }

        public string GetUOM(string sItemCode)
        {
            string sInvntoryUom = "";

            sInvntoryUom = sapHanaAccess.Get($"select InvntryUom from OITM where ItemCode = '{sItemCode}'").Rows[0]["InvntryUom"].ToString();

            return sInvntoryUom;
        }
    }
}

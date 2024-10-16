using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.InventoryRepository
{
    public class QueryRepository : IQueryRepository
    {
        public QueryRepository()
        {
        }

        public string GetSeriesQuery(string objectCode)
        {
            return "SELECT T0.Series [Code], T0.SeriesName [Name]" +

                "FROM NNM1 T0 " +

                $"Where T0.ObjectCode = '{objectCode}'";
        }
        public string GetDocNumQuery(string objectCode, string Series)
        {
            return "SELECT T0.NextNumber " +
                    "FROM NNM1 T0 " +
                    $"Where T0.ObjectCode = '{objectCode}' AND T0.Series = '{Series}'";
        }

        public string GetTransferTypeQuery()
        {
            return $"SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [@TRANSFER_TYPE] Order by Name";
        }

        public string GetCompanyQuery()
        {
            return "SELECT '' [Code], '' [Name] " +
                    "UNION " +
                    "SELECT Code, Name FROM [@CMP_INFO] Order by Name";
        }

        public string GetCompanyQuerySearch(string param)
        {
            return "SELECT '' [Code], '' [Name] " +
                    "UNION " +
                    $"SELECT Code, Name FROM [@CMP_INFO] WHERE Code = '{param}' Order by Name";
        }

        public string GetUdfTableValuesQuery(string value)
        {
            return $"SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [{value}]";
        }

        public string GetUdfValidValuesQUery(string value)
        {
            return "SELECT ''[Code] ,''[Name] UNION SELECT FldValue, Descr " +

                "FROM UFD1 " +

                $"WHERE TableID = 'OWTQ' AND FieldID = '{value}'";
        }

        public string GetUdfQuery(string table, string fields, string arrangedUDF)
        {
            return $"SELECT 'U_' + AliasID [AliasID], EditSize , TypeID , FieldID, '@' + ISNULL(RTable,RelUDO) [UserDefined], ISNULL(RelSO,'') [Table] , Dflt , Descr FROM CUFD WHERE TableID = '{table}' AND AliasID IN ({fields}) ORDER BY CASE AliasID {arrangedUDF} END";
        }

        public string GetMaintenanceLineQuery(int conditionnumber, string code, string item)
        {
            return $"SELECT U_Condition{conditionnumber} FROM [@A_MAINTENANCE_LINES] WHERE Code = '{code}' AND U_Table = '{item}'";
        }

        public string BPinformationQuery(string CardCode)
        {
            string query = "SELECT A.CardCode,A.CardName,A.MailAddres [Address]" +
                    ", (SELECT min(Y.Address) [Address] FROM CRD1 Y Where Y.CardCode = A.CardCode And AdresType = 'S') [Add2]" +
                    ", A.ProjectCod,A.SlpCode,A.ListNum,A.GroupCode " +
                    ",(Select Z.SlpName FROM OSLP Z Where Z.SlpCode = A.SlpCode) [SlpName] " +
                    $",(SELECT min(Y.U_Whs) [U_Whs] FROM CRD1 Y Where Y.CardCode = A.CardCode And AdresType = 'S') [Whs],ECVatGroup FROM OCRD A WHERE A.CardCode = '{CardCode}'";
            return query;
        }
        public string BpRateValueQuery(string currency)
        {
            return "SELECT TOP 1 Rate FROM ORTT " +
                    $"where Currency = '{currency}' Order by RateDate Desc";
        }
        public string BpTransferTypeQuery(string code)
        {
            string query = $"SELECT CASE WHEN QryGroup2 = 'Y' THEN 'Import' ELSE 'LOCAL' END AS [TransType] FROM OCRD WHERE CardCode = '{code}'";

            return query;
        }
        public string UdfOrderNoQuery(string code)
        {
            string query = "SELECT ifnull(b.Notes, '') + ' - ' + count(a.U_OrderNo) + 1 " +

                "FROM OPOR a " +

                "INNER JOIN OCRD b on a.CardCode = b.CardCode " +

                $"WHERE a.CardCode = '{code}' " +

                "and a.CANCELED = 'N' " +

                "GROUP BY b.Notes";

            return query;
        }

        public string VatGroupRateQuery(string code)
        {
            return $"SELECT Z.Rate FROM OVTG Z Where Z.Code =  '{code}'";
        }

        public string GetUdfQueryWOorderBy(string table)
        {
            return $"SELECT 'U_' + AliasID [AliasID], EditSize , TypeID , FieldID, '@' + ISNULL(RTable,RelUDO) [UserDefined], ISNULL(RelSO,'') [Table] , Dflt , Descr FROM CUFD WHERE TableID = '{table}' ";
        }

        public string GetCompanyPerLine(string BPCode, string ItemCode)
        {
            return "select ISNULL(T1.U_Company, '') [U_Company] from CPN3 T1 inner join CPN1 T2 on T2.CpnNo = T1.CpnNo inner join OCPN T3 on T3.CpnNo = T2.CpnNo " +
                   $" where T3.U_CType = 'VC' and T2.BpCode = '{BPCode}' and T1.U_Brand = (select U_ID019 from OITM where ItemCode = '{ItemCode}')";
        }
    }
}

using Context;
using zDeclare;
using DirecLayer;
using PresenterLayer;
using PresenterLayer.Services;
namespace PresenterLayer.Helper
{
    public class SP
    {
        public static string ITM_Brands { get; set; } = HanaQuery("ITEMMASTERDATA", "Brand");
        public static string ITM_Colors { get; set; } = HanaQuery("ITEMMASTERDATA", "Color");
        public static string ITM_DepartmentsByBrand { get; set; } = HanaQuery("ITEMMASTERDATA", "Dept");
        public static string ITM_SubDepartmentsByBrandDept { get; set; } = HanaQuery("ITEMMASTERDATA", "SubDept");
        public static string ITM_CategoryByBrandDeptSubDept { get; set; } = HanaQuery("ITEMMASTERDATA", "Categ");
        public static string ITM_ItmsGrpCod { get; set; } = HanaQuery("ITEMMASTERDATA", "ItmsGrpCod");
        public static string ITM_DeftItemGrp { get; set; } = HanaQuery("ITEMMASTERDATA", "DeftItemGrp");
        public static string ITM_PSize { get; set; } = HanaQuery("ITEMMASTERDATA", "PSize");
        public static string ITM_Suppliers { get; set; } = HanaQuery("ITEMMASTERDATA", "Supplier");
        public static string ITM_SubCategoryByBrandCategory { get; set; } = HanaQuery("ITEMMASTERDATA", "SubCat");
        public static string ITM_SizesByPSize { get; set; } = HanaQuery("ITEMMASTERDATA", "Size");
        public static string ITM_Style { get; set; } = HanaQuery("ITEMMASTERDATA", "Style");
        public static string ITM_Class { get; set; } = HanaQuery("ITEMMASTERDATA", "Class");
        public static string ITM_SubClass { get; set; } = HanaQuery("ITEMMASTERDATA", "SubClass");
        public static string ITM_Packaging { get; set; } = HanaQuery("ITEMMASTERDATA", "Packaging");
        public static string ITM_Specs { get; set; } = HanaQuery("ITEMMASTERDATA", "Specs");
        public static string ITM_Collect { get; set; } = HanaQuery("ITEMMASTERDATA", "Collect");
        public static string ITM_Company { get; set; } = HanaQuery("ITEMMASTERDATA", "Company");
        public static string ITM_ItemCode { get; set; } = HanaQuery("ITEMMASTERDATA", "ItemCode");
        public static string ITM_Uom { get; set; } = HanaQuery("ITEMMASTERDATA", "UOM");
        public static string ITM_NewDataStyle { get; set; } = HanaQuery("ITEMMASTERDATA", "NewDataStyle");
        public static string ITM_NewDataClass { get; set; } = HanaQuery("ITEMMASTERDATA", "NewDataClass");
        public static string ITM_NewDataSubClass { get; set; } = HanaQuery("ITEMMASTERDATA", "NewDataSubClass");
        public static string ITM_NewDataSpecs { get; set; } = HanaQuery("ITEMMASTERDATA", "NewDataSpecs");
        public static string ITM_NewDataPackage { get; set; } = HanaQuery("ITEMMASTERDATA", "NewDataPackage");
        public static string ITM_NewDataCollect { get; set; } = HanaQuery("ITEMMASTERDATA", "NewDataCollect");
        public static string ITM_NewColor { get; set; } = HanaQuery("ITEMMASTERDATA", "NewColor");
        public static string ITM_NewSize { get; set; } = HanaQuery("ITEMMASTERDATA", "NewSize");
        public static string ITM_NewStyle { get; set; } = HanaQuery("ITEMMASTERDATA", "NewStyle");
        public static string ITM_PSizeByBrand { get; set; } = HanaQuery("ITEMMASTERDATA", "PSizeByBrand");
        public static string ITM_GetItemGrpDetail { get; set; } = HanaQuery("ITEMMASTERDATA", "GetItemGrpDetail");
        public static string ITM_GetItemGrpSetup { get; set; } = HanaQuery("ITEMMASTERDATA", "GetItemGrpSetup");
        public static string ITM_UoMConcession { get; set; } = HanaQuery("ITEMMASTERDATA", "UoMConcession");
        public static string ITM_GetMaxQuantity { get; set; } = HanaQuery("ITEMMASTERDATA", "GetMaxQuantity");
        public static string ITM_GetQuantityPerCarton { get; set; } = HanaQuery("ITEMMASTERDATA", "GetQuantityPerCarton");
        public static string AW_AllocWizRuns { get; set; } = HanaQuery("ALLOCATIONWIZARD", "AllocWizRuns");
        public static string AW_ApprovedRuns { get; set; } = HanaQuery("ALLOCATIONWIZARD", "ApprovedRuns");
        public static string AW_WHS { get; set; } = HanaQuery("ALLOCATIONWIZARD", "WHS");
        public static string AW_MD { get; set; } = HanaQuery("ALLOCATIONWIZARD", "MD");
        public static string AW_RO_MD { get; set; } = HanaQuery("ALLOCATIONWIZARD", "RO_MD");
        public static string AW_IOP { get; set; } = HanaQuery("ALLOCATIONWIZARD", "IOP");
        public static string AW_IOPData { get; set; } = HanaQuery("ALLOCATIONWIZARD", "IOPData");
        public static string AW_GetIOPByCode { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetIOPByCode");
        public static string AW_GetCOPByCode { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetCOPByCode");
        public static string AW_GenItemList { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GenItemList");
        public static string AW_GenMarketingDoc { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GenMarketingDoc");
        public static string AW_GetROStores { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetROStores");
        public static string AW_GetROMarketingDoc { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetROMarketingDoc");
        public static string AW_GetClassify { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetClassify");
        public static string AW_GetItemDetails { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetItemDetails");
        public static string AW_SC { get; set; } = HanaQuery("ALLOCATIONWIZARD", "SC");
        public static string AW_COP { get; set; } = HanaQuery("ALLOCATIONWIZARD", "COP");
        public static string AW_GetCustomer { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetCustomer");
        public static string AW_LVL { get; set; } = HanaQuery("ALLOCATIONWIZARD", "LVL");
        public static string AW_LVL2 { get; set; } = HanaQuery("ALLOCATIONWIZARD", "LVL2");
        public static string AW_GetCustomerList { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetCustomerList");
        public static string AW_ISC { get; set; } = HanaQuery("ALLOCATIONWIZARD", "ISC");
        public static string AW_GetAllocDetails { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetAllocDetails");
        public static string AW_SMRY { get; set; } = HanaQuery("ALLOCATIONWIZARD", "SMRY");
        public static string AW_POST_Series { get; set; } = HanaQuery("ALLOCATIONWIZARD", "POST_Series");
        public static string AW_POST_TransType { get; set; } = HanaQuery("ALLOCATIONWIZARD", "POST_TransType");
        public static string AW_POST_UPDATE { get; set; } = HanaQuery("ALLOCATIONWIZARD", "UPDATE");
        public static string AW_POST_BPWHS { get; set; } = HanaQuery("ALLOCATIONWIZARD", "POST_BPWHS");
        public static string AW_POST_WHS { get; set; } = HanaQuery("ALLOCATIONWIZARD", "POST_WHS");
        public static string AW_POST_BPAddID { get; set; } = HanaQuery("ALLOCATIONWIZARD", "POST_BPAddID");
        public static string AW_GetClassicByBP { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetClassicByBP");
        public static string AW_GetITRbyDocEntry { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetITRbyItemCode");
        public static string AW_UPDATE_GetITRbyItemCode { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetITRbyItemCode");
        public static string AW_GetITRbyRunID { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetITRbyRunID");
        public static string AW_GetITRAppbyRunID { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetITRAppbyRunID");
        public static string AW_GetITRIDbyRunID { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetITRIDbyRunID");
        public static string AW_GetUserAccess { get; set; } = HanaQuery("ALLOCATIONWIZARD", "GetUserAccess");
        public static string AW_Company { get; set; } = HanaQuery("ALLOCATIONWIZARD", "Company");

        public static string US_EnterBarCode { get; set; } = HanaQuery("UNOFFICIALSALES", "EnterBarCode");

        public static string US_BarCodeExist { get; set; } = HanaQuery("UNOFFICIALSALES", "BarCodeExist");

        public static string US_BarCodeNotExist { get; set; } = HanaQuery("UNOFFICIALSALES", "BarCodeNotExist");

        public static string US_BarCodeList { get; set; } = HanaQuery("UNOFFICIALSALES", "BarCodeList");

        public static string UDF_FMS { get; set; } = HanaQuery("UDF", "UDF_FMS");

        public static string DFLT_ItemDetails { get; set; } = GetHanaQuery(HanaQuery("ITEM", "Details"), 1);

        public static string DFLT_ItemWhs { get; set; } = GetHanaQuery(HanaQuery("ITEM", "Whs"), 1);
        public static string DFLT_ItemProject { get; set; } = GetHanaQuery(HanaQuery("ITEM", "Project"), 1);
        public static string DFLT_ItemCompany { get; set; } = HanaQuery("ITEM", "Company");
        public static string BP_BarcodeAll { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeAll");
        public static string BP_BarcodeAll_CheckBPCode { get; set; } = HanaQuery("BARCODEPRINTING", "CheckBPCode");
        public static string BP_BarcodeAll_GetPath { get; set; } = HanaQuery("BARCODEPRINTING", "GetPath");
        public static string BP_BarcodeAll_BarcodeFromOITM { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeFromOITM");
        public static string BP_BarcodeAll_BarcodeLineItemsITR { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeLineItemsITR");
        public static string BP_BarcodeAll_BarcodeLineItemsITRNM { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeLineItemsITRNM");
        public static string BP_BarcodeAll_BarcodeLineItemsSO { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeLineItemsSO");
        public static string BP_BarcodeAll_BarcodeLineItemsSONM { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeLineItemsSONM");
        public static string BP_BarcodeAll_BarcodeLineItemsSOITR { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeLineItemsSOITR");
        public static string BP_BarcodeAll_ITRSOList { get; set; } = HanaQuery("BARCODEPRINTING", "ITRSOList");
        public static string BP_BarcodeAll_CheckDocDueDate { get; set; } = HanaQuery("BARCODEPRINTING", "CheckDocDueDate");
        public static string BP_BarcodeAll_CheckDocDueDateNM { get; set; } = HanaQuery("BARCODEPRINTING", "CheckDocDueDateNM");
        public static string BP_BarcodeAll_BarcodeLineItemsPO { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeLineItemsPO");
        public static string BP_BarcodeAll_BarcodeLineItemsPOforUPC { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeLineItemsPOforUPC");
        public static string BP_BarcodeAll_BarcodeFromOITMandPO { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeFromOITMandPO");
        public static string BP_BarcodeAll_BarcodeFromOITMandPOforUPC { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeFromOITMandPOforUPC");
        public static string BP_BarcodeAll_CheckExistingCampaigns { get; set; } = HanaQuery("BARCODEPRINTING", "CheckExistingCampaigns");
        public static string BP_BarcodeAll_GetExistingCampaigns { get; set; } = HanaQuery("BARCODEPRINTING", "GetExistingCampaigns");
        public static string BP_BarcodeAll_CheckItemIfExistingCampaign { get; set; } = HanaQuery("BARCODEPRINTING", "CheckItemIfExistingCampaign");
        public static string BP_BarcodeAll_CheckDocDueDateWithCpnNo { get; set; } = HanaQuery("BARCODEPRINTING", "CheckDocDueDateWithCpnNo");
        public static string BP_BarcodeAll_WithCpnNo { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeAllWithCpnNo");
        public static string BP_BarcodeAll_BarcodeLineItemsITRCpoStore { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeLineItemsITRCpoStore");
        public static string BP_BarcodeAll_BarcodeFromOITMandITRCpo { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeFromOITMandITRCpo");

        public static string BP_BarcodeAll_BarcodeFromOITMandITRforUPC { get; set; } = HanaQuery("BARCODEPRINTING", "BarcodeFromOITMandITRforUPC");

        public static string query(int conditionnumber, string code, string item)
        {
            return $"SELECT U_Condition{conditionnumber} FROM [@A_MAINTENANCE_LINES] WHERE Code = '{code}' AND U_Table = '{item}'";
        }

        public static string HanaQuery(string sModule,string sParameter)
        {
            return $@"CALL ""USR_SP_SAO_{sModule}"" ('{sParameter}')";
        }

        static string GetHanaQuery(string Query, int sColumn)
        {
            string ret = "";
            try
            {
                var hana = new SAPHanaAccess();
                var helper = new DataHelper();

                ret = helper.ReadDataRow(hana.Get(Query), sColumn, "", 0);
                
            }
            catch { }
            return ret;
        }



        public static string HanaQuery(string sModule, string sParameter, string sParameter2)
        {
            return $@"CALL ""USR_SP_SAO_{sModule}"" ('{sParameter}', '{sParameter2}')";
        }

        public static string query(int conditionnumber, string code)
        {
            return $"SELECT U_Condition{conditionnumber} FROM [@A_MAINTENANCE_LINES] WHERE Code = '{code}'";
        }
        
        public static string queryMain (int conditionnumber , string code)
        {
            return $"SELECT U_Condition{conditionnumber} FROM [@A_MAINTENANCE] WHERE Code = '{code}'";
        }

        public static string UDF(string table)
        {
            var hana = new SAPHanaAccess();
            var helper = new DataHelper();
            string fields = helper.ReadDataRow(hana.Get(query(1, "UDF", table)), 0, "", 0);
            return $"SELECT 'U_' + AliasID [AliasID], EditSize , TypeID , FieldID, '@' + ISNULL(RTable,RelUDO) [UserDefined], ISNULL(RelSO,'') [Table] , Dflt , Descr FROM CUFD WHERE TableID = '{table}' AND AliasID IN ({fields}) ORDER BY CASE AliasID {DECLARE.ArrangeUDF(fields)} END";
        }

        public static string UDFDetails(string table, string field)
        {
            return $"SELECT '' [Code] ,'' [Name] UNION SELECT FldValue, Descr FROM UFD1 WHERE TableID ='{table}' AND FieldID = '{field}'";
        }

        public static string UDFQuery(string table, string field)
        {
            return $"SELECT QString FROM OUQR WHERE IntrnalKey = (SELECT DISTINCT QueryId FROM CSHS WHERE ActionT = '2' AND FormID = '{table}' AND ItemID = '{field}')";
        }

        public static string UDFValidValues(string table, string field)
        {
            return $"SELECT '' [Code] ,'' [Name] UNION SELECT B.Value ,B.Value  FROM CSHS A INNER JOIN CUVV B ON A.IndexID = B.IndexID WHERE A.ActionT = '1' AND A.FormID = '{table}' AND A.ItemID = '{field}'";
        }

        public static string UDFUserDefined(string table)
        {
            return $"SELECT '' [Code] ,'' [Name] UNION SELECT SELECT Code, Name FROM [{table}]";
        }
    }
}

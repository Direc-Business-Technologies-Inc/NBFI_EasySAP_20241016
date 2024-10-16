using System;
using System.Data;
using System.Collections.Generic;
using DirecLayer;
using PresenterLayer.Helper;
using DomainLayer.Helper;

namespace PresenterLayer.Services
{
    class UploadController
    {
        DataContextList ContextList = new DataContextList();
        //public frmUploader frmUpload { get; set; }
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        public string tableName { get; set; }

        public UploadController()
        {
            hana = new SAPHanaAccess();
            helper = new DataHelper();
            msSql = new SAPMsSqlAccess();
        }

        public DataTable GetUploadType()
        {
            string tblName = string.Empty, query = string.Empty;

            var dt = new DataTable();
            if (tableName == "Inventory Counting")
            {
                AppConfig appConfig = new AppConfig();
                var msSqlDb = appConfig.AppSettings("SqlDatabase");
                query = "SELECT b.ORDINAL_POSITION[Position], b.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLES A " +
                        "INNER JOIN INFORMATION_SCHEMA.COLUMNS b on b.TABLE_NAME = a.TABLE_NAME " +
                        $"WHERE a.TABLE_CATALOG = '{msSqlDb}' AND a.Table_Name = 'OINC' ORDER BY b.ORDINAL_POSITION";
                dt = msSql.Get(query);
            }
            else
            {
                var sboCred = new SboCredentials();
                query += $"SELECT POSITION, COLUMN_NAME FROM SYS.COLUMNS WHERE SCHEMA_NAME = '{sboCred.Database}' ";

                switch (tableName)
                {
                    case "Sales Quotation":
                        query += " AND TABLE_NAME = 'OQUT' ORDER BY POSITION";
                        break;
                    // sales
                    case "Sales Order":
                        query += " AND TABLE_NAME = 'ORDR' ORDER BY POSITION";
                        break;

                    // invoice
                    case "A/R invoice":
                        query += " AND TABLE_NAME = 'OINV' ORDER BY POSITION";
                        break;

                    case "Delivery":
                        query += " AND TABLE_NAME = 'ODLN' ORDER BY POSITION";
                        break;

                    case "Purchase Order":
                        query += " AND TABLE_NAME = 'OPOR' ORDER BY POSITION";
                        break;

                    case "Goods Receipt PO":
                        query += " AND TABLE_NAME = 'OPDN' ORDER BY POSITION";
                        break;

                    case "A/P Invoice":
                        query += " AND TABLE_NAME = 'OPCH' ORDER BY POSITION";
                        break;

                    case "Carton Packing List":
                        query += " AND TABLE_NAME = '@CM_HEADER' ORDER BY POSITION";
                        break;

                    case "Inventory Counting":
                        query += " AND TABLE_NAME = 'OINC' ORDER BY POSITION";
                        break;

                    case "Inventory Transfer Request":
                        query += " AND TABLE_NAME = 'OWTR' ORDER BY POSITION";
                        break;
                    case "A/R Credit Memo":
                        query += " AND TABLE_NAME = 'ORIN' ORDER BY POSITION";
                        break;
                }
                
                dt = hana.Get(query);
            }

            return dt;
        }

        public DataTable GetUploadRowType()
        {
            var sboCred = new SboCredentials();
            string query = $"SELECT POSITION, COLUMN_NAME FROM SYS.COLUMNS WHERE SCHEMA_NAME = '{sboCred.Database}' ";

            switch (tableName)
            {
                case "Sales Order":
                    query += " AND TABLE_NAME = 'RDR1' ORDER BY POSITION";
                    break;

                case "Sales Quotation":
                    query += " AND TABLE_NAME = 'QUT1' ORDER BY POSITION";
                    break;

                case "Delivery":
                    query += " AND TABLE_NAME = 'DLN1' ORDER BY POSITION";
                    break;

                case "Carton Packing List":
                    query += " AND TABLE_NAME = '@CM_ROWS' ORDER BY POSITION";
                    break;

                case "Inventory Counting":
                    query += " AND TABLE_NAME = 'INC1' ORDER BY POSITION";
                    break;

                case "Inventory Transfer Request":
                    query += " AND TABLE_NAME = 'WTR1' ORDER BY POSITION";
                    break;

                case "A/R invoice":
                    query += " AND TABLE_NAME = 'INV1' ORDER BY POSITION";
                    break;

                case "A/R Credit Memo":
                    query += " AND TABLE_NAME = 'RIN1' ORDER BY POSITION";
                    break;


            }
            
            return hana.Get(query);
        }

        //public List<TemplateFields> PopulateColumnList(DataSet ds, int tblIndex)
        //{
        //    ContextList.columnList.Clear();
        //    ContextList.templateFields.Clear();

        //    foreach (var column in ds.Tables[tblIndex].Columns)
        //    {
        //        string columnName = Regex.Replace(column.ToString(), @"[\d-]", "");
        //        ContextList.columnList.Add(new ColumnList { columnName = Regex.Replace(columnName, "_", "") });
        //    }

        //    ContextList.columnList.Select(x => x.columnName).Distinct().ToList().ForEach(x =>
        //    {
        //        ContextList.templateFields.Add(new TemplateFields { HeaderName = x, Id = null });
        //    });

        //    return ContextList.templateFields;
        //}

        //public List<TemplateFields> ReturnTemplateField()
        //{
        //    return ContextList.templateFields;
        //}

        //public List<ColumnList> ReturnColumnList()
        //{
        //    return ContextList.columnList;
        //}

        //public string SKUupload(string itemCode)
        //{
        //    string Code = itemCode;

        //    for (int i = 1; 3 > i; i++)
        //    {
        //        string query = SP.query(i, "DT_ItemCode");

        //        if (SAPHana.Get(query.Replace("@ItemCode", Code)).Rows.Count <= 1)
        //        {
        //            Code = DataAccess.SearchData(DataAccess.conStr("HANA"), query.Replace("@ItemCode", Code), 0, "ItemCode");

        //            if (Code != string.Empty)
        //            {
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    return Code;
        //}

        //public string WhsUpload(string itemCode)
        //{
        //    string Code = itemCode;

        //    string query = SP.queryMain(1, "DT_Whs");

        //    Code = DataAccess.SearchData(DataAccess.conStr("HANA"), query.Replace("@CardCode", Code), 0, "U_Whs");

        //    return Code;
        //}

        public string CardName(string bp)
        {
            string cardname;

            string bpCount = $"SELECT CardName from OCRD where CardCode = '{bp}'";
            var dt = hana.Get(bpCount);
            if (helper.DataTableExist(dt))
            {
                cardname = helper.ReadDataRow(dt, "CardName", "", 0);
            }
            else
            {
                cardname = bp;
            }

            return cardname;
        }

        public string DeliveryBP(string bp)
        {
            string cardCode = "";

            string bpCount = $"SELECT CardCode from OCRD where AddID = '{bp}'";
            var dt = hana.Get(bpCount);
            if (helper.DataTableExist(dt))
            {
                cardCode = helper.ReadDataRow(dt, "CardCode", "", 0);
            }
            else
            {
                cardCode = bp;
            }

            return cardCode;
        }

        public List<string> BpCardProjectCode(string bp, bool isSMPO)
        {
            List<string> info = new List<string>();

            string bpCount = $"SELECT CardCode, ProjectCod from OCRD where AddID = '{bp}' {(isSMPO ? "AND U_OrderClass = 'Outright'" : "")}";


            var dt = hana.Get(bpCount);

            if (dt.Rows.Count > 0)
            {
                info.Add(ValidateInput.String(dt.Rows[0][0]));
                info.Add(ValidateInput.String(dt.Rows[0][1]));
            }
            else
            {
                info.Add(bp);
                info.Add("");
            }

            return info;
        }

        public string DeliveryWhsCode(string sCardCode)
        {
            var output = "";
            var sQuery = string.Format(SP.DFLT_ItemWhs, sCardCode);
            output = helper.ReadDataRow(hana.Get(sQuery), 0, "", 0);


            //string query = $"SELECT Distinct U_Whs FROM CRD1 where CardCode = '{bp}'";

            return output; //DataAccess.SearchData(DataAccess.conStr("HANA"), query, 0, "U_Whs");
        }

        public string SalesOrderWhsCode(string type)
        {
            var output = "";
            var sQuery = $"SELECT U_WhsCode FROM [@DOC_TYPE] WHERE Code = '{type}'";
            output = helper.ReadDataRow(hana.Get(sQuery), 0, "", 0);


            //string query = $"SELECT Distinct U_Whs FROM CRD1 where CardCode = '{bp}'";

            return output; //DataAccess.SearchData(DataAccess.conStr("HANA"), query, 0, "U_Whs");
        }
        public string VatGroup(string sItemCode )
        {
            var output = "";
            var sQuery = $"SELECT VatGourpSa FROM OITM T0 WHERE T0.ItemCode = '{sItemCode}'";
            output = helper.ReadDataRow(hana.Get(sQuery), 0, "", 0);

            return output; //DataAccess.SearchData(DataAccess.conStr("HANA"), query, 0, "U_Whs");
        }
        public string WhtTax(string sItemCode)
        {
            var output = "";
            var sQuery = $"SELECT WTLiable FROM OITM T0 WHERE T0.ItemCode = '{sItemCode}'";
            output = helper.ReadDataRow(hana.Get(sQuery), 0, "", 0);

            return output; //DataAccess.SearchData(DataAccess.conStr("HANA"), query, 0, "U_Whs");
        }
        public string MerchCredit(string sItemCode)
        {
            var output = "";
            var sQuery = $"SELECT T0.QryGroup4, T0.ItemCode FROM OITM T0 WHERE T0.ItemCode = '{sItemCode}'";
            output = helper.ReadDataRow(hana.Get(sQuery), 0, "", 0);

            return output; //DataAccess.SearchData(DataAccess.conStr("HANA"), query, 0, "U_Whs");
        }

        public List<string> DeliveryInfo(string sItemCode)
        {
            List<string> lItemDetails = new List<string>();

           //string query = $"SELECT U_Dim2, U_ID002, U_ID003 FROM OITM Where ItemCode = '{code}'";
           
            var sQuery = string.Format(SP.DFLT_ItemDetails, sItemCode);
            
            var dt = hana.Get(sQuery);

            //var dt = DataAccess.Select(DataAccess.conStr("HANA"), query);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    lItemDetails.Add(ValidateInput.String(dt.Rows[0]["Brand"]));
                    lItemDetails.Add(ValidateInput.String(dt.Rows[0]["Department"]));
                    lItemDetails.Add(ValidateInput.String(dt.Rows[0]["SubDepartment"]));
                    lItemDetails.Add(ValidateInput.String(dt.Rows[0]["Color"]));
                    lItemDetails.Add(ValidateInput.String(dt.Rows[0]["Size"]));
                    lItemDetails.Add(ValidateInput.String(dt.Rows[0]["Style"]));
                    lItemDetails.Add(ValidateInput.String(dt.Rows[0]["Sort"]));
                }
                else
                {
                    lItemDetails.Add("");
                    lItemDetails.Add("");
                    lItemDetails.Add("");
                    lItemDetails.Add("");
                    lItemDetails.Add("");
                    lItemDetails.Add("");
                    lItemDetails.Add("");
                }
            }
            
               
            return lItemDetails;
        }

        public string GetProjectCode(string sCardCode)
        {
            //string query = $"SELECT ProjectCod FROM OCRD where CardCode = '{bp}'";
            var sQuery = string.Format(SP.DFLT_ItemProject, sCardCode);
            var output = helper.ReadDataRow(hana.Get(sQuery), 0, "", 0);

            return output;//DataAccess.SearchData(DataAccess.conStr("HANA"), query, 0, "ProjectCod");
        }

        public string DeliveryItem(string sCardCode, string sItemCode)
        {
            var output = "";

            //var sQuery = string.Format(SP.DFLT_ItemCode, sItemCode, sCardCode);

            //output = SAPHana.GetQuery(sQuery, 1);

            var query = "SELECT T2.ItemCode FROM OCPN T0 " +
                            "INNER JOIN CPN1 T1 ON T0.CpnNo = T1.CpnNo " +
                            "INNER JOIN CPN2 T2 ON T0.CpnNo = T2.CpnNo " +
                            "INNER JOIN OITM T3 ON T2.ItemCode = T3.ItemCode " +
                            $"WHERE T0.U_SKUType = 'One-one' AND T0.U_CType = 'SKU' AND T1.BpCode = '{sCardCode}' AND U_SKU = '{sItemCode}' AND T0.Status = 'O' " +
                            $"OR T0.U_SKUType = 'One-one' AND T0.U_CType = 'SKU'AND T1.BpCode = '{sCardCode}' AND T2.ItemName = '{sItemCode}' AND T0.Status = 'O' ";
            var dt = hana.Get(query);

            var skuResult = helper.ReadDataRow(dt, "ItemCode","",0);

            if (skuResult != string.Empty)
            {
                output = skuResult;
            }
            else
            {
                var barCode = helper.ReadDataRow(hana.Get($"SELECT ItemCode FROM OITM where CodeBars = '{sItemCode}'"), "ItemCode", "", 0);

                output = barCode != "" ? barCode : sItemCode;
            }

            return output;
        }
        public string DeliveryItemARCM(string sCardCode, string sItemCode)
        {
            var output = "";

            //var sQuery = string.Format(SP.DFLT_ItemCode, sItemCode, sCardCode);

            //output = SAPHana.GetQuery(sQuery, 1);

            var query = "SELECT T2.ItemCode FROM OCPN T0 " +
                            "INNER JOIN CPN1 T1 ON T0.CpnNo = T1.CpnNo " +
                            "INNER JOIN CPN2 T2 ON T0.CpnNo = T2.CpnNo " +
                            "INNER JOIN OITM T3 ON T2.ItemCode = T3.ItemCode " +
                            $"WHERE T0.U_SKUType = 'Style Level' AND T0.U_CType = 'SKU' AND T1.BpCode = '{sCardCode}' AND U_SKU = '{sItemCode}' AND T0.Status = 'O' ";
            var dt = hana.Get(query);
            var skuResult = helper.ReadDataRow(dt, "ItemCode", "", 0);

            if (skuResult != string.Empty)
            {
                output = skuResult;
            }
            else
            {
                var barCode = helper.ReadDataRow(hana.Get($"SELECT ItemCode FROM OITM where CodeBars = '{sItemCode}'"), "ItemCode", "", 0);

                output = barCode != "" ? barCode : sItemCode;
            }

            return output;
        }

        public string BrandName(string itemcode)
        {
            return helper.ReadDataRow(hana.Get($"SELECT U_ID001 FROM OITM where ItemCode = '{itemcode}'"), "ItemCode", "", 0);
        }

        public string ItemName(string itemcode)
        {
            return helper.ReadDataRow(hana.Get($"SELECT ItemName FROM OITM where ItemCode = '{itemcode}'"), "ItemName", "", 0);
        }

        public string FinditemCodeByCodeBars(string itemCode)
        {
            var result = helper.ReadDataRow(hana.Get($"SELECT ItemCode FROM OITM where CodeBars = '{itemCode}'"), "ItemCode", "", 0);

            if (result == string.Empty)
            {
                result = itemCode;
            }

            return result;
        }

        public int UomEntry(string uomcode)
        {
            int UomEntry = -1;
            var UomResult = helper.ReadDataRow(hana.Get($"SELECT TOP 1 UomEntry FROM OUOM WHERE UomCode = '{uomcode}'"), "ItemCode", "", 0);

            if (UomResult != "")
            {
                UomEntry = Convert.ToInt32(UomResult);
            }

            return UomEntry;
        }

        public string AddId(string cardCode)
        {
            return helper.ReadDataRow(hana.Get($"SELECT TOP 1 Address FROM CRD1 WHERE CardCode = '{cardCode}'"), "Address", "", 0);
        }

        internal List<string> GetDeliveryInfo(string item)
        {
            throw new NotImplementedException();
        }

        public string SI(string runID)
        {
            return helper.ReadDataRow(hana.Get($"SELECT TOP 1 U_SINo + 1 FROM OINV WHERE U_UploadID = '{runID}' Order by DocEntry desc "), "U_SINo+1", "", 0);
        }
    }
}

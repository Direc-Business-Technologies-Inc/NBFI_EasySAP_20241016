using DirecLayer;
using DomainLayer.Models;
using PresenterLayer;
using System;
using System.Collections.Generic;
using PresenterLayer.Helper;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace DirecLayer._03_Repository
{
    public class StringQueryRepository
    {
        internal string GetCartonListItem(string docEntry)
        {
            string query = $"SELECT U_DocEntry, U_CartonNo, U_DocRef, U_Ref1, U_Ref2, U_Remarks FROM [@CL_ROWS] WHERE DocEntry ='{docEntry}' ";

            return query;
        }

        public string BasedDocumentCartonList(string table, string docEntry)
        {
            string query = $"select U_CartonList from O{table} where DocEntry = '{docEntry}'";

            return query;
        }

        internal string GLJobOrder()
        {
            string query = "SELECT AcctCode [Code], AcctName [Name] FROM OACT Where Postable = 'Y' AND AcctCode = '200-197'";

            return query;
        }

        internal string GetGLAccount(string TransType)
        {
            string query = $"SELECT AcctCode [Code], AcctName [Name] FROM OACT Where Postable = 'Y' AND AcctCode = (select ISNULL(U_AcctCode,'') [U_AcctCode] from [@TRANSACT_TYPE] where Code = '{TransType}')";

            return query;
        }

        internal string CurrencyCode()
        {
            string query = "SELECT Distinct CurrCode FROM OCRN";

            return query;
        }
        public static string GetChain(string Chain)
        {
            return $"SELECT GroupCode [Group Code] FROM OCRG Where GroupName = '{Chain}'";
        }
        internal string CurrencyRate(string code)
        {
            string query = $"SELECT Rate FROM ORTT WHERE Currency = '{code}' And RateDate = (SELECT max(RateDate) FROM ORTT)";

            return query;
        }

        public string InventoryWarehouse(string tableName, string basedId)
        {
            return $"SELECT Filler, ToWhsCode FROM O{tableName} WHERE DocEntry = '{basedId}'";
        }

        public string VatGroupRate(string code)
        {
            return $"SELECT Z.Rate FROM OVTG Z Where Z.Code =  '{code}'";
        }


        public string BpProjectCode(string code)
        {
            return $"SELECT Distinct(T0.ProjectCod) [Project] FROM OCRD T0 where T0.CardCode = '{code}'";
        }

        public string TransferType()
        {
            return "SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [@TRANSFER_TYPE] Order by Name";
        }

        public static string TransType(string name)
        {
            return $"SELECT * FROM (SELECT Code FROM [@TRANSACT_TYPE] WHERE Name = '{name}' " +
                        "UNION ALL " +
                        $"SELECT Code FROM [@TRANSFER_TYPE] WHERE Name = '{name}' " +
                        $"UNION ALL SELECT Code FROM [@DOC_TYPE] WHERE Name = '{name}') A " +
                        "ORDER BY A.Code";
        }

        public string UdfOrderNo(string code)
        {
            string query = "SELECT ifnull(b.Notes, '') + ' - ' + count(a.U_OrderNo) + 1 " +

                "FROM OPOR a " +

                "INNER JOIN OCRD b on a.CardCode = b.CardCode " +

                $"WHERE a.CardCode = '{code}' " +

                "and a.CANCELED = 'N' " +

                "GROUP BY b.Notes";

            return query;
        }

        public string UomEntry(string Uom)
        {
            return $"SELECT UomEntry FROM OUOM Where UomName = '{Uom}'";
        }

        public string UserApprovalCheck()
        {

            string query = $"SELECT Distinct UserID FROM OWTM T0 INNER JOIN WTM1 T1 ON T0.WtmCode = T1.WtmCode WHERE T0.Active = 'Y' AND UserID = (SELECT USERID FROM OUSR WHERE USER_CODE = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}')";

            return query;
        }

        public string BpTransferType(string code)
        {
            string query = $"SELECT CASE WHEN QryGroup2 = 'Y' THEN 'IMP' ELSE 'LOC' END AS [TransType] FROM OCRD WHERE CardCode = '{code}'";

            return query;
        }

        public string Brands(string itemCode)
        {
            string query = "SELECT (SELECT Z.Code FROM [@OBND] Z WHERE Z.Code = A.U_ID001) [BrandCode], " +

            $"(SELECT Z.Name FROM [@OBND] Z WHERE Z.Code = A.U_ID001) [Brand] FROM OITM A Where ItemCode = '{itemCode}'";

            return query;
        }

        public string BpAccount(string gl)
        {
            string query = $"SELECT AcctCode, AcctName FROM OACT Where AcctCode = '{gl}'";

            return query;
        }

        public string PurchaseOrder(string table, string docEntry)
        {
            string query = "Select T0.DocEntry, T0.DocNum, T0.CardCode, T0.CardName, T0.DocStatus, T0.DiscPrcnt, " +

            "T0.DocCur, T0.NumAtCard, T0.U_Department, T0.U_Remarks, DocRate, " +

            "(SELECT Z.SeriesName FROM NNM1 Z WHERE Z.Series = T0.Series) [Series]," +

            "(SELECT Z.Name FROM [@CMP_INFO] Z WHERE Z.Code = T0.U_CompanyTIN) [U_CompanyTIN], " +

            "T0.DocDate, T0.TaxDate, T0.DocDueDate, T0.CancelDate, T0.CANCELED, T0.Comments, T0.DocType, T0.U_DocType, T0.ShipToCode, T0.Address2 " +

            $"FROM O{table} T0 WHERE DocEntry = '{docEntry}'";

            return query;
        }

        internal string DraftLatestId()
        {
            string query = "SELECT TOP 1 DocEntry FROM ODRF";

            //string query = "SELECT TOP 1 T1.DocEntry " +

            //    "FROM OWDD T0 INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry " +

            //    $"WHERE T0.UserSign = (SELECT USERID FROM OUSR WHERE USER_CODE = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}') AND CANCELED = 'N' " +

            //    "ORDER BY DocEntry DESC";

            return query;
        }

        internal string GetDummyUser(string returnvalue)
        {
            //string query = "SELECT Distinct T1.U_ApproverUserCode " +

            //    "FROM OWDD T0 INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry " +

            //    $"WHERE T1.DocEntry = '{returnvalue}' AND Status = 'W'";

            string query = $"SELECT Distinct U_ApproverUserCode FROM ODRF WHERE DocEntry = '{returnvalue}'";

            return query;
        }

        public string PurchaserOrderDraft(string docEntry, string docStatus)
        {
            string query = "SELECT T1.DocEntry, T1.DocNum, T1.CardCode, T1.CardName, T1.CardName, T1.DocStatus, T1.DiscPrcnt, " +

            "T1.DocCur, T1.NumAtCard, T1.U_Department, T1.U_Remarks, DocRate," +

            "(SELECT Z.SeriesName FROM NNM1 Z WHERE Z.Series = T1.Series) [Series]," +

            "(SELECT Z.Name FROM [@CMP_INFO] Z WHERE Z.Code = T1.U_CompanyTIN) [U_CompanyTIN], " +

            "T1.DocDate, T1.TaxDate, T1.DocDueDate, T1.CancelDate, T1.CANCELED, T1.Comments, T1.DocType " +

            "FROM OWDD T0 " +

            "INNER JOIN ODRF T1 ON T0.DraftEntry = T1.DocEntry " +

            $"WHERE T1.DocEntry = '{docEntry}'";

            query += docStatus == "Draft-Pending" ? " AND Status = 'W'" : docStatus == "Draft-Rejected" ? " AND Status = 'N'" : " AND Status = 'Y'";

            return query;
        }

        public string PurchaseOrderLinesItem(string table, string docEntry)
        {
            string query = "SELECT T0.LineNum, T0.ItemCode [Item No.], T0.Dscription [Item Description], T0.Dscription, T0.U_Style [Style], " +

            "T0.UomCode [UOM], T0.U_PricetagCount [Pricetag Count], T0.U_Remarks [Remarks], T0.OcrCode2 [Brand], " +

            "U_ChainDescription [Chain Desc], '' [AcctCode], T0.Quantity [Quantity], T0.OpenQty," +

            "T0.Price [Unit Price], T0.DiscPrcnt, " +

            "T0.LineTotal, T0.WhsCode, T0.SlpCode, T0.PriceBefDi, T0.PriceAfVAT, T0.LineTotal [INMPrice], T0.GTotal, T0.Project, T0.VatGroup, " +

            "T0.VatPrcnt, T0.CodeBars, T0.TaxCode, T0.VatAppld, T0.LineVat, T0.U_Chain, " +

            $"T1.U_ID012, (SELECT Z.Name FROM [@OCLC] Z WHERE Z.Code = T1.U_ID022) [U_ID022], T1.U_ID018, T1.U_ID007, T1.CodeBars, '' [OrderEntry]" +

            $" , ISNULL((select Name from [@CMP_INFO] where Code = T0.U_Company),'') [U_Company] FROM {table}1 T0 " +

            $"INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode AND T0.LineStatus = 'O' Where T0.DocEntry = '{docEntry}'";

            return query;
        }

        public string PurchaseOrderLinesService(string table, string docEntry)
        {
            string query = "Select T0.LineNum, T0.Dscription [Item No.], T0.U_Description [Item Description], T0.U_Style [Style], " +

                   "T0.U_UOM [UOM], T0.U_PricetagCount [Pricetag Count], T0.U_Remarks [Remarks], T0.OcrCode2 [Brand]," +

                   " U_ChainDescription [Chain Desc], AcctCode [AcctCode], T0.U_Qty [Quantity], T0.Price [Unit Price], T0.DiscPrcnt, T0.LineTotal, " +

                   "T0.U_TargetWhs [WhsCode], T0.SlpCode, T0.PriceBefDi, T0.PriceAfVAT, T0.LineTotal [INMPrice], T0.GTotal, T0.Project, T0.VatGroup, T0.VatPrcnt, T0.U_OldItemNo [CodeBars], " +

                   "U_Style [U_ID012], U_Color [U_ID022], U_Size [U_ID007], U_PricetagCount [Pricetag Count], U_Style [U_ID012], T0.U_GrossPricePerPiece, T0.U_UnitPricePerPiece, " +

                   "T0.TaxCode, T0.VatAppld, T0.LineVat, T0.U_Chain " +

                   $"from O{table} A INNER JOIN {table}1 T0 ON T0.DocEntry = A.DocEntry " +

                   $"Where A.DocType = 'S' AND A.DocEntry = '{docEntry}'";

            return query;
        }
        public string IsWarehouseMethod(string ItemCode)
        {
            return $"SELECT * FROM OITM WHERE ItemCode = '{ItemCode}' AND GLMethod = 'W'";
        }
        public string GetGLAccountWhs(string Warehouse)
        {
            return $"SELECT B.AcctCode,B.AcctName FROM OWHS A INNER JOIN OACT B ON B.AcctCode = A.BalInvntAc WHERE A.WhsCode = '{Warehouse}'";
        }
        public string PurchaseOrderLinesItemDraft(string docEntry, string docStatus)
        {
            string query = "SELECT Distinct T1.LineNum, T1.ItemCode [Item No.], T2.ItemName [Item Description], T1.Dscription, T1.U_Style [Style], " +

                    "T1..UomCode[UOM], T1.U_PricetagCount[Pricetag Count], T1.U_Remarks[Remarks], T1.OcrCode2[Brand], T1.U_ChainDescription[Chain Desc], " +

                    "'' [AcctCode], T1.Quantity, T1.Price[Unit Price], T1.DiscPrcnt, T1.LineTotal, T1.WhsCode, T1.SlpCode, T1.PriceBefDi, T1.PriceAfVAT, T1.LineTotal [INMPrice], " +

                    "T1.GTotal, T1.Project, T1.VatGroup, T1.OpenQty, " +

                    "T1.VatPrcnt, T1.CodeBars, T1.TaxCode, T1.VatAppld, T1.LineVat, T1.U_Chain, " +

                    "T2.U_ID012, (SELECT Z.Name FROM [@OCLC] Z WHERE Z.Code = T2.U_ID022) [U_ID022], T2.U_ID018, T2.U_ID007, T1.CodeBars " +

                    "FROM OWDD T0 " +

                    "INNER JOIN DRF1 T1 ON T0.DraftEntry = T1.DocEntry " +

                    "INNER JOIN OITM T2 ON T1.ItemCode = T2.ItemCode " +

                    $"WHERE T1.DocEntry = '{docEntry}'";

            query += docStatus == "Draft-Approved" ? " AND Status = 'Y'" : docStatus == "Draft-Rejected" ? " AND Status = 'N'" : " AND Status = 'W'";

            return query;
        }

        public string PurchaseOrderLinesServiceDraft(string docEntry)
        {
            string query = "Select T0.LineNum, T0.Dscription[Item No.], T1.ItemName [Item Description], " +

                    "T0.U_Style [Style], T0.U_UOM [UOM], T0.U_PricetagCount [Pricetag Count], T0.U_Remarks [Remarks], T0.OcrCode2 [Brand], U_ChainDescription [Chain Desc], " +

                    "AcctCode [AcctCode], T0.U_Qty [Quantity], T0.Price[Unit Price], T0.DiscPrcnt, T0.LineTotal" +

                    ", T0.LineTotal [INMPrice], T0.GTotal, T0.U_GrossPricePerPiece, T0.U_UnitPricePerPiece" +

                    ", T0.U_TargetWhs [WhsCode], T0.SlpCode, T0.PriceBefDi, T0.Project, T0.VatGroup, " +

                    "T0.VatPrcnt, T0.CodeBars, T0.PriceAfVAT, T0.TaxCode, T0.VatAppld, T0.LineVat, T0.U_Chain, T1.U_ID012, (SELECT Z.Name FROM [@OCLC] Z WHERE Z.Code = T1.U_ID022) [U_ID022], T1.U_ID018, T1.U_ID007, T1.CodeBars " +

                    "FROM ODRF A " +

                    "INNER JOIN DRF1 T0 ON T0.DocEntry = A.DocEntry " +

                    "INNER JOIN OITM T1 ON T1.ItemCode = T0.Dscription " +

                    $"where A.DocType = 'S' AND A.DocEntry = '{docEntry}'";

            return query;
        }

        public string GoodReceiptExistingDocs()
        {
            string query = "SELECT A.DocEntry, " +

                        "CASE " +
                        "WHEN A.CANCELED = 'Y' THEN 'Canceled' " +
                        "WHEN A.CANCELED = 'N' AND A.DocStatus != 'O' THEN 'Closed'  " +
                        "WHEN A.DocStatus = 'O' THEN 'Open' END [Status], " +

                        "A.DocNum [Doc No.], A.CardCode [BP Code], A.CardName [BP Name], A.DocDate [Posting Date], " +

                        "A.DocDueDate [Delivery Date],TO_DECIMAL(A.DocTotal, 10, 2) [Total],(SELECT TO_DECIMAL(Sum(Z.Quantity),10,2) From POR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OPDN A" +

                        " UNION ALL " +

                        "SELECT Distinct T1.DocEntry, CASE WHEN Status = 'Y' THEN 'Draft-Approved' WHEN Status = 'W' THEN 'Draft-Pending'  END  [Status], T1.DocNum [Doc No.], " +

                        "T1.CardCode[BP Code], T1.CardName[BP Name], T1.DocDate [Posting Date], T1.DocDueDate [Delivery Date],TO_DECIMAL(T1.DocTotal,10,2)[Total], " +

                        "(SELECT TO_DECIMAL(Sum(Z.Quantity),10,2) From DRF1 Z Where Z.DocEntry = T1.DocEntry) [Total Quantity] " +

                        "FROM OWDD T0 " +

                        "INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry " +

                        $"WHERE T1.U_PrepBy = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}' AND CANCELED = 'N'" +

                        //$"WHERE T0.UserSign = (SELECT USERID FROM OUSR WHERE USER_CODE = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}') AND CANCELED = 'N'" +

                        "ORDER BY DocEntry DESC";

            return query;
        }

        public string PurchaseOrderExistingDocs()
        {
            string query = "SELECT A.DocEntry, " +
                        "CASE " +
                        "WHEN A.CANCELED = 'Y' THEN 'Canceled' " +
                        "WHEN A.CANCELED = 'N' AND A.DocStatus != 'O' THEN 'Closed'  " +
                        "WHEN A.DocStatus = 'O' THEN 'Open' END [Status], " +

                        "A.DocNum [Doc No.], A.CardCode [BP Code], A.CardName [BP Name], A.DocDate [Posting Date], " +

                        "A.DocDueDate [Delivery Date],TO_DECIMAL(A.DocTotal, 15, 2) [Total],(SELECT TO_DECIMAL(Sum(Z.Quantity),15,2) From POR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OPOR A" +

                        " UNION ALL " +

                        "SELECT Distinct T1.DocEntry, CASE WHEN Status = 'Y' THEN 'Draft-Approved' WHEN Status = 'W' THEN 'Draft-Pending' WHEN Status = 'N' THEN 'Draft-Rejected'  END  [Status], T1.DocNum [Doc No.], " +

                        "T1.CardCode[BP Code], T1.CardName[BP Name], T1.DocDate [Posting Date], T1.DocDueDate [Delivery Date],TO_DECIMAL(T1.DocTotal,10,2)[Total], " +

                        "(SELECT TO_DECIMAL(Sum(Z.Quantity),15,2) From DRF1 Z Where Z.DocEntry = T1.DocEntry) [Total Quantity] " +

                        "FROM OWDD T0 " +

                        "INNER JOIN ODRF T1 ON T0.DraftEntry = T1.DocEntry " +

                        $"WHERE T1.U_PrepBy = '{DomainLayer.Models.EasySAPCredentialsModel.EmployeeCompleteName}' AND CANCELED = 'N' AND T1.ObjType = '22'" +

                        //$"WHERE T0.UserSign = (SELECT USERID FROM OUSR WHERE USER_CODE = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}') AND CANCELED = 'N'" +

                        "ORDER BY DocEntry DESC";

            return query;
        }

        public string UdfValidValues(string value)
        {
            string query = "SELECT ''[Code] ,''[Name] UNION SELECT FldValue, Descr " +

                "FROM UFD1 " +

                $"WHERE TableID = 'OPOR' AND FieldID = '{value}'";

            return query;
        }

        internal string UdfTableValues(string v)
        {
            string query = $"SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [{v}]";

            return query;
        }

        public string CartonListLines(string docEntry)
        {
            string query = $"SELECT DocEntry, U_DocEntry, U_CartonNo, U_DocRef, U_Ref1, U_Ref2, U_Remarks FROM [@CL_ROWS] WHERE DocEntry = '{docEntry}'";

            return query;
        }

        public string CartonListHeader(string docEntry)
        {
            string query = $"SELECT DocEntry, Remark FROM [@CL_HEADER] WHERE DocEntry = '{docEntry}'";

            return query;
        }

        public string ExistingCartonList()
        {
            return "SELECT DocEntry [Document Entry], DocNum [Document No.], CreateDate [Date Created], UpdateDate [Date Updated], Remark [Remarks]  " +

                "FROM [@CL_HEADER] Order by DocNum DESC";
        }

        public string CartonMngtHeader(string docEntry)
        {
            string query = "Select DocEntry, DocNum, U_CartonNo, U_VendorCode, " +

                "U_VendorName, U_ChainName, U_DocRef, U_Ref1, U_Status, Remark, U_Ref2, " +

                "U_TransactionType, U_TargetWH, U_LastWH, U_DateChecked From [@CM_HEADER] " +

                $"Where DocEntry = {docEntry}";

            return query;
        }
        public string CartonMngtHeader(List<string> docEntries)
        {
            string docEntry = "'" + string.Join("','", docEntries) + "'";

            string query = "Select DocEntry, DocNum, U_CartonNo, U_VendorCode, " +

                "U_VendorName, U_ChainName, U_DocRef, U_Ref1, U_Status, Remark, U_Ref2, " +

                "U_TransactionType, U_TargetWH, U_LastWH, U_DateChecked From [@CM_HEADER] " +

                $"WHERE DocEntry in ({docEntry})";

            return query;
        }

        public string CartonMngtRow(string docEntry)
        {
            string query = "Select U_ItemNo, U_Description, " +

                "U_Quantity, U_QuantityInnerBox, U_BaseRef, U_BaseType From [@CM_ROWS] " +

                $"Where DocEntry = {docEntry}";

            return query;
        }

        public string ExistingCartonManagement(string list)
        {
            string query = "SELECT DocEntry [Document Entry], DocNum [Document No.], Remark [Remarks], CreateDate [Create Date], UpdateDate [Update Date], U_CartonNo [Carton Number], " +

                "U_VendorCode [Vendor Code], U_VendorName [Vendor Name], U_ChainName [Chain Name], U_DocRef [Document Reference], U_Ref1 [Ref], U_Ref2 [Ref 2]," +

                " U_Status [Status], U_DateChecked [Date Checked] FROM [@CM_HEADER] ";

            if (list != string.Empty && list != null)
            {
                query += $"WHERE DocEntry not in ({list}) ";
            }

            query += " Order By DocNum DESC";

            return query;
        }

        public string CartonMngtStatus()
        {
            return "select Descr [Value] from UFD1 where TableID = '@CM_HEADER' and FieldID = '7'";
        }

        public string SeriesCode(string objectCode)
        {
            string query = "SELECT T0.Series [Code], T0.SeriesName [Name]" +

                "FROM NNM1 T0 " +

                $"Where T0.ObjectCode = '{objectCode}'";

            return query;
        }

        public string SeriesNo(string objectCode, string Series)
        {
            return "SELECT T0.NextNumber " +
                    "FROM NNM1 T0 " +
                    $"Where T0.ObjectCode = '{objectCode}' AND T0.Series = '{Series}'";
        }
        
        public string BPinformation(string CardCode)
        {
            return "SELECT CntctPrsn" +
                   " , CASE WHEN A.Currency = '##' THEN " +
                    " CASE WHEN(select DfActCurr from OADM) = 'N' THEN " +
                    " (select MainCurncy from OADM) " +
                    "  ELSE " +
                    "  (select SysCurrncy from OADM) " +
                    "  END " +
                    " ELSE " +
                    " A.Currency " +
                    " END [Currency] " +
                    ", A.Currency [RawCurrency]" +
                    ", ECVatGroup, " +
                    "(SELECT Distinct Z.U_Whs FROM CRD1 Z where Z.CardCode = A.CardCode) [Whs] " +
                    $"FROM OCRD A WHERE A.CardCode = '{CardCode}' AND frozenFor = 'N'";
        }

        public string Company()
        {
            return "SELECT '' [Code], '' [Name] " +
                    "UNION " +
                    "SELECT Code, Name FROM [@CMP_INFO] Order by Name";
        }

        public string CompanyCode(string name)
        {
            return $"SELECT Code FROM [@CMP_INFO] Where Name = '{name}'";
        }

        public string DocumentType()
        {
            return "SELECT '' [Code], '' [Name] " +
                    "UNION " +
                    "SELECT Code, Name FROM [@DOC_TYPE] WHERE Order by Name";
        }

        public string BpRateValue(string currency)
        {
            return "SELECT TOP 1 Rate FROM ORTT " +
                    $"where Currency = '{currency}' Order by RateDate Desc";
        }

        internal string EmpId()
        {
            string query = $"SELECT empID FROM OHEM WHERE U_UserID = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}'";

            return query;
        }

        public static string GetTaxRate(string TaxCode)
        {
            return $"SELECT Z.Rate [Tax Rate] FROM OVTG Z Where Z.Code = '{TaxCode}')";
        }
        public string GetMarketingDocument(string DocEntry, string table)
        {
            string query = "";

            query = "SELECT T0.DocEntry, " +

                    "(SELECT Z.SeriesName FROM NNM1 Z WHERE Z.Series = T0.Series) [Series], " +

                    "T0.DocNum, T0.CardCode, T0.CardName, T0.CntctCode, T0.NumAtCard, " +

                    "(SELECT Distinct Z.Name FROM [@CMP_INFO] Z WHERE Z.Code = T0.U_CompanyTIN) [Company], " +

                    "(SELECT Distinct Z.Currency FROM OCRD Z WHERE Z.CardCode = T0.CardCode) [Currency], " +

                    "T0.U_DocType, T0.DocStatus, T0.DocDate, T0.DocDueDate, T0.TaxDate, " +

                    "(SELECT Distinct Z.SlpName FROM OSLP Z WHERE Z.SlpCode = T0.SlpCode) [SalesEmp], T0.Comments " +

                    $"FROM O{table} T0 WHERE T0.DocEntry = '{DocEntry}'";

            return query;
        }

        public string GetMarketingDocumentLines(string DocEntry, string table)
        {
            string query = "SELECT T0.LineNum, T0.ItemCode, T0.Dscription, " +

                           "(SELECT Z.U_ID012 FROM OITM Z WHERE Z.ItemCode = T0.ItemCode) [Style], " +

                           "T0.Quantity, T0.UomCode, T0.WhsCode, T0.PriceBefDi, T0.DiscPrcnt, " +

                           "T0.VatGroup, T0.VatPrcnt, " +

                           "CASE WHEN T0.VatPrcnt > 0 THEN (T0.PriceAfVAT - T0.PriceBefDi) " +

                           "ELSE 0 END [TaxAmount], " +

                           "T0.PriceAfVAT, T0.U_Remarks, T0.GTotal, " +

                           "T0.LineTotal  " +

                           $"FROM {table}1 T0 WHERE T0.DocEntry = '{DocEntry}'";

            return query;
        }

        public string PurchaseOrderItems(string whse, string vatGrp)
        {
            return $"SELECT T0.ItemCode [Item Code], T0.CodeBars [Barcode], T0.ItemName [Description], T0.U_ID012 [Style Code], (SELECT Distinct Z.Name FROM [@OBND] Z WHERE Z.Code = T0.U_ID001) [Brand], " +

            "(SELECT Z.Name FROM [@OCLC] Z Where Z.Code = T0.U_ID022) [Color], U_ID007 [Size], U_ID018 [Section]," +

            $"ISNULL((select OnHand - IsCommited + OnOrder from OITW where ItemCode = T0.ItemCode and WhsCode = '{whse}'), 0) [Available], " +

            "ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= A.DocDate) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0) [Unit Price], " +

            "ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= A.DocDate) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0) * " +

            $"(1 + (SELECT Z.Rate FROM OVTG Z Where Z.Code =  '{vatGrp}') / 100) [Gross Price], " +

            $"(SELECT Z.Rate FROM OVTG Z Where Z.Code = '{vatGrp}') [Tax Rate], " +

            "(ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= A.DocDate) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0) * " +

            $"(1 + (SELECT Z.Rate FROM OVTG Z Where Z.Code = '{vatGrp}') / 100) ) - " +

            "(ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= A.DocDate) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0)) [Tax Amount]," +

            $"FROM OITM T0 INNER JOIN OITW T1 ON T1.ItemCode = T0.ItemCode WHERE T0.ItemCode != 'Ching Chong' AND T1.WhsCode = '{whse}' ";
        }

        public string ItemsCondition(StringParameters.ItemCondtionParameter condition)
        {
            string query = condition.Query;

            if (condition.Brand != "")
            {
                query += $"AND T0.U_ID001 = '{condition.Brand}' ";
            }

            if (condition.Department != "")
            {
                query += $"AND T0.U_ID002 = '{condition.Department}' ";
            }

            if (condition.SubDepartment != "")
            {
                query += $"AND T0.U_ID003 = '{condition.SubDepartment}' ";
            }

            if (condition.Category != "")
            {
                query += $"AND T0.U_ID004 = '{condition.Category}' ";
            }

            if (condition.SubCategory != "")
            {
                query += $"AND T0.U_ID005 = '{condition.SubCategory}' ";
            }

            if (condition.Style != "")
            {
                query += $"AND T0.U_ID012 = '{condition.Style}' ";
            }

            if (condition.ParentSize != "")
            {
                query += $"AND T0.U_ID006 = '{condition.ParentSize}' ";
            }

            if (condition.Size != "")
            {
                query += $"AND T0.U_ID008 = '{condition.Size}' ";
            }

            if (condition.ParentColor != "")
            {
                query += $"AND T0.U_ID010 = '{condition.ParentColor}' ";
            }

            if (condition.Color != "")
            {
                query += $"AND T0.U_ID011 = '{condition.Color}' ";
            }

            string search = condition.SearchKeyword;
            string columnName = condition.Column;
            if (search != "" && search != null)
            {
                var index = search.IndexOf(@"*");
                var strLenght = search.Length - 1;
                string[] array = search.Split('*');

                if (index == strLenght)
                {
                    query += $"AND {columnName} LIKE '{array[0]}%' ";
                }
                else if (index == 0)
                {
                    query += $"AND {columnName} LIKE '%{array[1]}' ";
                }
                else
                {
                    query += $"AND {columnName} LIKE '{array[0]}%{array[1]}' ";
                }
            }

            return query;
        }
        public string InvoiceExistingDocs()
        {
            string query = "SELECT A.DocEntry, " +

                        " CASE WHEN A.CANCELED = 'Y' THEN 'Canceled' " +
                        " WHEN A.CANCELED = 'C' AND A.DocStatus = 'C' THEN 'Canceled' " +
                        " WHEN A.CANCELED = 'N' AND A.DocStatus != 'O' THEN 'Closed' " +
                        " WHEN A.CANCELED = 'N' AND A.DocStatus = 'O' THEN 'Open' END [Status], " +

                        "A.DocNum [Doc No.], A.CardCode [BP Code], A.CardName [BP Name], A.DocDate [Posting Date], " +

                        "A.DocDueDate [Delivery Date],TO_DECIMAL(A.DocTotal, 20, 2) [Total],(SELECT TO_DECIMAL(Sum(Z.Quantity),20,2) From INV1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OINV A" +

                        " UNION ALL " +

                        "SELECT Distinct T1.DocEntry, CASE WHEN Status = 'Y' THEN 'Draft-Approved' WHEN Status = 'W' THEN 'Draft-Pending'  END  [Status], T1.DocNum [Doc No.], " +

                        "T1.CardCode[BP Code], T1.CardName[BP Name], T1.DocDate [Posting Date], T1.DocDueDate [Delivery Date],TO_DECIMAL(T1.DocTotal,20,2)[Total], " +

                        "(SELECT TO_DECIMAL(Sum(Z.Quantity),20,2) From DRF1 Z Where Z.DocEntry = T1.DocEntry) [Total Quantity] " +

                        "FROM OWDD T0 " +

                        "INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry " +

                        $"WHERE T1.U_PrepBy = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}' AND CANCELED = 'N'" +

                        //$"WHERE T0.UserSign = (SELECT USERID FROM OUSR WHERE USER_CODE = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}') AND CANCELED = 'N'" +

                        "ORDER BY DocEntry DESC";

            return query;
        }

        public string UnofficialSalesLinesService(string table, string docEntry)
        {
            string query = "SELECT " +
                           //" T0.LineNum" +
                           " T0.ItemCode" +
                           ", T0.Dscription" +
                           ", T0.OpenQty [Quantity]" +
                           ", T0.Price" +
                           ", T0.DiscPrcnt" +
                           ", T0.LineTotal" +
                           ", T0.WhsCode" +
                           ", T0.SlpCode" +
                           ", T0.PriceBefDi" +
                           ", T0.Project" +
                           ", T0.VatGroup" +
                           ", T0.VatPrcnt" +
                           ", T0.CodeBars" +
                           ", T0.PriceAfVAT" +
                           ", T0.TaxCode" +
                           ", T0.VatAppld" +
                           ", T0.LineVat" +
                           ", T1.U_ID025 [U_StyleCode]" +
                           ", T1.U_ID011 [U_Color]" +
                           ", T1.U_ID018 [U_Section]" +
                           ", T1.U_ID007 [U_Size]" +
                           ", T1.CodeBars " +
                           " FROM DLN1 T0 " +
                           "INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode Where T0.DocEntry = '" + docEntry + "'";

            return query;
        }

        public string SalesOrderLinesItemDraft(string docEntry, string docStatus)
        {
            string query = "SELECT Distinct T1.LineNum, T1.ItemCode [Item No.], T2.ItemName [Item Description], T1.Dscription, T1.U_Style [Style], " +

                    "T1..UomCode[UOM], T1.U_PricetagCount[Pricetag Count], T1.U_Remarks[Remarks], T1.OcrCode2[Brand], T1.U_ChainDescription[Chain Desc], " +

                    "'' [AcctCode], T1.Quantity, T1.Price[Unit Price], T1.DiscPrcnt, T1.LineTotal, T1.WhsCode, T1.SlpCode, T1.PriceBefDi, T1.PriceAfVAT, T1.LineTotal [INMPrice], " +

                    "T1.GTotal, T1.Project, T1.VatGroup, " +

                    "T1.VatPrcnt, T1.CodeBars, T1.TaxCode, T1.VatAppld, T1.LineVat, T1.U_Chain, " +

                    "T2.U_ID012, (SELECT Z.Name FROM [@OCLC] Z WHERE Z.Code = T2.U_ID022) [U_ID022], T2.U_ID018, T2.U_ID007, T1.CodeBars " +

                    "FROM OWDD T0 " +

                    "INNER JOIN DRF1 T1 ON T0.DocEntry = T1.DocEntry " +

                    "INNER JOIN OITM T2 ON T1.ItemCode = T2.ItemCode " +

                    $"WHERE T0.DocEntry = '{docEntry}'";

            query += docStatus == "Draft-Approved" ? " AND Status = 'Y'" : " AND Status = 'W'";

            return query;
        }

        public string SalesOrderDraft(string docEntry, string docStatus)
        {
            string query = "SELECT T1.DocEntry, T1.DocNum, T1.CardCode, T1.CardName, T1.CardName, T1.DocStatus, T1.DiscPrcnt, " +

            "T1.DocCur, T1.NumAtCard, T1.U_Department, T1.U_Remarks, DocRate," +

            "(SELECT Z.SeriesName FROM NNM1 Z WHERE Z.Series = T1.Series) [Series]," +

            "(SELECT Z.Name FROM [@CMP_INFO] Z WHERE Z.Code = T1.U_CompanyTIN) [U_CompanyTIN], " +

            "T1.DocDate, T1.TaxDate, T1.DocDueDate, T1.CancelDate, T1.CANCELED, T1.Comments, T1.DocType " +

            "FROM OWDD T0 " +

            "INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry " +

            $"WHERE T1.DocEntry = '{docEntry}'";

            query += docStatus != "Draft-Pending" ? " AND Status = 'Y'" : " AND Status = 'W'";

            return query;
        }

        public string SalesOrderLinesServiceDraft(string docEntry)
        {
            string query = "Select T0.LineNum, T0.Dscription[Item No.], T1.ItemName [Item Description], " +

                    "T0.U_Style [Style], T0.U_UOM [UOM], T0.U_PricetagCount [Pricetag Count], T0.U_Remarks [Remarks], T0.OcrCode2 [Brand], U_ChainDescription [Chain Desc], " +

                    "AcctCode [AcctCode], T0.U_Qty [Quantity], T0.Price[Unit Price], T0.DiscPrcnt, T0.LineTotal, T0.WhsCode, T0.SlpCode, T0.PriceBefDi, T0.Project, T0.VatGroup, " +

                    "T0.VatPrcnt, T0.CodeBars, T0.PriceAfVAT, T0.TaxCode, T0.VatAppld, T0.LineVat, T0.U_Chain, T1.U_ID012, (SELECT Z.Name FROM [@OCLC] Z WHERE Z.Code = T1.U_ID022) [U_ID022], T1.U_ID018, T1.U_ID007, T1.CodeBars " +

                    "FROM ODRF A " +

                    "INNER JOIN DRF1 T0 ON T0.DocEntry = A.DocEntry " +

                    "INNER JOIN OITM T1 ON T1.ItemCode = T0.Dscription " +

                    $"where A.DocType = 'S' AND A.DocEntry = '{docEntry}'";

            return query;
        }

        public string UnofficialSales(string table, string docEntry)
        {
            string query = "Select T0.DocEntry, T0.DocNum, T0.CardCode, T0.CardName, T0.DocStatus, T0.DiscPrcnt, " +

            "T0.DocCur, T0.NumAtCard, T0.U_Department, T0.U_Remarks, DocRate, " +

            "(SELECT Z.SeriesName FROM NNM1 Z WHERE Z.Series = T0.Series) [Series]," +

            "(SELECT Z.Name FROM [@CMP_INFO] Z WHERE Z.Code = T0.U_CompanyTIN) [U_CompanyTIN], " +

            "T0.DocDate, T0.TaxDate, T0.DocDueDate, T0.CancelDate, T0.CANCELED, T0.Comments, T0.DocType, T0.U_DocType, T0.DocTotal, T0.DiscPrcnt, T0.U_WarehouseSalesType " +

            $"FROM O{table} T0 WHERE DocEntry = '{docEntry}'";

            return query;
        }

        public string InvoiceLinesServiceDraft(string docEntry)
        {
            string query = "Select T0.LineNum, T0.Dscription[Item No.], T1.ItemName [Item Description], " +

                    "T0.U_Style [Style], T0.U_UOM [UOM], T0.U_PricetagCount [Pricetag Count], T0.U_Remarks [Remarks], T0.OcrCode2 [Brand], U_ChainDescription [Chain Desc], " +

                    "AcctCode [AcctCode], T0.U_Qty [Quantity], T0.Price[Unit Price], T0.DiscPrcnt, T0.LineTotal, T0.WhsCode, T0.SlpCode, T0.PriceBefDi, T0.Project, T0.VatGroup, " +

                    "T0.VatPrcnt, T0.CodeBars, T0.PriceAfVAT, T0.TaxCode, T0.VatAppld, T0.LineVat, T0.U_Chain, T1.U_ID012, (SELECT Z.Name FROM [@OCLC] Z WHERE Z.Code = T1.U_ID022) [U_ID022], T1.U_ID018, T1.U_ID007, T1.CodeBars " +

                    "FROM ODRF A " +

                    "INNER JOIN DRF1 T0 ON T0.DocEntry = A.DocEntry " +

                    "INNER JOIN OITM T1 ON T1.ItemCode = T0.Dscription " +

                    $"where A.DocType = 'S' AND A.DocEntry = '{docEntry}'";

            return query;
        }

        public string InvoiceLinesItemDraft(string docEntry, string docStatus)
        {
            string query = "SELECT Distinct T1.LineNum, T1.ItemCode [Item No.], T2.ItemName [Item Description], T1.Dscription, T1.U_Style [Style], " +

                    "T1..UomCode[UOM], T1.U_PricetagCount[Pricetag Count], T1.U_Remarks[Remarks], T1.OcrCode2[Brand], T1.U_ChainDescription[Chain Desc], " +

                    "'' [AcctCode], T1.Quantity, T1.Price[Unit Price], T1.DiscPrcnt, T1.LineTotal, T1.WhsCode, T1.SlpCode, T1.PriceBefDi, T1.PriceAfVAT, T1.LineTotal [INMPrice], " +

                    "T1.GTotal, T1.Project, T1.VatGroup, " +

                    "T1.VatPrcnt, T1.CodeBars, T1.TaxCode, T1.VatAppld, T1.LineVat, T1.U_Chain, " +

                    "T2.U_ID012, (SELECT Z.Name FROM [@OCLC] Z WHERE Z.Code = T2.U_ID022) [U_ID022], T2.U_ID018, T2.U_ID007, T1.CodeBars " +

                    "FROM OWDD T0 " +

                    "INNER JOIN DRF1 T1 ON T0.DocEntry = T1.DocEntry " +

                    "INNER JOIN OITM T2 ON T1.ItemCode = T2.ItemCode " +

                    $"WHERE T0.DocEntry = '{docEntry}'";

            query += docStatus == "Draft-Approved" ? " AND Status = 'Y'" : " AND Status = 'W'";

            return query;
        }

        public string InvoiceDraft(string docEntry, string docStatus)
        {
            string query = "SELECT T1.DocEntry, T1.DocNum, T1.CardCode, T1.CardName, T1.CardName, T1.DocStatus, T1.DiscPrcnt, " +

            "T1.DocCur, T1.NumAtCard, T1.U_Department, T1.U_Remarks, DocRate," +

            "(SELECT Z.SeriesName FROM NNM1 Z WHERE Z.Series = T1.Series) [Series]," +

            "(SELECT Z.Name FROM [@CMP_INFO] Z WHERE Z.Code = T1.U_CompanyTIN) [U_CompanyTIN], " +

            "T1.DocDate, T1.TaxDate, T1.DocDueDate, T1.CancelDate, T1.CANCELED, T1.Comments, T1.DocType,T1.U_DocType " +

            "FROM OWDD T0 " +

            "INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry " +

            $"WHERE T1.DocEntry = '{docEntry}'";

            query += docStatus != "Draft-Pending" ? " AND Status = 'Y'" : " AND Status = 'W'";

            return query;
        }

        public string UnofficialSalesExistingDocs()
        {
            string query = "SELECT A.DocEntry, " +

                        "CASE " +
                        "WHEN A.CANCELED = 'Y' THEN 'Canceled' " +
                        "WHEN A.CANCELED = 'N' AND A.DocStatus != 'O' THEN 'Closed'  " +
                        "WHEN A.DocStatus = 'O' THEN 'Open' END [Status], " +

                        "A.DocNum [Doc No.], A.CardCode [BP Code], A.CardName [BP Name], A.DocDate [Posting Date], " +

                        "A.DocDueDate [Delivery Date], CAST(CAST(A.DocTotal as DECIMAL) as MONEY) [Total],(SELECT Sum(Z.Quantity) From DLN1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM ODLN A " +

                        //" UNION ALL " +

                        //"SELECT Distinct T1.DocEntry, CASE WHEN Status = 'Y' THEN 'Draft-Approved' WHEN Status = 'W' THEN 'Draft-Pending'  END  [Status], T1.DocNum [Doc No.], " +

                        //"T1.CardCode[BP Code], T1.CardName[BP Name], T1.DocDate [Posting Date], T1.DocDueDate [Delivery Date],TO_DECIMAL(T1.DocTotal,10,2)[Total], " +

                        //"(SELECT TO_DECIMAL(Sum(Z.Quantity),10,2) From DRF1 Z Where Z.DocEntry = T1.DocEntry) [Total Quantity] " +

                        //"FROM OWDD T0 " +

                        //"INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry " +

                        //$"WHERE T1.U_PrepBy = '{EasySAPCredentialsModel.ESUserId}' AND CANCELED = 'N'" +

                        //$"WHERE T0.UserSign = (SELECT USERID FROM OUSR WHERE USER_CODE = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}') AND CANCELED = 'N'" +

                        " ORDER BY DocEntry DESC";

            return query;
        }

        public string SalesOrderExistingDocs()
        {
            string query = "SELECT A.DocEntry, " +

                        "CASE " +
                        "WHEN A.CANCELED = 'Y' THEN 'Canceled' " +
                        "WHEN A.CANCELED = 'N' AND A.DocStatus != 'O' THEN 'Closed'  " +
                        "WHEN A.DocStatus = 'O' THEN 'Open' END [Status], " +

                        "A.DocNum [Doc No.], A.CardCode [BP Code], A.CardName [BP Name], A.DocDate [Posting Date], " +

                        "A.DocDueDate [Delivery Date],TO_DECIMAL(A.DocTotal, 10, 2) [Total],(SELECT TO_DECIMAL(Sum(Z.Quantity),10,2) From RDR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM ORDR A" +

                        " UNION ALL " +

                        "SELECT Distinct T1.DocEntry, CASE WHEN Status = 'Y' THEN 'Draft-Approved' WHEN Status = 'W' THEN 'Draft-Pending'  END  [Status], T1.DocNum [Doc No.], " +

                        "T1.CardCode[BP Code], T1.CardName[BP Name], T1.DocDate [Posting Date], T1.DocDueDate [Delivery Date],TO_DECIMAL(T1.DocTotal,10,2)[Total], " +

                        "(SELECT TO_DECIMAL(Sum(Z.Quantity),10,2) From DRF1 Z Where Z.DocEntry = T1.DocEntry) [Total Quantity] " +

                        "FROM OWDD T0 " +

                        "INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry " +

                        $"WHERE T1.U_PrepBy = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}' AND CANCELED = 'N'" +

                        //$"WHERE T0.UserSign = (SELECT USERID FROM OUSR WHERE USER_CODE = '{DomainLayer.Models.EasySAPCredentialsModel.ESUserId}') AND CANCELED = 'N'" +

                        "ORDER BY DocEntry DESC";

            return query;
        }

        public string SalesOrder(string table, string docEntry)
        {
            string query = "Select T0.DocEntry, T0.DocNum, T0.CardCode, T0.CardName, T0.DocStatus, T0.DiscPrcnt, T0.DiscSum, " +

            "T0.DocCur, T0.NumAtCard, T0.U_Department, T0.U_Remarks, DocRate, " +

            "(SELECT Z.SeriesName FROM NNM1 Z WHERE Z.Series = T0.Series) [Series]," +

            "(SELECT Z.Name FROM [@CMP_INFO] Z WHERE Z.Code = T0.U_CompanyTIN) [U_CompanyTIN], " +

            "T0.DocDate, T0.TaxDate, T0.DocDueDate, T0.CancelDate, T0.CANCELED, T0.Comments, T0.DocType, T0.U_DocType, (select SlpName from OSLP where SlpCode = T0.SlpCode) [SlpName] " +

            $"FROM O{table} T0 WHERE DocEntry = '{docEntry}'";

            return query;

        }

        public string SalesOrderLinesService(string table, string docEntry)
        {
            string query = "Select T0.LineNum, T0.Dscription [Item No.], T0.U_Description [Item Description], T0.U_Style [Style], " +

                   "T0.U_UOM [UOM], T0.U_PricetagCount [Pricetag Count], T0.U_Remarks [Remarks], T0.OcrCode2 [Brand]," +

                   " U_ChainDescription [Chain Desc], AcctCode [AcctCode], T0.U_Qty [Quantity], T0.Price [Unit Price], T0.DiscPrcnt, T0.LineTotal, " +

                   "T0.U_TargetWhs [WhsCode], T0.SlpCode, T0.PriceBefDi, T0.PriceAfVAT, T0.LineTotal [INMPrice], T0.GTotal, T0.Project, T0.VatGroup, T0.VatPrcnt, T0.U_OldItemNo [CodeBars], " +

                   "U_Style [U_ID012], U_Color [U_ID022], U_Size [U_ID007], U_PricetagCount [Pricetag Count], U_Style [U_ID012], T0.U_GrossPricePerPiece, T0.U_UnitPricePerPiece, " +

                   "T0.TaxCode, T0.VatAppld, T0.LineVat, T0.U_Chain " +

                   $"from O{table} A INNER JOIN {table}1 T0 ON T0.DocEntry = A.DocEntry " +

                   $"Where A.DocType = 'S' AND A.DocEntry = '{docEntry}'";

            return query;
        }


        public string SalesOrderLinesItem(string table, string docEntry, string status)
        {
            string query = "SELECT " +
                            " T0.LineNum" +
                            ", T0.ItemCode" +
                            ", T0.Dscription" +
                            ", T0.OpenQty [Quantity]" +
                            ", T0.Price" +
                            ", T0.DiscPrcnt" +
                            ", T0.LineTotal" +
                            ", T0.WhsCode" +
                            ", T0.SlpCode" +
                            ", T0.PriceBefDi" +
                            ", T0.Project" +
                            ", T0.VatGroup" +
                            ", T0.VatPrcnt" +
                            ", T0.CodeBars" +
                            ", T0.PriceAfVAT" +
                            ", T0.TaxCode" +
                            ", T0.VatAppld" +
                            ", T0.LineVat" +
                            ", T1.U_ID025 [U_StyleCode]" +
                            ", T1.U_ID011 [U_Color]" +
                            ", T1.U_ID018 [U_Section]" +
                            ", T1.U_ID007 [U_Size]" +
                            ", T1.CodeBars " +
                            " FROM RDR1 T0 " +
                            "INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode " +
                            "Where T0.DocEntry = '" + docEntry + "' ";
            query += status == "Open" ? " and T0.LineStatus = 'O' " : " and T0.LineStatus = 'C' ";
            query += "order by T0.LineNum";

            return query;
        }

        public string orderBy(string query)
        {
            return query += " Order By T0.U_ID023";
        }

        public string GetUdfValidValues(string TableID , string FieldID)
        {
            string query = "SELECT ''[Code] ,''[Name] UNION SELECT FldValue, Descr " +

                "FROM UFD1 " +

                $"WHERE TableID = '{TableID}' AND FieldID = '{FieldID}'";

            return query;
        }

        public string DeliveryLinesItem(string table, string docEntry)
        {
            string query = "SELECT " +
                            " T0.ItemCode" +
                            ", T0.Dscription" +
                            ", T0.Quantity [Quantity]" +
                            ", T0.Price [Unit Price]" +
                            ", T0.DiscPrcnt" +
                            ", T0.LineTotal" +
                            ", T0.WhsCode" +
                            ", T0.SlpCode" +
                            ", T0.PriceBefDi" +
                            ", T0.Project" +
                            ", T0.VatGroup" +
                            ", '' [U_DiscType]" +
                            ", T0.VatPrcnt" +
                            ", T0.CodeBars" +
                            ", T0.PriceAfVAT" +
                            ", T0.TaxCode" +
                            ", T0.VatAppld" +
                            ", T0.LineVat" +
                            ", T1.U_ID025 [U_StyleCode]" +
                            ", T1.U_ID011 [U_Color]" +
                            ", T1.U_ID018 [U_Section]" +
                            ", T1.U_ID007 [U_Size]" +
                            ", T1.CodeBars" +
                            ", T0.GTotal" +
                            ", T1.U_ID023" +
                            ", ISNULL(T0.ShipDate, '') [ShipDate] " +
                            ", ISNULL(T0.U_Company, '') [U_Company] " +
                            "FROM DLN1 T0 " +
                            "INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode " +
                            "Where T0.DocEntry = '" + docEntry + "' Order by T0.LineNum ASC";

            return query;
        }

        public string SOPickListHeader(string CardCode, string OrderEntry, string AbsEntry)
        {
            string query = $" SELECT Distinct " +
                " C.CardCode" +
                ", A.OrderEntry" +
                ", C.DocType " +
                ", C.DocEntry " +
                ", C.DocNum " +
                ", C.Series " +
                ", C.CardName " +
                ", C.NumAtCard " +
                ", (SELECT Z.Name FROM [@CMP_INFO] Z WHERE Z.Code = C.U_CompanyTIN) [U_CompanyTIN] " +
                ", C.U_Department " +
                ", C.DocCur " +
                ", C.DocRate " +
                ", C.DocDate " +
                ", C.TaxDate " +
                ", C.DocDueDate " +
                ", C.CancelDate " +
                ", C.Comments " +
                ", C.U_DocType " +
                ", C.ShipToCode " +
                ", C.Address2 " +
                " FROM PKL1 A LEFT JOIN RDR1 B ON A.OrderEntry = B.DocEntry " +
                $" LEFT JOIN ORDR C ON B.DocEntry = C.DocEntry and C.CardCode = '{CardCode}' " +
                $" Where A.OrderEntry = '{OrderEntry}' and A.AbsEntry = '{AbsEntry}'";

            return query;
        }

        public string SOPickListLinesItem()
        {
            string query = "";
            string oSelQry1 = "";

            oSelQry1 = "SELECT " +
                        "A.AbsEntry" +
                        ", A.OrderEntry" +
                        ", A.OrderLine [LineNum]" +
                        ", A.PickQtty" +
                        ", A.PickStatus" +
                        ", A.RelQtty" +
                        ", A.PrevReleas" +
                        ", A.BaseObject " +
                        ", C.U_ID025 [U_ID012]" + //U_StyleCode
                        ", C.U_ID011 [U_ID022]" + //U_Color
                        ", C.U_ID018 " + //U_Section
                        ", C.U_ID007 " + //U_Size
                        ", B.ItemCode [Item No.]" +
                        ", B.Dscription" +
                        ", A.PickQtty [Quantity]" +
                        ", B.Price" +
                        ", B.DiscPrcnt" +
                        ", B.LineTotal" +
                        ", B.WhsCode" +
                        ", B.SlpCode" +
                        ", B.PriceBefDi" +
                        ", B.Project" +
                        ", B.VatGroup" +
                        ", B.VatPrcnt" +
                        ", B.CodeBars" +
                        ", B.PriceAfVAT" +
                        ", B.TaxCode" +
                        ", B.VatAppld" +
                        ", B.LineVat" +
                        ", ISNULL((select Name from [@CMP_INFO] where Code = B.U_Company),'') [U_Company]" +
                        " FROM PKL1 A " +
                        " LEFT JOIN RDR1 B ON A.OrderEntry = B.DocEntry and A.OrderLine = B.LineNum";

            if (InvoiceHeaderModel.oDDW == "DrawAll")
            {
                var SelDocOrderEntry = InvoiceHeaderModel.DDWdocentry.GroupBy(x => new { x.DocEntry, x.OrderEntry }).Select(y => y.First());

                //foreach (string x in InvoiceHeaderModel.DDWdocentry.Select(x => x.OrderEntry).Distinct())
                foreach (var x in SelDocOrderEntry)
                {
                    //Added DDWdocentry 081519
                    //int DDWdocentry = InvoiceHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x).Select(z => z.DocEntry).First();

                    InvoiceHeaderModel.oBPCode = InvoiceHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.BpCode).First().ToString();
                    InvoiceHeaderModel.oCode = InvoiceHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.DocEntry).First().ToString();

                    //InvoiceHeaderModel.oOrderEntry = InvoiceHeaderModel.DDWdocentry.Where(y => y.DocEntry == DDWdocentry).Select(z => z.OrderEntry).First().ToString();

                    if (query == "")
                    {
                        query = oSelQry1 +
                                $" INNER JOIN ORDR T4 on B.DocEntry = T4.DocEntry and T4.CardCode = '{InvoiceHeaderModel.oBPCode}' " +
                                $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickQtty > 0 AND B.OpenQty > 0 AND A.PickStatus != 'C' and A.AbsEntry = '{InvoiceHeaderModel.oCode}' " +
                                $" and A.OrderEntry = '{x.OrderEntry}' ";
                    }
                    else
                    {
                        query += " UNION ALL " +
                                 oSelQry1 +
                                 $" INNER JOIN ORDR T4 on B.DocEntry = T4.DocEntry and T4.CardCode = '{InvoiceHeaderModel.oBPCode}' " +
                                 $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickQtty > 0 AND B.OpenQty > 0 AND A.PickStatus != 'C' and A.AbsEntry = '{InvoiceHeaderModel.oCode}' " +
                                 $" and A.OrderEntry = '{x.OrderEntry}' ";
                    }
                }

            }
            else
            {
                var SelDocOrderEntry = InvoiceHeaderModel.DDWdocentry.GroupBy(x => new { x.DocEntry, x.OrderEntry }).Select(y => y.First());

                //foreach (string x in InvoiceHeaderModel.DDWdocentry.Select(x => x.OrderEntry).Distinct())
                foreach (var x in SelDocOrderEntry)
                {
                    //int DDWdocentry = InvoiceHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.DocEntry).First();
                    InvoiceHeaderModel.oBPCode = InvoiceHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.BpCode).First().ToString();
                    InvoiceHeaderModel.oCode = InvoiceHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.DocEntry).First().ToString();
                    InvoiceHeaderModel.oLineNums = "";

                    //InvoiceHeaderModel.oOrderEntry = InvoiceHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x).Select(z => z.OrderEntry).First().ToString();

                    foreach (var y in InvoiceHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry))
                    {
                        if (InvoiceHeaderModel.oLineNums == "")
                        {
                            InvoiceHeaderModel.oLineNums = "'" + y.LineEntry.ToString() + "'";
                        }
                        else
                        {
                            InvoiceHeaderModel.oLineNums += ",'" + y.LineEntry.ToString() + "'";
                        }
                    }

                    if (query == "")
                    {
                        query = oSelQry1 +
                                $" INNER JOIN ORDR T4 on B.DocEntry = T4.DocEntry and T4.CardCode = '{InvoiceHeaderModel.oBPCode}' " +
                                $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickStatus != 'C' and A.AbsEntry = '{InvoiceHeaderModel.oCode}' " +
                                $" and A.OrderLine IN({InvoiceHeaderModel.oLineNums}) " +
                                $" and A.OrderEntry = '{x.OrderEntry}' ";
                    }
                    else
                    {
                        query += " UNION ALL " +
                                 oSelQry1 +
                                 $" INNER JOIN ORDR T4 on B.DocEntry = T4.DocEntry and T4.CardCode = '{InvoiceHeaderModel.oBPCode}' " +
                                 $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickStatus != 'C' and A.AbsEntry = '{InvoiceHeaderModel.oCode}' " +
                                 $" and A.OrderLine IN({InvoiceHeaderModel.oLineNums}) " +
                                 $" and A.OrderEntry = '{x.OrderEntry}' ";
                    }
                }

            }

            return query;
        }

    }
}

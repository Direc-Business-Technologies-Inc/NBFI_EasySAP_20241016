using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using zDeclare;
using System.Data;
using DirecLayer;
using System.IO;
using PresenterLayer.Views.Main;
using PresenterLayer.Helper;
using ServiceLayer.Services;
using DomainLayer;
using PresenterLayer.Views;
using System.Runtime.InteropServices;
using DomainLayer.Models;
using DomainLayer.Helper;
using MetroFramework;
using System.Text;

namespace PresenterLayer.Services
{
    public class DtController
    {
        DECLARE dc = new DECLARE();

        UploadController controller = new UploadController();

        CartonController cartonController = new CartonController();

        DataContextList context = new DataContextList();

        List<CartonListRow> ctnList { get; set; }
        List<ErrorIds> GotErrIds { get; set; }

        public static string objType { get; set; }
        public static int SINo { get; set; }
        public bool cardcode { get; set; }

        #region For Marketing Document Header List 
        public void Header(MarketingDocumentHeaders hdr, DataGridView DgvExcel, string ColumnName, int col, int row, bool isSMPO = false)
        {
            switch (ColumnName)
            {
                case "CardCode":

                    var info = controller.BpCardProjectCode(ValidateInput.String(DgvExcel[col, row].Value), isSMPO);

                    hdr.CardCode = info[0].Trim(); //ValidateInput.String(DgvExcel[col, row].Value);
                    hdr.Project = info[1];
                    //hdr.CardName = controller.CardName(hdr.CardCode);
                    break;

                case "CardName":
                    hdr.CardName = DgvExcel[col, row].Value.ToString();
                    break;

                case "DocDate":
                    hdr.DocDate = DgvExcel[col, row].Value.ToString();
                    break;

                case "TaxDate":
                    hdr.TaxDate = DgvExcel[col, row].Value.ToString();
                    break;

                case "DocDueDate":
                    hdr.DocDueDate = DgvExcel[col, row].Value.ToString();
                    break;

                case "CancelDate":
                    hdr.CancelDate = DgvExcel[col, row].Value.ToString();
                    break;

                case "ReqDate":
                    hdr.ReqDate = DgvExcel[col, row].Value.ToString();
                    break;

                case "ShipToCode":
                    hdr.ShipToCode = DgvExcel[col, row].Value.ToString();
                    break;

                case "GroupNum":
                    hdr.GroupNum = DgvExcel[col, row].Value.ToString();
                    break;

                case "Remark":
                    hdr.Remark = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_CartonNo":
                    hdr.U_CartonNo = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_VendorCode":
                    hdr.U_VendorCode = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_VendorName":
                    hdr.U_VendorName = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_ChainName":
                    hdr.U_ChainName = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_DocRef":
                    hdr.U_DocRef = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_Ref1":
                    hdr.U_Ref1 = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_Ref2":
                    hdr.U_Ref2 = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_Status":
                    hdr.U_Status = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_CompanyTIN":
                    hdr.U_CompanyTIN = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_PONo":
                    hdr.U_PONo = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_Remarks":
                    hdr.U_Remarks = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_GroupCode":
                    hdr.U_GroupCode = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_AddID":
                    hdr.U_AddID = DgvExcel[col, row].Value.ToString();
                    break;

                case "DueDate":
                    hdr.DueDate = DgvExcel[col, row].Value.ToString();
                    break;

                case "ToWarehouse":
                    hdr.ToWarehouse = DgvExcel[col, row].Value.ToString();
                    break;

                case "FromWarehouse":
                    hdr.FromWarehouse = DgvExcel[col, row].Value.ToString();
                    break;

                case "ToWhsCode":
                    hdr.ToWarehouse = DgvExcel[col, row].Value.ToString(); // for wtr if needed
                    break;

                case "Filler":
                    hdr.FromWarehouse = DgvExcel[col, row].Value.ToString(); // for wtr if needed
                    break;

                case "Address2":
                    hdr.Address2 = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_OrRecDate":
                    hdr.U_OrRecDate = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_DocType":
                    hdr.U_DocType = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_OrderNo":
                    hdr.U_OrderNo = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_OrderTime":
                    hdr.U_OrderTime = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_OrderFlag":
                    hdr.U_OrderFlag = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_ReceiverName":
                    hdr.U_ReceiverName = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_BillingPeriod":
                    hdr.U_BillingPeriod = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_ShippingOption":
                    hdr.U_ShippingOption = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_SINo":
                    hdr.U_SINo = DgvExcel[col, row].Value.ToString();
                    break;

                case "U_TransferType":
                    hdr.U_TransferType = DgvExcel[col, row].Value.ToString();
                    break;

                case "DiscPrcnt":
                    hdr.DiscPrcnt = DgvExcel[col, row].Value.ToString();
                    break;

                case "Series":
                    hdr.Series = DgvExcel[col, row].Value.ToString();
                    break;
            }
        }


        public void HeaderInventoryCount(DTInventoryCounting hdr, DataGridView DgvExcel, string ColumnName, int col, int row, bool isSMPO = false)
        {
            switch (ColumnName)
            {
                case "CountDate":
                    hdr.CountDate = DgvExcel[col, row].Value.ToString();
                    break;

                case "WhsCode":
                    hdr.WhsCode = DgvExcel[col, row].Value.ToString();
                    break;
            }
        }
        #endregion

        #region For Marketing Document Row List
        public void Row(MarketingDocumentLines lns, DataGridView DgvExcel, string ColumnName, int col, int row)
        {
            try
            {
                switch (ColumnName)
                {
                    case "ItemCode":
                        lns.ItemCode = ValidateInput.String(DgvExcel[col, row].Value);
                        break;

                    case "Dscription":
                        lns.Dscription = DgvExcel[col, row].Value.ToString();
                        break;

                    case "Quantity":
                        lns.Quantity = ValidateInput.Double(DgvExcel[col, row].Value).ToString();
                        break;

                    case "ShipDate":
                        lns.ShipDate = ValidateInput.Double(DgvExcel[col, row].Value).ToString();
                        break;

                    case "LineTotal":
                        lns.LineTotal = DgvExcel[col, row].Value.ToString();
                        break;

                    case "Price":
                        lns.Price = DgvExcel[col, row].Value.ToString();
                        break;

                    case "GTotal":
                        lns.GTotal = DgvExcel[col, row].Value.ToString();
                        break;

                    case "UomEntry":
                        lns.UomEntry = DgvExcel[col, row].Value.ToString();
                        break;

                    case "FromWhsCod":
                        lns.FromWhsCod = DgvExcel[col, row].Value.ToString(); ;
                        break;

                    case "WhsCode":
                        lns.WhsCode = DgvExcel[col, row].Value.ToString();
                        break;

                    case "PriceBefDi":
                        lns.PriceBefDi = DgvExcel[col, row].Value.ToString();
                        break;

                    case "PriceAfVAT":
                        lns.PriceAfVAT = DgvExcel[col, row].Value.ToString();
                        break;

                    case "DiscPrcnt":
                        lns.DiscPrcnt = DgvExcel[col, row].Value.ToString();
                        break;

                    case "TaxCode":
                        lns.TaxCode = DgvExcel[col, row].Value.ToString();
                        break;

                    case "Project":
                        lns.Project = DgvExcel[col, row].Value.ToString();
                        break;

                    case "Address":
                        lns.Address = DgvExcel[col, row].Value.ToString();
                        break;

                    case "U_AllocQty":
                        lns.U_AllocQty = DgvExcel[col, row].Value.ToString();
                        break;

                    case "U_Description":
                        lns.U_Description = DgvExcel[col, row].Value.ToString();
                        break;

                    case "U_ItemNo":
                        lns.U_ItemNo = DgvExcel[col, row].Value.ToString();
                        break;

                    case "U_ItemCode":
                        lns.U_ItemCode = DgvExcel[col, row].Value.ToString();
                        break;

                    case "U_Quantity":
                        lns.U_Quantity = DgvExcel[col, row].Value.ToString();
                        break;

                    //case "U_OrderTime":
                    //    lns.U_OrderTime = DgvExcel[col, row].Value.ToString();
                    //    break;

                    case "U_QuantityInnerBox":
                        lns.U_QuantityInnerBox = DgvExcel[col, row].Value.ToString();
                        break;

                    case "U_StyleSubCat":
                        lns.U_StyleSubCat = DgvExcel[col, row].Value.ToString();
                        break;

                    case "ProjectCode":
                        lns.ProjectCode = DgvExcel[col, row].Value.ToString();
                        break;

                    case "WarehouseCode":
                        lns.WarehouseCode = DgvExcel[col, row].Value.ToString();
                        break;

                    case "FromWarehouseCode":
                        lns.FromWarehouseCode = DgvExcel[col, row].Value.ToString();
                        break;
                    case "FreeTxt":
                        var info = controller.BpCardProjectCode(ValidateInput.String(DgvExcel[col, row].Value), false);
                        lns.FreeTxt = info[0];
                        break;
                    case "WtLiable":
                        lns.WtLiable = DgvExcel[col, row].Value.ToString();
                        break;
                    case "VatGroup":
                        lns.VatGroup = DgvExcel[col, row].Value.ToString();
                        break;
                }
            }
            catch (FormatException ex)
            {
                //PublicStatic.frmMain.Invoke(new Action(() =>
                //        PublicStatic.frmMain.NotiMsg(ex.Message, Color.Red)));
            }

            //lns.SapField = ColumnName;
        }

        public void RowInventoryCount(DTInventoryCountingRow lns, DataGridView DgvExcel, string ColumnName, int col, int row)
        {
            try
            {
                switch (ColumnName)
                {
                    case "CountQty":
                        lns.Quantity = Convert.ToInt32(DgvExcel[col, row].Value);
                        break;

                    case "ItemCode":
                        var item = controller.DeliveryItem("", DgvExcel[col, row].Value.ToString());
                        lns.ItemCode = item;
                        break;
                }
            }
            catch (FormatException ex)
            {
                //PublicStatic.frmMain.Invoke(new Action(() =>
                //        PublicStatic.frmMain.NotiMsg(ex.Message, Color.Red)));
            }

            //lns.SapField = ColumnName;
        }
        #endregion



        string GetTransactionType(string oTransType, string get, [Optional] string bpCode, [Optional] string address)
        {
            var output = "";
            var query = "";
            var hana = new SAPHanaAccess();
            var dt = new DataTable();
            var helper = new DataHelper();
            switch (get)
            {
                case "Series":
                    query = "SELECT (Select Series from NNM1 where SeriesName = T1.U_ITRSeries and ObjectCode = '1250000001' ) [Series] " +
                                " FROM [@TRANSFER_TYPE] T1 " +
                                $" WHERE T1.Code = '{oTransType}' ";
                    break;
                case "WhsFrom":
                    query = "SELECT DISTINCT " +
                               " CASE a.U_FillerSource " +
                               " WHEN 'WHS' " +
                               " THEN a.U_Filler " +
                               " WHEN 'WHS-LOC' " +
                               " THEN c.WhsCode " +
                               " WHEN 'CRD' " +
                               $" THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' and Address = '{ address }' and x.CardCode = '{ bpCode }') " +
                               " ELSE '' " +
                               " END [WhsFrom] " +
                               " FROM [@TRANSFER_TYPE] a  " +
                               " LEFT JOIN OLCT b on a.U_Filler = b.Location " +
                               " LEFT JOIN OWHS c on(b.Code <> c.Location and a.U_FillerComp = '<>') or(b.Code = c.Location and a.U_FillerComp = '=')  " +
                               $" WHERE a.Code = '{oTransType}'";
                    break;
                case "WhsTo":
                    dt = hana.Get($"SELECT (SELECT min(Y.Address) [Address] FROM CRD1 Y Where Y.CardCode = A.CardCode And AdresType = 'S') [Address] FROM OCRD A WHERE A.CardCode = '{bpCode}'");
                    var add = helper.DataTableRet(dt, 0, "Address", "");
                    query = "SELECT DISTINCT " +
                                " CASE a.U_DestSource  " +
                                " WHEN 'WHS'  " +
                                " THEN a.U_Destination " +
                                " WHEN 'WHS-LOC'  " +
                                " THEN c.WhsCode " +
                                " WHEN 'CRD' " +
                                $" THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' and Address = '{ add }' and x.CardCode = '{ bpCode }') " +
                                " END [WhsTo] " +
                                " FROM [@TRANSFER_TYPE] a  " +
                                " LEFT JOIN OLCT b on a.U_Destination = b.Location " +
                                " LEFT JOIN OWHS c on((b.Code <> c.Location and a.U_DestComp = '<>') " +
                                " or (b.Code = c.Location and a.U_DestComp = '=') )  " +
                                " and a.U_DestSource = 'WHS-LOC' " +
                                $" WHERE a.Code = '{oTransType}' Order by WhsTo ";
                    break;
                case "Address":
                    query = $"SELECT ShipToDef [Address] FROM OCRD A WHERE A.CardCode = '{bpCode}'";
                    break;
                case "SKU":

                    //On comment due to Pooling issue 083019
                    //            query = $" SELECT CASE WHEN ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = '{address}' and z.CardCode = '{bpCode}'),0) = 0) " +
                    //$" THEN ISNULL((select Price from ITM1 where ItemCode = '{address}' and PriceList = (select ListNum from OCRD where CardCode = '{bpCode}')), 0) " +
                    //$" ELSE ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = '{address}' and z.CardCode = '{bpCode}'),0) [Price] FROM DUMMY";

                    query = $" select distinct b.U_SKU [SKU]" +
                                        " from OCPN a " +
                                        " inner " +
                                        " join CPN1 c on a.CpnNo = c.CpnNo " +
                                        " inner join CPN2 b on a.CpnNo = b.CpnNo " +
                                        $" where c.BpCode = '{bpCode}' " +
                                        $" and a.U_CType = 'SKU' and b.ItemCode = '{address}' ";

                    //On comment due to Pooling issue 083019
                    //dt = hana.Get(query);
                    //var infoPrice = helper.DataTableRet(dt, 0, "Price", "");
                    //query = $@"CALL SKULOOP('{address}','{bpCode}','{infoPrice}',(Select count(*) from ""@OSKV"" x inner join ""@SKV1"" y on x.""Code"" = y.""Code"" where x.""Code"" = (Select yy.""SeriesName"" from OCRD xx inner join NNM1 yy on xx.""Series"" = yy.""Series"" where xx.""CardCode"" = '{bpCode}'))," +
                    //    $@"(Select yy.""SeriesName"" from OCRD xx inner join NNM1 yy on xx.""Series"" = yy.""Series"" where xx.""CardCode"" = '{bpCode}'))";
                    break;
                case "UnitPrice":
                    query = $"SELECT CASE WHEN ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T1.ItemCode and z.CardCode = '{bpCode}'),0) = 0) " +
                        $" THEN ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = (select ListNum from OCRD where CardCode = '{bpCode}')), 0) " +
                        $" ELSE ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T1.ItemCode and z.CardCode = '{bpCode}'),0) [UnitPrice] " +
                        $" FROM OITM T1 WHERE T1.frozenFor = 'N' and T1.ItemCode not like 'FA%' and T1.InvntItem = 'Y' and T1.ItemCode = '{address}'";
                    break;
                case "Brand":
                    query = "SELECT (SELECT Name FROM [@OBND] WHERE Code = T1.U_ID001) [Brand] " +
                        $" FROM OITM T1 WHERE T1.frozenFor = 'N' and T1.ItemCode not like 'FA%' and T1.InvntItem = 'Y' and T1.ItemCode = '{bpCode}'";
                    break;

            }

            if (string.IsNullOrEmpty(query) == false)
            {
                dt = hana.Get(query);
                output = helper.DataTableRet(dt, 0, get, "");
            }
            return output;
        }

        string GetUomID(string sUomCode)
        {
            string result = "";
            var helper = new DataHelper();
            var hana = new SAPHanaAccess();
            result = helper.ReadDataRow(hana.Get($"SELECT A.UomEntry FROM OUOM A WHERE A.UomCode = ( SELECT ISNULL(z.SalUnitMsr,z.InvntryUom) FROM OITM z WHERE z.ItemCode = '{sUomCode}' )"), 0, "", 0);
            string GetUOM = $"SELECT A.UomEntry FROM OUOM A WHERE A.UomCode = ( SELECT ISNULL(z.SalUnitMsr,z.InvntryUom) FROM OITM z WHERE z.CodeBars = '{sUomCode}' )";
            result = string.IsNullOrEmpty(result) ? helper.ReadDataRow(hana.Get(GetUOM), 0, "", 0) : "0";
            return string.IsNullOrEmpty(result) ? "0" : result;
        }


        #region Generic Uploading
        public void UploadSQLmarketingDocument(string header, string row, string module, string fileName)
        {
            // initialization
            try
            {
                int index = 0;
                var sql = new SAPMsSqlAccess();
                var context = new SAOContext();
                var hana = new SAPHanaAccess();
                var helper = new DataHelper();
                var serviceLayerAccess = new ServiceLayerAccess();
                int count = DECLARE.udf.Where(x => x.ObjCode == objType).Count();
                string tableName = "";
                string objDocLines = "DocumentLines";
                bool uploaded = false;
                var oinvID = 0;
                bool isGroupByName = false;
                if (module == "Delivery")
                {
                    module = "DeliveryNotes";
                    tableName = "ODLN";
                }
                else if (module == "Sales Order")
                {
                    module = "Orders";
                    tableName = "ORDR";
                }
                //Change Request 01/13/2020
                //else if (module == "Sales Quotation")
                //Change Request 11/04/2020
                //else if (module == "RAS Report")
                else if (module == "RAS Report" || module == "Sales Quotation")
                {
                    module = "Quotations";
                    tableName = "OQUT";
                }
                else if (module == "Inventory Transfer Request")
                {
                    module = "InventoryTransferRequests";
                    objDocLines = "StockTransferLines";
                    tableName = "OWTQ";
                }
                else if (module == "A/R invoice")
                {
                    module = "Invoices";
                    objDocLines = "DocumentLines";
                    tableName = "OINV";
                    SINo = 0;
                }
                else if (module == "A/R invoice (Group By Name)")
                {
                    isGroupByName = true;
                    module = "Invoices";
                    objDocLines = "DocumentLines";
                    tableName = "OINV";
                    SINo = 0;
                }
                else if (module == "A/R Credit Memo")
                {
                    module = "CreditNotes";
                    objDocLines = "DocumentLines";
                    tableName = "ORIN";
                }

                var query = "";
                if (isGroupByName)
                {
                    // Header Group BY NAME LOGIC QRY
                    query = $"SELECT * FROM (SELECT {header.Replace("DiscPrcnt", "MAX(DiscPrcnt) as DiscPrcnt")} FROM MarketingDocumentHeaders as Header WHERE ISNULL(Header.CardCode ,'') <> '' " +
                        $"AND Header.Session = '{PublicStatic.DtRunID}' AND Header.[User] = '{Environment.MachineName}' AND Header.CardCode like '%-%' GROUP BY {header.Replace("DiscPrcnt,", "")}) x " +
                        $"WHERE ISNULL(x.U_OrderNo, '') <> '' AND ISNULL(x.CardName, '') <> ''";
                }
                else
                {
                    // get all bp involve in this uploading
                    query = $"SELECT {header} FROM MarketingDocumentHeaders as Header " +
                    $"WHERE ISNULL(Header.CardCode ,'') <> '' AND Header.Session = '{PublicStatic.DtRunID}' AND Header.[User] = '{Environment.MachineName}' AND Header.CardCode like '%-%' GROUP BY {header}";

                    if (sql.Get(query).Rows.Count == 0) // WTR Header
                    {
                        query = $"SELECT {header} FROM MarketingDocumentHeaders as Header " +
                        $"WHERE Header.Session = '{PublicStatic.DtRunID}' AND Header.[User] = '{Environment.MachineName}' AND ISNULL(FromWarehouse, '') <> '' AND ISNULL(ToWarehouse, '') <> '' GROUP BY {header}";
                    }

                }

                if (module == "CreditNotes" && frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE"))
                {
                    query = $"SELECT {header} FROM MarketingDocumentHeaders as Header " +
                 $"WHERE ISNULL(Header.CardCode ,'') <> '' AND Header.Session = '{PublicStatic.DtRunID}' AND Header.[User] = '{Environment.MachineName}' AND Header.CardCode like '%-%' " +
                 //$"AND Header.DocDate != 'CreditDate' " +
                 //$" AND Header.DocDate not like '%ARCM%' " +
                 $"AND ISNULL(Header.U_OrderNo, '') <> '' " +
                 $"GROUP BY {header}";
                }

                //if (module.Contains("CreditNotes") && !string.IsNullOrEmpty(frmDT_UDF.CardCode) && cardcode == false)
                //{
                //    query = $"SELECT {header}, CardCode FROM MarketingDocumentHeaders as Header " +
                //$"WHERE ISNULL(Header.CardCode ,'') <> '' AND Header.Session = '{PublicStatic.DtRunID}' AND Header.[User] = '{Environment.MachineName}' GROUP BY {header}, CardCode, Id order by Id asc";
                //}

                var headers = sql.Get(query);
                if (headers != null)
                {
                    for (int hdrRow = 0; headers.Rows.Count > hdrRow; hdrRow++)
                    {
                        Dictionary<string, string> head = new Dictionary<string, string>();
                        string DocEntry = "";
                        //int[] xDocEntry = new int[] { };
                        var xID = new int[] { };
                        string CardCode = "";
                        string OrderNo = "";
                        string CardName = "";
                        string Address = "";
                        bool blockShopee = false;
                        var transferType = frmDT_UDF.TransactionType;

                        var getDocEntryQuery = $"SELECT DocEntry, CONVERT(varchar,Id) [Id] FROM MarketingDocumentHeaders as Header " +
                                                    $"WHERE Header.Session = '{PublicStatic.DtRunID}' AND Header.[User] = '{Environment.MachineName}' ";

                        StaticHelper._MainForm.Invoke(new Action(() =>
                            StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {hdrRow + 1} out of {headers.Rows.Count}", hdrRow + 1, headers.Rows.Count)
                        ));

                        for (int hdrCol = 0; headers.Columns.Count > hdrCol; hdrCol++)
                        {
                            var columnName = headers.Columns[hdrCol].ColumnName;
                            var rowData = headers.Rows[hdrRow][columnName] == null ? "" : headers.Rows[hdrRow][columnName].ToString();


                            if (columnName != "Id")
                            {
                                if (columnName == "CardCode")
                                {
                                    CardCode = rowData;
                                    if (string.IsNullOrEmpty(transferType) == false)
                                    {
                                        Address = GetTransactionType(transferType, "Address", CardCode);
                                        head.Add("U_AddID", Address);
                                    }

                                }

                                if (columnName == "U_OrderNo")
                                {
                                    OrderNo = rowData;
                                }

                                if (columnName == "CardName")
                                {
                                    CardName = rowData;
                                }

                                if (columnName.ToUpper().Contains("DATE"))
                                {
                                    if (rowData != string.Empty || row != null)
                                    {
                                        bool isNumber = int.TryParse(rowData, out int numericValue);

                                        if (rowData.Contains(".") || rowData.Contains("-") || rowData.Contains("/"))
                                        {
                                            var date = DateTime.TryParse(rowData, out DateTime outDate);
                                            if (date)
                                            {
                                                rowData = Convert.ToDateTime(outDate).ToString("yyyyMMdd");
                                            }
                                        }
                                    }
                                }

                                getDocEntryQuery += $" AND {(columnName.ToUpper().Contains("DATE") ? $"replace(replace(replace(SUBSTRING(isnull({columnName},''),1,10),'-',''),'/',''),'.','') " : $"isnull({columnName},'')")} = '{rowData}' ";

                                if (columnName == "DiscPrcnt")
                                {
                                    columnName = "DiscountPercent";
                                }

                                if (!string.IsNullOrEmpty(rowData))
                                {
                                    head.Add(columnName, rowData);

                                    //if (columnName.Contains("U_DocType"))
                                    //{
                                    //    frmDT_UDF.DocumentType = rowData;
                                    //}

                                }


                            }
                        }

                        var doc = sql.Get(getDocEntryQuery);

                        if (doc != null)
                        {
                            if (doc.Rows.Count > 0)
                            {
                                var qwe = doc.Rows.Cast<DataRow>().Select(x => x[0]).ToArray();

                                DocEntry = string.Join("','", qwe);
                                xID = doc.AsEnumerable().Select(x => Convert.ToInt32(x.Field<string>("Id"))).ToArray();
                            }
                        }

                        //Removed sum due to conflict in quantity by Cedi 07292019
                        var query2 = "";

                        //if (module == "CreditNotes" && !CardCode.ToUpper().Contains("SHOPEE"))
                        //{
                        //    query2 = $"SELECT {row.Replace("Quantity", "SUM(CAST(ISNULL(Quantity,0) AS FLOAT)) [Quantity]")} from (" +
                        //    //On Comment due to conflict in process 03/07/2020
                        //    //$"SELECT distinct {row} " +
                        //    $"SELECT {row} " +
                        //"FROM MarketingDocumentLines as lines " +
                        //$"WHERE ISNULL(ItemCode,'') <> '' AND {(!row.Contains("Quantity") ? (row.Contains("PriceAfVAT") ? "PriceAfVAT IS NOT NULL and PriceAfVAT <> '0' AND " : "") : "")}{(row.Contains("Quantity") ? "Quantity IS NOT NULL and CAST(Quantity AS FLOAT) <> 0 AND " : "")} " +
                        //$"lines.Session = '{PublicStatic.DtRunID}' AND lines.[User] = '{Environment.MachineName}' AND lines.DocEntry in ('{DocEntry}') ) MT1 " +
                        //$" ORDER BY MT1.ItemCode";
                        //}
                        //else
                        //{
                        if (isGroupByName)
                        {
                            List<string> termsList = new List<string>();
                            var arr = row.Split(',');
                            for (int i = 0; i < arr.Length; i++)
                            {
                                termsList.Add("B." + arr[i]);
                            }
                            var fieldString = string.Join(",", termsList);
                            query2 = $"SELECT {fieldString.Replace("B.Quantity", "SUM(CAST(ISNULL(Quantity,0) AS FLOAT)) [Quantity]")} " +
                                $"FROM MarketingDocumentHeaders A " +
                                $"INNER JOIN MarketingDocumentLines B ON A.DocEntry = B.DocEntry AND B.Session = A.Session AND B.[User] = A.[User]" +
                                $"WHERE ISNULL(B.ItemCode,'') <> '' AND A.U_OrderNo = '{OrderNo}' AND B.Session = '{PublicStatic.DtRunID}' AND B.[User] = '{Environment.MachineName}' " +
                                $"GROUP BY {fieldString.Replace(",B.Quantity", "")}; ";
                        }
                        else
                        {
                            query2 = $"SELECT {row.Replace("Quantity", "SUM(CAST(ISNULL(Quantity,0) AS FLOAT)) [Quantity]")} from (" +
                        //On Comment due to conflict in process 03/07/2020
                        //$"SELECT distinct {row} " +
                        $"SELECT {row} " +
                    "FROM MarketingDocumentLines as lines " +
                    $"WHERE ISNULL(ItemCode,'') <> '' AND {(!row.Contains("Quantity") ? (row.Contains("PriceAfVAT") ? "PriceAfVAT IS NOT NULL and PriceAfVAT <> '0' AND " : "") : "")}{(row.Contains("Quantity") ? "Quantity IS NOT NULL and CAST(Quantity AS FLOAT) <> 0 AND " : "")} " +
                    $"lines.Session = '{PublicStatic.DtRunID}' AND lines.[User] = '{Environment.MachineName}' AND lines.DocEntry in ('{DocEntry}') ) MT1 " +
                    $"GROUP BY {row.Replace(",Quantity", "")} ORDER BY MT1.ItemCode";
                        }

                        //}


                        var rows = sql.Get(query2);

                        var dictLines = new List<Dictionary<string, object>>();
                        var itemCode = "";
                        var merchCredit = "";
                        var reversal = false;
                        var price = "";

                        var maxLine = 0;


                        //Additional for AR INVOICE
                        bool headsAdded = false;
                        int SICount = 0;
                        if (rows != null)
                        {
                            int lineCount = 0;
                            int lineA = 0;

                            for (int lineRows = 0; rows.Rows.Count > lineRows; lineRows++)
                            {
                                var line = new Dictionary<string, object>();

                                for (int lineCol = 0; rows.Columns.Count > lineCol; lineCol++)
                                {
                                    var columnName = rows.Columns[lineCol].ColumnName;
                                    var rowData = rows.Rows[lineRows][columnName] == null ? "" : rows.Rows[lineRows][columnName].ToString();
                                    if (columnName != "Id" || columnName != "DocEntry" || string.IsNullOrEmpty(rowData))
                                    {
                                        switch (columnName)
                                        {
                                            case "PriceAfVAT":
                                                if (Convert.ToDouble(rowData) < 0 && module.Contains("CreditNotes"))
                                                {
                                                    var toPositive = Math.Abs(Convert.ToDouble(rowData));
                                                    rowData = toPositive.ToString();
                                                    price = "";
                                                }
                                                else if (Convert.ToDouble(rowData) > 0 && module.Contains("CreditNotes"))
                                                {
                                                    price = rowData;
                                                }
                                                line.Add("PriceAfterVAT", rowData);
                                                break;

                                            case "PriceBefDi":
                                                //if (module.Contains("DeliveryNotes"))
                                                //{
                                                //    line.Add("TaxCode", "OT1");
                                                //}
                                                line.Add("UnitPrice", rowData);
                                                break;

                                            case "ItemCode":

                                                //if (module == "CreditNotes" && CardCode.ToUpper().Contains("SHOPEE") && rowData == "Seller Voucher(PHP)"
                                                //    || module == "CreditNotes" && CardCode.ToUpper().Contains("SHOPEE") && rowData == "Seller Bundle Discount(PHP)")
                                                //{
                                                //    blockShopee = true;
                                                //}

                                                if (module == "CreditNotes" && CardCode.ToUpper().Contains("SHOPEE") && rowData == "Original Product Price")
                                                {
                                                    rowData = "Item Price Credit";
                                                }

                                                var item = controller.DeliveryItem(CardCode, rowData);
                                                itemCode = item;

                                                var whs = string.Empty;

                                                merchCredit = controller.MerchCredit(item);

                                                //if (head.Keys.Contains("U_DocType"))
                                                //{
                                                //    whs = controller.SalesOrderWhsCode(frmDT_UDF.DocumentType);
                                                //    if (!string.IsNullOrEmpty(whs))
                                                //    {
                                                //        line.Add("WarehouseCode", whs);
                                                //    }
                                                //}
                                                //else
                                                //{

                                                //MessageBox.Show($"Type - {frmDT_UDF.DocumentType}");

                                                whs = controller.SalesOrderWhsCode(frmDT_UDF.DocumentType);

                                                if (module == "InventoryTransferRequests")
                                                {
                                                
                                                    if (!line.Keys.Contains("FromWarehouseCode"))
                                                    {
                                                        

                                                        if (!string.IsNullOrEmpty(whs))
                                                        {
                                                            
                                                            line.Add("FromWarehouseCode", whs);
                                                        }
                                                        else
                                                        {
                                                           
                                                            whs = GetTransactionType(transferType, "WhsFrom", CardCode, Address);
                                                            //whs = controller.DeliveryWhsCode(CardCode);
                                                            
                                                            if (!string.IsNullOrEmpty(whs))
                                                            {
                                                                line.Add("FromWarehouseCode", whs);
                                                            }
                                                        }
                                                    }

                                                    var whsCode_2 = GetTransactionType(transferType, "WhsTo", CardCode, Address);

                                                    line.Add("WarehouseCode", whsCode_2);
                                                    
                                                }
                                                else
                                                {
                                                    if (!line.Keys.Contains("WarehouseCode"))
                                                    {
                                                        if (!string.IsNullOrEmpty(whs))
                                                        {
                                                            line.Add("WarehouseCode", whs);
                                                        }
                                                        else
                                                        {
                                                            whs = controller.DeliveryWhsCode(CardCode);

                                                            if (!string.IsNullOrEmpty(whs))
                                                            {
                                                                line.Add("WarehouseCode", whs);
                                                            }
                                                        }
                                                    }
                                                }

                                                

                                               
                                                //}

                                                var autoFields = controller.DeliveryInfo(item);

                                                if (frmDT_UDF.DocumentDate != null)
                                                {
                                                    line.Add("ShipDate", Convert.ToDateTime(frmDT_UDF.DocumentDate).ToString("yyyyMMdd"));
                                                }

                                                if (autoFields.Count > 0)
                                                {
                                                    var projectCode = controller.GetProjectCode(CardCode);
                                                    var cogs = autoFields[0];
                                                    var itemDept = autoFields[1];
                                                    var itemSubDept = autoFields[2];
                                                    var color = autoFields[3];
                                                    var size = autoFields[4];
                                                    var style = autoFields[5];
                                                    var sort = autoFields[6];

                                                    if (!string.IsNullOrEmpty(projectCode) && !line.Keys.Contains("ProjectCode"))
                                                    {
                                                        line.Add("ProjectCode", controller.GetProjectCode(CardCode));
                                                    }

                                                    if (!line.Keys.Contains("COGSCostingCode2") && !string.IsNullOrEmpty(cogs) && objDocLines.Equals("StockTransferLines") == false)
                                                    {
                                                        line.Add("COGSCostingCode2", cogs);
                                                    }

                                                    if (!line.Keys.Contains("U_ItemDept") && !string.IsNullOrEmpty(itemDept))
                                                    {
                                                        line.Add("U_ItemDept", itemDept);
                                                    }

                                                    if (!line.Keys.Contains("U_ItemSubDept") && !string.IsNullOrEmpty(itemSubDept))
                                                    {
                                                        line.Add("U_ItemSubDept", itemSubDept);
                                                    }

                                                    if (!line.Keys.Contains("DistributionRule2") && !string.IsNullOrEmpty(cogs))
                                                    {
                                                        line.Add("DistributionRule2", cogs);
                                                    }

                                                    if (!line.Keys.Contains("U_Color") && !string.IsNullOrEmpty(color))
                                                    {
                                                        line.Add("U_Color", color);
                                                    }

                                                    if (!line.Keys.Contains("U_Size") && !string.IsNullOrEmpty(size))
                                                    {
                                                        line.Add("U_Size", size);
                                                    }

                                                    if (!line.Keys.Contains("U_Style") && !string.IsNullOrEmpty(style))
                                                    {
                                                        line.Add("U_Style", style);
                                                    }

                                                    if (!line.Keys.Contains("U_SortCode") && !string.IsNullOrEmpty(sort))
                                                    {
                                                        line.Add("U_SortCode", sort);
                                                    }

                                                    var qry = string.Format(helper.ReadDataRow(hana.Get(SP.DFLT_ItemCompany), 1, "", 0), CardCode, item);
                                                    var company = helper.ReadDataRow(hana.Get(qry), 0, "", 0);
                                                    if (!line.Keys.Contains("U_Company") && !string.IsNullOrEmpty(company))
                                                    {
                                                        line.Add("U_Company", company);
                                                    }

                                                    var sku = GetTransactionType(transferType, "SKU", CardCode, item);
                                                    if (!line.Keys.Contains("U_SKU") && !string.IsNullOrEmpty(sku))
                                                    {
                                                        line.Add("U_SKU", sku);
                                                    }

                                                    if (!line.Keys.Contains("U_BrandName") && !string.IsNullOrEmpty(sort))
                                                    {
                                                        line.Add("U_BrandName", GetTransactionType(transferType, "Brand", item));
                                                    }

                                                    var unitPrice = GetTransactionType(transferType, "UnitPrice", CardCode, item);
                                                    if (!line.Keys.Contains("UnitPrice") && !string.IsNullOrEmpty(unitPrice) && objDocLines.Equals("StockTransferLines"))
                                                    {
                                                        line.Add("UnitPrice", unitPrice);
                                                    }

                                                    if (!row.Contains("UoMEntry") || !row.Contains("UoMCode"))
                                                    {
                                                        line.Add("UoMEntry", GetUomID(rowData));
                                                    }

                                                    //sbJson.AppendLine($@"       ""U_BrandName"": ""{row.Cells["Brand"].Value.ToString()}"",");
                                                }

                                                line.Add(columnName, item);
                                                break;

                                            case "ProjectCode":

                                                var projCode = controller.GetProjectCode(CardCode);

                                                line.Add("ProjectCode", projCode);
                                                break;

                                            case "WarehouseCode":

                                                var whsCode = controller.DeliveryWhsCode(CardCode);
                                                if (!string.IsNullOrEmpty(whsCode))
                                                {
                                                    line.Add("WarehouseCode", whsCode);
                                                }

                                                break;

                                            case "FromWarehouseCode":

                                                var whsCode2 = controller.DeliveryWhsCode(CardCode);

                                                line.Add("FromWarehouseCode", whsCode2);
                                                break;

                                            case "VatGroup":
                                                //try cache added by epi except inside the try
                                                try
                                                {
                                                    var vat = Convert.ToDouble(rowData);
                                                    if (vat > 0)
                                                    {
                                                        var vatGroup = controller.VatGroup(itemCode);
                                                        line.Add("VatGroup", vatGroup);
                                                    }
                                                    else
                                                    {
                                                        line.Add("VatGroup", "OTNV");
                                                    }
                                                }
                                                catch
                                                {
                                                    line.Add("VatGroup", rowData);//added by epi
                                                }


                                                break;

                                            case "WtLiable":
                                                if (rowData.Contains("No"))
                                                {
                                                    line.Add("WTLiable", "N");
                                                }
                                                else
                                                {
                                                    var wtLiable = controller.WhtTax(itemCode);
                                                    line.Add("WTLiable", wtLiable);
                                                }
                                                break;


                                            default:

                                                if (columnName.ToUpper().Contains("DATE"))
                                                {
                                                    if (rowData != string.Empty || row != null)
                                                    {
                                                        if (rowData.Contains(".") || rowData.Contains("-") || rowData.Contains("/"))
                                                        {
                                                            rowData = Convert.ToDateTime(rowData).ToString("yyyyMMdd");
                                                        }
                                                    }
                                                }
                                                if (columnName.ToUpper().Contains("PRICE") && module.Contains("CreditNotes"))
                                                {
                                                    if (rowData != string.Empty || row != null)
                                                    {
                                                        if (Convert.ToDouble(rowData) < 0)
                                                        {
                                                            var toPositive = Math.Abs(Convert.ToDouble(rowData));
                                                            rowData = toPositive.ToString();
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(price))
                                                {
                                                    if (merchCredit == "N" && Convert.ToDouble(price) > 0 && module.Contains("CreditNotes"))
                                                    {
                                                        if (columnName.ToUpper().Contains("Quantity"))
                                                        {
                                                            if (rowData != string.Empty || row != null)
                                                            {
                                                                int nega = Convert.ToInt32(rowData) * -1;
                                                                rowData = nega.ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                                line.Add(columnName, rowData);
                                                break;
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(price))
                                {
                                    if (merchCredit == "N" && Convert.ToDouble(price) > 0 && module.Contains("CreditNotes"))
                                    {
                                        if (!rows.Columns.Contains("Quantity"))
                                        {
                                            line.Add("Quantity", "-1");
                                        }
                                    }
                                }

                                dictLines.Add(line);
                                if (merchCredit == "Y" && module.Contains("CreditNotes"))
                                {
                                    line = new Dictionary<string, object>();

                                    for (int lineCol = 0; rows.Columns.Count > lineCol; lineCol++)
                                    {
                                        var columnName = rows.Columns[lineCol].ColumnName;
                                        var rowData = rows.Rows[lineRows][columnName] == null ? "" : rows.Rows[lineRows][columnName].ToString();
                                        if (columnName != "Id" || columnName != "DocEntry" || string.IsNullOrEmpty(rowData))
                                        {
                                            switch (columnName)
                                            {
                                                case "PriceAfVAT":
                                                    if (Convert.ToDouble(rowData) < 0 && module.Contains("CreditNotes"))
                                                    {
                                                        var toPositive = Math.Abs(Convert.ToDouble(rowData));
                                                        rowData = toPositive.ToString();
                                                    }
                                                    line.Add("PriceAfterVAT", rowData);
                                                    break;

                                                case "ItemCode":

                                                    if (module == "CreditNotes" && CardCode.ToUpper().Contains("SHOPEE") && rowData == "Original Product Price")
                                                    {
                                                        rowData = "Item Price Credit";
                                                    }

                                                    var item = controller.DeliveryItem(CardCode, rowData);
                                                    itemCode = item;
                                                    var whs = string.Empty;

                                                    merchCredit = controller.MerchCredit(item);
                                                    //if (head.Keys.Contains("U_DocType"))
                                                    //{
                                                    //    whs = controller.SalesOrderWhsCode(frmDT_UDF.DocumentType);
                                                    //    if (!string.IsNullOrEmpty(whs))
                                                    //    {
                                                    //        line.Add("WarehouseCode", whs);
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    whs = controller.SalesOrderWhsCode(frmDT_UDF.DocumentType);

                                                    if (!line.Keys.Contains("WarehouseCode"))
                                                    {
                                                        if (!string.IsNullOrEmpty(whs))
                                                        {
                                                            line.Add("WarehouseCode", whs);
                                                        }
                                                        else
                                                        {
                                                            whs = controller.DeliveryWhsCode(CardCode);

                                                            if (!string.IsNullOrEmpty(whs))
                                                            {
                                                                line.Add("WarehouseCode", whs);
                                                            }
                                                        }
                                                    }
                                                    //}

                                                    var autoFields = controller.DeliveryInfo(item);

                                                    if (frmDT_UDF.DocumentDate != null)
                                                    {
                                                        line.Add("ShipDate", Convert.ToDateTime(frmDT_UDF.DocumentDate).ToString("yyyyMMdd"));
                                                    }

                                                    if (autoFields.Count > 0)
                                                    {
                                                        var projectCode = controller.GetProjectCode(CardCode);
                                                        var cogs = autoFields[0];
                                                        var itemDept = autoFields[1];
                                                        var itemSubDept = autoFields[2];
                                                        var color = autoFields[3];
                                                        var size = autoFields[4];
                                                        var style = autoFields[5];
                                                        var sort = autoFields[6];

                                                        if (!string.IsNullOrEmpty(projectCode) && !line.Keys.Contains("ProjectCode"))
                                                        {
                                                            line.Add("ProjectCode", controller.GetProjectCode(CardCode));
                                                        }

                                                        if (!line.Keys.Contains("COGSCostingCode2") && !string.IsNullOrEmpty(cogs) && objDocLines.Equals("StockTransferLines") == false)
                                                        {
                                                            line.Add("COGSCostingCode2", cogs);
                                                        }

                                                        if (!line.Keys.Contains("U_ItemDept") && !string.IsNullOrEmpty(itemDept))
                                                        {
                                                            line.Add("U_ItemDept", itemDept);
                                                        }

                                                        if (!line.Keys.Contains("U_ItemSubDept") && !string.IsNullOrEmpty(itemSubDept))
                                                        {
                                                            line.Add("U_ItemSubDept", itemSubDept);
                                                        }

                                                        if (!line.Keys.Contains("DistributionRule2") && !string.IsNullOrEmpty(cogs))
                                                        {
                                                            line.Add("DistributionRule2", cogs);
                                                        }

                                                        if (!line.Keys.Contains("U_Color") && !string.IsNullOrEmpty(color))
                                                        {
                                                            line.Add("U_Color", color);
                                                        }

                                                        if (!line.Keys.Contains("U_Size") && !string.IsNullOrEmpty(size))
                                                        {
                                                            line.Add("U_Size", size);
                                                        }

                                                        if (!line.Keys.Contains("U_Style") && !string.IsNullOrEmpty(style))
                                                        {
                                                            line.Add("U_Style", style);
                                                        }

                                                        if (!line.Keys.Contains("U_SortCode") && !string.IsNullOrEmpty(sort))
                                                        {
                                                            line.Add("U_SortCode", sort);
                                                        }

                                                        var qry = string.Format(helper.ReadDataRow(hana.Get(SP.DFLT_ItemCompany), 1, "", 0), CardCode, item);
                                                        var company = helper.ReadDataRow(hana.Get(qry), 0, "", 0);
                                                        if (!line.Keys.Contains("U_Company") && !string.IsNullOrEmpty(company))
                                                        {
                                                            line.Add("U_Company", company);
                                                        }

                                                        var sku = GetTransactionType(transferType, "SKU", CardCode, item);
                                                        if (!line.Keys.Contains("U_SKU") && !string.IsNullOrEmpty(sku))
                                                        {
                                                            line.Add("U_SKU", sku);
                                                        }

                                                        if (!line.Keys.Contains("U_BrandName") && !string.IsNullOrEmpty(sort))
                                                        {
                                                            line.Add("U_BrandName", GetTransactionType(transferType, "Brand", item));
                                                        }

                                                        var unitPrice = GetTransactionType(transferType, "UnitPrice", CardCode, item);
                                                        if (!line.Keys.Contains("UnitPrice") && !string.IsNullOrEmpty(unitPrice) && objDocLines.Equals("StockTransferLines"))
                                                        {
                                                            line.Add("UnitPrice", unitPrice);
                                                        }

                                                        if (!row.Contains("UoMEntry") || !row.Contains("UoMCode"))
                                                        {
                                                            line.Add("UoMEntry", GetUomID(rowData));
                                                        }

                                                        //sbJson.AppendLine($@"       ""U_BrandName"": ""{row.Cells["Brand"].Value.ToString()}"",");
                                                    }

                                                    line.Add(columnName, item);
                                                    break;

                                                case "ProjectCode":

                                                    var projCode = controller.GetProjectCode(CardCode);

                                                    line.Add("ProjectCode", projCode);
                                                    break;

                                                case "WarehouseCode":

                                                    var whsCode = controller.DeliveryWhsCode(CardCode);
                                                    if (!string.IsNullOrEmpty(whsCode))
                                                    {
                                                        line.Add("WarehouseCode", whsCode);
                                                    }

                                                    break;

                                                case "FromWarehouseCode":

                                                    var whsCode2 = controller.DeliveryWhsCode(CardCode);

                                                    line.Add("FromWarehouseCode", whsCode2);
                                                    break;

                                                case "VatGroup":
                                                    var vat = Convert.ToDouble(rowData);
                                                    if (vat > 0)
                                                    {
                                                        var vatGroup = controller.VatGroup(itemCode);
                                                        line.Add("VatGroup", vatGroup);
                                                    }
                                                    else
                                                    {
                                                        line.Add("VatGroup", "OTNV");
                                                    }

                                                    break;

                                                case "WtLiable":
                                                    if (rowData.Contains("No"))
                                                    {
                                                        line.Add("WTLiable", "N");
                                                    }
                                                    else
                                                    {
                                                        var wtLiable = controller.WhtTax(itemCode);
                                                        line.Add("WTLiable", wtLiable);
                                                    }
                                                    break;


                                                default:

                                                    if (columnName.ToUpper().Contains("DATE"))
                                                    {
                                                        if (rowData != string.Empty || row != null)
                                                        {
                                                            if (rowData.Contains(".") || rowData.Contains("-") || rowData.Contains("/"))
                                                            {
                                                                rowData = Convert.ToDateTime(rowData).ToString("yyyyMMdd");
                                                            }
                                                        }
                                                    }
                                                    if (columnName.ToUpper().Contains("PRICE") && module.Contains("CreditNotes"))
                                                    {
                                                        if (rowData != string.Empty || row != null)
                                                        {
                                                            if (Convert.ToDouble(rowData) < 0)
                                                            {
                                                                var toPositive = Math.Abs(Convert.ToDouble(rowData));
                                                                rowData = toPositive.ToString();
                                                            }
                                                        }
                                                    }
                                                    if (columnName.ToUpper().Contains("Quantity") && module.Contains("CreditNotes"))
                                                    {
                                                        if (rowData != string.Empty || row != null)
                                                        {
                                                            int nega = Convert.ToInt32(rowData) * -1;
                                                            rowData = nega.ToString();
                                                        }
                                                    }
                                                    line.Add(columnName, rowData);
                                                    break;
                                            }
                                        }
                                    }
                                    if (!rows.Columns.Contains("Quantity") && module.Contains("CreditNotes"))
                                    {
                                        line.Add("Quantity", "-1");
                                    }
                                    dictLines.Add(line);
                                }

                                lineCount++;
                                lineA++;




                                //LOGIC WITH MAX LINE
                                //insert logic here
                                maxLine = module == "Invoices" ? frmDT_UDF.MaxLine : maxLine;

                                if (maxLine > 0 && module == "Invoices")
                                {
                                    if (lineCount == maxLine || lineA == rows.Rows.Count)
                                    {

                                        if (!headsAdded)
                                        {
                                            // Addition Upload fields for header
                                            if (count > 0)
                                            {
                                                foreach (var x in DECLARE.udf.Where(x => x.ObjCode == objType))
                                                {
                                                    if (!head.Keys.Contains((x.FieldCode)))
                                                    {
                                                        var rowData = x.FieldValue;

                                                        if (x.FieldCode.ToUpper().Contains("DATE"))
                                                        {
                                                            if (rowData != string.Empty || row != null)
                                                            {
                                                                if (rowData.Contains(".") || rowData.Contains("-") || rowData.Contains("/"))
                                                                {
                                                                    rowData = Convert.ToDateTime(rowData).ToString("yyyyMMdd");
                                                                }
                                                            }
                                                        }
                                                        if (module.Contains("Invoices"))
                                                        {
                                                            if (x.FieldCode.Contains("SINo"))
                                                            {
                                                                SINo = SINo != 0 ? SINo : Convert.ToInt32(rowData);
                                                                head.Add(x.FieldCode, Convert.ToString(SINo));
                                                            }
                                                            else
                                                            {
                                                                head.Add(x.FieldCode, rowData);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            head.Add(x.FieldCode, rowData);
                                                        }
                                                    }
                                                }
                                            }

                                            if (module == "Invoices")
                                            {
                                                if (!head.Keys.Contains("U_DocType"))
                                                {
                                                    head.Add("U_DocType", frmDT_UDF.DocumentType);
                                                }

                                                //NO OF SI PER TRANSACTION
                                                if (!head.Keys.Contains("U_ContainerNo"))
                                                {
                                                    head.Add("U_ContainerNo", (++SICount).ToString());
                                                }

                                                if (!head.Keys.Contains("U_RunID"))
                                                {
                                                    ++oinvID;
                                                    head.Add("U_RunID", PublicStatic.DtRunID + oinvID.ToString());
                                                }

                                                head.Add("Series", "4");
                                            }
                                        }



                                        if (head.Count != 0 && !headsAdded)
                                        {
                                            if (head.Keys.Contains("U_PostRem"))
                                            {
                                                head.Remove("U_PostRem");
                                            }

                                            SboCredentials sboCred = new SboCredentials();
                                            head.Add("U_PostRem", $"Created by EasySAP | Data Transfer : {sboCred.UserId} : {DateTime.Now} : {fileName} | Powered By : DIREC");

                                            if (head.Keys.Contains("U_UploadID"))
                                            {
                                                head.Remove("U_UploadID");
                                            }

                                            head.Add("U_UploadID", PublicStatic.DtRunID);


                                            if (!head.Keys.Contains("U_PrepBy"))
                                            {
                                                head.Add("U_PrepBy", EasySAPCredentialsModel.EmployeeCompleteName);
                                            }

                                            // Convert everthing above to json string
                                            var json = DataRepository.JsonBuilder(head, dictLines, objDocLines);

                                            //MessageBox.Show("This is Uploading 1");

                                            //string fullFilePath = $@"\\192.168.0.29\easy sap\0INSTALLER\Extract Tests\JsonFile{index}.txt";
                                            //File.WriteAllText(fullFilePath, $"{json}");

                                            //MessageBox.Show($"Save in the - {fullFilePath}");

                                            string message = "";

                                            var xDocDate = head.Where(x => x.Key == "DocDate");

                                            var dateParam = xDocDate.Any() ? $"AND DocDate = '{xDocDate.FirstOrDefault().Value}'" : "";

                                            DataTable transExist = new DataTable();
                                            transExist = hana.Get($"SELECT 'true' FROM {tableName} WHERE CardCode = '{CardCode}' AND Comments LIKE 'Created by EasySAP | Data Transfer : %{fileName}% | Powered By : DIREC' {dateParam}");

                                            if (helper.DataTableExist(transExist) == false)
                                            {
                                                //if (json.ToString().Contains("C-ALLH018"))
                                                //{
                                                //    string yow = "what";
                                                //}

                                                //if(module == "CreditNotes" && CardCode.ToUpper().Contains("SHOPEE") && dictLines.ToString().Contains("Seller Voucher(PHP)"))
                                                //{
                                                //    var xxx = "";
                                                //}

                                                if (blockShopee)
                                                {
                                                    IQueryable<MarketingDocumentHeaders> model = null;

                                                    if (string.IsNullOrEmpty(CardCode))
                                                    {
                                                        model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                        x.User == Environment.MachineName);
                                                    }
                                                    else
                                                    {
                                                        model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                            x.User == Environment.MachineName && x.CardCode == CardCode && xID.Contains(x.Id));
                                                    }

                                                    //MessageBox.Show(CardCode + message + PublicStatic.DtRunID + Environment.MachineName);

                                                    foreach (var qwe in model)
                                                    {
                                                        qwe.Uploaded = "No";
                                                        qwe.ErrorMessage = "error: Seller Bundle Discount(PHP)/Seller Voucher(PHP) is not equal to zero";
                                                    }
                                                }

                                                if (!blockShopee)
                                                {
                                                    if (!serviceLayerAccess.ServiceLayer_Posting(json, "POST", module, "DocEntry", out message, out string val))
                                                    {
                                                        IQueryable<MarketingDocumentHeaders> model = null;

                                                        if (string.IsNullOrEmpty(CardCode))
                                                        {
                                                            model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                            x.User == Environment.MachineName);
                                                        }
                                                        else
                                                        {
                                                            model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                                x.User == Environment.MachineName && x.CardCode == CardCode && xID.Contains(x.Id));
                                                        }

                                                        //MessageBox.Show(CardCode + message + PublicStatic.DtRunID + Environment.MachineName);

                                                        foreach (var qwe in model)
                                                        {
                                                            qwe.Uploaded = "No";
                                                            qwe.ErrorMessage = message;
                                                        }

                                                        //Get all docentry with the same headers and cancel it
                                                        var cc = head.TryGetValue("U_OrderNo", out string orderno).ToString();
                                                        string GetUOM = $"SELECT DocEntry FROM OINV WHERE U_OrderNo = '{orderno}' and U_UploadID = '{PublicStatic.DtRunID}' and U_RunID = '{PublicStatic.DtRunID}{oinvID}'";
                                                        var keysss = hana.Get(GetUOM);

                                                        foreach (var kk in keysss.Rows)
                                                        {
                                                            string url = $"Invoices({kk})/Cancel";
                                                            bool isPosted = serviceLayerAccess.ServiceLayer_Posting(new StringBuilder(), "POST", url, "DocEntry", out message, out val);
                                                        }

                                                        if (keysss.Rows.Count > 0)
                                                        {
                                                            var cancelled = hana.Get("SELECT top 1 T0.U_SINo, T0.DocNum FROM OINV T0 WHERE T0.CANCELED != 'Y' ORDER BY T0.DocEntry desc").Rows[0][0].ToString();
                                                            SINo = Convert.ToInt32(cancelled);
                                                        }

                                                        context.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        var model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                            x.User == Environment.MachineName && x.CardCode == CardCode && xID.Contains(x.Id));

                                                        foreach (var qwe in model)
                                                        {
                                                            qwe.Uploaded = "Yes";
                                                            qwe.ErrorMessage = message;
                                                        }

                                                        uploaded = true;

                                                        head.Remove("U_SINo");
                                                        head.Remove("U_ContainerNo");

                                                        if (module.Contains("Invoices"))
                                                        {
                                                            SINo = ++SINo;
                                                            SICount = ++SICount;
                                                            head.Add("U_SINo", Convert.ToString(SINo));
                                                            head.Add("U_ContainerNo", Convert.ToString(SICount));
                                                        }

                                                        dictLines = new List<Dictionary<string, object>>();

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                               x.User == Environment.MachineName && x.CardCode == CardCode && xID.Contains(x.Id));

                                                foreach (var qwe in model)
                                                {
                                                    qwe.Uploaded = "No";
                                                    qwe.ErrorMessage = "Document already exist.";
                                                }

                                            }

                                            headsAdded = true;

                                            context.SaveChanges();
                                        }
                                        else
                                        {

                                            string message = "";
                                            var json = DataRepository.JsonBuilder(head, dictLines, objDocLines);

                                            //MessageBox.Show("This is Uploading 2");

                                            //string fullFilePath = $@"\\192.168.0.29\easy sap\0INSTALLER\Extract Tests\JsonFile{index}.txt";
                                            //File.WriteAllText(fullFilePath, $"{json}");

                                            //MessageBox.Show($"Save in the - {fullFilePath}");

                                            if (blockShopee)
                                            {
                                                IQueryable<MarketingDocumentHeaders> model = null;

                                                if (string.IsNullOrEmpty(CardCode))
                                                {
                                                    model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                    x.User == Environment.MachineName);
                                                }
                                                else
                                                {
                                                    model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                        x.User == Environment.MachineName && x.CardCode == CardCode && xID.Contains(x.Id));
                                                }

                                                //MessageBox.Show(CardCode + message + PublicStatic.DtRunID + Environment.MachineName);

                                                foreach (var qwe in model)
                                                {
                                                    qwe.Uploaded = "No";
                                                    qwe.ErrorMessage = "error: Seller Bundle Discount(PHP)/Seller Voucher(PHP) is not equal to zero";
                                                }
                                            }

                                            if (!blockShopee)
                                            {
                                                if (!serviceLayerAccess.ServiceLayer_Posting(json, "POST", module, "DocEntry", out message, out string val))
                                                {
                                                    IQueryable<MarketingDocumentHeaders> model = null;

                                                    if (string.IsNullOrEmpty(CardCode))
                                                    {
                                                        model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                        x.User == Environment.MachineName);
                                                    }
                                                    else
                                                    {
                                                        model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                            x.User == Environment.MachineName && x.CardCode == CardCode && xID.Contains(x.Id));
                                                    }

                                                    //MessageBox.Show(CardCode + message + PublicStatic.DtRunID + Environment.MachineName);

                                                    foreach (var qwe in model)
                                                    {
                                                        qwe.Uploaded = "No";
                                                        qwe.ErrorMessage = message;
                                                    }

                                                    context.SaveChanges();

                                                    //Get all docentry with the same headers and cancel it
                                                    var cc = head.TryGetValue("U_OrderNo", out string orderno).ToString();
                                                    string GetUOM = $"SELECT DocEntry FROM OINV WHERE U_OrderNo = '{orderno}' and U_UploadID = '{PublicStatic.DtRunID}' and U_RunID = '{oinvID}'";
                                                    var keysss = hana.Get(GetUOM);

                                                    foreach (DataRow kk in keysss.Rows)
                                                    {
                                                        string url = $"Invoices({kk[0]})/Cancel";
                                                        bool isPosted = serviceLayerAccess.ServiceLayer_Posting(new StringBuilder(), "POST", url, "DocEntry", out message, out val);
                                                    }

                                                    if (keysss.Rows.Count > 0)
                                                    {
                                                        var cancelled = hana.Get("SELECT top 1 T0.U_SINo, T0.DocNum FROM OINV T0 WHERE T0.CANCELED != 'Y' ORDER BY T0.DocEntry desc").Rows[0][0].ToString();
                                                        SINo = Convert.ToInt32(cancelled);
                                                    }

                                                    break;
                                                }
                                                else
                                                {
                                                    var model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                        x.User == Environment.MachineName && x.CardCode == CardCode && xID.Contains(x.Id));

                                                    foreach (var qwe in model)
                                                    {
                                                        qwe.Uploaded = "Yes";
                                                        qwe.ErrorMessage = message;
                                                    }

                                                    uploaded = true;

                                                    head.Remove("U_SINo");
                                                    head.Remove("U_ContainerNo");

                                                    if (module.Contains("Invoices"))
                                                    {
                                                        SINo = ++SINo;
                                                        SICount = ++SICount;
                                                        head.Add("U_SINo", Convert.ToString(SINo));
                                                        head.Add("U_ContainerNo", Convert.ToString(SICount));
                                                    }
                                                    dictLines = new List<Dictionary<string, object>>();

                                                }
                                            }
                                        }
                                        lineCount = 0;
                                    }
                                }
                            }
                        }

                        if (maxLine == 0)
                        {

                            // Addition Upload fields for header
                            if (count > 0)
                            {
                                foreach (var x in DECLARE.udf.Where(x => x.ObjCode == objType))
                                {
                                    if (!head.Keys.Contains((x.FieldCode)))
                                    {
                                        var rowData = x.FieldValue;

                                        if (x.FieldCode.ToUpper().Contains("DATE"))
                                        {
                                            if (rowData != string.Empty || row != null)
                                            {
                                                if (rowData.Contains(".") || rowData.Contains("-") || rowData.Contains("/"))
                                                {
                                                    rowData = Convert.ToDateTime(rowData).ToString("yyyyMMdd");
                                                }
                                            }
                                        }
                                        if (module.Contains("Invoices"))
                                        {
                                            if (x.FieldCode.Contains("SINo"))
                                            {
                                                SINo = SINo != 0 ? ++SINo : Convert.ToInt32(rowData);
                                                head.Add(x.FieldCode, Convert.ToString(SINo));
                                            }
                                            else
                                            {
                                                head.Add(x.FieldCode, rowData);
                                            }
                                        }
                                        else
                                        {
                                            head.Add(x.FieldCode, rowData);
                                        }
                                    }
                                }
                            }

                            if (module == "DeliveryNotes")
                            {
                                head.Add("U_DocType", frmDT_UDF.DocumentType);
                                head.Add("Series", "6");

                                if (!head.Keys.Contains("DocDate"))
                                {
                                    head.Add("DocDate", Convert.ToDateTime(frmDT_UDF.PostingDate).ToString("yyyyMMdd"));
                                }

                                if (!head.Keys.Contains("DocDueDate"))
                                {
                                    head.Add("DocDueDate", Convert.ToDateTime(frmDT_UDF.DocumentDate).ToString("yyyyMMdd"));
                                }

                                if (!head.Keys.Contains("TaxDate"))
                                {
                                    head.Add("TaxDate", Convert.ToDateTime(frmDT_UDF.DocumentDate2).ToString("yyyyMMdd"));
                                }
                            }
                            else if (module == "Orders")
                            {
                                if (!head.Keys.Contains("U_DocType"))
                                {
                                    head.Add("U_DocType", frmDT_UDF.DocumentType);
                                }
                            }
                            else if (module == "InventoryTransferRequests")
                            {
                                //head.Add("U_DocType", frmDT_UDF.DocumentType);


                                if (!head.Keys.Contains("DocDate"))
                                {
                                    head.Add("DocDate", Convert.ToDateTime(frmDT_UDF.DocDate).ToString("yyyyMMdd"));
                                }

                                if (!head.Keys.Contains("DueDate"))
                                {
                                    head.Add("DueDate", Convert.ToDateTime(frmDT_UDF.DocDueDate).ToString("yyyyMMdd"));
                                }

                                if (!head.Keys.Contains("TaxDate"))
                                {
                                    head.Add("TaxDate", Convert.ToDateTime(frmDT_UDF.TaxDate).ToString("yyyyMMdd"));
                                }

                                if (!head.Keys.Contains("U_TransferType"))
                                {
                                    head.Add("U_TransferType", transferType);
                                }

                                if (!head.Keys.Contains("Series"))
                                {
                                    head.Add("Series", GetTransactionType(transferType, "Series"));
                                }

                                if (!head.Keys.Contains("FromWarehouse"))
                                {
                                    head.Add("FromWarehouse", GetTransactionType(transferType, "WhsFrom", CardCode, Address));
                                }

                                if (!head.Keys.Contains("ToWarehouse"))
                                {
                                    head.Add("ToWarehouse", GetTransactionType(transferType, "WhsTo", CardCode, Address));
                                }
                            }
                            else if (module == "Invoices")
                            {
                                if (!head.Keys.Contains("U_DocType"))
                                {
                                    head.Add("U_DocType", frmDT_UDF.DocumentType);
                                }
                                if (!head.Keys.Contains("Series"))
                                {
                                    head.Add("Series", "4");
                                }

                            }

                            if (head.Count != 0)
                            {
                                if (head.Keys.Contains("U_PostRem"))
                                {
                                    head.Remove("U_PostRem");
                                }

                                SboCredentials sboCred = new SboCredentials();
                                head.Add("U_PostRem", $"Created by EasySAP | Data Transfer : {sboCred.UserId} : {DateTime.Now} : {fileName} | Powered By : DIREC");

                                if (head.Keys.Contains("U_UploadID"))
                                {
                                    head.Remove("U_UploadID");
                                }

                                head.Add("U_UploadID", PublicStatic.DtRunID);


                                if (!head.Keys.Contains("U_PrepBy"))
                                {
                                    head.Add("U_PrepBy", EasySAPCredentialsModel.EmployeeCompleteName);
                                }

                                // Convert everthing above to json string
                                var json = DataRepository.JsonBuilder(head, dictLines, objDocLines);

                                //MessageBox.Show("This is Uploading 3");

                                //string fullFilePath = $@"\\192.168.0.29\easy sap\0INSTALLER\Extract Tests\JsonFile{index}.txt";
                                //File.WriteAllText(fullFilePath, $"{json}");

                                //MessageBox.Show($"Save in the - {fullFilePath}");

                                string message = "";

                                var xDocDate = head.Where(x => x.Key == "DocDate");

                                var dateParam = xDocDate.Any() ? $"AND DocDate = '{xDocDate.FirstOrDefault().Value}'" : "";
                                DataTable transExist = new DataTable();
                                transExist = hana.Get($"SELECT 'true' FROM {tableName} WHERE CardCode = '{CardCode}' AND Comments LIKE 'Created by EasySAP | Data Transfer : %{fileName}% | Powered By : DIREC' {dateParam}");

                                if (helper.DataTableExist(transExist) == false)
                                {
                                    //if (json.ToString().Contains("C-ALLH018"))
                                    //{
                                    //    string yow = "what";
                                    //}

                                    //if(module == "CreditNotes" && CardCode.ToUpper().Contains("SHOPEE") && dictLines.ToString().Contains("Seller Voucher(PHP)"))
                                    //{
                                    //    var xxx = "";
                                    //}

                                    if (blockShopee)
                                    {
                                        IQueryable<MarketingDocumentHeaders> model = null;

                                        if (string.IsNullOrEmpty(CardCode))
                                        {
                                            model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                            x.User == Environment.MachineName);
                                        }
                                        else
                                        {
                                            model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                x.User == Environment.MachineName && x.CardCode == CardCode && xID.Contains(x.Id));
                                        }

                                        //MessageBox.Show(CardCode + message + PublicStatic.DtRunID + Environment.MachineName);

                                        foreach (var qwe in model)
                                        {
                                            qwe.Uploaded = "No";
                                            qwe.ErrorMessage = "error: Seller Bundle Discount(PHP)/Seller Voucher(PHP) is not equal to zero";
                                        }
                                    }
                                    //bool isExported = false;
                                    //if (!isExported)
                                    //{

                                    //isExported = true;
                                    //}
                                    if (!blockShopee)
                                    {
                                        if (!serviceLayerAccess.ServiceLayer_Posting(json, "POST", module, "DocEntry", out message, out string val))
                                        {
                                            IQueryable<MarketingDocumentHeaders> model = null;
                                            if (CardCode == null || string.IsNullOrEmpty(CardCode))
                                            {
                                                model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                x.User == Environment.MachineName);
                                            }
                                            else
                                            {
                                                model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                    x.User == Environment.MachineName && xID.Contains(x.Id)); // && x.CardCode == CardCode
                                            }
                                            index++;
                                            
                                            StaticHelper.DownloadStringBuilderAsFile(json, $@"\\192.168.0.29\easy sap\0INSTALLER\Extract Tests\JsonFile{index}.txt");

                                            //MessageBox.Show(CardCode + message + PublicStatic.DtRunID + Environment.MachineName);
                                            foreach (var qwe in model)
                                            {
                                                qwe.Uploaded = "No";
                                                qwe.ErrorMessage = message;
                                            }

                                            
                                            //context.SaveChanges();
                                        }
                                        else
                                        {
                                            var model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                                x.User == Environment.MachineName && xID.Contains(x.Id)); // x.CardCode == CardCode &&

                                            foreach (var qwe in model)
                                            {
                                                qwe.Uploaded = "Yes";
                                                qwe.ErrorMessage = message;
                                            }

                                            uploaded = true;
                                        }
                                    }
                                }
                                else
                                {
                                    var model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
                                   x.User == Environment.MachineName && x.CardCode == CardCode && xID.Contains(x.Id));

                                    foreach (var qwe in model)
                                    {
                                        qwe.Uploaded = "No";
                                        qwe.ErrorMessage = "Document already exist.";
                                    }

                                }


                                context.SaveChanges();
                            }
                        }

                    }
                }

                if (uploaded == true)
                {
                    if (module.Contains("Invoices"))
                    {
                        var sapHana = new SAPHanaAccess();

                        var res = MetroMessageBox.Show(StaticHelper._MainForm, "Do you want to print uploaded transactions?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        var cr = new frmCrystalReports();
                        cr.oDocKey = PublicStatic.DtRunID;
                        cr.type = "AR";

                        if (res == DialogResult.Yes)
                        {
                            sapHana.Execute($"UPDATE OINV SET U_InvoicePrinted = 'Y' WHERE U_UploadID = '{PublicStatic.DtRunID}')");
                            cr.ShowDialog();
                        }
                        else
                        {

                        }

                        if (cr.print)
                        {
                            var pkl = MetroMessageBox.Show(StaticHelper._MainForm, "Do you want to print picklist?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                            if (pkl == DialogResult.Yes)
                            {
                                sapHana.Execute($"UPDATE OINV SET U_PKLPrinted = 'Y' WHERE U_UploadID = '{PublicStatic.DtRunID}')");
                                var crpkl = new frmCrystalReports();
                                crpkl.oDocKey = PublicStatic.DtRunID;
                                crpkl.type = "AR";
                                crpkl.ShowDialog();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ////ERRORFINDER
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                if (ex.Message.Contains("OutOfMemoryException"))
                {
                    GC.Collect();
                }
            }

        }

        private string NextKey()
        {
            try
            {
                var msSql = new SAPMsSqlAccess();
                var helper = new DataHelper();

                var dt = msSql.Get("SBO_GetNextAutoKey '1','Y'");
                var autokey = helper.ReadDataRow(dt, "AutoKey", "", 0);

                return autokey;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public void UploadToMain()
        {
            // initialization
            var sql = new SAPMsSqlAccess();
            var context = new SAOContext();
            var hana = new SAPHanaAccess();
            var helper = new DataHelper();
            var serviceLayerAccess = new ServiceLayerAccess();

            string query = "SELECT DocDate,CountDate,Counter,[Time],WhsCode,RefNo,NotedBy,Comments,SapUsername,SapCode,CheckedBy FROM DTOINC " +
                $"WHERE ISNULL(WhsCode,'') <> '' AND ISNULL(DocDate,'') <> '' AND ISNULL(SapCode,'') <> '' AND ISNULL(CountDate,'') <> '' AND RefNo = '{PublicStatic.DtRunID}' GROUP BY DocDate,CountDate,Counter,[Time],WhsCode,RefNo,NotedBy,Comments,SapUsername,SapCode,CheckedBy";

            var headers = sql.Get(query);
            int count = 1;
            if (helper.DataTableExist(headers))
            {
                foreach (DataRow item in headers.Rows)
                {
                    StaticHelper._MainForm.Invoke(new Action(() =>
                      StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to inventory counting table. {count++} out of {headers.Rows.Count}", count, headers.Rows.Count)
                  ));

                    string oDocDate = helper.ReadDataRow(item, "DocDate", "");
                    string oCountDate = helper.ReadDataRow(item, "CountDate", "");
                    string oCounter = helper.ReadDataRow(item, "Counter", "");
                    string oTime = helper.ReadDataRow(item, "Time", "");
                    string oWhsCode = helper.ReadDataRow(item, "WhsCode", "");
                    string oRefNo = helper.ReadDataRow(item, "RefNo", "");
                    string oNotedBy = helper.ReadDataRow(item, "NotedBy", "");
                    string oComments = helper.ReadDataRow(item, "Comments", "");
                    string oSapCode = helper.ReadDataRow(item, "SapCode", "");
                    string oCheckedBy = helper.ReadDataRow(item, "CheckedBy", "");

                    var header = new InventoryCounting();
                    int ids = int.Parse(NextKey());
                    header.DocEntry = ids;
                    header.DocDate = oDocDate;
                    header.CountDate = oCountDate;
                    header.Counter = oCounter;
                    header.Time = oTime;
                    header.WhsCode = oWhsCode;
                    header.NotedBy = oNotedBy;
                    header.Comments = oComments;
                    header.SapCode = oSapCode;
                    header.CheckedBy = oCheckedBy;
                    header.DocStatus = "O";
                    header.Canceled = "N";
                    header.RefNo = DateTime.Now.ToString("yyMMddHHmmssfff");//.ToString("yyMMddHHmmss.SSS");
                    header.SapUsername = EasySAPCredentialsModel.EmployeeName;
                    header.Remarks = $"Created by EasySAP | Data Transfer : {DateTime.Now} | Powered By : DIREC";
                    header.PreparedBy = EasySAPCredentialsModel.EmployeeCompleteName;
                    context.InventoryCounting.Add(header);

                    query = "SELECT DocEntry FROM DTOINC " +
                        $"WHERE ISNULL(WhsCode,'') = '{oWhsCode}' AND ISNULL(DocDate,'') = '{oDocDate}' AND ISNULL(SapCode,'') = '{oSapCode}' AND ISNULL(CountDate,'') = '{oCountDate}' AND RefNo = '{PublicStatic.DtRunID}'";
                    string DocEntry = "";
                    var doc = sql.Get(query);

                    if (doc != null)
                    {
                        if (doc.Rows.Count > 0)
                        {
                            var docEntry = doc.Rows.Cast<DataRow>().Select(x => x[0]).ToArray();

                            DocEntry = string.Join("','", docEntry);
                        }
                    }

                    query = $"SELECT * FROM DTINC1 WHERE HeaderId IN ('{DocEntry}') AND ISNULL(ItemCode,'') <> ''";

                    var rows = sql.Get(query);
                    int i = 0;
                    foreach (DataRow row in rows.Rows)
                    {
                        var lines = new InventoryCountingRow();
                        lines.HeaderId = ids;
                        lines.CompName = helper.ReadDataRow(row, "CompName", "");
                        lines.linenumber = i++;
                        lines.DocDate = oDocDate;
                        lines.ItemCode = helper.ReadDataRow(row, "ItemCode", "");
                        lines.ItemName = helper.ReadDataRow(row, "ItemName", "");
                        lines.Quantity = helper.ReadDataRow(row, "Quantity", 0);
                        lines.WhsCode = oWhsCode;
                        context.InventoryCountingRow.Add(lines);
                    }

                    context.SaveChanges();
                }
            }

            //if (headers != null)
            //{
            //    for (int hdrRow = 0; headers.Rows.Count > hdrRow; hdrRow++)
            //    {
            //        Dictionary<string, string> head = new Dictionary<string, string>();
            //        string DocEntry = "";
            //        string CardCode = "";
            //        string Address = "";
            //        var transferType = frmDT_UDF.TransactionType;
            //        var getDocEntryQuery = $"SELECT DocEntry FROM MarketingDocumentHeaders as Header " +
            //                                    $"WHERE Header.Session = '{PublicStatic.DtRunID}' AND Header.[User] = '{Environment.MachineName}' ";

            //        StaticHelper._MainForm.Invoke(new Action(() =>
            //            StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {hdrRow + 1} out of {headers.Rows.Count}", hdrRow + 1, headers.Rows.Count)
            //        ));

            //        for (int hdrCol = 0; headers.Columns.Count > hdrCol; hdrCol++)
            //        {
            //            var columnName = headers.Columns[hdrCol].ColumnName;
            //            var rowData = headers.Rows[hdrRow][columnName] == null ? "" : headers.Rows[hdrRow][columnName].ToString();
            //            if (columnName != "Id")
            //            {
            //                if (columnName == "CardCode")
            //                {
            //                    CardCode = rowData;
            //                    if (string.IsNullOrEmpty(transferType) == false)
            //                    {
            //                        Address = GetTransactionType(transferType, "Address", CardCode);
            //                        head.Add("U_AddID", Address);
            //                    }

            //                }

            //                if (columnName.ToUpper().Contains("DATE"))
            //                {
            //                    if (rowData != string.Empty || row != null)
            //                    {
            //                        if (rowData.Contains(".") || rowData.Contains("-") || rowData.Contains("/"))
            //                        {
            //                            var date = DateTime.TryParse(rowData, out DateTime outDate);
            //                            if (date)
            //                            {
            //                                rowData = Convert.ToDateTime(outDate).ToString("yyyyMMdd");
            //                            }
            //                        }
            //                    }
            //                }

            //                getDocEntryQuery += $" AND {(columnName.ToUpper().Contains("DATE") ? $"CONVERT(VARCHAR, CAST({columnName} AS DATE), 112) " : columnName)} = '{rowData}' ";
            //                if (!string.IsNullOrEmpty(rowData))
            //                {
            //                    head.Add(columnName, rowData);
            //                }


            //            }
            //        }

            //        var doc = sql.Get(getDocEntryQuery);

            //        if (doc != null)
            //        {
            //            if (doc.Rows.Count > 0)
            //            {
            //                var qwe = doc.Rows.Cast<DataRow>().Select(x => x[0]).ToArray();

            //                DocEntry = string.Join("','", qwe);
            //            }
            //        }

            //        //Removed sum due to conflict in quantity by Cedi 07292019
            //        var query2 = $"SELECT {row.Replace("Quantity", "SUM(CAST(ISNULL(Quantity,0) AS FLOAT)) [Quantity]")} from (" +
            //            $"SELECT distinct {row} " +
            //        "FROM MarketingDocumentLines as lines " +
            //        $"WHERE ISNULL(ItemCode,'') <> '' AND {(!row.Contains("Quantity") ? (row.Contains("PriceAfVAT") ? "PriceAfVAT IS NOT NULL and PriceAfVAT <> '0' AND " : "") : "")}{(row.Contains("Quantity") ? "Quantity IS NOT NULL and CAST(Quantity AS FLOAT) <> 0 AND " : "")} " +
            //        $"lines.Session = '{PublicStatic.DtRunID}' AND lines.[User] = '{Environment.MachineName}' AND lines.DocEntry in ('{DocEntry}') ) MT1 " +
            //        $"GROUP BY {row.Replace(",Quantity", "")} ORDER BY MT1.ItemCode";

            //        var rows = sql.Get(query2);

            //        var dictLines = new List<Dictionary<string, object>>();

            //        if (rows != null)
            //        {
            //            for (int lineRows = 0; rows.Rows.Count > lineRows; lineRows++)
            //            {
            //                var line = new Dictionary<string, object>();

            //                for (int lineCol = 0; rows.Columns.Count > lineCol; lineCol++)
            //                {
            //                    var columnName = rows.Columns[lineCol].ColumnName;
            //                    var rowData = rows.Rows[lineRows][columnName] == null ? "" : rows.Rows[lineRows][columnName].ToString();
            //                    if (columnName != "Id" || columnName != "DocEntry" || string.IsNullOrEmpty(rowData))
            //                    {
            //                        switch (columnName)
            //                        {
            //                            case "PriceAfVAT":
            //                                line.Add("PriceAfterVAT", rowData);
            //                                break;

            //                            case "ItemCode":
            //                                var item = controller.DeliveryItem(CardCode, rowData);
            //                                var whs = controller.DeliveryWhsCode(CardCode);

            //                                if (!line.Keys.Contains("WarehouseCode"))
            //                                {
            //                                    if (!string.IsNullOrEmpty(whs))
            //                                    {
            //                                        line.Add("WarehouseCode", whs);
            //                                    }
            //                                    else
            //                                    {
            //                                        whs = controller.SalesOrderWhsCode(frmDT_UDF.DocumentType);
            //                                        if (!string.IsNullOrEmpty(whs))
            //                                        {
            //                                            line.Add("WarehouseCode", whs);
            //                                        }
            //                                    }
            //                                }


            //                                var autoFields = controller.DeliveryInfo(item);

            //                                if (frmDT_UDF.DocumentDate != null)
            //                                {
            //                                    line.Add("ShipDate", Convert.ToDateTime(frmDT_UDF.DocumentDate).ToString("yyyyMMdd"));
            //                                }

            //                                if (autoFields.Count > 0)
            //                                {
            //                                    var projectCode = controller.GetProjectCode(CardCode);
            //                                    var cogs = autoFields[0];
            //                                    var itemDept = autoFields[1];
            //                                    var itemSubDept = autoFields[2];
            //                                    var color = autoFields[3];
            //                                    var size = autoFields[4];
            //                                    var style = autoFields[5];
            //                                    var sort = autoFields[6];

            //                                    if (!string.IsNullOrEmpty(projectCode) && !line.Keys.Contains("ProjectCode"))
            //                                    {
            //                                        line.Add("ProjectCode", controller.GetProjectCode(CardCode));
            //                                    }

            //                                    if (!line.Keys.Contains("COGSCostingCode2") && !string.IsNullOrEmpty(cogs) && objDocLines.Equals("StockTransferLines") == false)
            //                                    {
            //                                        line.Add("COGSCostingCode2", cogs);
            //                                    }

            //                                    if (!line.Keys.Contains("U_ItemDept") && !string.IsNullOrEmpty(itemDept))
            //                                    {
            //                                        line.Add("U_ItemDept", itemDept);
            //                                    }

            //                                    if (!line.Keys.Contains("U_ItemSubDept") && !string.IsNullOrEmpty(itemSubDept))
            //                                    {
            //                                        line.Add("U_ItemSubDept", itemSubDept);
            //                                    }

            //                                    if (!line.Keys.Contains("DistributionRule2") && !string.IsNullOrEmpty(cogs))
            //                                    {
            //                                        line.Add("DistributionRule2", cogs);
            //                                    }

            //                                    if (!line.Keys.Contains("U_Color") && !string.IsNullOrEmpty(color))
            //                                    {
            //                                        line.Add("U_Color", color);
            //                                    }

            //                                    if (!line.Keys.Contains("U_Size") && !string.IsNullOrEmpty(size))
            //                                    {
            //                                        line.Add("U_Size", size);
            //                                    }

            //                                    if (!line.Keys.Contains("U_Style") && !string.IsNullOrEmpty(style))
            //                                    {
            //                                        line.Add("U_Style", style);
            //                                    }

            //                                    if (!line.Keys.Contains("U_SortCode") && !string.IsNullOrEmpty(sort))
            //                                    {
            //                                        line.Add("U_SortCode", sort);
            //                                    }

            //                                    var qry = string.Format(helper.ReadDataRow(hana.Get(SP.DFLT_ItemCompany), 1, "", 0), CardCode, item);
            //                                    var company = helper.ReadDataRow(hana.Get(qry), 0, "", 0);
            //                                    if (!line.Keys.Contains("U_Company") && !string.IsNullOrEmpty(company))
            //                                    {
            //                                        line.Add("U_Company", company);
            //                                    }

            //                                    var sku = GetTransactionType(transferType, "SKU", CardCode, item);
            //                                    if (!line.Keys.Contains("U_SKU") && !string.IsNullOrEmpty(sku))
            //                                    {
            //                                        line.Add("U_SKU", sku);
            //                                    }

            //                                    if (!line.Keys.Contains("U_BrandName") && !string.IsNullOrEmpty(sort))
            //                                    {
            //                                        line.Add("U_BrandName", GetTransactionType(transferType, "Brand", item));
            //                                    }

            //                                    var unitPrice = GetTransactionType(transferType, "UnitPrice", CardCode, item);
            //                                    if (!line.Keys.Contains("UnitPrice") && !string.IsNullOrEmpty(unitPrice) && objDocLines.Equals("StockTransferLines"))
            //                                    {
            //                                        line.Add("UnitPrice", unitPrice);
            //                                    }

            //                                    if (!row.Contains("UoMEntry") || !row.Contains("UoMCode"))
            //                                    {
            //                                        line.Add("UoMEntry", GetUomID(rowData));
            //                                    }

            //                                    //sbJson.AppendLine($@"       ""U_BrandName"": ""{row.Cells["Brand"].Value.ToString()}"",");
            //                                }

            //                                line.Add(columnName, item);
            //                                break;

            //                            case "ProjectCode":

            //                                var projCode = controller.GetProjectCode(CardCode);

            //                                line.Add("ProjectCode", projCode);
            //                                break;

            //                            case "WarehouseCode":

            //                                var whsCode = controller.DeliveryWhsCode(CardCode);
            //                                if (!string.IsNullOrEmpty(whsCode))
            //                                {
            //                                    line.Add("WarehouseCode", whsCode);
            //                                }

            //                                break;

            //                            case "FromWarehouseCode":

            //                                var whsCode2 = controller.DeliveryWhsCode(CardCode);

            //                                line.Add("FromWarehouseCode", whsCode2);
            //                                break;

            //                            default:

            //                                if (columnName.ToUpper().Contains("DATE"))
            //                                {
            //                                    if (rowData != string.Empty || row != null)
            //                                    {
            //                                        if (rowData.Contains(".") || rowData.Contains("-") || rowData.Contains("/"))
            //                                        {
            //                                            rowData = Convert.ToDateTime(rowData).ToString("yyyyMMdd");
            //                                        }
            //                                    }
            //                                }

            //                                line.Add(columnName, rowData);
            //                                break;
            //                        }
            //                    }
            //                }
            //                dictLines.Add(line);
            //            }
            //        }

            //        // Addition Upload fields for header
            //        if (count > 0)
            //        {
            //            foreach (var x in DECLARE.udf.Where(x => x.ObjCode == objType))
            //            {
            //                if (!head.Keys.Contains((x.FieldCode)))
            //                {
            //                    var rowData = x.FieldValue;

            //                    if (x.FieldCode.ToUpper().Contains("DATE"))
            //                    {
            //                        if (rowData != string.Empty || row != null)
            //                        {
            //                            if (rowData.Contains(".") || rowData.Contains("-") || rowData.Contains("/"))
            //                            {
            //                                rowData = Convert.ToDateTime(rowData).ToString("yyyyMMdd");
            //                            }
            //                        }
            //                    }

            //                    head.Add(x.FieldCode, rowData);
            //                }
            //            }
            //        }

            //        if (module == "DeliveryNotes")
            //        {
            //            head.Add("U_DocType", frmDT_UDF.DocumentType);
            //            head.Add("Series", "6");

            //            if (!head.Keys.Contains("DocDate"))
            //            {
            //                head.Add("DocDate", Convert.ToDateTime(frmDT_UDF.PostingDate).ToString("yyyyMMdd"));
            //            }

            //            if (!head.Keys.Contains("DocDueDate"))
            //            {
            //                head.Add("DocDueDate", Convert.ToDateTime(frmDT_UDF.DocumentDate).ToString("yyyyMMdd"));
            //            }

            //            if (!head.Keys.Contains("TaxDate"))
            //            {
            //                head.Add("TaxDate", Convert.ToDateTime(frmDT_UDF.DocumentDate2).ToString("yyyyMMdd"));
            //            }
            //        }
            //        else if (module == "Orders")
            //        {
            //            head.Add("U_DocType", frmDT_UDF.DocumentType);
            //        }
            //        else if (module == "InventoryTransferRequests")
            //        {
            //            //head.Add("U_DocType", frmDT_UDF.DocumentType);


            //            if (!head.Keys.Contains("DocDate"))
            //            {
            //                head.Add("DocDate", Convert.ToDateTime(frmDT_UDF.DocDate).ToString("yyyyMMdd"));
            //            }

            //            if (!head.Keys.Contains("DueDate"))
            //            {
            //                head.Add("DueDate", Convert.ToDateTime(frmDT_UDF.DocDueDate).ToString("yyyyMMdd"));
            //            }

            //            if (!head.Keys.Contains("TaxDate"))
            //            {
            //                head.Add("TaxDate", Convert.ToDateTime(frmDT_UDF.TaxDate).ToString("yyyyMMdd"));
            //            }

            //            if (!head.Keys.Contains("U_TransferType"))
            //            {
            //                head.Add("U_TransferType", transferType);
            //            }

            //            if (!head.Keys.Contains("Series"))
            //            {
            //                head.Add("Series", GetTransactionType(transferType, "Series"));
            //            }

            //            if (!head.Keys.Contains("FromWarehouse"))
            //            {
            //                head.Add("FromWarehouse", GetTransactionType(transferType, "WhsFrom", CardCode, Address));
            //            }

            //            if (!head.Keys.Contains("ToWarehouse"))
            //            {
            //                head.Add("ToWarehouse", GetTransactionType(transferType, "WhsTo", CardCode, Address));
            //            }
            //        }

            //        if (head.Count != 0)
            //        {
            //            if (head.Keys.Contains("U_PostRem"))
            //            {
            //                head.Remove("U_PostRem");
            //            }

            //            SboCredentials sboCred = new SboCredentials();
            //            head.Add("U_PostRem", $"Created by EasySAP | Data Transfer : {sboCred.UserId} : {DateTime.Now} : {fileName} | Powered By : DIREC");

            //            if (!head.Keys.Contains("U_PrepBy"))
            //            {
            //                head.Add("U_PrepBy", EasySAPCredentialsModel.EmployeeCompleteName);
            //            }

            //            // Convert everthing above to json string
            //            var json = DataRepository.JsonBuilder(head, dictLines, objDocLines);

            //            string message = "";

            //            var xDocDate = head.Where(x => x.Key == "DocDate");

            //            var dateParam = xDocDate.Any() ? $"AND DocDate = '{xDocDate.FirstOrDefault().Value}'" : "";
            //            DataTable transExist = new DataTable();
            //            transExist = hana.Get($"SELECT 'true' FROM {tableName} WHERE CardCode = '{CardCode}' AND Comments LIKE 'Created by EasySAP | Data Transfer : %{fileName}% | Powered By : DIREC' {dateParam}");

            //            if (helper.DataTableExist(transExist) == false)
            //            {
            //                if (!serviceLayerAccess.ServiceLayer_Posting(json, "POST", module, "DocEntry", out message, out string val))
            //                {
            //                    IQueryable<MarketingDocumentHeaders> model = null;

            //                    if (string.IsNullOrEmpty(CardCode))
            //                    {
            //                        model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
            //                        x.User == Environment.MachineName);
            //                    }
            //                    else
            //                    {
            //                        model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
            //                     x.User == Environment.MachineName && x.CardCode == CardCode);
            //                    }


            //                    foreach (var qwe in model)
            //                    {
            //                        qwe.Uploaded = "No";
            //                        qwe.ErrorMessage = message;
            //                    }
            //                    //context.SaveChanges();
            //                }
            //                else
            //                {
            //                    var model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
            //                   x.User == Environment.MachineName && x.CardCode == CardCode);
            //                    foreach (var qwe in model)
            //                    {
            //                        qwe.Uploaded = "Yes";
            //                        qwe.ErrorMessage = message;
            //                    }

            //                }
            //            }
            //            else
            //            {
            //                var model = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID &&
            //               x.User == Environment.MachineName && x.CardCode == CardCode);
            //                foreach (var qwe in model)
            //                {
            //                    qwe.Uploaded = "No";
            //                    qwe.ErrorMessage = "Document already exist.";
            //                }

            //            }


            //            context.SaveChanges();
            //        }
            //    }
            //}


        }
        public void HeaderCardCode(MarketingDocumentHeaders hdr, string cardCode, string ColumnName, bool isSMPO = false)
        {
            switch (ColumnName)
            {
                case "CardCode":

                    var info = controller.BpCardProjectCode(cardCode, isSMPO);

                    hdr.CardCode = info[0]; //ValidateInput.String(DgvExcel[col, row].Value);
                    hdr.Project = info[1];
                    hdr.CardName = controller.CardName(hdr.CardCode);
                    break;
            }
        }
        #endregion

        #region Uploading of Marketing Document
        public void uploadMarketingDocument(List<MarketingDocumentHeaders> headers, List<MarketingDocumentLines> lines, string uploadType, MainForm frmMain)
        {
            var joins = headers.GroupJoin(lines, o => o.DocEntry, i => i.DocEntry, (o, i) => new
            {
                o,
                Items = i.ToList()
            }).ToList().GroupBy(x => new { x.o.CardCode, x.o.DocDate }).Select(grp => grp.ToList()).ToList();

            string cardCode = "";
            string cardCode2 = "";
            string itemcode1 = "";
            string itemcode2 = "";
            string gdocDate = "";

            //if (SAPAccess.ConnectToSAPDI())
            //{
            //    foreach (var join in joins.ToList())
            //    {
            //        bool isHeaderNull = true;

            //        // SELECT Document module to upload
            //        switch (uploadType)
            //        {
            //            case "Sales Quotation":
            //                SAPAccess.oDocument = (Documents)SAPAccess.oCompany.GetBusinessObject(BoObjectTypes.oQuotations);
            //                SAPAccess.oDocument.DocType = BoDocumentTypes.dDocument_Service;
            //                break;

            //            case "Sales Order":
            //                SAPAccess.oDocument = (Documents)SAPAccess.oCompany.GetBusinessObject(BoObjectTypes.oOrders);
            //                break;

            //            case "Delivery":
            //                SAPAccess.oDocument = (Documents)SAPAccess.oCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
            //                SAPAccess.oDocument.UserFields.Fields.Item("U_DocType").Value = frmDT_UDF.DocumentType;
            //                SAPAccess.oDocument.DocDate = Convert.ToDateTime(frmDT_UDF.PostingDate);

            //                var documentDate = frmDT_UDF.DocumentDate ?? frmDT_UDF.PostingDate;


            //                SAPAccess.oDocument.DocDueDate = Convert.ToDateTime(documentDate);
            //                SAPAccess.oDocument.TaxDate = Convert.ToDateTime(frmDT_UDF.DocumentDate2);
            //                SAPAccess.oDocument.Comments = "Uploaded through Data Transfer";
            //                break;
            //        }

            //        int count = DECLARE.udf.Where(x => x.ObjCode == objType).Count();
            //        if (count > 0)
            //        {
            //            foreach (var x in DECLARE.udf.Where(x => x.ObjCode == objType))
            //            {
            //                SAPAccess.oDocument.UserFields.Fields.Item(x.FieldCode).Value = x.FieldValue;
            //            }
            //        }

            //        string headerId = "";

            //        foreach (var header in join) // loop to get header then lines data
            //        {
            //            // header
            //            if (isHeaderNull) // to select only 1 header
            //            {
            //                headerId = header.o.DocEntry;

            //                if (header.o.CardCode != null) // CardCode
            //                {
            //                    cardCode = header.o.CardCode;
            //                    cardCode2 = uploadType != "Delivery" ? cardCode : controller.DeliveryBP(header.o.CardCode);
            //                    SAPAccess.oDocument.CardCode = cardCode2;
            //                }

            //                if (header.o.CardName != null) // CardName
            //                {
            //                    string cardName = header.o.CardName;
            //                    SAPAccess.oDocument.CardName = cardName;
            //                }

            //                if (header.o.DocDate != null) // DocDate
            //                {
            //                    DateTime docDate = Convert.ToDateTime(header.o.DocDate);
            //                    gdocDate = header.o.DocDate;
            //                    SAPAccess.oDocument.DocDate = docDate;
            //                }

            //                // uploadUDF(); // Upload UDF function
            //                uploadUDF(SAPAccess.oDocument);

            //                isHeaderNull = false;
            //            }

            //            // lines
            //            foreach (var item in header.Items) // Upload Items
            //            {
            //                if (item.ItemCode != null) // ItemCode
            //                {
            //                    string itemCode = item.ItemCode;

            //                    itemcode1 = itemCode;

            //                    if (uploadType == "Delivery" || uploadType == "Sales Qoutation")
            //                    {
            //                        itemCode = controller.DeliveryItem(cardCode2, itemCode);
            //                        SAPAccess.oDocument.Lines.WarehouseCode = controller.DeliveryWhsCode(cardCode2);

            //                        if (uploadType == "Delivery")
            //                        {
            //                            SAPAccess.oDocument.Lines.UserFields.Fields.Item("U_BrandName").Value = controller.BrandName(itemCode);
            //                        }
            //                    }

            //                    itemcode2 = itemCode;
            //                    SAPAccess.oDocument.Lines.ItemCode = itemCode;
            //                }

            //                if (item.Dscription != null) // item dscription
            //                {
            //                    string description = item.Dscription;
            //                    SAPAccess.oDocument.Lines.ItemDescription = description;
            //                }

            //                if (item.Quantity != null) // Quantity
            //                {
            //                    string quantity = item.Quantity;
            //                    SAPAccess.oDocument.Lines.Quantity = Convert.ToDouble(quantity);
            //                }

            //                if (item.Price != null) // price
            //                {
            //                    string price = item.Price;
            //                    SAPAccess.oDocument.Lines.Price = Convert.ToDouble(price);
            //                }

            //                if (item.LineTotal != null) // LineTotal
            //                {
            //                    string linetotal = item.LineTotal;
            //                    SAPAccess.oDocument.Lines.LineTotal = Convert.ToDouble(linetotal);
            //                }

            //                if (item.GTotal != null) // LineTotal
            //                {
            //                }

            //                if (item.UomEntry != null) // unit of meausures
            //                {
            //                    string uom = item.UomEntry;
            //                    SAPAccess.oDocument.Lines.UoMEntry = controller.UomEntry(uom);
            //                }

            //                if (item.PriceAfVAT != null) // price after vat
            //                {
            //                    string priceAfVat = item.PriceAfVAT;
            //                    SAPAccess.oDocument.Lines.PriceAfterVAT = Convert.ToDouble(item.PriceAfVAT);
            //                }

            //                SAPAccess.oDocument.Lines.VatGroup = frmDT_UDF.Vat;

            //                // add lines
            //                SAPAccess.oDocument.Lines.Add();
            //            }
            //        }

            //        // add document
            //        SAPAccess.lRetCode = SAPAccess.oDocument.Add();

            //        if (SAPAccess.lRetCode != 0) // if upload is failed
            //        {
            //            SAPAccess.oCompany.GetLastError(out int lErrCode, out string sErrMsg);
            //            SAPAccess.lErrCode = lErrCode;
            //            SAPAccess.sErrMsg = sErrMsg;
            //            //frmMain.NotiMsg($"Error Code: {SAPAccess.lErrCode}, Error Message: {SAPAccess.sErrMsg}", Color.Red);

            //            if (gdocDate == string.Empty)
            //            {
            //                var condition1 = headers.Where(x => x.CardCode == cardCode).ToList();
            //                foreach (var up1 in condition1)
            //                {
            //                    up1.Uploaded = "No";
            //                    up1.ErrorMessage = $"Error Code: {SAPAccess.lErrCode}, Error Message: {SAPAccess.sErrMsg}";
            //                }
            //            }
            //            else
            //            {
            //                var condition1 = headers.Where(x => x.CardCode == cardCode && x.DocDate == gdocDate).ToList();
            //                foreach (var up1 in condition1)
            //                {
            //                    up1.Uploaded = "No";
            //                    up1.ErrorMessage = $"Error Code: {SAPAccess.lErrCode}, Error Message: {SAPAccess.sErrMsg}";
            //                }
            //            }

            //        }
            //        else // if successs
            //        {
            //            //frmMain.NotiMsg("Document Successfully uploaded", Color.Green);

            //            if (gdocDate == string.Empty)
            //            {
            //                var condition1 = headers.Where(x => x.CardCode == cardCode).ToList();
            //                foreach (var up1 in condition1)
            //                {
            //                    up1.CardCode2 = cardCode2;
            //                }

            //                var condition2 = lines.Where(x => x.ItemCode == itemcode1).ToList();
            //                foreach (var up2 in condition2)
            //                {
            //                    up2.ItemCode2 = itemcode2;
            //                }
            //            }
            //            else
            //            {
            //                var condition1 = headers.Where(x => x.CardCode == cardCode && x.DocDate == gdocDate).ToList();
            //                foreach (var up1 in condition1)
            //                {
            //                    up1.CardCode2 = cardCode2;
            //                }

            //                var condition2 = lines.Where(x => x.ItemCode == itemcode1).ToList();
            //                foreach (var up2 in condition2)
            //                {
            //                    up2.ItemCode2 = itemcode2;
            //                }
            //            }
            //        }
            //    }
            //}
        }
        #endregion

        #region uploading of carton packing list part 2

        public void uploadCarton(List<MarketingDocumentHeaders> header, List<MarketingDocumentLines> lines, MainForm frmmain, out bool ret)
        {
            List<CartonManagement> cartonM = new List<CartonManagement>();
            List<CartonManagementRow> cartonMRow = new List<CartonManagementRow>();
            ctnList = null;

            var joins = header.GroupJoin(lines, o => o.DocEntry, i => i.DocEntry, (o, i) => new
            {
                o,
                Items = i.ToList()
            }).ToList().GroupBy(x => new { x.o.U_VendorCode, x.o.U_Ref1, x.o.U_DocRef, x.o.U_GroupCode, x.o.U_CartonNo }).Select(grp => grp.ToList()).ToList();

            int id = 0;

            foreach (var join in joins.ToList())
            {
                //Added 05 / 02 / 2019
                StaticHelper._MainForm.Invoke(new Action(() =>
                      StaticHelper._MainForm.Progress($"Please wait until all data are saved in addon database. {id + 1} out of {joins.ToList().Count}", id + 1, joins.ToList().Count)
                  ));
                bool isHeaderNull = false;

                foreach (var h in join.ToList())
                {
                    string cardCode = checkValue(h.o.U_VendorCode);

                    if (!isHeaderNull)
                    {
                        if (checkValue(h.o.U_VendorCode) != "")
                        {
                            cartonM.Add(new CartonManagement
                            {
                                Id = id,
                                CartonNo = checkValue(h.o.U_CartonNo),
                                VendorCode = checkValue(h.o.U_VendorCode),
                                VendorName = checkValue(h.o.U_VendorName),
                                ChainName = checkValue(h.o.U_ChainName),
                                DocRef = checkValue(h.o.U_DocRef),
                                Ref1 = checkValue(h.o.U_Ref1),
                                Ref2 = checkValue(h.o.U_Ref2),
                                TransactionType = frmDT_UDF.TransactionType,
                                LastWH = frmDT_UDF.LastWarehouse,
                                TargetWH = frmDT_UDF.TargetWarehouse,
                                Status = checkValue(h.o.U_Status) == "" ? "Draft" : checkValue(h.o.U_Status),
                                Remark = checkValue(h.o.Remark),
                                GroupCode = checkValue(h.o.U_GroupCode),
                            });
                        }

                        isHeaderNull = true;
                    }

                    foreach (var item in h.Items)
                    {
                        string itemNo = controller.FinditemCodeByCodeBars(checkValue(item.U_ItemNo));

                        //Added by Cedi on 05/27/2019, filters items being added in CartonRows
                        var cartonItemExists = cartonMRow.Any(x => x.Id == id && x.ItemNo == itemNo); // && x.Quantity == checkValue(item.U_Quantity)) && x.QtyPerInnerBox == checkValue(item.U_QuantityInnerBox));

                        if (cartonItemExists == false)
                        {
                            if (checkValue(item.U_Quantity) != "")
                            {
                                cartonMRow.Add(new CartonManagementRow
                                {
                                    Id = id,
                                    ItemNo = itemNo,
                                    Description = controller.ItemName(checkValue(itemNo)),
                                    Quantity = checkValue(item.U_Quantity),
                                    QtyPerInnerBox = checkValue(item.U_QuantityInnerBox),
                                });
                            }
                        }
                        else
                        {
                            if (checkValue(item.U_Quantity) != "")
                            {
                                var CartonList = cartonMRow.First(x => x.Id == id && x.ItemNo == itemNo);
                                int CartonQty = Convert.ToInt32(CartonList.Quantity.ToString());
                                CartonQty = CartonQty + Convert.ToInt32(checkValue(item.U_Quantity));

                                cartonMRow.Where(x => x.Id == id && x.ItemNo == itemNo).First().Quantity = CartonQty.ToString();
                            }
                        }

                    }
                }

                id++;
            }

            string sError = "";
            GotErrIds = null;

            Application.DoEvents();
            ret = cartonController.uploadCartonManagementSL((err) => sError = err, (cartonList) => ctnList = cartonList, cartonM, cartonMRow, (ids) => GotErrIds = ids);

            if (ret)
            {
                //////////// IF TRUE UPLOAD CARTON LIST 

                var groupItems = ctnList.GroupBy(x => new { x.Ref1, x.Ref2, x.VendorCode, x.DocRef }).Select(grp => grp.ToList()).ToList();

                List<CartonListRow> clRow = new List<CartonListRow>();

                foreach (var items in groupItems)
                {
                    context.cartonLists.Add(new CartonList
                    {
                        Remark = "Upload in Easy SAP Date: " + DateTime.Today.ToString()
                    });

                    items.ForEach(x =>
                    {
                        clRow.Add(new CartonListRow
                        {
                            DocEntry = x.DocEntry,
                            CartonNo = x.CartonNo,
                            DocRef = x.DocRef,
                            Ref1 = x.Ref1,
                            Ref2 = x.Ref2,
                            Remark = x.Remark,
                            VendorCode = x.VendorCode
                        });
                    });

                    if (cartonController.uploadCartonListSL((err) => StaticHelper._MainForm.ShowMessage(err), context.cartonLists, clRow))
                    {
                        //frmmain.NotiMsg("Upload Success", Color.Green);
                        context.cartonLists.Clear();
                        clRow.Clear();
                    }
                    else
                    {
                        //error encounter here
                        //break;
                    }
                }
            }

        }
        #endregion

        #region FUNCTIONS

        public string checkValue(string value)
        {
            return value == string.Empty || value == null ? "" : value;
        }
        #endregion

    }
}
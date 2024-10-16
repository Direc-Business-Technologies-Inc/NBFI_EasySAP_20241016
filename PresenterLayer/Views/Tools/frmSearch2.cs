using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zDeclare;
using MetroFramework.Forms;
using PresenterLayer.Views.Main;
using PresenterLayer;
using DomainLayer;
using DomainLayer.Models;
using PresenterLayer.Helper;
using DirecLayer._03_Repository;

namespace DirecLayer
{
    public partial class frmSearch2 : MetroForm, IfrmSearch2
    {
        MainForm frmMain;

        private frmSearch2 formsearch;
        //frmInventoryTransfer frmIT;
        DECLARE dc = new DECLARE();
        private string search;
        private string DocType;
        private string code;
        private string name;
        private string qty;
        private string rate;
        private string group;
        SAPHanaAccess hana { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        DataHelper helper { get; set; }
        public string oFormTitle { get; set; }
        public string oSearchMode { get { return search; } set { search = value; } }
        public string oCode { get { return code; } set { code = value; } }
        public string oName { get { return name; } set { name = value; } }
        public string oRate { get { return rate; } set { rate = value; } }
        public string oAvailable { get { return qty; } set { qty = value; } }
        public bool scanitems = false;

        private static int defaultColumn = 1, _rowIndex = 0, selectedrow = -1;

        public static string @Param1, @Param2, @Param3, @Param4, @Param5, _title, oFocus;
        public bool allowMultiple = false;
        public bool fromWhsLock = false;
        public bool fromSQL = false;

        int max_height = Screen.PrimaryScreen.Bounds.Height - 200;


        private Int64 PgSize = 1000;
        private Int64 CurrentPageIndex = 1;
        private Int64 TotalPage = 0;


        public frmSearch2()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            //this.frmIT = frmIT;
            helper = new DataHelper();
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {

            Text = oFormTitle;

            if (Text != null || Text.Length > 0)
            {
                lblTitle.Visible = false;
            }

            MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, max_height);

            defaultColumn = 1;

            DECLARE._multipleSelection.Clear();

            if (oFocus != "")
            {
                dgvSearchList.Focus();
            }
            else
            {
                txtSearch.Focus();
            }

            lblTitle.Text = _title;

            DataTable dt = null;
            string query = "";


            if (search == "AFIL")
            {
                query = "SELECT '1'[code], 'Customer' [name] UNION " +
                        "SELECT '2'[code], 'Channel'[name] UNION " +
                        "SELECT '3'[code], 'Area'[name] UNION " +
                        "SELECT '4'[code], 'Section'[name] UNION " +
                        "SELECT '5'[code], 'Brand'[name] ";
            }
            else if (search == "@OPKL - Automate Brand")
            {
                query = "SELECT DISTINCT Name, Code FROM [@OBND] ORDER BY Code";
            }
            else if (search == "G/L Account")
            {
                query = "SELECT AcctCode [Code], AcctName [Name] FROM OACT Where Postable = 'Y'";
            }
            else if (search == "UoM")
            {
                query = "SELECT UomEntry [#], UomCode [Code], UomName [Name] FROM OUOM";
            }
            else if (search == "Project")
            {
                query = "SELECT PrjCode [Code], PrjName [Name] FROM OPRJ WHERE Active = 'Y'";
            }
            else if (search == "UoM_Filter")
            {
                query = "Select T1.UomEntry [#], T2.UomCode [Code], T2.UomName [Name]  from UGP1 T1 inner join OUOM T2 on T1.UomEntry = T2.UomEntry " +
                        $" where T1.UgpEntry = (Select UgpEntry from OITM where ItemCode = '{Param1}')";
            }
            else if (search == "OITM - Get Size")
            {
                query = "select  U_Code [Code], U_Size [Size] from [@OSZS] where U_Size = $[OITM.U_ID007.0]";
            }
            else if (search == "@CM - Get target warehouse")
            {
                //query = "SELECT WhsCode [Warehouse Code], WhsName [Warehouse Name] from OWHS where Location != '4' AND Location != '1'";
                query = "SELECT WhsCode [Warehouse Code], WhsName [Warehouse Name] from OWHS";
            }
            else if (search == "Designers List")
            {
                query = "SELECT * FROM (SELECT DISTINCT(UPPER(a.firstName + ' ' + LEFT(ifnull(a.middleName, ''), 1) + '. ' + a.lastName))[Employee Name], b.name[Position] FROM OHEM a " +
                        "LEFT JOIN OHPS b on a.position = b.posID where b.name = 'Designer') Z " +
                        "WHERE Z.[Employee Name] is not null ORDER BY Z.[Employee Name] ASC";
            }
            else if (search == "Merch. Coor List")
            {
                query = "SELECT * FROM (SELECT DISTINCT(UPPER(a.firstName + ' ' + LEFT(ifnull(a.middleName, ''), 1) + '. ' + a.lastName))[Employee Name], b.name[Position] FROM OHEM a " +
                        "LEFT JOIN OHPS b on a.position = b.posID where b.name = 'Merch. Coordinator') Z " +
                        "WHERE Z.[Employee Name] is not null ORDER BY Z.[Employee Name] ASC";
            }
            else if (search == "Automate Budget Code")
            {
                query = "SELECT a.DocEntry [#], a.U_ActCode [Account Code], a.U_BudgetDescription [Budget Description] FROM [@BUDGET_H] a " +
                        $"WHERE a.U_Department = '{Param1}'";
            }
            if (search == "@CM - Get Transaction Type")
            {
                query = "SELECT * FROM (SELECT Name, 'Purchase' [Module] FROM [@TRANSACT_TYPE] " +
                        "UNION ALL " +
                        "SELECT Name, 'Transfer' [Module] FROM [@TRANSFER_TYPE] " +
                        "UNION ALL SELECT Name, 'Sales' [Module] FROM[@DOC_TYPE]) A " +
                        "ORDER BY A.Module, A.Name";
            }
            else if (search == "CM")
            {
                query = "SELECT DocNum ,CreateDate[Create Date], UpdateDate[Update Date], U_CartonNo[Carton Number], U_VendorCode[Vendor Code], " +
                        "U_VendorName[Vendor Name], U_ChainName[Chain Name], U_DocRef[Document Reference], U_Ref1[Ref], U_Ref2[Reference 2], U_Status[Status], " +
                        "U_DateChecked[Date Checked] FROM [@CM_HEADER] Order By DocNum";
            }
            else if (search == "PCAT")
            {
                query = "SELECT '1'[code], 'Markdown'[name] UNION " +
                        "SELECT '2'[code], 'Regular'[name] ";
            }
            else if (search == "barcodes")
            {
                query = $"SELECT BcdCode [Bar Code], BcdName [Free Text] from OBCD where ItemCode = '{Param1}'";
            }
            else if (search == "EmployeeList")
            {
                //query = "select (T0.lastName + '  ' + T0.firstName) [Employee Name] , T1.name [Position] from OHEM T0 left join OHPS T1 on T0.position = T1.posID Order by T1.name";

                query = "Select Distinct b.name [Signatories] from OHEM a inner join OHPS b on a.position = b.posID inner join HEM6 c on a.empID = c.empID " +

                        "inner join OHTY d on d.typeID = c.roleID and d.name = 'Approver'";
            }
            else if (search == "EmployeeList *")
            {
                query = "select * from (SELECT distinct (UPPER(a.firstName + ' ' + LEFT(a.middleName,1) + '. ' + a.lastName)) [Employee Name], b.name [Position]  FROM OHEM a " +

                        "left join OHPS b on a.position = b.posID) Z " +

                        "where z.[Employee Name] is not null order by [Employee Name] asc";
            }
            else if (search == "dept")
            {
                query = "Select PrcCode [Center Code], PrcName [Center Name] from OPRC where DimCode = 1 and Locked = 'N'";
            }
            else if (search == "dept*")
            {
                query = "SELECT a.PrcName FROM OPRC a WHERE a.DimCode = 1 ORDER BY a.PrcName asc";
            }
            else if (search == "findPackinglist")
            {
                query = "SELECT T0.DocEntry [Doc Entry], T0.DocNum [Doc Num], T0.U_Type [Type], T0.U_SIDRNo [SI/DR No],U_BranchCode as BranchCode,U_CardCode as CardCode,U_CardName AS CardName,U_ShipTo as ShipTo  FROM [@OPKL] T0 Order By DocEntry DESC";
            }
            else if (search == "oitm*")
            {
                query = "SELECT ItemCode [Item Code], ItemName [Item Name], OnHand [In Stock] FROM OITM";
            }
            else if (search == "cartonList")
            {
                query = "SELECT DocNum, CreateDate, UpdateDate, Remark FROM [@CL_HEADER] Order By DocNum";
            }
            else if (search == "chain")
            {
                query = "SELECT GroupCode [Group Code], GroupName [Group Name]  FROM OCRG";
            }
            else if (search == "SearchDocPackinglist")
            {
                query = "Select DocNum, CreateDate, UpdateDate, Remark, U_SIDRNo [SI/DR No], U_PONo [PO No], U_Department [Department], U_CardName [CardName], " +
                        "U_CardCode [CardCode], U_ShipTo [ShipTo], U_Weight [Weight], U_Box [Carton], U_TotalBox [Total Carton], U_Length [Length], U_Width [Width], U_Height [Height], " +
                        "U_Type [Type], U_BranchCode [BranchCode], U_Date [Date] FROM [@OPKL] Order by DocNum";
            }
            else if (search == "SearchDocAsnList")
            {
                query = "Select DocNum, CreateDate, UpdateDate, U_NoofBox [Total No of Box], Remark, U_DRNo from [@OPKC] Order by DocNum";
            }
            else if (search == "CopyFromPOToGrPO")
            {
                query = $"SELECT DocEntry [#], DocNum [Document No.], DocDate [Date], CardName [Vendor], Comments [Remarks], DocDueDate [Due Date] FROM OPOR where CardCode = '{@Param1}' AND DocStatus = 'O' Order By DocNum";
            }
            else if (search == "CopyFromPurchQout")
            {
                query = $"SELECT DocEntry [#], DocType[Document Type], DocNum[Document Number], CardCode[BP Code], CardName[Bp Name], DocDate[Posting Date], ReqDate[Delivery Date], TaxDate[Document Date] FROM OPQT WHERE DocStatus = 'O' { (string.IsNullOrEmpty(@Param1) ? "" : $" AND CardCode = '{@Param1}' ") } Order By DocNum";
            }
            else if (search == "CopyFromPurchRequest")
            {
                query = $"SELECT DocEntry [#], DocNum [PRQ No.], DocDate [Posting Date], ReqName [Requester], Comments [Remarks], ReqDate[Delivery Date], DocDueDate [Valid Until Date] FROM OPRQ WHERE DocStatus = 'O' Order By DocEntry";
            }
            else if (search == "CopyFromResInvoice")
            {
                query = $"SELECT DocNum [#], DocDate [Date], CardName [Vendor], Comments [Remarks], DocDueDate [Due Date] FROM OPCH where CardCode = '{@Param1}' AND DocStatus = 'O' AND isIns = 'Y' Order By DocNum";
            }
            else if (search == "ASNdocument")
            {
                query = "SELECT 'DR'[Doc Series], a.U_DRNo [DR No], a.CardCode [CarCode], a.CardName [CarName], a.DocNum [Doc Num], a.DocDate [Doc Date] " +
                        "FROM OINV a " +
                        "WHERE a.U_DocType = 'Outright Order' AND U_DRNo != '' " +
                        "UNION ALL " +
                        "SELECT 'CST'[Doc Series], a.U_DRNo [DR No], a.CardCode [CarCode], a.CardName [CarName], a.DocNum [Doc Num], a.DocDate [Doc Date] " +
                        "FROM OWTR a " +
                        "WHERE Series = '21' AND U_DRNo != '' ";
            }
            else if (search == "SearchArInvoiceOutrightWitOuthBP")
            {
                query = $"SELECT DocNum [#], U_SINo [SI No.], DocDate [Date], CardName [Vendor], Comments [Remarks], DocDueDate [Due Date] FROM  OINV where Series = '98' Order By DocNum";
            }
            else if (search == "SearchArInvoiceOutright")
            {
                query = $"SELECT DocNum [#], U_SINo [SI No.], DocDate [Date], CardName [Vendor], Comments [Remarks], DocDueDate [Due Date] FROM  OINV where CardCode = '{@Param1}' AND Series = '98' Order By DocNum";
            }
            else if (search == "SearchDrConcession")
            {
                query = $"SELECT DocNum [#], Series, U_DRNo [DR No.], DocDate [Date], CardName [Vendor], Comments [Remarks], DocDueDate [Due Date] FROM  OWTR where CardCode = '{@Param1}' AND Series IN (SELECT Series FROM NNM1 WHERE ObjectCode = ObjType AND SeriesName LIKE '%CST%') Order By DocNum";
            }
            else if (search == "signators")
            {
                query = "Select Distinct b.name [Signatories] from OHEM a inner join OHPS b on a.position = b.posID";
            }
            else if (search == "@BRAND")
            {
                query = "SELECT Distinct T0.Code [Brand Codes], T0.Name [Brands] FROM [@OBND] T0 Order by Name";
            }
            else if (search == "@SECTION*")
            {
                query = "SELECT Distinct T0.Code , T0.Name  FROM [@SECTION] T0 ";
            }
            else if (search == "AREA")
            {
                query = "SELECT Distinct T0.DfTcnician , T1.firstName , T1.lastName FROM OCRD T0 " +
                        "LEFT JOIN OHEM T1 ON T0.DfTcnician = T1.empID " +
                        "WHERE T0.DfTcnician is not null";
            }
            else if (search == "OCRD")
            {
                query = "Select A.CardCode [BP Code],A.CardName [BP Name],(SELECT Z.GroupName FROM OCRG Z Where Z.GroupCode = A.GroupCode) [Group], Currency [BP Currency] from OCRD A Where A.CardType = '" + @Param1.Replace("'", "''") + "'  And A.frozenFor = 'N' Order by A.CardCode";
            }
            else if (search == "OCRDDLVRY")
            {
                query = "Select A.CardCode [BP Code],A.CardName [BP Name],(SELECT Z.GroupName FROM OCRG Z Where Z.GroupCode = A.GroupCode) [Chain] from OCRD A Where A.CardType = '" + @Param1.Replace("'", "''") + "'  And A.frozenFor = 'N' Order by A.CardCode";
            }
            else if (search == "OCRD*")
            {
                query = "Select A.CardCode,A.CardName,(SELECT Z.GroupName FROM OCRG Z Where Z.GroupCode = A.GroupCode) [Group] from OCRD A Where A.frozenFor = 'N' Order by A.CardCode";
            }
            else if (search == "VNDR")
            {
                query = "Select A.CardCode [BP Code],A.CardName [BP Name],(SELECT Z.GroupName FROM OCRG Z Where Z.GroupCode = A.GroupCode) [BP Type] from OCRD A Where A.CardType = '" + @Param1.Replace("'", "''") + "'  And A.frozenFor = 'N' Order by A.CardCode";
            }
            else if (search == "OCRD BP")
            {
                query = "Select A.CardCode [BP Code], A.CardName [BP Name], Currency [Bp Currency], (SELECT Z.GroupName FROM OCRG Z Where Z.GroupCode = A.GroupCode) [Group Name] from OCRD A Where A.frozenFor = 'N' AND A.CardType = 'S' Order by A.CardCode";
            }
            else if (search == "OCRD SPCS")
            {
                query = "Select A.CardCode [BP Code],A.CardName [BP Name], Currency [Bp Currency], (SELECT Z.GroupName FROM OCRG Z Where Z.GroupCode = A.GroupCode) [Group Name] from OCRD A Where A.frozenFor = 'N' Order by A.CardCode";
            }
            else if (search == "OCRD**")
            {
                query = "Select A.CardCode, A.CardName, Z.GroupName [Group] FROM OCRD A INNER JOIN OCRG Z ON Z.GroupCode = A.GroupCode WHERE Z.GroupType = 'S' AND A.frozenFor = 'N' Order by A.CardCode";
            }
            else if (search == "OCRD***")
            {
                query = "Select A.CardCode [BP Code], A.CardName [BP Name], Z.GroupName [Chain] FROM OCRD A INNER JOIN OCRG Z ON Z.GroupCode = A.GroupCode WHERE A.frozenFor = 'N' And CardType = 'C' Order by A.CardCode";
            }
            else if (search == "OITM")
            {
                query = "SELECT  ItemCode" +
                                      ",ItemName" +
                                      ",FrgnName" +
                                      ",CodeBars" +
                               " FROM OITM Where frozenFor = 'N'";
            }
            else if (search == "OWTQ")
            {
                query = "SELECT DocEntry,DocNum,CardCode" +
                                      ",CardName" +
                                      ",DocDate" +
                                      ",DocDueDate" +
                                      ",U_PONo [PO #]" +
                               " FROM OWTQ Where DocStatus = 'O' And CardCode = '" + @Param1.Replace("'", "''") + "' Order By DocEntry Desc";
            }
            else if (search == "OWTQ_NOBP")
            {
                query = "SELECT DocEntry,DocNum,CardCode" +
                                      ",CardName" +
                                      ",DocDate" +
                                      ",DocDueDate" +
                                      ",U_PONo [PO #]" +
                               " FROM OWTQ Where DocStatus = 'O' Order By DocEntry Desc";
            }
            else if (search == "OPOR_BP")
            {
                query = "SELECT DocEntry,DocNum,[U_PONo] [PO No.],CardCode" +
                                      ",CardName" +
                                      ",DocDate" +
                                      ",DocDueDate" +
                                      ",TaxDate" +
                               " FROM OPOR Where CardCode = '" + @Param1.Replace("'", "''") + "' and DocStatus = 'O' Order By DocNum DESC";
            }
            else if (search == "OPCH_BP")
            {
                query = "SELECT DocEntry,DocNum,[U_PONo] [PO No.],CardCode" +
                                      ",CardName" +
                                      ",DocDate" +
                                      ",DocDueDate" +
                                      ",TaxDate" +
                               " FROM OPCH Where CardCode = '" + @Param1.Replace("'", "''") + "' and DocStatus = 'O'  and isIns = 'Y' and InvntSttus = 'O' Order By DocNum DESC";
            }
            else if (search == "ORDR_BP")
            {
                query = "SELECT DocEntry,DocNum,CardCode [BP Code]" +
                                      ",CardName [BP Name]" +
                                      ",DocDate [Posting Date]" +
                                      ",DocDueDate [Delivery/Pullout Date]" +
                                      ",TaxDate [Document Date]" +
                               " FROM ORDR Where CardCode = '" + @Param1.Replace("'", "''") + "'  and DocStatus = 'O' Order By DocNum DESC";
            }
            else if (search == "OWHS")
            {
                query = "SELECT WhsCode [Warehouse Code],WhsName [Warehouse Name] FROM OWHS";
            }
            else if (search == "OITW")
            {
                query = "SELECT A.WhsCode [Warehouse Code],(SELECT Z.WhsName FROM OWHS Z Where Z.WhsCode = A.WhsCode) [Warehouse Name],A.OnHand [InStock],(A.OnHand - A.IsCommited) [Available] FROM OITW A Where A.ItemCode = '" + @Param1.Replace("'", "''") + "' And A.OnHand > 0 and (A.OnHand - A.IsCommited) >= 0 Order by A.OnHand DESC";
            }
            else if (search == "OITW2")
            {
                query = "SELECT A.WhsCode [Warehouse Code],(SELECT Z.WhsName FROM OWHS Z Where Z.WhsCode = A.WhsCode) [Warehouse Name],A.OnHand [InStock],(A.OnHand - A.IsCommited) [Available] FROM OITW A LEFT JOIN OWHS B ON A.WhsCode = B.WhsCode Where A.ItemCode = '" + @Param1.Replace("'", "''") + "' And B.GlblLocNum ='Y' and (A.OnHand - A.IsCommited) >= 0 Order by A.WhsCode";
            }
            else if (search == "OITW3")
            {
                query = "SELECT DISTINCT A.WhsCode [Warehouse Code],(SELECT Z.WhsName FROM OWHS Z Where Z.WhsCode = A.WhsCode) [Warehouse Name] FROM OITW A LEFT JOIN OWHS B ON A.WhsCode = B.WhsCode Where B.GlblLocNum ='Y' Order by A.WhsCode";
            }
            else if (search == "@PRSTYLE")
            {
                query = "SELECT Distinct U_Code, U_Style [Name] FROM [@OSTL] Order By Name";
            }
            else if (search == "@PRCOLOR2")
            {
                query = "SELECT Code, Name FROM [@OCLC] Order By Name";
            }
            else if (search == "@PRCOLOR")
            {
                query = "SELECT DISTINCT A.U_Color,(SELECT Name FROM [@PRCOLOR] Z Where Z.Code = A.U_Color) [Name] FROM OITM A Where A.U_StyleCode = '" + @Param1.Replace("'", "''") + "'";
            }
            else if (search == "@Section")
            {
                query = "SELECT DISTINCT Replace(U_Section,'''','''') Section FROM OITM WHERE U_StyleCode = '" + @Param1.Replace("'", "''") + "' and U_Color = '" + @Param2.Replace("'", "''") + "'";
            }
            else if (search == "@Section_P")
            {
                query = "SELECT DISTINCT Replace(U_Section,'''','''') Section FROM OITM WHERE U_StyleCode = '" + @Param1.Replace("'", "''") + "' and U_Color = '" + @Param2.Replace("'", "''") + "' and PrchseItem = 'Y'";
            }
            else if (search == "@Section_S")
            {
                query = "SELECT DISTINCT Replace(U_Section,'''','''') Section FROM OITM WHERE U_StyleCode = '" + @Param1.Replace("'", "''") + "' and U_Color = '" + @Param2.Replace("'", "''") + "' and SellItem = 'Y'";
            }
            else if (search == "@Section_I")
            {
                query = "SELECT DISTINCT Replace(U_Section,'''','''') Section FROM OITM WHERE U_StyleCode = '" + @Param1.Replace("'", "''") + "' and U_Color = '" + @Param2.Replace("'", "''") + "' and InvntItem = 'Y'";
            }
            else if (search == "@Section_I2")
            {
                query = "SELECT Code, Name FROM [@OSZC] Order By Name";
            }
            else if (search == "OVTG")
            {
                query = " select Code,Rate,Name from OVTG Where Category = '" + @Param1.Replace("'", "''") + "' and Inactive = 'N'";
            }
            else if (search == "OVTG_VatEx")
            {
                query = " select Code,Name,Rate from OVTG where Inactive = 'N' and Rate = 0";
            }
            else if (search == "CRD1")
            {
                //query = " select Address as AddressCode,Street,Zipcode,City from CRD1 Where CardCode = '" + @Param1.Replace("'", "''") + "' and Adrestype = '" + @Param2.Replace("'", "''") + "'";
                query = "SELECT Address [AddressCode],(ISNULL(Street,'') + ' ' + ISNULL(Block,'') + ' ' + ISNULL(City,'') + ' ' + ISNULL(Country,'')) [Address] from CRD1 Where CardCode = '" + @Param1.Replace("'", "''") + "' and AdresType = '" + @Param2.Replace("'", "''") + "' and U_Whs is not null";
            }
            else if (search == "@ItemType")
            {
                query = "SELECT DISTINCT U_ItemType FROM [@SECCAT]";
            }
            else if (search == "@AgeGroup")
            {
                query = "SELECT DISTINCT U_AgeGroup FROM [@SECCAT] Where U_ItemType = '" + @Param1.Replace("'", "''") + "'";
            }
            else if (search == "OPLN")
            {
                query = "SELECT DISTINCT ListNum,ListName FROM OPLN";
            }
            else if (search == "OCTG")
            {
                query = "select GroupNum,PymntGroup  from OCTG";
            }
            else if (search == "OPKL_I")
            {
                //query = "SELECT DISTINCT A.AbsEntry [Picklist no.]" +
                //        ", B.OrderEntry, A.CardCode, A.Name, A.PickDate, A.Status, Replace(A.Remarks,'''','') [Remarks]  " +
                //        "FROM OPKL A LEFT JOIN PKL1 B ON A.AbsEntry = B.AbsEntry " +
                //        "Where A.Canceled = 'N' and Status <> 'C' and B.BaseObject = '1250000001' Order By A.AbsEntry Desc";

                query = "SELECT " +
                        " T2.AbsEntry [Picklist no.]" +
                        " , T1.CardCode [CardCode] " +
                        " , T1.CardName [Card Name] " +
                        " , T1.DocDate [Doc Date] " +
                        " , T2.OrderEntry" +
                        " , T1.DocEntry " +
                        " , T1.DocNum " +
                        " , 'ITR - Pick List'[FromDoc] " +
                        " , T1.DocType " +
                        " , SUM(T2.PickQtty) [Total Pick Qty.] " +
                        " FROM OWTQ T1 INNER JOIN PKL1 T2 on T1.DocEntry = T2.OrderEntry and T1.ObjType = T2.BaseObject and T2.PickStatus != 'C'" +
                        //" WHERE T1.Printed = 'N' " +  //On comment due to pick list not visible in selection list 081519
                        " Group By T1.DocEntry, T1.DocNum, T2.AbsEntry, T1.DocType, T1.CardCode, T1.CardName, T1.DocDate, T1.ObjType, T2.OrderEntry Order By T2.AbsEntry desc";

            }
            else if (search == "OPKL_S")
            {
                //query = "SELECT DISTINCT A.AbsEntry, A.Name, A.PickDate, A.Status, Replace(A.Remarks,'''','') [Remarks]  FROM OPKL A LEFT JOIN PKL1 B ON A.AbsEntry = B.AbsEntry Where A.Canceled = 'N' and Status <> 'C' and B.BaseObject = '17' Order By A.AbsEntry Desc";
                query = "SELECT " +
                        " T2.AbsEntry [Picklist no.]" +
                        " , T1.CardCode [CardCode] " +
                        " , T1.CardName [Card Name] " +
                        " , T1.DocDate [Doc Date] " +
                        " , T2.OrderEntry" +
                        " , T1.DocEntry " +
                        " , T1.DocNum " +
                        " , 'SO - Pick List'[FromDoc] " +
                        " , T1.DocType " +
                        " , SUM(T2.PickQtty) [Total Pick Qty.] " +
                        " FROM ORDR T1 INNER JOIN PKL1 T2 on T1.DocEntry = T2.OrderEntry and T1.ObjType = T2.BaseObject and T2.PickStatus != 'C'" +
                        " WHERE T1.Printed = 'N' " +
                        " Group By T1.DocEntry, T1.DocNum, T2.AbsEntry, T1.DocType, T1.CardCode, T1.CardName, T1.DocDate, T1.ObjType, T2.OrderEntry Order By T2.AbsEntry desc";
            }
            else if (search == "OSLP")
            {
                query = "SELECT SlpCode,SlpName FROM OSLP";
            }
            else if (search == "OSLP *")
            {
                query = "SELECT SlpCode [Code],SlpName [Sales Employee Name], Memo [Remarks] FROM OSLP Order By SlpCode";
            }
            else if (search == "OINV")
            {
                query = "SELECT A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.DocTotal [Total],A.U_SINo [SI No.] " +
                        ",(SELECT Sum(Z.Quantity) From INV1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OINV A Order By A.DocEntry DESC";
            }
            else if (search == "@ORDR")
            {
                query = "SELECT T0.TaxCode FROM RDR1 T0";
            }
            else if (search == "ORDR")
            {
                query = "SELECT A.DocEntry,A.DocStatus[Status], A.DocNum [Doc No.]" +
                    " ,A.DocDate [Doc Date],A.CardCode [BP Code],A.CardName [BP Name]" +
                    " ,A.DocTotal [Total] " +
                    " ,(SELECT Sum(Z.Quantity) From RDR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM ORDR A " +
                    " UNION ALL" +
                    " SELECT T1.DocEntry, CASE WHEN Status = 'Y' THEN 'D-Approved' WHEN Status = 'W' THEN 'D-Pending' END [Status], T1.DocNum [Doc No.] " +
                    " , T1.DocDate[Doc Date], T1.CardCode[BP Code], T1.CardName[BP Name] " +
                    " , T1.DocTotal[Total] " +
                    " , (SELECT Sum(Z.Quantity) From DRF1 Z Where Z.DocEntry = T1.DocEntry) [Total Quantity] " +
                    " FROM OWDD T0 " +
                    " INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry and T1.ObjType = '17' " +
                    $" WHERE T0.UserSign = (SELECT USERID FROM OUSR WHERE USER_CODE = '{SboCred.UserID}') AND CANCELED = 'N' " +
                    " ORDER BY DocEntry DESC";
            }
            else if (search == "ORDR_US")
            {
                //CalculateTotalPages();
                query = "SELECT A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.DocTotal [Total] " +
                    ",(SELECT Sum(Z.Quantity) From RDR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM ORDR A Where A.Series = 73 And A.DocStatus = 'O' Order By A.DocEntry DESC";
                //query = "SELECT TOP " + PgSize + " A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.DocTotal [Total] " +
                //    ",(SELECT Sum(Z.Quantity) From RDR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM ORDR A Where A.Series = 73 And A.DocStatus = 'O' Order By A.DocEntry DESC";

                //btnFirstPage.Visible = true; btnPrev.Visible = true; btnNext.Visible = true; btnLastPage.Visible = true;
            }
            else if (search == "OPDN")
            {
                CalculateTotalPages("OPDN");
                query = "SELECT TOP " + PgSize + " A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],(Case when A.DocStatus = 'O' then 'Open' else 'Closed' end) [Status],A.DocTotal [Total] " +
                   ",(SELECT Sum(Z.Quantity) From PDN1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OPDN A Order By DocNum";
                //query = "SELECT A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],(Case when A.DocStatus = 'O' then 'Open' else 'Closed' end) [Status],A.DocTotal [Total] " +
                //    ",(SELECT Sum(Z.Quantity) From PDN1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OPDN A Order By A.DocEntry DESC";

                btnFirstPage.Visible = true; btnPrev.Visible = true; btnNext.Visible = true; btnLastPage.Visible = true;
            }

            else if (search == "OPOR")
            {
                query = "SELECT A.DocEntry, A.DocStatus [Status], A.DocNum [Doc No.], A.CardCode [BP Code], A.CardName [BP Name], A.DocDate [Posting Date], " +

                        "A.DocDueDate [Delivery Date], A.DocTotal [Total], (SELECT Sum(Z.Quantity) From POR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OPOR A" +

                        " UNION ALL " +

                        "SELECT T1.DocEntry, CASE WHEN Status = 'Y' THEN 'D-A' WHEN Status = 'W' THEN 'D-P'  END  [Status], T1.DocNum [Doc No.], " +

                        "T1.CardCode[BP Code], T1.CardName[BP Name], T1.DocDate [Posting Date], T1.DocDueDate [Delivery Date], T1.DocTotal[Total], " +

                        "(SELECT Sum(Z.Quantity) From DRF1 Z Where Z.DocEntry = T1.DocEntry) [Total Quantity] " +

                        "FROM OWDD T0 " +

                        "INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry " +

                        $"WHERE T0.UserSign = (SELECT USERID FROM OUSR WHERE USER_CODE = '{SboCred.UserID}') AND CANCELED = 'N'" +

                        "ORDER BY DocEntry DESC";
            }
            else if (search == "OWTR")
            {
                CalculateTotalPages("OWTR");
                //query = "SELECT A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                //    ",(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTR A Order By A.DocEntry Desc";
                query = "SELECT TOP " + PgSize + " A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                   ",(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTR A Order By A.DocEntry Desc";
                btnFirstPage.Visible = true; btnPrev.Visible = true; btnNext.Visible = true; btnLastPage.Visible = true;
            }
            else if (search == "OWTQ_FIND")
            {
                CalculateTotalPages("OWTQ");
                query = "SELECT TOP " + PgSize + " A.DocEntry,(SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                    ",(SELECT Sum(Z.Quantity) From WTQ1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTQ A Order By A.DocEntry Desc";
                //query = "SELECT TOP " + PgSize.ToString() + " A.U_DRNo [DR No.],(SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_PONo [PO No.] " +
                //",(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTR A Where A.U_DRNo is not null AND A.U_DRNo <> '-' Order By A.DocNum Desc";
                btnFirstPage.Visible = true; btnPrev.Visible = true; btnNext.Visible = true; btnLastPage.Visible = true;

            }
            else if (search == "OINC")
            {
                query = "SELECT DocEntry,DocNum [Doc No.],CountDate [Count Date],Time [Time],Remarks FROM OINC Where Status = 'O'";
            }
            else if (search == "OCRN")
            {
                query = "SELECT CurrCode [Currency Code],CurrName [Currency Name] FROM OCRN";
            }
            else if (search == "OPKL")
            {
                //version 1.4.1.25
                CalculateTotalPages("OPKL");

                //query = "SELECT TOP " + PgSize + " A.U_DRNo [DR No.],A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_PONo [PO No.] " +
                //    ",(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTR A Where A.U_DRNo is not null AND A.U_DRNo <> '' AND A.Series = '21' Order By A.U_DRNo Desc ";

                //query = "SELECT TOP " + PgSize + " A.U_DRNo[DR No.],A.DocEntry, 'CST'[Series], A.DocNum[Doc No.],A.CardCode[BP Code],A.CardName[BP Name],A.DocDate[Doc Date],A.DocStatus[Status],A.U_SINo[SI No.],A.U_PONo[PO No.], " +
                //        "(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] " +
                //        "FROM OWTR A Where A.U_DRNo is not null AND A.U_DRNo<> ''  AND A.Series = '21' " +
                //        "UNION ALL " +
                //        "SELECT TOP " + PgSize + " A.U_DRNo[DR No.], A.DocEntry,  'DR'[Series], A.DocNum[Doc No.], A.CardCode[BP Code], A.CardName[BP Name], A.DocDate[Doc Date], A.DocStatus[Status], A.U_SINo[SI No.], A.U_PONo[PO No.], " +
                //        "(SELECT Sum(Z.Quantity) From INV1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] " +
                //        "FROM OINV A Where A.U_DRNo is not null AND A.U_DRNo<> ''  AND A.U_DocType= 'Outright Order'";

                query = "SELECT   A.DocNum[Doc No.] ,A.DocEntry, 'CST'[Series], A.U_DRNo [CST/AR No.],A.CardCode[BP Code],A.CardName[BP Name],A.DocDate[Doc Date],A.DocStatus[Status],A.U_SINo[SI No.],A.U_PONo[PO No.], " +
                        "(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] " +
                        "FROM OWTR A Where A.Series = '21' OR A.Series = '127' OR A.Series = '126'" +

                        "UNION ALL " +

                        "SELECT  A.DocNum[Doc No.], A.DocEntry,  'DR'[Series], A.U_DRNo [CST/AR No.], A.CardCode[BP Code], A.CardName[BP Name], A.DocDate[Doc Date], A.DocStatus[Status], A.U_SINo[SI No.], A.U_PONo[PO No.], " +
                        "(SELECT Sum(Z.Quantity) From INV1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] " +
                        "FROM OINV A Where A.U_DocType= 'Outright Order' order by Series,DocEntry";

                btnFirstPage.Visible = true; btnPrev.Visible = true; btnNext.Visible = true; btnLastPage.Visible = true;
            }
            else if (search == "OPKC")
            {
                query = "SELECT A.DocEntry,A.DocNum [Doc No.],A.U_DRNo [DR No.],A.U_NoofBox [Total No. of Box],Remark [Remarks] FROM [@OPKC] A Order By A.DocNum Desc";
            }
            else if (search == "OWHL")
            {
                query = "SELECT [DocEntry],[DocDate],(CASE WHEN [DocStatus] = 'O' AND Canceled = 'N' THEN 'Open' WHEN [DocStatus] = 'C' AND Canceled = 'N' THEN 'Closed' WHEN Canceled = 'Y' THEN 'Cancelled' END) [DocStatus],[DateFrom],[DateTo],[Remarks] FROM OWHL";
            }
            else if (search == "OCRG1")
            {
                query = " SELECT GroupCode, GroupName FROM OCRG WHERE GroupType = 'C' AND Locked = 'N'";
            }
            else if (search == "OCRG")
            {
                query = "SELECT GroupCode,GroupName FROM OCRG order by GroupCode";
            }
            else if (search == "OHEM")
            {
                query = "SELECT A.empID,CONCAT(CONCAT(A.lastName,', '),A.firstName) Name,B.name Position FROM OHEM A Inner Join OHPS B ON A.position = B.posID Where B.posID = '15'";
            }
            else if (search == "OCRD-ITRC")
            {
                query = "SELECT CardCode [BP Code],CardName [CardName] FROM OCRD Where GroupCode = '" + Param1 + "' and frozenFor = 'N' order by CardCode";
            }
            else if (search == "SERIES-ITRC")
            {
                query = "SELECT T0.Series,T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001";
            }
            else if (search == "PREP")
            {
                //v1.24 7517
                query = "SELECT DISTINCT U_PrepBy " +
                        $"from O{Param1} where U_OrderStatus <> 'Printed' " +
                        "and U_DRNo <> '-' and U_DRNo is not null";
            }
            else if (search == "@CMP_INFO")
            {
                query = "select '' [Code], '' [Name] UNION select Code, Name from [@CMP_INFO]";
            }
            else if (search == "CartonList")
            {
                query = "select DocNum, CreateDate, UpdateDate, Remark from [@CL_HEADER] order by DocNum Desc";
            }
            else if (search == "EmployeeList")
            {
                query = "select * from (SELECT distinct (UPPER(a.firstName + ' ' + LEFT(a.middleName,1) + '.' + ' ' + a.lastName)) [EmployeeName]" +
                        ", b.name [Position] " +
                        " FROM OHEM a " +
                        " left join OHPS b on a.position = b.posID) Z " +
                        " where z.EmployeeName is not null " +
                        " order by EmployeeName asc";
            }
            else if (search == "OCRDDLVRY")
            {
                query = "Select A.CardCode [BP Code],A.CardName [BP Name],(SELECT Z.GroupName FROM OCRG Z Where Z.GroupCode = A.GroupCode) [Chain] from OCRD A Where A.CardType = '" + @Param1.Replace("'", "''") + "'  And A.frozenFor = 'N' Order by A.CardCode";
            }
            else if (search == "Department")
            {
                query = " Select PrcCode [Code], PrcName [Name] from OPRC where DimCode = 1 and Locked = 'N' ";
            }
            else if (search == "Get Vehicle List")
            {
                query = "Select DISTINCT a.U_VDesc, a.Code, a.U_VPla from [@TRUCK] a " +
                        " inner join [@TRUCK_STATUS] b on a.Code = b.Code  " +
                        " where b.U_Active = 'Y' and  " +
                        " '2018-7-23' >= ifnull(b.U_DateFrom, '1990-01-01') and " +
                        " '2018-7-23' <= ifnull(b.U_DateTo, '2099-01-01')";
            }
            else if (search == "@OSBC")
            {
                query = "select distinct U_Code, U_Name [Name] FROM [@OSBC] Order by U_Name ";
            }
            else if (search == "AddressID")
            {
                //query = $"SELECT Address, 'AddressID' [AddressID] FROM CRD1 WHERE AdresType = 'S'";
                query = $"SELECT Address, 'AddressID' [AddressID] FROM CRD1 WHERE CardCode = '{Param1}' AND AdresType = 'S'";
            }
            else if (search == "Outgoing Packinglist - Get DR no.")
            {
                query = "SELECT U_DRNo [DR No.], 'IT' [Module], DocDate [Date], CardName [Vendor], Comments [Remarks], DocDueDate [Due Date] FROM  OWTR " +

                        $"where CardCode = '{@Param1}' AND U_DRNo != '' " +

                        "UNION ALL " +

                        "SELECT U_DRNo [DR No.], 'A/R Invoice' [Module], DocDate [Date], CardName [Vendor], Comments [Remarks], DocDueDate [Due Date] FROM  OINV " +

                        $"where CardCode = '{@Param1}' AND U_DRNo != ''";
            }
            else if (search == "list-of-brand")
            {
                query = "SELECT Code [Brand Codes], Name [Brands] FROM [@OBND]";
            }
            else if (search == "list-of-department")
            {
                query = $"SELECT Distinct B.Name [Departments] from [@OCTG] A inner join [@ODPT] B on A.U_Department = B.Code ";

                if (@Param1 != string.Empty)
                {
                    query += $"where A.U_Brand = '{@Param1}'";
                }
            }
            else if (search == "list-of-SubDepartment")
            {
                query = $"SELECT Distinct C.Name [Sub-Departments] from [@OCTG] A left join [@ODPT] B on A.U_Department = B.Code left join [@OSDP] C ON A.U_SubDepartments = C.Code";

                if (@Param1 != string.Empty)
                {
                    query += $" WHERE ifnull(A.U_Brand,'') = '{@Param1}' ";
                }

                if (@Param2 != string.Empty && Param2 != null)
                {
                    query += $" AND ifnull(B.Name,'') = '{@Param2}' ";
                }
            }
            else if (search == "list-of-category")
            {
                query = "select distinct a.U_Code [Category Codes], a.U_Description [Categories], a.U_Brand [Brand Codes], b.Name [Departments], c.Name [Sub-Departments] from [@OCTG] a " +

                        "left join [@ODPT] b on a.U_Department = b.Code left join [@OSDP] c ON a.U_SubDepartments = c.Code " +

                        $"WHERE a.U_Code != 'Ching Chong' ";

                if (@Param1 != string.Empty)
                {
                    query += $" AND ifnull(a.U_Brand, '') = '{@Param1}' ";
                }

                if (@Param2 != string.Empty)
                {
                    query += $" AND ifnull(b.Name, '') = '{@Param2}' ";
                }

                if (@Param3 != string.Empty)
                {
                    query += $" AND ifnull(c.Name, '') = '{@Param3}' ";
                }
            }
            else if (search == "list-of-SubCategory")
            {
                query = "select distinct a.U_Code [Sub-Category Codes], a.U_Name [Sub-Categories] from [@OSBC] a left join [@ODPT] b on ifnull(a.U_Dept, '') = b.Name left join [@OSDP] c on ifnull(a.U_SubDept, '') = c.Name ";

                if (@Param1 != string.Empty)
                {
                    query += $" WHERE IFNULL(a.U_Brand, '') = '{@Param1}' ";
                }

                if (@Param2 != string.Empty)
                {
                    query += $" AND IFNULL(b.Name,'') = '{@Param2}' ";
                }

                if (@Param3 != string.Empty)
                {
                    query += $" AND IFNULL(c.Name,'') = '{@Param3}' ";
                }

                if (Param4 != string.Empty)
                {
                    query += $" AND IFNULL(a.U_Category,'') = '{@Param4}' ";
                }
            }
            else if (search == "list-of-style")
            {
                query = "select distinct T1.U_Code [Style Codes], T1.U_Style [Styles] from [@OSBC] T0 " +

                        "inner join [@OSTL] T1 on T0.Code = T1.Code inner join [@ODPT] T2 on T0.U_Dept = T2.Name inner join [@OSDP] T3 on T0.U_SubDept = T3.Name " +

                        $"where T1.U_Code != 'ching chong' ";

                if (@Param1 != string.Empty)
                {
                    query += $" AND IFNULL(T0.U_Brand, '') = '{@Param1}' ";
                }

                if (@Param2 != string.Empty)
                {
                    query += $" AND IFNULL(T2.Name,'') = '{@Param2}' ";
                }

                if (@Param3 != string.Empty)
                {
                    query += $" AND IFNULL(T3.Name,'') = '{@Param3}' ";
                }

                if (@Param4 != string.Empty)
                {
                    query += $" AND IFNULL(T0.U_Category,'') = '{@Param4}' ";
                }

                if (@Param5 != string.Empty)
                {
                    query += $" AND IFNULL(T0.U_Name,'') = '{@Param5}' ";
                }
            }
            else if (search == "list-of-size=category")
            {
                query = "SELECT Code [Size Category Codes], Name [Size Categories] FROM [@OSZC]";
            }
            else if (search == "list-of-size")
            {
                query = $"SELECT T1.U_Code [Size Codes], T1.U_Size [Sizes] FROM [@OSZC] T0 INNER JOIN [@OSZS] T1 ON T1.Code = T0.Code WHERE T0.Name = '{Param1}'";
            }
            else if (search == "list-of-color-category")
            {
                query = "SELECT Code [Color Category Codes], Name [Color Categories] FROM [@OCLC]";
            }
            else if (search == "list-of-color")
            {
                query = $"SELECT T1.U_Code [Color Codes], T1.U_Color [Colors] FROM [@OCLR] T1 WHERE T1.Code ='{Param1}'";
            }
            else if (search == "@ODPT")
            {
                query = $" SELECT DISTINCT (SELECT Name FROM [@ODPT] WHERE Code = U_Department) [Departments] FROM [@OCTG] WHERE U_Brand = '{@Param1}' ";
            }
            else if (search == "@OSDP")
            {
                query = $" SELECT DISTINCT (SELECT Name FROM [@OSDP] WHERE Code = U_SubDepartments) [Sub-Departments] FROM [@OCTG] WHERE " +
                        $" U_Code != '' ";
                if (@Param1 != "")
                {
                    query += $" AND U_Brand = '{@Param1}' ";
                }
                if (@Param2 != "")
                {
                    query += $" AND (SELECT Name FROM [@ODPT] WHERE Code = U_Department) = '{@Param2}' ";
                }
            }
            else if (search == "@OCTG")
            {
                query = $" SELECT DISTINCT U_Code [Category Codes],U_Description [Categories] FROM [@OCTG] WHERE " +
                        $" U_Code != '' ";
                if (@Param1 != "")
                {
                    query += $" AND U_Brand = '{@Param1}' ";
                }
                if (@Param2 != "")
                {
                    query += $" AND (SELECT Name FROM [@ODPT] WHERE Code = U_Department) = '{@Param2}' ";
                }
                if (@Param3 != "")
                {
                    query += $" AND (SELECT Name FROM [@OSDP] WHERE Code = U_SubDepartments) = '{@Param3}' ORDER BY U_Code  ";
                }
            }
            else if (search == "@OSBC2")
            {
                query = $" SELECT U_Code [Sub-Category Codes], U_Name [Sub-Categories] FROM [@OSBC] WHERE " +
                        $" U_Code != '' ";
                if (@Param1 != "")
                {
                    query += $" AND U_Brand = '{@Param1}' ";
                }
                if (Param2 != "")
                {
                    query += $" AND U_Category = '{@Param2}' ORDER BY U_Code ";
                }
            }
            else if (search == "@OSZC")
            {
                query = $" SELECT Code [Size Category Codes], Name [Size Categories] FROM [@OSZC] Order By Code ";
            }
            else if (search == "@OSZS")
            {
                query = $" SELECT U_Code [Size Codes], U_Size [Sizes] FROM [@OSZS] WHERE Code = '{@Param1}' ";
            }
            else if (search == "@OCLC")
            {
                query = $" SELECT Code [Color Category Codes], Name [Color Categories] FROM [@OCLC] Order By Name  ";
            }
            else if (search == "@OCLR")
            {
                query = " SELECT B.U_Code [Child Code], B.U_Color [Child Name], A.Code [Parent Code], A.Name [Parent Name] FROM [@OCLC] A " +
                        $" INNER JOIN [@OCLR] B ON A.Code = B.Code WHERE A.Name = '{@Param1}' ORDER BY A.Name";
            }
            else if (search == "@OSTL")
            {
                query = " SELECT U_Code [Style Codes], U_Style [Styles] FROM [@OSTL] WHERE " +
                        " Code != '' ";
                if (@Param1 != "" && @Param2 != "" && @Param3 != "")
                {
                    query += $" AND Code = '{@Param1}{@Param2}{@Param3}' ";
                }
                else
                {
                    if (@Param1 != "")
                    {
                        query += $" AND Code like '{@Param1}%' ";
                    }
                    if (@Param3 != "")
                    {
                        query += $" AND Code like '%{@Param3}' ";
                    }
                    if (@Param2 != "")
                    {
                        query += $" AND Code like '%{@Param2}%' ";
                    }
                }

            }
            else if (search == "OPKL_BP")
            {

                query = "SELECT " +
                        " T2.AbsEntry [Picklist no.]" +
                        " , T1.CardCode [CardCode] " +
                        " , T1.CardName [Card Name] " +
                        " , T1.DocDate [Doc Date] " +
                        " , T2.OrderEntry" +
                        " , T1.DocEntry " +
                        " , T1.DocNum " +
                        " , 'ITR - Pick List'[FromDoc] " +
                        " , T1.DocType " +
                        " , SUM(T2.PickQtty) [Total Pick Qty.] " +
                        " FROM OWTQ T1 INNER JOIN PKL1 T2 on T1.DocEntry = T2.OrderEntry and T1.ObjType = T2.BaseObject and T2.PickStatus != 'C'" +
                        $" WHERE " +
                        //$"T1.Printed = 'N' and " +    //On comment due to pick list not visible in selection list 081519
                        $"T1.CardCode = '{@Param1}'" +
                        " Group By T1.DocEntry, T1.DocNum, T2.AbsEntry, T1.DocType, T1.CardCode, T1.CardName, T1.DocDate, T1.ObjType, T2.OrderEntry Order By T2.AbsEntry desc";

            }
            else if (search == "ODLN")
            {
                query = $"SELECT DocEntry,DocNum,CardCode [BP Code]" +
                                      ",CardName [BP Name]" +
                                      ",DocDate [Month of Sale]" +
                                      ",TaxDate [Document Date]" +
                                      ", ROUND(DocTotal,2) [Document Total]" +
                               " FROM ODLN Order By CreateDate DESC";
                //",DocDueDate [Delivery Date]" +
            }
            else if (search == "ODLN_BP")
            {
                query = $"SELECT DocEntry,DocNum,CardCode [BP Code]" +
                                      ",CardName [BP Name]" +
                                      ",DocDate [Month of Sale]" +
                                      ",DocDueDate [Delivery Date]" +
                                      ",TaxDate [Document Date]" +
                                      ", ROUND(DocTotal,2) [Document Total]" +
                               " FROM ODLN Where CardCode = '" + @Param1.Replace("'", "''") + "' Order By CreateDate DESC";
            }
            else if (search == "@Carton - Documentlist")
            {
                query = $"SELECT DocEntry [#], DocNum [Document No.], DocDate [Posting Date], U_Remarks [Remarks] " +

                $"FROM O{@Param1} " +

                $"WHERE ISNULL(CardCode,'') = '{@Param2}' ";

                if (@Param3 != string.Empty && @Param3 != null)
                {
                    if (@Param1 == "PDN")
                    {
                        string x = StringQueryRepository.TransType(@Param3);
                        var Transtype = hana.Get(x).Rows[0].ItemArray[0].ToString();
                        query += $" AND U_TransactionType = '{Transtype}'";
                    }
                    else
                    {
                        query += $" AND U_TransferType = '{@Param3}'";
                    }
                }
            }
            else if (search == "@OSTL_NoFilter")
            {
                query = $" SELECT U_Code [Code], U_Style [Name] FROM [@OSTL] ";
            }
            else if (search == "@ODPT_NoFilter")
            {
                query = $" SELECT DISTINCT (SELECT Name FROM [@ODPT] WHERE Code = U_Department) [Name] FROM [@OCTG] ";
            }
            else if (search == "@OSDP_NoFilter")
            {
                query = $" SELECT DISTINCT (SELECT Name FROM [@OSDP] WHERE Code = U_SubDepartments) [Name] FROM [@OCTG] ";
            }
            else if (search == "@OCTG_NoFilter")
            {
                query = $" SELECT DISTINCT U_Code [Code],U_Description [Name] FROM [@OCTG] ORDER BY U_Code  ";
            }
            else if (search == "@OSBC2_NoFilter")
            {
                query = $" SELECT U_Code [Code], U_Name [Name] FROM [@OSBC] ORDER BY U_Code ";
            }
            else if (search == "@OSZS_NoFilter")
            {
                query = $" SELECT U_Code [Code], U_Size [Name] FROM [@OSZS] ";
            }
            else if (search == "@OCLR_NoFilter")
            {
                query = " SELECT A.Code [Code], A.Name [Parent Name], B.U_Color [Child Name] FROM [@OCLC] A " +
                        $" INNER JOIN [@OCLR] B ON A.Code = B.Code ORDER BY A.Name";
            }
            else if (search == "CopyFromSO")
            {
                query = $"SELECT DocEntry [#], DocType[Document Type], DocNum[Document Number], CardCode[BP Code], CardName[Bp Name]" +
                        $", DocDate[Posting Date], ReqDate[Delivery Date], TaxDate[Document Date] FROM ORDR WHERE DocStatus = 'O' and CardCode = '{@Param1}' Order By DocNum";
            }
            else if (search == "GetUDF_FMS")
            {

                var GetUDF_FMS = hana.Get(SP.UDF_FMS);
                string ItrUDF_FmsQry = helper.ReadDataRow(GetUDF_FMS, 1, "", 0);
                query = hana.Get(string.Format(ItrUDF_FmsQry, @Param1, @Param2)).Rows[0]["QString"].ToString();
            }
            else if (search == "SapHeaderTable")
            {
                query = $"SELECT DocEntry [#]" +
                    $", DocNum[Document Number]" +
                    $", CardCode[BP Code]" +
                    $", CardName[Bp Name]" +
                    $", DocDate[Posting Date]" +
                    $", ReqDate[Delivery Date]" +
                    $", TaxDate[Document Date] " +
                    $"FROM {@Param1} WHERE DocStatus = 'O' and DocType = 'I' Order By DocEntry Desc";
            }

            //Bind Data
            if (fromSQL == false)
            {
                //On comment by Cedi 060819 due to conflict on getting values
                //dt = DataAccess.Select(DataAccess.conStr("HANA"), query);

                dt = hana.Get(query);
            }
            else
            {
                dt = msSql.Get(query);
            }
            selectedrow = -1;

            dgvSearchList.DataSource = dt;

            //DECLARE.dataGridLayout(dgvSearchList);
            dataGridLayout(dgvSearchList);
        }
        private void dataGridLayout(DataGridView dgv)
        {
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;

            if (allowMultiple == false)
            {
                dgv.MultiSelect = false;
            }
            else
            {
                dgv.MultiSelect = true;
            }
        }
        private void dgvSearchList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (allowMultiple == false)
                {
                    oCode = dgvSearchList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    if (dgvSearchList.Columns.Count > 1)
                    {
                        oName = dgvSearchList.Rows[e.RowIndex].Cells[1].Value.ToString();
                        if (search == "OITW" || search == "OITW1" || search == "OITW2" || search == "OITW3")
                        {
                            if (dgvSearchList.Columns.Count >= 3)
                            {
                                oAvailable = dgvSearchList.Rows[e.RowIndex].Cells[3].Value.ToString();
                            }
                        }

                        if (oSearchMode == "OVTG" || oSearchMode == "OCRD*" || oSearchMode == "OPKL")
                        {
                            oRate = dgvSearchList.Rows[e.RowIndex].Cells[2].Value.ToString();
                        }
                    }

                    if (search == "OWTQ_NOBP" || search == "OWTQ" || search == "OPKL_I" || search == "OPKL_BP")
                    {

                        InventoryTransferHeaderModel.oCode = dgvSearchList.Rows[e.RowIndex].Cells[0].Value.ToString();
                        InventoryTransferHeaderModel.oBPCode = dgvSearchList.Rows[e.RowIndex].Cells[1].Value.ToString();
                        if (search == "OPKL_I" || search == "OPKL_BP")
                        {
                            InventoryTransferHeaderModel.oOrderEntry = dgvSearchList.Rows[e.RowIndex].Cells[4].Value.ToString();
                        }
                    }
                    else if (search == "ORDR_BP" || search == "ORDR" || search == "OPKL_S")
                    {
                        SalesInvoiceHeaderModel.oCode = dgvSearchList.Rows[e.RowIndex].Cells[0].Value.ToString();
                        SalesInvoiceHeaderModel.oBPCode = dgvSearchList.Rows[e.RowIndex].Cells[1].Value.ToString();
                        if (search == "OPKL_S")
                        {
                            SalesInvoiceHeaderModel.oOrderEntry = dgvSearchList.Rows[e.RowIndex].Cells[4].Value.ToString();
                        }
                    }
                    else if (search == "OCRDDLVRY")
                    {
                        UnofficialSalesHeaderModel.oBPCode = dgvSearchList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    }
                }
                else
                {
                    if (search == "OWTQ_NOBP" || search == "OWTQ" || search == "OPKL_I" || search == "OPKL_BP")
                    {
                        foreach (DataGridViewRow row in dgvSearchList.Rows)
                        {
                            if (row.Selected == true)
                            {
                                if (search == "OPKL_I" || search == "OPKL_BP")
                                {
                                    InventoryTransferHeaderModel.DDWdocentry.Add(new InventoryTransferHeaderModel.DDWdocentryData
                                    {
                                        DocEntry = Convert.ToInt32(row.Cells[0].Value),
                                        BpCode = row.Cells[1].Value.ToString(),
                                        OrderEntry = row.Cells[4].Value.ToString()
                                    });
                                }
                                else
                                {
                                    InventoryTransferHeaderModel.DDWdocentry.Add(new InventoryTransferHeaderModel.DDWdocentryData
                                    {
                                        DocEntry = Convert.ToInt32(row.Cells[0].Value),
                                        BpCode = row.Cells[1].Value.ToString()
                                    });
                                }

                            }
                        }
                    }
                    else if (search == "ORDR_BP" || search == "ORDR" || search == "OPKL_S")
                    {
                        foreach (DataGridViewRow row in dgvSearchList.Rows)
                        {
                            if (row.Selected == true)
                            {
                                if (search == "OPKL_S")
                                {
                                    oCode = row.Cells[0].Value.ToString();
                                    InvoiceHeaderModel.DDWdocentry.Add(new InvoiceHeaderModel.DDWdocentryData
                                    {
                                        DocEntry = Convert.ToInt32(row.Cells[0].Value),
                                        BpCode = row.Cells[1].Value.ToString(),
                                        OrderEntry = row.Cells[4].Value.ToString()
                                    });
                                }
                                else
                                {
                                    InvoiceHeaderModel.DDWdocentry.Add(new InvoiceHeaderModel.DDWdocentryData
                                    {
                                        DocEntry = Convert.ToInt32(row.Cells[0].Value),
                                        BpCode = row.Cells[1].Value.ToString()
                                    });
                                }

                            }
                        }
                    }
                }
                this.Close();
            }
        }
        private void dgvSearchList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvSearchList.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            Choose();
        }

        void Choose()
        {
            if (dgvSearchList.Rows.Count > 0)
            {
                if (allowMultiple == false)
                {

                    int rowindex = dgvSearchList.CurrentCell.RowIndex;
                    int colindex = dgvSearchList.CurrentCell.ColumnIndex;

                    if (rowindex != -1)
                    {
                        oCode = dgvSearchList.Rows[rowindex].Cells[0].Value.ToString();
                        if (dgvSearchList.Columns.Count > 1)
                        {
                            oName = dgvSearchList.Rows[rowindex].Cells[1].Value.ToString();
                        }

                        if (oSearchMode == "OVTG" || oSearchMode == "OCRD*")
                        {
                            string hello = dgvSearchList.Rows[rowindex].Cells[2].Value.ToString();
                            oRate = hello;
                        }

                        if (search == "OWTQ_NOBP" || search == "OWTQ" || search == "OPKL_I" || search == "OPKL_BP")
                        {
                            InventoryTransferHeaderModel.oCode = dgvSearchList.Rows[rowindex].Cells[0].Value.ToString();
                            InventoryTransferHeaderModel.oBPCode = dgvSearchList.Rows[rowindex].Cells[1].Value.ToString();
                            if (search == "OPKL_I" || search == "OPKL_BP")
                            {
                                InventoryTransferHeaderModel.oOrderEntry = dgvSearchList.Rows[rowindex].Cells[4].Value.ToString();
                            }
                        }
                        else if (search == "ORDR_BP" || search == "ORDR" || search == "OPKL_S")
                        {
                            SalesInvoiceHeaderModel.oCode = dgvSearchList.Rows[rowindex].Cells[0].Value.ToString();
                            SalesInvoiceHeaderModel.oBPCode = dgvSearchList.Rows[rowindex].Cells[1].Value.ToString();
                            if (search == "OPKL_S")
                            {
                                SalesInvoiceHeaderModel.oOrderEntry = dgvSearchList.Rows[rowindex].Cells[4].Value.ToString();
                            }
                        }
                        this.Close();
                    }
                }
                else
                {
                    if (search == "OWTQ_NOBP" || search == "OWTQ" || search == "OPKL_I" || search == "OPKL_BP" || search == "OPKL_S")
                    {
                        foreach (DataGridViewRow row in dgvSearchList.Rows)
                        {
                            if (row.Selected == true)
                            {
                                if (search == "OPKL_I" || search == "OPKL_BP")
                                {
                                    InventoryTransferHeaderModel.DDWdocentry.Add(new InventoryTransferHeaderModel.DDWdocentryData
                                    {
                                        DocEntry = Convert.ToInt32(row.Cells[0].Value),
                                        BpCode = row.Cells[1].Value.ToString(),
                                        OrderEntry = row.Cells[4].Value.ToString()
                                    });
                                }
                                else
                                {
                                    InventoryTransferHeaderModel.DDWdocentry.Add(new InventoryTransferHeaderModel.DDWdocentryData
                                    {
                                        DocEntry = Convert.ToInt32(row.Cells[0].Value),
                                        BpCode = row.Cells[1].Value.ToString(),
                                        BpCode2 = row.Cells[2].Value.ToString()
                                    });
                                }

                                if (search == "OPKL_S")
                                {
                                    if (search == "OPKL_S")
                                    {
                                        InvoiceHeaderModel.DDWdocentry.Add(new InvoiceHeaderModel.DDWdocentryData
                                        {
                                            DocEntry = Convert.ToInt32(row.Cells[0].Value),
                                            BpCode = row.Cells[1].Value.ToString(),
                                            OrderEntry = row.Cells[4].Value.ToString()
                                        });
                                    }
                                    else
                                    {
                                        InvoiceHeaderModel.DDWdocentry.Add(new InvoiceHeaderModel.DDWdocentryData
                                        {
                                            DocEntry = Convert.ToInt32(row.Cells[0].Value),
                                            BpCode = row.Cells[1].Value.ToString()
   
                                        });
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dgvSearchList.Rows)
                        {
                            if (row.Selected == true)
                            {
                                DECLARE._multipleSelection.Add(new DECLARE.MultipleSelection { Code = row.Cells[0].Value.ToString(), Name = row.Cells[1].Value.ToString() });
                            }
                        }
                    }
                    this.Close();
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("No data to be selected.", true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void Search()
        {
            DataTable dt = null;
            string query = "";

            if (search == "OCRD")
            {
                query = "SELECT CardCode,CardName FROM OCRD Where CardType = '" + @Param1 + "' and CardName like '" + txtSearch.Text + "%' And frozenFor = 'N' Order by CardCode";
                dt = DataAccess.Select(DataAccess.conStr("HANA"), query);
            }
            else if (search == "@PRSTYLE")
            {
                query = "SELECT Code ,Name FROM [@PRSTYLE] Where Name like '" + txtSearch.Text + "%'";
            }
            else if (search == "@PRCOLOR")
            {
                // query = "SELECT Code ,Name FROM [@PRCOLOR]";

                query = "SELECT DISTINCT A.U_Color,B.Name FROM OITM A Left Join [@PRCOLOR] B " +
                        " On A.U_Color = B.Code  Where A.U_StyleCode = '" + @Param1 + "' and B.Name like '" + txtSearch.Text + "%'";
            }
            else if (search == "@Section")
            {
                query = "SELECT DISTINCT U_Section FROM OITM WHERE U_StyleCode = '" + @Param1 + "' and U_Color = '" + @Param2 + "' and U_Section like '" + txtSearch.Text + "%'  And frozenFor = 'N'";
            }

            //Bind Data

            dt = hana.Get(query);
            dgvSearchList.DataSource = dt;

            //DECLARE.dataGridLayout(dgvSearchList);
            dataGridLayout(dgvSearchList);
        }

        private void dgvSearchList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            _rowIndex = e.RowIndex;
        }

        private void frmSearch2_Resize(object sender, EventArgs e)
        {
            //if (WindowState == FormWindowState.Maximized)
            //{
            //    Size = new Size(MdiParent.ClientSize.Width, max_height);
            //    WindowState = FormWindowState.Normal;
            //    Location = new Point(0, 0);
            //}
            //if (WindowState == FormWindowState.Maximized)
            //{
            //    Size = new Size(MdiParent.ClientSize.Width, max_height);
            //    WindowState = FormWindowState.Normal;
            //    Location = new Point(0, 0);
            //}
            //if (WindowState == FormWindowState.Maximized)
            //{
            //    //Size = new Size(MdiParent.ClientSize.Width, max_height);
            //    //WindowState = FormWindowState.Normal;
            //    //Location = new Point(0, 0);
            //}
        }

        private void dgvSearchList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {

                if (keyData == Keys.Enter && fromWhsLock == true)
                {
                    foreach (DataGridViewRow row in dgvSearchList.Rows)
                    {
                        if (row.Selected == true)
                        {
                            DECLARE._multipleSelection.Add(new DECLARE.MultipleSelection { Code = row.Cells[0].Value.ToString(), Name = row.Cells[1].Value.ToString() });
                        }
                    }
                    this.Close();
                    return true;
                }
                else if (keyData == Keys.Enter && scanitems == false)
                {
                    oCode = dgvSearchList.Rows[_rowIndex].Cells[0].Value.ToString();
                    if (dgvSearchList.Columns.Count > 1)
                    {
                        oName = dgvSearchList.Rows[_rowIndex].Cells[1].Value.ToString();
                    }

                    this.Close();

                    return true;
                }
                else if (keyData == Keys.Escape)
                {
                    Close();
                }
                else if (keyData == (Keys.Alt | Keys.S))
                {
                    txtSearch.Focus();
                }
                else if (keyData == (Keys.Alt | Keys.A))
                {
                    dgvSearchList.Focus();
                }
                else if (keyData == (Keys.Alt | Keys.D1))
                {
                    DataGridViewColumn SelectedColumn = dgvSearchList.Columns[0];
                    dgvSearchList.Sort(SelectedColumn, ListSortDirection.Ascending);
                }
                else if (keyData == (Keys.Alt | Keys.D2))
                {
                    DataGridViewColumn SelectedColumn = dgvSearchList.Columns[1];
                    dgvSearchList.Sort(SelectedColumn, ListSortDirection.Ascending);
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            this.CurrentPageIndex = 1;
            this.dgvSearchList.DataSource = GetCurrentRecords(this.CurrentPageIndex);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (this.CurrentPageIndex > 1)
            {
                this.CurrentPageIndex--;
                this.dgvSearchList.DataSource =
            GetCurrentRecords(this.CurrentPageIndex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearchList.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgvSearchList.Rows)
                {
                    if (row.Cells[defaultColumn].Value.ToString().ToUpper().Contains(txtSearch.Text.ToUpper()))
                    {
                        row.Selected = true;
                        _rowIndex = row.Index;
                        selectedrow = row.Index;
                        dgvSearchList.FirstDisplayedScrollingRowIndex = _rowIndex;
                        break;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
                //string field = _rowIndex[""].tos
            }
        }


        private void dgvSearchList_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Down && dgvSearchList.Focus() && selectedrow != -1)
            {
                dgvSearchList.CurrentCell = dgvSearchList.Rows[selectedrow].Cells[0];
                dgvSearchList.Rows[selectedrow].Selected = true;
                selectedrow = -1;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.CurrentPageIndex < this.TotalPage)
            {
                this.CurrentPageIndex++;
                this.dgvSearchList.DataSource = GetCurrentRecords(this.CurrentPageIndex);
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {

            this.CurrentPageIndex = TotalPage;
            this.dgvSearchList.DataSource = GetCurrentRecords(this.CurrentPageIndex);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvSearchList.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgvSearchList.Rows)
                {
                    if (row.Cells[defaultColumn].Value.ToString().ToUpper().StartsWith(txtSearch.Text.ToUpper()))
                    {
                        row.Selected = true;
                        _rowIndex = row.Index;
                        selectedrow = row.Index;
                        dgvSearchList.FirstDisplayedScrollingRowIndex = _rowIndex;
                        break;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }

                //string field = _rowIndex[""].tos
            }
            else if (dgvSearchList.Columns.Count == 1)
            {
                foreach (DataGridViewRow row in dgvSearchList.Rows)
                {
                    if (row.Cells[0].Value.ToString().ToUpper().StartsWith(txtSearch.Text.ToUpper()))
                    {
                        row.Selected = true;
                        _rowIndex = row.Index;
                        selectedrow = row.Index;
                        dgvSearchList.FirstDisplayedScrollingRowIndex = _rowIndex;
                        break;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }
        }

        private void dgvSearchList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            _rowIndex = e.RowIndex;
        }
        private void dgvSearchList_SelectionChanged(object sender, EventArgs e)
        {
            //if (dgvSearchList.CurrentCell.RowIndex != -1)
            //{
            //    int _row = dgvSearchList.CurrentCell.RowIndex;
            //    _rowIndex = _row;
            //}

        }

        //Try pagination

        private void CalculateTotalPages(string Table)
        {
            if (Table == "OWTQ")
            {
                string query = "SELECT Count(DocEntry) [Count] from OWTQ";
                var dt = new DataTable();
                dt = hana.Get(query);
                Int64 rowCount = Convert.ToInt64(helper.ReadDataRow(dt, "Count", "0", 0));
                TotalPage = rowCount / PgSize;
                // if any row left after calculated pages, add one more page 
                if (rowCount % PgSize > 0)
                    TotalPage += 1;
            }
            else if (Table == "OPDN")
            {
                string query = "SELECT Count(DocEntry) [Count] from OPDN";
                var dt = new DataTable();
                dt = hana.Get(query);
                Int64 rowCount = Convert.ToInt64(helper.ReadDataRow(dt, "Count", "0", 0));
                TotalPage = rowCount / PgSize;
                // if any row left after calculated pages, add one more page 
                if (rowCount % PgSize > 0)
                    TotalPage += 1;
            }
            else if (Table == "OWTR")
            {
                string query = "SELECT Count(DocEntry) [Count] from OWTR";
                DataTable dt = new DataTable();
                dt = hana.Get(query);
                Int64 rowCount = Convert.ToInt64(helper.ReadDataRow(dt, "Count", "0", 0));
                TotalPage = rowCount / PgSize;
                // if any row left after calculated pages, add one more page 
                if (rowCount % PgSize > 0)
                    TotalPage += 1;
            }
        }

        private DataTable GetCurrentRecords(Int64 page)
        {
            var dt = new DataTable();
            if (search == "ORDR_US")
            {
                //string query = "SELECT TOP " + PgSize + " A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.DocTotal [Total] " +
                //    ",(SELECT Sum(Z.Quantity) From RDR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM ORDR A Where A.Series = 73 And A.DocStatus = 'O' Order By A.DocEntry DESC";

                //if (page == 1)
                //{
                //    dgvSearchList.Columns.Clear();
                //    dt = DataAccess.Select(DataAccess.conStr("HANA"), query);
                //}
                //else
                //{
                //    dgvSearchList.Columns.Clear();
                //    Int64 PreviousPageOffSet = (page - 1) * PgSize;

                //    query = "SELECT TOP " + PgSize + " A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.DocTotal [Total] " +
                //   ",(SELECT Sum(Z.Quantity) From RDR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM ORDR A Where NOT (A.DocEntry IN " +
                //   "(SELECT TOP " + PreviousPageOffSet.ToString() + "DocEntry FROM ORDR ORDER BY DocEntry Desc)) Order By A.DocDate Desc";
                //    //"A.Series = 73 And A.DocStatus = 'O' Order By A.DocEntry DESC"


                //    //query = "SELECT TOP " + PgSize + " A.DocEntry, (SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                //    //    ",(SELECT Sum(Z.Quantity) From WTQ1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTQ A Where NOT  (A.DocEntry  IN " +
                //    //    "(SELECT TOP " + PreviousPageOffSet.ToString() + " DocEntry  FROM OWTQ ORDER BY DocEntry Desc)) Order By A.DocDate Desc";
                //    dt = DataAccess.Select(DataAccess.conStr("HANA"), query);

                //}
            }
            else if (search == "OPDN")
            {
                string query = "SELECT TOP " + PgSize + " A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],(Case when A.DocStatus = 'O' then 'Open' else 'Closed' end) [Status],A.DocTotal [Total] " +
                   ",(SELECT Sum(Z.Quantity) From PDN1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OPDN A Order By DocNum";

                if (page == 1)
                {
                    dgvSearchList.Columns.Clear();
                    dt = hana.Get(query);

                }
                else
                {
                    dgvSearchList.Columns.Clear();
                    Int64 PreviousPageOffSet = (page - 1) * PgSize;

                    query = "SELECT TOP " + PgSize + " A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],(Case when A.DocStatus = 'O' then 'Open' else 'Closed' end) [Status],A.DocTotal [Total] " +
                        ",(SELECT Sum(Z.Quantity) From PDN1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OPDN A WHERE NOT (A.DocEntry  IN (SELECT TOP " + PreviousPageOffSet.ToString() + " DocEntry  FROM OPDN ORDER BY DocNum))  Order By DocNum";

                    //query = "SELECT TOP " + PgSize + " A.DocEntry, (SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                    //    ",(SELECT Sum(Z.Quantity) From WTQ1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTQ A Where NOT  (A.DocEntry  IN " +
                    //    "(SELECT TOP " + PreviousPageOffSet.ToString() + " DocEntry  FROM OWTQ ORDER BY DocEntry Desc)) Order By A.DocEntry Desc";



                    dt = dt = hana.Get(query);

                }
            }
            else if (search == "OWTR")
            {
                string query = "SELECT TOP " + PgSize + " A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                     ",(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTR A Order By A.DocEntry Desc";

                if (page == 1)
                {
                    dgvSearchList.Columns.Clear();

                    dt = dt = hana.Get(query);

                }
                else
                {
                    dgvSearchList.Columns.Clear();
                    Int64 PreviousPageOffSet = (page - 1) * PgSize;

                    query = "SELECT TOP " + PgSize + " A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                        ",(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTR A Where NOT  (A.DocEntry  IN " +
                        "(SELECT TOP " + PreviousPageOffSet.ToString() + " DocEntry  FROM OWTR ORDER BY DocEntry Desc)) Order By A.DocEntry Desc";

                    //query = "SELECT TOP " + PgSize + " A.DocEntry, (SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                    //    ",(SELECT Sum(Z.Quantity) From WTQ1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTQ A Where NOT  (A.DocEntry  IN " +
                    //    "(SELECT TOP " + PreviousPageOffSet.ToString() + " DocEntry  FROM OWTQ ORDER BY DocEntry Desc)) Order By A.DocEntry Desc";



                    dt = dt = hana.Get(query);

                }
            }
            else if (search == "OPKL")
            {
                string query = "SELECT TOP " + PgSize + " A.U_DRNo [DR No.],A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_PONo [PO No.] " +
                   ",(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTR A Where A.U_DRNo is not null AND A.U_DRNo <> '-' AND (SELECT GroupCode FROM OCRD Z Where Z.CardCode = A.CardCode) = '104' Order By A.U_DRNo Desc ";

                if (page == 1)
                {
                    dgvSearchList.Columns.Clear();

                    dt = dt = hana.Get(query);

                }
                else
                {
                    dgvSearchList.Columns.Clear();
                    Int64 PreviousPageOffSet = (page - 1) * PgSize;

                    query = "SELECT TOP " + PgSize + " A.U_DRNo [DR No.],A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_PONo [PO No.] " +
                    ",(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTR A Where A.U_DRNo is not null AND A.U_DRNo <> '-' AND (SELECT GroupCode FROM OCRD Z Where Z.CardCode = A.CardCode) = '104' " +
                    " Where NOT  (A.DocEntry  IN (SELECT TOP " + PreviousPageOffSet.ToString() + " DocEntry  FROM OWTR A Where A.U_DRNo is not null AND A.U_DRNo <> '-' AND (SELECT GroupCode FROM OCRD Z Where Z.CardCode = A.CardCode) = '104' ORDER BY DocEntry Desc)) Order By A.U_DRNo Desc ";

                    dt = dt = hana.Get(query);

                }
            }
            else
            {

                //string query = "SELECT A.U_DRNo [DR No.],A.DocEntry,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_PONo [PO No.] " +
                //        ",(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTR A Where A.U_DRNo is not null AND A.U_DRNo <> '-' Order By A.DocNum Desc";
                string query = "SELECT TOP " + PgSize + " A.DocEntry,(SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                        ",(SELECT Sum(Z.Quantity) From WTQ1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTQ A Order By A.DocEntry Desc";

                if (page == 1)
                {
                    dgvSearchList.Columns.Clear();

                    dt = dt = hana.Get(query);

                }
                else
                {
                    dgvSearchList.Columns.Clear();
                    Int64 PreviousPageOffSet = (page - 1) * PgSize;


                    query = "SELECT TOP " + PgSize + " A.DocEntry, (SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum [Doc No.],A.CardCode [BP Code],A.CardName [BP Name],A.DocDate [Doc Date],A.DocStatus [Status],A.U_SINo [SI No.],A.U_DRNo [DR No.],A.U_PONo [PO No.] " +
                        ",(SELECT Sum(Z.Quantity) From WTQ1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] FROM OWTQ A Where NOT  (A.DocEntry  IN " +
                        "(SELECT TOP " + PreviousPageOffSet.ToString() + " DocEntry  FROM OWTQ ORDER BY DocEntry Desc)) Order By A.DocEntry Desc";



                    dt = dt = hana.Get(query);

                }

            }

            return dt;
        }

    }
}

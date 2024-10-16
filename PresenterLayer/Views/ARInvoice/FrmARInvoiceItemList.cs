using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using DirecLayer._02_Form.MVP.Views;
using PresenterLayer.Views.Main;
using System.Runtime.InteropServices;
using PresenterLayer.Helper.SalesOrder;
using Context;
using PresenterLayer.Helper;
using zDeclare;
using DirecLayer;
using InfrastructureLayer.InventoryRepository;

namespace PresenterLayer.Views
{
    public partial class FrmARInvoiceItemList : MetroForm
    {
        private SalesStyle soic = new SalesStyle();
        private SOmaintenance SOm = new SOmaintenance();
        public MainForm frmMain;
        private SAPHanaAccess hana { get; set; }
        QueryRepository Query = new QueryRepository();
        DataHelper helper { get; set; }
        //need to clarify
        private string oCode, oName, _StyleCode;
        public FrmPurchaseOrder PO;
        private FrmARInvoice Invoice;
        private static int defaultColumn = 6, _rowIndex = 0;
        //private int DefaultColumn = 1, ColRowIndex = 0, SearchRowCount = 0, TotalDataCount = 0;
        //private long PageSize = 0, CurrentPageIndex = 1, DefaultPgSize = 500, TotalPage = 0;

        public string oStyleCode, oColorCode, oSection, oBPCode, oWhsCode, oBpCode, oBpName, oWhse, oTaxGroup, oDate;

        private long DefaultPgSize = 50000;
        private long PgSize = 0;
        private long CurrentPageIndex = 1;
        private long TotalPage = 0;
        int TotalDataCount = 0;
        int CurrentDataCount = 0;
        bool SearchEvent = false;
        int SearchRowCnt = 0;
        bool SearchClick = false;
        private int index = 0;
        private string strPreviousEvent = "";

        public FrmARInvoiceItemList([Optional] FrmARInvoice Invoice, [Optional] MainForm frmMain)
        {
            this.Invoice = Invoice;
            this.frmMain = frmMain;
           // this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            CalculateTotalPages();
            defaultColumn = 6;
            InitializeComponent();
            hana = new SAPHanaAccess();
            helper = new DataHelper();
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {

            if (keyData == (Keys.Alt | Keys.S))
            {
                txtSearch.Focus();
            }

            else if (keyData == (Keys.Alt | Keys.B))     //Brands
            {
                LoadSelectionOfBrands();
            }

            else if (keyData == (Keys.Alt | Keys.D))     //Departments
            {
                LoadSelectionOfDepts();
            }

            else if (keyData == (Keys.Alt | Keys.P))     //Sub Departments
            {
                LoadSelectionOfSubDepts();
            }

            else if (keyData == (Keys.Alt | Keys.C))     //Categories
            {
                LoadSelectionOfCategories();
            }

            else if (keyData == (Keys.Alt | Keys.U))     //Sub Categories
            {
                LoadSelectionOfSubCategories();
            }

            else if (keyData == (Keys.Alt | Keys.Z))     //Sizes
            {
                LoadSelectionOfSizes();
            }

            else if (keyData == (Keys.Alt | Keys.I))     //Sub Sizes
            {
                LoadSelectionOfSubSizes();
            }

            else if (keyData == (Keys.Alt | Keys.E))     //Colors
            {
                LoadSelectionOfColors();
            }

            else if (keyData == (Keys.Alt | Keys.O))     //Sub Colors
            {
                LoadSelectionOfSubColors();
            }

            else if (keyData == (Keys.Alt | Keys.Y))     //Styles
            {
                LoadSelectionOfStyles();
            }

            else if (keyData == (Keys.Alt | Keys.D1))   //Focus on List of Items Table
            {
                gvSO.Focus();
                SelectItemList(false);
            }

            else if (keyData == (Keys.Alt | Keys.D2))   //Focus on List of Selected Items Table
            {
                gvSelectedItem.Focus();
                SelectItemList(true);

                if (gvSelectedItem.Rows.Count > 0)
                {
                    int index = gvSelectedItem.CurrentRow.Index;
                    gvSelectedItem.CurrentCell = gvSelectedItem[7, index];
                    gvSelectedItem[7, index].Selected = true;
                    gvSelectedItem.BeginEdit(true);
                }
            }

            else if (keyData == Keys.Enter)
            {
                if (gvSO.Focused == true && strPreviousEvent == "")             //Transfer items from List to Selected Items Table
                {
                    soic.GetSelectedItems(gvSO, gvSelectedItem);
                }
                else if (gvSelectedItem.Focused == true && strPreviousEvent == "")        //Transfer items from Selected Items Table to the List
                {
                    GetBackSelectedItems(gvSelectedItem, gvSO);
                }

                strPreviousEvent = strPreviousEvent != "" ? "" : strPreviousEvent;
            }

            else if (keyData == (Keys.Alt | Keys.Q) && gvSelectedItem.Focused == true)
            {
                if (gvSelectedItem.Rows.Count > 0)
                {
                    int index = gvSelectedItem.CurrentRow.Index;
                    gvSelectedItem.CurrentCell = gvSelectedItem[7, index];
                    gvSelectedItem[7, index].Selected = true;
                    gvSelectedItem.BeginEdit(true);
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("No items to set Quantity.", true);
                }
            }

            else if (keyData == (Keys.Alt | Keys.A))
            {
                AddItems();
            }

            else if (keyData == Keys.Escape)
            {
                Close();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SelectItemList(bool ItemListSelected)
        {
            gvSelectedItem.BorderStyle = ItemListSelected == true ? System.Windows.Forms.BorderStyle.FixedSingle : System.Windows.Forms.BorderStyle.None;
            gvSO.BorderStyle = ItemListSelected == true ? System.Windows.Forms.BorderStyle.None : System.Windows.Forms.BorderStyle.FixedSingle;
        }

    

        private void FrmSalesOrderItemList_Load_1(object sender, EventArgs e)
        {
            soic.dgvSetup(gvSelectedItem);
            PgSize = DefaultPgSize;
            LoadCurrentRecords();
        }

        public bool IsCartonActive;

        private void navItemGet_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "navItemGet":
                    soic.GetSelectedItems(gvSO, gvSelectedItem);
                    break;
                case "navItemGetAll":
                    soic.GetAllSelectedItems(gvSO, gvSelectedItem);
                    break;
                case "navItemBackAll":
                    soic.GetAllSelectedItems(gvSelectedItem, gvSO);
                    break;
                case "navItemBack":
                    GetBackSelectedItems(gvSelectedItem, gvSO);
                    break;
            }
        }


        public Int32 GetBackSelectedItems(DataGridView dgvget, DataGridView dgvpost)
        {
            int negaQty = 0;
            int i = dgvpost.Rows.Count;
            string strExcludeItems = "";

            if (dgvget.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in dgvget.SelectedRows)
                {
                    dgvget.Rows.RemoveAt(dr.Index);
                }

                foreach (DataGridViewRow dr in dgvget.Rows)
                {
                    if (strExcludeItems == "")
                    {
                        strExcludeItems = "'" + dr.Cells[1].Value.ToString() + "'";
                    }
                    else
                    {
                        strExcludeItems += ",'" + dr.Cells[1].Value.ToString() + "'";
                    }
                }
                LoadCurrentRecords("N", null, null, strExcludeItems);

                foreach (DataGridViewRow row1 in dgvget.Rows)
                {
                    row1.HeaderCell.Value = String.Format("{0}", row1.Index + 1);
                }
                dgvget.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            }

            return negaQty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            ClearFilter(1, 1, 1, 1, 1, 1, 1, 1, 1);
            txtBrandCode.Clear();
            txtBrandDesc.Clear();
            LoadCurrentRecords("N");
        }


        public string oDocEntry { get; set; }

     

        public string oTable { get; set; }
        public List<string> oSelectedItems { get; set; }

        int ColumnIndex = 1;

        private void btnSize_Click(object sender, EventArgs e)
        {
            LoadSelectionOfSizes();
        }

        private void LoadSelectionOfSizes()
        {
            ViewList("@OSZC", out oCode, out oName, "List of Sizes", "DGV");
            txtSizeCode.Text = oCode;
            txtSizeDesc.Clear();

            if (oCode != null)
            {
                txtSizeDesc.Text = oName;
                ClearFilter(0, 0, 0, 0, 0, 1);
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }

        }

        private void btnSubSize_Click(object sender, EventArgs e)
        {
            LoadSelectionOfSubSizes();
        }

        private void LoadSelectionOfSubSizes()
        {
            if (txtSizeCode.Text != "")
            {
                ViewList("@OSZS", out oCode, out oName, "List of Sub Sizes", "DGV", txtSizeCode.Text);
                txtSubSizeCode.Text = oCode;
                txtSubSizeDesc.Clear();
            }
            else
            {
                //frmMain.NotiMsg("Please select a Size Category before using this filter.", Color.Red);
                ViewList("@OSZS_NoFilter", out oCode, out oName, "List of Sub Sizes", "DGV");
                txtSubSizeCode.Text = oCode;
                txtSubSizeDesc.Clear();
            }

            if (oCode != null)
            {
                txtSubSizeDesc.Text = oName;
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            LoadSelectionOfDepts();
        }

        private void LoadSelectionOfDepts()
        {
            if (txtBrandCode.Text != "")
            {
                ViewList("@ODPT", out oCode, out oName, "List of Departments", "DGV", txtBrandCode.Text);
                txtDepCode.Text = oCode;
                txtDepDesc.Clear();
            }
            else
            {
                //frmMain.NotiMsg("Please select a Brand before using this filter.", Color.Red);
                ViewList("@ODPT_NoFilter", out oCode, out oName, "List of Departments", "DGV");
                txtDepCode.Text = oCode;
                txtDepDesc.Clear();
            }

            if (oCode != null)
            {
                txtDepDesc.Text = oName;
                ClearFilter(0, 1, 1, 1);
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void btnSubDep_Click(object sender, EventArgs e)
        {
            LoadSelectionOfSubDepts();
        }

        private void LoadSelectionOfSubDepts()
        {
            ViewList("@OSDP", out oCode, out oName, "List of Sub-Departments", "DGV", txtBrandCode.Text, txtDepCode.Text);
            txtSubDepCode.Text = oCode;
            txtSubDepDesc.Clear();

            if (oCode != null)
            {
                txtSubDepDesc.Text = oName;
                ClearFilter(0, 0, 1, 1);
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void btnColorCode_Click(object sender, EventArgs e)
        {
            LoadSelectionOfColors();
        }

        private void LoadSelectionOfColors()
        {
            ViewList("@OCLC", out oCode, out oName, "List of Colors", "DGV");
            txtColorCode.Text = oCode;
            txtColorDesc.Clear();

            if (oCode != null)
            {
                txtColorDesc.Text = oName;
                ClearFilter(0, 0, 0, 0, 0, 0, 0, 1);
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void btnSubColCode_Click(object sender, EventArgs e)
        {
            LoadSelectionOfSubColors();
        }

        private void LoadSelectionOfSubColors()
        {
            if (txtColorCode.Text != "")
            {
                ViewList("@OCLR", out oCode, out oName, "List of Child Colors", "DGV", txtColorDesc.Text);
                txtSubColCode.Text = oCode;
                txtSubColDesc.Clear();
            }
            else
            {
                //frmMain.NotiMsg("Please select a Color before using this filter.", Color.Red);
                ViewList("@OCLR_NoFilter", out oCode, out oName, "List of Child Colors", "DGV");
                txtSubColCode.Text = oCode;
                txtSubColDesc.Clear();
            }

            if (oCode != null)
            {
                txtSubColDesc.Text = oName;
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void btnCategoryCode_Click(object sender, EventArgs e)
        {
            LoadSelectionOfCategories();
        }

        private void LoadSelectionOfCategories()
        {
            ViewList("@OCTG", out oCode, out oName, "List of Categories", "DGV", txtBrandCode.Text, txtDepCode.Text, txtSubDepCode.Text);
            txtCatCode.Text = oCode;
            txtCatDesc.Clear();
            if (oCode != null)
            {
                txtCatDesc.Text = oName;
                ClearFilter(0, 0, 0, 1);
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void btnSubCatCode_Click(object sender, EventArgs e)
        {
            LoadSelectionOfSubCategories();
        }

        private void LoadSelectionOfSubCategories()
        {
            ViewList("@OSBC2", out oCode, out oName, "List of Sub Categories", "DGV", txtBrandCode.Text, txtCatCode.Text);
            txtSubCatCode.Text = oCode;
            txtSubCatDesc.Clear();

            if (oCode != null)
            {
                txtSubCatDesc.Text = oName;
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void btnStyle_Click(object sender, EventArgs e)
        {
            LoadSelectionOfStyles();
        }

        private void LoadSelectionOfStyles()
        {
            ViewList("@OSTL", out oCode, out oName, "List of Styles", "DGV", txtBrandCode.Text, txtCatCode.Text, txtSubCatCode.Text);
            txtStyleCode.Text = oCode;
            txtStyleDesc.Clear();

            if (oCode != null)
            {
                txtStyleDesc.Text = oName;
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void pbSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Contains("*") == false)
            {
                SearchFunction(sender, e);
            }
            else
            {
                LoadCurrentRecords("N");
            }
        }


        private void SearchFunction(object sender, EventArgs e)
        {
            try
            {

                if (SearchEvent == false)
                {
                    if (txtSearch.Text != "")
                    {
                        PgSize = TotalDataCount;
                        SearchEvent = true;
                        LoadCurrentRecords("Y", sender, e);
                    }
                    else
                    {
                        SearchEvent = false;
                        LoadCurrentRecords("N");
                    }
                }
                else
                {
                    SearchEvent = false;
                    StaticHelper._MainForm.ShowMessage("Please search again after loading of data finishes.", true);
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private void navItemGetAll_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "navItemGet":
                    soic.GetSelectedItems(gvSO, gvSelectedItem);
                    break;
                case "navItemGetAll":
                    soic.GetAllSelectedItems(gvSO, gvSelectedItem);
                    break;
                case "navItemBackAll":
                    soic.GetAllSelectedItems(gvSelectedItem, gvSO);
                    break;
                case "navItemBack":
                    GetBackSelectedItems(gvSelectedItem, gvSO);
                    break;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (gvSO.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in gvSO.Rows)
                {
                    try
                    {
                        if (row.Cells[defaultColumn].Value.ToString().ToUpper().StartsWith(txtSearch.Text.Replace("\r\n", "").ToUpper()))
                        {
                            row.Selected = true;
                            gvSO.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                        else
                        {
                            row.Selected = false;
                        }
                    }
                    catch (Exception ex)
                    { }
                }
            }
        }

        private void txtSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtSearch.Focused == true && gvSelectedItem.Focused == false)
            {
                pbSearch_Click(sender, e);

                if (gvSO.Rows.Count > 0)
                {
                    //SelectItemList(false);

                    gvSO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    gvSO.Focus();
                }

                strPreviousEvent = "txtSearch_PreviewKeyDown";
            }
        }

        private void gvSO_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        private void navItemBack_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "navItemGet":
                    soic.GetSelectedItems(gvSO, gvSelectedItem);
                    break;
                case "navItemGetAll":
                    soic.GetAllSelectedItems(gvSO, gvSelectedItem);
                    break;
                case "navItemBackAll":
                    soic.GetAllSelectedItems(gvSelectedItem, gvSO);
                    break;
                case "navItemBack":
                    GetBackSelectedItems(gvSelectedItem, gvSO);
                    break;
            }
        }

        private void gvSO_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                gvSO.CurrentRow.Selected = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                soic.GetSelectedItems(gvSO, gvSelectedItem);
            }
        }

        private void navItemBackAll_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "navItemGet":
                    soic.GetSelectedItems(gvSO, gvSelectedItem);
                    break;
                case "navItemGetAll":
                    soic.GetAllSelectedItems(gvSO, gvSelectedItem);
                    break;
                case "navItemBackAll":
                    soic.GetAllSelectedItems(gvSelectedItem, gvSO);
                    break;
                case "navItemBack":
                    GetBackSelectedItems(gvSelectedItem, gvSO);
                    break;
            }
        }

        string ColumnName = "T0.ItemCode";
        
     

        public void LoadCurrentRecords([Optional] string strSearch, [Optional] object sender, [Optional] EventArgs e, [Optional] string strExcludeItems)
        {
            try
            {
                lblPageSize.Visible = true;
                lblPage.Visible = true;
                lblPage.Text = CurrentPageIndex.ToString();

                gvSO.DataSource = GetCurrentRecords(CurrentPageIndex, txtBrandCode.Text, txtColorCode.Text, txtSizeCode.Text, txtStyleCode.Text
                                , txtCatCode.Text, txtSubCatCode.Text, txtDepCode.Text, txtSubDepCode.Text, txtSubColDesc.Text, txtSubSizeCode.Text, strSearch, strExcludeItems);
                soic.dgvSetup(gvSO);
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message);
            }
        }


        private DataTable GetCurrentRecords(long page, string strBrand, string strColor, string strSize, string strStyle, string strCat, string strSubCat
                                        , string strDept, string strSubDept, string SubColor, string strSubSize, string frmgvSO, string strExludeItems)
        {
            DataTable dt = new DataTable();
            SAPHanaAccess sapHana = new SAPHanaAccess();
            DataHelper help = new DataHelper();

            string query = null;

            lblPageSize.Text = string.Empty;

            string SelectedField = "BarCode";

            if (frmgvSO == "Y" || txtSearch.Text.Contains("*"))
            {
                if (defaultColumn == 5 || defaultColumn == 7)
                {
                    string ColName = gvSO.Columns[defaultColumn].HeaderText.ToString();
                    SelectedField = ColName == "Item No." ? "ItemCode" : "ItemName";
                }
            }

            query = "SELECT *, MT1.GrossPriceBase * (1 +'.'+ Cast(MT1.TaxRate as INT)) [GrossPrice]  FROM ( " +
                    $" SELECT TOP {PgSize} " +
                    " T0.U_ID023 " +
                    ", T0.U_ID025 [Style]" +
                    ", T0.U_ID011 [Color]" +
                    ", T0.U_ID018 [Section]" +
                    ", T0.U_ID007 [Size] " +
                    ", T0.ItemCode" +
                    ", T0.CodeBars [BarCode]" +
                    ", T0.ItemName" +
                    ", ISNULL((select OnHand - IsCommited + OnOrder from OITW where ItemCode = T1.ItemCode and WhsCode = '" + InvoiceHeaderModel.oWhsCode + "'), 0) [Available] " +
                    ", '" + InvoiceHeaderModel.oWhsCode + "' [Warehouse]";
            
            int iPriceList = int.Parse(help.ReadDataRow(sapHana.Get($"SELECT ISNULL(ListNum,1) FROM OCRD WHERE CardCode = '{InvoiceHeaderModel.oBPCode}'"), 0, "", 0));

            //int iPriceDisc = 1;

            //if (SalesOrder.oItmsGrpCod != "" && SalesOrder.oItmsGrpCod != null)
            //{
            //    iPriceList = 2;
            //    iPriceDisc = 3;
            //}

            if (InvoiceHeaderModel.oBPCode != "")
            {
                query += $" , CASE WHEN ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T0.ItemCode and z.CardCode = '{InvoiceHeaderModel.oBPCode}'),0) = 0) " +
                        $" THEN ISNULL((select Price from ITM1 where ItemCode = T0.ItemCode and PriceList = '2'), 0) " +
                        $" ELSE ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T0.ItemCode and z.CardCode = '{InvoiceHeaderModel.oBPCode}'),0) [EffectivePrice] ";

                query += $" , CASE WHEN ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T0.ItemCode and z.CardCode = '{InvoiceHeaderModel.oBPCode}'),0) = 0) " +
                        $" THEN ISNULL((select Price from ITM1 where ItemCode = T0.ItemCode and PriceList = '{iPriceList}'), 0) " +
                        $" ELSE ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T0.ItemCode and z.CardCode = '{InvoiceHeaderModel.oBPCode}'),0) [GrossPriceBase] ";
            }
            else
            {
                query += $", ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '2'), 0) [EffectivePrice] ";
                query += $", ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '{iPriceList}'), 0) [GrossPriceBase] ";
            }

            query += $" , ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '{iPriceList}'), 0) [UnitPrice] " +
                    $" , '{InvoiceHeaderModel.oTaxGroup}' [Tax Code]" +
                    $", (select Rate from OVTG Where Code = '" + InvoiceHeaderModel.oTaxGroup + "' ) [TaxRate] " +
                    " , '0' [Tax Amount] " +
                    " , '0' [Line Total] ";

            if (InvoiceHeaderModel.oBPCode != "")
            {
                query += $" , CASE WHEN ISNULL((SELECT Discount from OSPP where CardCode = '{InvoiceHeaderModel.oBPCode}' and ItemCode = T0.ItemCode),0) = 0) " +
                        $" THEN ISNULL((select 100 - (Factor*100) [Discount] from OPLN where ListNum = '{iPriceList}'), 0) " +
                        $" ELSE ISNULL((SELECT Discount from OSPP where CardCode = '{InvoiceHeaderModel.oBPCode}' and ItemCode = T0.ItemCode),0) [Discount] ";
            }
            else
            {
                query += ", '0' [Discount]";
            }

            query += " FROM OITM T0 " +
                    " INNER JOIN OITW T1 ON T0.ItemCode = T1.ItemCode " +
                    $" INNER JOIN ITM1 T2 ON T0.ItemCode = T2.ItemCode and T2.PriceList = '{iPriceList}' " +
                    " WHERE T0.ItmsGrpCod != 101 AND T0.SellItem = 'Y'";
            //" WHERE T1.ItemCode not like 'FA%' ";

            if (strBrand != string.Empty)
            {
                query += "and T0.U_ID001 = '" + strBrand + "' ";
            }

            if (strColor != string.Empty)
            {
                query += " and T0.U_ID022 = '" + strColor + "' ";
            }

            if (strSize != string.Empty)
            {
                query += " and T0.U_ID006 = '" + strSize + "' ";
            }

            if (strStyle != string.Empty)
            {
                query += " and T0.U_ID025 = '" + strStyle + "' ";
            }

            if (strCat != string.Empty)
            {
                query += " and T0.U_ID020 = '" + strCat + "' ";
            }

            if (strSubCat != string.Empty)
            {
                query += " and T0.U_ID021 = '" + strSubCat + "' ";
            }

            if (strDept != string.Empty)
            {
                query += " and T0.U_ID002 = '" + strDept + "' ";
            }

            if (strSubDept != string.Empty)
            {
                query += " and T0.U_ID003 = '" + strSubDept + "' ";
            }

            if (SubColor != string.Empty)
            {
                query += " and T0.U_ID011 = '" + SubColor + "' ";
            }

            if (strSubSize != string.Empty)
            {
                query += " and T0.U_ID008 = '" + strSubSize + "' ";
            }

            if (strExludeItems != null && strExludeItems != "")
            {
                query += $" and T1.ItemCode NOT IN({strExludeItems}) ";
            }

            string strSearch = txtSearch.Text;
            if (strSearch != "" && SearchEvent == true)
            {
                query += "GROUP BY T0.U_ID025, T0.U_ID011, T0.U_ID018, T0.U_ID007, T0.ItemCode, T0.CodeBars, T0.ItemName" +
                         ", T1.ItemCode, T0.U_ID023 ORDER BY T0.U_ID023) MT1 ";
                         //$" WHERE MT1.{SelectedField} like '" + strSearch + "%' ";

                if (SelectedField == "BarCode")
                {
                    query += $" WHERE MT1.{SelectedField} like '" + strSearch + "%' OR MT1.ItemCode LIKE '" + strSearch + "%'";
                }
                else
                {
                    query += $" WHERE MT1.{SelectedField} like '" + strSearch + "%' ";
                }

                query += " ORDER BY MT1.U_ID023";
            }
            else if (strSearch != "" && SearchEvent == false && strSearch.Contains("*") && txtSearch.Focused == true && gvSelectedItem.Focused == false)
            {
                query += "GROUP BY T0.U_ID025, T0.U_ID011, T0.U_ID018, T0.U_ID007 , T0.ItemCode, T0.CodeBars, T0.ItemName " +
                         ", T1.ItemCode, T0.U_ID023 ORDER BY T0.U_ID023) MT1 ";

                var index = strSearch.IndexOf(@"*");
                var strLength = strSearch.Length - 1;
                string[] array = strSearch.Split('*');

                if (index == strLength)
                {
                    query += $"WHERE MT1.{SelectedField} LIKE '{array[0]}%' ";
                }
                else if (index == 0)
                {
                    query += $"WHERE MT1.{SelectedField} LIKE '%{array[1]}' ";
                }
                else
                {
                    query += $"WHERE MT1.{SelectedField} LIKE '{array[0]}%{array[1]}' ";
                }
                query += " ORDER BY MT1.U_ID023";
            }
            else
            {
                query += " GROUP BY T0.U_ID025, T0.U_ID011, T0.U_ID018, T0.U_ID007, T0.ItemCode, T0.CodeBars, T0.ItemName" +
                         ", T1.ItemCode, T0.U_ID023 ORDER BY T0.U_ID023) MT1 ";
            }

            if (page != 1)
            {
                long PreviousPageOffSet = (page - 1) * DefaultPgSize;

                query += $" WHERE MT1.No NOT IN(SELECT TOP " + PreviousPageOffSet.ToString() + " ROW_NUMBER() OVER (ORDER BY U_ID023) [No] FROM OITM ORDER BY U_ID023) ORDER BY MT1.U_ID023";
            }

            dt = sapHana.Get(query);

            gvSO.Columns.Clear();

            lblPageSize.Text = $"Page Size - {dt.Rows.Count}";

            return dt;
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddItems();
        }


        private void AddItems()
        {
            try
            {
                if (gvSelectedItem.Rows.Count > 0)
                {
                    if (AddItem() == true)
                    {
                        Invoice.RefreshData();
                        this.Close();
                    }
                }
                else
                {
                    frmMain.ShowMessage("Please select an item(s) first before adding.", false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Boolean AddItem()
        {
            bool result = false;
            SAPHanaAccess sapHana = new SAPHanaAccess();
            DataHelper help = new DataHelper();
            try
            {

                foreach (DataGridViewRow row in gvSelectedItem.Rows)
                {
                    var getDetails = help.ReadDataRow(sapHana.Get(SP.AW_GetItemDetails), 1, "", 0);
                    var dt = sapHana.Get(string.Format(getDetails, row.Cells[4].Value.ToString()));

                    double dblDiscPerc = Convert.ToDouble(DECLARE.Replace(row, "Employee Discount", "0"));
                    double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0"));
                    double dblDiscAmt = (dblUnitPrice / 100) * dblDiscPerc;

                    if ((row.Cells["Quantity"].Value != null))
                    {
                        string strItemCode = row.Cells[4].Value.ToString();
                        bool AllowDupItem = true;

                        if (SOm.SelValue("AllowDupItems", InvoiceHeaderModel.oDocType) != "Y" && InvoiceItemsModel.InvoiceItems.Where(x => x.ItemCode == strItemCode).ToList().Count() >= 1)
                        {
                            AllowDupItem = false;
                        }

                        if (AllowDupItem)
                        {
                            if (InvoiceItemsModel.InvoiceItems.Count > 0)
                            {
                                if (InvoiceItemsModel.InvoiceItems.Select(x => x.Linenum).Max() != 0)
                                {
                                    index = InvoiceItemsModel.InvoiceItems.Select(x => x.Linenum).Max() + 1;
                                }
                            }

                            int iIndex = index++;
                            string GetCompany = hana.Get(Query.GetCompanyPerLine(InvoiceHeaderModel.oBPCode, strItemCode)).Rows.Count > 0 ? hana.Get(Query.GetCompanyPerLine(InvoiceHeaderModel.oBPCode, strItemCode)).Rows[0].ItemArray[0].ToString() : "";
                            string company = GetCompany != "" ? hana.Get(Query.GetCompanyQuerySearch(GetCompany)).Rows[1].ItemArray[1].ToString() : "";
                            InvoiceItemsModel.InvoiceItems.Add(new InvoiceItemsModel.InvoiceItemsData
                            {
                                Linenum = iIndex,
                                ObjType = FrmARInvoice.objType, //ObjType
                                Style = row.Cells[0].Value.ToString(), //Style
                                Color = row.Cells[1].Value.ToString(), //Color
                                Section = row.Cells[2].Value.ToString(), //Section
                                Size = row.Cells[3].Value.ToString(),
                                ItemCode = strItemCode, // ItemCode
                                Brand = LibraryHelper.DataTableRet(dt, 0, "Brand", ""),
                                BarCode = DECLARE.Replace(row, "Barcode", ""),
                                ItemName = row.Cells[6].Value.ToString(),
                                EffectivePrice = Convert.ToDouble(DECLARE.Replace(row, "Effective Price", "0.00")),
                                GrossPrice = Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0.00")),
                                DiscountPerc = 0,//C
                                DiscountAmount = 0,
                                EmpDiscountPerc = dblDiscPerc,
                                Quantity = Convert.ToInt32(row.Cells[7].Value.ToString()), //Qty
                                FWhsCode = /*row.Cells["Warehouse"].Value.ToString()*/ Invoice.Warehouse, //WHsCode
                                TaxCode = row.Cells["Tax Code"].Value.ToString(), //Tax Code
                                TaxAmount = Convert.ToDouble(DECLARE.Replace(row, "Tax Amount", "0.00")),//C
                                TaxRate = Convert.ToDouble(DECLARE.Replace(row, "Tax Rate", "0.00")),//C
                                UnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0.00")),//C
                                LineTotal = Convert.ToDouble(DECLARE.Replace(row, "Line Total", "0.00")),//C
                                PriceAfterDisc = Convert.ToDouble(DECLARE.Replace(row, "PriceAfterDisc", "0.00")),
                                GrossTotal = Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0.00"))
                                //DiscountAmount = Convert.ToDouble(DECLARE.Replace(row, "Discount Amt", "0")),  
                                ,Company = company

                            });
                        }
                        else
                        {
                            frmMain.ShowMessage("Duplicate item cannot be added, please check SAP Document Type maintenance.", false);
                        }
                    }
                    //}
                }

                frmMain.ShowMessage("Item(s) added", false);
                //CLear Data
                gvSelectedItem.Columns.Clear();
                txtSizeCode.Text = "";


                result = true;

                return result;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                return result;
            }
        }



        private void ClearFilter([Optional] int Dep, [Optional] int SubDep, [Optional] int Cat, [Optional] int SubCat
                              , [Optional] int Size, [Optional] int SubSize, [Optional] int Color, [Optional] int SubColor
                              , [Optional] int Style)
        {
            if (Dep == 1)
            { txtDepCode.Clear(); txtDepDesc.Clear(); }
            if (SubDep == 1)
            { txtSubDepCode.Clear(); txtSubDepDesc.Clear(); }
            if (Cat == 1)
            { txtCatCode.Clear(); txtCatDesc.Clear(); }
            if (SubCat == 1)
            { txtSubCatCode.Clear(); txtSubCatDesc.Clear(); }
            if (Size == 1)
            { txtSizeCode.Clear(); txtSizeDesc.Clear(); }
            if (SubSize == 1)
            { txtSubSizeCode.Clear(); txtSubSizeDesc.Clear(); }
            if (Color == 1)
            { txtColorCode.Clear(); txtColorDesc.Clear(); }
            if (SubColor == 1)
            { txtSubColCode.Clear(); txtSubColDesc.Clear(); }
            if (Style == 1)
            { txtStyleCode.Clear(); txtStyleDesc.Clear(); }
        }

        private void CalculateTotalPages()
        {
            //string query = "SELECT Count(ItemCode) [Count] from OITM where ItemCode not like 'FA%'";
            var query = "SELECT Count(ItemCode) [Count] from OITM where ItmsGrpCod != 101 AND SellItem = 'Y'";
            DataTable dt = new DataTable();

            SAPHanaAccess sapHana = new SAPHanaAccess();
            DataHelper help = new DataHelper();
            dt = sapHana.Get(query);
            
            Int64 rowCount = Convert.ToInt64(help.ReadDataRow(dt, "Count", "", 0));
            TotalDataCount = Convert.ToInt32(rowCount);
            TotalPage = rowCount / DefaultPgSize;

            if (rowCount % DefaultPgSize > 0)
            {
                TotalPage += 1;
            }
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            LoadSelectionOfBrands();
        }

        private void LoadSelectionOfBrands()
        {
            ViewList("@BRAND", out oCode, out oName, "List of Brands", "DGV");

            txtBrandCode.Text = oCode;
            txtBrandDesc.Clear();

            if (oCode != null)
            {
                txtBrandDesc.Text = oName;
                ClearFilter(1, 1, 1, 1);
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }


        private void ViewList(string SearchTable
                         , out string Code
                         , out string Name
                         , string title
                         , string Focus
                         , [Optional] string Param1
                         , [Optional] string Param2
                         , [Optional] string Param3
                         , [Optional] string Param4
                         )
        {
            var fS = new frmSearch2();
            fS.oSearchMode = SearchTable;
            frmSearch2.Param1 = Param1;
            frmSearch2.Param2 = Param2;
            frmSearch2.Param3 = Param3;
            frmSearch2.Param4 = Param4;
            frmSearch2._title = title;
            fS.oFormTitle = title;
            frmSearch2.oFocus = Focus;
            fS.ShowDialog();

            Code = fS.oCode;
            Name = fS.oName;
        }
    }
}

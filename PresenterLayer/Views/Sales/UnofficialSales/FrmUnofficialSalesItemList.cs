using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using PresenterLayer.Views.Main;
using DirecLayer;
using System.Runtime.InteropServices;
using PresenterLayer.Helper;
using PresenterLayer.Helper.Unofficial_Sales;
using System.Threading;
using zDeclare;

namespace PresenterLayer.Views
{
    public partial class FrmUnofficialSalesItemList : MetroForm
    {
        private UnofficialSalesItemsController usic = new UnofficialSalesItemsController();

        MainForm frmMain;
        private FrmUnofficialSales frmUnofficialSales;
        DataAccess da = new DataAccess();
        DataTable dt = new DataTable();
        DataTable dtable = new DataTable();
        private string table { get; set; }

        Thread gvSiThread;
        DataTable dtGCR = new DataTable();

        public List<string> oCodeList = new List<string>();

        private string oCode, oName, _StyleCode;
        private static int defaultColumn = 6, _rowIndex = 0;

        public static string oStyleCode, oColorCode, oSection, oBPCode, oWhsCode;
        public static DateTime oDocDate;

        private long DefaultPgSize = 50000;
        private long PgSize = 0;
        private long CurrentPageIndex = 1;
        private long TotalPage = 0;
        int TotalDataCount = 0;
        int CurrentDataCount = 0;
        bool SearchEvent = false;
        int SearchRowCnt = 0;

        private string strPreviousEvent = "";

        public FrmUnofficialSalesItemList([Optional] FrmUnofficialSales frmUnofficialSales, [Optional] MainForm frmMain)
        {
            InitializeComponent(); ;
            this.frmUnofficialSales = frmUnofficialSales;
            this.frmMain = frmMain;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            CalculateTotalPages();
            defaultColumn = 6;
        }

        private void frmSI_items_new_Load(object sender, EventArgs e)
        {
            usic.dgvSetup(gvSelectedItem);
            PgSize = DefaultPgSize;
            LoadCurrentRecords();
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
                gvUOS.Focus();
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
                if (gvUOS.Focused == true && strPreviousEvent == "")             //Transfer items from List to Selected Items Table
                {
                    int intNegaQty = 0;
                    intNegaQty = usic.GetSelectedItems(gvUOS, gvSelectedItem);
                }
                else if (gvSelectedItem.Focused == true && strPreviousEvent == "")        //Transfer items from Selected Items Table to the List
                {
                    GetBackSelectedItems(gvSelectedItem, gvUOS);
                }

                strPreviousEvent = strPreviousEvent != "" ? "" : strPreviousEvent;
            }

            else if (keyData == (Keys.Alt | Keys.Q) && gvSelectedItem.Focused == true)
            {
                if (gvSelectedItem.Rows.Count > 0)
                {
                    int index = gvSelectedItem.CurrentRow.Index;
                    gvSelectedItem.CurrentCell = gvSelectedItem[3, index];
                    gvSelectedItem[3, index].Selected = true;
                    gvSelectedItem.BeginEdit(true);
                }
                else
                {
                    frmMain.ShowMessage("No items to set Quantity.", true);
                }
            }

            //else if (keyData == (Keys.Alt | Keys.A))
            //{
            //    AddItems();
            //}

            else if (keyData == Keys.Escape)
            {
                Close();
            }

            return base.ProcessCmdKey(ref msg, keyData);
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

        public void LoadCurrentRecords([Optional] string strSearch, [Optional] object sender, [Optional] EventArgs e, [Optional] string strExcludeItems)
        {

            //dtGCR = GetCurrentRecords(CurrentPageIndex, txtBrandCode.Text, txtColorCode.Text, txtSizeCode.Text, txtStyleCode.Text
            //        , txtCatCode.Text, txtSubCatCode.Text, txtDepCode.Text, txtSubDepCode.Text, txtSubColDesc.Text, txtSubSizeCode.Text, "N");

            try
            {
                lblPageSize.Visible = true;
                lblPage.Visible = true;
                lblPage.Text = CurrentPageIndex.ToString();
                LoadingData(3);
                gvUOS.DataSource = GetCurrentRecords(CurrentPageIndex, txtBrandCode.Text, txtColorCode.Text, txtSizeCode.Text, txtStyleCode.Text
                                , txtCatCode.Text, txtSubCatCode.Text, txtDepCode.Text, txtSubDepCode.Text, txtSubColDesc.Text, txtSubSizeCode.Text, strSearch, strExcludeItems);
                usic.dgvSetup(gvUOS);
                StaticHelper._MainForm.ProgressClear();
                //SalesInvoiceHeaderModel.oTaxGroup;

                //gvSiThread = new Thread(new ThreadStart(
                //() =>
                //{
                //    try
                //    {
                //        LoadDataWithThreading();
                //    }
                //    finally
                //    {
                //        if (strSearch == "Y")
                //        {
                //            gvUOS.Invoke(new Action(() => { txtSearch_TextChanged(sender, e); SearchClick = false; }));
                //        }
                //    }
                //}
                //));
                //gvSiThread.Start();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage("No data(s) record.", true);
            }
        }

        void LoadingData(int seconds)
        {
            StaticHelper._MainForm.Progress("Please wait...", 100, 0);
            int max = seconds;
            for (int min = 0; min < max; min++)
            {
                Thread.Sleep(1000);
                StaticHelper._MainForm.Progress($"Please wait until all data are loaded.", min, max);
            }
        }

        public void LoadDataWithThreading()
        {
            CurrentDataCount = 0;
            foreach (DataRow row1 in dtGCR.Rows)
            {

                object[] lineitem = { row1["Style"].ToString(), row1["Color"].ToString(), row1["Section"].ToString(), row1["Size"].ToString()
                                    , row1["ItemCode"].ToString(), row1["BarCode"].ToString(), row1["ItemName"].ToString(), row1["Available"].ToString()
                                    , row1["Warehouse"].ToString(), row1["GrossPrice"].ToString(), row1["UnitPrice"].ToString(), InvoiceHeaderModel.oTaxGroup
                                    , row1["TaxRate"].ToString(), 0, 0, row1["Discount"].ToString(), row1["GrossPriceBase"].ToString()
                                    };
                try
                {
                    gvUOS.Invoke(new Action(() => { gvUOS.Rows.Add(lineitem); }));
                    lblPageSize.Invoke(new Action(() => { lblPageSize.Text = "Wait to finish loading of data: count " + gvUOS.Rows.Count.ToString(); }));
                    gvUOS.Invoke(new Action(() => {
                        foreach (DataGridViewRow row2 in gvUOS.Rows)
                        {
                            row2.HeaderCell.Value = String.Format("{0}", row2.Index + 1);
                        }

                        gvUOS.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                    }));
                }
                catch (Exception ex)
                {

                }

                CurrentDataCount += 1;
            }

            Thread.Sleep(5000);
        }


        private void btnBrand_Click(object sender, EventArgs e)
        {

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

        private void btnDept_Click(object sender, EventArgs e)
        {

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

        }

        private void LoadSelectionOfSubDepts()
        {
            //if (txtBrandCode.Text != "" && txtDepCode.Text != "")
            //{
            ViewList("@OSDP", out oCode, out oName, "List of Sub-Departments", "DGV", txtBrandCode.Text, txtDepCode.Text);
            txtSubDepCode.Text = oCode;
            txtSubDepDesc.Clear();
            //}
            //else
            //{
            //    //frmMain.NotiMsg("Please select a Brand and Department before using this filter.", Color.Red);
            //    ViewList("@OSDP_NoFilter", out oCode, out oName, "List of Sub-Departments", "DGV");
            //    txtSubDepCode.Text = oCode;
            //    txtSubDepDesc.Clear();
            //}

            if (oCode != null)
            {
                txtSubDepDesc.Text = oName;
                ClearFilter(0, 0, 1, 1);
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void btnCategoryCode_Click(object sender, EventArgs e)
        {

        }

        private void LoadSelectionOfCategories()
        {
            //if (txtBrandCode.Text != "" && txtDepCode.Text != "" && txtSubDepCode.Text != "")
            //{
            ViewList("@OCTG", out oCode, out oName, "List of Categories", "DGV", txtBrandCode.Text, txtDepCode.Text, txtSubDepCode.Text);
            txtCatCode.Text = oCode;
            txtCatDesc.Clear();
            //}
            //else
            //{
            //    //frmMain.NotiMsg("Please select a Brand, Department and Sub-Department before using this filter.", Color.Red);
            //    ViewList("@OCTG_NoFilter", out oCode, out oName, "List of Categories", "DGV");
            //    txtCatCode.Text = oCode;
            //    txtCatDesc.Clear();
            //}

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

        }

        private void LoadSelectionOfSubCategories()
        {
            //if (txtBrandCode.Text != "" && txtCatCode.Text != "")
            //{
            ViewList("@OSBC2", out oCode, out oName, "List of Sub Categories", "DGV", txtBrandCode.Text, txtCatCode.Text);
            txtSubCatCode.Text = oCode;
            txtSubCatDesc.Clear();
            //}
            //else
            //{
            //    //frmMain.NotiMsg("Please select a Brand and Category before using this filter.", Color.Red);
            //    ViewList("@OSBC2_NoFilter", out oCode, out oName, "List of Sub Categories", "DGV");
            //    txtSubCatCode.Text = oCode;
            //    txtSubCatDesc.Clear();
            //}

            if (oCode != null)
            {
                txtSubCatDesc.Text = oName;
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
            }
        }

        private void btnSize_Click(object sender, EventArgs e)
        {

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

        private void btnColorCode_Click(object sender, EventArgs e)
        {

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

        private void btnStyleCode_Click(object sender, EventArgs e)
        {

        }
        private void LoadSelectionOfStyles()
        {
            //if (txtBrandCode.Text != "" && txtCatCode.Text != "" && txtSubCatCode.Text != "")
            //{
            ViewList("@OSTL", out oCode, out oName, "List of Styles", "DGV", txtBrandCode.Text, txtCatCode.Text, txtSubCatCode.Text);
            txtStyleCode.Text = oCode;
            txtStyleDesc.Clear();
            //}
            //else
            //{
            //    //frmMain.NotiMsg("Please select a Brand, Category and Sub-Category before using this filter.", Color.Red);
            //    ViewList("@OSTL_NoFilter", out oCode, out oName, "List of Styles", "DGV");
            //    txtStyleCode.Text = oCode;
            //    txtStyleDesc.Clear();
            //}

            if (oCode != null)
            {
                txtStyleDesc.Text = oName;
                PgSize = DefaultPgSize;
                CurrentPageIndex = 1;
                LoadCurrentRecords();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void AddItems()
        {
            try
            {
                if (gvSelectedItem.Rows.Count > 0)
                {
                    if (AddItemNew() == true)
                    {
                       // frmUnofficialSales.RefreshData();
                        this.Close();
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Please select an item(s) first before adding.", true);
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage("Please select an item(s) first before adding.", true);
            }
        }

        private Boolean AddItemNew()
        {
            bool result = false;
            try
            {
                oCodeList.Clear();
                foreach (DataGridViewRow row in gvSelectedItem.Rows)
                {
                    oCodeList.Add(row.Cells[4].Value.ToString());
                }
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                result = false;
                return result;
            }
        }

        private void gvUOS_Click(object sender, EventArgs e)
        {
            SelectItemList(false);
        }

        private Boolean AddItem()
        {
            bool result = false;
            try
            {

                foreach (DataGridViewRow row in gvSelectedItem.Rows)
                {
                    if (InvoiceItemsModel.InvoiceItems.Exists(a => a.ItemCode == row.Cells[4].Value.ToString()))
                    {
                        //If exist add update qty
                        foreach (var x in InvoiceItemsModel.InvoiceItems.Where(a => a.ItemCode == row.Cells[4].Value.ToString() && a.ObjType == frmUnofficialSales.objType))
                        {
                            double dblDiscPerc = Convert.ToDouble(DECLARE.Replace(row, "Discount", "0:.##"));
                            double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0:.##"));
                            double dblDiscAmt = (dblUnitPrice / 100) * dblDiscPerc;

                            x.ObjType = frmUnofficialSales.objType; //ObjType
                            x.Style = txtStyleCode.Text; //Style
                            x.Color = txtColorCode.Text; //Color
                            x.Section = txtSizeCode.Text; //Section
                            x.Size = DECLARE.Replace(row, "Size", "0");
                            x.ItemCode = row.Cells[4].Value.ToString(); // ItemCode
                            x.BarCode = row.Cells[5].Value.ToString(); // BarCode
                            x.ItemName = row.Cells[6].Value.ToString();
                            x.GrossPrice = Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0:.##"));
                            x.UnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0:.##"));
                            x.DiscountPerc = dblDiscPerc;
                            x.DiscountAmount = dblDiscAmt;
                            x.Quantity = Convert.ToInt32(row.Cells[7].Value.ToString()); ; //Qty
                            x.FWhsCode = row.Cells["Warehouse"].Value.ToString(); //WHsCode
                            x.TaxCode = row.Cells["Tax Code"].Value.ToString();//DECLARE.Replace(row, "Tax", ""); //Tax Code
                            x.TaxAmount = Convert.ToDouble(DECLARE.Replace(row, "Tax Amount", "0:.##"));
                            x.TaxRate = Convert.ToDouble(DECLARE.Replace(row, "Tax Rate", "0:.##"));
                            x.LineTotal = Convert.ToDouble(DECLARE.Replace(row, "Line Total", "0:.##"));
                            x.PriceAfterDisc = Convert.ToDouble(DECLARE.Replace(row, "PriceAfterDisc", "0:.##"));

                        }
                    }
                    else
                    {

                        double dblDiscPerc = Convert.ToDouble(DECLARE.Replace(row, "Discount", "0"));
                        double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0"));
                        double dblDiscAmt = (dblUnitPrice / 100) * dblDiscPerc;

                        if ((row.Cells["Quantity"].Value != null))
                        {
                            InvoiceItemsModel.InvoiceItems.Add(new InvoiceItemsModel.InvoiceItemsData
                            {
                                ObjType = frmUnofficialSales.objType, //ObjType
                                Style = txtStyleCode.Text, //Style
                                Color = txtColorCode.Text, //Color
                                Section = txtSizeCode.Text, //Section
                                Size = DECLARE.Replace(row, "Size", "0"),
                                ItemCode = row.Cells[4].Value.ToString(), // ItemCode
                                BarCode = DECLARE.Replace(row, "Barcode", ""),
                                ItemName = row.Cells[6].Value.ToString(),
                                GrossPrice = Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0")),
                                DiscountPerc = dblDiscPerc,//C
                                DiscountAmount = dblDiscAmt,
                                Quantity = Convert.ToInt32(row.Cells[7].Value.ToString()), //Qty
                                FWhsCode = row.Cells["Warehouse"].Value.ToString(), //WHsCode
                                TaxCode = row.Cells["Tax Code"].Value.ToString(), //Tax Code
                                TaxAmount = Convert.ToDouble(DECLARE.Replace(row, "Tax Amount", "0")),//C
                                TaxRate = Convert.ToDouble(DECLARE.Replace(row, "Tax Rate", "0")),//C
                                UnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0")),//C
                                LineTotal = Convert.ToDouble(DECLARE.Replace(row, "Line Total", "0")),//C
                                PriceAfterDisc = Convert.ToDouble(DECLARE.Replace(row, "PriceAfterDisc", "0"))

                            });
                        }
                    }
                }

                StaticHelper._MainForm.ShowMessage("Item(s) added", true);
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
            frmSearch2 fS = new frmSearch2();
            fS.oSearchMode = SearchTable;
            frmSearch2.Param1 = Param1;
            frmSearch2.Param2 = Param2;
            frmSearch2.Param3 = Param3;
            frmSearch2.Param4 = Param4;
            frmSearch2._title = title;
            frmSearch2.oFocus = Focus;
            fS.oFormTitle = title;
            fS.ShowDialog();

            Code = fS.oCode;
            Name = fS.oName;
        }

        void CheckColor(string StyleCode)
        {
            var sapHana = new SAPHanaAccess();
            string query = query = $"SELECT DISTINCT A.U_Color,(SELECT Name FROM [@PRCOLOR] Z Where Z.Code = A.U_Color) [Name] FROM OITM A Where A.U_StyleCode = '{StyleCode.Replace("'", "''")}'";
            DataTable dtColor = sapHana.Get(query);

            if (dtColor.Rows.Count == 1)
            {
                string ColorCode = dtColor.Rows[0][0].ToString();
                txtColorCode.Text = ColorCode;
                //Check if section has 1 value
                string query2 = $"SELECT DISTINCT Replace(U_Section,'''','''') Section FROM OITM WHERE U_StyleCode = '{StyleCode.Replace("'", "''")}' and U_Color = '{ColorCode.Replace("'", "''")}'";
                DataTable dtSection = sapHana.Get(query2);

                if (dtSection.Rows.Count == 1)
                {
                    txtSizeCode.Text = dtSection.Rows[0][0].ToString();
                }
            }
        }

        private void gvITR_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void pbSearch_Click(object sender, EventArgs e)
        {

        }

        private void gvSelectedItem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }
        

        private void gvSelectedItem_Click(object sender, EventArgs e)
        {

        }

        private void SelectItemList(bool ItemListSelected)
        {
            gvSelectedItem.BorderStyle = ItemListSelected == true ? System.Windows.Forms.BorderStyle.FixedSingle : System.Windows.Forms.BorderStyle.None;
            gvUOS.BorderStyle = ItemListSelected == true ? System.Windows.Forms.BorderStyle.None : System.Windows.Forms.BorderStyle.FixedSingle;
        }

     

        private void txtDepDesc_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmUnofficialSalesItemList_Load(object sender, EventArgs e)
        {
            usic.dgvSetup(gvSelectedItem);
            PgSize = DefaultPgSize;
            LoadCurrentRecords();
        }

        private void btnCategoryCode_Click_1(object sender, EventArgs e)
        {
            LoadSelectionOfCategories();
        }

        private void btnSubCatCode_Click_1(object sender, EventArgs e)
        {
            LoadSelectionOfSubCategories();
        }

        private void btnBrand_Click_1(object sender, EventArgs e)
        {
            LoadSelectionOfBrands();
        }

        private void btnSize_Click_1(object sender, EventArgs e)
        {
            LoadSelectionOfSizes();
        }

        private void btnSubSize_Click_1(object sender, EventArgs e)
        {
            LoadSelectionOfSubSizes();
        }

        private void btnDept_Click_1(object sender, EventArgs e)
        {
            LoadSelectionOfDepts();
        }

        private void btnSubDep_Click_1(object sender, EventArgs e)
        {
            LoadSelectionOfSubDepts();
        }

        private void btnColorCode_Click_1(object sender, EventArgs e)
        {
            LoadSelectionOfColors();
        }

        private void btnSubColCode_Click_1(object sender, EventArgs e)
        {
            LoadSelectionOfSubColors();
        }

        private void btnStyle_Click(object sender, EventArgs e)
        {
            LoadSelectionOfStyles();
        }

        private void btnClearFilters_Click_1(object sender, EventArgs e)
        {
            ClearFilter(1, 1, 1, 1, 1, 1, 1, 1, 1);
            txtBrandCode.Clear();
            txtBrandDesc.Clear();
            LoadCurrentRecords("N");
        }

        private void pbSearch_Click_1(object sender, EventArgs e)
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

        private void navItemGet_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "navItemGet":
                    int intNegaQty = 0;
                    intNegaQty = usic.GetSelectedItems(gvUOS, gvSelectedItem);
                    //if (intNegaQty == 0)
                    //{
                    //    //gvSelectedItem.FirstDisplayedScrollingRowIndex = gvSelectedItem.RowCount - 1;
                    //}
                    //else
                    //{
                    //    PublicStatic.frmMain.NotiMsg("Selected item(s) does not have any available quantity.", Color.Red);
                    //}
                    //gvSelectedItem.Sort(gvSelectedItem.Columns["SortCode"], ListSortDirection.Ascending);
                    break;
                case "navItemGetAll":
                    usic.GetAllSelectedItems(gvUOS, gvSelectedItem);
                    break;
                case "navItemBackAll":
                    usic.GetAllSelectedItems(gvSelectedItem, gvUOS);
                    break;
                case "navItemBack":
                    //usic.GetSelectedItems(gvSelectedItem, gvUOS);
                    GetBackSelectedItems(gvSelectedItem, gvUOS);

                    break;
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            if (gvUOS.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in gvUOS.Rows)
                {
                    try
                    {
                        if (row.Cells[defaultColumn].Value.ToString().ToUpper().StartsWith(txtSearch.Text.Replace("\r\n", "").ToUpper()))
                        {
                            row.Selected = true;
                            gvUOS.FirstDisplayedScrollingRowIndex = row.Index;
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

        private void navItemBack_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "navItemGet":
                    int intNegaQty = 0;
                    intNegaQty = usic.GetSelectedItems(gvUOS, gvSelectedItem);
                    //if (intNegaQty == 0)
                    //{
                    //    //gvSelectedItem.FirstDisplayedScrollingRowIndex = gvSelectedItem.RowCount - 1;
                    //}
                    //else
                    //{
                    //    PublicStatic.frmMain.NotiMsg("Selected item(s) does not have any available quantity.", Color.Red);
                    //}
                    //gvSelectedItem.Sort(gvSelectedItem.Columns["SortCode"], ListSortDirection.Ascending);
                    break;
                case "navItemGetAll":
                    usic.GetAllSelectedItems(gvUOS, gvSelectedItem);
                    break;
                case "navItemBackAll":
                    usic.GetAllSelectedItems(gvSelectedItem, gvUOS);
                    break;
                case "navItemBack":
                    //usic.GetSelectedItems(gvSelectedItem, gvUOS);
                    GetBackSelectedItems(gvSelectedItem, gvUOS);

                    break;
            }
        }

        private void navItemGetAll_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "navItemGet":
                    int intNegaQty = 0;
                    intNegaQty = usic.GetSelectedItems(gvUOS, gvSelectedItem);
                    //if (intNegaQty == 0)
                    //{
                    //    //gvSelectedItem.FirstDisplayedScrollingRowIndex = gvSelectedItem.RowCount - 1;
                    //}
                    //else
                    //{
                    //    PublicStatic.frmMain.NotiMsg("Selected item(s) does not have any available quantity.", Color.Red);
                    //}
                    //gvSelectedItem.Sort(gvSelectedItem.Columns["SortCode"], ListSortDirection.Ascending);
                    break;
                case "navItemGetAll":
                    usic.GetAllSelectedItems(gvUOS, gvSelectedItem);
                    break;
                case "navItemBackAll":
                    usic.GetAllSelectedItems(gvSelectedItem, gvUOS);
                    break;
                case "navItemBack":
                    //usic.GetSelectedItems(gvSelectedItem, gvUOS);
                    GetBackSelectedItems(gvSelectedItem, gvUOS);

                    break;
            }
        }

        private void navItemBackAll_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "navItemGet":
                    int intNegaQty = 0;
                    intNegaQty = usic.GetSelectedItems(gvUOS, gvSelectedItem);
                    //if (intNegaQty == 0)
                    //{
                    //    //gvSelectedItem.FirstDisplayedScrollingRowIndex = gvSelectedItem.RowCount - 1;
                    //}
                    //else
                    //{
                    //    PublicStatic.frmMain.NotiMsg("Selected item(s) does not have any available quantity.", Color.Red);
                    //}
                    //gvSelectedItem.Sort(gvSelectedItem.Columns["SortCode"], ListSortDirection.Ascending);
                    break;
                case "navItemGetAll":
                    usic.GetAllSelectedItems(gvUOS, gvSelectedItem);
                    break;
                case "navItemBackAll":
                    usic.GetAllSelectedItems(gvSelectedItem, gvUOS);
                    break;
                case "navItemBack":
                    //usic.GetSelectedItems(gvSelectedItem, gvUOS);
                    GetBackSelectedItems(gvSelectedItem, gvUOS);

                    break;
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            AddItems();
        }

        private void btnFirstPage_Click_1(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            PgSize = DefaultPgSize;
            txtSearch.Clear();
            LoadCurrentRecords();
        }

        private void btnPrev_Click_1(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                PgSize -= DefaultPgSize;
                LoadCurrentRecords();
            }
        }

        private void btnNext_Click_1(object sender, EventArgs e)
        {
            if (this.CurrentPageIndex < this.TotalPage)
            {
                CurrentPageIndex++;
                PgSize += DefaultPgSize;
                LoadCurrentRecords();
            }
        }

        private void txtSearch_PreviewKeyDown_1(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtSearch.Focused == true && gvSelectedItem.Focused == false)
            {
                pbSearch_Click_1(sender, e);

                if (gvUOS.Rows.Count > 0)
                {
                    gvUOS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    gvUOS.Focus();
                }

                strPreviousEvent = "txtSearch_PreviewKeyDown";
            }
        }

        private void gvUOS_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        private void SearchFunction(object sender, EventArgs e)
        {
            try
            {
                int RowCnt = 0;
                long FindPage = 0;

                if (SearchEvent == false)
                {
                    if (txtSearch.Text != "")
                    {
                        PgSize = TotalDataCount;
                        SearchEvent = true;
                        LoadCurrentRecords("Y", sender, e);

                        //if (GetCurrentRecords(1, txtBrandCode.Text, txtColorCode.Text, txtSizeCode.Text, txtStyleCode.Text
                        //    , txtCatCode.Text, txtSubCatCode.Text, txtDepCode.Text, txtSubDepCode.Text, txtSubColDesc.Text, txtSubSizeCode.Text, "Y").Rows.Count > 0)
                        //{
                        //    RowCnt = SearchRowCnt;
                        //    SearchEvent = false;
                        //    FindPage = (RowCnt / DefaultPgSize) + 1;
                        //    CurrentPageIndex = FindPage;
                        //    PgSize = FindPage * DefaultPgSize;
                        //    LoadCurrentRecords("Y", sender, e);
                        //}
                        //else
                        //{
                        //    SearchEvent = false;
                        //    SearchClick = false;
                        //    frmMain.NotiMsg("No data found, please try again.", Color.Red);
                        //}
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
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void CalculateTotalPages()
        {
            string query = "SELECT Count(ItemCode) [Count] from OITM where ItemCode not like 'FA%'";
            DataTable dt = new DataTable();
            var sapHana = new SAPHanaAccess();
            dt = sapHana.Get(query);
            Int64 rowCount = Convert.ToInt64(DataAccess.Search(dt, 0, "Count"/*, frmMain*/));
            TotalDataCount = Convert.ToInt32(rowCount);
            TotalPage = rowCount / DefaultPgSize;

            if (rowCount % DefaultPgSize > 0)
            {
                TotalPage += 1;
            }
        }

        private void gvSelectedItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private DataTable GetCurrentRecords(long page, string strBrand, string strColor, string strSize, string strStyle, string strCat, string strSubCat
                                        , string strDept, string strSubDept, string SubColor, string strSubSize, string frmgvUOS, string strExludeItems)
        {

            DataTable dt = new DataTable();
            string query = null;
            var sapHana = new SAPHanaAccess();

            lblPageSize.Text = string.Empty;

            string SelectedField = "BarCode";

            if (frmgvUOS == "Y" || txtSearch.Text.Contains("*"))
            {
                if (defaultColumn == 5 || defaultColumn == 7)
                {
                    string ColName = gvUOS.Columns[defaultColumn].HeaderText.ToString();
                    SelectedField = ColName == "Item No." ? "ItemCode" : "ItemName";
                }
            }

            query = "SELECT *, MT1.GrossPriceBase * (1 +'.'+ Cast(MT1.TaxRate as INT)) [GrossPrice]  FROM ( " +
                    $" SELECT TOP {PgSize} " +
                    //" ROW_NUMBER() OVER (ORDER BY T0.U_ID023) [No]" +
                    //", " +
                    " T0.U_ID023 " +
                    ", T0.U_ID025 [Style]" +
                    ", T0.U_ID011 [Color]" +
                    ", T0.U_ID018 [Section]" +
                    ", T0.U_ID007 [Size] " +
                    ", T0.ItemCode" +
                    ", T0.CodeBars [BarCode]" +
                    ", T0.ItemName" +
                    ", ISNULL((select OnHand - IsCommited + OnOrder from OITW where ItemCode = T1.ItemCode and WhsCode = '" + DomainLayer.UnofficialSalesHeaderModel.oWhsCode + "'), 0) [Available] " +
                    ", '" + DomainLayer.UnofficialSalesHeaderModel.oWhsCode + "' [Warehouse]";

            if (DomainLayer.UnofficialSalesHeaderModel.oBPCode != "")
            {
                //query += $", (SELECT Discount from OSPP where CardCode = '{UnofficialSalesHeaderModel.oBPCode}' and ItemCode = T1.ItemCode) [Discount]" +
                //        $", CASE WHEN (select EFFECTIVE_MD_PRICE(T1.ItemCode, '{UnofficialSalesHeaderModel.oBPCode}', replace(convert(varchar(10),cast(GETDATE() as date),112), '-', ''))) = 0) " +
                //        $" THEN ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '1'), 0) " +
                //        $" ELSE (select EFFECTIVE_MD_PRICE(T1.ItemCode, '{UnofficialSalesHeaderModel.oBPCode}', replace(convert(varchar(10),cast(GETDATE() as date),112), '-', ''))) [GrossPriceBase]";

                query += $" , CASE WHEN ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T0.ItemCode and z.CardCode = '{DomainLayer.UnofficialSalesHeaderModel.oBPCode}'),0) = 0) " +
                        " THEN ISNULL((select Price from ITM1 where ItemCode = T0.ItemCode and PriceList = '1'), 0) " +
                        $" ELSE ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T0.ItemCode and z.CardCode = '{DomainLayer.UnofficialSalesHeaderModel.oBPCode}'),0) [GrossPriceBase] ";
            }
            else
            {
                query += ", ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '1'), 0) [GrossPriceBase] ";
            }

            query += " , ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '1'), 0) [UnitPrice] " +
                    $" , '{DomainLayer.UnofficialSalesHeaderModel.oTaxGroup}' [Tax Code]" +
                    $", (select Rate from OVTG Where Code = '" + DomainLayer.UnofficialSalesHeaderModel.oTaxGroup + "' ) [TaxRate] " +
                    " , '0' [Tax Amount] " +
                    " , '0' [Line Total] ";

            if (DomainLayer.UnofficialSalesHeaderModel.oBPCode != "")
            {
                query += $", ISNULL((SELECT Discount from OSPP where CardCode = '{DomainLayer.UnofficialSalesHeaderModel.oBPCode}' and ItemCode = T0.ItemCode),0) [Discount]";
            }
            else
            {
                query += ", '0' [Discount]";
            }

            query += " FROM OITM T0 " +
                    " INNER JOIN OITW T1 ON T0.ItemCode = T1.ItemCode " +
                    " INNER JOIN ITM1 T2 ON T0.ItemCode = T2.ItemCode and T2.PriceList = '1' " +
                    " WHERE T0.ItmsGrpCod = '100' ";

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
                query += "GROUP BY T0.U_ID025, T0.ItmsGrpCod, T0.U_ID011, T0.U_ID018, T0.U_ID007, T0.ItemCode, T0.CodeBars, T0.ItemName " +
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
                query += "GROUP BY T0.U_ID025, T0.ItmsGrpCod, T0.U_ID011, T0.U_ID018, T0.U_ID007 , T0.ItemCode, T0.CodeBars, T0.ItemName " +
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
                query += " GROUP BY T0.U_ID025, T0.ItmsGrpCod, T0.U_ID011, T0.U_ID018, T0.U_ID007, T0.ItemCode, T0.CodeBars, T0.ItemName" +
                         ", T1.ItemCode, T0.U_ID023 ORDER BY T0.U_ID023) MT1 ";
            }

            if (page != 1)
            {
                long PreviousPageOffSet = (page - 1) * DefaultPgSize;

                query += $" WHERE MT1.No NOT IN(SELECT TOP " + PreviousPageOffSet.ToString() + " ROW_NUMBER() OVER (ORDER BY U_ID023) [No] FROM OITM ORDER BY U_ID023) ORDER BY MT1.U_ID023";
            }

            dt = sapHana.Get(query);

            //if (txtSearch.Text != "" && SearchEvent == true & dt.Rows.Count > 0)
            //{
            //    SearchRowCnt = Convert.ToInt32(dt.Rows[0]["No"].ToString());
            //}

            gvUOS.Columns.Clear();
            //usic.dgvSetup(gvUOS);
            lblPageSize.Text = $"Page Size - {dt.Rows.Count}";

            return dt;

        }

        private void navClick(object sender, EventArgs e)
        {

        }

    }
}

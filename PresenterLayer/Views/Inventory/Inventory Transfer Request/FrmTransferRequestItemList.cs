using DirecLayer;
using DomainLayer.Models.Inventory_Transfer_Request;
using InfrastructureLayer.InventoryRepository;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using PresenterLayer.Services.Inventory;
using PresenterLayer.Views.Main;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using zDeclare;

namespace PresenterLayer.Views.Inventory.Inventory_Transfer_Request
{
    public partial class FrmTransferRequestItemList : MetroForm
    {
        Inventory_Style _Style = new Inventory_Style();
        Inventory_generics _Generics = new Inventory_generics();
        SAPHanaAccess sapHanaAccess = new SAPHanaAccess();
        QueryRepository Query = new QueryRepository();
        private ITRmaintenance ITRm = new ITRmaintenance();

        public static int FrmCartonManagementIndex = 0;
        public MainForm frmMain;

        private int DefaultColumn = 1, ColRowIndex = 0, SearchRowCount = 0, TotalDataCount = 0, index = 0, LineNum = 0;
        private long PageSize = 0, CurrentPageIndex = 1, DefaultPgSize = 500, TotalPage = 0;

        public string oStyleCode, oColorCode, oSection, oBPCode, oWhsCode, oBpCode, oBpName, oWhse, oTaxGroup, oDate, oCompany;

        public bool IsCartonActive;
        bool SearchEvent = false;
        public string frmgvITR = "";

        public string oDocEntry { get; set; }
        public string oTable { get; set; }
        public List<string> oSelectedItems { get; set; }

        int ColumnIndex = 1;
        string ColumnName = "T1.CodeBars";

        private string strPreviousEvent = "";
        public FrmTransferRequestItemList()
        {
            InitializeComponent();
        }

        private void FrmPurchasingItemList_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            PageSize = DefaultPgSize;
            CalculateTotalPages();
            _Style.dgvSetup(gvSelectedItem);
            //_Style.ItemListColumn(gvSelectedItem);
            gvITR.DataSource = LoadItems();
            //gvIT.Sort(gvIT.Columns["Barcode"], ListSortDirection.Ascending);
            //ColumnName = FindFieldNameByColumnName(gvIT.Columns[ColumnIndex].Name);

            pnlItemOption.Visible = IsCartonActive;
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            ListofBrand();
        }

        private void pbDept_Click(object sender, EventArgs e)
        {
            ListofDepartment();
        }

        private void pbSubDepartment_Click(object sender, EventArgs e)
        {
            ListofSubDepartment();
        }

        private void pbCategory_Click(object sender, EventArgs e)
        {
            ListofCategory();
        }

        private void pbSubCategory_Click(object sender, EventArgs e)
        {
            ListofSubCategory();
        }

        private void btnStyleCode_Click(object sender, EventArgs e)
        {
            ListofStyleCode();
        }

        private void pbSizeCategory_Click(object sender, EventArgs e)
        {
            ListofSizeCategory();
        }

        private void pbSize_Click(object sender, EventArgs e)
        {
            ListofSize();
        }

        private void pbColorCategory_Click(object sender, EventArgs e)
        {
            ListofColorCategory();
        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            ListofColor();
        }

        private void FrmPurchasingItemList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                if (e.KeyCode == Keys.S)
                {
                    txtSearch.Focus();
                }

                else if (e.KeyCode == Keys.B)
                {
                    ListofBrand();
                }

                else if (e.KeyCode == Keys.D)
                {
                    ListofDepartment();
                }

                else if (e.KeyCode == Keys.P)
                {
                    ListofSubDepartment();
                }

                else if (e.KeyCode == Keys.C)
                {
                    ListofCategory();
                }

                else if (e.KeyCode == Keys.U)
                {
                    ListofSubCategory();
                }

                else if (e.KeyCode == Keys.I)
                {
                    ListofSizeCategory();
                }

                else if (e.KeyCode == Keys.Z)
                {
                    ListofSize();
                }

                else if (e.KeyCode == Keys.E)
                {
                    ListofColorCategory();
                }

                else if (e.KeyCode == Keys.O)
                {
                    ListofColor();
                }

                else if (e.KeyCode == Keys.Y)
                {
                    ListofStyleCode();
                }

                else if (e.KeyCode == Keys.D1)
                {
                    if (gvITR.Rows.Count > 0)
                    {
                        gvITR.Focus();
                        gvITR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    }
                }

                else if (e.KeyCode == Keys.D2)
                {
                    if (gvSelectedItem.Rows.Count > 0)
                    {
                        gvSelectedItem.Focus();
                        gvITR.BorderStyle = System.Windows.Forms.BorderStyle.None;
                        gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

                        int index = gvSelectedItem.CurrentRow.Index;
                        gvSelectedItem.CurrentCell = gvSelectedItem["Quantity", index];
                        gvSelectedItem[3, index].Selected = true;
                        gvSelectedItem.BeginEdit(true);
                    }
                }

                else if (e.KeyCode == Keys.Left)
                {
                    try
                    {
                        ColumnIndex++;
                        ColumnName = FindFieldNameByColumnName(gvITR.Columns[ColumnIndex].Name);
                        lblColumnSelected.Text = gvITR.Columns[ColumnIndex].Name;
                    }
                    catch
                    {

                    }
                }

                else if (e.KeyCode == Keys.Left)
                {
                    if (ColumnIndex >= 0)
                    {
                        ColumnIndex--;
                        ColumnName = FindFieldNameByColumnName(gvITR.Columns[ColumnIndex].Name);
                        lblColumnSelected.Text = gvITR.Columns[ColumnIndex].Name;
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Dispose();
            }
        }

        private void gvIT_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                gvITR.CurrentRow.Selected = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                //bool isCarton = ITR == null ? true : false;
                _Style.GetSelectedItems(gvITR, gvSelectedItem);
                oSelectedItems = Inventory_Style.itemSelected;
                gvITR.ClearSelection();
                //gvSelectedItem.Sort(gvSelectedItem.Columns[0], ListSortDirection.Ascending);
            }
        }

        private void gvSelectedItem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                int index = gvSelectedItem.CurrentRow.Index;

                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    gvSelectedItem.CurrentRow.Selected = true;
                }

                else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    gvSelectedItem.CurrentRow.Selected = false;
                }

                else if (e.KeyCode == Keys.Enter)
                {
                    //foreach (DataGridViewRow row in gvSelectedItem.SelectedRows)
                    //{
                    //    int indexs = row.Index;

                    //    gvSelectedItem.Rows.RemoveAt(indexs);
                    //}

                    //Part of Change Request - 01/09/2020
                    if (gvSelectedItem.Focused == true && strPreviousEvent == "")             //Transfer items from List to Selected Items Table
                    {
                        foreach (DataGridViewRow row in gvSelectedItem.SelectedRows)
                        {
                            int indexs = row.Index;

                            gvSelectedItem.Rows.RemoveAt(indexs);
                        }
                    }

                    strPreviousEvent = strPreviousEvent != "" ? "" : strPreviousEvent;
                }

                else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.Q)
                {
                    if (gvSelectedItem.Rows.Count > 0)
                    {

                        gvSelectedItem.CurrentCell = gvSelectedItem["Quantity", index];
                        gvSelectedItem[3, index].Selected = true;
                        gvSelectedItem.BeginEdit(true);
                    }
                    else
                    {
                        //frmMain.NotiMsg("No items to set Quantity.", Color.Red);
                        StaticHelper._MainForm.ShowMessage("No items to set Quantity", true);
                    }
                }

                else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.G)
                {
                    if (gvSelectedItem.Rows.Count > 0)
                    {
                        gvSelectedItem.CurrentRow.Selected = false;
                        gvSelectedItem.CurrentCell = gvSelectedItem["Gross Price", index];
                        gvSelectedItem[4, index].Selected = true;
                        gvSelectedItem.BeginEdit(true);
                    }
                }

                else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.N)
                {
                    var ChainList = _Generics.ModalShow("chain", "", "List of Chains");

                    gvSelectedItem.CurrentRow.Cells["Chain"].Value = ChainList[1];
                }

                else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
                {
                    string clipBoard = Clipboard.GetText();

                    foreach (DataGridViewCell cell in gvSelectedItem.SelectedCells)
                    {
                        cell.Value = clipBoard;
                    }
                }

            }
            catch { }
        }

        private void gvIT_Click(object sender, EventArgs e)
        {
            gvITR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //if (gvIT.Columns.Count > 1)
            //{
            //    foreach (DataGridViewRow row in gvIT.Rows)
            //    {
            //        if (row.Cells[ColumnIndex].Value != null || row.Cells[ColumnIndex + 1].Value != null)
            //        {
            //            if (row.Cells[ColumnIndex].Value.ToString().ToUpper().StartsWith(txtSearch.Text.ToUpper()) || row.Cells[ColumnIndex + 1].Value.ToString().ToUpper().StartsWith(txtSearch.Text.ToUpper()))
            //            {
            //                row.Selected = true;
            //                gvIT.FirstDisplayedScrollingRowIndex = row.Index;
            //                break;
            //            }
            //            else
            //            {
            //                row.Selected = false;
            //            }
            //        }
            //    }
            //}

            if (gvITR.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in gvITR.Rows)
                {
                    if (row.Cells[ColumnIndex].Value != null)
                    {
                        if (row.Cells[ColumnIndex].Value.ToString().ToUpper().StartsWith(txtSearch.Text.Replace("\r\n", "").ToUpper()))
                        {
                            row.Selected = true;
                            gvITR.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                        else
                        {
                            row.Selected = false;
                        }
                    }
                }
            }
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtBrand.Text = "";
            txtBrandDesc.Text = "";
            txtDepartment.Text = "";
            txtDepartmentDesc.Text = "";
            txtSubDepartment.Text = "";
            txtSubDepartmentDesc.Text = "";
            txtCategory.Text = "";
            txtCategoryDesc.Text = "";
            txtStyle.Text = "";
            txtStyleDesc.Text = "";
            txtSizeCategory.Text = "";
            txtSizeCategoryDesc.Text = "";
            txtSize.Text = "";
            txtSizeDesc.Text = "";
            txtColorCategory.Text = "";
            txtColorCategoryDesc.Text = "";
            txtColor.Text = "";
            txtColorDescription.Text = "";
            txtSubCategory.Text = "";
            txtSubCategoryDesc.Text = "";
            txtSubCategoryDesc.Text = "";
            gvITR.DataSource = null;
            gvITR.DataSource = LoadItems();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex < TotalPage)
            {
                CurrentPageIndex++;
                PageSize += DefaultPgSize;
                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = TotalPage;
            PageSize = TotalDataCount;
            gvITR.DataSource = null;
            gvITR.DataSource = LoadItems();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                PageSize -= DefaultPgSize;
                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            PageSize = DefaultPgSize;
            SearchEvent = false;
            gvITR.DataSource = null;
            gvITR.DataSource = LoadItems();
        }

        private void gvIT_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex = e.ColumnIndex;
            ColumnName = FindFieldNameByColumnName(gvITR.Columns[ColumnIndex].Name);
            lblColumnSelected.Text = gvITR.Columns[ColumnIndex].Name;
        }

        private string FindFieldNameByColumnName(string name)
        {
            string result = "";

            switch (name)
            {
                case "Item Code":
                    result = "T1.ItemCode";
                    break;

                case "Barcode":
                    result = "T1.CodeBars";
                    break;

                case "Description":
                    result = "T1.ItemName";
                    break;
            }

            return result;
        }

        private void navClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //bool isCarton = ITR == null ? true : false;
            switch (btn.Name)
            {
                case "navItemGet":

                    _Style.GetSelectedItems(gvITR, gvSelectedItem);
                    oSelectedItems = Inventory_Style.itemSelected;
                    break;

                case "navItemGetAll":

                    _Style.GetAllSelectedItems(gvITR, gvSelectedItem);
                    //gvIT.Rows.Clear();
                    DataTable DT = (DataTable)gvITR.DataSource;
                    if (DT != null)
                    {
                        DT.Clear();
                    }
                    break;

                case "navItemBackAll":

                    if (gvSelectedItem.RowCount > 0 && gvITR.Rows.Count == 0)
                    {
                        gvITR.DataSource = LoadItems();
                        gvSelectedItem.Rows.Clear();
                    }
                    break;

                case "navItemBack":

                    foreach (DataGridViewRow row in gvSelectedItem.SelectedRows)
                    {
                        int index = row.Index;
                        DataTable dt = gvITR.DataSource as DataTable;
                        DataRow dtrow = dt.NewRow();

                        dtrow["BarCode"] = row.Cells[0].Value;
                        dtrow["Item Code"] = row.Cells[1].Value;
                        dtrow["Description"] = row.Cells[2].Value;
                        dtrow["Available"] = 0;
                        dtrow["BrandCode"] = row.Cells[4].Value;
                        dtrow["Brand"] = row.Cells[5].Value;
                        dtrow["Style Code"] = row.Cells[6].Value;
                        dtrow["Style"] = row.Cells[7].Value;
                        dtrow["Color Code"] = row.Cells[8].Value;
                        dtrow["Color"] = row.Cells[9].Value;
                        dtrow["Size"] = row.Cells[10].Value;
                        dtrow["Section"] = row.Cells[11].Value;
                        dtrow["Price"] = row.Cells[12].Value;
                        dtrow["Warehouse"] = row.Cells[13].Value;
                        dtrow["U_ID023"] = row.Cells[14].Value;

                        dt.Rows.Add(dtrow);

                        gvSelectedItem.Rows.RemoveAt(index);
                    }
                    break;
            }
        }

        private void txtSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtSearch.Focused == true && gvSelectedItem.Focused == false)
            {
                //gvITR.DataSource = null;
                //gvITR.DataSource = LoadItems();
                pbSearch_Click(sender, e);

                if (gvITR.Rows.Count > 0)
                {
                    gvITR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    gvITR.Focus();
                }

                strPreviousEvent = "txtSearch_PreviewKeyDown";
            }
        }

        private void gvSelectedItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string ColumnName = gvSelectedItem.Columns[e.ColumnIndex].Name;

            if (ColumnName == "Gross Price")
            {
                double tax = (Convert.ToDouble(gvSelectedItem.CurrentRow.Cells["Tax Rate"].Value) / 100);
                tax = tax != 0 ? (tax + 1) : 0;

                double grossPrice = Convert.ToDouble(gvSelectedItem.CurrentRow.Cells["Gross Price"].Value);
                grossPrice = tax != 0 ? (grossPrice / tax) : 1;

                gvSelectedItem.CurrentRow.Cells["Unit Price"].Value = grossPrice;
            }
            else if (ColumnName == "Quantity")
            {
                string sItemCode = gvSelectedItem.CurrentRow.Cells["Item No."].Value.ToString();
                double dOrigQty = Convert.ToDouble(gvSelectedItem.CurrentRow.Cells["Quantity"].Value);

                gvSelectedItem.CurrentRow.Cells["Ordered Quantity"].Value = dOrigQty; //Requested to remove logic 07212021 //InventoryTransferRequestService.GetCartonQty(sItemCode, dOrigQty);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (gvSelectedItem.Rows.Count > 0)
            {
                if (AddItem())
                {
                    Close();
                }
            }
            else
            {
                //frmMain.NotiMsg("Please select an item(s) first before adding.", Color.Red);
                StaticHelper._MainForm.ShowMessage("Please select an item(s) first before adding", true);
            }
        }

        private void pbSearch_Click(object sender, EventArgs e)
        {
            gvITR.DataSource = null;
            gvITR.DataSource = LoadItems();
            if (SearchEvent == false)
            {
                if (txtSearch.Text != "")
                {
                    PageSize = TotalDataCount;
                    SearchEvent = true;
                    frmgvITR = "Y";
                    LoadItems();
                }
                else
                {
                    SearchEvent = false;
                    frmgvITR = "N";
                    LoadItems();
                }
            }
            else
            {
                SearchEvent = false;
                //frmMain.NotiMsg("Please search again after loading of data finishes.", Color.Red);
                StaticHelper._MainForm.ShowMessage("Please search again after loading of data finishes.", true);
            }
        }

        private void radBtnBasedDocument_CheckedChanged(object sender, EventArgs e)
        {
            gvITR.DataSource = null;
            gvITR.DataSource = LoadItems();
        }

        private void radBtnITM_CheckedChanged(object sender, EventArgs e)
        {
            gvITR.DataSource = null;
            gvITR.DataSource = LoadItems();
        }

        private void gvSelectedItem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var colname = gvSelectedItem.Columns[e.ColumnIndex].Name;

            if (colname == "Chain")
            {
                var ChainList = _Generics.ModalShow("chain", "", "List of Chains");

                if (ChainList != null)
                {
                    if (ChainList[1].Count() > 0)
                    {
                        gvSelectedItem.CurrentRow.Cells["Chain"].Value = ChainList[1];
                    }
                }
            }
        }

        private void gvSelectedItem_Click(object sender, EventArgs e)
        {
            gvITR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        #region FUNCTION

        private void ListofColor()
        {
            var modalParams = new List<string>();

            modalParams.Add(txtColorCategory.Text);

            var color = _Generics.ModalShow("list-of-color", modalParams, "List of Color");

            if (color.Count > 0)
            {
                txtColor.Text = color[0].ToString();
                txtColorDescription.Text = color[1].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void ListofColorCategory()
        {
            var colorCategory = _Generics.ModalShow("list-of-color-category", "", "List of Color Category");

            if (colorCategory.Count > 0)
            {
                txtColorCategory.Text = colorCategory[0].ToString();
                txtColorCategoryDesc.Text = colorCategory[1].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void ListofSize()
        {
            List<string> modalParams = new List<string>();

            modalParams.Add(txtSizeCategoryDesc.Text);

            var size = _Generics.ModalShow("list-of-size", modalParams, "List of Size");

            if (size.Count > 0)
            {
                txtSize.Text = size[0].ToString();
                txtSizeDesc.Text = size[1].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void ListofSizeCategory()
        {
            var size = _Generics.ModalShow("list-of-size=category", "", "List of Size Category");

            if (size.Count > 0)
            {
                txtSizeCategory.Text = size[0].ToString();
                txtSizeCategoryDesc.Text = size[1].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void ListofStyleCode()
        {
            List<string> modalParams = new List<string>();

            modalParams.Add(txtBrand.Text);
            modalParams.Add(txtDepartment.Text);
            modalParams.Add(txtSubDepartment.Text);
            modalParams.Add(txtCategory.Text);
            modalParams.Add(txtSubCategoryDesc.Text);

            var style = _Generics.ModalShow("list-of-style", modalParams, "List of Style");

            if (style.Count > 0)
            {
                txtStyle.Text = style[0].ToString();
                txtStyleDesc.Text = style[1].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void ListofSubCategory()
        {
            List<string> modalParams = new List<string>();

            modalParams.Add(txtBrand.Text);
            modalParams.Add(txtDepartment.Text);
            modalParams.Add(txtSubDepartment.Text);
            modalParams.Add(txtCategory.Text);

            var subCategory = _Generics.ModalShow("list-of-SubCategory", modalParams, "List of Sub-Category");

            if (subCategory.Count > 0)
            {
                txtStyle.Text = "";
                txtStyleDesc.Text = "";
                txtSizeCategory.Text = "";
                txtSizeCategoryDesc.Text = "";
                txtSize.Text = "";
                txtSizeDesc.Text = "";
                txtColorCategory.Text = "";
                txtColorCategoryDesc.Text = "";
                txtColor.Text = "";
                txtColorDescription.Text = "";

                txtSubCategory.Text = subCategory[0].ToString();
                txtSubCategoryDesc.Text = subCategory[1].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void ListofSubDepartment()
        {
            List<string> modalParams = new List<string>();

            modalParams.Add(txtBrand.Text);
            modalParams.Add(txtDepartment.Text);

            var subDeparmtent = _Generics.ModalShow("list-of-SubDepartment", modalParams, "List of Sub-Department");

            if (subDeparmtent.Count > 0)
            {
                txtCategory.Text = "";
                txtCategoryDesc.Text = "";
                txtStyle.Text = "";
                txtStyleDesc.Text = "";
                txtSizeCategory.Text = "";
                txtSizeCategoryDesc.Text = "";
                txtSize.Text = "";
                txtSizeDesc.Text = "";
                txtColorCategory.Text = "";
                txtColorCategoryDesc.Text = "";
                txtColor.Text = "";
                txtColorDescription.Text = "";
                txtSubCategory.Text = "";
                txtSubCategoryDesc.Text = "";
                txtSubCategoryDesc.Text = "";

                txtSubDepartment.Text = subDeparmtent[0].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void ListofCategory()
        {
            List<string> modalParams = new List<string>();

            modalParams.Add(txtBrand.Text);
            modalParams.Add(txtDepartment.Text);
            modalParams.Add(txtSubDepartment.Text);

            var category = _Generics.ModalShow("list-of-category", modalParams, "List of Category");

            if (category.Count > 0)
            {
                txtStyle.Text = "";
                txtStyleDesc.Text = "";
                txtSizeCategory.Text = "";
                txtSizeCategoryDesc.Text = "";
                txtSize.Text = "";
                txtSizeDesc.Text = "";
                txtColorCategory.Text = "";
                txtColorCategoryDesc.Text = "";
                txtColor.Text = "";
                txtColorDescription.Text = "";
                txtSubCategory.Text = "";
                txtSubCategoryDesc.Text = "";
                txtSubCategoryDesc.Text = "";

                txtCategory.Text = category[0].ToString();
                txtCategoryDesc.Text = category[1].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void ListofDepartment()
        {
            List<string> modalParams = new List<string>();

            modalParams.Add(txtBrand.Text);

            var deparmtent = _Generics.ModalShow("list-of-department", modalParams, "List of Department");

            if (deparmtent.Count > 0)
            {
                txtSubDepartment.Text = "";
                txtSubDepartmentDesc.Text = "";
                txtCategory.Text = "";
                txtCategoryDesc.Text = "";
                txtStyle.Text = "";
                txtStyleDesc.Text = "";
                txtSizeCategory.Text = "";
                txtSizeCategoryDesc.Text = "";
                txtSize.Text = "";
                txtSizeDesc.Text = "";
                txtColorCategory.Text = "";
                txtColorCategoryDesc.Text = "";
                txtColor.Text = "";
                txtColorDescription.Text = "";
                txtSubCategory.Text = "";
                txtSubCategoryDesc.Text = "";
                txtSubCategoryDesc.Text = "";

                txtDepartment.Text = deparmtent[0].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void ListofBrand()
        {
            var brand = _Generics.ModalShow("list-of-brand", "", "List of Brand");

            if (brand.Count > 0)
            {
                txtDepartment.Text = "";
                txtDepartmentDesc.Text = "";
                txtSubDepartment.Text = "";
                txtSubDepartmentDesc.Text = "";
                txtCategory.Text = "";
                txtCategoryDesc.Text = "";
                txtStyle.Text = "";
                txtStyleDesc.Text = "";
                txtSizeCategory.Text = "";
                txtSizeCategoryDesc.Text = "";
                txtSize.Text = "";
                txtSizeDesc.Text = "";
                txtColorCategory.Text = "";
                txtColorCategoryDesc.Text = "";
                txtColor.Text = "";
                txtColorDescription.Text = "";
                txtSubCategory.Text = "";
                txtSubCategoryDesc.Text = "";
                txtSubCategoryDesc.Text = "";

                txtBrand.Text = brand[0].ToString();
                txtBrandDesc.Text = brand[1].ToString();

                gvITR.DataSource = null;
                gvITR.DataSource = LoadItems();
            }
        }

        private void CalculateTotalPages()
        {
            string query = "SELECT Count(ItemCode) [Count] from OITM where ItemCode not like 'FA%'";
            DataTable dt = new DataTable();
            //dt = DataAccess.Select(DataAccess.conStr("HANA"), query);
            dt = sapHanaAccess.Get(query);
            Int64 rowCount = Convert.ToInt64(DataAccess.Search(dt, 0, "Count"));
            TotalDataCount = Convert.ToInt32(rowCount);
            TotalPage = rowCount / DefaultPgSize;

            if (rowCount % DefaultPgSize > 0)
            {
                TotalPage += 1;
            }
        }

        private string ChainCode(object chainDesc)
        {
            string result = "";

            if (chainDesc != null && chainDesc.ToString() != string.Empty && string.IsNullOrWhiteSpace(chainDesc.ToString()) && string.IsNullOrEmpty(chainDesc.ToString()))
            {
                string query = $"SELECT GroupCode FROM OCRG WHERE GroupName = '{chainDesc.ToString()}'";
                var dt = sapHanaAccess.Get(query);
                result = DataAccess.Search(dt, 0, "GroupCode");
            }

            return result;
        }

        //private DataTable LoadItems()
        //{
        //    long page = CurrentPageIndex;
        //    DataTable dt = new DataTable();

        //    string query = string.Empty;
        //    string SelectedField = "BarCode";

        //    if (txtSearch.Text == "Y" || txtSearch.Text.Contains("*"))
        //    {
        //        if (DefaultColumn == 2 || DefaultColumn == 3)
        //        {
        //            string ColName = gvIT.Columns[DefaultColumn].HeaderText.ToString();
        //            SelectedField = ColName == "Item No." ? "ItemCode" : "ItemName";
        //        }
        //    }

        //    query = "SELECT * from (" +
        //           $" SELECT TOP {PageSize} " +
        //           //" ROW_NUMBER() OVER (ORDER BY U_ID023) [No]" +
        //           //" , 
        //           " T1.U_ID023 " +
        //           " , T1.CodeBars [BarCode] " +
        //           " , T1.ItemCode " +
        //           " , T1.ItemName " +
        //           " , ISNULL((select OnHand - IsCommited + OnOrder from OITW where ItemCode = T1.ItemCode and WhsCode = '" + InventoryTransferReqHeaderModel.oWhsCode + "'), 0) [Available] " +
        //           " , T1.U_ID001 [BrandCode] " +
        //           " , (SELECT Name FROM [@OBND] WHERE Code = T1.U_ID001) [Brand] " +
        //           " , T1.U_ID025 [Style Code] " +
        //           " , (SELECT MAX(U_Style) [U_Style] FROM [@OSTL] WHERE U_Code = T1.U_ID025) [Style] " +

        //           " , (SELECT U_Code FROM [@OCLR] Where U_Color = T1.U_ID011 and Code = T1.U_ID022) [Color Code] " +
        //           " , T1.U_ID011 [Color] " +

        //           " , T1.U_ID007 [Size] " +
        //           " , T1.U_ID018 [Section] ";

        //    string itemCode = "'";

        //    if (InventoryTransferReqHeaderModel.oBPCode != "")
        //    {
        //        //query += $" , CASE WHEN (select EFFECTIVE_MD_PRICE(T1.ItemCode, '{InventoryTransferReqHeaderModel.oBPCode}', replace(convert(varchar(10),cast(GETDATE() as date),112), '-', ''))) = 0) " +
        //        //       $" THEN ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '1'), 0) " +
        //        //       $" ELSE (select EFFECTIVE_MD_PRICE(T1.ItemCode, '{InventoryTransferReqHeaderModel.oBPCode}', replace(convert(varchar(10),cast(GETDATE() as date),112), '-', ''))) [Price]";

        //        query += $" , CASE WHEN ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T1.ItemCode and z.CardCode = '{InventoryTransferReqHeaderModel.oBPCode}'),0) = 0) " +
        //                " THEN ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '1'), 0) " +
        //                $" ELSE ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T1.ItemCode and z.CardCode = '{InventoryTransferReqHeaderModel.oBPCode}'),0) [Price] ";
        //    }
        //    else
        //    {
        //        query += ", ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '1'), 0) [Price]";
        //    }
        //    query += " , '" + InventoryTransferReqHeaderModel.oWhsCode + "' [Warehouse] " +
        //            " FROM OITM T1 " +
        //            " WHERE " +
        //            " T1.frozenFor = 'N' and T1.ItemCode not like 'FA%' and T1.InvntItem = 'Y' ";

        //    if (oSelectedItems != null)
        //    {
        //        itemCode += string.Join("','", oSelectedItems.ToArray());
        //        itemCode += "'";
        //        query += $"AND T1.ItemCode not in ({itemCode})";
        //    }

        //    if (txtBrand.Text != string.Empty)
        //    {
        //        query += "and T1.U_ID001 = '" + txtBrand.Text + "' ";
        //    }

        //    if (txtColorCategory.Text != string.Empty)
        //    {
        //        query += " and T1.U_ID022 = '" + txtColorCategory.Text + "' ";
        //    }

        //    if (txtSizeCategory.Text != string.Empty)
        //    {
        //        query += " and T1.U_ID006 = '" + txtSizeCategory.Text + "' ";
        //    }

        //    if (txtStyle.Text != string.Empty)
        //    {
        //        query += " and T1.U_ID025 = '" + txtStyle.Text + "' ";
        //    }

        //    if (txtCategory.Text != string.Empty)
        //    {
        //        query += " and T1.U_ID020 = '" + txtCategory.Text + "' ";
        //    }

        //    if (txtSubCategory.Text != string.Empty)
        //    {
        //        query += " and T1.U_ID021 = '" + txtSubCategory.Text + "' ";
        //    }

        //    if (txtDepartment.Text != string.Empty)
        //    {
        //        query += " and T1.U_ID002 = '" + txtDepartment.Text + "' ";
        //    }

        //    if (txtSubDepartment.Text != string.Empty)
        //    {
        //        query += " and T1.U_ID003 = '" + txtSubDepartment.Text + "' ";
        //    }

        //    if (txtColorDescription.Text != string.Empty)
        //    {
        //        query += " and T1.U_ID011 = '" + txtColorDescription.Text + "' ";
        //    }

        //    if (txtSizeDesc.Text != string.Empty)
        //    {
        //        query += " and T1.U_ID007 = '" + txtSizeDesc.Text + "' ";
        //    }

        //    //if (strExludeItems != null && strExludeItems != "")
        //    //{
        //    //    query += $" and T1.ItemCode NOT IN({strExludeItems}) ";
        //    //}

        //    string search = txtSearch.Text;

        //    string strSearch = txtSearch.Text;
        //    if (strSearch != "" && SearchEvent == true)
        //    {
        //        query += " GROUP BY T1.U_ID023, T1.CodeBars, T1.ItemCode, T1.ItemName, T1.U_ID001, T1.U_ID025, T1.FrgnName, T1.U_ID022, T1.U_ID007 " +
        //                 ", T1.U_ID018, T1.U_ID011 ORDER BY T1.U_ID023) MT1 " +
        //                 $" WHERE MT1.{SelectedField} like '" + txtSearch.Text + "%' " +
        //                 " ORDER BY MT1.U_ID023";
        //    }
        //    else if (strSearch != "" && SearchEvent == false && strSearch.Contains("*") && txtSearch.Focused == true && gvSelectedItem.Focused == false)
        //    {
        //        query += " GROUP BY T1.U_ID023, T1.CodeBars, T1.ItemCode, T1.ItemName, T1.U_ID001, T1.U_ID025, T1.FrgnName, T1.U_ID022, T1.U_ID007 " +
        //                 ", T1.U_ID018, T1.U_ID011 ORDER BY T1.U_ID023) MT1 ";

        //        var index = strSearch.IndexOf(@"*");
        //        var strLength = strSearch.Length - 1;
        //        string[] array = strSearch.Split('*');

        //        if (index == strLength)
        //        {
        //            query += $"WHERE MT1.{SelectedField} LIKE '{array[0]}%' ";
        //        }
        //        else if (index == 0)
        //        {
        //            query += $"WHERE MT1.{SelectedField} LIKE '%{array[1]}' ";
        //        }
        //        else
        //        {
        //            query += $"WHERE MT1.{SelectedField} LIKE '{array[0]}%{array[1]}' ";
        //        }
        //        query += " ORDER BY MT1.U_ID023";
        //    }
        //    else
        //    {
        //        query += " GROUP BY T1.U_ID023, T1.CodeBars, T1.ItemCode, T1.ItemName, T1.U_ID001, T1.U_ID025, T1.FrgnName, T1.U_ID022, T1.U_ID007 " +
        //                 ", T1.U_ID018, T1.U_ID011 ORDER BY T1.U_ID023) MT1 ";
        //    }

        //    if (page != 1)
        //    {
        //        long PreviousPageOffSet = (page - 1) * DefaultPgSize;

        //        query += $" WHERE MT1.No NOT IN(SELECT TOP " + PreviousPageOffSet.ToString() + " ROW_NUMBER() OVER (ORDER BY U_ID023) [No] FROM OITM ORDER BY U_ID023) ORDER BY MT1.U_ID023";
        //    }

        //    //var dtItem = DataAccess.Select(DataAccess.conStr("HANA"), query);
        //    var dtItem = sapHanaAccess.Get(query);

        //    try
        //    {
        //        lblPageSize.Text = $"Page Size - {dtItem.Rows.Count}";
        //    }
        //    catch { }

        //    return dtItem;
        //}

        private DataTable LoadItems()
        {
            var dt = new DataTable();

            string query = string.Empty;

            string itemCode = "'";

            if (radBtnITM.Checked || oTable == string.Empty || oTable == null)
            {
                //changed from oWhsCode == string.empty to string.IsNullOrEmpty(oWhsCode) by Cedi on May 30,2019
                oWhsCode = string.IsNullOrEmpty(oWhsCode) ? "02-RSRV" : oWhsCode;

                //query = $"SELECT T0.ItemCode [Item Code]" +
                //    $", T0.CodeBars [Barcode]" +
                //    $", T0.ItemName [Description]" +
                //    $", T0.U_ID012 [Style Code]" +
                //    $", ISNULL(T0.U_ID011,T0.U_ID010) [Color]" +
                //    $", U_ID018 [Section]" +
                //    $", ISNULL(U_ID007,U_ID009) [Size]" +
                //    $", ISNULL((select OnHand - IsCommited + OnOrder from OITW where ItemCode = T0.ItemCode and WhsCode = '{oWhsCode}'), 0) [Available]" +
                //    $", round(ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= (select MAX(Y.DocDate) " +
                //    $"OVER (PARTITION BY Y.ItemCode) [DocDate] from PCH1 Y where Y.ItemCode = Z.ItemCode)) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0),2) [UnitPrice]" +
                //    $", round(ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= (select MAX(Y.DocDate) " +
                //    $"OVER (PARTITION BY Y.ItemCode) [DocDate] from PCH1 Y where Y.ItemCode = Z.ItemCode)) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0) * " +
                //    $"(1 + (SELECT Z.Rate FROM OVTG Z Where Z.Code =  '{oTaxGroup}') / 100),2) [Gross Price]" +
                //    $", round((SELECT Z.Rate FROM OVTG Z Where Z.Code = '{oTaxGroup}'),2) [Tax Rate]" +
                //    $",round((ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= (select MAX(Y.DocDate) " +
                //    $"OVER (PARTITION BY Y.ItemCode) [DocDate] from PCH1 Y where Y.ItemCode = Z.ItemCode)) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0) * " +
                //    $"(1 + (SELECT Z.Rate FROM OVTG Z Where Z.Code = '{oTaxGroup}') / 100) ) - (ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z " +
                //    $"WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= (select MAX(Y.DocDate) OVER (PARTITION BY Y.ItemCode) [DocDate] from PCH1 Y where Y.ItemCode = Z.ItemCode)) " +
                //    $"FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0)),2) [Tax Amount] " +

                //$"FROM OITM T0 INNER JOIN OITW T1 ON T1.ItemCode = T0.ItemCode WHERE T1.WhsCode = '{oWhsCode}' AND frozenFor = 'N'";

                //Newly added 061719
                query = "SELECT " +
                    " T1.U_ID023 " +
                    " , T1.CodeBars [Barcode] " +
                    " , T1.ItemCode [Item Code]" +
                    " , T1.ItemName [Description]";

                if (oDocEntry != null && oDocEntry != "")
                {
                    //On Comment, for approval 102119
                    //query += " , ISNULL((select OnHand from OITW where ItemCode = T1.ItemCode and WhsCode = '" + InventoryTransferReqHeaderModel.oWhsCode + "') " +
                    //        $"- (SELECT RelQtty FROM PKL1 T3 inner join WTQ1 T2 on T3.OrderEntry = T2.DocEntry and T3.PickEntry = T2.LineNum WHERE T2.ItemCode = T1.ItemCode and T2.DocEntry = {oDocEntry}), 0) [Available for Release] ";

                    query += " , ISNULL((select OnHand - IsCommited + OnOrder from OITW where ItemCode = T1.ItemCode and WhsCode = '" + InventoryTransferReqHeaderModel.oWhsCode + "'), 0) [Available] ";
                }
                else
                {
                    query += " , ISNULL((select OnHand - IsCommited + OnOrder from OITW where ItemCode = T1.ItemCode and WhsCode = '" + InventoryTransferReqHeaderModel.oWhsCode + "'), 0) [Available] ";
                }

                query += " , T1.U_ID001 [BrandCode] " +
                    " , (SELECT Name FROM [@OBND] WHERE Code = T1.U_ID001) [Brand] " +
                    " , T1.U_ID012 [Style Code] " +
                    " , (SELECT MAX(U_Style) [U_Style] FROM [@OSTL] WHERE U_Code = T1.U_ID012) [Style] " +

                    " , (SELECT MAX(U_Code) FROM [@OCLR] Where U_Color = T1.U_ID011 and Code = T1.U_ID022) [Color Code] " +
                    " , T1.U_ID011 [Color] " +

                    " , T1.U_ID007 [Size] " +
                    " , T1.U_ID018 [Section] ";

            }

            if (InventoryTransferReqHeaderModel.oBPCode != "")
            {
                //(select EFFECTIVE_MD_PRICE(T1.ItemCode, '{UnofficialSalesHeaderModel.oBPCode}', replace(convert(varchar(10), cast(GETDATE() as date), 112), '-', ''))) = 0)

                query += $" , CASE WHEN ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T1.ItemCode and z.CardCode = '{InventoryTransferReqHeaderModel.oBPCode}'),0) = 0) " +
                        $" THEN ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = (select ListNum from OCRD where CardCode = '{InventoryTransferReqHeaderModel.oBPCode}')), 0) " +
                        $" ELSE ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T1.ItemCode and z.CardCode = '{InventoryTransferReqHeaderModel.oBPCode}'),0) [Price] ";

                //query += $" , CASE WHEN (select EFFECTIVE_MD_PRICE(T1.ItemCode, '{InventoryTransferReqHeaderModel.oBPCode}', replace(convert(varchar(10), cast(GETDATE() as date), 112), '-', ''))) = 0) " +
                //        $" THEN ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = (select ListNum from OCRD where CardCode = '{InventoryTransferReqHeaderModel.oBPCode}')), 0) " +
                //        $" ELSE ISNULL(((select EFFECTIVE_MD_PRICE(T1.ItemCode, '{InventoryTransferReqHeaderModel.oBPCode}', replace(convert(varchar(10), cast(GETDATE() as date), 112), '-', ''))),0) [Price] ";
            }
            else
            {
                //query += ", ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = '1'), 0) [Price]";
                query += $", ISNULL((select Price from ITM1 where ItemCode = T1.ItemCode and PriceList = (select ListNum from OCRD where CardCode = '{InventoryTransferReqHeaderModel.oBPCode}')), 0) [Price]";
            }
            //else
            //{
            //    query = "SELECT T0.ItemCode[Item Code], T0.CodeBars[Barcode], T0.ItemName[Description], " +

            //        "T0.U_ID012[Style Code], ISNULL(T0.U_ID011, T0.U_ID010)[Color], T0.U_ID018[Section], ISNULL(T0.U_ID007, T0.U_ID009)[Size], " +

            //        $"(T1.Quantity - ISNULL((SELECT SUM(Z.U_Quantity) FROM[@CM_ROWS] Z WHERE Z.U_BaseRef = '{oDocEntry}' AND Z.U_BaseType = 'O{oTable}' " +
            //        "AND Z.U_ItemNo = T0.ItemCode), 0)) [Available], " +

            //        "T1.PriceBefDi[UnitPrice], T1.PriceAfVAT[Gross Price], 0 [Tax Rate], 0 [Tax Amount] " +

            //        "FROM OITM T0 " +

            //        $"INNER JOIN {oTable}1 T1 ON T1.ItemCode = T0.ItemCode " +

            //        $"WHERE T1.DocEntry = '{oDocEntry}' AND frozenFor = 'N' " +

            //        $"AND (T1.Quantity - ISNULL((SELECT SUM(Z.U_Quantity) FROM[@CM_ROWS] Z WHERE Z.U_BaseRef = '{oDocEntry}' AND Z.U_BaseType = 'O{oTable}' " +
            //        "AND Z.U_ItemNo = T0.ItemCode), 0)) > 0";
            //}

            //Newly added 061719
            query += " , '" + oWhsCode + "' [Warehouse] " +
                    " FROM OITM T1 " +
                    " WHERE " +
                    " T1.frozenFor = 'N' and T1.ItemCode not like 'FA%' and T1.InvntItem = 'Y' ";

            if (oSelectedItems != null)
            {
                itemCode += string.Join("','", oSelectedItems.ToArray());
                itemCode += "'";
                query += $"AND T1.ItemCode not in ({itemCode})";
            }

            if (txtBrand.Text != "")
            {
                query += $"AND T1.U_ID001 = '{txtBrand.Text}' ";
            }

            if (txtDepartment.Text != "")
            {
                query += $"AND T1.U_ID002 = '{txtDepartment.Text}' ";
            }

            if (txtSubDepartment.Text != "")
            {
                query += $"AND T1.U_ID003 = '{txtSubDepartment.Text}' ";
            }

            if (txtCategory.Text != "")
            {
                query += $"AND T1.U_ID004 = '{txtCategoryDesc.Text}' ";
            }

            if (txtSubCategory.Text != "")
            {
                query += $"AND T1.U_ID005 = '{txtSubCategoryDesc.Text}' ";
            }

            if (txtStyle.Text != "")
            {
                query += $"AND T1.U_ID012 = '{txtStyle.Text}' ";
            }

            if (txtSizeCategory.Text != "")
            {
                query += $"AND T1.U_ID006 = '{txtSizeCategory.Text}' ";
            }

            if (txtSize.Text != "")
            {
                query += $"AND T1.U_ID008 = '{txtSize.Text}' ";
            }

            if (txtColorCategory.Text != "")
            {
                query += $"AND T1.U_ID010 = '{txtColorCategory.Text}' ";
            }

            if (txtColor.Text != "")
            {
                query += $"AND T1.U_ID011 = '{txtColorDescription.Text}' ";
            }

            string search = txtSearch.Text;

            if (search.Length > 2)
            {
                if (search.Contains("*"))
                {
                    var index = search.IndexOf(@"*");
                    var strLenght = search.Length - 1;
                    string[] array = search.Split('*');

                    if (index == strLenght)
                    {
                        query += $"AND {ColumnName} LIKE '{array[0]}%' ";
                    }
                    else if (index == 0)
                    {
                        query += $"AND {ColumnName} LIKE '%{array[1]}' ";
                    }
                    else
                    {
                        if (array.Count() == 2)
                        {
                            query += $"AND {ColumnName} LIKE '{ValidateInput.String(array[0])}%{ValidateInput.String(array[1])}' ";
                        }
                    }
                }
                else
                {
                    if (ColumnName == "T1.CodeBars")
                    {
                        query += $"AND {ColumnName} LIKE '{search}%' OR T1.ItemCode LIKE '{search}%'";
                    }
                    else
                    {
                        query += $"AND {ColumnName} LIKE '{search}%' ";
                    }
                }
            }

            query += " Order By T1.U_ID023 ASC";

            var dtItem = sapHanaAccess.Get(query);

            try
            {
                lblPageSize.Text = $"Page Size - {dtItem.Rows.Count}";
            }
            catch { }

            return dtItem;
        }

        private Boolean AddItem()
        {
            bool result = false;
            try
            {
                var bpcode = InventoryTransferReqHeaderModel.oBPCode;

                if (!String.IsNullOrEmpty(bpcode.ToString()))
                {
                    var _Chain = "";
                    var _ChainDesc = "";
                    DataTable dtChains;
                    var queryChain = "Select" +
                            " T0.GroupCode" +
                           ",T1.GroupName" +
                            " FROM OCRD T0 INNER JOIN OCRG T1 ON T0.GroupCode = T1.GroupCode" +
                            " where CardCode ='" + bpcode + "'";

                    dtChains = sapHanaAccess.Get(queryChain);

                    if (dtChains.Rows.Count > 0)
                    {
                        _Chain = dtChains.Rows[0]["GroupCode"].ToString();
                        _ChainDesc = dtChains.Rows[0]["GroupName"].ToString();
                    }

                    foreach (DataGridViewRow row in gvSelectedItem.Rows)
                    {
                        if ((row.Cells["Quantity"].Value != null && row.Cells["Quantity"].Value.ToString() != "0"))
                        {
                            string strItemCode = row.Cells[1].Value.ToString();
                            bool AllowDupItem = true;

                            if (ITRm.SelValue("AllowDupItems", InventoryTransferReqHeaderModel.oTransferType) != "Y" && InventoryTransferReqItemsModel.ITRitems.Where(x => x.ItemCode == strItemCode).ToList().Count() >= 1)
                            {
                                AllowDupItem = false;
                            }

                            if (AllowDupItem)
                            {

                                if (InventoryTransferReqItemsModel.ITRitems.Count > 0)
                                {
                                    //if (InventoryTransferReqItemsModel.ITRitems.Select(x => x.Linenum).Max() != 0)
                                    //{
                                    index = InventoryTransferReqItemsModel.ITRitems.Select(x => x.Linenum).Max() + 1;
                                    //}
                                }

                                //int iIndex = index++;
                                string company = sapHanaAccess.Get(Query.GetCompanyQuerySearch(oCompany)).Rows[1].ItemArray[1].ToString();
                                InventoryTransferReqItemsModel.ITRitems.Add(new InventoryTransferReqItemsModel.ITRItemsData
                                {
                                    Linenum = index++,
                                    ObjType = FrmInventoryTransferRequest.objType,
                                    BarCode = row.Cells[0].Value.ToString(),
                                    ItemCode = strItemCode,
                                    ItemName = row.Cells[2].Value.ToString(),
                                    GrossPrice = Convert.ToDouble(DECLARE.Replace(row, "Price", "0")),
                                    BrandCode = row.Cells[4].Value.ToString(),
                                    Brand = row.Cells[5].Value.ToString(),
                                    StyleCode = row.Cells[6].Value.ToString(),
                                    Style = row.Cells[7].Value.ToString(),
                                    ColorCode = row.Cells[8].Value.ToString(),
                                    Color = row.Cells[9].Value.ToString(),
                                    Size = row.Cells[10].Value.ToString(),
                                    Section = row.Cells[11].Value.ToString(),
                                    Quantity = Convert.ToInt32(row.Cells[3].Value.ToString()),
                                    FWhsCode = row.Cells[13].Value.ToString(),
                                    TWhsCode = InventoryTransferReqHeaderModel.oToWhsCode,
                                    SortCode = row.Cells[14].Value.ToString(),
                                    Company = company,
                                    InventoryUOM = ITRm.GetUOM(row.Cells[1].Value.ToString()),
                                    Chain = _Chain,
                                    ChainDescription = _ChainDesc
                                });
                            }
                        }
                        //}
                    }//old


                    //frmMain.NotiMsg("Item(s) added", Color.Green);
                    //CLear Data
                    gvSelectedItem.Columns.Clear();
                    StaticHelper._MainForm.ShowMessage("Item(s) added");

                    result = true;

                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Please select a Business Partner", true);

                }



                return result;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, frmITR.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                return result;
            }
        }

        #endregion
    }
}

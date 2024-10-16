using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using zDeclare;
using DirecLayer._02_Form.MVP.Views;
using PresenterLayer.Views;
using PresenterLayer.Views.Main;
using DirecLayer;
using DomainLayer;
using PresenterLayer;
using PresenterLayer.Helper;

namespace DirecLayer
{
    public partial class FrmPurchasingItemList : MetroForm
    {
        PurchasingAP_Style _Style = new PurchasingAP_Style();
        PurchasingAP_generics _Generics = new PurchasingAP_generics();
        SAPHanaAccess Hana = new SAPHanaAccess();
        public MainForm frmMain;
        //need to clarify
        public FrmPurchaseOrder PO = new FrmPurchaseOrder();
        public FrmGoodsReceiptPO GRPO = new FrmGoodsReceiptPO();
        private int DefaultColumn, ColRowIndex = 0, SearchRowCount = 0, TotalDataCount = 0;
        private long PageSize = 0, CurrentPageIndex = 1, DefaultPgSize = 500, TotalPage = 0;

        public string oStyleCode, oColorCode, oSection, oBPCode, oWhsCode, oBpCode, oBpName, oWhse, oTaxGroup, oDate;

        public bool IsCartonActive { get; set; }

        public string oDocEntry { get; set; }
        public string oTable { get; set; }
        public List<string> oSelectedItems { get; set; }
        public static bool isCarton { get; set; } = false;
        int ColumnIndex;
        //string ColumnName = "T0.ItemCode";
        string ColumnName = "T0.CodeBars";

        private string strPreviousEvent = "";

        public FrmPurchasingItemList()
        {
            InitializeComponent();
        }

        private void FrmPurchasingItemList_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            PageSize = DefaultPgSize;
            CalculateTotalPages();
            _Style.ItemListColumn(gvSelectedItem);
            gvIT.DataSource = LoadItems();
            //gvIT.Sort(gvIT.Columns["Barcode"], ListSortDirection.Ascending);
            //ColumnName = FindFieldNameByColumnName(gvIT.Columns[ColumnIndex].Name);
            ColumnIndex = 1;
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
                    if (gvIT.Rows.Count > 0)
                    {
                        gvIT.Focus();
                        gvIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    }
                }

                else if (e.KeyCode == Keys.D2)
                {
                    if (gvSelectedItem.Rows.Count > 0)
                    {
                        gvSelectedItem.Focus();
                        gvIT.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
                        ColumnName = FindFieldNameByColumnName(gvIT.Columns[ColumnIndex].Name);
                        lblColumnSelected.Text = gvIT.Columns[ColumnIndex].Name;
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
                        ColumnName = FindFieldNameByColumnName(gvIT.Columns[ColumnIndex].Name);
                        lblColumnSelected.Text = gvIT.Columns[ColumnIndex].Name;
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
                gvIT.CurrentRow.Selected = true;
            }

            else if (e.KeyCode == Keys.Enter)
            {
                bool isCarton = PO == null ? true : false;
                _Style.GetSelectedItems(gvIT, gvSelectedItem, isCarton);
                gvIT.ClearSelection();
                gvSelectedItem.Sort(gvSelectedItem.Columns[0], ListSortDirection.Ascending);
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
                    foreach (DataGridViewRow row in gvSelectedItem.SelectedRows)
                    {
                        int indexs = row.Index;
                        DataTable dt = gvIT.DataSource as DataTable;
                        DataRow dtrow = dt.NewRow();

                        dtrow["Item Code"] = row.Cells[0].Value;
                        dtrow["Barcode"] = row.Cells[1].Value;
                        dtrow["Description"] = row.Cells[2].Value;
                        dtrow["Style Code"] = row.Cells[6].Value;
                        dtrow["Color"] = row.Cells[7].Value;
                        dtrow["Section"] = row.Cells[9].Value;
                        dtrow["Size"] = row.Cells[8].Value;
                        dtrow["Available"] = row.Cells[10].Value;
                        dtrow["UnitPrice"] = row.Cells[11].Value;
                        dtrow["Gross Price"] = row.Cells[4].Value;
                        dtrow["Tax Rate"] = row.Cells[12].Value;
                        dtrow["Tax Amount"] = row.Cells[13].Value;


                        dt.Rows.Add(dtrow);
                        gvSelectedItem.Rows.RemoveAt(indexs);
                    }
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
            gvIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (gvIT.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in gvIT.Rows)
                {
                    if (row.Cells[ColumnIndex].Value != null)
                    {
                        if (row.Cells[ColumnIndex].Value.ToString().ToUpper().StartsWith(txtSearch.Text.Replace("\r\n", "").ToUpper()))
                        {
                            row.Selected = true;
                            gvIT.FirstDisplayedScrollingRowIndex = row.Index;
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
            gvIT.DataSource = null;
            gvIT.DataSource = LoadItems();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex < TotalPage)
            {
                CurrentPageIndex++;
                PageSize += DefaultPgSize;
                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = TotalPage;
            PageSize = TotalDataCount;
            gvIT.DataSource = null;
            gvIT.DataSource = LoadItems();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 1)
            {
                CurrentPageIndex--;
                PageSize -= DefaultPgSize;
                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            PageSize = DefaultPgSize;
            gvIT.DataSource = null;
            gvIT.DataSource = LoadItems();
        }

        private void gvIT_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex = e.ColumnIndex;
            ColumnName = FindFieldNameByColumnName(gvIT.Columns[ColumnIndex].Name);
            lblColumnSelected.Text = gvIT.Columns[ColumnIndex].Name;
        }

        private string FindFieldNameByColumnName(string name)
        {
            string result = "";

            switch (name)
            {
                case "Item Code":
                    result = "T0.ItemCode";
                    break;

                case "Barcode":
                    result = "T0.CodeBars";
                    break;

                case "Description":
                    result = "T0.ItemName";
                    break;
            }

            return result;
        }

        private void navClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.Name)
            {
                case "navItemGet":

                    _Style.GetSelectedItems(gvIT, gvSelectedItem, isCarton);
                    break;

                case "navItemGetAll":

                    _Style.GetAllSelectedItems(gvIT, gvSelectedItem, isCarton);
                    break;

                case "navItemBackAll":

                    if (gvSelectedItem.RowCount > 0)
                    {
                        gvSelectedItem.Rows.Clear();
                    }
                    break;

                case "navItemBack":
                   
                    foreach (DataGridViewRow row in gvSelectedItem.SelectedRows)
                    {
                        int index = row.Index;
                        DataTable dt = gvIT.DataSource as DataTable;
                        DataRow dtrow = dt.NewRow();

                        dtrow["Item Code"] = row.Cells[0].Value;
                        dtrow["Barcode"] = row.Cells[1].Value;
                        dtrow["Description"] = row.Cells[2].Value;
                        dtrow["Style Code"] = row.Cells[6].Value;
                        dtrow["Color"] = row.Cells[7].Value;
                        dtrow["Section"] = row.Cells[9].Value;
                        dtrow["Size"] = row.Cells[8].Value;
                        dtrow["Available"] = row.Cells[10].Value;
                        dtrow["UnitPrice"] = row.Cells[11].Value;
                        dtrow["Gross Price"] = row.Cells[4].Value;
                        dtrow["Tax Rate"] = row.Cells[12].Value;
                        dtrow["Tax Amount"] = row.Cells[13].Value;

                        if (isCarton)
                        {
                            dtrow["LineNum"] = row.Cells[14].Value;
                        }
                        
                        dt.Rows.Add(dtrow);
                        gvSelectedItem.Rows.RemoveAt(index);
                    }
                    break;
            }
        }

        private void txtSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Part of Change Request 01/09/2020
                //gvIT.DataSource = null;
                //gvIT.DataSource = LoadItems();

                pbSearch_Click(sender, e);

                if (gvIT.Rows.Count > 0)
                {
                    gvIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    gvIT.Focus();
                }

                strPreviousEvent = "txtSearch_PreviewKeyDown";
            }
        }

        private void gvSelectedItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string ColumnName = gvSelectedItem.Columns[e.ColumnIndex].Name;

                if (ColumnName == "Gross Price")
                {
                    string strTaxRate = string.IsNullOrEmpty(gvSelectedItem.CurrentRow.Cells["Tax Rate"].Value.ToString()) ? "0" : gvSelectedItem.CurrentRow.Cells["Tax Rate"].Value.ToString();
                    string strTaxNotif = string.IsNullOrEmpty(gvSelectedItem.CurrentRow.Cells["Tax Rate"].Value.ToString()) ? "None" : "";

                    double tax = (Convert.ToDouble(strTaxRate) / 100);
                    tax = tax != 0 ? (tax + 1) : 0;

                    double grossPrice = Convert.ToDouble(gvSelectedItem.CurrentRow.Cells["Gross Price"].Value);

                    //grossPrice = tax != 0 ? (grossPrice / tax) : 1;
                    grossPrice = tax != 0 ? (grossPrice / tax) : grossPrice;

                    gvSelectedItem.CurrentRow.Cells["Unit Price"].Value = grossPrice;

                    if (strTaxNotif == "None")
                    {
                        StaticHelper._MainForm.ShowMessage("No Tax Rate, please check setup in SAP.", true);
                    }
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
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
            }
        }

        private void pbSearch_Click(object sender, EventArgs e)
        {
            gvIT.DataSource = null;
            gvIT.DataSource = LoadItems();
        }

        private void radBtnBasedDocument_CheckedChanged(object sender, EventArgs e)
        {
            gvIT.DataSource = null;
            gvIT.DataSource = LoadItems();
        }

        private void radBtnITM_CheckedChanged(object sender, EventArgs e)
        {
            gvIT.DataSource = null;
            gvIT.DataSource = LoadItems();
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
            gvIT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gvSelectedItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        #region FUNCTION

        private void ListofColor()
        {
            List<string> modalParams = new List<string>();

            modalParams.Add(txtColorCategoryDesc.Text);

            var color = _Generics.ModalShow("list-of-color", modalParams, "List of Color");

            if (color.Count > 0)
            {
                txtColor.Text = color[0].ToString();
                txtColorDescription.Text = color[1].ToString();

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
            }
        }

        private void ListofColorCategory()
        {
            var colorCategory = _Generics.ModalShow("list-of-color-category", "", "List of Color Category");

            if (colorCategory.Count > 0)
            {
                txtColorCategory.Text = colorCategory[0].ToString();
                txtColorCategoryDesc.Text = colorCategory[1].ToString();

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
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

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
            }
        }

        private void ListofSizeCategory()
        {
            var size = _Generics.ModalShow("list-of-size=category", "", "List of Size Category");

            if (size.Count > 0)
            {
                txtSizeCategory.Text = size[0].ToString();
                txtSizeCategoryDesc.Text = size[1].ToString();

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
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

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
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

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
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

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
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

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
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

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
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

                gvIT.DataSource = null;
                gvIT.DataSource = LoadItems();
            }
        }

        private void CalculateTotalPages()
        {
            string query = "SELECT Count(ItemCode) [Count] from OITM where ItemCode not like 'FA%'";
            DataTable dt = new DataTable();
            dt = Hana.Get(query);
            Int64 rowCount = Convert.ToInt64(DataAccess.Search(dt, 0, "Count"/*, frmMain*/));
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
                result = DataAccess.Search(Hana.Get($"SELECT GroupCode FROM OCRG WHERE GroupName = '{chainDesc.ToString()}'"), 0, "GroupCode"/*, frmMain*/);
            }

            return result;
        }

        private DataTable LoadItems()
        {
            DataTable dt = new DataTable();

            string query = string.Empty;

            string itemCode = "'";

            if (radBtnITM.Checked || oTable == string.Empty || oTable == null)
            {
                oWhsCode = (oWhsCode == string.Empty || oWhsCode == null) ? "02-RSRV" : oWhsCode;

                query = $"SELECT T0.ItemCode [Item Code], T0.CodeBars [Barcode], T0.ItemName [Description], T0.U_ID012 [Style Code], " +

                "ISNULL(T0.U_ID011,T0.U_ID010) [Color], U_ID018 [Section], ISNULL(U_ID007,U_ID009) [Size], " +

                $"ISNULL((select OnHand - IsCommited + OnOrder from OITW where ItemCode = T0.ItemCode and WhsCode = '{oWhsCode}'), 0) [Available], " +

                "round(ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= (select MAX(Y.DocDate) OVER (PARTITION BY Y.ItemCode) [DocDate] from PCH1 Y where Y.ItemCode = Z.ItemCode)) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0),2) [UnitPrice], " +

                "round(ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= (select MAX(Y.DocDate) OVER (PARTITION BY Y.ItemCode) [DocDate] from PCH1 Y where Y.ItemCode = Z.ItemCode)) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0) * " +

                $"(1 + (SELECT Z.Rate FROM OVTG Z Where Z.Code =  '{oTaxGroup}') / 100),2) [Gross Price], " +

                $"round((SELECT Z.Rate FROM OVTG Z Where Z.Code = '{oTaxGroup}'),2) [Tax Rate], " +

                "round((ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= (select MAX(Y.DocDate) OVER (PARTITION BY Y.ItemCode) [DocDate] from PCH1 Y where Y.ItemCode = Z.ItemCode)) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0) * " +

                $"(1 + (SELECT Z.Rate FROM OVTG Z Where Z.Code = '{oTaxGroup}') / 100) ) - " +

                "(ISNULL((SELECT Distinct(SELECT Distinct  MAX(Z.Price) FROM PCH1 Z WHERE Z.ItemCode = A.ItemCode AND Z.DocDate >= (select MAX(Y.DocDate) OVER (PARTITION BY Y.ItemCode) [DocDate] from PCH1 Y where Y.ItemCode = Z.ItemCode)) FROM PCH1 A WHERE A.ItemCode =  T0.ItemCode), 0)),2) [Tax Amount] " +

                $"FROM OITM T0 INNER JOIN OITW T1 ON T1.ItemCode = T0.ItemCode WHERE T1.WhsCode = '{oWhsCode}' AND frozenFor = 'N'";
            }
            else
            {

                query = "SELECT T0.ItemCode[Item Code], T0.CodeBars[Barcode], T0.ItemName[Description], " +

                    "T0.U_ID012[Style Code], ISNULL(T0.U_ID011, T0.U_ID010)[Color], T0.U_ID018[Section], ISNULL(T0.U_ID007, T0.U_ID009)[Size], " +

                    $"(T1.Quantity - ISNULL((SELECT SUM(Z.U_Quantity) FROM[@CM_ROWS] Z WHERE Z.U_BaseRef = '{oDocEntry}' AND Z.U_BaseType = 'O{oTable}' " +
                    "AND Z.U_ItemNo = T0.ItemCode), 0)) [Available], " +

                    "T1.PriceBefDi[UnitPrice], T1.PriceAfVAT[Gross Price], 0 [Tax Rate], 0 [Tax Amount], T1.LineNum " +

                    "FROM OITM T0 " +

                    $"INNER JOIN {oTable}1 T1 ON T1.ItemCode = T0.ItemCode " +

                    $"WHERE  frozenFor = 'N' AND T0.validFor = 'Y' and T0.ItmsGrpCod = 100 " +

                    $"AND (T1.Quantity - ISNULL((SELECT SUM(Z.U_Quantity) FROM[@CM_ROWS] Z WHERE Z.U_BaseRef = '{oDocEntry}' AND Z.U_BaseType = 'O{oTable}' " +
                    "AND Z.U_ItemNo = T0.ItemCode), 0)) > 0";
            }
            if(oDocEntry != "" && oDocEntry != null)
            {
                query += $"AND T1.DocEntry = '{oDocEntry}' ";
            }

            if (oSelectedItems != null)
            {
                itemCode += string.Join("','", oSelectedItems.ToArray());
                itemCode += "'";
                query += $"AND T1.ItemCode not in ({itemCode})";
            }

            if (txtBrand.Text != "")
            {
                query += $"AND T0.U_ID001 = '{txtBrand.Text}' ";
            }

            if (txtDepartment.Text != "")
            {
                query += $"AND T0.U_ID002 = '{txtDepartment.Text}' ";
            }

            if (txtSubDepartment.Text != "")
            {
                query += $"AND T0.U_ID003 = '{txtSubDepartment.Text}' ";
            }

            if (txtCategory.Text != "")
            {
                query += $"AND T0.U_ID004 = '{txtCategoryDesc.Text}' ";
            }

            if (txtSubCategory.Text != "")
            {
                query += $"AND T0.U_ID005 = '{txtSubCategoryDesc.Text}' ";
            }

            if (txtStyle.Text != "")
            {
                query += $"AND T0.U_ID012 = '{txtStyle.Text}' ";
            }

            if (txtSizeCategory.Text != "")
            {
                query += $"AND T0.U_ID006 = '{txtSizeCategory.Text}' ";
            }

            if (txtSize.Text != "")
            {
                query += $"AND T0.U_ID008 = '{txtSize.Text}' ";
            }

            if (txtColorCategory.Text != "")
            {
                query += $"AND T0.U_ID010 = '{txtColorCategory.Text}' ";
            }

            if (txtColor.Text != "")
            {
                query += $"AND T0.U_ID011 = '{txtColorDescription.Text}' ";
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
                    if (ColumnName == "T0.CodeBars")
                    {
                        query += $"AND {ColumnName} LIKE '{search}%' OR T0.ItemCode LIKE '{search}%'";
                    }
                    else
                    {
                        query += $"AND {ColumnName} LIKE '{search}%' ";
                    }
                }

            }

            if (radBtnITM.Checked || oTable == string.Empty || oTable == null)
            {
                query += " Group By T0.ItemCode, T0.CodeBars, T0.ItemName, T0.U_ID012, T0.U_ID011, T0.U_ID010, T0.U_ID018, T0.U_ID007, T0.U_ID009, T0.U_ID023, ";
            }

            if (radBtnITM.Checked || oTable == string.Empty || oTable == null)
            {
                query += " Order By T0.U_ID023 ASC";
            }
            else
            {
                //Added new condition for Carton Management July 22,2019
                query += " Order By T1.LineNum ASC";
            }

            var dtItem = Hana.Get(query);

            try
            {
                lblPageSize.Text = $"Page Size - {dtItem.Rows.Count}";
            }
            catch { }

            return dtItem;
        }

        private Boolean AddItem()
        {
            try
            {
                if (IsCartonActive == false)
                {
                    foreach (DataGridViewRow row in gvSelectedItem.Rows)
                    {
                        double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0"));

                        string size = "";
                        if (row.Cells["Size"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Size"].Value.ToString()))
                        {
                            string query = $"SELECT U_ID008 [Child Size], U_ID009 [Parent Size] FROM OITM WHERE ItemCode = '{row.Cells["Item code"].Value.ToString()}'";
                            size = DECLARE.dtNull(Hana.Get(query), 0, "Parent Size", "");
                        }
                        else
                        {
                            size = row.Cells["Size"].Value.ToString();
                        }

                        string color = "";
                        if (row.Cells["Color"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Color"].Value.ToString()))
                        {
                            string query = $"SELECT U_ID010 [Parent Color], U_ID011 [Child Color] FROM OITM WHERE ItemCode = '{row.Cells["Item code"].Value.ToString()}'";
                            color = DECLARE.dtNull(Hana.Get(query), 0, "Parent Color", "");
                        }
                        else
                        {
                            color = row.Cells["Color"].Value.ToString();
                        }

                        FormCollection frms = Application.OpenForms;
                        foreach(Form frm in frms)
                        {
                            if(frm.Name == "FrmGoodsReceiptPO")
                            {
                                int index = PurchasingModel.GRPOdocument.Count;
                                PurchasingModel.GRPOdocument.Add(new PurchasingModel.GoodsReceiptPO
                                {
                                    Index = GRPO.Table.RowCount - 1 == 0?index:index++,
                                    StyleCode = row.Cells["Style Code"].Value.ToString(),
                                    Style = _Generics.StyleName(row.Cells["Style Code"].Value.ToString()),
                                    ColorCode = row.Cells["Color"].Value.ToString(),
                                    Color = color,
                                    Size = size,
                                    ItemNo = row.Cells["Item code"].Value.ToString(),
                                    ItemDescription = row.Cells["Description"].Value.ToString(),
                                    Quantity = row.Cells["Quantity"].Value.ToString() == "" ? 0d : Convert.ToDouble(row.Cells["Quantity"].Value),
                                    UoM = _Generics.Uom(row.Cells["Item code"].Value.ToString()),
                                    GrossPrice = Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0")),
                                    BrandCode = _Generics.Brand(row.Cells["Item code"].Value.ToString())[0].ToString(),
                                    Brand = _Generics.Brand(row.Cells["Item code"].Value.ToString())[1].ToString(),
                                    TaxCode = oTaxGroup,

                                    ChainDescription = row.Cells["Chain"].Value.ToString() != string.Empty ? row.Cells["Chain"].Value.ToString() : "",
                                    ChainPricetag = ChainCode(row.Cells["Chain"].Value) == string.Empty ? null : ChainCode(row.Cells["Chain"].Value),
                                    PricetagCount = row.Cells["Chain"].Value != null ? Convert.ToDouble(row.Cells["Quantity"].Value.ToString()) : 0.00,

                                    TaxRate = Convert.ToDouble(DECLARE.Replace(row, "Tax Rate", "0")),
                                    UnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0")),
                                    Warehouse = oWhsCode,
                                    BarCode = row.Cells["Barcode"].Value.ToString(),
                                });
                            }
                            else if(frm.Name == "FrmPurchaseOrder")
                            {
                                //int index = PurchasingModel.PurchaseOrderDocument.Count;
                                int LastIndexUsed = PurchasingModel.PurchaseOrderDocument.ToList().Count() > 0 ? Convert.ToInt32(PurchasingModel.PurchaseOrderDocument.Max(y => y.Index)) : 0;
                                int IntIndex = PurchasingModel.PurchaseOrderDocument.ToList().Count() > 0 ? LastIndexUsed + 1 : 0;

                                PurchasingModel.PurchaseOrderDocument.Add(new PurchasingModel.PurchaseOrder
                                {
                                    //Index = PO.Table.RowCount - 1 == 0 ? index : index++,
                                    Index = IntIndex,
                                    StyleCode = row.Cells["Style Code"].Value.ToString(),
                                    Style = _Generics.StyleName(row.Cells["Style Code"].Value.ToString()),
                                    ColorCode = row.Cells["Color"].Value.ToString(),
                                    Color = color,
                                    Size = size,
                                    ItemNo = row.Cells["Item code"].Value.ToString(),
                                    ItemDescription = row.Cells["Description"].Value.ToString(),
                                    Quantity = row.Cells["Quantity"].Value.ToString() == "" ? 0d : Convert.ToDouble(row.Cells["Quantity"].Value),
                                    UoM = _Generics.Uom(row.Cells["Item code"].Value.ToString()),
                                    GrossPrice = Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0")),
                                    BrandCode = _Generics.Brand(row.Cells["Item code"].Value.ToString())[0].ToString(),
                                    Brand = _Generics.Brand(row.Cells["Item code"].Value.ToString())[1].ToString(),
                                    TaxCode = oTaxGroup,

                                    //Current Default is RDS for Barcode Guide 080919
                                    //ChainDescription = row.Cells["Chain"].Value.ToString() != string.Empty ? row.Cells["Chain"].Value.ToString() : "",
                                    //ChainPricetag = ChainCode(row.Cells["Chain"].Value) == string.Empty ? null : ChainCode(row.Cells["Chain"].Value),
                                    ChainDescription = "RDS",
                                    ChainPricetag = "141",

                                    PricetagCount = row.Cells["Chain"].Value != null ? Convert.ToDouble(row.Cells["Quantity"].Value.ToString()) : 0.00,

                                    TaxRate = Convert.ToDouble(DECLARE.Replace(row, "Tax Rate", "0")),
                                    UnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0")),
                                    Warehouse = oWhsCode,
                                    BarCode = row.Cells["Barcode"].Value.ToString(),
                                });
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in gvSelectedItem.Rows)
                    {
                        //New Logic of index for No.359 Issue Logs - Cedi 071719
                        int iGetIndex = string.IsNullOrEmpty(row.Cells["LineNum"].Value.ToString()) ? (CartonItem.items.ToList().Count() > 0 ? CartonItem.items.Max(y => y.Index) + 1 : FrmCartonManagement.ItemIndex++) : Convert.ToInt32(row.Cells["LineNum"].Value.ToString());
                        CartonItem.items.Add(new CartonItem.Item
                        {
                            //Index = FrmCartonManagement.ItemIndex++,
                            Index = iGetIndex,
                            ItemCode = row.Cells["Item Code"].Value.ToString(),
                            Description = row.Cells["Description"].Value.ToString(),
                            Quantity = row.Cells["Quantity"].Value.ToString(),
                            BasedDocEntry = oDocEntry
                        });
                    }
                }

                gvSelectedItem.Columns.Clear();
                txtSubCategory.Text = "";

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}

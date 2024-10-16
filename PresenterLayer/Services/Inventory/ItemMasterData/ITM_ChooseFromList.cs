using Context;
using DirecLayer;
using PresenterLayer.Helper;
using PresenterLayer.Views;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PresenterLayer.Services
{
    public class ITM_ChooseFromList
    {
        public frmItemMasterData frmItemMasterData { get; set; }
        public frmITM_ChooseFromList frmCFL { get; set; }
        SAPHanaAccess sapHana = new SAPHanaAccess();
        DataHelper helper = new DataHelper();
        string sQuery { get; set; }
        TextBox txtCode { get; set; }
        TextBox txtName { get; set; }

        int iColumn { get; set; } = 0;

        public string SubCategoryCode { get; set; }

        public void GetBrands()
        {
            sQuery = helper.ReadDataRow(sapHana.Get(SP.ITM_Brands),1,"",0);
            txtCode = frmItemMasterData.U_ID001;
            txtName = frmItemMasterData.U_Name_ID001;
            LoadCFL("Brands");
        }

        public void GetCategories()
        {
            if (!string.IsNullOrEmpty(frmItemMasterData.U_ID001.Text) &&
                !string.IsNullOrEmpty(frmItemMasterData.U_ID002.Text) &&
                !string.IsNullOrEmpty(frmItemMasterData.U_ID003.Text))
            {
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_CategoryByBrandDeptSubDept), 1,"",0),
                                        frmItemMasterData.U_ID001.Text.Replace("'", "''"),
                                        frmItemMasterData.U_ID002.Text.Replace("'", "''"),
                                        frmItemMasterData.U_ID003.Text.Replace("'", "''"));
                txtCode = frmItemMasterData.U_ID020;
                txtName = frmItemMasterData.U_ID004;
                LoadCFL("Categories");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand, department and/or sub-department first!", true); }
        }

        public void GetDepartments()
        {
            if (!string.IsNullOrEmpty(frmItemMasterData.U_ID001.Text))
            {
                txtCode = frmItemMasterData.U_ID002;
                txtName = null;
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_DepartmentsByBrand), 1,"",0),
                                        frmItemMasterData.U_ID001.Text);
                LoadCFL("Departments");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand first!", true); }
        }

        public void GetSubDepartments()
        {
            if (!string.IsNullOrEmpty(frmItemMasterData.U_ID001.Text) &&
                !string.IsNullOrEmpty(frmItemMasterData.U_ID002.Text))
            {
                txtCode = frmItemMasterData.U_ID003;
                txtName = null;
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_SubDepartmentsByBrandDept), 1,"",0),
                                        frmItemMasterData.U_ID001.Text.Replace("'","''"),
                                        frmItemMasterData.U_ID002.Text.Replace("'", "''"));
                LoadCFL("Sub-Departments");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand and/or department first!", true); }
        }

        public void GetSuppliers()
        {
            sQuery = helper.ReadDataRow(sapHana.Get(SP.ITM_Suppliers), 1,"",0);
            txtCode = frmItemMasterData.CardCode;
            txtName = frmItemMasterData.CardName;
            LoadCFL("Suppliers");
        }

        public void GetSubCategories()
        {
            if (!string.IsNullOrEmpty(frmItemMasterData.U_ID001.Text) &&
                !string.IsNullOrEmpty(frmItemMasterData.U_ID020.Text))
            {
                txtCode = frmItemMasterData.U_ID021;
                txtName = frmItemMasterData.U_ID005;
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_SubCategoryByBrandCategory), 1,"",0),
                                        frmItemMasterData.U_ID001.Text.Replace("'", "''"),
                                        frmItemMasterData.U_ID020.Text.Replace("'", "''"));
                LoadCFL("Sub-Categories");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand and/or category first!", true); }
        }

        public void GetStyles()
        {
            string Brand = frmItemMasterData.U_ID001.Text.Replace("'", "''");
            string Category = frmItemMasterData.U_ID020.Text.Replace("'", "''");
            string SubCategory = frmItemMasterData.U_ID021.Text.Replace("'", "''");
            
            if (!string.IsNullOrEmpty(Brand) &&
                !string.IsNullOrEmpty(Category) &&
                !string.IsNullOrEmpty(SubCategory))
            {
                txtCode = frmItemMasterData.U_ID012;
                txtName = frmItemMasterData.U_ID025;
                SubCategoryCode = $"{Brand}{Category}{SubCategory}";
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_Style), 1,"",0),
                                        SubCategoryCode);
                LoadCFL("Styles");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand, category and/or sub-category first!", true); }
        }

        public void GetClass()
        {
            string Brand = frmItemMasterData.U_ID001.Text;
            string Category = frmItemMasterData.U_ID020.Text;
            string SubCategory = frmItemMasterData.U_ID021.Text;

            if (!string.IsNullOrEmpty(Brand) &&
                !string.IsNullOrEmpty(Category) &&
                !string.IsNullOrEmpty(SubCategory))
            {
                txtCode = frmItemMasterData.U_ID013;
                txtName = null;
                SubCategoryCode = $"{Brand}{Category}{SubCategory}";
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_Class), 1,"",0),
                                        SubCategoryCode);
                LoadCFL("Classes");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand, category and/or sub-category first!", true); }
        }

        public void GetSubClass()
        {
            string Brand = frmItemMasterData.U_ID001.Text;
            string Category = frmItemMasterData.U_ID020.Text;
            string SubCategory = frmItemMasterData.U_ID021.Text;

            if (!string.IsNullOrEmpty(Brand) &&
                !string.IsNullOrEmpty(Category) &&
                !string.IsNullOrEmpty(SubCategory) &&
                !string.IsNullOrEmpty(frmItemMasterData.U_ID013.Text))
            {
                txtCode = frmItemMasterData.U_ID014;
                txtName = null;
                SubCategoryCode = $"{Brand}{Category}{SubCategory}";
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_SubClass), 1,"",0),
                                        SubCategoryCode, frmItemMasterData.U_ID013.Text);
                LoadCFL("Sub-Classes");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand, category, sub-category and/or class first!", true); }
        }

        public void GetPackaging()
        {
            string Brand = frmItemMasterData.U_ID001.Text.Replace("'", "''");
            string Category = frmItemMasterData.U_ID020.Text.Replace("'", "''");
            string SubCategory = frmItemMasterData.U_ID021.Text.Replace("'", "''");

            if (!string.IsNullOrEmpty(Brand) &&
                !string.IsNullOrEmpty(Category) &&
                !string.IsNullOrEmpty(SubCategory))
            {
                txtCode = frmItemMasterData.U_ID015;
                txtName = null;
                SubCategoryCode = $"{Brand}{Category}{SubCategory}";
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_Packaging), 1,"",0),
                                        SubCategoryCode);
                LoadCFL("Packaging");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand, category and/or sub-category first!", true); }
        }

        public void GetSpecifications()
        {
            string Brand = frmItemMasterData.U_ID001.Text.Replace("'", "''");
            string Category = frmItemMasterData.U_ID020.Text.Replace("'", "''");
            string SubCategory = frmItemMasterData.U_ID021.Text.Replace("'", "''");
            
            if (!string.IsNullOrEmpty(Brand) &&
                !string.IsNullOrEmpty(Category) &&
                !string.IsNullOrEmpty(SubCategory))
            {
                txtCode = frmItemMasterData.U_ID016;
                txtName = null;
                SubCategoryCode = $"{Brand}{Category}{SubCategory}";
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_Specs), 1,"",0),
                                        SubCategoryCode);
                LoadCFL("Specifications");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand, category and/or sub-category first!", true); }
        }

        public void GetColections()
        {
            string Brand = frmItemMasterData.U_ID001.Text.Replace("'", "''");
            string Category = frmItemMasterData.U_ID020.Text.Replace("'", "''");
            string SubCategory = frmItemMasterData.U_ID021.Text.Replace("'", "''");

            if (!string.IsNullOrEmpty(Brand) &&
                !string.IsNullOrEmpty(Category) &&
                !string.IsNullOrEmpty(SubCategory))
            {
                txtCode = frmItemMasterData.U_ID017;
                txtName = null;
                SubCategoryCode = $"{Brand}{Category}{SubCategory}";
                sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_Collect), 1,"",0),
                                        SubCategoryCode);
                LoadCFL("Collections");
            }
            else
            { StaticHelper._MainForm.ShowMessage("Please choose a brand, category and/or sub-category first!", true); }
        }

        public void GetLastRecord()
        {
            var dgv = frmCFL.dgvChooseFromList;
            if (dgv.Rows.Count >= 1)
            {
                int iRow = dgv.Rows.Count - 2;
                dgv.CurrentCell = dgv.Rows[iRow].Cells[0];
                dgv.FirstDisplayedScrollingRowIndex = iRow;
                dgv.BeginEdit(true);
            }
        }

        public void GetInvntryUom(string ItmGrp)
        {
            txtCode = frmItemMasterData.InvntryUom;
            txtName = null;
            sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_Uom), 1,"",0), ItmGrp);
            LoadCFL("Units of Measure");
        }

        public void GetSalUnitMsr(string ItmGrp)
        {
            txtCode = frmItemMasterData.SalUnitMsr;
            txtName = null;
            sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_Uom), 1,"",0), ItmGrp);
            LoadCFL("Units of Measure");
        }

        public void GetBuyUnitMsr(string ItmGrp)
        {
            txtCode = frmItemMasterData.BuyUnitMsr;
            txtName = null;
            sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_Uom), 1, "", 0), ItmGrp);
            LoadCFL("Units of Measure");
        }

        public void GetSize()
        {
            frmITM_DefineNewCustom frmNewCustom = new frmITM_DefineNewCustom();
            frmNewCustom.MdiParent = StaticHelper._MainForm;
            frmNewCustom.frmIMD = frmItemMasterData;
            frmNewCustom.Text = "Define New Size";
            frmNewCustom.Show();
            
        }

        public void GetColors()
        {
            frmITM_DefineNewCustom frmNewCustom = new frmITM_DefineNewCustom();
            frmNewCustom.MdiParent = StaticHelper._MainForm;
            frmNewCustom.frmIMD = frmItemMasterData;
            frmNewCustom.Text = "Define New Color";
            frmNewCustom.Show();
            
        }

        void LoadCFL(string sTitle)
        {
            frmITM_ChooseFromList frm = new frmITM_ChooseFromList();
            frmCFL = frm;
            frm.cfl = this;
            frm.Text = sTitle;
            frm.MdiParent = StaticHelper._MainForm;
            frm.Show();
        }
        
        public void LoadForm()
        {
            DataGridView dgv = frmCFL.dgvChooseFromList;
            dgv.Columns.Clear();
            dgv.DataSource = sapHana.Get(sQuery);
            dgvSetup(dgv);

            if (dgv.Rows.Count == 1)
            {
                dgv.Rows[0].Selected = true;
                if (dgv.Rows[0].Cells[0].Value.ToString() != "Define New")
                { btnCommand(); }

            }
            else if (dgv.Rows.Count == 0)
            {
                StaticHelper._MainForm.ShowMessage("No Data Found!", true);
                frmCFL.BeginInvoke(new MethodInvoker(frmCFL.Dispose));
            }
            dgv.Focus();
        }

        void dgvSetup(DataGridView dt)
        {
            try
            {
                dt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dt.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dt.MultiSelect = false;
                dt.RowTemplate.Resizable = DataGridViewTriState.False;
                dt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dt.RowHeadersVisible = false;
                dt.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dt.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        public void btnCancel()
        { frmCFL.Dispose(); }

        string GetQuery(string sSP)
        {
            return string.Format(helper.ReadDataRow(sapHana.Get(sSP), 1,"",0),
                                                frmItemMasterData.U_ID001.Text,
                                                frmItemMasterData.U_ID020.Text,
                                                frmItemMasterData.U_ID021.Text,
                                                frmItemMasterData.U_ID002.Text.Replace("'","''"),
                                                frmItemMasterData.U_ID003.Text.Replace("'","''"));
        }

        public void DefineNew(string sQuery)
        {
            DataTable dt = new DataTable();

            dt = sapHana.Get(sQuery);

            frmITM_DefineNew definenew = new frmITM_DefineNew();
            definenew.Text = frmCFL.Text;
            definenew.cfl = this;
            definenew.MdiParent = StaticHelper._MainForm;
            definenew.Show();
        }

        public void dgvSearch_ColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            var dgv = frmCFL.dgvChooseFromList;
            if (dgv.Rows.Count > 0)
            { iColumn = dgv.Columns[e.ColumnIndex].Index; }

        }

        public void txtSearch_TextChanged(EventArgs e)
        {
            try
            {
                if (frmCFL.dgvChooseFromList.Columns.Count >0)
                {
                    foreach (DataGridViewRow row in frmCFL.dgvChooseFromList.Rows)
                    {
                        if (row.Cells[iColumn].Value.ToString().ToUpper().Contains(frmCFL.txtSearch.Text.ToUpper()))
                        {
                            row.Selected = true;
                            frmCFL.dgvChooseFromList.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                        else
                        { row.Selected = false; }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void btnCommand()
        {
            DataGridView dgv = frmCFL.dgvChooseFromList;
            int i = dgv.SelectedRows[0].Index;
            if (i < 0)
            { StaticHelper._MainForm.ShowMessage("No matching records found", true); }
            else if (dgv.Rows[i].Cells[0].Value.ToString() == "Define New")
            {
                DataTable dt = new DataTable();
                string query = "",
                       Code = "",
                       Name = "";
             
                switch (frmCFL.Text)
                {
                    case "Styles":
                        query = GetQuery(SP.ITM_NewDataStyle);

                        Code = GetQuery(SP.ITM_NewStyle);

                        Code = helper.ReadDataRow(sapHana.Get(Code),0,"",0).PadLeft(4, '0');
                        Name = Code;
                        break;
                    case "Classes":
                        query = GetQuery(SP.ITM_NewDataClass);
                        Code = "";
                        Name = "";
                        break;
                    case "Sub-Classes":
                        query = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_NewDataSubClass), 1, "", 0),
                                                frmItemMasterData.U_ID001.Text,
                                                frmItemMasterData.U_ID020.Text,
                                                frmItemMasterData.U_ID021.Text,
                                                frmItemMasterData.U_ID002.Text,
                                                frmItemMasterData.U_ID003.Text,
                                                frmItemMasterData.U_ID013.Text);
                        break;
                    case "Packaging":
                        query = GetQuery(SP.ITM_NewDataPackage);
                        Code = "";
                        Name = "";
                        break;
                    case "Specifications":
                        query = GetQuery(SP.ITM_NewDataSpecs);
                        Code = "";
                        Name = "";
                        break;
                    case "Collections":
                        query = GetQuery(SP.ITM_NewDataCollect);
                        Code = "";
                        Name = "";
                        break;
                }

                dt = sapHana.Get(query);

                frmITM_DefineNew definenew = new frmITM_DefineNew();
                definenew.Text = frmCFL.Text;
                definenew.cfl = this;
                definenew.MdiParent = StaticHelper._MainForm;
                definenew.Show();

                foreach (DataRow dr in dt.Rows)
                { definenew.dgvDefine.Rows.Add(dr[0].ToString(), dr[1].ToString(),false); }

                definenew.dgvDefine.Rows.Add();

                definenew.PopulateData(Code, Name);
            }
            else
            {
                var frm = frmItemMasterData;

                if (txtCode == frm.U_ID001)
                {
                    frm.U_ID002.Clear();
                    frm.U_ID003.Clear();
                    frm.U_ID020.Clear();
                    frm.U_ID004.Clear();
                    frm.U_ID021.Clear();
                    frm.U_ID005.Clear();
                    frm.U_ID012.Clear();
                    frm.U_ID025.Clear();
                    frm.U_ID013.Clear();
                    frm.U_ID014.Clear();
                    frm.U_ID015.Clear();
                    frm.U_ID016.Clear();
                    frm.U_ID017.Clear();
                }
                else if (txtCode == frm.U_ID002)
                {
                    frm.U_ID003.Clear();
                    frm.U_ID020.Clear();
                    frm.U_ID004.Clear();
                    frm.U_ID021.Clear();
                    frm.U_ID005.Clear();
                    frm.U_ID012.Clear();
                    frm.U_ID025.Clear();
                    frm.U_ID013.Clear();
                    frm.U_ID014.Clear();
                    frm.U_ID015.Clear();
                    frm.U_ID016.Clear();
                    frm.U_ID017.Clear();
                }
                else if (txtCode == frm.U_ID003)
                {
                    frm.U_ID020.Clear();
                    frm.U_ID004.Clear();
                    frm.U_ID021.Clear();
                    frm.U_ID005.Clear();
                    frm.U_ID012.Clear();
                    frm.U_ID025.Clear();
                    frm.U_ID013.Clear();
                    frm.U_ID014.Clear();
                    frm.U_ID015.Clear();
                    frm.U_ID016.Clear();
                    frm.U_ID017.Clear();
                }
                else if (txtCode == frm.U_ID020)
                {
                    frm.U_ID021.Clear();
                    frm.U_ID005.Clear();
                    frm.U_ID012.Clear();
                    frm.U_ID025.Clear();
                    frm.U_ID013.Clear();
                    frm.U_ID014.Clear();
                    frm.U_ID015.Clear();
                    frm.U_ID016.Clear();
                    frm.U_ID017.Clear();
                }
                else if (txtCode == frm.U_ID021)
                {
                    frm.U_ID012.Clear();
                    frm.U_ID025.Clear();
                    frm.U_ID013.Clear();
                    frm.U_ID014.Clear();
                    frm.U_ID015.Clear();
                    frm.U_ID016.Clear();
                    frm.U_ID017.Clear();
                }
                else if (txtCode == frm.U_ID012)
                {
                    frm.U_ID013.Clear();
                    frm.U_ID014.Clear();
                    frm.U_ID015.Clear();
                    frm.U_ID016.Clear();
                    frm.U_ID017.Clear();
                }
                else if (txtCode == frm.U_ID013)
                {
                    frm.U_ID014.Clear();
                    frm.U_ID015.Clear();
                    frm.U_ID016.Clear();
                    frm.U_ID017.Clear();
                }
                else if (txtCode == frm.U_ID014)
                {
                    frm.U_ID015.Clear();
                    frm.U_ID016.Clear();
                    frm.U_ID017.Clear();
                }
                else if (txtCode == frm.U_ID015)
                {
                    frm.U_ID016.Clear();
                    frm.U_ID017.Clear();
                }
                else if (txtCode == frm.U_ID016)
                { frm.U_ID017.Clear(); }

                txtCode.Text = dgv.Rows[i].Cells[0].Value.ToString();
                if (txtName != null)
                { txtName.Text = dgv.Rows[i].Cells[1].Value.ToString(); }
                frmCFL.BeginInvoke(new MethodInvoker(frmCFL.Dispose));
            }
        }
    }
}

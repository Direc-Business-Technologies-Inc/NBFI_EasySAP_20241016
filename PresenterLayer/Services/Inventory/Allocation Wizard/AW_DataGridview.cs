using Context;
using DirecLayer;
using DomainLayer.Models.Inventory.Allocation_Wizard;
using PresenterLayer;
using PresenterLayer.Helper;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PresenterLayer
{
    class AW_DataGridview
    {
        public frmAllocationWizard form { get; set; }
        private DataHelper helper { get; set; }
        private SAPHanaAccess sapHana { get; set; }
        public void DataGridView_Load(string sVal)
        {
            helper = new DataHelper();
            sapHana = new SAPHanaAccess();
            var dgv = new DataGridView();
            var dt = new DataTable();

            switch (sVal)
            {
                case "AllocWizRuns":
                    dgv = form.dgvAllocWizRuns;
                    dt = sapHana.Get(SP.AW_AllocWizRuns);
                    SelectionSetup(dgv, dt);
                    break;
                case "ApprovedRuns":
                    dgv = form.dgvApprovedRuns;
                    
                    var sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_ApprovedRuns), 1, "", 0),
                                                                   form.dtpApprovedFrom.Value.ToString("yyyyMMdd"),
                                                                   form.dtpApprovedTo.Value.ToString("yyyyMMdd"));
                    
                    dt = sapHana.Get(sQuery);
                    SelectionSetup(dgv, dt);
                    break;
                case "Whs":
                    dgv = form.dgvWhs;
                    dgv.Rows.Clear();
                    dgv.Columns.Clear();
                    dt = sapHana.Get(SP.AW_WHS);
                    CheckboxSetup(dgv, dt);
                    break;

                case "MarketingDocs":
                    dgv = form.dgvMarketingDocs;

                    //if (form.rbCreateNewAlloc.Checked)
                    //{ dt = SAPHana.GetHana(SP.AW_MD); }
                    //else if (form.rbRepeatOrder.Checked)
                    //{ dt = SAPHana.GetHana(SP.AW_RO_MD); }
                    
                    dt = form.rbCreateNewAlloc.Checked ? sapHana.Get(SP.AW_MD) : sapHana.Get(SP.AW_RO_MD);

                    dgv.Columns.Clear();

                    ButtonSetup(dgv, dt);

                    break;
                case "ItemOtherParam":
                    dgv = form.dgvItemOtherParam;
                    dgv.Rows.Clear();
                    dgv.Columns.Clear();
                    if (form.rbRepeatOrder.Checked)
                    {
                        var modelIOP = SelectionModel.Selection.Where(x => x.Type == "IOP");
                        if (modelIOP.Any())
                        {
                            foreach (var item in modelIOP.ToList())
                            { SelectionModel.Selection.Remove(item); }

                        }
                    }
                    OtherParamSetup(dgv, SP.AW_IOPData);
                    break;
                case "ItemSelection":
                    dgv = form.dgvItemSelection;
                    SelectionSetup(dgv);
                    break;
                case "ItemSelected":
                    dgv = form.dgvItemSelected;
                    SelectionSetup(dgv);
                    break;
                case "AllocationBase":
                    dgv = form.dgvAllocationBase;
                    if (dgv.Rows.Count > 0)
                    {
                        dgv.CurrentCell = dgv.Rows[0].Cells[0];
                        dgv.Rows.Clear();
                        dgv.Columns.Clear();
                    }
                    string aBrand = form.dgvItemOtherParam.Rows[0].Cells["Value"].Value.ToString();
                    dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetClassify), 1, "", 0), aBrand));
                    SelectionSetup(dgv, dt);
                    break;
                case "StoreCriteria":
                    dgv = form.dgvStoreCriteria;
                    if (form.rbCreateNewAlloc.Checked)
                    {
                        dgv.Enabled = true;
                        dgv = form.dgvStoreCriteria;
                        dt = sapHana.Get(SP.AW_SC);
                        ButtonSetup(dgv, dt);
                    }
                    else if (form.rbRepeatOrder.Checked)
                    { dgv.Enabled = false; }
                    break;
                case "CustOtherParam":
                    if (form.rbCreateNewAlloc.Checked)
                    {
                        form.btnCustNewParam.Enabled = true;
                        dgv = form.dgvCustOtherParam;
                        OtherParamSetup(dgv);
                    }
                    else if (form.rbRepeatOrder.Checked)
                    { form.btnCustNewParam.Enabled = false; }
                    break;
                case "CustSelection":
                    dgv = form.dgvCustSelection;
                    SelectionSetup(dgv);
                    break;
                case "CustSelected":
                    dgv = form.dgvCustSelected;
                    if (form.rbCreateNewAlloc.Checked)
                    {
                        if (dgv.Rows.Count > 0)
                        {
                            dgv.Rows.Clear();
                        }
                        var cBrand = form.dgvItemOtherParam.Rows[0].Cells["Value"].Value.ToString();
                        dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetCustomer), 1, "", 0), cBrand));
                        SelectionSetup(dgv, dt);
                    }
                    else if (form.rbRepeatOrder.Checked)
                    {

                        var module = form.dgvMarketingDocs;
                        for (int i = 0; i < module.Rows.Count; i++)
                        {
                            var sModule = module.Rows[i].Cells["Code"].Value.ToString();
                            var lMarketingDocs = SelectionModel.Selection.Where(x => x.Type == "MD" && x.TableID == sModule);
                            if (lMarketingDocs.Any())
                            {
                                StringBuilder sbDocEntries = new StringBuilder();

                                foreach (var dr in lMarketingDocs.ToList())
                                { sbDocEntries.Append($"{dr.ID},"); }

                                StringBuilder sbItemCode = new StringBuilder();

                                var dgvItems = form.dgvItemSelected;

                                for (int iItems = 0; iItems < dgvItems.Rows.Count; iItems++)
                                {
                                    var Qty = dgvItems.Rows[iItems].Cells["Allocate"].Value;
                                    if (!string.IsNullOrEmpty(Qty != null ? Qty.ToString() : ""))
                                    { sbItemCode.Append($"'{dgvItems.Rows[iItems].Cells[0].Value.ToString()}',"); }
                                }

                                var sbQuery = new StringBuilder();
                                sbQuery.AppendFormat(helper.ReadDataRow(sapHana.Get(SP.AW_GetROStores), 1, "", 0), sModule.Remove(sModule.Length - 1), sModule, sbItemCode, sbDocEntries.ToString());

                                if (!string.IsNullOrEmpty(sbItemCode.ToString()))
                                {
                                    dgv.Rows.Clear();
                                    dt = sapHana.Get(sbQuery.ToString());
                                    SelectionSetup(dgv, dt);
                                }
                                else
                                { StaticHelper._MainForm.ShowMessage($"Please allocate quantity first.", true); }
                            }
                        }
                    }
                    break;
                case "Levels":
                    dgv = form.dgvLevels;
                    if (dgv.Rows.Count > 0)
                    {
                        dgv.Rows.Clear();
                        dgv.Columns.Clear();
                    }
                    dt = sapHana.Get(SP.AW_LVL);
                    ComboBoxLevelSetup(dgv, dt);
                    break;
                case "SalesCritera":
                    dgv = form.dgvSalesCritera;
                    if (dgv.Rows.Count > 0)
                    {
                        dgv.Rows.Clear();
                        dgv.Columns.Clear();
                    }
                    dt = sapHana.Get(SP.AW_ISC);
                    ComboBoxSetup(dgv, dt);
                    break;
                case "Allocation":
                    dgv = form.dgvAllocation;
                    if (dgv.Rows.Count > 0)
                    { dgv.Rows.Clear(); }
                    dgvAllocation(dgv);
                    break;
                case "Parameters":
                    dgv = form.dgvParameter;
                    dt = sapHana.Get(SP.AW_SMRY);
                    if (dgv.Rows.Count > 0)
                    { dgv.Rows.Clear(); }

                    ComboBoxParamSetup(dgv, dt);
                    break;
                case "Summary":
                    dgv = form.dgvSummary;
                    SummarySetup(dgv);
                    break;
                default:
                    break;
            }
            dgvSetup(dgv);
        }

        void CheckboxSetup(DataGridView dgv, DataTable dt)
        {
            dgvDesign(dgv);
            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
            dgv.Columns.Add(col);

            dgv.ColumnCount = dt.Columns.Count;

            for (int i = 0; i < dt.Columns.Count; i++)
            { dgv.Columns[i].Name = dt.Columns[i].ColumnName; }
            
            for (int iAdd = 0; iAdd < dt.Rows.Count; iAdd++)
            {
                var dr = dt.Rows[iAdd];
                dgv.Rows.Add(dr[0], dr[1], dr[2]);
            }
            
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns[1].Visible = false;
            dgv.Columns[2].ReadOnly = true;

        }

        void ButtonSetup(DataGridView dgv, DataTable dt)
        {
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns.Clear();
            }
            dgvDesign(dgv);
            dgv.DataSource = dt;

            dgv.Columns[0].Visible = false;
            dgv.Columns[2].Visible = false;
            dgv.Columns[3].Visible = false;

            var col = new DataGridViewButtonColumn();
            dgv.Columns.Add(col);
            dgv.Columns[4].Name = "btn";
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        void ComboBoxLevelSetup(DataGridView dgv, DataTable dt)
        {
            dgvDesign(dgv);
            dgv.RowHeadersVisible = true;
            dgv.ColumnCount = 3;
            dgv.Columns[0].Name = "Code";
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Name = "Query";
            dgv.Columns[1].Visible = false;
            dgv.Columns[2].Name = "Field";
            string[] row = new string[] { "", "" };
            dgv.Rows.Add(row);

            var cmb = new DataGridViewComboBoxCell();

            cmb.DisplayMember = "Name";
            cmb.ValueMember = "Code";
            cmb.DataSource = dt;
            dgv.Rows[0].Cells["Field"] = cmb;
        }

        void ComboBoxSetup(DataGridView dgv, DataTable dt)
        {
            dgvDesign(dgv);
            dgv.ColumnCount = 4;
            dgv.Columns[0].Name = "Code";
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Name = "Name";
            dgv.Columns[2].Name = "Query";
            dgv.Columns[2].Visible = false;
            dgv.Columns[3].Name = "Field";

            var sBrand = "";
            var sDept = "";
            var sSubDept = "";
            var sCat = "";
            var sSubCat = "";
            if (!form.rbAllocationApproval.Checked)
            {
                // 1. Brand
                sBrand = form.dgvItemOtherParam.Rows[0].Cells["Value"].Value != null ? form.dgvItemOtherParam.Rows[0].Cells["Value"].Value.ToString() : "";
                // 2. Department
                sDept = form.dgvItemOtherParam.Rows[1].Cells["Value"].Value != null ? form.dgvItemOtherParam.Rows[1].Cells["Value"].Value.ToString() : "";
                // 3. Sub-Department
                sSubDept = form.dgvItemOtherParam.Rows[2].Cells["Value"].Value != null ? form.dgvItemOtherParam.Rows[2].Cells["Value"].Value.ToString() : "";
                // 4. Category
                sCat = form.dgvItemOtherParam.Rows[3].Cells["Value"].Value != null ? form.dgvItemOtherParam.Rows[3].Cells["Value"].Value.ToString() : "";
                // 5. Sub-Category
                sSubCat = form.dgvItemOtherParam.Rows[4].Cells["Value"].Value != null ? form.dgvItemOtherParam.Rows[4].Cells["Value"].Value.ToString() : "";
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                string[] row = new string[] { dr[0].ToString(), dr[1].ToString(), dr[3].ToString() };
                dgv.Rows.Add(row);
                if (!form.rbAllocationApproval.Checked)
                {
                    var dtcmb = new DataTable();
                    string sQuery = dr["Query"].ToString();

                    switch (i)
                    {
                        case 0:
                            dtcmb = sapHana.Get(sQuery);
                            break;
                        case 1:
                            dtcmb = sapHana.Get(string.Format(sQuery, sBrand));
                            break;
                        case 2:
                            dtcmb = sapHana.Get(string.Format(sQuery, sBrand, sDept));
                            break;
                        case 3:
                            dtcmb = sapHana.Get(string.Format(sQuery, sBrand, sDept, sSubDept));
                            break;
                        case 4:
                            dtcmb = sapHana.Get(string.Format(sQuery, sBrand, sCat));
                            break;
                        default:
                            break;
                    }

                    var cmb = new DataGridViewComboBoxCell();
                    cmb.DisplayMember = "Name";
                    cmb.ValueMember = "Code";
                    cmb.DataSource = dtcmb;
                    dgv.Rows[i].Cells["Field"] = cmb;

                    switch (i)
                    {
                        case 0:
                            dgv.Rows[i].Cells["Field"].Value = sBrand;
                            break;
                        case 1:
                            dgv.Rows[i].Cells["Field"].Value = sDept;
                            break;
                        case 2:
                            dgv.Rows[i].Cells["Field"].Value = sSubDept;
                            break;
                        case 3:
                            dgv.Rows[i].Cells["Field"].Value = sCat;
                            break;
                        case 4:
                            dgv.Rows[i].Cells["Field"].Value = sSubCat;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void ComboBoxParamSetup(DataGridView dgv, DataTable dt)
        {
            dgvDesign(dgv);
            dgv.ColumnCount = 3;
            dgv.Columns[0].Name = "Code";
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Name = "Name";
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[2].Name = "Field";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                var sQuery = dr["Query"].ToString();
                string[] row = new string[] { dr["Code"].ToString(), dr["Name"].ToString() };
                dgv.Rows.Add(row);

                if (dr["Type"].ToString() == "Many")
                {
                    var dtcmb = new DataTable();
                    dtcmb = sapHana.Get(sQuery);

                    var cmb = new DataGridViewComboBoxCell();
                    cmb.DisplayMember = "Name";
                    cmb.ValueMember = "Code";
                    cmb.DataSource = dtcmb;
                    dgv.Rows[dgv.Rows.Count - 1].Cells["Field"] = cmb;
                }
            }
        }

        void SummarySetup(DataGridView dgv)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersVisible = true;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.ScrollBars = ScrollBars.Both;

            dgv.Columns.Add("CardCode", "Customer Code");
            dgv.Columns.Add("CardName", "Customer Name");
            dgv.Columns.Add("Classification", "Classification");
            dgv.Columns.Add("ItemCode", "Item No.");
            dgv.Columns.Add("CodeBars", "Bar Code");
            dgv.Columns.Add("ItemName", "Description");
            dgv.Columns.Add("Style", "Style");
            dgv.Columns.Add("Color", "Color");
            dgv.Columns.Add("Size", "Size");
            dgv.Columns.Add("Class", "Class");
            dgv.Columns.Add("SubClass", "Subclass");
            dgv.Columns.Add("Collection", "Collection");
            dgv.Columns.Add("Packaging", "Packaging");
            dgv.Columns.Add("Specification", "Specification");
            dgv.Columns.Add("Section", "Section");
            dgv.Columns.Add("ItemClass", "Item Class");
            dgv.Columns.Add("Qty", "Quantity");

            if (form.rbCreateNewAlloc.Checked)
            {
                var max = form.dgvAllocation.Rows.Count;
                for (int i = 0; i < max; i++)
                {
                    StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {i + 1} out of {max}", i + 1, max);
                    var dr = form.dgvAllocation.Rows[i];
                    var sAllocated = dr.Cells["Allocated"].Value.ToString();
                    if (double.Parse(sAllocated) > 0)
                    {
                        string[] row = new string[] { dr.Cells["CardCode"].Value.ToString()
                                                , dr.Cells["CardName"].Value.ToString()
                                                , dr.Cells["Classification"].Value.ToString()
                                                , dr.Cells["ItemCode"].Value.ToString()
                                                , dr.Cells["CodeBars"].Value.ToString()
                                                , dr.Cells["ItemName"].Value.ToString()
                                                , dr.Cells["Style"].Value.ToString()
                                                , dr.Cells["Color"].Value.ToString()
                                                , dr.Cells["Size"].Value.ToString()
                                                , dr.Cells["Class"].Value.ToString()
                                                , dr.Cells["SubClass"].Value.ToString()
                                                , dr.Cells["Collection"].Value.ToString()
                                                , dr.Cells["Packaging"].Value.ToString()
                                                , dr.Cells["Specification"].Value.ToString()
                                                , dr.Cells["Section"].Value.ToString()
                                                , dr.Cells["ItemClass"].Value.ToString()
                                                , dr.Cells["Allocated"].Value.ToString()};
                        dgv.Rows.Add(row);
                    }
                }
            }
            else if (form.rbRepeatOrder.Checked || form.rbAllocationApproval.Checked)
            {
                dgv.Columns.Add("DocEntry", "DocEntry");
                dgv.Columns[17].Visible = false;
                dgv.Columns.Add("LineNum", "LineNum");
                dgv.Columns[18].Visible = false;

                var sbDocEntries = new StringBuilder();
                var model = SelectionModel.Selection.Where(x => x.Type == "MD");

                foreach (var drDocEntry in model.ToList())
                { sbDocEntries.Append($"{drDocEntry.ID},"); }

                var sbQuery = new StringBuilder();
                

                sbQuery.AppendFormat(helper.ReadDataRow(sapHana.Get(SP.AW_GetITRbyDocEntry), 1, "", 0), sbDocEntries.ToString());

                var dtData = new DataTable();
                dtData = sapHana.Get(sbQuery.ToString());
                var max = dtData.Rows.Count;
                for (int i = 0; i < max; i++)
                {
                    StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {i + 1} out of {max}", i + 1, max);
                    var drData = dtData.Rows[i];
                    var sCardCode = drData["CardCode"].ToString();
                    var sItemCode = drData["ItemCode"].ToString();
                    var iDocEntry = drData["DocEntry"].ToString();
                    var iLineNum = drData["LineNum"].ToString();

                    for (int iAlloc = 0; iAlloc < form.dgvAllocation.Rows.Count; iAlloc++)
                    {
                        var dr = form.dgvAllocation.Rows[iAlloc];

                        if (sCardCode == dr.Cells["CardCode"].Value.ToString() && sItemCode == dr.Cells["ItemCode"].Value.ToString())
                        {
                            var sAllocated = dr.Cells["Allocated"].Value == null ? "" : dr.Cells["Allocated"].Value.ToString();

                            if (double.Parse(sAllocated) > 0)
                            {
                                double dQty = double.Parse(sAllocated);
                                double dDocQty = double.Parse(dr.Cells["Allocated"].Value.ToString());

                                if (dQty > dDocQty)
                                { sAllocated = dDocQty.ToString(); }
                                else if (dQty < dDocQty)
                                { sAllocated = dQty.ToString(); }

                                string[] row = new string[] { sCardCode
                                                    , dr.Cells["CardName"].Value.ToString()
                                                    , dr.Cells["Classification"].Value.ToString()
                                                    , sItemCode
                                                    , dr.Cells["CodeBars"].Value.ToString()
                                                    , dr.Cells["ItemName"].Value.ToString()
                                                    , dr.Cells["Style"].Value.ToString()
                                                    , dr.Cells["Color"].Value.ToString()
                                                    , dr.Cells["Size"].Value.ToString()
                                                    , dr.Cells["Class"].Value.ToString()
                                                    , dr.Cells["SubClass"].Value.ToString()
                                                    , dr.Cells["Collection"].Value.ToString()
                                                    , dr.Cells["Packaging"].Value.ToString()
                                                    , dr.Cells["Specification"].Value.ToString()
                                                    , dr.Cells["Section"].Value.ToString()
                                                    , dr.Cells["ItemClass"].Value.ToString()
                                                    , sAllocated
                                                    , iDocEntry
                                                    , iLineNum};
                                dgv.Rows.Add(row);
                            }
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < dgv.Columns.Count; i++)
            { dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable; }
            
        }

        void OtherParamSetup(DataGridView dgv, string sQuery = "")
        {
            dgvDesign(dgv);

            if (dgv.Columns.Count > 0)
            { dgv.Columns.Clear(); }


            dgv.ColumnCount = 5;

            dgv.Columns[0].Name = "Code";
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[1].Name = "Name";
            dgv.Columns[2].ReadOnly = true;
            dgv.Columns[2].Name = "Value";
            dgv.Columns[3].Visible = false;
            dgv.Columns[3].Name = "Type";
            dgv.Columns[4].Visible = false;
            dgv.Columns[4].Name = "Query";

            dgv.Rows.Clear();
            if (!string.IsNullOrEmpty(sQuery))
            {
                var dt = new DataTable();
                dt = sapHana.Get(sQuery);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    dgv.Rows.Add(dr[0], dr[1], "", dr[2], dr[3]);
                }
            }

            var col = new DataGridViewButtonColumn();
            dgv.Columns.Add(col);
            dgv.Columns[5].Name = "btn";
            dgv.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        void SelectionSetup(DataGridView dgv, DataTable dt = null)
        {
            dgvSelection(dgv);

            if (dgv.Rows.Count > 0)
            {
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
            }

            if (dgv.Columns.Count > 0)
            { dgv.Columns.Clear(); }

            switch (dgv.Name)
            {
                case "dgvItemSelection":
                    dgv.ColumnCount = 14;
                    break;
                case "dgvItemSelected":
                    dgv.ColumnCount = 16;
                    break;
                case "dgvAllocationBase":
                    dgv.ColumnCount = dt.Rows.Count + 14;
                    break;
                default:
                    break;
            }

            if (dgv.Name.Contains("dgvItemSelect"))
            {
                dgv.Columns[0].Name = "Item No";
                dgv.Columns[1].Name = "Bar Code";
                dgv.Columns[2].Name = "Description";
                dgv.Columns[3].Name = "Color";
                dgv.Columns[4].Name = "Size";

                dgv.Columns[5].Name = "Class";
                dgv.Columns[6].Name = "Subclass";
                dgv.Columns[7].Name = "Collection";
                dgv.Columns[8].Name = "Packaging";
                dgv.Columns[9].Name = "Specification";
                dgv.Columns[10].Name = "Section";
                dgv.Columns[11].Name = "Item Class";

                dgv.Columns[12].Name = "Sort";
                dgv.Columns[12].Visible = false;

                dgv.Columns[13].Name = "Qty";
                dgv.Columns[13].Width = 50;


                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgv.ScrollBars = ScrollBars.Both;
            }
            else if (dgv.Name.Equals("dgvAllocationBase"))
            {
                dgv.Columns[0].Name = "Style";
                dgv.Columns[1].Name = "Item No";
                dgv.Columns[2].Name = "Bar Code";
                dgv.Columns[3].Name = "Description";
                dgv.Columns[4].Name = "Color";
                dgv.Columns[5].Name = "Size";

                dgv.Columns[6].Name = "Class";
                dgv.Columns[7].Name = "Subclass";
                dgv.Columns[8].Name = "Collection";
                dgv.Columns[9].Name = "Packaging";
                dgv.Columns[10].Name = "Specification";
                dgv.Columns[11].Name = "Section";
                dgv.Columns[12].Name = "Item Class";
                dgv.Columns[13].Name = "Sort";
                dgv.Columns[13].Visible = false;
            }
            else if (dgv.Name.Contains("dgvCustSelect"))
            {
                dgv.ColumnCount = 3;
                dgv.Columns[0].Name = "Customer Code";
                dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns[1].Name = "Customer Name";
                dgv.Columns[2].Name = "Classification";
                dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            else if (dgv.Name.Contains("Runs"))
            {
                dgv.DataSource = dt;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                dgv.MultiSelect = false;
                if (dt.Rows.Count > 0)
                { dgv.Rows[0].Selected = true; }
            }

            switch (dgv.Name)
            {
                case "dgvItemSelected":
                    dgv.Columns[14].Name = "%";
                    dgv.Columns[15].Name = "Allocate";

                    for (int iDgv = 0; iDgv < dgv.Columns.Count; iDgv++)
                    {
                        var dc = dgv.Columns[iDgv];
                        if (dc.Index.Equals(14) || dc.Index.Equals(15))
                        { dc.ReadOnly = false; }
                        else
                        { dc.ReadOnly = true; }
                    }
                    break;
                case "dgvAllocationBase":
                    int i = 14;

                    for (int iDt = 0; iDt < dt.Rows.Count; iDt++)
                    {
                        var dr = dt.Rows[iDt];
                        dgv.Columns[i].Name = dr[0].ToString();
                        i++;
                    }

                    var max = form.dgvItemSelected.Rows.Count;
                    for (int iItms = 0; iItms < max; iItms++)
                    {
                        StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {iItms + 1} out of {max}", iItms + 1, max);
                        var dr = form.dgvItemSelected.Rows[iItms];
                        var ItemCode = dr.Cells[0].Value;
                        
                        dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetItemDetails), 1, "", 0), ItemCode));

                        var sQty = dr.Cells["Allocate"].Value;
                        if (sQty != null)
                        {
                            if (double.Parse(sQty.ToString()) > 0)
                            {
                                dgv.Rows.Add(LibraryHelper.DataTableRet(dt, 0, "Style", ""),
                                              ItemCode,
                                              LibraryHelper.DataTableRet(dt, 0, "CodeBars", ""),
                                              dr.Cells["Description"].Value,
                                              LibraryHelper.DataTableRet(dt, 0, "Color", ""),
                                              LibraryHelper.DataTableRet(dt, 0, "Size", ""),
                                              LibraryHelper.DataTableRet(dt, 0, "Class", ""),
                                              LibraryHelper.DataTableRet(dt, 0, "SubClass", ""),
                                              LibraryHelper.DataTableRet(dt, 0, "Collection", ""),
                                              LibraryHelper.DataTableRet(dt, 0, "Packaging", ""),
                                              LibraryHelper.DataTableRet(dt, 0, "Specification", ""),
                                              LibraryHelper.DataTableRet(dt, 0, "Section", ""),
                                              LibraryHelper.DataTableRet(dt, 0, "ItemClass", ""),
                                              LibraryHelper.DataTableRet(dt, 0, "Sort", ""));
                            }
                        }

                    }
                    
                    dgv.Sort(dgv.Columns["Sort"], ListSortDirection.Ascending);
                    dgv.MultiSelect = false;
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                    for (int iCol = 0; iCol < dgv.Columns.Count; iCol++)
                    {
                        var dc = dgv.Columns[iCol];
                        if (dc.Index < 14)
                        { dc.ReadOnly = true; }
                    }
                    
                    break;
                case "dgvCustSelected":
                    if (form.rbCreateNewAlloc.Checked)
                    {
                        var dgvAlloc = form.dgvAllocationBase;

                        for (int iCol = 0; iCol < dgvAlloc.Columns.Count; iCol++)
                        {
                            var dc = dgvAlloc.Columns[iCol];
                            double sum = 0;
                            if (dc.Index > 13)
                            {
                                for (int iRow = 0; iRow < dgvAlloc.Rows.Count; iRow++)
                                {
                                    var drRows = dgvAlloc.Rows[iRow];
                                    var drInt = drRows.Cells[dc.HeaderText].Value;
                                    sum += double.Parse((drInt != null ? drInt.ToString() : "0").ToString());
                                    if (dgvAlloc.Rows.Count - 1 == drRows.Index)
                                    {
                                        if (sum > 0)
                                        {
                                            for (int iAdd = 0; iAdd < dt.Rows.Count; iAdd++)
                                            {
                                                var dr = dt.Rows[iAdd];
                                                if (dc.HeaderText == dr[2].ToString())
                                                { dgv.Rows.Add(dr[0], dr[1], dr[2]); }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (form.rbRepeatOrder.Checked)
                    {
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            var dr = dt.Rows[iRow];
                            dgv.Rows.Add(dr[0], dr[1], (string.IsNullOrEmpty(dr[2].ToString()) ? "" : dr[2].ToString()));
                        }
                    }
                    dgv.Sort(dgv.Columns[2], ListSortDirection.Ascending);
                    break;
                default:
                    break;
            }
        }

        void dgvAllocation(DataGridView dgv)
        {
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersVisible = true;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.ScrollBars = ScrollBars.Both;
            dgv.Columns.Clear();
            if (form.rbCreateNewAlloc.Checked || form.rbAllocationApproval.Checked)
            {
                dgv.Columns.Add("CardCode", "Customer Code");
                dgv.Columns.Add("CardName", "Customer Name");
                dgv.Columns.Add("Classification", "Classification");

                dgv.Columns.Add("ItemCode", "Item No.");
                dgv.Columns.Add("CodeBars", "Bar Code");
                dgv.Columns.Add("ItemName", "Description");
                dgv.Columns.Add("Style", "Style");
                dgv.Columns.Add("Color", "Color");
                dgv.Columns.Add("Size", "Size");
                dgv.Columns.Add("Class", "Class");
                dgv.Columns.Add("SubClass", "Subclass");
                dgv.Columns.Add("Collection", "Collection");
                dgv.Columns.Add("Packaging", "Packaging");
                dgv.Columns.Add("Specification", "Specification");
                dgv.Columns.Add("Section", "Section");
                dgv.Columns.Add("ItemClass", "Item Class");

            }
            else if (form.rbRepeatOrder.Checked)
            {
                dgv.Columns.Add("ItemCode", "Item No.");
                dgv.Columns.Add("CodeBars", "Bar Code");
                dgv.Columns.Add("ItemName", "Description");
                dgv.Columns.Add("Style", "Style");
                dgv.Columns.Add("Color", "Color");
                dgv.Columns.Add("Size", "Size");
                dgv.Columns.Add("Class", "Class");
                dgv.Columns.Add("SubClass", "Subclass");
                dgv.Columns.Add("Collection", "Collection");
                dgv.Columns.Add("Packaging", "Packaging");
                dgv.Columns.Add("Specification", "Specification");
                dgv.Columns.Add("Section", "Section");
                dgv.Columns.Add("ItemClass", "Item Class");

                dgv.Columns.Add("CardCode", "Customer Code");
                dgv.Columns.Add("CardName", "Customer Name");
                dgv.Columns.Add("Classification", "Classification");
            }

            if (form.rbAllocationApproval.Checked)
            {
                dgv.Columns.Add("Quantity", "Quantity");
                dgv.Columns.Add("Allocated", "Allocated");
            }
            else
            {
                dgv.Columns.Add("Avail", "Available");
                dgv.Columns.Add("Allocation", "Allocation");
                if (form.rbRepeatOrder.Checked)
                {
                    dgv.Columns.Add("MaxQty", "Max Quantity");
                    dgv.Columns.Add("OpenQty", "Doc Quantity");
                    dgv.Columns.Add("Allocated", "Allocated");
                }
                else if (form.rbCreateNewAlloc.Checked)
                {
                    dgv.Columns.Add("Allocated", "Allocated");
                }
            }

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                var dc = dgv.Columns[i];
                if (!dc.Name.Equals("Allocated"))
                { dc.ReadOnly = true; }
            }
        }

        void dgvSelection(DataGridView dgv)
        {
            dgv.RowHeadersVisible = true;
            dgv.MultiSelect = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToResizeColumns = true;
        }

        void dgvDesign(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.BackColor;
            dgv.DefaultCellStyle.SelectionForeColor = dgv.DefaultCellStyle.ForeColor;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersVisible = false;
            dgv.MultiSelect = false;
            dgv.AllowUserToResizeColumns = false;
        }

        void dgvSetup(DataGridView dgv)
        {
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToResizeRows = false;
        }
    }
}

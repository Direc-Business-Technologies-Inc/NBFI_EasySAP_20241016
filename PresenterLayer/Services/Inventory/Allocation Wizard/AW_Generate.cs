using Context;
using DirecLayer;
using DomainLayer.Models;
using DomainLayer.Models.Inventory.Allocation_Wizard;
using MetroFramework;
using PresenterLayer.Helper;
using ServiceLayer.Services;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PresenterLayer
{
    class AW_Generate
    {
        public frmAllocationWizard form { get; set; }
        //SAOContext sao { get; set; } = new SAOContext();
        private SAPHanaAccess sapHana { get; set; }
        private DataHelper helper { get; set; }
        private ServiceLayerAccess serviceLayerAccess { get; set; }
        public AW_Generate()
        {
            helper = new DataHelper();
            sapHana = new SAPHanaAccess();
            serviceLayerAccess = new ServiceLayerAccess();
        }

        #region Allocation Wizard Runs
        
        public void GenerateAllocWizRuns()
        {
            var sRunID = form.dgvAllocWizRuns.SelectedRows[0].Cells["Run ID"].Value.ToString();

            GenerateMarketingDoc(sRunID);

            AllocWizRuns(sRunID);
        }

        void GenerateMarketingDoc(string sRunID)
        {
            using (var dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetITRIDbyRunID), 1, "", 0), sRunID)))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SelectionModel.Selection.Add(new SelectionModel.SelectionData
                    {
                        Choose = true,
                        TableID = "WTQ1",
                        ID = dr["DocEntry"].ToString(),
                        Type = "MD"
                    });
                }
            }
        }

        void AllocWizRuns(string sRunID)
        {
            using (var dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetITRbyRunID), 1, "", 0), sRunID)))
            {
                int max = dt.Rows.Count;
                int min = 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    var sItemCode = dr["ItemCode"].ToString();
                    
                    var drItem = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetItemDetails), 1, "", 0), sItemCode));

                    form.dgvAllocation.Rows.Add(dr["CardCode"].ToString(),//Customer Code
                                                dr["CardName"].ToString(),//Customer Name
                                                dr["Classification"].ToString(),//Classification
                                                sItemCode,//ItemCode
                                                LibraryHelper.DataTableRet(drItem, 0, "CodeBars", ""),//Barcode
                                                dr["Dscription"].ToString(),//Desc
                                                LibraryHelper.DataTableRet(drItem, 0, "Style", ""),//Style
                                                LibraryHelper.DataTableRet(drItem, 0, "Color", ""),//Color
                                                LibraryHelper.DataTableRet(drItem, 0, "Size", ""),//Size
                                                LibraryHelper.DataTableRet(drItem, 0, "Class", ""),//Class
                                                LibraryHelper.DataTableRet(drItem, 0, "SubClass", ""),//SubClass
                                                LibraryHelper.DataTableRet(drItem, 0, "Collection", ""),//Collection
                                                LibraryHelper.DataTableRet(drItem, 0, "Packaging", ""),//Packaging
                                                LibraryHelper.DataTableRet(drItem, 0, "Specification", ""),//Specification
                                                LibraryHelper.DataTableRet(drItem, 0, "Section", ""),//Section
                                                LibraryHelper.DataTableRet(drItem, 0, "ItemClass", ""),//ItemClass
                                                dr["Quantity"].ToString(),
                                                dr["Quantity"].ToString());

                    if (min >= 20)
                    {
                       StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {min} out of {max}", min, max);
                    }
                    else if (max < 20 && min == max)
                    {
                        Application.DoEvents();
                    }

                    min++;
                }
            }
        }
        #endregion

        #region Allocated Wizard Runs

        public void GenerateAllocatedWizRuns()
        {
            var sRunID = form.dgvApprovedRuns.SelectedRows[0].Cells["Run ID"].Value.ToString();

            GenerateMarketingDoc(sRunID);
        }

        public void AllocatedWizRuns()
        {
            var sRunID = form.dgvApprovedRuns.SelectedRows[0].Cells["Run ID"].Value.ToString();
            
            using (var dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetITRAppbyRunID), 1, "", 0), sRunID)))
            {
                int max = dt.Rows.Count;
                int min = 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    var sItemCode = dr["ItemCode"].ToString();
                    
                    var drItem = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetItemDetails), 1, "", 0), sItemCode));

                    form.dgvSummary.Rows.Add(dr["CardCode"].ToString(),//Customer Code
                                                dr["CardName"].ToString(),//Customer Name
                                                dr["Classification"].ToString(),//Classification
                                                sItemCode,//ItemCode
                                                LibraryHelper.DataTableRet(drItem, 0, "CodeBars", ""),//Barcode
                                                dr["Dscription"].ToString(),//Desc
                                                LibraryHelper.DataTableRet(drItem, 0, "Style", ""),//Style
                                                LibraryHelper.DataTableRet(drItem, 0, "Color", ""),//Color
                                                LibraryHelper.DataTableRet(drItem, 0, "Size", ""),//Size
                                                LibraryHelper.DataTableRet(drItem, 0, "Class", ""),//Class
                                                LibraryHelper.DataTableRet(drItem, 0, "SubClass", ""),//SubClass
                                                LibraryHelper.DataTableRet(drItem, 0, "Collection", ""),//Collection
                                                LibraryHelper.DataTableRet(drItem, 0, "Packaging", ""),//Packaging
                                                LibraryHelper.DataTableRet(drItem, 0, "Specification", ""),//Specification
                                                LibraryHelper.DataTableRet(drItem, 0, "Section", ""),//Section
                                                LibraryHelper.DataTableRet(drItem, 0, "ItemClass", ""),//ItemClass
                                                dr["Quantity"].ToString(),
                                                dr["DocEntry"].ToString(),
                                                dr["LineNum"].ToString());
                    if (min >= 20)
                    {
                        StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {min} out of {max}", min, max);
                    }
                    else if (max < 20 && min == max)
                    {
                        Application.DoEvents();
                    }
                    min++;
                }
            }
        }
        #endregion

        #region Item Selection
        public void GenerateItemSelection()
        {
            ItemSelectionClear();
            ItemSelection();
        }

        void ItemSelectionClear()
        {
            form.dgvItemSelected.Rows.Clear();
            form.dgvItemSelection.Rows.Clear();

            ItemDelete();
        }

        void ItemDelete()
        {
            //var sUserCode = SboCred.GetEmployeeCode();
            //var ifexist = sao.OISM.Where(x => x.UserCode == sUserCode);

            //if (ifexist.Any())
            //{
            //    sao.OISM.RemoveRange(ifexist);
            //    sao.SaveChanges();
            //}
        }

        void ItemSelection()
        {
            StaticHelper._MainForm.Progress($"Please wait...",1, 100);
            int max = 0;
            int min = 0;

            // Generate Warehouse Filter
            var sWhs = new StringBuilder();

            for (int i = 0; i < form.dgvWhs.Rows.Count; i++)
            {
                var dr = form.dgvWhs.Rows[i];
                if (PublicStatic.isCancel)
                {
                    StaticHelper._MainForm.ProgressClear();
                    return;
                }

                if (bool.Parse(LibraryHelper.DataGridViewRowRet(dr, "cb")))
                { sWhs.Append($"'{dr.Cells[1].Value.ToString()}',"); }
                Application.DoEvents();
            }
            // Generate Other Parameter Filter
            var sbWhere = new StringBuilder();
            var sbCondition = new StringBuilder();
            var sbParameter = new StringBuilder();

            //int loop = 0;
            string[] TableID = new string[form.dgvItemOtherParam.Rows.Count];

            for (int i = 0; i < form.dgvItemOtherParam.Rows.Count; i++)
            {
                var dr = form.dgvItemOtherParam.Rows[i];

                if (PublicStatic.isCancel)
                {
                    StaticHelper._MainForm.ProgressClear();
                    return;
                }

                if (dr.Cells[0].Value != null)
                { TableID[i] = dr.Cells[0].Value.ToString(); }
                //loop++;
            }

            var mParameter = SelectionModel.Selection.Where(x => x.Type == "IOP" && TableID.Contains(x.TableID)).OrderBy(x => x.TableID).ThenBy(x => x.ID);

            if (mParameter.Any())
            {
                foreach (var dr in mParameter.ToList())
                {
                    if (PublicStatic.isCancel)
                    {
                        StaticHelper._MainForm.ProgressClear();
                        return;
                    }

                    if (sbCondition.ToString().Contains($"{dr.TableID}"))
                    { sbParameter.Append($"'{dr.ID}',"); }
                    else
                    {
                        if (sbCondition.ToString() != "")
                        {
                            sbParameter.Append(") ");
                            sbWhere.AppendLine($"{sbCondition.ToString()} IN {sbParameter} AND ");
                            sbCondition.Clear();
                            sbParameter.Clear();
                        }
                        sbCondition.Append($"{dr.TableID}");
                        sbParameter.Append($"('{dr.ID}',");
                    }
                }
                sbParameter.Append(") ");
                sbWhere.AppendLine($"{sbCondition.ToString()} IN {sbParameter} AND ");
                sbParameter.Clear();
            }

            // Generate Marketing Document Filter
            var sbItemList = new StringBuilder();
            var sbWhsList = new StringBuilder();

            for (int i = 0; i < form.dgvMarketingDocs.Rows.Count; i++)
            {
                var module = form.dgvMarketingDocs.Rows[i];
                if (PublicStatic.isCancel)
                {
                    StaticHelper._MainForm.ProgressClear();
                    return;
                }

                var sModule = module.Cells["Code"].Value != null ? module.Cells["Code"].Value.ToString() : "";
                var lMarketingDocs = SelectionModel.Selection.Where(x => x.Type == "MD" && x.TableID == sModule);
                if (lMarketingDocs.Any())
                {
                    StaticHelper._MainForm.Progress($"Consolidating items.",1, 100);

                    var sbDocEntries = new StringBuilder();

                    foreach (var dr in lMarketingDocs.ToList())
                    { sbDocEntries.Append($"{dr.ID},"); }

                    var sbQuery = new StringBuilder();
                    
                    sbQuery.AppendFormat(helper.ReadDataRow(sapHana.Get(SP.AW_GenMarketingDoc), 1, "", 0), sModule, $"ItemCode IN ({string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GenItemList), 1, "", 0), sbWhere)})", sbDocEntries.ToString());

                    using (var dt = sapHana.Get(sbQuery.ToString()))
                    {
                        max = dt.Rows.Count;
                        min = 0;

                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            var drItem = dt.Rows[iRow];
                            if (PublicStatic.isCancel)
                            {
                                StaticHelper._MainForm.ProgressClear();
                                return;
                            }

                            if (!sbParameter.ToString().Contains(drItem["ItemCode"].ToString()))
                            { sbParameter.Append($"'{drItem["ItemCode"].ToString()}',"); }

                            if (string.IsNullOrEmpty(sWhs.ToString()))
                            {
                                if (!sbWhsList.ToString().Contains(drItem["WhsCode"].ToString()))
                                { sbWhsList.Append($"'{drItem["WhsCode"].ToString()}',"); }
                            }
                            min++;
                            StaticHelper._MainForm.Progress($"Checking items {drItem["ItemCode"].ToString()} in document {sModule}. {min} out of {max}.", min, max);
                        }

                    }
                    sWhs.Append(sbWhsList.ToString());
                    if (!string.IsNullOrEmpty(sbParameter.ToString()))
                    { sbWhere.AppendLine($"ItemCode IN ({sbParameter}) AND "); }
                }
            }
            
            // Get Item Parameter and replace filters
            StringBuilder sbItemSelection = new StringBuilder();
            
            sbItemSelection.AppendFormat(helper.ReadDataRow(sapHana.Get(SP.AW_GenItemList), 1, "", 0), sbWhere);
            
            using (var dt = sapHana.Get(sbItemSelection.ToString()))
            {
                max = dt.Rows.Count;
                min = 0;
                sbParameter.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];

                    if (PublicStatic.isCancel)
                    {
                        StaticHelper._MainForm.ProgressClear();
                        return;
                    }


                    if (!sbParameter.ToString().Contains(dr["ItemCode"].ToString()))
                    { sbParameter.Append($"'{dr["ItemCode"].ToString()}',"); }
                }
            }

            // Generate in Datagridview the Items

            var ItemCode = !string.IsNullOrEmpty(sbParameter.ToString()) ? $@"""ITEMCODE"" IN ({sbParameter.ToString().Remove(sbParameter.Length - 1)}) " : "";

            var Whs = !string.IsNullOrEmpty(sWhs.ToString()) ? $@"""WhsCode"" IN ({ sWhs.ToString().Remove(sWhs.Length - 1)}) " : "";

            var Where = !string.IsNullOrEmpty(ItemCode) ? $"WHERE {ItemCode} AND {Whs}" : (!string.IsNullOrEmpty(Whs) ? $"WHERE {Whs}" : "");

            var sQuery = form.rbCreateNewAlloc.Checked ? $@"SELECT ""ITEMCODE"", ""ITEMNAME"", SUM(""QTY"") AS ""QTY"" FROM {EasySAPCredentialsModel.ESDatabase}.SAO_CREATE_INV_AVAILABLE_QTY {Where} GROUP BY ""ITEMCODE"",""ITEMNAME""" : $@"SELECT ""ITEMCODE"", ""ITEMNAME"", SUM(""QTY"") AS ""QTY"" FROM {SboCred.Database}.SAO_INV_AVAILABLE_QTY {Where} GROUP BY ""ITEMCODE"",""ITEMNAME""";
            
            using (var dt = sapHana.Get(sQuery)) // SELECT  FROM OISM GROUP BY ItemCode, ItemName
            {
                if (LibraryHelper.DataExist(dt))
                {
                    ItemDelete();
                    max = dt.Rows.Count;
                    min = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var dr = dt.Rows[i];
                        if (PublicStatic.isCancel)
                        {
                            StaticHelper._MainForm.ProgressClear();
                            return;
                        }

                        var sItemName = dr["ItemName"].ToString();
                        var sItemCode = dr["ItemCode"].ToString();
                        
                        DataRow dtItemDetails = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetItemDetails), 1, "", 0), sItemCode)).Rows[0];
                        form.dgvItemSelection.Rows.Add(sItemCode,
                                                       LibraryHelper.DataRowRet(dtItemDetails, "CodeBars", ""),
                                                       sItemName.Substring(0, Math.Min(sItemName.Length, 50)),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "Color", ""),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "Size", ""),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "Class", ""),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "SubClass", ""),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "Collection", ""),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "Packaging", ""),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "Specification", ""),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "Section", ""),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "ItemClass", ""),
                                                       LibraryHelper.DataRowRet(dtItemDetails, "Sort", ""),
                                                       IsNumeric(dr["Qty"]));
                        min++;

                        if (min >= 20)
                        {
                           StaticHelper._MainForm.Progress($"Please wait until all items are loaded. {min} out of {max}.", min, max);
                        }
                        
                    }

                    var dgv = form.dgvItemSelection;
                    dgv.Sort(dgv.Columns["Sort"], ListSortDirection.Ascending);
                }
                else { StaticHelper._MainForm.ShowMessage("No data found", true); }
            }
        }

        string IsNumeric(object e)
        {
            string result = "0";
            bool chkChar = false;

            if (e != null)
            { chkChar = double.TryParse(e.ToString(), out double n); }

            if (chkChar)
            {
                double sNumber = double.Parse(e.ToString());
                result = string.Format("{0:#,0}", sNumber);
            }

            return result;
        }
        #endregion

        #region Store Selection
        public void GenerateStoreSelection()
        {
            StoreSelectionClear();
            StoreSelection();
        }

        void StoreSelectionClear()
        { form.dgvCustSelection.Rows.Clear(); }

        void StoreSelection()
        {
            var sbWhere = new StringBuilder();
            var sbCondition = new StringBuilder();
            var sbParameter = new StringBuilder();

            var paramCount = form.dgvCustOtherParam.Rows.Count;

            string[] sTableID = new string[paramCount + form.dgvStoreCriteria.Rows.Count];

            // Generate Other Parameters Filter

            for (int i = 0; i < paramCount; i++)
            {
                var dr = form.dgvCustOtherParam.Rows[i];
                if (dr.Cells[0].Value != null)
                {
                    sTableID[i] = dr.Cells[0].Value.ToString();
                }
            }
            
            // Generate Store Criteria Filter

            for (int i = 0; i < form.dgvStoreCriteria.Rows.Count; i++)
            {
                var dr = form.dgvStoreCriteria.Rows[i];
                if (dr.Cells[0].Value != null)
                {
                    sTableID[paramCount + i] = dr.Cells[0].Value.ToString();
                }
            }
            
            string[] type = { "COP", "SC" };

            // Create Parameter for Customer List
            var mParameter = SelectionModel.Selection.Where(x => type.Contains(x.Type) && sTableID.Contains(x.TableID)).OrderBy(x => x.TableID).ThenBy(x => x.ID);
            if (mParameter.Any())
            {
                foreach (var dr in mParameter.ToList())
                {
                    if (sbCondition.ToString().Contains($"{dr.TableID}"))
                    {
                        sbParameter.Append($"'{dr.ID}',");
                    }
                    else
                    {
                        if (sbCondition.ToString() != "")
                        {
                            sbParameter.Append(") ");
                            sbWhere.AppendLine($"{sbCondition.ToString()} IN {sbParameter} AND ");
                            sbCondition.Clear();
                            sbParameter.Clear();
                        }
                        sbCondition.Append($"{dr.TableID}");
                        sbParameter.Append($"('{dr.ID}',");
                    }
                }
                sbParameter.Append(") ");
                sbWhere.AppendLine($"{sbCondition.ToString()} IN {sbParameter} AND ");
                sbParameter.Clear();
            }

            // Generate Customer that is selected
            if (form.dgvCustSelected.Rows.Count > 0)
            {
                for (int i = 0; i < form.dgvCustSelected.Rows.Count; i++)
                {
                    var dr = form.dgvCustSelected.Rows[i];

                    if (sbParameter.Length > 0)
                    { sbParameter.Append($"'{dr.Cells[0].Value.ToString()}',"); }
                    else
                    { sbParameter.Append($"('{dr.Cells[0].Value.ToString()}',"); }
                }
                
                sbParameter.Append($") ");
                sbWhere.AppendLine($"CardCode NOT IN {sbParameter} AND ");
            }


            var sbStoreSelection = new StringBuilder();
            
            sbStoreSelection.AppendFormat(helper.ReadDataRow(sapHana.Get(SP.AW_GetCustomerList), 1, "", 0), sbWhere);
            var sBrand = form.dgvItemOtherParam.Rows[0].Cells["Value"].Value.ToString();

            using (var dt = sapHana.Get(sbStoreSelection.ToString()))
            {
                try
                {
                    if (LibraryHelper.DataExist(dt))
                    {
                        var max = dt.Rows.Count;
                        for (int i = 0; i < max; i++)
                        {

                            if ((i + 1) >= 20)
                            {
                                StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {i + 1} out of {max}", i + 1, max);
                            }
                            else if (max < 20 && (i + 1) == max)
                            {
                                Application.DoEvents();
                            }
                            
                            var dr = dt.Rows[i];
                            var CardCode = dr[0].ToString();
                            
                            var sQuery = string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetClassicByBP), 1, "", 0), sBrand, CardCode);
                            
                            var sClassification = helper.ReadDataRow(sapHana.Get(sQuery), 0, "", 0);
                            form.dgvCustSelection.Rows.Add(CardCode, dr[1].ToString(), string.IsNullOrEmpty(sClassification) ? "" : sClassification);
                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("No data found", true);
                    }
                }
                catch (Exception ex) { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
            }
        }
        #endregion

        #region Ranking
        public void GenerateRanking()
        {
            AllocationClear();
            Allocation();
        }

        void AllocationClear()
        {
            if (form.dgvAllocation.Rows.Count > 0)
            {
                form.dgvAllocation.Rows.Clear();
            }

        }

        void Allocation()
        {
            StaticHelper._MainForm.Progress("Please wait...",1, 100);
            if (!string.IsNullOrEmpty(form.dgvLevels.Rows[0].Cells[0].Value.ToString()))
            {
                var sbSort = new StringBuilder();
                var sbSorting = new StringBuilder();
                var dt = new DataTable();
                dt.Columns.Add("CardCode");
                dt.Columns.Add("CardName");
                dt.Columns.Add("Classification");

                for (int i = 0; i < form.dgvCustSelected.Rows.Count; i++)
                {
                    var dr = form.dgvCustSelected.Rows[i];

                    if (PublicStatic.isCancel)
                    {
                        StaticHelper._MainForm.ProgressClear();
                        return;
                    }

                    dt.Rows.Add();
                    dt.Rows[dr.Index][0] = dr.Cells[0].Value.ToString();
                    dt.Rows[dr.Index][1] = dr.Cells[1].Value.ToString();
                    dt.Rows[dr.Index][2] = dr.Cells[2].Value.ToString();
                }
                
                for (int i = 0; i < form.dgvLevels.Rows.Count; i++)
                {
                    var dr = form.dgvLevels.Rows[i];
                    if (PublicStatic.isCancel)
                    {
                        StaticHelper._MainForm.ProgressClear();
                        return;
                    }

                    var val = dr.Cells[0].Value;
                    if (val != null)
                    {
                        if (val.ToString() == "SalesRank")
                        {
                            string sDateFrom = form.dtpDateFrom.Value.ToString("yyyy-MM-dd");
                            string sDateTo = form.dtpDateTo.Value.ToString("yyyy-MM-dd");

                            string sValueBasedOn;

                            if (form.rbAmount.Checked)
                            { sValueBasedOn = "AMOUNT"; }
                            else
                            { sValueBasedOn = "QTY"; }

                            string sTotalSalesBasedOn;
                            string sBasedOn = "";

                            if (form.rbAverage.Checked)
                            {
                                sTotalSalesBasedOn = "a";
                                sBasedOn = $"_{form.cbAverage.Text}";
                            }
                            else
                            { sTotalSalesBasedOn = "t"; }

                            // 1. Brand
                            var sBrand = (form.dgvSalesCritera.Rows[0].Cells["Field"].Value != null ? form.dgvSalesCritera.Rows[0].Cells["Field"].Value.ToString() : "");
                            // 2. Category
                            var sCat = (form.dgvSalesCritera.Rows[3].Cells["Field"].Value != null ? form.dgvSalesCritera.Rows[3].Cells["Field"].Value.ToString() : "");
                            // 3. Sub-Category
                            var sSubCat = (form.dgvSalesCritera.Rows[4].Cells["Field"].Value != null ? form.dgvSalesCritera.Rows[4].Cells["Field"].Value.ToString() : "");

                            var sbItemCode = new StringBuilder();

                            for (int iItemSelected = 0; iItemSelected < form.dgvItemSelected.Rows.Count; iItemSelected++)
                            {
                                var drItemSelected = form.dgvItemSelected.Rows[iItemSelected];
                                if (PublicStatic.isCancel)
                                {
                                    StaticHelper._MainForm.ProgressClear();
                                    return;
                                }

                                var sAllocate = double.Parse(drItemSelected.Cells["Allocate"].Value != null ? drItemSelected.Cells["Allocate"].Value.ToString() : "0");
                                if (sAllocate > 0)
                                {
                                    var sItemCode = drItemSelected.Cells[0].Value != null ? drItemSelected.Cells[0].Value.ToString() : "";
                                    sbItemCode.Append($"''{sItemCode}'',");
                                }
                            }

                            var dtSalesCont = new DataTable();
                            if (form.rbCreateNewAlloc.Checked)
                            { dtSalesCont = sapHana.Get($@"SELECT * FROM dbs_ALLOC_WIZ_SALES_RANKING{sBasedOn.ToUpper()}('{sTotalSalesBasedOn}','{sDateFrom}','{sDateTo}','','{sBrand}','{sCat}','{sSubCat}') WHERE {sValueBasedOn} > 0"); }
                            else if (form.rbRepeatOrder.Checked)
                            { dtSalesCont = sapHana.Get($@"CALL ""USR_SP_ALLOC_WIZ_SALES_RANKING{sBasedOn.ToUpper()}"" ('{sTotalSalesBasedOn}','{sDateFrom}','{sDateTo}','','({(string.IsNullOrEmpty(sbItemCode.ToString()) ? "''" : sbItemCode.ToString().Remove(sbItemCode.Length - 1))})')"); }

                            if (LibraryHelper.DataExist(dtSalesCont))
                            {
                                sbSorting.Append($"{val.ToString()} DESC");
                                dt.Columns.Add(val.ToString(), typeof(Int32));

                                for (int iList = 0; iList < dt.Rows.Count; iList++)
                                {
                                    var drlist = dt.Rows[iList];
                                    for (int iSalesCont = 0; iSalesCont < dtSalesCont.Rows.Count; iSalesCont++)
                                    {
                                        var drsalescont = dtSalesCont.Rows[iSalesCont];

                                        if (PublicStatic.isCancel)
                                        {
                                            StaticHelper._MainForm.ProgressClear();
                                            return;
                                        }

                                        if (drlist["CardCode"].ToString() == drsalescont["CARDCODE"].ToString())
                                        {
                                            drlist[val.ToString()] = int.Parse(Math.Round(decimal.Parse(drsalescont[sValueBasedOn].ToString())).ToString());
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(val.ToString()))
                            {
                                sbSort.Append($"{dr.Cells[0].Value.ToString()} ASC,");
                                //dt.Columns.Add(dr.Cells[0].Value.ToString());

                                for (int iDetails = 0; iDetails < dt.Rows.Count; iDetails++)
                                {
                                    var drdetails = dt.Rows[iDetails];
                                    if (PublicStatic.isCancel)
                                    {
                                        StaticHelper._MainForm.ProgressClear();
                                        return;
                                    }

                                    var sBrand = form.dgvSalesCritera.Rows[0].Cells["Field"].Value.ToString();
                                    using (var dtlevels = sapHana.Get(string.Format(dr.Cells[1].Value.ToString(), sBrand, drdetails[0].ToString())))
                                    {
                                        if (LibraryHelper.DataExist(dtlevels))
                                        {
                                            if (dtlevels.Rows.Count > 0)
                                            { drdetails[val.ToString()] = dtlevels.Rows[0][0].ToString(); }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                dt.Columns.Add("Sum", typeof(Int32));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    int index = 0;
                    int sum = 0;
                    for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                    {
                        var dc = dt.Columns[iCol];

                        if (PublicStatic.isCancel)
                        {
                            StaticHelper._MainForm.ProgressClear();
                            return;
                        }

                        if (dc.ColumnName != "Sum" && dc.ColumnName != "CardCode")
                        {
                            var isint = int.TryParse(dr[dc.ColumnName].ToString(), out int n);
                            if (isint)
                            { sum += int.Parse(dr[dc.ColumnName].ToString()); }
                        }
                        else if (dc.ColumnName == "Sum")
                        {
                            dr["Sum"] = sum;
                        }
                        index++;
                    }

                }
                
                var dv = new DataView(dt);
                if (!string.IsNullOrEmpty(sbSort.ToString()))
                {
                    if (string.IsNullOrEmpty(sbSorting.ToString()))
                    {
                        sbSorting.Append(sbSort.ToString().Remove(sbSort.Length - 1));
                        sbSort.Clear();
                    }
                }

                var dgv = form.dgvAllocation;
                var sSort = $"{sbSort.ToString()} {sbSorting.ToString()}";
                if (!string.IsNullOrEmpty(sSort.Trim()))
                {
                    dv.Sort = sSort;
                    if (form.rbCreateNewAlloc.Checked)
                    {
                        int max = dv.ToTable().Rows.Count;

                        for (int i = 0; i < max; i++)
                        {
                            var dr = dv.ToTable().Rows[i];
                            for (int iItem = 0; iItem < form.dgvAllocationBase.Rows.Count; iItem++)
                            {
                                var drItem = form.dgvAllocationBase.Rows[iItem];

                                for (int iCol = 0; iCol < form.dgvAllocationBase.Columns.Count; iCol++)
                                {
                                    var drColumn = form.dgvAllocationBase.Columns[iCol];

                                    if (PublicStatic.isCancel)
                                    {
                                        StaticHelper._MainForm.ProgressClear();
                                        return;
                                    }

                                    if (drColumn.Index > 4)
                                    {
                                        // 1. Brand
                                        var sBrand = form.dgvSalesCritera.Rows[0].Cells["Field"].Value.ToString();
                                        var qry = string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetAllocDetails), 1, "", 0), sBrand, dr[0].ToString());
                                        var sClassification = helper.ReadDataRow(sapHana.Get(qry), 0, "", 0);
                                        
                                        if (sClassification == drColumn.HeaderText)
                                        {
                                            //CardCode, CardName, ItemCode, ItemName, Avail, Allocation, Allocated
                                            var ItemCode = drItem.Cells["Item No"].Value.ToString();

                                            for (int iitem = 0; iitem < form.dgvItemSelected.Rows.Count; iitem++)
                                            {
                                                var item = form.dgvItemSelected.Rows[iitem];
                                                if (item.Cells[0].Value.ToString() == ItemCode)
                                                {
                                                    var Qty = drItem.Cells[sClassification].Value;
                                                    if (double.Parse((Qty == null ? 0 : Qty).ToString()) > 0)
                                                    {
                                                        
                                                        var dtItemDetails = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetItemDetails), 1, "", 0), ItemCode));

                                                        dgv.Rows.Add(dr[0].ToString(),//CardCode
                                                                     dr[1].ToString(),//CardName
                                                                     sClassification,//Classification
                                                                     ItemCode,//ItemCode
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "CodeBars", ""),//CodeBars
                                                                     item.Cells["Description"].Value.ToString(),//Description
                                                                     drItem.Cells["Style"].Value.ToString(),//Style
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "Color", ""),//Color
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "Size", ""),//Size
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "Class", ""),//Class
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "SubClass", ""),//Subclass
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "Collection", ""),//Collection
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "Packaging", ""),//Packaging
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "Specification", ""),//Specification
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "Section", ""),//Section
                                                                     LibraryHelper.DataTableRet(dtItemDetails, 0, "ItemClass", ""),//ItemClass
                                                                     item.Cells["Qty"].Value.ToString(),//Avail
                                                                     item.Cells["Allocate"].Value.ToString(),//Allocation
                                                                     Qty.ToString());
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (i > 5)
                            {
                                StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {i+1} out of {max}", i+1, max);
                            }
                            else if (max < 5  && (i + 1) == max)
                            {
                                Application.DoEvents();
                            }
                        }
                    }
                    else if (form.rbRepeatOrder.Checked)
                    {
                        var sbDocEntries = new StringBuilder();

                        for (int i = 0; i < form.dgvMarketingDocs.Rows.Count; i++)
                        {
                            var module = form.dgvMarketingDocs.Rows[i];

                            var sModule = module.Cells["Code"].Value.ToString();
                            var lMarketingDocs = SelectionModel.Selection.Where(x => x.Type == "MD" && x.TableID == sModule);
                            if (lMarketingDocs.Any())
                            {
                                foreach (var drDocEntry in lMarketingDocs.ToList())
                                { sbDocEntries.Append($"{drDocEntry.ID},"); }

                                var sbItemCode = new StringBuilder();

                                for (int iItemSelected = 0; iItemSelected < form.dgvItemSelected.Rows.Count; iItemSelected++)
                                {
                                    var drItemSelected = form.dgvItemSelected.Rows[iItemSelected];

                                    if (PublicStatic.isCancel)
                                    {
                                        StaticHelper._MainForm.ProgressClear();
                                        return;
                                    }

                                    var sItemCode = drItemSelected.Cells[0].Value != null ? drItemSelected.Cells[0].Value.ToString() : "";
                                    sbItemCode.Append($"'{sItemCode}',");
                                }
                                
                                var sbQuery = new StringBuilder();
                                sbQuery.AppendFormat(helper.ReadDataRow(sapHana.Get(SP.AW_GetROMarketingDoc), 1, "", 0), sModule.Remove(sModule.Length - 1), sModule, sbItemCode, sbDocEntries.ToString());

                                var dtData = new DataTable();
                                dtData = sapHana.Get(sbQuery.ToString());

                                int min = 0;

                                for (int iItemSelected = 0; iItemSelected < form.dgvItemSelected.Rows.Count; iItemSelected++)
                                {
                                    var drItemSelected = form.dgvItemSelected.Rows[iItemSelected];

                                    if (PublicStatic.isCancel)
                                    {
                                        StaticHelper._MainForm.ProgressClear();
                                        return;
                                    }
                                    
                                    var sItemCode = drItemSelected.Cells[0].Value;
                                    var sItemName = drItemSelected.Cells[1].Value;
                                    var sColor = drItemSelected.Cells["Color"].Value;
                                    var sSize = drItemSelected.Cells["Size"].Value;
                                    var iQty = drItemSelected.Cells["Qty"].Value;
                                    var iAllocate = drItemSelected.Cells["Allocate"].Value;
                                    int iDif = 0;
                                    int max = dtData.Rows.Count * dv.ToTable().Rows.Count;

                                    for (int iDr = 0; iDr < dv.ToTable().Rows.Count; iDr++)
                                    {
                                        var dr = dv.ToTable().Rows[iDr];

                                        for (int iitem = 0; iitem < dtData.Rows.Count; iitem++)
                                        {
                                            var item = dtData.Rows[iitem];

                                            if (PublicStatic.isCancel)
                                            {
                                                StaticHelper._MainForm.ProgressClear();
                                                return;
                                            }

                                            var iOpenQty = double.Parse(item["Quantity"].ToString());
                                            if (item["ItemCode"].ToString() == sItemCode.ToString() && item["CardCode"].ToString() == dr["CardCode"].ToString())
                                            {
                                                var iMaxQty = double.Parse(item["MaxQty"].ToString());
                                                var iFinalQty = iMaxQty == 0 ? iOpenQty : (iMaxQty > iOpenQty ? iOpenQty : iMaxQty);
                                                iDif = (int)double.Parse(iAllocate.ToString()) - (int)iFinalQty;
                                                
                                                var dtItemDetails = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetItemDetails), 1, "", 0), sItemCode));

                                                dgv.Rows.Add(sItemCode,//ItemCode
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "CodeBars", ""),//CodeBars
                                                                sItemName,//Description
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "Style", ""),//Style
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "Color", ""),//Color
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "Size", ""),//Size
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "Class", ""),//Class
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "SubClass", ""),//Subclass
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "Collection", ""),//Collection
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "Packaging", ""),//Packaging
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "Specification", ""),//Specification
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "Section", ""),//Section
                                                                LibraryHelper.DataTableRet(dtItemDetails, 0, "ItemClass", ""),//ItemClass
                                                                dr[0].ToString(),//CardCode
                                                                dr[1].ToString(),//CardName
                                                                dr[2].ToString(),//Classification
                                                                iQty,//Avail
                                                                iAllocate,//Allocation
                                                                (int)iMaxQty,//MaxQty
                                                                (int)iOpenQty,//OpenQty
                                                                (iDif > 0 ? iFinalQty : iAllocate));//Allocated

                                                if (iDif > 0)
                                                { iAllocate = iDif; }
                                                else
                                                { iAllocate = 0; }

                                                min++;
                                                if (min >= 20)
                                                {
                                                    StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {min} out of {max}", min, max);
                                                }
                                                else if (max < 20 && min == max)
                                                {
                                                    Application.DoEvents();
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        var column = dgv.Columns[i];
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    
                    if (form.dgvAllocation.Rows.Count > 0)
                    {
                        //foreach (DataGridViewRow item in form.dgvItemSelected.Rows)
                        for (int i = 0; i < form.dgvItemSelected.Rows.Count; i++)
                        {
                            var item = form.dgvItemSelected.Rows[i];

                            if (PublicStatic.isCancel)
                            {
                                StaticHelper._MainForm.ProgressClear();
                                return;
                            }
                            var sItemCode = item.Cells["Item No"].Value.ToString();
                            var iTotal = double.Parse((item.Cells["Qty"].Value == null ? "0" : item.Cells["Qty"].Value.ToString()));
                            double iSum = 0;

                            for (int iDr = 0; iDr < dgv.Rows.Count; iDr++)
                            {
                                var dr = dgv.Rows[iDr];
                                if (PublicStatic.isCancel)
                                {
                                    StaticHelper._MainForm.ProgressClear();
                                    return;
                                }
                                if (dr.Cells["ItemCode"].Value.ToString() == sItemCode)
                                { iSum += double.Parse(dr.Cells["Allocated"].Value.ToString()); }
                            }
                            
                            if (iTotal < iSum)
                            {
                                for (int iDr = 0; iDr < dgv.Rows.Count; iDr++)
                                {
                                    var dr = dgv.Rows[iDr];
                                    if (PublicStatic.isCancel)
                                    {
                                        StaticHelper._MainForm.ProgressClear();
                                        return;
                                    }

                                    if (dr.Cells["ItemCode"].Value.ToString() == sItemCode)
                                    {
                                        dr.DefaultCellStyle.BackColor = Color.Red;
                                        Application.DoEvents();
                                    }
                                }
                                
                            }
                            else if (iTotal == iSum)
                            {
                                for (int iDr = 0; iDr < dgv.Rows.Count; iDr++)
                                {
                                    var dr = dgv.Rows[iDr];
                                    if (PublicStatic.isCancel)
                                    {
                                        StaticHelper._MainForm.ProgressClear();
                                        return;
                                    }

                                    if (dr.Cells["ItemCode"].Value.ToString() == sItemCode)
                                    {
                                        dr.DefaultCellStyle.BackColor = Color.LightBlue;
                                        Application.DoEvents();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ProgressClear();
                        StaticHelper._MainForm.ShowMessage("Please input allocated quantity first.", true);
                    }
                }
                else
                {
                    StaticHelper._MainForm.ProgressClear();
                    StaticHelper._MainForm.ShowMessage("Data not found.", true);
                }
            }
            else
            {
                StaticHelper._MainForm.ProgressClear();
                StaticHelper._MainForm.ShowMessage("Please choose ranking level.", true);
            }
        }
        #endregion

        #region Uploading
        public void GenerateUploading()
        {
            if (MetroMessageBox.Show(StaticHelper._MainForm, $"This will serve as your inventory transfer request. Do you want to continue?", LibraryHelper.AssemblyInfo.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                string ret = "";
                if (form.rbCreateNewAlloc.Checked)
                { ret = UploadITR(); }
                else if (form.rbRepeatOrder.Checked || form.rbAllocationApproval.Checked)
                { ret = UpdateITR(); }

                if (ret == "")
                {
                    StaticHelper._MainForm.ShowMessage("Operation completed successfully");
                    form.btnGenerate.Enabled = false;
                    form.btnPrev.Enabled = false;
                    form.btnCancel.Enabled = false;
                    //form.MetroTabControl.Enabled = false;

                    foreach (Control ctl in form.MetroTabControl.Controls) ctl.Enabled = false;

                    form.btnFinish.Enabled = true;
                    form.dgvSummary.Enabled = true;
                    PublicStatic.sbCardCode.Clear();
                    StaticHelper._MainForm.ProgressClear();
                }
                else
                {
                    for (int i = 0; i < form.dgvSummary.Rows.Count; i++)
                    {
                        var dr = form.dgvSummary.Rows[i];
                        if (PublicStatic.sbCardCode.ToString().Contains(dr.Cells["CardCode"].Value.ToString()))
                        { dr.DefaultCellStyle.BackColor = Color.Green; }
                    }
                    
                    StaticHelper._MainForm.ShowMessage(ret, true);
                    StaticHelper._MainForm.ProgressClear();
                }

            }
        }

        public string UploadITR()
        {
            string result = "";
            var dt = new DataTable();
            StaticHelper._MainForm.Progress("Please wait...", 1,100);
            var json = new StringBuilder();
            int cnt = form.dgvSummary.Rows.Count - 1;
            var sQuery = "";
            int max = form.dgvSummary.Rows.Count;
            int min = 0;

            if (string.IsNullOrEmpty(PublicStatic.sbCardCode.ToString()))
            { PublicStatic.sRunID = DateTime.Now.ToString("yyMMddHHmmss"); }

            //foreach (DataGridViewRow dr in form.dgvSummary.Rows)
            for (int i = 0; i < form.dgvSummary.Rows.Count; i++)
            {
                var dr = form.dgvSummary.Rows[i];
                min++;
                var sCardCode = dr.Cells["CardCode"].Value.ToString();

                if (!PublicStatic.sbCardCode.ToString().Contains(sCardCode))
                {
                    
                    sQuery = helper.ReadDataRow(sapHana.Get(SP.AW_POST_BPWHS), 1, "", 0);
                    var sWhsTo = helper.ReadDataRow(sapHana.Get(sQuery), 0, "", 0);
                    var sWhsFrom = helper.ReadDataRow(sapHana.Get(SP.AW_POST_WHS), 0, "", 0);

                    var sNextCardCode = "";
                    var sAddId = "";

                    if (dr.Index != cnt)
                    { sNextCardCode = form.dgvSummary.Rows[dr.Index + 1].Cells["CardCode"].Value.ToString(); }

                    if (dr.Index == 0)
                    {
                        sQuery = helper.ReadDataRow(sapHana.Get(SP.AW_POST_BPAddID), 1, "", 0);
                        sAddId = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, sCardCode)), 0, "", 0);
                        var sStreet = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, sCardCode)), 1, "", 0);
                        UploadITRHeader(json, sCardCode, sAddId, sStreet, sWhsFrom, sWhsTo, PublicStatic.sRunID);
                        UploadITRLines(json, dr.Cells["ItemCode"].Value.ToString(), 
                                            dr.Cells["Qty"].Value.ToString(),
                                            dr.Cells["Color"].Value.ToString(),
                                            dr.Cells["Size"].Value.ToString(), 
                                            dr.Cells["Style"].Value.ToString(),
                                            sWhsFrom, sWhsTo, sCardCode);

                        if (dr.Index != cnt)
                        {
                            if (sCardCode == sNextCardCode && sNextCardCode != "")
                            { json.AppendLine(","); }
                            else
                            {
                                json.AppendLine($"  ]");
                                json.AppendLine("}");
                                result = SAPHana.PostITR_AllocationWizard(json);

                                if (result.Contains("error"))
                                { return result; }
                                else
                                {
                                    PublicStatic.sbCardCode.AppendLine($"{result};");
                                    result = "";
                                }
                            }
                        }
                        else
                        {
                            result = SAPHana.PostITR_AllocationWizard(json);

                            if (result.Contains("error"))
                            { return result; }
                            else
                            {
                                PublicStatic.sbCardCode.AppendLine($"{result};");
                                result = "";
                            }

                        }
                    }
                    else if (dr.Index <= cnt)
                    {
                        if (sCardCode == sNextCardCode && sNextCardCode != "")
                        {
                            if (json.ToString().Contains("CardCode") == false)
                            {
                                
                                sQuery = helper.ReadDataRow(sapHana.Get(SP.AW_POST_BPAddID), 1, "", 0);
                                sAddId = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, sCardCode)), 0, "", 0);
                                var sStreet = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, sCardCode)), 1, "", 0);
                                UploadITRHeader(json, sCardCode, sAddId, sStreet, sWhsFrom, sWhsTo, PublicStatic.sRunID);
                            }
                            UploadITRLines(json, dr.Cells["ItemCode"].Value.ToString(), dr.Cells["Qty"].Value.ToString(), dr.Cells["Color"].Value.ToString(), dr.Cells["Size"].Value.ToString(), dr.Cells["Style"].Value.ToString(), sWhsFrom, sWhsTo, sCardCode);
                            json.Append(",");
                        }
                        else
                        {
                            if (json.ToString().Contains("CardCode") == false)
                            {
                                
                                sQuery = helper.ReadDataRow(sapHana.Get(SP.AW_POST_BPAddID), 1, "", 0);

                                sAddId = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, sCardCode)), 0, "", 0);
                                var sStreet = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, sCardCode)), 1, "", 0);
                                UploadITRHeader(json, sCardCode, sAddId, sStreet, sWhsFrom, sWhsTo, PublicStatic.sRunID);
                            }
                            UploadITRLines(json, dr.Cells["ItemCode"].Value.ToString(), dr.Cells["Qty"].Value.ToString(), dr.Cells["Color"].Value.ToString(), dr.Cells["Size"].Value.ToString(), dr.Cells["Style"].Value.ToString(), sWhsFrom, sWhsTo, sCardCode);
                            json.AppendLine($"  ]");
                            json.AppendLine("}");
                            result = SAPHana.PostITR_AllocationWizard(json);

                            if (result.Contains("error"))
                            { return result; }
                            else
                            {
                                PublicStatic.sbCardCode.AppendLine($"{result};");
                                result = "";
                            }

                            if (sNextCardCode != "")
                            {
                                
                                sQuery = helper.ReadDataRow(sapHana.Get(SP.AW_POST_BPWHS), 1, "", 0);
                                sWhsTo = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, sNextCardCode)), 0, "", 0);
                                json.Clear();
                                sQuery = helper.ReadDataRow(sapHana.Get(SP.AW_POST_BPAddID), 1, "", 0);
                                sAddId = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, sNextCardCode)), 0, "", 0);
                                var sStreet = helper.ReadDataRow(sapHana.Get(string.Format(sQuery, sNextCardCode)), 1, "", 0);
                                UploadITRHeader(json, sNextCardCode, sAddId, sStreet, sWhsFrom, sWhsTo, PublicStatic.sRunID);
                            }
                        }
                    }
                }
                StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {min} out of {max}", min, max);
            }
            return result;
        }

        public string UpdateITR()
        {
            string result = "";
            var dt = new DataTable();
            StaticHelper._MainForm.Progress("Please wait...", 1,100);
            var json = new StringBuilder();
            int cnt = form.dgvSummary.Rows.Count - 1;
            int max = form.dgvSummary.Rows.Count;
            int min = 0;
            string sCardCode = "";

            //foreach (DataGridViewRow dr in form.dgvSummary.Rows)

            for (int i = 0; i < form.dgvSummary.Rows.Count; i++)
            {
                var dr = form.dgvSummary.Rows[i];
                min++;

                var sNextCardCode = "";
                if (sCardCode == "" || !PublicStatic.sbCardCode.ToString().Contains(dr.Cells["CardCode"].Value.ToString()))
                {
                    if (dr.Index != cnt)
                    { sNextCardCode = form.dgvSummary.Rows[dr.Index + 1].Cells["CardCode"].Value.ToString(); }

                    if (sCardCode != dr.Cells["CardCode"].Value.ToString())
                    {
                        sCardCode = dr.Cells["CardCode"].Value.ToString();
                        UpdateITRHeader(json);
                        UpdateITRLines(json, dr.Cells["ItemCode"].Value.ToString(), dr.Cells["Qty"].Value.ToString(), dr.Cells["LineNum"].Value.ToString());
                    }
                    else
                    { UpdateITRLines(json, dr.Cells["ItemCode"].Value.ToString(), dr.Cells["Qty"].Value.ToString(), dr.Cells["LineNum"].Value.ToString()); }

                    if (dr.Index != cnt)
                    {
                        if (sCardCode == sNextCardCode && sNextCardCode != "")
                        { json.AppendLine(","); }
                        else
                        {
                            json.AppendLine($"  ]");
                            json.AppendLine("}");
                            result = SAPHana.PatchITR_AllocationWizard(json, int.Parse(dr.Cells["DocEntry"].Value.ToString()));

                            if (result.Contains("error"))
                            { return result; }
                            else
                            {
                                json.Clear();
                                PublicStatic.sbCardCode.AppendLine($"{sCardCode};");
                                result = "";
                            }
                        }
                    }
                    else
                    {
                        json.AppendLine($"  ]");
                        json.AppendLine("}");
                        result = SAPHana.PatchITR_AllocationWizard(json, int.Parse(dr.Cells["DocEntry"].Value.ToString()));

                        if (result.Contains("error"))
                        { return result; }
                        else
                        {
                            json.Clear();
                            PublicStatic.sbCardCode.AppendLine($"{sCardCode};");
                            result = "";
                        }

                    }
                }
                StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {min} out of {max}", min, max);
            }

            return result;
        }

        void UploadITRHeader(StringBuilder json, string cardcode, string addid, string street, string whsfrom, string whsto, string runId)
        {
            var sSeries = helper.ReadDataRow(sapHana.Get(SP.AW_POST_Series), 0, "", 0);
            var sTransferType = helper.ReadDataRow(sapHana.Get(SP.AW_POST_TransType), 0, "", 0);
            
            json.AppendLine("{");
            json.AppendLine($@" ""CardCode"" : ""{cardcode}"",");
            json.AppendLine($@" ""Series"" : {sSeries},");
            json.AppendLine($@" ""DocDate"" : ""{form.dtpDocDate.Value.ToString("yyyy-MM-dd")}"",");
            json.AppendLine($@" ""DueDate"" : ""{form.dtpDueDate.Value.ToString("yyyy-MM-dd")}"",");
            json.AppendLine($@" ""TaxDate"" : ""{form.dtpTaxDate.Value.ToString("yyyy-MM-dd")}"",");
            json.AppendLine($@" ""U_AddID"" : ""{addid}"",");
            json.AppendLine($@" ""Address"" : ""{street}"",");
            json.AppendLine($@" ""FromWarehouse"" : ""{whsfrom}"",");
            json.AppendLine($@" ""ToWarehouse"" : ""{whsto}"",");
            json.AppendLine($@" ""U_TransferType"" : ""{sTransferType}"",");
            json.AppendLine($@" ""U_Remarks"" : ""{form.txtRemarks.Text}"",");
            json.AppendLine($@" ""U_PrepBy"" : ""{EasySAPCredentialsModel.EmployeeCompleteName}"",");
            json.AppendLine($@" ""U_PostRem"": ""{DateTime.Now} - {EasySAPCredentialsModel.ESUserId} - {Environment.MachineName}"",");
            json.AppendLine($@" ""U_RunID"" : {runId},");
            json.AppendLine($@" ""Comments"": ""Created by EasySAP | Allocation Wizard : {EasySAPCredentialsModel.ESUserId} : {DateTime.Now} | Powered By : DIREC"",");

            //(ItemName.Length <= 100 ? ItemName : ItemName.Substring(0, 100))

            for (int i = 0; i < form.dgvParameter.Rows.Count; i++)
            {
                var drlist = form.dgvParameter.Rows[i];
                var sCode = LibraryHelper.DataGridViewRowRet(drlist, "Code");
                var sValue = LibraryHelper.DataGridViewRowRet(drlist, "Field");

                if (sCode.Contains("By"))
                { json.AppendLine($@" ""{sCode}"" : ""{(sValue.Length <= 25 ? sValue : sValue.Substring(0, 25))}"","); }
                else
                { json.AppendLine($@" ""{sCode}"" : ""{sValue}"","); }
            }
            json.AppendLine($@" ""StockTransferLines"" : [");
        }

        void UpdateITRHeader(StringBuilder json)
        {
            json.AppendLine("{");
            json.AppendLine($@" ""U_PostRem"": ""{DateTime.Now} - {EasySAPCredentialsModel.ESUserId} - {Environment.MachineName}"",");
            json.AppendLine($@" ""Comments"": ""Updated by EasySAP | Allocation Wizard : {EasySAPCredentialsModel.ESUserId} : {DateTime.Now} | Powered By : DIREC"",");
            
            json.AppendLine($@" ""StockTransferLines"" : [");
        }

        void UploadITRLines(StringBuilder json, string ItemCode, string Quantity, string Color, string Size, string Style, string whsfrom, string whsto, string cardcode)
        {
            json.AppendLine("       {");
            json.AppendLine($@"       ""ItemCode"" : ""{ItemCode}"",");
            json.AppendLine($@"       ""Quantity"" : ""{Quantity}"",");
            //
            json.AppendLine($@"       ""U_Color"" : ""{Color}"",");
            json.AppendLine($@"       ""U_Size"" : ""{Size}"",");
            json.AppendLine($@"       ""U_Style"" : ""{Style}"",");
            //
            json.AppendLine($@"       ""FromWarehouseCode"" : ""{whsfrom}"",");
            json.AppendLine($@"       ""WarehouseCode"" : ""{whsto}"",");
            json.AppendLine($@"       ""ProjectCode"" : ""{cardcode}"",");            
            json.AppendLine($@"       ""DistributionRule2"" : ""{form.dgvItemOtherParam.Rows[0].Cells["Value"].Value}"",");

            var qry = string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_Company), 1, "", 0), cardcode, ItemCode);
            json.AppendLine($@"       ""U_Company"" : ""{helper.ReadDataRow(sapHana.Get(qry), 0, "", 0)}"",");
            qry = string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_GetItemDetails), 1, "", 0), ItemCode);
            json.AppendLine($@"       ""U_SortCode"" : ""{helper.ReadDataRow(sapHana.Get(qry), 3, "", 0)}"",");
            json.AppendLine($@"       ""U_AppStat"" : ""P""");
            json.AppendLine("       }");
        }

        void UpdateITRLines(StringBuilder json, string ItemCode, string Quantity, string LineNum)
        {
            json.AppendLine("       {");
            json.AppendLine($@"       ""ItemCode"" : ""{ItemCode}"",");
            json.AppendLine($@"       ""Quantity"" : ""{Quantity}"",");
            json.AppendLine($@"       ""LineNum"" : ""{LineNum}"",");
            json.AppendLine($@"       ""U_AppStat"" : ""A""");
            json.AppendLine("       }");
        }
        #endregion
    }
}

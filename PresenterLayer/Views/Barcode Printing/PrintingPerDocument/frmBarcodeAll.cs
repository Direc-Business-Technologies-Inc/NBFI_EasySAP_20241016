using Context;
using MetroFramework;
using MetroFramework.Forms;
using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using PresenterLayer.Helper;
using DirecLayer;
using DomainLayer.Models.Inventory;
using PresenterLayer.Services;

namespace PresenterLayer.Views
{
    public partial class frmBarcodeAll : MetroForm
    {
        private string previewPath = "";
        #region Bartender
        Engine btEngine = null; // The BarTender Print Engine
        LabelFormatDocument btFormat = null; // The currently open Format
        //bool isClosing = false; // Set to true if we are closing. This helps discontinue thumbnail loading.
        //string BarcodeFile;
        string thumbnailFile = "";
        //DataAccess da = new DataAccess();


        #endregion

        private SAPHanaAccess sapHana { get; set; }
        private const string appName = "Label Print";
        //frmMainStaticHelper._MainForm;
        int iRow = 0;
        string iColumn;
        bool iCanceled;
        string tmpFile = Path.GetTempFileName();
        string path = "";
        int oCnt { get; set; } = 0;

        private static int defaultColumn = 1, _rowIndex = 0;
        public string BPCode = "";
        public string DocEntry2 = "";
        public bool UPCcheck = false;

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmBarcodeAll()
        {
            InitializeComponent();
            //this.frmMain = StaticHelper._MainForm;
            ActiveControl = SearchDoc;
            WindowState = FormWindowState.Maximized;
            sapHana = new SAPHanaAccess();
            Printers();
            gvSetup(gvBarCode);
            gvprinterSetup(gvPrinter);
        }

        //public void LoadLinq()
        //{
        //    foreach (DataGridViewRow dr in gvBarCode.Rows)
        //    {
        //        var LinqLines = LinqAccess.frmBarcodeItem.Where(x => x.DocEntry == Convert.ToInt32(dr.Cells["DocEntry"].Value) && x.IsTick == true && x.TableID == dr.Cells["TableLine"].Value.ToString());
        //        if (LinqLines.Count() > 0)
        //        { dr.Cells[0].Value = true; }
        //        else { dr.Cells[0].Value = false; }
        //    }
        //}

        private void gvSetup(DataGridView dt)
        {
            try
            {
                dt.Rows.Clear();
                dt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dt.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dt.MultiSelect = false;
                dt.RowTemplate.Resizable = DataGridViewTriState.False;
                dt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dt.RowHeadersVisible = false;
                dt.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dt.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);
                //===============================HEADER WITH TICKBOX=========================
                DataGridViewCheckBoxColumn col1 = new DataGridViewCheckBoxColumn();
                var checkheader = new CheckBoxHeaderCell();
                checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
                col1.HeaderCell = checkheader;
                dt.Columns.Add(col1);
                //===========================================================================
                dt.ColumnCount = 10;
                dt.Columns[1].Name = "From Document";
                dt.Columns[2].Name = "DocEntry";
                dt.Columns[3].Name = "DocNum";
                dt.Columns[4].Name = "Pick Number";
                dt.Columns[5].Name = "DocType";
                dt.Columns[6].Name = "CardCode";
                dt.Columns[7].Name = "Card Name";
                dt.Columns[8].Name = "Doc Date";
                dt.Columns[9].Name = "TableLine";

                DataTable dtItems = new DataTable();
                var helper = new DataHelper();
                var GetITRSOList = sapHana.Get(SP.BP_BarcodeAll_ITRSOList);

                string ITRSOListQry = helper.ReadDataRow(GetITRSOList, 1, "", 0);

                dtItems = sapHana.Get(ITRSOListQry);

                foreach (DataRow dr in dtItems.Rows)
                {
                    dt.Rows.Add(false, dr["FromDoc"].ToString(), dr["DocEntry"].ToString(), dr["DocNum"].ToString()
                    , dr["AbsEntry"].ToString(), dr["DocType"].ToString(), dr["CardCode"].ToString()
                    , dr["Card Name"].ToString(), dr["Doc Date"].ToString(), dr["TableLine"].ToString());
                }
                dt.Columns["From Document"].ReadOnly = true;
                dt.Columns["DocEntry"].Visible = false;
                dt.Columns["DocNum"].Visible = false;
                dt.Columns["Pick Number"].ReadOnly = true;
                dt.Columns["DocType"].Visible = false;
                dt.Columns["CardCode"].ReadOnly = true;
                dt.Columns["Card Name"].ReadOnly = true;
                dt.Columns["Doc Date"].ReadOnly = true;
                dt.Columns["TableLine"].Visible = false;
                iColumn = "DocNum";

            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message); }
        }

        #region Tickbox Header
        void checkheader_OnCheckBoxHeaderClick(CheckBoxHeaderCellEventArgs e)
        {
            if (gvBarCode.Rows.Count > 0)
            {
                gvBarCode.BeginEdit(true);
                foreach (DataGridViewRow item in gvBarCode.Rows)
                {
                    item.Cells[0].Value = e.IsChecked;
                }
                gvBarCode.EndEdit();
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Empty row");
            }
        }
        public class CheckBoxHeaderCellEventArgs : EventArgs
        {
            private bool _isChecked;
            public bool IsChecked
            {
                get { return _isChecked; }
            }

            public CheckBoxHeaderCellEventArgs(bool _checked)
            {
                _isChecked = _checked;

            }

        }
        public delegate void CheckBoxHeaderClickHandler(CheckBoxHeaderCellEventArgs e);

        class CheckBoxHeaderCell : DataGridViewColumnHeaderCell
        {
            Size checkboxsize;
            bool ischecked;
            Point location;
            Point cellboundsLocation;
            CheckBoxState state = CheckBoxState.UncheckedNormal;
            public event CheckBoxHeaderClickHandler OnCheckBoxHeaderClick;
            public CheckBoxHeaderCell()
            {
                location = new Point();
                cellboundsLocation = new Point();
                ischecked = false;
            }
            protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
            {
                /* Make a condition to check whether the click is fired inside a checkbox region */
                Point clickpoint = new Point(e.X + cellboundsLocation.X, e.Y + cellboundsLocation.Y);
                if ((clickpoint.X > location.X && clickpoint.X < (location.X + checkboxsize.Width)) && (clickpoint.Y > location.Y && clickpoint.Y < (location.Y + checkboxsize.Height)))
                {
                    ischecked = !ischecked;
                    if (OnCheckBoxHeaderClick != null)
                    {
                        OnCheckBoxHeaderClick(new CheckBoxHeaderCellEventArgs(ischecked));
                        this.DataGridView.InvalidateCell(this);
                    }
                }
                base.OnMouseClick(e);
            }
            protected override void Paint(Graphics graphics, Rectangle clipBounds,
                 Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle
                advancedBorderStyle, DataGridViewPaintParts paintParts)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState,
                value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                checkboxsize = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal);
                location.X = cellBounds.X + (cellBounds.Width / 2 - checkboxsize.Width / 2);
                location.Y = cellBounds.Y + (cellBounds.Height / 2 - checkboxsize.Height / 2);
                cellboundsLocation = cellBounds.Location;
                if (ischecked)
                    state = CheckBoxState.CheckedNormal;
                else
                    state = CheckBoxState.UncheckedNormal;
                CheckBoxRenderer.DrawCheckBox(graphics, location, state);
            }
        }
        #endregion
        private void Printers()
        {
            try
            {
                btEngine = new Engine(true);
                btEngine.Start();
                //======Debugging=======
                //btEngine.Window.VisibleWindows = VisibleWindows.All;
                Printers printers = new Printers();
                foreach (Printer printer in printers)
                {
                    cbPrinter.Items.Add(printer.PrinterName);
                }
                thumbnailFile = Path.GetTempFileName();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                StaticHelper._MainForm.ShowMessage(ex.Message);
            }
        }
        private void btnCommand_Click(object sender, EventArgs e)
        {
            var test = "1001";
            try
            {
                List<DataGridViewRow> list = gvBarCode.Rows.Cast<DataGridViewRow>().Where(k => Convert.ToBoolean(k.Cells[0].Value) == true).ToList();
                if (list.Count() > 0)
                {
                    if (gvPrinter.Rows.Count != 0)
                    {
                        int iPrintcount = 0;
                        string oPath = "";
                        oCnt = 0;
                        int iCntSuccess = 0;
                        int iCntChkDocDueDate = 0;
                        int iCntQty = 0;
                        var helper = new DataHelper();

                        foreach (DataGridViewRow row in gvBarCode.Rows)
                        {
                            //=========================CREATE DATA TEMPLATE============================START
                            if (row.Cells[0].Value is true)
                            {
                                string letters = "ABCDEFGHIJKL";
                                string oTbline = row.Cells["TableLine"].Value.ToString();
                                string oTable = oTbline == "RDR1" ? oTable = "ORDR" : oTable = "OWTQ";
                                int oDocEntry = Convert.ToInt32(row.Cells["DocEntry"].Value);
                                int oAbsEntry = Convert.ToInt32(row.Cells["Pick Number"].Value);
                                var iLine = LinqAccess.frmBarcodeItem.Where(x => x.DocEntry == oAbsEntry && x.IsTick == true && x.TableID == oTbline).Distinct();
                                int iMax = iLine.Count();
                                string bufRRow = "";
                                string bufMRow = "";
                                bool noMaintenance = false;

                                string buf = BarcodeCtrl.GetBarcodeHeader(); // header    
                                //=========================CREATE DATA TEMPLATE============================END

                                test = $"1002 | Data preparation {oCnt} out of {iMax}";
                                string bufRH = buf, bufMH = buf;
                                foreach (var x in iLine)
                                {
                                    string oBPCode = row.Cells["CardCode"].Value.ToString().Replace("'", "''");
                                    string oItemCode = x.ItemCode.ToString().Replace("'", "''");
                                    double oQty = Convert.ToDouble(x.Qty.ToString());
                                    string sFromDoc = row.Cells["From Document"].Value.ToString();
                                    string sCheckDate = "N";

                                    if (oQty > 0)
                                    {
                                        test = $"1003 | Data preparation {oCnt} out of {iMax}";
                                        var GetCheckDocDueDateQry = sapHana.Get(SP.BP_BarcodeAll_CheckDocDueDate);
                                        test = $"1004 | Data preparation {oCnt} out of {iMax}";
                                        var GetItemDetailsQry = sapHana.Get(SP.BP_BarcodeAll);

                                        string sCheckDocDueDateQry = string.Format(helper.ReadDataRow(GetCheckDocDueDateQry, 1, "", 0), oBPCode, oTbline, oItemCode, oTable, oDocEntry);
                                        string sGetItemDetailsQry = string.Format(helper.ReadDataRow(GetItemDetailsQry, 1, "", 0), oBPCode, oTbline, oItemCode, oTable, oDocEntry);
                                    
                                        //Added new logic for Printing      11/29/19
                                        var CheckExistingCampaigns = string.Format(helper.ReadDataRow(sapHana.Get(SP.BP_BarcodeAll_CheckExistingCampaigns), 1, "", 0), oBPCode);
                                        string GetCpnNo = "0";
                                        string noMaintenance_path = "";

                                        noMaintenance = BarcodeCtrl.CheckNoMaintenance(oBPCode);
                                        test = $"1005 | Data preparation {oCnt} out of {iMax}";
                                        if (noMaintenance)
                                        {
                                            test = $"1006 | Data preparation {oCnt} out of {iMax}";
                                            var GetCheckDocDueDateNM = sapHana.Get(SP.BP_BarcodeAll_CheckDocDueDateNM);
                                            var GetBarcodeFromOITM = sapHana.Get(SP.BP_BarcodeAll_BarcodeFromOITM);
                                            var GetPath = sapHana.Get(SP.BP_BarcodeAll_GetPath);
                                            sCheckDocDueDateQry = string.Format(helper.ReadDataRow(GetCheckDocDueDateNM, 1, "", 0), oBPCode, oTbline, oItemCode, oTable, oDocEntry);
                                            sGetItemDetailsQry = string.Format(helper.ReadDataRow(GetBarcodeFromOITM, 1, "", 0), oBPCode, oTbline, oItemCode, oTable, oDocEntry);
                                            var sCheckPathQry = string.Format(helper.ReadDataRow(GetPath, 1, "", 0), oBPCode);
                                            var dt_getPath = sapHana.Get(sCheckPathQry);
                                            noMaintenance_path = dt_getPath.Rows[0]["U_Path"].ToString();
                                        }
                                        else if (sFromDoc == "PO" && cb_UPC.Checked == true)
                                        {
                                            test = $"1007 | Data preparation {oCnt} out of {iMax}";
                                            var GetBarcodeFromOITMandPO = sapHana.Get(SP.BP_BarcodeAll_BarcodeFromOITMandPOforUPC);
                                            sGetItemDetailsQry = string.Format(helper.ReadDataRow(GetBarcodeFromOITMandPO, 1, "", 0), oBPCode, oTbline, oItemCode, oTable, oDocEntry);
                                        }
                                        else if (sFromDoc == "PO" && cb_UPC.Checked == false)
                                        {
                                            test = $"1008 | Data preparation {oCnt} out of {iMax}";
                                            var GetBarcodeFromOITMandPO = sapHana.Get(SP.BP_BarcodeAll_BarcodeFromOITMandPO);
                                            sGetItemDetailsQry = string.Format(helper.ReadDataRow(GetBarcodeFromOITMandPO, 1, "", 0), oBPCode, oTbline, oItemCode, "O" + oTbline.Remove(3, 1), oDocEntry);
                                        }
                                        else if (sFromDoc == "ITR - CPO Store")
                                        {
                                            test = $"1009 | Data preparation {oCnt} out of {iMax}";
                                            var GetBarcodeFromOITMandITRCpo = sapHana.Get(SP.BP_BarcodeAll_BarcodeFromOITMandITRCpo);
                                            sGetItemDetailsQry = string.Format(helper.ReadDataRow(GetBarcodeFromOITMandITRCpo, 1, "", 0), oBPCode, oTbline.Replace("2","1"), oItemCode, "O" + oTbline.Remove(3, 1), oDocEntry);
                                        }

                                        //Added new logic for Printing      11/29/19
                                        else if (sapHana.Get(CheckExistingCampaigns).Rows.Count > 0)
                                        {
                                            test = $"1010 | Data preparation {oCnt} out of {iMax}";
                                            if (Convert.ToInt32(sapHana.Get(CheckExistingCampaigns).Rows[0]["CampaignCount"].ToString()) > 1)
                                            {
                                                var GetExistingCampaigns = string.Format(helper.ReadDataRow(sapHana.Get(SP.BP_BarcodeAll_GetExistingCampaigns), 1, "", 0), oBPCode);
                                                test = $"1014 | Data preparation {oCnt} out of {iMax}";
                                                if (sapHana.Get(GetExistingCampaigns).Rows.Count > 0)
                                                {
                                                    test = $"1012 | Data preparation {oCnt} out of {iMax}";
                                                    DataTable dtGetC = sapHana.Get(GetExistingCampaigns);

                                                    if (dtGetC.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow rowC in dtGetC.Rows)
                                                        {
                                                            var CheckItemIfExistingCampaign = string.Format(helper.ReadDataRow(sapHana.Get(SP.BP_BarcodeAll_CheckItemIfExistingCampaign), 1, "", 0), oBPCode, rowC["CpnNo"].ToString(), oItemCode);

                                                            test = $"1013 | Data preparation {oCnt} out of {iMax}";
                                                            if (sapHana.Get(CheckItemIfExistingCampaign).Rows.Count > 0)
                                                            {
                                                                GetCpnNo = rowC["CpnNo"].ToString();
                                                                break;
                                                            }
                                                        }

                                                        if (GetCpnNo != "0")
                                                        {
                                                            test = $"1014 | Data preparation {oCnt} out of {iMax}";
                                                            var CheckDocDueDateWithCpnNo = sapHana.Get(SP.BP_BarcodeAll_CheckDocDueDateWithCpnNo);
                                                            sCheckDocDueDateQry = string.Format(helper.ReadDataRow(CheckDocDueDateWithCpnNo, 1, "", 0), oBPCode, oTbline, oItemCode, oTable, oDocEntry, GetCpnNo);

                                                            test = $"1015 | Data preparation {oCnt} out of {iMax}";
                                                            var BarcodeAll_WithCpnNo = sapHana.Get(SP.BP_BarcodeAll_WithCpnNo);
                                                            sGetItemDetailsQry = string.Format(helper.ReadDataRow(BarcodeAll_WithCpnNo, 1, "", 0), oBPCode, oTbline, oItemCode, oTable, oDocEntry, GetCpnNo);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (sFromDoc != "ITR - CPO Store")
                                        {
                                            test = $"1016 | Data preparation {oCnt} out of {iMax}";
                                            if (sapHana.Get(sCheckDocDueDateQry).Rows.Count > 0)
                                            {
                                                test = $"1017 | Data preparation {oCnt} out of {iMax}";
                                                sCheckDate = sapHana.Get(sCheckDocDueDateQry).Rows[0]["WithinDateRange"].ToString();
                                            }
                                        }

                                        if (sCheckDate == "Y" || (sFromDoc == "PO" || sFromDoc == "ITR - CPO Store"))
                                        {
                                            test = sGetItemDetailsQry;//sGetItemDetailsQry;
                                            DataTable dtCampDetails = sapHana.Get(sGetItemDetailsQry);
                                            //MessageBox.Show(sGetItemDetailsQry);
                                            int rowPcnt = 0;

                                            if (dtCampDetails.Rows.Count > 0)
                                            {
                                                foreach (DataGridViewRow rowP in gvPrinter.Rows)
                                                {
                                                    test = $"1019 | Data preparation {oCnt} out of {iMax}";
                                                    Double SRP = gvPrinter.Rows[rowPcnt].Cells[1].Value.ToString() == "Regular" ? SRP = Convert.ToDouble(dtCampDetails.Rows[0]["RegularPrice"].ToString()) : SRP = Convert.ToDouble(dtCampDetails.Rows[0]["MarkDownPrice"].ToString());
                                                    oPath = noMaintenance ? noMaintenance_path : dtCampDetails.Rows[0]["U_Path"].ToString();
                                                    //MessageBox.Show($"No Maintenance ({noMaintenance}) ? {noMaintenance_path} : {oPath}; ");

                                                    //MessageBox.Show(oPath);
                                                    if (CheckIfConnected(oPath))
                                                    {
                                                        test = $"1020 | Data preparation {oCnt} out of {iMax}";
                                                        string AG3 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMyy").Remove(2, 1);
                                                        string SC1 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yyMM");
                                                        string strSC2 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yyM").Substring(2);
                                                        string SC2 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yyMM").Remove(2, 2) + letters.Substring(Convert.ToInt32(strSC2) - 1, 1);
                                                        string SC3 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yMM").Remove(0, 1);
                                                        string SC4 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MM/yy");
                                                        //string SC5 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMy");
                                                        string strColor1 = dtCampDetails.Rows[0]["Color"].ToString();
                                                        string strColor2 = strColor1 == "" ? "" : strColor1.Length > 3 ? strColor1.Substring(0, 3) : strColor1;
                                                        string strDept = dtCampDetails.Rows[0]["DeptCode"].ToString();
                                                        string strSubDept = dtCampDetails.Rows[0]["SubDept"].ToString();
                                                        int oItemCodeCnt = oItemCode.Count() - 4;
                                                        string ItemType = dtCampDetails.Rows[0]["OITM_ItemClass"].ToString().Equals("Sale or Discontinued") ? "M" : "R";//dtCampDetails.Rows[0]["RegularPrice"].ToString().Contains(".00") ? "M" : "R";
                                                        string strItemType2 = ItemType == "R" ? "REG" : "X";
                                                        string MonthYear = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMMy");
                                                        string ReceiveCode = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MM/y");
                                                        string strSubClass = dtCampDetails.Rows[0]["SubClass"].ToString();
                                                        string strDeliveryDate = dtCampDetails.Rows[0]["DeliveryDate"].ToString() != "" ? Convert.ToDateTime(dtCampDetails.Rows[0]["DeliveryDate"].ToString()).ToString("yyyyMMdd") : "";
                                                        string SkuType = dtCampDetails.Rows[0]["SkuType"].ToString();
                                                        string strSkuType = SkuType.Substring(0, 1);
                                                        string strSKuType2 = "";
                                                        string strItemDescOrig = dtCampDetails.Rows[0]["IMD_ItemDesc"].ToString();
                                                        string strItemDesc = strItemDescOrig.Length > 15 ? strItemDescOrig.Substring(0, 15) : strItemDescOrig;
                                                        string strUPC = dtCampDetails.Rows[0]["UPC"].ToString();
                                                        string strCPN2_BarOption = strUPC == "" ? dtCampDetails.Rows[0]["CPN2_ItemCode"].ToString() : strUPC;
                                                        string strSKU = dtCampDetails.Rows[0]["SKU"].ToString();
                                                        string strCPN2_SKU = strSKU.Contains("-") ? strSKU.Remove(strSKU.IndexOf("-"), 2) : strSKU;
                                                        string strItemDesc_Long = dtCampDetails.Rows[0]["ItemDesc"].ToString();
                                                        string strItemDesc_Trim = strItemDesc_Long.Length > 20 ? strItemDesc_Long.Substring(0, 19) : strItemDesc_Long;
                                                        string strIMDSubCat = dtCampDetails.Rows[0]["IMD_SubCat"].ToString();
                                                        string strIMDCat = dtCampDetails.Rows[0]["IMD_Cat"].ToString();
                                                        string strPriceFormatZero = SRP.ToString("000000.00").Replace(".", "");
                                                        string strIMDCatCode = dtCampDetails.Rows[0]["OITM_CatCode"].ToString();
                                                        string strIMDSubCatCode = dtCampDetails.Rows[0]["OITM_SubCatCode"].ToString();
                                                        string strPromoIndi = ItemType == "R" ? "REGULAR" : "SALE";
                                                        string strAG4 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMyy");
                                                        string strMonthValue = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMM").ToUpper();
                                                        string strDateValue = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("dd");
                                                        string strYearValue = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yyyy");

                                                        if (SkuType == "Outright")
                                                        {
                                                            strSKuType2 = "01";
                                                        }
                                                        else if (SkuType == "Concession")
                                                        {
                                                            strSKuType2 = "02";
                                                        }
                                                        else
                                                        {
                                                            strSKuType2 = "03";
                                                        }

                                                        //string ItemType = gvPrinter.Rows[rowPcnt].Cells[1].Value.ToString() == "Regular" ? "R" : "M";

                                                        string bufRow = $"\"0\"" +               //AgeCode1
                                                                        $",\"0\"" +              //AgeCode2
                                                                        $",\"{AG3}\"" +          //AgeCode3
                                                                        $",\"{dtCampDetails.Rows[0]["BrandName"].ToString()}\"" +
                                                                        $",\"{Convert.ToDouble(dtCampDetails.Rows[0]["BeforePrice"]).ToString("#,##0.00")}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["Class"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CncCode"].ToString()}\"" +             //CNCcode
                                                                        $",\"{strColor2}\"" +                                               //Color
                                                                        $",\"0\"" +                                                         //ConCode
                                                                        $",\"{dtCampDetails.Rows[0]["DateFrom"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["DateTo"].ToString()}\"" +
                                                                        $",\"{strDeliveryDate}\"" +                                         //DeliveryDate or DeliveryData
                                                                        $",\"{strDept}\"" +                                                 //DeptCode
                                                                        $",\"{dtCampDetails.Rows[0]["DeptName"].ToString()}\"" +
                                                                        $",\"{strDept}-{strSubDept}\"" +                                    //Dept_SubDept
                                                                        $",\"0\"" +                                                         //DiscountFreeText
                                                                        $",\"{strDept}\"" +                                                 //Division
                                                                        $",\"0\"" +                                                         //HierarchyCode
                                                                        $",\"{oItemCode.Substring(oItemCodeCnt, 4)}\"" +                    //IntSKU
                                                                        $",\"{strItemDesc_Long}\"" +
                                                                        $",\"{ItemType}\"" +
                                                                        $",\"{strItemType2}\"" +
                                                                        $",\"{SRP.ToString("#,##0.00")}\"" +                        //MarkdownPrice
                                                                        $",\"{MonthYear}\"" +                                       //Month_Year
                                                                        $",\"{SRP.ToString("#,##0.00")}\"" +                        //Orig Price
                                                                        $",\"{strSKU}\"" +                                          //ORIN
                                                                        $",\"{SRP.ToString("#,##0.00")}\"" +                        //Price
                                                                        $",\"{strSKU}\"" +                                          //ProductNo
                                                                        $",\"{ItemType}\"" +                                        //PromoIndicator
                                                                        $",\"{ReceiveCode}\"" +                                     //ReceiveCode
                                                                        $",\"{SRP.ToString("#,##0.00")}\"" +                        //RegularPrice
                                                                        $",\"{SRP.ToString("#,##0.00")}\"" +                        //RetailPrice
                                                                        $",\"{SC1}\"" +                                             //SeasonCode1
                                                                        $",\"{SC2}\"" +                                             //SeasonCode2
                                                                        $",\"{SC3}\"" +                                             //SeasonCode3
                                                                        $",\"{SC4}\"" +                                             //SeasonCode4
                                                                        $",\"{dtCampDetails.Rows[0]["SalesUOM"].ToString()}\"" +    //SellingUOM
                                                                        $",\"{dtCampDetails.Rows[0]["Size"].ToString()}\"" +
                                                                        $",\"{strSKU}\"" +
                                                                        $",\"{SkuType}\"" +
                                                                        $",\"{strSKuType2}\"" +
                                                                        $",\"0\"" +                                                   //StoreName
                                                                        $",\"{dtCampDetails.Rows[0]["SubClass"].ToString()}\"" +
                                                                        $",\"{strSubDept}\"" +                                          //SubDept
                                                                        $",\"{strSubDept}{strSubClass}\"" +                             //SubDept_Class
                                                                        $",\"{dtCampDetails.Rows[0]["SupplierCode"].ToString()}\"" +    //SupplierCode
                                                                        $",\"{dtCampDetails.Rows[0]["SalesUOM"].ToString()}\"" +        //UOM
                                                                        $",\"{strUPC}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["SupplierCode"].ToString()}\"" +      //VendorCode
                                                                        $",\"{oItemCode}\"" +                                             //VendorPartNo
                                                                        $",\"{strSkuType}\"" +                                            //VendorType
                                                                        $",\"{dtCampDetails.Rows[0]["Style"].ToString()}\"" +             //VPN
                                                                        $",\"{dtCampDetails.Rows[0]["IMD_CodeBars"].ToString()}\"" +      //IMD_CodeBars
                                                                        $",\"{dtCampDetails.Rows[0]["IMD_Color"].ToString()}\"" +         //IMD_Color
                                                                        $",\"{dtCampDetails.Rows[0]["IMD_Size"].ToString()}\"" +          //IMD_Size
                                                                        $",\"{dtCampDetails.Rows[0]["IMD_Style"].ToString()}\"" +         //IMD_Style
                                                                        $",\"{strItemDesc}\"" +                                           //IMD_ItemDesc
                                                                        $",\"{dtCampDetails.Rows[0]["IMD_BrandName"].ToString()}\"" +     //IMD_BrandName
                                                                        $",\"{strCPN2_BarOption}\"" +                                     //CPN2_BarOption
                                                                        $",\"{strCPN2_SKU}\"" +                                           //CPN2_SKU
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_VPN"].ToString()}\"" +          //CPN2_VPN
                                                                        $",\"{strIMDSubCat}\"" +                                          //IMD_SubCat
                                                                        $",\"{strIMDCat}\"" +                                             //IMD_Cat
                                                                        $",\"{strItemDesc_Trim}\"" +                                      //ItemDesc_Trim
                                                                        $",\"{strIMDCat + strIMDSubCat}\"" +                              //IMD_CatSubCat
                                                                        $",\"{dtCampDetails.Rows[0]["CPN3_BrandCode"].ToString()}\"" +    //CPN3_BrandCode
                                                                        $",\"{dtCampDetails.Rows[0]["IMD_DeptName"].ToString()}\"" +      //IMD_DeptName
                                                                        $",\"{dtCampDetails.Rows[0]["CPN3_DeptCode"].ToString()}\"" +     //CPN3_DeptCode
                                                                        $",\"{strItemDescOrig}\"" +                                       //IMD_ItemDescFull
                                                                        $",\"{strPriceFormatZero}\"" +                                    //PriceFormatZero
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_LongItemDesc"].ToString()}\"" +     //CPN2_LongItemDesc
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_SizeShortName"].ToString()}\"" +    //OITM_SizeShortName
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_CodeBars"].ToString()}\"" +         //OITM_CodeBars
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_Division"].ToString()}\"" +      //CPN2_Division
                                                                        $",\"{strIMDCatCode}\"" +                                          //OITM_CatCode
                                                                        $",\"{strIMDSubCatCode}\"" +                                       //OITM_SubCatCode
                                                                        $",\"{strPromoIndi}\"" +                                           //PromoIndicator2
                                                                        $",\"{strAG4}\"" +                                                 //AgeCode4
                                                                        $",\"{dtCampDetails.Rows[0]["SampleCode"].ToString()}\"" +         //SampleCode
                                                                        $",\"{strMonthValue}\"" +                                          //MonthValue
                                                                        $",\"{strDateValue}\"" +                                           //DateValue
                                                                        $",\"{strYearValue}\"" +                                           //YearValue
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_BrandName"].ToString()}\"" +     //OITM_BrandName
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Section"].ToString()}\"" +       //OITM_Section
                                                                        $",\"{dtCampDetails.Rows[0]["CPN3_VendorID"].ToString()}\"" +    //CPN3_VendorID
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Department"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Brand2"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Dimension3"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Dimension4"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Dimension5"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_SubDepartment"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_SizeCategory"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Size2"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_ParentColor"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Class"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_SubClass"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Packaging"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Specification"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Collection"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_BrandCode"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_SortCode"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_ItemClass"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_StyleName"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Remarks"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OITM_Designer"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_AliasName"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_AddID"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_VatIdUnCmp"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_Notes"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_GlblLocNum"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_Series"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_GroupCode"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_Department"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_Dimension2"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_Dimension3"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_Dimension4"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_Dimension5"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_OrderClassification"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_StoreClassification"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_DeliveryClassification"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_Region"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["OCRD_ClassificationForSupplies"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["RDR1_U_SampleCode"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["RDR1_U_UnitPricePerPiece"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["ORDR_DocDueDate"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["ORDR_CancelDate"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_CpnLineNum"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_CpnNo"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_ItemCode"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_ItemGrp"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_ItemName"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_ItemType"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_LogIns"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_BarCodeColor"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Desc1"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Desc2"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc1"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc2"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc3"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc4"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc5"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_ItemType"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_MDPrice"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_OcpnArtNo"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_OldSKU"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_OldUPC"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_PartNo"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Path"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_PathMDPrice"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_PriceCat"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Pricetag"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_PricetagEquivalent"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_RPrice"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_SC"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_SeasonCode"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_Section"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_StyleCode"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_U_UPC"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["CPN2_VisOrder"].ToString()}\"" +
                                                                        $",\"{dtCampDetails.Rows[0]["SP_Price"].ToString()}\"\r\n";


                                                        //string sTempPath = oPath.Replace("\\NBFIHVMSVR01", "");
                                                        //sTempPath = "D:" + sTempPath;

                                                        string sTempPath = oPath;
                                                        bufRRow = "";
                                                        bufMRow = "";

                                                        //NEW LOGIC 050819
                                                        buf = "";
                                                        //bufRRow += bufRow;
                                                        //buf = bufRH + bufRRow;

                                                        //REPLACED THIS LOGIC 050819
                                                        if (ItemType == "R")
                                                        {
                                                            for (int qty = 0; qty < oQty; qty++)
                                                            { bufRRow += bufRow; }
                                                            buf = bufRH + bufRRow;
                                                        }
                                                        else
                                                        {
                                                            for (int qty = 0; qty < oQty; qty++)
                                                            { bufMRow += bufRow; }
                                                            buf = bufMH + bufMRow;
                                                        }
                                                        //REPLACED THIS LOGIC 050819

                                                        oCnt += 1;
                                                        StaticHelper._MainForm.Progress($"Data preparation {oCnt} out of {iMax}", oCnt, iMax);

                                                        //if (cb_UPC.Checked == true || cb_IB.Checked == true)
                                                        if (cb_UPC.Checked == true)
                                                        {
                                                            sTempPath += "\\UPC.btw";
                                                            //if (cb_IB.Checked)
                                                            //{
                                                            //    if (dtCampDetails.Rows[0]["IBPath"].ToString() != "")
                                                            //    {
                                                            //        sTempPath = dtCampDetails.Rows[0]["IBPath"].ToString() + "\\INTERNAL_BARCODE.btw";
                                                            //    }
                                                            //}
                                                        }
                                                        else if (sFromDoc == "PO")
                                                        {
                                                            sTempPath += "\\INTERNAL_BARCODE.btw";
                                                        }
                                                        //else if (sFromDoc == "SO - Pick List")
                                                        else if (dtCampDetails.Rows[0]["DocType"].ToString() == "Sample")
                                                        {
                                                            sTempPath += "\\Regular_S.btw";
                                                        }
                                                        else
                                                        {
                                                            if (ItemType == "R")
                                                            {
                                                                sTempPath += CheckFiles(sTempPath) ? "\\Regular_I.btw" : "\\Regular.btw";
                                                            }
                                                            else
                                                            {
                                                                sTempPath += CheckFiles(sTempPath) ? "\\Markdown_I.btw" : "\\Markdown.btw";
                                                            }
                                                        }

                                                        File.WriteAllText(tmpFile, buf);
                                                        //oPath += gvPrinter.Rows[iPrintcount].Cells[1].Value.ToString() == "Regular" ? "\\MARKDOWN1.btw" : "\\REGULAR1.btw";
                                                        btFormat = btEngine.Documents.Open(sTempPath); //Open Layout  
                                                    reset:
                                                        if (rowPcnt > gvPrinter.Rows.Count - 1) { rowPcnt = 0; goto reset; }
                                                        else if (ItemType == "R")
                                                        {
                                                            btFormat.PrintSetup.PrinterName = gvPrinter.Rows[rowPcnt].Cells[0].Value.ToString();
                                                        }
                                                        else if (ItemType == "M")
                                                        {

                                                            btFormat.PrintSetup.PrinterName = gvPrinter.Rows[rowPcnt].Cells[0].Value.ToString();
                                                        }

                                                        File.WriteAllText(tmpFile, buf);
                                                        //MessageBox.Show(tmpFile + oPath, strUPC);
                                                        dbbackup();

                                                        if (cb_IB.Checked)
                                                        {
                                                            btFormat.PageSetup.Orientation = Seagull.BarTender.Print.Orientation.Portrait180;
                                                        }

                                                        Messages messages;
                                                        int waitForCompletionTimeout = 10000; // 10 seconds
                                                        Result result = btFormat.Print(appName, waitForCompletionTimeout, out messages);
                                                        string messageString = "\n\nMessages:";

                                                        foreach (Seagull.BarTender.Print.Message message in messages)
                                                        {
                                                            messageString += "\n\n" + message.Text + "\n\n" + sTempPath;
                                                        }

                                                        if (result == Result.Failure)
                                                        {
                                                            if (messageString.Contains("BarTender successfully sent the print job to the spooler."))
                                                            {
                                                                StaticHelper._MainForm.ShowMessage("Print Failed" + messageString + tmpFile);
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show(this, "Print Failed" + messageString + tmpFile, appName);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //MessageBox.Show(this, "Label was successfully sent to printer." + messageString, appName);
                                                        }

                                                        //var xxx = btFormat.Print();


                                                        //MessageBox.Show(dtCampDetails.Rows[0]["OITM_Department"].ToString() +
                                                        //    dtCampDetails.Rows[0]["OITM_SubDepartment"].ToString() +
                                                        //    dtCampDetails.Rows[0]["OITM_BrandName"].ToString() +
                                                        //    strPriceFormatZero
                                                        //;

                                                        StaticHelper._MainForm.Progress($"Printing template {Path.GetFileName(sTempPath)}", 1, 1);
                                                        Application.DoEvents();

                                                        rowPcnt++;
                                                    }
                                                    else
                                                    {
                                                        StaticHelper._MainForm.ShowMessage($"Unable to print {iCntQty} price tag(s). Please check path in maintenance.", true);
                                                    }
                                                }
                                                //MessageBox.Show(tmpFile + oPath);
                                                //MessageBox.Show(gvPrinter.Rows.Count.ToString());

                                                iCntSuccess++;
                                            }
                                        }
                                        else
                                        {
                                            iCntChkDocDueDate++;
                                        }
                                    }
                                    else
                                    {
                                        iCntQty++;
                                    }


                                    iPrintcount++;
                                }

                            }
                        }

                        StaticHelper._MainForm.ProgressClear();
                        UnTickAll();

                        if (iCntQty > 0)
                        {
                            StaticHelper._MainForm.ShowMessage($"Unable to print {iCntQty} price tag(s). Please check Quantity.", true);
                        }
                        else if (iCntChkDocDueDate > 0)
                        {
                            StaticHelper._MainForm.ShowMessage($"Unable to print {iCntChkDocDueDate} price tag(s) is/are not within Campaign's start date and end date.", true);
                        }
                        else if (iCntSuccess > 0)
                        {
                            StaticHelper._MainForm.ShowMessage($"{iCntSuccess} price tag(s) printed successfully.");
                        }
                        else if (iCntSuccess == 0)
                        {
                            StaticHelper._MainForm.ShowMessage($"No price tag(s) printed. Please check items if its existing in SAP - Campaign.", true);
                        }
                    }
                    else { StaticHelper._MainForm.ShowMessage("Kindly select a printer", true); }
                }
                else { StaticHelper._MainForm.ShowMessage("Kindly choose a document to print", true); }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter($@"D:\DBSI Files\EASY SAP\0INSTALLER\ErrorLogs\Log.txt"))
                {
                    writer.Write(test);
                }
                if (!ex.Message.Contains("BarTender successfully sent the print job to the spooler"))
                {
                    MessageBox.Show(ex.Message + " " + test);
                    StaticHelper._MainForm.ShowMessage(ex.Message, true);
                }
            }
        }

        private static bool CheckFiles(string path)
        {
            bool bNotInverted = false;

            try
            {
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.btw");
                foreach (FileInfo file in Files)
                {
                    string fileName = file.Name;
                    if (fileName.Contains("_I"))
                    {
                        bNotInverted = true;
                    }
                }
                return bNotInverted;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true); return bNotInverted;
            }
        }

        private bool CheckIfConnected(string path)
        {
            var isConnected = false;

            try
            {
                var sys = new SystemSettings();
                isConnected = sys.FolderExist(path);
            }
            catch
            {
            }
            return isConnected;
        }

        private void dbbackup()
        {
            Seagull.BarTender.Print.Database.TextFile tf = new Seagull.BarTender.Print.Database.TextFile(btFormat.DatabaseConnections[0].Name);
            //((TextFile)btFormat.DatabaseConnections["TextFileDB"]).FileName = tmpFile;
            tf.FileName = tmpFile;
            btFormat.DatabaseConnections.SetDatabaseConnection(tf);
            btFormat.PrintSetup.ReloadTextDatabaseFields = true;
            string backup = @"C:\Templates\Backup\" + path + ".jpg";
            btFormat.ExportImageToFile(backup, ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, new Resolution(300), OverwriteOptions.DoNotOverwrite);

        }

        private void frmBarcodePrinting_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }
        private void SearchDoc_TextChanged(object sender, EventArgs e)
        {
            if (gvBarCode.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in gvBarCode.Rows)
                {
                    if (row.Cells[defaultColumn].Value.ToString().ToUpper().Contains(SearchDoc.Text.ToUpper()))
                    {
                        row.Selected = true;
                        _rowIndex = row.Index;
                        gvBarCode.FirstDisplayedScrollingRowIndex = _rowIndex;
                        break;
                    }
                    else
                    { row.Selected = false; }
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> list = gvBarCode.Rows.Cast<DataGridViewRow>().Where(k => Convert.ToBoolean(k.Cells[0].Value) == true).ToList();
            int iCount = list.Count();

            if (btnCancel.Text == "&Cancel")
            { Close(); }
            else
            { iCanceled = true; }
        }

        private void IsPrinted_Click(object sender, EventArgs e)
        {
            //int iRow = gvBarCode.SelectedRows[0].Index, i = Convert.ToInt32(gvBarCode.Rows[iRow].Cells["DocEntry"].Value);
            //if (SAPAccess.uPrintedStatus(gvBarCode.Rows[iRow].Cells["TableLine"].Value.ToString(), i, "Y", StaticHelper._MainForm) == true)
            //{ gvSetup(gvBarCode); }
        }
            
        private void gvBarCode_DoubleClick(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["frmItems"];
            if (fc == null)
            {
                int i = gvBarCode.CurrentCell.RowIndex;
                PublicHelper.oDocEntry = int.Parse(gvBarCode.Rows[i].Cells["Pick Number"].Value.ToString());
                BPCode = gvBarCode.Rows[i].Cells["CardCode"].Value.ToString();
                DocEntry2 = gvBarCode.Rows[i].Cells["DocEntry"].Value.ToString();
                UPCcheck = cb_UPC.Checked;
                frmItems frmItems = new frmItems(this);
                frmItems.MdiParent =StaticHelper._MainForm;
                frmItems.Tag = gvBarCode.Rows[i].Cells["TableLine"].Value.ToString();
                frmItems.Show();
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Item selection list already open.");
            }

        }
        private void gvBarCode_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (gvBarCode.Columns.Count > 1 && e.ColumnIndex == 0)
            {
                DataGridViewRow gvr = gvBarCode.Rows[e.RowIndex];
                if (Convert.ToBoolean(gvr.Cells[0].Value) == true)
                { gvTick(gvr, e.ColumnIndex, e.RowIndex); }
                else
                { gvUntick(gvr, e.RowIndex); }
            }
            gvBarCode.EndEdit();
        }
        void gvTick(DataGridViewRow gvr, int iColumn, int iRow)
        {
            try
            {
                string TableID = gvr.Cells["TableLine"].Value.ToString();
                int oDocEntry = Convert.ToInt32(gvr.Cells["DocEntry"].Value);
                //v1.24 7617
                //LinqAccess.frmBarcodeItem.RemoveAll(x => x.DocEntry == oDocEntry);
                DataTable dt = new DataTable();
                dt = sapHana.Get($"SELECT B.ItemCode, A.RelQtty FROM PKL1 A INNER JOIN {TableID} B ON A.OrderEntry = B.DocEntry AND A.OrderLine = B.LineNum WHERE A.AbsEntry = {oDocEntry}");
                foreach (DataRow dr in dt.Rows)
                {
                    var LinqLines = LinqAccess.frmBarcodeItem.Where(x => x.DocEntry == oDocEntry && x.ItemCode == dr["ItemCode"].ToString() && x.TableID == TableID);
                    if (LinqLines.Count() == 0)
                    {
                        LinqAccess.frmBarcodeItem.Add(new LinqAccess.gvBarcodeItem
                        {
                            IsTick = true,
                            TableID = TableID,
                            DocEntry = oDocEntry,
                            ItemCode = dr["ItemCode"].ToString(),
                            Qty = dr["RelQtty"].ToString()
                        });
                    }
                    else
                    {
                        foreach (var x in LinqLines.Where(x => x.IsTick == false))
                        { x.IsTick = true; }
                    }
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message); }
        }
        void gvUntick(DataGridViewRow gvr, int iRow)
        {
            try
            {
                string TableID = gvr.Cells["TableLine"].Value.ToString();
                int oDocEntry = Convert.ToInt32(gvr.Cells["DocEntry"].Value);
                DataTable dt = new DataTable();
                dt = sapHana.Get($"SELECT B.ItemCode FROM PKL1 A INNER JOIN {TableID} B ON A.OrderEntry = B.DocEntry AND A.OrderLine = B.LineNum WHERE A.AbsEntry = {oDocEntry}");
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (var x in LinqAccess.frmBarcodeItem.Where(x => x.DocEntry == oDocEntry && x.ItemCode == dr["ItemCode"].ToString() && x.TableID == TableID))
                    { x.IsTick = false; }
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message); }
        }
        private void frmBarcodeAll_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseForm(e);
        }

        void CloseForm(FormClosingEventArgs e)
        {
            if (MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close this document?", LibraryHelper.AssemblyInfo.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Form frm = Application.OpenForms["frmItems"];
                if (frm != null)
                { frm.Close(); }
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void frmBarcodeAll_Load(object sender, EventArgs e)
        {
            string tempPath = Path.GetTempPath();
            string newFolder;
            do
            {
                newFolder = Path.GetRandomFileName();
                previewPath = tempPath + newFolder;
            } while (Directory.Exists(previewPath));
            Directory.CreateDirectory(previewPath);
            WindowState = FormWindowState.Maximized;
        }
        private void gvprinterSetup(DataGridView dtt)
        {
            try
            {
                dtt.Rows.Clear();
                dtt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dtt.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dtt.MultiSelect = false;
                dtt.RowTemplate.Resizable = DataGridViewTriState.False;
                dtt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dtt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dtt.RowHeadersVisible = false;
                dtt.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dtt.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);
                //=========================Headers==============================================         
                dtt.ColumnCount = 2;
                dtt.Columns[0].Name = "Printer";
                dtt.Columns[0].Width = 250;
                //dtt.Columns[1].Name = "Type";
                //dtt.Columns[1].Width = 120;
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message); }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //cbPrinter.Text == "" && cbType.Text == "" || cbPrinter.Text == ""
            if (cbPrinter.Text == "")
            {
                StaticHelper._MainForm.ShowMessage("Please select a printer");
            }
            else
            {
                gvPrinter.Rows.Add(cbPrinter.Text);
                DataGridViewButtonCell remove1 = new DataGridViewButtonCell();
                remove1.Value = "Remove";
                gvPrinter.Rows[gvPrinter.Rows.Count - 1].Cells[1] = remove1;
                cbPrinter.Text = null;
                cbType.Text = null;
                //validatetype();
            }
        }
        private void gvPrinter_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int rowindex = gvPrinter.CurrentCell.RowIndex;
                gvPrinter.Rows.RemoveAt(rowindex);
            }
        }
        private void validatetype()
        {
            try
            {
                if (gvPrinter.Rows.Count - 1 != 0)
                {
                    foreach (DataGridViewRow row in gvPrinter.Rows)
                    {
                        if (gvPrinter.Rows[gvPrinter.Rows.Count - 1] != row)
                        {
                            if (gvPrinter.Rows[gvPrinter.Rows.Count - 1].Cells[0].Value.ToString() == row.Cells[0].Value.ToString() && gvPrinter.Rows[gvPrinter.Rows.Count - 1].Cells[1].Value.ToString() == row.Cells[1].Value.ToString())
                            {
                                btnCommand.Focus();
                                gvPrinter.Rows.RemoveAt(gvPrinter.Rows.Count - 1);
                                StaticHelper._MainForm.ShowMessage("Please select a different type");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message);
            }
        }

        private void gvBarCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Space && gvBarCode.Focused == true)
            {
                if (Convert.ToBoolean(gvBarCode.SelectedRows[0].Cells[0].Value) == false)
                {
                    gvBarCode.SelectedRows[0].Cells[0].Value = true;
                }
                else
                {
                    gvBarCode.SelectedRows[0].Cells[0].Value = false;
                }
            }
        }

        private void gvBarCode_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        private void UnTickAll()
        {
            try
            {
                foreach (DataGridViewRow row in gvBarCode.Rows)
                {
                    row.Cells[0].Value = false;
                }

                LinqAccess.frmBarcodeItem.Clear();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message);
            }
        }

        private void cb_UPC_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_UPC.Checked == true)
            {
                UPCandIBcheck("UPC");
            }
        }

        private void cb_IB_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_IB.Checked == true)
            {
                UPCandIBcheck("IB");
            }
        }

        private void UPCandIBcheck(string chkbox)
        {
            CheckBox chkbox1 = new CheckBox();
            CheckBox chkbox2 = new CheckBox();
            chkbox1 = chkbox == "UPC" ? cb_UPC : cb_IB;
            chkbox2 = chkbox == "IB" ? cb_UPC : cb_IB;
            chkbox1.Checked = true;
            chkbox2.Checked = false;
        }
    }
}
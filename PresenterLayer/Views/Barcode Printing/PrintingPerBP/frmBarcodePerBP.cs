using Context;
using MetroFramework;
using MetroFramework.Forms;
using Seagull.BarTender.Print;
using Seagull.BarTender.Print.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using zDeclare;
using PresenterLayer.Helper;
using DirecLayer;
using DomainLayer.Models.Inventory;

using PresenterLayer.Services;
using static System.Net.Mime.MediaTypeNames;

namespace PresenterLayer.Views
{
    public partial class frmBarcodePerBP : MetroForm
    {

        #region Bartender
        Engine btEngine = null; // The BarTender Print Engine
        LabelFormatDocument btFormat = null; // The currently open Format
        bool isClosing = false; // Set to true if we are closing. This helps discontinue thumbnail loading.
        string BarcodeFile = System.Windows.Forms.Application.StartupPath + @"\Bartender\Layout\WBII.btw";
        string thumbnailFile = "";
        //DataAccess da = new DataAccess();
        #endregion

        private SAPHanaAccess sapHana { get; set; }

        bool iCanceled;
        int iRow = 0;
        string iColumn;
        string tmpFile = Path.GetTempFileName();
        private const string appName = "Label Print";
        string path = "";
        private static int defaultColumn = 1, _rowIndex = 0;
        private static string oCode, oName;

        int oCnt { get; set; } = 0;
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Dispose(); }
            else if ((txtBpCode.Focused == true || txtBpName.Focused == true) && keyData == Keys.Tab && txtBpCode.Text == "")
            { DataSearch(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void gvSetup(DataGridView dgv, string sTable, string sDocEntry)
        {
            try
            {
                dgv.Rows.Clear();
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.MultiSelect = false;
                dgv.RowTemplate.Resizable = DataGridViewTriState.False;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.RowHeadersVisible = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dgv.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                //===============================HEADER WITH TICKBOX=========================
                DataGridViewCheckBoxColumn col1 = new DataGridViewCheckBoxColumn();
                var checkheader = new CheckBoxHeaderCell();
                checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
                col1.HeaderCell = checkheader;
                dgv.Columns.Add(col1);
                //===========================================================================

                dgv.ColumnCount = 11;
                dgv.Columns[1].Name = "LINENUM";
                dgv.Columns[2].Name = "BRAND";
                dgv.Columns[3].Name = "SRP TYPE";
                dgv.Columns[4].Name = "ITEM NO";
                dgv.Columns[5].Name = "ITEM LONG DESCRIPTION";
                dgv.Columns[6].Name = "UOM";
                dgv.Columns[7].Name = "QUANTITY";
                dgv.Columns[8].Name = "COLOR";
                dgv.Columns[9].Name = "SIZE";
                dgv.Columns[10].Name = "SRP";

                DataTable dtItems = new DataTable();
                var helper = new DataHelper();

                var GetBarcodeLineItems = sapHana.Get(SP.BP_BarcodeAll_BarcodeLineItemsSOITR);
                string BarcodeLineItems = helper.ReadDataRow(GetBarcodeLineItems, 1, "", 0);
                string selqry = string.Format(BarcodeLineItems, sDocEntry, sTable, sTable.Remove(0,1) + "1");



                dtItems = GetData(selqry);

                if (dtItems.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtItems.Rows)
                    {
                        dgv.Rows.Add(false, dr["LineNum"].ToString(), dr["U_Brand"].ToString(), dr["SRPType"].ToString()
                                    , dr["ItemCode"].ToString(), dr["Dscription"].ToString(), dr["UOM"].ToString(), dr["Quantity"].ToString()
                                    , dr["Color"].ToString(), dr["U_Size"].ToString(), Convert.ToDouble(dr["Price"]).ToString("#,##0.00"));
                    }

                    dgv.Columns["LINENUM"].Visible = false;
                    dgv.Columns["BRAND"].ReadOnly = true;
                    dgv.Columns["SRP TYPE"].ReadOnly = true;
                    dgv.Columns["ITEM NO"].ReadOnly = true;
                    dgv.Columns["ITEM LONG DESCRIPTION"].ReadOnly = true;
                    dgv.Columns["UOM"].ReadOnly = true;
                    dgv.Columns["QUANTITY"].ReadOnly = true;
                    dgv.Columns["COLOR"].ReadOnly = true;
                    dgv.Columns["SIZE"].ReadOnly = true;
                    dgv.Columns["SRP"].ReadOnly = true;


                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        row.HeaderCell.Value = String.Format("{0}", row.Index + 1);

                        if (row.Cells[2].Value.ToString() == "Markdown")
                        {
                            row.Cells[2].Style.ForeColor = Color.Red;
                        }

                        if (row.Cells[9].Value.ToString() == "0.000000")
                        {
                            row.Cells[9].Style.ForeColor = Color.Red;
                        }
                    }
                    //DataGridViewTextBoxColumn tb = new DataGridViewTextBoxColumn();
                    //tb.HeaderText = "Reg Copy";
                    //tb.Name = "tb";
                    //DataGridViewTextBoxColumn tb2 = new DataGridViewTextBoxColumn();
                    //tb2.HeaderText = "MD Copy";
                    //tb2.Name = "tb2";
                    //gvBarCode.Columns.Add(tb);
                    //gvBarCode.Columns.Add(tb2);
                    //dgv.Columns[5].Visible = false;
                    //dgv.Columns[6].Visible = false;
                    //DefValue();
                }

            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message); }
        }

        public frmBarcodePerBP()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Printers();
            gvprinterSetup(gvPrinter);
            sapHana = new SAPHanaAccess();
            // DataAccessSQL.Execute(DataAccess.conStr("SQL"), $"sp_BarcodePrinting 'D',1,'{GetLogonSid.getLogonSid(frmMain)}','{Config.oLoginUserID}'", frmMain);
            AutoComplete("Code");
            AutoComplete("Name");
        }
        public void AutoComplete([Optional]string AutoText)
        {
            AutoCompleteStringCollection AutoItem = new AutoCompleteStringCollection();
            DataTable dt = new DataTable();

            dt = sapHana.Get("SELECT DISTINCT B.BpCode [Code], B.BpName [Name] FROM OCPN A INNER JOIN CPN1 B ON A.CpnNo = B.CpnNo WHERE A.Status = 'O'");

            foreach (DataRow rw in dt.Rows)
            { AutoItem.Add(rw[AutoText].ToString()); }
            switch (AutoText)
            {
                case "Code":
                    txtBpCode.AutoCompleteMode = AutoCompleteMode.Suggest;
                    txtBpCode.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtBpCode.AutoCompleteCustomSource = AutoItem;
                    break;
                case "Name":
                    txtBpName.AutoCompleteMode = AutoCompleteMode.Suggest;
                    txtBpName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtBpName.AutoCompleteCustomSource = AutoItem;
                    break;
            }
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

                Printers printers = new Printers();

                foreach (Printer printer in printers)
                {
                    cbPrinter.Items.Add(printer.PrinterName);
                }

                thumbnailFile = Path.GetTempFileName();
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message); }
        }

        private void frmBarcodePerPB_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            if (gvBarCode.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in gvBarCode.Rows)
                {
                    if (row.Cells[defaultColumn].Value.ToString().ToUpper().Contains(txtSearchItem.Text.ToUpper()))
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

        void DataSearch()
        {
            DataTable dt = new DataTable();
            dt = sapHana.Get("SELECT DISTINCT B.BpCode [BP Code], B.BpName [BP Name] FROM OCPN A INNER JOIN CPN1 B ON A.CpnNo = B.CpnNo WHERE A.Status = 'O' order by B.BpCode");
            frmSearch frmSearch = new frmSearch(StaticHelper._MainForm);
            frmSearch.MdiParent = StaticHelper._MainForm;
            frmSearch.Tag = "BarCode";
            frmSearch.Text = "BP Code";
            frmSearch.SearchData(dt);
            frmSearch.Show();
        }
        private void DefValue()
        {
            foreach (DataGridViewRow row in gvBarCode.Rows)
            {
                row.Cells["tb"].Value = "0";
                row.Cells["tb2"].Value = "0";
            }

        }

        private void pbStyleCode_Click(object sender, EventArgs e)
        {
            DataSearch();
        }

        private void pbStyleName_Click(object sender, EventArgs e)
        { DataSearch(); }
        private void btnCommand_Click(object sender, EventArgs e)
        {
            try
            {
                List<DataGridViewRow> list = gvBarCode.Rows.Cast<DataGridViewRow>().Where(k => Convert.ToBoolean(k.Cells[0].Value) == true).ToList();
                if (list.Count() > 0)
                {
                    if (gvPrinter.Rows.Count != 0)
                    {
                        string oPath = "";
                        oCnt = 0;
                        int iMax = 0;
                        for (int i = 0; i <= gvBarCode.Rows.Count - 1; i++) { iMax = gvBarCode.Rows[i].Cells[0].Value is true ? iMax += 1 : iMax += 0; }
                        int iPrintcount = 0;
                        string bufRRow = "";
                        string bufMRow = "";
                        bool noMaintenance = false;

                        int iCntChkDocDueDate = 0;
                        int iCntSuccess = 0;

                        var helper = new DataHelper();

                        //=========================CREATE DATA TEMPLATE============================START

                        string buf = BarcodeCtrl.GetBarcodeHeader();
                        string bufRH = buf, bufMH = buf;

                        //=========================CREATE DATA TEMPLATE============================END

                        foreach (DataGridViewRow row in gvBarCode.Rows)
                        {
                            if (row.Cells[0].Value is true)
                            {
                                string letters = "ABCDEFGHIJKL";
                                string oBPCode = txtBpCode.Text;
                                DataTable oTableCheck = sapHana.Get($"SELECT A.CardCode FROM ORDR A WHERE A.CardCode = '{oBPCode}'");
                                string oTable = cbB_DocType.Text.ToString() == "Sales Order" ? "ORDR" : cbB_DocType.Text.ToString() == "Purchase Order" ? "OPOR" : cbB_DocType.Text.ToString() == "Inventory Transfer" ? "OWTR" : "OWTQ";
                                string oTableline = oTable.Remove(0, 1) + "1";
                                string oCpnNo = row.Cells[1].Value.ToString();
                                string oItemCode = row.Cells[4].Value.ToString();
                                iPrintcount = iPrintcount > gvPrinter.Rows.Count - 1 ? iPrintcount = 0 : iPrintcount;
                                string sCheckDate = "N";

                                string oDocEntry = txtDocEntry.Text;

                                var GetCheckDocDueDateQry = sapHana.Get(SP.BP_BarcodeAll_CheckDocDueDate);
                                var GetItemDetailsQry = sapHana.Get(SP.BP_BarcodeAll);

                                string sCheckDocDueDateQry = string.Format(helper.ReadDataRow(GetCheckDocDueDateQry, 1, "", 0), oBPCode, oTableline, oItemCode, oTable, oDocEntry);
                                string sGetItemDetailsQry = string.Format(helper.ReadDataRow(GetItemDetailsQry, 1, "", 0), oBPCode, oTableline, oItemCode, oTable, oDocEntry);
                                string noMaintenance_path = "";

                                noMaintenance = BarcodeCtrl.CheckNoMaintenance(oBPCode);
                                if (noMaintenance)
                                {
                                    var GetCheckDocDueDateNM = sapHana.Get(SP.BP_BarcodeAll_CheckDocDueDateNM);
                                    var GetBarcodeFromOITM = sapHana.Get(SP.BP_BarcodeAll_BarcodeFromOITM);
                                    sCheckDocDueDateQry = string.Format(helper.ReadDataRow(GetCheckDocDueDateNM, 1, "", 0), oBPCode, oTableline, oItemCode, oTable, oDocEntry);
                                    sGetItemDetailsQry = string.Format(helper.ReadDataRow(GetBarcodeFromOITM, 1, "", 0), oBPCode, oTableline, oItemCode, oTable, oDocEntry);

                                    var GetPath = sapHana.Get(SP.BP_BarcodeAll_GetPath);
                                    var sCheckPathQry = string.Format(helper.ReadDataRow(GetPath, 1, "", 0), oBPCode);
                                    var dt_getPath = sapHana.Get(sCheckPathQry);
                                    noMaintenance_path = dt_getPath.Rows[0]["U_Path"].ToString();
                                }

                                if (sapHana.Get(sCheckDocDueDateQry).Rows.Count > 0)
                                {
                                    sCheckDate = sapHana.Get(sCheckDocDueDateQry).Rows[0]["WithinDateRange"].ToString();
                                }

                                if (sCheckDate == "Y")
                                {
                                    DataTable dtCampDetails = sapHana.Get(sGetItemDetailsQry);

                                    int rowPcnt = 0;

                                    if (dtCampDetails.Rows.Count > 0)
                                    {
                                        foreach (DataGridViewRow rowP in gvPrinter.Rows)
                                        {

                                            Double SRP = gvPrinter.Rows[rowPcnt].Cells[1].Value.ToString() == "Regular" ? SRP = Convert.ToDouble(dtCampDetails.Rows[0]["RegularPrice"].ToString()) : SRP = Convert.ToDouble(dtCampDetails.Rows[0]["MarkDownPrice"].ToString());
                                            path = $"{dtCampDetails.Rows[0]["CampaignName"].ToString()} Backup {DateTime.Now}".Replace("/", "-").Replace(":", "-");
                                            //oPath = dtCampDetails.Rows[0]["U_Path"].ToString();
                                            oPath = noMaintenance ? noMaintenance_path : dtCampDetails.Rows[0]["U_Path"].ToString();

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
                                            string ItemType = dtCampDetails.Rows[0]["OITM_ItemClass"].ToString().ToLower().Contains("discontinued") ? "M" : "R";//dtCampDetails.Rows[0]["CPN2_U_PriceCat"].ToString() == "Markdown" ? "M" : "R"; ///dtCampDetails.Rows[0]["RegularPrice"].ToString().Contains(".00") ? "M" : "R";
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
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_VisOrder"].ToString()}\"\r\n";

                                            //string sTempPath = oPath.Replace("\\NBFIHVMSVR01", "");
                                            //sTempPath = "D:" + sTempPath;

                                            string sTempPath = /*"C:\\Templates";//*/oPath;
                                            bufRRow = "";

                                            string sFromDoc = cbB_DocType.Text != "" ? cbB_DocType.Text.ToString() : "";

                                            oCnt += 1;
                                            StaticHelper._MainForm.Progress($"Data preparation {oCnt} out of {iMax}", oCnt, iMax);
                                            if (gvPrinter.Rows[iPrintcount].Cells[1].Value.ToString() == "Regular")
                                            {
                                                for (int qty = 0; qty < Convert.ToInt32(gvPrinter.Rows[iPrintcount].Cells[2].Value.ToString()); qty++)
                                                { bufRRow += bufRow; }
                                                buf = bufRH + bufRRow;
                                            }
                                            else
                                            {
                                                for (int qty = 0; qty < Convert.ToInt32(gvPrinter.Rows[iPrintcount].Cells[2].Value.ToString()); qty++)
                                                { bufMRow += bufRow; }
                                                buf = bufMH + bufMRow;
                                            }

                                            if (cb_UPC.Checked == true)
                                            {
                                                sTempPath += "\\UPC.btw";
                                            }
                                            else if (sFromDoc == "Purchase Order")
                                            {
                                                sTempPath += "\\INTERNAL_BARCODE.btw";
                                            }
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

                                            if (cb_IB.Checked)
                                            {
                                                btFormat.PageSetup.Orientation = Seagull.BarTender.Print.Orientation.Portrait180;
                                            }

                                            if (gvPrinter.Rows[iPrintcount].Cells[1].Value.ToString() == "Regular")
                                            {
                                                btFormat = btEngine.Documents.Open(sTempPath);
                                                btFormat.PrintSetup.PrinterName = gvPrinter.Rows[iPrintcount].Cells[0].Value.ToString();
                                                File.WriteAllText(tmpFile, buf);
                                                dbbackup();

                                                //btFormat.Print();

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
                                                    MessageBox.Show(this, "Print Failed" + messageString, appName);
                                                }
                                                else
                                                {
                                                    //MessageBox.Show(this, "Label was successfully sent to printer." + messageString, appName);
                                                    //MessageBox.Show(this, "Label was successfully sent to printer." + tmpFile, appName);
                                                }

                                                iPrintcount++;
                                                bufRRow = "";
                                                File.Delete(tmpFile);
                                                StaticHelper._MainForm.Progress($"Printing template {Path.GetFileName(sTempPath)}", 1, 1);
                                            }
                                            else if (gvPrinter.Rows[iPrintcount].Cells[1].Value.ToString() == "Markdown")
                                            {
                                                btFormat = btEngine.Documents.Open(sTempPath);
                                                btFormat.PrintSetup.PrinterName = gvPrinter.Rows[iPrintcount].Cells[0].Value.ToString();
                                                File.WriteAllText(tmpFile, buf);
                                                dbbackup();

                                                //btFormat.Print();

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
                                                    MessageBox.Show(this, "Print Failed" + messageString, appName);
                                                }
                                                else
                                                {
                                                    //MessageBox.Show(this, "Label was successfully sent to printer." + messageString, appName);
                                                }


                                                iPrintcount++;
                                                bufRRow = "";
                                                File.Delete(tmpFile);
                                                StaticHelper._MainForm.Progress($"Printing template {Path.GetFileName(sTempPath)}", 1, 1);
                                            }
                                        }
                                        iCntSuccess++;
                                    }
                                }// CheckDate
                                else
                                {
                                    iCntChkDocDueDate++;
                                }
                            }
                        }

                        StaticHelper._MainForm.ProgressClear();
                        System.Windows.Forms.Application.DoEvents();
                        UnTickAll();

                        if (iCntChkDocDueDate > 0)
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
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Kindly select a printer", true);
                    }
                }
                else { StaticHelper._MainForm.ShowMessage("Kindly choose a document to print", true); }
            }
            catch (Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                //string strMsg = ex.Message == "There is no row at position 0" ? "Sales " : "";
                string methodName = frame.GetMethod().Name;
                //StaticHelper._MainForm.ShowMessage($"({methodName}) in line" + line + ex.Message, true);
                //MessageBox.Show($"({methodName}) in line" + line + ex.Message);

                if (!ex.Message.Contains("BarTender successfully sent the print job to the spooler"))
                {
                    MessageBox.Show($"({methodName}) in line" + line + ex.Message);
                    StaticHelper._MainForm.ShowMessage(ex.Message, true);
                }
            }
        }

        private void dbbackup()
        {
            Seagull.BarTender.Print.Database.TextFile tf = new Seagull.BarTender.Print.Database.TextFile(btFormat.DatabaseConnections[0].Name);
            tf.FileName = tmpFile;
            btFormat.DatabaseConnections.SetDatabaseConnection(tf);
            btFormat.PrintSetup.ReloadTextDatabaseFields = true;
            string backup = @"C:\Templates\Backup\" + path + ".jpg";
            btFormat.ExportImageToFile(backup, ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, new Resolution(300), OverwriteOptions.DoNotOverwrite);
        }


        private void frmBarcodePerPB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (txtBpCode.Text != "")
            {
                if (MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close this document?", LibraryHelper.AssemblyInfo.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) != DialogResult.Yes)
                { e.Cancel = true; }
                else { Dispose(); }
            }
        }

        void RefreshData()
        {
            foreach (DataGridViewRow dr in gvBarCode.Rows)
            {
                if (Convert.ToBoolean(dr.Cells[0].Value) == true)
                { dr.Cells[0].Value = false; }
            }
            iCanceled = true;
            // DataAccessSQL.Execute(DataAccess.conStr("SQL"), $"sp_BarcodePrinting 'D',1,'{GetLogonSid.getLogonSid(frmMain)}','{Config.oLoginUserID}'", frmMain);
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
        private void frmBarcodePerPB_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }
        void ChangeCurrentCell(int rowIndex, int ColNum)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                gvBarCode.CurrentCell = gvBarCode.Rows[rowIndex].Cells[ColNum];
            }));
        }
        private void gvBarCode_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //v1.24 7517
            double result;
            if (e.ColumnIndex == 5 || e.ColumnIndex == 6)
            {
                if (gvBarCode.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    bool isnUmeric = double.TryParse(gvBarCode.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out result);

                    if (isnUmeric == false)
                    {
                        ChangeCurrentCell(e.RowIndex, e.ColumnIndex);
                        gvBarCode.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                        StaticHelper._MainForm.ShowMessage("Please input valid numeric value", true);
                    }
                    else
                    {
                        //CHECKED
                        if (gvBarCode.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "0")
                        {
                            gvBarCode.Rows[e.RowIndex].Cells[0].Value = true;
                        }
                        //UNCHECKED
                        if (gvBarCode.Rows[e.RowIndex].Cells[5].Value.ToString() == "0" && gvBarCode.Rows[e.RowIndex].Cells[6].Value.ToString() == "0")
                        {
                            gvBarCode.Rows[e.RowIndex].Cells[0].Value = false;
                        }
                    }
                }
            }
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
                dtt.ColumnCount = 4;
                dtt.Columns[0].Name = "Printer";
                dtt.Columns[0].Width = 200;
                dtt.Columns[1].Name = "Type";
                dtt.Columns[1].Width = 90;
                dtt.Columns[2].Name = "Qty";
                dtt.Columns[2].Width = 60;
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        private void gvPrinter_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                int rowindex = gvPrinter.CurrentCell.RowIndex;
                gvPrinter.Rows.RemoveAt(rowindex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbPrinter.Text == "" && cbType.Text == "" || cbPrinter.Text == "")
            {
                StaticHelper._MainForm.ShowMessage("Please select a printer", true);
            }
            else if (cbType.Text == "")
            {
                StaticHelper._MainForm.ShowMessage("Please select a type", true);
            }
            else
            {
                gvPrinter.Rows.Add(cbPrinter.Text, cbType.Text, numQty.Value);
                DataGridViewButtonCell remove1 = new DataGridViewButtonCell();
                remove1.Value = "Remove";
                gvPrinter.Rows[gvPrinter.Rows.Count - 1].Cells[3] = remove1;
                cbPrinter.Text = null;
                cbType.Text = null;
                numQty.Value = 1;
                validatetype();
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
                                StaticHelper._MainForm.ShowMessage("Please select a different type", true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private void pB_GetDocNum_Click(object sender, EventArgs e)
        {
            if (cbB_DocType.Text.ToString() != "")
            {
                string sTable = cbB_DocType.Text.ToString() == "Sales Order" ? "ORDR" : cbB_DocType.Text.ToString() == "Purchase Order" ? "OPOR" : cbB_DocType.Text.ToString() == "Inventory Transfer" ? "OWTR" : "OWTQ";

                ViewList("SapHeaderTable", out oCode, out oName, "List of " + cbB_DocType.Text.ToString(), sTable);

                txtDocNum.Text = oName;
                txtDocEntry.Text = oCode;

                if (!string.IsNullOrEmpty(oCode))
                {
                    GetDocDetails(oCode, sTable);
                }


            }
        }

        private void ViewList(string SearchTable
                , out string Code
                , out string Name
                , string title
                , [Optional] string Param1
                , [Optional] string Param2
                , [Optional] string Param3
                , [Optional] string Param4
                )
        {
            frmSearch2 fS = new frmSearch2();
            fS.oSearchMode = SearchTable;
            //Set Parameter 1
            frmSearch2.Param1 = Param1;
            //Set Parameter 2
            frmSearch2.Param2 = Param2;
            //Set Parameter 3
            frmSearch2.Param3 = Param3;
            //Set Parameter 4
            frmSearch2.Param3 = Param4;
            //Set Title
            frmSearch2._title = title;
            fS.oFormTitle = title;
            fS.ShowDialog();

            Code = fS.oCode;
            Name = fS.oName;
        }

        private void gvBarCode_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        void GetDocDetails(string DocEntry, string Table)
        {
            DataTable header = new DataTable();
            DataTable Lines = new DataTable();

            header = GetDocHeader(DocEntry, Table);

            txtBpCode.Text = ConvertToString(header.Rows[0]["CardCode"]);
            txtBpName.Text = ConvertToString(header.Rows[0]["CardName"]);

            gvSetup(gvBarCode, Table, DocEntry);
        }

        private DataTable GetDocHeader(string sDocEntry, string sTable)
        {
            return GetData($"select DocEntry, DocNum, CardCode, CardName from {sTable} where DocEntry = '{sDocEntry}'");
        }

        public static DataTable GetData(string query)
        {
            var sapHana = new SAPHanaAccess();
            var queryReturn = sapHana.Get(query);

            return queryReturn;
        }

        private string ConvertToString(object value)
        {
            return value == null ? "" : value.ToString();
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
    }
}

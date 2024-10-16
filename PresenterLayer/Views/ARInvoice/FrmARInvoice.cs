using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Runtime.InteropServices;
using DirecLayer;
using PresenterLayer.Views.Main;
using PresenterLayer.Helper;
using PresenterLayer.Helper.SalesOrder;
using Context;
using DirecLayer._02_Form.MVP.Presenters.PriceTag;
using zDeclare;
using PresenterLayer.Services;
using MetroFramework;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using DomainLayer.Helper;
using PresenterLayer.Services.Security;

namespace PresenterLayer.Views
{
    public partial class FrmARInvoice : MetroForm, IFrmARInvoice
    {
        public MainForm frmMain { get; set; }
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        SalesAR_generics _generics = new SalesAR_generics();
        SalesRepository _repository = new SalesRepository();
        private SOmaintenance SOm = new SOmaintenance();
        public static bool FindMode = false;
        int max_width = Screen.PrimaryScreen.Bounds.Width - 320;
        int max_height = Screen.PrimaryScreen.Bounds.Height - 250;
        private static string oSeries, oSalesEmployee, oProject, oCode, oName, oAddressCode, oFindStat;
        public static string oPriceList;
        public static string oTaxGroup;
        public static string oTaxRate;
        public static int datetimeCount = 0;
        public static int xdocentry;
        public static string objType = "OINV";
        public bool fromAR = false;
        public static string oFWhsCode, oTWhsCode;
        private static string DiscType;
        public static string oItmsGrpCod { get; set; }
        public static string oOutRight { get; set; }

        private static double oDiscount;
        private string Vat { get; set; }
        private double VatRate { get; set; }
        private string DefaultWarehouse { get; set; }
        private string table { get; set; }
        private string RawCurr { get; set; }
        private DataTable CurrentDataSet { get; set; }
        private int findDocSearch;
        public static string _DocEntry;

        private void FrmSalesOrder_Load(object sender, EventArgs e)
        {
            //FindMode = false;
            //MaximumSize = new Size(max_width, max_height);
            WindowState = FormWindowState.Maximized;
            NumSeries();
            LoadCompany();
            LoadDocumentType();

            dtCancelDate.CustomFormat = " ";
            dtCancelDate.Format = DateTimePickerFormat.Custom;

            var files = Presenter.GetDocumentCrystalForms();
            if (files != null)
            {
                foreach (FileInfo file in files)
                {
                    CmbPrintPreview.Items.Add(file.Name);
                }
            }
            

            cmbLogShipTo.SelectedIndex = cmbLogShipTo.FindString("Ship To");
            //if (fromAR == true)
            //{
            //    LoadSAPDAta(xdocentry);
            //    pictureBox2.Enabled = false;
            //    button1.Enabled = false;
            //    btnAdd.Enabled = false;
            //    dtDocDate.Enabled = false;
            //    dtDueDate.Enabled = false;
            //    dtPostingDate.Enabled = false;
            //    pictureBox2.Enabled = false;
            //    txtRemarks.Enabled = false;
            //    txtAddress.Enabled = false;
            //}
            DgvFindDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (DataGridViewRow row in DgvUdf.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals("Prepared By"))
                {
                    DgvUdf.Rows[row.Index].Cells["Field"].Value = DomainLayer.Models.EasySAPCredentialsModel.EmployeeCompleteName;
                    DgvUdf.Rows[row.Index].Cells["Field"].ReadOnly = true;
                    break;
                }
            }
            DgvUdf.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.Resizable = DataGridViewTriState.False);
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.D1:
                    TabSO.SelectedIndex = 0;
                    break;

                case Keys.Control | Keys.D2:
                    TabSO.SelectedIndex = 1;
                    break;

                case Keys.Control | Keys.D3:
                    TabSO.SelectedIndex = 2;
                    break;

                case Keys.Control | Keys.D4:
                    TabSO.SelectedIndex = 3;
                    break;

                case Keys.Control | Keys.D5:
                    TabSO.SelectedIndex = 4;
                    break;

                case Keys.Control | Keys.B:
                    Presenter.GetSupplierInformation();
                    break;

                case Keys.Control | Keys.M:
                    cbCompany.Focus();
                    break;

                case Keys.Control | Keys.D:
                    Presenter.GetDepartment();
                    break;

                case Keys.Control | Keys.E:
                    cbSeries.Focus();
                    break;

                case Keys.Control | Keys.S:
                    txtDocStatus.Focus();
                    break;
                case Keys.Escape:
                    Close();
                    break;
                case Keys.Control | Keys.I:
                    Presenter.DisplayItemList(cbDocumentType.Text);
                    break;

                //case Keys.Control | Keys.A:

                //    if (TxtVendorCode.Text != string.Empty)
                //    {
                //        if (Presenter.ExecuteRequest(BtnRequest.Text))
                //        {
                //            BtnRequest.Text = BtnRequest.Text == "Update" ? "Add" : "Add";
                //        }
                //    }
                //    else
                //    {
                //        PublicStatic.frmMain.NotiMsg("Warning: Enter BP Code", Color.Red);
                //    }

                //    break;

                case Keys.Control | Keys.N:
                    var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Presenter.ClearField(true);
                        TabSO.SelectedIndex = 0;
                        btnAdd.Text = "Add";
                    }
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public bool IsFindMode
        {
            get => FindMode;
            set => FindMode = value;
        }

        public Panel UdfPanel
        {
            get => panel1;
        }

        public string DocEntry
        {
            get => txtDocEntry2.Text;
            set => txtDocEntry2.Text = value;
        }
        public string RefNo
        {
            get => txtCustomRefNo.Text;
            set => txtCustomRefNo.Text = value;
        }

        public string DocNum
        {
            get => txtDocEntry.Text;
            set => txtDocEntry.Text = value;
        }

        public string Series
        {
            //get => Convert.ToString(cbSeries.SelectedText);
            //set => cbSeries.Text = value;
            get => oSeries;
            set => cbSeries.Text = oSeries;
        }

        public string SuppCode
        {
            get => txtBpCode.Text;
            set => txtBpCode.Text = value;
        }

        public string SuppName
        {
            get => txtBpName.Text;
            set => txtBpName.Text = value;
        }

        public string ContactPerson
        {
            get /*=> TxtContactPerson.Text*/;
            set /*=> TxtContactPerson.Text = value*/;
        }

        public string Company
        {
            get => Convert.ToString(cbCompany.SelectedValue);
            set => cbCompany.Text = value;
        }

        public string Department
        {
            get => txtDepartment.Text;
            set => txtDepartment.Text = value;
        }

        public string BpCurrency
        {
            get /*=> CmbCurrency.Text*/;
            set /*=> CmbCurrency.Text = value*/;
        }

        public string BpRate
        {
            get /*=> txt.Text*/;
            set/* => TxtBpRate.Text = value*/;
        }

        public string Status
        {
            get => txtDocStatus.Text;
            set => txtDocStatus.Text = value;
        }

        public string PostingDate
        {
            get => dtPostingDate.Text;
            set => dtPostingDate.Text = value;
        }

        public string DocumentDate
        {
            get => dtDocDate.Text;
            set => dtDocDate.Text = value;
        }

        public string DeliveryDate
        {
            get => dtDueDate.Text;
            set => dtDueDate.Text = value;
        }


        public string CancellationDate
        {
            get => dtCancelDate.Text;
            set => dtCancelDate.Text = value;
        }

        public DataGridView Table
        {
            get => dgvItems;
        }

        public DataGridView Udf
        {
            get => DgvUdf;
        }

        public DataGridView TablePreview
        {
            get => DgvPreviewItem;
        }

        public string Remark
        {
            get => txtRemarks.Text;
            set => txtRemarks.Text = value;
        }

        public string VatGroup
        {
            get => Vat;
            set => Vat = value;
        }

        public double VatGroupRate
        {
            get => VatRate;
            set => VatRate = value;
        }


        public string Warehouse
        {
            get => txtWhsCode.Text;
            set => txtWhsCode.Text = value;
        }
        public string Service
        {
            get => Convert.ToString(cbDocumentType.SelectedValue);
            set => cbDocumentType.Text = value;
        }

        public string TotalBeforeDiscount
        {
            get => txtTotalBefDisc.Text;
            set => txtTotalBefDisc.Text = value;
        }

        public string Tax
        {
            get => txtTaxGroup.Text;
            set => txtTaxGroup.Text = value;
        }

        public string DiscountInput
        {
            get => txtDiscPercent.Text;
            set => txtDiscPercent.Text = value.ToString();
        }

        public string DiscountAmount
        {
            get => txtDiscount.Text;
            set => txtDiscount.Text = value;
        }

        public string Total
        {
            get => txtTotal.Text;
            set => txtTotal.Text = value;
        }

        public string TaxAmount
        {
            get => txtTaxAmount.Text;
            set => txtTaxAmount.Text = value;
        }

        public ContextMenuStrip MsItems
        {
            get => msItems;
        }

        public string RawCurrency
        {
            get => RawCurr;
            set => RawCurr = value;
        }

        public string oShipTo
        {
            get => txtAddress.Text;
            set => txtAddress.Text = value;
        }

        public string oLogShipTo
        {
            get => cmbLogShipTo.Text;
            set => cmbLogShipTo.Text = value;
        }

        public string TxtTotalQty
        {
            get => txtTotal.Text;
            set => txtTotal.Text = value;
        }

        //private SO_UDF SOu = new SO_UDF();
        //SO_UDF frmUDF;
        public FrmARInvoice()
        {
            InitializeComponent();
            // LocationChanged += new EventHandler(Form1_LocationChanged);
            frmMain = StaticHelper._MainForm;
            hana = new SAPHanaAccess();
            helper = new DataHelper();
        }

        private void LoadDocumentType()
        {
            try
            {
                string sdata = "SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [@DOC_TYPE] Order by Name";
                DataTable dt = hana.Get(sdata);
                cbDocumentType.Items.Clear();
                Application.DoEvents();
                cbDocumentType.DisplayMember = "Name";
                cbDocumentType.ValueMember = "Code";
                cbDocumentType.DataSource = dt;
            }
            catch (Exception ex) { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        private void NumSeries()
        {

            try
            {
                string sdata = "SELECT SeriesName FROM NNM1 Where ObjectCode = 13";
                DataTable dt = hana.Get(sdata);

                Application.DoEvents();
                cbSeries.DisplayMember = "SeriesName";
                cbSeries.DataSource = dt;
                txtDocStatus.Text = "Open";

                var sdata1 = $"SELECT T0.ObjectCode,T0.Series,T0.SeriesName,T0.NextNumber FROM NNM1 T0 Where T0.ObjectCode = 13 Where T0.SeriesName = '{ cbSeries.Text }'";
                var dt1 = hana.Get(sdata1);

                if (dt1.Rows.Count > 0)
                {
                    cbSeries.SelectedIndex = 0;
                    oSeries = DataAccess.Search(dt1, 0, "Series"/*, StaticHelper._MainForm*/);
                    txtDocEntry.Text = DataAccess.Search(dt1, 0, "NextNumber"/*, StaticHelper._MainForm*/);
                }

            }
            catch (Exception ex) { }
        }

        private void ClearList()
        {
            //CLEAR LIST DATA
            InvoiceItemsModel.InvoiceItems.RemoveAll(x => x.ObjType != "");
            InvoiceHeaderModel.DDWdocentry.RemoveAll(x => x.DocEntry != -1);
            DECLARE._DocHeader.RemoveAll(x => x.ObjType == objType);
            DECLARE.udf.RemoveAll(x => x.ObjCode == objType);
        }

        private void LoadSAPDAta(int DocEntry)
        {
           
            FindMode = true;
            //CLEAR LIST DATA
            ClearList();
            //DataTable
            DataTable dtItems;
            DataTable dtHeader;

            var query = "Select DocNum,CardCode,CardName,DocStatus,DiscPrcnt,U_Remarks,TaxDate,DocDueDate,DocDate " +
                        ", NumAtCard " +
                        ", (select Name from [@CMP_INFO] where Code = U_CompanyTIN) [U_CompanyTIN] " +
                        ", U_Department " +
                        ", U_DocType " +
                        ", U_CancelDate " +
                        $"FROM ORDR WHERE DocEntry = {DocEntry}";

            if (oFindStat.Contains("Draft"))
            {
                query = "SELECT T1.DocNum, T1.CardCode, T1.CardName, T1.DocStatus, T1.DiscPrcnt, T1.U_Remarks, T1.TaxDate, T1.DocDueDate, T1.DocDate " +
                        ", T1.NumAtCard " +
                        ", (select Name from [@CMP_INFO] where Code = U_CompanyTIN) [U_CompanyTIN] " +
                        ", T1.U_Department" +
                        ", T1.U_DocType " +
                        ", T1.U_CancelDate " +
                        "FROM OWDD T0 " +
                        "INNER JOIN ODRF T1 ON T0.DocEntry = T1.DocEntry and T1.ObjType = '17'" +
                        $"WHERE T1.DocEntry = {DocEntry}";
            }


            string _bpCode, _docNum, _status, _discperc, _remarks, taxdate, docduedate, docdate;

            DataTable headerDetails = DataAccess.Select(DataAccess.conStr("HANA"), query);

            _bpCode = DECLARE.dtNull(headerDetails, 0, "CardCode", "");
            _docNum = DECLARE.dtNull(headerDetails, 0, "DocNum", "0");
            _status = DECLARE.dtNull(headerDetails, 0, "DocStatus", "");
            _discperc = DECLARE.dtNull(headerDetails, 0, "DiscPrcnt", "");
            _remarks = DECLARE.dtNull(headerDetails, 0, "U_Remarks", "");

            taxdate = DECLARE.dtNull(headerDetails, 0, "TaxDate", "");
            docduedate = DECLARE.dtNull(headerDetails, 0, "DocDueDate", "");
            docdate = DECLARE.dtNull(headerDetails, 0, "DocDate", "");

            if (_status == "O")
            {
                txtDocStatus.Text = "Open";
            }
            else
            {
                txtDocStatus.Text = "Closed";
            }

            txtBpCode.Text = _bpCode;
            txtDocEntry.Text = _docNum;
            txtRemarks.Text = _remarks;
            txtDocEntry2.Text = DocEntry.ToString();
            txtDiscPercent.Text = _discperc;

            dtPostingDate.Text = docdate;
            dtDocDate.Text = taxdate;
            dtDueDate.Text = docduedate;

            txtCustomRefNo.Text = DECLARE.dtNull(headerDetails, 0, "NumAtCard", "");
            cbCompany.SelectedIndex = cbCompany.FindString(DECLARE.dtNull(headerDetails, 0, "U_CompanyTIN", ""));
            txtDepartment.Text = DECLARE.dtNull(headerDetails, 0, "U_Department", "");
            cbDocumentType.Text = DECLARE.dtNull(headerDetails, 0, "U_DocType", "");
            dtCancelDate.Text = DECLARE.dtNull(headerDetails, 0, "U_CancelDate", "");

            LoadBPDetails(_bpCode, true);

            var queryItems = "SELECT " +
                            " T0.ItemCode" +
                            ", T0.Dscription" +
                            ", T0.OpenQty [Quantity]" +
                            ", T0.Price" +
                            ", T0.DiscPrcnt" +
                            ", T0.LineTotal" +
                            ", T0.WhsCode" +
                            ", T0.SlpCode" +
                            ", T0.PriceBefDi" +
                            ", T0.Project" +
                            ", T0.VatGroup" +
                            ", T0.VatPrcnt" +
                            ", T0.CodeBars" +
                            ", T0.PriceAfVAT" +
                            ", T0.TaxCode" +
                            ", T0.VatAppld" +
                            ", T0.LineVat" +
                            ", T1.U_ID025 [U_StyleCode]" +
                            ", T1.U_ID011 [U_Color]" +
                            ", T1.U_ID018 [U_Section]" +
                            ", T1.U_ID007 [U_Size]" +
                            ", T1.CodeBars " +
                            " FROM RDR1 T0 " +
                            "INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode Where T0.DocEntry = '" + DocEntry + "'";

            if (oFindStat.Contains("Draft"))
            {
                queryItems = "SELECT T1.ItemCode, T1.Dscription " +
                    ", T1.Quantity, T1.Price, T1.DiscPrcnt, T1.LineTotal, T1.WhsCode, T1.SlpCode " +
                    ", T1.PriceBefDi, T1.Project, T1.VatGroup, T1.VatPrcnt " +
                    ", T1.CodeBars, T1.PriceAfVAT, T1.TaxCode, T1.VatAppld, T1.LineVat " +
                    ", T2.U_ID025 [U_StyleCode]" +
                    ", T2.U_ID011 [U_Color]" +
                    ", T2.U_ID018 [U_Section]" +
                    ", T2.U_ID007 [U_Size]" +
                    ", T2.CodeBars " +
                    "FROM OWDD T0 " +
                    "INNER JOIN DRF1 T1 ON T0.DocEntry = T1.DocEntry and T1.ObjType = '17' " +
                    "INNER JOIN OITM T2 ON T1.ItemCode = T2.ItemCode " +
                    $"WHERE T0.DocEntry = '{DocEntry}'";
            }

            //LINE ITEMS
            dtItems = DataAccess.Select(DataAccess.conStr("HANA"), queryItems);

            for (int x = 0; x < dtItems.Rows.Count; x++)
            {
                Double LineTotal;
                Double GrossTotal;
                Double VatAmount;
                Double GrossPrice;
                Double PriceVatInc;
                Double DiscAmt;
                Double Discount;

                double qty = Convert.ToDouble(dtItems.Rows[x]["Quantity"]);
                double discount = Convert.ToDouble(dtItems.Rows[x]["DiscPrcnt"]);
                double price = Convert.ToDouble(dtItems.Rows[x]["Price"]);
                double vatrate = Convert.ToDouble(dtItems.Rows[x]["VatPrcnt"]);
                double priceaftvat = Convert.ToDouble(dtItems.Rows[x]["PriceAfVat"]);
                double pricebefdisc = Convert.ToDouble(dtItems.Rows[x]["PriceBefDi"]);

                PriceVatInc = price + (price * (vatrate / 100));
                DiscAmt = pricebefdisc * (discount / 100);
                LineTotal = (qty * pricebefdisc);
                //VatAmount = LineTotal * (vatrate / 100);

                VatAmount = LineTotal * (vatrate / 100) - ((LineTotal * (vatrate / 100)) * (discount / 100));

                GrossTotal = (LineTotal + VatAmount) - (DiscAmt * qty);
                GrossPrice = PriceVatInc - (PriceVatInc * (discount / 100));


                Discount = priceaftvat / (1 - (discount / 100));
                var itemDetails = helper.ReadDataRow(hana.Get(SP.AW_GetItemDetails), 1, "", 0);
                var dt = hana.Get(string.Format(itemDetails, dtItems.Rows[x]["ItemCode"].ToString()));
                InvoiceItemsModel.InvoiceItems.Add(new InvoiceItemsModel.InvoiceItemsData
                {
                    ObjType = objType,
                    ItemCode = dtItems.Rows[x]["ItemCode"].ToString(), // ItemCode
                    ItemName = dtItems.Rows[x]["Dscription"].ToString(), // ItemCode
                    Style = dtItems.Rows[x]["U_StyleCode"].ToString(), //Style
                    Color = dtItems.Rows[x]["U_Color"].ToString(), //Color
                    Size = dtItems.Rows[x]["U_Size"].ToString(), //Size
                    Brand = LibraryHelper.DataTableRet(dt, 0, "Brand", ""), //Brand
                    Section = dtItems.Rows[x]["U_Section"].ToString(), //Section
                    BarCode = dtItems.Rows[x]["CodeBars"].ToString(),
                    GrossPrice = Math.Round(Discount, 2), //GrossPrice
                    UnitPrice = Math.Round(Convert.ToDouble(dtItems.Rows[x]["PriceBefDi"]), 2),
                    Quantity = Convert.ToInt32(dtItems.Rows[x]["Quantity"]), //Qty
                    DiscountPerc = Convert.ToDouble(dtItems.Rows[x]["DiscPrcnt"]),
                    DiscountAmount = Math.Round(DiscAmt * qty, 2),
                    FWhsCode = dtItems.Rows[x]["WhsCode"].ToString(), //WHsCode
                    TaxCode = dtItems.Rows[x]["VatGroup"].ToString(), //Tax Code
                    TaxAmount = Math.Round(VatAmount, 2),
                    TaxRate = Math.Round(Convert.ToDouble(vatrate), 2),
                    LineTotal = Math.Round(LineTotal, 2),
                    GrossTotal = Math.Round(GrossTotal, 2),
                    EffectivePrice = GetEffectivePrice(_bpCode, dtItems.Rows[x]["ItemCode"].ToString())
                });
            }

            dgvItems.Columns.Clear();

            RefreshData();

            DisableControls();
        }

        private void DisableControls()
        {
            //txtDiscPercent.ReadOnly = false;
            //button1.Enabled = false;

            //Hide Selection
            pbBPList.Visible = false;
            pbTaxList.Visible = false;
            pbWhs.Visible = false;
            //disable DateTime Picker
            dtDocDate.Enabled = true;
            dtDueDate.Enabled = true;
            dtPostingDate.Enabled = true;
            cbSeries.Enabled = false;
            //Mode
            FindMode = false;
        }

        private double GetEffectivePrice(string strBpCode, string strItemCode)
        {
            double GetEP = 0;
            try
            {
                double tax = Convert.ToDouble(hana.Get($@"SELECT T0.ECVatGroup, T1.Rate FROM OCRD T0  INNER JOIN OVTG T1 ON T0.ECVatGroup = T1.Code WHERE T0.CardCode = '{txtBpCode.Text}'").Rows[0]["Rate"].ToString());
                tax = 1 + (tax / 100);

                string strGetEPqry = "select T1.CardCode " +
                                    " , CASE WHEN(T1.GroupCode) = '151' " +
                                    " THEN " +
                                    " (CASE WHEN(select IsGrossPrc from OPLN where ListNum = '3') = 'Y' " +
                                    $" THEN(Select ISNULL(ROUND(Z.Price / {tax}, 2), 0) From ITM1 Z Where Z.ItemCode = '{strItemCode}' and Z.PriceList = '3') " +
                                    $" ELSE(Select ISNULL(Z.Price, 0) From ITM1 Z Where Z.ItemCode = '{strItemCode}' and Z.PriceList = '3') END) " +
                                    " ELSE " +
                                    " (CASE WHEN(select IsGrossPrc from OPLN where ListNum = '2') = 'Y' " +
                                    $" THEN(Select ISNULL(ROUND(Z.Price / {tax}, 2), 0) From ITM1 Z Where Z.ItemCode = '{strItemCode}' and Z.PriceList = '2') " +
                                    $" ELSE(Select ISNULL(Z.Price, 0) From ITM1 Z Where Z.ItemCode = '{strItemCode}' and Z.PriceList = '2') END) " +
                                    " END[EffectivePrice] " +
                                    " from OCRD T1 " +
                                    " where " +
                                    $" T1.CardCode = '{strBpCode}'";

                if (DataAccess.Select(DataAccess.conStr("HANA"), strGetEPqry).Rows.Count > 0)
                {
                    GetEP = Convert.ToDouble(DataAccess.Select(DataAccess.conStr("HANA"), strGetEPqry).Rows[0]["EffectivePrice"].ToString());
                }

                return GetEP;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                return GetEP;
            }

        }


        public InvoiceService Presenter
        {
            private get;
            set;
        }

        private void LoadCompany()
        {
            try
            {
                string sdata = "select '' [Code], '' [Name] UNION select Code, Name from [@CMP_INFO]";
                DataTable dt = hana.Get(sdata);
                cbCompany.Items.Clear();
                Application.DoEvents();
                cbCompany.DisplayMember = "Name";
                cbCompany.ValueMember = "Code";
                cbCompany.DataSource = dt;
            }
            catch (Exception ex) { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        private void pbBPList_Click_1(object sender, EventArgs e)
        {
            ViewList("OCRD", out oCode, out oName, "List of Business Partners", "C");
            InvoiceHeaderModel.oBPCode = "";

            if (oCode != null)
            {
                txtBpCode.Text = oCode;
                InvoiceHeaderModel.oBPCode = oCode;
                LoadBPDetails(oCode, false);
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pbDepartment_Click(object sender, EventArgs e)
        {
            //Presenter.GetDepartment();

            ViewList("Department", out oCode, out oName, "List of Departments", "C");

            if (oCode != null)
            {
                txtDepartment.Text = oCode;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtBpCode.Text != "" && txtTaxGroup.Text != "" && txtWhsCode.Text != "" && cbDocumentType.Text != "")
            {
                InvoiceHeaderModel.oDocDate = Convert.ToDateTime(dtPostingDate.Text);
                InvoiceHeaderModel.oDocType = cbDocumentType.Text;
                FrmARInvoiceItemList so = new FrmARInvoiceItemList(this, StaticHelper._MainForm);
                so.ShowDialog();
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("BP Code/Tax Group/Price List/Warehouse/Document Type should not be empty", true);
                //MessageBox.Show(, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pbWhs_Click(object sender, EventArgs e)
        {
            ViewList("OWHS", out oCode, out oName, "List of Warehouses", "C");

            txtWhsCode.Text = oCode;

            oFWhsCode = oCode;
            InvoiceHeaderModel.oWhsCode = oFWhsCode;
        }

        private void LoadBPDetails(string CardCode, bool FromFind)
        {

            string query = $"SELECT A.CardCode" +
                ",A.CardName" +
                ",A.MailAddres [Address]" +
                ",(SELECT Z.Address FROM CRD1 Z Where Z.CardCode = A.CardCode And Z.Street = A.MailAddres  And Z.AdresType = 'S') [AddressCode]" +
                ",A.ShipToDef [AddressID]" +
                ",A.ProjectCod" +
                ",A.SlpCode" +
                ",A.ListNum" +
                ",(Select Z.SlpName FROM OSLP Z Where Z.SlpCode = A.SlpCode) [SlpName]" +
                $",ECVatGroup FROM OCRD A WHERE A.CardCode = '{ CardCode }'";

            //On comment by Cedi on 061219
            //var dt = DataAccess.Select(DataAccess.conStr("HANA"), query);
            var dt = hana.Get(query);

            if (dt.Rows.Count > 0)
            {
                txtBpName.Text = DECLARE.dtNull(dt, 0, "CardName", "");
                txtAddress.Text = DECLARE.dtNull(dt, 0, "AddressID", "");
                oSalesEmployee = DECLARE.dtNull(dt, 0, "SlpCode", "");
                txtSalesEmployee.Text = DECLARE.dtNull(dt, 0, "SlpName", "");
                //oPriceList = DECLARE.dtNull(dt, 0, "ListNum", "0");
                InvoiceHeaderModel.oPriceList = oPriceList;
                txtBpName.Text = DECLARE.dtNull(dt, 0, "CardName", "");
                oTaxGroup = DECLARE.dtNull(dt, 0, "ECVatGroup", "");
                oProject = DECLARE.dtNull(dt, 0, "ProjectCod", "");
                oAddressCode = DECLARE.dtNull(dt, 0, "AddressCode", "");
                //oFWhsCode = DataAccess.SearchData(DataAccess.conStr("HANA"), $"Select U_Whs FROM CRD1 Where CardCode = '{CardCode}'", 0, "U_Whs", frmMain);
                //txtPriceList.Text = oPriceList;
                txtTaxGroup.Text = oTaxGroup;
                InvoiceHeaderModel.oTaxGroup = oTaxGroup;
                //txtWhsCode.Text = oFWhsCode;

                string query1 = "Select Code,Name,Rate From OVTG Where Code = '" + oTaxGroup + "'";

                var dt1 = new DataTable();
                //On comment by Cedi on 061219
                //dt1 = DataAccess.Select(DataAccess.conStr("HANA"), query1);
                dt1 = hana.Get(query1);

                if (dt1.Rows.Count > 0)
                {
                    oTaxRate = DECLARE.dtNull(dt1, 0, "rate", "");
                }

                if (FromFind == false)
                {
                    LoadRAS(CardCode);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var trans = new object();
            foreach (DataGridViewRow row in DgvUdf.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals("Trans. Category"))
                {
                    trans = DgvUdf.Rows[row.Index].Cells["Field"].Value;
                    break;
                }
            }

            //On comment 091319
            //var isZeroQuantity = InvoiceItemsModel.InvoiceItems.Where(x => x.Quantity == 0).Any();
            //if ((txtBpCode.Text != string.Empty && trans != null && isZeroQuantity is false) || InvoiceHeaderModel.oSelectedDoc == "Pick List")
            if ((txtBpCode.Text != string.Empty && trans != null) || InvoiceHeaderModel.oSelectedDoc == "Pick List")
            {
                btnAdd.Enabled = false;
                if (Presenter.ExecuteRequest(btnAdd.Text))
                {
                    btnAdd.Text = btnAdd.Text == "Update" ? "Add" : "Add";
                }
                btnAdd.Enabled = true;
            }
            else
            {
                if (DgvUdf.Rows[4].Cells["Field"].Value != null)
                {
                    //On comment 091319
                    //var msg = DgvUdf.Rows[4].Cells["Field"].Value.ToString() == string.Empty ? "Select Transaction Category" : isZeroQuantity ? "Please Input Quantity" : "Enter Bp Code";
                    var msg = DgvUdf.Rows[4].Cells["Field"].Value.ToString() == string.Empty ? "Select Transaction Category" : "Enter Bp Code";
                    StaticHelper._MainForm.ShowMessage($"Warning: {msg}", true);
                }
                else
                {
                    //On comment 091319
                    //var msg = DgvUdf.Rows[4].Cells["Field"].Value == null ? "Select Transaction Category" : isZeroQuantity ? "Please Input Quantity" : "Enter Bp Code";
                    var msg = DgvUdf.Rows[4].Cells["Field"].Value == null ? "Select Transaction Category" : "Enter Bp Code";
                    StaticHelper._MainForm.ShowMessage($"Warning: {msg}", true);
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BtnNewDocument_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Presenter.ClearField(true);
                dgvItems.ReadOnly = false;
                dgvItems.ReadOnly = false;
                button1.Enabled = true;
                pbBPList.Enabled = true;
                pbTaxList.Enabled = true;
                cbCompany.Enabled = true;
                cbDocumentType.Enabled = true;
                pbDepartment.Enabled = true;
                pbWhs.Enabled = true;
                TabSO.SelectedIndex = 0;
                btnAdd.Text = "Add";
                InvoiceHeaderModel.oSelectedDoc = "";
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (txtDocEntry.Text == string.Empty)
            {
                //PublicStatic.frmMain.NotiMsg("Warning: Please select document first", Color.Red);
            }
            else
            {
                TabSO.SelectedIndex = 3;
                TxtPrintDocNo.Text = txtDocEntry.Text;
            }
        }
        
        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void DgvPreviewItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex - 1;
            colIndex = colIndex < 0 ? 0 : colIndex;

            string columnName = dgvItems.Columns[colIndex].Name;
            int index = dgvItems.Focus() ? Convert.ToInt32(dgvItems.CurrentRow.Cells["LineNum"].Value) : Convert.ToInt32(DgvPreviewItem.CurrentRow.Cells["LineNum"].Value);

            switch (columnName)
            {
                case "Warehouse":

                    Presenter.GetWarehouse(colIndex, index);
                    break;

                case "Tax Code":

                    Presenter.GetTaxCode(e);
                    break;

                case "Department":

                    Presenter.GetDepartment(colIndex, index);
                    break;

                case "Chain Description":

                    Presenter.GetChain(colIndex, index);
                    break;

                case "UoM":

                    Presenter.GetUom(colIndex, index);
                    break;

                case "Project":

                    Presenter.GetProject(colIndex, index);
                    break;

                case "G/L Account Name":

                    Presenter.GetGlAccount(colIndex, index);
                    break;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            DataRepository.RowSearch(DgvPreviewItem, TxtSearch.Text, 0);
        }

        private void TxtSearchDocument_TextChanged(object sender, EventArgs e)
        {
            DataRepository.RowSearch(DgvFindDocument, TxtSearchDocument.Text, findDocSearch);
        }

        private void CmbFilterDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            DgvFindDocument.DataSource = null;
            try
            {
                switch (CmbFilterDocument.Text)
                {
                    case "All":
                        DgvFindDocument.DataSource = CurrentDataSet;
                        break;
                    case "Open":
                        var dt = CurrentDataSet.Select("Status = 'Open'").CopyToDataTable();
                        DgvFindDocument.DataSource = dt;
                        break;

                    case "Canceled":
                        var dtc = CurrentDataSet.Select("Status = 'Canceled'").CopyToDataTable();
                        DgvFindDocument.DataSource = dtc;
                        break;

                    case "Closed":
                        var dtd = CurrentDataSet.Select("Status = 'Closed'").CopyToDataTable();
                        DgvFindDocument.DataSource = dtd;
                        break;
                    case "Draft Pending":
                        if (!Presenter.CheckUser())
                        {
                            var dt1 = CurrentDataSet.Select("Status = 'Draft-Pending'").CopyToDataTable();
                            DgvFindDocument.DataSource = dt1;
                        }
                        break;
                    case "Draft Approved":
                        if (!Presenter.CheckUser())
                        {
                            var dt2 = CurrentDataSet.Select("Status = 'Draft-Approved'").CopyToDataTable();
                            DgvFindDocument.DataSource = dt2;
                        }
                        break;
                }
            }
            catch
            {
                DgvFindDocument.DataSource = null;
            }
        }

        private void TabSO_SelectedIndexChanged(object sender, EventArgs e)
        {


            switch (TabSO.SelectedIndex)
            {
                case 0:
              
                    Presenter.LoadData(dgvItems);
                    break;

                case 1:
                 
                    Presenter.LoadData(DgvPreviewItem);
                    break;

                case 2:
                    
                    Presenter.GetExistingDocument(DgvFindDocument);
                    CurrentDataSet = Presenter.ListDocuments();
                    CmbFilterDocument.Text = "All";
                    break;

                case 3:
            
                    if (txtDocEntry.Text == string.Empty)
                    {
                        //PublicStatic.frmMain.NotiMsg("Warning: Please select document first", Color.Red);
                    }
                    else
                    {
                        TabSO.SelectedIndex = 3;
                        TxtPrintDocNo.Text = txtDocEntry.Text;
                    }
                    break;

                default:
                    MessageBox.Show("TabSO_SelectedIndexChanged :Default");
                    break;
            }
        }

        private void dgvItems_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Presenter.DeleteItem(e.RowIndex, e.ColumnIndex);
            }
        }

        private void dgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                Presenter.CurrentRowComputation(e);

                DataGridView dgv = (DataGridView)sender;
                var row = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                DataGridViewColumn cell = dgv.Columns[e.ColumnIndex];

                if (cell.Name.Equals("Unit Price") ||
                    cell.Name.Equals("Disc Amount") ||
                    cell.Name.Equals("Total(LC)") ||
                    cell.Name.Equals("Gross Total (LC)") ||
                    cell.Name.Equals("Unit Price per piece") ||
                    cell.Name.Equals("Gross Price per piece") ||
                    cell.Name.Equals("Gross Price"))
                {
                    if (IsNumeric(row))
                    {
                        double sNumber = double.Parse(row.ToString());
                        string ret = string.Format("{0:#,0.00}", Math.Round(sNumber, 2));
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ret;
                    }
                }

                if (cell.Name.Equals("Tax Code"))
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = row.ToString();
                }

                this.BeginInvoke(new MethodInvoker(() =>
                {
                    Application.DoEvents();
                }));
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }

        }

        void ComputeLines()
        {
            int iLineNum = 0;
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (dgvItems.Rows[iLineNum].Cells[10].Value != null)
                {
                    DataGridViewCellEventArgs dgvcecnt = new DataGridViewCellEventArgs(10, iLineNum);
                    //RecomputeLines(dgvcecnt);
                    Presenter.CurrentRowComputation(dgvcecnt);
                }
                iLineNum++;
            }
        }

        bool IsNumeric(object e)
        {
            bool result = false;
            if (e != null)
            { result = double.TryParse(e.ToString(), out double n); }

            return result;
        }

        private void dgvItems_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rowCount = dgvItems.RowCount;
            int currentRow = e.RowIndex + 1;

            if (rowCount == currentRow)
            {
                dgvItems.Rows[e.RowIndex].ReadOnly = true;
            }
        }

        private void dgvItems_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void Dgv_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var table = DgvPreviewItem.Focus() ? DgvPreviewItem : dgvItems;

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            {
                string clipBoard = Clipboard.GetText();

                foreach (DataGridViewCell cell in table.SelectedCells)
                {
                    cell.Value = clipBoard;

                    Presenter.CurrentRowComputation(null, cell.RowIndex);
                }
            }
        }

        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvItems.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        //private void dgvItems_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (e.RowIndex != -1)
        //        {
        //            dgvItems.CurrentCell = dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex + 1];
        //            dgvItems.Rows[e.RowIndex].Selected = true;
        //            dgvItems.Focus();

        //            var mousePosition = dgvItems.PointToClient(Cursor.Position);

        //            msItems.Show(dgvItems, mousePosition);
        //        }
        //    }
        //}

        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var fieldName = dgvItems.Columns[e.ColumnIndex].Name;

                if (fieldName.Equals("Tax Rate"))
                {
                    var itemcode = dgvItems.Rows[e.RowIndex].Cells["Item No."].Value.ToString();
                    var LineNum = dgvItems.Rows[e.RowIndex].Cells["LineNum"].Value.ToString();
                    var SOList = InvoiceItemsModel.InvoiceItems.First(x => x.ItemCode == itemcode && LineNum == LineNum.ToString());
                    var row = dgvItems.Rows[e.RowIndex];

                    var unitPrice = double.Parse(DECLARE.Replace(row, "Unit Price", "0"));

                    var UdiscountAmount = ComputeDiscountAmount(row, unitPrice);
                    SOList.DiscountAmount = UdiscountAmount;

                    var UgrossPrice = ComputeGrossPrice(row, unitPrice, UdiscountAmount);
                    SOList.GrossPrice = UgrossPrice;

                    var UpriceAfterDisc = ComputePriceAfterDisc(row, UgrossPrice);
                    SOList.PriceAfterDisc = UpriceAfterDisc;

                    SOList.LineTotal = ComputeLineTotal(row, UpriceAfterDisc, UdiscountAmount);

                    SOList.GrossTotal = ComputeGrossTotal(row, UgrossPrice);

                    ComputeTotal();
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        double ComputeDiscountAmount(DataGridViewRow row, double dUnitPrice)
        {
            var output = 0.0;
            var discountPercentage = double.Parse(DECLARE.Replace(row, "Discount %", "0"));
            output = (dUnitPrice / 100) * discountPercentage;

            row.Cells["Discount"].Value = Math.Round(output, 3).ToString("#,#00.00");
            return output;
        }

        double ComputeUnitPrice(DataGridViewRow row, double dGrossPrice)
        {
            var output = 0.0;
            var taxAmount = 1 + (double.Parse(DECLARE.Replace(row, "Tax Rate", "0.00")) / 100);
            output = dGrossPrice / taxAmount;

            row.Cells["Unit Price"].Value = Math.Round(output, 3).ToString("#,#00.00");
            return output;
        }

        double ComputeGrossPrice(DataGridViewRow row, double dUnitPrice, double dDiscountAmount)
        {
            var output = 0.0;
            var taxAmount = 1 + (double.Parse(DECLARE.Replace(row, "Tax Rate", "0.00")) / 100);
            output = (dUnitPrice - dDiscountAmount) * taxAmount;

            row.Cells["Gross Price"].Value = Math.Round(output, 3).ToString("#,#00.00");
            return output;
        }

        double ComputeDiscountPercentage(DataGridViewRow row, double dDiscountAmount, double dUnitPrice)
        {
            var output = 0.0;

            output = (dDiscountAmount / dUnitPrice) * 100;

            row.Cells["Discount %"].Value = Math.Round(output, 3).ToString("#,#00.00");
            return output;

        }

        double ComputePriceAfterDisc(DataGridViewRow row, double dGrossPrice)
        {
            var output = 0.0;
            var taxAmount = 1 + (double.Parse(DECLARE.Replace(row, "Tax Rate", "0.00")) / 100);
            output = dGrossPrice / taxAmount;
            row.Cells["PriceAfterDisc"].Value = Math.Round(output, 3).ToString("#,#00.00");
            return output;
        }

        double ComputeLineTotal(DataGridViewRow row, double dPriceAfterDisc, double dDiscountAmount)
        {
            var output = 0.0;
            var quantity = double.Parse(DECLARE.Replace(row, "Quantity", "0"));
            output = quantity * (dPriceAfterDisc - dDiscountAmount);

            row.Cells["Line Total"].Value = Math.Round(output, 3).ToString("#,#00.00");
            return output;
        }

        double ComputeGrossTotal(DataGridViewRow row, double dGrossPrice)
        {
            var output = 0.0;
            var quantity = double.Parse(DECLARE.Replace(row, "Quantity", "0"));
            output = dGrossPrice * quantity;

            row.Cells["Gross Total"].Value = Math.Round(output, 3).ToString("#,#00.00");
            return output;
        }

        private void dgvItems_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex - 1;
            colIndex = colIndex < 0 ? 0 : colIndex;

            string columnName = dgvItems.Columns[colIndex].Name;
            int index = 0;
            try
            {

                index = dgvItems.Focus() ? Convert.ToInt32(dgvItems.CurrentRow.Cells["Linenum"].Value) : Convert.ToInt32(DgvPreviewItem.CurrentRow.Cells["Linenum"].Value);
            }
            catch { }

            switch (columnName)
            {
                case "Warehouse":

                    Presenter.GetWarehouse(colIndex, index);
                    break;

                case "Tax Code":

                    Presenter.GetTaxCode(e);
                    break;

                case "Department":

                    Presenter.GetDepartment(colIndex, index);
                    break;

                case "Chain Description":

                    Presenter.GetChain(colIndex, index);
                    break;

                case "UoM":

                    Presenter.GetUom(colIndex, index);
                    break;

                case "Project":

                    Presenter.GetProject(colIndex, index);
                    break;

                case "G/L Account Name":

                    Presenter.GetGlAccount(colIndex, index);
                    break;

                case "Company":

                    Presenter.GetCompany(colIndex, index);
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddNew();
        }

        private void AddNew()
        {
            if (dgvItems.Rows.Count > 0)
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    btnAdd.Enabled = true;
                    oEnabled(true);
                    EnableControls();
                    ClearData();
                    FindMode = false;
                    btnAdd.Text = "Add";
                    //ClearList();
                }
            }
            else
            {
                oEnabled(true);
                EnableControls();
                ClearData();
                FindMode = false;
                btnAdd.Text = "Add";
            }
        }

        private void EnableControls()
        {
            txtDiscPercent.ReadOnly = false;
            button1.Enabled = true;

            //Hide Selection
            pbBPList.Visible = true;
            pbTaxList.Visible = true;
            pbWhs.Visible = true;
            //disable DateTime Picker
            dtDocDate.Enabled = true;
            dtDueDate.Enabled = true;
            dtPostingDate.Enabled = true;
            cbSeries.Enabled = true;
            //Mode
            FindMode = false;
        }

        private void ClearData()
        {
            ClearList();
            dgvItems.Columns.Clear();
            cbCompany.SelectedIndex = 0;
            cbDocumentType.SelectedIndex = 0;

            foreach (Control c in panel2.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
            }
            txtAddress.Text = "";
            NumSeries();
            //frmUDF._LoadData();
            oFWhsCode = ""; oTWhsCode = "";
        }

        private void oEnabled(Boolean bEnabled)
        {
            pbBPList.Enabled = bEnabled;
            pbTaxList.Enabled = bEnabled;
            txtCustomRefNo.Enabled = bEnabled;
            cbCompany.Enabled = bEnabled;
            pbDepartment.Enabled = bEnabled;
            cbDocumentType.Enabled = bEnabled;
            cbSeries.Enabled = bEnabled;

            dtPostingDate.Enabled = bEnabled;
            dtDueDate.Enabled = bEnabled;
            dtDocDate.Enabled = bEnabled;
            dtCancelDate.Enabled = bEnabled;

            pbWhs.Enabled = bEnabled;
            pictureBox2.Enabled = bEnabled;
            button1.Enabled = bEnabled;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Find();
        }

        private void Find()
        {
            if (DECLARE._DocItems.Where(x => x.ObjType == objType).ToList().Count > 0)
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    LoadFindDetails();
                }
            }
            else
            {
                LoadFindDetails();
            }
        }

        private void LoadFindDetails()
        {
            ViewList("ORDR", out oCode, out oName, "List of Sales Orders", "C");

            if (oCode != null)
            {
                txtDocEntry.Text = oName;
                _DocEntry = oCode;

                FindMode = false;

                if (oName.Equals("D-Pending") || oName.Equals("C"))
                {
                    btnAdd.Enabled = false;
                }
                else
                {
                    btnAdd.Enabled = true;
                    btnAdd.Text = oName == "D-Approved" ? "Add" : "Update";
                }

                oFindStat = "Regular";

                if (oName == "D-Approved" || oName == "D-Pending")
                {
                    oFindStat = "Draft-P";
                    if (oName == "D-Approved")
                    {
                        oEnabled(false);
                        oFindStat = "Draft-A";
                    }
                    //frmUDF.LoadUDF(oCode, "ODRF");
                }
                else
                {
                    oEnabled(true);
                    // frmUDF.LoadUDF(oCode, "ORDR");
                }

                ///comment ko muna
                LoadSAPDAta(Convert.ToInt32(oCode));

            }
        }

        private void DgvUdf_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Presenter.UdfRequest();
            txtDocEntry.Focus();
        }

        void Cancel()
        {
            if (InvoiceItemsModel.InvoiceItems.ToList().Count > 0)
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    ClearList();
                    Dispose();
                }
            }
            else
            {
                ClearList();
                Dispose();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void BtnChoose_Click(object sender, EventArgs e)
        {

            ChooseDocument();
        }

        private void ChooseDocument()
        {
            var itemNo = DgvFindDocument.CurrentRow.Cells[0].Value;

            if (itemNo != null)
            {
                string status = DgvFindDocument.CurrentRow.Cells[1].Value.ToString();
                Presenter.ClearField(true);
                Presenter.GetSelectedDocument(table, itemNo.ToString(), status);
                TabSO.SelectedIndex = 0;
                dgvItems.ReadOnly = true;
                dgvItems.ReadOnly = true;
                cbCompany.Enabled = false;
                cbDocumentType.Enabled = false;
                button1.Enabled = false;
                pbBPList.Enabled = false;
                pbTaxList.Enabled = false;
                pbDepartment.Enabled = false;
                pbWhs.Enabled = false;
                btnAdd.Text = "Update";
            }
        }

        private void cbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sdata = $"SELECT T0.ObjectCode,T0.Series,T0.SeriesName,T0.NextNumber FROM NNM1 T0 Where T0.ObjectCode = 17 And SeriesName = '{ cbSeries.Text }'";

            var dt = DataAccess.Select(DataAccess.conStr("HANA"), sdata);

            oSeries = DataAccess.Search(dt, 0, "Series"/*, PublicStatic.frmMain*/);
            txtDocEntry.Text = DataAccess.Search(dt, 0, "NextNumber"/*, PublicStatic.frmMain*/);
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void cbDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckDocumentTypeMaintenance();
        }


        private void CheckDocumentTypeMaintenance()
        {
            try
            {
                if (SOm.SelValue("ARseries", cbDocumentType.Text) != "")
                {
                    cbSeries.SelectedIndex = cbSeries.FindString(SOm.SelValue("ARseries", cbDocumentType.Text));
                }

                txtWhsCode.Clear();

                if (SOm.SelValue("toWhs1", cbDocumentType.Text, txtBpCode.Text, txtAddress.Text) != "")
                {
                    txtWhsCode.Text = SOm.SelValue("toWhs1", cbDocumentType.Text, txtBpCode.Text, txtAddress.Text);
                }

                if (cbDocumentType.Text == "Personal Order")
                {
                    oItmsGrpCod = "100";
                    oOutRight = "";
                    //ReloadDiscount("Y");
                    ComputeLines();
                }
                else if (cbDocumentType.Text == "Outright Order")
                {
                    oItmsGrpCod = "";
                    oOutRight = "Y";
                    //ReloadDiscount("N");
                    ComputeLines();
                }
                else
                {
                    oItmsGrpCod = "";
                    oOutRight = "";
                    //ReloadDiscount("N");
                    ComputeLines();
                }

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }


        private void ReloadDiscount(string WithDiscount)
        {
            try
            {
                int iLineNum = 0;
                double dblDiscPerc = 0;

                if (dgvItems.Rows.Count > 0)
                {
                    if (WithDiscount == "Y")
                    {
                        dblDiscPerc = Convert.ToDouble(DataAccess.Select(DataAccess.conStr("HANA"), "select ISNULL(100 - (Factor*100),0) [Discount] from OPLN where ListNum = '3' ").Rows[0]["Discount"].ToString());
                    }

                    foreach (DataGridViewRow row in dgvItems.Rows)
                    {
                        if (dgvItems.Rows[iLineNum].Cells[10].Value != null)
                        {
                            dgvItems.Rows[iLineNum].Cells[10].Value = dblDiscPerc;
                            DataGridViewCellEventArgs dgvcecnt = new DataGridViewCellEventArgs(10, iLineNum);
                            RecomputeLines(dgvcecnt);
                        }
                        iLineNum++;
                    }
                    ComputeTotal();
                }

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private void RecomputeLines(DataGridViewCellEventArgs e)
        {
            try
            {
                var col1 = e.ColumnIndex;
                var row1 = e.RowIndex;
                string oItemCode = dgvItems.Rows[row1].Cells[0].Value.ToString();
                string ColName = dgvItems.Columns[e.ColumnIndex].Name;

                if (ColName == "Unit Price")
                {
                    RLgrossprice(e, oItemCode);
                    RLpriceafterdisc(e, oItemCode, "UP");
                    RLlinetotal(e, oItemCode);
                    RLdiscamount(e, oItemCode);
                }
                else if (ColName == "Gross Price")
                {
                    RLpriceafterdisc(e, oItemCode, "GP");
                    RLlinetotal(e, oItemCode);
                }
                else if (ColName == "Quantity")
                {
                    RLlinetotal(e, oItemCode);
                }
                else if (ColName == "Discount %" || ColName == "Discount")
                {
                    if (ColName == "Discount %")
                    {
                        RLdiscamount(e, oItemCode);
                    }
                    else
                    {
                        RLdiscpercent(e, oItemCode);
                    }
                    RLgrossprice(e, oItemCode);
                    RLlinetotal(e, oItemCode);
                    RLpriceafterdisc(e, oItemCode, "UP");
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }

        }

        private void RLpriceafterdisc(DataGridViewCellEventArgs e, string oItemCode, string oValueFrom)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblPriceAfterDisc = 0;

            //PriceAfterDisc
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string ogvItem = row.Cells[0].Value.ToString();
                    if (ogvItem == oItemCode)
                    {
                        double dblQty = Convert.ToDouble(DECLARE.Replace(row, "Quantity", "0"));
                        double dblTax = Convert.ToDouble(DECLARE.Replace(row, "Tax Rate", "0.00"));
                        double dblTaxRate = Convert.ToDouble(1 + "." + dblTax);
                        double dblGrossPrice = Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0.00"));
                        double dblDiscAmt = Convert.ToDouble(DECLARE.Replace(row, "Discount", "0.00"));

                        dblPriceAfterDisc = dblGrossPrice / dblTaxRate;
                    }
                }
            }
            foreach (var x in InvoiceItemsModel.InvoiceItems.Where(x => x.ItemCode == oItemCode))
            {
                x.PriceAfterDisc = Convert.ToDouble(dblPriceAfterDisc.ToString("#,#00.00"));
                dgvItems.Rows[row1].Cells["Line Total"].Value = Convert.ToDouble(dblPriceAfterDisc.ToString("#,#00.00"));
                if (oValueFrom == "GP")
                {
                    x.UnitPrice = Convert.ToDouble(dblPriceAfterDisc.ToString("#,#00.00"));
                    dgvItems.Rows[row1].Cells["Unit Price"].Value = Convert.ToDouble(dblPriceAfterDisc.ToString("#,#00.00"));
                }
            }

        }

        private void RLlinetotal(DataGridViewCellEventArgs e, string oItemCode)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblLineTotal = 0;

            //Line Total
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string ogvItem = row.Cells[0].Value.ToString();
                    if (ogvItem == oItemCode)
                    {
                        double dblQty = Convert.ToDouble(DECLARE.Replace(row, "Quantity", "0"));
                        double dblPrice = Convert.ToDouble(DECLARE.Replace(row, "PriceAfterDisc", "0.00"));
                        double dblDiscAmt = Convert.ToDouble(DECLARE.Replace(row, "Discount", "0.00"));
                        dblLineTotal = dblQty * (dblPrice - dblDiscAmt);
                    }
                }
            }
            foreach (var x in InvoiceItemsModel.InvoiceItems.Where(x => x.ItemCode == oItemCode))
            {
                x.LineTotal = Convert.ToDouble(dblLineTotal.ToString("#,#00.00"));
                dgvItems.Rows[row1].Cells["Line Total"].Value = Convert.ToDouble(dblLineTotal.ToString("#,#00.00"));
            }
        }

        private void RLdiscamount(DataGridViewCellEventArgs e, string oItemCode)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblDiscAmount = 0;

            //Discount Amount
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string ogvItem = row.Cells[0].Value.ToString();
                    if (ogvItem == oItemCode)
                    {
                        double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0.00"));
                        double dblDiscPercent = Convert.ToDouble(DECLARE.Replace(row, "Discount %", "0"));
                        dblDiscAmount = (dblUnitPrice / 100) * dblDiscPercent;
                    }
                }
            }
            foreach (var x in InvoiceItemsModel.InvoiceItems.Where(x => x.ItemCode == oItemCode))
            {
                x.DiscountAmount = Convert.ToDouble(dblDiscAmount.ToString("#,#00.00"));
                dgvItems.Rows[row1].Cells["Discount"].Value = Convert.ToDouble(dblDiscAmount.ToString("#,#00.00"));
            }
        }

        private void RLdiscpercent(DataGridViewCellEventArgs e, string oItemCode)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblDiscPercent = 0;

            //Discount Percent
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string ogvItem = row.Cells[0].Value.ToString();
                    if (ogvItem == oItemCode)
                    {
                        double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0.00"));
                        double dblDiscAmount = Convert.ToDouble(DECLARE.Replace(row, "Discount", "0"));
                        dblDiscPercent = (dblDiscAmount / dblUnitPrice) * 100;
                    }
                }
            }
            foreach (var x in InvoiceItemsModel.InvoiceItems.Where(x => x.ItemCode == oItemCode))
            {
                x.DiscountPerc = Convert.ToDouble(dblDiscPercent.ToString("#,#00.00"));
                dgvItems.Rows[row1].Cells["Discount %"].Value = Convert.ToDouble(dblDiscPercent.ToString("#,#00.00"));
            }
        }

        private void RLgrossprice(DataGridViewCellEventArgs e, string oItemCode)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblCompGrossPrice = 0;

            //Gross Price
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string ogvItem = row.Cells[0].Value.ToString();
                    if (ogvItem == oItemCode)
                    {
                        double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0.00"));
                        double dblDiscPercent = Convert.ToDouble(DECLARE.Replace(row, "Discount %", "0"));
                        double dblDiscAmount = (dblUnitPrice / 100) * dblDiscPercent;
                        double dblTax = Convert.ToDouble(DECLARE.Replace(row, "Tax Rate", "0.00"));
                        double dblTaxRate = Convert.ToDouble(1 + "." + dblTax);

                        dblCompGrossPrice = (dblUnitPrice - dblDiscAmount) * dblTaxRate;
                    }
                }
            }
            foreach (var x in InvoiceItemsModel.InvoiceItems.Where(x => x.ItemCode == oItemCode))
            {
                x.GrossPrice = Convert.ToDouble(dblCompGrossPrice.ToString("#,#00.00"));
                dgvItems.Rows[row1].Cells["Gross Price"].Value = Convert.ToDouble(dblCompGrossPrice.ToString("#,#00.00"));
            }

        }

        private void cbSeries_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var sdata = $"SELECT T0.ObjectCode,T0.Series,T0.SeriesName,T0.NextNumber FROM NNM1 T0 Where T0.ObjectCode = 13 And SeriesName = '{ cbSeries.Text }'";
            var dt = hana.Get(sdata);
            oSeries = helper.ReadDataRow(dt, "Series", "", 0);
            txtDocEntry.Text = DataAccess.Search(dt, 0, "NextNumber"/*, PublicStatic.frmMain*/);
        }

        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dgvItems.CurrentCell.ColumnIndex == 9 || dgvItems.CurrentCell.ColumnIndex == 10 ||
                dgvItems.CurrentCell.ColumnIndex == 11 || dgvItems.CurrentCell.ColumnIndex == 12 ||
                dgvItems.CurrentCell.ColumnIndex == 13 || dgvItems.CurrentCell.ColumnIndex == 14 ||
                dgvItems.CurrentCell.ColumnIndex == 19 || dgvItems.CurrentCell.ColumnIndex == 20 ||
                dgvItems.CurrentCell.ColumnIndex == 21) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true;
            //}
            if (dgvItems.CurrentCell.ColumnIndex != 11)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
            }
            else if (dgvItems.CurrentCell.ColumnIndex == 11)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.ExecuteDeleteItem();
        }

        private void DgvPreviewItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Presenter.DeleteItem(e.RowIndex, e.ColumnIndex);
            }
        }

        private void DgvPreviewItem_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(DgvPreviewItem.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void BtnCopyFrom_Click(object sender, EventArgs e)
        {
            CmbCopyFromOption.DroppedDown = CmbCopyFromOption.DroppedDown == false ? true : false;
        }

        private void CmbCopyFromOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Presenter.ExecuteCopyDocument(CmbCopyFromOption.Text, this);
        }

        private void DgvFindDocument_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvFindDocument.CurrentRow.Selected = true;
        }

        private void SelectDocument_Event(object sender, DataGridViewCellEventArgs e)
        {
            ChooseDocument();
        }

        private void DgvFindDocument_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            findDocSearch = e.ColumnIndex;
        }

        private void FrmARInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MetroMessageBox.Show(this, $"Closing {SystemSettings.Info.Title} will stop all running processes and close all open windows. Do you want to continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    InvoiceItemsModel.InvoiceItems.Clear();
                    InvoiceHeaderModel.InvoiceHeader.Clear();
                    InvoiceHeaderModel.DDWdocentry.Clear();
                    Dispose();
                }
            }
        }

        private void txtDiscPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtDiscPercent_Leave(object sender, EventArgs e)
        {
            //GetDiscPerc();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            //Presenter.ComputeAmountDiscount();
        }

        private void FrmARInvoice_Resize(object sender, EventArgs e)
        {
            //On Comment by Cedi 060719
            this.MdiParent = StaticHelper._MainForm;
            FormHelper.ResizeForm(this);
        }

        private void CmbPrintPreview_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                ReportDocument cryRpt = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                var sboCred = new SboCredentials();
                //string path = $"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\AR\\{CmbPrintPreview.Text}";
                var _settingsService = new SettingsService();

                string path = $"{_settingsService.GetReportPath()}Sales\\{CmbPrintPreview.Text}";

               

                cryRpt.Load(path);
                cryRpt.SetParameterValue("DocKey@", txtDocEntry2.Text);
                cryRpt.SetParameterValue("UserCode@", sboCred.UserId);

                //string constring = $"DRIVER=HDBODBC32;SERVERNODE={sboCred.DbServer};DATABASE={sboCred.Database}";
                //crConnectionInfo.IntegratedSecurity = false;

                //#############################################################################
                //Added by Cedi 070119
                var logonProperties = new DbConnectionAttributes();
               
                logonProperties.Collection.Set("Connection String", @"DRIVER={HDBODBC32};SERVERNODE=" + sboCred.DbServer + "; UID=" + sboCred.DbUserId + ";PWD=" + sboCred.DbPassword + ";CS=" + sboCred.Database + "; ");
                logonProperties.Collection.Set("UseDSNProperties", false);
                var connectionAttributes = new DbConnectionAttributes();
                connectionAttributes.Collection.Set("Database DLL", "crdb_odbc.dll");
                connectionAttributes.Collection.Set("QE_DatabaseName", String.Empty);
                connectionAttributes.Collection.Set("QE_DatabaseType", "ODBC (RDO)");
                connectionAttributes.Collection.Set("QE_LogonProperties", logonProperties);
                connectionAttributes.Collection.Set("QE_ServerDescription", sboCred.DbServer);
                connectionAttributes.Collection.Set("QE_SQLDB", false);
                connectionAttributes.Collection.Set("SSO Enabled", false);
                //#############################################################################

                //crConnectionInfo.ServerName = sboCredentials.DbServer;
                //crConnectionInfo.DatabaseName = sboCredentials.Database;
                //crConnectionInfo.UserID = sboCredentials.DbUserId;
                //crConnectionInfo.Password = sboCredentials.DbPassword;

                crConnectionInfo.Attributes = connectionAttributes;
                crConnectionInfo.Type = ConnectionInfoType.CRQE;
                crConnectionInfo.IntegratedSecurity = false;
                //crConnectionInfo.Type = ConnectionInfoType.SQL;

                foreach (Table CrTable in cryRpt.Database.Tables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;

                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                crystalReportViewer1.ReportSource = cryRpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }

        }

        private void txtDiscPercent_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtDiscPercent.Focused == true)
            {
                GetDiscPerc();
            }
        }

        void GetDiscPerc()
        {
            try
            {
                double result;

                if (double.TryParse(txtTotalBefDisc.Text, out result) == true)
                {
                    if (txtTotalBefDisc.Text != "0.00")
                    {
                        DiscType = "GetAmount";
                        ComputeTotal();
                    }
                    oDiscount = Convert.ToDouble(txtDiscPercent.Text);
                }
            }
            catch (Exception ex)
            {
                txtDiscPercent.Text = "";
            }
        }

        private void txtDiscount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtDiscount.Focused == true)
            {
                GetDiscAmount();
            }
        }

        void GetDiscAmount()
        {
            try
            {
                double result;

                if (double.TryParse(txtTotalBefDisc.Text, out result) == true)
                {
                    if (txtTotalBefDisc.Text != "0.00")
                    {
                        DiscType = "GetPercent";
                        ComputeTotal();
                    }
                    oDiscount = Convert.ToDouble(txtDiscPercent.Text);
                }

            }
            catch (Exception ex)
            {
                txtDiscPercent.Text = "";
            }
        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            //GetDiscAmount();
        }

        private void dtCancelDate_DropDown(object sender, EventArgs e)
        {
            dtCancelDate.Format = DateTimePickerFormat.Short;
            dtCancelDate.Select();
        }

        private void txtDiscPercent_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    double result;
            //    if (double.TryParse(txtTotalBefDisc.Text, out result) == true)
            //    {
            //        if (txtTotalBefDisc.Text != "")
            //        {
            //            ComputeTotal();
            //        }
            //        oDiscount = Convert.ToDouble(txtDiscPercent.Text);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    txtDiscPercent.Text = "";
            //}
        }

        private void DgvUdf_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pbTaxList_Click(object sender, EventArgs e)
        {
            frmSearch2 fS = new frmSearch2();
            fS.oSearchMode = "OVTG";
            frmSearch2.Param1 = "O";
            frmSearch2._title = "List of Tax Group";
            fS.ShowDialog();
            txtTaxGroup.Text = fS.oCode;

            oTaxGroup = fS.oCode;
            InvoiceHeaderModel.oTaxGroup = oTaxGroup;
            if (InvoiceItemsModel.InvoiceItems.Count != 0)
            {
                foreach (var x in InvoiceItemsModel.InvoiceItems)
                {
                    x.TaxCode = oTaxGroup;
                }
            }
            string query = "select Code,Name,cast(Rate as numeric(19,2)) as rate,CAST(EffecDate as date) as effecdate from OVTG where Category = 'O' and Inactive = 'N' and Code = '" + fS.oCode + "'";
            var dt = DataAccess.Select(DataAccess.conStr("HANA"), query);
            if (DataAccess.Exist(dt/*, StaticHelper._MainForm*/) == true)
            {
                oTaxRate = DataAccess.Search(dt, 0, "rate"/*, StaticHelper._MainForm*/);
            }
        }

        private void LoadRAS(string CardCode)
        {
            string selqry = "SELECT distinct ISNULL((ifnull(upper(b.firstName),'') + ' ' + ifnull(upper(b.lastName),'')), '') [FullName] " +
                                                " FROM OCRD a INNER JOIN OHEM b ON a.U_Dim3 = b.CostCenter INNER JOIN OHPS c ON b.position = c.posID " +
                                                $" where a.CardCode = '{CardCode}' and c.name = 'Retail Area Supervs'";
        }


        public void RefreshData()
        {
            try
            {
                SalesStyle.dgvItemsLayout(dgvItems);
                SalesStyle.dgvItemsLayout(DgvPreviewItem);
                foreach (var x in InvoiceItemsModel.InvoiceItems.Where(x => x.ObjType == objType))
                {
                    object[] a = { x.ItemCode, x.ItemName ,x.Brand, x.Style, x.Color, x.Size, x.Section, x.BarCode, x.EffectivePrice,x.GrossPrice.ToString("0.00"),
                                   x.UnitPrice.ToString("0.00"), x.Quantity, x.DiscountPerc.ToString("0.00"), x.DiscountAmount, x.EmpDiscountPerc.ToString("0.00") ,
                                   x.FWhsCode, "...", x.TaxCode, "...", x.TaxRate.ToString("0.00"), x.LineTotalManual.ToString("0.00"), x.GrossTotal.ToString("0.00"),
                                   x.PriceAfterDisc.ToString("0.00"), x.Linenum, "...", x.Company };
                    dgvItems.Rows.Add(a);
                }

                SalesStyle.dataGridLayout(dgvItems);
                SalesStyle.dgvItemsLayout(DgvPreviewItem);
                ComputeTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void ComputeTotal()
        {
            try
            {
                var dDiscPercent = 0.0;
                var dDiscAmount = 0.0;
                var dTotalQty = 0.0;
                var dTotalTax = 0.0;
                var dBefPrice = 0.0;
                var dTotalGroPrice = 0.0;
               
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                  
                    string sTaxRate = "." + DECLARE.Replace(row, "Tax Rate", "0");
                    

                    var dTaxRate = double.Parse(sTaxRate.Contains(".00") ? sTaxRate.Replace(".00", "") : sTaxRate);
                    //var dTaxRate = double.Parse(sTaxRate);
                  
                    //On comment due to carton qty 092419
                    //var dQty = double.Parse(DECLARE.Replace(row, "Quantity", "0.00"));
                    var dQty = double.Parse(DECLARE.Replace(row, "Quantity", "0.00"));
                
                    var dPriceAfterDisc = double.Parse(DECLARE.Replace(row, "PriceAfterDisc", "0.00"));
                 

                    var dGroPrice = dQty * double.Parse(DECLARE.Replace(row, "Gross Price", "0.00"));

                  

                    double dLineTotal = double.Parse(DECLARE.Replace(row, "Line Total", "0.00"));

                    
                    //double dTotal = double.Parse(DECLARE.Replace(row, "Total", "0.00"));

                  
                    //Correct Computation is Qty * (Price * Tax) 11/21/19
                    var dCompTax = dQty * (dPriceAfterDisc * dTaxRate);
                    //var dCompTax = dQty * (dLineTotal * dTaxRate);

                    dTotalQty += dQty;
                    //dBefPrice += (Convert.ToDouble(dPriceAfterDisc) * Convert.ToDouble(dQty));
                    dBefPrice += dLineTotal;
                    //dBefPrice += dTotal;
                    dTotalTax += dCompTax;
                    dTotalGroPrice += dGroPrice;
                }

                //double checkDiscPercent = txtDiscPercent.Text != string.Empty ? (dbleTotaLT - dbleTotalDisc) * (dbleDiscPercent / 100) : 0;
                if (txtDiscPercent.Text != "")
                {
                    dDiscPercent = double.Parse(txtDiscPercent.Text);
                }
                if (txtDiscount.Text != "")
                {
                    dDiscAmount = double.Parse(txtDiscount.Text);
                }

                double checkDiscPercent = txtDiscPercent.Text != string.Empty ? (dBefPrice / 100) * dDiscPercent : 0;
                double GetDiscPercent = txtDiscount.Text != string.Empty ? (dDiscAmount / dBefPrice) * 100 : 0;

                if (DiscType != null && DiscType == "GetAmount" && txtDiscPercent.Focus() == true)
                {
                    txtDiscount.Text = Math.Round(checkDiscPercent, 3).ToString("#,##0.00");
                }
                else if (DiscType != null && DiscType == "GetPercent" && txtDiscount.Focus() == true)
                {
                    txtDiscPercent.Text = Math.Round(GetDiscPercent, 3).ToString("#,##0.00");
                }

                if (txtDiscPercent.Text != "")
                {
                    var d_DiscPerc = double.Parse(txtDiscPercent.Text) / 100;
                    var taxpercent = (dTotalTax * d_DiscPerc);
                    dTotalTax = dTotalTax - taxpercent;
                    dTotalGroPrice = dTotalGroPrice - (dTotalGroPrice * d_DiscPerc);
                }
       

                txtTotalQty.Text = Math.Round(dTotalQty, 3).ToString("#,##0.00");
                txtTotalBefDisc.Text = Math.Round(dBefPrice, 3).ToString("#,##0.00");
                txtTaxAmount.Text = Math.Round(dTotalTax, 3).ToString("#,##0.00");
                txtTotal.Text = Math.Round(dTotalGroPrice, 3).ToString("#,##0.00");
              

                //txtDiscPercent.Text = Math.Round(checkDiscPercent, 3).ToString("#,#00.00");
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
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
    }
}
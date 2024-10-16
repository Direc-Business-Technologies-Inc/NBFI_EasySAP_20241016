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
using DomainLayer.Models;
using MetroFramework;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DomainLayer.Helper;
using System.IO;
using PresenterLayer.Services.Security;

namespace PresenterLayer.Views
{
    public partial class FrmSalesOrder : MetroForm, IFrmSalesOrder
    {
        public MainForm frmMain { get; set; }
        SalesAR_generics _generics = new SalesAR_generics();
        SalesRepository _repository = new SalesRepository();
        private SOmaintenance SOm = new SOmaintenance();
        public static bool FindMode = false;
        int max_width = Screen.PrimaryScreen.Bounds.Width - 320;
        int max_height = Screen.PrimaryScreen.Bounds.Height - 250;
        private static string oSeries, oProject, oCode, oName, oAddressCode, oFindStat;
        public static string oPriceList;
        public static string oTaxGroup;
        public static string oTaxRate;
        public static int datetimeCount = 0;
        public static int xdocentry;
        public static string objType = "ORDR";
        public bool fromAR = false;
        public static string oFWhsCode, oTWhsCode;
        public static string oItmsGrpCod { get; set; }
        public static string oOutRight { get; set; }

        private static double oDiscount;
        private static string DiscType;
        private static bool CalcDisc = false;
        private string Vat { get; set; }
        private double VatRate { get; set; }
        private string DefaultWarehouse { get; set; }
        private string table { get; set; }
        private string RawCurr { get; set; }
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        private DataTable CurrentDataSet { get; set; }
        private int findDocSearch;
        public static string _DocEntry;

        SettingsService _settingsService { get; set; }

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
            cmbLogShipTo.SelectedIndex = cmbLogShipTo.FindString("Ship To");

            var files = Presenter.GetDocumentCrystalForms();
            if (files != null)
            {
                foreach (FileInfo file in files)
                {
                    CmbPrintPreview.Items.Add(file.Name);
                }
            }

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
                    DgvUdf.Rows[row.Index].Cells["Field"].Value = DomainLayer.Models.EasySAPCredentialsModel.ESUserId;
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

                case Keys.Control | Keys.I:
                    Presenter.DisplayItemList(cbDocumentType.Text);
                    break;

                case Keys.Escape:
                    Close();
                    break;  
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


        public string DocNum
        {
            get => txtDocEntry.Text;
            set => txtDocEntry.Text = value;
        }

        public string Series
        {
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
            get => txtCustomRefNo.Text;
            set => txtCustomRefNo.Text = value;
        }

        public string Company
        {
            get => Convert.ToString(cbCompany.SelectedValue);
            set => cbCompany.Text = value;
        }
        public string BPCode
        {
            get => txtBpCode.Text;
            set => txtBpCode.Text = value;
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
            get => cbDocumentType.Text;
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

        public ContextMenuStrip MsItems
        {
            get => msItems;
        }

        public string RawCurrency
        {
            get => RawCurr;
            set => RawCurr = value;
        }

        public string oSalesEmployee
        {
            get => txtSalesEmpCode.Text;
            set => txtSalesEmpCode.Text = value;
        }

        public string oSalesEmployeeName
        {
            get => txtSalesEmployee.Text;
            set => txtSalesEmployee.Text = value;
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

        //private SO_UDF SOu = new SO_UDF();
        //SO_UDF frmUDF;
        public FrmSalesOrder()
        {
            InitializeComponent();
            // LocationChanged += new EventHandler(Form1_LocationChanged);
            frmMain = StaticHelper._MainForm;
            hana = new SAPHanaAccess();
            helper = new DataHelper();
            _settingsService = new SettingsService();
        }


        private void LoadDocumentType()
        {
            try
            {
                var sdata = "SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [@DOC_TYPE] Order by Name";
                var dt = hana.Get(sdata);
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
                var sdata = "SELECT SeriesName FROM NNM1 Where ObjectCode = 17";
                var dt = hana.Get(sdata);

                Application.DoEvents();
                cbSeries.DisplayMember = "SeriesName";
                cbSeries.DataSource = dt;
                txtDocStatus.Text = "Open";

                var sdata1 = $"SELECT T0.ObjectCode,T0.Series,T0.SeriesName,T0.NextNumber FROM NNM1 T0 Where T0.ObjectCode = 17 Where T0.SeriesName = '{ cbSeries.Text }'";
                var dt1 = hana.Get(sdata1);

                if (dt1.Rows.Count > 0)
                {
                    cbSeries.SelectedIndex = 0;

                    oSeries = helper.ReadDataRow(dt1, "Series", "", 0);
                    txtDocEntry.Text = helper.ReadDataRow(dt1, "NextNumber", "", 0);
                }

            }
            catch (Exception ex) { }
        }

        private void ClearList()
        {
            //CLEAR LIST DATA
            SalesOrderItemsModel.SalesOrderItems.Clear();
            DECLARE._DocHeader.RemoveAll(x => x.ObjType == objType);
            //SalesOrderItemsModel.SalesOrderItems.RemoveAll(x => x.ObjType == objType);
            // SalesOrderItemsController.oRAS = "";
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

            var headerDetails = hana.Get(query);

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
            dtItems = hana.Get(queryItems);

            for (int x = 0; x < dtItems.Rows.Count; x++)
            {
                double LineTotal;
                double GrossTotal;
                double VatAmount;
                double GrossPrice;
                double PriceVatInc;
                double DiscAmt;
                double Discount;

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

                var dt = hana.Get(string.Format(helper.ReadDataRow(hana.Get(SP.AW_GetItemDetails), 1, "", 0), dtItems.Rows[x]["ItemCode"].ToString()));
                SalesOrderItemsModel.SalesOrderItems.Add(new SalesOrderItemsModel.SalesOrderItemsData
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

                var strGetEPqry = "select T1.CardCode " +
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

                var dtItem = hana.Get(strGetEPqry);
                if (helper.DataTableExist(dtItem))
                {
                    GetEP = Convert.ToDouble(helper.ReadDataRow(dtItem, "EffectivePrice", "", 0));
                }

                return GetEP;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                return GetEP;
            }

        }


        public SalesOrderService Presenter
        {
            private get;
            set;
        }

        private void LoadCompany()
        {
            try
            {
                var sdata = "select '' [Code], '' [Name] UNION select Code, Name from [@CMP_INFO]";
                var dt = hana.Get(sdata);

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
            SalesOrdersHeaderModel.oBPCode = "";

            if (oCode != null)
            {
                txtBpCode.Text = oCode;
                SalesOrdersHeaderModel.oBPCode = oCode;
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
                SalesOrdersHeaderModel.oDocDate = Convert.ToDateTime(dtPostingDate.Text);
                SalesOrdersHeaderModel.oDocType = cbDocumentType.Text;
                FrmSalesOrderItemList so = new FrmSalesOrderItemList(this, StaticHelper._MainForm);
                so.ShowDialog();
                ComputeTotal();
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
            SalesOrdersHeaderModel.oWhsCode = oFWhsCode;
        }

        private void LoadBPDetails(string CardCode, bool FromFind)
        {
            var query = $"SELECT A.CardCode" +
                ",A.CardName" +
                ",A.MailAddres [Address]" +
                ",(SELECT Z.Address FROM CRD1 Z Where Z.CardCode = A.CardCode And Z.Street = A.MailAddres  And Z.AdresType = 'S') [AddressCode]" +
                ",A.ShipToDef [AddressID]" +
                ",A.ProjectCod" +
                ",A.SlpCode" +
                ",A.ListNum" +
                ",(Select Z.SlpName FROM OSLP Z Where Z.SlpCode = A.SlpCode) [SlpName]" +
                $",ECVatGroup FROM OCRD A WHERE A.CardCode = '{ CardCode }'";

            var dt = hana.Get(query);

            if (dt.Rows.Count > 0)
            {
                txtBpName.Text = DECLARE.dtNull(dt, 0, "CardName", "");
                txtAddress.Text = DECLARE.dtNull(dt, 0, "AddressID", "");
                oSalesEmployee = DECLARE.dtNull(dt, 0, "SlpCode", "");
                txtSalesEmployee.Text = DECLARE.dtNull(dt, 0, "SlpName", "");
                //oPriceList = DECLARE.dtNull(dt, 0, "ListNum", "0");
                SalesOrdersHeaderModel.oPriceList = oPriceList;
                txtBpName.Text = DECLARE.dtNull(dt, 0, "CardName", "");
                oTaxGroup = DECLARE.dtNull(dt, 0, "ECVatGroup", "");
                oProject = DECLARE.dtNull(dt, 0, "ProjectCod", "");
                oAddressCode = DECLARE.dtNull(dt, 0, "AddressCode", "");
                //oFWhsCode = DataAccess.SearchData(DataAccess.conStr("HANA"), $"Select U_Whs FROM CRD1 Where CardCode = '{CardCode}'", 0, "U_Whs", frmMain);
                //txtPriceList.Text = oPriceList;
                txtTaxGroup.Text = oTaxGroup;
                SalesOrdersHeaderModel.oTaxGroup = oTaxGroup;
                //txtWhsCode.Text = oFWhsCode;

                string query1 = "Select Code,Name,Rate From OVTG Where Code = '" + oTaxGroup + "'";

                var dt1 = new DataTable();

                dt1 = hana.Get(query1);

                if (dt1.Rows.Count > 0)
                {
                    oTaxRate = DECLARE.dtNull(dt1, 0, "rate", "");
                }

                if (FromFind == false)
                {
                    LoadRAS(CardCode);
                }
                foreach (DataGridViewRow row in DgvUdf.Rows)
                {
                    if (row.Cells["Code"].Value.ToString() == "U_RAS")
                    {
                        DgvUdf.Rows[row.Index].Cells["Field"].Value = SalesOrderItemsController.oRAS;
                        DgvUdf.Rows[row.Index].Cells["Field"].ReadOnly = true;
                        break;
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvItems.Rows.Count > 0)
                {
                    if (txtBpCode.Text != string.Empty)
                    {
                        btnAdd.Enabled = false;
                        if (Presenter.ExecuteRequest(btnAdd.Text))
                        {
                            ClearData();
                            btnAdd.Text = btnAdd.Text == "Update" ? "Add" : "Add";
                        }
                        btnAdd.Enabled = true;
                        pbBPList.Visible = true;
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Please select an item(s) before adding.", true);
                }
            }
            catch(Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private void dtCancelDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ViewList("OSLP", out oCode, out oName, "List of Sales Employees");

            if (oCode != null)
            {
                txtSalesEmployee.Text = oName;
                oSalesEmployee = oCode;
                oSalesEmployeeName = oName;
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
                ClearData();
                TabSO.SelectedIndex = 0;
                btnAdd.Text = "Add";
                pbBPList.Visible = true;
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

        private void DgvPreviewItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex - 1;
            colIndex = colIndex < 0 ? 0 : colIndex;

            var dgv = (DataGridView)sender;
            //var itemCode = dgv.Rows[e.RowIndex].Cells["Item No."].Value.ToString();
            //var LineNum = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["LineNum"].Value.ToString());
            //var fieldname = dgv.Columns[e.ColumnIndex].Name;

            string columnName = DgvPreviewItem.Columns[colIndex].Name;
            int index = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["LineNum"].Value.ToString());

            switch (columnName)
            {
                case "Warehouse":

                    Presenter.GetWarehouse(colIndex, index);
                    break;

                case "Tax":

                    Presenter.GetTaxCode(e, colIndex, index);
                    ComputeTotal();
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

                    if (dgvItems.Rows.Count > 0)
                    {
                        Presenter.LoadData(dgvItems);

                        if (cbDocumentType.Text == "Service" && btnAdd.Text == "Update")
                        {
                            dgvItems.Rows.RemoveAt(0);
                        }
                    }
 
                    break;

                case 1:

                    Presenter.LoadData(DgvPreviewItem);

                    if (cbDocumentType.Text == "Service" && btnAdd.Text == "Update")
                    {
                        DgvPreviewItem.Rows.RemoveAt(0);
                    }
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
            LineComputation(sender, e);
        }

        private void LineComputation(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;
            var fieldname = dgv.Columns[e.ColumnIndex].Name;

            if (fieldname.Equals("Quantity") ||
                fieldname.Equals("Gross Price") ||
                fieldname.Equals("Unit Price") ||
                fieldname.Equals("Discount %") ||
                fieldname.Equals("Discount"))
            {
                var validNumber = dgv.Rows[e.RowIndex].Cells[fieldname].Value;

                if (IsNumeric(validNumber))
                {
                    var itemCode = dgv.Rows[e.RowIndex].Cells["Item No."].Value.ToString();
                    var LineNum = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["LineNum"].Value.ToString());
                    var val = double.Parse(validNumber.ToString());

                    var row = dgv.Rows[e.RowIndex];

                    double OrdrQty = val; //Requested to remove logic 07212021 //SalesOrderItemsController.GetCartonQty(itemCode, val);

                    foreach (var x in SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode == itemCode && x.Linenum == LineNum))
                    {
                        if (fieldname.Equals("Quantity"))
                        {
                            dgv.Rows[e.RowIndex].Cells[fieldname].Value = val;
                            dgv.Rows[e.RowIndex].Cells["Ordered Qty"].Value = OrdrQty;
                        }
                        else
                        {
                            dgv.Rows[e.RowIndex].Cells[fieldname].Value = Math.Round(val, 3).ToString("#,#00.00");
                            //dgv.Rows[e.RowIndex].Cells["Ordered Qty"].Value = OrdrQty;
                        }

                        switch (fieldname)
                        {
                            case "Gross Price":
                                x.GrossPrice = val;

                                x.DiscountAmount = 0;
                                dgv.Rows[e.RowIndex].Cells["Discount"].Value = 0;
                                x.DiscountPerc = 0;
                                dgv.Rows[e.RowIndex].Cells["Discount %"].Value = 0;

                                x.UnitPrice = ComputeUnitPrice(row, val);

                                var GpriceAfterDisc = ComputePriceAfterDisc(row, val);
                                x.PriceAfterDisc = GpriceAfterDisc;

                                x.LineTotal = ComputeLineTotal(row, GpriceAfterDisc, 0);

                                x.GrossTotal = ComputeGrossTotal(row, val);

                                break;
                            case "Unit Price":
                                x.UnitPrice = val;

                                var UdiscountAmount = ComputeDiscountAmount(row, val);
                                x.DiscountAmount = UdiscountAmount;

                                var UgrossPrice = ComputeGrossPrice(row, val, UdiscountAmount);
                                x.GrossPrice = UgrossPrice;

                                var UpriceAfterDisc = ComputePriceAfterDisc(row, UgrossPrice);
                                x.PriceAfterDisc = UpriceAfterDisc;

                                x.LineTotal = ComputeLineTotal(row, UpriceAfterDisc, UdiscountAmount);

                                x.GrossTotal = ComputeGrossTotal(row, UgrossPrice);

                                break;
                            case "Quantity":
                                //On comment due to Carton Qty 092419
                                //x.Quantity = val;
                                x.Quantity = OrdrQty;
                                

                                var QUnitPrice = double.Parse(DECLARE.Replace(row, "Unit Price", "0"));
                                var QdiscountAmount = ComputeDiscountAmount(row, QUnitPrice);
                                var QgrossPrice = ComputeGrossPrice(row, QUnitPrice, QdiscountAmount);
                                var QpriceAfterDisc = ComputePriceAfterDisc(row, QgrossPrice);

                                x.LineTotal = ComputeLineTotal(row, QpriceAfterDisc, QdiscountAmount);
                                x.GrossTotal = ComputeGrossTotal(row, QgrossPrice);

                                break;
                            case "Discount %":
                                x.DiscountPerc = val;

                                var PUnitPrice = double.Parse(DECLARE.Replace(row, "Unit Price", "0"));
                                var PdiscountAmount = ComputeDiscountAmount(row, PUnitPrice);
                                x.DiscountAmount = PdiscountAmount;

                                var PgrossPrice = ComputeGrossPrice(row, PUnitPrice, PdiscountAmount);
                                x.GrossPrice = PgrossPrice;

                                var PpriceAfterDisc = ComputePriceAfterDisc(row, PgrossPrice);
                                x.PriceAfterDisc = PpriceAfterDisc;
                                x.LineTotal = ComputeLineTotal(row, PpriceAfterDisc, PdiscountAmount);
                                x.GrossTotal = ComputeGrossTotal(row, PgrossPrice);

                                break;
                            case "Discount":
                                x.DiscountAmount = val;

                                var DUnitPrice = double.Parse(DECLARE.Replace(row, "Unit Price", "0"));
                                var DgrossPrice = ComputeGrossPrice(row, DUnitPrice, val);
                                x.GrossPrice = DgrossPrice;

                                var DdiscountPercentage = ComputeDiscountPercentage(row, val, DUnitPrice);
                                x.DiscountPerc = DdiscountPercentage;

                                var DpriceAfterDisc = ComputePriceAfterDisc(row, DgrossPrice);
                                x.PriceAfterDisc = DpriceAfterDisc;
                                x.LineTotal = ComputeLineTotal(row, DpriceAfterDisc, val);
                                x.GrossTotal = ComputeGrossTotal(row, DgrossPrice);

                                //Added UnitPrice 12/13/19
                                //x.UnitPrice = ComputeUnitPrice(row, DgrossPrice);
                                break;
                            default:
                                break;
                        }
                    }

                    ComputeTotal();
                }
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


        private void msbtnDelete_Click(object sender, EventArgs e)
        {
            //var result = MessageBox.Show("Are you sure you want to remove selected item?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (result == DialogResult.Yes)
            //{
            foreach (DataGridViewRow row2 in dgvItems.Rows)
            {
                if (row2.Selected == true)
                {
                    int row = row2.Index;
                    string itemCode = dgvItems.Rows[row].Cells[0].Value.ToString();
                    int id = Convert.ToInt32(dgvItems.Rows[row].Cells["LineNum"].Value.ToString());

                    SalesOrderItemsModel.SalesOrderItems.RemoveAll(x => x.ItemCode == itemCode && x.Linenum == id);

                }
            }

            //UpdateTotal();
            RefreshData();
            //}
        }

        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvItems.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }


        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var row = ((DataGridView)sender).Rows[e.RowIndex];
            var fieldName = dgvItems.Columns[e.ColumnIndex].Name;

            if (fieldName.Equals("Tax Rate"))
            {
                var itemcode = dgvItems.Rows[e.RowIndex].Cells["Item No."].Value.ToString();
                var LineNum = dgvItems.Rows[e.RowIndex].Cells["LineNum"].Value.ToString();
                var SOList = SalesOrderItemsModel.SalesOrderItems.First(x => x.ItemCode == itemcode && LineNum == LineNum.ToString());
                //var row = dgvItems.Rows[e.RowIndex];

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

        double ComputeDiscountAmount(DataGridViewRow row, double dUnitPrice)
        {
            var output = 0.0;
            var discountPercentage = double.Parse(DECLARE.Replace(row, "Discount %", "0"));
            output = (dUnitPrice / 100) * discountPercentage;

            row.Cells["Discount"].Value = Math.Round(output, 2).ToString("#,#00.00");
            return output;
        }

        double ComputeUnitPrice(DataGridViewRow row, double dGrossPrice)
        {
            var output = 0.0;
            var taxAmount = 1 + (double.Parse(DECLARE.Replace(row, "Tax Rate", "0.00")) / 100);
            output = dGrossPrice / taxAmount;

            row.Cells["Unit Price"].Value = Math.Round(output, 2).ToString("#,#00.00");
            return output;
        }

        double ComputeGrossPrice(DataGridViewRow row, double dUnitPrice, double dDiscountAmount)
        {
            var output = 0.0;
            var taxAmount = 1 + (double.Parse(DECLARE.Replace(row, "Tax Rate", "0.00")) / 100);
            output = Math.Round(((dUnitPrice - (dDiscountAmount / taxAmount)) * taxAmount), 2);

            row.Cells["Gross Price"].Value = output.ToString("#,#00.00");
            return output;
        }

        double ComputeDiscountPercentage(DataGridViewRow row, double dDiscountAmount, double dUnitPrice)
        {
            var output = 0.0;

            var taxAmount = 1 + (double.Parse(DECLARE.Replace(row, "Tax Rate", "0.00")) / 100);
            output = (Math.Round((dDiscountAmount / taxAmount), 2) / dUnitPrice) * 100;

            row.Cells["Discount %"].Value = Math.Round(output, 2).ToString("#,#00.00");
            return output;

        }

        double ComputePriceAfterDisc(DataGridViewRow row, double dGrossPrice)
        {
            var output = 0.0;
            var taxAmount = 1 + (double.Parse(DECLARE.Replace(row, "Tax Rate", "0.00")) / 100);
            output = dGrossPrice / taxAmount;
            row.Cells["PriceAfterDisc"].Value = Math.Round(output, 2).ToString("#,#00.00");
            return output;
        }

        double ComputeLineTotal(DataGridViewRow row, double dPriceAfterDisc, double dDiscountAmount)
        {
            var output = 0.0;
            //On Comment due to Carton Qty 092419
            //var quantity = double.Parse(DECLARE.Replace(row, "Quantity", "0"));
            var quantity = double.Parse(DECLARE.Replace(row, "Ordered Qty", "0"));

            //output = quantity * (dPriceAfterDisc - dDiscountAmount);
            output = quantity * (dPriceAfterDisc);

            row.Cells["Line Total"].Value = Math.Round(output, 2).ToString("#,#00.00");
            return output;
        }

        double ComputeGrossTotal(DataGridViewRow row, double dGrossPrice)
        {
            var output = 0.0;
            //On Comment due to Carton Qty 092419
            //var quantity = double.Parse(DECLARE.Replace(row, "Quantity", "0"));
            var quantity = double.Parse(DECLARE.Replace(row, "Ordered Qty", "0"));
            output = dGrossPrice * quantity;

            row.Cells["Gross Total"].Value = Math.Round(output, 2).ToString("#,#00.00");
            return output;
        }

        private void dgvItems_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var col = e.ColumnIndex;
                var row = e.RowIndex;

                if (e.ColumnIndex == 18 && e.RowIndex >= 0)
                {
                    var fS = new frmSearch2();
                    fS.oSearchMode = "OVTG";
                    frmSearch2.Param1 = "O";
                    fS.ShowDialog();

                    Int32 intRate = Convert.ToInt32(fS.oName.Replace(".000000", ""));
                    dgvItems.Rows[row].Cells["Tax"].Value = fS.oCode;
                    dgvItems.Rows[row].Cells["Tax Rate"].Value = intRate;

                    var strItemcode = dgvItems.Rows[row].Cells[0].Value.ToString();
                    var strStyle = dgvItems.Rows[row].Cells[3].Value.ToString();
                    var strColor = dgvItems.Rows[row].Cells[4].Value.ToString();
                    var strSize = dgvItems.Rows[row].Cells[5].Value.ToString();
                    var strBarcode = dgvItems.Rows[row].Cells[7].Value.ToString();

                    //SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode == strItemcode && x.Style == strStyle && x.Color == strColor && x.Size == strSize && x.BarCode == strBarcode).First().TaxCode = fS.oCode;
                    SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode == strItemcode && x.BarCode == strBarcode).First().TaxCode = fS.oCode;
                    SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode == strItemcode && x.BarCode == strBarcode).First().TaxRate = intRate;
                    ComputeTotal();
                }

                if (e.ColumnIndex == 16 && e.RowIndex >= 0)
                {
                    ViewList("OWHS", out oCode, out oName, "");

                    dgvItems.Rows[row].Cells["Warehouse"].Value = oCode;
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
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
            dgvItems.Rows.Clear();
            dgvItems.DataSource = null;
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
            //txtDocEntry.Focus();
        }

        void Cancel()
        {
            if (DECLARE._DocItems.Where(x => x.ObjType == objType).ToList().Count > 0)
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the Document? Unsaved data will be lost.", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ClearList();
                    Dispose();
                }
            }
            else
            {

                Dispose();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Cancel();
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close this Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ClearData();
                Dispose();
            }
        }

        private void BtnChoose_Click(object sender, EventArgs e)
        {
            ChooseDocument();
            //ComputeTotal();
        }

        private void ChooseDocument()
        {
            var itemNo = DgvFindDocument.CurrentRow.Cells[0].Value;

            if (itemNo != null)
            {
                ClearData();
                string status = DgvFindDocument.CurrentRow.Cells[1].Value.ToString();
                Presenter.ClearField(true);
                Presenter.GetSelectedDocument(table, itemNo.ToString(), status);
                TabSO.SelectedIndex = 0;
                ComputeTotal();
                btnAdd.Text = "Update";
                pbBPList.Visible = false;
            }

            if (txtDocStatus.Text.Contains("Closed"))
            {
                btnAdd.Text = "Add";
                btnAdd.Enabled = false;
            }
            else
            {
                btnAdd.Enabled = true;
            }
        }

        private void cbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sdata = $"SELECT T0.ObjectCode,T0.Series,T0.SeriesName,T0.NextNumber FROM NNM1 T0 Where T0.ObjectCode = 17 And SeriesName = '{ cbSeries.Text }'";
            
            var dt = hana.Get(sdata);
           
            oSeries = helper.ReadDataRow(dt, "Series", "", 0);
            txtDocEntry.Text = helper.ReadDataRow(dt, "NextNumber", "", 0);
        }
        
        private void cbDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckDocumentTypeMaintenance();
        }


        private void CheckDocumentTypeMaintenance()
        {
            try
            {
                if (SOm.SelValue("series1", cbDocumentType.Text) != "")
                {
                    cbSeries.SelectedIndex = cbSeries.FindString(SOm.SelValue("series1", cbDocumentType.Text));
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
                    ReloadDiscount("Y");
                }
                else if (cbDocumentType.Text == "Outright Order")
                {
                    oItmsGrpCod = "";
                    oOutRight = "Y";
                    ReloadDiscount("N");
                }
                else
                {
                    oItmsGrpCod = "";
                    oOutRight = "";
                    ReloadDiscount("N");
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
                        dblDiscPerc = Convert.ToDouble(helper.ReadDataRow(hana.Get("select ISNULL(100 - (Factor*100),0) [Discount] from OPLN where ListNum = '3' "), "Discount", "", 0));
                    }

                    foreach (DataGridViewRow row in dgvItems.Rows)
                    {
                        if (row.Cells["Item No."].Value != null)
                        {
                            dgvItems.Rows[iLineNum].Cells[10].Value = dblDiscPerc;
                            var dgvcecnt = new DataGridViewCellEventArgs(10, iLineNum);
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
                if (dgvItems.Rows[row1].Cells[0].Value != null)
                {
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
                        double dblTaxRate = Convert.ToDouble(1 + "." + DECLARE.Replace(row, "Tax Rate", "0.00"));
                        double dblGrossPrice = Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0.00"));
                        double dblDiscAmt = Convert.ToDouble(DECLARE.Replace(row, "Discount", "0.00"));

                        dblPriceAfterDisc = dblGrossPrice / dblTaxRate;
                    }
                }
                    
            }
            foreach (var x in SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode == oItemCode))
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
            foreach (var x in SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode == oItemCode))
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
            foreach (var x in SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode == oItemCode))
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
            foreach (var x in SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode == oItemCode))
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
                        double dblTaxRate = Convert.ToDouble(1 + "." + DECLARE.Replace(row, "Tax Rate", "0.00"));

                        dblCompGrossPrice = (dblUnitPrice - dblDiscAmount) * dblTaxRate;
                    }
                }
            }
            foreach (var x in SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode == oItemCode))
            {
                x.GrossPrice = Convert.ToDouble(dblCompGrossPrice.ToString("#,#00.00"));
                dgvItems.Rows[row1].Cells["Gross Price"].Value = Convert.ToDouble(dblCompGrossPrice.ToString("#,#00.00"));
            }

        }

        private void FrmSalesOrder_Resize(object sender, EventArgs e)
        {
            this.MdiParent = StaticHelper._MainForm;
            FormHelper.ResizeForm(this);
        }

        private void dtCancelDate_DropDown(object sender, EventArgs e)
        {
            dtCancelDate.Format = DateTimePickerFormat.Short;
            dtCancelDate.Select();
        }

        private void FrmSalesOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close this Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ClearData();
                Dispose();
            }
            else
            { e.Cancel = true; }
            
        }

        private void DgvFindDocument_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvFindDocument.CurrentRow.Selected = true;
        }

        private void DgvFindDocument_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ChooseDocument();
        }

        private void txtWhsCode_TextChanged(object sender, EventArgs e)
        {
            if (txtWhsCode.Text != "")
            {
                SalesOrdersHeaderModel.oWhsCode = txtWhsCode.Text;
            }
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells["Item No."].Value != null)
                {
                    row.Cells["Warehouse"].Value = txtWhsCode.Text;
                }
            }
            foreach (var x in SalesOrderItemsModel.SalesOrderItems.Where(x => x.ItemCode != "").ToList())
            {
                x.FWhsCode = txtWhsCode.Text;
            }
        }

        private void msItems_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDiscount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtDiscount.Focused == true)
            {
                ComputeDiscPerc();
            }
        }

        void ComputeDiscPerc()
        {
            try
            {
                double result;

                if (double.TryParse(txtDiscount.Text, out result) == true)
                {
                    if (txtDiscount.Text != "")
                    {
                        DiscType = "GetPercent";
                        ComputeTotal();
                    }
                    oDiscount = Convert.ToDouble(txtTotalBefDisc.Text);
                }

            }
            catch (Exception ex)
            {
                txtTotalBefDisc.Text = "";
            }
        }

        private void txtDiscPercent_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtDiscPercent.Focused == true)
            {
                ComputeDiscAmount();
            }  
        }

        void ComputeDiscAmount()
        {
            try
            {
                double result;

                if (double.TryParse(txtTotalBefDisc.Text, out result) == true)
                {
                    if (txtTotalBefDisc.Text != "")
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

        private void txtDiscPercent_Leave(object sender, EventArgs e)
        {
            //ComputeDiscAmount();
        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            //ComputeDiscPerc();
        }

        private void txtCustomRefNo_TextChanged(object sender, EventArgs e)
        {
            ContactPerson = txtCustomRefNo.Text;
        }

        private void cbSeries_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var sdata = $"SELECT T0.ObjectCode,T0.Series,T0.SeriesName,T0.NextNumber FROM NNM1 T0 Where T0.ObjectCode = 17 And SeriesName = '{ cbSeries.Text }'";
            var dt = hana.Get(sdata);
            oSeries = helper.ReadDataRow(dt, "Series", "", 0);
            txtDocEntry.Text = helper.ReadDataRow(dt, "NextNumber", "", 0); 
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void btnClearDate_Click(object sender, EventArgs e)
        {
            dtCancelDate.CustomFormat = " ";
            dtCancelDate.Format = DateTimePickerFormat.Custom;
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
                string path = $"{_settingsService.GetReportPath()}\\Sales\\{CmbPrintPreview.Text}";

                cryRpt.Load(path);
                cryRpt.SetParameterValue("DocKey@", txtDocEntry2.Text);
                cryRpt.SetParameterValue("UserCode@", sboCred.UserId);

                //string constring = $"DRIVER=HDBODBC32;SERVERNODE={sboCred.DbServer};DATABASE={sboCred.Database}";
                //crConnectionInfo.IntegratedSecurity = false;

                //#############################################################################
                //Added by Cedi 070119
                var logonProperties = new DbConnectionAttributes();
                logonProperties.Collection.Set("Connection String", @"DRIVER={HDBODBC32};SERVERNODE=" + sboCred.DbServer + "; UID=" + sboCred.DbUserId + ";PWD=" + sboCred.DbPassword + ";CS=" + sboCred.Database + "; ");
                //logonProperties.Collection.Set("Connection String", @"DRIVER ={B1CRHPROXY32}; SERVERNODE = HANASERVERNBFI:30015; DATABASE = " + sboCred.Database + "");
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

                //crConnectionInfo.ServerName = sboCred.DbServer;
                //crConnectionInfo.DatabaseName = sboCred.Database;
                //crConnectionInfo.UserID = sboCred.DbUserId;
                //crConnectionInfo.Password = sboCred.DbPassword;

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

        private void DgvFindDocument_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            findDocSearch = e.ColumnIndex;
        }

        private void DgvPreviewItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            LineComputation(sender, e);
        }

        private void pbTaxList_Click(object sender, EventArgs e)
        {
            var fS = new frmSearch2();
            fS.oSearchMode = "OVTG";
            frmSearch2.Param1 = "O";
            frmSearch2._title = "List of Tax Group";
            fS.ShowDialog();
            txtTaxGroup.Text = fS.oCode;

            oTaxGroup = fS.oCode;
            SalesOrdersHeaderModel.oTaxGroup = oTaxGroup;

            var query = "select Code,Name,cast(Rate as numeric(19,2)) as rate,CAST(EffecDate as date) as effecdate from OVTG where Category = 'O' and Inactive = 'N' and Code = '" + fS.oCode + "'";
            
            var dt = hana.Get(query);
            if (helper.DataTableExist(dt))
            {
                oTaxRate = helper.ReadDataRow(dt, "rate", "", 0);
            }
        }

        private void LoadRAS(string CardCode)
        {
            string selqry = $"SELECT distinct ifnull(upper(b.firstName),'') +' ' + ifnull(upper(b.lastName), '') [FullName] " +
                            $" FROM OCRD a LEFT JOIN OHEM b ON a.U_Dim3 = b.CostCenter " +
                            $" LEFT JOIN OHPS c ON b.position = c.posID " +
                            $" where a.CardCode = '{ txtBpCode.Text}'  and c.posID IN('15','18')";

            if (hana.Get(selqry).Rows.Count > 0)
            {
                SalesOrderItemsController.oRAS = hana.Get(selqry).Rows[0]["FullName"].ToString();
            }
            else
            {
                SalesOrderItemsController.oRAS = "";
            }
            //frmUDF.dataGridLayout(UOSudf.gvUDF);
            //frmUDF._LoadData();
        }


        public void RefreshData()
        {
            try
            {
                Presenter.LoadData(dgvItems);

                //Replaced below by LoadData by 
                //SalesOrderItemsController.dgvItemsLayout(dgvItems);

                //foreach (var x in SalesOrderItemsModel.SalesOrderItems.Where(x => x.ObjType == objType))
                //{
                //    object[] a = { x.ItemCode, x.ItemName ,x.Brand, x.Style, x.Color, x.Size, x.Section, x.BarCode, x.EffectivePrice,x.GrossPrice.ToString("0.##"), x.UnitPrice.ToString("0.##"), x.Quantity, x.DiscountPerc.ToString("0.##"), x.DiscountAmount, x.EmpDiscountPerc.ToString("0.##") ,
                //                    x.FWhsCode, "...", x.TaxCode, "...", x.TaxRate, x.LineTotalManual, x.GrossTotal, x.PriceAfterDisc, x.Linenum, "", x.SKU };
                //    dgvItems.Rows.Add(a);
                //}

                //SalesOrderItemsController.dataGridLayout(dgvItems);

                ComputeTotal();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
            }
        }


        private void ComputeTotal()
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

                    var dTaxRate = double.Parse(sTaxRate);

                    //On comment due to carton qty 092419
                    //var dQty = double.Parse(DECLARE.Replace(row, "Quantity", "0.00"));
                    var dQty = double.Parse(DECLARE.Replace(row, "Ordered Qty", "0.00"));

                    var dPriceAfterDisc = double.Parse(DECLARE.Replace(row, "PriceAfterDisc", "0.00"));

                    var dGroPrice = dQty * double.Parse(DECLARE.Replace(row, "Gross Price", "0.00"));

                    double dLineTotal = double.Parse(DECLARE.Replace(row, "Line Total", "0.00"));

                    //Correct Computation is Qty * (Price * Tax) 11/21/19
                    var dCompTax = dQty * (dPriceAfterDisc * dTaxRate);
                    //var dCompTax = dQty * (dLineTotal * dTaxRate);

                    dTotalQty += dQty;
                    //dBefPrice += (Convert.ToDouble(dPriceAfterDisc) * Convert.ToDouble(dQty));
                    dBefPrice += dLineTotal;
                    dTotalTax += dCompTax;
                    dTotalGroPrice += dGroPrice;
                }

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
                    txtDiscount.Text = Math.Round(checkDiscPercent, 3).ToString("#,#00.00");
                }
                else if (DiscType != null && DiscType == "GetPercent" && txtDiscount.Focus() == true)
                {
                    txtDiscPercent.Text = Math.Round(GetDiscPercent, 3).ToString("#,#00.00");
                }

                if (txtDiscPercent.Text != "")
                {
                    var d_DiscPerc = double.Parse(txtDiscPercent.Text) / 100;
                    var taxpercent = (dTotalTax * d_DiscPerc);
                    dTotalTax = dTotalTax - taxpercent;
                    dTotalGroPrice = dTotalGroPrice - (dTotalGroPrice * d_DiscPerc);
                }

                txtTotalQty.Text = Math.Round(dTotalQty, 3).ToString("#,##0");
                txtTotalBefDisc.Text = Math.Round(dBefPrice, 3).ToString("#,##0.00");
                txtTaxAmount.Text = Math.Round(dTotalTax, 3).ToString("#,##0.00");
                txtTotal.Text = Math.Round(dTotalGroPrice, 3).ToString("#,##0.00");
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
            var fS = new frmSearch2();
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
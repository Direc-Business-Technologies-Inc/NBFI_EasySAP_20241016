using MetroFramework.Forms;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PresenterLayer.Helper;
using PresenterLayer.Helper.Unofficial_Sales;
using PresenterLayer.Views.Main;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using PresenterLayer.Views.Tools;
using System.Threading;
using System.Globalization;
using DirecLayer._05_Repository;
using DirecLayer._03_Repository;
using PresenterLayer.Services.SalesOrder;
using zDeclare;
using DirecLayer;
using PresenterLayer.Services;
using MetroFramework;
using InfrastructureLayer.InventoryRepository;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DomainLayer.Helper;
using System.IO;
using PresenterLayer.Services.Security;

namespace PresenterLayer.Views
{

    public partial class FrmUnofficialSales : MetroForm, IFrmUnofficialSales
    {
        private readonly IFrmUnofficialSales _View;
        private UnofficialSalesItemsController USIC = new UnofficialSalesItemsController();
        private SalesRepository SR = new SalesRepository();
        private SalesAR_generics SG = new SalesAR_generics();

        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        PurchasingAP_Style _style = new PurchasingAP_Style();
        ValidationRepository _validation = new ValidationRepository();
        UdfRepository _udfRepo = new UdfRepository();
        StringQueryRepository _reps = new StringQueryRepository();
        QueryRepository Query = new QueryRepository();

        public UOS_ServiceLayer UOS_SL { get; set; }
        private readonly ISalesOrderModel _repository;

        public string oSeries, oProject, oPriceList, oSalesEmployee, oAddressCode, oCode, oName;
        private int index = 0;
        private double DocTotal = 0.00;
        private bool FindMode = false;
        public string objType = "US";
        public static string oFWhsCode, oTWhsCode;
        public static string oTaxGroup;
        private static string DiscType;
        private static string oOneTime = "N";
        private static string oModule;
        int max_width = Screen.PrimaryScreen.Bounds.Width - 220;
        int max_height = Screen.PrimaryScreen.Bounds.Height - 150;

        private static double oDiscount;
        private DataTable CurrentDataSet { get; set; }
        private static DateTimePicker oDateTimePicker = new DateTimePicker();
        private static int prow = 0;
        private static string getDate = " ";
        private string table { get; set; }
        private string Vat { get; set; }
        private double VatRate { get; set; }
        private string RawCurr { get; set; }
        int DelDateSelRow = 0;
        int DelDateSelCol = -1;

        public FrmUnofficialSales()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            helper = new DataHelper();
        }
        public DataGridView Udf
        {
            get => DgvUdf;
        }

        public UnofficialSalesService Presenter
        {
            private get;
            set;
        }
        public string BpCurrency { get /*=> throw new NotImplementedException()*/; set /*=> throw new NotImplementedException()*/; }
        public string BpRate { get /*=> throw new NotImplementedException()*/; set /*=> throw new NotImplementedException()*/; }
        public string CancellationDate { get /*=> throw new NotImplementedException()*/; set /*=> throw new NotImplementedException()*/; }
        public string Company
        {
            get /*=> Convert.ToString(cb.SelectedValue)*/;
            set/* => cbCompany.Text = value*/;
        }

        public string CustomRef
        {
            get => txtCustomerRefNo.Text;
            set => txtCustomerRefNo.Text = value;
        }

        public string TotalQuantity
        {
            get => txtTotalQty.Text;
            set => txtTotalQty.Text = value;
        }

        public string Address
        {
            get => txtAddress.Text;
            set => txtAddress.Text = value;
        }
        public string BPCode
        {
            get => txtBpCode.Text;
            set => txtBpCode.Text = value;
        }


        public string ContactPerson
        {
            get => txtContactPerson.Text;
            set => txtContactPerson.Text = value;
        }
        public string DeliveryDate
        {
            get => dtDeliveryDate.Text;
            set => dtDeliveryDate.Text = value;
        }
        public string Department { get/* => throw new NotImplementedException()*/; set/* => throw new NotImplementedException()*/; }
        public string DiscountAmount
        {
            get => txtDiscount.Text;
            set => txtDiscount.Text = value;
        }
        public string DiscountInput
        {
            get => txtDiscount.Text;
            set => txtDiscount.Text = value.ToString();
        }
        public string DiscountPerc
        {
            get => txtDiscPercent.Text;
            set => txtDiscPercent.Text = value.ToString();
        }
        public string DocEntry
        {
            get => txtDocEntry.Text;
            set => txtDocEntry.Text = value;
        }
        public string DocNo
        {
            get => txtDocNum.Text;
            set => txtDocNum.Text = value;
        }
        public string DocNum { get; set; }
        public string DocumentDate
        {
            get => dtDocDate.Text;
            set => dtDocDate.Text = value;
        }
        public MainForm frmMain { get; set; }
        public bool IsFindMode
        {
            get => FindMode;
            set => FindMode = value;
        }

        public ContextMenuStrip MsItems { get => msItems; }

        public string PostingDate
        {
            get => dtMonthofSales.Text;
            set => dtMonthofSales.Text = value;
        }
        public string RawCurrency
        {
            get => RawCurr;
            set => RawCurr = value;
        }
        public string Remark
        {
            get => txtRemarks.Text;
            set => txtRemarks.Text = value;
        }
        public string Series
        {
            get => cbSeries.SelectedValue.ToString();
            set => cbSeries.Text = value;
        }
        public string Service
        {
            get => cbDocumentType.Text;
            set => cbDocumentType.Text = value;
        }
        public string Status
        {
            get => txtDocStatus.Text;
            set => txtDocStatus.Text = value;
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

        public DataGridView Table
        {
            get => dgvBarcodeItems;
        }

        public DataGridView TablePreview
        {
            get => DgvPreviewItem;
        }

        public string Tax { get; set; }
        public string Total
        {
            get => txtTotal.Text;
            set => txtTotal.Text = value;
        }
        public string TotalBeforeDiscount { get; set; }

        public string TotalBefDisc
        {
            get => txtTotalBefDisc.Text;
            set => txtTotalBefDisc.Text = value;
        }

        public Panel UdfPanel
        {
            get => panel1;
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
        public ComboBox WhsSls
        {
            get => cbWhsType;
        }
        public string WhsSlsType
        {
            get => cbWhsType.Text;
            set => cbWhsType.Text = value;
        }

        public string sOProject
        {
            get => oProject;
            set => oProject = value;
        }

        private void FrmUnofficialSales_Load(object sender, EventArgs e)
        {
            try
            {
                WindowState = FormWindowState.Maximized;
                NumSeries();
                LoadBlankDates();
                LoadDocumentType();
                LoadUnofficialSales();
                CheckDocumentTypeMaintenance();
                LoadWhsSalesType();

                if (oDateTimePicker.IsDisposed)
                {
                    oDateTimePicker = new DateTimePicker();
                }
                if (oDateTimePicker.Visible)
                {
                    oDateTimePicker.Visible = false;
                }

                DgvFindDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DgvUdf.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.Resizable = DataGridViewTriState.False);

                foreach (DataGridViewRow row in DgvUdf.Rows)
                {
                    if (row.Cells[1].Value.ToString().Equals("Prepared By"))
                    {
                        DgvUdf.Rows[row.Index].Cells["Field"].Value = DomainLayer.Models.EasySAPCredentialsModel.ESUserId;
                        DgvUdf.Rows[row.Index].Cells["Field"].ReadOnly = true;
                        break;
                    }
                }

                var files = Presenter.GetDocumentCrystalForms();

                if (files != null)
                {
                    foreach (FileInfo file in files)
                    {
                        CmbPrintPreview.Items.Add(file.Name);
                    }
                }

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }

        }

        void NumSeries()
        {
            txtDocStatus.Text = "Open";
            string sdata = "SELECT SeriesName FROM NNM1 Where IsForCncl = 'N' and ObjectCode = 15";
            DataTable dt = hana.Get(sdata);

            Application.DoEvents();
            cbSeries.DisplayMember = "SeriesName";
            cbSeries.DataSource = dt;

        }

        private void LoadUnofficialSales()
        {
            try
            {
                if (this.Text != "Delivery")
                {
                    //lblWhsSale.Visible = true;
                    dtDeliveryDate.Visible = false;
                    label1.Visible = false;
                    cbSeries.SelectedIndex = cbSeries.FindString("UnSales");
                    cbSeries.Enabled = false;
                    cbDocumentType.SelectedIndex = cbDocumentType.FindString("Unofficial Sales");
                    cbDocumentType.Enabled = false;
                    label22.Visible = false;
                    txtFromDoc.Visible = false;
                    txtSONumber.Visible = false;
                    label10.Visible = false;
                    BtnCopyFrom.Visible = false;
                    CmbCopyFromOption.Visible = false;
                    //cbDocList.Visible = false;
                    label9.Visible = true;
                    txtItemCode.Visible = true;
                    txtCurrency.Visible = false;
                    label8.Visible = false;
                    lblWsType.Visible = true;
                    cbWhsType.Visible = true;
                    txtDiscount.Text = "";
                    cbWhsType.Text = "";

                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab && txtBpCode.Focused && txtBpCode.Text == "")
            {
                ViewList("OCRD", out oCode, out oName, "List of Customers", "C");
                UnofficialSalesHeaderModel.oBPCode = "";

                if (oCode != null)
                {
                    txtBpCode.Text = oCode;
                    //PublicStatic.DeliveryCardCode = oCode;
                    UnofficialSalesHeaderModel.oBPCode = oCode;
                    LoadBPDetails(oCode, false);
                }

                CheckDocumentTypeMaintenance();
                return true;
            }
            else if (keyData == Keys.Tab && txtWhsCode.Focused && txtWhsCode.Text == "")
            {
                LoadWhseList();
                return true;
            }
            //else if (keyData == Keys.Tab && dtMonthofSales.Focused && dtMonthofSales.Text == " ")
            //{
            //    dtMonthofSales.Format = DateTimePickerFormat.Short;
            //    dtMonthofSales.Select();
            //    SendKeys.Send("%{DOWN}");
            //    return true;
            //}
            else if (keyData == Keys.Tab && dtDeliveryDate.Focused && dtDeliveryDate.Text == " ")
            {
                dtDeliveryDate.Format = DateTimePickerFormat.Short;
                dtDeliveryDate.Select();
                SendKeys.Send("%{DOWN}");
                return true;
            }
            else if (keyData == Keys.Tab && dtDocDate.Focused && dtDocDate.Text == " ")
            {
                dtDocDate.Format = DateTimePickerFormat.Short;
                dtDocDate.Select();
                SendKeys.Send("%{DOWN}");
                return true;
            }
            else if (keyData == Keys.Tab && dtDocDate.Focused && dtDocDate.Text != " ")
            {
                txtItemCode.Focus();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                Cancel();
            }
            else if (keyData == (Keys.Control | Keys.M))
            {
                ViewList("OCRD", out oCode, out oName, "List of Customers", "C");
                UnofficialSalesHeaderModel.oBPCode = "";

                if (oCode != null)
                {
                    txtBpCode.Text = oCode;
                    //PublicStatic.DeliveryCardCode = oCode;
                    UnofficialSalesHeaderModel.oBPCode = oCode;
                    LoadBPDetails(oCode, false);
                }

                CheckDocumentTypeMaintenance();
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                txtCustomerRefNo.Focus();
            }
            else if (keyData == (Keys.Control | Keys.Y))
            {
                cbDocumentType.DroppedDown = true;
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                if (this.Text != "Unofficial Sales")
                {
                    cbSeries.Focus();
                    cbSeries.DroppedDown = true;
                }
            }
            else if (keyData == (Keys.Control | Keys.D1))
            {
                //dtMonthofSales.Focus();

                //if (dtMonthofSales.Text != " ")
                //{
                //    dtMonthofSales.CustomFormat = " ";
                //    dtMonthofSales.Format = DateTimePickerFormat.Custom;
                //}

                //Thread.Sleep(300);

                //dtMonthofSales.Format = DateTimePickerFormat.Short;
                //dtMonthofSales.Select();
                //SendKeys.Send("%{DOWN}");
                //return true;
            }
            else if (keyData == (Keys.Control | Keys.D3))
            {
                dtDocDate.Focus();

                if (dtDocDate.Text != " ")
                {
                    dtDocDate.CustomFormat = " ";
                    dtDocDate.Format = DateTimePickerFormat.Custom;
                }

                Thread.Sleep(300);

                dtDocDate.Format = DateTimePickerFormat.Short;
                dtDocDate.Select();
                SendKeys.Send("%{DOWN}");
                return true;
            }
            else if (keyData == (Keys.Control | Keys.D2))
            {
                dtDeliveryDate.Focus();

                if (dtDeliveryDate.Text != " ")
                {
                    dtDeliveryDate.CustomFormat = " ";
                    dtDeliveryDate.Format = DateTimePickerFormat.Custom;
                }

                Thread.Sleep(300);

                dtDeliveryDate.Format = DateTimePickerFormat.Short;
                dtDeliveryDate.Select();
                SendKeys.Send("%{DOWN}");
                return true;
            }
            else if (keyData == (Keys.Control | Keys.H))
            {
                LoadWhseList();
            }
            else if (keyData == (Keys.Control | Keys.E))
            {
                SalesEmployee();
            }
            else if (keyData == (Keys.Control | Keys.K))
            {
                txtRemarks.Focus();
            }
            else if (keyData == (Keys.Control | Keys.U))
            {
                txtDiscPercent.Focus();
            }
            else if (keyData == (Keys.Control | Keys.I))
            {
                AddItem();
            }
            else if (keyData == (Keys.Control | Keys.N))
            {
                NewDocument();
            }
            else if (keyData == (Keys.Control | Keys.A))
            {
                AddtoSAP();
            }
            else if (keyData == (Keys.Control | Keys.F))
            {
                Find();
            }
            else if (keyData == (Keys.Control | Keys.B))
            {
                txtItemCode.Focus();
            }
            else if (keyData == (Keys.Control | Keys.D0))
            {
                dgvBarcodeItems.Focus();
            }
            //else if (DelDateSelCol == 19)
            //{
            //    if ((keyData == Keys.Right) || (keyData == Keys.Left) || (keyData == Keys.Up) || (keyData == Keys.Down) || (keyData == Keys.Tab))
            //    {
            //        return true;
            //    }
            //    else if (keyData == Keys.Enter)
            //    {
            //        SetDelDate();
            //        oDateTimePicker.Visible = false;
            //    }
            //}
            else if (DelDateSelCol == 2 || DelDateSelCol == 3 || DelDateSelCol == 4 || DelDateSelCol == 5 || DelDateSelCol == 6)
            {
                if (keyData == (Keys.Control | Keys.V))
                {
                    string strCopyVal = "0";
                    if (Clipboard.ContainsText())
                    {
                        strCopyVal = Clipboard.GetText(TextDataFormat.Text);
                    }
                    dgvBarcodeItems.CurrentCell.Value = strCopyVal;
                    //dgvBarcodeItems.Rows[DelDateSelRow].Cells[DelDateSelCol].Value = strCopyVal;
                    DelDateSelRow = dgvBarcodeItems.CurrentCell.RowIndex;
                    DelDateSelCol = dgvBarcodeItems.CurrentCell.ColumnIndex;
                    DataGridViewCellEventArgs dgvCEA = new DataGridViewCellEventArgs(DelDateSelCol, DelDateSelRow);
                    dgvBarCEE(dgvCEA);
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void dgvBarCEE(DataGridViewCellEventArgs e)
        {
            try
            {
                string oEditField = dgvBarcodeItems.Columns[e.ColumnIndex].Name;
                int intIndex = Convert.ToInt32(dgvBarcodeItems.Rows[e.RowIndex].Cells["index"].Value.ToString());

                if (oEditField == "Quantity" || oEditField == "Unit Price" || oEditField == "Gross Price" || oEditField == "Discount %" || oEditField == "Discount Amount")
                {
                    string ItemCode = dgvBarcodeItems.Rows[e.RowIndex].Cells["Item No."].Value.ToString();
                    var UOSList = UnofficialSalesItemsModel.UnofficialSalesItems.First(x => x.ItemCode == ItemCode && x.Linenum == intIndex);

                    if (dgvBarcodeItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Any(char.IsDigit) == false)
                    {
                        if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Quantity")
                        {
                            string oQty = UOSList.Quantity.ToString();
                            dgvBarcodeItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oQty;
                        }
                        else if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Unit Price")
                        {
                            string oUPrice = UOSList.UnitPrice.ToString("0.##");
                            dgvBarcodeItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oUPrice;
                        }
                        else if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Gross Price")
                        {
                            string oGPrice = UOSList.GrossPrice.ToString("0.##");
                            dgvBarcodeItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oGPrice;
                        }
                        else if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Discount %")
                        {
                            string oDiscPerc = UOSList.DiscountPerc.ToString();
                            dgvBarcodeItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oDiscPerc;
                        }
                        else if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Discount Amount")
                        {
                            string oDiscAmount = UOSList.DiscountAmount.ToString();
                            dgvBarcodeItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oDiscAmount;
                        }
                    }
                    else
                    {
                        if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Quantity")
                        {
                            string oQty = dgvBarcodeItems.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();

                            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == ItemCode && x.Linenum == intIndex))
                            {
                                x.Quantity = Convert.ToDouble(oQty);
                            }
                        }
                        else if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Unit Price")
                        {
                            string oUPrice = dgvBarcodeItems.Rows[e.RowIndex].Cells["Unit Price"].Value.ToString();

                            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == ItemCode && x.Linenum == intIndex))
                            {
                                x.UnitPrice = Convert.ToDouble(oUPrice);
                            }
                        }
                        else if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Gross Price")
                        {
                            string oGPrice = dgvBarcodeItems.Rows[e.RowIndex].Cells["Gross Price"].Value.ToString();

                            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == ItemCode && x.Linenum == intIndex))
                            {
                                x.GrossPrice = Convert.ToDouble(oGPrice);
                            }
                        }
                        else if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Discount %")
                        {
                            string oDiscPerc = dgvBarcodeItems.Rows[e.RowIndex].Cells["Discount %"].Value.ToString();

                            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == ItemCode && x.Linenum == intIndex))
                            {
                                x.DiscountPerc = Convert.ToDouble(oDiscPerc);
                            }
                        }
                        else if (dgvBarcodeItems.Columns[e.ColumnIndex].Name == "Discount Amount")
                        {
                            string oDiscAmount = dgvBarcodeItems.Rows[e.RowIndex].Cells["Discount Amount"].Value.ToString();

                            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == ItemCode && x.Linenum == intIndex))
                            {
                                x.DiscountAmount = Convert.ToDouble(oDiscAmount);
                            }
                        }

                        RecomputeLines(e);
                        ComputeTotal();
                    }

                }
                else if (oEditField == "Item Description")
                {
                    string ItemCode = dgvBarcodeItems.Rows[e.RowIndex].Cells["Item No."].Value.ToString();
                    var UOSList = UnofficialSalesItemsModel.UnofficialSalesItems.First(x => x.ItemCode == ItemCode);
                    string oItemDesc = dgvBarcodeItems.Rows[e.RowIndex].Cells["Item Description"].Value.ToString();
                    int LineNo = Convert.ToInt32(dgvBarcodeItems.Rows[e.RowIndex].Cells["index"].Value.ToString());

                    foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == ItemCode && x.Linenum == LineNo))
                    {
                        x.ItemName = oItemDesc;
                    }
                }
                else if (oEditField == "DeliveryDate")
                {
                    DateTime dt;
                    string oItemCode = dgvBarcodeItems.Rows[e.RowIndex].Cells["Item No."].Value.ToString();
                    string strDeliveryDate = dgvBarcodeItems.Rows[e.RowIndex].Cells["DeliveryDate"].Value.ToString();

                    if (!DateTime.TryParseExact(strDeliveryDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        StaticHelper._MainForm.ShowMessage("Wrong format for date. Please change to this format MM/dd/yyyy.", true);
                        foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == oItemCode && x.Linenum == intIndex))
                        {
                            if (x.DelDate != "")
                            {
                                dgvBarcodeItems.Rows[e.RowIndex].Cells["DeliveryDate"].Value = x.DelDate;
                            }
                            else
                            {
                                strDeliveryDate = oDateTimePicker.Value.ToString("MM/dd/yyyy");
                                dgvBarcodeItems.Rows[e.RowIndex].Cells["DeliveryDate"].Value = strDeliveryDate;
                                SaveDelDate(oItemCode, strDeliveryDate);
                            }
                        }
                    }
                    else
                    {
                        SaveDelDate(oItemCode, strDeliveryDate);
                    }
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private void msbtnDelete_Click(object sender, EventArgs e)
        {
            //var result = MessageBox.Show("Are you sure you want to remove selected item?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (result == DialogResult.Yes)
            //{
            foreach (DataGridViewRow row2 in dgvBarcodeItems.Rows)
            {
                if (row2.Selected == true)
                {
                    int row = row2.Index;
                    string itemCode = dgvBarcodeItems.Rows[row].Cells[0].Value.ToString();
                    int id = Convert.ToInt32(dgvBarcodeItems.Rows[row].Cells["index"].Value.ToString());

                    UnofficialSalesItemsModel.UnofficialSalesItems.RemoveAll(x => x.ItemCode == itemCode && x.Linenum == id);

                }
            }

            Presenter.LoadData(dgvBarcodeItems);
            UpdateTotal();
            //RefreshData();
            //}
        }




        private void dgvBarcodeItems_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    dgvBarcodeItems.CurrentCell = dgvBarcodeItems.Rows[e.RowIndex].Cells[e.ColumnIndex + 1];
                    dgvBarcodeItems.Rows[e.RowIndex].Selected = true;
                    dgvBarcodeItems.Focus();

                    var mousePosition = dgvBarcodeItems.PointToClient(Cursor.Position);

                    msItems.Show(dgvBarcodeItems, mousePosition);
                }
            }
        }

        private void dgvBarcodeItems_Scroll(object sender, ScrollEventArgs e)
        {
            oDateTimePicker.Visible = false;
        }


        private void RecomputeLines(DataGridViewCellEventArgs e)
        {
            try
            {
                var col1 = e.ColumnIndex;
                var row1 = e.RowIndex;
                string oItemCode = dgvBarcodeItems.Rows[row1].Cells[0].Value.ToString();
                string ColName = dgvBarcodeItems.Columns[e.ColumnIndex].Name;

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
                else if (ColName == "Discount %" || ColName == "Discount Amount")
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


        private void RLgrossprice(DataGridViewCellEventArgs e, string oItemCode)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblCompGrossPrice = 0;
            int intIndex = Convert.ToInt32(dgvBarcodeItems.Rows[e.RowIndex].Cells["index"].Value.ToString());
            //Gross Price
            foreach (DataGridViewRow row in dgvBarcodeItems.Rows)
            {
                if (DECLARE.Replace(row, "Item No.", "") == oItemCode && row.Cells["index"].Value.ToString() == intIndex.ToString())
                {
                    double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0.00"));
                    double dblDiscPercent = Convert.ToDouble(DECLARE.Replace(row, "Discount %", "0"));
                    double dblDiscAmount = (dblUnitPrice / 100) * dblDiscPercent;

                    double tax = Convert.ToDouble(hana.Get($@"SELECT T0.ECVatGroup, T1.Rate FROM OCRD T0  INNER JOIN OVTG T1 ON T0.ECVatGroup = T1.Code WHERE T0.CardCode = '{BPCode}'").Rows[0]["Rate"].ToString());
                    tax = 1 + (tax / 100);

                    //double dblTaxRate = Convert.ToDouble("1.12");
                    double dblTaxRate = tax;

                    dblCompGrossPrice = (dblUnitPrice - dblDiscAmount) * dblTaxRate;
                    if (dblCompGrossPrice.ToString() == "NaN")
                    {
                        dblCompGrossPrice = 0;
                    }
                }
            }
            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == oItemCode && x.Linenum == intIndex))
            {
                x.GrossPrice = Convert.ToDouble(dblCompGrossPrice.ToString("#,#00.00"));
                dgvBarcodeItems.Rows[row1].Cells["Gross Price"].Value = Convert.ToDouble(dblCompGrossPrice.ToString("#,#00.00"));
            }

        }

        private void RLpriceafterdisc(DataGridViewCellEventArgs e, string oItemCode, string oValueFrom)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblPriceAfterDisc = 0;
            int intIndex = Convert.ToInt32(dgvBarcodeItems.Rows[e.RowIndex].Cells["index"].Value.ToString());
            //PriceAfterDisc
            foreach (DataGridViewRow row in dgvBarcodeItems.Rows)
            {
                string ItemNo = row.Cells["Item No."].Value.ToString();
                if (ItemNo == oItemCode && row.Cells["index"].Value.ToString() == intIndex.ToString())
                {
                    double dblQty = Convert.ToDouble(DECLARE.Replace(row, "Quantity", "0"));

                    double tax = Convert.ToDouble(hana.Get($@"SELECT T0.ECVatGroup, T1.Rate FROM OCRD T0  INNER JOIN OVTG T1 ON T0.ECVatGroup = T1.Code WHERE T0.CardCode = '{BPCode}'").Rows[0]["Rate"].ToString());
                    tax = 1 + (tax / 100);

                    //double dblTaxRate = Convert.ToDouble("1.12");
                    double dblTaxRate = tax;
                    double dblGrossPrice = Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0.00"));
                    double dblDiscAmt = Convert.ToDouble(DECLARE.Replace(row, "Discount Amount", "0.00"));

                    dblPriceAfterDisc = dblGrossPrice / dblTaxRate;
                    if (dblPriceAfterDisc.ToString() == "NaN")
                    {
                        dblPriceAfterDisc = 0;
                    }
                }
            }
            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == oItemCode && x.Linenum == intIndex))
            {
                x.PriceBefDisc = Convert.ToDouble(dblPriceAfterDisc.ToString("#,#00.00"));
                dgvBarcodeItems.Rows[row1].Cells["PriceBeforeDiscount"].Value = Convert.ToDouble(dblPriceAfterDisc.ToString("#,#00.00"));
                if (oValueFrom == "GP")
                {
                    x.UnitPrice = Convert.ToDouble(dblPriceAfterDisc.ToString("#,##0.00"));
                    dgvBarcodeItems.Rows[row1].Cells["Unit Price"].Value = Convert.ToDouble(dblPriceAfterDisc.ToString("#,##0.00"));
                }
            }

        }

        private void RLlinetotal(DataGridViewCellEventArgs e, string oItemCode)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblLineTotal = 0;
            int intIndex = Convert.ToInt32(dgvBarcodeItems.Rows[e.RowIndex].Cells["index"].Value.ToString());
            //Line Total
            foreach (DataGridViewRow row in dgvBarcodeItems.Rows)
            {
                string ItemNo = row.Cells["Item No."].Value.ToString();
                if (ItemNo == oItemCode && row.Cells["index"].Value.ToString() == intIndex.ToString())
                {
                    double dblQty = Convert.ToDouble(DECLARE.Replace(row, "Quantity", "0"));
                    double dblPrice = Convert.ToDouble(DECLARE.Replace(row, "PriceBeforeDiscount", "0.00"));
                    double dblDiscAmt = Convert.ToDouble(DECLARE.Replace(row, "Discount Amount", "0.00"));
                    dblLineTotal = dblQty * (dblPrice - dblDiscAmt);
                    if (dblLineTotal.ToString() == "NaN")
                    {
                        dblLineTotal = 0;
                    }
                }
            }
            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == oItemCode && x.Linenum == intIndex))
            {
                x.LineTotal = Convert.ToDouble(dblLineTotal.ToString("#,#00.00"));
                dgvBarcodeItems.Rows[row1].Cells["Total"].Value = Convert.ToDouble(dblLineTotal.ToString("#,#00.00"));
            }
        }

        private void RLdiscamount(DataGridViewCellEventArgs e, string oItemCode)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblDiscAmount = 0;
            int intIndex = Convert.ToInt32(dgvBarcodeItems.Rows[e.RowIndex].Cells["index"].Value.ToString());
            //Discount Amount
            foreach (DataGridViewRow row in dgvBarcodeItems.Rows)
            {
                string ItemNo = row.Cells["Item No."].Value.ToString();
                if (ItemNo == oItemCode && row.Cells["index"].Value.ToString() == intIndex.ToString())
                {
                    double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0.00"));
                    double dblDiscPercent = Convert.ToDouble(DECLARE.Replace(row, "Discount %", "0"));
                    dblDiscAmount = (dblUnitPrice / 100) * dblDiscPercent;
                    if (dblDiscAmount.ToString() == "NaN")
                    {
                        dblDiscAmount = 0;
                    }
                }
            }
            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == oItemCode && x.Linenum == intIndex))
            {
                x.DiscountAmount = Convert.ToDouble(dblDiscAmount.ToString("#,#00.00"));
                dgvBarcodeItems.Rows[row1].Cells["Discount Amount"].Value = Convert.ToDouble(dblDiscAmount.ToString("#,#00.00"));
            }
        }

        private void RLdiscpercent(DataGridViewCellEventArgs e, string oItemCode)
        {
            var col1 = e.ColumnIndex;
            var row1 = e.RowIndex;

            double dblDiscPercent = 0;
            int intIndex = Convert.ToInt32(dgvBarcodeItems.Rows[e.RowIndex].Cells["index"].Value.ToString());
            //Discount Percent
            foreach (DataGridViewRow row in dgvBarcodeItems.Rows)
            {
                string ogvItem = row.Cells[0].Value.ToString();
                if (ogvItem == oItemCode && row.Cells["index"].Value.ToString() == intIndex.ToString())
                {
                    double dblUnitPrice = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0.00"));
                    double dblDiscAmount = Convert.ToDouble(DECLARE.Replace(row, "Discount Amount", "0"));
                    dblDiscPercent = (dblDiscAmount / dblUnitPrice) * 100;
                    if (dblDiscPercent.ToString() == "NaN")
                    {
                        dblDiscPercent = 0;
                    }
                }
            }
            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == oItemCode && x.Linenum == intIndex))
            {
                x.DiscountPerc = Convert.ToDouble(dblDiscPercent.ToString("#,##0.00"));
                dgvBarcodeItems.Rows[row1].Cells["Discount %"].Value = Convert.ToDouble(dblDiscPercent.ToString("#,##0.00"));
            }
        }


        private void Find()
        {
            if (InvoiceItemsModel.InvoiceItems.Where(x => x.ObjType == objType).ToList().Count > 0)
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    findDocument();
                }
            }
            else
            {
                findDocument();
            }
        }


        void findDocument()
        {
            string oTable = txtBpCode.Text == "" ? "ODLN" : "ODLN_BP";
            string oBPCode = txtBpCode.Text == "" ? "" : txtBpCode.Text;

            ViewList(oTable, out oCode, out oName, "List of Deliveries", oBPCode);

            if (oCode != null)
            {
                txtDocEntry.Text = oName;
                txtSODocEntry.Text = oCode;
                LoadSAPDAta(Convert.ToInt32(oCode));
                FindMode = false;
                //frmUDF.LoadUDF(oCode, "ODLN");
                btnAdd.Text = "Update";
                DisableControls();
            }
        }

        void LoadSAPDAta(int DocEntry)
        {
            FindMode = true;
            txtRemarks.Text = "";

            ClearList();

            var query = $"Select DocNum" +
                        ",CardCode" +
                        ",CardName" +
                        ",(Case when DocStatus = 'O' then 'Open' Else 'Closed' end) DocStatus" +
                        ",DiscPrcnt" +
                        ",U_Remarks" +
                        $",TaxDate,DocDueDate,DocDate " +
                        $"FROM ODLN WHERE DocEntry = {DocEntry}";

            string _bpCode, _docNum, _status, _discperc, _remarks, taxdate, docduedate, docdate;

            DataTable headerDetails = hana.Get(query);

            _bpCode = DECLARE.dtNull(headerDetails, 0, "CardCode", "");
            _docNum = DECLARE.dtNull(headerDetails, 0, "DocNum", "0");
            _status = DECLARE.dtNull(headerDetails, 0, "DocStatus", "");
            _discperc = DECLARE.dtNull(headerDetails, 0, "DiscPrcnt", "");
            _remarks = DECLARE.dtNull(headerDetails, 0, "U_Remarks", "");

            taxdate = DECLARE.dtNull(headerDetails, 0, "TaxDate", "");
            docduedate = DECLARE.dtNull(headerDetails, 0, "DocDueDate", "");
            docdate = DECLARE.dtNull(headerDetails, 0, "DocDate", "");

            txtBpCode.Text = _bpCode;
            txtDocEntry.Text = _docNum;
            txtDocStatus.Text = _status;

            dtMonthofSales.Format = DateTimePickerFormat.Short;
            dtMonthofSales.Text = docduedate;

            LoadBPDetails(_bpCode);

            var queryItems = "SELECT T0.ItemCode, T0.Dscription, T0.Quantity [Quantity], T0.Price [Unit Price], T0.DiscPrcnt, " +
                            "T0.LineTotal, T0.WhsCode, T0.SlpCode, T0.PriceBefDi, T0.Project, T0.VatGroup, '' [U_DiscType]," +
                            "T0.VatPrcnt, T0.CodeBars, T0.PriceAfVAT, T0.TaxCode, T0.VatAppld, T0.LineVat, " +
                            "T1.U_ID025 [U_StyleCode], T1.U_ID011 [U_Color], T1.U_ID018 [U_Section], T1.U_ID007 [U_Size], T1.CodeBars,T0.GTotal, T1.U_ID023" +
                            ", ISNULL(T0.ShipDate, '') [ShipDate] " +
                            "FROM DLN1 T0 " +
                            "INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode " +
                            "Where T0.DocEntry = '" + DocEntry + "' Order by T0.LineNum DESC";

            DataTable dtItems = hana.Get(queryItems);

            for (int x = 0; x < dtItems.Rows.Count; x++)
            {
                //Double LineTotal, GrossTotal, VatAmount, GrossPrice, PriceVatInc, DiscAmt;

                double qty = Convert.ToDouble(dtItems.Rows[x]["Quantity"]);
                //double discount = Convert.ToDouble(dtItems.Rows[x]["DiscPrcnt"]);
                //double vatrate = Convert.ToDouble(dtItems.Rows[x]["VatPrcnt"]);
                //double gtotal = Convert.ToDouble(dtItems.Rows[x]["GTotal"]);

                string price = dtItems.Rows[x]["Unit Price"].ToString();
                double DocTotal = Convert.ToDouble(price) * qty;
                double Discount = Convert.ToDouble(dtItems.Rows[x]["DiscPrcnt"].ToString());
                double PriceBeforeDiscount = Convert.ToDouble(dtItems.Rows[x]["PriceBefDi"].ToString());

                double tax = Convert.ToDouble(hana.Get($@"SELECT T0.ECVatGroup, T1.Rate FROM OCRD T0  INNER JOIN OVTG T1 ON T0.ECVatGroup = T1.Code WHERE T0.CardCode = '{BPCode}'").Rows[0]["Rate"].ToString());
                tax = 1 + (tax / 100);


                double GrossPrice = Convert.ToDouble(PriceBeforeDiscount * tax);
                string oStyle = dtItems.Rows[x]["U_StyleCode"].ToString();
                string oColor = dtItems.Rows[x]["U_Color"].ToString();
                string oSize = dtItems.Rows[x]["U_Size"].ToString();
                string oSection = dtItems.Rows[x]["U_Section"].ToString();
                string oDelDate = dtItems.Rows[x]["ShipDate"].ToString() == "" ? "" : Convert.ToDateTime(dtItems.Rows[x]["ShipDate"].ToString()).ToString("MM/dd/yyyy");

                //PriceVatInc = price + (price * (vatrate / 100));
                //DiscAmt = price * (discount / 100);
                //LineTotal = (qty * price);
                //VatAmount = LineTotal * (vatrate / 100);
                //GrossTotal = (LineTotal + VatAmount) - DiscAmt;
                //GrossPrice = PriceVatInc - (PriceVatInc * (discount / 100));

                DECLARE.so_items.Add(new DECLARE.SalesOrderItems
                {
                    ItemCode = dtItems.Rows[x]["ItemCode"].ToString(),
                    ItemName = dtItems.Rows[x]["Dscription"].ToString(),
                    Quantity = qty,
                    //Price = Convert.ToDouble(String.Format(dtItems.Rows[x]["PriceAfVAT"].ToString(), "{0:N}")),
                    Price = Convert.ToDouble(price),
                    DiscPerc = Math.Round(Discount, 2),
                    DiscAmount = Math.Round(Convert.ToDouble(price) * (Discount / 100), 2),
                    Total = DocTotal,
                    positive = true,
                    GrossPrice = Convert.ToDouble(GrossPrice.ToString("#00.00")),
                    PriceBeforeDiscount = PriceBeforeDiscount,
                    Style = oStyle,
                    Color = oColor,
                    Size = oSize,
                    Section = oSection,
                    SortCode = dtItems.Rows[x]["U_ID023"].ToString(),
                    ItemProperty = "N",
                    DelDate = oDelDate

                });
            }

            RefreshData();

            DisableControls();
        }


        private void dgvBarcodeItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvBarCEE(e);
        }




        private void DisableControls()
        {

            pbBPList.Visible = false;
            dtMonthofSales.Enabled = true;
            cbSeries.Enabled = false;

            FindMode = true;
        }

        void LoadBPDetails(string CardCode)
        {
            try
            {
                txtBpCode.Text = CardCode;

                string query = "SELECT A.CardCode,A.CardName,A.Currency,A.MailAddres [Address],A.ECVatGroup,A.ProjectCod,A.SlpCode,CntctPrsn, " +
                                "(SELECT Z.Address FROM CRD1 Z Where Z.CardCode = A.CardCode And Z.Street = A.MailAddres And Z.AdresType = 'S' and Z.Address = A.ShipToDef) [AddressCode], " +
                                "(Select Z.SlpName FROM OSLP Z Where Z.SlpCode = A.SlpCode) [SlpName],A.ListNum,(SELECT DISTINCT Z.ListName FROM OPLN Z Where Z.ListNum = A.ListNum) [ListName], QryGroup17 [OneTime] " +
                                $"FROM OCRD A WHERE A.CardCode = '{CardCode}'";

                var dt = new DataTable();

                dt = hana.Get(query);

                var dt2 = hana.Get($"select Distinct U_Whs from CRD1 where CardCode = '{CardCode}'");

                var strDefWhs = helper.DataTableExist(dt2) == true ? DECLARE.dtNull(dt2, 0, "U_Whs", "") : "";

                if (helper.DataTableExist(dt))
                {
                    oOneTime = DECLARE.dtNull(dt, 0, "OneTime", "");
                    txtBpName.Text = DECLARE.dtNull(dt, 0, "CardName", "");
                    txtSalesEmployee.Text = DECLARE.dtNull(dt, 0, "SlpName", "");
                    txtContactPerson.Text = DECLARE.dtNull(dt, 0, "CntctPrsn", "");
                    oProject = DECLARE.dtNull(dt, 0, "ProjectCod", "");
                    sOProject = oProject;
                    oPriceList = DECLARE.dtNull(dt, 0, "ListNum", "");
                    oSalesEmployee = DECLARE.dtNull(dt, 0, "SlpCode", "");
                    oAddressCode = DECLARE.dtNull(dt, 0, "AddressCode", "");
                    txtAddress.Text = DECLARE.dtNull(dt, 0, "Address", "");
                    txtCurrency.Text = DECLARE.dtNull(dt, 0, "Currency", "");
                    UnofficialSalesHeaderModel.oTaxGroup = DECLARE.dtNull(dt, 0, "ECVatGroup", "0");
                    txtWhsCode.Text = strDefWhs;
                    UnofficialSalesHeaderModel.oWhsCode = strDefWhs;

                }
                else
                {
                    oOneTime = string.Empty;
                    txtBpName.Text = string.Empty;
                    txtSalesEmployee.Text = string.Empty;
                    txtContactPerson.Text = string.Empty;
                    oProject = string.Empty;
                    sOProject = oProject;
                    oPriceList = string.Empty;
                    oSalesEmployee = string.Empty;
                    oAddressCode = string.Empty;
                    txtAddress.Text = string.Empty;
                    txtCurrency.Text = string.Empty;
                    UnofficialSalesHeaderModel.oTaxGroup = "0";
                    txtWhsCode.Text = strDefWhs;
                    UnofficialSalesHeaderModel.oWhsCode = strDefWhs;
                }

                LoadRAS(CardCode);

                //Replaced txtBPName with RAS by Cedi 061019
                //DgvUdf.Rows[row.Index].Cells["Field"].Value = txtBpName.Text;
                foreach (DataGridViewRow row in DgvUdf.Rows)
                {
                    if (row.Cells["Code"].Value.ToString() == "U_RAS")
                    {
                        DgvUdf.Rows[row.Index].Cells["Field"].Value = UnofficialSalesItemsController.oRAS;
                        DgvUdf.Rows[row.Index].Cells["Field"].ReadOnly = true;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }


        private void AddtoSAP()
        {
            if (txtBpCode.Text != "")
            {
                if (dgvBarcodeItems.Rows.Count > 0)
                {
                    switch (btnAdd.Text)
                    {
                        case "Update":
                            UpdateUDF();
                            break;

                        case "&Add":
                            if (SboCred.IsDIAPI)
                            {
                                Upload();
                            }
                            else
                            {
                                if (txtSODocEntry.Text == "")
                                {
                                    SG.PreLoading(dgvBarcodeItems.Rows.Count, "added");
                                    UOS_SL.SI_Posting("POST");
                                }
                                else
                                {
                                    UOS_SL.SI_Posting("POST", Convert.ToInt32(txtSODocEntry.Text));
                                }
                            }

                            break;
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Please select an item(s) before adding.", true);
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Error: Customer is required field", true);
            }
        }


        void Upload()
        {
            //if (SAPAccess.ConnectToSAPDI())
            //{
            //    btnAdd.Enabled = false;
            //    //PublicStatic.frmMain.Progress("Please wait...", 100);
            //    int max = dgvBarcodeItems.Rows.Count;
            //    int min = 0;

            //    // Init the Order object
            //    SAPAccess.oOrder = (SAPbobsCOM.Documents)SAPAccess.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
            //    SAPAccess.oOrder.DocDate = Convert.ToDateTime(dtPostingDate.Text);

            //    if (this.Text == "Delivery")
            //    {
            //        SAPAccess.oOrder.DocDueDate = Convert.ToDateTime(dtDeliveryDate.Text);
            //    }
            //    else
            //    {
            //        SAPAccess.oOrder.DocDueDate = Convert.ToDateTime(dtPostingDate.Text);
            //    }

            //    SAPAccess.oOrder.TaxDate = Convert.ToDateTime(dtDocDate.Text);
            //    SAPAccess.oOrder.Series = Convert.ToInt32(oSeries);
            //    SAPAccess.oOrder.CardCode = txtBpCode.Text;
            //    SAPAccess.oOrder.UserFields.Fields.Item("U_Remarks").Value = txtRemarks.Text;
            //    SAPAccess.oOrder.Comments = "Uploaded by EasySAP";
            //    SAPAccess.oOrder.UserFields.Fields.Item("U_PrepBy").Value = SboCred.UserID;
            //    SAPAccess.oOrder.UserFields.Fields.Item("U_DocType").Value = cbDocumentType.SelectedValue == null ? "" : cbDocumentType.SelectedValue.ToString();
            //    //SAPAccess.oOrder.UserFields.Fields.Item("U_DocType").Value = cbDocumentType.SelectedValue.ToString();
            //    SAPAccess.oOrder.ShipToCode = oAddressCode;
            //    SAPAccess.oOrder.DocCurrency = txtCurrency.Text;
            //    //SAPAccess.oOrder.DocRate = txtCurrency.Text != "PHP" ? 50.00D : 0;

            //    //Employee
            //    SAPAccess.oOrder.DocumentsOwner = Convert.ToInt32(SboCred.GetEmployeeCode());

            //    //Posting of UDF
            //    if (DECLARE.udf.Where(x => x.ObjCode == objType).ToList().Count > 0)
            //    {
            //        foreach (var x in DECLARE.udf.Where(x => x.ObjCode == objType))
            //        {
            //            SAPAccess.oOrder.UserFields.Fields.Item(x.FieldCode).Value = x.FieldValue;
            //        }
            //    }

            //    //Sales Employee
            //    if (txtSalesEmployee.Text != null)
            //    {
            //        SAPAccess.oOrder.SalesPersonCode = Convert.ToInt32(oSalesEmployee);
            //    }

            //    foreach (DataGridViewRow row in dgvBarcodeItems.Rows)
            //    {
            //        min++;

            //        double discperc = Convert.ToDouble(row.Cells["Discount %"].Value);
            //        double priveafvat = Convert.ToDouble(row.Cells["Unit Price"].Value);
            //        string itemCode = row.Cells[0].Value.ToString();
            //        string itemDesc = row.Cells[1].Value.ToString();
            //        string warehouse = txtWhsCode.Text;

            //        foreach (var x in DECLARE.so_items.Where(y => y.Quantity > 0 && y.ItemCode == itemCode && y.BaseEntry == txtSODocEntry.Text))
            //        {
            //            if (String.IsNullOrEmpty(x.BaseEntry) == false)
            //            {
            //                SAPAccess.oOrder.Lines.BaseEntry = Convert.ToInt32(x.BaseEntry);
            //                SAPAccess.oOrder.Lines.BaseLine = Convert.ToInt32(x.BaseLine);
            //                SAPAccess.oOrder.Lines.BaseType = 17;
            //            }
            //        }

            //        SAPAccess.oOrder.Lines.ItemCode = itemCode;
            //        SAPAccess.oOrder.Lines.ItemDescription = itemDesc;
            //        SAPAccess.oOrder.Lines.Quantity = Convert.ToDouble(row.Cells["Quantity"].Value.ToString());
            //        SAPAccess.oOrder.Lines.DiscountPercent = Convert.ToDouble(row.Cells["Discount %"].Value);
            //        SAPAccess.oOrder.Lines.WarehouseCode = warehouse;
            //        SAPAccess.oOrder.Lines.UnitPrice = priveafvat;
            //        SAPAccess.oOrder.Lines.UserFields.Fields.Item("U_SortCode").Value = row.Cells["SortCode"].Value;

            //        string brand = DataAccess.SearchData(DataAccess.conStr("HANA"), "SELECT U_ID019 FROM OITM Where ItemCode = '" + itemCode + "'", 0, "U_ID019");

            //        SAPAccess.oOrder.Lines.CostingCode2 = brand;
            //        SAPAccess.oOrder.Lines.COGSCostingCode2 = brand;
            //        SAPAccess.oOrder.Lines.ProjectCode = oProject;
            //        SAPAccess.oOrder.Lines.ShipDate = Convert.ToDateTime(row.Cells["DeliveryDate"].Value);

            //        SAPAccess.oOrder.Lines.Add();

            //        StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {min} out of {max}", min, max);
            //    }

            //    DECLARE dc = new DECLARE();

            //    SAPAccess.lRetCode = SAPAccess.oOrder.Add();

            //    if (SAPAccess.lRetCode == 0)
            //    {
            //        ClearData();

            //        string strDocEntry = "0";
            //        string DocNum = "0";
            //        SAPAccess.oCompany.GetNewObjectCode(out strDocEntry);
            //        SAPAccess.oOrder.GetByKey(Convert.ToInt32(strDocEntry));

            //        DocNum = SR.GetDocNum(strDocEntry, "ODLN");

            //        StaticHelper._MainForm.ProgressClear();
            //        StaticHelper._MainForm.ShowMessage($"Document No.{DocNum} has been successfully added", true);
            //        btnAdd.Enabled = true;
            //    }
            //    else
            //    {
            //        StaticHelper._MainForm.ProgressClear();
            //        StaticHelper._MainForm.ShowMessage(SAPAccess.oCompany.GetLastErrorDescription(), true);
            //        btnAdd.Enabled = true;
            //    }
            //}
        }


        private void UpdateUDF()
        {

            try
            {
                var _isError = false;
                int count = DECLARE.udf.Where(x => x.ObjCode == objType).Count();

                if (count > 0)
                {
                    if (DECLARE.udf.Where(x => x.ObjCode == objType).ToList().Count > 0)
                    {
                        int max = DECLARE.udf.ToList().Count;
                        int min = 0;
                        foreach (var x in DECLARE.udf.Where(x => x.ObjCode == objType))
                        {
                            min++;
                            if (((x.FieldCode != "U_CartonList") && (x.FieldValue != "")) == true)
                            {
                                if (DataAccess.Execute(DataAccess.conStr("HANA"), "Update ODLN Set " + x.FieldCode + " = '" + x.FieldValue + "' Where DocEntry =" + txtSODocEntry.Text + ""/*, frmMain*/) != true)
                                {
                                    _isError = true;
                                }
                            }
                            StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {min} out of {max}", min, max);
                        }
                        DataAccess.Execute(DataAccess.conStr("HANA"), "Update ODLN Set U_PrepBy = '" + SboCred.DBUserid + "' Where DocEntry =" + txtSODocEntry.Text + ""/*, frmMain*/);
                    }
                }

                if (_isError != true)
                {
                    ClearData();
                    // frmUDF._LoadData();

                    string DocNum = "0";
                    DocNum = txtSONumber.Text.ToString();
                    StaticHelper._MainForm.ProgressClear();
                    StaticHelper._MainForm.ShowMessage($"Document No.{DocNum} has been successfully updated", true);

                    btnAdd.Enabled = true;

                    NewDocument();
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Error in updating data", true);
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }

        }

        //public static bool ConnectToSAPDI()
        //{
        //    try
        //    {
        //        //CONNECTING TO SAP DATABASE
        //        SboDiApi sap = new SboDiApi();

        //        oCompany = sap.Connect(oCompany,
        //                                SboCred.LicenseServer,
        //                                SboCred.Server,
        //                                "dst_HANADB",
        //                                SboCred.DBUserid,
        //                                SboCred.DBPassword,
        //                                SboCred.Database,
        //                                SboCred.UserID,
        //                                SboCred.Password);

        //        if (sap.IsConnected(oCompany) == false)
        //        {
        //            PublicStatic.frmMain.NotiMsg(SboDiApi.ErrCode + "-" + SboDiApi.ErrMsg, Color.Red);
        //            return false;
        //        }
        //        else
        //        { return true; }

        //    }
        //    catch (Exception ex)
        //    {
        //        PublicStatic.frmMain.NotiMsg(ex.Message, Color.Red);
        //        return false;
        //    }
        //}

        public void ClearData()
        {
            ClearList();
            //frmUDF._LoadData();
            dgvBarcodeItems.Columns.Clear();

            foreach (Control c in panel2.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
            }

            NumSeries();
        }


        void NewDocument()
        {
            if (dgvBarcodeItems.Rows.Count > 0)
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    EnableControls();
                    ClearData();
                    FindMode = false;
                    Presenter.ClearField(true);
                    TabUS.SelectedIndex = 0;
                    btnAdd.Text = "Add";

                    if (this.Text != "Delivery")
                    {
                        LoadUnofficialSales();
                    }
                    DelDateSelRow = 0;
                    DelDateSelCol = -1;
                }
            }
            else
            {
                EnableControls();
                ClearData();
                btnAdd.Text = "&Add";
                FindMode = false;
            }
        }

        void EnableControls()
        {
            btnSearchItem.Enabled = true;
            txtDocStatus.Text = "Open";
            pbBPList.Visible = true;
            dtMonthofSales.Enabled = true;
            dtDocDate.Enabled = true;
            btnSearchItem.Enabled = true;
            cbSeries.Enabled = true;
            dgvBarcodeItems.ReadOnly = false;
            FindMode = false;
        }

        private void SalesEmployee()
        {
            ViewList("OSLP", out oCode, out oName, "List of Sales Employees");

            if (oCode != null)
            {
                txtSalesEmployee.Text = oName;
                oSalesEmployee = oCode;
            }
        }

        void Cancel()
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the Document? Unsaved data will be lost.", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ClearList();

                Form frm = Application.OpenForms["frmBarcodeEncoding_UDF"];
                if (frm != null)
                { /*frmUDF.Dispose();*/ }

                Dispose();
            }
        }

        void ClearList()
        {
            DECLARE.so_items.Clear();
            DECLARE.udf.RemoveAll(x => x.ObjCode == objType);
            UnofficialSalesItemsController.oRAS = "";
            UnofficialSalesItemsController.oTotalPrice = 0;
            UnofficialSalesItemsController.oTotalDiff = 0;
            UnofficialSalesItemsController.oTotalQty = 0;
            UnofficialSalesItemsController.oQtyDiff = 0;
        }

        private void DgvUdf_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Presenter.UdfRequest();
            txtDocEntry.Focus();
        }

        private void CheckDocumentTypeMaintenance()
        {
            try
            {
                if (USIC.SelValue("series1", cbDocumentType.Text) != "")
                {
                    cbSeries.SelectedIndex = cbSeries.FindString(USIC.SelValue("series1", cbDocumentType.Text));
                }

                txtWhsCode.Clear();

                if (USIC.SelValue("toWhs1", cbDocumentType.Text, txtBpCode.Text, txtAddress.Text) != "")
                {
                    txtWhsCode.Text = USIC.SelValue("toWhs1", cbDocumentType.Text, txtBpCode.Text, txtAddress.Text);
                }

                UnofficialSalesHeaderModel.oDocType = cbDocumentType.Text;

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private void LoadBlankDates()
        {

            //dtMonthofSales.CustomFormat = " ";
            //dtMonthofSales.Format = DateTimePickerFormat.Custom;

            dtDeliveryDate.CustomFormat = " ";
            dtDeliveryDate.Format = DateTimePickerFormat.Custom;

            //dtDocDate.CustomFormat = " ";
            //dtDocDate.Format = DateTimePickerFormat.Custom;
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
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, false);
            }
        }
        private void LoadWhsSalesType()
        {
            try
            {
                string sdata = $@"SELECT '' [Code] ,'' [Name] UNION SELECT FldValue as Code, Descr as Name FROM UFD1 WHERE TableID = 'ODLN' AND FieldID = (Select FieldID from CUFD where AliasID = 'WarehouseSalesType' and TableID = 'ODLN')";
                DataTable dt = hana.Get(sdata);
                cbWhsType.Items.Clear();
                Application.DoEvents();
                cbWhsType.DisplayMember = "Name";
                cbWhsType.ValueMember = "Code";
                cbWhsType.DataSource = dt;
                //cbWhsType.SelectedValue = "-";
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, false);
            }
        }

        private void cbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeSeries();
        }


        void changeSeries()
        {
            var sdata1 = $"SELECT T0.ObjectCode,T0.Series,T0.SeriesName,T0.NextNumber FROM NNM1 T0 Where T0.IsForCncl = 'N' and T0.ObjectCode = 15 And T0.SeriesName = '{cbSeries.Text}'";
            var dt1 = hana.Get(sdata1);

            if (dt1.Rows.Count > 0)
            {
                oSeries = DataAccess.Search(dt1, 0, "Series"/*, StaticHelper._MainForm*/);
                txtDocEntry.Text = DataAccess.Search(dt1, 0, "NextNumber"/*, StaticHelper._MainForm*/);
            }
        }

        private void txtDocEntry_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckDocumentTypeMaintenance();
        }


        private void dtDeliveryDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtWhsCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void pbWhse_Click(object sender, EventArgs e)
        {
            LoadWhseList();
        }


        private void LoadWhseList()
        {
            ViewList2("OWHS", out oCode, out oName, "List of Warehouses", "C");
            txtWhsCode.Text = "";

            if (oCode != null)
            {
                txtWhsCode.Text = oCode;
                UnofficialSalesHeaderModel.oWhsCode = oCode;
            }

            if (oCode != null && dgvBarcodeItems.RowCount > 0)
            {
                RefreshData();
            }
        }




        public void RefreshData([Optional] int index)
        {
            int i = 0;

            try
            {
                UnofficialSalesItemsController.dgvBarcodeItemsLayout(dgvBarcodeItems);

                int RowCnt = dgvBarcodeItems.Rows.Count;

                foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.OrderByDescending(x => x.Linenum).ToList())
                {
                    string findInstock = $"SELECT OnHand FROM OITW WHERE ItemCode = '{x.ItemCode}' and WhsCode = '{txtWhsCode.Text}' ";
                    var dt = hana.Get(findInstock);
                    string instock = "";

                    if (dt.Rows.Count > 0)
                    {
                        instock = DECLARE.dtNull(dt, 0, "OnHand", " ") ?? " ";
                    }

                    object[] a = { x.ItemCode, x.ItemName, x.GrossPrice, x.UnitPrice, x.DiscountAmount
                                , x.DiscountPerc, instock, x.LineTotal, x.Linenum, 0, true
                                , x.PriceBefDisc, x.Style, x.Color, x.Size, x.Section, x.SortCode, x.ItemProperty, x.DelDate };

                    dgvBarcodeItems.Rows.Insert(RowCnt, a);
                    //dgvBarcodeItems.Rows.Add(a);
                }

                UnofficialSalesItemsController.dataGridLayout(dgvBarcodeItems);
                //dgvBarcodeItems.Sort(dgvBarcodeItems.Columns["SortCode"], ListSortDirection.Ascending);
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ComputeTotal(double disPer = 0)
        {
            try
            {
                double dbleDiscPercent = 0;
                double dbleTotalQty = 0;
                double dbleTotaLT = 0;
                double dbleTotalDisc = 0;
                double dbleTotalTax = 0;
                double dbleBefPrice = 0;
                double dbleTotalGroPrice = 0;
                double dbleTotalGroPriceDisc = 0;

                foreach (DataGridViewRow row in dgvBarcodeItems.Rows)
                {
                    double dblQty = Convert.ToDouble(DECLARE.Replace(row, "Quantity", "0.00"));
                    double dblPriceAfterDisc = Convert.ToDouble(DECLARE.Replace(row, "PriceBeforeDiscount", "0.00"));
                    if (oOneTime == "Y" && cbWhsType.SelectedValue.ToString() == "DP")
                    {
                    }
                    //double dblPriceAfterDisc = Convert.ToDouble(DECLARE.Replace(row, "Unit Price", "0.00"));
                    //double dblGroPrice = dblQty * Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0.00"));
                   
                    double dblGroPrice = dblQty * (Convert.ToDouble(DECLARE.Replace(row, "Gross Price", "0.00")) - Convert.ToDouble(DECLARE.Replace(row, "Discount Amount", "0.00")));

                    dbleTotalQty += dblQty;
                    //dbleBefPrice += (Convert.ToDouble(dblPriceAfterDisc) * Convert.ToDouble(dblQty));
                    dbleBefPrice += Convert.ToDouble(DECLARE.Replace(row, "Total", "0.00"));
                    dbleTotalDisc += Convert.ToDouble(DECLARE.Replace(row, "Discount Amount", "0.00"));
                    dbleTotalGroPrice += dblGroPrice;
                }

                txtTotalQty.Text = Convert.ToInt32(dbleTotalQty).ToString();
                txtTotalBefDisc.Text = dbleBefPrice.ToString("#,##0.00");

                var disc = !string.IsNullOrEmpty(txtDiscPercent.Text) ? Convert.ToDouble(txtDiscPercent.Text) : 0;

                if (oOneTime == "Y" && cbWhsType.SelectedValue.ToString() != "DP")
                {
                    txtDiscPercent.Text = "0";
                }
                else
                {
                    ComputeWSDisc(disPer, dbleTotalGroPrice);
                }

                if (txtDiscPercent.Text != "")
                {
                    dbleDiscPercent = Convert.ToDouble(txtDiscPercent.Text);
                }
                //double checkDiscPercent = txtDiscPercent.Text != string.Empty ? (dbleTotaLT - dbleTotalDisc) * (dbleDiscPercent / 100) : 0;

                if (txtDiscPercent.Text != "" && txtDiscPercent.Text != "0" && txtDiscPercent.Text != "0.000000")
                {
                    double dbl_DiscPerc = dbleDiscPercent / 100;
                    //double taxpercent = (dbleTotalTax * dbl_DiscPerc);
                    //dbleTotalTax = dbleTotalTax - taxpercent;
                    dbleTotalGroPrice = dbleTotalGroPrice - (dbleTotalGroPrice * dbl_DiscPerc);

                }

                double checkDiscPercent = txtDiscPercent.Text != string.Empty ? (dbleBefPrice / 100) * dbleDiscPercent : 0;
                txtDiscount.Text = checkDiscPercent.ToString("#,##0.00");

                //txtTaxAmount.Text = dbleTotalTax.ToString("#,##0.00");
                var gp = Math.Truncate(dbleTotalGroPrice * 100) / 100;
                txtTotal.Text = gp.ToString();

                //ComputeWSDisc();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, false);
            }
        }


        private void txtBpCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            if (dtMonthofSales.Text != " ")
            {
                if (oOneTime == "Y" && cbWhsType.SelectedValue.ToString() == "")
                {
                    StaticHelper._MainForm.ShowMessage("Please select warehouse sales type.", true);
                }
                else
                {
                    AddItem();
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Please select a date for the Month of sale", true);
            }
        }

        private void AddItem()
        {
            if (txtBpCode.Text != "")
            {
                if (cbDocumentType.Text != "")
                {
                    if (txtWhsCode.Text != "")
                    {
                        if (dtMonthofSales.Text != " " || dtDeliveryDate.Text != " " || dtDocDate.Text != " ")
                        {
                            FrmUnofficialSalesItemList frmUOSIN = new FrmUnofficialSalesItemList(this, frmMain);
                            frmUOSIN.ShowDialog();

                            int max = frmUOSIN.oCodeList.Count;
                            int min = 0;

                            foreach (string x in frmUOSIN.oCodeList)
                            {
                                StaticHelper._MainForm.Progress($"Please wait until all data are loaded. ({min}/{max}) ", min, max);
                                txtItemCode.Text = x;
                                EnterBarCode();
                                txtItemCode.Text = "";
                                min++;
                            }

                            StaticHelper._MainForm.ProgressClear();
                        }
                        else
                        {
                            StaticHelper._MainForm.ShowMessage("Please fill in the dates first.", true);
                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Please select a warehouse.", true);
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Please select a Document Type.", true);
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Please select a customer", true);
            }
        }



        void EnterBarCode()
        {
            string barcode_string = txtItemCode.Text;

            List<string> barcode = barcode_string.Split('|').ToList<string>();

            //string query = "SELECT * from ( SELECT TOP 1 " +

            string query = string.Format(helper.ReadDataRow(hana.Get(SP.US_EnterBarCode), 1, "", 0), barcode[0]);
            //

            if (hana.Get(query).Rows.Count == 0)
            {
                query = string.Format(helper.ReadDataRow(hana.Get(SP.US_BarCodeNotExist), 1, "", 0), barcode[0], txtBpCode.Text);
            }
            else
            {
                query = string.Format(helper.ReadDataRow(hana.Get(SP.US_BarCodeExist), 1, "", 0), txtBpCode.Text, barcode[0]);
            }

            var dt = hana.Get(query);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 1)
                {
                    StaticHelper._MainForm.ShowMessage("UPC/SKU has more than one item.", true);
                    frmDuplicateItemSelection frmDIS = new frmDuplicateItemSelection(this);
                    frmDIS.strBPcode = txtBpCode.Text;
                    frmDIS.strBarCode = barcode[0];
                    frmDIS.ShowDialog();

                    if (frmDIS.oSelectedCode != "" && frmDIS.oSelectedCode != null)
                    {
                        txtItemCode.Text = frmDIS.oSelectedCode;
                        EnterBarCode();
                        txtItemCode.Text = "";
                        frmDIS.oSelectedCode = "";
                    }
                }
                else
                {
                    bool AllowDupItem = true;
                    string itemCode = dt.Rows[0][0].ToString();

                    if (USIC.SelValue("AllowDupItems", UnofficialSalesHeaderModel.oDocType) != "Y" && DECLARE.so_items.Where(x => x.ItemCode == itemCode).ToList().Count() >= 1)
                    {
                        AllowDupItem = false;
                    }

                    if (AllowDupItem)
                    {

                        string itemName = dt.Rows[0][1].ToString();
                        string price = dt.Rows[0][2].ToString();
                        double DocTotal = Convert.ToDouble(price) * 1;
                        double Discount = Convert.ToDouble(dt.Rows[0][3].ToString());
                        double PriceBeforeDiscount = Convert.ToDouble(dt.Rows[0][4].ToString());

                        double tax = 0;

                        try
                        {
                            tax = Convert.ToDouble(hana.Get($@"SELECT T0.ECVatGroup, T1.Rate FROM OCRD T0  INNER JOIN OVTG T1 ON T0.ECVatGroup = T1.Code WHERE T0.CardCode = '{BPCode}'").Rows[0]["Rate"].ToString());
                        }
                        catch (Exception e)
                        {
                            StaticHelper._MainForm.ShowMessage($"Please specify vat group for {BPCode}.", true);
                            return;
                        }

                        tax = 1 + (tax / 100);

                        double GrossPrice = Convert.ToDouble((PriceBeforeDiscount * tax).ToString("#0.00"));
                        string oStyle = dt.Rows[0]["Style"].ToString();
                        string oColor = dt.Rows[0]["Color"].ToString();
                        string oSize = dt.Rows[0]["Size"].ToString();
                        string oSection = dt.Rows[0]["Section"].ToString();
                        double dblPrice = Math.Round(Convert.ToDouble(price), 2);
                        double dblDiscPerc = Math.Round(Discount, 2);
                        double dblDiscAmt = Math.Round(Convert.ToDouble(price) * (Discount / 100), 2);
                        double dblTotal = Math.Round(DocTotal - (DocTotal * (Discount / 100)), 2);
                        string strSortCode = dt.Rows[0]["SortCode"].ToString();
                        string strItmProp = dt.Rows[0]["ItemProperty"].ToString();

                        if (itemCode != "")
                        {
                            int iIndex = index++;
                            string GetCompany = hana.Get(Query.GetCompanyPerLine(txtBpCode.Text, itemCode)).Rows.Count > 0 ? hana.Get(Query.GetCompanyPerLine(txtBpCode.Text, itemCode)).Rows[0].ItemArray[0].ToString() : "";
                            string company = GetCompany != "" ? hana.Get(Query.GetCompanyQuerySearch(GetCompany)).Rows[1].ItemArray[1].ToString() : "";
                            UnofficialSalesItemsModel.UnofficialSalesItems.Add(new UnofficialSalesItemsModel.UnofficialSalesItemsData
                            {
                                ItemCode = itemCode,
                                ItemName = itemName,
                                GrossPrice = GrossPrice,
                                UnitPrice = dblPrice,
                                Quantity = 1,
                                DiscountAmount = dblDiscAmt,
                                DiscountPerc = dblDiscPerc,
                                LineTotal = dblTotal,
                                Linenum = iIndex,
                                PriceBefDisc = PriceBeforeDiscount,
                                Style = oStyle,
                                Color = oColor,
                                Size = oSize,
                                Section = oSection,
                                SortCode = strSortCode,
                                ItemProperty = strItmProp
                                ,
                                Company = company
                            });

                            int RowCnt = dgvBarcodeItems.Rows.Count;

                            if (RowCnt == 0)
                            {
                                UnofficialSalesItemsController.dgvBarcodeItemsLayout(dgvBarcodeItems);
                            }

                            object[] a = { itemCode, itemName, GrossPrice, dblPrice, 1, dblDiscAmt
                                        , dblDiscPerc, 0, dblTotal, iIndex, 0, true
                                        , PriceBeforeDiscount, oStyle, oColor, oSize, oSection, strSortCode, strItmProp, "", "", company };

                            dgvBarcodeItems.Rows.Insert(RowCnt, a);
                            dgvBarcodeItems.FirstDisplayedScrollingRowIndex = dgvBarcodeItems.RowCount - 1;

                            UpdateTotal();
                            GetDelDate(itemCode);

                            UnofficialSalesItemsController.dataGridLayout(dgvBarcodeItems);

                            //UpdateTotal();
                            //RefreshData();
                            //GetDelDate(itemCode);
                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Duplicate item cannot be added, please check SAP Document Type maintenance.", true);
                    }
                }

            }
            else
            {

                string strStatus = "Item does not exist in SAP";
                string selqry = "SELECT * FROM (select A.ItemCode, A.CodeBars from OITM A) MT1 WHERE" +
                                $" MT1.ItemCode LIKE '%{barcode[0]}%' " +
                                $" OR MT1.CodeBars LIKE '%{barcode[0]}%' ";

                if (hana.Get(selqry).Rows.Count > 0)
                {
                    strStatus = "YES";
                    StaticHelper._MainForm.ShowMessage("Item does not exist in SKU Maintenance", true);
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Item does not exist in SAP", true);
                }

                USIC.CreateLogs(txtBpCode.Text, dtMonthofSales.Text, barcode[0], strStatus);
                txtItemCode.Text = "";
                txtItemCode.Focus();
            }
        }


        void GetDelDate(string oItemCode)
        {
            try
            {
                int dc = dgvBarcodeItems.Rows.Count;
                string FinalDate = "";
                string DocDate = dtMonthofSales.Value.ToString("MM/dd/yyyy");
                if (dc == 1)
                {
                    dgvBarcodeItems.Rows[0].Cells["DeliveryDate"].Value = DocDate;
                    FinalDate = DocDate;
                }
                else
                {
                    int rc = dc - 2;
                    int nc = dc - 1;
                    string LastDatePerRow = dgvBarcodeItems.Rows[rc].Cells["DeliveryDate"].Value.ToString();
                    if (LastDatePerRow == DocDate)
                    {
                        dgvBarcodeItems.Rows[nc].Cells["DeliveryDate"].Value = DocDate;
                        FinalDate = DocDate;
                    }
                    else
                    {
                        dgvBarcodeItems.Rows[nc].Cells["DeliveryDate"].Value = LastDatePerRow;
                        FinalDate = LastDatePerRow;
                    }
                }

                SaveDelDate(oItemCode, FinalDate);
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        void SaveDelDate(string oItemCode, string DelDate)
        {
            foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.Where(x => x.ItemCode == oItemCode))
            {
                x.DelDate = DelDate;
            }
        }

        void UpdateTotal()
        {

            DocTotal = 0;
            double TotalQty = 0;
            double dbleDiscPercent = 0;
            double dblPriceBefDisc = 0;
            double dblGrossPrice = 0;
            var dDiscAmount = 0.0;

            txtItemCode.Text = "";
            txtItemCode.Focus();

            UnofficialSalesItemsModel.UnofficialSalesItems.ForEach(item =>
            {
                //DocTotal += item.LineTotal;
                DocTotal += item.UnitPrice * item.Quantity;
                TotalQty += item.Quantity;
                dblPriceBefDisc += item.LineTotal; //item.PriceBefDisc * item.Quantity;
                
                dblGrossPrice += (item.GrossPrice - item.DiscountAmount) * item.Quantity;
            });

            //added by Cedi on 061119
            if (txtDiscPercent.Text != "")
            {
                dbleDiscPercent = Convert.ToDouble(txtDiscPercent.Text);
            }
            if (txtDiscount.Text != "")
            {
                dDiscAmount = double.Parse(txtDiscount.Text);
            }

            double checkDiscPercent = txtDiscPercent.Text != string.Empty ? (dblPriceBefDisc / 100) * dbleDiscPercent : 0;
            double GetDiscPercent = txtDiscount.Text != string.Empty ? (dDiscAmount / dblPriceBefDisc) * 100 : 0;

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
                double dbl_DiscPerc = Convert.ToDouble(txtDiscPercent.Text) / 100;
                dblGrossPrice = dblGrossPrice - (dblGrossPrice * dbl_DiscPerc);
            }

            txtTotalQty.Text = TotalQty.ToString();
            txtTotalBefDisc.Text = dblPriceBefDisc.ToString("#,##0.00");
            //txtTotal.Text = String.Format(dblGrossPrice.ToString(), "{0:N}");
            var gp = Math.Truncate(dblGrossPrice * 100) / 100;
            txtTotal.Text = gp.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvBarcodeItems.Rows.Count > 0)
            {
                if (txtBpCode.Text != string.Empty)
                {
                    btnAdd.Enabled = false;
                    if (Presenter.ExecuteRequest(btnAdd.Text))
                    {
                        btnAdd.Text = btnAdd.Text == "Update" ? "Add" : "Add";
                        NewDocument();
                    }
                    btnAdd.Enabled = true;
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Please select an item(s) before adding.", true);
            }
        }

        private void DgvFindDocument_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvFindDocument.CurrentRow.Selected = true;
        }


        private void dgvBarcodeItems_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                foreach (DataGridViewRow row in dgvBarcodeItems.Rows)
                {
                    string itemCode = row.Cells[0].Value.ToString();
                    double price = Convert.ToDouble(row.Cells["Unit Price"].Value);
                    double qty = Convert.ToDouble(row.Cells["Quantity"].Value);
                    double discPerc = Convert.ToDouble(row.Cells["Discount %"].Value);

                    DECLARE.so_items.Find(x => x.ItemCode == itemCode).Quantity = qty;
                    DECLARE.so_items.Find(x => x.ItemCode == itemCode).Price = price;
                    DECLARE.so_items.Find(x => x.ItemCode == itemCode).DiscPerc = discPerc;
                    DECLARE.so_items.Find(x => x.ItemCode == itemCode).DiscAmount = price * (discPerc / 100) * qty;
                    DECLARE.so_items.Find(x => x.ItemCode == itemCode).Total = (price * qty) - price * (discPerc / 100) * qty;
                }

                ComputeTotal();
                RefreshData();
            }

        }


        private void txtItemCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtBpName.Text != string.Empty && txtItemCode.Text != string.Empty)
            {
                if (cbDocumentType.Text != "")
                {
                    if (dtMonthofSales.Text != " " && dtDocDate.Text != " ")
                    {
                        if (txtWhsCode.Text != "")
                        {
                            if (oOneTime == "Y" && cbWhsType.SelectedValue.ToString() == "")
                            {
                                StaticHelper._MainForm.ShowMessage("Please select warehouse sales type.", true);
                            }
                            else
                            {
                                EnterBarCode();
                            }
                        }
                        else
                        {
                            StaticHelper._MainForm.ShowMessage("Please select a warehouse.", true);
                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Please fill in the dates first.", true);
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Please select a Document Type.", true);
                }
            }
        }

        private void dgvBarcodeItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex - 1;
            colIndex = colIndex < 0 ? 0 : colIndex;

            string columnName = dgvBarcodeItems.Columns[colIndex].Name;
            int index = Convert.ToInt32(dgvBarcodeItems.CurrentRow.Cells["index"].Value.ToString());

            switch (columnName)
            {
                case "Company":
                    var Company = SelectCompany();
                    if (Company != string.Empty)
                    {
                        dgvBarcodeItems.CurrentRow.Cells[colIndex].Value = Company;
                        UnofficialSalesItemsModel.UnofficialSalesItems.Find(x => x.Linenum == index).Company = Company;
                    }
                    break;
            }

        }


        private void dgvBarcodeItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (DelDateSelRow != e.RowIndex)
                {
                    oDateTimePicker.Visible = false;
                }
                if (e.ColumnIndex == 20)
                {
                    if (oDateTimePicker.Visible == true)
                    {
                        oDateTimePicker.Visible = false;
                    }
                    else
                    {
                        DelDateSelRow = e.RowIndex;
                        DelDateSelCol = 19;
                        oDateTimePicker = new DateTimePicker();
                        dgvBarcodeItems.Controls.Add(oDateTimePicker);
                        oDateTimePicker.Format = DateTimePickerFormat.Short;
                        Rectangle oRectangle = dgvBarcodeItems.GetCellDisplayRectangle(19, e.RowIndex, true);
                        oDateTimePicker.Size = new Size(oRectangle.Width, oRectangle.Height);
                        oDateTimePicker.Location = new Point(oRectangle.X, oRectangle.Y);
                        if (dgvBarcodeItems.Rows[DelDateSelRow].Cells["DeliveryDate"].Value != null && dgvBarcodeItems.Rows[DelDateSelRow].Cells["DeliveryDate"].Value.ToString() != "")
                        {
                            oDateTimePicker.Value = Convert.ToDateTime(dgvBarcodeItems.Rows[DelDateSelRow].Cells["DeliveryDate"].Value.ToString());
                        }
                        oDateTimePicker.TextChanged += new EventHandler(DateTimePickerChange);
                        oDateTimePicker.CloseUp += new EventHandler(DateTimePickerClose);
                    }
                }
                else
                {
                    oDateTimePicker.Visible = false;
                    DelDateSelCol = e.ColumnIndex;
                    DelDateSelRow = e.RowIndex;
                }
            }
        }

        private void DateTimePickerChange(object sender, EventArgs e)
        {
            SetDelDate();
        }

        private void DateTimePickerClose(object sender, EventArgs e)
        {
            oDateTimePicker.Visible = false;
        }

        private void SetDelDate()
        {
            dgvBarcodeItems.Rows[DelDateSelRow].Cells[19].Value = oDateTimePicker.Value.ToString("MM/dd/yyyy");
            string ItemCode = dgvBarcodeItems.Rows[DelDateSelRow].Cells["Item No."].Value.ToString();
            SaveDelDate(ItemCode, oDateTimePicker.Value.ToString("MM/dd/yyyy"));
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
                TabUS.SelectedIndex = 0;
                //UpdateTotal();
                btnAdd.Text = "Update";
                dtMonthofSales.Enabled = false;
                btnSearchItem.Enabled = false;
                dgvBarcodeItems.Enabled = true;
                dtDocDate.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmUnofficialSales_Resize(object sender, EventArgs e)
        {
            this.MdiParent = StaticHelper._MainForm;
            FormHelper.ResizeForm(this);
        }

        private void txtDiscPercent_TextChanged(object sender, EventArgs e)
        {
            //GetDiscAmount();
            //ComputeTotal();
        }

        private void btnCopyFrom_Click(object sender, EventArgs e)
        {
            CmbCopyFromOption.DroppedDown = CmbCopyFromOption.DroppedDown == false ? true : false;
        }

        private void CmbCopyFromOption_Click(object sender, EventArgs e)
        {
            Presenter.ExecuteCopyDocument(CmbCopyFromOption.Text, this);
        }

        private void DgvFindDocument_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ChooseDocument();
        }

        private void FrmUnofficialSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ClearData();
                Presenter.ClearField(true);
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void txtDiscPercent_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtDiscPercent.Focused == true)
            {
                ComputeTotal(Convert.ToDouble(txtDiscPercent.Text));
                //GetDiscAmount();
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
                        DiscType = "GetAmount";
                        UpdateTotal();
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
                        DiscType = "GetPercent";
                        UpdateTotal();
                    }
                    oDiscount = Convert.ToDouble(txtDiscPercent.Text);
                }

            }
            catch (Exception ex)
            {
                txtDiscPercent.Text = "";
            }
        }

        private void DgvUdf_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DgvUdf_CellClick(sender, e);
        }

        private void PbSalesEmployee_Click(object sender, EventArgs e)
        {
            ViewList("OSLP", out oCode, out oName, "List of Sales Employees");

            if (oCode != null)
            {
                txtSalesEmployee.Text = oName;
                oSalesEmployee = oCode;
                //oSalesEmployeeName = oName;
            }
        }

        private void BtnNewDocument_Click(object sender, EventArgs e)
        {
            NewDocument();
        }

        private void pbBPList_Click(object sender, EventArgs e)
        {
            if (dgvBarcodeItems.Rows.Count > 0)
            {
                NewDocument();
            }
            else
            {
                ViewList("OCRD", out oCode, out oName, "List of Customers", "C");
                UnofficialSalesHeaderModel.oBPCode = "";

                if (oCode != null)
                {
                    txtBpCode.Text = oCode;
                    //PublicStatic.DeliveryCardCode = oCode;
                    UnofficialSalesHeaderModel.oBPCode = oCode;
                    LoadBPDetails(oCode);
                }
                if (oOneTime.Equals("Y"))
                {
                    cbDocumentType.SelectedValue = "Warehouse Sale";
                }
                else
                {
                    cbDocumentType.SelectedValue = "Unofficial Sales";
                }

                CheckDocumentTypeMaintenance();
            }
        }

        private void DgvUdf_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DgvUdf.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                DgvUdf.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
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
                cryRpt.SetParameterValue("DocKey@", txtDocEntry.Text);
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

        private void txtTotalBefDisc_TextChanged(object sender, EventArgs e)
        {
            //var disc = !string.IsNullOrEmpty(txtDiscPercent.Text) ? Convert.ToDouble(txtDiscPercent.Text) : 0;
            //ComputeWSDisc(disc);
        }

        private void TabUS_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TabUS.SelectedIndex)
            {
                case 0:

                    if (dgvBarcodeItems.Rows.Count > 0)
                    {
                        Presenter.LoadData(dgvBarcodeItems);

                        if (cbDocumentType.Text == "Service" && btnAdd.Text == "Update")
                        {
                            dgvBarcodeItems.Rows.RemoveAt(0);
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
                        TabUS.SelectedIndex = 3;
                        TxtPrintDocNo.Text = txtDocEntry.Text;
                    }
                    break;

                default:
                    break;
            }
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            if (cbWhsType.SelectedValue.ToString() != "-")
            {
                foreach (DataGridViewRow row in DgvUdf.Rows)
                {
                    if (row.Cells[1].Value.ToString().Equals("Promo Total"))
                    {
                        var gross = txtTotal.Text.Replace(",", "");
                        DgvUdf.Rows[row.Index].Cells["Field"].Value = gross;
                        break;
                    }
                }
            }
        }

        private void txtDiscPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                 (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtDiscPercent_MouseLeave(object sender, EventArgs e)
        {
            //ComputeTotal();
        }

        private void txtTotalQty_TextChanged(object sender, EventArgs e)
        {
            if (cbWhsType.SelectedValue.ToString() != "-")
            {
                foreach (DataGridViewRow row in DgvUdf.Rows)
                {
                    if (row.Cells[1].Value.ToString().Equals("Promo Quantity"))
                    {
                        DgvUdf.Rows[row.Index].Cells["Field"].Value = txtTotalQty.Text;
                        break;
                    }
                }
            }
        }

        private void cbWhsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbWhstype = string.IsNullOrEmpty(cbWhsType.SelectedValue.ToString()) ? "" : (cbWhsType.SelectedValue.ToString().Equals("-") ? "" : cbWhsType.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(cbWhstype))
            {
                cbDocumentType.SelectedValue = "Warehouse Sale";
                var dt = hana.Get("SELECT TOP 1 T0.CardCode, T0.CardName FROM OCRD T0 WHERE T0.QryGroup17 = 'Y'");
                if (dt.Rows.Count > 0)
                {
                    oCode = dt.Rows[0][0].ToString();
                    oName = dt.Rows[0][1].ToString();

                    UnofficialSalesHeaderModel.oBPCode = "";

                    if (oCode != null)
                    {
                        txtBpCode.Text = oCode;
                        //PublicStatic.DeliveryCardCode = oCode;
                        UnofficialSalesHeaderModel.oBPCode = oCode;
                        LoadBPDetails(oCode);
                    }
                    CheckDocumentTypeMaintenance();

                }
            }
            else
            {
                cbDocumentType.SelectedValue = "Unofficial Sales";
                if (dgvBarcodeItems.Rows.Count > 0)
                {
                    NewDocument();
                }
                else
                {
                    UnofficialSalesHeaderModel.oBPCode = "";
                    oCode = string.Empty;
                    oName = string.Empty;
                    LoadBPDetails(oCode);

                    CheckDocumentTypeMaintenance();
                }
            }
            if (dgvBarcodeItems.Rows.Count > 0)
            {
                ComputeTotal();
            }
        }

        private void txtItemCode_MouseClick(object sender, MouseEventArgs e)
        {
            DelDateSelRow = 0;
            DelDateSelCol = -1;
        }

        private void txtItemCode_TextChanged(object sender, EventArgs e)
        {

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


        private void ViewList2(string SearchTable
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

            var dt = hana.Get(query);

            if (dt.Rows.Count > 0)
            {
                txtBpName.Text = DECLARE.dtNull(dt, 0, "CardName", "");
                txtAddress.Text = DECLARE.dtNull(dt, 0, "AddressID", "");
                oSalesEmployee = DECLARE.dtNull(dt, 0, "SlpCode", "");
                txtSalesEmployee.Text = DECLARE.dtNull(dt, 0, "SlpName", "");
                //oPriceList = DECLARE.dtNull(dt, 0, "ListNum", "0");
                UnofficialSalesHeaderModel.oPriceList = oPriceList;
                txtBpName.Text = DECLARE.dtNull(dt, 0, "CardName", "");
                oTaxGroup = DECLARE.dtNull(dt, 0, "ECVatGroup", "");
                oProject = DECLARE.dtNull(dt, 0, "ProjectCod", "");
                sOProject = oProject;
                oAddressCode = DECLARE.dtNull(dt, 0, "AddressCode", "");
                oFWhsCode = DataAccess.SearchData(DataAccess.conStr("HANA"), $"Select U_Whs FROM CRD1 Where CardCode = '{CardCode}'", 0, "U_Whs"/*, frmMain*/);
                //txtPriceList.Text = oPriceList;
                // txtTaxGroup.Text = oTaxGroup;
                //UnofficialSalesHeaderModel.oTaxGroup = oTaxGroup;
                txtWhsCode.Text = oFWhsCode;
                //txtContactPerson = "";
                // string query1 = "Select Code,Name,Rate From OVTG Where Code = '" + oTaxGroup + "'";

                var dt1 = new DataTable();
                // dt1 = hana.Get(query1);

                //if (dt1.Rows.Count > 0)
                //{
                //    oTaxRate = DECLARE.dtNull(dt1, 0, "rate", "");
                //}

                if (FromFind == false)
                {
                    LoadRAS(CardCode);
                }
            }
        }


        private void LoadRAS(string CardCode)
        {
            string selqry = $"SELECT distinct ifnull(upper(b.firstName),'') +' ' + ifnull(upper(b.lastName), '') [FullName] " +
                            $" FROM OCRD a LEFT JOIN OHEM b ON a.U_Dim3 = b.CostCenter " +
                            $" LEFT JOIN OHPS c ON b.position = c.posID " +
                            $" where a.CardCode = '{txtBpCode.Text}'  and c.posID IN('15','18')";

            if (hana.Get(selqry).Rows.Count > 0)
            {
                UnofficialSalesItemsController.oRAS = hana.Get(selqry).Rows[0]["FullName"].ToString();
            }
            else
            {
                UnofficialSalesItemsController.oRAS = "";
            }
            //frmUDF.dataGridLayout(UOSudf.gvUDF);
            //frmUDF._LoadData();
        }

        public string SelectCompany()
        {
            string result = "";
            var list = Modal("@CMP_INFO", null, "List of Companies");

            if (list.Count > 0)
            {
                result = list[1];
            }

            return result;
        }

        static List<string> Modal(string searchKey, List<string> Parameters, string title)
        {
            List<string> modalValue = new List<string>();

            frmSearch2 fS = new frmSearch2()
            {
                oSearchMode = searchKey,
                oFormTitle = title
            };

            if (Parameters != null)
            {
                for (int i = 0; Parameters.Count > i; i++)
                {
                    switch (i)
                    {
                        case 0:
                            frmSearch2.Param1 = Parameters[i].ToString();
                            break;

                        case 1:
                            frmSearch2.Param2 = Parameters[i].ToString();
                            break;

                        case 2:
                            frmSearch2.Param3 = Parameters[i].ToString();
                            break;

                        case 3:
                            frmSearch2.Param4 = Parameters[i].ToString();
                            break;

                        case 4:
                            frmSearch2.Param5 = Parameters[i].ToString();
                            break;
                    }
                }
            }

            fS.ShowDialog();

            if (fS.oCode != null)
            {
                modalValue.Add(fS.oCode);
            }

            if (fS.oName != null)
            {
                modalValue.Add(fS.oName);
            }

            if (fS.oRate != null)
            {
                modalValue.Add(fS.oRate);
            }

            return modalValue;
        }
        void ComputeWSDisc(double disc, double grosstotal)
        {

            if (oOneTime == "Y" && cbWhsType.SelectedValue.ToString() == "DP")
            {
                var grossDisc = grosstotal > 0 ? grosstotal : 0;
                var befDisc = !string.IsNullOrEmpty(txtTotalBefDisc.Text) ? Convert.ToDouble(txtTotalBefDisc.Text) : 0;
                //var befDisc = 10000;
                var sQuery = $"SELECT U_Discount FROM [@WHS_SALES] WHERE {grossDisc} BETWEEN U_AmountFrom AND U_AmountTo";
                var dp = helper.ReadDataRow(hana.Get(sQuery), 0, "", 0);

                if (string.IsNullOrEmpty(dp))
                {
                    sQuery = $"SELECT CASE WHEN MAX(U_AmountFrom) > MAX(U_AmountTo) THEN MAX(U_AmountFrom) ELSE MAX(U_AmountTo) END [MAXCOLLINS] FROM [@WHS_SALES]";
                    var maxAmnt = helper.ReadDataRow(hana.Get(sQuery), 0, "", 0);

                    if (grossDisc >= Convert.ToDouble(maxAmnt))
                    {
                        sQuery = $"SELECT U_Discount FROM [@WHS_SALES] WHERE U_AmountFrom = {maxAmnt} or U_AmountTo = {maxAmnt}";
                        dp = helper.ReadDataRow(hana.Get(sQuery), 0, "", 0);
                    }
                }


                var disp = disc != 0 ? disc : (!string.IsNullOrEmpty(dp) ? Convert.ToDouble(dp) : 0);

                txtDiscPercent.Text = disp.ToString();

                if (!string.IsNullOrEmpty(dp))
                {
                    var totalDisc = befDisc * (disp / 100);
                    txtDiscount.Text = totalDisc.ToString();
                    //txtTotal.Text = (disc - totalDisc).ToString();
                }
            }
        }
    }
}

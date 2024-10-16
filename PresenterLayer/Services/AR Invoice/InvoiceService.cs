using DirecLayer;
using DomainLayer;
using DirecLayer._03_Repository;
using DirecLayer._05_Repository;
using PresenterLayer.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using PresenterLayer.Views;
using MetroFramework;
using System.IO;
using EasySAP;
using DomainLayer.Helper;
using PresenterLayer.Services.Security;

using zDeclare;

namespace PresenterLayer.Services
{
    public class InvoiceService
    {
        private readonly IFrmARInvoice _View;
        private readonly IARInvoiceModel _repository;
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        PurchasingAP_Style _style = new PurchasingAP_Style();
        PurchasingAP_Computation _computation = new PurchasingAP_Computation();
        ValidationRepository _validation = new ValidationRepository();
        UdfRepository _udfRepo = new UdfRepository();
        StringQueryRepository _reps = new StringQueryRepository();
        SAPHanaAccess Hana = new SAPHanaAccess();
        //private FrmSalesOrder view;
        //private Models.SalesOrderModel model;
        DateTimePicker oDateTimePicker = new DateTimePicker();

        public InvoiceService(IFrmARInvoice view, IARInvoiceModel repository)
        {
            _View = view;
            _View.Presenter = this;
            _repository = repository;
            hana = new SAPHanaAccess();
            helper = new DataHelper();
            Onload();
        }


        internal void Onload()
        {
            DataTable udf = _repository.GetUDF();

            _udfRepo.Style(_View.Udf);
            _udfRepo.LoadUdf(_View.Udf, udf, "OINV");

            _View.Status = "Open";
        }

        internal DataTable GetCurrencies()
        {
            DataTable currencies = _repository.GetCurrencyCodes();

            if (currencies == null)
            {
                return null;
            }
            else if (currencies.Rows.Count <= 0)
            {
                return null;
            }

            return currencies;
        }

        internal bool InternationalBP(bool isLocalBP = false)
        {
            return isLocalBP;
        }

        internal void GetSupplierInformation()
        {
            var m = _repository.SelectSupplier();

            if (m.Count > 0)
            {
                _View.SuppCode = m["SupplierCode"];
                _View.SuppName = m["SupplierName"];
                _View.ContactPerson = m["ContactPerson"];

                _View.BpCurrency = m["Currency"];
                _View.BpRate = m["CurrencyRate"];

                _View.VatGroup = m["VatGroup"];
                _View.VatGroupRate = _repository.GetVatGroupRate(_View.VatGroup);
                _View.Warehouse = m["Warehouse"];

                _View.RawCurrency = m["RawCurrency"];

                string transtype = _repository.AutomateTransferType(_View.SuppCode);
                string orderNo = _repository.GetOrderNo(_View.SuppCode);

                _View.Udf.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[1].Value != null)
                    {
                        if (x.Cells[1].Value.ToString() == "Transaction Type")
                        {
                            x.Cells[2].Value = transtype;
                        }
                        else if (x.Cells[1].Value.ToString() == "Order No.")
                        {
                            x.Cells[2].Value = orderNo;
                        }
                    }
                });
            }
        }

        internal void NewRowGetDefaultValue()
        {
            var dgv = _View.Table.Focus() ? _View.Table : _View.TablePreview;

            int id = 0;
            int id2 = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells["Tax Code"].Value = _View.VatGroup;
                row.Cells["Tax Rate"].Value = _repository.GetVatGroupRate(_View.VatGroup);
                row.Cells["Quantity"].Value = 1;
                row.Cells["Index"].Value = id;

                id++;
            }

            int count = InvoiceItemsModel.InvoiceItems.Count();
            int totalRow = id - count;

            InvoiceItemsModel.InvoiceItems.ToList().ForEach(x =>
            {
                x.Linenum = id2;
                id2++;
            });

            //for (int i = 0; totalRow > i; i++)
            //{
            //    PurchasingModel.PurchaseOrderDocument.Add(new PurchasingModel.PurchaseOrder
            //    {
            //        Index = id2
            //    });

            //    id2++;
            //}

            //var items = new PurchasingModel.PurchaseOrder();
            //items.Index = PurchasingAP_generics.index++;
            //items.TaxCode = _View.VatGroup;
            //items.TaxRate = _View.VatGroupRate;
            //items.Quantity = 1;
            //PurchasingModel.PurchaseOrderDocument.Add(items);

        }

        internal DataTable GetCompany()
        {
            return _repository.SelectCompany();
        }

        internal DataTable GetDocumentSeries()
        {
            return _repository.SelectDocumentSeries();
        }


        internal void GetDepartment()
        {
            string m = _repository.SelectDepartment();

            if (m != string.Empty)
            {
                _View.Department = m;

                if (_View.Table.RowCount > 0)
                {
                    DomainLayer.InvoiceModel.InvoiceDocument.ForEach(x =>
                    {
                        x.Department = _View.Department;
                    });
                    LoadData(_View.Table);
                }
            }
        }

        internal void GetDepartment(int colIndex, int index)
        {
            string m = _repository.SelectDepartment();

            if (m != string.Empty)
            {
                if (_View.Table.RowCount > 0)
                {
                    //UpdateCell(colIndex, m);
                    //InvoiceItemsModel.InvoiceItems.Find(x => x.Linenum == index).Department = m;
                }
            }
        }
            
        internal void ChangeDocumentNumber(string series)
        {
            string documentNo = _repository.SelectDocumentNo(series);

            if (documentNo != string.Empty)
            {
                _View.DocNum = documentNo;
            }
        }

        internal void DisplayItemList(string service)
        {
            if (_View.SuppCode != "")
            {
                FrmARInvoiceItemList form = new FrmARInvoiceItemList()
                {
                    IsCartonActive = false,
                    oBpCode = _View.SuppCode,
                    oBpName = _View.SuppName,
                    oTaxGroup = _View.VatGroup,
                    oWhsCode = _View.Warehouse                    
                };

                form.ShowDialog();

                LoadData(_View.Table);

                //foreach (var x in SalesModel.PurchaseOrderDocument.OrderBy(x => x.Index).ToList())
                //{
                //    if (x.GrossPrice > 0)
                //    {
                //        CurrentRowComputation(null, x.Index);
                //    }
                //}
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Warning: Empty Supplier or Department", true);
            }
        }

        public void LoadData(DataGridView dgv, bool isFirstLoad = false)
        {
            try
            {
             
                if (dgv.RowCount > 0)
                {
                    dgv.Rows.Clear();
                }
               

                foreach (var x in InvoiceItemsModel.InvoiceItems.ToList())
                {
                    object[] a = { x.ItemCode, x.ItemName ,x.Brand, x.Style, x.Color, x.Size, x.Section, x.BarCode, x.EffectivePrice,x.GrossPrice.ToString("0.00"),
                                   x.UnitPrice.ToString("0.00"), x.Quantity, x.DiscountPerc.ToString("0.00"), x.DiscountAmount, x.EmpDiscountPerc.ToString("0.00") ,
                                   x.FWhsCode, "...", x.TaxCode, "...", x.TaxRate.ToString("0.00"), x.LineTotal.ToString("0.00"), x.GrossTotal.ToString("0.00"),
                                   x.PriceAfterDisc.ToString("0.00"), x.Linenum, x.BaseEntry, x.Company
                    };

                    dgv.Rows.Add(a);
                }
           
                dataGridLayout(dgv, _View.Service);
                _View.ComputeTotal();
         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void dataGridLayout(DataGridView dgv, string service)
        {
            try
            {
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                //dgv.ReadOnly = true;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dgv.MultiSelect = true;
                dgv.RowTemplate.Resizable = DataGridViewTriState.False;
                //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgv.EnableHeadersVisualStyles = false;
                dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
                dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
                dgv.DefaultCellStyle.BackColor = Color.White;
                dgv.DefaultCellStyle.ForeColor = Color.Black;
                
                if (dgv.Name == "dgvItems")
                {
                    if (service == "Online Order")
                    {
                        dgv.Columns[11].DefaultCellStyle.ForeColor = Color.White;
                        dgv.Columns[11].DefaultCellStyle.BackColor = Color.RoyalBlue;
                    }
                    else
                    {
                        dgv.Columns[9].DefaultCellStyle.ForeColor = Color.White;
                        dgv.Columns[9].DefaultCellStyle.BackColor = Color.RoyalBlue;
                        dgv.Columns[10].DefaultCellStyle.ForeColor = Color.White;
                        dgv.Columns[10].DefaultCellStyle.BackColor = Color.RoyalBlue;
                        dgv.Columns[11].DefaultCellStyle.ForeColor = Color.White;
                        dgv.Columns[11].DefaultCellStyle.BackColor = Color.RoyalBlue;
                        dgv.Columns[12].DefaultCellStyle.ForeColor = Color.White;
                        dgv.Columns[12].DefaultCellStyle.BackColor = Color.RoyalBlue;
                        dgv.Columns[13].DefaultCellStyle.ForeColor = Color.White;
                        dgv.Columns[13].DefaultCellStyle.BackColor = Color.RoyalBlue;
                    }
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, false);
            }

        }

        internal void GetWarehouse(int colIndex, int index)
        {
            string warehouse = _repository.SelectWarehouse();

            if (warehouse != string.Empty)
            {
                UpdateCell(colIndex, warehouse);
                InvoiceItemsModel.InvoiceItems.Find(x => x.Linenum == index).FWhsCode = warehouse;
            }
        }

        internal void GetCompany(int colIndex, int index)
        {
            string Comp = SelectCompany();

            if (Comp != string.Empty)
            {
                UpdateCell(colIndex, Comp);
                InvoiceItemsModel.InvoiceItems.Find(x => x.Linenum == index).Company = Comp;
            }
        }

        internal void GetTaxCode(DataGridViewCellEventArgs e)
        {
            var table = _View.Table.Focus() ? _View.Table : _View.TablePreview;

            int colIndex = e.ColumnIndex;
            int index = ValidateInput.Int(table.CurrentRow.Cells["Index"].Value);


            Dictionary<string, string> taxCode = _repository.SelectTaxGroup();

            if (taxCode.Count > 0)
            {
                UpdateCell(colIndex - 1, taxCode["Group"]);
                UpdateCell(colIndex + 1, Convert.ToDouble(taxCode["Rate"]).ToString("0.00"));

                InvoiceItemsModel.InvoiceItems.Where(x => x.Linenum == index).ToList().ForEach(x =>
                {
                    x.TaxCode = taxCode["Group"];
                    x.TaxRate = Math.Round(Convert.ToDouble(taxCode["Rate"]), 2);
                });

                CurrentRowComputation(e);
            }
        }

        internal void GetChain(int colIndex, int index)
        {
            Dictionary<string, string> chainDetails = _repository.SelectChain();

            if (chainDetails.Count() > 0)
            {
                double qty = ValidateInput.Double(_View.Table.CurrentRow.Cells["Quantity"].Value);

                UpdateCell(colIndex, chainDetails["Value"]);
                UpdateCell(colIndex - 1, chainDetails["Code"]);
                _View.Table.CurrentRow.Cells["Pricetag Count"].Value = qty;

                PurchasingModel.PurchaseOrderDocument.Where(x => x.Index == index)
                    .ToList().ForEach(x =>
                    {
                        x.ChainPricetag = chainDetails["Code"];
                        x.ChainDescription = chainDetails["Value"];
                        x.PricetagCount = qty;
                    });
            }
        }

        internal void GetSelectedDocument(string table, string docEntry, string status)
        {
            DataTable header = new DataTable();
            DataTable Lines = new DataTable();

            table = table ?? "INV";
            string oSelCode = "";

            if (status == "Draft-Approved" || status == "Draft-Pending")
            {
                header = _repository.SelectDraftDocument(docEntry, status);

                if (_View.Service != "Item")
                {
                    Lines = _repository.SelectItemDraftDocumentLines(docEntry, status);
                }
                else
                {
                    Lines = _repository.SelectServiceDraftDocumentLines(docEntry);
                }

                _View.Status = status == "Draft-Approved" ? "Draft Approved" : "Draft Pending";
                _udfRepo.SelectUdfDratf(_View.Udf, docEntry, "ODRF");
            }
            else
            {

                if (InvoiceHeaderModel.DDWdocentry.Count > 0)
                {
                    InvoiceHeaderModel.oBPCode = InvoiceHeaderModel.DDWdocentry.Where(y => y.DocEntry != 0).Select(z => z.BpCode).First().ToString();
                    InvoiceHeaderModel.oOrderEntry = InvoiceHeaderModel.DDWdocentry.Where(y => y.DocEntry != 0).Select(z => z.OrderEntry).First().ToString();
                    oSelCode = InvoiceHeaderModel.DDWdocentry.Where(y => y.BpCode != "").Select(z => z.DocEntry).First().ToString();
                }

                header = (table == "PKL" && InvoiceHeaderModel.DDWdocentry.Select(x => x.OrderEntry).Distinct().Count() == 1) ? _repository.SelectPickListDocument(InvoiceHeaderModel.oBPCode, InvoiceHeaderModel.oOrderEntry, oSelCode) : (table == "PKL" ? null : _repository.SelectDocument(table, docEntry));

                if (header != null && table != "PKL")
                {
                    if (ConvertToString(header.Rows[0]["DocType"]) == "I")
                    {
                        Lines =  _repository.SelectDocumentLines(table, docEntry);
                    }
                    else
                    {
                        Lines = _repository.SelectServiceDocumentLines(table, docEntry);
                    }

                    //_View.Status = status == "O" ? "Open" : "Closed";
                    //_View.Status = ConvertToString(header.Rows[0]["CANCELED"]) == "N" ? _View.Status : "Canceled";
                    _View.Status = status;
                    _udfRepo.SelectUdfDratf(_View.Udf, docEntry, $"O{table}");
                }
                else if (header == null && table == "PKL")
                {
                    Lines = _repository.SelectPickListDocumentLines(table, docEntry);
                }
                else if (header.Rows.Count > 0 && table == "PKL")
                {
                    Lines = _repository.SelectPickListDocumentLines(table, docEntry);
                    //added copy from Pick List 091219
                    _udfRepo.SelectUdfDratf(_View.Udf, InvoiceHeaderModel.oOrderEntry, $"ORDR");
                }
            }

            if (header != null || table == "PKL")
            {
                if (header != null)
                {
                    if (header.Rows.Count > 0)
                    {
                        foreach (DataRow head in header.Rows)
                        {
                            _View.DocEntry = ConvertToString(head["DocEntry"]);
                            _View.DocNum = ConvertToString(head["DocNum"]);
                            //On Comment due to automated DocType with Series
                            //_View.Series = ConvertToString(head["Series"]);
                            _View.SuppCode = ConvertToString(head["CardCode"]);
                            _View.SuppName = ConvertToString(head["CardName"]);
                            _View.RefNo = ConvertToString(head["NumAtCard"]);
                            _View.ContactPerson = ConvertToString(head["NumAtCard"]);
                            _View.Company = ConvertToString(head["U_CompanyTIN"]);
                            _View.Department = ConvertToString(head["U_Department"]);
                            _View.BpCurrency = ConvertToString(head["DocCur"]);
                            _View.BpRate = ConvertToString(head["DocRate"]);
                            _View.PostingDate = ConvertToString(head["DocDate"]) != "" ? Convert.ToDateTime(head["DocDate"]).ToShortDateString() : "";
                            _View.DocumentDate = ConvertToString(head["TaxDate"]) != "" ? Convert.ToDateTime(head["TaxDate"]).ToShortDateString() : "";
                            _View.DeliveryDate = ConvertToString(head["DocDueDate"]) != "" ? Convert.ToDateTime(head["DocDueDate"]).ToShortDateString() : "";
                            _View.CancellationDate = ConvertToString(head["CancelDate"]) != "" ? Convert.ToDateTime(head["CancelDate"]).ToShortDateString() : "";
                            _View.Remark = ConvertToString(head["Comments"]);
                            _View.Service = ConvertToString(head["U_DocType"]);
                            _View.oShipTo = ConvertToString(head["ShipToCode"]) != "" ? ConvertToString(head["ShipToCode"]) : ConvertToString(head["Address2"]);
                            _View.oLogShipTo = ConvertToString(head["ShipToCode"]) != "" ? "Ship To" : "Address";
                        }
                    }
                }

                if (Lines.Rows.Count > 0)
                {
                    Double LineTotal;
                    Double GrossTotal;
                    Double VatAmount;
                    Double GrossPrice;
                    Double PriceVatInc;
                    Double DiscAmt;
                    Double Discount;
                    foreach (DataRow line in Lines.Rows)
                    {
                        InvoiceItemsModel.InvoiceItemsData sales = new InvoiceItemsModel.InvoiceItemsData();
                        Dictionary<string, string> DocumentBrand = _repository.GetDocumentBrand(ConvertToString(line["Item No."]));
                        //Dictionary<string, string> DocumentGl = _repository.GetDocumentGl(ConvertToString(line["AcctCode"]));

                        double qty = ConvertToDouble(line["Quantity"]);
                        //On Comment due to conflict in process 091219 - returned 091619
                        double discperc = ConvertToDouble(line["DiscPrcnt"]);
                        //double discAmount = ConvertToDouble(0);
                        double pricebefdisc = ConvertToDouble(line["PriceBefDi"]);
                        //double discount = Convert.ToDouble(discAmount);
                        double vatrate = ConvertToDouble(line["VatPrcnt"]);

                        LineTotal = (qty * pricebefdisc);
                        //DiscAmt = pricebefdisc * (discount / 100);
                        DiscAmt = Math.Round(((pricebefdisc / 100) * discperc) * (1 + (vatrate / 100)), 2);
                        VatAmount = LineTotal * (vatrate / 100) - ((LineTotal * (vatrate / 100)) * (discperc / 100));
                        GrossTotal = (LineTotal + VatAmount) - (DiscAmt * qty);
                        sales.Linenum = ConvertToInt(line["LineNum"]);
                        sales.ItemCode = ConvertToString(line["Item No."]); // ItemCode
                        sales.ItemName = ConvertToString(line["Dscription"]); // ItemCode
                        sales.Style = ConvertToString(line["U_ID012"]); //Style
                        sales.Color = ConvertToString(line["U_ID022"]); //Color
                        sales.Size = ConvertToString(line["U_ID007"]); //Size
                        //sales.Brand = DocumentBrand.Count > 0 ? DocumentBrand["Name"] : ""; //Brand
                        sales.Section = ConvertToString(line["U_ID018"].ToString()); //Section
                        sales.BarCode = ConvertToString(line["CodeBars"]);
                        sales.GrossPrice = ConvertToDouble(line["PriceAfVAT"]); //GrossPrice
                        sales.UnitPrice = ConvertToDouble(line["PriceBefDi"]);
                        sales.Quantity = qty; //Qty
                        sales.DiscountPerc = discperc;
                        sales.DiscountAmount = DiscAmt * qty;
                        _View.Warehouse = ConvertToString(line["WhsCode"]);
                        _View.Tax = ConvertToString(line["VatGroup"]);
                        sales.FWhsCode = ConvertToString(line["WhsCode"]); //WHsCode
                        sales.TaxCode = ConvertToString(line["VatGroup"]); //Tax Code
                        sales.TaxAmount = ConvertToDouble(line["LineVat"]);
                        sales.TaxRate = ConvertToDouble(line["VatPrcnt"]);
                        sales.GrossTotal = GrossTotal;
                        sales.LineTotal = ConvertToDouble(line["LineTotal"]);
                        
                       
                        //sales.Index = Convert.ToInt32(ConvertToString(line["LineNum"]));
                        //sales.GrossTotalLC = Math.Round(GrossTotal, 2);
                        sales.PriceAfterDisc = Math.Round(Convert.ToDouble(pricebefdisc), 2);
                        sales.EmpDiscountPerc = GetDiscount(_View.SuppCode, ConvertToString(line["Item No."].ToString()));
                        sales.EffectivePrice = GetEffectivePrice(_View.SuppCode, ConvertToString(line["Item No."].ToString()));
                        sales.BaseEntry = ConvertToString(line["OrderEntry"]);
                        sales.Company = ConvertToString(line["U_Company"]);

                        InvoiceItemsModel.InvoiceItems.Add(sales);
                       
                    }
                    SalesStyle.dgvItemsLayout(_View.Table);
                    SalesStyle.dgvItemsLayout(_View.TablePreview);
                }

                InvoiceHeaderModel.DDWdocentry.RemoveAll(z => z.BpCode != "");

                LoadData(_View.Table, true);
            }
        }

        private double GetEffectivePrice(string strBpCode, string strItemCode)
        {
            double GetEP = 0;
            try
            {
                //string strGetEPqry = "select T1.CardCode " +
                //                    " , CASE WHEN(T1.GroupCode) = '151' " +
                //                    " THEN " +
                //                    " (CASE WHEN(select IsGrossPrc from OPLN where ListNum = '3') = 'Y' " +
                //                    $" THEN(Select ISNULL(ROUND(Z.Price / 1.12, 2), 0) From ITM1 Z Where Z.ItemCode = '{strItemCode}' and Z.PriceList = '3') " +
                //                    $" ELSE(Select ISNULL(Z.Price, 0) From ITM1 Z Where Z.ItemCode = '{strItemCode}' and Z.PriceList = '3') END) " +
                //                    " ELSE " +
                //                    " (CASE WHEN(select IsGrossPrc from OPLN where ListNum = '2') = 'Y' " +
                //                    $" THEN(Select ISNULL(ROUND(Z.Price / 1.12, 2), 0) From ITM1 Z Where Z.ItemCode = '{strItemCode}' and Z.PriceList = '2') " +
                //                    $" ELSE(Select ISNULL(Z.Price, 0) From ITM1 Z Where Z.ItemCode = '{strItemCode}' and Z.PriceList = '2') END) " +
                //                    " END[EffectivePrice] " +
                //                    " from OCRD T1 " +
                //                    " where " +
                //                    $" T1.CardCode = '{strBpCode}'";

                string strGetEPqry = $"SELECT " +
                                    $" (CASE WHEN ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T0.ItemCode and z.CardCode = '{strBpCode}'),0) = 0) " +
                                    $" THEN ISNULL((select Price from ITM1 where ItemCode = T0.ItemCode and PriceList = '2'), 0) " +
                                    $" ELSE ISNULL((SELECT z.Price From OSPP z Where z.ItemCode = T0.ItemCode and z.CardCode = '{strBpCode}'),0)) [EffectivePrice] " +
                                    $" from OITM T0 where T0.ItemCode = '{strItemCode}'";

                if (Hana.Get( strGetEPqry).Rows.Count > 0)
                {
                    GetEP = Convert.ToDouble(Hana.Get( strGetEPqry).Rows[0]["EffectivePrice"].ToString());
                }

                return GetEP;
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
                return GetEP;
            }

        }

        private double GetDiscount(string strBpCode, string strItemCode)
        {
            double GetEP = 0;
            try
            {
                string strGetDisc = $"SELECT " +
                            $" (CASE WHEN ISNULL((SELECT Discount from OSPP where CardCode = '{strBpCode}' and ItemCode = T0.ItemCode),0) = 0) " +
                            $" THEN ISNULL((select 100 - (Factor*100) [Discount] from OPLN where ListNum = (SELECT ISNULL(ListNum,1) FROM OCRD WHERE CardCode = '{strBpCode}')), 0) " +
                            $" ELSE ISNULL((SELECT Discount from OSPP where CardCode = '{strBpCode}' and ItemCode = T0.ItemCode),0)) [Discount] " +
                            $" from OITM T0 where T0.ItemCode = '{strItemCode}'";

                if (Hana.Get( strGetDisc).Rows.Count > 0)
                {
                    GetEP = Convert.ToDouble(Hana.Get( strGetDisc).Rows[0]["Discount"].ToString());
                }

                return GetEP;
            }
            catch (Exception ex)
            {
                ///StaticHelper._MainForm(ex.Message, true);
                return GetEP;
            }

        }

        internal void ExecuteCopyDocument(string doc, FrmARInvoice SalesInvoice)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var suppcode = _View.SuppCode;
                ClearField(true);

                switch (doc)
                {
                    case "Sales Order":

                        if (suppcode != string.Empty)
                        {
                            _View.DocEntry = _repository.CopyFrom(suppcode, "CopyFromSO");

                            if (_View.DocEntry != string.Empty || _View.DocEntry.Length > 0)
                            {
                                InvoiceHeaderModel.oSelectedDoc = doc;
                                GetSelectedDocument("RDR", _View.DocEntry, "O");
                            }
                        }
                        else
                        {
                            StaticHelper._MainForm.ShowMessage("Please select a supplier first.", true);
                        }

                        break;

                    case "Pick List":

                        _View.DocEntry = _repository.CopyFrom(_View.SuppCode, "OPKL_S");

                        if (InvoiceHeaderModel.DDWdocentry.Count > 0)
                        {
                            string picklistEntry = null;
                            picklistEntry = InvoiceHeaderModel.oCode;
                            frmSI_DrawDoumentWizard frmDDW = new frmSI_DrawDoumentWizard(SalesInvoice);
                            frmDDW.ShowDialog();

                            if (InvoiceHeaderModel.DDWdocentry.Count > 0)
                            {
                                InvoiceHeaderModel.oSelectedDoc = doc;
                                GetSelectedDocument("PKL", _View.DocEntry, "O");
                            }
                        }

                        break;
                }
            }
        }

        internal void ComputeAmountDiscount()
        {
            try
            {
                if (Convert.ToDouble(_View.TotalBeforeDiscount) > Convert.ToDouble(_View.DiscountAmount))
                {
                    string amount = ((Convert.ToDouble(_View.DiscountAmount) / Convert.ToDouble(_View.TotalBeforeDiscount)) * 100).ToString();
                    var strformat = string.Format("{0:0.00}", amount);
                    _View.DiscountInput = strformat;
                }
                else
                {
                    _View.DiscountInput = "0.00";
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                TotalComputation();
            }
        }

        internal void ComputeInputDiscount()
        {
            try
            {
                double discount = _computation.PercentConverter(_View.DiscountInput);

                _View.DiscountAmount = _computation.ComputeFooterDiscount(ConvertToDouble(_View.TotalBeforeDiscount),
                    discount).ToString();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                TotalComputation();
            }
        }

        internal void ExecuteDeleteItem()
        {
            var currentTable = _View.Table.Focus() ? _View.Table : _View.TablePreview;

            for (int i = 0; i < currentTable.RowCount - 1; i++)
            {
                if (currentTable.Rows[i].Selected)
                {
                    int index = Convert.ToInt32(currentTable.Rows[i].Cells["LineNum"].Value);
                    InvoiceItemsModel.InvoiceItems.RemoveAll(x => x.Linenum == index);
                }
            }

            LoadData(currentTable);
        }

        internal void DeleteItem(int rowIndex, int colIndex)
        {
            if (_View.Status == "Open" || _View.Status.Contains("Draft"))
            {
                if (rowIndex != -1)
                {
                    _View.Table.CurrentCell = _View.Table.CurrentRow.Cells[colIndex + 1];
                    _View.Table.CurrentRow.Selected = true;
                    _View.Table.Focus();

                    var mousePosition = _View.Table.PointToClient(Cursor.Position);

                    _View.MsItems.Show(_View.Table, mousePosition);
                }
            }
        }

        internal void UdfRequest()
        {
            string fieldName = _View.Udf.CurrentRow.Cells[0].Value.ToString();

            string result = "";

            //if (fieldName == "U_CheckBy" || fieldName == "U_AppBy" || fieldName == "U_NotedBy")
            //{
            //    result = _repository.GetEmployees();
            //}
            if (fieldName == "U_RevBy" || fieldName == "U_NotedBy" || fieldName == "U_AppBy")
            {
                result = _repository.GetEmployees();
            }
            else if (fieldName == "U_OrdrPromo" || fieldName == "U_ReqBy")
            {
                result = _repository.GetAllEmployees();
            }
            else if (fieldName.Contains("U_ShipmentDate") || fieldName.Contains("U_OrRecDate") || fieldName.Contains("U_RetDate"))
            {
                _udfRepo.ConvertToDate(_View.Udf);
                ConvertToDate(_View.Udf);
            }
            else if (fieldName == "U_ORNo")
            {
                result = _repository.GetOrderNo(_View.SuppCode);
            }
            else if (fieldName == "U_RAS")
            {
                result = _repository.GetOrderNo(_View.SuppCode);
            }
            if (result != string.Empty)
            {
                _View.Udf.CurrentRow.Cells[2].Value = result;
            }
        }

        public void ConvertToDate(DataGridView dgv)
        {
            var row = dgv.CurrentRow;
            string fieldName = row.Cells[0].Value.ToString();

            if (fieldName.Contains("U_ShipmentDate") || fieldName.Contains("U_ShipmentDate"))
            {
                //DateTimePicker oDateTimePicker = new DateTimePicker();

                var date = DateTime.Now.ToString("MM/dd/yyyy");

                if (dgv.CurrentCell.Value != null)
                {
                    date = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                }

                dgv[2, row.Index].Value = date;
                //oDateTimePicker.Name = $"dt{FrmPurchaseOrder.datetimeCount++}";
                oDateTimePicker.Value = Convert.ToDateTime(date);
                dgv.Controls.Add(oDateTimePicker);
                CreateDateTimePicker(oDateTimePicker, dgv, row);
                //_View.Udf = dgv;
            }
        }

        private void CreateDateTimePicker(DateTimePicker dtPicker, DataGridView dgv, DataGridViewRow row)
        {
            dtPicker.Format = DateTimePickerFormat.Short;

            Rectangle oRectangle = dgv.GetCellDisplayRectangle(2, row.Index, true);

            dtPicker.Size = new Size(oRectangle.Width, oRectangle.Height);
            dtPicker.Location = new Point(oRectangle.X, oRectangle.Y);
            dtPicker.Visible = true;

            dtPicker.CloseUp += new EventHandler(dateTimePicker_CloseUp);
            dtPicker.Visible = false;
        }

        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            var dtPicker = (DateTimePicker)sender;
            var qe = dtPicker.Value.ToShortDateString();

            _View.Udf.CurrentCell.Value = qe;
            oDateTimePicker.Visible = false;
            dtPicker.Visible = false;
        }

        internal void GetExistingDocument(DataGridView dgv)
        {
            dgv.DataSource = null;
            dgv.DataSource = ListDocuments();
        }

        internal void GetCurrencyRate()
        {
            string rate = _repository.GetCurrencyRate(_View.BpCurrency);

            _View.BpRate = ValidateInput.String(rate);
        }

        internal void AutomateGlAccountName()
        {
            if (ValidateInput.String(_View.Udf.CurrentRow.Cells[0].Value) == "U_TransactionType" && _View.Service == "Service")
            {
                if (ValidateInput.String(_View.Udf.CurrentRow.Cells[2].Value) == "Job Order")
                {
                    List<string> val = _repository.GetJobOrderGL();

                    for (int i = 0; i < _View.Table.RowCount - 1; i++)
                    {
                        _View.Table.Rows[i].Cells["G/L Account"].Value = val[0];
                        _View.Table.Rows[i].Cells["G/L Account Name"].Value = val[1];

                        int index = ValidateInput.Int(_View.Table.Rows[0].Cells["Index"].Value);

                        PurchasingModel.PurchaseOrderDocument.Where(a => a.Index == index)
                        .ToList().ForEach(a =>
                        {
                            a.GLAccount = val[0];
                            a.GLAccountName = val[1];
                        });
                    }

                    var qwe = PurchasingAP_generics.index++;

                    object[] Services = { "", "", "", "", "", "", "", "", "1", "", "", "", "",
                                "", "", "", "", "", "", "", "", "", "0",
                                _View.VatGroup, "", _View.VatGroupRate, "0", "0", "0", "", "", _View.Department, "", "", qwe, ""
                        };

                    //var items = new PurchasingModel.PurchaseOrder();
                    //items.Index = qwe;
                    //items.TaxCode = _View.VatGroup;
                    //items.TaxRate = _View.VatGroupRate;
                    //items.Quantity = 1;
                    //PurchasingModel.PurchaseOrderDocument.Add(items);

                    //_View.Table.Rows.Add(Services);
                }
            }
        }

        internal DataTable ListDocuments()
        {
            DataTable dt = _repository.ExistingDocument();

            return dt;
        }

        internal void GetGlAccount(int colIndex, int index)
        {
            Dictionary<string, string> gl = _repository.SelectGeneralLedger();

            if (gl.Count() > 0)
            {
                bool itHasValue = false;

                var dgv = _View.Table.Focus() ? _View.Table : _View.TablePreview;

                //Added GetValue for conflict on Service Type adding of 3rd row (Darrel - 042419)
                string GetValue = dgv.CurrentRow.Cells[colIndex].Value == null ? "" : dgv.CurrentRow.Cells[colIndex].Value.ToString();
                itHasValue = GetValue != "" ? true : false;

                UpdateCell(colIndex - 1, gl["Code"]);
                UpdateCell(colIndex, gl["Name"]);

                PurchasingModel.PurchaseOrderDocument.Where(a => a.Index == index)
                    .ToList().ForEach(a =>
                    {
                        a.GLAccount = gl["Code"];
                        a.GLAccountName = gl["Name"];
                    });
                if (itHasValue == false)
                {
                    var x = PurchasingModel.PurchaseOrderDocument.SingleOrDefault(s => s.Index == 0);

                    var indexnumber = PurchasingAP_generics.index++;

                    object[] Services = { "", "", "", "", "", "", "", "", "1", "", "", "", "",
                            "", "", "", "", "", "", "", "", "", "", "0",
                            _View.VatGroup, "", _View.VatGroupRate, "0", "0", "0", "", "", _View.Department, "", "", indexnumber, ""
                    };

                    //var items = new PurchasingModel.PurchaseOrder();
                    //items.Index = qwe;
                    //items.TaxCode = _View.VatGroup;
                    //items.TaxRate = _View.VatGroupRate;
                    //items.Quantity = 1;
                    //PurchasingModel.PurchaseOrderDocument.Add(items);

                    //dgv.Rows.Add(Services);
                    //NewRowGetDefaultValue();
                }
            }
        }

        internal void GetProject(int colIndex, int index)
        {
            string project = _repository.SelectProject(_View.SuppCode);

            if (project != string.Empty)
            {
                UpdateCell(colIndex, project);
                //InvoiceItemsModel.InvoiceItems.Find(x => x.Linenum == index).Project = project;
            }
        }

        internal void GetUom(int colIndex, int index)
        {
            List<string> UomValue = _repository.SelectUom();

            if (UomValue != null)
            {
                UpdateCell(colIndex, UomValue[1]);

                PurchasingModel.PurchaseOrderDocument.Find(x => x.Index == index).UoM = UomValue[1];
            }
        }

        private void UpdateCell(int ColIndex, string value)
        {
            try
            {
                _View.Table.CurrentRow.Cells[ColIndex].Value = value;

                int count = _View.TablePreview.RowCount;

                if (count > 1)
                {
                    _View.TablePreview.CurrentRow.Cells[ColIndex].Value = value;
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        public void CurrentRowComputation(DataGridViewCellEventArgs e, int intIndex = 0, bool isTaxChange = false)
        {
            try
            {
                var rowIndex = e != null ? e.RowIndex : intIndex;
                var colIndex = e == null ? 0 : e.ColumnIndex;
                Double DiscAmt;
                DataGridViewRow currentRow = null;

                var qweee = _View.TablePreview.Rows.Count;
                var qweee2 = _View.Table.Rows.Count;

                if (_View.TablePreview.Focus())
                {
                    currentRow = _View.TablePreview.Rows[rowIndex];
                }
                else
                {
                    currentRow = _View.Table.Rows[rowIndex];
                }

                var curCell = currentRow.Cells[colIndex];

                string currentColumn = _View.TablePreview.Focus() ? _View.TablePreview.Columns[_View.TablePreview.CurrentCell.ColumnIndex].Name :
                      _View.Table.Columns[_View.Table.CurrentCell.ColumnIndex].Name;

                double index = ConvertToDouble(currentRow.Cells["LineNum"].Value);

                var item = InvoiceItemsModel.InvoiceItems.Find(x => x.Linenum == index);

                if (item != null)
                {
                    double taxRate = item.TaxRate != 0D ? _computation.PercentConverter(item.TaxRate) + 1 : 1D;
                    double discountPerc = _computation.PercentConverter(_validation.ValidateCellsDouble(currentRow, "Discount %").ToString());
                    var TaxCode = _validation.ValidateCells(currentRow, "Tax");
                    item.TaxCode = TaxCode;
                    //item.ChainDescription = _validation.ValidateCells(currentRow, "Chain Description");
                    //item.Remarks = _validation.ValidateCells(currentRow, "Remarks");
                    //item.Project = _validation.ValidateCells(currentRow, "Project");
                    item.Quantity = _validation.ValidateCellsDouble(currentRow, "Quantity");
                    item.DiscountPerc = _validation.ValidateCellsDouble(currentRow, "Discount %");
                    item.DiscountAmount = item.DiscountPerc * item.Quantity;
                    try { item.TaxRate = Convert.ToDouble(Hana.Get(StringQueryRepository.GetTaxRate(item.TaxCode)).Rows[0].ItemArray[0]); }
                    catch { item.TaxRate = 0.00; }
                    //try { item.ChainPricetag = Hana.Get( StringQueryRepository.GetChain(item.ChainDescription)).Rows[0].ItemArray[0].ToString(); }
                    //catch { item.ChainPricetag = string.Empty; }
                    taxRate = item.TaxRate != 0D ? _computation.PercentConverter(item.TaxRate) + 1 : 1D;

                    if (currentColumn.Equals("Gross Price"))
                    {
                        var GrossPrice = _validation.ValidateCellsDouble(currentRow, "Gross Price");
                        item.UnitPrice = _computation.ComputeUnitPrice(GrossPrice, taxRate, TaxCode);
                        item.GrossPrice = _computation.ComputeGrossPrice(item.UnitPrice, taxRate, TaxCode);
                        item.DiscountAmount = 0.00;
                        item.DiscountPerc = 0.00;
                    }
                    else if (currentColumn.Equals("Unit Price"))
                    {
                        var UnitPrice = _validation.ValidateCellsDouble(currentRow, "Unit Price");
                        UnitPrice = ConvertToDouble(UnitPrice);
                        item.UnitPrice = UnitPrice;
                        item.GrossPrice = _computation.ComputeGrossPrice(UnitPrice, taxRate, TaxCode);
                        item.DiscountAmount = 0.00;
                        item.DiscountPerc = 0.00;
                    }
                    else
                    {
                        var UnitPrice = _validation.ValidateCellsDouble(currentRow, "Unit Price");
                        UnitPrice = ConvertToDouble(UnitPrice);
                        item.UnitPrice = UnitPrice;
                        item.GrossPrice = _computation.ComputeGrossPrice(UnitPrice, taxRate, TaxCode);
                    }

                    // discount function
                    //item.DiscountAmount = _computation.ComputeDiscountAmount(discountPerc, Convert.ToDouble(item.UnitPrice), item.Quantity);
                    double dGetDiscount = 0.0;
                    dGetDiscount = _validation.ValidateCellsDouble(currentRow, "Discount");
                    item.DiscountAmount = dGetDiscount > 0 ? Math.Round((dGetDiscount / taxRate),3) : ComputeDiscountAmount(discountPerc, Convert.ToDouble(item.UnitPrice), item.Quantity);

                    //On Comment - already on upper logic 12/13/19
                    //var computeDiscount = item.GrossPrice - (item.GrossPrice * discountPerc);
                    var computeDiscount = (item.UnitPrice - item.DiscountAmount) * taxRate;
                    item.GrossPrice = Convert.ToDouble(Math.Round(computeDiscount, 2).ToString("0.00"));

                    var qty = item.Quantity;

                    item.LineTotal = Convert.ToDouble(Math.Round(_computation.ComputeRowTotals(qty, Convert.ToDouble(item.UnitPrice), item.DiscountAmount), 2).ToString("0.00"));

                    item.GrossTotal = _computation.ComputeRowTotals(qty, Convert.ToDouble(item.GrossPrice), item.DiscountAmount);

                    item.TaxAmount = _computation.ComputeTax(item.GrossTotal, taxRate, item.DiscountAmount);

                    item.PriceAfterDisc = Math.Round(Convert.ToDouble(item.PriceAfterDisc), 2);
                    //double LineTotal = (qty * item.PriceAfterDisc);
                    //DiscAmt = item.PriceAfterDisc * (item.DiscountPerc / 100);
                    //double VatAmount = LineTotal * (vatrate / 100) - ((LineTotal * (vatrate / 100)) * (item.DiscountPerc / 100));
                    //double GrossTotal = (LineTotal + VatAmount) - (DiscAmt * qty);

                    
                    currentRow.Cells["Unit Price"].Value = item.UnitPrice.ToString("0.00");
                    currentRow.Cells["Gross Price"].Value = item.GrossPrice.ToString("0.00");
                    //currentRow.Cells["Total(LC)"].Value = item.TotalLC.ToString("0.00");
                    //currentRow.Cells["Gross Total (LC)"].Value = item.GrossTotalLC.ToString("0.00");

                    currentRow.Cells["Line Total"].Value = item.LineTotal.ToString("0.00");
                    currentRow.Cells["Gross Total"].Value = item.GrossTotal.ToString("0.00");
                    currentRow.Cells["Quantity"].Value = item.Quantity;

                    double dDiscountPer = item.DiscountAmount / item.PriceAfterDisc;
                    currentRow.Cells["Discount %"].Value = Math.Round((dDiscountPer * 100),4);
                    currentRow.Cells["Discount"].Value = dGetDiscount;

                    //TotalComputation();
                    _View.ComputeTotal();
                    UpdateCell(19, Convert.ToDouble(item.TaxRate).ToString("0.00"));
                    _View.Table.Update();
                    _View.Table.Refresh();
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        double ComputeDiscountAmount(double dDiscountPercent, double dUnitPrice, double dquantity)
        {
            var output = 0.0;

            output = Math.Round(((dDiscountPercent * dUnitPrice) * dquantity), 4);

            return output;
        }

        double ComputeGrossPrice(double dUnitPrice, double dDiscountAmount, double dTaxMultiplier)
        {

            var output = 0.0;
            output = (dUnitPrice - (dDiscountAmount / dTaxMultiplier)) * dTaxMultiplier;

            return output;
        }

        private void TotalComputation()
        {
            double taxAmount = 0;
            double totalBefDiscount = 0;

            foreach (var x in InvoiceItemsModel.InvoiceItems.ToList())
            {
                taxAmount += x.TaxAmount;
                totalBefDiscount += x.GrossTotal;
            }

            _View.TotalBeforeDiscount = Math.Round(Convert.ToDecimal(totalBefDiscount), 3).ToString("0.00");
            //_View.DiscountAmount = Math.Round(Convert.ToDecimal(taxAmount), 2).ToString("0.00");
            //_View.DiscountAmount = discount.ToString("#,##0.00");

            if (_View.DiscountAmount != null)
            {
                try
                {
                    if (Convert.ToDouble(taxAmount) > 0)
                    {
                        //double result = (Convert.ToDouble(_View.TotalBeforeDiscount) - Convert.ToDouble(_View.DiscountAmount)) * .12;
                        //_View.TaxAmount = Math.Round(Convert.ToDecimal(result), 2).ToString("0.00");
                    }
                    else
                    {
                        //_View.TaxAmount = Math.Round(Convert.ToDecimal(taxAmount), 2).ToString("0.00");
                    }
                }
                catch (Exception ex)
                {
                    //_View.TaxAmount = Math.Round(Convert.ToDecimal(taxAmount), 2).ToString("0.00");
                }
            }

            double total = Convert.ToDouble(_View.TotalBeforeDiscount) + Convert.ToDouble(_View.TaxAmount);

            if (_View.DiscountAmount != string.Empty)
            {
                total = total - Convert.ToDouble(_View.DiscountAmount);
            }

            _View.Total = Math.Round(Convert.ToDecimal(total), 2).ToString("0.00");
            Application.DoEvents();
        }

        public bool CheckUser()
        {
            return _repository.CheckUser();
        }

        public bool ExecuteRequestDIAPI(string request)
        {
            return false;

            //bool isSuccess = false;

            //bool isSuperUser = _repository.CheckUser();

            //if (isSuperUser)
            //{
            //    var company = SAPAccess.oCompany;
            //    DECLARE.oPurchase = (SAPbobsCOM.Documents)SAPAccess.oCompany
            //    .GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
            //}
            //else
            //{
            //    if (request == "Add")
            //    {
            //        DECLARE.oPurchase = (SAPbobsCOM.Documents)SAPAccess
            //            .oCompany
            //    .GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
            //    }
            //    else
            //    {
            //        DECLARE.oPurchase = (SAPbobsCOM.Documents)SAPAccess.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
            //    }

            //    if (_View.Status == "Draft Pending")
            //    {
            //        DECLARE.oPurchase.GetByKey(Convert.ToInt32(_View.DocEntry));

            //        int i = DECLARE.oPurchase.Cancel();

            //        if (i != 0)
            //        {
            //            Globals.Main.NotiMsg(SAPAccess.oCompany.GetLastErrorDescription(), System.Drawing.Color.Red);
            //        }

            //        DECLARE.oPurchase = null;
            //        DECLARE.oPurchase = (SAPbobsCOM.Documents)SAPAccess.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
            //        request = "Add";
            //    }
            //    else
            //    {
            //        DECLARE.oPurchase.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseOrders;
            //    }
            //}


            //if (request == "Update")
            //{
            //    DECLARE.oPurchase.GetByKey(Convert.ToInt32(_View.DocEntry));
            //}

            //DECLARE.oPurchase.CardCode = _View.SuppCode;
            //DECLARE.oPurchase.NumAtCard = _View.ContactPerson;

            //if (_View.PostingDate != string.Empty)
            //{
            //    DECLARE.oPurchase.DocDate = Convert.ToDateTime(_View.PostingDate);
            //}

            //if (_View.DocumentDate != string.Empty)
            //{
            //    DECLARE.oPurchase.TaxDate = Convert.ToDateTime(_View.DocumentDate);
            //}

            //if (_View.DeliveryDate != string.Empty)
            //{
            //    DECLARE.oPurchase.DocDueDate = Convert.ToDateTime(_View.DeliveryDate);
            //}

            //if (_View.CancellationDate != string.Empty)
            //{
            //    DECLARE.oPurchase.CancelDate = Convert.ToDateTime(_View.CancellationDate);
            //}

            //DECLARE.oPurchase.DiscountPercent = ConvertToDouble(_View.DiscountInput);
            //DECLARE.oPurchase.UserFields.Fields.Item("U_CompanyTIN").Value = _View.Company;
            //DECLARE.oPurchase.UserFields.Fields.Item("U_Department").Value = _View.Department;
            //DECLARE.oPurchase.UserFields.Fields.Item("U_Remarks").Value = _View.Remark;
            //DECLARE.oPurchase.Comments = $"Created by EasySAP | Purchase order : {SboCred.UserID} {DateTime.Today}";
            //DECLARE.oPurchase.DocType = _View.Service == "Item" ? SAPbobsCOM.BoDocumentTypes.dDocument_Items : SAPbobsCOM.BoDocumentTypes.dDocument_Service;
            //DECLARE.oPurchase.DocCurrency = _View.BpCurrency;
            //DECLARE.oPurchase.DocRate = _View.BpRate == string.Empty ? ConvertToDouble("1") : ConvertToDouble(_View.BpRate);

            //foreach (DataGridViewRow udfRow in _View.Udf.Rows)
            //{
            //    try
            //    {
            //        DECLARE.oPurchase.UserFields.Fields.Item(udfRow.Cells[0].Value.ToString()).Value = ConvertToString(udfRow.Cells[2].Value);
            //    }
            //    catch { }
            //}

            //DECLARE.oPurchase.UserFields.Fields.Item("U_PrepBy").Value = SboCred.UserID;

            //if (_View.Status != "Draft Approved")
            //{
            //    foreach (DataGridViewRow x in _View.Table.Rows)
            //    {
            //        if (x.Cells["Item No."].Value != null)
            //        {

            //            int index = Convert.ToInt32(x.Cells["Index"].Value);

            //            if (_View.Service == "Item")
            //            {
            //                DECLARE.oPurchase.Lines.ItemCode = x.Cells["Item No."].Value.ToString();
            //                DECLARE.oPurchase.Lines.BarCode = x.Cells["Old Item No."].Value.ToString();
            //                DECLARE.oPurchase.Lines.ItemDescription = x.Cells["Item Description"].Value.ToString();
            //                DECLARE.oPurchase.Lines.DiscountPercent = Convert.ToDouble(x.Cells["Discount %"].Value);
            //                DECLARE.oPurchase.Lines.DiscountPercent = Convert.ToDouble(x.Cells["Disc Amount"].Value);

            //                DECLARE.oPurchase.Lines.Quantity = Convert.ToDouble(x.Cells["Quantity"].Value);
            //                DECLARE.oPurchase.Lines.UoMEntry = _repository.GetUomEntry(x.Cells["UoM"].Value.ToString()); //_Generics.UomEntry();
            //                DECLARE.oPurchase.Lines.WarehouseCode = x.Cells["Warehouse"].Value == null ? "" : x.Cells["Warehouse"].Value.ToString();
            //                DECLARE.oPurchase.Lines.UnitPrice = Convert.ToDouble(x.Cells["Unit Price"].Value);
            //            }
            //            else
            //            {
            //                DECLARE.oPurchase.Lines.ItemDescription = x.Cells["Item No."].Value.ToString();
            //                DECLARE.oPurchase.Lines.AccountCode = ConvertToString(x.Cells["G/L Account"].Value);
            //                DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_OldItemNo").Value = ConvertToString(x.Cells["Old Item No."].Value);
            //                DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_Description").Value = ConvertToString(x.Cells["Item Description"].Value);
            //                DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_Qty").Value = Convert.ToDouble(x.Cells["Quantity"].Value);
            //                DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_UOM").Value = _repository.GetUomEntry(ConvertToString(x.Cells["UoM"].Value)).ToString();
            //                DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_TargetWhs").Value = x.Cells["Warehouse"].Value == null ? "" : x.Cells["Warehouse"].Value.ToString();

            //                DECLARE.oPurchase.Lines.Quantity = Convert.ToDouble(x.Cells["Quantity"].Value);
            //                DECLARE.oPurchase.Lines.UnitPrice = Convert.ToDouble(x.Cells["Unit Price"].Value);
            //                DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_UnitPricePerPiece").Value = x.Cells["Unit Price per piece"].Value.ToString();
            //            }

            //            DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_Style").Value = ConvertToString(x.Cells["Style"].Value);
            //            DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_Color").Value = x.Cells["Color"].Value == null ? "" : x.Cells["Color"].Value.ToString();
            //            DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_Size").Value = x.Cells["Size"].Value == null ? "" : x.Cells["Size"].Value.ToString();
            //            DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_Chain").Value = x.Cells["Chain Pricetag"].Value == null ? " " : x.Cells["Chain Pricetag"].Value.ToString();
            //            DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_ChainDescription").Value = x.Cells["Chain Description"].Value == null ? "" : x.Cells["Chain Description"].Value.ToString();
            //            DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_PricetagCount").Value = x.Cells["Pricetag Count"].Value == null ? "0" : x.Cells["Pricetag Count"].Value.ToString();
            //            DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_Remarks").Value = x.Cells["Remarks"].Value == null ? "" : x.Cells["Remarks"].Value.ToString();

            //            DECLARE.oPurchase.Lines.CostingCode2 = PurchasingModel.PurchaseOrderDocument.Find(f => f.Index == index).BrandCode;
            //            DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_BrandName").Value = PurchasingModel.PurchaseOrderDocument.Find(f => f.Index == index).Brand;

            //            DECLARE.oPurchase.Lines.Quantity = Convert.ToDouble(x.Cells["Quantity"].Value);
            //            DECLARE.oPurchase.Lines.COGSCostingCode = x.Cells["Department"].Value.ToString();
            //            DECLARE.oPurchase.Lines.VatGroup = x.Cells["Tax Code"].Value.ToString();
            //            DECLARE.oPurchase.Lines.CostingCode = x.Cells["Department"].Value.ToString();
            //            DECLARE.oPurchase.Lines.ProjectCode = x.Cells["Project"].Value == null ? "" : x.Cells["Project"].Value.ToString();
            //            DECLARE.oPurchase.Lines.Add();
            //        }
            //    }
            // }


            // POSTING or UPDATING A DOCUMENT 
            //int result = request == "Add" ? DECLARE.oPurchase.Add() : DECLARE.oPurchase.Update();

            //if (result == 0)
            //{
            //    if (_View.Status == "Draft Approved")
            //    {
            //        int resultValue = DECLARE.oPurchase.SaveDraftToDocument();

            //        if (resultValue != 0)
            //        {
            //            isSuccess = false;
            //            var qweqewqewq = SAPAccess.oCompany.GetLastErrorDescription();
            //            Globals.Main.NotiMsg(SAPAccess.oCompany.GetLastErrorDescription(), System.Drawing.Color.Red);
            //        }
            //    }
            //    string newDoc = "";
            //    SAPAccess.oCompany.GetNewObjectCode(out newDoc);
            //    Globals.Main.NotiMsg($"{newDoc} Document has been successfully added", System.Drawing.Color.Green);
            //    ClearField(true);
            //}
            //else
            //{
            //    Globals.Main.NotiMsg(SAPAccess.oCompany.GetLastErrorDescription(), System.Drawing.Color.Red);
            //}

            //return isSuccess;
        }

        string GetUomID(string sUomCode)
        {
            var result = helper.ReadDataRow(hana.Get($"SELECT UomEntry FROM OUOM WHERE UomName = '{sUomCode}'"), 0, "", 0);
            return string.IsNullOrEmpty(result) ? "0" : result;
        }


        public bool ExecuteRequest(string request)
        {
            try
            {
                bool isPosted = false;
                string msg = string.Empty;
                int UDfCount = _View.Udf.RowCount;
                int rowCount = _View.Table.RowCount;
                string strBaseEntry = "";

                Dictionary<string, string> dict = new Dictionary<string, string>()
                {
                    {"CardCode", _View.SuppCode},
                    {"CardName", _View.SuppName},
                    {"NumAtCard", _View.RefNo},
                    {"U_CompanyTIN", _View.Company },
                    {"U_Department", _View.Department},
                    {"DocCurrency", _View.BpCurrency},
                    {"Series", _View.Series},
                    //{"DocRate", _View.BpRate == string.Empty ? "1" : _View.BpRate},
                    {"DocDate", ValidateInput.ConvertToDate(_View.PostingDate, "yyyy-MM-dd") },
                    {"TaxDate", ValidateInput.ConvertToDate(_View.DocumentDate, "yyyy-MM-dd") },
                    {"DocDueDate",ValidateInput.ConvertToDate(_View.DeliveryDate, "yyyy-MM-dd")},
                   // {"CancelDate", ValidateInput.ConvertToDate(_View.CancellationDate, "yyyyMMdd")},
                    {"Comments", _View.Remark},
                    {"U_PrepBy", DomainLayer.Models.EasySAPCredentialsModel.EmployeeCompleteName},
                    {"U_DocType", _View.Service},
                    {"DocObjectCode" , "17"},
                    {"DocumentsOwner",  _repository.GetEmpID() }
                };

                if (_View.oShipTo != "")
                {
                    if (_View.oLogShipTo == "Ship To")
                    {
                        dict.Add("ShipToCode", _View.oShipTo);
                    }
                    else
                    {
                        dict.Add("Address2", _View.oShipTo);
                    }
                }

                for (int i = 0; UDfCount > i; i++)
                {
                    if (!dict.Keys.Contains(_View.Udf.Rows[i].Cells[0].Value))
                    {
                        if (ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value).Contains("Date") || ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value).Contains("date"))
                        {
                            if (_View.Udf.Rows[i].Cells[2].Value != null)
                            {
                                var date = Convert.ToDateTime(ValidateInput.String(_View.Udf.Rows[i].Cells[2].Value)).ToString("yyyyMMdd");

                                dict.Add(ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value), date);
                            }
                        }
                        else
                        {
                            dict.Add(ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value), ValidateInput.String(_View.Udf.Rows[i].Cells[2].Value));
                        }
                    }
                }

                if (dict.Keys.Contains("U_PostRem"))
                {
                    dict.Remove("U_PostRem");
                }

                SboCredentials sboCred = new SboCredentials();
                dict.Add("U_PostRem", $"{(request.Equals("Add") ? "Created" : "Updated")} by EasySAP | AR Invoice : {sboCred.UserId} : {DateTime.Now} : | Powered By : DIREC");

                List<Dictionary<string, object>> dictLines = new List<Dictionary<string, object>>();

                for (int index = 0; rowCount > index; index++)
                {
                    bool isOkay = false;

                    if (_View.Service == "Service" && _View.Table.Rows[index].Cells["G/L Account"].Value != null)
                    {
                        if (_View.Table.Rows[index].Cells["G/L Account"].Value.ToString() != string.Empty)
                        {
                            isOkay = true;
                        }
                    }
                    else if (_View.Table.Rows[index].Cells[0].Value != null)
                    {
                        isOkay = true;
                    }

                    if (isOkay)
                    {
                        var items = new Dictionary<string, object>();

                        var ItemCode = ValidateInput.String(_View.Table.Rows[index].Cells["Item No."].Value);

                        if (InvoiceHeaderModel.oSelectedDoc == "Pick List" && _View.DocEntry == "")
                        {
                            strBaseEntry = ValidateInput.String(_View.Table.Rows[index].Cells["BaseEntry"].Value);
                        }
                        else
                        {
                            strBaseEntry = InvoiceHeaderModel.oSelectedDoc == "" ? "0" : _View.DocEntry.ToString();
                        }

                        if (strBaseEntry != "0")
                        {
                            string strBaseLine = ValidateInput.String(_View.Table.Rows[index].Cells["LineNum"].Value);

                            items.Add("BaseType", "17");
                            items.Add("BaseEntry", strBaseEntry);
                            items.Add("BaseLine", strBaseLine);

                        }
                        else
                        {
                            if (_View.Service == "Item" || InvoiceHeaderModel.oSelectedDoc == "Pick List")
                            {
                                items.Add("ItemCode", ItemCode);
                                items.Add("WarehouseCode", ValidateInput.String(_View.Table.Rows[index].Cells["Warehouse"].Value));

                                //items.Add("DiscountPercent", ValidateInput.String(_View.Table.Rows[index].Cells["Discount %"].Value));

                                //On Comment by Cedi=======
                                //var uom = _repository.GetUomEntry(ValidateInput.String(_View.Table.Rows[index].Cells["UoM"].Value));
                                //items.Add("UoMEntry", uom);
                                //========
                            }
                            else
                            {
                                items.Add("ItemCode", ItemCode);
                                items.Add("WarehouseCode", ValidateInput.String(_View.Table.Rows[index].Cells["Warehouse"].Value));
                                //items.Add("ItemDescription", ValidateInput.String(_View.Table.Rows[index].Cells["Item No."].Value));
                                items.Add("U_Description", ValidateInput.String(_View.Table.Rows[index].Cells["Item Description"].Value));
                                items.Add("U_Qty", ValidateInput.String(_View.Table.Rows[index].Cells["Quantity"].Value));
                                items.Add("U_TargetWhs", ValidateInput.String(_View.Table.Rows[index].Cells["Warehouse"].Value));

                                //var uom = _repository.GetUomEntry(ValidateInput.String(_View.Table.Rows[index].Cells["UoM"].Value));
                                //items.Add("UoMEntry", uom);
                                //items.Add("U_UOM", ValidateInput.String(_View.Table.Rows[index].Cells["UoM"].Value));
                                //items.Add("AccountCode", ValidateInput.String(_View.Table.Rows[index].Cells["G/L Account"].Value));
                                //items.Add("U_OldItemNo", ValidateInput.String(_View.Table.Rows[index].Cells["Old Item No."].Value));
                                //items.Add("U_UnitPricePerPiece", ValidateInput.Double(_View.Table.Rows[index].Cells["Unit Price per piece"].Value));
                                //items.Add("U_GrossPricePerPiece", ValidateInput.Double(_View.Table.Rows[index].Cells["Gross Price per piece"].Value));
                            }

                            //int rowIndex = Convert.ToInt32(_View.Table.Rows[index].Cells["Index"].Value);

                            //On Comment 12/13/19
                            //var unitprice = ValidateInput.String(_View.Table.Rows[index].Cells["Unit Price"].Value).Replace(",", "");
                            //items.Add("UnitPrice", unitprice);

                            //01/14/2020

                            //if (ValidateInput.String(_View.Table.Rows[index].Cells["Discount"].Value) != "0")
                            //{
                            //    items.Add("DiscountPercent", ValidateInput.String(_View.Table.Rows[index].Cells["Discount %"].Value));

                            //    var grossprice = ValidateInput.String(_View.Table.Rows[index].Cells["Gross Price"].Value).Replace(",", "");
                            //    items.Add("PriceAfterVAT", grossprice);

                            //    double NetPrice = Convert.ToDouble(_View.Table.Rows[index].Cells["Line Total"].Value);
                            //    //string strNetPrice = Math.Round(NetPrice / Convert.ToDouble(_View.Table.Rows[index].Cells["Quantity"].Value.ToString()), 3).ToString();
                            //    var strNetPrice = NetPrice / Convert.ToDouble(_View.Table.Rows[index].Cells["Quantity"].Value);
                            //    items.Add("Price", strNetPrice);
                            //}
                            //else
                            //{
                            //    var unitprice = ValidateInput.String(_View.Table.Rows[index].Cells["Unit Price"].Value).Replace(",", "");
                            //    items.Add("UnitPrice", unitprice);

                            //    double NetPrice = Convert.ToDouble(_View.Table.Rows[index].Cells["Line Total"].Value);
                            //    items.Add("LineTotal", Math.Round(NetPrice, 3).ToString());
                            //}


                            //========

                            //items.Add("Quantity", ValidateInput.String(_View.Table.Rows[index].Cells["Quantity"].Value));
                            items.Add("U_Style", ValidateInput.String(_View.Table.Rows[index].Cells["Style"].Value));
                            items.Add("U_Color", ValidateInput.String(_View.Table.Rows[index].Cells["Color"].Value));
                            items.Add("U_Size", ValidateInput.String(_View.Table.Rows[index].Cells["Size"].Value));
                            items.Add("VatGroup", ValidateInput.String(_View.Table.Rows[index].Cells["Tax"].Value));

                            string dept = helper.ReadDataRow(Hana.Get("SELECT U_Dim1 FROM OCRD Where CardCode = '" + _View.SuppCode + "'"), "U_Dim1", "", 0);
                            string brand = helper.ReadDataRow(Hana.Get("SELECT U_ID019 FROM OITM Where ItemCode = '" + ItemCode + "'"), "U_ID019", "", 0);
                            string project = helper.ReadDataRow(Hana.Get("SELECT ProjectCod FROM OCRD Where CardCode = '" + _View.SuppCode + "'"), "ProjectCod", "", 0);

                            items.Add("CostingCode", ValidateInput.String(dept));
                            items.Add("COGSCostingCode", ValidateInput.String(dept));

                            items.Add("CostingCode2", ValidateInput.String(brand));
                            items.Add("COGSCostingCode2", ValidateInput.String(brand));

                            items.Add("ProjectCode", ValidateInput.String(project));
                            items.Add("U_Company", ValidateInput.String(_View.Table.Rows[index].Cells["Company"].Value));


                            //items.Add("U_Chain", ValidateInput.String(_View.Table.Rows[index].Cells["Chain Pricetag"].Value));
                            //items.Add("U_ChainDescription", ValidateInput.String(_View.Table.Rows[index].Cells["Chain Description"].Value));
                            //items.Add("U_PricetagCount", ValidateInput.String(_View.Table.Rows[index].Cells["Pricetag Count"].Value));
                            //items.Add("U_Remarks", ValidateInput.String(_View.Table.Rows[index].Cells["Remarks"].Value));
                            //items.Add("U_BrandName", ValidateInput.String(PurchasingModel.PurchaseOrderDocument.Find(f => f.Index == rowIndex).Brand));
                            //items.Add("DocType", ValidateInput.String(_View.Table.Rows[index].Cells["DocType"].Value));

                            //if (InvoiceHeaderModel.oSelectedDoc == "Pick List" && _View.DocEntry == "")
                            //{
                            //    strBaseEntry = ValidateInput.String(_View.Table.Rows[index].Cells["BaseEntry"].Value);
                            //}
                            //else
                            //{
                            //    strBaseEntry = InvoiceHeaderModel.oSelectedDoc == "" ? "0" : _View.DocEntry.ToString();
                            //}

                            //if (strBaseEntry != "0")
                            //{
                            //    string strBaseLine = ValidateInput.String(_View.Table.Rows[index].Cells["LineNum"].Value);

                            //    items.Add("BaseType", "17");
                            //    items.Add("BaseEntry", strBaseEntry);
                            //    items.Add("BaseLine", strBaseLine);
                            //}
                        }

                        if (_View.Service != "Online Order")
                        {
                            if (ValidateInput.String(_View.Table.Rows[index].Cells["Discount"].Value) != "0")
                            {
                                items.Add("DiscountPercent", ValidateInput.String(_View.Table.Rows[index].Cells["Discount %"].Value));

                                var grossprice = ValidateInput.String(_View.Table.Rows[index].Cells["Gross Price"].Value).Replace(",", "");
                                items.Add("PriceAfterVAT", grossprice);

                                double NetPrice = Convert.ToDouble(_View.Table.Rows[index].Cells["Line Total"].Value);
                                //string strNetPrice = Math.Round(NetPrice / Convert.ToDouble(_View.Table.Rows[index].Cells["Quantity"].Value.ToString()), 3).ToString();
                                var strNetPrice = NetPrice / Convert.ToDouble(_View.Table.Rows[index].Cells["Quantity"].Value);
                                items.Add("Price", strNetPrice);
                            }
                            else
                            {
                                var unitprice = ValidateInput.String(_View.Table.Rows[index].Cells["Unit Price"].Value).Replace(",", "");
                                items.Add("UnitPrice", unitprice);

                                double NetPrice = Convert.ToDouble(_View.Table.Rows[index].Cells["Line Total"].Value);
                                items.Add("LineTotal", Math.Round(NetPrice, 3).ToString());
                            }
                        }
                       

                        items.Add("Quantity", ValidateInput.String(_View.Table.Rows[index].Cells["Quantity"].Value));

                        dictLines.Add(items);
                    }
                }

                StringBuilder json = new StringBuilder();

                bool isUserGoApproval = false;

                var userApprovalCount = DataRepositoryForInvoice.GetData(_reps.UserApprovalCheck());

                if (userApprovalCount.Rows.Count > 0)
                {
                    if (userApprovalCount.Rows[0][0] != null)
                    {
                        if (userApprovalCount.Rows[0][0].ToString() != string.Empty)
                        {
                            isUserGoApproval = true;
                        }
                    }
                }

                json = DataRepositoryForInvoice.JsonBuilder(dict, dictLines, "DocumentLines");

                var jsonHeaderOnly = DataRepositoryForInvoice.JsonHeaderBuilder(dict, dictLines, "DocumentLines");

                isPosted = _repository.ActivateService((message) => msg = message, request, _View.DocEntry, json, jsonHeaderOnly, isUserGoApproval);

                if (isPosted)
                {
                    ClearField(true);
                    StaticHelper._MainForm.ShowMessage("Operation completed successfully", false);
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage(msg, true);
                }
                return isPosted;
            }
            catch(Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                return false;
            }
           
        }

        private string ConvertToString(object value)
        {
            return value == null ? "" : value.ToString();
        }

        private double ConvertToDouble(object value)
        {
            return value == null || value.ToString() == string.Empty ? 0D : Convert.ToDouble(value);
        }
        private int ConvertToInt(object value)
        {
            return value == null || value.ToString() == string.Empty ? 0 : Convert.ToInt32(value);
        }

        public void ClearField(bool clearItems)
        {

            try
            {
                _View.DocEntry = string.Empty;
                _View.DocNum = string.Empty;
                _View.SuppCode = string.Empty;
                _View.SuppName = string.Empty;
                _View.ContactPerson = string.Empty;
                _View.Company = string.Empty;
                _View.Department = string.Empty;
                _View.BpCurrency = string.Empty;
                _View.BpRate = string.Empty;
                _View.Service = " ";
                _View.Remark = string.Empty;
                _View.TotalBeforeDiscount = string.Empty;
                _View.DiscountInput = string.Empty;
                _View.DiscountInput = string.Empty;
                _View.Tax = string.Empty;
                _View.Total = string.Empty;
                _View.RefNo = string.Empty;
                _View.PostingDate = DateTime.Today.ToString("MM/dd/yyyy");
                _View.DeliveryDate = string.Empty;
                _View.DocumentDate = DateTime.Today.ToString("MM/dd/yyyy");
                _View.CancellationDate = string.Empty;
                _View.Status = "Open";
                _View.oShipTo = string.Empty;
                _View.TaxAmount = string.Empty;
                _View.TxtTotalQty = string.Empty;

                //ChangeDocumentNumber(_View.Series);

                SalesAR_generics.index = 0;

                foreach (DataGridViewRow udfRow in _View.Udf.Rows)
                {
                    try
                    {
                        udfRow.Cells[2].Value = null;
                    }
                    catch { }

                    foreach (DataGridViewRow row in _View.Udf.Rows)
                    {
                        if (row.Cells[1].Value.ToString().Equals("Prepared By"))
                        {
                            _View.Udf.Rows[row.Index].Cells["Field"].Value = DomainLayer.Models.EasySAPCredentialsModel.ESUserId;
                            _View.Udf.Rows[row.Index].Cells["Field"].ReadOnly = true;
                            break;
                        }
                    }
                }

                _View.Udf.Controls.Cast<Control>().Where(x => x.GetType() == typeof(DateTimePicker)).ToList().ForEach(x => {

                    _View.Udf.Controls.Remove(x);
                });


                if (clearItems)
                {
                    ClearItems();
                }
            }
            catch(Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
            
        }

        private void ClearItems()
        {
            if (_View.Table.RowCount > 0)
            {
                InvoiceHeaderModel.InvoiceHeader.Clear();
                InvoiceItemsModel.InvoiceItems.Clear();
                _View.Table.Rows.Clear();
                _View.TablePreview.Rows.Clear();
                _View.Table.Columns.Clear();
                _View.TablePreview.Columns.Clear();
            }
        }

        public FileInfo[] GetDocumentCrystalForms()
        {
            var sys = new SystemSettings();
            //string path = sys.PathExist($"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\AR");
            var _settingsService = new SettingsService();

            string path = sys.PathExist($"{_settingsService.GetReportPath()}Sales\\");
            
            FileInfo[] Files = sys.FileList(path, "*OINV*" + "*.rpt");

            return Files;
        }

        public void LoadPickListDocDetails()
        {

            string oSelCode = InvoiceHeaderModel.oCode;
            string oSelQry1 = "";

            if (InvoiceHeaderModel.DDWdocentry.Select(x => x.OrderEntry).Distinct().Count() == 1)
            {
                //oSelCode = InvoiceHeaderModel.DDWdocentry.Where(y => y.BpCode != "").Select(z => z.DocEntry).First().ToString();
                //InvoiceHeaderModel.oBPCode = InvoiceHeaderModel.DDWdocentry.Where(y => y.DocEntry != 0).Select(z => z.BpCode).First().ToString();
                //InvoiceHeaderModel.oOrderEntry = InvoiceHeaderModel.DDWdocentry.Where(y => y.DocEntry != 0).Select(z => z.OrderEntry).First().ToString();
                //picklistEntry = InvoiceHeaderModel.oOrderEntry;

                //var q = " SELECT Distinct C.CardCode,A.OrderEntry " +
                //        " FROM PKL1 A LEFT JOIN RDR1 B ON A.OrderEntry = B.DocEntry " +
                //        $" LEFT JOIN ORDR C ON B.DocEntry = C.DocEntry and C.CardCode = '{InvoiceHeaderModel.oBPCode}' Where A.OrderEntry = '{InvoiceHeaderModel.oOrderEntry}' and A.AbsEntry = '" + oSelCode + "'";

                //var dt = DataAccess.Select(DataAccess.conStr("HANA"), q);

                //txtBpCode.Text = DECLARE.dtNull(dt, 0, "CardCode", "");
                //LoadBPDetails(DECLARE.dtNull(dt, 0, "CardCode", ""));

                //txtSODocEntry.Text = DECLARE.dtNull(dt, 0, "OrderEntry", "");
                //txtSONumber.Text = fS.oCode;
            }

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
    }
}

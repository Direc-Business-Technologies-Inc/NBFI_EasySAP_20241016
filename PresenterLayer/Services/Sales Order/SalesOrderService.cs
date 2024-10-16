using DirecLayer;
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
using PresenterLayer.Services.SalesOrder;
using DomainLayer;
using PresenterLayer.Helper.SalesOrder;
using PresenterLayer.Views;
using MetroFramework;
using DomainLayer.Helper;
using System.IO;
using PresenterLayer.Services.Security;

namespace PresenterLayer.Services
{
    public class SalesOrderService
    {
        private readonly IFrmSalesOrder _View;
        private readonly ISalesOrderModel _repository;

        PurchasingAP_Style _style = new PurchasingAP_Style();
        PurchasingAP_Computation _computation = new PurchasingAP_Computation();
        ValidationRepository _validation = new ValidationRepository();
        UdfRepository _udfRepo = new UdfRepository();
        StringQueryRepository _reps = new StringQueryRepository();
        SAPHanaAccess Hana = new SAPHanaAccess();
        //private FrmSalesOrder view;
        //private Models.SalesOrderModel model;
        private static DateTimePicker oDateTimePicker = new DateTimePicker();
        public SalesOrderService(IFrmSalesOrder view, ISalesOrderModel repository)
        {
            _View = view;
            _View.Presenter = this;
            _repository = repository;

            Onload();
        }

        internal void Onload()
        {
            DataTable udf = _repository.GetUDF();

            _udfRepo.Style(_View.Udf);
            _udfRepo.LoadUdf(_View.Udf, udf, "ORDR");

            _View.Status = "Open";

            if (oDateTimePicker.IsDisposed)
            {
                oDateTimePicker = new DateTimePicker();
            }
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

            int count = PurchasingModel.PurchaseOrderDocument.Count();
            int totalRow = id - count;

            PurchasingModel.PurchaseOrderDocument.ToList().ForEach(x =>
            {
                x.Index = id2;
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
                    SalesModel.SalesOrderDocument.ForEach(x =>
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
                    UpdateCell(colIndex, m);
                    PurchasingModel.PurchaseOrderDocument.Find(x => x.Index == index).Department = m;
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
                FrmSalesOrderItemList form = new FrmSalesOrderItemList()
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
                SalesOrderItemsController.dgvItemsLayout(dgv);
                foreach (var x in SalesOrderItemsModel.SalesOrderItems.ToList())
                {
                    object[] a = { x.ItemCode, x.ItemName ,x.Brand, x.Style, x.Color, x.Size, x.Section, x.BarCode, x.EffectivePrice,x.GrossPrice.ToString("0.##"),
                                   x.UnitPrice.ToString("0.##"), x.Quantity, x.DiscountPerc.ToString("0.##"), x.DiscountAmount, x.EmpDiscountPerc.ToString("0.##") ,
                                   x.FWhsCode, "...", x.TaxCode, "...", x.TaxRate.ToString("0.##"), x.LineTotal.ToString("0.##"), x.GrossTotal.ToString("0.##"),
                                   x.PriceAfterDisc.ToString("0.##"), x.Linenum, "", "", x.OrderedQty };
                    dgv.Rows.Add(a);
                }
                SalesOrderItemsController.dataGridLayout(dgv);
                //TotalComputation();
            }
            catch(Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message,false);
            }
           
        }

        internal void GetWarehouse(int colIndex, int index)
        {
            string warehouse = _repository.SelectWarehouse();

            if (warehouse != string.Empty)
            {
                UpdateCell(colIndex, warehouse);
                SalesOrderItemsModel.SalesOrderItems.Find(x => x.Linenum == index).FWhsCode = warehouse;
            }
        }

        internal void GetTaxCode(DataGridViewCellEventArgs e, int colIndex, int index)
        {
            //var table = _View.Table.Focus() ? _View.Table : _View.TablePreview;

            Dictionary<string, string> taxCode = _repository.SelectTaxGroup();

            if (taxCode.Count > 0)
            {
                UpdateCell(colIndex, taxCode["Group"]);
                UpdateCell(colIndex + 2, Convert.ToDecimal(taxCode["Rate"]).ToString("0.##"));

                //UpdateCell(colIndex + 2, Convert.ToDouble(taxCode["Rate"]).ToString("0.00"));

                SalesOrderItemsModel.SalesOrderItems.Where(x => x.Linenum == index).ToList().ForEach(x =>
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
            var header = new DataTable();
            var Lines = new DataTable();

            table = table ?? "RDR";

            if (status == "Draft-Approved" || status == "Draft-Pending")
            {
                header = _repository.SelectDraftDocument(docEntry, status);

                if (_View.Service == "Item")
                {
                    Lines = _repository.SelectItemDraftDocumentLines(docEntry, status);
                }
                else
                {
                    Lines = _repository.SelectServiceDraftDocumentLines(docEntry);
                }

                _View.Status = status == "Draft-Approved" ? "Draft Approved" : "Draft Pending";
                _udfRepo.SelectUdfDratfSO(_View.Udf, docEntry, "ODRF");
            }
            else
            {
                header = _repository.SelectDocument(table, docEntry);
                var helper = new DataHelper();
                SalesOrdersHeaderModel.oBPCode = helper.ReadDataRow(header, "CardCode", "", 0);
                //SalesOrdersHeaderModel.oWhsCode = helper.ReadDataRow(header, "CardCode", "", 0);
                if (ConvertToString(header.Rows[0]["DocType"]) == "I")
                {
                    Lines = _repository.SelectDocumentLines(table, docEntry, status);
                }
                else
                {
                    Lines = _repository.SelectServiceDocumentLines(table, docEntry);
                }

                _View.Status = status == "Open" ? "Open" : "Closed";
                _View.Status = ConvertToString(header.Rows[0]["CANCELED"]) == "N" ? _View.Status : "Canceled";
                _udfRepo.SelectUdfDratfSO(_View.Udf, docEntry, $"O{table}");
            }

            if (header.Rows.Count > 0)
            {
                foreach (DataRow head in header.Rows)
                {
                    _View.DocEntry = ConvertToString(head["DocEntry"]);

                    _View.SuppCode = ConvertToString(head["CardCode"]);
                    _View.SuppName = ConvertToString(head["CardName"]);
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

                    //_View.Series = ConvertToString(head["Series"]);
                    _View.DocNum = ConvertToString(head["DocNum"]);

                    _View.oSalesEmployeeName = ConvertToString(head["SlpName"]);
                    _View.DiscountInput = ConvertToString(Convert.ToDouble(head["DiscPrcnt"]).ToString("#,##0.00"));
                    _View.DiscountAmount = ConvertToString(Convert.ToDouble(head["DiscSum"]).ToString("#,##0.00"));
                    //_View.Tax = ConvertToString(head["Tax"]);
                }

                if (Lines.Rows.Count > 0)
                {
                    Double LineTotal;
                    Double GrossTotal;
                    Double VatAmount;
                    Double DiscAmt;
                    foreach (DataRow line in Lines.Rows)
                    {
                        var sales = new SalesOrderItemsModel.SalesOrderItemsData();
                        var DocumentBrand = _repository.GetDocumentBrand(ConvertToString(line["ItemCode"]));
                       // Dictionary<string, string> DocumentGl = _repository.GetDocumentGl(ConvertToString(line["AcctCode"]));

                        double qty = ConvertToDouble(line["Quantity"]);
                        //double discAmount = ConvertToDouble(line["DiscPrcnt"]);
                        double pricebefdisc = ConvertToDouble(line["PriceBefDi"]);
                        double discount = ConvertToDouble(line["DiscPrcnt"]);
                        double vatrate = ConvertToDouble(line["VatPrcnt"]);

                        //LineTotal = (qty * pricebefdisc);
                        LineTotal = ConvertToDouble(line["LineTotal"]);

                           double tax = Convert.ToDouble(Hana.Get($@"SELECT T0.ECVatGroup, T1.Rate FROM OCRD T0  INNER JOIN OVTG T1 ON T0.ECVatGroup = T1.Code WHERE T0.CardCode = '{_View.BPCode}'").Rows[0]["Rate"].ToString());
                            tax = 1 + (tax / 100);

                        DiscAmt = (discount / 100) * Convert.ToDouble(pricebefdisc * tax);
                        VatAmount = LineTotal * (vatrate / 100) - ((LineTotal * (vatrate / 100)) * (discount / 100));
                        GrossTotal = (LineTotal + VatAmount) - (DiscAmt * qty);

                        sales.ItemCode = ConvertToString(line["ItemCode"]); // ItemCode
                        sales.ItemName = ConvertToString(line["Dscription"]); // ItemCode
                        sales.Style = ConvertToString(line["U_StyleCode"]); //Style
                        sales.Color = ConvertToString(line["U_Color"]); //Color
                        sales.Size = ConvertToString(line["U_Size"]); //Size
                        sales.Brand = DocumentBrand.Count > 0 ? DocumentBrand["Name"] : ""; //Brand
                        sales.Section = ConvertToString(line["U_Section"].ToString()); //Section
                        sales.BarCode = ConvertToString(line["CodeBars"]);
                        sales.GrossPrice = ConvertToDouble(line["PriceAfVAT"]); //GrossPrice
                        sales.UnitPrice = ConvertToDouble(line["PriceBefDi"]);
                        sales.Quantity = qty; //Requested to remove logic 07212021 //SalesOrderItemsController.GetOrigQty(ConvertToString(line["ItemCode"]), qty); //Qty
                        sales.OrderedQty = qty;
                        sales.DiscountPerc = discount;
                        sales.DiscountAmount = Math.Round(DiscAmt, 2);

                        sales.FWhsCode = ConvertToString(line["WhsCode"]); //WHsCode
                        sales.TaxCode = ConvertToString(line["VatGroup"]); //Tax Code
                        sales.TaxAmount = ConvertToDouble(line["LineVat"]);
						//sales.TotalLCRaw = ConvertToDouble(line["INMPrice"]); //CHECKING
						sales.TaxRate = ConvertToDouble(line["VatPrcnt"]);
                        sales.LineTotal = Math.Round(LineTotal, 2);
                        //sales.Index = Convert.ToInt32(ConvertToString(line["LineNum"]));
                        sales.GrossTotal = Math.Round(GrossTotal, 2);
                        sales.PriceAfterDisc = Math.Round(Convert.ToDouble(pricebefdisc), 2);
                        sales.EmpDiscountPerc = GetDiscount(_View.SuppCode, ConvertToString(line["ItemCode"].ToString()));
                        sales.EffectivePrice = GetEffectivePrice(_View.SuppCode, ConvertToString(line["ItemCode"].ToString()));
                        sales.Linenum = Convert.ToInt32(line["LineNum"]);

                        _View.Tax = ConvertToString(line["VatGroup"]);

                        //sales.Index = Convert.ToInt32(ConvertToString(line["LineNum"]));
                        //sales.ItemNo = ConvertToString(line["Item No."]);
                        //sales.ItemDescription = ConvertToString(line["Item Description"]);
                        //sales.BarCode = ConvertToString(line["CodeBars"]);
                        //sales.Color = ConvertToString(line["U_ID022"]);
                        //sales.Size = ConvertToString(line["U_ID007"]);
                        //sales.StyleCode = ConvertToString(line["U_ID012"]);
                        //sales.Style = ConvertToString(line["U_ID012"]);
                        //sales.UoM = ConvertToString(line["UOM"]);
                        //sales.BrandCode = DocumentBrand.Count > 0 ? DocumentBrand["Code"] : "";
                        //sales.Brand = DocumentBrand.Count > 0 ? DocumentBrand["Name"] : "";
                        //sales.PricetagCount = ConvertToDouble(line["Pricetag Count"]);
                        //sales.ChainDescription = ConvertToString(line["Chain Desc"]);
                        //sales.GLAccount = DocumentGl.Count > 0 ? DocumentGl["Code"] : "";
                        //sales.GLAccountName = DocumentGl.Count > 0 ? DocumentGl["Name"] : "";
                        //sales.Quantity = qty;
                        //sales.Warehouse = ConvertToString(line["WhsCode"]);
                        //sales.TaxCode = ConvertToString(line["VatGroup"]);
                        //sales.TaxAmount = ConvertToDouble(line["LineVat"]);
                        //sales.TaxRate = ConvertToDouble(line["VatPrcnt"]);
                        //sales.UnitPrice = ConvertToDouble(line["PriceBefDi"]);
                        //sales.TotalLC = ConvertToDouble(line["INMPrice"]);
                        //sales.TotalLCRaw = ConvertToDouble(line["INMPrice"]);
                        //sales.GrossTotalLC = ConvertToDouble(line["GTotal"]);
                        //sales.DiscountPerc = discAmount;
                        //sales.DiscAmount = discAmount * qty;
                        //sales.ChainPricetag = ConvertToString(line["U_Chain"]);
                        //sales.GrossPrice = ConvertToDouble(line["PriceAfVAT"]);
                        //sales.Remarks = ConvertToString(line["Remarks"]);
                        //sales.Project = ConvertToString(line["Project"]);

                        //if (_View.Service == "Service")
                        //{
                        //    sales.UnitPricePerPCS = ConvertToDouble(line["U_UnitPricePerPiece"]);
                        //    sales.GrossPricePerPCS = ConvertToDouble(line["U_GrossPricePerPiece"]);
                        //}
                        //sales.Section = ConvertToString(line["U_ID018"].ToString());

                        SalesOrderItemsModel.SalesOrderItems.Add(sales);
                    }
                    //SalesOrderItemsController.dgvItemsLayout(_View.Table);
                    //SalesOrderItemsController.dgvItemsLayout(_View.TablePreview);
                }

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

        internal void ExecuteCopyDocument(string doc)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var suppcode = _View.SuppCode;
                ClearField(true);

                switch (doc)
                {
                    case "Purchase Quotation":

                        if (suppcode != string.Empty)
                        {
                            _View.DocEntry = _repository.CopyFrom(_View.SuppCode, "CopyFromPurchQout");

                            if (_View.DocEntry != string.Empty || _View.DocEntry.Length > 0)
                            {
                                GetSelectedDocument("PQT", _View.DocEntry, "O");
                            }
                        }

                        break;

                    case "Purchase Request":

                        _View.DocEntry = _repository.CopyFrom(_View.SuppCode, "CopyFromPurchRequest");

                        if (_View.DocEntry != string.Empty || _View.DocEntry.Length > 0)
                        {
                            GetSelectedDocument("PRQ", _View.DocEntry, "O");
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
            //try
            //{
            //    double discount = _computation.PercentConverter(_View.DiscountInput);

            //    _View.DiscountAmount = _computation.ComputeFooterDiscount(ConvertToDouble(_View.TotalBeforeDiscount),
            //        discount).ToString();
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    TotalComputation();
            //}
        }

        internal void ExecuteDeleteItem()
        {
            var currentTable = _View.Table.Focus() ? _View.Table : _View.TablePreview;

            for (int i = 0; i < currentTable.RowCount - 1; i++)
            {
                if (currentTable.Rows[i].Selected)
                {
                    int index = Convert.ToInt32(currentTable.Rows[i].Cells["Index"].Value);
                    PurchasingModel.PurchaseOrderDocument.RemoveAll(x => x.Index == index);
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
        DataTable GetData(string query)
        {
            var sapHanaAccess = new SAPHanaAccess();
            DataTable queryReturn = sapHanaAccess.Get(query);
            return queryReturn;
        }
        internal void UdfRequest()
        {
            string fieldName = _View.Udf.CurrentRow.Cells[0].Value.ToString();
            var row = _View.Udf.CurrentRow.Index;
            var col = _View.Udf.CurrentCell.ColumnIndex;
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
                //result = _repository.GetAllEmployees();
                result = GetSOqry(fieldName).Contains("$") ? _repository.GetAllEmployees() : SelectUDFFMS("ORDR", fieldName);
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
            //else if (fieldName == "U_RAS")
            //{
            //    result = result = _repository.GetAllEmployees();
            //}
            else if (fieldName.Equals("U_VDesc"))
            {
                //Vehicle Desc
                var code = SelectVehicle();
                _View.Udf.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[0].Value != null)
                    {
                        if (x.Cells[0].Value.ToString() == fieldName)
                        {

                            x.Cells[2].Value = code == "" ? x.Cells[2].Value : code;
                            //_frmITR.UDF.CurrentCell = x.Cells[2];
                            _View.Udf.CurrentCell = _View.Udf[col, row];

                        }
                    }
                });

                if (code != "")
                {
                    DataTable dt1 = GetData($"SELECT U_VPla, U_VDriver FROM [@TRUCK] WHERE U_VDesc = '{code}'");
                    _View.Udf.Rows.Cast<DataGridViewRow>().ToList()
                    .ForEach(x =>
                    {
                        if (x.Cells[0].Value != null)
                        {
                            if (x.Cells[0].Value.ToString() == fieldName)
                            {
                                var vcode = code == "" ? x.Cells[2].Value : code;
                                x.Cells[2].Value = $"{vcode} - {dt1.Rows[0]["U_VPla"].ToString()}";

                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                _View.Udf.CurrentCell = _View.Udf[col, row];

                            }
                            else if (x.Cells[0].Value.ToString() == "U_VPla")
                            {

                                x.Cells[2].Value = dt1.Rows[0]["U_VPla"].ToString();

                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                _View.Udf.CurrentCell = _View.Udf[col, row];

                            }
                            else if (x.Cells[0].Value.ToString() == "U_Driver")
                            {
                                x.Cells[2].Value = dt1.Rows[0]["U_VDriver"].ToString();

                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                _View.Udf.CurrentCell = _View.Udf[col, row];
                            }
                        }
                    });

                }
            }
            if (result != string.Empty)
            {
                _View.Udf.CurrentRow.Cells[2].Value = result;
            }
            
            _View.Udf.CurrentCell = _View.Udf[col, row];
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

        string SelectVehicle()
        {
            string result = "";
            var list = Modal("Get Vehicle List", null, "List of Vehicle");
            if (list.Count > 0)
            {
                result = list[0];
            }
            return result;
        }

        public void ConvertToDate(DataGridView dgv)
        {
            var row = dgv.CurrentRow;
            string fieldName = row.Cells[0].Value.ToString();

            if (fieldName.Contains("U_ShipmentDate") || fieldName.Contains("U_OrRecDate") || fieldName.Contains("U_RetDate"))
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
            try
            {
                var dtPicker = (DateTimePicker)sender;
                var qe = dtPicker.Value.ToShortDateString();

                _View.Udf.CurrentCell.Value = qe;

                dtPicker.Visible = false;
                oDateTimePicker.Visible = false;
            }
            catch(Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
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
                PurchasingModel.PurchaseOrderDocument.Find(x => x.Index == index).Project = project;
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
            _View.Table.CurrentRow.Cells[ColIndex].Value = value;

            int count = _View.TablePreview.RowCount;

            if (count > 1)
            {
                _View.TablePreview.CurrentRow.Cells[ColIndex].Value = value;
            }
        }

        public void CurrentRowComputation(DataGridViewCellEventArgs e, int intIndex = 0, bool isTaxChange = false)
        {
            var rowIndex = e != null ? e.RowIndex : intIndex;
            var colIndex = e == null ? 0 : e.ColumnIndex;

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

            //double index = ConvertToDouble(currentRow.Cells["LineNum"].Value);

            //var item = PurchasingModel.PurchaseOrderDocument.Find(x => x.Index == index);
            //var item = SalesOrderItemsModel.SalesOrderItems.Find(x => x.Linenum == index);

            
            //if (item != null)
            //{
            //    double taxRate = item.TaxRate != 0D ? _computation.PercentConverter(item.TaxRate) + 1 : 1D;
            //    double discountPerc = _computation.PercentConverter(_validation.ValidateCellsDouble(currentRow, "Discount %").ToString());

            //    item.Remarks = _validation.ValidateCells(currentRow, "Remarks");
            //    item.Quantity = _validation.ValidateCellsDouble(currentRow, "Quantity");
            //    item.DiscountPerc = _validation.ValidateCellsDouble(currentRow, "Discount %");

            //    var TaxCode = _validation.ValidateCells(currentRow, "Tax Code");

            //    if (currentColumn.Equals("Gross Price"))
            //    {
            //        var GrossPrice = _validation.ValidateCellsDouble(currentRow, "Gross Price");
            //        item.UnitPrice = _computation.ComputeUnitPrice(GrossPrice, taxRate, TaxCode);
            //        item.GrossPrice = _computation.ComputeGrossPrice(item.UnitPrice, taxRate, TaxCode);
            //        item.UnitPriceRaw = _computation.ComputeUnitPriceRaw(item.GrossPrice, taxRate, TaxCode);
            //    }
            //    else if (currentColumn.Equals("Unit Price"))
            //    {
            //        var UnitPrice = _validation.ValidateCellsDouble(currentRow, "Unit Price");
            //        UnitPrice = ConvertToDouble(UnitPrice);
            //        item.UnitPrice = UnitPrice;
            //        item.UnitPriceRaw = UnitPrice;
            //        item.GrossPrice = _computation.ComputeGrossPrice(UnitPrice, taxRate, TaxCode);
            //    }
            //    else
            //    {
            //        var UnitPrice = _validation.ValidateCellsDouble(currentRow, "Unit Price");
            //        UnitPrice = ConvertToDouble(UnitPrice);
            //        item.UnitPrice = UnitPrice;
            //        item.GrossPrice = _computation.ComputeGrossPrice(UnitPrice, taxRate, TaxCode);
            //        item.UnitPriceRaw = _computation.ComputeUnitPriceRaw(item.GrossPrice, taxRate, TaxCode);
            //    }

            //    // discount function
            //    item.DiscAmount = _computation.ComputeDiscountAmount(discountPerc, Convert.ToDouble(item.UnitPriceRaw), item.Quantity);

            //    var computeDiscount = item.GrossPrice - (item.GrossPrice * discountPerc);

            //    item.GrossPrice = Convert.ToDouble(Math.Round(computeDiscount, 2).ToString("0.00"));

            //    item.GrossPriceRaw = computeDiscount;

            //    var qty = _View.Service == "Service" ? 1 : item.Quantity;

            //    item.TotalLC = Convert.ToDouble(Math.Round(_computation.ComputeRowTotals(qty, Convert.ToDouble(item.UnitPrice), item.DiscAmount), 2).ToString("0.00"));

            //    item.TotalLCRaw = _computation.ComputeRowTotals(qty, Convert.ToDouble(item.UnitPriceRaw), item.DiscAmount);

            //    item.TaxAmount = _computation.ComputeTax(item.TotalLCRaw, taxRate, item.DiscAmount);

            //    item.GrossTotalLC = Convert.ToDouble(Math.Round((qty * item.GrossPriceRaw), 2).ToString("0.00"));

            //    if (_View.Service == "Service")
            //    {
            //        item.ItemNo = ValidateInput.String(currentRow.Cells[0].Value);
            //        item.BarCode = ValidateInput.String(currentRow.Cells[1].Value);
            //        item.ItemDescription = ValidateInput.String(currentRow.Cells[2].Value);
            //    }

            //    if (item.ChainPricetag != "" && item.ChainPricetag != null)
            //    {
            //        item.PricetagCount = item.Quantity;
            //        currentRow.Cells["Pricetag Count"].Value = item.Quantity;
            //    }

            //    currentRow.Cells["Unit Price"].Value = item.UnitPrice.ToString("0.00");
            //    currentRow.Cells["Gross Price"].Value = item.GrossPrice.ToString("0.00");
            //    currentRow.Cells["Total(LC)"].Value = item.TotalLC.ToString("0.00");
            //    currentRow.Cells["Gross Total (LC)"].Value = item.GrossTotalLC.ToString("0.00");

            //    if (_View.Service == "Item")
            //    {
            //        try
            //        {
            //            currentRow.Cells["Disc Amount"].Value = item.DiscAmount;
            //        }
            //        catch { }
            //    }

            //    TotalComputation();
            }
        

        private void TotalComputation()
        {
            double taxAmount = 0;
            double totalBefDiscount = 0;

            foreach (var x in SalesOrderItemsModel.SalesOrderItems.ToList()) //DomainLayer.SalesModel.SalesOrderDocument.ToList())

			{
                taxAmount += x.TaxAmount;
                totalBefDiscount += x.TotalLCRaw;
            }

            _View.TotalBeforeDiscount = Math.Round(Convert.ToDecimal(totalBefDiscount), 3).ToString("0.00");
            _View.Tax = Math.Round(Convert.ToDecimal(taxAmount), 2).ToString("0.00");
            //_View.DiscountAmount = discount.ToString("#,##0.00");

            if (_View.Tax != null)
            {
                try
                {
                    if (Convert.ToDouble(taxAmount) > 0)
                    {
                        double result = (Convert.ToDouble(_View.TotalBeforeDiscount) - Convert.ToDouble(_View.DiscountAmount)) * .12;
                        _View.Tax = Math.Round(Convert.ToDecimal(result), 2).ToString("0.00");
                    }
                    else
                    {
                        _View.Tax = Math.Round(Convert.ToDecimal(taxAmount), 2).ToString("0.00");
                    }
                }
                catch (Exception ex)
                {
                    _View.Tax = Math.Round(Convert.ToDecimal(taxAmount), 2).ToString("0.00");
                }
            }

            double total = Convert.ToDouble(_View.TotalBeforeDiscount) + Convert.ToDouble(_View.Tax);

            if (_View.DiscountAmount != string.Empty)
            {
                total = total - Convert.ToDouble(_View.DiscountAmount);
            }

            _View.Total = Math.Round(Convert.ToDecimal(total), 2).ToString("0.00");
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

            string result = "";
            //result = SAPHana.GetQuery($"SELECT UomEntry FROM OUOM WHERE UomName = '{sUomCode}'", 0);
            return string.IsNullOrEmpty(result) ? "0" : result;
        }


        public bool ExecuteRequest(string request)
        {
            bool isPosted = false;
            string msg = string.Empty;
            int UDfCount = _View.Udf.RowCount;
            int rowCount = _View.Table.RowCount;

            var dict = new Dictionary<string, string>()
            {
                {"CardCode", _View.SuppCode},
                {"CardName", _View.SuppName},
                {"NumAtCard", _View.ContactPerson},
                {"U_CompanyTIN", _View.Company },
                {"U_Department", _View.Department},
                {"DocCurrency", _View.BpCurrency},
                {"Warehouse", _View.Warehouse},
                {"Series", _View.Series},
                //{"DocRate", _View.BpRate == string.Empty ? "1" : _View.BpRate},
                {"DocDate", ValidateInput.ConvertToDate(_View.PostingDate, "yyyy-MM-dd") },
                {"TaxDate", ValidateInput.ConvertToDate(_View.DocumentDate, "yyyy-MM-dd") },
                {"DocDueDate",ValidateInput.ConvertToDate(_View.DeliveryDate, "yyyy-MM-dd")},
               // {"CancelDate", ValidateInput.ConvertToDate(_View.CancellationDate, "yyyyMMdd")},

                { "Comments", _View.Remark},
                {"U_PrepBy", DomainLayer.Models.EasySAPCredentialsModel.ESUserId},
                {"U_DocType", _View.Service},
                {"DocObjectCode" , "17"},
                {"DocumentsOwner",  _repository.GetEmpID() },
                {"DiscountPercent",  _View.DiscountInput }
            };

            if (_View.CancellationDate != " ")
            {
                dict.Add("U_CancelDate", ValidateInput.ConvertToDate(_View.CancellationDate, "yyyy-MM-dd"));
                //dict.Add("U_CancelDate", ValidateInput.ConvertToDate(_View.CancellationDate, "MMddyyyy"));
            }

            if (_View.oSalesEmployee != "")
            {
                dict.Add("SalesPersonCode", _View.oSalesEmployee);
            }

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
                            var date = Convert.ToDateTime(ValidateInput.String(_View.Udf.Rows[i].Cells[2].Value)).ToString("yyyy-MM-dd");

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
            dict.Add("U_PostRem", $"{(request.Equals("Add") ? "Created" : "Updated")} by EasySAP | Sales Order : {sboCred.UserId} : {DateTime.Now} : | Powered By : DIREC");

            var dictLines = new List<Dictionary<string, object>>();

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

                    //if (_View.Service == "Item")
                    //{
                    //    items.Add("ItemCode", ValidateInput.String(_View.Table.Rows[index].Cells["Item No."].Value));
                    //    items.Add("DiscountPercent", ValidateInput.String(_View.Table.Rows[index].Cells["Discount %"].Value));
                    //    var uom = _repository.GetUomEntry(ValidateInput.String(_View.Table.Rows[index].Cells["UoM"].Value));
                    //    items.Add("UoMEntry", uom);
                    //    items.Add("WarehouseCode", ValidateInput.String(_View.Table.Rows[index].Cells["Warehouse"].Value));
                    //}
                    //else
                    //{
                        items.Add("ItemCode", ValidateInput.String(_View.Table.Rows[index].Cells["Item No."].Value));
                        
                        //var uom = _repository.GetUomEntry(ValidateInput.String(_View.Table.Rows[index].Cells["UoM"].Value));
                        //items.Add("UoMEntry", uom);
                        items.Add("WarehouseCode", ValidateInput.String(_View.Warehouse));
                        items.Add("ItemDescription", ValidateInput.String(_View.Table.Rows[index].Cells["Item Description"].Value));
                        //items.Add("AccountCode", ValidateInput.String(_View.Table.Rows[index].Cells["G/L Account"].Value));
                        //items.Add("U_OldItemNo", ValidateInput.String(_View.Table.Rows[index].Cells["Old Item No."].Value));
                        items.Add("U_Description", ValidateInput.String(_View.Table.Rows[index].Cells["Item Description"].Value));
                        items.Add("U_Qty", ValidateInput.String(_View.Table.Rows[index].Cells["Quantity"].Value));
                        //items.Add("U_UOM", ValidateInput.String(_View.Table.Rows[index].Cells["UoM"].Value));
                        items.Add("U_TargetWhs", ValidateInput.String(_View.Warehouse));
                        //items.Add("U_UnitPricePerPiece", ValidateInput.Double(_View.Table.Rows[index].Cells["Unit Price per piece"].Value));
                        //items.Add("U_GrossPricePerPiece", ValidateInput.Double(_View.Table.Rows[index].Cells["Gross Price per piece"].Value));
                    //}

                    //int rowIndex = Convert.ToInt32(_View.Table.Rows[index].Cells["Index"].Value);
                    var grossprice = ValidateInput.String(_View.Table.Rows[index].Cells["Gross Price"].Value).Replace(",", "");
                    var unitprice = ValidateInput.String(_View.Table.Rows[index].Cells["Unit Price"].Value).Replace(",", "");
                    //On comment backup before Carton Qty 091819
                    //items.Add("Quantity", ValidateInput.String(_View.Table.Rows[index].Cells["Quantity"].Value));


                    //if (ValidateInput.String(_View.Table.Rows[index].Cells["Discount %"].Value) != "0")
                    //{
                    //items.Add("UnitPrice", unitprice);
                    //}
                    //else
                    //{

                    //New Logic for price 12/18/19
                    if (ValidateInput.String(_View.Table.Rows[index].Cells["Discount"].Value) != "0" && request.Equals("Add"))
                    {
                        items.Add("DiscountPercent", ValidateInput.String(_View.Table.Rows[index].Cells["Discount %"].Value));
                    }
                        
                    items.Add("PriceAfterVAT", grossprice);
                    var NetPrice = Convert.ToDouble(_View.Table.Rows[index].Cells["Line Total"].Value) / Convert.ToDouble(_View.Table.Rows[index].Cells["Ordered Qty"].Value);

                    if (ValidateInput.String(_View.Table.Rows[index].Cells["Discount"].Value) != "0" && request.Equals("Add"))
                    {
                        items.Add("Price", NetPrice);
                    }

                    //else
                    //{
                    //    items.Add("DiscountPercent", ValidateInput.String(_View.Table.Rows[index].Cells["Discount %"].Value));
                    //    items.Add("LineTotal", _View.Table.Rows[index].Cells["Line Total"].Value.ToString());
                    //}

                    //Old Logic
                    //items.Add("PriceAfterVAT", grossprice);
                    //items.Add("LineTotal", ValidateInput.String(_View.Table.Rows[index].Cells["Line Total"].Value).Replace(",", ""));
                    //}

                    items.Add("Quantity", ValidateInput.String(_View.Table.Rows[index].Cells["Ordered Qty"].Value));

                    items.Add("U_Style", ValidateInput.String(_View.Table.Rows[index].Cells["Style"].Value));
                    items.Add("U_Color", ValidateInput.String(_View.Table.Rows[index].Cells["Color"].Value));
                    items.Add("U_Size", ValidateInput.String(_View.Table.Rows[index].Cells["Size"].Value));
                    //items.Add("U_Chain", ValidateInput.String(_View.Table.Rows[index].Cells["Chain Pricetag"].Value));
                    //items.Add("U_ChainDescription", ValidateInput.String(_View.Table.Rows[index].Cells["Chain Description"].Value));
                    //items.Add("U_PricetagCount", ValidateInput.String(_View.Table.Rows[index].Cells["Pricetag Count"].Value));
                    //items.Add("U_Remarks", ValidateInput.String(_View.Table.Rows[index].Cells["Remarks"].Value));
                    //items.Add("CostingCode2", ValidateInput.String(PurchasingModel.PurchaseOrderDocument.Find(f => f.Index == rowIndex).BrandCode));
                    //items.Add("U_BrandName", ValidateInput.String(PurchasingModel.PurchaseOrderDocument.Find(f => f.Index == rowIndex).Brand));
                    //items.Add("COGSCostingCode", ValidateInput.String(_View.Table.Rows[index].Cells["Department"].Value));
                    items.Add("VatGroup", ValidateInput.String(_View.Table.Rows[index].Cells["Tax"].Value));
                    //items.Add("DocType", ValidateInput.String(_View.Table.Rows[index].Cells["DocType"].Value));
                    //items.Add("ProjectCode", ValidateInput.String(_View.Table.Rows[index].Cells["Project"].Value));

                    dictLines.Add(items);
                }
            }

            var json = new StringBuilder();

            var isUserGoApproval = false;

            var userApprovalCount = DataRepositoryForSO.GetData(_reps.UserApprovalCheck());

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

            json = DataRepositoryForSO.JsonBuilder(dict, dictLines, "DocumentLines");

            var jsonHeaderOnly = DataRepositoryForSO.JsonHeaderBuilder(dict, dictLines, "DocumentLines");

            isPosted = _repository.ActivateService((message) => msg = message, request, _View.DocEntry, json, jsonHeaderOnly, isUserGoApproval);
            
            if (isPosted)
            {
                ClearField(true);
                StaticHelper._MainForm.ShowMessage("Operation completed successfully");
            }
            else
            {
                StaticHelper._MainForm.ShowMessage(msg, true);
            }
            return isPosted;
        }

        private string ConvertToString(object value)
        {
            return value == null ? "" : value.ToString();
        }

        private double ConvertToDouble(object value)
        {
            return value == null || value.ToString() == string.Empty ? 0D : Convert.ToDouble(value);
        }

        public void ClearField(bool clearItems)
        {
            _View.DocEntry = string.Empty;
            _View.SuppCode = string.Empty;
            _View.SuppName = string.Empty;
            _View.ContactPerson = string.Empty;
            _View.Company = string.Empty;
            _View.Department = string.Empty;
            _View.BpCurrency = string.Empty;
            _View.BpRate = string.Empty;
            _View.Service = "Item";
            _View.Remark = string.Empty;
            _View.TotalBeforeDiscount = string.Empty;
            _View.DiscountInput = string.Empty;
            _View.DiscountInput = string.Empty;
            _View.Tax = string.Empty;
            _View.Total = string.Empty;
            _View.PostingDate = DateTime.Today.ToString("MM/dd/yyyy");
            _View.DeliveryDate = string.Empty;
            _View.DocumentDate = DateTime.Today.ToString("MM/dd/yyyy");
            _View.CancellationDate = string.Empty;
            _View.Status = "Open";
            _View.oShipTo = string.Empty;

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

            

            //foreach (Control ctrl in _View.Udf.Controls)
            //{
            //    
            //    var type = ctrl.GetType();
            //    if (ctrl.GetType() == typeof(DateTimePicker))
            //    {
            //        _View.Udf.Controls.Remove(ctrl);
            //    }
            //}
            _View.Udf.Controls.Cast<Control>().Where(x => x.GetType() == typeof(DateTimePicker)).ToList().ForEach(x => {

                _View.Udf.Controls.Remove(x);
            });


            //var count = _View.Udf.Controls.Count;

            //for (int i = 0; count > i; i++)
            //{
            //    try
            //    {
            //        var type = _View.Udf.Controls[i].GetType();

            //        if (type == typeof(DateTimePicker))
            //        {
            //            _View.Udf.Controls.Remove(_View.Udf.Controls[i]);
            //        }
            //    }
            //    catch { }
            //}


            if (clearItems)
            {
                ClearItems();
            }
        }

        private void ClearItems()
        {
            if (_View.Table.RowCount > 0)
            {
                SalesModel.SalesOrderDocument.Clear();
                _View.Table.Rows.Clear();
                _View.TablePreview.Rows.Clear();
            }
        }

        public FileInfo[] GetDocumentCrystalForms()
        {           
            var sys = new SystemSettings();
            var _settingsService = new SettingsService();

            string path = sys.PathExist($"{_settingsService.GetReportPath()}\\Sales\\");
            //string path = sys.PathExist($"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\Sales\\");

            FileInfo[] Files = sys.FileList(path, "*ORDR*" + "*.rpt");

            return Files;
        }

        public string SelectUDFFMS(string table, string AliasID)
        {
            string result = "";

            List<string> parameters = new List<string>()
            {
                "139",
                AliasID
            };

            var list = Modal("GetUDF_FMS", parameters, "");
            if (list.Count > 0)
            {
                result = list[0];
            }
            return result;
        }

        public string GetSOqry(string AliasID)
        {
            var Hana = new SAPHanaAccess();
            var dataHelper = new DataHelper();

            var GetUDF_FMS = Hana.Get(SP.UDF_FMS);
            string SOUDF_FmsQry = dataHelper.ReadDataRow(GetUDF_FMS, 1, "", 0);
            string query = Hana.Get(string.Format(SOUDF_FmsQry, "139", AliasID)).Rows[0]["QString"].ToString();

            return query;
        }

    }
}

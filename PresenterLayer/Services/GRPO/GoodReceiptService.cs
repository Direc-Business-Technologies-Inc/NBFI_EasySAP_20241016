using DirecLayer;
using DomainLayer;
using DirecLayer._02_Form.MVP.Views;
using DirecLayer._03_Repository;
using DirecLayer._05_Repository;
using PresenterLayer.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirecLayer._02_Form.MVP.Presenters;
using MetroFramework;
using PresenterLayer.Services;
using PresenterLayer.Views;
using DomainLayer.Helper;
using PresenterLayer.Services.Security;

namespace PresenterLayer
{
    public class GoodReceiptService
    {
        private readonly IFrmGoodsReceiptPO _View;
        private readonly IGoodReceiptModel _repository;
        DataHelper helper { get; set; }
        SAPHanaAccess hana { get; set; }
        PurchasingAP_Style _style = new PurchasingAP_Style();
        PurchasingAP_Computation _computation = new PurchasingAP_Computation();
        ValidationRepository _validation = new ValidationRepository();
        UdfRepository _udfRepo = new UdfRepository();
        StringQueryRepository _reps = new StringQueryRepository();

        public GoodReceiptService(IFrmGoodsReceiptPO view, IGoodReceiptModel repository)
        {
            helper = new DataHelper();
            hana = new SAPHanaAccess();
            _View = view;
            _View.Presenter = this;
            _repository = repository;

            Onload();
        }

        internal void Onload()
        {
            DataTable udf = _repository.GetUDF();

            _udfRepo.Style(_View.Udf);
            _udfRepo.LoadUdf(_View.Udf, udf, "OPDN");

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

            int count = PurchasingModel.GRPOdocument.Count();
            int totalRow = id - count;

            PurchasingModel.GRPOdocument.ToList().ForEach(x =>
            {
                x.Index = id2;
                id2++;
            });

            //for (int i = 0; totalRow > i; i++)
            //{
            //    PurchasingModel.GRPOdocument.Add(new PurchasingModel.PurchaseOrder
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
            //PurchasingModel.GRPOdocument.Add(items);

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
                    PurchasingModel.GRPOdocument.ForEach(x =>
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
                    PurchasingModel.GRPOdocument.Find(x => x.Index == index).Department = m;
                }
            }
        }

        internal bool ChangeDocumentStatus()
        {
            bool isSuccess = false;

            if (_View.Status == "Cancel Document")
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to cancel the document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var docEntry = _View.DocEntry;

                    if (_repository.CancelDocument(docEntry))
                    {
                        ClearField(true);
                        isSuccess = true;
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Error in cancelling the document", true);
                    }
                }
            }

            return isSuccess;
        }

        internal void ChangeServiceType()
        {
            _View.Table.Columns.Clear();
            _View.Table.Rows.Clear();

            switch (_View.Service)
            {
                case "Service":
                    _style.PurchaseOrderItem("Service", true, _View.Table);
                    _style.PurchaseOrderItem("Service", true, _View.TablePreview);

                    break;

                default:
                    _style.PurchaseOrderItem("Item", true, _View.Table);
                    _style.PurchaseOrderItem("Item", true, _View.TablePreview);
                    break;
            }

            PurchasingModel.GRPOdocument.Clear();

            if (_View.Service == "Service")
            {
                //var id = PurchasingAP_generics.index;
                //if(_View.Table.Rows.Count > 0)
                //{

                //}
                //var items = new PurchasingModel.PurchaseOrder();
                //items.Index = id;
                //items.TaxCode = _View.VatGroup;
                //items.TaxRate = _View.VatGroupRate;
                //items.Quantity = 1;
                //PurchasingModel.GRPOdocument.Add(items);

                //LoadData(_View.Table);

                //PurchasingAP_generics.index++;
            }
        }

        internal void ChangeDocumentNumber()
        {
            string documentNo = _repository.SelectDocumentNo(_View.Series);

            if (documentNo != string.Empty)
            {
                _View.DocNum = documentNo;
            }
        }

        internal void DisplayItemList(string service)
        {
            if (_View.SuppCode != "")
            {
                FrmPurchasingItemList form = new FrmPurchasingItemList()
                {
                    IsCartonActive = false,
                    oBpCode = _View.SuppCode,
                    oBpName = _View.SuppName,
                    oTaxGroup = _View.VatGroup,
                    oWhsCode = _View.Warehouse
                };

                form.ShowDialog();

                LoadData(_View.Table);

                foreach (var x in PurchasingModel.GRPOdocument.OrderBy(x => x.Index).ToList())
                {
                    if (x.GrossPrice > 0)
                    {
                        CurrentRowComputation(null, x.Index);
                    }
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Warning: Empty Supplier or Department", true);
            }
        }

        public void LoadData(DataGridView dgv, bool isFirstLoad = false)
        {
            if (dgv.RowCount > 0)
            {
                dgv.Rows.Clear();
            }

            foreach (var x in PurchasingModel.GRPOdocument.ToList())
            {
                x.Department = _View.Department;

                if (_View.Service == "Item")
                {
                    object[] items = { x.ItemNo, x.BarCode, x.ItemDescription, x.StyleCode, x.Brand, "", x.Color, x.Size, x.Quantity, x.UoM, "",x.Warehouse,
                        "", x.OpenQty, x.ChainPricetag, x.ChainDescription , "", x.PricetagCount.ToString("0.00"), x.UnitPrice.ToString("0.00"), x.DiscountPerc, x.DiscAmount, x.TaxCode, "", x.TaxRate.ToString("0.00"), x.GrossPrice.ToString("0.00"),
                        x.TotalLC.ToString("0.00"), x.GrossTotalLC.ToString("0.00"), x.Project, "", x.Department, "", x.Remarks, x.Index, x.TaxAmount
                    };

                    dgv.Rows.Add(items);
                }
                else
                {
                    object[] Services = { x.ItemNo, x.BarCode, x.ItemDescription, x.StyleCode, x.Brand, "", x.Color, x.Size, x.Quantity, x.UoM, "", x.Warehouse, "",
                        x.GLAccount, x.GLAccountName, "", x.OpenQty, x.ChainPricetag, x.ChainDescription, "", x.PricetagCount.ToString("0.00"), x.UnitPricePerPCS.ToString("0.00"), x.GrossPricePerPCS.ToString("0.00"), x.UnitPrice.ToString("0.00"),
                        x.TaxCode, "", x.TaxRate.ToString("0.00"), x.GrossPrice.ToString("0.00"), x.TotalLC.ToString("0.00"), x.GrossTotalLC.ToString("0.00"), x.Project, "", x.Department, "", x.Remarks, x.Index, x.TaxAmount
                    };

                    dgv.Rows.Add(Services);
                }
            }

            TotalComputation();
        }

        internal void GetWarehouse(int colIndex, int index)
        {
            string warehouse = _repository.SelectWarehouse();

            if (warehouse != string.Empty)
            {
                UpdateCell(colIndex, warehouse);
                PurchasingModel.GRPOdocument.Find(x => x.Index == index).Warehouse = warehouse;
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

                PurchasingModel.GRPOdocument.Where(x => x.Index == index).ToList().ForEach(x =>
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

                PurchasingModel.GRPOdocument.Where(x => x.Index == index)
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

            table = table ?? "POR";

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
                _udfRepo.SelectUdfDratf(_View.Udf, docEntry, "ODRF");
            }
            else
            {
                header = _repository.SelectDocument(table, docEntry);

                if (ConvertToString(header.Rows[0]["DocType"]) == "I")
                {
                    Lines = _repository.SelectDocumentLines(table, docEntry);
                }
                else
                {
                    Lines = _repository.SelectServiceDocumentLines(table, docEntry);
                }

                _View.Status = status == "Open" ? "Open" : "Closed";
                _View.Status = ConvertToString(header.Rows[0]["CANCELED"]) == "N" ? _View.Status : "Canceled";
                _udfRepo.SelectUdfDratf(_View.Udf, docEntry, $"O{table}");
            }

            if (header.Rows.Count > 0)
            {
                foreach (DataRow head in header.Rows)
                {
                    _View.DocEntry = ConvertToString(head["DocEntry"]);
                    _View.DocNum = ConvertToString(head["DocNum"]);
                    _View.Series = ConvertToString(head["Series"]);
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
                    _View.Service = ConvertToString(head["DocType"]) == "I" ? "Item" : "Service";
                }

                if (Lines.Rows.Count > 0)
                {
                    foreach (DataRow line in Lines.Rows)
                    {
                        PurchasingModel.GoodsReceiptPO grpo = new PurchasingModel.GoodsReceiptPO();
                        Dictionary<string, string> DocumentBrand = _repository.GetDocumentBrand(ConvertToString(line["Item No."]));
                        Dictionary<string, string> DocumentGl = _repository.GetDocumentGl(ConvertToString(line["AcctCode"]));

                        double qty = ConvertToDouble(line["Quantity"]);
                        double discAmount = ConvertToDouble(line["DiscPrcnt"]);

                        grpo.Index = Convert.ToInt32(ConvertToString(line["LineNum"]));
                        grpo.ItemNo = ConvertToString(line["Item No."]);
                        grpo.ItemDescription = ConvertToString(line["Item Description"]);
                        grpo.BarCode = ConvertToString(line["CodeBars"]);
                        grpo.Color = ConvertToString(line["U_ID022"]);
                        grpo.Size = ConvertToString(line["U_ID007"]);
                        grpo.StyleCode = ConvertToString(line["U_ID012"]);
                        grpo.Style = ConvertToString(line["U_ID012"]);
                        grpo.UoM = ConvertToString(line["UOM"]);
                        grpo.BrandCode = DocumentBrand.Count > 0 ? DocumentBrand["Code"] : "";
                        grpo.Brand = DocumentBrand.Count > 0 ? DocumentBrand["Name"] : "";
                        grpo.PricetagCount = ConvertToDouble(line["Pricetag Count"]);
                        grpo.ChainDescription = ConvertToString(line["Chain Desc"]);
                        grpo.GLAccount = DocumentGl.Count > 0 ? DocumentGl["Code"] : "";
                        grpo.GLAccountName = DocumentGl.Count > 0 ? DocumentGl["Name"] : "";
                        grpo.Quantity = qty;
                        grpo.Warehouse = ConvertToString(line["WhsCode"]);
                        grpo.TaxCode = ConvertToString(line["VatGroup"]);
                        grpo.TaxAmount = ConvertToDouble(line["LineVat"]);
                        grpo.TaxRate = ConvertToDouble(line["VatPrcnt"]);
                        grpo.UnitPrice = ConvertToDouble(line["PriceBefDi"]);
                        grpo.TotalLC = ConvertToDouble(line["INMPrice"]);
                        grpo.TotalLCRaw = ConvertToDouble(line["INMPrice"]);
                        grpo.GrossTotalLC = ConvertToDouble(line["GTotal"]);
                        grpo.DiscountPerc = discAmount;
                        grpo.DiscAmount = discAmount * qty;
                        grpo.ChainPricetag = ConvertToString(line["U_Chain"]);
                        grpo.GrossPrice = ConvertToDouble(line["PriceAfVAT"]);
                        grpo.Remarks = ConvertToString(line["Remarks"]);
                        grpo.Project = ConvertToString(line["Project"]);

                        if (_View.Service == "Service")
                        {
                            grpo.UnitPricePerPCS = ConvertToDouble(line["U_UnitPricePerPiece"]);
                            grpo.GrossPricePerPCS = ConvertToDouble(line["U_GrossPricePerPiece"]);
                        }
                        //purchase.Section = dtItems.Rows[x]["U_ID018"].ToString();

                        PurchasingModel.GRPOdocument.Add(grpo);
                    }
                }

                LoadData(_View.Table, true);
            }
        }

        internal void ExecuteCopyDocument(string doc)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var suppcode = _View.SuppCode;
                //ClearField(true);

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
                    case "Purchase Order":

                        _View.DocEntry = _repository.CopyFrom(_View.SuppCode, "CopyFromPOToGrPO");

                        if (_View.DocEntry != string.Empty || _View.DocEntry.Length > 0)
                        {
                            GetSelectedDocument("POR", _View.DocEntry, "O");
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
                    int index = Convert.ToInt32(currentTable.Rows[i].Cells["Index"].Value);
                    PurchasingModel.GRPOdocument.RemoveAll(x => x.Index == index);
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
            var row = _View.Udf.CurrentRow.Index;
            var col = _View.Udf.CurrentCell.ColumnIndex;
            string result = "";

            if (fieldName == "U_CheckBy" || fieldName == "U_AppBy" || fieldName == "U_NotedBy")
            {
                result = _repository.GetEmployees();
            }
            else if (fieldName == "U_RevBy")
            {
                result = _repository.GetAllEmployees();
            }
            else if (fieldName == "U_CartonList")
            {
                if (_View.Udf.CurrentRow.Cells[2].Value == null || _View.Udf.CurrentRow.Cells[2].Value == DBNull.Value || String.IsNullOrWhiteSpace(_View.Udf.CurrentRow.Cells[2].Value.ToString()))
                {
                    result = _repository.GetCartonList();
                }
                else
                {
                    result = _View.Udf.CurrentRow.Cells[2].Value.ToString();

                    var model = new CartonListModel();
                    var view = new FrmCartonList(result);
                    var presenter = new CartonListPresenter(view, model);

                    view.Show();
                }
            }
            else if (fieldName == "U_Designer")
            {
                result = _repository.GetDesigner();
            }
            else if (fieldName == "U_MerchCoord")
            {
                result = _repository.GetMerch();
            }
            else if (fieldName == "U_BudgetCode")
            {
                result = _repository.GetBudgetCode();
            }
            else if (fieldName == "U_OrderNo")
            {
                result = _repository.GetOrderNo(_View.SuppCode);
            }
            else if (fieldName.Contains("Date") || fieldName.Contains("date"))
            {
                _udfRepo.ConvertToDate(_View.Udf);
                ConvertToDate(_View.Udf);
            }

            if (result != string.Empty)
            {
                _View.Udf.CurrentRow.Cells[2].Value = result;
            }

            try
            {
                _View.Udf.CurrentCell = _View.Udf[col - 1, row];
                _View.Udf.CurrentCell = _View.Udf[col, row];
            }
            catch
            {

            }
            
        }

        public void ConvertToDate(DataGridView dgv)
        {
            var row = dgv.CurrentRow;
            string fieldName = row.Cells[0].Value.ToString();

            if (fieldName.Contains("Date") || fieldName.Contains("date"))
            {
                DateTimePicker oDateTimePicker = new DateTimePicker();

                var date = DateTime.Now.Date.ToString("MM/dd/yyyy");

                if (dgv.CurrentCell.Value != null)
                {
                    date = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
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
        }

        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            var dtPicker = (DateTimePicker)sender;
            var qe = dtPicker.Value.ToShortDateString();

            _View.Udf.CurrentCell.Value = qe;
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

                        PurchasingModel.GRPOdocument.Where(a => a.Index == index)
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
                    //PurchasingModel.GRPOdocument.Add(items);

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

                PurchasingModel.GRPOdocument.Where(a => a.Index == index)
                    .ToList().ForEach(a =>
                    {
                        a.GLAccount = gl["Code"];
                        a.GLAccountName = gl["Name"];
                    });
                if (itHasValue == false)
                {
                    var x = PurchasingModel.GRPOdocument.SingleOrDefault(s => s.Index == 0);

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
                    //PurchasingModel.GRPOdocument.Add(items);

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
                PurchasingModel.GRPOdocument.Find(x => x.Index == index).Project = project;
            }
        }

        internal void GetUom(int colIndex, int index)
        {
            List<string> UomValue = _repository.SelectUom();

            if (UomValue != null)
            {
                UpdateCell(colIndex, UomValue[1]);

                PurchasingModel.GRPOdocument.Find(x => x.Index == index).UoM = UomValue[1];
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

            double index = ConvertToDouble(currentRow.Cells["Index"].Value);

            var item = PurchasingModel.GRPOdocument.Find(x => x.Index == index);

            if (item != null)
            {
                double taxRate = item.TaxRate != 0D ? _computation.PercentConverter(item.TaxRate) + 1 : 1D;
                double discountPerc = _computation.PercentConverter(_validation.ValidateCellsDouble(currentRow, "Discount %").ToString());

                item.Remarks = _validation.ValidateCells(currentRow, "Remarks");
                item.Quantity = _validation.ValidateCellsDouble(currentRow, "Quantity");
                item.DiscountPerc = _validation.ValidateCellsDouble(currentRow, "Discount %");

                var TaxCode = _validation.ValidateCells(currentRow, "Tax Code");

                if (_View.Service == "Service" && (currentColumn.Equals("Unit Price per piece") || currentColumn.Equals("Quantity")))
                {
                    item.UnitPricePerPCS = ValidateInput.Double(currentRow.Cells["Unit Price per piece"].Value);
                    currentRow.Cells["Gross Price per piece"].Value = item.UnitPricePerPCS * taxRate;

                    item.GrossPricePerPCS = item.UnitPricePerPCS * taxRate;

                    item.UnitPrice = _computation.MultipleByQty(item.UnitPricePerPCS, item.Quantity);
                    item.GrossPrice = _computation.ComputeGrossPrice(item.UnitPrice, taxRate, TaxCode);
                    item.UnitPriceRaw = _computation.ComputeUnitPriceRaw(item.GrossPrice, taxRate, TaxCode);
                }
                else if (_View.Service == "Service" && (currentColumn.Equals("Gross Price per piece") || currentColumn.Equals("Quantity")))
                {
                    currentRow.Cells["Unit Price per piece"].Value = Math.Round((ValidateInput.Double(currentRow.Cells["Gross Price per piece"].Value) / taxRate), 3).ToString("0.00");
                    item.UnitPricePerPCS = Convert.ToDouble(Math.Round((ValidateInput.Double(currentRow.Cells["Gross Price per piece"].Value) / taxRate), 3).ToString("0.00"));

                    item.GrossPricePerPCS = ValidateInput.Double(currentRow.Cells["Gross Price per piece"].Value);
                    item.GrossPrice = _computation.MultipleByQty(item.GrossPricePerPCS, item.Quantity);

                    item.UnitPrice = _computation.ComputeUnitPrice(item.GrossPrice, taxRate, TaxCode);
                    item.UnitPriceRaw = _computation.ComputeUnitPriceRaw(item.GrossPrice, taxRate, TaxCode);
                }
                else if (currentColumn.Equals("Gross Price"))
                {
                    var GrossPrice = _validation.ValidateCellsDouble(currentRow, "Gross Price");
                    item.UnitPrice = _computation.ComputeUnitPrice(GrossPrice, taxRate, TaxCode);
                    item.GrossPrice = _computation.ComputeGrossPrice(item.UnitPrice, taxRate, TaxCode);
                    item.UnitPriceRaw = _computation.ComputeUnitPriceRaw(item.GrossPrice, taxRate, TaxCode);
                }
                else if (currentColumn.Equals("Unit Price"))
                {
                    var UnitPrice = _validation.ValidateCellsDouble(currentRow, "Unit Price");
                    UnitPrice = ConvertToDouble(UnitPrice);
                    item.UnitPrice = UnitPrice;
                    item.UnitPriceRaw = UnitPrice;
                    item.GrossPrice = _computation.ComputeGrossPrice(UnitPrice, taxRate, TaxCode);
                }
                else
                {
                    var UnitPrice = _validation.ValidateCellsDouble(currentRow, "Unit Price");
                    UnitPrice = ConvertToDouble(UnitPrice);
                    item.UnitPrice = UnitPrice;
                    item.GrossPrice = _computation.ComputeGrossPrice(UnitPrice, taxRate, TaxCode);
                    item.UnitPriceRaw = _computation.ComputeUnitPriceRaw(item.GrossPrice, taxRate, TaxCode);
                }

                // discount function
                item.DiscAmount = _computation.ComputeDiscountAmount(discountPerc, Convert.ToDouble(item.UnitPriceRaw), item.Quantity);

                var computeDiscount = item.GrossPrice - (item.GrossPrice * discountPerc);

                item.GrossPrice = Convert.ToDouble(Math.Round(computeDiscount, 2).ToString("0.00"));

                item.GrossPriceRaw = computeDiscount;

                var qty = _View.Service == "Service" ? 1 : item.Quantity;

                item.TotalLC = Convert.ToDouble(Math.Round(_computation.ComputeRowTotals(qty, Convert.ToDouble(item.UnitPrice), item.DiscAmount), 2).ToString("0.00"));

                item.TotalLCRaw = _computation.ComputeRowTotals(qty, Convert.ToDouble(item.UnitPriceRaw), item.DiscAmount);

                item.TaxAmount = _computation.ComputeTax(item.TotalLCRaw, taxRate, item.DiscAmount);

                item.GrossTotalLC = Convert.ToDouble(Math.Round((qty * item.GrossPriceRaw), 2).ToString("0.00"));

                if (_View.Service == "Service")
                {
                    item.ItemNo = ValidateInput.String(currentRow.Cells[0].Value);
                    item.BarCode = ValidateInput.String(currentRow.Cells[1].Value);
                    item.ItemDescription = ValidateInput.String(currentRow.Cells[2].Value);
                }

                if (item.ChainPricetag != "" && item.ChainPricetag != null)
                {
                    item.PricetagCount = item.Quantity;
                    currentRow.Cells["Pricetag Count"].Value = item.Quantity;
                }

                currentRow.Cells["Unit Price"].Value = item.UnitPrice.ToString("0.00");
                currentRow.Cells["Gross Price"].Value = item.GrossPrice.ToString("0.00");
                currentRow.Cells["Total(LC)"].Value = item.TotalLC.ToString("0.00");
                currentRow.Cells["Gross Total (LC)"].Value = item.GrossTotalLC.ToString("0.00");

                if (_View.Service == "Item")
                {
                    try
                    {
                        currentRow.Cells["Disc Amount"].Value = item.DiscAmount;
                    }
                    catch { }
                }

                TotalComputation();
            }
        }

        private void TotalComputation()
        {
            double taxAmount = 0;
            double totalBefDiscount = 0;

            foreach (var x in PurchasingModel.GRPOdocument.ToList())
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

            //            DECLARE.oPurchase.Lines.CostingCode2 = PurchasingModel.GRPOdocument.Find(f => f.Index == index).BrandCode;
            //            DECLARE.oPurchase.Lines.UserFields.Fields.Item("U_BrandName").Value = PurchasingModel.GRPOdocument.Find(f => f.Index == index).Brand;

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
            result = helper.ReadDataRow(hana.Get($"SELECT UomEntry FROM OUOM WHERE UomName = '{sUomCode}'"), 0, "", 0);
            return string.IsNullOrEmpty(result) ? "0" : result;
        }


        public bool ExecuteRequest(string request)
        {
            bool isPosted = false;
            string msg = string.Empty;
            int UDfCount = _View.Udf.RowCount;
            int rowCount = _View.Table.RowCount;

            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
               {"CardCode", _View.SuppCode},
                {"CardName", _View.SuppName},
                {"CntctCode", _View.ContactPerson},
                {"NumAtCard", _View.RefNo},
                {"U_CompanyTIN", _View.Company },
                {"U_Department", _View.Department},
                {"DocCurrency", _View.BpCurrency},
                {"DocRate", _View.BpRate == string.Empty ? "1" : _View.BpRate},
                {"DocDate", ValidateInput.ConvertToDate(_View.PostingDate, "yyyyMMdd") },
                {"TaxDate", ValidateInput.ConvertToDate(_View.DocumentDate, "yyyyMMdd") },
                {"DocDueDate",ValidateInput.ConvertToDate(_View.DeliveryDate, "yyyyMMdd")},
                {"CancelDate", ValidateInput.ConvertToDate(_View.CancellationDate, "yyyyMMdd")},
                {"Comments", _View.Remark},
                {"U_PrepBy", DomainLayer.Models.EasySAPCredentialsModel.ESUserId},
                {"DocType", _View.Service == "Item" ? "dDocument_Items" : "dDocument_Service" },
                {"DocObjectCode" , "22"},
                {"DocumentsOwner",  _repository.GetEmpID() }
            };

            for (int i = 0; UDfCount > i; i++)
            {
                if (!dict.Keys.Contains(_View.Udf.Rows[i].Cells[0].Value))
                {
                    if (ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value).Contains("Date") || ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value).Contains("date"))
                    {
                        if (_View.Udf.Rows[i].Cells[2].Value != null)
                        {
                            var date = DateTime.TryParse(ValidateInput.String(_View.Udf.Rows[i].Cells[2].Value), out DateTime dateonly); //Convert.ToDateTime(ValidateInput.String(_View.Udf.Rows[i].Cells[2].Value)).ToString("yyyyMMdd");

                            dict.Add(ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value), date ? Convert.ToDateTime(ValidateInput.String(_View.Udf.Rows[i].Cells[2].Value)).ToString("yyyyMMdd") : dateonly.ToString("yyyyMMdd"));
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
            dict.Add("U_PostRem", $"{(request.Equals("Add") ? "Created" : "Updated")} by EasySAP | Good Receipt PO : {sboCred.UserId} : {DateTime.Now} : | Powered By : DIREC");

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

                if (isOkay && rowCount != (index + 1))
                {
                    var items = new Dictionary<string, object>();

                    if (_View.Service == "Item")
                    {
                        if (string.IsNullOrEmpty(_View.DocEntry))
                        {
                            items.Add("ItemCode", ValidateInput.String(_View.Table.Rows[index].Cells["Item No."].Value));
                        }

                        items.Add("DiscountPercent", ValidateInput.String(_View.Table.Rows[index].Cells["Discount %"].Value));
                        var uom = _repository.GetUomEntry(ValidateInput.String(_View.Table.Rows[index].Cells["UoM"].Value));
                        items.Add("UoMEntry", uom);
                        items.Add("WarehouseCode", ValidateInput.String(_View.Table.Rows[index].Cells["Warehouse"].Value));
                    }
                    else
                    {
                        items.Add("ItemDescription", ValidateInput.String(_View.Table.Rows[index].Cells["Item No."].Value));
                        items.Add("AccountCode", ValidateInput.String(_View.Table.Rows[index].Cells["G/L Account"].Value));
                        items.Add("U_OldItemNo", ValidateInput.String(_View.Table.Rows[index].Cells["Old Item No."].Value));
                        items.Add("U_Description", ValidateInput.String(_View.Table.Rows[index].Cells["Item Description"].Value));
                        items.Add("U_Qty", ValidateInput.String(_View.Table.Rows[index].Cells["Quantity"].Value));
                        items.Add("U_UOM", ValidateInput.String(_View.Table.Rows[index].Cells["UoM"].Value));
                        items.Add("U_TargetWhs", ValidateInput.String(_View.Table.Rows[index].Cells["Warehouse"].Value));
                        items.Add("U_UnitPricePerPiece", ValidateInput.Double(_View.Table.Rows[index].Cells["Unit Price per piece"].Value));
                        items.Add("U_GrossPricePerPiece", ValidateInput.Double(_View.Table.Rows[index].Cells["Gross Price per piece"].Value));
                    }

                    int rowIndex = Convert.ToInt32(_View.Table.Rows[index].Cells["Index"].Value);
                    var grossprice = ValidateInput.String(_View.Table.Rows[index].Cells["Gross Price"].Value).Replace(",", "");

                    if (string.IsNullOrEmpty(_View.DocEntry) == false)
                    {
                        items.Add("BaseType", 22);
                        items.Add("BaseEntry", _View.DocEntry);
                        items.Add("BaseLine", ValidateInput.String(_View.Table.Rows[index].Cells["Index"].Value));
                    }

                    items.Add("Quantity", ValidateInput.String(_View.Table.Rows[index].Cells["Quantity"].Value));
                    items.Add("PriceAfterVAT", grossprice);
                    items.Add("U_Style", ValidateInput.String(_View.Table.Rows[index].Cells["Style"].Value));
                    items.Add("U_Color", ValidateInput.String(_View.Table.Rows[index].Cells["Color"].Value));
                    items.Add("U_Size", ValidateInput.String(_View.Table.Rows[index].Cells["Size"].Value));
                    items.Add("U_Chain", ValidateInput.String(_View.Table.Rows[index].Cells["Chain Pricetag"].Value));
                    items.Add("U_ChainDescription", ValidateInput.String(_View.Table.Rows[index].Cells["Chain Description"].Value));
                    items.Add("U_PricetagCount", ValidateInput.String(_View.Table.Rows[index].Cells["Pricetag Count"].Value));
                    items.Add("U_Remarks", ValidateInput.String(_View.Table.Rows[index].Cells["Remarks"].Value));
                    items.Add("CostingCode2", ValidateInput.String(PurchasingModel.GRPOdocument.Find(f => f.Index == rowIndex).BrandCode));
                    items.Add("U_BrandName", ValidateInput.String(PurchasingModel.GRPOdocument.Find(f => f.Index == rowIndex).Brand));
                    items.Add("COGSCostingCode", ValidateInput.String(_View.Table.Rows[index].Cells["Department"].Value));
                    items.Add("VatGroup", ValidateInput.String(_View.Table.Rows[index].Cells["Tax Code"].Value));
                    items.Add("CostingCode", ValidateInput.String(_View.Table.Rows[index].Cells["Department"].Value));
                    items.Add("ProjectCode", ValidateInput.String(_View.Table.Rows[index].Cells["Project"].Value));

                    dictLines.Add(items);
                }
            }

            StringBuilder json = new StringBuilder();

            bool isUserGoApproval = false;

            var userApprovalCount = DataRepository.GetData(_reps.UserApprovalCheck());

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

            json = DataRepository.JsonBuilder(dict, dictLines, "DocumentLines");

            var jsonHeaderOnly = DataRepository.JsonHeaderBuilder(dict, dictLines, "DocumentLines");

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
            _View.RefNo = string.Empty;
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

            ChangeDocumentNumber();

            PurchasingAP_generics.index = 0;

            foreach (DataGridViewRow udfRow in _View.Udf.Rows)
            {
                try
                {
                    udfRow.Cells[2].Value = null;
                }
                catch { }
                _View.Udf.Rows[4].Cells["Field"].Value = DomainLayer.Models.EasySAPCredentialsModel.ESUserId;
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
            _View.Udf.Controls.Cast<Control>().Where(x => x.GetType() == typeof(DateTimePicker)).ToList().ForEach(x =>
            {

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
                PurchasingModel.GRPOdocument.Clear();
                _View.Table.Rows.Clear();
                _View.TablePreview.Rows.Clear();
            }
        }

        public FileInfo[] GetDocumentCrystalForms()
        {
            var sys = new SystemSettings();
            //string path = $"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\PO";
            var _settingsService = new SettingsService();

            string path = sys.PathExist($"{_settingsService.GetReportPath()}Purchasing\\");

            FileInfo[] Files = sys.FileList(path, "*OPDN*" + "*.rpt");

            return Files;
        }
    }
}

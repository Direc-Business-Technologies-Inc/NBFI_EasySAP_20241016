using DirecLayer._03_Repository;
using DirecLayer._05_Repository;
using PresenterLayer.Services.UnofficialSales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PresenterLayer.Helper;
using PresenterLayer.Helper.Unofficial_Sales;
using PresenterLayer.Views;
using DirecLayer;
using MetroFramework;
using DomainLayer.Helper;
using System.IO;
using PresenterLayer.Services.Security;

namespace PresenterLayer.Services
{
    public class UnofficialSalesService
    {
        private readonly IFrmUnofficialSales _View;
        private readonly IUnofficialSalesModel _repository;
        PurchasingAP_Style _style = new PurchasingAP_Style();
        PurchasingAP_Computation _computation = new PurchasingAP_Computation();
        ValidationRepository _validation = new ValidationRepository();
        UdfRepository _udfRepo = new UdfRepository();
        StringQueryRepository _reps = new StringQueryRepository();
        SAPHanaAccess Hana = new SAPHanaAccess();
        DateTimePicker oDateTimePicker = new DateTimePicker();
        public FrmUnofficialSales frmUOS { get; set; }

        //private FrmUnofficialSales view;
        //private UnofficialSalesModel model;
        DataGridView Dgv { get; set; }
        public UnofficialSalesService(IFrmUnofficialSales view, IUnofficialSalesModel repository)
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
            _udfRepo.LoadUdf(_View.Udf, udf, "ODLN");

            _View.Status = "Open";

        }

        internal void UdfRequest()
        {
            string fieldName = _View.Udf.CurrentRow.Cells[0].Value.ToString();

            string result = "";

            if (fieldName == "Requested By" || fieldName == "Approved By" || fieldName == "Reviewed By" || fieldName == "Received By" || fieldName == "Noted By")
            {
                result = _repository.GetEmployees();
            }
            else if (fieldName == "U_DSRDate")
            {
                _udfRepo.ConvertToDate(_View.Udf);
                ConvertToDate(_View.Udf);
            }
            //else if (fieldName == "U_RAS")
            //{
            //    //result = _repository.GetAllEmployees();
            //    result = GetSOqry(fieldName).Contains("$") ? _repository.GetAllEmployees() : SelectUDFFMS("ODLN", fieldName);
            //}
            else if (fieldName == "U_PromoQuantity")
            {
                //_udfRepo.GetDiff(_View.Udf, _View, "U_PromoQuantity", "Quantity Difference");
                GetDiff(_View.Udf, _View, "U_PromoQuantity", "Quantity Difference");

            }
            else if (fieldName == "U_PromoTotal")
            {
                //_udfRepo.GetDiff(_View.Udf, _View, "U_PromoTotal", "Total Difference");
                GetDiff(_View.Udf, _View, "U_PromoTotal", "Total Difference");
            }

            if (result != string.Empty)
            {
                _View.Udf.CurrentRow.Cells[2].Value = result;
            }
        }

        void GetDiff(DataGridView dgv, IFrmUnofficialSales _View, string GetFieldName, string GetChangeValueFName)
        {
            var row = dgv.CurrentRow;
            string fieldName = row.Cells[0].Value.ToString();
            decimal PromoQty = 0;
            decimal QtyDiff = 0;
            bool CheckIfNoChar = true;

            if (fieldName == GetFieldName && dgv[2, row.Index].Value != null)
            {
                char[] StrToChar = dgv[2, row.Index].Value.ToString().ToCharArray();

                foreach (char a in StrToChar)
                {
                    if (!char.IsControl(a) && !char.IsDigit(a) && (a != '.'))
                    {
                        CheckIfNoChar = false;
                    }
                }


                //asdladasd
                //if (dgv[2, row.Index].Value.ToString().Any(char.IsDigit) && )
                if (CheckIfNoChar)
                {
                    PromoQty = Convert.ToDecimal(dgv[2, row.Index].Value.ToString());
                    decimal TotQty = GetFieldName == "U_PromoQuantity" ? Convert.ToDecimal(_View.TotalQuantity != "" ? _View.TotalQuantity : "0") : Convert.ToDecimal(_View.Total != "" ? _View.Total : "0");
                    QtyDiff = TotQty - PromoQty;

                    foreach (DataGridViewRow row2 in dgv.Rows)
                    {
                        if (row2.Cells[1].Value.ToString().Equals(GetChangeValueFName))
                        {
                            dgv[2, row2.Index].Value = QtyDiff;
                        }
                    }
                }
                else
                {
                    dgv[2, row.Index].Value = "0";
                    StaticHelper._MainForm.ShowMessage("Input format is number only.", true);
                }
            }
        }


        private void UDFComputeTotals()
        {

            foreach (DataGridViewRow row in _View.Udf.Rows)
            {
                string UDFName = row.Cells[1].Value.ToString();
                if (row.Cells[2].Value != null && UnofficialSalesItemsController.oTotalPrice != 0 && UDFName == "Promo Total")
                {
                    //Total Difference = Total - Promo Total
                    var dPromoTotal = UnofficialSalesItemsController.oTotalPrice - Convert.ToDouble((row.Cells[2].Value == null ? 0D : (string.IsNullOrEmpty(row.Cells[2].Value.ToString()) ? 0D : row.Cells[2].Value)).ToString());
                    var dPromoTotalConvert = dPromoTotal.ToString("0.00");
                    UnofficialSalesItemsController.oTotalDiff = Convert.ToDouble(dPromoTotalConvert);
                }
                else if (row.Cells[2].Value != null && UnofficialSalesItemsController.oTotalQty != 0 && UDFName == "Promo Quantity")
                {
                    //Quantity Difference = Quantity - Promo Quantity
                    double dPromoTotal = UnofficialSalesItemsController.oTotalQty - Convert.ToDouble((row.Cells[2].Value == null ? 0D : (string.IsNullOrEmpty(row.Cells[2].Value.ToString()) ? 0D : row.Cells[2].Value)).ToString());
                    string dPromoTotalConvert = dPromoTotal.ToString("0.00");
                    UnofficialSalesItemsController.oQtyDiff = Convert.ToDouble(dPromoTotalConvert);
                }
            }

        }

        public void ConvertToDate(DataGridView dgv)
        {
            var row = dgv.CurrentRow;
            string date2 = "";
            DateTime dt;
                
            if (row.Cells[0].Value != null)
            {
                string fieldName = row.Cells[0].Value.ToString();

                if ((fieldName.Contains("Date") || fieldName.Contains("date")) && (DateTime.TryParse(dgv.CurrentCell.Value.ToString(), out dt)))
                {
                    DateTimePicker oDateTimePicker = new DateTimePicker();
                    var date = dgv.CurrentCell.Value.ToString();

                    dgv[2, row.Index].Value = date2;

                    dt = DateTime.Parse(date);
                    string s2 = dt.ToString("MM/dd/yyyy");
                    DateTime dtnew = DateTime.Parse(s2);


                    oDateTimePicker.Value = dtnew;
                    dgv.Controls.Add(oDateTimePicker);
                    CreateDateTimePicker(oDateTimePicker, dgv, row);
                }
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
        }

        public void LoadData(DataGridView dgv, bool isFirstLoad = false)
        {
            try
            {
                if (dgv.RowCount > 0)
                {
                    dgv.Rows.Clear();
                }
                UnofficialSalesItemsController.dgvBarcodeItemsLayout(dgv);
                foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.ToList())
                {
                    //object[] a = { x.ItemCode
                    //        , x.ItemName 
                    //        , x.Brand
                    //        , x.Style
                    //        , x.Color
                    //        , x.Size
                    //        , x.Section
                    //        , x.BarCode
                    //        , x.EffectivePrice
                    //        , x.GrossPrice.ToString("0.##")
                    //        , x.UnitPrice.ToString("0.##")
                    //        , x.Quantity
                    //        , x.DiscountPerc.ToString("0.##")
                    //        , x.DiscountAmount
                    //        , x.EmpDiscountPerc.ToString("0.##") 
                    //        , x.FWhsCode
                    //        , "..."
                    //        , x.TaxCode
                    //        , "..."
                    //        , x.TaxRate.ToString("0.##")
                    //        , x.LineTotalManual.ToString("0.##")
                    //        , x.GrossTotal.ToString("0.##")
                    //        , x.PriceAfterDisc.ToString("0.##")
                    //        , x.Linenum };

                    object[] a = { x.ItemCode
                            , x.ItemName
                            , x.GrossPrice.ToString("0.##")
                            , x.UnitPrice.ToString("0.##")
                            , x.Quantity
                            , x.DiscountAmount
                            , x.DiscountPerc.ToString("0.##")
                            , ""
                            , x.LineTotal.ToString("0.##")
                            , x.Linenum
                            , ""
                            , ""
                            , x.PriceBefDisc.ToString("0.##")
                            , x.Style
                            , x.Color
                            , x.Size
                            , x.Section
                            , x.SortCode
                            , x.ItemProperty
                            , x.DelDate
                            , "..."
                            , x.Company
                            , "..."
                             };
                    dgv.Rows.Add(a);
                }
                UnofficialSalesItemsController.dataGridLayout(dgv);
                
                //TotalComputation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        internal void GetExistingDocument(DataGridView dgv)
        {
            dgv.DataSource = null;
            dgv.DataSource = ListDocuments();
            dgv.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["Total Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["Total"].ValueType = typeof(Int32);
            dgv.Columns["Total"].DefaultCellStyle.Format = "N2";
            dgv.Columns["Total Quantity"].ValueType = typeof(Int32);
            dgv.Columns["Total Quantity"].DefaultCellStyle.Format = "##,0";
        }

        internal DataTable ListDocuments()
        {
            DataTable dt = _repository.ExistingDocument();

            return dt;
        }


        private void TotalComputation()
        {
            double taxAmount = 0;
            double totalBefDiscount = 0;
			double totalQuantity = 0;

			foreach (var x in UnofficialSalesItemsModel.UnofficialSalesItems.ToList())
            {
                taxAmount += x.TaxAmount;
				totalBefDiscount += x.TotalLCRaw;
				totalQuantity += x.Quantity;
            }

			_View.TotalQuantity = totalQuantity.ToString();
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
            Application.DoEvents();
        }

        public bool ExecuteRequest(string request)
        {
            try
            {
                bool isPosted = false;
                string msg = string.Empty;
                int UDfCount = _View.Udf.RowCount;
                int rowCount = _View.Table.RowCount;

                var dict = new Dictionary<string, string>();

                if (request.Contains("Add"))
                {
                    dict.Add("DocDate", ValidateInput.ConvertToDate(_View.PostingDate, "yyyy-MM-dd"));
                    dict.Add("TaxDate", ValidateInput.ConvertToDate(_View.DocumentDate, "yyyy-MM-dd"));
                    dict.Add("Series", "6");
                    dict.Add("CardCode", _View.SuppCode);
                    dict.Add("CardName", _View.SuppName);
                    dict.Add("Comments", _View.Remark);
                    dict.Add("U_PrepBy", DomainLayer.Models.EasySAPCredentialsModel.ESUserId);
                    dict.Add("U_DocType", _View.Service);
                    dict.Add("Address2", _View.Address);
                    //dict.Add("DocCurrency", _View.BpCurrency);
                    dict.Add("DocCurrency", "PHP");
                    dict.Add("DocObjectCode", "15");
                    dict.Add("DocumentsOwner", _repository.GetEmpID());
                    dict.Add("Warehouse", _View.Warehouse);
                    dict.Add("DiscountPercent", _View.DiscountPerc);
                    dict.Add("U_WarehouseSalesType", _View.WhsSls.SelectedValue.ToString());
                }
                else
                {
                    dict.Add("Comments", _View.Remark);
                }

                for (int i = 0; UDfCount > i; i++)
                {
                    if (!dict.Keys.Contains(_View.Udf.Rows[i].Cells[0].Value))
                    {
                        if (ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value).Contains("Date") || ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value).Contains("date"))
                        {
                            if (_View.Udf.Rows[i].Cells[2].Value != null)
                            {
                                if (_View.Udf.Rows[i].Cells[2].Value.ToString() != "")
                                {
                                    var date = Convert.ToDateTime(ValidateInput.String(_View.Udf.Rows[i].Cells[2].Value)).ToString("yyyy-MM-dd");

                                    dict.Add(ValidateInput.String(_View.Udf.Rows[i].Cells[0].Value), date);
                                }
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
                dict.Add("U_PostRem", $"{(request.Equals("Add") ? "Created" : "Updated")} by EasySAP | Unofficial Sales : {sboCred.UserId} : {DateTime.Now} : | Powered By : DIREC");


                List<Dictionary<string, object>> dictLines = new List<Dictionary<string, object>>();
                if (request.Contains("Add"))
                {
                    
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
                            //Item Lines
                            var items = new Dictionary<string, object>();
                            var helper = new DataHelper();
                            var sapHana = new SAPHanaAccess();

                            string sItemCode = ValidateInput.String(_View.Table.Rows[index].Cells["Item No."].Value);
                            double discperc = Convert.ToDouble(ValidateInput.String(_View.Table.Rows[index].Cells["Discount %"].Value));
                            double priveafvat = Convert.ToDouble(ValidateInput.String(_View.Table.Rows[index].Cells["Unit Price"].Value));

                            DataTable dtGetBrand = sapHana.Get("SELECT U_ID019 FROM OITM Where ItemCode = '" + sItemCode + "'");
                            string brand = helper.ReadDataRow(dtGetBrand, 0, "", 0);

                            items.Add("ItemCode", sItemCode);
                            items.Add("Quantity", ValidateInput.String(_View.Table.Rows[index].Cells["Quantity"].Value));
                            items.Add("DiscountPercent", ValidateInput.String(discperc));
                            items.Add("WarehouseCode", ValidateInput.String(_View.Warehouse));
                            items.Add("UnitPrice", ValidateInput.String(priveafvat));
                            items.Add("U_SortCode", ValidateInput.String(_View.Table.Rows[index].Cells["SortCode"].Value));
                            items.Add("CostingCode2", ValidateInput.String(brand));
                            items.Add("COGSCostingCode2", ValidateInput.String(brand));
                            items.Add("ProjectCode", ValidateInput.String(_View.sOProject));
                            items.Add("ShipDate", ValidateInput.String(Convert.ToDateTime(_View.Table.Rows[index].Cells["DeliveryDate"].Value).ToString("yyyy-MM-dd")));
                            items.Add("U_Company", ValidateInput.String(_View.Table.Rows[index].Cells["Company"].Value));      
                            dictLines.Add(items);
                        }
                    }
                }
                

                StringBuilder json = new StringBuilder();

                bool isUserGoApproval = false;

                var userApprovalCount = DataRepositoryUnofficialSales.GetData(_reps.UserApprovalCheck());

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

                json = DataRepositoryUnofficialSales.JsonBuilder(dict, dictLines, "DocumentLines");

                var jsonHeaderOnly = DataRepositoryUnofficialSales.JsonHeaderBuilder(dict, dictLines, "DocumentLines");


                isPosted = _repository.ActivateService((message) => msg = message, request, _View.DocEntry, json, jsonHeaderOnly, isUserGoApproval);

                //if (isPosted)
                //{
                //    ClearField(true);
                //}
                //else
                //{
                //    StaticHelper._MainForm.ShowMessage(msg, true);
                //}

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


        public void ClearField(bool clearItems)
        {
            _View.TotalQuantity = string.Empty;
            _View.CustomRef = string.Empty;
            _View.DocNo = string.Empty;
            _View.SuppCode = string.Empty;
            _View.SuppName = string.Empty;
            _View.ContactPerson = string.Empty;
            _View.Company = string.Empty;
            _View.Department = string.Empty;
            _View.BpCurrency = string.Empty;
            _View.BpRate = string.Empty;
            _View.Service = string.Empty;
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


        internal void GetSelectedDocument(string table, string docEntry, string status)
        {
            DataTable header = new DataTable();
            DataTable Lines = new DataTable();

            table = table ?? "DLN";

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

                //Replaced by Cedi due to conflict on 061119
                //if (ConvertToString(header.Rows[0]["U_DocType"]) == "I")
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
                _udfRepo.SelectUdfDratfSO(_View.Udf, docEntry, $"O{table}");
             
            }

            if (header.Rows.Count > 0)
            {
                foreach (DataRow head in header.Rows)
                {
                    _View.DocEntry = ConvertToString(head["DocEntry"]);
                    _View.DocNum = ConvertToString(head["DocNum"]);
                    _View.DocNo = ConvertToString(head["DocNum"]);
                    _View.Series = ConvertToString(head["Series"]);
                    _View.SuppCode = ConvertToString(head["CardCode"]);
                    _View.SuppName = ConvertToString(head["CardName"]);
                    _View.ContactPerson = ConvertToString(head["NumAtCard"]);
                   // _View.Company = ConvertToString(head["U_CompanyTIN"]);
                    _View.Department = ConvertToString(head["U_Department"]);
                    //_View.BpCurrency = ConvertToString(head["DocCur"]);
                    _View.BpRate = ConvertToString(head["DocRate"]);
                    _View.PostingDate = ConvertToString(head["DocDate"]) != "" ? Convert.ToDateTime(head["DocDate"]).ToShortDateString() : "";
                    _View.DocumentDate = ConvertToString(head["TaxDate"]) != "" ? Convert.ToDateTime(head["TaxDate"]).ToShortDateString() : "";
                    //_View.DeliveryDate = ConvertToString(head["DocDueDate"]) != "" ? Convert.ToDateTime(head["DocDueDate"]).ToShortDateString() : "";
                   // _View.CancellationDate = ConvertToString(head["CancelDate"]) != "" ? Convert.ToDateTime(head["CancelDate"]).ToShortDateString() : "";
                    _View.Remark = ConvertToString(head["Comments"]);
                    _View.Service = ConvertToString(head["U_DocType"]);
                    _View.Total = Convert.ToDouble(ConvertToString(head["DocTotal"])).ToString("#,##0.00");
                    _View.WhsSls.SelectedValue = ConvertToString(head["U_WarehouseSalesType"]);

                    _View.TotalQuantity = Convert.ToInt32(Lines.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<decimal>("Quantity"))).ToString()).ToString();
                    _View.TotalBefDisc = Lines.AsEnumerable().Sum(x => x.Field<decimal>("LineTotal")).ToString("#,##0.00");
                    _View.DiscountPerc = Convert.ToInt32(header.AsEnumerable().Sum(x => x.Field<decimal>("DiscPrcnt"))).ToString();
                    var dp = (Convert.ToDouble(header.AsEnumerable().Sum(x => x.Field<decimal>("DiscPrcnt"))) / 100);
                    _View.DiscountAmount = (Convert.ToDouble(Lines.AsEnumerable().Sum(x => x.Field<decimal>("LineTotal")).ToString("#,##0.00")) * dp).ToString("#,##0.00");
                    //_View.Tax = ConvertToString(head["Tax"]);
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
                        UnofficialSalesItemsModel.UnofficialSalesItemsData sales = new UnofficialSalesItemsModel.UnofficialSalesItemsData();
                        Dictionary<string, string> DocumentBrand = _repository.GetDocumentBrand(ConvertToString(_View.SuppCode));
                        // Dictionary<string, string> DocumentGl = _repository.GetDocumentGl(ConvertToString(line["AcctCode"]));

                        double qty = ConvertToDouble(line["Quantity"]);
                        double pricebefdisc = ConvertToDouble(line["PriceBefDi"]);
                        double vatrate = ConvertToDouble(line["VatPrcnt"]);

                        Discount = Convert.ToDouble(line["DiscPrcnt"]);

                        double tax = Convert.ToDouble(Hana.Get($@"SELECT T0.ECVatGroup, T1.Rate FROM OCRD T0  INNER JOIN OVTG T1 ON T0.ECVatGroup = T1.Code WHERE T0.CardCode = '{_View.BPCode}'").Rows[0]["Rate"].ToString());
                        tax = 1 + (tax / 100);

                        DiscAmt = (Discount / 100) * Convert.ToDouble(pricebefdisc * tax);
                        GrossPrice = Convert.ToDouble(pricebefdisc * tax) - DiscAmt;
                        string oDelDate = ConvertToString(line["ShipDate"]) == "" ? "" : Convert.ToDateTime(ConvertToString(line["ShipDate"])).ToString("MM/dd/yyyy");

                        LineTotal = (qty * GrossPrice);
                        //DiscAmt = pricebefdisc * (discount / 100);
                        //VatAmount = LineTotal * (vatrate / 100) - ((LineTotal * (vatrate / 100)) * (discount / 100));
                        //GrossTotal = (LineTotal + VatAmount) - (DiscAmt * qty);

                        sales.ItemCode = ConvertToString(line["ItemCode"]); // ItemCode
                        sales.ItemName = ConvertToString(line["Dscription"]); // ItemCode
                        sales.Quantity = qty; //Qty
                        sales.UnitPrice = pricebefdisc;
                        sales.DiscountPerc = Math.Round(Discount, 2);
                        sales.DiscountAmount = Math.Round(DiscAmt, 2);
                        sales.LineTotal = Math.Round(LineTotal, 2);
                        sales.GrossPrice = Convert.ToDouble(GrossPrice.ToString("#00.00")); //GrossPrice
                        sales.PriceBefDisc = Math.Round(Convert.ToDouble(pricebefdisc), 2);
                        sales.Style = ConvertToString(line["U_StyleCode"]); //Style
                        sales.Color = ConvertToString(line["U_Color"]); //Color
                        sales.Size = ConvertToString(line["U_Size"]); //Size
                        sales.Section = ConvertToString(line["U_Section"].ToString()); //Section
                        sales.ItemProperty = "N";
                        sales.DelDate = oDelDate;
                        sales.Company = ConvertToString(line["U_Company"].ToString());

                        sales.FWhsCode = ConvertToString(line["WhsCode"]); //WHsCode
                        sales.TaxCode = ConvertToString(line["VatGroup"]); //Tax Code
                        sales.TaxAmount = ConvertToDouble(line["LineVat"]);
                        sales.TaxRate = ConvertToDouble(line["VatPrcnt"]);
                        sales.EmpDiscountPerc = GetDiscount(_View.SuppCode, ConvertToString(_View.SuppCode.ToString()));
                        sales.EffectivePrice = GetEffectivePrice(_View.SuppCode, ConvertToString(_View.SuppCode.ToString()));
                        sales.BarCode = ConvertToString(line["CodeBars"]);


                        //sales.GrossTotal = Math.Round(GrossTotal, 2);
                        //sales.TotalLCRaw = ConvertToDouble(line["INMPrice"]);
                        //sales.Linenum = Convert.ToInt32(ConvertToString(line["LineNum"]));
                        //sales.Brand = DocumentBrand.Count > 0 ? DocumentBrand["Name"] : ""; //Brand

                        UnofficialSalesItemsModel.UnofficialSalesItems.Add(sales);
                    }
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
                //StaticHelper._MainForm(ex.Message, false);
                return GetEP;
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

        private void ClearItems()
        {
            //if (_View.Table.RowCount > 0)
            //{
                UnofficialSalesItemsModel.UnofficialSalesItems.Clear();
                _View.Table.Rows.Clear();
                _View.TablePreview.Rows.Clear();
                _View.Table.Columns.Clear();
                _View.TablePreview.Columns.Clear();
            //}
        }

        internal void ExecuteCopyDocument(string doc, FrmUnofficialSales UOS)
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

                        break;

                    case "Pick List":

                        _View.DocEntry = _repository.CopyFrom(_View.SuppCode, "OPKL_S");

                        if (InvoiceHeaderModel.DDWdocentry.Count > 0)
                        {
                            string picklistEntry = null;
                            picklistEntry = InvoiceHeaderModel.oCode;
                            //frmSI_DrawDoumentWizard frmDDW = new frmSI_DrawDoumentWizard(SalesInvoice);
                            //frmDDW.ShowDialog();

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

        public string SelectUDFFMS(string table, string AliasID)
        {
            string result = "";

            List<string> parameters = new List<string>()
            {
                "140",
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
            string UosUDF_FmsQry = dataHelper.ReadDataRow(GetUDF_FMS, 1, "", 0);
            string query = Hana.Get(string.Format(UosUDF_FmsQry, "140", AliasID)).Rows[0]["QString"].ToString();

            return query;
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
        public FileInfo[] GetDocumentCrystalForms()
        {
            var sys = new SystemSettings();
            var _settingsService = new SettingsService();

            string path = sys.PathExist($"{_settingsService.GetReportPath()}Sales\\");

            FileInfo[] Files = sys.FileList(path, "*ODLN*" + "*.rpt");

            return Files;
        }
    }
}

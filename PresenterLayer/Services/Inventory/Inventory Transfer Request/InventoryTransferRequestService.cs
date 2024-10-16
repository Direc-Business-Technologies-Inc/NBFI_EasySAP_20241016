using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DirecLayer;
using DomainLayer.Helper;
using DomainLayer.Models.Inventory_Transfer_Request;
using InfrastructureLayer.InventoryRepository;
using MetroFramework;
using PresenterLayer.Helper;
using PresenterLayer.Views.Inventory.Inventory_Transfer_Request;
using PresenterLayer.Views.Main;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zDeclare;
using PresenterLayer.Services.Security;
using System.Diagnostics;
using DomainLayer;
using PresenterLayer.Views;

namespace PresenterLayer.Services.Inventory
{
    public class InventoryTransferRequestService : IInventoryTransferRequestService
    {
        SAPHanaAccess sapHanaAccess = new SAPHanaAccess();
        DirectoryAccess directoryAccess = new DirectoryAccess();
        ServiceLayerAccess serviceLayerAccess = new ServiceLayerAccess();
        MainForm mainForm = new MainForm();
        ITRmaintenance ITRm = new ITRmaintenance();
        DataHelper dataHelper = new DataHelper();
        SboCredentials sboCredentials = new SboCredentials();


        IFrmInventoryTransferRequest _frmITR;
        IQueryRepository _queryRepository;
        public InventoryTransferRequestService(IFrmInventoryTransferRequest frmITR, IQueryRepository queryRepository)
        {
            _frmITR = frmITR;
            _queryRepository = queryRepository;
            EventsSubscription();
        }
        public IFrmInventoryTransferRequest GetFrmITR() { return _frmITR; }
        private void EventsSubscription()
        {
            _frmITR.InventoryRequestLoad += new EventHandler(LoadData);
            _frmITR.InventoryRequestUdfLoad += new DataGridViewCellEventHandler(LoadFields);
            _frmITR.InventoryRequestBPLoad += new EventHandler(GetSupplierInformation);
            _frmITR.InventoryRequestFromWhsLoad += new EventHandler(GetFrmWhsInformation);
            _frmITR.InventoryRequestToWhsLoad += new EventHandler(GetToWhsInformation);
            _frmITR.InventoryRequestSalesEmployeeLoad += new EventHandler(GetSalesEmployeeInformation);
            _frmITR.InventoryRequestAddItem += new EventHandler(SearchItem);
            _frmITR.InventoryRequestCellClick += new DataGridViewCellEventHandler(CellContentClick);
            _frmITR.InventoryRequestCellEndEdit += new DataGridViewCellEventHandler(CellEndEdit);
            _frmITR.InventoryRequestPostITR += new EventHandler(Post);
            _frmITR.InventoryRequestRowHeaderClick += new DataGridViewCellMouseEventHandler(RowHeaderMouseClick);
            _frmITR.InventoryRequestMenuStripDelete += new EventHandler(ContextMenuStripDelete);
            _frmITR.InventoryRequestSeriesChange += new EventHandler(ChangeSeries);
            _frmITR.InventoryRequestTransferTypeChange += new EventHandler(CheckTransferTypeMaintenance);
            _frmITR.InventoryRequestSearchTextChange += new EventHandler(PreviewSearch);

            _frmITR.InventoryRequestPreview += new EventHandler(PreviewData);
            _frmITR.InventoryRequestFindDocument += new EventHandler(FindDocument);
            _frmITR.InventoryRequestChooseDocument += new EventHandler(ChooseDocument);
            _frmITR.InventoryRequestFrmWhsTextChange += new EventHandler(FrmWhsTextChange);
            _frmITR.InventoryRequestToWhsTextChange += new EventHandler(ToWhsTextChange);
            _frmITR.InventoryRequestPrintDocumentChange += new EventHandler(PrintDocumentChange);
            _frmITR.InventoryRequestNewDocument += new EventHandler(NewDocument);
            _frmITR.InventoryRequestCloseForm += new EventHandler(CloseDocuent);
            _frmITR.InventoryRequestFindDocumentTextChange += new EventHandler(SearchDocumentTextChange);
            _frmITR.InventoryRequestFormClose += new FormClosingEventHandler(CloseForm);
            _frmITR.InventoryCopy += new PreviewKeyDownEventHandler(InventoryCopy);

        }
        /// <summary>
        /// pre-loading of fileds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadData(object sender, EventArgs e)
        {
            LoadSeries();
            LoadCompany();
            LoadTransferType();

            _frmITR.PrintPreviewItems(GetDocumentCrystalForms());

            if (!(_frmITR.UDF.Rows.Count > 0))
            {
                Udf_Load();
            }
            LoadData(_frmITR.table);
            LoadData(_frmITR.ItemPreview);
            ChangeDocumentNumber();
            _frmITR.DocStatus = "Open";
        }

        public void LoadCompany()
        {
            string query = "";
            DataTable queryResult = new DataTable();
            query = _queryRepository.GetCompanyQuery();
            queryResult = GetData(query);
            _frmITR.showCompany(queryResult);
        }
        public void LoadTransferType()
        {
            string query = "";
            DataTable queryResult = new DataTable();
            query = _queryRepository.GetTransferTypeQuery();
            queryResult = GetData(query);
            _frmITR.showTransferType(queryResult);
        }
        public void LoadSeries()
        {
            string query = "";
            DataTable queryResult = new DataTable();
            query = _queryRepository.GetSeriesQuery("1250000001");
            queryResult = GetData(query);
            _frmITR.showSeries(queryResult);
        }

        /// <summary>
        /// searching of item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchItem(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_frmITR.Company) == false)
                {
                    InventoryTransferReqHeaderModel.oWhsCode = _frmITR.FrmWhsCode;
                    InventoryTransferReqHeaderModel.oToWhsCode = _frmITR.ToWhsCode;
                    InventoryTransferReqHeaderModel.oTransferType = _frmITR.TransferType;
                    if (_frmITR.FrmWhsCode != "")
                    {
                        FrmTransferRequestItemList form = new FrmTransferRequestItemList()
                        {
                            IsCartonActive = false,
                            oBpCode = _frmITR.BpCode,
                            oBpName = _frmITR.BpName,
                            oWhsCode = _frmITR.FrmWhsCode,
                            oCompany = _frmITR.Company,
                            oDocEntry = _frmITR.DocEntry1.Text.ToString()
                        };

                        form.ShowDialog();
                        LoadData(_frmITR.table);
                        LoadData(_frmITR.ItemPreview);
                    }
                    else
                    {
                        //StaticHelper._MainForm.ShowMessage("Please select a from warehouse before adding.", Color.Red);
                        StaticHelper._MainForm.ShowMessage("Please Select a from warehouse before adding", true);
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Please Select a company before adding", true);
                }
            }
            catch (Exception exc)
            {
                var st = new StackTrace(exc, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                string methodName = frame.GetMethod().Name;
                StaticHelper._MainForm.ShowMessage("Error: " + exc.Message + $"in line ({methodName})" + line, true);
            }


        }
        #region UDF

        private void LoadFields(object sender, DataGridViewCellEventArgs e)
        {
            string fieldName = _frmITR.UDF.CurrentRow.Cells[0].Value.ToString();
            var row = e.RowIndex;
            var col = e.ColumnIndex;
            string result = "";

            try
            {
                if (fieldName.Contains("date") || fieldName.Contains("Date"))
                {
                    //_udfRepo.ConvertToDate(_frmITR.Udf);
                    ConvertToDate(_frmITR.UDF);
                    _frmITR.DocEntry1.Focus();
                    _frmITR.UDF.CurrentCell = _frmITR.UDF[col, row];
                }
                else if (fieldName.Contains("by") || fieldName.Contains("By"))
                {

                    var code = GetITRqry(fieldName).Contains("$") ? SelectEmployee() : SelectUDFFMS("OWTQ", fieldName);

                    //x.Cells[2].Value = m["Address2"];
                    _frmITR.UDF.Rows.Cast<DataGridViewRow>().ToList()
                    .ForEach(x =>
                    {
                        if (x.Cells[0].Value != null)
                        {
                            if (x.Cells[0].Value.ToString() == fieldName)
                            {

                                x.Cells[2].Value = code;
                                _frmITR.DocEntry1.Focus();
                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                //_frmITR.UDF.Rows[row].Cells[col];

                                _frmITR.UDF.CurrentCell = _frmITR.UDF[col, row];

                            }
                        }
                    });

                }
                else if (fieldName.Contains("Carton") || fieldName.Contains("carton"))
                {
                    var code = SelectCartonList();
                    //x.Cells[2].Value = m["Address2"];
                    _frmITR.UDF.Rows.Cast<DataGridViewRow>().ToList()
                    .ForEach(x =>
                    {
                        if (x.Cells[0].Value != null)
                        {
                            if (x.Cells[0].Value.ToString() == fieldName)
                            {

                                x.Cells[2].Value = code;
                                _frmITR.DocEntry1.Focus();
                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                _frmITR.UDF.CurrentCell = _frmITR.UDF[col, row];

                            }
                        }
                    });
                }

                else if (fieldName.Equals("U_VDesc"))
                {
                    //Vehicle Desc
                    var code = SelectVehicle();
                    _frmITR.UDF.Rows.Cast<DataGridViewRow>().ToList()
                    .ForEach(x =>
                    {
                        if (x.Cells[0].Value != null)
                        {
                            if (x.Cells[0].Value.ToString() == fieldName)
                            {

                                x.Cells[2].Value = code == "" ? x.Cells[2].Value : code;
                                _frmITR.DocEntry1.Focus();
                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                _frmITR.UDF.CurrentCell = _frmITR.UDF[col, row];

                            }
                        }
                    });

                    if (code != "")
                    {
                        DataTable dt1 = GetData($"SELECT U_VPla, U_VDriver FROM [@TRUCK] WHERE U_VDesc = '{code}'");
                        _frmITR.UDF.Rows.Cast<DataGridViewRow>().ToList()
                        .ForEach(x =>
                        {
                            if (x.Cells[0].Value != null)
                            {
                                if (x.Cells[0].Value.ToString() == fieldName)
                                {
                                    var vcode = code == "" ? x.Cells[2].Value : code;
                                    //x.Cells[2].Value = $"{vcode} - {dt1.Rows[0]["U_VPla"].ToString()}";
                                    x.Cells[2].Value = $"{vcode}";
                                    _frmITR.DocEntry1.Focus();
                                    //_frmITR.UDF.CurrentCell = x.Cells[2];
                                    _frmITR.UDF.CurrentCell = _frmITR.UDF[col, row];

                                }
                                else if (x.Cells[0].Value.ToString() == "U_VPla")
                                {

                                    x.Cells[2].Value = dt1.Rows[0]["U_VPla"].ToString();
                                    _frmITR.DocEntry1.Focus();
                                    //_frmITR.UDF.CurrentCell = x.Cells[2];
                                    _frmITR.UDF.CurrentCell = _frmITR.UDF[col, row];

                                }
                                else if (x.Cells[0].Value.ToString() == "U_Driver")
                                {
                                    x.Cells[2].Value = dt1.Rows[0]["U_VDriver"].ToString();
                                    _frmITR.DocEntry1.Focus();
                                    //_frmITR.UDF.CurrentCell = x.Cells[2];
                                    _frmITR.UDF.CurrentCell = _frmITR.UDF[col, row];
                                }
                            }
                        });

                    }
                }
                else if (fieldName.Equals("U_AddID"))
                {
                    var code = SelectAddress();

                    _frmITR.UDF.Rows.Cast<DataGridViewRow>().ToList()
                    .ForEach(x =>
                    {
                        if (x.Cells[0].Value != null)
                        {
                            if (x.Cells[0].Value.ToString() == fieldName)
                            {

                                x.Cells[2].Value = code;
                                _frmITR.DocEntry1.Focus();
                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                _frmITR.UDF.CurrentCell = _frmITR.UDF[col, row];

                            }
                        }
                    });
                }
            }
            catch (Exception ex) { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
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
                    date = Convert.ToDateTime(dgv.CurrentCell.Value).ToString("MM/dd/yyyy");
                }

                dgv[2, row.Index].Value = date;
                //oDateTimePicker.Name = $"dt{FrmPurchaseOrder.datetimeCount++}";
                oDateTimePicker.Value = Convert.ToDateTime(date);
                dgv.Controls.Add(oDateTimePicker);
                CreateDateTimePicker(oDateTimePicker, dgv, row);
                //_frmITR.Udf = dgv;
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
            var dtPickerValue = dtPicker.Value.ToShortDateString();

            _frmITR.UDF.CurrentCell.Value = dtPickerValue;
        }
        public void Udf_Load()
        {
            DataTable udf = GetUDF();
            Style(_frmITR.UDF);
            LoadUdf(_frmITR.UDF, udf);
            //LoadData(_frmITR.UDF);
        }
        public DataTable GetUDF()
        {
            string query = UDF("OWTQ");
            return GetData(query);
        }
        public string UDF(string table)
        {
            string query = _queryRepository.GetMaintenanceLineQuery(1, "UDF", table);
            DataTable result = GetData(query);

            string fields = GetColumn(0, result);
            string arrangedUDF = DECLARE.ArrangeUDF(fields);
            string finalQuery = _queryRepository.GetUdfQuery(table, fields, arrangedUDF);
            return finalQuery;
        }
        public string GetColumn(int col, DataTable dt)
        {
            string ret = "";
            if (dt.Rows.Count > 0)
            { ret = dt.Rows[0][col].ToString(); }
            return ret;
        }
        public void Style(DataGridView dgv)
        {
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgv.ReadOnly = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.MultiSelect = false;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersVisible = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;

            //add columns
            //add columns
            dgv.ColumnCount = 3;
            dgv.Columns[0].Name = "Code";
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[1].Name = "Name";
            dgv.Columns[2].Name = "Field";
            dgv.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(191, 235, 245);
        }
        public void LoadUdf(DataGridView dgv, DataTable udf)
        {
            int i = 0;

            foreach (DataRow dr in udf.Rows)
            {
                DataGridViewComboBoxCell cmb = new DataGridViewComboBoxCell();

                string[] row = new string[]
                {
                    dr["AliasID"].ToString(),
                    dr["Descr"].ToString()
                };

                dgv.Rows.Add(row);

                if (dr["UserDefined"].ToString() == string.Empty || dr["UserDefined"].ToString() == "@CartonList")
                {
                    DataTable values = GetUdfValues(dr["FieldID"].ToString());

                    //var GetUDF_FMS = sapHanaAccess.Get(SP.UDF_FMS);
                    //string ItrUDF_FmsQry = dataHelper.ReadDataRow(GetUDF_FMS, 1, "", 0);

                    if (values.Rows.Count > 1)
                    {
                        cmb.DisplayMember = "Name";
                        cmb.ValueMember = "Code";
                        cmb.DataSource = values;

                        dgv.Rows[i].Cells["Field"] = cmb;
                    }
                    //else if ( sapHanaAccess.Get(string.Format(ItrUDF_FmsQry, "1250000940", dr["AliasID"].ToString()) ).Rows.Count > 0)
                    //{
                    //    DataTable dtUDFfms = new DataTable();
                    //    string Get_UDFfms = sapHanaAccess.Get(string.Format(ItrUDF_FmsQry, "1250000940", dr["AliasID"].ToString())).Rows[0]["QString"].ToString();
                    //    if (Get_UDFfms.Contains("$") == false)
                    //    {
                    //        if (sapHanaAccess.Get(Get_UDFfms).Rows.Count > 0)
                    //        {
                    //            values = sapHanaAccess.Get(Get_UDFfms);
                    //            foreach (DataColumn col1 in values.Columns)
                    //            {
                    //                cmb.DisplayMember = col1.ColumnName.ToString();
                    //                cmb.ValueMember = col1.ColumnName.ToString();
                    //            }
                    //            cmb.DataSource = values;
                    //            dgv.Rows[i].Cells["Field"] = cmb;
                    //        }
                    //    }
                    //}
                    else
                    {
                        dgv.Rows[i].Cells["Field"] = new DataGridViewTextBoxCell();
                        if (dr["Descr"].ToString().Contains("Vehicle") || dr["Descr"].ToString().Contains("Driver"))
                        {
                            dgv.Rows[i].Cells["Field"].ReadOnly = true;
                        }
                    }
                }
                else
                {
                    DataTable tableValues = GetUdfTableValues(dr["UserDefined"].ToString());

                    cmb.DisplayMember = "Name";
                    cmb.ValueMember = "Code";
                    cmb.DataSource = tableValues;

                    dgv.Rows[i].Cells["Field"] = cmb;
                }

                i++;
            }
        }
        private DataTable GetUdfValues(string value)
        {
            string query = _queryRepository.GetUdfValidValuesQUery(value);
            return GetData(query);
        }
        private DataTable GetUdfTableValues(string value)
        {
            string query = _queryRepository.GetUdfTableValuesQuery(value);
            return GetData(query);
        }
        #endregion

        public FileInfo[] GetDocumentCrystalForms()
        {
            //string path = $"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\ITR";
            //directoryAccess.CreateDirectory(path);
            var sys = new SystemSettings();
            var _settingsService = new SettingsService();

            string path = sys.PathExist($"{_settingsService.GetReportPath()}Inventory\\");

            FileInfo[] Files = sys.FileList(path, "*OWTQ*" + "*.rpt");

            return Files;
        }

        /// <summary>
        /// query to database using direlayer.saphanaaccess.get
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetData(string query)
        {
            DataTable queryReturn = sapHanaAccess.Get(query);
            return queryReturn;
        }
        #region MyRegion
        internal void GetSupplierInformation(object sender, EventArgs e)
        {
            var m = SelectSupplier();

            if (m.Count > 0)
            {
                _frmITR.BpCode = m["CardCode"];
                _frmITR.BpName = m["CardName"];
                _frmITR.Address = m["Address"];
                _frmITR.SalesEmployee = m["SalesEmployeeName"];
                _frmITR.oSalesEmployee = m["SalesEmployeeCode"];

                //string transtype = _repository.AutomateTransferType(_frmITR.SuppCode);
                //string orderNo = _repository.GetOrderNo(_frmITR.SuppCode);

                //var ITRUdf = _frmITR.UDF.Rows.Cast<DataGridViewRow>().ToList();
                //int temp = 0;
                //var temp2 = ITRUdf.Select(x => x.Cells).ToList();

                try
                {
                    _frmITR.UDF.Rows.Cast<DataGridViewRow>().ToList()
                    .ForEach(x =>
                    {
                        if (x.Cells[1].Value != null)
                        {
                            if (x.Cells[1].Value.ToString() == "Address ID")
                            {
                                x.Cells[2].Value = m["Address2"];
                            }
                            else if (x.Cells[1].Value.ToString().Equals("Area"))
                            {
                                x.Cells[2].Value = GetUdfTableValues("@GEO_LOC").Select().ToList().Exists(a => a["Name"].ToString() == GetArea(m["Address2"])) ? GetArea(m["Address2"]) : "";
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    StaticHelper._MainForm.ShowMessage("Invalid UDF Value. Please check setup in SAP.", true);
                }


                InventoryTransferReqHeaderModel.oBPCode = _frmITR.BpCode;
                _frmITR.OCode = _frmITR.BpCode;
                if (_frmITR.OCode != null)
                {
                    LoadBPDetails(_frmITR.OCode);
                    InventoryTransferReqHeaderModel.oBPCode = _frmITR.OCode;
                }
                else
                {
                    _frmITR.oProject = "";
                    _frmITR.BpName = "";
                    _frmITR.BpAddress = "";
                }
            }
        }

        public string GetArea(string sAddress)
        {
            var query = $"SELECT A.U_Gloc FROM CRD1 A WHERE A.CardCode = '{_frmITR.BpCode}' AND A.Address = '{sAddress}'";
            var dt = sapHanaAccess.Get(query);
            return dataHelper.DataTableRet(dt, 0, "U_Gloc", "");
        }
        public Dictionary<string, string> SelectSupplier()
        {
            var info = new Dictionary<string, string>();

            var parameters = new List<string>()
            {
                "C"
            };

            List<string> m = Modal("OCRD", parameters, "List Of Business Partners");

            if (m.Count > 0)
            {
                var query = _queryRepository.BPinformationQuery(m[0]);
                DataTable dt = GetData(query);

                if (dt.Rows.Count > 0)
                {
                    info.Add("CardCode", m[0]);
                    info.Add("CardName", m[1]);
                    info.Add("Address", ValidateInput.String(dt.Rows[0]["Address"]));
                    info.Add("Address2", ValidateInput.String(dt.Rows[0]["Add2"]));
                    info.Add("SalesEmployeeCode", ValidateInput.String(dt.Rows[0]["SlpCode"]));
                    info.Add("SalesEmployeeName", ValidateInput.String(dt.Rows[0]["SlpName"]));
                    info.Add("PriceList", ValidateInput.String(dt.Rows[0]["ListNum"]));
                    info.Add("GroupCode", ValidateInput.String(dt.Rows[0]["GroupCode"]));
                    info.Add("Whs", ValidateInput.String(dt.Rows[0]["Whs"]));
                    info.Add("ECVatGroup", ValidateInput.String(dt.Rows[0]["ECVatGroup"]));
                }
            }

            return info;
        }
        #endregion

        #region warehouse info
        internal void GetFrmWhsInformation(object sender, EventArgs e)
        {
            var m = SelectWhs();
            if (m.Count > 0)
            {
                _frmITR.FrmWhsCode = m[0];
                //foreach (DataGridViewRow row in _frmITR.table.Rows)
                //{
                //    row.Cells["From Warehouse"].Value = _frmITR.FrmWhsCode;
                //}

                //var rowCount = _frmITR.table.Rows.Count;
                //DataGridViewRowCollection row = _frmITR.table.Rows;
                //for (int x = 0; x < rowCount; x++)
                //{
                //    row[x].Cells["From Warehouse"].Value = _frmITR.FrmWhsCode;
                //}
                foreach (var x in InventoryTransferReqItemsModel.ITRitems.Where(x => x.ItemCode != "").ToList())
                {
                    x.FWhsCode = _frmITR.FrmWhsCode;
                }

                LoadData(_frmITR.table);
                LoadData(_frmITR.ItemPreview);
                //var items = InventoryTransferReqItemsModel.ITRitems.Where(x => x.ItemCode != "").ToList();
                //var itemCount = items.Count();
                //for (int x = 0; x < itemCount; x++)
                //{
                //    items[x].FWhsCode = _frmITR.FrmWhsCode;
                //}
            }
        }
        internal void FrmWhsTextChange(object sender, EventArgs e)
        {
            foreach (var x in InventoryTransferReqItemsModel.ITRitems.Where(x => x.ItemCode != "").ToList())
            {
                x.FWhsCode = _frmITR.FrmWhsCode;
            }

            LoadData(_frmITR.table);
            LoadData(_frmITR.ItemPreview);
        }
        internal void ToWhsTextChange(object sender, EventArgs e)
        {
            foreach (var x in InventoryTransferReqItemsModel.ITRitems.Where(x => x.ItemCode != "").ToList())
            {
                x.TWhsCode = _frmITR.ToWhsCode;
            }

            LoadData(_frmITR.table);
            LoadData(_frmITR.ItemPreview);
        }
        internal void GetToWhsInformation(object sender, EventArgs e)
        {
            var m = SelectWhs();
            if (m.Count > 0)
            {
                _frmITR.ToWhsCode = m[0];
                //foreach (DataGridViewRow row in _frmITR.table.Rows)
                //{
                //    row.Cells["To Warehouse"].Value = _frmITR.ToWhsCode;
                //}

                //var rowCount = _frmITR.table.Rows.Count;
                //DataGridViewRowCollection row = _frmITR.table.Rows;
                //for (int x = 0; x < rowCount; x++)
                //{
                //    row[x].Cells["To Warehouse"].Value = _frmITR.ToWhsCode;
                //}
                foreach (var x in InventoryTransferReqItemsModel.ITRitems.Where(x => x.ItemCode != "").ToList())
                {
                    x.TWhsCode = _frmITR.ToWhsCode;
                }

                LoadData(_frmITR.table);
                LoadData(_frmITR.ItemPreview);
                //var items = InventoryTransferReqItemsModel.ITRitems.Where(x => x.ItemCode != "").ToList();
                //var itemCount = items.Count();
                //for (int x = 0; x < itemCount; x++)
                //{
                //    items[x].TWhsCode = _frmITR.ToWhsCode;
                //}

            }
        }
        public List<string> SelectWhs()
        {
            List<string> parameters = new List<string>()
            {
                "C"
            };

            List<string> m = Modal("OWHS", parameters, "List Of Warehouses");

            return m;
        }
        #endregion

        #region show sales employee
        internal void GetSalesEmployeeInformation(object sender, EventArgs e)
        {
            var m = SelectSalesEmployee();
            if (m.Count > 0)
            {
                _frmITR.SalesEmployee = m[1];
            }
        }
        public List<string> SelectSalesEmployee()
        {
            List<string> parameters = new List<string>();
            //{
            //    "C"
            //};

            List<string> m = Modal("OSLP", parameters, "List Of Employees");

            return m;
        }
        #endregion

        /// <summary>
        /// conversion of object to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        #region loading of items
        public void LoadData(DataGridView dgv, bool isFirstLoad = false)
        {
            string objType = FrmInventoryTransferRequest.objType;
            try
            {
                dgv.Columns.Clear();

                InventoryTransferRequestItemsController.DataGridViewITRData(dgv);

                var samp = InventoryTransferReqItemsModel.ITRitems.Where(x => x.ObjType == objType);
                int rowcnt = 0;
                foreach (var x in InventoryTransferReqItemsModel.ITRitems.Where(x => x.ObjType == objType))
                {
                    //double InfoPrice = x.GrossPrice / 1.12;
                    double InfoPrice = x.GrossPrice;
                    object[] lineitem = { x.BarCode, x.ItemCode, x.ItemName, InfoPrice, x.Brand, x.StyleCode, x.Style
                                        , x.ColorCode, x.Color, x.Section, x.Size, x.Quantity, x.BrandCode, x.FWhsCode,"...", x.TWhsCode,"...", x.Company, "", "", x.Linenum, x.SortCode, x.InventoryUOM,x.Chain,x.ChainDescription
                                        };

                    dgv.Rows.Add(lineitem);
                    AdditionalColumns(dgv, rowcnt, x.ItemCode, _frmITR.BpCode, InfoPrice);
                    rowcnt++;
                }

                InventoryTransferRequestItemsController.dataGridLayout(dgv);
                //dgvItems.Sort(dgvItems.Columns["SortCode"], ListSortDirection.Ascending);

                // Computation For Total
                //DECLARE.ComputeTotal(dgvItems, null, out TotalBefDIsc, out TotalQty, out TotalDisc, out TotalAftDisc, out TotalTax);
                ComputeTotal();

            }
            catch (Exception ex)
            {
                //StaticHelper._MainForm.ShowMessage(ex.Message, Color.Red);
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }

        }
        private void ComputeTotal()
        {
            double dblTotalQty = 0;
            double dblTotalPrice = 0;

            foreach (DataGridViewRow row in _frmITR.table.Rows)
            {
                double dblQty = Convert.ToDouble(DECLARE.Replace(row, "Quantity", "0.00"));
                double dblPrice = Convert.ToDouble(DECLARE.Replace(row, "Info Price", "0.00"));

                dblTotalQty += dblQty;

                double dblPriceQty = (dblPrice * dblQty);
                dblTotalPrice += dblPriceQty;
            }

            _frmITR.TotalQuantity = dblTotalQty.ToString("#,#00");
            _frmITR.Total = dblTotalPrice.ToString("#,#00.00");
        }

        private void AdditionalColumns(DataGridView dgv, int rowcnt, string oItemCode, string oBPCode, double oInfoPrice)
        {
            try
            {
                if (_frmITR.BpCode != "")
                {
                    string strSKU = " select distinct b.U_SKU [SKU]" +
                                        " from OCPN a " +
                                        " inner " +
                                        " join CPN1 c on a.CpnNo = c.CpnNo " +
                                        " inner join CPN2 b on a.CpnNo = b.CpnNo " +
                                        $" where c.BpCode = '{_frmITR.BpCode}' " +
                                        $" and a.U_CType = 'SKU' and b.ItemCode = '{oItemCode}' " +
                                        " order by SKU asc";

                    //On comment due to Pooling issue 083019
                    //string strSKU = $@"CALL SKULOOP('{oItemCode}','{oBPCode}','{oInfoPrice}',(Select count(*) from ""@OSKV"" x inner join ""@SKV1"" y on x.""Code"" = y.""Code"" where x.""Code"" = (Select yy.""SeriesName"" from OCRD xx inner join NNM1 yy on xx.""Series"" = yy.""Series"" where xx.""CardCode"" = '{oBPCode}'))," +
                    //    $@"(Select yy.""SeriesName"" from OCRD xx inner join NNM1 yy on xx.""Series"" = yy.""Series"" where xx.""CardCode"" = '{oBPCode}'))";

                    DataTable dt1 = sapHanaAccess.Get(strSKU);


                    if (dt1.Rows.Count > 0)
                    {
                        dgv.Rows[rowcnt].Cells["SKU"].Value = dt1.Rows[0][0].ToString();
                    }
                    else
                    {
                        dgv.Rows[rowcnt].Cells["SKU"].Value = BarcodeCtrl.CheckNoMaintenance(oBPCode) ? oItemCode : "";
                    }

                }

            }
            catch (Exception ex)
            {
                //StaticHelper._MainForm.ShowMessage(ex.Message, Color.Red);
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }
        #endregion

        /// <summary>
        /// show search modal
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="Parameters"></param>
        /// <param name="title"> </param>
        /// <returns></returns>
        public static List<string> Modal(string searchKey, List<string> Parameters, string title)
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

        #region cell content click
        private void CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex - 1;
            colIndex = colIndex < 0 ? 0 : colIndex;
            DataTable dt = _frmITR.table.DataSource as DataTable;

            string columnName = _frmITR.table.Columns[colIndex].Name;
            int index = _frmITR.table.Focus() ? Convert.ToInt32(_frmITR.table.CurrentRow.Cells["LineNum"].Value) : Convert.ToInt32(_frmITR.ItemPreview.CurrentRow.Cells["LineNum"].Value);
            //To Warehouse
            //From WareHouse
            //Company
            switch (columnName)
            {
                case "To Warehouse":
                    var toWarehouse = SelectWarehouse();
                    if (toWarehouse != string.Empty)
                    {
                        UpdateCell(colIndex, toWarehouse);
                        InventoryTransferReqItemsModel.ITRitems.Find(x => x.Linenum == index).TWhsCode = toWarehouse;
                    }
                    break;

                case "From Warehouse":
                    var frmWarehouse = SelectWarehouse();
                    if (frmWarehouse != string.Empty)
                    {
                        UpdateCell(colIndex, frmWarehouse);
                        InventoryTransferReqItemsModel.ITRitems.Find(x => x.Linenum == index).FWhsCode = frmWarehouse;
                        InventoryTransferReqHeaderModel.oWhsCode = frmWarehouse;
                    }
                    break;

                case "Company":
                    var Company = SelectCompany();
                    if (Company != string.Empty)
                    {
                        UpdateCell(colIndex, Company);
                        InventoryTransferReqItemsModel.ITRitems.Find(x => x.Linenum == index).Company = Company;
                    }
                    break;

                case "Chain Description":
                    //GetChain(colIndex, index);
                    //if (Chain != string.Empty)
                    //{
                    //    zUpdateCell(colIndex, );
                    //    InventoryTransferReqItemsModel.ITRitems.Find(x => x.Linenum == index).Company = Company;
                    //}

                    Dictionary<string, string> chainDetails = SelectChain();

                    if (chainDetails.Count() > 0)
                    {
                        UpdateCell(colIndex, chainDetails["Value"]);
                        UpdateCell(colIndex - 1, chainDetails["Code"]);
                        InventoryTransferReqItemsModel.ITRitems.Where(x => x.Linenum == index)
                            .ToList().ForEach(x =>
                            {
                                x.Chain = chainDetails["Code"];
                                x.ChainDescription = chainDetails["Value"];

                            });
                    }



                    break;
            }
        }

        //internal void GetChain(int colIndex, int index)
        //{
        //    Dictionary<string, string> chainDetails = SelectChain();

        //    if (chainDetails.Count() > 0)
        //    {
        //        UpdateCell(colIndex, chainDetails["Value"]);
        //        UpdateCell(colIndex - 1, chainDetails["Code"]);
        //        InventoryTransferItemsModel.ITitems.Where(x => x.Linenum == index)
        //            .ToList().ForEach(x =>
        //            {
        //                x.Chain = chainDetails["Code"];
        //                x.ChainDescription = chainDetails["Value"];

        //            });
        //    }
        //}

        public Dictionary<string, string> SelectChain()
        {
            Dictionary<string, string> chain = new Dictionary<string, string>();

            var m = DataRepository.Modal("chain", null, "List of Chains");

            if (m.Count > 0)
            {
                chain.Add("Code", m[0]);
                chain.Add("Value", m[1]);
            }

            return chain;
        }
    

        public string SelectWarehouse()
        {
            string result = "";

            var list = Modal("OWHS", null, "List of Warehouse");

            if (list.Count > 0)
            {
                result = list[0];
            }

            return result;
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


        private void UpdateCell(int ColIndex, string value)
        {
            _frmITR.table.CurrentRow.Cells[ColIndex].Value = value;

            int count = _frmITR.ItemPreview.RowCount;

            if (count > 1)
            {
                //_frmITR.ItemPreview.CurrentRow.Cells[ColIndex].Value = value;
                //_frmITR.ItemPreview.Rows.Clear();
                //LoadData(_frmITR.ItemPreview);
            }
        }
        #endregion

        #region Posting
        private void Post(object sender, EventArgs e)
        {

            var itrServiceLayer = new ITR_ServiceLayer(_frmITR, mainForm, this);
            //serviceLayerAccess.ITR_Posting("POST");
            if (_frmITR.datePickerPostingDate.Text != " " && _frmITR.datePickerDueDate.Text != " " && _frmITR.datePickerDocDate.Text != " ")
            {
                if (PreChecking() == true)
                {
                    if (CheckReqWhs("U_FillerAction", 13, _frmITR.FrmWhsCode) == true)
                    {
                        if (CheckReqWhs("U_DestAction", 15, _frmITR.ToWhsCode) == true)
                        {
                            if (_frmITR.buttonBtnRequest.Text == "Add")
                            {
                                itrServiceLayer.ITR_Posting("POST");
                            }
                            else
                            {
                                itrServiceLayer.ITR_Posting("PATCH", Convert.ToInt32(_frmITR.DocEntry1.Text));
                            }
                        }
                        else
                        {
                            StaticHelper._MainForm.ShowMessage("Header To Warehouse and per line To Warehouse does not match.", true);
                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Header From Warehouse and per line From Warehouse does not match.", true);
                    }
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Please complete the required dates.", true);
            }

        }
        public void ClearData()
        {
            //numseries
            //ClearList();
            ClearField();
            _frmITR.BtnRequestText = "Add";
            LoadTransferType();
            LoadCompany();
            LoadSeries();
            _frmITR.table.Columns.Clear();
            _frmITR.ItemPreview.Columns.Clear();
            _frmITR.UDF.Columns.Clear();
            if (!(_frmITR.UDF.Rows.Count > 0))
            {
                Udf_Load();
            }
            LoadData(_frmITR.table);
            LoadData(_frmITR.ItemPreview);
            ChangeDocumentNumber();
            _frmITR.DocStatus = "Open";
            foreach (Control c in _frmITR.Panel2.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
            }
            _frmITR.FrmWhsCode = ""; _frmITR.ToWhsCode = "";
            EnableControls();
        }

        public void ClearList()
        {
            //CLEAR LIST DATA
            DECLARE._DocHeader.RemoveAll(x => x.ObjType == FrmInventoryTransferRequest.objType);
            //DECLARE._DocItems.RemoveAll(x => x.ObjType == objType);
            InventoryTransferReqItemsModel.ITRitems.RemoveAll(x => x.ObjType == FrmInventoryTransferRequest.objType);
            InventoryTransferReqHeaderModel.oAddressID = "";
            InventoryTransferReqHeaderModel.oArea = "";
            //DECLARE.udf.RemoveAll(x => x.ObjCode == objType);
            DECLARE.udf.RemoveAll(x => x.ObjCode == FrmInventoryTransferRequest.objType);
            _frmITR.oProject = "";
        }

        #endregion

        private void LoadBPDetails(string CardCode)
        {
            string query = "SELECT A.CardCode,A.CardName,A.MailAddres [Address]" +
                    ", (SELECT min(Y.Address) [Address] FROM CRD1 Y Where Y.CardCode = A.CardCode And AdresType = 'S') [Add2]" +
                    ", A.ProjectCod,A.SlpCode,A.ListNum,A.GroupCode " +
                    ",(Select Z.SlpName FROM OSLP Z Where Z.SlpCode = A.SlpCode) [SlpName] " +
                    $",(SELECT min(Y.U_Whs) [U_Whs] FROM CRD1 Y Where Y.CardCode = A.CardCode And AdresType = 'S') [Whs],ECVatGroup FROM OCRD A WHERE A.CardCode = '{CardCode}'";

            var dt = GetData(query);
            var helper = new DataHelper();
            string strAddressID = helper.ReadDataRow(dt, "Add2", "", 0);
            InventoryTransferReqHeaderModel.oAddressID = strAddressID;
            string strGetAreaQry = $"SELECT A.U_Gloc FROM CRD1 A WHERE A.CardCode = '{CardCode}' AND A.Address = '{strAddressID}'";
            if (GetData(strGetAreaQry).Rows.Count > 0)
            {
                InventoryTransferReqHeaderModel.oArea = GetData(strGetAreaQry).Rows[0]["U_Gloc"].ToString();
            }
            _frmITR.oProject = helper.ReadDataRow(dt, "ProjectCod", "", 0);
        }

        internal void UdfRequest()
        {
            string fieldName = _frmITR.UDF.CurrentRow.Cells[0].Value.ToString();

            string result = "";

            if (result != string.Empty)
            {
                _frmITR.UDF.CurrentRow.Cells[2].Value = result;
            }
        }

        public string SelectEmployee()
        {
            string result = "";
            var list = Modal("EmployeeList *", null, "List of Employee");
            if (list.Count > 0)
            {
                result = list[0];
            }
            return result;
        }

        public string SelectVehicle()
        {
            string result = "";
            var list = Modal("Get Vehicle List", null, "List of Vehicle");
            if (list.Count > 0)
            {
                result = list[0];
            }
            return result;
        }
        public string SelectAddress()
        {
            string result = "";

            //x.Cells[2].Value = m["Address2"];
            List<string> Parameters = new List<string>()
                {
                   _frmITR.BpCode
                };
            var list = Modal("AddressID", Parameters, "List of Address");
            if (list.Count > 0)
            {
                result = list[0];
            }
            return result;
        }
        public string SelectCartonList()
        {
            string result = "";
            var list = Modal("CartonList", null, "List of CartonList");
            if (list.Count > 0)
            {
                result = list[0];
            }
            return result;
        }

        public string SelectSKU()
        {
            string result = "";
            var list = Modal("SKU", null, "List of SKU");

            if (list.Count > 0)
            {
                result = list[0];
            }

            return result;
        }

        public void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_frmITR.table.Columns[e.ColumnIndex].Name == "Quantity" || _frmITR.table.Columns[e.ColumnIndex].Name == "Info Price")
            {
                string ItemCode = _frmITR.table.Rows[e.RowIndex].Cells["Item No."].Value.ToString();
                int strLineNum = Convert.ToInt32(_frmITR.table.Rows[e.RowIndex].Cells["LineNum"].Value.ToString());

                var ITRList = InventoryTransferReqItemsModel.ITRitems.First(x => x.ItemCode == ItemCode && x.Linenum == strLineNum);

                if (_frmITR.table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Any(char.IsDigit) == false)
                {
                    if (_frmITR.table.Columns[e.ColumnIndex].Name == "Quantity")
                    {
                        double dOrigQty = Convert.ToDouble(ITRList.Quantity.ToString());
                        _frmITR.table.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = Convert.ToDouble(dOrigQty); //Requested to remove logic 07212021 //GetCartonQty(ItemCode, dOrigQty);
                        _frmITR.table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dOrigQty;
                    }
                    else if (_frmITR.table.Columns[e.ColumnIndex].Name == "Info Price")
                    {
                        string oUPrice = ITRList.UnitPrice.ToString();
                        _frmITR.table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oUPrice;
                    }
                }
                else
                {
                    if (_frmITR.table.Columns[e.ColumnIndex].Name == "Quantity")
                    {
                        string oQty = _frmITR.table.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();

                        foreach (var x in InventoryTransferReqItemsModel.ITRitems.Where(x => x.ItemCode == ItemCode))
                        {
                            x.Quantity = Convert.ToDouble(oQty);
                        }

                        _frmITR.table.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = Convert.ToDouble(oQty); //Requested to remove logic 07212021 //GetCartonQty(ItemCode, Convert.ToDouble(oQty));
                    }
                    else if (_frmITR.table.Columns[e.ColumnIndex].Name == "Info Price")
                    {
                        string oUPrice = _frmITR.table.Rows[e.RowIndex].Cells["Info Price"].Value.ToString();

                        foreach (var x in InventoryTransferReqItemsModel.ITRitems.Where(x => x.ItemCode == ItemCode))
                        {
                            x.UnitPrice = Convert.ToDouble(oUPrice);
                        }
                    }

                    ComputeTotal();
                }

            }
        }

        public void RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    _frmITR.table.CurrentCell = _frmITR.table.Rows[e.RowIndex].Cells[e.ColumnIndex + 1];
                    _frmITR.table.Rows[e.RowIndex].Selected = true;
                    _frmITR.table.Focus();

                    var mousePosition = _frmITR.table.PointToClient(Cursor.Position);

                    _frmITR.MsItems.Show(_frmITR.table, mousePosition);
                }
            }
        }

        public void ContextMenuStripDelete(object sender, EventArgs e)
        {
            var table = _frmITR.ItemPreview.Focus() ? _frmITR.ItemPreview : _frmITR.table;
            foreach (DataGridViewRow row in table.Rows)
            {
                if (row.Selected == true)
                {
                    //var index = _frmITR.table.CurrentCell.RowIndex;
                    var index = row.Index;
                    string StyleName, ColorName, ItemCode, LineNum;

                    ItemCode = _frmITR.table.Rows[index].Cells["Item No."].Value.ToString();
                    StyleName = _frmITR.table.Rows[index].Cells["Style"].Value.ToString();
                    ColorName = _frmITR.table.Rows[index].Cells["Color"].Value.ToString();
                    LineNum = _frmITR.table.Rows[index].Cells["LineNum"].Value.ToString();

                    InventoryTransferReqItemsModel.ITRitems.RemoveAll(x => x.ObjType == FrmInventoryTransferRequest.objType && x.Style == StyleName && x.Color == ColorName && x.ItemCode == ItemCode && x.Linenum == Convert.ToInt32(LineNum));
                }
            }

            LoadData(_frmITR.table);
            LoadData(_frmITR.ItemPreview);
            //RefreshData();
        }

        public void RefreshData()
        {
            try
            {
                _frmITR.table.Columns.Clear();
                _frmITR.ItemPreview.Columns.Clear();

                InventoryTransferRequestItemsController.DataGridViewITRData(_frmITR.table);
                InventoryTransferRequestItemsController.DataGridViewITRData(_frmITR.ItemPreview);

                int rowcnt = 0;
                foreach (var x in InventoryTransferReqItemsModel.ITRitems.Where(x => x.ObjType == FrmInventoryTransferRequest.objType))
                {
                    //double InfoPrice = x.GrossPrice / 1.12;
                    double InfoPrice = x.GrossPrice;
                    object[] lineitem = { x.BarCode, x.ItemCode, x.ItemName, InfoPrice, x.BrandCode, x.Brand, x.StyleCode, x.Style
                                        , x.ColorCode, x.Color, x.Section, x.Size, x.Quantity, x.FWhsCode,"...", x.TWhsCode,"...", x.Company, "", "", x.Linenum, x.SortCode, x.InventoryUOM
                                        };

                    _frmITR.table.Rows.Add(lineitem);
                    AdditionalColumns(_frmITR.table, rowcnt, x.ItemCode, _frmITR.BpCode, InfoPrice);
                    _frmITR.ItemPreview.Rows.Add(lineitem);
                    AdditionalColumns(_frmITR.table, rowcnt, x.ItemCode, _frmITR.BpCode, InfoPrice);
                    rowcnt++;
                }

                InventoryTransferRequestItemsController.dataGridLayout(_frmITR.table);
                InventoryTransferRequestItemsController.dataGridLayout(_frmITR.ItemPreview);
                //dgvItems.Sort(dgvItems.Columns["SortCode"], ListSortDirection.Ascending);

                // Computation For Total
                //DECLARE.ComputeTotal(dgvItems, null, out TotalBefDIsc, out TotalQty, out TotalDisc, out TotalAftDisc, out TotalTax);
                ComputeTotal();

            }
            catch (Exception ex)
            {
                //StaticHelper._MainForm.ShowMessage(ex.Message, Color.Red);
                StaticHelper._MainForm.ShowMessage(ex.Message, true);

            }
        }

        internal void ChangeDocumentNumber()
        {
            if (_frmITR.BtnRequestText == "Add")
            {
                string result = "";
                string query = _queryRepository.GetDocNumQuery("1250000001", _frmITR.oSeries);
                DataTable dt = GetData(query);

                if (dt.Rows.Count > 0)
                {
                    result = ValidateInput.String(dt.Rows[0]["NextNumber"]);
                }

                if (result != string.Empty)
                {
                    _frmITR.DocNum = result;
                }
            }
        }

        public void ChangeSeries(object sender, EventArgs e)
        {
            //var sdata = $"SELECT T0.ObjectCode,T0.Series,T0.SeriesName,T0.NextNumber FROM NNM1 T0 Where T0.ObjectCode = 1250000001 And SeriesName = '{ _frmITR.oSeries }'";

            //var dt = GetData(sdata);

            //_frmITR.oSeries = DataAccess.Search(dt, 0, "Series");
            //FrmInventoryTransferRequest.oSeriesName = DataAccess.Search(dt, 0, "SeriesName");

            //_frmITR.DocNum = DataAccess.Search(dt, 0, "NextNumber");

            ChangeDocumentNumber();
        }

        private void CheckTransferTypeMaintenance(object sender, EventArgs e)
        {
            try
            {
                if (_frmITR.BtnRequestText == "Add")
                {
                    if (ITRm.SelValue("series1", _frmITR.comboboxTransferType.SelectedValue.ToString()) != "")
                    {
                        _frmITR.comboboxSeries.SelectedIndex = _frmITR.comboboxSeries.FindString(ITRm.SelValue("series1", _frmITR.comboboxTransferType.SelectedValue.ToString()));
                    }
                }

                _frmITR.FrmWhsCode = "";
                _frmITR.ToWhsCode = "";

                if (ITRm.SelValue("frmWhs1", _frmITR.comboboxTransferType.Text, _frmITR.BpCode, InventoryTransferReqHeaderModel.oAddressID) != "")
                {
                    InventoryTransferReqHeaderModel.oWhsCode = ITRm.SelValue("frmWhs1", _frmITR.comboboxTransferType.Text, _frmITR.BpCode, InventoryTransferReqHeaderModel.oAddressID);
                    /*txtFWhsCode.Text*/
                    _frmITR.FrmWhsCode = InventoryTransferReqHeaderModel.oWhsCode;
                }

                if (ITRm.SelValue("toWhs1", _frmITR.comboboxTransferType.Text, _frmITR.BpCode, InventoryTransferReqHeaderModel.oAddressID) != "")
                {
                    /*txtTWhsCode.Text*/
                    //On comment by Cedi 061719, based on old codes
                    //_frmITR.ToWhsCode = ITRm.SelValue("toWhs1", _frmITR.FrmWhsCode, _frmITR.comboboxTransferType.Text, InventoryTransferReqHeaderModel.oAddressID);
                    //ITRm.SelValue("toWhs1", cbTransferType.Text, txtBpCode.Text, InventoryTransferReqHeaderModel.oAddressID)  //OLD CODE
                    _frmITR.ToWhsCode = ITRm.SelValue("toWhs1", _frmITR.comboboxTransferType.Text, _frmITR.BpCode, InventoryTransferReqHeaderModel.oAddressID);
                }

                InventoryTransferReqHeaderModel.oTransferType = _frmITR.comboboxTransferType.Text;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        public void PreviewSearch(object sender, EventArgs e)
        {
            DataHelper.RowSearch(_frmITR.ItemPreview, _frmITR.ItemPreviewSearch, 0);
        }

        public void PreviewData(object sender, EventArgs e)
        {
            _frmITR.ItemPreview.Rows.Clear();
            LoadData(_frmITR.ItemPreview);
        }
        public void FindDocument(object sender, EventArgs e)
        {
            int PgSize = Convert.ToInt32(_frmITR.FindPageLimit);
            string query = "SELECT TOP " + PgSize + " A.DocEntry," +
                 "CASE " +
                        "WHEN A.CANCELED = 'Y' THEN 'Canceled' " +
                        "WHEN A.CANCELED = 'N' AND A.DocStatus != 'O' THEN 'Closed'  " +
                        "WHEN A.DocStatus = 'O' THEN 'Open' END [Status], " +
                "(SELECT T0.SeriesName FROM NNM1 T0 " +
                "Where T0.ObjectCode = 1250000001 and T0.Series = A.Series)Series," +
                "A.DocNum [Doc No.]," +
                "A.CardCode [BP Code]," +
                "A.CardName [BP Name]," +
                "A.DocDate [Doc Date]," +
                "A.DocDueDate [Delivery Date]," +
                "A.TaxDate [Document Date]," +
                "(select MailAddres from OCRD where CardCode = A.CardCode) [Address]" +
                ", A.U_Remarks" +
                ", A.U_DateDel" +
                ", (select ProjectCod from OCRD where CardCode = A.CardCode) [ProjectCod]" +
                ", A.Filler" +
                ", A.SlpCode " +
                ", (select SlpName from OSLP where SlpCode = A.SlpCode) [SlpName]" +
                ", A.U_TransferType " +
                ", (select Name from [@CMP_INFO] where Code = A.U_CompanyTIN) [U_CompanyTIN], " +
                "A.U_SINo [SI No.]," +
                "A.U_DRNo [DR No.]," +
                "A.U_PONo [PO No.]," +
                "(SELECT Sum(Z.Quantity) From WTQ1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] " +
                "FROM OWTQ A " +
                $"WHERE CANCELED = 'N'" +
                "Order By A.DocEntry Desc";


            var dt = GetData(query);
            //if (InventoryTransferReqItemsModel.ITRitems.Where(x => x.ObjType == FrmInventoryTransferRequest.objType).ToList().Count > 0)
            //{
            //var result = MessageBox.Show("Unsaved data will be lost. Continue?", this.lblTitle.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (result == DialogResult.Yes)
            //{
            //LoadFindDetails();
            //}

            _frmITR.FindDocumentTable.DataSource = dt;
            InventoryTransferRequestItemsController.dataGridLayout(_frmITR.FindDocumentTable);
            //}
            //else
            //{
            //    LoadFindDetails();
            //}
        }

        public void ChooseDocument(object sender, EventArgs e)
        {
            var itemNo = _frmITR.FindDocumentTable.CurrentRow.Cells[0].Value;
            if (itemNo != null)
            {
                string status = _frmITR.FindDocumentTable.CurrentRow.Cells[1].Value.ToString();
                ClearField(true);
                _frmITR.ITRTab.SelectedIndex = 0;
                _frmITR.BtnRequestText = "Update";
                LoadSAPDAta(Convert.ToInt32(itemNo));
                DisableControls();
                _frmITR.ITRTab.SelectedIndex = 0;
            }
        }

        public void ClearField(bool clearItems = true)
        {
            _frmITR.DocEntry1.Text = string.Empty;
            _frmITR.BpCode = string.Empty;
            _frmITR.Address = string.Empty;
            _frmITR.Company = string.Empty;
            _frmITR.TransferType = string.Empty;
            _frmITR.oSeries = string.Empty;
            _frmITR.DocNum = string.Empty;
            _frmITR.DocStatus = string.Empty;
            _frmITR.FrmWhsCode = string.Empty;
            _frmITR.ToWhsCode = string.Empty;
            _frmITR.SalesEmployee = string.Empty;
            _frmITR.sRemarks = string.Empty;
            _frmITR.TotalQuantity = string.Empty;
            _frmITR.Total = string.Empty;
            _frmITR.datePickerPostingDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            _frmITR.datePickerDueDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            _frmITR.datePickerDocDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            _frmITR.DocStatus = "Open";

            ChangeDocumentNumber();

            //PurchasingAP_generics.index = 0;

            foreach (DataGridViewRow udfRow in _frmITR.UDF.Rows)
            {
                try
                {
                    udfRow.Cells[2].Value = null;
                }
                catch { }
            }

            //foreach (Control ctrl in _frmITR.Udf.Controls)
            //{
            //    
            //    var type = ctrl.GetType();
            //    if (ctrl.GetType() == typeof(DateTimePicker))
            //    {
            //        _frmITR.Udf.Controls.Remove(ctrl);
            //    }
            //}
            _frmITR.UDF.Controls.Cast<Control>().Where(x => x.GetType() == typeof(DateTimePicker)).ToList().ForEach(x =>
            {

                _frmITR.UDF.Controls.Remove(x);
            });


            //var count = _frmITR.Udf.Controls.Count;

            //for (int i = 0; count > i; i++)
            //{
            //    try
            //    {
            //        var type = _frmITR.Udf.Controls[i].GetType();

            //        if (type == typeof(DateTimePicker))
            //        {
            //            _frmITR.Udf.Controls.Remove(_frmITR.Udf.Controls[i]);
            //        }
            //    }
            //    catch { }
            //}


            //if (clearItems)
            //{
            ClearList();
            //}

        }
        private void DisableControls()
        {
            _frmITR.buttonBtnRequest.Enabled = true;

            //Hide Selection
            _frmITR.buttonBPList.Visible = false;
            //On comment due to Issue Logs 101419
            //_frmITR.buttonToWhs.Visible = false;
            //_frmITR.buttonFrmWhs.Visible = false;


            _frmITR.comboboxSeries.Enabled = false;
            //disable DateTime Picker
            //dtDocDate.Enabled = true;
            //dtPostingDate.Enabled = true;
            //Mode
            //FindMode = false;
        }
        private void EnableControls()
        {
            _frmITR.buttonBtnRequest.Enabled = true;

            //Hide Selection
            _frmITR.buttonBPList.Visible = true;
            //pbPriceList.Visible = true;
            _frmITR.buttonToWhs.Visible = true;
            _frmITR.buttonFrmWhs.Visible = true;
            _frmITR.comboboxSeries.Enabled = true;

            //disable DateTime Picker
            //dtDocDate.Enabled = true;
            //dtPostingDate.Enabled = true;
            ////Mode
            //FindMode = false;
        }

        private void LoadSAPDAta(int DocEntry)
        {
            //FindMode = true;
            //DataTable
            DataTable dtItems;
            //DataTable dtHeader;
            DataTable dtChains;
            StringBuilder udf = new StringBuilder();
            foreach (DataGridViewRow row in _frmITR.UDF.Rows)
            {
                udf.Append($"A.{row.Cells[0].Value.ToString()},");
            }

            var query = $"Select (SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) [Series]" +
                ", A.DocNum" +
                ", A.CardCode " +
                ", A.CardName" +
                ", (Case when A.DocStatus = 'O' then 'Open' else 'Closed' end) [DocStatus]" +
                ", (select MailAddres from OCRD where CardCode = A.CardCode) [Address]" +
                ", A.U_Remarks" +
                ", A.U_DateDel" +
                ", (select ProjectCod from OCRD where CardCode = A.CardCode) [ProjectCod]" +
                ", A.Filler" +
                ", A.ToWhsCode" +
                ", A.DocDate" +
                ", A.TaxDate" +
                ", A.DocDueDate" +
                ", A.SlpCode " +
                $",{udf.ToString()}" +
                ", (select SlpName from OSLP where SlpCode = A.SlpCode) [SlpName]" +
                ", A.U_TransferType, " +
                ", (select Name from [@CMP_INFO] where Code = A.U_CompanyTIN) [U_CompanyTIN] " +
                ", A.Comments " +
                $" FROM OWTQ A WHERE A.DocEntry = {DocEntry}";
            string _bpCode, _docNum, _status, _remarks, _whscode, _twhscode, _datedel, taxdate, docdate, docduedate, docStatus;

            DataTable headerDetails = GetData(query);

            _bpCode = DECLARE.dtNull(headerDetails, 0, "CardCode", "");
            LoadBPDetails(_bpCode);
            _docNum = DECLARE.dtNull(headerDetails, 0, "DocNum", "0");
            _status = DECLARE.dtNull(headerDetails, 0, "DocStatus", "");
            //_remarks = DECLARE.dtNull(headerDetails, 0, "U_Remarks", "");
            _remarks = DECLARE.dtNull(headerDetails, 0, "Comments", "");
            _whscode = DECLARE.dtNull(headerDetails, 0, "Filler", "");
            _twhscode = DECLARE.dtNull(headerDetails, 0, "ToWhsCode", "");
            _datedel = DECLARE.dtNull(headerDetails, 0, "U_DateDel", "");

            taxdate = DECLARE.dtNull(headerDetails, 0, "TaxDate", "");
            docdate = DECLARE.dtNull(headerDetails, 0, "DocDate", "");
            docduedate = DECLARE.dtNull(headerDetails, 0, "DocDueDate", "");
            docStatus = DECLARE.dtNull(headerDetails, 0, "DocStatus", "");

            _frmITR.BpCode = _bpCode;
            _frmITR.BpName = DECLARE.dtNull(headerDetails, 0, "CardName", "");
            _frmITR.Address = DECLARE.dtNull(headerDetails, 0, "Address", "");
            _frmITR.sRemarks = _remarks;
            _frmITR.DocNum = _docNum;
            _frmITR.DocEntry1.Text = DocEntry.ToString();

            _frmITR.comboboxTransferType.Text = DECLARE.dtNull(headerDetails, 0, "U_TransferType", "");


            //_frmITR.comboboxCompany.SelectedIndex = _frmITR.comboboxCompany.FindString(DECLARE.dtNull(headerDetails, 0, "U_CompanyTIN", ""));
            _frmITR.comboboxCompany.Text = DECLARE.dtNull(headerDetails, 0, "U_CompanyTIN", "");
            //_frmITR.comboboxCompany.SelectedValue = DECLARE.dtNull(headerDetails, 0, "U_CompanyTIN", "");
            _frmITR.Company = Convert.ToString(_frmITR.comboboxCompany.SelectedValue);

            _frmITR.FrmWhsCode = _whscode;
            _frmITR.ToWhsCode = _twhscode;
            FrmInventoryTransferRequest.oFWhsCode = _whscode;
            FrmInventoryTransferRequest.oTWhsCode = _twhscode;
            _frmITR.oProject = DECLARE.dtNull(headerDetails, 0, "ProjectCod", "");
            _frmITR.comboboxSeries.Text = DECLARE.dtNull(headerDetails, 0, "Series", "");

            _frmITR.SalesEmployee = DECLARE.dtNull(headerDetails, 0, "SlpName", "");


            _frmITR.datePickerPostingDate.Format = DateTimePickerFormat.Short;
            _frmITR.datePickerDocDate.Format = DateTimePickerFormat.Short;
            _frmITR.datePickerDueDate.Format = DateTimePickerFormat.Short;

            _frmITR.datePickerPostingDate.Text = docdate;
            _frmITR.datePickerDocDate.Text = taxdate;
            _frmITR.datePickerDueDate.Text = docduedate;
            _frmITR.DocStatus = docStatus;


            try
            {
                _frmITR.UDF.Rows.Cast<DataGridViewRow>().ToList()
               .ForEach(x =>
               {
                   var fieldName = x.Cells[0].Value.ToString();
                   if (fieldName.ToUpper().Contains("DATE"))
                   {
                       string date = DECLARE.dtNull(headerDetails, 0, fieldName, "");
                       if (date != "")
                       {
                           DateTimePicker oDateTimePicker = new DateTimePicker();
                           oDateTimePicker.Value = Convert.ToDateTime(date);
                           _frmITR.UDF[2, x.Index].Value = date;
                           _frmITR.UDF.Controls.Add(oDateTimePicker);
                           CreateDateTimePicker(oDateTimePicker, _frmITR.UDF, x);
                       }
                   }
                   else
                   {
                       x.Cells[2].Value = DECLARE.dtNull(headerDetails, 0, fieldName, "");
                   }
               });
            }
            catch (Exception ex) { StaticHelper._MainForm.ShowMessage(ex.Message, true); }

            //LoadBPDetails(_bpCode);


            var queryItems = "SELECT " +
                              " T0.LineNum" +
                              ", T0.ItemCode" +
                              ", T0.Dscription" +
                              ", T0.Quantity [Quantity]" +
                              ", T0.Price" +
                              ", T0.DiscPrcnt" +
                              ", ((T1.OnHand + T1.OnOrder) -  T1.IsCommited) [Available]" +
                              ", T0.LineTotal" +
                              ", T0.WhsCode" +
                              ", T0.FromWhsCod " +
                              ", T0.SlpCode" +
                              ", T0.PriceBefDi" +
                              ", T0.Project" +
                              ", T0.VatGroup" +
                              ", T0.VatPrcnt" +
                              ", T1.CodeBars" +
                              ", T0.PriceAfVAT" +
                              ", T0.TaxCode" +
                              ", T0.VatAppld" +
                              ", T0.LineVat" +
                              ", T1.U_ID001 [BrandCode] " +
                              ", (SELECT Name FROM [@OBND] WHERE Code = T1.U_ID001) [BrandName] " +
                              ", T1.U_ID012 [U_StyleCode]" +
                              ", (SELECT Name FROM [@PRSTYLE] WHERE Code = T1.U_ID012) [StyleName] " +
                              ", (SELECT U_Code FROM [@OCLR] Where U_Color = T1.U_ID011 and Code = T1.U_ID022) [U_Color]" +
                              ", T1.U_ID011 [ColorName] " +
                              ", T1.U_ID018 [U_Section]" +
                              ", T1.U_ID007 [U_Size]" +
                              ", T1.U_ID023 " +
                              ", T0.U_Chain " +
                              ", T0.U_ChainDescription " +
                              ", (select Name from [@CMP_INFO] where Code = T0.U_Company) [U_Company] " +
                              ", T1.InvntryUom " +
                              " FROM WTQ1 T0 INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode " +
                              " Where T0.DocEntry = '" + DocEntry + "' ORDER BY T0.LineNum";

            //LINE ITEMS

            var Chain = "";
            var ChainDesc = "";

            if (!String.IsNullOrEmpty(_bpCode.ToString()))
            {
                var queryChain = "Select" +
                        " T0.GroupCode" +
                       ",T1.GroupName" +
                        " FROM OCRD T0 INNER JOIN OCRG T1 ON T0.GroupCode = T1.GroupCode" +
                        " where CardCode ='" + _bpCode + "'";

                dtChains = sapHanaAccess.Get(queryChain);

                if (dtChains.Rows.Count > 0)
                {
                    Chain = dtChains.Rows[0]["GroupCode"].ToString();
                    ChainDesc = dtChains.Rows[0]["GroupName"].ToString();
                }

            }
            dtItems = GetData(queryItems);

            for (int x = 0; x < dtItems.Rows.Count; x++)
            {
                Double LineTotal;
                Double GrossTotal;
                Double VatAmount;
                Double GrossPrice;
                Double PriceVatInc;
                Double DiscAmt;
                Double Discount;

                //linenum = Convert.ToInt16(dtItems.Rows[x]["Line Number"]);

                double qty = Convert.ToDouble(dtItems.Rows[x]["Quantity"]);
                double discount = 0;
                double price = Convert.ToDouble(dtItems.Rows[x]["Price"]);
                double vatrate = Convert.ToDouble(dtItems.Rows[x]["VatPrcnt"]);
                double priceaftvat = 0;
                double pricebefdisc = 0;


                PriceVatInc = price + (price * (vatrate / 100));
                DiscAmt = pricebefdisc * (discount / 100);
                LineTotal = (qty * price);
                //VatAmount = LineTotal * (vatrate / 100);

                VatAmount = LineTotal * (vatrate / 100) - ((LineTotal * (vatrate / 100)) * (discount / 100));

                GrossTotal = (LineTotal + VatAmount) - (DiscAmt * qty);
                GrossPrice = PriceVatInc - (PriceVatInc * (discount / 100));

                Discount = priceaftvat / (1 - (discount / 100));

                InventoryTransferReqItemsModel.ITRitems.Add(new InventoryTransferReqItemsModel.ITRItemsData
                {
                    Linenum = Convert.ToInt32(dtItems.Rows[x]["LineNum"]),
                    ObjType = FrmInventoryTransferRequest.objType,
                    ItemCode = dtItems.Rows[x]["ItemCode"].ToString(), // ItemCode
                    ItemName = dtItems.Rows[x]["Dscription"].ToString(), // ItemCode
                    BarCode = dtItems.Rows[x]["CodeBars"].ToString(),
                    Chain = string.IsNullOrEmpty(dtItems.Rows[x]["U_Chain"].ToString()) ? "" : dtItems.Rows[x]["U_Chain"].ToString(),
                    ChainDescription = string.IsNullOrEmpty(dtItems.Rows[x]["U_ChainDescription"].ToString()) ? "" : dtItems.Rows[x]["U_ChainDescription"].ToString(),
                    //On comment for Inner Carton Qty 091719
                    //BrandCode = dtItems.Rows[x]["BrandCode"].ToString(),

                    BrandCode = dtItems.Rows[x]["Quantity"].ToString(),
                    Brand = dtItems.Rows[x]["BrandName"].ToString(),
                    StyleCode = dtItems.Rows[x]["U_StyleCode"].ToString(), //Style
                    Style = dtItems.Rows[x]["U_StyleCode"].ToString(),
                    ColorCode = dtItems.Rows[x]["U_Color"].ToString(), //Color
                    Color = dtItems.Rows[x]["ColorName"].ToString(), //Color
                    Size = dtItems.Rows[x]["U_Size"].ToString(), //Size
                    Section = dtItems.Rows[x]["U_Section"].ToString(), //Section
                    Quantity = Convert.ToDouble(dtItems.Rows[x]["Quantity"]), //Requested to remove logic 07212021 //GetOrigQty(dtItems.Rows[x]["ItemCode"].ToString(), Convert.ToDouble(dtItems.Rows[x]["Quantity"])), //Qty
                    FWhsCode = dtItems.Rows[x]["FromWhsCod"].ToString(), //WHsCode
                    TWhsCode = dtItems.Rows[x]["WhsCode"].ToString(), //WHsCode
                    TaxCode = dtItems.Rows[x]["VatGroup"].ToString(), //Tax Code
                    TaxAmount = VatAmount,
                    TaxRate = Convert.ToDouble(vatrate),
                    UnitPrice = Convert.ToDouble(dtItems.Rows[x]["Price"]), //Convert.ToDouble(dtGRPO.Rows[x]["PriceBefDi"]),
                    LineTotal = LineTotal,
                    GrossTotal = GrossTotal, // Convert.ToDouble(dtGRPO.Rows[x]["PriceAfVat"]),
                    DiscountPerc = 0,
                    DiscountAmount = DiscAmt * qty,
                    GrossPrice = Convert.ToDouble(dtItems.Rows[x]["Price"]), //GrossPrice
                    Available = Convert.ToDouble(dtItems.Rows[x]["Available"]),
                    SortCode = dtItems.Rows[x]["U_ID023"].ToString(),
                    Company = dtItems.Rows[x]["U_Company"].ToString(),
                    InventoryUOM = dtItems.Rows[x]["InvntryUom"].ToString()
                    //,Selected = false
                });
            }

            LoadData(_frmITR.table);
            LoadData(_frmITR.ItemPreview);

            //DisableControls();
        }
        #region search document
        private void SearchDocumentTextChange(object sender, EventArgs e)
        {
            DataHelper.RowSearch(_frmITR.FindDocumentTable, _frmITR.txtSearchDocument.Text, FrmInventoryTransferRequest.oColumnIndex);

        }
        #endregion

        public void PrintDocumentChange(object sender, EventArgs e)
        {
            try
            {
                ReportDocument cryRpt = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();

                //string path = $"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\ITR\\{_frmITR.PrintDocNo}";
                var _settingsService = new SettingsService();

                string path = $"{_settingsService.GetReportPath()}Inventory\\{_frmITR.PrintDocNo}";
                cryRpt.Load(path);
                if (!_frmITR.PrintDocNo.Contains("Batch Printing"))
                {
                    cryRpt.SetParameterValue("DocKey@", _frmITR.DocEntry1.Text);
                }
                cryRpt.SetParameterValue("UserCode@", sboCredentials.UserId);
                //string constring = $"DRIVER=HDBODBC32;SERVERNODE=HANASERVERNBFI:30015;DATABASE={sboCredentials.Database}";

                //crConnectionInfo.IntegratedSecurity = false;

                //#############################################################################
                //Added by Cedi 070119
                var logonProperties = new DbConnectionAttributes();
                var sboCred = new SboCredentials();
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

                _frmITR.crystalReportViewer.ReportSource = cryRpt;
                _frmITR.crystalReportViewer.Refresh();
                //cryRpt.Close();
                //cryRpt.Dispose();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        public void NewDocument(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Creating new document may clear all the content. Do you wish to continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ClearData();
                _frmITR.ITRTab.SelectedIndex = 0;
            }
        }

        public void CloseDocuent(object sender, EventArgs e)
        {
            ClearData();
        }


        #region Checking

        private Boolean PreChecking()
        {
            bool result = false;

            if (_frmITR.comboboxTransferType.Text != "")
            {
                if (InventoryTransferReqItemsModel.ITRitems.Where(x => x.ObjType == FrmInventoryTransferRequest.objType).ToList().Count > 0)
                {
                    Int32 fwhscnt = 0;
                    Int32 twhscnt = 0;
                    int count = InventoryTransferReqItemsModel.ITRitems.Where(x => x.ObjType == FrmInventoryTransferRequest.objType).ToList().Count;
                    for (int x = 0; x < count; x++)
                    {
                        if (_frmITR.table.Rows[x].Cells[13].Value == null)
                        {
                            fwhscnt++;
                        }
                        if (_frmITR.table.Rows[x].Cells[15].Value == null)
                        {
                            twhscnt++;
                        }
                    }
                    //foreach (DataGridViewRow row in _frmITR.table.Rows)
                    //{
                    //    if (row.Cells[13].Value == null)
                    //    {
                    //        fwhscnt++;
                    //    }
                    //    if (row.Cells[15].Value == null)
                    //    {
                    //        twhscnt++;
                    //    }
                    //}

                    if (fwhscnt == 0)
                    {
                        if (twhscnt == 0)
                        {
                            result = true;
                        }
                        else
                        {
                            StaticHelper._MainForm.ShowMessage("Please fill in the To Warehouse per line.", true);

                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Please fill in the From Warehouse per line.", true);
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Please select item(s) to be transferred.", true);
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Please select a Transfer Type before adding.", true);
            }

            return result;
        }

        private Boolean CheckRequired()
        {
            bool result = true;

            if (GetData($"select U_BPRequired from [@TRANSFER_TYPE] where Code = '{_frmITR.comboboxTransferType.Text}'").Rows[0]["U_BPRequired"].ToString() == "Y" && _frmITR.BpCode == "")
            {
                result = false;
                StaticHelper._MainForm.ShowMessage("Please select a Business Partner before adding.", true);
            }
            else if (CheckReqCondition("U_ReqRequestBy", "U_ReqBy", "Requested By") == false)
            {
                result = false;
            }
            else if (CheckReqCondition("U_ReqAppBy", "U_AppBy", "Approved By") == false)
            {
                result = false;
            }
            else if (CheckReqCondition("U_ReqRecBy", "U_ReceivedBy", "Received By") == false)
            {
                result = false;
            }
            else if (CheckReqCondition("U_ReqVehicleDesc", "U_VDesc", "Vehicle Description") == false)
            {
                result = false;
            }
            else if (CheckReqCondition("U_ReqVehiclePlate", "U_VPla", "Vehicle Plate No.") == false)
            {
                result = false;
            }
            else if (CheckReqCondition("U_ReqDriver", "U_Driver", "Driver") == false)
            {
                result = false;
            }
            else if (CheckReqCondition("U_ReqHelper1", "U_Helper1", "Helper 1") == false)
            {
                result = false;
            }
            //else if (ITRm.SelValue("frmWhs2", cbTransferType.Text, txtBpCode.Text, txtAddress.Text, txtFWhsCode.Text) != txtFWhsCode.Text)
            //{
            //    result = false;
            //    StaticHelper._MainForm.ShowMessage($"Default From Warehouse for Transfer Type {cbTransferType.Text} does not match.", true);
            //}
            //else if (ITRm.SelValue("toWhs2", cbTransferType.Text, txtBpCode.Text, txtAddress.Text, txtTWhsCode.Text) != txtTWhsCode.Text)
            //{
            //    result = false;
            //    StaticHelper._MainForm.ShowMessage($"Default To Warehouse for Transfer Type {cbTransferType.Text} does not match.", true);
            //}

            return result;
        }

        private Boolean CheckReqWhs(string fieldname, int cellnum, string whsvalue)
        {
            bool result = false;
            bool boolBlockDiffWhs = false;
            int intCntDiffWhs = 0;

            if (GetData($"select {fieldname} from [@TRANSFER_TYPE] where Name = '{_frmITR.comboboxTransferType.Text}'").Rows[0][fieldname].ToString() == "B")
            {
                boolBlockDiffWhs = true;
            }
            else { boolBlockDiffWhs = false; }

            int count = InventoryTransferReqItemsModel.ITRitems.Where(x => x.ObjType == FrmInventoryTransferRequest.objType).ToList().Count;
            for (int x = 0; x < count; x++)
            {
                if (_frmITR.table.Rows[x].Cells[cellnum].Value.ToString() != whsvalue)
                {
                    intCntDiffWhs++;
                }
            }

            //foreach (DataGridViewRow row in _frmITR.table.Rows)
            //{
            //    if (row.Cells[cellnum].Value.ToString() != whsvalue)
            //    {
            //        intCntDiffWhs++;
            //    }
            //}

            if (boolBlockDiffWhs == true)
            {
                if (intCntDiffWhs == 0)
                {
                    result = true;
                }
            }
            else
            {
                if (intCntDiffWhs != 0 || intCntDiffWhs == 0)
                {
                    result = true;
                }
            }

            return result;
        }

        private Boolean CheckReqCondition(string FieldName, string FieldCode, string UDFName)
        {
            bool result = true;

            if (GetData($"select {FieldName} from [@TRANSFER_TYPE] where Code = '{_frmITR.comboboxTransferType.Text}'").Rows[0][FieldName].ToString() == "Y")
            {
                if (DECLARE.udf.Exists(x => x.FieldCode == FieldCode) == false)
                {
                    result = false;
                    //StaticHelper._MainForm.ShowMessage($"Please insert a value in the {UDFName} before adding.", true);
                    StaticHelper._MainForm.ShowMessage($"{UDFName} is required for the selected transfer type.", true);
                }
                else
                {
                    if (DECLARE.udf.Where(x => x.FieldCode == FieldCode).First().FieldValue.ToString() == "")
                    {
                        result = false;
                        StaticHelper._MainForm.ShowMessage($"{UDFName} is required for the selected transfer type.", true);
                    }
                }
            }

            return result;
        }
        #endregion

        void CloseForm(object sender, FormClosingEventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ClearData();


                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        void InventoryCopy(object sender, PreviewKeyDownEventArgs e)
        {
            var table = _frmITR.ItemPreview.Focus() ? _frmITR.ItemPreview : _frmITR.table;

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            {
                string clipBoard = Clipboard.GetText();

                foreach (DataGridViewCell cell in table.SelectedCells)
                {
                    cell.Value = clipBoard;

                    ComputeTotal();
                }
            }
        }


        public string SelectUDFFMS(string table, string AliasID)
        {
            string result = "";

            //string GETitrQry = _queryRepository.GetMaintenanceLineQuery(1, "UDF", "OWTQ");
            //DataTable dtITRqry = GetData(GETitrQry);
            //string fields = GetColumn(0, dtITRqry);
            //string GetUDFListqry = _queryRepository.GetUdfQueryWOorderBy("OWTQ");
            //GetUDFListqry += $" AND AliasID = '{AliasID.Replace("U_", "")}'";
            //string sFieldID = dataHelper.ReadDataRow(sapHanaAccess.Get(GetUDFListqry), "FieldID", "", 0);

            List<string> parameters = new List<string>()
            {
                "1250000940",
                AliasID
                //sFieldID.Replace("U_","")
            };

            var list = Modal("GetUDF_FMS", parameters, "");
            if (list.Count > 0)
            {
                result = list[0];
            }
            return result;
        }

        public string GetITRqry(string AliasID)
        {
            //string GETitrQry = _queryRepository.GetMaintenanceLineQuery(1, "UDF", "OWTQ");
            //DataTable dtITRqry = GetData(GETitrQry);
            //string fields = GetColumn(0, dtITRqry);
            //string GetUDFListqry = _queryRepository.GetUdfQueryWOorderBy("OWTQ");
            //GetUDFListqry += $" AND AliasID = '{AliasID.Replace("U_", "")}'";
            //string sFieldID = dataHelper.ReadDataRow(sapHanaAccess.Get(GetUDFListqry), "FieldID", "", 0);


            var GetUDF_FMS = sapHanaAccess.Get(SP.UDF_FMS);
            string ItrUDF_FmsQry = dataHelper.ReadDataRow(GetUDF_FMS, 1, "", 0);
            //string query = sapHanaAccess.Get(string.Format(ItrUDF_FmsQry, "1250000940", sFieldID)).Rows[0]["QString"].ToString();
            string query = sapHanaAccess.Get(string.Format(ItrUDF_FmsQry, "1250000940", AliasID)).Rows[0]["QString"].ToString();

            return query;
        }

        public static double GetCartonQty(string sItemCode, double OrigQty)
        {
            //===============================CartonQty==============================
            double dOrigQty = OrigQty;
            double Qty = 0;

            SAPHanaAccess sapHanaAccess = new SAPHanaAccess();
            DataTable dt = sapHanaAccess.Get($"select ItemCode, isnull(OrdrMulti, 0) [OrdrMulti] from OITM where ItemCode = '{sItemCode}'");

            if (dt.Rows.Count > 0)
            {
                double dOrderMQty = Convert.ToDouble(dt.Rows[0]["OrdrMulti"].ToString());

                if (dOrderMQty > 0)
                {
                    for (double a = 0; dOrigQty > a; a++)
                    {
                        Qty += dOrderMQty;
                    }
                }
                else
                {
                    Qty = dOrigQty;
                }
            }
            //===============================CartonQty==============================

            return Qty;
        }

        public static double GetOrigQty(string sItemCode, double CartonQty)
        {
            //===============================CartonQty==============================
            double dCartQty = CartonQty;
            double Qty = 0;

            SAPHanaAccess sapHanaAccess = new SAPHanaAccess();
            DataTable dt = sapHanaAccess.Get($"select ItemCode, isnull(OrdrMulti, 0) [OrdrMulti] from OITM where ItemCode = '{sItemCode}'");

            if (dt.Rows.Count > 0)
            {
                double dOrderMQty = Convert.ToDouble(dt.Rows[0]["OrdrMulti"].ToString());

                Qty = dOrderMQty > 0 ? dCartQty / dOrderMQty : dCartQty;
            }
            //===============================CartonQty==============================

            return Qty;
        }
    }
}

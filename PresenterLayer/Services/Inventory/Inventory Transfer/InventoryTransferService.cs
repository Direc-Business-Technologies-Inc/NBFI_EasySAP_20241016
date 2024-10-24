using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DirecLayer;
using DomainLayer;
using DomainLayer.Helper;
using DomainLayer.Models;
using InfrastructureLayer.InventoryRepository;
using MetroFramework;
using PresenterLayer.Helper;
using PresenterLayer.Views.Inventory.Inventory_Transfer;
using PresenterLayer.Views.Main;
using PresenterLayer.Views.Tools;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using zDeclare;
using PresenterLayer.Services.Security;
using Unity.Storage;
using System.Reflection;
using System.Security.Claims;
using System.Web.UI.WebControls;
using DirecLayer._02_Form.MVP.Views;

namespace PresenterLayer.Services.Inventory.Inventory_Transfer
{
    public class InventoryTransferService
    {
        IInventoryTransfer _frmIT;
        //private readonly IInventoryTransfer _View;
        IQueryRepository _queryRepository;
        SAPHanaAccess sapHanaAccess = new SAPHanaAccess();
        DirectoryAccess directoryAccess = new DirectoryAccess();
        ServiceLayerAccess serviceLayerAccess = new ServiceLayerAccess();
        MainForm mainForm = new MainForm();
        ITmaintenance ITm = new ITmaintenance();
        DataHelper dataHelper = new DataHelper();
        SboCredentials sboCredentials = new SboCredentials();

        private static DateTimePicker oDateTimePicker = new DateTimePicker();

        //private System.Windows.Forms.DataGridView DgvItem;
        public InventoryTransferService(IInventoryTransfer frmIT, IQueryRepository queryRepository)
        {
            _frmIT = frmIT;
            _queryRepository = queryRepository;
            EventSubscription();

        }

        private void EventSubscription()
        {
            _frmIT.InventoryTransferLoad += new EventHandler(LoadData);
            _frmIT.InventoryTransferBpClick += new EventHandler(GetBPInformation);
            _frmIT.InventoryTransferFrmWhsClick += new EventHandler(GetFrmWhsInformation);
            _frmIT.InventoryTransferToWhsClick += new EventHandler(GetToWhsInformation);
            _frmIT.InventoryTransferSalesEmployeeClick += new EventHandler(GetSalesEmployeeInformation);
            _frmIT.InventoryTransferSeriesChange += new EventHandler(SeriesChange);
            _frmIT.InventoryTransferCompanyChange += new EventHandler(CompanyChange);
            _frmIT.InventoryTransferTransferTypeChange += new EventHandler(TransferTypeChange);
            _frmIT.InventoryTransferFromWhsChange += new EventHandler(FrmWhsChange);
            _frmIT.InventoryTransferToWhsChange += new EventHandler(ToWhsChange);
            _frmIT.InventoryTransferAddItemClick += new EventHandler(SearchItem);
            _frmIT.InventoryTransferFindDocument += new EventHandler(FindDocument);
            _frmIT.InventoryTransferChooseDocument += new EventHandler(ChooseDocument);
            _frmIT.InventoryTransferPostClick += new EventHandler(Post);
            _frmIT.InventoryTransferCopyFromChange += new EventHandler(DocListChange);
            _frmIT.InventoryTransferDeleteRowClick += new EventHandler(DeleteRow);
            _frmIT.InventoryTransferCloseForm += new EventHandler(CloseDocument);
            _frmIT.InventoryTransferNewDocument += new EventHandler(NewDocument);
            _frmIT.InventoryTransferSearchTextChange += new EventHandler(PreviewSearch);
            _frmIT.InventoryTransferSearchDocumentTextChange += new EventHandler(SearchDocumentTextChange);
            _frmIT.InventoryTransferPrintPreviewFromChange += new EventHandler(PrintDocumentChange);

            _frmIT.InventoryTransferCellClick += new DataGridViewCellEventHandler(LoadFields);

            _frmIT.InventoryTransferItemCellClick += new DataGridViewCellEventHandler(ItemCellClick);

            _frmIT.InventoryTransferChainCellClick += new DataGridViewCellEventHandler(ItemChainCellClick);


            _frmIT.InventoryTransferItemCellEndEdit += new DataGridViewCellEventHandler(CellEndEdit);

            _frmIT.InventoryTransferCloseFormEvent += new FormClosingEventHandler(CloseForm);

            _frmIT.InventoryCopy += new PreviewKeyDownEventHandler(InventoryCopy);
            _frmIT.InventoryUDFscroll += new ScrollEventHandler(InventoryUDFscroll);
        }

        #region loading of header

        private void LoadData(object sender, EventArgs e)
        {
            LoadCompany();
            LoadTransferType();
            LoadSeries();
            _frmIT.PrintPreviewItems(GetDocumentCrystalForms());
            ChangeDocumentNumber();
            InventoryTransferHeaderModel.selObjType = InventoryTransfer.objType1;
            Udf_Load();
        }


        public void LoadCompany()
        {
            string query = "";
            DataTable queryResult = new DataTable();
            query = _queryRepository.GetCompanyQuery();
            queryResult = sapHanaAccess.Get(query);
            _frmIT.showCompany(queryResult);
        }
        public void LoadTransferType()
        {
            string query = "";
            DataTable queryResult = new DataTable();
            query = _queryRepository.GetTransferTypeQuery();
            queryResult = sapHanaAccess.Get(query);
            _frmIT.showTransferType(queryResult);
        }
        public void LoadSeries()
        {
            string query = "";
            DataTable queryResult = new DataTable();
            query = _queryRepository.GetSeriesQuery("67");
            queryResult = sapHanaAccess.Get(query);
            _frmIT.showSeries(queryResult);
        }

        public void ChangeDocumentNumber()
        {
            string result = "";
            string query = _queryRepository.GetDocNumQuery("67", _frmIT.series);
            DataTable dt = sapHanaAccess.Get(query);

            if (dt.Rows.Count > 0)
            {
                result = ValidateInput.String(dt.Rows[0]["NextNumber"]);
            }

            if (result != string.Empty)
            {
                _frmIT.txtDocNum.Text = result;
            }
        }
        #endregion

        #region UDF

        private void LoadFields(object sender, DataGridViewCellEventArgs e)
        {
            string fieldName = _frmIT.UDF.CurrentRow.Cells[0].Value.ToString();
            var row = e.RowIndex;
            var col = e.ColumnIndex;
            string result = "";
            if (fieldName.Contains("date") || fieldName.Contains("Date"))
            {
                //_udfRepo.ConvertToDate(_frmIT.Udf);
                ConvertToDate(_frmIT.UDF);
                _frmIT.TxtITR_DocEntry.Focus();
                _frmIT.UDF.CurrentCell = _frmIT.UDF[col, row];
            }
            else if (fieldName.Contains("by") || fieldName.Contains("By"))
            {
                var code = SelectEmployee();
                //x.Cells[2].Value = m["Address2"];
                _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[0].Value != null)
                    {
                        if (x.Cells[0].Value.ToString() == fieldName)
                        {

                            x.Cells[2].Value = code;
                            _frmIT.TxtITR_DocEntry.Focus();
                            //_frmIT.UDF.CurrentCell = x.Cells[2];
                            //_frmIT.UDF.Rows[row].Cells[col];

                            _frmIT.UDF.CurrentCell = _frmIT.UDF[col, row];

                        }
                    }
                });

            }
            else if (fieldName.Contains("Carton") || fieldName.Contains("carton"))
            {
                var code = SelectCartonList();
                //x.Cells[2].Value = m["Address2"];
                _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[0].Value != null)
                    {
                        if (x.Cells[0].Value.ToString() == fieldName)
                        {

                            x.Cells[2].Value = code;
                            _frmIT.TxtITR_DocEntry.Focus();
                            //_frmIT.UDF.CurrentCell = x.Cells[2];
                            _frmIT.UDF.CurrentCell = _frmIT.UDF[col, row];

                        }
                    }
                });
            }

            else if (fieldName.Equals("U_VDesc"))
            {
                var code = SelectVehicle();
                _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[0].Value != null)
                    {
                        if (x.Cells[0].Value.ToString() == fieldName)
                        {

                            x.Cells[2].Value = code;
                            _frmIT.TxtITR_DocEntry.Focus();
                            //_frmIT.UDF.CurrentCell = x.Cells[2];
                            _frmIT.UDF.CurrentCell = _frmIT.UDF[col, row];

                        }
                    }
                });

                if (code != "")
                {
                    DataTable dt1 = sapHanaAccess.Get($"SELECT U_VPla, U_VDriver FROM [@TRUCK] WHERE U_VDesc = '{code}'");
                    _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList()
                    .ForEach(x =>
                    {
                        if (x.Cells[0].Value != null)
                        {
                            if (x.Cells[0].Value.ToString() == "U_VPla")
                            {

                                x.Cells[2].Value = dt1.Rows[0]["U_VPla"].ToString();
                                _frmIT.TxtITR_DocEntry.Focus();
                                //_frmIT.UDF.CurrentCell = x.Cells[2];
                                _frmIT.UDF.CurrentCell = _frmIT.UDF[col, row];

                            }
                            else if (x.Cells[0].Value.ToString() == "U_Driver")
                            {
                                x.Cells[2].Value = dt1.Rows[0]["U_VDriver"].ToString();
                                _frmIT.TxtITR_DocEntry.Focus();
                                //_frmIT.UDF.CurrentCell = x.Cells[2];
                                _frmIT.UDF.CurrentCell = _frmIT.UDF[col, row];
                            }
                        }
                    });

                }
            }
            else if (fieldName.Equals("U_AddID"))
            {
                var code = SelectAddress();

                _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[0].Value != null)
                    {
                        if (x.Cells[0].Value.ToString() == fieldName)
                        {

                            x.Cells[2].Value = code;
                            _frmIT.TxtITR_DocEntry.Focus();
                            //_frmIT.UDF.CurrentCell = x.Cells[2];
                            _frmIT.UDF.CurrentCell = _frmIT.UDF[col, row];

                        }
                    }
                });
            }

        }
        public void ConvertToDate(DataGridView dgv)
        {
            var row = dgv.CurrentRow;
            var curCell = dgv.CurrentCell.ColumnIndex;
            if (curCell == 2)
            {
                string fieldName = row.Cells[0].Value.ToString();

                if (fieldName.Contains("Date") || fieldName.Contains("date"))
                {
                    //DateTimePicker oDateTimePicker = new DateTimePicker();

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
                    //_frmIT.Udf = dgv;
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
        }
        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            var dtPicker = (DateTimePicker)sender;
            var dtPickerValue = dtPicker.Value.ToShortDateString();
            //    if (_frmIT.UDF.Rows[_frmIT.UDF.CurrentRow.Index].Cells[_frmIT.UDF.CurrentCell.ColumnIndex].Value.ToString().Contains("Date") || _frmIT.UDF.Rows[_frmIT.UDF.CurrentRow.Index].Cells[_frmIT.UDF.CurrentCell.ColumnIndex].Value.ToString().Contains("date"))
            //{
            //    _frmIT.UDF.Rows[_frmIT.UDF.CurrentRow.Index].Cells[2].Value = dtPickerValue;
            //    //_frmIT.UDF.CurrentCell.Value = dtPickerValue;
            //}
            _frmIT.UDF.CurrentCell.Value = dtPickerValue;
            oDateTimePicker.Visible = false;
        }
        public void Udf_Load()
        {
            DataTable udf = GetUDF();
            Style(_frmIT.UDF);
            LoadUdf(_frmIT.UDF, udf);
            //LoadData(_frmIT.UDF);
        }
        public DataTable GetUDF()
        {
            string query = UDF("OWTR");
            return sapHanaAccess.Get(query);
        }
        public string UDF(string table)
        {
            string query = _queryRepository.GetMaintenanceLineQuery(1, "UDF", table);
            DataTable result = sapHanaAccess.Get(query);

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

                    if (values.Rows.Count > 1)
                    {
                        cmb.DisplayMember = "Name";
                        cmb.ValueMember = "Code";
                        cmb.DataSource = values;

                        dgv.Rows[i].Cells["Field"] = cmb;
                    }
                    else
                    {
                        dgv.Rows[i].Cells["Field"] = new DataGridViewTextBoxCell();
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
            return sapHanaAccess.Get(query);
        }
        private DataTable GetUdfTableValues(string value)
        {
            string query = _queryRepository.GetUdfTableValuesQuery(value);
            return sapHanaAccess.Get(query);
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
                   _frmIT.TxtBpCode.Text
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
        #endregion

        #region bp list event
        internal void GetBPInformation(object sender, EventArgs e)
        {
            var m = SelectBP();

            if (m.Count > 0)
            {
                _frmIT.TxtBpCode.Text = m["CardCode"];
                _frmIT.TxtBpName.Text = m["CardName"];
                _frmIT.TxtAddress.Text = m["Address"];
                _frmIT.TxtSalesEmployee.Text = m["SalesEmployeeName"];
                _frmIT.OSalesEmployee = m["SalesEmployeeCode"];

                //string transtype = _repository.AutomateTransferType(_frmIT.SuppCode);
                //string orderNo = _repository.GetOrderNo(_frmIT.SuppCode);

                //var ITRUdf = _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList();
                //int temp = 0;
                //var temp2 = ITRUdf.Select(x => x.Cells).ToList();
                _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[1].Value != null)
                    {
                        if (x.Cells[1].Value.ToString() == "Address ID")
                        {
                            x.Cells[2].Value = m["Address2"];
                        }
                    }
                });

                InventoryTransferHeaderModel.oBPCode = _frmIT.TxtBpCode.Text;
                _frmIT.OCode = _frmIT.TxtBpCode.Text;
                if (_frmIT.OCode != null)
                {
                    LoadBPDetails(_frmIT.OCode);
                    InventoryTransferHeaderModel.oBPCode = _frmIT.OCode;
                }
                else
                {
                    _frmIT.OProject = "";
                    _frmIT.TxtBpName.Text = "";
                    _frmIT.TxtAddress.Text = "";
                }
            }
        }
        public Dictionary<string, string> SelectBP()
        {
            Dictionary<string, string> info = new Dictionary<string, string>();

            List<string> parameters = new List<string>()
            {
                "C"
            };

            List<string> m = Modal("OCRD", parameters, "List Of Business Partners");

            if (m.Count > 0)
            {
                string query = _queryRepository.BPinformationQuery(m[0]);
                DataTable dt = sapHanaAccess.Get(query);

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

        private void LoadBPDetails(string CardCode)
        {
            string query = "SELECT A.CardCode,A.CardName,A.MailAddres [Address]" +
                    ", (SELECT min(Y.Address) [Address] FROM CRD1 Y Where Y.CardCode = A.CardCode And AdresType = 'S') [Add2]" +
                    ", A.ProjectCod," +
                    "A.SlpCode," +
                    "A.ListNum," +
                    "A.GroupCode, " +
                    "(Select Z.SlpName FROM OSLP Z Where Z.SlpCode = A.SlpCode) [SlpName], " +
                    $"(SELECT min(Y.U_Whs) [U_Whs] FROM CRD1 Y Where Y.CardCode = A.CardCode And AdresType = 'S') [Whs]," +
                    $"ECVatGroup FROM OCRD A WHERE A.CardCode = '{CardCode}'";

            var dt = sapHanaAccess.Get(query);
            string strAddressID = DECLARE.dtNull(dt, 0, "Add2", "");
            InventoryTransferHeaderModel.oAddressID = strAddressID;
            string strGetAreaQry = $"SELECT A.U_Gloc FROM CRD1 A WHERE A.CardCode = '{CardCode}' AND A.Address = '{strAddressID}'";
            if (sapHanaAccess.Get(strGetAreaQry).Rows.Count > 0)
            {
                InventoryTransferHeaderModel.oArea = sapHanaAccess.Get(strGetAreaQry).Rows[0]["U_Gloc"].ToString();
            }

            _frmIT.OProject = DECLARE.dtNull(dt, 0, "ProjectCod", "");
        }
        #endregion

        #region Whs Event
        internal void GetFrmWhsInformation(object sender, EventArgs e)
        {
            var m = SelectWhs();
            if (m.Count > 0)
            {
                _frmIT.TxtFWhsCode.Text = m[0];
                foreach (var x in InventoryTransferItemsModel.ITitems.Where(x => x.ItemCode != "").ToList())
                {
                    x.FWhsCode = _frmIT.TxtFWhsCode.Text;
                }

                LoadData(_frmIT.dgvItem);
                LoadData(_frmIT.dgvPreviewItem);
            }
        }
        internal void GetToWhsInformation(object sender, EventArgs e)
        {
            var m = SelectWhs();
            if (m.Count > 0)
            {
                _frmIT.TxtTWhsCode.Text = m[0];
                foreach (var x in InventoryTransferItemsModel.ITitems.Where(x => x.ItemCode != "").ToList())
                {
                    x.TWhsCode = _frmIT.TxtTWhsCode.Text;
                }
                LoadData(_frmIT.dgvItem);
                LoadData(_frmIT.dgvPreviewItem);
            }
        }
        internal void FrmWhsChange(object sender, EventArgs e)
        {
            if (_frmIT.TxtFWhsCode.Text != "")
            {
                InventoryTransferHeaderModel.oWhsCode = _frmIT.TxtFWhsCode.Text;
            }
            foreach (var x in InventoryTransferItemsModel.ITitems.Where(x => x.ItemCode != "").ToList())
            {
                x.FWhsCode = _frmIT.TxtFWhsCode.Text;
            }
            LoadData(_frmIT.dgvItem);
            LoadData(_frmIT.dgvPreviewItem);

        }
        internal void ToWhsChange(object sender, EventArgs e)
        {
            InventoryTransferHeaderModel.oToWhsCode = _frmIT.TxtTWhsCode.Text;
            foreach (var x in InventoryTransferItemsModel.ITitems.Where(x => x.ItemCode != "").ToList())
            {
                x.TWhsCode = _frmIT.TxtTWhsCode.Text;
            }
            LoadData(_frmIT.dgvItem);
            LoadData(_frmIT.dgvPreviewItem);
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

        #region sales employee event
        internal void GetSalesEmployeeInformation(object sender, EventArgs e)
        {
            var m = SelectSalesEmployee();
            if (m.Count > 0)
            {
                _frmIT.TxtSalesEmployee.Text = m[1];
            }
        }
        public List<string> SelectSalesEmployee()
        {
            List<string> parameters = new List<string>();
            List<string> m = Modal("OSLP", parameters, "List Of Employees");
            return m;
        }
        #endregion

        #region Series event

        internal void SeriesChange(object sender, EventArgs e)
        {
            var sdata = $"SELECT T0.ObjectCode,T0.Series,T0.SeriesName,T0.NextNumber FROM NNM1 T0 Where T0.ObjectCode = 67 And SeriesName = '{_frmIT.CmbSeries.Text}'";

            var dt = sapHanaAccess.Get(sdata);

            _frmIT.OSeries = DataAccess.Search(dt, 0, "Series");
            ChangeDocumentNumber();

            string strSeries = dt.Rows[0]["SeriesName"].ToString();

            if (strSeries.Contains("NBFI"))
            {
                _frmIT.CbCompany.SelectedIndex = _frmIT.CbCompany.FindStringExact("NEW BARBIZON FASHION INC.");
            }
            else if (strSeries.Contains("CMC"))
            {
                _frmIT.CbCompany.SelectedIndex = _frmIT.CbCompany.FindStringExact("COTTON MOUNTAIN CORPORATION");
            }
            else if (strSeries.Contains("AC"))
            {
                _frmIT.CbCompany.SelectedIndex = _frmIT.CbCompany.FindStringExact("ACTIVESTYLE CORPORATION");
            }
        }


        #endregion

        #region company event
        void CompanyChange(object sender, EventArgs e)
        {
            try
            {
                int SelIndex = _frmIT.CbTransferType.FindStringExact("CST");
                if (_frmIT.CbTransferType.SelectedIndex == SelIndex)
                {
                    if (_frmIT.CbCompany.SelectedIndex == _frmIT.CbCompany.FindStringExact("NEW BARBIZON FASHION INC."))
                    {
                        _frmIT.CmbSeries.SelectedIndex = _frmIT.CmbSeries.FindStringExact("CST-NBFI");
                    }
                    else if (_frmIT.CbCompany.SelectedIndex == _frmIT.CbCompany.FindStringExact("COTTON MOUNTAIN CORPORATION"))
                    {
                        _frmIT.CmbSeries.SelectedIndex = _frmIT.CmbSeries.FindStringExact("CST-CMC");
                    }
                    else if (_frmIT.CbCompany.SelectedIndex == _frmIT.CbCompany.FindStringExact("ACTIVESTYLE CORPORATION"))
                    {
                        _frmIT.CmbSeries.SelectedIndex = _frmIT.CmbSeries.FindStringExact("CST-AC");
                    }
                }

                //Added for updating lines 090419
                if (_frmIT.CbTransferType.SelectedIndex > -1)
                {
                    foreach (var x in InventoryTransferItemsModel.ITitems.Where(x => x.ItemCode != "").ToList())
                    {
                        x.CompanyCode = _frmIT.CbCompany.SelectedValue.ToString();
                        x.Company = _frmIT.CbCompany.Text.ToString();
                    }

                    LoadData(_frmIT.dgvItem);
                    LoadData(_frmIT.dgvPreviewItem);
                }

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }
        #endregion

        #region transfertype event
        private void TransferTypeChange(object sender, EventArgs e)
        {
            if (_frmIT.btnRequest.Text == "Add")
            {
                try
                {
                    if (ITm.SelValue("series1", _frmIT.CbTransferType.Text) != "")
                    {
                        _frmIT.CmbSeries.SelectedIndex = _frmIT.CmbSeries.FindString(ITm.SelValue("series1", _frmIT.CbTransferType.Text));
                    }

                    int SelIndex = _frmIT.CbTransferType.FindStringExact("CST");
                    if (_frmIT.CbTransferType.SelectedIndex == SelIndex)
                    {
                        //GetCSTseries();
                        CompanyChange(null, null);
                    }

                    _frmIT.TxtFWhsCode.Clear();
                    _frmIT.TxtTWhsCode.Clear();

                    if (ITm.SelValue("frmWhs1", _frmIT.CbTransferType.Text, _frmIT.TxtBpCode.Text, InventoryTransferHeaderModel.oAddressID) != "")
                    {
                        InventoryTransferHeaderModel.oWhsCode = ITm.SelValue("frmWhs1", _frmIT.CbTransferType.Text, _frmIT.TxtBpCode.Text, InventoryTransferHeaderModel.oAddressID);
                        _frmIT.TxtFWhsCode.Text = InventoryTransferHeaderModel.oWhsCode;
                    }

                    if (ITm.SelValue("toWhs1", _frmIT.CbTransferType.Text, _frmIT.TxtBpCode.Text, InventoryTransferHeaderModel.oAddressID) != "")
                    {
                        _frmIT.TxtTWhsCode.Text = ITm.SelValue("toWhs1", _frmIT.CbTransferType.Text, _frmIT.TxtBpCode.Text, InventoryTransferHeaderModel.oAddressID);
                    }
                }
                catch (Exception ex)
                {
                    StaticHelper._MainForm.ShowMessage(ex.Message, true);
                }
            }
        }
        #endregion

        #region loading of items
        public void LoadData(DataGridView dgv, bool isFirstLoad = false)
        {

            string objType = InventoryTransferHeaderModel.selObjType;
            try
            {
                dgv.Columns.Clear();

                InventoryTransferItemsController.DataGridViewITData(dgv);

                int rowcnt = 0;
                foreach (var x in InventoryTransferItemsModel.ITitems.Where(x => x.ObjType == objType))
                {
                    double InfoPrice = x.GrossPrice;
                    object[] lineitem = { x.BarCode, x.ItemCode, x.ItemName, InfoPrice.ToString("#,#00.##"), x.Brand, x.StyleCode, x.Style
                                        , x.ColorCode, x.Color, x.Section, x.Size, x.Quantity, x.BrandCode, x.FWhsCode,"...", x.TWhsCode,"...", x.Company, "...", x.SKU, x.SortCode, x.InventoryUOM,
                                        x.BaseEntry, x.BaseLine,x.Chain,x.ChainDescription,"...",x.Linenum};

                    dgv.Rows.Add(lineitem);
                    AdditionalColumns(dgv, rowcnt, x.ItemCode, _frmIT.TxtBpCode.Text, InfoPrice);
                    rowcnt++;
                }

                InventoryTransferItemsController.dataGridLayout(dgv);
                //dgvItems.Sort(dgvItems.Columns["SortCode"], ListSortDirection.Ascending);

                // Computation For Total
                //DECLARE.ComputeTotal(dgvItems, null, out TotalBefDIsc, out TotalQty, out TotalDisc, out TotalAftDisc, out TotalTax);
                ComputeTotal();

            }
            catch (Exception ex)
            {
                //StaticHelper._MainForm.ShowMessage(ex.Message, true);
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }


        }
        private void ComputeTotal()
        {
            double dblTotalQty = 0;
            double dblTotalPrice = 0;

            foreach (DataGridViewRow row in _frmIT.dgvItem.Rows)
            {
                double dblQty = Convert.ToDouble(DECLARE.Replace(row, "Quantity", "0.00"));
                double dblPrice = Convert.ToDouble(DECLARE.Replace(row, "Info Price", "0.00"));

                dblTotalQty += dblQty;

                double dblPriceQty = (dblPrice * dblQty);
                dblTotalPrice += dblPriceQty;
            }

            _frmIT.TxtTotalQty.Text = dblTotalQty.ToString("#,#00");
            _frmIT.TxtTotal.Text = dblTotalPrice.ToString("#,#00.00");
        }
        private void AdditionalColumns(DataGridView dgv, int rowcnt, string oItemCode, string oBPCode, double oInfoPrice)
        {
            try
            {
                if (_frmIT.TxtBpCode.Text != "")
                {
                    string strSKU = " select distinct b.U_SKU [SKU]" +
                                        " from OCPN a " +
                                        " inner " +
                                        " join CPN1 c on a.CpnNo = c.CpnNo " +
                                        " inner join CPN2 b on a.CpnNo = b.CpnNo " +
                                        $" where c.BpCode = '{_frmIT.TxtBpCode.Text}' " +
                                        $" and a.U_CType = 'SKU' and b.U_SKU is not null and b.ItemCode = '{oItemCode}' " +
                                        " order by SKU asc";

                    //On comment due to Pooling issue 083019
                    //string strSKU = $@"CALL SKULOOP('{oItemCode}','{oBPCode}','{oInfoPrice}',(Select count(*) from ""@OSKV"" x inner join ""@SKV1"" y on x.""Code"" = y.""Code"" where x.""Code"" = (Select yy.""SeriesName"" from OCRD xx inner join NNM1 yy on xx.""Series"" = yy.""Series"" where xx.""CardCode"" = '{oBPCode}'))," +
                    //   $@"(Select yy.""SeriesName"" from OCRD xx inner join NNM1 yy on xx.""Series"" = yy.""Series"" where xx.""CardCode"" = '{oBPCode}'))";

                    DataTable dt1 = sapHanaAccess.Get(strSKU);


                    if (dt1.Rows.Count > 0)
                    {
                        dgv.Rows[rowcnt].Cells["SKU"].Value = dt1.Rows[0][0].ToString();
                    }
                    else
                    {
                        dgv.Rows[rowcnt].Cells["SKU"].Value = "";
                    }

                }

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                //StaticHelper._StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }
        #endregion

        #region Item add Event
        private void SearchItem(object sender, EventArgs e)
        {

            InventoryTransferHeaderModel.oWhsCode = _frmIT.TxtFWhsCode.Text;
            InventoryTransferHeaderModel.oToWhsCode = _frmIT.TxtTWhsCode.Text;
			InventoryTransferHeaderModel.oSeries = _frmIT.OSeries;
			InventoryTransferHeaderModel.CardCode = _frmIT.TxtBpCode.Text;
			//InventoryTransferHeaderModel.oTransferType = _frmIT.TransferType;
			if (_frmIT.TxtFWhsCode.Text != "")
            {
                FrmTransferItemList form = new FrmTransferItemList()
                {
                    IsCartonActive = false,
                    oBpCode = _frmIT.TxtBpCode.Text,
                    oBpName = _frmIT.TxtBpName.Text,
                    oWhsCode = _frmIT.TxtFWhsCode.Text
                };

                form.ShowDialog();

                LoadData(_frmIT.dgvItem);
                LoadData(_frmIT.dgvPreviewItem);
            }
            else
            {
                //StaticHelper._MainForm.ShowMessage("Please select a from warehouse before adding.", true);
                StaticHelper._MainForm.ShowMessage("Please Select a from warehouse before adding", true);
            }
        }
        #endregion

        #region item cell event



        internal void ItemCellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_frmIT.btnRequest.Text == "Add")
                {
                    string oCode = "";
                    string oName = "";
                    var col = e.ColumnIndex;
                    var row = e.RowIndex;
                    bool isEdited = false;
                    if (e.ColumnIndex == 14 && e.RowIndex >= 0)
                    {
                        var warehouse = SelectWhs();
                        if (warehouse.Count > 0)
                        {
                            oCode = warehouse[0];
                        }
                        _frmIT.dgvItem.Rows[row].Cells[13].Value = oCode;
                        foreach (var x in InventoryTransferItemsModel.ITitems.Where(x => x.ItemCode == _frmIT.dgvItem.Rows[row].Cells[1].Value.ToString()).ToList())
                        {
                            x.FWhsCode = oCode;
                        }
                        isEdited = true;
                    }

                    if (e.ColumnIndex == 16 && e.RowIndex >= 0)
                    {
                        var warehouse = SelectWhs();
                        if (warehouse.Count > 0)
                        {
                            oCode = warehouse[0];
                        }
                        _frmIT.dgvItem.Rows[row].Cells[15].Value = oCode;
                        foreach (var x in InventoryTransferItemsModel.ITitems.Where(x => x.ItemCode == _frmIT.dgvItem.Rows[row].Cells[1].Value.ToString()).ToList())
                        {
                            x.TWhsCode = oCode;
                        }
                        isEdited = true;
                    }

                    if (e.ColumnIndex == 18 && e.RowIndex >= 0)
                    {
                        //ViewList("@CMP_INFO", out oCode, out oName, "List of Companies");
                        var companies = SelectCompany();
                        if (companies.Count > 1)
                        {
                            oCode = companies[0];
                            oName = companies[1];
                        }
                        _frmIT.dgvItem.Rows[row].Cells[17].Value = oName;
                        foreach (var x in InventoryTransferItemsModel.ITitems.Where(x => x.ItemCode == _frmIT.dgvItem.Rows[row].Cells[1].Value.ToString()).ToList())
                        {
                            x.Company = oName;
                            x.CompanyCode = oCode;
                        }
                        isEdited = true;
                    }

                    if (isEdited)
                    {
                        LoadData(_frmIT.dgvItem);
                        LoadData(_frmIT.dgvPreviewItem);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        internal void ItemChainCellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int colIndex = e.ColumnIndex - 1;
                colIndex = colIndex < 0 ? 0 : colIndex;

                string columnName = _frmIT.dgvItem.Columns[colIndex].Name;

                int CurrentRowIndex = _frmIT.dgvItem.CurrentRow.Cells["LineNum"].Value == null ? -1 : Convert.ToInt32((string.IsNullOrEmpty(_frmIT.dgvItem.CurrentRow.Cells["LineNum"].Value.ToString()) ? -1 : _frmIT.dgvItem.CurrentRow.Cells["LineNum"].Value));
                int index = CurrentRowIndex;

                index = index == CurrentRowIndex ? CurrentRowIndex : index;

                if (columnName.Equals("Chain Description"))
                {
                    GetChain(colIndex,index);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal void GetChain(int colIndex, int index)
        {
            Dictionary<string, string> chainDetails = SelectChain();

            if (chainDetails.Count() > 0)
            {
                UpdateCell(colIndex, chainDetails["Value"]);
                UpdateCell(colIndex - 1, chainDetails["Code"]);
                InventoryTransferItemsModel.ITitems.Where(x => x.Linenum == index)
                    .ToList().ForEach(x =>
                    {
                        x.Chain = chainDetails["Code"];
                        x.ChainDescription = chainDetails["Value"];
                       
                    });
            }
        }

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
        private void UpdateCell(int ColIndex, string value)
        {
            _frmIT.dgvItem.CurrentRow.Cells[ColIndex].Value = value;

            //int count = _frmIT.dgvPreviewItem.RowCount;
            //if (count > 1)
            //{
            //    _frmIT.dgvPreviewItem.CurrentRow.Cells[ColIndex].Value = value;
            //}

        }

        #endregion

        #region post event
        private void Post(object sender, EventArgs e)
        {
            var itServiceLayer = new IT_ServiceLayer(_frmIT, mainForm, this);
            //serviceLayerAccess.ITR_Posting("POST");
            _frmIT.btnRequest.Enabled = false;

            if (PreChecking())
            {
                if (CheckReqWhs("U_FillerAction", 13, _frmIT.TxtFWhsCode.Text))
                {
                    if (CheckReqWhs("U_DestAction", 15, _frmIT.TxtTWhsCode.Text))
                    {
                        //PreLoading(_frmIT.dgvItem.Rows.Count, "added");
                        if (CheckRequired())
                        {
                            if (_frmIT.btnRequest.Text == "Add")
                            {
                                itServiceLayer.IT_Posting("POST");
                            }
                            else
                            {
                                itServiceLayer.IT_Posting("PATCH", Convert.ToInt32(_frmIT.TxtDocentry.Text));
                            }
                        }
                    }

                    else
                    {
                        //StaticHelper._MainForm.ShowMessage("Header To Warehouse and per line To Warehouse does not match.", true);
                        StaticHelper._MainForm.ShowMessage("Header To Warehouse and per line To Warehouse does not match.", true);
                    }
                }

                else
                {
                    StaticHelper._MainForm.ShowMessage("Header From Warehouse and per line From Warehouse does not match.", true);
                }
            }


            _frmIT.btnRequest.Enabled = true;
        }
        #endregion

        #region pre checking

        private Boolean PreChecking()
        {
            bool result = false;

            try
            {
                if (_frmIT.CbTransferType.Text != "")
                {
                    if (InventoryTransferItemsModel.ITitems.ToList().Count > 0 || (InventoryTransferHeaderModel.DDWdocentry.Count == 1 || InventoryTransferHeaderModel.DDWdocentry.Count > 0))
                    {
                        if (InventoryTransferItemsModel.ITitems.Where(x => x.ObjType == InventoryTransferHeaderModel.selObjType).ToList().Count > 0)
                        {
                            Int32 fwhscnt = 0;
                            Int32 twhscnt = 0;
                            int count = InventoryTransferItemsModel.ITitems.Where(x => x.ObjType == InventoryTransferHeaderModel.selObjType).ToList().Count;

                            for (int x = 0; x < count; x++)
                            {
                                if (_frmIT.dgvItem.Rows[x].Cells[13].Value == null)
                                {
                                    fwhscnt++;
                                }
                                if (_frmIT.dgvItem.Rows[x].Cells[15].Value == null)
                                {
                                    twhscnt++;
                                }
                            }
                            //foreach (DataGridViewRow row in _frmIT.dgvItem.Rows)
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
                                    if (_frmIT.CbCompany.SelectedIndex > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        StaticHelper._MainForm.ShowMessage("Please fill in the company first.", true);
                                    }
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
                        StaticHelper._MainForm.ShowMessage("Please select item(s) to be transferred.", true);
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Please select a Transfer Type before adding.", true);
                }

                return result;
            }
            catch (Exception e)
            {
                StaticHelper._MainForm.ShowMessage(e.Message, true);
                return result;
            }

        }
        private Boolean CheckReqWhs(string fieldname, int cellnum, string whsvalue)
        {
            bool result = false;
            bool boolBlockDiffWhs = false;
            int intCntDiffWhs = 0;

            try
            {
                if (InventoryTransferHeaderModel.DDWdocentry.Count == 1)
                {
                    if (sapHanaAccess.Get($"select {fieldname} from [@TRANSFER_TYPE] where Name = '{_frmIT.CbTransferType.Text}'").Rows[0][fieldname].ToString() == "B")
                    {
                        boolBlockDiffWhs = true;
                    }
                    else { boolBlockDiffWhs = false; }
                    int count = InventoryTransferItemsModel.ITitems.Where(x => x.ObjType == InventoryTransferHeaderModel.selObjType).ToList().Count;

                    for (int x = 0; x < count; x++)
                    {
                        if (_frmIT.dgvItem.Rows[x].Cells[cellnum].Value.ToString() != whsvalue)
                        {
                            intCntDiffWhs++;
                        }
                    }

                    //foreach (DataGridViewRow row in _frmIT.dgvItem.Rows)
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
                }
                else
                {
                    result = true;
                }

                return result;
            }
            catch (Exception e)
            {
                StaticHelper._MainForm.ShowMessage(e.Message, true);
                return result;
            }
        }
        private Boolean CheckRequired()
        {
            bool result = true;

            try
            {
                if (InventoryTransferHeaderModel.DDWdocentry.Count == 1)
                {
                    if (sapHanaAccess.Get($"select U_BPRequired from [@TRANSFER_TYPE] where Name = '{_frmIT.CbTransferType.Text}'").Rows[0]["U_BPRequired"].ToString() == "Y" && _frmIT.TxtBpCode.Text == "")
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
                }

                //else if (ITm.SelValue("frmWhs2", cbTransferType.Text, txtBpCode.Text, txtAddress.Text, txtFWhsCode.Text) != txtFWhsCode.Text)
                //{
                //    result = false;
                //    StaticHelper._MainForm.ShowMessage($"Default From Warehouse for Transfer Type {cbTransferType.Text} does not match.", true);
                //}
                //else if (ITm.SelValue("toWhs2", cbTransferType.Text, txtBpCode.Text, txtAddress.Text, txtTWhsCode.Text) != txtTWhsCode.Text)
                //{
                //    result = false;
                //    StaticHelper._MainForm.ShowMessage($"Default To Warehouse for Transfer Type {cbTransferType.Text} does not match.", true);
                //}

                return result;
            }
            catch (Exception e)
            {
                StaticHelper._MainForm.ShowMessage(e.Message, true);
                return false;
            }

        }
        private Boolean CheckReqCondition(string FieldName, string FieldCode, string UDFName)
        {
            bool result = true;

            if (sapHanaAccess.Get($"select {FieldName} from [@TRANSFER_TYPE] where Name = '{_frmIT.CbTransferType.Text}'").Rows[0][FieldName].ToString() == "Y")
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
        public void PreLoading(int NoOfItems, string Action)
        {
            int max = NoOfItems;
            int min = 0;

            for (int i = 0; max > i; i++)
            {
                min++;
                Thread.Sleep(10);
                //PublicStatic.frmMain.Progress2($"Please wait until all data are uploaded. {min} out of {max}", min, max);
                mainForm.Progress($"Please wait until all data are uploaded. {min} out of {max}", min, max);
            }

            //PublicStatic.frmMain.Progress($"Document is now being {Action}. Please wait.", 100);
            //mainForm.Progress($"Please wait until all data are uploaded. {min} out of {max}", min, max);
        }
        #endregion

        #region clearing
        public void ClearData()
        {
            //numseries
            //ClearList();
            _frmIT.btnRequest.Text = "Add";
            _frmIT.TxtFWhsCode.Text = "";
            _frmIT.TxtTWhsCode.Text = "";

            _frmIT.UDF.Columns.Clear();
            if (!(_frmIT.UDF.Rows.Count > 0))
            {
                Udf_Load();
            }
            foreach (Control c in _frmIT.Panel2.Controls)
            {
                //Change from TextBox to System.Windows.Forms.TextBox
                if (c is System.Windows.Forms.TextBox)
                {
                    c.Text = "";
                }
            }


            _frmIT.dgvItem.Columns.Clear();
            _frmIT.dgvPreviewItem.Columns.Clear();
            InventoryTransferItemsModel.ITitems.Clear();

            LoadData(_frmIT.dgvItem);
            LoadData(_frmIT.dgvPreviewItem);

            LoadTransferType();
            LoadCompany();
            ChangeDocumentNumber();
            LoadSeries();
            EnableControls();
            ClearField();
        }
        public void ClearField(bool clearItems = true)
        {
            _frmIT.TxtITR_DocEntry.Text = string.Empty;
            _frmIT.TxtDocentry.Text = string.Empty;
            _frmIT.TxtBpCode.Text = string.Empty;
            _frmIT.TxtAddress.Text = string.Empty;
            _frmIT.CbCompany.Text = string.Empty;
            _frmIT.CbTransferType.Text = string.Empty;
            _frmIT.CmbSeries.Text = string.Empty;
            _frmIT.txtDocNum.Text = string.Empty;
            _frmIT.TxtDocStatus.Text = string.Empty;
            _frmIT.TxtFWhsCode.Text = string.Empty;
            _frmIT.TxtTWhsCode.Text = string.Empty;
            _frmIT.TxtSalesEmployee.Text = string.Empty;
            _frmIT.TxtRemarks.Text = string.Empty;
            _frmIT.TxtTotalQty.Text = string.Empty;
            _frmIT.TxtTotal.Text = string.Empty;
            _frmIT.TxtITR_DocNum.Text = string.Empty;
            _frmIT.TxtFromDoc.Text = string.Empty;
            _frmIT.DtDocDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            _frmIT.DtPostingDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            _frmIT.TxtDocStatus.Text = "Open";

            ChangeDocumentNumber();

            //PurchasingAP_generics.index = 0;

            foreach (DataGridViewRow udfRow in _frmIT.UDF.Rows)
            {
                try
                {
                    udfRow.Cells[2].Value = null;
                }
                catch { }
            }
            _frmIT.UDF.Controls.Cast<Control>().Where(x => x.GetType() == typeof(DateTimePicker)).ToList().ForEach(x =>
            {

                _frmIT.UDF.Controls.Remove(x);
            });

            _frmIT.dgvItem.Rows.Clear();
            _frmIT.dgvPreviewItem.Rows.Clear();
            //if (clearItems)
            //{
            ClearList();
            //}

        }
        public void ClearList()
        {
            //CLEAR LIST DATA
            InventoryTransferHeaderModel.oArea = "";
            _frmIT.OProject = "";

            InventoryTransferHeaderModel._DocHeader.RemoveAll(x => x.ObjType == InventoryTransfer.objType || x.ObjType == InventoryTransfer.objType1);
            InventoryTransferItemsModel.ITitems.RemoveAll(x => x.ObjType == InventoryTransfer.objType || x.ObjType == InventoryTransfer.objType1);
            InventoryTransferHeaderModel.oAddressID = "";
            //DECLARE.udf.RemoveAll(x => x.ObjCode == objType || x.ObjCode == objType1);
            DECLARE.udf.RemoveAll(x => x.ObjCode == "OWTR" || x.ObjCode == "OWTQ");
            InventoryTransferHeaderModel.DDWdocentry.RemoveAll(x => x.BpCode != "");
        }
        #endregion

        #region controls
        private void DisableControls()
        {
            //_frmIT.btnRequest.Enabled = false;
            _frmIT.DtDocDate.Enabled = false;
            _frmIT.btnItem.Enabled = false;
            _frmIT.btnCopyFrom.Enabled = false;
            //Hide Selection
            _frmIT.PbBPList.Visible = false;
            _frmIT.PbToWhsList.Visible = false;
            _frmIT.PbFromWhsList.Visible = false;
            _frmIT.CbCompany.Enabled = false;
            _frmIT.CbTransferType.Enabled = false;

            _frmIT.dgvItem.ReadOnly = true;
            _frmIT.dgvPreviewItem.ReadOnly = true;

            _frmIT.DtPostingDate.Enabled = false;

            _frmIT.TxtAddress.ReadOnly = true;

            _frmIT.CmbSeries.Enabled = false;
            //disable DateTime Picker
            //dtDocDate.Enabled = true;
            //dtPostingDate.Enabled = true;
            //Mode
            //FindMode = false;
        }
        private void EnableControls()
        {
            _frmIT.btnRequest.Enabled = true;
            _frmIT.btnCopyFrom.Enabled = true;
            _frmIT.DtDocDate.Enabled = true;
            _frmIT.btnItem.Enabled = true;
            //Hide Selection
            _frmIT.PbBPList.Visible = true;
            //pbPriceList.Visible = true;
            _frmIT.PbToWhsList.Visible = true;
            _frmIT.PbFromWhsList.Visible = true;
            _frmIT.CbCompany.Enabled = true;
            _frmIT.CbTransferType.Enabled = true;
            _frmIT.dgvItem.ReadOnly = false;
            _frmIT.dgvPreviewItem.ReadOnly = false;
            _frmIT.DtPostingDate.Enabled = true;

            _frmIT.TxtAddress.ReadOnly = false;
            _frmIT.CmbSeries.Enabled = true;


            //disable DateTime Picker
            //dtDocDate.Enabled = true;
            //dtPostingDate.Enabled = true;
            ////Mode
            //FindMode = false;
        }
        #endregion

        #region Find Document
        public void FindDocument(object sender, EventArgs e)
        {
            StringBuilder udf = new StringBuilder();
            foreach (DataGridViewRow row in _frmIT.UDF.Rows)
            {
                udf.Append($"A.{row.Cells[0].Value.ToString()},");
            }
            //int PgSize = 1000;
            int PgSize = Convert.ToInt32(_frmIT.FindPageLimit);
            string query = "SELECT TOP " + PgSize + " A.DocEntry," +
                 "CASE " +
                        "WHEN A.CANCELED = 'Y' THEN 'Canceled' " +
                        "WHEN A.CANCELED = 'N' AND A.DocStatus != 'O' THEN 'Closed'  " +
                        "WHEN A.DocStatus = 'O' THEN 'Open' END [Status], " +
                "(SELECT T0.SeriesName FROM NNM1 T0 " +
                "Where T0.ObjectCode = 67 and T0.Series = A.Series)Series," +
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
                 $"{udf.ToString()}" +
                "(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] " +
                "FROM OWTR A " +
                //$"WHERE A.U_PrepBy = '{sboCredentials.UserId}' AND CANCELED = 'N'" +
                "Order By A.DocEntry Desc";

            //string query2 = "SELECT TOP 1000 A.DocEntry," +
            //    "A.DocNum [Doc No.]," +
            //    "A.CardCode [BP Code]," +
            //    "A.CardName [BP Name]," +
            //    "A.DocDate [Doc Date]," +
            //    "A.DocStatus [Status]," +
            //    "A.U_SINo [SI No.]," +
            //    "A.U_DRNo [DR No.]," +
            //    "A.U_PONo [PO No.]," +
            //    "(SELECT Sum(Z.Quantity) From WTR1 Z Where Z.DocEntry = A.DocEntry) [Total Quantity] " +
            //    "FROM OWTR A Order By A.DocEntry Desc";


            var dt = sapHanaAccess.Get(query);
            //if (InventoryTransferReqItemsModel.ITRitems.Where(x => x.ObjType == FrmInventoryTransferRequest.objType).ToList().Count > 0)
            //{
            //var result = MessageBox.Show("Unsaved data will be lost. Continue?", this.lblTitle.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (result == DialogResult.Yes)
            //{
            //LoadFindDetails();
            //}

            _frmIT.dgvFindDocument.DataSource = dt;
            InventoryTransferItemsController.dataGridLayout(_frmIT.dgvFindDocument);
            //}
            //else
            //{
            //    LoadFindDetails();
            //}
        }
        public void ChooseDocument(object sender, EventArgs e)
        {
            var itemNo = _frmIT.dgvFindDocument.CurrentRow.Cells[0].Value;
            if (itemNo != null)
            {
                string status = _frmIT.dgvFindDocument.CurrentRow.Cells[1].Value.ToString();
                ClearField(true);
                _frmIT.tabIT.SelectedIndex = 0;
                _frmIT.btnRequest.Text = "Update";
                LoadSAPDAta(Convert.ToInt32(itemNo));
                DisableControls();
                _frmIT.tabIT.SelectedIndex = 0;
            }
        }

        private void LoadSAPDAta(int DocEntry)
        {
            //MessageBox.Show("IT Transfer Service");
            //FindMode = true;
            //CLEAR LIST DATA
            ClearList();
            //DataTable
            DataTable dtItems;
            DataTable dtChains;
            DataTable dtHeader;
            StringBuilder udf = new StringBuilder();
            foreach (DataGridViewRow row in _frmIT.UDF.Rows)
            {
                udf.Append($"A.{row.Cells[0].Value.ToString()},");
            }

            var query = "Select (SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 67 and T0.Series = A.Series) [Series]" +
                        ", A.DocNum" +
                        ", A.CardCode" +
                        ", A.CardName" +
                        ", (Case when A.DocStatus = 'O' then 'Open' else 'Closed' end) [DocStatus]" +
                        ", (select MailAddres from OCRD where CardCode = A.CardCode) [Address]" +
                        ", A.U_Remarks" +
                        ", A.U_DateDel" +
                        ", (select ProjectCod from OCRD where CardCode = A.CardCode) [ProjectCod]" +
                        ", A.Filler" +
                        ", A.ToWhsCode" +
                        ", A.DocDate" +
                        ", A.TaxDate " +
                        ", A.DocDueDate" +
                        ", A.SlpCode " +
                        ", (select SlpName from OSLP where SlpCode = A.SlpCode) [SlpName]" +
                        ", A.U_TransferType " +
                       $",{udf.ToString()}" +
                        ", (select Name from [@CMP_INFO] where Code = A.U_CompanyTIN) [U_CompanyTIN] " +
                        ", A.Comments " +
                        $" FROM OWTR A WHERE A.DocEntry = {DocEntry}";



            string _bpCode, _docNum, _status, _discperc, _remarks, _whscode, _towhscode, _datedel, taxdate, docdate, docduedate, docStatus;

            DataTable headerDetails = sapHanaAccess.Get(query);

            _bpCode = DECLARE.dtNull(headerDetails, 0, "CardCode", "");
            _docNum = DECLARE.dtNull(headerDetails, 0, "DocNum", "0");
            _status = DECLARE.dtNull(headerDetails, 0, "DocStatus", "");
            //_remarks = DECLARE.dtNull(headerDetails, 0, "U_Remarks", "");
            _remarks = DECLARE.dtNull(headerDetails, 0, "Comments", "");
            _whscode = DECLARE.dtNull(headerDetails, 0, "Filler", "");
            _towhscode = DECLARE.dtNull(headerDetails, 0, "ToWhsCode", "");
            _datedel = DECLARE.dtNull(headerDetails, 0, "U_DateDel", "");

            taxdate = DECLARE.dtNull(headerDetails, 0, "DocDate", "");
            docdate = DECLARE.dtNull(headerDetails, 0, "TaxDate", "");
            docduedate = DECLARE.dtNull(headerDetails, 0, "DocDueDate", "");
            docStatus = DECLARE.dtNull(headerDetails, 0, "DocStatus", "");

            _frmIT.TxtBpCode.Text = _bpCode;
            _frmIT.TxtBpName.Text = DECLARE.dtNull(headerDetails, 0, "CardName", "");
            _frmIT.TxtAddress.Text = DECLARE.dtNull(headerDetails, 0, "Address", "");
            _frmIT.TxtRemarks.Text = _remarks;
            _frmIT.txtDocNum.Text = _docNum;
            _frmIT.TxtDocentry.Text = DocEntry.ToString();
            _frmIT.CbTransferType.Text = DECLARE.dtNull(headerDetails, 0, "U_TransferType", "");

            _frmIT.TxtFWhsCode.Text = _whscode;
            _frmIT.TxtTWhsCode.Text = _towhscode;
            InventoryTransfer.oFWhsCode = _whscode;
            InventoryTransfer.oTWhsCode = _towhscode;
            _frmIT.OProject = DECLARE.dtNull(headerDetails, 0, "ProjectCod", "");
            //dateTimePicker1.Text = _datedel;

            _frmIT.CmbSeries.Text = DECLARE.dtNull(headerDetails, 0, "Series", "");
            _frmIT.CbCompany.SelectedIndex = _frmIT.CbCompany.FindString(DECLARE.dtNull(headerDetails, 0, "U_CompanyTIN", ""));

            _frmIT.TxtSalesEmployee.Text = DECLARE.dtNull(headerDetails, 0, "SlpName", "");
            _frmIT.DtPostingDate.Text = docdate;
            _frmIT.DtDocDate.Text = taxdate;
            _frmIT.TxtDocStatus.Text = docStatus;

            _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    var fieldName = x.Cells[0].Value.ToString();
                    if (fieldName.Contains("date") || fieldName.Contains("Date"))
                    {
                        string date = DECLARE.dtNull(headerDetails, 0, fieldName, "");
                        if (date != "")
                        {
                            //DateTimePicker oDateTimePicker = new DateTimePicker();
                            oDateTimePicker.Value = Convert.ToDateTime(date);
                            //x.Cells[2].Value = Convert.ToDateTime().ToShortDateString();
                            _frmIT.UDF[2, x.Index].Value = date;
                            _frmIT.UDF.Controls.Add(oDateTimePicker);
                            CreateDateTimePicker(oDateTimePicker, _frmIT.UDF, x);
                        }

                    }
                    else
                    {
                        x.Cells[2].Value = DECLARE.dtNull(headerDetails, 0, fieldName, "");
                    }

                });

            //LoadBPDetails(_bpCode);

            var queryItems = "SELECT " +
                             " T0.BaseRef" +
                             ", T0.BaseEntry" +
                             ", T0.LineNum" +
                             ", T0.ItemCode" +
                             ", T0.Dscription" +
                             ", T0.OpenQty [Quantity]" +
                             ", T0.Price" +
                             ", T0.DiscPrcnt" +
                             ", ((T1.OnHand + T1.OnOrder) -  T1.IsCommited) [Available]" +
                             ", T0.LineTotal" +
                             ", T0.WhsCode" +
                             ", T0.FromWhsCod" +
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
                             ", T1.U_ID001 [BrandCode] " +
                            ", (SELECT Name FROM [@OBND] WHERE Code = T1.U_ID001) [BrandName] " +
                            ", T1.U_ID012 [U_StyleCode]" +
                            ", (SELECT Name FROM [@PRSTYLE] WHERE Code = T1.U_ID012) [StyleName] " +
                            ", (SELECT U_Code FROM [@OCLR] Where U_Color = T1.U_ID011 and Code = T1.U_ID022) [U_Color]" +
                            ", T1.U_ID011 [ColorName] " +
                            ", T1.U_ID018 [U_Section]" +
                            ", T1.U_ID007 [U_Size]" +
                            ", (select Name from [@CMP_INFO] where Code = T0.U_Company) [U_Company] " +
                            ", T0.U_SKU" +
                            ", T1.U_ID023" +
                            ", T1.InvntryUom " +
                            ", T0.U_Chain " +
                            ", T0.U_ChainDescription " +
                             " FROM WTR1 T0 INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode " +
                             " Where T0.DocEntry = '" + DocEntry + "'";




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

            dtItems = sapHanaAccess.Get(queryItems);


            _frmIT.TxtFromDoc.Text = "Inventory Transfer Request";
            _frmIT.TxtITR_DocNum.Text = dtItems.Rows[0]["BaseRef"].ToString();
            _frmIT.TxtITR_DocEntry.Text = dtItems.Rows[0]["BaseEntry"].ToString();

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

                InventoryTransferItemsModel.ITitems.Add(new InventoryTransferItemsModel.ITitemsData
                {
                    ObjType = InventoryTransferHeaderModel.selObjType,
                    Linenum = Convert.ToInt32(dtItems.Rows[x]["LineNum"].ToString()),
                    ItemCode = dtItems.Rows[x]["ItemCode"].ToString(), // ItemCode
                    ItemName = dtItems.Rows[x]["Dscription"].ToString(), // ItemCode
                    BarCode = dtItems.Rows[x]["CodeBars"].ToString(),
                    Chain = string.IsNullOrEmpty(dtItems.Rows[x]["U_Chain"].ToString()) ? "" : dtItems.Rows[x]["U_Chain"].ToString(),
                    ChainDescription =string.IsNullOrEmpty(dtItems.Rows[x]["U_ChainDescription"].ToString()) ? "": dtItems.Rows[x]["U_ChainDescription"].ToString(),
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
                    Company = DECLARE.dtNull(dtItems, x, "U_Company", ""),
                    SKU = DECLARE.dtNull(dtItems, x, "U_SKU", ""),
                    SortCode = dtItems.Rows[x]["U_ID023"].ToString(),
                    InventoryUOM = dtItems.Rows[x]["InvntryUom"].ToString()
                    //,Selected = false
                });
            }

            _frmIT.dgvItem.Columns.Clear();

            LoadData(_frmIT.dgvItem);
            LoadData(_frmIT.dgvPreviewItem);

            DisableControls();
        }
        #endregion

        public FileInfo[] GetDocumentCrystalForms()
        {
            //string path = $"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\IT";
            //directoryAccess.CreateDirectory(path);
            var sys = new SystemSettings();
            var _settingsService = new SettingsService();

            string path = sys.PathExist($"{_settingsService.GetReportPath()}Inventory\\");

            FileInfo[] Files = sys.FileList(path, "*OWTR*" + "*.rpt");

            return Files;
        }

        #region Print Document
        public void PrintDocumentChange(object sender, EventArgs e)
        {
            try
            {
                var cryRpt = new ReportDocument();
                var crtableLogoninfos = new TableLogOnInfos();
                var crtableLogoninfo = new TableLogOnInfo();
                var crConnectionInfo = new ConnectionInfo();

                //string path = $"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\IT\\{_frmIT.vmbPrintPreview.SelectedItem.ToString()}";
                var _settingsService = new SettingsService();

                string path = $"{_settingsService.GetReportPath()}Inventory\\{_frmIT.vmbPrintPreview.SelectedItem.ToString()}";

                cryRpt.Load(path);
                cryRpt.SetParameterValue("DocKey@", _frmIT.TxtDocentry.Text);
                cryRpt.SetParameterValue("UserCode@", sboCredentials.UserId);

                //string constring = $"DRIVER=HDBODBC32;SERVERNODE=HANASERVERNBFI:30015;DATABASE={sboCredentials.Database}";

                //crConnectionInfo.IntegratedSecurity = false;

                //#############################################################################
                //Added by Cedi 070119
                var logonProperties = new DbConnectionAttributes();
                var sboCred = new SboCredentials();
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

                //crConnectionInfo.ServerName = sboCredentials.DbServer;
                //crConnectionInfo.DatabaseName = sboCredentials.Database;
                //crConnectionInfo.UserID = sboCredentials.DbUserId;
                //crConnectionInfo.Password = sboCredentials.DbPassword;

                crConnectionInfo.Attributes = connectionAttributes;
                crConnectionInfo.Type = ConnectionInfoType.CRQE;
                crConnectionInfo.IntegratedSecurity = false;
                //crConnectionInfo.Type = ConnectionInfoType.SQL;

                //from Table to  CrystalDecisions.CrystalReports.Engine.Table
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in cryRpt.Database.Tables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;

                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                _frmIT.CrystalReportViewer1.ReportSource = cryRpt;
                _frmIT.CrystalReportViewer1.Refresh();
                //cryRpt.Close();
                //cryRpt.Dispose();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }
        #endregion

        #region copyfrom
        private void DocListChange(object sender, EventArgs e)
        {
            try
            {
                InventoryTransferHeaderModel.selObjType = InventoryTransfer.objType1;
                InventoryTransferHeaderModel.oCode = "";
                _frmIT.btnRequest.Text = "Add";
                //if (InventoryTransferItemsModel.ITitems.Where(x => x.ObjType == InventoryTransferHeaderModel.selObjType).ToList().Count > 0)
                //{
                //    //var result = MessageBox.Show("Unsaved data will be lost. Continue?", this.lblTitle.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //    //if (result == DialogResult.Yes)
                //    //{
                //    CopyFrom(_frmIT.cmbCopyFromOption.Text);
                //    //}
                //}
                //else
                //{
                //    CopyFrom(_frmIT.cmbCopyFromOption.Text);
                //}

                CopyFrom(_frmIT.cmbCopyFromOption.Text);
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }

        }

        internal void CopyFrom(string doc)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost.Do you wish to continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var bpCode = _frmIT.TxtBpCode.Text;
                ClearField(true);
                //Inventory Transfer Request
                //Pick Lists
                switch (doc)
                {
                    case "Inventory Transfer Request":

                        CopyFromITR();
                        break;

                    case "Pick Lists":
                        CopyPickList(bpCode);
                        break;
                }
            }
        }

        internal void CopyPickList(string bpCode)
        {
            string picklistEntry = null;
            //GENERATE FROM PICKLIST
            frmSearch2 fS = new frmSearch2();
            if (bpCode != "")
            {
                frmSearch2.Param1 = bpCode;
                fS.allowMultiple = true;
                fS.oSearchMode = "OPKL_BP";
                InventoryTransferHeaderModel.oSearchTable = "OPKL_I";
            }
            else
            {
                fS.oSearchMode = "OPKL_I";
                fS.allowMultiple = true;
                InventoryTransferHeaderModel.oSearchTable = "OPKL_I";
            }

            fS.ShowDialog();
            ////v1.24 7517

            if (InventoryTransferHeaderModel.DDWdocentry.Count > 0)
            {

                picklistEntry = InventoryTransferHeaderModel.oCode;
                frmIT_DrawDoumentWizard frmDDW = new frmIT_DrawDoumentWizard(new InventoryTransfer());
                frmDDW.ShowDialog();
                string oSelCode = InventoryTransferHeaderModel.oCode;
                string oSelQry1 = "";

                if (InventoryTransferHeaderModel.DDWdocentry.Count > 0)
                {

                    //Added checking of BP if the same copy from multiple ITR's - 080919
                    string SelectedOneBP = "";
                    string SelDocEntry = "";
                    string SelOrderEntry = "";
                    int SelectBPcount = 0;
                    int SelEntrycnt = 0;

                    foreach (var x in InventoryTransferHeaderModel.DDWdocentry.Where(x => x.BpCode != ""))
                    {
                        if (SelectedOneBP == "")
                        {
                            SelectedOneBP = x.BpCode.ToString();

                            SelDocEntry = x.DocEntry.ToString();
                            SelOrderEntry = x.OrderEntry.ToString();
                            SelectBPcount++;
                            SelEntrycnt++;
                        }
                        else
                        {
                            if (SelectedOneBP == x.BpCode.ToString())
                            {
                                SelectedOneBP = SelectedOneBP == x.BpCode.ToString() ? SelectedOneBP : "";
                                SelectBPcount++;
                            }

                            if (SelDocEntry == x.DocEntry.ToString() && SelOrderEntry == x.OrderEntry.ToString())
                            {
                                SelDocEntry = SelDocEntry == x.DocEntry.ToString() ? SelDocEntry : "";
                                SelOrderEntry = SelOrderEntry == x.OrderEntry.ToString() ? SelOrderEntry : "";
                                SelEntrycnt++;
                            }
                        }
                    }
                    SelectedOneBP = SelectBPcount == InventoryTransferHeaderModel.DDWdocentry.Count ? SelectedOneBP : "";
                    SelEntrycnt = SelEntrycnt == InventoryTransferHeaderModel.DDWdocentry.Count ? 0 : SelEntrycnt;
                    //080919

                    if (InventoryTransferHeaderModel.DDWdocentry.Count == 1 || SelEntrycnt == 0)
                    {
                        oSelCode = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.BpCode != "").Select(z => z.DocEntry).First().ToString();
                        InventoryTransferHeaderModel.oBPCode = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.DocEntry != 0).Select(z => z.BpCode).First().ToString();
                        InventoryTransferHeaderModel.oOrderEntry = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.DocEntry != 0).Select(z => z.OrderEntry).First().ToString();
                        picklistEntry = InventoryTransferHeaderModel.oOrderEntry;
                        StringBuilder udf = new StringBuilder();
                        string udfQuery = UDF("OWTQ");
                        DataTable udfDt = sapHanaAccess.Get(udfQuery);
                        foreach (DataRow row in udfDt.Rows)
                        {
                            udf.Append($"C.{row["AliasID"].ToString()},");
                        }
                        var q = "SELECT DISTINCT C.CardCode,C.CardName,A.OrderEntry,C.U_PONo,(select MailAddres from OCRD where CardCode = C.CardCode) [Address], " +
                        $"(select ProjectCod from OCRD where CardCode = C.CardCode) [ProjectCod], C.U_TransferType,{udf.ToString()}" +
                        $"(select Name from[@CMP_INFO] where Code = C.U_CompanyTIN) [U_CompanyTIN],(select SlpName from OSLP where SlpCode = C.SlpCode) [SlpName] " +
                        " FROM PKL1 A LEFT JOIN WTQ1 B ON A.OrderEntry = B.DocEntry " +
                        $" LEFT JOIN OWTQ C ON B.DocEntry = C.DocEntry and C.CardCode = '{InventoryTransferHeaderModel.oBPCode}' Where B.OpenQty > 0 AND A.OrderEntry = '{InventoryTransferHeaderModel.oOrderEntry}' and A.AbsEntry = '" + oSelCode + "' ";

                        var dt = sapHanaAccess.Get(q);

                        _frmIT.TxtBpCode.Text = DECLARE.dtNull(dt, 0, "CardCode", "");

                        _frmIT.TxtFromDoc.Text = "Pick List";
                        _frmIT.TxtITR_DocEntry.Text = DECLARE.dtNull(dt, 0, "OrderEntry", "");

                        var selqry = "SELECT DocNum FROM OWTQ WHERE DocEntry = '" + InventoryTransferHeaderModel.oOrderEntry + "'";
                        var dt2 = sapHanaAccess.Get(selqry);
                        _frmIT.TxtITR_DocNum.Text = DECLARE.dtNull(dt2, 0, "DocNum", "");

                        LoadBPDetails(DECLARE.dtNull(dt, 0, "CardCode", ""));
                        _frmIT.TxtBpName.Text = DECLARE.dtNull(dt, 0, "CardName", "");
                        _frmIT.TxtAddress.Text = DECLARE.dtNull(dt, 0, "Address", "");
                        _frmIT.CbTransferType.Text = DECLARE.dtNull(dt, 0, "U_TransferType", "");
                        _frmIT.CbCompany.SelectedIndex = _frmIT.CbCompany.FindString(DECLARE.dtNull(dt, 0, "U_CompanyTIN", ""));

                        _frmIT.TxtSalesEmployee.Text = DECLARE.dtNull(dt, 0, "SlpName", "");
                        _frmIT.OProject = DECLARE.dtNull(dt, 0, "ProjectCod", "");

                        //From WHS
                        var dtWhs = sapHanaAccess.Get($"Select FromWhsCod From WTQ1 Where DocEntry = {DECLARE.dtNull(dt, 0, "OrderEntry", "")} ");
                        _frmIT.TxtFWhsCode.Text = DECLARE.dtNull(dtWhs, 0, "FromWhsCod", "");

                        _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList()
                       .ForEach(x =>
                       {
                           var fieldName = x.Cells[0].Value.ToString();

                           DataColumnCollection columns = dt.Columns;
                           if (columns.Contains(fieldName))
                           {
                               if (fieldName.Contains("date") || fieldName.Contains("Date"))
                               {
                                   string date = DECLARE.dtNull(dt, 0, fieldName, "");
                                   if (date != "")
                                   {
                                       //DateTimePicker oDateTimePicker = new DateTimePicker();
                                       oDateTimePicker.Value = Convert.ToDateTime(date);
                                       //x.Cells[2].Value = Convert.ToDateTime().ToShortDateString();
                                       _frmIT.UDF[2, x.Index].Value = date;
                                       _frmIT.UDF.Controls.Add(oDateTimePicker);
                                       CreateDateTimePicker(oDateTimePicker, _frmIT.UDF, x);
                                   }

                               }
                               else
                               {
                                   x.Cells[2].Value = DECLARE.dtNull(dt, 0, fieldName, "");
                               }
                           }
                       });
                    }

                    if (SelectedOneBP != "")
                    {
                        string fieldQuery = $"Select CardName from OCRD where CardCode = '{SelectedOneBP}'";

                        DataTable dtBP = sapHanaAccess.Get(fieldQuery);
                        LoadBPDetails(SelectedOneBP);

                        _frmIT.TxtBpCode.Text = SelectedOneBP;
                        _frmIT.TxtBpName.Text = DECLARE.dtNull(dtBP, 0, "CardName", "");

                    }

                    //GENERATE ITEMS FROM PICKLIST
                    string query = "";

                    oSelQry1 = "SELECT " +
                                                        " A.AbsEntry" +
                                                        ", A.OrderEntry" +
                                                        ", A.OrderLine" +
                                                        ", A.PickQtty" +
                                                        ", A.PickStatus" +
                                                        ", A.RelQtty" +
                                                        ", A.PrevReleas" +
                                                        ", A.BaseObject" +
                                                        ", C.U_ID001 [BrandCode] " +
                                                        ", (SELECT Name FROM [@OBND] WHERE Code = C.U_ID001) [BrandName] " +
                                                        ", C.U_ID012 [U_StyleCode]" +
                                                        ", (SELECT Name FROM [@PRSTYLE] WHERE Code = C.U_ID012) [StyleName] " +
                                                        ", (SELECT U_Code FROM [@OCLR] Where U_Color = C.U_ID011 and Code = C.U_ID022) [U_Color]" +
                                                        ", C.U_ID011 [ColorName] " +
                                                        ", C.U_ID018 [U_Section]" +
                                                        ", C.U_ID007 [U_Size]" +
                                                        ", B.ItemCode" +
                                                        ", B.Dscription" +
                                                        ", A.PickQtty [Quantity]" +
                                                        ", B.Price" +
                                                        ", ISNULL(B.DiscPrcnt, 0)[DiscPrcnt]" +
                                                        ", B.LineTotal" +
                                                        ", B.WhsCode" +
                                                        ", B.FromWhsCod" +
                                                        ", B.SlpCode" +
                                                        ", B.PriceBefDi" +
                                                        ", B.Project" +
                                                        ", B.VatGroup" +
                                                        ", B.VatPrcnt" +
                                                        ", B.CodeBars" +
                                                        ", B.PriceAfVAT" +
                                                        ", B.TaxCode" +
                                                        ", B.VatAppld" +
                                                        ", B.LineVat " +
                                                        ", (select Name from [@CMP_INFO] where Code = B.U_Company) [U_Company] " +
                                                        ", B.U_SKU " +
                                                        ", C.U_ID023 " +
                                                        ", C.InvntryUom" +
                                                        " FROM PKL1 A " +
                                                        " LEFT JOIN WTQ1 B ON A.OrderEntry = B.DocEntry and A.OrderLine = B.LineNum ";

                    //", (A.PickQtty + A.RelQtty)[Quantity]" +
                    if (InventoryTransferHeaderModel.oDDW == "DrawAll")
                    {

                        var SelDocOrderEntry = InventoryTransferHeaderModel.DDWdocentry.GroupBy(x => new { x.DocEntry, x.OrderEntry }).Select(y => y.First());

                        //foreach (string x in InventoryTransferHeaderModel.DDWdocentry.Select(x => new { DocEn2 = x.DocEntry , x.OrderEntry }).Distinct())
                        foreach (var x in SelDocOrderEntry)
                        {
                            //Added DDWdocentry 081519
                            //int DDWdocentry = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.DocEntry).First();
                            InventoryTransferHeaderModel.oBPCode = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.BpCode).First().ToString();
                            InventoryTransferHeaderModel.oCode = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.DocEntry).First().ToString();

                            //Added OrderEntry filter for better identifying line items by Darrel 081419
                            //InventoryTransferHeaderModel.oOrderEntry = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.DocEntry == DDWdocentry).Select(z => z.OrderEntry).First().ToString();

                            if (query == "")
                            {
                                query = oSelQry1 +
                                        $" INNER JOIN OWTQ T4 on B.DocEntry = T4.DocEntry and T4.CardCode = '{InventoryTransferHeaderModel.oBPCode}' " +
                                        $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickQtty > 0 AND B.OpenQty > 0 AND A.PickStatus != 'C' and A.AbsEntry = '{InventoryTransferHeaderModel.oCode}' " +
                                        $" and A.OrderEntry = '{x.OrderEntry}' ";
                            }
                            else
                            {
                                query += " UNION ALL " +
                                         oSelQry1 +
                                         $" INNER JOIN OWTQ T4 on B.DocEntry = T4.DocEntry and T4.CardCode = '{InventoryTransferHeaderModel.oBPCode}' " +
                                         $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickQtty > 0 AND B.OpenQty > 0 AND A.PickStatus != 'C' and A.AbsEntry = '{InventoryTransferHeaderModel.oCode}' " +
                                         $" and A.OrderEntry = '{x.OrderEntry}' ";
                            }
                        }

                        //" order by A.OrderLine asc";
                    }
                    else
                    {
                        //New logic added by darrel 081619
                        var SelDocOrderEntry = InventoryTransferHeaderModel.DDWdocentry.GroupBy(x => new { x.DocEntry, x.OrderEntry }).Select(y => y.First());

                        //foreach (string x in InventoryTransferHeaderModel.DDWdocentry.Select(x => x.OrderEntry).Distinct())
                        foreach (var x in SelDocOrderEntry)
                        {
                            //int DDWdocentry = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.DocEntry).First();
                            InventoryTransferHeaderModel.oBPCode = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.BpCode).First().ToString();
                            InventoryTransferHeaderModel.oCode = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry).Select(z => z.DocEntry).First().ToString();
                            InventoryTransferHeaderModel.oLineNums = "";

                            //Added OrderEntry filter for better identifying line items by Darrel 081419
                            //InventoryTransferHeaderModel.oOrderEntry = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.DocEntry == DDWdocentry).Select(z => z.OrderEntry).First().ToString();

                            foreach (var y in InventoryTransferHeaderModel.DDWdocentry.Where(y => y.OrderEntry == x.OrderEntry && y.DocEntry == x.DocEntry))
                            {
                                if (InventoryTransferHeaderModel.oLineNums == "")
                                {
                                    InventoryTransferHeaderModel.oLineNums = "'" + y.LineEntry.ToString() + "'";
                                }
                                else
                                {
                                    InventoryTransferHeaderModel.oLineNums += ",'" + y.LineEntry.ToString() + "'";
                                }
                            }

                            if (query == "")
                            {
                                query = oSelQry1 +
                                        $" INNER JOIN OWTQ T4 on B.DocEntry = T4.DocEntry and T4.CardCode = '{InventoryTransferHeaderModel.oBPCode}' " +
                                        $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickStatus != 'C' and A.AbsEntry = '{InventoryTransferHeaderModel.oCode}' " +
                                        $" and A.OrderLine IN({InventoryTransferHeaderModel.oLineNums}) " +
                                        $" and A.OrderEntry = '{x.OrderEntry}' ";
                            }
                            else
                            {
                                query += " UNION ALL " +
                                         oSelQry1 +
                                         $" INNER JOIN OWTQ T4 on B.DocEntry = T4.DocEntry and T4.CardCode = '{InventoryTransferHeaderModel.oBPCode}' " +
                                         $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickStatus != 'C' and A.AbsEntry = '{InventoryTransferHeaderModel.oCode}' " +
                                         $" and A.OrderLine IN({InventoryTransferHeaderModel.oLineNums}) " +
                                         $" and A.OrderEntry = '{x.OrderEntry}' ";
                            }
                        }

                        //query += " and A.OrderLine IN(" + InventoryTransferHeaderModel.oLineNums + ") ";
                    }

                    //$" INNER JOIN OWTQ T4 on B.DocEntry = T4.DocEntry and T4.CardCode = '{InventoryTransferHeaderModel.oBPCode}' " +
                    //$" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickStatus != 'C' and A.AbsEntry = '{oSelCode}' " +
                    //" order by A.OrderLine asc";

                    //LINE ITEMS
                    DataTable dtIT_Items = sapHanaAccess.Get(query);

                    for (int x = 0; x < dtIT_Items.Rows.Count; x++)
                    {
                        //Init
                        Double LineTotal;
                        Double GrossTotal;
                        Double VatAmount;
                        Double GrossPrice;
                        Double PriceVatInc;
                        Double DiscAmt;

                        double qty = Convert.ToDouble(dtIT_Items.Rows[x]["Quantity"]);
                        double discount = Convert.ToDouble(dtIT_Items.Rows[x]["DiscPrcnt"]);
                        double price = Convert.ToDouble(dtIT_Items.Rows[x]["PriceBefDi"]);
                        double vatrate = 0;

                        PriceVatInc = price + (price * (vatrate / 100));
                        DiscAmt = price * (discount / 100);
                        LineTotal = (qty * price);
                        VatAmount = LineTotal * (vatrate / 100);
                        GrossTotal = (LineTotal + VatAmount) - DiscAmt;
                        GrossPrice = PriceVatInc - (PriceVatInc * (discount / 100));

                        double dQty = Convert.ToDouble(DECLARE.dtNull(dtIT_Items, x, "Quantity", "0"));
                        double dExistingQty = 0;
                        string sOrderEntry = DECLARE.dtNull(dtIT_Items, x, "OrderEntry", "");
                        string sOrderLine = DECLARE.dtNull(dtIT_Items, x, "OrderLine", "");

                        if (InventoryTransferItemsModel.ITitems.Where(y => y.BaseEntry == sOrderEntry && y.BaseLine == sOrderLine).ToList().Count == 0)
                        {
                            InventoryTransferItemsModel.ITitems.Add(new InventoryTransferItemsModel.ITitemsData
                            {
                                ObjType = InventoryTransferHeaderModel.selObjType,
                                BaseEntry = sOrderEntry, //    dtIT_Items.Rows[x]["OrderEntry"].ToString(),
                                BaseLine = sOrderLine, //dtIT_Items.Rows[x]["OrderLine"].ToString(),
                                BaseType = DECLARE.dtNull(dtIT_Items, x, "BaseObject", ""),// dtIT_Items.Rows[x]["BaseObject"].ToString(),
                                ItemCode = DECLARE.dtNull(dtIT_Items, x, "ItemCode", ""), //dtIT_Items.Rows[x]["ItemCode"].ToString(), // ItemCode
                                ItemName = DECLARE.dtNull(dtIT_Items, x, "Dscription", ""),
                                //BrandCode = DECLARE.dtNull(dtIT_Items, x, "BrandCode", ""),
                                BrandCode = dQty.ToString(),
                                Brand = DECLARE.dtNull(dtIT_Items, x, "BrandName", ""),
                                StyleCode = DECLARE.dtNull(dtIT_Items, x, "U_StyleCode", ""), //dtIT_Items.Rows[x]["U_StyleCode"].ToString(), //Style
                                Style = DECLARE.dtNull(dtIT_Items, x, "U_StyleCode", ""),
                                ColorCode = DECLARE.dtNull(dtIT_Items, x, "U_Color", ""), //dtIT_Items.Rows[x]["U_Color"].ToString(), //Color
                                Color = DECLARE.dtNull(dtIT_Items, x, "ColorName", ""),
                                Section = DECLARE.dtNull(dtIT_Items, x, "U_Section", ""), //dtIT_Items.Rows[x]["U_Section"].ToString(), //Section
                                Quantity = InventoryTransferRequestService.GetOrigQty(DECLARE.dtNull(dtIT_Items, x, "ItemCode", ""), dQty), //Qty
                                FWhsCode = DECLARE.dtNull(dtIT_Items, x, "FromWhsCod", ""), //dtIT_Items.Rows[x]["FromWhsCod"].ToString(), //WHsCode]
                                TWhsCode = DECLARE.dtNull(dtIT_Items, x, "WhsCode", ""), //dtIT_Items.Rows[x]["WhsCode"].ToString(), //To WHSCODE
                                BarCode = DECLARE.dtNull(dtIT_Items, x, "CodeBars", ""), //dtIT_Items.Rows[x]["CodeBars"].ToString(), //Barcode
                                DiscountPerc = Convert.ToDouble(DECLARE.dtNull(dtIT_Items, x, "DiscPrcnt", "0")),
                                GrossPrice = Convert.ToDouble(DECLARE.dtNull(dtIT_Items, x, "Price", "0")), //%Convert.ToDouble(DECLARE.dtNull(dtIT_Items, x, "PriceAfVAT", "0")),
                                TaxCode = DECLARE.dtNull(dtIT_Items, x, "VatGroup", ""), //dtIT_Items.Rows[x]["VatGroup"].ToString(),
                                DiscountAmount = DiscAmt,
                                GrossTotal = GrossTotal,
                                LineTotal = LineTotal,
                                Size = DECLARE.dtNull(dtIT_Items, x, "U_Size", ""), //dtIT_Items.Rows[x]["U_Size"].ToString(),
                                TaxAmount = VatAmount,
                                TaxRate = 0,
                                UnitPrice = Convert.ToDouble(DECLARE.dtNull(dtIT_Items, x, "PriceBefDi", "0")),
                                Company = DECLARE.dtNull(dtIT_Items, x, "U_Company", ""),
                                SKU = DECLARE.dtNull(dtIT_Items, x, "U_SKU", ""),
                                SortCode = DECLARE.dtNull(dtIT_Items, x, "U_ID023", ""),
                                InventoryUOM = DECLARE.dtNull(dtIT_Items, x, "InvntryUom", ""),

                            });
                        }
                        else
                        {
                            dExistingQty = Convert.ToDouble(InventoryTransferItemsModel.ITitems.Where(y => y.BaseEntry == sOrderEntry && y.BaseLine == sOrderLine).Select(v => v.Quantity).First().ToString());
                            dQty = dQty + dExistingQty;

                            foreach (var z in InventoryTransferItemsModel.ITitems.Where(y => y.BaseEntry == sOrderEntry && y.BaseLine == sOrderLine))
                            {
                                z.Quantity = Convert.ToDouble(dQty);
                            }
                        }

                    }

                    if (InventoryTransferHeaderModel.DDWdocentry.Count == 1)
                    {
                        LoadUDFDataFromPickList(picklistEntry);
                    }

                    LoadData(_frmIT.dgvItem);
                    LoadData(_frmIT.dgvPreviewItem);

                }

            }

        }


        void CopyFromITR()
        {
            frmSearch2 fS = new frmSearch2();

            if (_frmIT.TxtBpCode.Text == "")
            {
                fS.oSearchMode = "OWTQ_NOBP";
                fS.allowMultiple = true;
                InventoryTransferHeaderModel.oSearchTable = "OWTQ_NOBP";
            }
            else
            {
                fS.oSearchMode = "OWTQ";
                fS.allowMultiple = true;
                InventoryTransferHeaderModel.oSearchTable = "OWTQ";
                frmSearch2.Param1 = _frmIT.TxtBpCode.Text;
            }

            fS.ShowDialog();

            if (InventoryTransferHeaderModel.DDWdocentry.Count > 0)
            {

                //frmIT_DrawDoumentWizard frmDDW = new frmIT_DrawDoumentWizard(this);
                //frmDDW.ShowDialog();

                if (InventoryTransferHeaderModel.DDWdocentry.Count > 0)
                {
                    string oSelCode = "";
                    string oSelQry1 = "";

                    //Added checking of BP if the same copy from multiple ITR's - 080919
                    string SelectedOneBP = "";
                    int SelectBPcount = 0;
                    foreach (var x in InventoryTransferHeaderModel.DDWdocentry.Where(x => x.BpCode != ""))
                    {
                        if (SelectedOneBP == "")
                        {
                            SelectedOneBP = x.BpCode.ToString();
                            SelectBPcount++;
                        }
                        else
                        {
                            if (SelectedOneBP == x.BpCode.ToString())
                            {
                                SelectedOneBP = SelectedOneBP == x.BpCode2.ToString() ? SelectedOneBP : "";
                                SelectBPcount++;
                            }
                        }
                    }
                    SelectedOneBP = SelectBPcount == InventoryTransferHeaderModel.DDWdocentry.Count ? SelectedOneBP : "";
                    //080919

                    if (InventoryTransferHeaderModel.DDWdocentry.Count == 1)
                    {
                        oSelCode = InventoryTransferHeaderModel.DDWdocentry.Where(y => y.BpCode != "").Select(z => z.DocEntry).First().ToString();

                        var udf = new StringBuilder();
                        var udfQuery = UDF("OWTQ");
                        var udfDt = sapHanaAccess.Get(udfQuery);
                        foreach (DataRow row in udfDt.Rows)
                        {
                            udf.Append($"A.{row["AliasID"].ToString()},");
                        }
                        string fieldQuery = $"Select (SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) [Series]" +
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
                                    $" FROM OWTQ A WHERE A.DocEntry = {oSelCode}";

                        DataTable dtBP = sapHanaAccess.Get(fieldQuery);

                        string BP = DataAccess.Search(dtBP, 0, "CardCode");
                        string taxdate = DECLARE.dtNull(dtBP, 0, "TaxDate", "");
                        string docdate = DECLARE.dtNull(dtBP, 0, "DocDate", "");

                        _frmIT.TxtFromDoc.Text = "Inventory Transfer Request";
                        _frmIT.TxtITR_DocEntry.Text = oSelCode;
                        _frmIT.TxtITR_DocNum.Text = DECLARE.dtNull(dtBP, 0, "DocNum", "0");
                        LoadBPDetails(BP);

                        _frmIT.TxtBpCode.Text = BP;
                        _frmIT.TxtBpName.Text = DECLARE.dtNull(dtBP, 0, "CardName", "");
                        _frmIT.TxtAddress.Text = DECLARE.dtNull(dtBP, 0, "Address", "");
                        _frmIT.CbTransferType.Text = DataAccess.Search(dtBP, 0, "U_TransferType");
                        _frmIT.CbCompany.SelectedIndex = _frmIT.CbCompany.FindString(DECLARE.dtNull(dtBP, 0, "U_CompanyTIN", ""));
                        _frmIT.TxtRemarks.Text = DECLARE.dtNull(dtBP, 0, "U_Remarks", "");
                        _frmIT.TxtFWhsCode.Text = DataAccess.Search(dtBP, 0, "Filler");
                        _frmIT.TxtTWhsCode.Text = DataAccess.Search(dtBP, 0, "ToWhsCode");
                        _frmIT.TxtSalesEmployee.Text = DECLARE.dtNull(dtBP, 0, "SlpName", "");

                        _frmIT.DtPostingDate.Format = DateTimePickerFormat.Short;
                        _frmIT.DtDocDate.Format = DateTimePickerFormat.Short;
                        _frmIT.DtPostingDate.Text = docdate;
                        _frmIT.DtDocDate.Text = taxdate;


                        _frmIT.UDF.Rows.Cast<DataGridViewRow>().ToList()
                        .ForEach(x =>
                        {
                            var fieldName = x.Cells[0].Value.ToString();

                            DataColumnCollection columns = dtBP.Columns;
                            if (columns.Contains(fieldName))
                            {
                                if (fieldName.Contains("date") || fieldName.Contains("Date"))
                                {
                                    string date = DECLARE.dtNull(dtBP, 0, fieldName, "");
                                    if (date != "")
                                    {
                                        //DateTimePicker oDateTimePicker = new DateTimePicker();
                                        oDateTimePicker.Value = Convert.ToDateTime(date);
                                        //x.Cells[2].Value = Convert.ToDateTime().ToShortDateString();
                                        _frmIT.UDF[2, x.Index].Value = date;
                                        _frmIT.UDF.Controls.Add(oDateTimePicker);
                                        CreateDateTimePicker(oDateTimePicker, _frmIT.UDF, x);
                                    }

                                }
                                else
                                {
                                    x.Cells[2].Value = DECLARE.dtNull(dtBP, 0, fieldName, "");
                                }
                            }
                        });
                    }
                    else
                    {
                        if (SelectedOneBP != "")
                        {
                            string fieldQuery = $"Select CardName from OCRD where CardCode = '{SelectedOneBP}'";

                            DataTable dtBP = sapHanaAccess.Get(fieldQuery);
                            LoadBPDetails(SelectedOneBP);

                            _frmIT.TxtBpCode.Text = SelectedOneBP;
                            _frmIT.TxtBpName.Text = DECLARE.dtNull(dtBP, 0, "CardName", "");

                        }

                        foreach (var x in InventoryTransferHeaderModel.DDWdocentry.Where(x => x.BpCode != ""))
                        {
                            if (oSelCode == "")
                            {
                                oSelCode = "'" + x.DocEntry.ToString() + "'";
                            }
                            else
                            {
                                oSelCode += ",'" + x.DocEntry.ToString() + "'";
                            }
                        }
                    }

                    string query = "";
                    var queryItems = "SELECT " +
                              ", T0.DocEntry " +
                              ", T0.LineNum" +
                              ", T0.ItemCode" +
                              ", T0.Dscription" +
                              ", T0.Quantity [Quantity]" +
                              ", T0.Price" +
                              ", ISNULL(T0.DiscPrcnt, 0) [DiscPrcnt]" +
                              ", ((T1.OnHand + T1.OnOrder) -  T1.IsCommited) [Available]" +
                              ", T0.LineTotal" +
                              ", T0.WhsCode" +
                              ", T0.FromWhsCod " +
                              ", T0.SlpCode" +
                              ", T0.PriceBefDi" +
                              ", T0.Project" +
                              ", T0.VatGroup" +
                              ", T0.VatPrcnt" +
                              ", T0.CodeBars" +
                              ", ISNULL(T0.PriceAfVAT, 0) [PriceAfVAT]" +
                              ", T0.TaxCode" +
                              ", T0.VatAppld" +
                              ", T0.LineVat" +
                              ", T0.U_SKU" +
                              ", T1.U_ID001 [BrandCode] " +
                              ", (SELECT Name FROM [@OBND] WHERE Code = T1.U_ID001) [BrandName] " +
                              ", T1.U_ID012 [U_StyleCode]" +
                              ", (SELECT Name FROM [@PRSTYLE] WHERE Code = T1.U_ID012) [StyleName] " +
                              ", (SELECT U_Code FROM [@OCLR] Where U_Color = T1.U_ID011 and Code = T1.U_ID022) [U_Color]" +
                              ", T1.U_ID011 [ColorName] " +
                              ", T1.U_ID018 [U_Section]" +
                              ", T1.U_ID007 [U_Size]" +
                              ", T1.U_ID023 " +
                              ", (select Name from [@CMP_INFO] where Code = T0.U_Company) [U_Company] " +
                              ", T1.InvntryUom " +
                              " FROM WTQ1 T0 INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode " +
                              " Where T0.DocEntry IN(" + oSelCode + ")";
                    //" Where T0.DocEntry = '" + oSelCode + "'";

                    //oSelQry1 = "SELECT " +
                    //        " T0.LineNum" +
                    //        ", T0.DocEntry " +
                    //        ", T0.ItemCode" +
                    //        ", T0.Dscription" +
                    //        ", T0.OpenQty [Quantity]" +
                    //        ", ISNULL(T0.Price, 0) [Price]" +
                    //        ", ISNULL(T0.DiscPrcnt, 0) [DiscPrcnt]" +
                    //        ", ISNULL(T0.LineTotal, 0) [LineTotal]" +
                    //        ", T0.WhsCode" +
                    //        ", T0.FromWhsCod" +
                    //        ", T0.SlpCode" +
                    //        ", T0.PriceBefDi" +
                    //        ", T0.Project" +
                    //        ", T0.VatGroup" +
                    //        ", T0.VatPrcnt" +
                    //        ", T0.CodeBars" +
                    //        ", ISNULL(T0.PriceAfVAT, 0) [PriceAfVAT]" +
                    //        ", T0.TaxCode" +
                    //        ", T0.VatAppld" +
                    //        ", T0.LineVat" +
                    //        ", T1.U_ID001 [BrandCode] " +
                    //        ", (SELECT Name FROM [@OBND] WHERE Code = T1.U_ID001) [BrandName] " +
                    //        ", T1.U_ID012 [U_StyleCode]" +
                    //        ", (SELECT Name FROM [@PRSTYLE] WHERE Code = T1.U_ID012) [StyleName] " +
                    //        ", (SELECT U_Code FROM [@OCLR] Where U_Color = T1.U_ID011 and Code = T1.U_ID022) [U_Color]" +
                    //        ", T1.U_ID011 [ColorName] " +
                    //        ", T1.U_ID018 [U_Section]" +
                    //        ", T1.U_ID007 [U_Size]" +
                    //        ", (select Name from [@CMP_INFO] where Code = T0.U_Company) [U_Company] " +
                    //        ", T0.U_SKU" +
                    //        ", T1.U_ID023" +
                    //        " FROM WTQ1 T0 INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode";

                    //if (InventoryTransferHeaderModel.oDDW == "DrawAll")
                    //{
                    //    query += oSelQry1 +
                    //             $" Where T0.DocEntry IN({oSelCode}) ORDER BY T0.Dscription asc";
                    //}
                    //else
                    //{
                    //    foreach (int x in InventoryTransferHeaderModel.DDWdocentry.Select(x => x.DocEntry).Distinct())
                    //    {
                    //        InventoryTransferHeaderModel.oLineNums = "";

                    //        foreach (var y in InventoryTransferHeaderModel.DDWdocentry.Where(y => y.DocEntry == x))
                    //        {
                    //            if (InventoryTransferHeaderModel.oLineNums == "")
                    //            {
                    //                InventoryTransferHeaderModel.oLineNums = "'" + y.LineEntry.ToString() + "'";
                    //            }
                    //            else
                    //            {
                    //                InventoryTransferHeaderModel.oLineNums += ",'" + y.LineEntry.ToString() + "'";
                    //            }
                    //        }

                    //        if (query == "")
                    //        {
                    //            query = oSelQry1 +
                    //                    $" Where T0.DocEntry = {x} " +
                    //                    $" and T0.LineNum IN(" + InventoryTransferHeaderModel.oLineNums + ") ";
                    //        }
                    //        else
                    //        {
                    //            query += " UNION ALL " +
                    //                     oSelQry1 +
                    //                     $" Where T0.DocEntry = {x} " +
                    //                     $" and T0.LineNum IN(" + InventoryTransferHeaderModel.oLineNums + ") ";
                    //        }

                    //    }

                    //}

                    //LINE ITEMS
                    DataTable dtIT_Items = sapHanaAccess.Get(queryItems);

                    for (int x = 0; x < dtIT_Items.Rows.Count; x++)
                    {

                        Double LineTotal;
                        Double GrossTotal;
                        Double VatAmount;
                        Double GrossPrice;
                        Double PriceVatInc;
                        Double DiscAmt;
                        Double Discount;

                        double qty = Convert.ToDouble(dtIT_Items.Rows[x]["Quantity"]);
                        double discount = Convert.ToDouble(dtIT_Items.Rows[x]["DiscPrcnt"]);
                        double price = Convert.ToDouble(dtIT_Items.Rows[x]["Price"]);
                        double vatrate = Convert.ToDouble(dtIT_Items.Rows[x]["VatPrcnt"]);
                        double priceaftvat = Convert.ToDouble(dtIT_Items.Rows[x]["PriceAfVat"]);
                        double pricebefdisc = Convert.ToDouble(dtIT_Items.Rows[x]["PriceBefDi"]);


                        PriceVatInc = price + (price * (vatrate / 100));
                        DiscAmt = pricebefdisc * (discount / 100);
                        LineTotal = (qty * pricebefdisc);
                        //VatAmount = LineTotal * (vatrate / 100);

                        VatAmount = LineTotal * (vatrate / 100) - ((LineTotal * (vatrate / 100)) * (discount / 100));

                        GrossTotal = (LineTotal + VatAmount) - (DiscAmt * qty);
                        GrossPrice = PriceVatInc - (PriceVatInc * (discount / 100));


                        Discount = priceaftvat / (1 - (discount / 100));

                        InventoryTransferItemsModel.ITitems.Add(new InventoryTransferItemsModel.ITitemsData
                        {
                            ObjType = InventoryTransferHeaderModel.selObjType,
                            BaseLine = dtIT_Items.Rows[x]["LineNum"].ToString(),
                            BaseEntry = dtIT_Items.Rows[x]["DocEntry"].ToString(),
                            ItemCode = dtIT_Items.Rows[x]["ItemCode"].ToString(), // ItemCode
                            ItemName = dtIT_Items.Rows[x]["Dscription"].ToString(),
                            BarCode = dtIT_Items.Rows[x]["CodeBars"].ToString(),
                            //BrandCode = dtIT_Items.Rows[x]["BrandCode"].ToString(),
                            BrandCode = dtIT_Items.Rows[x]["Quantity"].ToString(),
                            Brand = dtIT_Items.Rows[x]["BrandName"].ToString(),
                            StyleCode = dtIT_Items.Rows[x]["U_StyleCode"].ToString(), //Style
                            Style = dtIT_Items.Rows[x]["U_StyleCode"].ToString(),
                            ColorCode = dtIT_Items.Rows[x]["U_Color"].ToString(), //Color
                            Color = dtIT_Items.Rows[x]["ColorName"].ToString(), //Color
                            Size = dtIT_Items.Rows[x]["U_Size"].ToString(), //Size
                            Section = dtIT_Items.Rows[x]["U_Section"].ToString(), //Section
                            Quantity = Convert.ToDouble(dtIT_Items.Rows[x]["Quantity"]), //Requested to remove logic 07212021 //InventoryTransferRequestService.GetOrigQty(dtIT_Items.Rows[x]["ItemCode"].ToString(), Convert.ToDouble(dtIT_Items.Rows[x]["Quantity"])), //Qty
                            FWhsCode = dtIT_Items.Rows[x]["FromWhsCod"].ToString(), //FromWHsCode
                            TWhsCode = dtIT_Items.Rows[x]["WhsCode"].ToString(), //ToWHsCode
                            TaxCode = dtIT_Items.Rows[x]["VatGroup"].ToString(), //Tax Code
                            TaxAmount = VatAmount,
                            TaxRate = 0,
                            UnitPrice = Convert.ToDouble(dtIT_Items.Rows[x]["PriceBefDi"]), //Convert.ToDouble(dtGRPO.Rows[x]["PriceBefDi"]),
                            LineTotal = LineTotal,
                            GrossTotal = GrossTotal, // Convert.ToDouble(dtGRPO.Rows[x]["PriceAfVat"]),
                            DiscountPerc = Convert.ToDouble(dtIT_Items.Rows[x]["DiscPrcnt"]),
                            DiscountAmount = DiscAmt * qty,
                            GrossPrice = price, //GrossPrice,
                            Company = dtIT_Items.Rows[x]["U_Company"].ToString(),
                            SKU = dtIT_Items.Rows[x]["U_SKU"].ToString(),
                            SortCode = dtIT_Items.Rows[x]["U_ID023"].ToString(),
                            InventoryUOM = dtIT_Items.Rows[x]["InvntryUom"].ToString()
                        });
                    }

                    //if (InventoryTransferHeaderModel.DDWdocentry.Count == 1)
                    //{
                    //    frmUDF.LoadUDF(oSelCode, "ITR");
                    //}

                }

                LoadData(_frmIT.dgvItem);
                LoadData(_frmIT.dgvPreviewItem);
            }

        }

        public void LoadUDFDataFromPickList(string DocEntry)
        {
            try
            {
                DataTable dtUDF = GetUDF();
                string dtQuery = "";
                foreach (DataRow dr in dtUDF.Rows)
                { dtQuery += $", {dr["AliasID"].ToString()}"; }

                var udfFields = dtQuery.Remove(0, 1);

                var dt = new DataTable();
                var dt2 = new DataTable();

                dt = sapHanaAccess.Get($"SELECT TOP 1 C.U_PONo FROM PKL1 A LEFT JOIN WTQ1 B ON A.OrderEntry = B.DocEntry LEFT JOIN OWTQ C ON B.DocEntry = C.DocEntry  Where A.AbsEntry = '{DocEntry}'");
                //dt2 = DataAccess.Select(DataAccess.conStr("HANA"), $"SELECT TOP 1 C.U_Remarks, C.U_OrderType FROM PKL1 A LEFT JOIN WTQ1 B ON A.OrderEntry = B.DocEntry LEFT JOIN OWTQ C ON B.DocEntry = C.DocEntry  Where A.AbsEntry = '{DocEntry}'", frmMain);
                dt2 = sapHanaAccess.Get($"SELECT TOP 1 C.U_Remarks FROM PKL1 A LEFT JOIN WTQ1 B ON A.OrderEntry = B.DocEntry LEFT JOIN OWTQ C ON B.DocEntry = C.DocEntry  Where A.AbsEntry = '{DocEntry}'");

                foreach (DataRow dr in dtUDF.Rows)
                {
                    string oUDF = dr["AliasID"].ToString();
                    if (dr["AliasID"].ToString() == "U_PONo")
                    {
                        DECLARE.udf.Add(new DECLARE.UDF
                        {
                            ObjCode = InventoryTransferHeaderModel.selObjType,
                            FieldCode = oUDF,
                            FieldValue = dt.Rows[0][0].ToString()
                        });
                    }
                    else if (dr["AliasID"].ToString() == "U_PrepBy")
                    {
                        DECLARE.udf.Add(new DECLARE.UDF
                        {
                            ObjCode = InventoryTransferHeaderModel.selObjType,
                            FieldCode = oUDF,
                            FieldValue = sboCredentials.UserId
                        });
                    }
                    else if (dr["AliasID"].ToString() == "U_Remarks")
                    {
                        DECLARE.udf.Add(new DECLARE.UDF
                        {
                            ObjCode = InventoryTransferHeaderModel.selObjType,
                            FieldCode = oUDF,
                            FieldValue = dt2.Rows[0][0].ToString()
                        });
                    }
                    //else if (dr["AliasID"].ToString() == "U_OrderType")
                    //{
                    //    DECLARE.udf.Add(new DECLARE.UDF
                    //    {
                    //        ObjCode = InventoryTransferHeaderModel.selObjType,
                    //        FieldCode = oUDF,
                    //        FieldValue = dt2.Rows[0][1].ToString()
                    //    });
                    //}
                }

                LoadUdf(_frmIT.UDF, dtUDF);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Delete row
        public void DeleteRow(object sender, EventArgs e)
        {
            if (_frmIT.btnRequest.Text == "Add")
            {
                var table = _frmIT.dgvPreviewItem.Focus() ? _frmIT.dgvPreviewItem : _frmIT.dgvItem;
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to remove selected item?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in table.Rows)
                    {
                        if (row.Selected == true)
                        {
                            var index = row.Index;
                            string StyleName, ColorName, ItemCode;

                            ItemCode = _frmIT.dgvPreviewItem.Rows[index].Cells["Item No."].Value.ToString();
                            StyleName = _frmIT.dgvPreviewItem.Rows[index].Cells["Style"].Value.ToString();
                            ColorName = _frmIT.dgvPreviewItem.Rows[index].Cells["Color"].Value.ToString();

                            //Remove items
                            InventoryTransferItemsModel.ITitems.RemoveAll(x => x.ObjType == InventoryTransferHeaderModel.selObjType &&
                                                            x.Style == StyleName &&
                                                            x.Color == ColorName &&
                                                            x.ItemCode == ItemCode);
                        }
                    }

                    //Rebind data
                    LoadData(_frmIT.dgvItem);
                    LoadData(_frmIT.dgvPreviewItem);
                }
            }
        }
        #endregion

        #region close document
        public void CloseDocument(object sender, EventArgs e)
        {
            ClearData();
        }
        #endregion

        #region New Document
        public void NewDocument(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Creating new document may clear all the content. Do you wish to continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ClearData();
                _frmIT.tabIT.SelectedIndex = 0;
            }
        }
        #endregion

        #region form closing
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
        #endregion

        #region Cell end edit
        public void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_frmIT.dgvItem.Columns[e.ColumnIndex].Name == "Quantity" || _frmIT.dgvItem.Columns[e.ColumnIndex].Name == "Info Price")
            {
                string ItemCode = _frmIT.dgvItem.Rows[e.RowIndex].Cells["Item No."].Value.ToString();
                var ITList = InventoryTransferItemsModel.ITitems.First(x => x.ItemCode == ItemCode);

                if (_frmIT.dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Any(char.IsDigit) == false)
                {
                    if (_frmIT.dgvItem.Columns[e.ColumnIndex].Name == "Quantity")
                    {
                        string oQty = ITList.Quantity.ToString();
                        _frmIT.dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oQty;
                        _frmIT.dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = Convert.ToDouble(oQty); //Requested to remove logic 07212021 //GetCartonQty(ItemCode, Convert.ToDouble(oQty));
                    }
                    else if (_frmIT.dgvItem.Columns[e.ColumnIndex].Name == "Info Price")
                    {
                        string oGPrice = ITList.GrossPrice.ToString();
                        _frmIT.dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oGPrice;
                    }
                }
                else
                {
                    string oQty = _frmIT.dgvItem.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                    _frmIT.dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = Convert.ToDouble(oQty); //Requested to remove logic 07212021 //GetCartonQty(ItemCode, Convert.ToDouble(oQty));
                    ComputeTotal();
                }
            }
        }

        #endregion

        #region Preview search
        public void PreviewSearch(object sender, EventArgs e)
        {
            DataHelper.RowSearch(_frmIT.dgvPreviewItem, _frmIT.txtSearch.Text, 0);
        }
        #endregion

        #region search document
        private void SearchDocumentTextChange(object sender, EventArgs e)
        {
            DataHelper.RowSearch(_frmIT.dgvFindDocument, _frmIT.txtSearchDocument.Text, InventoryTransfer.oRowIndex);

        }
        #endregion


        /// <summary>
        /// Search Modal
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="Parameters"></param>
        /// <param name="title"></param>
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

        public List<string> SelectCompany()
        {
            List<string> parameters = new List<string>()
            {
                "C"
            };

            List<string> m = Modal("@CMP_INFO", null, "List of Companies");
            return m;
        }

        void InventoryCopy(object sender, PreviewKeyDownEventArgs e)
        {
            var table = _frmIT.dgvPreviewItem.Focus() ? _frmIT.dgvPreviewItem : _frmIT.dgvItem;

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

        void InventoryUDFscroll(object sender, ScrollEventArgs e)
        {
            if (oDateTimePicker.Visible == true)
            {
                oDateTimePicker.Visible = false;
            }
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

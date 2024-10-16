using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using zDeclare;
using DirecLayer;
using PresenterLayer.Helper;
using PresenterLayer.Views.Data_Transfer.UC.UDF;

namespace PresenterLayer.Views
{
    public partial class frmDT_UDF : MetroForm
    {
        DECLARE dc = new DECLARE();
        DataAccess da = new DataAccess();
        DataTable dtIT = new DataTable();
        DataTable dtIT_Items = new DataTable();
        DataTable dtUDF = new DataTable();
        private static DateTimePicker oDateTimePicker = new DateTimePicker();

        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        public static string oSeries { get; set; }
        public static string oSeriesName { get; set; }
        public static string frmWhs { get; set; }
        public static string ToWhs { get; set; }
        public static string DocDate { get; set; }
        public static string DocDueDate { get; set; }
        public static string TaxDate { get; set; }
        public static string Vat { get; set; }
        public static string TransactionType { get; set; }
        public static string TargetWarehouse { get; set; }
        public static string LastWarehouse { get; set; }
        public static string DocumentDate { get; set; }
        public static string CardCode { get; set; }
        public static string DocumentType { get; set; }
        public static int MaxLine { get; set; }
        public static string DocumentDate2 { get; set; }
        public static string PostingDate { get; set; }
        public string objType { get; set; }
        public string objCode { get; set; }
        public string uploadType { get; set; }

        
        public frmDT_UDF()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            helper = new DataHelper();
        }

        private void frmDT_UDF_Load(object sender, EventArgs e)
        {
            pnlContainer.Controls.Clear();

            switch (objType)
            {
                case "ODLN":
                    pnlContainer.Controls.Add(new UcUDF_delivery());
                    objCode = "15";
                    break;

                case "ORDR":
                    pnlContainer.Controls.Add(new UcUDF_salesorder());
                    objCode = "17";
                    break;

                case "OWTQ":
                    pnlContainer.Controls.Add(new UcUDF_itr());
                    objCode = "1250000001";
                    break;

                case "OQUT":
                    objCode = "23";
                    break;

                case "OCTN":
                    pnlContainer.Controls.Add(new UcUDF_carton());
                    objCode = "";
                    break;
                case "OINV":
                    pnlContainer.Controls.Add(new UcUDF_invoice());
                    objCode = "13";
                    break;
                case "ORIN":
                    pnlContainer.Controls.Add(new UcUDF_creditmemo());
                    objCode = "14";
                    break;
            }

            if (objType != "OCTN")
            {
                dataGridLayout(gvUDF);
                _LoadData();
            }

            if (oDateTimePicker.IsDisposed)
            {
                oDateTimePicker = new DateTimePicker();
            }
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

        private DataTable GetUdfTableValues(string value)
        {
            string query = $"SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [{value}]";
            return hana.Get(query);
        }

        private DataTable GetUdfValues(string value)
        {
            string query = "SELECT ''[Code] ,''[Name] UNION SELECT FldValue, Descr " +

                "FROM UFD1 " +

                $"WHERE TableID = '{objType}' AND FieldID = '{value}'";
            return hana.Get(query);
        }

        public void _LoadData()
        {
            var UDF = SP.UDF(objType);

            dtUDF = hana.Get(UDF);

            gvUDF.Rows.Clear();

            LoadUdf(gvUDF, dtUDF);
        }

        private void gvUDF_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvUDF_SelectionChanged(object sender, EventArgs e)
        {
            this.gvUDF.ClearSelection();
        }

        public static void dataGridLayout(DataGridView dgv)
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

            dgv.ColumnCount = 3;
            dgv.Columns[0].Name = "Code";
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[1].Name = "Name";
            dgv.Columns[2].Name = "Field";
            dgv.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(191, 235, 245);
        }

        void ConvertToDate(DataGridView dgv)
        {
            var row = dgv.CurrentRow;
            string fieldName = row.Cells[0].Value.ToString();
            var curCell = dgv.CurrentCell.ColumnIndex;

            if (curCell == 2)
            {
                if (fieldName.Contains("Date") || fieldName.Contains("date"))
                {
                    //oDateTimePicker = new DateTimePicker();

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

        }

        void CreateDateTimePicker(DateTimePicker dtPicker, DataGridView dgv, DataGridViewRow row)
        {
            dtPicker.Format = DateTimePickerFormat.Short;

            Rectangle oRectangle = dgv.GetCellDisplayRectangle(2, row.Index, true);

            dtPicker.Size = new Size(oRectangle.Width, oRectangle.Height);
            dtPicker.Location = new Point(oRectangle.X, oRectangle.Y);
            dtPicker.Visible = true;

            dtPicker.CloseUp += new EventHandler(dateTimePicker_CloseUp);
            //dtPicker.Visible = false;            
        }

        List<string> Modal(string searchKey, List<string> Parameters, string title)
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

        string SelectEmployee()
        {
            string result = "";
            var list = Modal("EmployeeList *", null, "List of Employee");
            if (list.Count > 0)
            {
                result = list[0];
            }
            return result;
        }

        string SelectUDFFMS(string table, string AliasID)
        {
            string result = "";

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

        string GetITRqry(string AliasID)
        {
            var GetUDF_FMS = hana.Get(SP.UDF_FMS);
            string ItrUDF_FmsQry = helper.ReadDataRow(GetUDF_FMS, 1, "", 0);
            //string query = sapHanaAccess.Get(string.Format(ItrUDF_FmsQry, "1250000940", sFieldID)).Rows[0]["QString"].ToString();
            string query = hana.Get(string.Format(ItrUDF_FmsQry, "1250000940", AliasID)).Rows[0]["QString"].ToString();

            return query;
        }
        private void gvUDF_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string fieldName = gvUDF.CurrentRow.Cells[0].Value.ToString();
            var row = e.RowIndex;
            var col = 1;
            string result = "";

            if (fieldName.Contains("date") || fieldName.Contains("Date"))
            {
                //_udfRepo.ConvertToDate(_frmITR.Udf);
                ConvertToDate(gvUDF);
                gvUDF.CurrentCell = gvUDF[col, row];
                //gvUDF[0, row].Focus();
                gvUDF.BeginEdit(true);
            }
            else if (fieldName.Contains("by") || fieldName.Contains("By"))
            {

                var code = GetITRqry(fieldName).Contains("$") ? SelectEmployee() : SelectUDFFMS(objType, fieldName);

                //x.Cells[2].Value = m["Address2"];
                gvUDF.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[0].Value != null)
                    {
                        if (x.Cells[0].Value.ToString() == fieldName)
                        {

                            x.Cells[2].Value = code;
                            //_frmITR.UDF.CurrentCell = x.Cells[2];
                            //_frmITR.UDF.Rows[row].Cells[col];
                            
                            gvUDF.CurrentCell = gvUDF[col, row];
                            gvUDF.BeginEdit(true);
                        }
                    }
                });

            }
            else if (fieldName.Contains("Carton") || fieldName.Contains("carton"))
            {
                var code = SelectCartonList();
                //x.Cells[2].Value = m["Address2"];
                gvUDF.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[0].Value != null)
                    {
                        if (x.Cells[0].Value.ToString() == fieldName)
                        {

                            x.Cells[2].Value = code;
                            //_frmITR.UDF.CurrentCell = x.Cells[2];
                            gvUDF.CurrentCell = gvUDF[col, row];
                            gvUDF.BeginEdit(true);
                        }
                    }
                });
            }

            else if (fieldName.Equals("U_VDesc"))
            {
                //Vehicle Desc
                var code = SelectVehicle();
                gvUDF.Rows.Cast<DataGridViewRow>().ToList()
                .ForEach(x =>
                {
                    if (x.Cells[0].Value != null)
                    {
                        if (x.Cells[0].Value.ToString() == fieldName)
                        {

                            x.Cells[2].Value = code == "" ? x.Cells[2].Value : code;

                            //_frmITR.UDF.CurrentCell = x.Cells[2];
                            gvUDF.CurrentCell = gvUDF[col, row];
                            gvUDF.BeginEdit(true);
                        }
                    }
                });

                if (code != "")
                {
                    DataTable dt1 = GetData($"SELECT U_VPla, U_VDriver FROM [@TRUCK] WHERE U_VDesc = '{code}'");
                    gvUDF.Rows.Cast<DataGridViewRow>().ToList()
                    .ForEach(x =>
                    {
                        if (x.Cells[0].Value != null)
                        {
                            if (x.Cells[0].Value.ToString() == fieldName)
                            {
                                var vcode = code == "" ? x.Cells[2].Value : code;
                                //x.Cells[2].Value = $"{vcode} - {dt1.Rows[0]["U_VPla"].ToString()}";
                                x.Cells[2].Value = $"{vcode}";

                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                gvUDF.CurrentCell = gvUDF[col, row];
                                gvUDF.BeginEdit(true);
                            }
                            else if (x.Cells[0].Value.ToString() == "U_VPla")
                            {

                                x.Cells[2].Value = dt1.Rows[0]["U_VPla"].ToString();

                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                gvUDF.CurrentCell = gvUDF[col, row];
                                gvUDF.BeginEdit(true);
                            }
                            else if (x.Cells[0].Value.ToString() == "U_Driver")
                            {
                                x.Cells[2].Value = dt1.Rows[0]["U_VDriver"].ToString();
                                //_frmITR.UDF.CurrentCell = x.Cells[2];
                                gvUDF.CurrentCell = gvUDF[col, row];
                                gvUDF.BeginEdit(true);
                            }
                        }
                    });

                }
            }
        }


        DataTable GetData(string query)
        {
            DataTable queryReturn = hana.Get(query);
            return queryReturn;
        }

        string SelectCartonList()
        {
            string result = "";
            var list = Modal("CartonList", null, "List of CartonList");
            if (list.Count > 0)
            {
                result = list[0];
            }
            return result;
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
        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            if (gvUDF.Rows.Count > 0)
            {
                var index = gvUDF.CurrentRow.Index;
                var dtPicker = (DateTimePicker)sender;
                var dtPickerValue = dtPicker.Value.ToShortDateString();

                if (DECLARE.udf.Exists(a => a.FieldCode == DECLARE._DataGridReplace(gvUDF, index, "Code", "0")))
                {
                    //If exist update
                    foreach (var x in DECLARE.udf.Where(a => a.FieldCode == DECLARE._DataGridReplace(gvUDF, index, "Code", "0")))
                    {
                        //x.FieldValue = DECLARE._DataGridReplace(gvUDF, index, "Field", "0"); //dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                        x.FieldValue = dtPickerValue;
                        //gvUDF.CurrentCell.Value = dtPickerValue;
                        gvUDF[2,index].Value = dtPickerValue;
                    }
                }
                else
                {
                    DECLARE.udf.Add(new DECLARE.UDF
                    {
                        ObjCode = objType,
                        FieldCode = DECLARE._DataGridReplace(gvUDF, index, "Code", "0"),
                        FieldName = DECLARE._DataGridReplace(gvUDF, index, "Name", "0"),
                        FieldValue = dtPickerValue
                        //FieldValue = DECLARE._DataGridReplace(gvUDF, index, "Field", "0")
                    });
                    //gvUDF.CurrentCell.Value = dtPickerValue;
                    gvUDF[2, index].Value = dtPickerValue;
                }
                dtPicker.Visible = false;
            }
        }

        private void GvUDF_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (gvUDF.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                gvUDF.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void GvUDF_Leave(object sender, EventArgs e)
        {
            if (oDateTimePicker != null)
            {
                if (oDateTimePicker.Visible == true)
                {
                    oDateTimePicker.Visible = false;
                }
            }

        }

        private void GvUDF_CellEndEdit_1(object sender, DataGridViewCellEventArgs e)
        {
            if (gvUDF.Rows.Count > 0)
            {
                var index = gvUDF.CurrentRow.Index;

                if (DECLARE.udf.Exists(a => a.FieldCode == DECLARE._DataGridReplace(gvUDF, index, "Code", "0")))
                {
                    //If exist update
                    foreach (var x in DECLARE.udf.Where(a => a.FieldCode == DECLARE._DataGridReplace(gvUDF, index, "Code", "0")))
                    {
                        x.FieldValue = DECLARE._DataGridReplace(gvUDF, index, "Field", "0"); //dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    }
                }
                else
                {
                    DECLARE.udf.Add(new DECLARE.UDF
                    {
                        ObjCode = objType,
                        FieldCode = DECLARE._DataGridReplace(gvUDF, index, "Code", "0"),
                        FieldName = DECLARE._DataGridReplace(gvUDF, index, "Name", "0"),
                        FieldValue = DECLARE._DataGridReplace(gvUDF, index, "Field", "0")
                    });
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using DirecLayer._03_Repository;
using PresenterLayer;
using PresenterLayer.Views;
using PresenterLayer.Helper;

namespace DirecLayer._05_Repository
{
    public class UdfRepository
    {
        StringQueryRepository _Repo = new StringQueryRepository();
        private static DateTimePicker oDateTimePicker = new DateTimePicker();

        DataGridView Dgv { get; set; }

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

            if (oDateTimePicker.IsDisposed)
            {
                oDateTimePicker = new DateTimePicker();
            }
        }

        public void LoadUdf(DataGridView dgv, DataTable udf, string TableID)
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

                if (dr["UserDefined"].ToString() == string.Empty)
                {
                    DataTable values = GetUdfValues(TableID, dr["FieldID"].ToString());

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
                        //if (dr["AliasID"].ToString() == "U_PrepBy")
                        //{
                        //    dgv.Rows[i].Cells["Field"].Value = DomainLayer.Models.EasySAPCredentialsModel.ESUserId;
                        //    dgv.Rows[i].Cells["Field"].ReadOnly = true;
                        //}
                    }
                }
                else
                {
                    if (dr["UserDefined"].ToString() == "@CartonList")
                    {
                        dgv.Rows[i].Cells["Field"] = new DataGridViewTextBoxCell();
                    }
                    else
                    {

                        DataTable tableValues = GetUdfTableValues(dr["UserDefined"].ToString());

                        cmb.DisplayMember = "Name";
                        cmb.ValueMember = "Code";
                        cmb.DataSource = tableValues;

                        dgv.Rows[i].Cells["Field"] = cmb;

                    }
                }

                i++;
            }
        }

        public void ConvertToDate(DataGridView dgv)
        {
            var row = dgv.CurrentRow;
            string fieldName = row.Cells[0].Value.ToString();

            if (fieldName.Contains("Date") || fieldName.Contains("date"))
            {
                //DateTimePicker oDateTimePicker = new DateTimePicker();

                dgv[2, row.Index].Value = DateTime.Now.Date.ToString("MM/dd/yyyy");
                dgv.Controls.Add(oDateTimePicker);
                CreateDateTimePicker(oDateTimePicker, dgv, row);
                Dgv = dgv;
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
            try
            {
                if (Dgv != null)
                {
                    var dtPicker = (DateTimePicker)sender;
                    var qe = dtPicker.Value.ToShortDateString();
                    Dgv.CurrentCell.Value = qe;
                    oDateTimePicker.Visible = false;
                }
            }
            catch(Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        public void SelectUdfDratf(DataGridView dgv, string docEntry, string table)
        {
            var array = dgv.Rows.Cast<DataGridViewRow>().Select(x => x.Cells[0].Value).ToArray();
            var fields = string.Join(",", array);

            DataTable udf = DataRepository.GetData($"SELECT {fields} FROM {table} A WHERE DocEntry = '{docEntry}'");

            if (udf.Rows.Count > 0)
            {
                int udfIndex = 0;
                foreach (DataColumn col in udf.Columns)
                {
                    try
                    {
                        string value = col.ColumnName.Contains("Date") || col.ColumnName.Contains("date") ? Convert.ToDateTime(udf.Rows[0][col.ColumnName]).ToShortDateString() : udf.Rows[0][col.ColumnName].ToString();
                        dgv.Rows[udfIndex].Cells[2].Value = value;
                    }
                    catch { }
                    udfIndex++;
                }
            }
        }

        public void SelectUdfDratfSO(DataGridView dgv, string docEntry, string table)
        {
            var array = dgv.Rows.Cast<DataGridViewRow>().Select(x => x.Cells[0].Value).ToArray();
            var fields = string.Join(",", array);

            DataTable udf = DataRepositoryForSO.GetData($"SELECT {fields} FROM {table} A WHERE DocEntry = '{docEntry}'");

            if (udf.Rows.Count > 0)
            {
                int udfIndex = 0;
                foreach (DataColumn col in udf.Columns)
                {
                    try
                    {
                        string value = col.ColumnName.Contains("Date") || col.ColumnName.Contains("date") ? Convert.ToDateTime(udf.Rows[0][col.ColumnName]).ToShortDateString() : udf.Rows[0][col.ColumnName].ToString();
                        dgv.Rows[udfIndex].Cells[2].Value = value;
                    }
                    catch { }
                    udfIndex++;
                }
            }
        }

        private DataTable GetUdfValues(string TableID, string value)
        {
            return DataRepository.GetData(_Repo.GetUdfValidValues(TableID, value));
        }

        private DataTable GetUdfTableValues(string v)
        {
            return DataRepository.GetData(_Repo.UdfTableValues(v));
        }

        public void GetDiff(DataGridView dgv, IFrmUnofficialSales _View, string GetFieldName, string GetChangeValueFName)
        {
            var row = dgv.CurrentRow;
            string fieldName = row.Cells[0].Value.ToString();
            decimal PromoQty = 0;
            decimal QtyDiff = 0;

            if (fieldName == GetFieldName && dgv[2, row.Index].Value != null)
            {
                if (dgv[2, row.Index].Value.ToString().Any(char.IsDigit))
                {
                    PromoQty = Convert.ToDecimal(dgv[2, row.Index].Value.ToString());
                    decimal TotQty = GetFieldName == "U_PromoQuantity" ? Convert.ToDecimal(_View.TotalQuantity) : Convert.ToDecimal(_View.Total);
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

    }
}
using Context;
using DirecLayer;
using PresenterLayer.Helper;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PresenterLayer.Tools
{
    public class UDF_Form
    {
        public frm_UDF frmUDF { get; set; }
        //public SP _SP { get; set; }
        DataHelper helper { get; set; }
        SAPHanaAccess hana { get; set; }
        public string sTable { get; set; }
        public string sPublicStatic { get; set; }

        public UDF_Form()
        {
            helper = new DataHelper();
            hana = new SAPHanaAccess();
        }

        public void LoadUDF(string sTable,string sPublicStatic)
        {
            frm_UDF frm = new frm_UDF();
            frm.udf = this;
            this.sTable = sTable;
            this.sPublicStatic = sPublicStatic;
            frmUDF = frm;
            frm.MdiParent = StaticHelper._MainForm;
            frm.Show();
        }

        public void LoadForm()
        {
            dgvSetup(frmUDF.dgvUDF);
            LoadData();
        }

        void dgvSetup(DataGridView dgv)
        {
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgv.ReadOnly = false;
            dgv.MultiSelect = false;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersVisible = false;
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv.AllowUserToResizeColumns = false;
            dgv.ColumnCount = 4;
            dgv.Columns[0].Name = "Code";
            dgv.Columns[1].Name = "Name";
            dgv.Columns[2].Name = "Field";
            dgv.Columns[3].Name = "Button";
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(191, 235, 245);
            dgv.Columns[3].Width = 20;
            dgv.Columns[3].ReadOnly = true;           
        }

        DataGridViewComboBoxCell SetField(DataTable dt)
        {
            DataGridViewComboBoxCell result = null;

            DataGridViewComboBoxCell cmb = new DataGridViewComboBoxCell();
            cmb.DisplayMember = "Name";
            cmb.ValueMember = "Code";
            cmb.DataSource = dt;
            result = cmb;

            return result;
        }

        DataGridViewComboBoxCell SetFieldForOneColumn(DataTable dt)
        {
            DataGridViewComboBoxCell result = null;

            DataGridViewComboBoxCell cmb = new DataGridViewComboBoxCell();
            cmb.DisplayMember = dt.Columns[0].ColumnName.ToString();
            cmb.ValueMember = dt.Columns[0].ColumnName.ToString();
            cmb.DataSource = dt;
            result = cmb;

            return result;
        }

        public void CreateDateTimePicker(DateTimePicker dtPicker, int index)
        {
            dtPicker.Format = DateTimePickerFormat.Short;
            Rectangle oRectangle = frmUDF.dgvUDF.GetCellDisplayRectangle(2, index, true);
            dtPicker.Size = new Size(oRectangle.Width, oRectangle.Height);
            dtPicker.Location = new Point(oRectangle.X, oRectangle.Y);
            dtPicker.Visible = true;

            dtPicker.CloseUp += new EventHandler(DateTimePicker_CloseUp);
        }

        private void DateTimePicker_CloseUp(object sender, EventArgs e)
        {
            if (frmUDF.dgvUDF.Rows.Count > 0)
            {
                var index = frmUDF.dgvUDF.CurrentRow.Index;
            }
        }

        public void LoadData()
        {
            var sapHana = new SAPHanaAccess();
            DataTable dt = new DataTable();
            dt = sapHana.Get(SP.UDF(sTable));
            
            frmUDF.dgvUDF.Rows.Clear();
            int index = -1;
            foreach (DataRow dr in dt.Rows)
            {
                index++;
                string sDataType = dr["TypeID"].ToString();
                string sField = dr["AliasID"].ToString();
                string[] row = new string[] { sField, dr["Descr"].ToString() };
                frmUDF.dgvUDF.Rows.Add(row);
                DataTable data = new DataTable();

                data = sapHana.Get(SP.UDFValidValues(sTable, sField));

                if (data.Rows.Count > 1)
                { frmUDF.dgvUDF.Rows[index].Cells["Field"] = SetField(data); continue; }
                
                if (!helper.ReadDataRow(hana.Get(SP.UDFQuery(sPublicStatic, sField)), 0, "", 0).Contains("$"))
                {
                    data = sapHana.Get(SP.UDFQuery(sPublicStatic, sField));

                    if (data.Rows.Count > 0)
                    {
                        string sSelectQuery = data.Rows[0]["QString"].ToString();

                        data = sapHana.Get(sSelectQuery);
                    }

                    if (LibraryHelper.DataExist(data))
                    {
                        frmUDF.dgvUDF.Rows[index].Cells["Field"] = data.Columns.Count > 1 ? SetField(data) : SetFieldForOneColumn(data); continue;
                    }
                }

                string oDflt = dr["Dflt"].ToString();
                string oUserDefined = dr["UserDefined"].ToString();


                if (oUserDefined != "")
                {
                    data = sapHana.Get(SP.UDFUserDefined(oUserDefined));

                    if (LibraryHelper.DataExist(data))
                    {
                        frmUDF.dgvUDF.Rows[index].Cells["Field"] = SetField(data);

                        if (oDflt != "")
                        { frmUDF.dgvUDF.Rows[index].Cells["Field"].Value = oDflt; }

                        continue;
                    }
                }
                else if (dr["Table"].ToString() != "")
                {
                    frmUDF.dgvUDF.Rows[index].Cells["Button"].ReadOnly = false;
                    frmUDF.dgvUDF.Rows[index].Cells["Button"] = new DataGridViewButtonCell();
                }
                else
                {
                    data = sapHana.Get(SP.UDFDetails(sTable, dr["FieldID"].ToString()));

                    if (data.Rows.Count > 1)
                    { frmUDF.dgvUDF.Rows[index].Cells["Field"] = SetField(data); }
                    else
                    {
                        if (sDataType == "D")
                        {
                            //DateTimePicker oDateTimePicker = new DateTimePicker();
                            //frmUDF.dgvUDF[2, index].Value = DateTime.Now.Date.ToString("MM/dd/yyyy");
                            //frmUDF.dgvUDF.Controls.Add(oDateTimePicker);
                            //CreateDateTimePicker(oDateTimePicker, index);
                        }
                        else
                        {
                            DataGridViewTextBoxCell txt = new DataGridViewTextBoxCell();
                            txt.MaxInputLength = int.Parse(dr["EditSize"].ToString());
                            frmUDF.dgvUDF.Rows[index].Cells["Field"] = txt;
                        }
                       
                    }
                }
            }
        }
    }
}

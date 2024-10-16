using MetroFramework.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresenterLayer.Tools
{
    public partial class frm_UDF : MetroForm
    {
        public UDF_Form udf { get; set; }
        
        public frm_UDF()
        { InitializeComponent(); }

        private void frm_UDF_Load(object sender, EventArgs e)
        { udf.LoadForm(); }

        private void frm_UDF_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void dgvUDF_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
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
            //var dtPicker = (DateTimePicker)sender;
            //var dtPickerValue = dtPicker.Value.ToShortDateString();

            //_frmITR.UDF.CurrentCell.Value = dtPickerValue;
            int index = dgvUDF.CurrentCell.RowIndex;
            DateTimePicker dtPick = (DateTimePicker)sender;
            dgvUDF.Rows[index].Cells[2].Value = dtPick.Value.ToString("MM/dd/yyyy");
            dgvUDF.CurrentCell = dgvUDF[2, index + 1];
            dgvUDF.CurrentCell = dgvUDF[2, index];
            //dtPick.Visible = false;
        }

        private void dgvUDF_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string fieldName = dgvUDF.CurrentRow.Cells[0].Value.ToString();
            var row = e.RowIndex;
            var col = e.ColumnIndex;


            if (fieldName.Contains("date") || fieldName.Contains("Date"))
            {
                //_udfRepo.ConvertToDate(_frmITR.Udf);
                ConvertToDate(dgvUDF);
                dgvUDF.ClearSelection();


                //DateTimePicker oDateTimePicker = new DateTimePicker();
                //CreateDateTimePicker(oDateTimePicker, row);

                dgvUDF.CurrentCell = dgvUDF[col, row + 1];
                dgvUDF.CurrentCell = dgvUDF[col, row];
            }
        }
    }
}

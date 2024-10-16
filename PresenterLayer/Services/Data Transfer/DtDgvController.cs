using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresenterLayer.Services
{
    public class DtDgvController
    {
        private List<string> Flows = new List<string>();
        
        public void DataGridViewSelectMap (DataGridView dgv)
        {
            DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
            col1.Name = "Id";
            dgv.Columns.Add(col1);

            DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
            col2.Name = "Map Code";
            dgv.Columns.Add(col2);

            DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
            col3.Name = "Map Description";
            dgv.Columns.Add(col3);
        }

        public void DataGridViewMapColumns(DataGridView dgv)
        {
            if (Flows.Count <= 0)
            {
                Flows.Add("Vertical");
                Flows.Add("Horizontal");
            }

            if (dgv.ColumnCount <= 0)
            {
                DataGridViewColumn col1 = new DataGridViewTextBoxColumn(); // 0
                col1.Name = "SAP Field";
                dgv.Columns.Add(col1);

                DataGridViewButtonColumn sapFieldBtn = new DataGridViewButtonColumn(); // 1
                sapFieldBtn.Text = "...";
                sapFieldBtn.UseColumnTextForButtonValue = true;
                dgv.Columns.Add(sapFieldBtn);

                //DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
                //col2.Name = "Template Field";
                //dgv.Columns.Add(col2);

                DataGridViewColumn col3 = new DataGridViewTextBoxColumn(); // 2
                col3.Name = "Type";
                dgv.Columns.Add(col3);

                DataGridViewColumn col4 = new DataGridViewTextBoxColumn(); // 3
                col4.Name = "Row Start";
                dgv.Columns.Add(col4);

                DataGridViewColumn col5 = new DataGridViewTextBoxColumn(); // 4
                col5.Name = "Column Start";
                dgv.Columns.Add(col5);

                DataGridViewComboBoxColumn col6 = new DataGridViewComboBoxColumn(); // 5
                col6.Name = "Flow";
                col6.HeaderText = "Flow";
                col6.DataSource = null;
                col6.DataSource = Flows;
                col6.ReadOnly = false;
                dgv.Columns.Add(col6);

                DataGridViewColumn col8 = new DataGridViewTextBoxColumn(); // 6
                col8.Name = "Row Interval";
                dgv.Columns.Add(col8);

                DataGridViewColumn col7 = new DataGridViewTextBoxColumn(); // 7
                col7.Name = "Column Interval";
                dgv.Columns.Add(col7);

                if (dgv.RowCount > 0)
                {
                    dgv.Rows.Clear();
                }

                dataGridLayout(dgv);
            }
        }

        public void DataGridViewMapColumns(DataGridView dgv, List<string> dtSource)
        {
            if (Flows.Count <= 0)
            {
                Flows.Add("Vertical");
                Flows.Add("Horizontal");
            }
            
            DataGridViewColumn col1 = new DataGridViewTextBoxColumn(); // 0
            col1.Name = "SAP Field";
            dgv.Columns.Add(col1);
            dgv.Columns["SAP Field"].ReadOnly = true;

            DataGridViewButtonColumn sapFieldBtn = new DataGridViewButtonColumn(); // 1
            sapFieldBtn.Text = "...";
            sapFieldBtn.UseColumnTextForButtonValue = true;
            dgv.Columns.Add(sapFieldBtn);

            DataGridViewComboBoxColumn col2 = new DataGridViewComboBoxColumn(); // 2
            col2.Name = "Template Field";
            col2.HeaderText = "Template Field";
            col2.DataSource = null;
            col2.DataSource = dtSource;
            col2.ReadOnly = false;
            dgv.Columns.Add(col2);

            DataGridViewColumn col3 = new DataGridViewTextBoxColumn(); // 3
            col3.Name = "Type";
            dgv.Columns.Add(col3);

            DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
            col4.Name = "Row Start";
            dgv.Columns.Add(col4);

            DataGridViewColumn col5 = new DataGridViewTextBoxColumn();
            col5.Name = "Column Start";
            dgv.Columns.Add(col5);

            DataGridViewComboBoxColumn col6 = new DataGridViewComboBoxColumn(); // 2
            col6.Name = "Flow";
            col6.HeaderText = "Flow";
            col6.DataSource = null;
            col6.DataSource = Flows;
            col6.ReadOnly = false;
            dgv.Columns.Add(col6);

            DataGridViewColumn col7 = new DataGridViewTextBoxColumn();
            col7.Name = "Column Interval";
            dgv.Columns.Add(col7);

            DataGridViewColumn col8 = new DataGridViewTextBoxColumn();
            col8.Name = "Row Interval";
            dgv.Columns.Add(col8);
        }

        public void dataGridLayout(DataGridView dgv)
        {
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.MultiSelect = false;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
        }
    }
}

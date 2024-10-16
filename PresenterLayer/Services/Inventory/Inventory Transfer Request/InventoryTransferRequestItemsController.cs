using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace PresenterLayer.Services.Inventory
{
    class InventoryTransferRequestItemsController
    {
        public Int32 GetSelectedItems(DataGridView dgvget, DataGridView dgvpost)
        {
            int negaQty = 0;
            int i = dgvpost.Rows.Count;

            if (dgvget.SelectedRows.Count > 0)
            {
                IEnumerable<DataGridViewRow> dgvSortInsert = dgvget.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(x => x.Index);

                foreach (DataGridViewRow dr in dgvSortInsert.ToList())
                {
                    dgvpost.Rows.Add();
                    DataGridViewRow row = dgvpost.Rows[i];

                    row.Cells[0].Value = dr.Cells[1].Value;
                    row.Cells[1].Value = dr.Cells[2].Value;
                    row.Cells[2].Value = dr.Cells[3].Value;
                    row.Cells[3].Value = "0";
                    row.Cells[4].Value = dr.Cells[5].Value;
                    row.Cells[5].Value = dr.Cells[6].Value;
                    row.Cells[6].Value = dr.Cells[7].Value;
                    row.Cells[7].Value = dr.Cells[8].Value;
                    row.Cells[8].Value = dr.Cells[9].Value;
                    row.Cells[9].Value = dr.Cells[10].Value;
                    row.Cells[10].Value = dr.Cells[11].Value;
                    row.Cells[11].Value = dr.Cells[12].Value;
                    row.Cells[12].Value = dr.Cells[13].Value;
                    row.Cells[13].Value = dr.Cells[14].Value;
                    row.Cells[14].Value = dr.Cells[0].Value;

                    dgvget.Rows.RemoveAt(dr.Index);

                    i++;
                }
                dgvpost.FirstDisplayedScrollingRowIndex = dgvpost.RowCount - 1;

                foreach (DataGridViewRow row1 in dgvpost.Rows)
                {
                    row1.HeaderCell.Value = String.Format("{0}", row1.Index + 1);
                }
                dgvpost.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            }
               
            return negaQty;
        }

        public Int32 GetAllSelectedItems(DataGridView dgvget, DataGridView dgvpost)
        {
            int negaQty = 0;
            int i = dgvpost.Rows.Count;
            foreach (DataGridViewRow dr in dgvget.Rows)
            {

                dgvpost.Rows.Add();
                int index = dr.Index;
                DataGridViewRow row = dgvpost.Rows[i];

                row.Cells[0].Value = dr.Cells[0].Value;
                row.Cells[1].Value = dr.Cells[1].Value;
                row.Cells[2].Value = dr.Cells[2].Value;
                row.Cells[3].Value = "1";
                row.Cells[4].Value = dr.Cells[4].Value;
                row.Cells[5].Value = dr.Cells[5].Value;
                row.Cells[6].Value = dr.Cells[6].Value;
                row.Cells[7].Value = dr.Cells[7].Value;
                row.Cells[8].Value = dr.Cells[8].Value;
                row.Cells[9].Value = dr.Cells[9].Value;
                row.Cells[10].Value = dr.Cells[10].Value;
                row.Cells[11].Value = dr.Cells[11].Value;
                row.Cells[12].Value = dr.Cells[12].Value;
                row.Cells[13].Value = dr.Cells[13].Value;
                i++;

                //if (Convert.ToDecimal(dr.Cells[3].Value) > 0)
                //{
                    
                //}
                //else
                //{
                //    negaQty += 1;
                //}
               
            }
            dgvget.Rows.Clear();

            return negaQty;
        }


        public void dgvSetup(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.RowHeadersVisible = true;
            dgv.MultiSelect = true;
            dgv.ColumnHeadersVisible = true;

            if (dgv.Name == "gvSelectedItem")
            {
                dgv.ColumnCount = 15;
                dgv.Columns[0].Name = "BarCode";
                dgv.Columns[1].Name = "Item No.";
                dgv.Columns[2].Name = "Item Description";
                dgv.Columns[3].Name = "Quantity";
                dgv.Columns[4].Name = "BrandCode";
                dgv.Columns[5].Name = "Brand";
                dgv.Columns[6].Name = "Style Code";
                dgv.Columns[7].Name = "Style";
                dgv.Columns[8].Name = "Color Code";
                dgv.Columns[9].Name = "Color";
                dgv.Columns[10].Name = "Size";
                dgv.Columns[11].Name = "Section";
                dgv.Columns[12].Name = "Price";
                dgv.Columns[13].Name = "Warehouse";
                dgv.Columns[14].Name = "SortCode";
            }

            foreach (DataGridViewColumn dc in dgv.Columns)
            {
                string CN = dc.Name;

                if (CN == "Quantity" && dgv.Name == "gvSelectedItem")
                {
                    dc.ReadOnly = false;
                }
                else
                {
                    dc.ReadOnly = true;
                }

                if (CN == "Available" && dgv.Name == "gvITR")
                {
                    dgv.Columns["Available"].DefaultCellStyle.Format = "#,##0.##";
                }

                if (CN == "Price")
                {
                    dgv.Columns["Price"].DefaultCellStyle.Format = "#,##0.00####";
                }

                if (dgv.Name == "gvITR")
                {
                    if (CN == "U_ID023")
                    {
                        dc.Visible = false;
                    }
                    else if (CN == "ItemCode")
                    {
                        dc.HeaderText = "Item No.";
                    }
                    else if (CN == "ItemName")
                    {
                        dc.HeaderText = "Item Description";
                    }
                }
               
            }

        }


        public static void dataGridLayout(DataGridView dgv)
        {

            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.MultiSelect = true;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;

            foreach (DataGridViewRow row1 in dgv.Rows)
            {
                row1.HeaderCell.Value = String.Format("{0}", row1.Index + 1);
            }
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;

            if (dgv.Name == "dgvItems")
            {
                dgv.Columns[0].ReadOnly = true;
                dgv.Columns[1].ReadOnly = true;
                dgv.Columns[2].ReadOnly = true;
                dgv.Columns[3].ReadOnly = false;
                dgv.Columns[4].ReadOnly = true;
                dgv.Columns[4].Visible = false;     //BrandCode
                dgv.Columns[5].ReadOnly = true;
                dgv.Columns[6].ReadOnly = true;
                dgv.Columns[6].Visible = false;     //StyleCode
                dgv.Columns[7].ReadOnly = true;
                dgv.Columns[8].ReadOnly = true;
                dgv.Columns[8].Visible = false;     //ColorCode
                dgv.Columns[9].ReadOnly = true;
                dgv.Columns[10].ReadOnly = true;
                dgv.Columns[11].ReadOnly = true;
                dgv.Columns[12].ReadOnly = false;
                dgv.Columns[13].ReadOnly = true;
                dgv.Columns[14].ReadOnly = false;
                dgv.Columns[15].ReadOnly = true;
                dgv.Columns[16].ReadOnly = false;
                dgv.Columns[20].Visible = false;
                dgv.Columns[21].Visible = false;
                dgv.Columns[22].Visible = true;
            }
        }


        public static void DataGridViewITRData(DataGridView gvITR)
        {
            if (gvITR.Columns.Count > 0)
            {
                gvITR.Columns.Clear();
                gvITR.Rows.Clear();
            }

            DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
            col1.Name = "Barcode";
            gvITR.Columns.Add(col1);
            gvITR.Columns["BarCode"].ReadOnly = true;

            DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
            col2.Name = "Item No.";
            gvITR.Columns.Add(col2);
            gvITR.Columns["Item No."].ReadOnly = true;

            DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
            col3.Name = "Item Description";
            gvITR.Columns.Add(col3);
            gvITR.Columns["Item Description"].ReadOnly = true;

            DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
            col4.Name = "Info Price";
            gvITR.Columns.Add(col4);
            gvITR.Columns["Info Price"].ReadOnly = true;

            DataGridViewColumn brand2 = new DataGridViewTextBoxColumn();
            brand2.Name = "Brand";
            gvITR.Columns.Add(brand2);
            gvITR.Columns["Brand"].ReadOnly = true;

            DataGridViewColumn style1 = new DataGridViewTextBoxColumn();
            style1.Name = "Style Code";
            gvITR.Columns.Add(style1);
            gvITR.Columns["Style Code"].ReadOnly = true;

            DataGridViewColumn style2 = new DataGridViewTextBoxColumn();
            style2.Name = "Style";
            gvITR.Columns.Add(style2);
            gvITR.Columns["Style"].ReadOnly = true;

            DataGridViewColumn color1 = new DataGridViewTextBoxColumn();
            color1.Name = "Color Code";
            gvITR.Columns.Add(color1);
            gvITR.Columns["Color Code"].ReadOnly = true;

            DataGridViewColumn color2 = new DataGridViewTextBoxColumn();
            color2.Name = "Color";
            gvITR.Columns.Add(color2);
            gvITR.Columns["Color"].ReadOnly = true;

            DataGridViewColumn section1 = new DataGridViewTextBoxColumn();
            section1.Name = "Section";
            gvITR.Columns.Add(section1);
            gvITR.Columns["Section"].ReadOnly = true;

            DataGridViewColumn size1 = new DataGridViewTextBoxColumn();
            size1.Name = "Size";
            gvITR.Columns.Add(size1);
            gvITR.Columns["Size"].ReadOnly = true;

            DataGridViewColumn col13 = new DataGridViewTextBoxColumn();
            col13.Name = "Quantity";
            gvITR.Columns.Add(col13);
            gvITR.Columns["Quantity"].ReadOnly = false;

            DataGridViewColumn brand1 = new DataGridViewTextBoxColumn();
            //Formerly Brand Code
            brand1.Name = "Ordered Quantity";
            //brand1.DisplayIndex = 12;
            gvITR.Columns.Add(brand1);
            gvITR.Columns["Ordered Quantity"].ReadOnly = true;

            DataGridViewColumn col14 = new DataGridViewTextBoxColumn();
            col14.Name = "From Warehouse";
            gvITR.Columns.Add(col14);
            gvITR.Columns["From Warehouse"].ReadOnly = true;

            DataGridViewButtonColumn col15 = new DataGridViewButtonColumn();
            col15.Text = "...";
            col15.UseColumnTextForButtonValue = true;
            gvITR.Columns.Add(col15);

            DataGridViewColumn col16 = new DataGridViewTextBoxColumn();
            col16.Name = "To Warehouse";
            gvITR.Columns.Add(col16);
            gvITR.Columns["To Warehouse"].ReadOnly = true;

            DataGridViewButtonColumn col17 = new DataGridViewButtonColumn();
            col17.Text = "...";
            col17.UseColumnTextForButtonValue = true;
            gvITR.Columns.Add(col17);

            DataGridViewColumn col18 = new DataGridViewTextBoxColumn();
            col18.Name = "Company";
            gvITR.Columns.Add(col18);

            DataGridViewButtonColumn col19 = new DataGridViewButtonColumn();
            col19.Text = "...";
            col19.UseColumnTextForButtonValue = true;
            gvITR.Columns.Add(col19);

            DataGridViewColumn col20 = new DataGridViewTextBoxColumn();
            col20.Name = "SKU";
            gvITR.Columns.Add(col20);

            DataGridViewColumn col21 = new DataGridViewTextBoxColumn();
            col21.Name = "LineNum";
            gvITR.Columns.Add(col21);

            DataGridViewColumn col22 = new DataGridViewTextBoxColumn();
            col22.Name = "SortCode";
            gvITR.Columns.Add(col22);

            DataGridViewColumn col23 = new DataGridViewTextBoxColumn();
            col23.Name = "UOM";
            col23.DisplayIndex = 13;
            gvITR.Columns.Add(col23);
            gvITR.Columns["UOM"].ReadOnly = true;

            DataGridViewColumn col24 = new DataGridViewTextBoxColumn();
            col24.Name = "Chain";
            //col24.DisplayIndex = 14;
            gvITR.Columns.Add(col24);
            //gvITR.Columns["UOM"].ReadOnly = true;

            DataGridViewColumn col25 = new DataGridViewTextBoxColumn();
            col25.Name = "Chain Description";
            //col25.DisplayIndex = 15;
            gvITR.Columns.Add(col25);
            //gvITR.Columns["UOM"].ReadOnly = true;

            DataGridViewButtonColumn col26 = new DataGridViewButtonColumn();
            col26.Text = "...";
            col26.UseColumnTextForButtonValue = true;
            gvITR.Columns.Add(col26);
        }

    }
}

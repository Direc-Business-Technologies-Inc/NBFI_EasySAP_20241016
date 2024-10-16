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

namespace PresenterLayer.Services.Inventory.Inventory_Transfer
{
    class InventoryTransferItemsController
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

                    //if (Convert.ToDecimal(dr.Cells[4].Value) > 0)
                    //{
                        dgvpost.Rows.Add();
                        DataGridViewRow row = dgvpost.Rows[i];

                        row.Cells[0].Value = dr.Cells[1].Value;
                        row.Cells[1].Value = dr.Cells[2].Value;
                        row.Cells[2].Value = dr.Cells[3].Value;
                        row.Cells[3].Value = "1";
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
                    //}
                    //else
                    //{
                    //    negaQty += 1;
                    //}

                }
                //gvSelectedItem.FirstDisplayedScrollingRowIndex = gvSelectedItem.RowCount - 1;
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

            dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;

            if (dgv.Name != "gvITddw")
            {
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

                    if (CN == "Available" && dgv.Name == "gvIT")
                    {
                        dgv.Columns["Available"].DefaultCellStyle.Format = "#,##0.##";
                    }

                    if (CN == "Price")
                    {
                        dgv.Columns["Price"].DefaultCellStyle.Format = "#,##0.00####";
                    }

                    if (dgv.Name == "gvIT")
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
            else
            {
                dgv.ColumnCount = 13;

                dgv.Columns[0].Name = "Base Document";
                dgv.Columns[0].ReadOnly = true;
                dgv.Columns[1].Name = "Line No.";
                dgv.Columns[1].ReadOnly = true;
                dgv.Columns[2].Name = "Item No.";
                dgv.Columns[2].ReadOnly = true;
                dgv.Columns[3].Name = "Item Description";
                dgv.Columns[3].ReadOnly = true;
                dgv.Columns[4].Name = "Quantity";
                dgv.Columns[4].ReadOnly = true;
                dgv.Columns[5].Name = "From Warehouse";
                dgv.Columns[5].ReadOnly = true;
                dgv.Columns[6].Name = "To Warehouse";
                dgv.Columns[6].ReadOnly = true;
                dgv.Columns[7].Name = "BrandName";
                dgv.Columns[7].ReadOnly = true;
                dgv.Columns[8].Name = "StyleName";
                dgv.Columns[8].ReadOnly = true;
                dgv.Columns[9].Name = "ColorName";
                dgv.Columns[9].ReadOnly = true;
                dgv.Columns[10].Name = "OrderEntry";
                dgv.Columns[10].Visible = false;
                dgv.Columns[11].Name = "Company";
                dgv.Columns[11].DisplayIndex = 0;
                dgv.Columns[12].Name = "BPCode";
                dgv.Columns[12].ReadOnly = true;
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
                dgv.Columns[4].Visible = false;     //Brand Code
                dgv.Columns[5].ReadOnly = true;
                dgv.Columns[6].ReadOnly = true;
                dgv.Columns[6].Visible = false;     //Style Code
                dgv.Columns[7].ReadOnly = true;
                dgv.Columns[8].ReadOnly = true;
                dgv.Columns[8].Visible = false;     //Color Code
                dgv.Columns[9].ReadOnly = true;
                dgv.Columns[10].ReadOnly = true;
                dgv.Columns[11].ReadOnly = true;
                dgv.Columns[12].ReadOnly = false;
                dgv.Columns[13].ReadOnly = true;
                dgv.Columns[14].ReadOnly = false;
                dgv.Columns[15].ReadOnly = true;
                dgv.Columns[16].ReadOnly = false;
                dgv.Columns[20].Visible = false;
                dgv.Columns[25].Visible = true;
                dgv.Columns[26].Visible = true;

            }
        }


        public static void DataGridViewITData(DataGridView gvIT)
        {
            if (gvIT.Columns.Count > 0)
            {
                gvIT.Columns.Clear();
                gvIT.Rows.Clear();
            }

            //DataGridViewButtonColumn printingBTN = new DataGridViewButtonColumn();
            //printingBTN.HeaderText = "";
            //printingBTN.Text = "Select";
            //printingBTN.UseColumnTextForButtonValue = true;
            //gvITR.Columns.Add(printingBTN);

            DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
            col1.Name = "BarCode";
            gvIT.Columns.Add(col1);
            gvIT.Columns["BarCode"].ReadOnly = true;

            DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
            col2.Name = "Item No.";
            gvIT.Columns.Add(col2);
            gvIT.Columns["Item No."].ReadOnly = true;

            DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
            col3.Name = "Item Description";
            gvIT.Columns.Add(col3);
            gvIT.Columns["Item Description"].ReadOnly = true;

            DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
            col4.Name = "Info Price";
            gvIT.Columns.Add(col4);
            gvIT.Columns["Info Price"].ReadOnly = false;

            DataGridViewColumn brand2 = new DataGridViewTextBoxColumn();
            brand2.Name = "Brand";
            gvIT.Columns.Add(brand2);
            gvIT.Columns["Brand"].ReadOnly = true;

            DataGridViewColumn style1 = new DataGridViewTextBoxColumn();
            style1.Name = "Style Code";
            gvIT.Columns.Add(style1);
            gvIT.Columns["Style Code"].ReadOnly = true;

            DataGridViewColumn style2 = new DataGridViewTextBoxColumn();
            style2.Name = "Style";
            gvIT.Columns.Add(style2);
            gvIT.Columns["Style"].ReadOnly = true;

            DataGridViewColumn color1 = new DataGridViewTextBoxColumn();
            color1.Name = "Color Code";
            gvIT.Columns.Add(color1);
            gvIT.Columns["Color Code"].ReadOnly = true;

            DataGridViewColumn color2 = new DataGridViewTextBoxColumn();
            color2.Name = "Color";
            gvIT.Columns.Add(color2);
            gvIT.Columns["Color"].ReadOnly = true;

            DataGridViewColumn section1 = new DataGridViewTextBoxColumn();
            section1.Name = "Section";
            gvIT.Columns.Add(section1);
            gvIT.Columns["Section"].ReadOnly = true;

            DataGridViewColumn size1 = new DataGridViewTextBoxColumn();
            size1.Name = "Size";
            gvIT.Columns.Add(size1);
            gvIT.Columns["Size"].ReadOnly = true;

            DataGridViewColumn col13 = new DataGridViewTextBoxColumn();
            col13.Name = "Quantity";
            gvIT.Columns.Add(col13);
            gvIT.Columns["Quantity"].ReadOnly = false;

            DataGridViewColumn brand1 = new DataGridViewTextBoxColumn();
            //On Comment for Carton Qty 091719
            //brand1.Name = "Brand Code";
            brand1.Name = "Ordered Qty";
            gvIT.Columns.Add(brand1);
            gvIT.Columns["Ordered Qty"].ReadOnly = true;

            DataGridViewColumn col14 = new DataGridViewTextBoxColumn();
            col14.Name = "From Warehouse";
            gvIT.Columns.Add(col14);
            gvIT.Columns["From Warehouse"].ReadOnly = true;

            DataGridViewButtonColumn col15 = new DataGridViewButtonColumn();
            col15.Text = "...";
            col15.UseColumnTextForButtonValue = true;
            gvIT.Columns.Add(col15);

            DataGridViewColumn col16 = new DataGridViewTextBoxColumn();
            col16.Name = "To Warehouse";
            gvIT.Columns.Add(col16);
            gvIT.Columns["To Warehouse"].ReadOnly = true;

            DataGridViewButtonColumn col17 = new DataGridViewButtonColumn();
            col17.Text = "...";
            col17.UseColumnTextForButtonValue = true;
            gvIT.Columns.Add(col17);

            DataGridViewColumn col18 = new DataGridViewTextBoxColumn();
            col18.Name = "Company";
            gvIT.Columns.Add(col18);

            DataGridViewButtonColumn col19 = new DataGridViewButtonColumn();
            col19.Text = "...";
            col19.UseColumnTextForButtonValue = true;
            gvIT.Columns.Add(col19);

            DataGridViewColumn col20 = new DataGridViewTextBoxColumn();
            col20.Name = "SKU";
            gvIT.Columns.Add(col20);

            DataGridViewColumn col21 = new DataGridViewTextBoxColumn();
            col21.Name = "SortCode";
            gvIT.Columns.Add(col21);

            DataGridViewColumn col22 = new DataGridViewTextBoxColumn();
            col22.Name = "UOM";
            col22.DisplayIndex = 13;
            gvIT.Columns.Add(col22);

            DataGridViewColumn col23 = new DataGridViewTextBoxColumn();
            col23.Name = "BaseEntry";
            gvIT.Columns.Add(col23);
            gvIT.Columns["BaseEntry"].Visible = false;

            DataGridViewColumn col24 = new DataGridViewTextBoxColumn();
            col24.Name = "BaseLine";
            gvIT.Columns.Add(col24);
            gvIT.Columns["BaseLine"].Visible = false;

            //Additional col 25 and 26 for Chain
            DataGridViewColumn col25 = new DataGridViewTextBoxColumn();
            col25.Name = "Chain";
            gvIT.Columns.Add(col25);
            //col25.DisplayIndex = 14;
            //gvIT.Columns["Chain"].Visible = false;

            DataGridViewColumn col26 = new DataGridViewTextBoxColumn();
            col26.Name = "Chain Description";
            gvIT.Columns.Add(col26);
            //col26.DisplayIndex = 15;

            DataGridViewButtonColumn col27 = new DataGridViewButtonColumn();
            col27.Text = "...";
            col27.UseColumnTextForButtonValue = true;
            //col26.DisplayIndex = 16;
            gvIT.Columns.Add(col27);

            DataGridViewButtonColumn col28 = new DataGridViewButtonColumn();
           
            col28.Name = "LineNum";
            //col26.DisplayIndex = 16;
            gvIT.Columns.Add(col28);
            gvIT.Columns["LineNum"].ReadOnly = true;
        }
    }
}

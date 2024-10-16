using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using PresenterLayer.Services;
using DirecLayer;

namespace PresenterLayer.Helper
{


    public class SalesOrderItemsController
    {
        public static string oRAS = "";
        public SalesOrderService SOS { get; set; } 


        public void GetSelectedItems(DataGridView dgvget, DataGridView dgvpost)
        {
            int i = dgvpost.Rows.Count;

            if (dgvget.SelectedRows.Count > 0)
            {
                IEnumerable<DataGridViewRow> dgvSortInsert = dgvget.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(x => x.Index);
                foreach (DataGridViewRow dr in dgvSortInsert.ToList())
                {
                    dgvpost.Rows.Add();
                    DataGridViewRow row = dgvpost.Rows[i];

                    row.Cells[0].Value = dr.Cells[1].Value;//Style
                    row.Cells[1].Value = dr.Cells[2].Value;//Color
                    row.Cells[2].Value = dr.Cells[3].Value;//Section
                    row.Cells[3].Value = dr.Cells[4].Value;//Size
                    row.Cells[4].Value = dr.Cells[5].Value;//Item No.
                    row.Cells[5].Value = dr.Cells[6].Value;//BarCode
                    row.Cells[6].Value = dr.Cells[7].Value;//Item Description
                    row.Cells[7].Value = "1";//Quantity
                    row.Cells[8].Value = dr.Cells["Warehouse"].Value;//Warehouse
                    row.Cells[9].Value = dr.Cells["EffectivePrice"].Value;//Effective Price
                    row.Cells[10].Value = dr.Cells["GrossPrice"].Value;//Gross Price ()
                    row.Cells[11].Value = dr.Cells["UnitPrice"].Value;//Unit Price
                    row.Cells[12].Value = dr.Cells["Tax Code"].Value;//Tax Code
                    row.Cells[13].Value = dr.Cells["TaxRate"].Value;//Tax Rate
                    row.Cells[14].Value = dr.Cells["Tax Amount"].Value;//Tax Amount
                    row.Cells[15].Value = dr.Cells[16].Value;//Line Total
                    row.Cells[16].Value = dr.Cells[18].Value;//Employee Discount
                    row.Cells[17].Value = dr.Cells["GrossPrice"].Value;//PriceAfterDisc
                    row.Cells[18].Value = dr.Cells[0].Value;//SortCode
                    row.Cells[19].Value = dr.Cells[8].Value;//SKU
                    row.Cells[20].Value = "1"; //Requested to remove logic 07212021 //GetCartonQty(dr.Cells[5].Value.ToString(), 1);   //Ordered Quantity

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

        }

        public void GetAllSelectedItems(DataGridView dgvget, DataGridView dgvpost)
        {
            int i = dgvpost.Rows.Count;
            foreach (DataGridViewRow dr in dgvget.Rows)
            {
                dgvpost.Rows.Add();
                int index = dr.Index;
                DataGridViewRow row = dgvpost.Rows[i];

                row.Cells[0].Value = dr.Cells[0].Value;
                row.Cells[1].Value = dr.Cells[1].Value;
                row.Cells[2].Value = dr.Cells[2].Value;
                row.Cells[3].Value = dr.Cells[3].Value;
                row.Cells[4].Value = dr.Cells[4].Value;
                row.Cells[5].Value = dr.Cells[5].Value;
                row.Cells[6].Value = dr.Cells[6].Value;
                row.Cells[7].Value = "1";
                row.Cells[8].Value = dr.Cells[8].Value;
                row.Cells[9].Value = dr.Cells[9].Value;
                row.Cells[10].Value = dr.Cells[10].Value;
                row.Cells[11].Value = dr.Cells[11].Value;
                row.Cells[12].Value = dr.Cells[12].Value;
                row.Cells[13].Value = dr.Cells[13].Value;
                row.Cells[14].Value = dr.Cells[14].Value;
                row.Cells[15].Value = dr.Cells[15].Value;
                row.Cells[16].Value = dr.Cells[16].Value;
                i++;
            }
            dgvget.Rows.Clear();
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

            if (dgv.Name == "gvSelectedItem")
            {
                dgv.ColumnCount = 21;

                dgv.Columns[0].Name = "Style";
                dgv.Columns[1].Name = "Color";
                dgv.Columns[2].Name = "Section";
                dgv.Columns[3].Name = "Size";
                dgv.Columns[4].Name = "Item No.";
                dgv.Columns[5].Name = "BarCode";
                dgv.Columns[6].Name = "Item Description";
                dgv.Columns[7].Name = "Quantity";
                dgv.Columns[8].Name = "Warehouse";
                dgv.Columns[9].Name = "Effective Price";
                dgv.Columns[10].Name = "Gross Price";
                dgv.Columns[11].Name = "Unit Price";
                dgv.Columns[12].Name = "Tax Code";
                dgv.Columns[13].Name = "Tax Rate";
                dgv.Columns[14].Name = "Tax Amount";
                dgv.Columns[15].Name = "Line Total";
                dgv.Columns[16].Name = "Employee Discount";
                dgv.Columns[17].Name = "PriceAfterDisc";
                dgv.Columns[18].Name = "SortCode";
                dgv.Columns[19].Name = "SKU";
                dgv.Columns[20].Name = "Ordered Quantity";
            }

            foreach (DataGridViewColumn dc in dgv.Columns)
            {
                string ColumnName = dc.Name;

                if (ColumnName == "Quantity" && dgv.Name == "gvSelectedItem")
                {
                    dc.ReadOnly = false;
                }
                else
                {
                    dc.ReadOnly = true;
                }

                if (ColumnName == "Available" && dgv.Name == "gvSO")
                {
                    dgv.Columns["Available"].DefaultCellStyle.Format = "#,##0.##";
                }

                if (ColumnName == "UnitPrice")
                {
                    dgv.Columns["UnitPrice"].DefaultCellStyle.Format = "#,##0.00####";
                }

                if (ColumnName == "Ordered Quantity" && dgv.Name == "gvSelectedItem")
                {
                    dc.ReadOnly = true;
                    dc.DisplayIndex = 8;
                }

                if (dgv.Name == "gvSO")
                {
                    switch (ColumnName)
                    {
                        case "U_ID023":
                            dc.Visible = false;
                            break;
                        case "ItemCode":
                            dc.HeaderText = "Item No.";
                            break;
                        case "ItemName":
                            dc.HeaderText = "Item Description";
                            break;
                        case "Discount":
                            dc.DisplayIndex = 16;
                            break;
                        case "GrossPriceBase":
                            dc.DisplayIndex = 17;
                            dc.Visible = false;
                            break;
                        case "EffectivePrice":
                            dc.DisplayIndex = 18;
                            dc.Visible = false;
                            break;
                        case "UnitPrice":
                            dc.DisplayIndex = 11;
                            break;
                        case "GrossPrice":
                            dc.DisplayIndex = 10;
                            break;
                        case "Tax Code":
                            dc.DisplayIndex = 12;
                            break;
                        case "Tax Rate":
                            dc.DisplayIndex = 13;
                            break;
                        case "Tax Amount":
                            dc.DisplayIndex = 14;
                            break;
                        case "Line Total":
                            dc.DisplayIndex = 15;
                            break;
                    }
                }
            }

        }


        public static void dataGridLayout(DataGridView dgv)
        {
            try
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

                //foreach (DataGridViewRow row in dgv.Rows)
                //{
                //    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                //}
                //dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;

                if (dgv.Name == "dgvItems" || dgv.Name == "DgvPreviewItem")
                {
                    dgv.Columns[9].DefaultCellStyle.ForeColor = Color.White;
                    dgv.Columns[9].DefaultCellStyle.BackColor = Color.RoyalBlue;
                    dgv.Columns[10].DefaultCellStyle.ForeColor = Color.White;
                    dgv.Columns[10].DefaultCellStyle.BackColor = Color.RoyalBlue;
                    dgv.Columns[11].DefaultCellStyle.ForeColor = Color.White;
                    dgv.Columns[11].DefaultCellStyle.BackColor = Color.RoyalBlue;
                    dgv.Columns[12].DefaultCellStyle.ForeColor = Color.White;
                    dgv.Columns[12].DefaultCellStyle.BackColor = Color.RoyalBlue;
                    dgv.Columns[13].DefaultCellStyle.ForeColor = Color.White;
                    dgv.Columns[13].DefaultCellStyle.BackColor = Color.RoyalBlue;

                    dgv.Columns[0].ReadOnly = true;     //Item No.
                    dgv.Columns[1].ReadOnly = true;     //Item Description
                    dgv.Columns[2].ReadOnly = true;     //Brand
                    dgv.Columns[3].ReadOnly = true;     //Style
                    dgv.Columns[4].ReadOnly = true;     //Color
                    dgv.Columns[5].ReadOnly = true;     //Size
                    dgv.Columns[6].ReadOnly = true;     //Section
                    dgv.Columns[7].ReadOnly = true;     //Barcode
                    dgv.Columns[8].ReadOnly = true;     //Effective Price
                    dgv.Columns[9].ReadOnly = false;    //Gross Price = 112 | Unit Price * (1 + Tax Rate)
                    dgv.Columns[10].ReadOnly = false;   //Unit Price = Gross Price / (1 + Tax Rate) | 100
                    dgv.Columns[11].ReadOnly = false;   //Quantity = 1 | 1
                    dgv.Columns[12].ReadOnly = false;   //Discount % = 0 | 
                    dgv.Columns[13].ReadOnly = false;   //Discount = 0 |
                    dgv.Columns[14].ReadOnly = true;    //Employee Discount
                    dgv.Columns[15].ReadOnly = true;    //Warehouse
                    dgv.Columns[16].ReadOnly = false;    //Warehouse Button
                    dgv.Columns[17].ReadOnly = true;    //Tax
                    dgv.Columns[18].ReadOnly = false;    //Tax Button
                    dgv.Columns[19].ReadOnly = true;    //Tax Rate = 0.12
                    dgv.Columns[20].ReadOnly = true;    //Line Total = Unit Price * QTY
                    dgv.Columns[21].ReadOnly = true;    //Gross Total = Gross Price * QTY
                    dgv.Columns[22].ReadOnly = true;    //Price After Discount = ( Unit Price * QTY ) - Discount
                    dgv.Columns[23].ReadOnly = true;    //LineNum
                    dgv.Columns[24].ReadOnly = true;    //BaseEntry
                    dgv.Columns[25].ReadOnly = true;    //SKU

                }

                foreach (DataGridViewColumn dc in dgv.Columns)
                {
                    string ColumnName = dc.Name;

                    if (dgv.Name == "dgvItems" || dgv.Name == "DgvPreviewItem")
                    {
                        switch (ColumnName)
                        {
                            case "SKU":
                                dc.DisplayIndex = 8;
                                break;

                            case "Ordered Qty":
                                dc.DisplayIndex = 16;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, false);
            }

        }

        public static void dgvItemsLayout(DataGridView dgvItems)
        {
            if (dgvItems.Columns.Count > 0)
            {
                dgvItems.Columns.Clear();
                dgvItems.Rows.Clear();
            }

            var col1 = new DataGridViewTextBoxColumn();
            col1.Name = "Item No.";
            dgvItems.Columns.Add(col1);

            var col2 = new DataGridViewTextBoxColumn();
            col2.Name = "Item Description";
            dgvItems.Columns.Add(col2);

            var colBrand = new DataGridViewTextBoxColumn();
            colBrand.Name = "Brand";
            dgvItems.Columns.Add(colBrand);

            var col3 = new DataGridViewTextBoxColumn();
            col3.Name = "Style";
            dgvItems.Columns.Add(col3);

            var col4 = new DataGridViewTextBoxColumn();
            col4.Name = "Color";
            dgvItems.Columns.Add(col4);

            var col5 = new DataGridViewTextBoxColumn();
            col5.Name = "Size";
            dgvItems.Columns.Add(col5);

            var col6 = new DataGridViewTextBoxColumn();
            col6.Name = "Section";
            dgvItems.Columns.Add(col6);

            var col7 = new DataGridViewTextBoxColumn();
            col7.Name = "Barcode";
            dgvItems.Columns.Add(col7);

            var col8 = new DataGridViewTextBoxColumn();
            col8.Name = "Effective Price";
            dgvItems.Columns.Add(col8);

            var col9 = new DataGridViewTextBoxColumn();
            col9.Name = "Gross Price";
            dgvItems.Columns.Add(col9);

            var col10 = new DataGridViewTextBoxColumn();
            col10.Name = "Unit Price";
            dgvItems.Columns.Add(col10);

            var col11 = new DataGridViewTextBoxColumn();
            col11.Name = "Quantity";
            dgvItems.Columns.Add(col11);

            var col13 = new DataGridViewTextBoxColumn();
            col13.Name = "Discount %";
            dgvItems.Columns.Add(col13);

            var col14 = new DataGridViewTextBoxColumn();
            col14.Name = "Discount";
            dgvItems.Columns.Add(col14);

            var col12 = new DataGridViewTextBoxColumn();
            col12.Name = "Employee Discount";
            dgvItems.Columns.Add(col12);

            var col15 = new DataGridViewTextBoxColumn();
            col15.Name = "Warehouse";
            dgvItems.Columns.Add(col15);

            var col16 = new DataGridViewButtonColumn();
            col16.Text = "...";
            col16.UseColumnTextForButtonValue = true;
            dgvItems.Columns.Add(col16);

            var col17 = new DataGridViewTextBoxColumn();
            col17.Name = "Tax";
            dgvItems.Columns.Add(col17);

            var col18 = new DataGridViewButtonColumn();
            col18.Text = "...";
            col18.UseColumnTextForButtonValue = true;
            dgvItems.Columns.Add(col18);

            var col19 = new DataGridViewTextBoxColumn();
            col19.Name = "Tax Rate";
            dgvItems.Columns.Add(col19);

            var col20 = new DataGridViewTextBoxColumn();
            col20.Name = "Line Total";
            dgvItems.Columns.Add(col20);
            //dgvItems.Columns["Line Total"].Visible = true;

            var col21 = new DataGridViewTextBoxColumn();
            col21.Name = "Gross Total";
            dgvItems.Columns.Add(col21);
            //dgvItems.Columns["Gross Total"].Visible = true;

            var col22 = new DataGridViewTextBoxColumn();
            col22.Name = "PriceAfterDisc";
            dgvItems.Columns.Add(col22);
            dgvItems.Columns["PriceAfterDisc"].Visible = false;

            var col23 = new DataGridViewTextBoxColumn();
            col23.Name = "LineNum";
            dgvItems.Columns.Add(col23);
            dgvItems.Columns["LineNum"].Visible = false;

            var col24 = new DataGridViewTextBoxColumn();
            col24.Name = "BaseEntry";
            dgvItems.Columns.Add(col24);
            dgvItems.Columns["BaseEntry"].Visible = false;

            var col25 = new DataGridViewTextBoxColumn();
            col25.Name = "SKU";
            dgvItems.Columns.Add(col25);
            dgvItems.Columns["SKU"].Visible = true;

            //Added new column for Carton Qty 091819
            var col26 = new DataGridViewTextBoxColumn();
            col26.Name = "Ordered Qty";
            dgvItems.Columns.Add(col26);
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

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DirecLayer;

namespace EasySAP
{
    class SalesInvoiceItemsController
    {
        private SAPHanaAccess sapHana { get; set; }
        public Int32 GetSelectedItems(DataGridView dgvget, DataGridView dgvpost)
        {
            int negaQty = 0;
            int i = dgvpost.Rows.Count;
            if (dgvget.SelectedRows.Count > 0)
            {
                IEnumerable<DataGridViewRow> dgvSortInsert = dgvget.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(x => x.Index);
                foreach (DataGridViewRow dr in dgvSortInsert.ToList())
                {
                    //if (Convert.ToDecimal(dr.Cells[8].Value) > 0)
                    //{
                        dgvpost.Rows.Add();
                        DataGridViewRow row = dgvpost.Rows[i];

                        row.Cells[0].Value = dr.Cells[1].Value;
                        row.Cells[1].Value = dr.Cells[2].Value;
                        row.Cells[2].Value = dr.Cells[3].Value;
                        row.Cells[3].Value = dr.Cells[4].Value;
                        row.Cells[4].Value = dr.Cells[5].Value;
                        row.Cells[5].Value = dr.Cells[6].Value;
                        row.Cells[6].Value = dr.Cells[7].Value;
                        row.Cells[7].Value = "1";
                        row.Cells[8].Value = dr.Cells[9].Value;
                        row.Cells[9].Value = dr.Cells[10].Value;
                        row.Cells[10].Value = dr.Cells[11].Value;
                        row.Cells[11].Value = dr.Cells[12].Value;
                        row.Cells[12].Value = dr.Cells[13].Value;
                        row.Cells[13].Value = dr.Cells[14].Value;
                        row.Cells[14].Value = dr.Cells[15].Value;
                        row.Cells[15].Value = dr.Cells[16].Value;
                        row.Cells[16].Value = dr.Cells[17].Value;
                        row.Cells[17].Value = dr.Cells[0].Value;

                        dgvget.Rows.RemoveAt(dr.Index);
                        i++;
                    //}
                    //else
                    //{
                    //    negaQty += 1;
                    //}

                }
                //dgvpost.FirstDisplayedScrollingRowIndex = dgvpost.RowCount - 1;

                foreach (DataGridViewRow row1 in dgvpost.Rows)
                {
                    row1.HeaderCell.Value = String.Format("{0}", row1.Index + 1);
                }
                dgvpost.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            }
            return negaQty;
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

            dgv.RowHeadersVisible = true;
            dgv.MultiSelect = true;
            dgv.ColumnHeadersVisible = true;

            if (dgv.Name != "gvSIddw")
            {
                if (dgv.Name == "gvSelectedItem")
                {
                    dgv.ColumnCount = 18;

                    dgv.Columns[0].Name = "Style";
                    dgv.Columns[1].Name = "Color";
                    dgv.Columns[2].Name = "Section";
                    dgv.Columns[3].Name = "Size";
                    dgv.Columns[4].Name = "Item No.";
                    dgv.Columns[5].Name = "BarCode";
                    dgv.Columns[6].Name = "Item Description";
                    dgv.Columns[7].Name = "Quantity";
                    dgv.Columns[8].Name = "Warehouse";
                    dgv.Columns[9].Name = "Gross Price";
                    dgv.Columns[10].Name = "Unit Price";
                    dgv.Columns[11].Name = "Tax Code";
                    dgv.Columns[12].Name = "Tax Rate";
                    dgv.Columns[13].Name = "Tax Amount";
                    dgv.Columns[14].Name = "Line Total";
                    dgv.Columns[15].Name = "Discount";
                    dgv.Columns[16].Name = "PriceAfterDisc";
                    dgv.Columns[17].Name = "SortCode";
                }

            }
            else
            {
                dgv.ColumnCount = 10;

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
                dgv.Columns[5].Name = "Price";
                dgv.Columns[5].ReadOnly = true;
                dgv.Columns[6].Name = "Line Total";
                dgv.Columns[6].ReadOnly = true;
                dgv.Columns[7].Name = "Warehouse";
                dgv.Columns[7].ReadOnly = true;
                dgv.Columns[8].Name = "BPCode";
                dgv.Columns[8].ReadOnly = true;
                dgv.Columns[9].Name = "OrderEntry";
                dgv.Columns[9].ReadOnly = true;
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

                if (ColumnName == "Available" && dgv.Name == "gvSI")
                {
                    dgv.Columns["Available"].DefaultCellStyle.Format = "#,##0.##";
                }

                if (ColumnName == "UnitPrice")
                {
                    dgv.Columns["UnitPrice"].DefaultCellStyle.Format = "#,##0.00####";
                }

                if (dgv.Name == "gvSI")
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

            if (dgv.Name == "dgvItems")
            {
                dgv.Columns[0].ReadOnly = true;     //Item No.
                dgv.Columns[1].ReadOnly = true;     //Item Description
                dgv.Columns[2].ReadOnly = true;     //Style
                dgv.Columns[3].ReadOnly = true;     //Color
                dgv.Columns[4].ReadOnly = true;     //Size
                dgv.Columns[5].ReadOnly = true;     //Section
                dgv.Columns[6].ReadOnly = true;     //Barcode
                dgv.Columns[7].ReadOnly = false;    //Gross Price
                dgv.Columns[8].ReadOnly = false;    //Unit Price
                dgv.Columns[9].ReadOnly = false;    //Quantity
                dgv.Columns[10].ReadOnly = false;   //Discount %
                dgv.Columns[11].ReadOnly = false;   //Discount
                dgv.Columns[12].ReadOnly = true;    //Warehouse
                dgv.Columns[13].ReadOnly = true;    //WHS Button
                dgv.Columns[14].ReadOnly = false;   //Tax
                dgv.Columns[15].ReadOnly = true;    //Tax Button
                dgv.Columns[16].ReadOnly = true;    //Tax Rate
                dgv.Columns[17].ReadOnly = true;    //Line Total
                dgv.Columns[18].ReadOnly = true;    //Gross Total
                dgv.Columns[19].ReadOnly = true;    //Price After Discount
                dgv.Columns[20].ReadOnly = true;    //BaseLine
                dgv.Columns[21].ReadOnly = true;    //BaseEntry
            }
        }

        public string SelValue(string SOqrycode, string strDocType, [Optional] string strBPCode, [Optional] string strAddress, [Optional] string strTxtbox)
        {
            string strSelectedQry = "";
            string strSelValue = "";

            string strAddr = string.IsNullOrEmpty(strAddress) ? "" : strAddress;
            string strBPC = string.IsNullOrEmpty(strBPCode) ? "" : strBPCode;

            if (SOqrycode == "series1" && strDocType != "")
            {
                strSelectedQry = "SELECT " +
                                " (Select SeriesName from NNM1 where SeriesName = T1.U_Series and ObjectCode = '13' ) [Value] " +
                                " FROM [@DOC_TYPE] T1 " +
                                $" WHERE T1.Name = '{strDocType}' ";
            }
            else if (SOqrycode == "toWhs1" && strDocType != "")
            {
                strSelectedQry = "SELECT DISTINCT " +
                                " CASE a.U_WhsSource  " +
                                " WHEN 'WHS'  " +
                                " THEN a.U_WhsCode " +
                                " WHEN 'CRD' " +
                                " THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' ";

                if (strAddr != "")
                {
                    strSelectedQry += $" and x.Street = '{ strAddr }' ";
                }

                strSelectedQry += $" and x.CardCode = '{ strBPC }') " +
                " END [Value] " +
                " FROM [@DOC_TYPE] a  " +
                " LEFT JOIN OLCT b on a.U_WhsSource = b.Location " +
                " LEFT JOIN OWHS c on((b.Code <> c.Location) " +
                " or (b.Code = c.Location) )  " +
                " and a.U_WhsSource = 'WHS-LOC' " +
                $" WHERE a.Name = '{ strDocType }' Order by Value ";
            }
            else if (SOqrycode == "toWhs2" && strDocType != "" && strTxtbox != "")
            {
                strSelectedQry = "SELECT Value FROM ( " +
                                "SELECT DISTINCT " +
                                " CASE a.U_WhsSource  " +
                                " WHEN 'WHS'  " +
                                " THEN a.U_WhsCode " +
                                " WHEN 'CRD' " +
                                $" THEN (Select max(x.U_Whs) from CRD1 x where x.AdresType = 'S' ";

                if (strAddr != "")
                {
                    strSelectedQry += $" and x.Street = '{ strAddr }' ";
                }

                strSelectedQry += $" and x.CardCode = '{ strBPC }') " +
                " END [Value] " +
                " FROM [@DOC_TYPE] a  " +
                " LEFT JOIN OLCT b on a.U_WhsSource = b.Location " +
                " LEFT JOIN OWHS c on((b.Code <> c.Location) " +
                " or (b.Code = c.Location) )  " +
                " and a.U_WhsSource = 'WHS-LOC' " +
                $" WHERE a.Name = '{ strDocType }' ) MT1 where Value = '{strTxtbox}' Order by Value ";
            }

            if (strSelectedQry != "" && sapHana.Get(strSelectedQry).Rows.Count > 0)
            {
                strSelValue = sapHana.Get(strSelectedQry).Rows[0]["Value"].ToString();
            }

            return strSelValue;
        }

        public static void dgvItemsLayout(DataGridView dgvItems)
        {
            if (dgvItems.Columns.Count > 0)
            {
                dgvItems.Columns.Clear();
                dgvItems.Rows.Clear();
            }

            dgvItems.Columns.Clear();

            DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
            col1.Name = "Item No.";
            dgvItems.Columns.Add(col1);

            DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
            col2.Name = "Item Description";
            dgvItems.Columns.Add(col2);

            DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
            col3.Name = "Style";
            dgvItems.Columns.Add(col3);

            DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
            col4.Name = "Color";
            dgvItems.Columns.Add(col4);

            DataGridViewColumn col5 = new DataGridViewTextBoxColumn();
            col5.Name = "Size";
            dgvItems.Columns.Add(col5);

            DataGridViewColumn col6 = new DataGridViewTextBoxColumn();
            col6.Name = "Section";
            dgvItems.Columns.Add(col6);

            DataGridViewColumn col7 = new DataGridViewTextBoxColumn();
            col7.Name = "Barcode";
            dgvItems.Columns.Add(col7);

            DataGridViewColumn col8 = new DataGridViewTextBoxColumn();
            col8.Name = "Gross Price";
            dgvItems.Columns.Add(col8);

            DataGridViewColumn col9 = new DataGridViewTextBoxColumn();
            col9.Name = "Unit Price";
            dgvItems.Columns.Add(col9);

            DataGridViewColumn col10 = new DataGridViewTextBoxColumn();
            col10.Name = "Quantity";
            dgvItems.Columns.Add(col10);

            DataGridViewColumn col11 = new DataGridViewTextBoxColumn();
            col11.Name = "Discount %";
            dgvItems.Columns.Add(col11);

            DataGridViewColumn col12 = new DataGridViewTextBoxColumn();
            col12.Name = "Discount";
            dgvItems.Columns.Add(col12);

            DataGridViewColumn col13 = new DataGridViewTextBoxColumn();
            col13.Name = "Warehouse";
            dgvItems.Columns.Add(col13);

            DataGridViewButtonColumn col14 = new DataGridViewButtonColumn();
            col14.Text = "...";
            col14.UseColumnTextForButtonValue = true;
            dgvItems.Columns.Add(col14);

            DataGridViewColumn col15 = new DataGridViewTextBoxColumn();
            col15.Name = "Tax";
            dgvItems.Columns.Add(col15);

            DataGridViewButtonColumn col16 = new DataGridViewButtonColumn();
            col16.Text = "...";
            col16.UseColumnTextForButtonValue = true;
            dgvItems.Columns.Add(col16);

            DataGridViewColumn col17 = new DataGridViewTextBoxColumn();
            col17.Name = "Tax Rate";
            dgvItems.Columns.Add(col17);

            DataGridViewColumn col18 = new DataGridViewTextBoxColumn();
            col18.Name = "Line Total";
            dgvItems.Columns.Add(col18);

            DataGridViewColumn col19 = new DataGridViewTextBoxColumn();
            col19.Name = "Gross Total";
            dgvItems.Columns.Add(col19);
            dgvItems.Columns["Gross Total"].Visible = false;

            DataGridViewColumn col20 = new DataGridViewTextBoxColumn();
            col20.Name = "PriceAfterDisc";
            dgvItems.Columns.Add(col20);
            dgvItems.Columns["PriceAfterDisc"].Visible = true;

            DataGridViewColumn col21 = new DataGridViewTextBoxColumn();
            col21.Name = "LineNum";
            dgvItems.Columns.Add(col21);
            dgvItems.Columns["LineNum"].Visible = false;

            DataGridViewColumn col22 = new DataGridViewTextBoxColumn();
            col22.Name = "BaseEntry";
            dgvItems.Columns.Add(col22);
            dgvItems.Columns["BaseEntry"].Visible = true;
        }
    }
}

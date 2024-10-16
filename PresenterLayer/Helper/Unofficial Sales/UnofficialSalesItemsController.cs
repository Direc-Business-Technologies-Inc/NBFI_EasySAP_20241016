using DirecLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresenterLayer.Helper.Unofficial_Sales
{
    public class UnofficialSalesItemsController
    {
        public static string oRAS = "";
        public static double oTotalDiff = 0;
        public static double oTotalPrice = 0;

        public static double oQtyDiff = 0;
        public static double oTotalQty = 0;
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

                    //if (Convert.ToDecimal(dr.Cells[8].Value) > 0)
                    //{

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

            if (dgv.Name != "gvUOSddw")
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
                    dgv.Columns[16].Name = "PriceAfterDisc";        //GrossPriceBase
                    dgv.Columns[16].Visible = false;
                    dgv.Columns[17].Name = "SortCode";
                    dgv.Columns[17].Visible = false;
                }
            }
            else
            {
                dgv.ColumnCount = 8;

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

                if (ColumnName == "Available" && dgv.Name == "gvUOS")
                {
                    dgv.Columns["Available"].DefaultCellStyle.Format = "#,##0.##";
                }

                if (ColumnName == "UnitPrice")
                {
                    dgv.Columns["UnitPrice"].DefaultCellStyle.Format = "#,##0.00####";
                }

                if (dgv.Name == "gvUOS")
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

            foreach (DataGridViewRow row2 in dgv.Rows)
            {
                row2.HeaderCell.Value = String.Format("{0}", row2.Index + 1);
            }
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;

            if (dgv.Name == "dgvBarcodeItems")
            {
                dgv.Columns[2].DefaultCellStyle.ForeColor = Color.White;
                dgv.Columns[2].DefaultCellStyle.BackColor = Color.RoyalBlue;
                dgv.Columns[3].DefaultCellStyle.ForeColor = Color.White;
                dgv.Columns[3].DefaultCellStyle.BackColor = Color.RoyalBlue;
                dgv.Columns[4].DefaultCellStyle.ForeColor = Color.White;
                dgv.Columns[4].DefaultCellStyle.BackColor = Color.RoyalBlue;
                dgv.Columns[5].DefaultCellStyle.ForeColor = Color.White;
                dgv.Columns[5].DefaultCellStyle.BackColor = Color.RoyalBlue;
                dgv.Columns[6].DefaultCellStyle.ForeColor = Color.White;
                dgv.Columns[6].DefaultCellStyle.BackColor = Color.RoyalBlue;

                foreach (DataGridViewColumn dc in dgv.Columns)
                {
                    string ColumnName = dc.Name;

                    if (ColumnName == "Gross Price" || ColumnName == "Unit Price" || ColumnName == "Discount %" || ColumnName == "Discount Amount" || ColumnName == "Quantity" || ColumnName == "DeliveryDate")
                    {
                        dc.ReadOnly = false;
                    }
                    else
                    {
                        if (ColumnName == "Item Description")
                        {
                            foreach (DataGridViewRow dr in dgv.Rows)
                            {
                                if (dr.Cells[18].Value.ToString() == "Y")
                                {
                                    dr.Cells[1].ReadOnly = false;
                                }
                                else
                                {
                                    dr.Cells[1].ReadOnly = true;
                                }
                            }
                        }
                        else
                        {
                            dc.ReadOnly = true;
                        }
                    }

                    if (ColumnName == "In Stock" || ColumnName == "index" || ColumnName == "disc" || ColumnName == "positive" || ColumnName == "PriceBeforeDiscount" || ColumnName == "SortCode" || ColumnName == "ItemProperty")
                    //if (ColumnName == "In Stock" ||  ColumnName == "index" || ColumnName == "disc" || ColumnName == "positive" || ColumnName == "SortCode" || ColumnName == "ItemProperty")
                    {
                        dc.Visible = false;
                    }
                }

            }
            dgv.FirstDisplayedScrollingRowIndex = dgv.RowCount - 1;
        }

        public string SelValue(string SOqrycode, string strDocType, [Optional] string strBPCode, [Optional] string strAddress, [Optional] string strTxtbox)
        {
            string strSelectedQry = "";
            string strSelValue = "";

            string strAddr = string.IsNullOrEmpty(strAddress) ? "" : strAddress;
            string strBPC = string.IsNullOrEmpty(strBPCode) ? "" : strBPCode;
            var sapHana = new SAPHanaAccess();

            if (SOqrycode == "series1" && strDocType != "")
            {
                strSelectedQry = "SELECT " +
                                " (Select SeriesName from NNM1 where SeriesName = T1.U_Series and ObjectCode = '15') [Value] " +
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

        public void CreateLogs(string strBPCode, string strSellingDate, string strBarCode, string strStatus)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime dtSalesDate = DateTime.ParseExact(strSellingDate, "M/d/yyyy", provider);
            string strSalesDate = dtSalesDate.ToString("MM-dd-yyyy");
            string strDocDateMY = DateTime.Today.ToString("MMMyyyy");
            string strDocDate = DateTime.Today.ToString("MM-dd-yyyy");

            strSalesDate = strSalesDate.Replace("-", "");
            strDocDate = strDocDate.Replace("-", "");

            string filename = strBPCode.Replace("-", "") + "_" + strSalesDate + "_ItemLogs" + ".txt";

            string path = $@"D:\EasySAP\ItemLogs\{strDocDateMY}\{strDocDate}\{strBPCode}\";
            string filepath = path + filename;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine("TIME | BARCODE | NOT IN SKU MAINTENANCE");
                    sw.WriteLine(DateTime.Now.ToShortTimeString() + "|" + strBarCode + "|" + strStatus);
                }
            }
            else
            {
                File.AppendAllText(filepath, DateTime.Now.ToShortTimeString() + "|" + strBarCode + "|" + strStatus + Environment.NewLine);
            }
        }

        public static void dgvBarcodeItemsLayout(DataGridView gvUOS)
        {
            gvUOS.Columns.Clear();

            DataGridViewTextBoxColumn col0 = new DataGridViewTextBoxColumn();
            col0.Name = "Item No.";
            gvUOS.Columns.Add(col0);

            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.Name = "Item Description";
            gvUOS.Columns.Add(col1);

            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            col2.Name = "Gross Price";
            col2.Visible = true;
            gvUOS.Columns.Add(col2);

            DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
            col3.Name = "Unit Price";
            gvUOS.Columns.Add(col3);

            DataGridViewTextBoxColumn col4 = new DataGridViewTextBoxColumn();
            col4.Name = "Quantity";
            gvUOS.Columns.Add(col4);

            DataGridViewTextBoxColumn col5 = new DataGridViewTextBoxColumn();
            col5.Name = "Discount Amount";
            gvUOS.Columns.Add(col5);

            DataGridViewTextBoxColumn col6 = new DataGridViewTextBoxColumn();
            col6.Name = "Discount %";
            gvUOS.Columns.Add(col6);

            DataGridViewTextBoxColumn col7 = new DataGridViewTextBoxColumn();
            col7.Name = "In Stock";
            gvUOS.Columns.Add(col7);

            DataGridViewTextBoxColumn col8 = new DataGridViewTextBoxColumn();
            col8.Name = "Total";
            gvUOS.Columns.Add(col8);

            DataGridViewTextBoxColumn col9 = new DataGridViewTextBoxColumn();
            col9.Name = "index";
            gvUOS.Columns.Add(col9);

            DataGridViewTextBoxColumn col10 = new DataGridViewTextBoxColumn();
            col10.Name = "disc";
            gvUOS.Columns.Add(col10);

            DataGridViewTextBoxColumn col11 = new DataGridViewTextBoxColumn();
            col11.Name = "positive";
            gvUOS.Columns.Add(col11);

            DataGridViewTextBoxColumn col12 = new DataGridViewTextBoxColumn();
            col12.Name = "PriceBeforeDiscount";
            gvUOS.Columns.Add(col12);

            DataGridViewTextBoxColumn col13 = new DataGridViewTextBoxColumn();
            col13.Name = "Style";
            gvUOS.Columns.Add(col13);

            DataGridViewTextBoxColumn col14 = new DataGridViewTextBoxColumn();
            col14.Name = "Color";
            gvUOS.Columns.Add(col14);

            DataGridViewTextBoxColumn col15 = new DataGridViewTextBoxColumn();
            col15.Name = "Size";
            gvUOS.Columns.Add(col15);

            DataGridViewTextBoxColumn col16 = new DataGridViewTextBoxColumn();
            col16.Name = "Section";
            gvUOS.Columns.Add(col16);

            DataGridViewTextBoxColumn col17 = new DataGridViewTextBoxColumn();
            col17.Name = "SortCode";
            gvUOS.Columns.Add(col17);

            DataGridViewTextBoxColumn col18 = new DataGridViewTextBoxColumn();
            col18.Name = "ItemProperty";
            gvUOS.Columns.Add(col18);

            DataGridViewTextBoxColumn col19 = new DataGridViewTextBoxColumn();
            col19.Name = "DeliveryDate";
            col19.DisplayIndex = 0;
            gvUOS.Columns.Add(col19);

            DataGridViewButtonColumn col20 = new DataGridViewButtonColumn();
            col20.Text = "...";
            col20.UseColumnTextForButtonValue = true;
            col20.DisplayIndex = 1;
            gvUOS.Columns.Add(col20);

            DataGridViewTextBoxColumn col21 = new DataGridViewTextBoxColumn();
            col21.Name = "Company";
            gvUOS.Columns.Add(col21);

            DataGridViewButtonColumn col22 = new DataGridViewButtonColumn();
            col22.Text = "...";
            col22.UseColumnTextForButtonValue = true;
            gvUOS.Columns.Add(col22);
        }
    }
}

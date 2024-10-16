﻿using PresenterLayer.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PresenterLayer.Services.Inventory;

namespace PresenterLayer.Views.Inventory
{
    public class Inventory_Style
    {
        public static DataGridView dgv { get; set; }

        public static int MdiSize { get; set; }

        public static string DocumentStatus { get; set; }

        public static List<string> itemSelected { get; set; }

        public void GoodReceiptItem(string type, bool IsAddMode)
        {
            if (dgv.ColumnCount > 0)
            {
                dgv.Columns.Clear();

                if (dgv.RowCount > 0)
                {
                    dgv.Rows.Clear();
                }
            }

            ItemNoColumn();
            OldItemNo();
            ItemDesc();
            Style();
            Brand(IsAddMode);
            Clr();
            Size();
            PoQty();
            TotalItemQtyInCarton();
            ReceivedQty();
            UoM(IsAddMode);
            Warehouse(IsAddMode);
            OpenQty();
            Chain(IsAddMode);
            PricetagCount();
            Project(IsAddMode);
            Remarks();

            TaxCode(IsAddMode);
            DiscountPerc();
            UnitPrice();
            GrossPrice();
            TotalLc();
            GrossTotal();
            DiscAmount();

            DataGridViewColumn Index = new DataGridViewTextBoxColumn();
            AddDataGridViewColumn(Index, "Index", true);
            Index.Visible = false;

            DataGridViewColumn TaxAmount = new DataGridViewTextBoxColumn();
            AddDataGridViewColumn(TaxAmount, "Tax Amount", true);
            TaxAmount.Visible = false;

            dataGridLayout();
        }

        public void PurchaseOrderItem(string type, bool IsAddMode, [Optional] DataGridView _dgv)
        {
            if (_dgv != null)
            {
                dgv = _dgv;
            }

            if (dgv.ColumnCount > 0)
            {
                dgv.Columns.Clear();

                if (dgv.RowCount > 0)
                {
                    dgv.Rows.Clear();
                }
            }

            ItemNoColumn(type);
            OldItemNo(type);
            ItemDesc(type);
            Style(type);
            Brand(IsAddMode);
            Clr(type);
            Size(type);
            Quantity(type);
            UoM(IsAddMode);
            Warehouse(IsAddMode);

            if (type == "Item")
            {
                OpenQty(type);
                Chain(IsAddMode);
                PricetagCount();
                UnitPrice();
                DiscountPerc(type);
                DiscAmount(type);
            }
            else
            {
                GlAccount(type);
                GlAccountName(IsAddMode);
                OpenQty(type);
                Chain(IsAddMode);
                PricetagCount(type);
                UnitPricePerPiece(type);
                UnitPrice();
            }

            TaxCode(IsAddMode);
            GrossPrice();
            TotalLc();
            GrossTotal();
            Project(IsAddMode);
            Department(IsAddMode);
            Remarks();

            DataGridViewColumn Index = new DataGridViewTextBoxColumn();
            AddDataGridViewColumn(Index, "Index", true);
            Index.Visible = true;

            DataGridViewColumn TaxAmount = new DataGridViewTextBoxColumn();
            AddDataGridViewColumn(TaxAmount, "Tax Amount", true);
            TaxAmount.Visible = false;

            dataGridLayout();
        }

        bool isService(string docType)
        {
            return docType == "Service" ? false : true;
        }

        private void ItemNoColumn([Optional] string type)
        {
            DataGridViewColumn ItemNo = new DataGridViewTextBoxColumn(); //  
            AddDataGridViewColumn(ItemNo, "Item No.", isService(type));
        }

        private void OldItemNo([Optional] string type)
        {
            DataGridViewColumn OldItemNo = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(OldItemNo, "Old Item No.", isService(type));
        }

        private void ItemDesc([Optional] string type)
        {
            DataGridViewColumn ItemDesc = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(ItemDesc, "Item Description", isService(type));
        }

        private void Style([Optional] string type)
        {
            DataGridViewColumn Style = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(Style, "Style", isService(type));
        }

        private void Clr([Optional] string type)
        {
            DataGridViewColumn Color = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(Color, "Color", isService(type));
        }

        private void Size([Optional] string type)
        {
            DataGridViewColumn Size = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(Size, "Size", isService(type));
        }

        private void Quantity([Optional] string type)
        {
            DataGridViewColumn Quantity = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(Quantity, "Quantity", isService(type));
        }

        private void UoM(bool IsAddMode)
        {
            DataGridViewColumn UoM = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(UoM, "UoM", true);

            DataGridViewButtonColumn UomBtn = new DataGridViewButtonColumn(); // B U T T O N
            AddDataGridButtonColumn(UomBtn, IsAddMode);
        }

        private void Warehouse(bool IsAddMode)
        {
            DataGridViewColumn Warehouse = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(Warehouse, "Warehouse", true);

            DataGridViewButtonColumn WarehouseBtn = new DataGridViewButtonColumn(); // B U T T O N
            AddDataGridButtonColumn(WarehouseBtn, IsAddMode);
        }

        private void OpenQty([Optional] string type)
        {
            DataGridViewColumn OpenQty = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(OpenQty, "Open Qty", isService(type));
        }

        private void PoQty([Optional] string type)
        {
            DataGridViewColumn PoQty = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(PoQty, "PO Quantity", isService(type));
        }

        private void TotalItemQtyInCarton([Optional] string type)
        {
            DataGridViewColumn totlaItmQtyCtn = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(totlaItmQtyCtn, "Total Item Quantity in Carton/s", isService(type));
        }

        private void ReceivedQty([Optional] string type)
        {
            DataGridViewColumn ReceivedQty = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(ReceivedQty, "Received Quantity", isService(type));
        }

        private void Chain(bool IsAddMode)
        {
            DataGridViewColumn ChainPricetag = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(ChainPricetag, "Chain Pricetag", true);

            DataGridViewColumn ChainDescription = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(ChainDescription, "Chain Description", true);

            DataGridViewButtonColumn ChainBtn = new DataGridViewButtonColumn(); // B U T T O N
            AddDataGridButtonColumn(ChainBtn, IsAddMode);
        }

        private void GlAccount([Optional] string type)
        {
            DataGridViewColumn GlAccount = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(GlAccount, "G/L Account", isService(type));
        }

        private void GlAccountName(bool IsAddMode)
        {
            DataGridViewColumn GlAccountName = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(GlAccountName, "G/L Account Name", true);

            DataGridViewButtonColumn GlAcctBTN = new DataGridViewButtonColumn(); // B U T T O N
            AddDataGridButtonColumn(GlAcctBTN, IsAddMode);
        }

        private void PricetagCount([Optional] string type)
        {
            DataGridViewColumn PricetagCount = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(PricetagCount, "Pricetag Count", isService(type));
        }

        private void UnitPrice()
        {
            DataGridViewColumn UnitPrice = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(UnitPrice, "Unit Price", false);
        }

        private void DiscountPerc([Optional] string type)
        {
            DataGridViewColumn DiscountPerc = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(DiscountPerc, "Discount %", isService(type));
        }

        private void DiscAmount([Optional] string type)
        {
            DataGridViewColumn DiscAmount = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(DiscAmount, "Disc Amount", isService(type));
            dgv.Columns["Disc Amount"].ReadOnly = true;
        }

        private void UnitPricePerPiece([Optional] string type)
        {
            DataGridViewColumn UnitPricePerPiece = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(UnitPricePerPiece, "Unit Price per piece", isService(type));

            DataGridViewColumn GrossPricePerPiece = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(GrossPricePerPiece, "Gross Price per piece", isService(type));
        }

        private void TaxCode(bool IsAddMode)
        {
            DataGridViewColumn TaxCode = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(TaxCode, "Tax Code", true);

            DataGridViewButtonColumn TaxCodeBtn = new DataGridViewButtonColumn(); // B U T T O N
            AddDataGridButtonColumn(TaxCodeBtn, IsAddMode);

            DataGridViewColumn TaxRate = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(TaxRate, "Tax Rate", true);
        }

        private void GrossPrice()
        {
            DataGridViewColumn GrossPrice = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(GrossPrice, "Gross Price", false);
        }

        private void TotalLc()
        {
            DataGridViewColumn TotalLC = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(TotalLC, "Total(LC)", true);
        }

        private void GrossTotal()
        {
            DataGridViewColumn GrossTotal = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(GrossTotal, "Gross Total (LC)", true);
        }

        private void Project(bool IsAddMode)
        {
            DataGridViewColumn Project = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(Project, "Project", true);

            DataGridViewButtonColumn ProjectBtn = new DataGridViewButtonColumn(); // B U T T O N
            AddDataGridButtonColumn(ProjectBtn, IsAddMode);
        }

        private void Department(bool IsAddMode)
        {
            DataGridViewColumn Department = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(Department, "Department", true);

            DataGridViewButtonColumn DeptBtn = new DataGridViewButtonColumn(); // B U T T O N
            AddDataGridButtonColumn(DeptBtn, IsAddMode);
        }

        private void Brand(bool IsAddMode)
        {
            DataGridViewColumn Brand = new DataGridViewTextBoxColumn(); //
            AddDataGridViewColumn(Brand, "Brand", true);

            DataGridViewButtonColumn BrandBtn = new DataGridViewButtonColumn(); // B U T T O N
            AddDataGridButtonColumn(BrandBtn, IsAddMode);
        }

        private void Remarks()
        {
            DataGridViewColumn Remarks = new DataGridViewTextBoxColumn(); // 
            AddDataGridViewColumn(Remarks, "Remarks", false);
        }

        private void AddDataGridViewColumn(DataGridViewColumn col, string colName, bool isReadonly)
        {
            col.Name = colName;
            dgv.Columns.Add(col);
            dgv.Columns[colName].ReadOnly = false;
        }

        private void AddDataGridButtonColumn(DataGridViewButtonColumn col, bool isVisible)
        {
            col.Text = "...";
            col.UseColumnTextForButtonValue = true;
            dgv.Columns.Add(col);
            col.Visible = isVisible;
        }


        private void dataGridLayout()
        {
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //dgv.EnableHeadersVisualStyles = false;
            //dgv.MultiSelect = true;

            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
        }

        public Int32 GetSelectedItems(DataGridView dgvget, DataGridView dgvpost)
        {
            int negaQty = 0;
            int i = dgvpost.Rows.Count;

            if (dgvget.SelectedRows.Count > 0)
            {
                IEnumerable<DataGridViewRow> dgvSortInsert = dgvget.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(x => x.Index);
                itemSelected = new List<string>();
                foreach (DataGridViewRow dr in dgvSortInsert.ToList())
                {
                    dgvpost.Rows.Add();
                    DataGridViewRow row = dgvpost.Rows[i];
                    row.Cells[0].Value = dr.Cells[1].Value;
                    row.Cells[1].Value = dr.Cells[2].Value; //ItemCode
                    row.Cells[2].Value = dr.Cells[3].Value;
                    row.Cells[3].Value = "1";
                    row.Cells[4].Value = "1"; //Requested to remove logic 07212021 //InventoryTransferRequestService.GetCartonQty(dr.Cells[2].Value.ToString(), 1); ;
                    //row.Cells[4].Value = dr.Cells[5].Value;
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

                    itemSelected.Add(dr.Cells[2].Value.ToString());

                    i++;

                    dgvget.Rows.RemoveAt(dr.Index);
                }

                // oSelectedItems;

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
                row.Cells[1].Value = dr.Cells[2].Value;
                row.Cells[2].Value = dr.Cells[3].Value;
                row.Cells[3].Value = "1";
                row.Cells[4].Value = "0";
                //row.Cells[4].Value = dr.Cells[5].Value;
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
                row.Cells[0].Value = dr.Cells[1].Value;
                StaticHelper._MainForm.Invoke(new Action(() =>
        StaticHelper._MainForm.Progress($"Please wait until all data are transfered. {i} out of {dgvget.Rows.Count}", i, dgvget.Rows.Count)
    ));
                i++;

                //if (Convert.ToDecimal(dr.Cells[3].Value) > 0)
                //{

                //}
                //else
                //{
                //    negaQty += 1;
                //}

            }
            //dgvget.DataSource = null;
            //dgvget.Rows.Clear();
            StaticHelper._MainForm.ProgressClear();
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
                dgv.Columns[4].Name = "Ordered Quantity";
                //dgv.Columns[4].Name = "BrandCode";
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

        public void ItemListColumn(DataGridView dgv)
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

            dgv.ColumnCount = 14;

            dgv.Columns[0].Name = "Item code";
            dgv.Columns[0].ReadOnly = true;

            dgv.Columns[1].Name = "Barcode";
            dgv.Columns[1].ReadOnly = true;

            dgv.Columns[2].Name = "Description";
            dgv.Columns[2].ReadOnly = true;

            dgv.Columns[3].Name = "Quantity";

            dgv.Columns[4].Name = "Gross Price"; // 10

            dgv.Columns[5].Name = "Chain";
            dgv.Columns[5].ReadOnly = true;

            dgv.Columns[6].Name = "Style Code";
            dgv.Columns[6].ReadOnly = true;

            dgv.Columns[7].Name = "Color";
            dgv.Columns[7].ReadOnly = true;

            dgv.Columns[8].Name = "Size";
            dgv.Columns[8].ReadOnly = true;

            dgv.Columns[9].Name = "Section";
            dgv.Columns[9].ReadOnly = true;

            dgv.Columns[10].Name = "Available";
            dgv.Columns[10].ReadOnly = true;

            dgv.Columns[11].Name = "Unit Price";
            dgv.Columns[11].ReadOnly = true;

            dgv.Columns[12].Name = "Tax Rate";
            dgv.Columns[12].ReadOnly = true;

            dgv.Columns[13].Name = "Tax Amount";
            dgv.Columns[13].ReadOnly = true;
        }

        public static void dataGridLayout(DataGridView dgv)
        {

            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.MultiSelect = false;
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
                dgv.Columns[10].ReadOnly = true;    //Discount %
                dgv.Columns[11].ReadOnly = true;    //Discount
                dgv.Columns[12].ReadOnly = true;    //Warehouse
                dgv.Columns[13].ReadOnly = true;    //WHS Button
                dgv.Columns[14].ReadOnly = false;   //Tax
                dgv.Columns[15].ReadOnly = true;    //Tax Button
                dgv.Columns[16].ReadOnly = true;    //Tax Rate
                dgv.Columns[17].ReadOnly = true;    //Line Total
                dgv.Columns[18].ReadOnly = true;    //Gross Total
                dgv.Columns[19].ReadOnly = true;    //Price After Discount
            }
        }

    }
}

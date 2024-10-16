using System;
using Context;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms.VisualStyles;
using PresenterLayer.Helper;
using DirecLayer;
using PresenterLayer.Views;
using System.Linq;

namespace PresenterLayer.Services
{
    class ITM_Preview
    {
        public frmItemMasterData imd { get; set; }
        SAPHanaAccess sapHana = new SAPHanaAccess();
        DataHelper helper = new DataHelper();
        string sColName { get; set; } = "ItemCode";
        

        #region Tickbox Header
        public void checkheader_OnCheckBoxHeaderClick(CheckBoxHeaderCellEventArgs e)
        {
            if (imd.dgvItemList.Rows.Count > 0)
            {
                imd.dgvItemList.BeginEdit(true);
                foreach (DataGridViewRow item in imd.dgvItemList.Rows)
                {
                    item.Cells[0].Value = e.IsChecked;
                }
            }
            imd.dgvItemList.EndEdit();
        }

        public class CheckBoxHeaderCellEventArgs : EventArgs
        {
            private bool _isChecked;
            public bool IsChecked
            {
                get { return _isChecked; }
            }

            public CheckBoxHeaderCellEventArgs(bool _checked)
            {
                _isChecked = _checked;

            }

        }

        public delegate void CheckBoxHeaderClickHandler(CheckBoxHeaderCellEventArgs e);

        public class CheckBoxHeaderCell : DataGridViewColumnHeaderCell
        {
            Size checkboxsize;
            bool ischecked;
            Point location;
            Point cellboundsLocation;
            CheckBoxState state = CheckBoxState.UncheckedNormal;

            public event CheckBoxHeaderClickHandler OnCheckBoxHeaderClick;

            public CheckBoxHeaderCell()
            {
                location = new Point();
                cellboundsLocation = new Point();
                ischecked = false;
            }

            protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
            {
                /* Make a condition to check whether the click is fired inside a checkbox region */
                Point clickpoint = new Point(e.X + cellboundsLocation.X, e.Y + cellboundsLocation.Y);

                if ((clickpoint.X > location.X && clickpoint.X < (location.X + checkboxsize.Width)) && (clickpoint.Y > location.Y && clickpoint.Y < (location.Y + checkboxsize.Height)))
                {
                    ischecked = !ischecked;
                    if (OnCheckBoxHeaderClick != null)
                    {
                        OnCheckBoxHeaderClick(new CheckBoxHeaderCellEventArgs(ischecked));
                        this.DataGridView.InvalidateCell(this);
                    }
                }
                base.OnMouseClick(e);
            }

            protected override void Paint(Graphics graphics, Rectangle clipBounds,
                 Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle
                advancedBorderStyle, DataGridViewPaintParts paintParts)
            {

                base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState,
               value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

                checkboxsize = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal);
                location.X = cellBounds.X + (cellBounds.Width / 2 - checkboxsize.Width / 2);
                location.Y = cellBounds.Y + (cellBounds.Height / 2 - checkboxsize.Height / 2);
                cellboundsLocation = cellBounds.Location;

                if (ischecked)
                    state = CheckBoxState.CheckedNormal;
                else
                    state = CheckBoxState.UncheckedNormal;

                CheckBoxRenderer.DrawCheckBox(graphics, location, state);

            }
        }

        #endregion

        public void Form_Load()
        {
            dgvSetup();
            
        }
        
        void dgvSetup()
        {
            try
            {
                var dgv = imd.dgvItemList;

                dgv.Columns.Clear();
                dgv.Rows.Clear();

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.MultiSelect = false;
                dgv.RowTemplate.Resizable = DataGridViewTriState.False;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.RowHeadersVisible = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dgv.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);
                dgv.ScrollBars = ScrollBars.Both;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                //===============================HEADER WITH TICKBOX=========================
                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                var checkheader = new CheckBoxHeaderCell();
                checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
                col.HeaderCell = checkheader;
                dgv.Columns.Add(col);
                //===========================================================================             

                DataGridViewCheckBoxColumn col0 = new DataGridViewCheckBoxColumn();
                dgv.Columns.Add(col0);
                dgv.Columns[1].Name = "Existing Item";

                dgv.Columns.Add("ItemCode", "Item No");
                dgv.Columns.Add("BarCode", "Bar Code");
                dgv.Columns.Add("ItemName", "Item Name");
                dgv.Columns.Add("SRP", "SRP");
                dgv.Columns.Add("OutrightPrice", "Outright Price");
                dgv.Columns.Add("Brand", "Brand");
                dgv.Columns.Add("SizeName", "Size Name");
                dgv.Columns.Add("ParentCode", "Parent Code");
                dgv.Columns.Add("ChildName", "ChildName");
                dgv.Columns.Add("Dept", "Department");
                dgv.Columns.Add("SubDept", "Sub-Department");
                dgv.Columns.Add("Cat", "Category");
                dgv.Columns.Add("SubCat", "Sub-Category");
                dgv.Columns.Add("Style", "Style");
                dgv.Columns.Add("Class", "Class");
                dgv.Columns.Add("SubClass", "Sub-Class");
                dgv.Columns.Add("Packaging", "Packaging");
                dgv.Columns.Add("Specs", "Specification");
                dgv.Columns.Add("Collection", "Collection");
                dgv.Columns.Add("Size", "Size");
                dgv.Columns[21].Visible = false;
                dgv.Columns.Add("Color", "Color");
                dgv.Columns[22].Visible = false;
                dgv.Columns.Add("Path", "Path");
                dgv.Columns[23].Visible = false;

            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        public void LoadData()
        {
            if (imd.dgvItemList.RowCount > 0)
            {
                imd.dgvItemList.Rows.Clear();
            }
            string Company = helper.ReadDataRow(sapHana.Get(SP.ITM_Company), 0,"",0);
            string Brand = imd.U_ID001.Text;
            string Category = imd.U_ID020.Text;
            string SubCategory = imd.U_ID021.Text;
            string Style = imd.U_ID012.Text;
            var dgvColor = imd.dgvColor;
            var dgvSize = imd.dgvSize;
            int countColor = 0;
            int countSize = 0;

            ////Color
            foreach (DataGridViewRow row in imd.dgvColor.Rows)
            {
                if ((bool)row.Cells[0].Value == true)
                {
                    countColor++;
                }
            }
            if (countColor == 0)
            {
                StaticHelper._MainForm.ShowMessage("Please fillup the following fields (Brand,Category,Sub Category,Color,Size,Style)", true);
            }

            //Size
            //Erwin
            //3-1-2019
            foreach (DataGridViewRow row in imd.dgvSize.Rows)
            {
                if ((bool)row.Cells[0].Value == true)
                {
                    countSize++;
                }
            }

            if (countSize == 0)
            {
                StaticHelper._MainForm.ShowMessage("Please fillup the following fields (Brand,Category,Sub Category,Color,Size,Style)", true);
            }


            if (string.IsNullOrEmpty(Brand) ||
                string.IsNullOrEmpty(Category) ||
                string.IsNullOrEmpty(SubCategory) ||
                string.IsNullOrEmpty(Style))
            { StaticHelper._MainForm.ShowMessage("Please fillup the following fields (Brand,Category,Sub Category,Color,Size,Style)", true); }
            else
            {
                foreach (DataGridViewRow drColor in dgvColor.Rows)
                {
                    if (bool.Parse(drColor.Cells[0].Value.ToString()))
                    {
                        foreach (DataGridViewRow drSize in dgvSize.Rows)
                        {
                            if (bool.Parse(drSize.Cells[0].Value.ToString()))
                            {
                                var Size = LibraryHelper.DataGridViewRowRet(drSize, "Code");
                                var Color = LibraryHelper.DataGridViewRowRet(drColor, "Code");
                                var Path = LibraryHelper.DataGridViewRowRet(drColor, "Path");
                                var ItemCode = $"{Company}{Brand}{Category}{SubCategory}{Size}{Color}{Style}";
                                var ItemName = (string.IsNullOrEmpty(imd.U_ID017.Text) ? "" : imd.U_ID017.Text) +
                                                (string.IsNullOrEmpty(imd.U_ID025.Text) ? "" : $" {imd.U_ID025.Text}") +
                                                (string.IsNullOrEmpty(imd.U_ID004.Text) ? "" : $" {imd.U_ID004.Text}") +
                                                (string.IsNullOrEmpty(imd.U_ID005.Text) ? "" : $" {imd.U_ID005.Text}") +
                                                (string.IsNullOrEmpty(imd.U_ID013.Text) ? "" : $" {imd.U_ID013.Text}") +
                                                (string.IsNullOrEmpty(imd.U_ID014.Text) ? "" : $" {imd.U_ID014.Text}") +
                                                (string.IsNullOrEmpty(imd.U_ID015.Text) ? "" : $" {imd.U_ID015.Text}") +
                                                (string.IsNullOrEmpty(imd.U_ID016.Text) ? "" : $" {imd.U_ID016.Text}");

                                
                                var dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_ItemCode), 1,"",0), ItemCode));
                                var IsExist = LibraryHelper.DataExist(dt);
                                var CodeBar = IsExist ? LibraryHelper.DataTableRet(dt, 0, "CodeBars", "") : "";

                                imd.dgvItemList.Rows.Add(true, IsExist,
                                                        ItemCode, CodeBar,
                                                        (ItemName.Length <= 100 ? ItemName : ItemName.Substring(0, 100)).ToUpper(),
                                                        imd.Price.Text,
                                                        imd.OutrightPrice.Text,
                                                        imd.U_Name_ID001.Text,
                                                        LibraryHelper.DataGridViewRowRet(drSize, "Size Name"),
                                                        LibraryHelper.DataGridViewRowRet(drSize, "Parent Code"),
                                                        LibraryHelper.DataGridViewRowRet(drColor, "Child Name"),
                                                        imd.U_ID002.Text,
                                                        imd.U_ID003.Text,
                                                        imd.U_ID004.Text,
                                                        imd.U_ID005.Text,
                                                        imd.U_ID025.Text,
                                                        imd.U_ID013.Text,
                                                        imd.U_ID014.Text,
                                                        imd.U_ID015.Text,
                                                        imd.U_ID016.Text,
                                                        imd.U_ID017.Text,
                                                        Size, Color, Path);
                            }
                        }
                    }
                }

                foreach (DataGridViewColumn dc in imd.dgvItemList.Columns)
                {
                    if (dc.Index.Equals(3) || dc.Index.Equals(4) || dc.Index.Equals(5) || dc.Index.Equals(6) || dc.Index.Equals(0))
                    { dc.ReadOnly = false; }
                    else
                    { dc.ReadOnly = true; }
                }
            }
        }

        public void ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (dgv.Rows.Count > 0)
            { sColName = dgv.Columns[e.ColumnIndex].Name; }
            else
            { sColName = ""; }
        }

        public void SearchEngine(DataGridView dt, Control TextSearch)
        {
            try
            {
                if (dt.Columns.Count > 1)
                {
                    foreach (DataGridViewRow row in dt.Rows)
                    {
                        if (row.Cells[sColName].Value.ToString().ToUpper().StartsWith(TextSearch.Text.ToUpper()))
                        {
                            row.Selected = true;
                            dt.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                        else
                        { row.Selected = false; }
                    }
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        public void IsInputtedValueNumber(DataGridViewCellEventArgs e)
        {
            try
            {
                string oEditField = imd.dgvItemList.Columns[e.ColumnIndex].Name;

                if (oEditField == "SRP" || oEditField == "OutrightPrice")
                {
                    
                    string strNumber = imd.dgvItemList.Rows[e.RowIndex].Cells[oEditField].Value.ToString();

                    if (strNumber.Any(char.IsDigit) == false)
                    {
                        imd.dgvItemList.Rows[e.RowIndex].Cells[oEditField].Value = "1";
                        StaticHelper._MainForm.ShowMessage("Price inputted is not a number.", true);
                    }

                    imd.dgvItemList.Rows[e.RowIndex].Cells[oEditField].Value = Convert.ToDouble(imd.dgvItemList.Rows[e.RowIndex].Cells[oEditField].Value).ToString("#,###.00");
                }
                
            }
            catch (Exception ex)
            {
              StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }
    }
}

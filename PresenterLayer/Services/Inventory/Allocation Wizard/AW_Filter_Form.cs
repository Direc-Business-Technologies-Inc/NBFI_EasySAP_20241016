using Context;
using DirecLayer;
using DomainLayer.Models.Inventory.Allocation_Wizard;
using PresenterLayer;
using PresenterLayer.Helper;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PresenterLayer
{
    public class AW_Filter_Form
    {
        public frmAW_Filter filter;
        public string sQuery { get; set; }

        public string TableID { get; set; }
        public string Type { get; set; }
        string iColumn { get; set; } = "Doc. No.";
        public DataGridViewRow dgvdr { get; set; }
        
        private SAPHanaAccess sapHana { get; set; }

        public AW_Filter_Form()
        {
            sapHana = new SAPHanaAccess();
        }


        #region Tickbox Header
        public void checkheader_OnCheckBoxHeaderClick(CheckBoxHeaderCellEventArgs e)
        {
            if (filter.dgvSearch.Rows.Count > 0)
            {
                filter.dgvSearch.BeginEdit(true);

                for (int i = 0; i < filter.dgvSearch.Rows.Count; i++)
                { filter.dgvSearch.Rows[i].Cells[0].Value = e.IsChecked; }

                //foreach (DataGridViewRow item in filter.dgvSearch.Rows)
                //{
                //    item.Cells[0].Value = e.IsChecked;
                //}
            }
            filter.dgvSearch.EndEdit();
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

        public void dgvSetup(DataGridView dgv, string TableID, string Type)
        {
            dgv.Columns.Clear();
            dgv.Rows.Clear();
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.RowHeadersVisible = false;
            dgv.MultiSelect = false;
            //===============================HEADER WITH TICKBOX=========================
            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
            var checkheader = new CheckBoxHeaderCell();
            checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
            col.HeaderCell = checkheader;
            dgv.Columns.Add(col);
            //===========================================================================                


            var dt = new DataTable();
            dt = sapHana.Get(sQuery);

            dgv.ColumnCount = dt.Columns.Count + 1;
            
            int icount = 1;
            foreach (DataColumn dc in dt.Columns)
            {
                dgv.Columns[icount].Name = dc.ColumnName;
                icount++;
            }

            //int drindex = 0;

            var model = SelectionModel.Selection.Where(x => x.TableID == TableID && x.Type == Type);

            var max = dt.Rows.Count;

            dgv.Columns[0].Width = 50;
            dgv.Columns[1].Visible = false;

            for (int i = 0; i < filter.dgvSearch.Columns.Count; i++)
            {
                var dc = filter.dgvSearch.Columns[i];
                if (dc.Index.Equals(0))
                { dc.ReadOnly = false; }
                else
                { dc.ReadOnly = true; }
            }
            
           StaticHelper._MainForm.Progress("Please wait...",1, 100);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];

                try
                { dgv.Rows.Add(); }
                catch
                {
                    StaticHelper._MainForm.ProgressClear();
                    break;
                }

                DataGridViewRow row = dgv.Rows[i];
                //drindex++;

                bool ischoose = false;

                if (model.Count() > 0)
                {
                    var tick = model.Where(x => x.ID == dr[0].ToString()).FirstOrDefault();
                    if (tick != null)
                    { ischoose = tick.Choose; }
                }

                row.Cells[0].Value = ischoose;
                
                var icol = 1;
                foreach (DataColumn dc in dt.Columns)
                {
                    string cname = dc.ColumnName;
                    row.Cells[icol].Value = dr[cname].ToString();
                    icol++;
                }
                
                if (i > 20)
                {
                    StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {i + 1} out of {max}", i + 1, max);
                }
                else if (max < 20 && (i + 1) == max)
                {
                    Application.DoEvents();
                }
                
            }
        }

        public void dgvSearch_ColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (filter.dgvSearch.Rows.Count > 0)
            { iColumn = filter.dgvSearch.Columns[e.ColumnIndex].Name; }

        }

        public void txtSearch_TextChanged(EventArgs e)
        {
            try
            {
                if (filter.dgvSearch.Columns.Count > 1)
                {
                    for (int i = 0; i < filter.dgvSearch.Rows.Count; i++)
                    {
                        var row = filter.dgvSearch.Rows[i];

                        if (string.IsNullOrEmpty(iColumn))
                        { iColumn = "Code"; }

                        if (row.Cells[iColumn].Value == null)
                        { return; }

                        if (row.Cells[iColumn].Value.ToString().ToUpper().Contains(filter.txtSearch.Text.ToUpper()))
                        {
                            row.Selected = true;
                            filter.dgvSearch.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                        else
                        { row.Selected = false; }
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        public void dgvSearch_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (filter.dgvSearch.Rows.Count > 0 && e.ColumnIndex == 0)
            {
                for (int i = 0; i < filter.dgvSearch.Rows.Count; i++)
                {
                    var dgvr = filter.dgvSearch.Rows[i];
                    dgvTick(TableID, Type, dgvr.Cells[1].Value.ToString(), Convert.ToBoolean(dgvr.Cells[0].Value));
                }
            }
            filter.dgvSearch.EndEdit();
        }

        bool IsNumeric(object e)
        {
            bool result = false;
            if (e != null)
            { result = int.TryParse(e.ToString(), out int n); }

            return result;
        }

        public void dgvTick(string TableID, string Type, string ID, bool val)
        {
            var model = SelectionModel.Selection.Where(x => x.TableID == TableID && x.ID == ID && x.Type == Type);

            if (model.Any())
            {
                if (val)
                { model.FirstOrDefault().Choose = val; }
                else { SelectionModel.Selection.Remove(model.FirstOrDefault()); }
            }
            else
            {
                if (val)
                {
                    SelectionModel.Selection.Add(new SelectionModel.SelectionData
                    {
                        Choose = val,
                        TableID = TableID,
                        ID = ID,
                        Type = Type
                    });
                }
            }
        }

        public void dgvNumberFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var dgv = (DataGridView)sender;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                var dr = dgv.Rows[i];
                for (int iCol = 0; iCol < dgv.Columns.Count; iCol++)
                {
                    var dc = dgv.Columns[iCol];
                    if (dc.Name == "Value")
                    {
                        try
                        {
                            if (IsNumeric(dr.Cells["Value"].Value))
                            {
                                double sNumber = double.Parse(dr.Cells["Value"].Value.ToString());
                                string ret = string.Format("{0:#,0}", sNumber);
                                dr.Cells["Value"].Value = ret;
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        public void Form_Closing()
        {
            if (dgvdr != null)
            {
                int cnt = 0;

                for (int i = 0; i < filter.dgvSearch.Rows.Count; i++)
                {
                    var row = filter.dgvSearch.Rows[i];
                    if (bool.Parse(row.Cells[0].Value.ToString()))
                    { cnt++; }
                }
                dgvdr.Cells["Value"].Value = $"{cnt} Selected {dgvdr.Cells["Name"].Value}";
            }
            StaticHelper._MainForm.ProgressClear();
        }
    }
}

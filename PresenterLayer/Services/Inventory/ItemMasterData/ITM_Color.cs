using Context;
using DirecLayer;
using PresenterLayer.Helper;
using PresenterLayer.Views;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PresenterLayer.Services
{
    public class ITM_Color
    {
        public frmItemMasterData imd { get; set; }
        SAPHanaAccess saphana = new SAPHanaAccess();

        #region Tickbox Header
        public void checkheader_OnCheckBoxHeaderClick(CheckBoxHeaderCellEventArgs e)
        {
            if (imd.dgvColor.Rows.Count > 0)
            {
                imd.dgvColor.BeginEdit(true);
                foreach (DataGridViewRow item in imd.dgvColor.Rows)
                {
                    item.Cells[0].Value = e.IsChecked;
                }
            }
            imd.dgvColor.EndEdit();
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

        public void LoadGridView()
        {
            DataGridView dgv = imd.dgvColor;
            dgv.Columns.Clear();
            dgv.Rows.Clear();
            
            //===============================HEADER WITH TICKBOX=========================
            //DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
            //var checkheader = new CheckBoxHeaderCell();
            //checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
            //col.HeaderCell = checkheader;
            //dgv.Columns.Add(col);
            //===========================================================================             


            DataGridViewCheckBoxColumn col0 = new DataGridViewCheckBoxColumn();
            dgv.Columns.Add(col0);
            dgv.Columns[0].Name = "Existing Item";

            DataTable dt = new DataTable();

            dt = saphana.Get(SP.ITM_Colors);

            int i = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                i++;
                DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
                dgv.Columns.Add(col1);
                dgv.Columns[i].Name = dc.ColumnName;
            }
            
            DataGridViewButtonColumn col2 = new DataGridViewButtonColumn();
            DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
            dgv.Columns.Add(col2);
            dgv.Columns.Add(col3);
            dgv.Columns[4].Name = "Browse";
            dgv.Columns[5].Name = "Path";
            
            foreach (DataRow dr in dt.Rows)
            {
                dgv.Rows.Add(false, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }

            dgvSetup(dgv);
        }

        void dgvSetup(DataGridView dt)
        {
            try
            {
                dt.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dt.Columns[1].Visible = false;
                dt.Columns[2].ReadOnly = true;
                dt.Columns[3].ReadOnly = true;
                dt.Columns[5].Visible = false;
                dt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dt.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dt.MultiSelect = false;
                dt.RowTemplate.Resizable = DataGridViewTriState.False;
                dt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dt.RowHeadersVisible = false;
                dt.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dt.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
                dt.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dt.Columns[2].Width = 100;
                dt.Columns[4].Width = 50;
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        public void CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.RowIndex >= 0)
            {
                int col_index = imd.dgvColor.CurrentCell.ColumnIndex;
                int row_index = imd.dgvColor.CurrentCell.RowIndex;

                if (e.ColumnIndex == 0)
                { ColorTick(row_index); }
                else if (e.ColumnIndex == 4)
                { ShowBrowse(row_index); }
            }
        }

        void ColorTick(int i)
        {
            var dgvColor = imd.dgvColor;

            if (Convert.ToBoolean(dgvColor.Rows[i].Cells[0].Value) == true)
            {
                dgvColor.Rows[i].Cells["Path"].Value = "";
                dgvColor.Rows[i].Cells["Browse"].Value = "";
            }
        }
        void ShowBrowse(int iRow)
        {
            var OFileDialog = imd.OFileDialog;
            var dgvColor = imd.dgvColor;

            OFileDialog.Title = "Images File";
            OFileDialog.FileName = "";
            OFileDialog.Filter = "All Files (JPEG/PNG/BMP)|*.jpg;*.jpeg;*.png;*.bmp|JPEG Files|*.jpg;*.jpeg|PNG Files|*.png|BMP Files|*.bmp";
            if (OFileDialog.ShowDialog() == DialogResult.OK)
            {
                dgvColor.Rows[iRow].Cells["Path"].Value = OFileDialog.FileName;
                dgvColor.Rows[iRow].Cells["Browse"].Value = Path.GetFileNameWithoutExtension(OFileDialog.FileName);
                dgvColor.Rows[iRow].Cells[0].Value = true;
            }
            else
            {
                dgvColor.Rows[iRow].Cells["Path"].Value = "";
                dgvColor.Rows[iRow].Cells["Browse"].Value = "";
            }
        }

        public void SearchColor(DataGridView dt, string i, Control TextSearch)
        {
            try
            {
                if (dt.Columns.Count > 0)
                {
                    foreach (DataGridViewRow row in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(i))
                        {
                            i = "Parent Name";
                        }
                        if (row.Cells[i].Value.ToString().ToUpper().StartsWith(TextSearch.Text.ToUpper()))
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

        public void dgvColor_CellValueChanged(DataGridViewCellCancelEventArgs e)
        {
            var dgv = imd.dgvColor;
            var rowDgv = e.RowIndex;
            var actVal = bool.Parse(dgv.Rows[rowDgv].Cells[0].Value.ToString());
            
            var isChecked = 0;

            if (actVal)
            { return; }
            else
            { isChecked = 1; }

            foreach (DataGridViewRow dr in dgv.Rows)
            {
                if (bool.Parse(dr.Cells[0].Value.ToString()) && dr.Cells[1].Value.ToString() == imd.dgvColor.Rows[rowDgv].Cells[1].Value.ToString())
                {
                    isChecked++;
                    if (isChecked > 1)
                    {
                        e.Cancel = true;
                        StaticHelper._MainForm.ShowMessage("You cannot select two colors under the same Parent Color.", true);
                    }
                }
            }
        }

    }
}

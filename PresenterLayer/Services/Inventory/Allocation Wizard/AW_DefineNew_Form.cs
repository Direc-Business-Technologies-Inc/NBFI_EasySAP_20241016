using Context;
using DirecLayer;
using PresenterLayer;
using PresenterLayer.Helper;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PresenterLayer
{
    public class AW_DefineNew_Form
    {
        public frmAW_DefineNew definenew { get; set; }
        public frmAllocationWizard faw { get; set; }
        private DataHelper helper { get; set; }
        private SAPHanaAccess sapHana { get; set; }

        #region Tickbox Header
        public void checkheader_OnCheckBoxHeaderClick(CheckBoxHeaderCellEventArgs e)
        {
            if (definenew.dgvDefine.Rows.Count > 0)
            {
                definenew.dgvDefine.BeginEdit(true);

                for (int i = 0; i < definenew.dgvDefine.Rows.Count; i++)
                { definenew.dgvDefine.Rows[i].Cells[0].Value = e.IsChecked; }

                //foreach (DataGridViewRow item in definenew.dgvDefine.Rows)
                //{
                //    item.Cells[0].Value = e.IsChecked;
                //}
            }
            definenew.dgvDefine.EndEdit();
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

        public void dgvSetup(DataTable dt)
        {
            var dgv = definenew.dgvDefine;
            dgv.Columns.Clear();
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.RowHeadersVisible = false;
            //===============================HEADER WITH TICKBOX=========================
            var col = new DataGridViewCheckBoxColumn();
            var checkheader = new CheckBoxHeaderCell();
            checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
            col.HeaderCell = checkheader;
            dgv.Columns.Add(col);
            //===========================================================================                
            
            dgv.DataSource = dt;
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns[1].Visible = false;
            dgv.Columns[3].Visible = false;
            dgv.Columns[4].Visible = false;

            for (int i = 0; i < definenew.dgvDefine.Columns.Count; i++)
            {
                var dc = definenew.dgvDefine.Columns[i];
                if (dc.Index.Equals(0))
                { dc.ReadOnly = false; }
                else
                { dc.ReadOnly = true; }
            }
        }

        public void CommandClick(DataGridView dgv)
        {
            StringBuilder param = new StringBuilder();

            for (int i = 0; i < definenew.dgvDefine.Rows.Count; i++)
            {
                var dr = definenew.dgvDefine.Rows[i];
                if (dr.Cells[0].Value != null)
                {
                    if (bool.Parse(dr.Cells[0].Value.ToString()))
                    {
                        param.Append($"'{dr.Cells[1].Value.ToString()}',");
                    }
                }
            }
            
            var sQuery = dgv.Name.Equals("dgvItemOtherParam") ? helper.ReadDataRow(sapHana.Get(SP.AW_GetIOPByCode), 1, "", 0) : helper.ReadDataRow(sapHana.Get(SP.AW_GetCOPByCode), 1, "", 0);
            var sFormat = string.Format(sQuery, param.ToString());

            var dt = new DataTable();
            dt = sapHana.Get(sFormat);

            for (int i = dgv.Rows.Count; i > 6; i--)
            { dgv.Rows.RemoveAt(i - 1); }

            if (LibraryHelper.DataExist(dt))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    dgv.Rows.Add(dr[0], dr[1], "", dr[2], dr[3]);
                }
                
            }

            definenew.Dispose();
        }
    }
}

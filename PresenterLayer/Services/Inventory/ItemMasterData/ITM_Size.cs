using Context;
using DirecLayer;
using PresenterLayer.Helper;
using PresenterLayer.Views;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PresenterLayer.Services
{
   public class ITM_Size
    {
        public frmItemMasterData imd { get; set; }
        SAPHanaAccess sapHana = new SAPHanaAccess();
        DataHelper helper = new DataHelper();
        #region Tickbox Header
        public void checkheader_OnCheckBoxHeaderClick(CheckBoxHeaderCellEventArgs e)
        {
            if (imd.dgvSize.Rows.Count > 0)
            {
                imd.dgvSize.BeginEdit(true);
                foreach (DataGridViewRow item in imd.dgvSize.Rows)
                {
                    item.Cells[0].Value = e.IsChecked;
                }
            }
            imd.dgvSize.EndEdit();
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

        public void LoadSizeCategories()
        {
            var frm = imd;
            //frm.U_ID006.DataSource = null;
            frm.U_ID006.ValueMember = "Code";
            frm.U_ID006.DisplayMember = "Name";
            frm.U_ID006.DataSource = sapHana.Get(SP.ITM_PSize);
            SelectedIndexChanged();
        }

        public void SubClass_TextChanged()
        {
            var query = string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_PSizeByBrand), 1,"",0),
                                            imd.U_ID001.Text,
                                            imd.U_ID020.Text,
                                            imd.U_ID021.Text,
                                            imd.U_ID002.Text,
                                            imd.U_ID003.Text);

            //Added on 082019 due to advance notification of "Please choose a size category first!"
            string SelVal = helper.ReadDataRow(sapHana.Get(query), 0, "", 0);
            //if (SelVal != "") { imd.U_ID006.SelectedValue = SelVal; }
            imd.U_ID006.SelectedValue = SelVal;
        }

        public void SelectedIndexChanged()
        {
            if (!string.IsNullOrEmpty(imd.U_ID006.SelectedValue != null ? imd.U_ID006.SelectedValue.ToString() : ""))
            {
                var dgv = imd.dgvSize;
                dgv.Columns.Clear();
                dgv.Rows.Clear();

                //===============================HEADER WITH TICKBOX=========================
                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                var checkheader = new CheckBoxHeaderCell();
                checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
                col.HeaderCell = checkheader;
                dgv.Columns.Add(col);
                //===========================================================================             

                var dt = new DataTable();
                dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_SizesByPSize), 1,"",0),
                                                                    imd.U_ID006.SelectedValue));

                int i = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    i++;
                    DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
                    dgv.Columns.Add(col1);
                    dgv.Columns[i].Name = dc.ColumnName;
                }
                
                foreach (DataRow dr in dt.Rows)
                {
                    dgv.Rows.Add(false, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                
                dgvSetup(dgv);
            }
            else
            {
                if (string.IsNullOrEmpty(StaticHelper._MainForm.lblStatus.Text) && imd.btnCommand.Enabled == true)
                { StaticHelper._MainForm.ShowMessage("Please choose a size category first!", true); }
            }

        }

        void dgvSetup(DataGridView dt)
        {
            try
            {
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
                dt.Columns[1].Visible = false;
                dt.Columns[2].ReadOnly = true;
                dt.Columns[3].ReadOnly = true;
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }
    }
}

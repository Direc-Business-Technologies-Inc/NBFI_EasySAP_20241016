using MetroFramework.Forms;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DomainLayer.Models.Inventory;
using DirecLayer;
using PresenterLayer.Helper;
using PresenterLayer.Services;
using MetroFramework;

namespace PresenterLayer.Views
{
    public partial class frmItems : MetroForm
    {
        //frmMain frmMain;
        frmBarcodeAll frmBarcodeAll;
        string TableID;
        public static string BPCode, PickDate;
        Int64 oDocEntry = Convert.ToInt64(PublicHelper.oDocEntry);
        int ColumnIndex = 2;
        private SAPHanaAccess sapHana { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmItems(frmBarcodeAll frmBarcodeAll)
        {
            InitializeComponent();
            //this.frmMain = frmMain;
            sapHana = new SAPHanaAccess();
            this.frmBarcodeAll = frmBarcodeAll;
        }

        private void gvSetup(DataGridView dt)
        {
            try
            {
                dt.Rows.Clear();
                dt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dt.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dt.MultiSelect = false;
                dt.RowTemplate.Resizable = DataGridViewTriState.False;
                dt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dt.RowHeadersVisible = false;
                dt.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dt.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);
                dt.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                //===============================HEADER WITH TICKBOX=========================
                DataGridViewCheckBoxColumn col1 = new DataGridViewCheckBoxColumn();
                var checkheader = new CheckBoxHeaderCell();
                checkheader.OnCheckBoxHeaderClick += checkheader_OnCheckBoxHeaderClick;
                col1.HeaderCell = checkheader;
                dt.Columns.Add(col1);
                //===========================================================================

                dt.ColumnCount = 14;
                dt.Columns[1].Name = "LineNum";
                dt.Columns[2].Name = "BRAND";
                dt.Columns[3].Name = "SRP TYPE";
                dt.Columns[4].Name = "SKU";
                dt.Columns[5].Name = "ITEM NO";
                dt.Columns[6].Name = "ITEM LONG DESCRIPTION";
                dt.Columns[7].Name = "UOM";
                dt.Columns[8].Name = "ORDERED QTY";
                dt.Columns[9].Name = "COLOR";
                dt.Columns[10].Name = "SIZE";
                dt.Columns[11].Name = "SRP";
                dt.Columns[12].Name = "PICKED QTY";
                dt.Columns[13].Name = "IN CAMPAIGN";

                DataTable dtItems = new DataTable();
                var helper = new DataHelper();

                string selqry = "";

                if (Tag.ToString() == "WTQ1")
                {
                    if (BarcodeCtrl.CheckNoMaintenance(frmBarcodeAll.BPCode))
                    {
                        var GetBarcodeLineItemsITRNM = sapHana.Get(SP.BP_BarcodeAll_BarcodeLineItemsITRNM);
                        string BarcodeLineItemsITRNM = helper.ReadDataRow(GetBarcodeLineItemsITRNM, 1, "", 0);
                        selqry = string.Format(BarcodeLineItemsITRNM, PublicHelper.oDocEntry, frmBarcodeAll.BPCode, frmBarcodeAll.DocEntry2);
                    }
                    else
                    {
                        var GetBarcodeLineItemsITR = sapHana.Get(SP.BP_BarcodeAll_BarcodeLineItemsITR);
                        string BarcodeLineItemsITR = helper.ReadDataRow(GetBarcodeLineItemsITR, 1, "", 0);
                        selqry = string.Format(BarcodeLineItemsITR, PublicHelper.oDocEntry, frmBarcodeAll.BPCode, frmBarcodeAll.DocEntry2);
                    }
                    //MessageBox.Show(BarcodeCtrl.CheckNoMaintenance(frmBarcodeAll.BPCode).ToString());
                }
                else if (Tag.ToString() == "RDR1")
                {
                    if (BarcodeCtrl.CheckNoMaintenance(frmBarcodeAll.BPCode))
                    {
                        var GetBarcodeLineItemsSONM = sapHana.Get(SP.BP_BarcodeAll_BarcodeLineItemsSONM);
                        string BarcodeLineItemsSONM = helper.ReadDataRow(GetBarcodeLineItemsSONM, 1, "", 0);
                        selqry = string.Format(BarcodeLineItemsSONM, PublicHelper.oDocEntry, frmBarcodeAll.BPCode, frmBarcodeAll.DocEntry2);
                    }
                    else
                    {
                        var GetBarcodeLineItemsSO = sapHana.Get(SP.BP_BarcodeAll_BarcodeLineItemsSO);
                        string BarcodeLineItemsSO = helper.ReadDataRow(GetBarcodeLineItemsSO, 1, "", 0);
                        selqry = string.Format(BarcodeLineItemsSO, PublicHelper.oDocEntry, frmBarcodeAll.BPCode, frmBarcodeAll.DocEntry2);
                    }
                }
                else if (Tag.ToString() == "POR1" && frmBarcodeAll.UPCcheck == false)
                {
                    var GetBarcodeLineItemsPO = sapHana.Get(SP.BP_BarcodeAll_BarcodeLineItemsPO);
                    string BarcodeLineItemsPO = helper.ReadDataRow(GetBarcodeLineItemsPO, 1, "", 0);
                    selqry = string.Format(BarcodeLineItemsPO, PublicHelper.oDocEntry, frmBarcodeAll.BPCode);
                }
                else if (Tag.ToString() == "POR1" && frmBarcodeAll.UPCcheck == true)
                {
                    var GetBarcodeLineItemsPOforUPC = sapHana.Get(SP.BP_BarcodeAll_BarcodeLineItemsPOforUPC);
                    string BarcodeLineItemsPOforUPC = helper.ReadDataRow(GetBarcodeLineItemsPOforUPC, 1, "", 0);
                    selqry = string.Format(BarcodeLineItemsPOforUPC, PublicHelper.oDocEntry, frmBarcodeAll.BPCode);
                }
                else if (Tag.ToString() == "WTQ2")
                {
                    string sTable = "OWTQ";
                    var GetBarcodeLineItemsITRforCPOStore = sapHana.Get(SP.BP_BarcodeAll_BarcodeLineItemsITRCpoStore);
                    string BarcodeLineItemsITRforCPOStore = helper.ReadDataRow(GetBarcodeLineItemsITRforCPOStore, 1, "", 0);
                    selqry = string.Format(BarcodeLineItemsITRforCPOStore, PublicHelper.oDocEntry, sTable, sTable.Remove(0, 1) + "1");
                }

                dtItems = sapHana.Get(selqry);

                foreach (DataRow dr in dtItems.Rows)
                {
                    dt.Rows.Add(false, dr["LineNum"].ToString(), dr["U_Brand"].ToString(), dr["SRPType"].ToString(), dr["SKU"].ToString()
                                , dr["ItemCode"].ToString(), dr["Dscription"].ToString(), dr["UOM"].ToString(), dr["Ordered Quantity"].ToString()
                                , dr["Color"].ToString(), dr["U_Size"].ToString(), Convert.ToDouble(dr["Price"]).ToString("#,##0.00"), dr["PickQuantity"].ToString(),
                                dr["InCampaign"].ToString());

                    //dt.Rows.Add(false, 0, "BARBIZON", "Regular", ""
                    //           , "1010060136038958", "FC-RP-UW-POLYESTER", "PCS", "2.000000"
                    //           , "MOCHA", "36A", Convert.ToDouble("429.75").ToString("#,##0.00"),"2.000000",
                    //           "Y");
                    
                }
                //MessageBox.Show(dtItems.Rows[0]["LineNum"].ToString() +
                //    dtItems.Rows[0]["U_Brand"].ToString() +
                //    dtItems.Rows[0]["SRPType"].ToString() +
                //    dtItems.Rows[0]["SKU"].ToString()
                //    +
                //    dtItems.Rows[0]["ItemCode"].ToString() +
                //    dtItems.Rows[0]["Dscription"].ToString() +
                //    dtItems.Rows[0]["UOM"].ToString() +
                //    dtItems.Rows[0]["Ordered Quantity"].ToString() +
                //    dtItems.Rows[0]["Color"].ToString() +
                //    dtItems.Rows[0]["U_Size"].ToString() +
                //    dtItems.Rows[0]["Price"].ToString() +
                //    dtItems.Rows[0]["PickQuantity"].ToString() +
                //    dtItems.Rows[0]["InCampaign"].ToString())
                //;

                //MessageBox.Show(Tag.ToString());
                foreach (DataGridViewRow vr in dt.Rows)
                {
                    var LinqLine = LinqAccess.frmBarcodeItem.Where(x => x.IsTick == true
                    && x.DocEntry == oDocEntry
                    && x.ItemCode == vr.Cells["ITEM NO"].Value.ToString()
                    && x.InCampaign == vr.Cells["IN CAMPAIGN"].Value.ToString()
                    && x.TableID == Tag.ToString()).Distinct();
                    if (LinqLine.Count() > 0)
                    { vr.Cells[0].Value = true; }
                }

                dt.Columns["LineNum"].Visible = false;

                dt.Columns["BRAND"].ReadOnly = true;
                dt.Columns["SRP Type"].ReadOnly = true;
                dt.Columns["SKU"].ReadOnly = true;
                dt.Columns["ITEM NO"].ReadOnly = true;
                dt.Columns["ITEM LONG DESCRIPTION"].ReadOnly = true;
                dt.Columns["UOM"].ReadOnly = true;
                dt.Columns["ORDERED QTY"].ReadOnly = true;
                dt.Columns["COLOR"].ReadOnly = true;
                dt.Columns["SIZE"].ReadOnly = true;
                dt.Columns["SRP"].ReadOnly = true;
                dt.Columns["PICKED QTY"].ReadOnly = true;
                dt.Columns["IN CAMPAIGN"].ReadOnly = true;

                foreach (DataGridViewRow row1 in dt.Rows)
                {
                    row1.HeaderCell.Value = String.Format("{0}", row1.Index + 1);
                }

                foreach (DataGridViewRow row in dt.Rows)
                {
                    double iPickQty = Convert.ToDouble(row.Cells[12].Value.ToString());
                    if (row.Cells[13].Value.ToString() == "Y" && iPickQty > 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }

                    if (row.Cells[3].Value.ToString() == "Markdown")
                    {
                        row.Cells[3].Style.ForeColor = Color.Red;
                    }

                    if (row.Cells[12].Value.ToString() == "0.000000")
                    {
                        row.Cells[12].Style.ForeColor = Color.Red;
                    }
                }

                dt.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;

            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message); }
        }

        #region Tickbox Header

        void checkheader_OnCheckBoxHeaderClick(CheckBoxHeaderCellEventArgs e)
        {
            gvItems.BeginEdit(true);
            foreach (DataGridViewRow item in gvItems.Rows)
            {
                item.Cells[0].Value = e.IsChecked;
            }
            gvItems.EndEdit();

        }

        public class CheckBoxHeaderCellEventArgs : EventArgs
        {
            private bool _isChecked;
            public bool IsChecked
            {
                get { return _isChecked; }
            }

            public CheckBoxHeaderCellEventArgs(bool _checked)
            { _isChecked = _checked; }

        }

        public delegate void CheckBoxHeaderClickHandler(CheckBoxHeaderCellEventArgs e);

        class CheckBoxHeaderCell : DataGridViewColumnHeaderCell
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

        private void btnCommand_Click(object sender, EventArgs e)
        { Close(); }

        private void frmItems_Load(object sender, EventArgs e)
        {
            gvSetup(gvItems); TableID = Tag.ToString();
        }

        private void frmItems_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                if (Size == new Size(MinimumSize.Width, MinimumSize.Height))
                {
                    Size = new Size(Screen.PrimaryScreen.Bounds.Width - 50, Screen.PrimaryScreen.Bounds.Height - 220);
                    Location = new Point(25, 25);
                }
                else { Size = new Size(MinimumSize.Width, MinimumSize.Height); }
            }
        }

        private void gvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (gvItems.Columns.Count > 1 && e.ColumnIndex == 0)
            {
                DataGridViewRow gvr = gvItems.Rows[e.RowIndex];

                if (Convert.ToBoolean(gvr.Cells[0].Value) == true)
                { gvTick(gvr, e.ColumnIndex, e.RowIndex); }
                else
                { gvUntick(gvr, e.ColumnIndex, e.RowIndex); }

            }
            else if (gvItems.Columns.Count > 1 && e.ColumnIndex == 6)
            {
                string qty = gvItems.Rows[e.RowIndex].Cells["PICKED QTY"].Value.ToString();
                string itemcode = gvItems.Rows[e.RowIndex].Cells["ITEM NO"].Value.ToString();
                foreach (var item in LinqAccess.frmBarcodeItem.Where(a => a.ItemCode == itemcode))
                {
                    item.Qty = qty;
                }
            }
            gvItems.EndEdit();
        }

        void gvTick(DataGridViewRow gvr, int iColumn, int iRow)
        {
            try
            {

                string oLineNume = gvr.Cells["ITEM NO"].Value.ToString();
                string qty = gvr.Cells["PICKED QTY"].Value.ToString();
                string strInCampaign = gvr.Cells["IN CAMPAIGN"].Value.ToString();
                double dPickQty = Convert.ToDouble(qty);
                if (strInCampaign == "Y")
                {
                    if (dPickQty > 0)
                    {
                        var LinqLines = LinqAccess.frmBarcodeItem.Distinct().Where(x => x.DocEntry == oDocEntry && x.ItemCode == oLineNume && x.TableID == TableID && x.Qty == qty);
                        int s = LinqLines.Count();

                        if (LinqLines.Count() == 0)
                        {
                            LinqAccess.frmBarcodeItem.Add(new LinqAccess.gvBarcodeItem
                            {
                                IsTick = true,
                                TableID = TableID,
                                DocEntry = oDocEntry,
                                ItemCode = oLineNume,
                                Qty = qty,
                                InCampaign = strInCampaign
                            });
                        }
                        else
                        {
                            foreach (var x in LinqLines.Where(x => x.IsTick == false))
                            { x.IsTick = true; }
                        }
                    }
                    else
                    {
                        gvr.Cells[0].Value = false;
                        StaticHelper._MainForm.ShowMessage("Unable to select the item. Please check the Picked Quantity.");
                    }
                }
                else
                {
                    gvr.Cells[0].Value = false;
                    StaticHelper._MainForm.ShowMessage("Unable to select the item. Please include this in the Campaign.");
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message); }
        }

        void gvUntick(DataGridViewRow gvr, int iColumn, int iRow)
        {
            try
            {
                LinqAccess.frmBarcodeItem.RemoveAll(x => x.DocEntry == oDocEntry && x.ItemCode == gvr.Cells["ITEM NO"].Value.ToString() && x.TableID == TableID && x.Qty == gvr.Cells["PICKED QTY"].Value.ToString());

            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message); }
        }

        private void frmItems_FormClosing(object sender, FormClosingEventArgs e)
        {
            //frmBarcodeAll.LoadLinq();
        }

        private void gvItems_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Space && gvItems.Focused == true)
            {
                int iRow = gvItems.SelectedRows[0].Index;
                DataGridViewRow gvr = gvItems.Rows[iRow];

                if (Convert.ToBoolean(gvr.Cells[0].Value) == false)
                {
                    gvItems.SelectedRows[0].Cells[0].Value = true;
                }
                else
                {
                    gvItems.SelectedRows[0].Cells[0].Value = false;
                }

                if (Convert.ToBoolean(gvr.Cells[0].Value) == true)
                { gvTick(gvr, 0, iRow); }
                else
                { gvUntick(gvr, 0, iRow); }
            }
        }

        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            if (gvItems.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in gvItems.Rows)
                {
                    if (row.Cells[ColumnIndex].Value != null)
                    {
                        if (row.Cells[ColumnIndex].Value.ToString().ToUpper().StartsWith(txtSearchItem.Text.ToUpper()))
                        {
                            row.Selected = true;
                            gvItems.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                        else
                        {
                            row.Selected = false;
                        }
                    }
                }
            }
        }

        private void gvItems_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex = e.ColumnIndex;

            if (gvItems.Rows.Count > 0 && e.ColumnIndex == 0)
            {
                foreach (DataGridViewRow gvr in gvItems.Rows)
                {
                    if (Convert.ToBoolean(gvr.Cells[0].Value) == true)
                    { gvTick(gvr, e.ColumnIndex, e.RowIndex); }
                    else { gvUntick(gvr, e.ColumnIndex, e.RowIndex); }
                }
                ActiveControl = btnCommand;
            }
        }
    }
}

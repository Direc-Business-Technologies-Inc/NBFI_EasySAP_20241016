using CrystalDecisions.Windows.Forms;
using DirecLayer;
using DirecLayer._02_Form.MVP.Presenters;
using MetroFramework;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using PresenterLayer.Helper.Unofficial_Sales;
using PresenterLayer.Services.Inventory.Inventory_Transfer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresenterLayer.Views.Inventory.Inventory_Transfer
{
    public partial class InventoryTransfer : MetroForm, IInventoryTransfer
    {
        public event EventHandler InventoryTransferLoad;
        public event EventHandler InventoryTransferBpClick;
        public event EventHandler InventoryTransferToWhsClick;
        public event EventHandler InventoryTransferFrmWhsClick;
        public event EventHandler InventoryTransferSalesEmployeeClick;
        public event EventHandler InventoryTransferSeriesChange;
        public event EventHandler InventoryTransferCompanyChange;
        public event EventHandler InventoryTransferTransferTypeChange;
        public event EventHandler InventoryTransferFromWhsChange;
        public event EventHandler InventoryTransferToWhsChange;
        public event EventHandler InventoryTransferAddItemClick;
        public event EventHandler InventoryTransferPostClick;
        public event EventHandler InventoryTransferFindDocument;
        public event EventHandler InventoryTransferChooseDocument;
        public event EventHandler InventoryTransferCopyFromChange;
        public event EventHandler InventoryTransferDeleteRowClick;
        public event EventHandler InventoryTransferCloseForm;
        public event EventHandler InventoryTransferNewDocument;
        public event EventHandler InventoryTransferSearchTextChange;
        public event EventHandler InventoryTransferSearchDocumentTextChange;
        public event EventHandler InventoryTransferPrintPreviewFromChange;
        public event DataGridViewCellEventHandler InventoryTransferCellClick;
        public event DataGridViewCellEventHandler InventoryTransferItemCellClick;
        public event DataGridViewCellEventHandler InventoryTransferChainCellClick;
        public event DataGridViewCellEventHandler InventoryTransferItemCellEndEdit;
        public event FormClosingEventHandler InventoryTransferCloseFormEvent;
        public event PreviewKeyDownEventHandler InventoryCopy;
        public event ScrollEventHandler InventoryUDFscroll;

        private string oProject;
        public static string oPriceList;
        public static string oTaxGroup;
        public static string oTaxRate;
        public static string oFWhsCode, oTWhsCode;
        public static string objType = "OWTR";
        public static string objType1 = "OWTQ";
        public static int oRowIndex;
        private string TotalBefDIsc, TotalQty, TotalDisc, TotalTax, TotalAftDisc;
        public static bool FindMode = false;
        public static string _DocEntry;
        private string sample;
        private string oSalesEmployee;
        private string oSeries;
        private string oCode;
        private string oName;

        public Panel Panel2 { get => panel2; set => panel2 = value; }
        public Button btnClose { get => BtnClose; set => BtnClose = value; }
        public Button btnRequest { get => BtnRequest; set => BtnRequest = value; }
        public CrystalReportViewer CrystalReportViewer1 { get => crystalReportViewer1; set => crystalReportViewer1 = value; }
        public DataGridView dgvItem { get => DgvItem; set => DgvItem = value; }

        public ComboBox vmbPrintPreview { get => CmbPrintPreview; set => CmbPrintPreview = value; }
        public TextBox txtPrintDocNo { get => TxtPrintDocNo; set => TxtPrintDocNo = value; }
        public ContextMenuStrip MsItems { get => msItems; set => msItems = value; }
        public ToolStripMenuItem DeleteItemsToolStripMenuItem { get => deleteItemsToolStripMenuItem; set => deleteItemsToolStripMenuItem = value; }
        public Button btnPrint { get => BtnPrint; set => BtnPrint = value; }
        public Button btnNewDocument { get => BtnNewDocument; set => BtnNewDocument = value; }
        public TabControl tabIT { get => TabIT; set => TabIT = value; }
        public Button btnCopyFrom { get => BtnCopyFrom; set => BtnCopyFrom = value; }
        public ComboBox cmbCopyFromOption { get => CmbCopyFromOption; set => CmbCopyFromOption = value; }
        public DataGridView dgvPreviewItem { get => DgvPreviewItem; set => DgvPreviewItem = value; }
        public TextBox txtSearch { get => TxtSearch; set => TxtSearch = value; }
        public ComboBox cmbFilterDocument { get => CmbFilterDocument; set => CmbFilterDocument = value; }
        public Button btnChoose { get => BtnChoose; set => BtnChoose = value; }
        public DataGridView dgvFindDocument { get => DgvFindDocument; set => DgvFindDocument = value; }
        public TextBox txtSearchDocument { get => TxtSearchDocument; set => TxtSearchDocument = value; }
        public TextBox TxtTotal { get => txtTotal; set => txtTotal = value; }
        public TextBox TxtTotalQty { get => txtTotalQty; set => txtTotalQty = value; }
        public TextBox TxtSalesEmployee { get => txtSalesEmployee; set => txtSalesEmployee = value; }
        public TextBox TxtRemarks { get => txtRemarks; set => txtRemarks = value; }
        public TextBox TxtITR_DocEntry { get => txtITR_DocEntry; set => txtITR_DocEntry = value; }
        public TextBox TxtITR_DocNum { get => txtITR_DocNum; set => txtITR_DocNum = value; }
        public TextBox txtDocNum { get => TxtDocNum; set => TxtDocNum = value; }
        public ComboBox CmbSeries { get => cmbSeries; set => cmbSeries = value; }
        public PictureBox PbFromWhsList { get => pbFromWhsList; set => pbFromWhsList = value; }
        public TextBox TxtDocStatus { get => txtDocStatus; set => txtDocStatus = value; }
        public DateTimePicker DtDocDate { get => dtDocDate; set => dtDocDate = value; }
        public DateTimePicker DtPostingDate { get => dtPostingDate; set => dtPostingDate = value; }
        public PictureBox PbToWhsList { get => pbToWhsList; set => pbToWhsList = value; }
        public TextBox TxtFWhsCode { get => txtFWhsCode; set => txtFWhsCode = value; }
        public TextBox TxtTWhsCode { get => txtTWhsCode; set => txtTWhsCode = value; }
        public ComboBox CbCompany { get => cbCompany; set => cbCompany = value; }
        public ComboBox CbTransferType { get => cbTransferType; set => cbTransferType = value; }
        public PictureBox PbBPList { get => pbBPList; set => pbBPList = value; }
        public TextBox TxtBpCode { get => txtBpCode; set => txtBpCode = value; }
        public TextBox TxtBpName { get => txtBpName; set => txtBpName = value; }
        public TextBox TxtAddress { get => txtAddress; set => txtAddress = value; }
        public TextBox TxtFromDoc { get => txtFromDoc; set => txtFromDoc = value; }
        public TextBox TxtDocentry { get => txtDocEntry1; set => txtDocEntry1 = value; }
        public Button btnItem { get => BtnItem; set => BtnItem = value; }
        public DataGridView UDF { get => DgvUdf; set => DgvUdf = value; }
        public string OSalesEmployee { get => oSalesEmployee; set => oSalesEmployee = value; }
        public string OSeries { get => oSeries; set => oSeries = value; }
        public string OProject { get => oProject; set => oProject = value; }
        public string OCode { get => oCode; set => oCode = value; }
        public string OName { get => oName; set => oName = value; }
        public string FindPageLimit
        {
            get => txtPageSizeLimit.Text;
            set => txtPageSizeLimit.Text = value;
        }
        public string series
        {
            get => cmbSeries.SelectedValue.ToString();
            set => cmbSeries.SelectedValue.ToString();
        }

        private void DgvUdf_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedCellEvent(this, InventoryTransferCellClick, e);
        }

        public InventoryTransferService Presenter
        {
            private get;
            set;
        }
        private void InventoryTransfer_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void pbBPList_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferBpClick, e);
        }

        private void pbFromWhsList_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferFrmWhsClick, e);
        }

        private void pbToWhsList_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferToWhsClick, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferSalesEmployeeClick, e);
        }
        

        private void TabIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TabIT.SelectedIndex)
            {
                case 0:
                    break;

                case 1:

                    break;

                case 2:
                    EventHelper.RaisedEvent(sender, InventoryTransferFindDocument, e);
                    CmbFilterDocument.Text = "All";
                    break;

                case 3:
                    //PublicStatic.frmMain.NotiMsg("Warning: Please select document first", Color.Red);
                    if (txtDocEntry1.Text == string.Empty)
                    {
                        TabIT.SelectedIndex = 0;
                        StaticHelper._MainForm.ShowMessage("Warning: Please select document first", true);
                    }
                    else
                    {
                        TabIT.SelectedIndex = 3;
                        TxtPrintDocNo.Text = txtDocEntry1.Text;
                    }
                    break;

                default:
                    break;
            }
        }

        private void cmbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferSeriesChange, e);

        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferCompanyChange, e);

        }

        private void cbTransferType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferTransferTypeChange, e);

        }

        private void txtFWhsCode_TextChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferFromWhsChange, e);

        }

        private void txtTWhsCode_TextChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferToWhsChange, e);

        }

        private void dtPostingDate_DropDown(object sender, EventArgs e)
        {
            dtPostingDate.Format = DateTimePickerFormat.Short;
            dtPostingDate.Select();
        }

        private void dtDocDate_DropDown(object sender, EventArgs e)
        {
            dtDocDate.Format = DateTimePickerFormat.Short;
            dtDocDate.Select();
        }

        private void BtnItem_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferAddItemClick, e);
        }

        private void DgvItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex - 1;
            colIndex = colIndex < 0 ? 0 : colIndex;

            string columnName = DgvItem.Columns[colIndex].Name;

            if (columnName.Equals("Chain Description"))
            {
                //PasstoGetChain(colIndex, index);
                EventHelper.RaisedCellEvent(this, InventoryTransferChainCellClick, e);

                
            }

            EventHelper.RaisedCellEvent(this, InventoryTransferItemCellClick, e);
        }

        //private void PasstoGetChain(int colIndex , int index)
        //{
        //    Presenter.GetChain(colIndex, index);

        //}

        private void BtnRequest_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferPostClick, e);
        }

        private void BtnChoose_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferChooseDocument, e);
        }

        private void BtnCopyFrom_Click(object sender, EventArgs e)
        {
            CmbCopyFromOption.DroppedDown = true;
        }

        private void CmbCopyFromOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferCopyFromChange, e);
        }

        private void DgvPreviewItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedCellEvent(this, InventoryTransferItemCellClick, e);
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferDeleteRowClick, e);            
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to cancel the document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                EventHelper.RaisedEvent(sender, InventoryTransferCloseForm, e);
                Dispose();
            }
        }

        private void BtnNewDocument_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferNewDocument, e);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (txtDocEntry1.Text == string.Empty)
            {
                tabIT.SelectedIndex = 0;
                //PublicStatic.frmMain.NotiMsg("Warning: Please select document first", Color.Red);
                StaticHelper._MainForm.ShowMessage("Warning: Please select document first", true);

            }
            else
            {
                tabIT.SelectedIndex = 3;
                TxtPrintDocNo.Text = txtDocEntry1.Text;
            }

        }

        private void InventoryTransfer_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHelper.RaiseFormCloseEvent(sender, InventoryTransferCloseFormEvent, e);
        }

        private void DgvItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedCellEvent(sender, InventoryTransferItemCellEndEdit, e);
        }

        private void DgvItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    DgvItem.CurrentCell = dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex + 1];
                    DgvItem.Rows[e.RowIndex].Selected = true;
                    DgvItem.Focus();

                    var mousePosition = DgvItem.PointToClient(Cursor.Position);

                    msItems.Show(DgvItem, mousePosition);
                }
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferSearchTextChange, e);
        }

        private void DgvPreviewItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedCellEvent(sender, InventoryTransferItemCellEndEdit, e);
        }

        private void DgvPreviewItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    DgvPreviewItem.CurrentCell = dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex + 1];
                    DgvPreviewItem.Rows[e.RowIndex].Selected = true;
                    DgvPreviewItem.Focus();

                    var mousePosition = dgvItem.PointToClient(Cursor.Position);

                    msItems.Show(dgvItem, mousePosition);
                }
            }
        }

        private void TxtSearchDocument_TextChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferSearchDocumentTextChange, e);
        }

        private void DgvFindDocument_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            oRowIndex = e.ColumnIndex;
        }

        private void DgvFindDocument_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferChooseDocument, e);
        }

        private void DgvFindDocument_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvFindDocument.CurrentRow.Selected = true;
        }

        public void PrintPreviewItems(FileInfo[] files)
        {
            try
            {
                foreach (FileInfo file in files)
                {
                    CmbPrintPreview.Items.Add(file.Name);
                }
            }
            catch (Exception)
            {

            }
            
        }

        private void CmbPrintPreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryTransferPrintPreviewFromChange, e);

        }

        public void showSeries(DataTable resultOfQuery)
        {
            cmbSeries.DataSource = resultOfQuery;
        }

        private void DgvItem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            EventHelper.RaisedPreviewKeyDown(sender, InventoryCopy, e);
        }

        private void DgvUdf_Scroll(object sender, ScrollEventArgs e)
        {

            EventHelper.RaisedScroll(sender, InventoryUDFscroll, e);
        }

        private void DgvUdf_Leave(object sender, EventArgs e)
        {
            //EventHelper.RaisedLeave(sender, InventoryUDFscroll, e);
        }

        private void btnRefreshFindPage_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryTransferFindDocument, e);
            CmbFilterDocument.Text = "All";
        }

        private void btnBarcode_Click(object sender, EventArgs e)
        {
            ITRHelper.ITR_ImageFile(DgvItem,txtBpCode.Text,DtDocDate.Text,txtDocEntry1.Text);
        }

        public void showTransferType(DataTable resultOfQUery)
        {
            cbTransferType.DataSource = resultOfQUery;
        }
        public void showCompany(DataTable resultOfQuery)
        {
            cbCompany.DataSource = resultOfQuery;
        }

        public InventoryTransfer()
        {
            InitializeComponent();

        }

        private void InventoryTransfer_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            cmbSeries.DisplayMember = "Name";
            cmbSeries.ValueMember = "Code";
            cbTransferType.DisplayMember = "Name";
            cbTransferType.ValueMember = "Name";
            cbCompany.DisplayMember = "Name";
            cbCompany.ValueMember = "Code";
            DgvFindDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DgvUdf.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.Resizable = DataGridViewTriState.False);
            TxtITR_DocEntry.Focus();

            txtDocStatus.Text = "Open";

            EventHelper.RaisedEvent(sender, InventoryTransferLoad, e);

        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            //if (keyData == (Keys.Control | Keys.Shift | Keys.U))
            //{ ShowUDF(); }
            //else 
            if (keyData == (Keys.Control | Keys.D1))
            {
                TabIT.SelectedIndex = 0;
            }
            else if (keyData == (Keys.Control | Keys.D2))
            {
                TabIT.SelectedIndex = 1;
            }
            else if (keyData == (Keys.Control | Keys.D3))
            {
                TabIT.SelectedIndex = 2;
            }
            else if (keyData == (Keys.Control | Keys.D4))
            {
                if (txtDocEntry1.Text == string.Empty)
                {
                    TabIT.SelectedIndex = 0;
                    StaticHelper._MainForm.ShowMessage("Warning: Please select document first", true);
                }
                else
                {
                    TabIT.SelectedIndex = 3;
                    TxtPrintDocNo.Text = txtDocEntry1.Text;
                }
            }
            else if (keyData == (Keys.Control | Keys.D5))
            {
                TabIT.SelectedIndex = 4;
            }
            else if (keyData == Keys.Tab && txtBpCode.Focused && txtBpCode.Text == "")
            {
                EventHelper.RaisedEvent(null, InventoryTransferBpClick, null);
                //BPList();
                return true;
            }
            else if (keyData == Keys.Tab && txtFWhsCode.Focused && txtFWhsCode.Text == "")
            {
                EventHelper.RaisedEvent(null, InventoryTransferFrmWhsClick, null);
                //FromWarehouse();
                return true;
            }
            else if (keyData == Keys.Tab && txtTWhsCode.Focused && txtTWhsCode.Text == "")
            {
                EventHelper.RaisedEvent(null, InventoryTransferToWhsClick, null);
                //ToWarehouse();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Escape))
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    EventHelper.RaisedEvent(null, InventoryTransferCloseForm, null);
                    Dispose();
                }
            }
            else if (keyData == (Keys.Control | Keys.O))
            {
                cbCompany.Focus();
                cbCompany.DroppedDown = true;
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                cmbSeries.Focus();
                cmbSeries.DroppedDown = true;
            }
            else if (keyData == (Keys.Control | Keys.D1))
            {
                dtPostingDate.Focus();

                if (dtPostingDate.Text != " ")
                {
                    dtPostingDate.CustomFormat = " ";
                    dtPostingDate.Format = DateTimePickerFormat.Custom;
                }

                Thread.Sleep(300);

                dtPostingDate.Format = DateTimePickerFormat.Short;
                dtPostingDate.Select();
                SendKeys.Send("%{DOWN}");
                return true;
            }
            else if (keyData == (Keys.Control | Keys.D2))
            {
                dtDocDate.Focus();

                if (dtDocDate.Text != " ")
                {
                    dtDocDate.CustomFormat = " ";
                    dtDocDate.Format = DateTimePickerFormat.Custom;
                }

                Thread.Sleep(300);

                dtDocDate.Format = DateTimePickerFormat.Short;
                dtDocDate.Select();
                SendKeys.Send("%{DOWN}");
                return true;
            }
            else if (keyData == (Keys.Control | Keys.E))
            {
                //SalesEmployee();
                EventHelper.RaisedEvent(null, InventoryTransferSalesEmployeeClick, null);

            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                txtRemarks.Focus();
            }
            else if (keyData == (Keys.Control | Keys.I))
            {
                EventHelper.RaisedEvent(null, InventoryTransferAddItemClick, null);
                //AddItem();
            }
            else if (keyData == (Keys.Control | Keys.N))
            {
                EventHelper.RaisedEvent(null, InventoryTransferNewDocument, null);
                //AddNew();
            }
            else if (keyData == (Keys.Control | Keys.A))
            {
                EventHelper.RaisedEvent(null, InventoryTransferPostClick, null);
                //AddtoSAP();
            }
            else if (keyData == (Keys.Control | Keys.F))
            {
                TabIT.SelectedIndex = 2;
                //Find();
            }
            else if (keyData == (Keys.Control | Keys.P))
            {
                if (txtDocEntry1.Text == string.Empty)
                {
                    TabIT.SelectedIndex = 0;
                    StaticHelper._MainForm.ShowMessage("Warning: Please select document first", true);
                }
                else
                {
                    TabIT.SelectedIndex = 3;
                    TxtPrintDocNo.Text = txtDocEntry1.Text;
                }
            }
            //else if (keyData == (Keys.Control | Keys.C))
            //{
            //    CmbCopyFromOption.DroppedDown = true;
            //}
            else if (keyData == (Keys.Control | Keys.B))
            {
                EventHelper.RaisedEvent(null, InventoryTransferBpClick, null);
                //BPList();
            }
            else if (keyData == (Keys.Control | Keys.H))
            {
                txtAddress.Focus();
            }
            else if (keyData == (Keys.Control | Keys.Y))
            {
                cbTransferType.Focus();
                cbTransferType.DroppedDown = true;
            }
            //else if (keyData == (Keys.Control | Keys.V))
            //{
            //    TabIT.SelectedIndex = 1;
            //    //Preview();
            //}
            else if (keyData == (Keys.Control | Keys.W))
            {
                EventHelper.RaisedEvent(null, InventoryTransferFrmWhsClick, null);
                //FromWarehouse();
            }
            else if (keyData == (Keys.Control | Keys.T))
            {
                EventHelper.RaisedEvent(null, InventoryTransferToWhsClick, null);
                //ToWarehouse();
            }
            else if (keyData == (Keys.Control | Keys.D0))
            {
                dgvItem.Focus();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}

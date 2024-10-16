using DirecLayer;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;
using System.Threading;
using MetroFramework;
using DirecLayer._02_Form.MVP.Presenters.PriceTag;
using static PresenterLayer.Services.CartonController;
using PresenterLayer.Helper.Unofficial_Sales;

namespace PresenterLayer.Views.Inventory.Inventory_Transfer_Request
{
    public partial class FrmInventoryTransferRequest : MetroForm, IFrmInventoryTransferRequest
    {
        //InventoryTransferRequestService _inventoryTransferService = new InventoryTransferRequestService();
        //IInventoryTransferRequest _inventoryTransferRequest = new IInventoryTransferRequest();
        public event EventHandler InventoryRequestLoad;
        public event DataGridViewCellEventHandler InventoryRequestUdfLoad;
        public event EventHandler InventoryRequestBPLoad;
        public event EventHandler InventoryRequestPreview;
        public event EventHandler InventoryRequestToWhsLoad;
        public event EventHandler InventoryRequestFromWhsLoad;
        public event EventHandler InventoryRequestSalesEmployeeLoad;
        public event EventHandler InventoryRequestAddItem;
        public event EventHandler InventoryRequestPostITR;
        public event EventHandler InventoryRequestMenuStripDelete;
        public event DataGridViewCellEventHandler InventoryRequestCellClick;
        public event DataGridViewCellEventHandler InventoryRequestCellEndEdit;
        public event DataGridViewCellMouseEventHandler InventoryRequestRowHeaderClick;
        public event EventHandler InventoryRequestSeriesChange;
        public event EventHandler InventoryRequestTransferTypeChange;
        public event EventHandler InventoryRequestSearchTextChange;
        public event EventHandler InventoryRequestFindDocument;
        public event EventHandler InventoryRequestChooseDocument;
        public event EventHandler InventoryRequestFrmWhsTextChange;
        public event EventHandler InventoryRequestToWhsTextChange;
        public event EventHandler InventoryRequestPrintDocumentChange;
        public event EventHandler InventoryRequestNewDocument;
        public event EventHandler InventoryRequestCloseForm;
        public event EventHandler InventoryRequestFindDocumentTextChange;
        public event FormClosingEventHandler InventoryRequestFormClose;
        public event PreviewKeyDownEventHandler InventoryCopy;

        private bool withdata = false;

        public static string oSeriesName;
        public static string oPriceList, oBPGroup;
        public static string oTaxGroup;
        public static string oTaxRate;
        public static string oFWhsCode, oTWhsCode;
        public static string objType = "OWTQ";//OBJTYPE
        private string TotalBefDIsc, TotalQty, TotalDisc, TotalTax, TotalAftDisc;
        private string oCode, oName;
        public static int oColumnIndex;
        int max_width = Screen.PrimaryScreen.Bounds.Width - 220;
        int max_height = Screen.PrimaryScreen.Bounds.Height - 200;


        public static bool FindMode = false;
        public static string _DocEntry;

        public Panel Panel2
        {
            get => panel2;
            set => panel2 = value;
        }
        public DataGridView FindDocumentTable
        {
            get => DgvFindDocument;
            set => DgvFindDocument = value;
        }
        public string DocNum
        {
            get => TxtDocNum.Text;
            set => TxtDocNum.Text = value;
        }
        public string DocStatus
        {
            get => txtDocStatus.Text;
            set => txtDocStatus.Text = value;
        }
        public ContextMenuStrip MsItems
        {
            get => msItems;
            set => msItems = value;
        }
        public TextBox DocEntry1
        {
            get => txtDocEntry1;
            set => txtDocEntry1 = value;
        }
        public string oSalesEmployee { get; set; }
        public string oProject { get; set; }
        public string oSeries
        {
            get => cmbSeries.SelectedValue.ToString();
            set => cmbSeries.SelectedValue.ToString();
        }
        public DataGridView UDF => DgvUdf;
        public DataGridView table
        {
            get => DgvItem;
            set => DgvItem = value;
        }
        public DataGridView ItemPreview
        {
            get => DgvPreviewItem;
            set => DgvPreviewItem = value;
        }
        public string BpCode
        {
            get => txtBpCode.Text;
            set => txtBpCode.Text = value;
        }
        public string BpAddress
        {
            get => txtAddress.Text;
            set => txtAddress.Text = value;
        }
        public string BpName
        {
            get => txtBpName.Text;
            set => txtBpName.Text = value;
        }
        public string FrmWhsCode
        {
            get => txtFWhsCode.Text;
            set => txtFWhsCode.Text = value;
        }
        public string ToWhsCode
        {
            get => txtTWhsCode.Text;
            set => txtTWhsCode.Text = value;
        }
        public string SalesEmployee
        {
            get => txtSalesEmployee.Text;
            set => txtSalesEmployee.Text = value;
        }
        public string Address
        {
            get => txtAddress.Text;
            set => txtAddress.Text = value;
        }
        public string Total
        {
            get => txtTotal.Text;
            set => txtTotal.Text = value;
        }
        public string TotalQuantity
        {
            get => txtTotalQty.Text;
            set => txtTotalQty.Text = value;
        }
        public string TransferType
        {
            get => cbTransferType.Text;
            set => cbTransferType.Text = value;
        }
        public string Company
        {
            get => Convert.ToString(cbCompany.SelectedValue);
            set => cbCompany.Text = value;
        }
        public bool BtnRequestEnabled
        {
            get => BtnRequest.Enabled;
            set => BtnRequest.Enabled = value;
        }
        public string BtnRequestText
        {
            get => BtnRequest.Text;
            set => BtnRequest.Text = value;
        }

        public Button buttonBtnRequest
        {
            get => BtnRequest;
            set => BtnRequest = value;
        }

        public PictureBox buttonBPList
        {
            get => pbBPList;
            set => pbBPList = value;
        }

        public PictureBox buttonFrmWhs
        {
            get => pbFromWhsList;
            set => pbFromWhsList = value;
        }
        public PictureBox buttonToWhs
        {
            get => pbToWhsList;
            set => pbToWhsList = value;
        }
        public PictureBox buttonSalesEmployee
        {
            get => pictureBox1;
            set => pictureBox1 = value;
        }

        public ComboBox comboboxTransferType
        {
            get => cbTransferType;
            set => cbTransferType = value;
        }
        public ComboBox comboboxCompany
        {
            get => cbCompany;
            set => cbCompany = value;
        }
        public DateTimePicker datePickerPostingDate
        {
            get => dtPostingDate;
            set => dtPostingDate = value;
        }
        public DateTimePicker datePickerDueDate
        {
            get => dtDueDate;
            set => dtDueDate = value;
        }
        public DateTimePicker datePickerDocDate
        {
            get => dtDocDate;
            set => dtDocDate = value;
        }
        public string sRemarks
        {
            get => txtRemarks.Text;
            set => txtRemarks.Text = value;
        }
        public string sAddress
        {
            get => txtAddress.Text;
            set => txtAddress.Text = value;
        }

        public ComboBox comboboxSeries
        {
            get => cmbSeries;
            set => cmbSeries = value;
        }

        public string ItemPreviewSearch
        {
            get => TxtSearch.Text;
            set => TxtSearch.Text = value;
        }

        public TabControl ITRTab
        {
            get => TabITR;
            set => TabITR = value;
        }
        public string PrintDocNo
        {
            get => CmbPrintPreview.Text;
            set => CmbPrintPreview.Text = value;
        }

        public CrystalReportViewer crystalReportViewer
        {
            get => crystalReportViewer1;
            set => crystalReportViewer1 = value;
        }

        public string FindPageLimit
        {
            get => txtPageSizeLimit.Text;
            set => txtPageSizeLimit.Text = value;
        }

        public TextBox txtSearchDocument { get => TxtSearchDocument; set => TxtSearchDocument = value; }
        public string OCode { get => oCode; set => oCode = value; }
        public string OName { get => oName; set => oName = value; }

        public FrmInventoryTransferRequest()
        {
            InitializeComponent();
        }
        private void TabITR_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TabITR.SelectedIndex)
            {
                case 0:
                    //Presenter.LoadData(DgvItem);

                    //EventHelper.RaisedEvent(this, InventoryRequestLoad, e);
                    //if (/*CmbServiceType.Text == "Service" && */BtnRequest.Text == "Update")
                    //{
                    //    //DgvItem.Rows.RemoveAt(0);
                    //}
                    break;

                case 1:

                    //Presenter.LoadData(DgvPreviewItem);
                    EventHelper.RaisedEvent(this, InventoryRequestPreview, e);
                    //if (/*CmbServiceType.Text == "Service" &&*/ BtnRequest.Text == "Update")
                    //{
                    //    //DgvPreviewItem.Rows.RemoveAt(0);
                    //}
                    break;

                case 2:
                    //Presenter.GetExistingDocument(DgvFindDocument);
                    //CurrentDataSet = Presenter.ListDocuments();
                    EventHelper.RaisedEvent(this, InventoryRequestFindDocument, e);
                    CmbFilterDocument.Text = "All";
                    break;

                case 3:
                    //PublicStatic.frmMain.NotiMsg("Warning: Please select document first", Color.Red);
                    if (txtDocEntry1.Text == string.Empty)
                    {
                        TabITR.SelectedIndex = 0;
                        StaticHelper._MainForm.ShowMessage("Warning: Please select document first", true);
                    }
                    else
                    {
                        TabITR.SelectedIndex = 3;
                        TxtPrintDocNo.Text = txtDocEntry1.Text;
                    }
                    break;

                default:
                    break;
            }
        }
        public void showSeries(DataTable resultOfQuery)
        {
            cmbSeries.DataSource = resultOfQuery;
        }
        public void showTransferType(DataTable resultOfQUery)
        {
            cbTransferType.DataSource = resultOfQUery;
        }
        public void showCompany(DataTable resultOfQuery)
        {
            cbCompany.DisplayMember = "Name";
            cbCompany.ValueMember = "Code";
            cbCompany.DataSource = resultOfQuery;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryRequestSalesEmployeeLoad, e);
        }

        private void BtnItem_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryRequestAddItem, e);
        }

        private void BtnRequest_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryRequestPostITR, e);
        }

        private void DgvItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedCellEvent(this, InventoryRequestCellClick, e);
        }

        private void DgvUdf_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DgvUdf.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                DgvUdf.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void FrmInventoryTransferRequest_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void DgvItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedCellEvent(sender, InventoryRequestCellEndEdit, e);
        }

        private void DgvItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EventHelper.RaisedCellMouseEvent(sender, InventoryRequestRowHeaderClick, e);
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (DgvItem.Rows.Count > 0)
                {
                    var mousePosition = panel2.PointToClient(Cursor.Position);
                    //msDuplicate.Show(panel2, mousePosition);
                }
            }
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryRequestMenuStripDelete, e);
        }

        private void cmbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryRequestSeriesChange, e);
        }

        private void cbTransferType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryRequestTransferTypeChange, e);
        }

        private void DgvPreviewItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedCellEvent(this, InventoryRequestCellClick, e);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryRequestSearchTextChange, e);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                EventHelper.RaisedEvent(sender, InventoryRequestCloseForm, e);
                Dispose();
            }

        }
        private void txtFWhsCode_TextChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryRequestFrmWhsTextChange, e);
        }

        private void txtTWhsCode_TextChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryRequestToWhsTextChange, e);
        }

        private void BtnChoose_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryRequestChooseDocument, e);
        }

        private void DgvFindDocument_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryRequestChooseDocument, e);
        }

        private void CmbPrintPreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryRequestPrintDocumentChange, e);
        }

        private void BtnNewDocument_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryRequestNewDocument, e);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (txtDocEntry1.Text == string.Empty)
            {
                ITRTab.SelectedIndex = 0;
                //PublicStatic.frmMain.NotiMsg("Warning: Please select document first", Color.Red);
                StaticHelper._MainForm.ShowMessage("Warning: Please select document first", true);

            }
            else
            {
                TabITR.SelectedIndex = 3;
                TxtPrintDocNo.Text = txtDocEntry1.Text;
            }
        }

        private void DgvFindDocument_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvFindDocument.CurrentRow.Selected = true;
        }

        private void TxtSearchDocument_TextChanged(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(sender, InventoryRequestFindDocumentTextChange, e);
        }

        private void DgvFindDocument_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            oColumnIndex = e.ColumnIndex;
        }

        private void FrmInventoryTransferRequest_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHelper.RaiseFormCloseEvent(sender, InventoryRequestFormClose, e);
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var x = Convert.ToString(cbCompany.SelectedValue);
            //Company = x;
        }

        private void DgvItem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            EventHelper.RaisedPreviewKeyDown(sender, InventoryCopy, e);
        
        }

        private void btnRefreshFindPage_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryRequestFindDocument, e);
            CmbFilterDocument.Text = "All";
        }

        private void txtPageSizeLimit_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBarcode_Click(object sender, EventArgs e)
        {

        }

        //private void btnBarcode_Click(object sender, EventArgs e)
        //{
        //    ITRHelper.ITR_ImageFile(DgvItem, txtBpCode.Text, dtDueDate.Text,txtDocEntry1.Text);
        //}



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
        private void FrmInventoryTransferRequest_Load(object sender, EventArgs e)
        {

            WindowState = FormWindowState.Maximized;
            cmbSeries.DisplayMember = "Name";
            cmbSeries.ValueMember = "Code";
            cbTransferType.DisplayMember = "Name";
            cbTransferType.ValueMember = "Code";
            cbCompany.DisplayMember = "Name";
            cbCompany.ValueMember = "Code";

            EventHelper.RaisedEvent(this, InventoryRequestLoad, e);
            DgvFindDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DgvUdf.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.Resizable = DataGridViewTriState.False);
            txtDocEntry1.Focus();
        }
        private void DgvUdf_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            EventHelper.RaisedCellEvent(this, InventoryRequestUdfLoad, e);
        }
        private void pbFromWhsList_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryRequestFromWhsLoad, e);
        }
        private void pbToWhsList_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryRequestToWhsLoad, e);
        }
        private void pbBPList_Click(object sender, EventArgs e)
        {
            EventHelper.RaisedEvent(this, InventoryRequestBPLoad, e);
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            //if (keyData == (Keys.Control | Keys.Shift | Keys.U))
            //{ ShowUDF(); }
            /*else*/
            switch (keyData)
            {
                case Keys.Control | Keys.D1:
                    TabITR.SelectedIndex = 0;
                    break;

                case Keys.Control | Keys.D2:
                    TabITR.SelectedIndex = 1;
                    break;

                case Keys.Control | Keys.D3:
                    TabITR.SelectedIndex = 2;
                    break;

                case Keys.Control | Keys.D4:
                    //TabITR.SelectedIndex = 3;
                    if (txtDocEntry1.Text == string.Empty)
                    {
                        //PublicStatic.frmMain.NotiMsg("Warning: Please select document first", Color.Red);
                        StaticHelper._MainForm.ShowMessage("Warning: Please select document first", true);

                    }
                    else
                    {
                        TabITR.SelectedIndex = 3;
                        TxtPrintDocNo.Text = txtDocEntry1.Text;
                    }
                    break;

                case Keys.Control | Keys.D5:
                    TabITR.SelectedIndex = 4;
                    break;
            }


            if (keyData == Keys.Tab && txtBpCode.Focused && txtBpCode.Text == "")
            {
                pbBPList_Click(null, null);
                return true;
            }
            else if (keyData == Keys.Tab && txtFWhsCode.Focused && txtFWhsCode.Text == "")
            {
                pbFromWhsList_Click(null, null);
                return true;
            }
            else if (keyData == Keys.Tab && txtTWhsCode.Focused && txtTWhsCode.Text == "")
            {
                pbToWhsList_Click(null, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Escape))
            {
                //this.Close();
                //Cancel();
                BtnClose_Click(null, null);
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
            else if (keyData == (Keys.Control | Keys.D3))
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
            else if (keyData == (Keys.Control | Keys.D2))
            {
                dtDueDate.Focus();

                if (dtDueDate.Text != " ")
                {
                    dtDueDate.CustomFormat = " ";
                    dtDueDate.Format = DateTimePickerFormat.Custom;
                }

                Thread.Sleep(300);

                dtDueDate.Format = DateTimePickerFormat.Short;
                dtDueDate.Select();
                SendKeys.Send("%{DOWN}");
                return true;
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                txtRemarks.Focus();
            }
            else if (keyData == (Keys.Control | Keys.I))
            {
                //AddItem();
                BtnItem_Click(null, null);
            }
            else if (keyData == (Keys.Control | Keys.N))
            {
                //New();
                BtnNewDocument_Click(null, null);
            }
            else if (keyData == (Keys.Control | Keys.A))
            {
                //AddtoSAP();
                BtnRequest_Click(null, null);
            }
            else if (keyData == (Keys.Control | Keys.F))
            {
                // Find();
                TabITR.SelectedIndex = 2;
            }
            else if (keyData == (Keys.Control | Keys.P))
            {
                // PrintReport();
                //TabITR.SelectedIndex = 3;
                if (txtDocEntry1.Text == string.Empty)
                {
                    //PublicStatic.frmMain.NotiMsg("Warning: Please select document first", Color.Red);
                    StaticHelper._MainForm.ShowMessage("Warning: Please select document first", true);

                }
                else
                {
                    TabITR.SelectedIndex = 3;
                    TxtPrintDocNo.Text = txtDocEntry1.Text;
                }
            }
            else if (keyData == (Keys.Control | Keys.B))
            {
                //BPList();
                pbBPList_Click(null, null);
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
            //    //Preview();
            //    TabITR.SelectedIndex = 1;
            //}
            else if (keyData == (Keys.Control | Keys.W))
            {
                //FromWarehouse();
                pbFromWhsList_Click(null, null);
            }
            else if (keyData == (Keys.Control | Keys.T))
            {
                //ToWarehouse();
                pbToWhsList_Click(null, null);
            }
            else if (keyData == (Keys.Control | Keys.D0))
            {
                DgvItem.Focus();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}

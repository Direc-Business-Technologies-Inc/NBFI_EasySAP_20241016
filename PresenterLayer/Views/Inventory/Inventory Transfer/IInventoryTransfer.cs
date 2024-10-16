using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;

namespace PresenterLayer.Views.Inventory.Inventory_Transfer
{
    public interface IInventoryTransfer
    {
        event EventHandler InventoryTransferLoad;
        event EventHandler InventoryTransferBpClick;
        event EventHandler InventoryTransferToWhsClick;
        event EventHandler InventoryTransferFrmWhsClick;
        event EventHandler InventoryTransferSalesEmployeeClick;
        event EventHandler InventoryTransferTransferTypeChange;
        event EventHandler InventoryTransferCompanyChange;
        event EventHandler InventoryTransferSeriesChange;
        event EventHandler InventoryTransferFromWhsChange;
        event EventHandler InventoryTransferToWhsChange;
        event EventHandler InventoryTransferFindDocument;
        event EventHandler InventoryTransferPostClick;
        event EventHandler InventoryTransferAddItemClick;
        event EventHandler InventoryTransferCopyFromChange;
        event EventHandler InventoryTransferDeleteRowClick;
        event EventHandler InventoryTransferChooseDocument;
        event EventHandler InventoryTransferCloseForm;
        event EventHandler InventoryTransferNewDocument;
        event EventHandler InventoryTransferSearchTextChange;
        event EventHandler InventoryTransferSearchDocumentTextChange;
        event EventHandler InventoryTransferPrintPreviewFromChange;
        event DataGridViewCellEventHandler InventoryTransferCellClick;
        event DataGridViewCellEventHandler InventoryTransferItemCellClick;
        event DataGridViewCellEventHandler InventoryTransferChainCellClick;
        event DataGridViewCellEventHandler InventoryTransferItemCellEndEdit;
        event FormClosingEventHandler InventoryTransferCloseFormEvent;
        event PreviewKeyDownEventHandler InventoryCopy;
        event ScrollEventHandler InventoryUDFscroll;
        void showSeries(DataTable resultOfQuery);
        void showTransferType(DataTable resultOfQUery);
        void showCompany(DataTable resultOfQuery);
        Button btnChoose { get; set; }
        void PrintPreviewItems(FileInfo[] files);
        Button btnClose { get; set; }
        Button btnCopyFrom { get; set; }
        Button btnItem { get; set; }
        Button btnNewDocument { get; set; }
        Button btnPrint { get; set; }
        Button btnRequest { get; set; }
        ComboBox CbTransferType { get; set; }
        ComboBox cmbCopyFromOption { get; set; }
        ComboBox cmbFilterDocument { get; set; }
        ComboBox CmbSeries { get; set; }
        CrystalReportViewer CrystalReportViewer1 { get; set; }
        ToolStripMenuItem DeleteItemsToolStripMenuItem { get; set; }
        DataGridView dgvFindDocument { get; set; }
        DataGridView dgvItem { get; set; }
        DataGridView dgvPreviewItem { get; set; }
        DateTimePicker DtDocDate { get; set; }
        DateTimePicker DtPostingDate { get; set; }
        ContextMenuStrip MsItems { get; set; }
        PictureBox PbBPList { get; set; }
        PictureBox PbFromWhsList { get; set; }
        PictureBox PbToWhsList { get; set; }
        TabControl tabIT { get; set; }
        TextBox TxtAddress { get; set; }
        TextBox TxtBpCode { get; set; }
        TextBox TxtBpName { get; set; }
        TextBox txtDocNum { get; set; }
        TextBox TxtDocStatus { get; set; }
        TextBox TxtFWhsCode { get; set; }
        TextBox TxtITR_DocEntry { get; set; }
        TextBox TxtITR_DocNum { get; set; }
        TextBox txtPrintDocNo { get; set; }
        TextBox TxtRemarks { get; set; }
        TextBox TxtSalesEmployee { get; set; }
        TextBox txtSearch { get; set; }
        TextBox txtSearchDocument { get; set; }
        TextBox TxtFromDoc { get; set; }
        TextBox TxtTotal { get; set; }
        TextBox TxtTotalQty { get; set; }
        TextBox TxtTWhsCode { get; set; }
        TextBox TxtDocentry { get; set; }
        DataGridView UDF { get; set; }
        ComboBox vmbPrintPreview { get; set; }
        ComboBox CbCompany { get; set; }
        string OSalesEmployee { get; set; }
        string OSeries { get; set; }
        string OProject { get; set; }
        string series { get; set; }
        string OCode { get; set; }
        string OName { get; set; }
        Panel Panel2 { get; set; }
        string FindPageLimit { get; set; }
       

    }
}
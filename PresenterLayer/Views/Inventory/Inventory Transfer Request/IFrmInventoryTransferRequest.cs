using CrystalDecisions.Windows.Forms;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace PresenterLayer.Views.Inventory.Inventory_Transfer_Request
{
    public interface IFrmInventoryTransferRequest
    {
        event EventHandler InventoryRequestLoad;
        event EventHandler InventoryRequestPreview;
        event DataGridViewCellEventHandler InventoryRequestUdfLoad;
        event EventHandler InventoryRequestBPLoad;
        event EventHandler InventoryRequestToWhsLoad;
        event EventHandler InventoryRequestFromWhsLoad;
        event EventHandler InventoryRequestSalesEmployeeLoad;
        event EventHandler InventoryRequestAddItem;
        event EventHandler InventoryRequestPostITR;
        event EventHandler InventoryRequestFindDocumentTextChange;
        event DataGridViewCellEventHandler InventoryRequestCellClick;
        void showSeries(DataTable resultOfQuery);
        void showTransferType(DataTable resultOfQUery);
        void showCompany(DataTable resultOfQuery);
        void PrintPreviewItems(FileInfo[] files);
        DataGridView UDF { get; }
        DataGridView table { get; set; }
        string oSalesEmployee { get; set; }
        string oProject { get; set; }
        string oSeries { get; set; }
        string BpCode { get; set; }
        string BpAddress { get; set; }
        string BpName { get; set; }
        string FrmWhsCode { get; set; }
        string ToWhsCode { get; set; }
        string SalesEmployee { get; set; }
        string Address { get; set; }
        string Total { get; set; }
        string TotalQuantity { get; set; }
        string TransferType { get; set; }
        string Company { get; set; }
        bool BtnRequestEnabled { get; set; }
        string BtnRequestText { get; set; }
        ComboBox comboboxTransferType { get; set; }
        ComboBox comboboxCompany { get; set; }
        DateTimePicker datePickerPostingDate { get; set; }
        DateTimePicker datePickerDueDate { get; set; }
        DateTimePicker datePickerDocDate { get; set; }
        string sRemarks { get; set; }
        string sAddress { get; set; }
        string OCode { get; set; }
        string OName { get; set; }
        string FindPageLimit { get; set; }
        TextBox txtSearchDocument { get; set; }
        TextBox DocEntry1 { get; set; }
        event DataGridViewCellEventHandler InventoryRequestCellEndEdit;
        event DataGridViewCellMouseEventHandler InventoryRequestRowHeaderClick;
        ContextMenuStrip MsItems { get; set; }
        event EventHandler InventoryRequestMenuStripDelete;
        string DocNum { get; set; }
        event EventHandler InventoryRequestSeriesChange;
        string DocStatus { get; set; }
        ComboBox comboboxSeries { get; set; }
        event EventHandler InventoryRequestTransferTypeChange;
        DataGridView ItemPreview { get; set; }
        event EventHandler InventoryRequestSearchTextChange;
        string ItemPreviewSearch { get; set; }
        Panel Panel2 { get; set; }
        DataGridView FindDocumentTable { get; set; }
        event EventHandler InventoryRequestFindDocument;
        event EventHandler InventoryRequestChooseDocument;
        TabControl ITRTab { get; set; }
        Button buttonBtnRequest { get; set; }
        PictureBox buttonBPList { get; set; }
        PictureBox buttonSalesEmployee { get; set; }
        PictureBox buttonToWhs { get; set; }
        PictureBox buttonFrmWhs { get; set; }

        event EventHandler InventoryRequestFrmWhsTextChange;
        event EventHandler InventoryRequestToWhsTextChange;
        event EventHandler InventoryRequestPrintDocumentChange;
        string PrintDocNo { get; set; }
        CrystalReportViewer crystalReportViewer { get; set; }
        event EventHandler InventoryRequestNewDocument;
        event EventHandler InventoryRequestCloseForm;
        event FormClosingEventHandler InventoryRequestFormClose;
        event PreviewKeyDownEventHandler InventoryCopy;
    }
}
using System.Windows.Forms;
using DirecLayer._02_Form.MVP.Presenters;
using PresenterLayer.Views.Main;
using PresenterLayer.Services;

namespace PresenterLayer.Views
{
    public interface IFrmSalesOrder
    {
        string BpCurrency { get; set; }
        string BpRate { get; set; }
        string CancellationDate { get; set; }
        string Company { get; set; }
        string BPCode { get; set; }
        string ContactPerson { get; set; }
        string DeliveryDate { get; set; }
        string Department { get; set; }
        string DiscountAmount { get; set; }
        string DiscountInput { get; set; }
        string DocEntry { get; set; }
        string DocNum { get; set; }
        string DocumentDate { get; set; }
        MainForm frmMain { get; set; }
        bool IsFindMode { get; set; }
        ContextMenuStrip MsItems { get; }
        string PostingDate { get; set; }
        SalesOrderService Presenter { set; }
        string RawCurrency { get; set; }
        string Remark { get; set; }
        string Series { get; set; }
        string Service { get; set; }
        string Status { get; set; }
        string SuppCode { get; set; }
        string SuppName { get; set; }
        DataGridView Table { get; }
        DataGridView TablePreview { get; }
        string Tax { get; set; }
        string Total { get; set; }
        string TotalBeforeDiscount { get; set; }
        DataGridView Udf { get; }
        Panel UdfPanel { get; }
        string VatGroup { get; set; }
        double VatGroupRate { get; set; }
        string Warehouse { get; set; }
        string oSalesEmployee { get; set; }
        string oSalesEmployeeName { get; set; }
        string oShipTo { get; set; }
        string oLogShipTo { get; set; }
    }
}
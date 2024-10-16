using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirecLayer._02_Form.MVP.Views
{
    public interface IPurchaseOrder
    {
        string DocEntry { get; set; }
        string DocNum { get; set; }
        string Series { get; set; }
        string SuppCode { get; set; }
        string SuppName { get; set; }
        string ContactPerson { get; set; }
        string Company { get; set; }
        string Department { get; set; }
        string BpCurrency { get; set; }
        string BpRate { get; set; }
        string Status { get; set; }
        string PostingDate { get; set; }
        string DocumentDate { get; set; }
        string DeliveryDate { get; set; }
        string CancellationDate { get; set; }
        DataGridView Table { get; }
        DataGridView Udf { get; }
        string Remark { get; set; }
        Presenters.PurchaseOrderPresenter Presenter {set; }
        string VatGroup { get; set; }
        string Warehouse { get; set; }
        string Service { get; set; }
        string DiscountInput { get; set; }
        string TotalBeforeDiscount { get; set; }
        string Tax { get; set; }
        string DiscountAmount { get; set; }
        string Total { get; set; }
        DataGridView TablePreview { get; }
        ContextMenuStrip MsItems { get; }
        double VatGroupRate { get; set; }
        bool IsFindMode { get; set; }
        string RawCurrency { get; set; }

        Panel UdfPanel { get; }
    }
}

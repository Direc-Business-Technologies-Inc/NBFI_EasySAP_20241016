using PresenterLayer.Services;
using System.Data;
using System.Windows.Forms;

namespace PresenterLayer.Views
{
    public interface ICartonManagement
    {
        string DocEntry { get; set; }
        DateTimePicker DTPDateChecked { get; set; }
        string DocNo { get; set; }

        string CartonNo { get; set; }

        string SuppCode { get; set; }

        string SuppName { get; set; }

        string Chain { get; set; }

        string GroupCode { get; set; }

        string DocRef { get; set; }

        string OrderNo { get; set; }

        string ShipmentNo { get; set; }

        string TransType { get; set; }

        string TransferType { get; set; }

        string TargetWhse { get; set; }

        string LastWhse { get; set; }

        string Status { get; set; }

        string DateChecked { get; set; }

        string Remarks { get; set; }

        string BasedDocEntry { get; set; }

        string TotalQuantity { get; set; }
        DataGridView Table { get; }
        
        FrmCartonManagement Form { get; }
        
        ContextMenuStrip MouseSelect { get; }

        DataTable TableFindDocument { set; }

        CartonManagementPresenter Presenter { set; }
        string BasedDocument { get; set; }
    }
}

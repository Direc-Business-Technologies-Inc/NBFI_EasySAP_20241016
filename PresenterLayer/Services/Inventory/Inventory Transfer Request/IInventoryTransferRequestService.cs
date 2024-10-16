using PresenterLayer.Views.Inventory.Inventory_Transfer_Request;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace PresenterLayer.Services.Inventory
{
    public interface IInventoryTransferRequestService
    {
        IFrmInventoryTransferRequest GetFrmITR();
        FileInfo[] GetDocumentCrystalForms();
        void LoadData(DataGridView dgv, bool isFirstLoad = false);
    }
}
using System.Data;

namespace PresenterLayer.Helper
{
    public interface IPreviewItemRepository
    {
        DataTable ItemList();
    }
}

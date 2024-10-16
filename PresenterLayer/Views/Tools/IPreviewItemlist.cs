using PresenterLayer.Helper;
using System;
using System.Data;
using System.Windows.Forms;

namespace PresenterLayer.Views
{
    public interface IPreviewItemlist
    {
        Tuple<DataTable, DataGridView> ItemSource { get; set; }

        PreviewItemPresenter Presenter { set; }
    }
}

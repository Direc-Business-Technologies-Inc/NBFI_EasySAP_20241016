using PresenterLayer.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresenterLayer.Views
{
    public interface ICartonList
    {
        
        DataGridView Table { get; }

        string DocEntry { get; set; }

        string Remark { get; set; }
        
        DataTable TableSearch { set; }
        Button Request { get; set; }

        CartonListPresenter Presenter { set; }
    }
}

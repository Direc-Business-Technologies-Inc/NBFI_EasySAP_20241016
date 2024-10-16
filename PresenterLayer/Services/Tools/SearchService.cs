using InfrastructureLayer.InventoryRepository;
using PresenterLayer.Views.Inventory.Inventory_Transfer_Request;
using PresenterLayer.Views.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterLayer.Services.Tools
{
    public class SearchService : ISearchService
    {
        //IFrmInventoryTransferRequest _frmITR;
        IQueryRepository _queryRepository;
        IFrmSearch _frmSearch;
        public SearchService(IFrmSearch frmSearch, IQueryRepository queryRepository)
        {
            _frmSearch = frmSearch;
            _queryRepository = queryRepository;
            EventsSubscription();
        }
        private void EventsSubscription()
        {
            _frmSearch.SearchLoad += new EventHandler(LoadData);
            //_frmSearch.InventoryRequestUdfLoad += new EventHandler(LoadFields);
        }
        public IFrmSearch GetFrmSearch() { return _frmSearch; }

        public void LoadData(object sender, EventArgs e)
        {

        }
    }
}

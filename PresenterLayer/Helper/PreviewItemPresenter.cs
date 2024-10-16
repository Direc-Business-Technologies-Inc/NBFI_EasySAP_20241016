using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using PresenterLayer.Views;

namespace PresenterLayer.Helper
{
    public class PreviewItemPresenter
    {
        private readonly IPreviewItemlist _view;
        private readonly IPreviewItemRepository _repository;

        public IEnumerable<DataGridViewRow> returnItems { get; set; }

        public PreviewItemPresenter(IPreviewItemlist v, IPreviewItemRepository r)
        {
            _view = v;
            _view.Presenter = this;
            _repository = r;

            LoadItems();
        }

        public void LoadItems()
        {
            _view.ItemSource = Tuple.Create(_repository.ItemList(), new DataGridView());
        }

        public void ReturnItems()
        {
            var dgv = _view.ItemSource.Item2;
            var data = dgv.Rows.Cast<DataGridViewRow>().ToList().OrderBy(x => x.Index);

            returnItems = data.Take(data.Count() - 1);
        }
    }
}

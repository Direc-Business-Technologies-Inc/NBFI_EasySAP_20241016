using DirecLayer._02_Form.MVP.Views;
using PresenterLayer.Helper;
using PresenterLayer.Views;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PresenterLayer.Services
{
    public class CartonListPresenter
    {
        private readonly ICartonList _view;
        private readonly ICartonListModel _repository;

        public CartonListPresenter(ICartonList view, ICartonListModel repo)
        {
            _view = view;
            _view.Presenter = this;
            _repository = repo;

            Onload();
        }

        private void Onload()
        {
            if (_view.Table.RowCount <= 0)
            {
                DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
                col1.Name = "Document Entry";
                _view.Table.Columns.Add(col1);
                _view.Table.Columns["Document Entry"].ReadOnly = true;

                DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
                col2.Name = "Carton No.";
                _view.Table.Columns.Add(col2);
                _view.Table.Columns["Carton No."].ReadOnly = true;

                DataGridViewColumn col0 = new DataGridViewTextBoxColumn();
                col0.Name = "Supplier Name";
                _view.Table.Columns.Add(col0);
                _view.Table.Columns["Supplier Name"].ReadOnly = true;

                DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
                col4.Name = "Reference 2";
                _view.Table.Columns.Add(col4);
                _view.Table.Columns["Reference 2"].ReadOnly = true;

                DataGridViewColumn col5 = new DataGridViewTextBoxColumn();
                col5.Name = "Remarks";
                _view.Table.Columns.Add(col5);
                _view.Table.Columns["Remarks"].ReadOnly = true;

                _view.Table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        public void GetCartonInformation()
        {
            string docentry = ConvertToString(_view.Table.CurrentRow.Cells[0].Value);

            var model = new CartonManagementModel();
            var view = new FrmCartonManagement(docentry);
            var presenter = new CartonManagementPresenter(view, model);

            view.ShowDialog();
        }
        
        public void GetExistingCartonList()
        {
            _view.TableSearch = null;
            _view.TableSearch = _repository.DisplayExistingCartonList();
        }

        public void GetExistingCartonList(string docEntry)
        {
            DataTable header = _repository.SelectCartonListHeader(docEntry);
            
            if (header.Rows.Count > 0)
            {
                ClearForm();

                _view.DocEntry = ConvertToString(header.Rows[0]["DocEntry"]);
                _view.Remark = ConvertToString(header.Rows[0]["Remark"]);

                DataTable DocumentLines = _repository.SelectCartonListRows(docEntry);
                
                foreach (DataRow row in DocumentLines.Rows)
                {
                    object[] a =
                    {
                        ConvertToString(row["U_DocEntry"]),
                        ConvertToString(row["U_CartonNo"]),
                        ConvertToString(row["U_DocRef"]),
                        ConvertToString(row["U_Ref2"]),
                        ConvertToString(row["U_Remarks"]),
                    };

                    _view.Table.Rows.Add(a);
                }
            }
        }

        internal void ClearField(bool v)
        {
            _view.Remark = string.Empty;
            _view.DocEntry = string.Empty;
            
            if (v)
            {
                if (_view.Table.RowCount > 0)
                {
                    _view.Table.Rows.Clear();

                }
            }
            _view.Request.Text = "Add";
        }

        internal bool ExecuteRequest(string method)
        {
            string msg = string.Empty;

            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"Remark", _view.Remark},
            };

            List<Dictionary<string, object>> dictLines = new List<Dictionary<string, object>>();

            //On Comment due to conflict in Update
            //if (method == "Add")
            //{
                foreach (DataGridViewRow row in _view.Table.Rows)
                {
                    if (row.Index != _view.Table.RowCount - 1)
                    {
                        dictLines.Add(new Dictionary<string, object>() {
                        {"U_DocEntry", row.Cells[0].Value},
                        {"U_CartonNo",  row.Cells[1].Value },
                        {"U_Ref1", row.Cells[2].Value },
                        {"U_Ref2", row.Cells[3].Value },
                        {"U_Remarks", row.Cells[4].Value },
                    });
                    }
                }
            //}
            
            var json = DataRepository.JsonBuilder(dict, dictLines, "CL_ROWSCollection");

            bool isPosted = _repository.ActivateService((message) => msg = message, method, _view.DocEntry, json);

            if (isPosted)
            {
                StaticHelper._MainForm.ShowMessage($"{msg} Document has been successfully added");
                ClearForm();
            }
            else
            {
                StaticHelper._MainForm.ShowMessage(msg,true);
            }

            return isPosted;
        }

        public void ShowExistingCartons()
        {
            string joins = "";

            if (_view.Table.RowCount > 1)
            {
                joins += "'";
                joins += string.Join("','", _view.Table.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[0].Value != null).ToList().Select(x => x.Cells[0].Value.ToString()).ToArray());
                joins += "'";
            }

            var model= new CartonManagementModel();
            var view = new FrmCartonManagement(true, joins);
            var presenter = new CartonManagementPresenter(view, model);

            view.ShowDialog();

            if (view.DocumentEntries.Count > 0)
            {
                DataTable cartonMng = _repository.SelectCarton(view.DocumentEntries);

                foreach (DataRow row in cartonMng.Rows)
                {
                    object[] a =
                    {
                        ConvertToString(row["DocEntry"]),
                        ConvertToString(row["U_CartonNo"]),
                        ConvertToString(row["U_VendorCode"]),
                        ConvertToString(row["U_Ref2"]),
                        ConvertToString(row["Remark"]),
                    };

                    _view.Table.Rows.Add(a);
                }

                view.DocumentEntries.Clear();
            }
        }
        public void ShowExistingCartons1()
        {

        }

            private string ConvertToString(object value)
        {
            return value == null ? "" : value.ToString();
        }

        private void ClearForm()
        {
            _view.Remark = string.Empty;
            
            if (_view.Table.RowCount > 0)
            {
                _view.Table.Rows.Clear();
            }
        }
    }
}

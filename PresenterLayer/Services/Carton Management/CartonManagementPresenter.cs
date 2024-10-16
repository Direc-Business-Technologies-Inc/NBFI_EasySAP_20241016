using DirecLayer;
using PresenterLayer.Helper;
using PresenterLayer.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PresenterLayer.Services
{
    public class CartonManagementPresenter
    {
        private readonly ICartonManagement _view;
        private readonly ICartonManagementModel _repository;

        public CartonManagementPresenter(ICartonManagement view,
            ICartonManagementModel repository)
        {
            _view = view;
            _view.Presenter = this;
            _repository = repository;

            DataGridViewColumns(_view.Table);
            _view.Status = "Draft";
        }

        public void DataGridViewColumns(DataGridView dgvCartonItems)
        {
            if (dgvCartonItems.Columns.Count <= 0)
            {
                DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
                col1.Name = "Item Number";
                dgvCartonItems.Columns.Add(col1);
                dgvCartonItems.Columns["Item Number"].ReadOnly = true;

                DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
                col2.Name = "Description";
                dgvCartonItems.Columns.Add(col2);
                dgvCartonItems.Columns["Description"].ReadOnly = true;

                DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
                col3.Name = "Quantity";
                dgvCartonItems.Columns.Add(col3);

                DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
                col4.Name = "Quantity Per Inner Box";
                dgvCartonItems.Columns.Add(col4);

                DataGridViewColumn col5 = new DataGridViewTextBoxColumn();
                col5.Name = "Index";
                dgvCartonItems.Columns.Add(col5);
                col5.Visible = true;

                DataGridViewColumn col6 = new DataGridViewTextBoxColumn();
                col6.Name = "Based Document";
                dgvCartonItems.Columns.Add(col6);
                col6.Visible = true;

                DataGridViewColumn col7 = new DataGridViewTextBoxColumn();
                col7.Name = "Based Document Type";
                dgvCartonItems.Columns.Add(col7);
                col7.Visible = true;

                dgvCartonItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        public DataTable StatusDataSource()
        {
            return _repository.StatusData();
        }

        public void GetSupplier()
        {
            List<string> m = _repository.SelectSupplier();

            if (m.Count() > 0)
            {
                _view.SuppCode = m[0];
                _view.SuppName = m[1];
                _view.Chain = GetChainByBP(m[0]);
            }
        }

        public string GetChainByBP(string bpCode)
        {
            return _repository.SelectChainByBP(bpCode);
        }

        public void GetChain()
        {
            List<string> m = _repository.SelectChain();

            if (m.Count() > 0)
            {
                _view.Chain = m[0];
            }
        }

        internal void GetTransactionType()
        {
            List<string> m = _repository.SelectTransactionType();

            if (m.Count() > 0)
            {
                _view.TransType = m[0];
            }
        }

        internal void GetTargetWarehouse()
        {
            List<string> m = _repository.SelectWarehouse("target");

            if (m.Count() > 0)
            {
                _view.TargetWhse = m[0];
            }
        }

        internal void GetLastWarehouse()
        {
            List<string> m = _repository.SelectWarehouse("target");

            if (m.Count() > 0)
            {
                _view.LastWhse = m[0];
            }
        }

        public bool GetBasedDocumentEntry()
        {
            bool isUserChoose = false;

            string tableName = _view.BasedDocument;

            List<string> m = _repository.SelectDocuments(tableName, _view.SuppCode, _view.TransType, _view.TransferType);

            if (m.Count() > 0)
            {
                _view.BasedDocEntry = m[0];

                List<string> m2 = _repository.SelectInventoryWarehouse(tableName, _view.BasedDocEntry);

                if (m2.Count > 0)
                {
                    _view.LastWhse = m2[0];
                    _view.TargetWhse = m2[1];
                }
                isUserChoose = true;
            }

            return isUserChoose;
        }

        public void GetItems(string tableName)
        {
            var form = new FrmPurchasingItemList();
            FrmPurchasingItemList.isCarton = true;
            form.oWhsCode = "02-RCV";

            if (string.IsNullOrEmpty(tableName) == false)
            {
                form.IsCartonActive = true;
                form.oTable = _view.BasedDocument;
                form.pnlItemOption.Visible = false;
                form.oDocEntry = _view.BasedDocEntry;

                if (_view.Table.Rows.Count > 1)
                {
                    List<string> qwe = new List<string>();

                    foreach (DataGridViewRow row in _view.Table.Rows)
                    {
                        if (row.Index != _view.Table.Rows.Count - 1)
                        {
                            qwe.Add(row.Cells[0].Value.ToString());
                        }
                    }

                    form.oSelectedItems = qwe;
                }
                form.ShowDialog();

                RefreshItemList(tableName);
            }
            else
            {
                StaticHelper._MainForm.ShowMessage($"Please make sure you selected based document first.",true);
            }
            FrmPurchasingItemList.isCarton = false;
        }

        private void RefreshItemList([Optional] string sTableName)
        {
            if (CartonItem.items.Count() > 0)
            {
                foreach (var x in CartonItem.items.ToList())
                {
                    CartonManagementRow cm = new CartonManagementRow();

                    cm.Index = x.Index;
                    cm.ItemNo = x.ItemCode;
                    cm.Description = x.Description;
                    cm.Quantity = x.Quantity;
                    cm.QtyPerInnerBox = x.QuantityInnerBox ?? x.Quantity;
                    cm.BasedDocEntry = x.BasedDocEntry;
                    //added new logic 082719
                    cm.BasedDocType = !string.IsNullOrEmpty(x.BasedDocType) ? x.BasedDocType : "O" + sTableName;
                    _repository.Save(cm);
                }

                CartonItem.items.Clear();

                LoadItems(_view.Table);
                ComputeTotal();
            }
        }

        public void ComputeTotal()
        {
            double dblTotalQty = 0;

            foreach (DataGridViewRow row in _view.Table.Rows)
            {
                if (row.Cells[2].Value != null)
                {
                    double dblQty = Convert.ToDouble(row.Cells[2].Value.ToString());
                    dblTotalQty += dblQty;
                }
            }

            _view.TotalQuantity = dblTotalQty.ToString();
        }

        internal DataTable GetTransferType()
        {
            DataTable v = _repository.SelectTransferType();

            return v;
        }

        public void LoadItems(DataGridView dgv)
        {
            List<CartonManagementRow> rows = _repository.GetItem();

            if (dgv.RowCount > 0)
            {
                dgv.Rows.Clear();
            }

            foreach (var x in rows.OrderBy(x => x.Index))
            {
                dgv.Rows.Add(x.ItemNo, x.Description,
                    x.Quantity, x.QtyPerInnerBox, x.Index, x.BasedDocEntry, x.BasedDocType);
            }
        }

        public bool ExecuteRequest(string method)
        {
            string msg = string.Empty;

            Dictionary<string, object> dict = new Dictionary<string, object>()
            {
                {"U_CartonNo", _view.CartonNo},
                {"U_VendorCode", _view.SuppCode},
                {"U_VendorName", _view.SuppName},
                {"U_ChainName", _view.Chain},
                {"U_GroupCode", _view.GroupCode},
                {"U_Ref2", _view.DocRef},
                {"U_Ref1", _view.ShipmentNo},
                {"U_DocRef", _view.OrderNo},
                {"U_Status", _view.Status},
                {"Remark", _view.Remarks},
                {"U_TransactionType", _view.TransType},
                {"U_TargetWH", _view.TargetWhse},
                {"U_LastWH", _view.LastWhse},
                { "U_DateChecked", DateTime.Parse(_view.DateChecked).ToString("yyyyMMdd")}
            };//DateTime.Parse(string.IsNullOrEmpty(TxtDateChecked.Text) ? DateTime.Now.ToShortDateString() : TxtDateChecked.Text).ToString("yyyyMMdd");

            List<Dictionary<string, object>> dictLines = new List<Dictionary<string, object>>();

            foreach (DataGridViewRow row in _view.Table.Rows)
            {
                if (row.Index != _view.Table.RowCount - 1)
                {
                    var lines = new Dictionary<string, object>();

                    lines.Add("U_ItemNo", row.Cells[0].Value);
                    lines.Add("U_Description", row.Cells[1].Value.ToString().Replace("\"",""));
                    lines.Add("U_Quantity", row.Cells[2].Value);
                    lines.Add("U_QuantityInnerBox", row.Cells[3].Value);

                    if (row.Cells[5].Value != null)
                    {
                        lines.Add("U_BaseRef", $"{row.Cells[5].Value}");
                        lines.Add("U_BaseType", $"{row.Cells[6].Value}");
                    }

                    dictLines.Add(lines);
                }
            }

            var json = DataRepository.JsonBuilder(dict, dictLines, "CM_ROWSCollection");

            bool isPosted = _repository.ActivateService((message) => msg = message, method, _view.DocEntry, json, $"CartonMngt");

            if (isPosted)
            {
                //string cartonlist = "";

                //if (_repository.ConnectCartonList((docEntry) => cartonlist = docEntry, _view.BasedDocument, _view.BasedDocEntry))
                //if (string.IsNullOrEmpty(_view.BasedDocEntry))
                if (string.IsNullOrEmpty(_view.DocEntry))
                {
                    AutomateCartonList("", msg, "Add");
                }
                else
                {
                    AutomateCartonList(_view.DocEntry, msg, "Update");
                }

                StaticHelper._MainForm.ShowMessage($"{msg} Document has been successfully added");
                ClearField();
            }
            else
            {
                StaticHelper._MainForm.ShowMessage(msg,true);
            }

            return isPosted;
        }

        private void AutomateCartonList(string cartonlistDocEntry, string cartonDocEntry, string method)
        {
            string msg = string.Empty;

            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"Remark", $"Created by EasySAP | Carton Management : {SboCred.UserID} : {DateTime.Today}"},
            };

            List<Dictionary<string, object>> dictLines = new List<Dictionary<string, object>>();

            if (method == "Update")
            {
                DataTable dt = _repository.GetCartonListItem(cartonlistDocEntry);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            dictLines.Add(new Dictionary<string, object>() {
                                {"U_DocEntry", ValidateInput.String(row["U_DocEntry"])},
                                {"U_CartonNo", ValidateInput.String(row["U_CartonNo"])},
                                {"U_Ref1", ValidateInput.String(row["U_Ref1"]) },
                                {"U_Ref2", ValidateInput.String(row["U_Ref2"]) },
                                {"U_Remarks", ValidateInput.String(row["U_Remarks"]) },
                            });
                        }
                    }
                }
            }

            dictLines.Add(new Dictionary<string, object>() {
                {"U_DocEntry", cartonDocEntry},
                {"U_CartonNo",  _view.CartonNo },
                {"U_Ref1", _view.SuppName },
                {"U_Ref2", _view.DocRef },
                {"U_Remarks", _view.Remarks },
            });

            var json = DataRepository.JsonBuilder(dict, dictLines, "CL_ROWSCollection");


            if (cartonlistDocEntry != string.Empty)
            {
                bool isPosted = _repository.ActivateService((message) => msg = message, method, cartonlistDocEntry, json, $"CartonList");

                if (isPosted)
                {
                    if (method == "Add")
                    {
                        string msg1 = string.Empty;

                        StringBuilder itr = new StringBuilder();

                        var q = '"';

                        itr.Append("{" + q + "U_CartonList" + q + ": " + q + msg + q + "}");

                        bool isSuccess = _repository.ActivateService((message) => msg1 = message, "Update", _view.BasedDocEntry, itr, $"InventoryTransferRequests");
                    }

                    //PublicStatic.frmMain.NotiMsg($"{msg} Document has been successfully added", System.Drawing.Color.Green);
                }
                else
                {
                    //PublicStatic.frmMain.NotiMsg(msg, System.Drawing.Color.Red);
                }
            }
        }

        public void DeleteItem(DataGridView dgv, int colIndex)
        {
            dgv.CurrentCell = dgv.CurrentRow.Cells[colIndex + 1];
            dgv.CurrentRow.Selected = true;
            dgv.Focus();

            var mousePosition = dgv.PointToClient(Cursor.Position);

            _view.MouseSelect.Show(dgv, mousePosition);
        }

        public void CommitDelete(DataGridView dgv)
        {
            var currentRow = dgv.CurrentRow;

            int index = Convert.ToInt32(currentRow.Cells["Index"].Value);

            _repository.DeleteItem(index);

            dgv.Rows.RemoveAt(currentRow.Index);
        }

        public bool GetDocument(string docEntry)
        {
            bool isInFindMode = false;

            DataTable header = _repository.GetDocumentHeaders(docEntry);

            if (header.Rows.Count > 0)
            {
                _view.DocEntry = ConvertToString(header.Rows[0]["DocEntry"]);
                _view.DocNo = ConvertToString(header.Rows[0]["DocNum"]);
                _view.CartonNo = ConvertToString(header.Rows[0]["U_CartonNo"]);
                _view.SuppCode = ConvertToString(header.Rows[0]["U_VendorCode"]);
                _view.SuppName = ConvertToString(header.Rows[0]["U_VendorName"]);
                _view.Chain = ConvertToString(header.Rows[0]["U_ChainName"]);
                _view.DocRef = ConvertToString(header.Rows[0]["U_DocRef"]);
                _view.OrderNo = ConvertToString(header.Rows[0]["U_Ref2"]);
                _view.ShipmentNo = ConvertToString(header.Rows[0]["U_Ref1"]);
                _view.TransType = ConvertToString(header.Rows[0]["U_TransactionType"]);
                _view.TargetWhse = ConvertToString(header.Rows[0]["U_TargetWH"]);
                _view.LastWhse = ConvertToString(header.Rows[0]["U_LastWH"]);
                _view.Status = ConvertToString(header.Rows[0]["U_Status"]);
                _view.DateChecked = ConvertToString(header.Rows[0]["U_DateChecked"]);
                _view.Remarks = ConvertToString(header.Rows[0]["Remark"]);

                DataTable lines = _repository.GetDocumentLines(docEntry);

                foreach (DataRow line in lines.Rows)
                {
                    CartonItem.items.Add(new CartonItem.Item
                    {
                        Index = FrmCartonManagement.ItemIndex++,
                        ItemCode = ConvertToString(line["U_ItemNo"]),
                        Description = ConvertToString(line["U_Description"]),
                        Quantity = ConvertToString(line["U_Quantity"]),
                        QuantityInnerBox = ConvertToString(line["U_QuantityInnerBox"]),
                        BasedDocEntry = ConvertToString(line["U_BaseRef"]),
                        BasedDocType = ConvertToString(line["U_BaseType"])
                    });
                }

                RefreshItemList();

                isInFindMode = true;
            }

            return isInFindMode;
        }

        public void GetExistingDocument(string DocumentEntryList)
        {
            _view.TableFindDocument = null;
            _view.TableFindDocument = _repository.SelectExistingDocument(DocumentEntryList);
        }

        public void ClearField()
        {
            _view.DocEntry = string.Empty;
            _view.DocNo = string.Empty;
            _view.CartonNo = string.Empty;
            _view.SuppCode = string.Empty;
            _view.SuppName = string.Empty;
            _view.Chain = string.Empty;
            _view.GroupCode = string.Empty;
            _view.OrderNo = string.Empty;
            _view.ShipmentNo = string.Empty;
            _view.TransType = string.Empty;
            _view.TargetWhse = string.Empty;
            _view.LastWhse = string.Empty;
            _view.Status = string.Empty;
            _view.DateChecked = string.Empty;
            _view.DocRef = string.Empty;
            _view.Remarks = string.Empty;
            _view.BasedDocEntry = string.Empty;
            _view.BasedDocument = string.Empty;
            if (_view.Table.RowCount > 0)
            {
                _view.Table.Rows.Clear();
            }

            _repository.ClearItemList();
            FrmCartonManagement.ItemIndex = 0;
            _view.Status = "Draft";
        }

        private string ConvertToString(object obj)
        {

            return obj == null ? "" : obj.ToString();
        }
    }
}

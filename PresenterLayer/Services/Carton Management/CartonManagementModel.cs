using DirecLayer;
using DirecLayer._03_Repository;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PresenterLayer.Services
{
    public class CartonManagementModel : ICartonManagementModel
    {
        StringQueryRepository _Repo = new StringQueryRepository();
        DataContextList _Context = new DataContextList();

        private string msg = string.Empty;

        public DataTable GetCartonListItem(string docEntry)
        {
            var dt = DataRepository.GetData(_Repo.GetCartonListItem(docEntry));

            return dt;
        }

        public bool ConnectCartonList(Func<string, string> returnMessage, string table, string cartonDocEntry)
        {
            var dt = DataRepository.GetData(_Repo.BasedDocumentCartonList(table, cartonDocEntry));

            if (dt == null)
            {
                return false; 
            }

            if (dt.Rows.Count > 0)
            {
                string docEntry = dt.Rows[0][0].ToString();

                if (docEntry == string.Empty)
                {
                    return false;
                }

                returnMessage(docEntry);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ActivateService(Func<string, string> returnMessage, string method,
            string docEntry, StringBuilder json, string table)
        {
            string message = "";
            var serviceLayer = new ServiceLayerAccess();
            bool isPosted = false;

            if (method == "Add")
            {
                //isPosted = SAPHana.SL_Posting(table, SAPHana.SL_Mode.POST, json,
                //   "DocEntry", out message);

                isPosted = serviceLayer.ServiceLayer_Posting(json, "POST", table, "DocEntry", out message, out string val);
            }
               
            else
            {
                if (docEntry != string.Empty)
                {
                  //  isPosted = SAPHana.SL_Posting(table, SAPHana.SL_Mode.PATCH, json,
                  //Convert.ToInt32(docEntry), out message);
                    string url = $"{table}({docEntry})";
                    isPosted = serviceLayer.ServiceLayer_Posting(json, "PATCH", url, "DocEntry", out message, out string val);
                }
            }
               
            returnMessage(message);
            return isPosted;
        }

        public void ClearItemList()
        {
            _Context.cartonManagementRow.Clear();
        }

        public void DeleteItem(int index)
        {
            _Context.cartonManagementRow.RemoveAll(x => x.Index == index);
        }

        public DataTable GetDocumentHeaders(string docEntry)
        {
            return DataRepository.GetData(_Repo.CartonMngtHeader(docEntry));
        }

        public DataTable GetDocumentLines(string docEntry)
        {
            return DataRepository.GetData(_Repo.CartonMngtRow(docEntry));
        }

        public List<CartonManagementRow> GetItem()
        {
            return _Context.cartonManagementRow.ToList();
        }

        public void Save(CartonManagementRow cm)
        {
            _Context.cartonManagementRow.Add(cm);
        }

        public List<string> SelectChain()
        {
            var m = DataRepository.Modal("chain", null, "List of documents");

            return m;
        }

        public string SelectChainByBP(string cardCode)
        {
            var sapHana = new SAPHanaAccess();
            var helper = new DataHelper();
            var m = helper.DataTableRet(sapHana.Get($"SELECT GroupCode FROM OCRD WHERE CardCode = '{cardCode}'"),0,"GroupCode","");

            return m;
        }

        public List<string> SelectDocuments(string tableName, string suppCode, string transtype, string transferType)
        {
            List<string> parameter = new List<string>
            {
                tableName,
                suppCode,
                transtype,
                transferType
            };

            var m = DataRepository.Modal("@Carton - Documentlist", parameter, "List of documents");

            return m;
        }

        public DataTable SelectExistingDocument(string DocumentEntryList)
        {
            return DataRepository.GetData(_Repo.ExistingCartonManagement(DocumentEntryList));
        }

        public List<string> SelectInventoryWarehouse(string tableName, string basedDocEntry)
        {
            List<string> listResult = new List<string>();

            if (tableName != string.Empty && basedDocEntry != string.Empty)
            {
                var dt = DataRepository.GetData(_Repo.InventoryWarehouse(tableName, basedDocEntry));

                if (dt.Rows.Count == 1)
                {
                    listResult.Add(dt.Rows[0][0].ToString()); // from
                    listResult.Add(dt.Rows[0][1].ToString()); // to
                }
            }

            return listResult;
        }

        public List<string> SelectSupplier()
        {
            var m = DataRepository.Modal("OCRD SPCS", null, "List of BP");

            return m;
        }

        public List<string> SelectTransactionType()
        {
            var m = DataRepository.Modal("@CM - Get Transaction Type", null, "List of Transaction Type");

            return m;
        }

        public DataTable SelectTransferType()
        {
            return DataRepository.GetData(_Repo.TransferType());
        }

        public List<string> SelectWarehouse(string v)
        {
            var m = DataRepository.Modal("@CM - Get target warehouse", null, "List of Target Warehouse");

            return m;
        }

        public DataTable StatusData()
        {
            return DataRepository.GetData(_Repo.CartonMngtStatus());
        }
    }
}

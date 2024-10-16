using DirecLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PresenterLayer.Services
{
    public interface ICartonManagementModel
    {
        DataTable StatusData();
        List<string> SelectSupplier();
        List<string> SelectChain();
        List<string> SelectTransactionType();
        List<string> SelectWarehouse(string v);
        List<string> SelectDocuments(string tableName, string suppCode, string transtype, string transferType);
        void Save(CartonManagementRow cm);
        List<CartonManagementRow> GetItem();
        void DeleteItem(int index);
        string SelectChainByBP(string cardCode);
        DataTable SelectExistingDocument(string DocumentEntryList);
        void ClearItemList();
        DataTable GetDocumentHeaders(string docEntry);
        DataTable GetDocumentLines(string docEntry);
        bool ActivateService(Func<string, string> returnMessage, string method, string docEntry, StringBuilder json, string table);
        DataTable SelectTransferType();
        List<string> SelectInventoryWarehouse(string tableName, string basedDocEntry);
        bool ConnectCartonList(Func<string, string> returnMessage, string table, string cartonDocEntry);
        DataTable GetCartonListItem(string cartonlistDocEntry);
    }
}

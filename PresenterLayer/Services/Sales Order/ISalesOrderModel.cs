using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterLayer.Services.SalesOrder
{
    public interface ISalesOrderModel
    {
        DataTable SelectCompany();
        DataTable SelectDocumentSeries();
        Dictionary<string, string> SelectSupplier();
        string SelectDepartment();
        string SelectDocumentNo(string seriesCode);
        string SelectWarehouse();
        Dictionary<string, string> SelectTaxGroup();
        Dictionary<string, string> SelectChain();
        List<string> SelectUom();
        string SelectProject(string code);
        Dictionary<string, string> SelectGeneralLedger();
        DataTable GetUDF();
        bool ActivateService(Func<string, string> p, string request, string docEntry, StringBuilder json, StringBuilder jsonH, bool isUserGoApproval);
        DataTable ExistingDocument();
        DataTable SelectDraftDocument(string docEntry, string status);
        DataTable SelectItemDraftDocumentLines(string docEntry, string docStatus);
        DataTable SelectServiceDraftDocumentLines(string docEntry);
        DataTable SelectDocument(string table, string docEntry);
        DataTable SelectDocumentLines(string table, string docEntry, string status);
        DataTable SelectServiceDocumentLines(string table, string docEntry);
        Dictionary<string, string> GetDocumentBrand(string v);
        DataTable GetCurrencyCodes();
        Dictionary<string, string> GetDocumentGl(string v);
        string AutomateTransferType(string suppCode);
        string CopyFrom(string code, string lookUp);
        string GetEmployees();
        string GetAllEmployees();
        string GetCartonList();
        string GetDesigner();
        string GetMerch();
        string GetBudgetCode();
        string GetCompanyCode(string company);
        int GetUomEntry(string v);
        bool CheckUser();
        string GetOrderNo(string code);
        double GetVatGroupRate(string vatGroup);
        bool CancelDocument(string docEntry);
        string GetCurrencyRate(string bpCurrency);
        List<string> GetJobOrderGL();
        string GetEmpID();
    }
}

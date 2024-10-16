namespace InfrastructureLayer.InventoryRepository
{
    public interface IQueryRepository
    {
        string GetCompanyQuery();
        string GetCompanyQuerySearch(string param);
        string GetSeriesQuery(string objectCode);
        string GetTransferTypeQuery();
        string GetUdfTableValuesQuery(string value);
        string GetUdfValidValuesQUery(string value);
        string GetMaintenanceLineQuery(int conditionnumber, string code, string item);
        string GetUdfQuery(string table, string fields, string arrangedUDF);
        string BPinformationQuery(string CardCode);
        string BpRateValueQuery(string currency);
        string BpTransferTypeQuery(string code);
        string UdfOrderNoQuery(string code);
        string VatGroupRateQuery(string code);
        string GetDocNumQuery(string objectCode, string Series);
        string GetUdfQueryWOorderBy(string table);
    }
}
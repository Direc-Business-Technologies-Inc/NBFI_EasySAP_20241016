using DirecLayer;
using DirecLayer._03_Repository;
using PresenterLayer.Helper;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PresenterLayer
{
    public class GoodRecieptModel : IGoodReceiptModel
    {
        StringQueryRepository _Repo = new StringQueryRepository();
        public static SAPHana HANA { get; set; }
        public List<string> GetJobOrderGL()
        {
            var dt = DataRepository.GetData(_Repo.GLJobOrder());

            List<string> result = new List<string>();

            if (dt == null)
            {
                result.Add("");
                result.Add("");

                return result;
            }
            else if (dt.Rows.Count <= 0)
            {
                result.Add("");
                result.Add("");

                return result;
            }

            result.Add(ValidateInput.String(dt.Rows[0][0]));
            result.Add(ValidateInput.String(dt.Rows[0][1]));

            return result;
        }

        public string GetCurrencyRate(string bpCurrency)
        {
            var dt = DataRepository.GetData(_Repo.CurrencyRate(bpCurrency));

            if (dt == null)
            {
                return null;
            }
            else if (dt.Rows.Count <= 0)
            {
                return null;
            }

            return dt.Rows[0][0].ToString();
        }

        public DataTable GetCurrencyCodes()
        {
            var dt = DataRepository.GetData(_Repo.CurrencyCode());

            return dt;
        }

        public bool CheckUser()
        {
            bool isSuperUser = false;

            var dt = DataRepository.GetData(_Repo.UserApprovalCheck());

            if (dt.Rows.Count <= 0)
            {
                isSuperUser = true;
            }

            return isSuperUser;
        }

        public bool ActivateService(Func<string, string> returnMessage, string request, string docEntry, StringBuilder json, StringBuilder jsonH, bool isUserGoApproval)
        {
            var isPosted = false;
            var serviceLayer = new ServiceLayerAccess();
            var returnvalue = string.Empty;
            string svalue;
            string err;

            if (request == "Add")
            {
                // DARREL CHECKING IF THE DOCUMENT IS NEW ADDING DOCUMENT OR APPROVAL POSTING
                //isPosted = SAPHana.SL_PostingWithApproval("PurchaseDeliveryNotes", json,out string sMsg);
                var serviceLayerAccess = new ServiceLayerAccess();
                isPosted = serviceLayerAccess.ServiceLayer_Posting(json, "POST", "PurchaseDeliveryNotes", "DocEntry", out err, out svalue);
            }
            else
            {
                string url = $"PurchaseDeliveryNotes({docEntry})";
                //isPosted = SAPHana.SL_AJAX((value) => returnvalue = value, url, "PATCH", json.ToString(), "DocEntry");
                isPosted = serviceLayer.ServiceLayer_Posting(jsonH, "PATCH", url, "DocEntry", out err, out svalue);
            }

            //StaticHelper.TempUser = null;

            //string message = "";
            returnMessage(svalue);
            return isPosted;
        }

        public bool CancelDocument(string docEntry)
        {
            bool isSuccess = true;
            string url = $"PurchaseDeliveryNotes({docEntry})/Cancel";
            string returnvalue = string.Empty;
            isSuccess = SAPHana.SL_AJAX((value) => returnvalue = value, url, "POST", string.Empty, "DocEntry");
            return isSuccess;
        }

        public Dictionary<string, string> SelectChain()
        {
            Dictionary<string, string> chain = new Dictionary<string, string>();

            var m = DataRepository.Modal("chain", null, "List of Chains");

            if (m.Count > 0)
            {
                chain.Add("Code", m[0]);
                chain.Add("Value", m[1]);
            }

            return chain;
        }

        public string SelectDepartment()
        {
            string result = "";

            var m = DataRepository.Modal("dept", null, "List of Department");

            if (m.Count > 0)
            {
                result = m[0];
            }

            return result;
        }

        public string SelectDocumentNo(string seriesCode)
        {
            string result = "";

            DataTable dt = DataRepository.GetData(_Repo.SeriesNo("22", seriesCode));

            if (dt.Rows.Count > 0)
            {
                result = ConvertToString(dt.Rows[0]["NextNumber"]);
            }

            return result;
        }

        public Dictionary<string, string> SelectGeneralLedger()
        {
            Dictionary<string, string> gl = new Dictionary<string, string>();

            List<string> m = DataRepository.Modal("G/L Account", null, "List of G/L Accounts");

            if (m.Count > 0)
            {
                gl.Add("Code", m[0]);
                gl.Add("Name", m[1]);
            }

            return gl;
        }

        public string SelectProject(string code)
        {
            string result = "";
            var m = DataRepository.Modal("Project", null, "List of Projects");
            //var m = DataRepository.GetData(_Repo.BpProjectCode(code));
            //if (m != null)
            //{
            //    if (m.Rows.Count > 0)
            //    {
            //        result = m.Rows[0][0].ToString();
            //    }
            //}
            if (m.Count > 0)
            {
                result = m[0];
            }

            return result;
        }

        public Dictionary<string, string> SelectSupplier()
        {
            Dictionary<string, string> info = new Dictionary<string, string>();

            List<string> parameters = new List<string>()
            {
                "S"
            };

            List<string> m = DataRepository.Modal("OCRD BP", parameters, "List of Supplier");

            if (m.Count > 0)
            {
                DataTable dt = DataRepository.GetData(_Repo.BPinformation(m[0]));

                if (dt.Rows.Count > 0)
                {
                    string currency = ConvertToString(dt.Rows[0]["Currency"]);

                    info.Add("SupplierCode", m[0]);
                    info.Add("SupplierName", m[1]);
                    info.Add("ContactPerson", ConvertToString(dt.Rows[0]["CntctPrsn"]));
                    info.Add("Currency", currency);
                    info.Add("VatGroup", ConvertToString(dt.Rows[0]["ECVatGroup"]));
                    info.Add("Warehouse", ConvertToString(dt.Rows[0]["Whs"]));
                    info.Add("RawCurrency", ConvertToString(dt.Rows[0]["RawCurrency"]));

                    DataTable dtBpRate = DataRepository.GetData(_Repo.BpRateValue(currency));

                    string rate = dtBpRate.Rows.Count > 0 ? ConvertToString(dtBpRate.Rows[0]["Rate"]) : "";

                    info.Add("CurrencyRate", rate);
                }
            }

            return info;
        }

        public DataTable GetUDF()
        {

            return DataRepository.GetData(SP.UDF("OPDN"));
        }

        public DataTable SelectCompany()
        {
            return DataRepository.GetData(_Repo.Company());
        }

        public DataTable ExistingDocument()
        {
            return DataRepository.GetData(_Repo.GoodReceiptExistingDocs());
        }

        public DataTable SelectDraftDocument(string docEntry, string status)
        {
            return DataRepository.GetData(_Repo.PurchaserOrderDraft(docEntry, status));
        }

        public DataTable SelectItemDraftDocumentLines(string docEntry, string docStatus)
        {
            return DataRepository.GetData(_Repo.PurchaseOrderLinesItemDraft(docEntry, docStatus));
        }

        public DataTable SelectServiceDraftDocumentLines(string docEntry)
        {
            return DataRepository.GetData(_Repo.PurchaseOrderLinesServiceDraft(docEntry));
        }

        public DataTable SelectDocument(string table, string docEntry)
        {
            return DataRepository.GetData(_Repo.PurchaseOrder(table, docEntry));
        }

        public DataTable SelectDocumentLines(string table, string docEntry)
        {
            return DataRepository.GetData(_Repo.PurchaseOrderLinesItem(table, docEntry));
        }

        public DataTable SelectServiceDocumentLines(string table, string docEntry)
        {
            return DataRepository.GetData(_Repo.PurchaseOrderLinesService(table, docEntry));
        }

        public DataTable SelectDocumentSeries()
        {
            return DataRepository.GetData(_Repo.SeriesCode("22"));
        }

        public Dictionary<string, string> SelectTaxGroup()
        {
            Dictionary<string, string> TaxGroup = new Dictionary<string, string>();

            List<string> parameters = new List<string>()
            {
                "I"
            };

            List<string> list = DataRepository.Modal("OVTG", parameters, "Tax Code");

            if (list.Count > 0)
            {
                TaxGroup.Add("Group", list[0]);
                TaxGroup.Add("Rate", list[1]);
            }

            return TaxGroup;
        }

        public List<string> SelectUom()
        {
            List<string> result = new List<string>();

            List<string> list = DataRepository.Modal("UoM", null, "UoM List");

            if (list.Count > 0)
            {
                result.Add(list[0]);
                result.Add(list[1]);
            }

            return result;
        }

        public string SelectWarehouse()
        {
            string result = "";

            var list = DataRepository.Modal("OWHS", null, "List of Warehouse");

            if (list.Count > 0)
            {
                result = list[0];
            }

            return result;
        }

        private string ConvertToString(object value)
        {
            return value == null ? "" : value.ToString();
        }

        public Dictionary<string, string> GetDocumentBrand(string v)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            var dt = DataRepository.GetData(_Repo.Brands(v));

            if (dt.Rows.Count > 0)
            {
                values.Add("Code", dt.Rows[0][0].ToString());
                values.Add("Name", dt.Rows[0][1].ToString());
            }

            return values;
        }

        public Dictionary<string, string> GetDocumentGl(string v)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            var dt = DataRepository.GetData(_Repo.BpAccount(v));

            if (dt.Rows.Count > 0)
            {
                values.Add("Code", dt.Rows[0][0].ToString());
                values.Add("Name", dt.Rows[0][1].ToString());
            }

            return values;
        }

        public string AutomateTransferType(string suppCode)
        {
            string result = "";

            var dt = DataRepository.GetData(_Repo.BpTransferType(suppCode));

            if (dt.Rows.Count > 0 || dt != null)
            {
                result = dt.Rows[0]["TransType"].ToString();
            }

            return result;
        }

        public string CopyFrom(string code, string lookUp)
        {
            string result = "";

            List<string> parameter = new List<string>()
            {
                code,
            };

            var m = DataRepository.Modal(lookUp, parameter, "List of Documents");

            if (m.Count > 0)
            {
                result = m[0];
            }

            return result;
        }

        public string GetEmployees()
        {
            string result = "";

            var m = DataRepository.Modal("EmployeeList", null, "Signatories");

            if (m.Count > 0)
            {
                result = m[0];
            }

            return result;
        }

        public string GetAllEmployees()
        {
            string result = "";

            var m = DataRepository.Modal("EmployeeList *", null, "Employee List");

            if (m.Count > 0)
            {
                result = m[0];
            }

            return result;
        }

        public string GetCartonList()
        {
            string result = "";

            var m = DataRepository.Modal("cartonList", null, "List of Cartons");

            if (m.Count > 0)
            {
                result = m[0];
            }

            return result;
        }

        public string GetDesigner()
        {
            string result = "";

            var m = DataRepository.Modal("Designers List", null, "List of Designers");

            if (m.Count > 0)
            {
                result = m[0];
            }

            return result;
        }

        public string GetMerch()
        {
            string result = "";

            var m = DataRepository.Modal("Merch. Coor List", null, "List of Merch. Coordinator");

            if (m.Count > 0)
            {
                result = m[0];
            }

            return result;
        }

        public string GetBudgetCode()
        {
            string result = "";

            var m = DataRepository.Modal("Automate Budget Code", null, "List of Budgets");

            if (m.Count > 0)
            {
                result = m[0];
            }

            return result;
        }

        public string GetCompanyCode(string company)
        {
            string result = "";

            var dt = DataRepository.GetData(_Repo.CompanyCode(company));

            if (dt.Rows.Count > 0 || dt != null)
            {
                result = dt.Rows[0][0].ToString();
            }

            return result;
        }

        public int GetUomEntry(string v)
        {
            int result = 1;

            var dt = DataRepository.GetData(_Repo.UomEntry(v));

            if (dt.Rows.Count > 0 && dt != null)
            {
                result = Convert.ToInt32(dt.Rows[0][0]);
            }

            return result;
        }

        public string GetOrderNo(string code)
        {
            string result = "";

            var dt = DataRepository.GetData(_Repo.UdfOrderNo(code));

            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0][0].ToString();
            }

            return result;
        }

        public double GetVatGroupRate(string vatGroup)
        {
            double a = 0D;

            if (vatGroup != string.Empty)
            {
                var dt = DataRepository.GetData(_Repo.VatGroupRate(vatGroup));

                if (dt.Rows.Count > 0)
                {
                    //a = 1 + Convert.ToDouble(dt.Rows[0][0]) / 100);

                    a = Convert.ToDouble(dt.Rows[0][0]);
                }
            }

            return a;
        }

        public string GetEmpID()
        {
            var dt = DataRepository.GetData(_Repo.EmpId());

            string result = string.Empty;

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0][0].ToString();
                }
            }

            return result;
        }
    }
}

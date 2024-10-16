using Context;
using DirecLayer;
using DirecLayer._02_Form.MVP.Views;
using DomainLayer.Models;
using PresenterLayer.Views;
using PresenterLayer.Views.Main;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresenterLayer.Helper.Unofficial_Sales
{
    public class UOS_ServiceLayer
    {
        public FrmUnofficialSales frmUOS { get; set; }
        public object Presenter { get; private set; }
   
        // public FrmBarcodeEncoding_UDF frmUOS_UDF { get; set; }
        public MainForm frmMain;
        private ITmaintenance ITm = new ITmaintenance();

        public void SI_Posting(string sTypeOfRequest, [Optional] int sDocEntry)
        {
            try
            {
                var sbJson = new StringBuilder();
                var mode = new SAPHana.SL_Mode();

                var dgvItems = frmUOS.dgvBarcodeItems;
                //var dgvUDF = Presenter.UdfRequest();

                sbJson.AppendLine("{");

                mode = SAPHana.SL_Mode.POST;

                var oSettings = Properties.Settings.Default;
                string sDocumentType = frmUOS.cbDocumentType.SelectedValue.ToString() == "" ? "" : frmUOS.cbDocumentType.SelectedValue.ToString();

                string strBpCode = frmUOS.txtBpCode.Text;

                sbJson.AppendLine($@"   ""DocDate"": ""{frmUOS.dtMonthofSales.Value.ToString("yyyy-MM-dd")}"",");

                if (frmUOS.Text == "Delivery")
                {
                    sbJson.AppendLine($@"   ""DocDueDate"": ""{frmUOS.dtDeliveryDate.Value.ToString("yyyy-MM-dd")}"",");
                }
                else
                {
                    sbJson.AppendLine($@"   ""DocDueDate"": ""{frmUOS.dtMonthofSales.Value.ToString("yyyy-MM-dd")}"",");
                }

                sbJson.AppendLine($@"   ""TaxDate"": ""{frmUOS.dtDocDate.Value.ToString("yyyy-MM-dd")}"",");
                sbJson.AppendLine($@"   ""Series"": ""{Convert.ToInt32(frmUOS.oSeries)}"",");
                sbJson.AppendLine($@"   ""CardCode"": ""{strBpCode}"",");
                sbJson.AppendLine($@"   ""U_Remarks"": ""Created by EasySAP  : {SboCred.UserID} : {DateTime.Now} | Powered By : DIREC"",");

                if (frmUOS.txtSONumber.Text != "")
                {
                    sbJson.AppendLine($@"   ""Comments"": ""Uploaded by EasySAP - AR Inv based from SO No. { frmUOS.txtSONumber.Text } ,");
                }
                else
                {
                    sbJson.AppendLine($@"   ""Comments"": ""Uploaded by EasySAP"",");
                }

                sbJson.AppendLine($@"   ""U_PrepBy"": ""{SboCred.UserID}"",");
                sbJson.AppendLine($@"   ""U_DocType"": ""{sDocumentType}"",");
                sbJson.AppendLine($@"   ""ShipToCode"": ""{frmUOS.oAddressCode}"",");
                //
                sbJson.AppendLine($@"   ""DocCurrency"": ""{frmUOS.txtCurrency.Text}"",");
                
                sbJson.AppendLine($@"   ""DocumentsOwner"": ""{Convert.ToInt32(EasySAPCredentialsModel.GetEmployeeCode())}"",");

                if (frmUOS.oSalesEmployee != null)
                {
                    sbJson.AppendLine($@"   ""SalesPersonCode"": ""{frmUOS.oSalesEmployee}"",");
                }

                if (frmUOS.txtSalesEmployee.Text != null)
                {
                    sbJson.AppendLine($@"   ""SalesPersonCode"": ""{Convert.ToInt32(frmUOS.oSalesEmployee)}"",");
                }

                //Posting of UDF
                //foreach (DataGridViewRow dr in dgvUDF.Rows)
                //{
                //    string code = LibraryHelper.DataGridViewRowRet(dr, "Code");
                //    string val = LibraryHelper.DataGridViewRowRet(dr, "Field");

                //    if (!string.IsNullOrEmpty(val))
                //    { sbJson.AppendLine($@"   ""{code}"": ""{val}"","); }
                //}

                //Per line/row
                sbJson.AppendLine(@"   ""DocumentLines"": [");

                int dgvCount = 0;

                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    double discperc = Convert.ToDouble(row.Cells["Discount %"].Value);
                    double priveafvat = Convert.ToDouble(row.Cells["Unit Price"].Value);
                    string sItemCode = row.Cells[0].Value.ToString();
                    string warehouse = frmUOS.txtWhsCode.Text;

                    string strOpen = dgvCount == 0 ? "      {" : ",      {";
                    sbJson.AppendLine(strOpen);
                    sbJson.AppendLine($@"       ""ItemCode"": ""{sItemCode}"",");
                    sbJson.AppendLine($@"       ""Quantity"": ""{Convert.ToDouble(row.Cells["Quantity"].Value)}"",");
                    sbJson.AppendLine($@"       ""DiscountPercent"": ""{Convert.ToDouble(row.Cells["Discount %"].Value)}"",");
                    sbJson.AppendLine($@"       ""WarehouseCode"": ""{warehouse}"",");
                    sbJson.AppendLine($@"       ""UnitPrice"": ""{priveafvat}"",");
                    sbJson.AppendLine($@"       ""U_SortCode"": ""{row.Cells["SortCode"].Value.ToString()}"",");

                    string dept = DataAccess.SearchData(DataAccess.conStr("HANA"), "SELECT U_Dim1 FROM OCRD Where CardCode = '" + strBpCode + "'", 0, "U_Dim1");
                    string brand = DataAccess.SearchData(DataAccess.conStr("HANA"), "SELECT U_ID019 FROM OITM Where ItemCode = '" + sItemCode + "'", 0, "U_ID019");

                    sbJson.AppendLine($@"       ""CostingCode2"": ""{brand}"",");
                    sbJson.AppendLine($@"       ""COGSCostingCode2"": ""{brand}"",");
                    sbJson.AppendLine($@"       ""ProjectCode"": ""{frmUOS.oProject}"", ");
                    sbJson.AppendLine($@"       ""ShipDate"": ""{Convert.ToDateTime(row.Cells["DeliveryDate"].Value)}"" ");
                    sbJson.AppendLine("     }");

                    dgvCount++;

                    Application.DoEvents();
                }

                sbJson.AppendLine(" ]");
                sbJson.AppendLine("}");

                string sValue;
                string err;
                string sSapModule = sTypeOfRequest == "POST" ? "DeliveryNotes" : "DeliveryNotes" + $"({ sDocEntry })";

                var serviceLayerAccess = new ServiceLayerAccess();
                bool ret = serviceLayerAccess.ServiceLayer_Posting(sbJson, sTypeOfRequest, sSapModule, "DocEntry", out err, out sValue);

                if (ret)
                {
                    frmUOS.ClearData();
                    //Presenter.UdfRequest();

                    string DocNum = "0";
                    DocNum = ITm.GetDocNum(sValue, "ODLN");

                    StaticHelper._MainForm.ProgressClear();
                    err = $"Document No.{DocNum} has been successfully added";
                }

                frmMain.ShowMessage(err, ret == true ? false : true);

            }
            catch (Exception ex)
            {
                frmMain.ShowMessage(ex.Message, true);
            }
        }

    }
}

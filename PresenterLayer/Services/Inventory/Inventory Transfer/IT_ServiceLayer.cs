using System;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PresenterLayer.Views.Main;
using ServiceLayer.Services;
using DirecLayer;
using DomainLayer.Helper;
using PresenterLayer.Views.Inventory.Inventory_Transfer;
using PresenterLayer.Helper;

namespace PresenterLayer.Services.Inventory.Inventory_Transfer
{
    public class IT_ServiceLayer
    {
        public IInventoryTransfer frmIT { get; set; }
        //public frmIT_UDF frmIT_UDF { get; set; }
        public MainForm frmMain;



        private ITmaintenance ITm = new ITmaintenance();
        SAPHanaAccess sapHanaAccess = new SAPHanaAccess();
        SboCredentials sboCredentials = new SboCredentials();
        DataHelper dataHelper = new DataHelper();
        ServiceLayerAccess serviceLayerAccess = new ServiceLayerAccess();
        InventoryTransferService itService;

        public IT_ServiceLayer(IInventoryTransfer frmIT, MainForm frmMain, InventoryTransferService itService)
        {
            this.frmIT = frmIT;
            this.frmMain = frmMain;
            this.itService = itService;
        }

        public enum SL_Mode
        {
            PATCH = 1,
            POST = 2,
            DELETE = 3
        }

        public void IT_Posting(string sTypeOfRequest, [Optional] int sDocEntry)
        {
            try
            {
                var sbJson = new StringBuilder();
                var mode = new SL_Mode();

                var dgvItems = frmIT.dgvItem;
                var dgvUDF = frmIT.UDF;

                string strBpCode = frmIT.TxtBpCode.Text;
                string strFWhsCode = frmIT.TxtFWhsCode.Text;
                string strTWhsCode = frmIT.TxtTWhsCode.Text;

                sbJson.AppendLine("{");

                if (sTypeOfRequest == "POST")
                {
                    if (strBpCode != "")
                    {
                        sbJson.AppendLine($@"   ""CardCode"": ""{strBpCode}"",");
                    }

                    var oSettings = Properties.Settings.Default;
                    string sCompany = frmIT.CbCompany.SelectedValue.ToString() == "" ? "" : frmIT.CbCompany.SelectedValue.ToString();
                    string sTransferType = frmIT.CbTransferType.SelectedValue.ToString() == "" ? "" : frmIT.CbTransferType.SelectedValue.ToString();

                    sbJson.AppendLine($@"   ""Series"": ""{Convert.ToInt32(frmIT.OSeries)}"",");
                    sbJson.AppendLine($@"   ""DocDate"": ""{frmIT.DtPostingDate.Value.ToString("yyyy-MM-dd")}"",");
                    sbJson.AppendLine($@"   ""TaxDate"": ""{frmIT.DtDocDate.Value.ToString("yyyy-MM-dd")}"",");
                    sbJson.AppendLine($@"   ""Address"": ""{frmIT.TxtAddress.Text}"",");
                    sbJson.AppendLine($@"   ""U_TransferType"": ""{sTransferType}"",");
                    sbJson.AppendLine($@"   ""U_CompanyTIN"": ""{sCompany}"",");
                    //sbJson.AppendLine($@"   ""U_Remarks"": ""Created by EasySAP  : {sboCredentials.UserId} : {DateTime.Now} | Powered By : DIREC"",");

                    sbJson.AppendLine($@"   ""Comments"": ""{frmIT.TxtRemarks.Text}"" ,");

                    if (sbJson.ToString().Contains("U_PostRem") == false && sTypeOfRequest == "POST")
                    {
                        sbJson.AppendLine($@"   ""U_PostRem"": ""Transact using EasySAP | Inventory Transfer : {sboCredentials.UserId} : {DateTime.Now} : | Powered By : DIREC"",");
                    }



                    //Header WareHouse
                    if (strFWhsCode != "")
                    {
                        sbJson.AppendLine($@"   ""FromWarehouse"": ""{strFWhsCode}"",");
                    }

                    if (strTWhsCode != "")
                    {
                        sbJson.AppendLine($@"   ""ToWarehouse"": ""{strTWhsCode}"",");
                    }

                    if (frmIT.TxtSalesEmployee.Text != null && frmIT.TxtSalesEmployee.Text != "-No Sales Employee-" && frmIT.TxtSalesEmployee.Text != "")
                    {
                        sbJson.AppendLine($@"   ""SalesPersonCode"": ""{Convert.ToInt32(frmIT.TxtSalesEmployee.Text)}"",");
                    }
                }

                //Posting of UDF
                //foreach (DataGridViewRow dr in dgvUDF.Rows)
                //{
                //    string code = LibraryHelper.DataGridViewRowRet(dr, "Code");
                //    string val = LibraryHelper.DataGridViewRowRet(dr, "Field");

                //    if (!string.IsNullOrEmpty(val))
                //    { sbJson.AppendLine($@"   ""{code}"": ""{val}"","); }
                //}
                foreach (DataGridViewRow dr in dgvUDF.Rows)
                {
                    string code = DataGridViewRowRet(dr, "Code");
                    string val = DataGridViewRowRet(dr, "Field");
                    if (code.Contains("Date") || code.Contains("date"))
                    {
                        if (val != "")
                        {
                            var date = Convert.ToDateTime(val);
                            val = date.ToString("yyyy-MM-dd");
                        }
                    }

                    if (!string.IsNullOrEmpty(val))
                    { sbJson.AppendLine($@"   ""{code}"": ""{val}"","); }
                }

                if (sTypeOfRequest == "POST")
                {
                    //Per line/row
                    sbJson.AppendLine(@"   ""StockTransferLines"": [");

                    int dgvCount = 0;

                    foreach (DataGridViewRow row in dgvItems.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["Quantity"].Value) > 0 && row.Cells["From Warehouse"] != null)
                        {

                            string strSKU = string.IsNullOrEmpty(row.Cells["SKU"].Value.ToString()) ? "" : row.Cells["SKU"].Value.ToString();
                            string strCompany = string.IsNullOrEmpty(row.Cells["Company"].Value.ToString()) ? "" : GetCompanyCode(row.Cells["Company"].Value.ToString());
                            string strSortCode = string.IsNullOrEmpty(row.Cells["SortCode"].Value.ToString()) ? "" : row.Cells["SortCode"].Value.ToString();

                            string strOpen = dgvCount == 0 ? "      {" : ",      {";
                            sbJson.AppendLine(strOpen);

                            if (string.IsNullOrEmpty(frmIT.TxtITR_DocEntry.Text))
                            {
                                sbJson.AppendLine($@"       ""ItemCode"": ""{row.Cells["Item No."].Value.ToString()}"",");

                                if (row.Cells["BaseEntry"].Value != null)
                                {
                                    if (!string.IsNullOrEmpty(row.Cells["BaseEntry"].Value.ToString()))
                                    {
                                        sbJson.AppendLine($@"       ""BaseType"": ""1250000001"",");
                                        sbJson.AppendLine($@"       ""BaseEntry"": ""{row.Cells["BaseEntry"].Value.ToString()}"",");
                                        sbJson.AppendLine($@"       ""BaseLine"": ""{row.Cells["BaseLine"].Value.ToString()}"",");
                                    }
                                }

                            }
                            else
                            {
                                sbJson.AppendLine($@"       ""BaseType"": ""1250000001"",");
                                sbJson.AppendLine($@"       ""BaseEntry"": ""{frmIT.TxtITR_DocEntry.Text}"",");
                                sbJson.AppendLine($@"       ""BaseLine"": ""{row.Cells["BaseLine"].Value.ToString()}"",");
                            }

                            //On Comment for previous logic, changes due to carton qty 091719
                            //sbJson.AppendLine($@"       ""Quantity"": ""{Convert.ToDouble(row.Cells["Quantity"].Value)}"",");
                            sbJson.AppendLine($@"       ""Quantity"": ""{Convert.ToDouble(row.Cells["Ordered Qty"].Value)}"",");
                            sbJson.AppendLine($@"       ""UnitPrice"": ""{Convert.ToDouble(row.Cells["Info Price"].Value)}"",");
                            sbJson.AppendLine($@"       ""WarehouseCode"": ""{row.Cells["To Warehouse"].Value}"",");
                            sbJson.AppendLine($@"       ""FromWarehouseCode"": ""{row.Cells["From Warehouse"].Value}"",");
                            sbJson.AppendLine($@"       ""ProjectCode"": ""{frmIT.OProject}"",");
                            sbJson.AppendLine($@"       ""U_Style"": ""{row.Cells["Style Code"].Value}"",");
                            //sbJson.AppendLine($@"       ""U_StyleName"": ""{row.Cells["Style"].Value}"",");
                            sbJson.AppendLine($@"       ""U_Company"": ""{strCompany}"",");
                            sbJson.AppendLine($@"       ""U_SortCode"": ""{strSortCode}"",");
                            sbJson.AppendLine($@"       ""U_SKU"": ""{strSKU}"",");
                            sbJson.AppendLine($@"       ""U_Chain"": ""{row.Cells["Chain"].Value}"",");
                            sbJson.AppendLine($@"       ""U_ChainDescription"": ""{row.Cells["Chain Description"].Value}""");
                            sbJson.AppendLine("     }");

                            dgvCount++;
                        }
                    }

                    sbJson.AppendLine(" ]");
                }
                
                sbJson.AppendLine("}");

                string sValue;
                string err;
                string sSapModule = "";

                if (sTypeOfRequest == "POST")
                {
                    sSapModule = sTypeOfRequest == "POST" ? "StockTransfers" : "StockTransfers" + $"({ sDocEntry })";
                }
                else
                {
                    sSapModule = "StockTransfers" + $"({ sDocEntry })";
                }

                bool ret = serviceLayerAccess.ServiceLayer_Posting(sbJson, sTypeOfRequest, sSapModule, "DocEntry", out err, out sValue);

                if (ret)
                {
                    //frmIT.ClearData();
                    //frmIT_UDF._LoadData();
                    itService.ClearData();
                    string DocNum = "0";
                    sValue = sTypeOfRequest == "POST" ? sValue : sDocEntry.ToString();
                    DocNum = ITm.GetDocNum(sValue, "OWTR");

                    //PublicStatic.frmMain.ProgressClear();
                    string strMessage = sTypeOfRequest == "POST" ? "added" : "updated";
                    err = $"Document No.{DocNum} has been successfully {strMessage}.";
                }

               StaticHelper._MainForm.ShowMessage(err, !ret);

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
            }
        }


        private string GetCompanyCode(string strCompName)
        {

            string CompCode = "";

            try
            {
                CompCode = sapHanaAccess.Get($"select Code from [@CMP_INFO] where Name = '{strCompName}'").Rows[0]["Code"].ToString();
                return CompCode;
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
                return CompCode;
            }

        }

        public static string DataGridViewRowRet(DataGridViewRow dr, string field)
        {
            string ret;
            if (dr.Cells[field].Value == null)
            {
                ret = "";
            }
            else
            {
                ret = dr.Cells[field].Value.ToString();
            }
            return ret;
        }
    }
}

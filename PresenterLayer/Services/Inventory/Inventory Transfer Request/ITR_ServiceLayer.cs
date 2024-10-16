using MetroFramework;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;
using PresenterLayer.Views.Inventory.Inventory_Transfer_Request;
using PresenterLayer.Views.Main;
using DirecLayer;
using DomainLayer.Helper;
using ServiceLayer.Services;
using PresenterLayer.Helper;
using DomainLayer.Models;

namespace PresenterLayer.Services.Inventory
{
    public class ITR_ServiceLayer
    {
        public IFrmInventoryTransferRequest frmITR;
        public MainForm frmMain;
        public InventoryTransferRequestService inventoryTransferRequestService;

        SAPHanaAccess sapHanaAccess = new SAPHanaAccess();
        SboCredentials sboCredentials = new SboCredentials();
        DataHelper dataHelper = new DataHelper();
        ServiceLayerAccess serviceLayerAccess = new ServiceLayerAccess();

        private ITRmaintenance ITRm = new ITRmaintenance();

        public ITR_ServiceLayer(IFrmInventoryTransferRequest frmITR, MainForm frmMain, InventoryTransferRequestService inventoryTransferRequestService)
        {
            this.frmITR = frmITR;
            this.frmMain = frmMain;
            this.inventoryTransferRequestService = inventoryTransferRequestService;
        }

        public enum SL_Mode
        {
            PATCH = 1,
            POST = 2,
            DELETE = 3
        }


        public void ITR_Posting(string sTypeOfRequest, [Optional] int sDocEntry)
        {
            try
            {
                var sbJson = new StringBuilder();
                var mode = new SL_Mode();

                var dgvItems = frmITR.table;
                var dgvUDF = frmITR.UDF;

                string strBpCode = frmITR.BpCode;
                string strFWhsCode = frmITR.FrmWhsCode;
                string strTWhsCode = frmITR.ToWhsCode;

                int max = dgvItems.Rows.Count;
                int min = 0;

                sbJson.AppendLine("{");

                mode = SL_Mode.POST;

                if (strBpCode != "")
                {
                    sbJson.AppendLine($@"   ""CardCode"": ""{strBpCode}"",");
                }

                string qry = $"SELECT ISNULL(firstName, '') [firstName], ISNULL(lastName, '') [lastName] FROM OHEM where U_User = '{GetEmployeeCode()}'";
                DataTable dtuser = new DataTable();
                dtuser = sapHanaAccess.Get(qry);
                string fname = "";
                string lname = "";
                string fullname = " ";

                if (dtuser.Rows.Count > 0)
                {
                    fname = DataAccess.Search(dtuser, 0, "firstName");
                    lname = DataAccess.Search(dtuser, 0, "lastName");
                    fullname = fname + " " + lname;
                }

                fullname = fullname != " " ? fullname : EasySAPCredentialsModel.EmployeeCompleteName;

                string strcbTransferType = frmITR.comboboxTransferType.SelectedValue.ToString() == "" ? "" : frmITR.comboboxTransferType.SelectedValue.ToString();
                string strcbCompany = frmITR.Company == "" ? "" : frmITR.Company == "\\" ? "" : frmITR.Company;

                sbJson.AppendLine($@"   ""Series"": ""{Convert.ToInt32(frmITR.oSeries)}"",");
                sbJson.AppendLine($@"   ""DocDate"": ""{frmITR.datePickerPostingDate.Value.ToString("yyyy-MM-dd")}"",");
                sbJson.AppendLine($@"   ""TaxDate"": ""{frmITR.datePickerDocDate.Value.ToString("yyyy-MM-dd")}"",");
                sbJson.AppendLine($@"   ""DueDate"": ""{frmITR.datePickerDueDate.Value.ToString("yyyy-MM-dd")}"",");
                //sbJson.AppendLine($@"   ""U_Remarks"": ""{frmITR.sRemarks}"",");
                sbJson.AppendLine($@"   ""U_PrepBy"": ""{fullname}"",");
                sbJson.AppendLine($@"   ""U_TransferType"": ""{strcbTransferType}"",");
                sbJson.AppendLine($@"   ""U_CompanyTIN"": ""{strcbCompany}"",");
                sbJson.AppendLine($@"   ""Address"": ""{frmITR.sAddress}"",");
                //sbJson.AppendLine($@"   ""Comments"": ""Created by EasySAP  : {sboCredentials.UserId} : {DateTime.Now} | Powered By : DIREC"",");
                sbJson.AppendLine($@"   ""Comments"": ""{frmITR.sRemarks}"",");


                if (sbJson.ToString().Contains("U_PostRem") == false && sTypeOfRequest == "POST")
                {
                    sbJson.AppendLine($@"   ""U_PostRem"": ""Transact using EasySAP | Inventory Transfer Request : {sboCredentials.UserId} : {DateTime.Now} : | Powered By : DIREC"",");
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

                if (frmITR.SalesEmployee != null && frmITR.SalesEmployee != "-No Sales Employee-" && frmITR.SalesEmployee != "")
                {
                    sbJson.AppendLine($@"   ""SalesPersonCode"": ""{Convert.ToInt32(frmITR.SalesEmployee)}"",");
                }

                //Posting of UDF
                foreach (DataGridViewRow dr in dgvUDF.Rows)
                {
                    string code = DataGridViewRowRet(dr, "Code");
                    string val = DataGridViewRowRet(dr, "Field");
                    if (code.Contains("Date") || code.Contains("date"))
                    {
                        if(val != "")
                        {
                            var date = Convert.ToDateTime(val);
                            val = date.ToString("yyyy-MM-dd");
                        }
                    }

                    if (!string.IsNullOrEmpty(val))
                    { sbJson.AppendLine($@"   ""{code}"": ""{val}"","); }
                }

                //Per line/row
                sbJson.AppendLine(@"   ""StockTransferLines"": [");

                int dgvCount = 0;

                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    min++;
                    if (Convert.ToInt32(row.Cells["Quantity"].Value) > 0 && row.Cells["From Warehouse"] != null)
                    {

                        string strSKU = string.IsNullOrEmpty(row.Cells["SKU"].Value.ToString()) ? "" : row.Cells["SKU"].Value.ToString();
                        string strComp1 = "";
                        if (row.Cells["Company"].Value != null)
                        {
                            strComp1 = row.Cells["Company"].Value.ToString();
                        }

                        string strCompany = (strComp1 == "") ? "" : GetCompanyCode(row.Cells["Company"].Value.ToString());

                        string strOpen = dgvCount == 0 ? "      {" : ",      {";
                        sbJson.AppendLine(strOpen);
                        if (sTypeOfRequest == "PATCH")
                        {
                            sbJson.AppendLine($@"       ""LineNum"": ""{row.Cells["LineNum"].Value.ToString()}"",");
                        }

                        string qryBC = $"SELECT ItemCode, ISNULL(U_ID019, '') [BrandCode] FROM OITM where ItemCode = '{row.Cells["Item No."].Value.ToString()}'";
                        DataTable dtBC = new DataTable();
                        dtBC = sapHanaAccess.Get(qryBC);
                        string sBrandC = dtBC.Rows.Count > 0 ? DataAccess.Search(dtBC, 0, "BrandCode") : "";
                        string sPriceCat = row.Cells["Info Price"].Value.ToString().Contains(".00") ? "Markdown" : "Regular";

                        sbJson.AppendLine($@"       ""ItemCode"": ""{row.Cells["Item No."].Value.ToString()}"",");
                        //sbJson.AppendLine($@"       ""Quantity"": ""{Convert.ToDouble(row.Cells["Quantity"].Value)}"",");
                        sbJson.AppendLine($@"       ""Quantity"": ""{Convert.ToDouble(row.Cells["Ordered Quantity"].Value)}"",");
                        sbJson.AppendLine($@"       ""UnitPrice"": ""{Convert.ToDouble(row.Cells["Info Price"].Value)}"",");
                        sbJson.AppendLine($@"       ""WarehouseCode"": ""{row.Cells["To Warehouse"].Value}"",");
                        sbJson.AppendLine($@"       ""FromWarehouseCode"": ""{row.Cells["From Warehouse"].Value}"",");
                        sbJson.AppendLine($@"       ""ProjectCode"": ""{frmITR.oProject}"",");
                        sbJson.AppendLine($@"       ""U_Company"": ""{strCompany}"",");
                        sbJson.AppendLine($@"       ""U_SKU"": ""{strSKU}"",");
                        sbJson.AppendLine($@"       ""U_Style"": ""{row.Cells["Style Code"].Value.ToString()}"",");
                        sbJson.AppendLine($@"       ""U_BrandName"": ""{row.Cells["Brand"].Value.ToString()}"",");
                        sbJson.AppendLine($@"       ""U_Size"": ""{row.Cells["Size"].Value.ToString()}"",");
                        sbJson.AppendLine($@"       ""U_Color"": ""{row.Cells["Color"].Value.ToString()}"",");
                        sbJson.AppendLine($@"       ""U_SortCode"": ""{row.Cells["SortCode"].Value.ToString()}"",");
                        sbJson.AppendLine($@"       ""U_AppStat"": ""P"",");
                        sbJson.AppendLine($@"       ""U_PriceCat"": ""{sPriceCat}"",");
                        sbJson.AppendLine($@"       ""U_Chain"": ""{row.Cells["Chain"].Value}"",");
                        sbJson.AppendLine($@"       ""U_ChainDescription"": ""{row.Cells["Chain Description"].Value}"",");
                        //sbJson.AppendLine($@"       ""DistributionRule2"": ""{row.Cells["Brand Code"].Value.ToString()}""");
                        sbJson.AppendLine($@"       ""DistributionRule2"": ""{sBrandC}""");
                        sbJson.AppendLine("     }");
                        //sbJson.AppendLine($@"       ""DistributionRule2"": ""{row.Cells["Brand Code"].Value.ToString()}""");
                        dgvCount++;
                    }
                    //PublicStatic.frmMain.Progress2($"Please wait until all data are uploaded. {min} out of {max}", min, max);
                }

                sbJson.AppendLine(" ]");
                sbJson.Append("}");

                string err;
                string svalue;
                string sSapModule = sTypeOfRequest == "POST" ? "InventoryTransferRequests" : "InventoryTransferRequests" + $"({ sDocEntry })";
                bool ret = serviceLayerAccess.ServiceLayer_Posting(sbJson, sTypeOfRequest, sSapModule, "DocEntry", out err, out svalue);

                if (ret)
                {
                    inventoryTransferRequestService.ClearData();
                    //inventoryTransferRequestService.LoadData();

                    string DocNum = "0";
                    string strMessage = sTypeOfRequest != "PATCH" ? "added" : "updated";
                    DocNum = sTypeOfRequest != "PATCH" ? ITRm.GetDocNum(svalue, "OWTQ") : ITRm.GetDocNum(sDocEntry.ToString(), "OWTQ");

                    //PublicStatic.frmMain.ProgressClear();
                    err = $"Document No.{DocNum} has been successfully {strMessage}";
                }

                //frmMain.NotiMsg(err, ret == true ? Color.Green : Color.Red);
                StaticHelper._MainForm.ShowMessage(err, ret == true ? false : true);
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
                //frmMain.NotiMsg(ex.Message, Color.Red);
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
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
                //frmMain.NotiMsg(ex.Message, Color.Red);
                return CompCode;
            }

        }


        //methods here can be moved to generic folders

        public string GetEmployeeCode()
        {
            string result = "";
            var query = $"SELECT empID FROM OHEM Where U_UserID = '{sboCredentials.UserId}'";
            var dt = sapHanaAccess.Get(query);

            if (dataHelper.DataTableExist(dt))
            {
                if (dataHelper.DataTableRet(dt, 0, "empID", "") != "")
                {
                    result = dt.Rows[0][0].ToString();
                }
                else
                {
                    result = "";
                    MessageBox.Show("User ID not registered to the Database.", "EasySAP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            else
            {
                result = "";
            }
            return result;
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

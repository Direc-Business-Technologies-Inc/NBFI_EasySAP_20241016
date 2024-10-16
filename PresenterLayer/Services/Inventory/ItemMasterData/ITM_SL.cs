using Context;
using DirecLayer;
using MetroFramework;
using PresenterLayer;
using PresenterLayer.Helper;
using PresenterLayer.Views;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PresenterLayer.Services
{
    public class ITM_SL
    {
        public ITM_Form form { get; set; }
        public frmItemMasterData imd { get; set; }
        SAPHanaAccess sapHana = new SAPHanaAccess();
        DataHelper helper = new DataHelper();
        public void Post()
        {
            try 
            {
                imd.btnCommand.Enabled = false;
                if (imd.dgvItemList.RowCount <= 0)
                {
                    StaticHelper._MainForm.ShowMessage("Please generate item list first!", true);
                    imd.btnCommand.Enabled = true;
                    return;
                }

                if ((MetroMessageBox.Show(StaticHelper._MainForm, "This will serve as your item master data. Do you want to continue?", LibraryHelper.AssemblyInfo.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
                {
                    StaticHelper._MainForm.Progress($"Please wait...", 10,100);
                    string Company = helper.ReadDataRow(sapHana.Get(SP.ITM_Company), 0,"",0);
                    string Brand = imd.U_ID001.Text;
                    string Category = imd.U_ID020.Text;
                    string SubCategory = imd.U_ID021.Text;
                    string Style = imd.U_ID012.Text;
                    var dgvColor = imd.dgvColor;
                    var dgvSize = imd.dgvSize;

                    int color = 0;

                    foreach (DataGridViewRow dr in dgvColor.Rows)
                    {
                        if (bool.Parse(dr.Cells[0].Value.ToString()))
                        { color++; }
                    }

                    int size = 0;

                    foreach (DataGridViewRow dr in dgvSize.Rows)
                    {
                        if (bool.Parse(dr.Cells[0].Value.ToString()))
                        { size++; }
                    }

                    if (imd.U_ID006.SelectedValue == null)
                    {
                        StaticHelper._MainForm.ShowMessage("Please select a Size Category in the Sizes Tab.", true);
                    }
                    else if (string.IsNullOrEmpty(Company) ||
                        string.IsNullOrEmpty(Brand) ||
                        string.IsNullOrEmpty(Category) ||
                        string.IsNullOrEmpty(SubCategory) ||
                        string.IsNullOrEmpty(Style) ||
                        color == 0 ||
                        size == 0)
                    { StaticHelper._MainForm.ShowMessage("Please fillup the following fields (Brand,Category,Sub Category,Color,Size,Style)", true); }
                    else
                    {
                        int max = 0;
                        foreach (DataGridViewRow dr in imd.dgvItemList.Rows)
                        {
                            if (bool.Parse(dr.Cells[0].Value.ToString()))
                            { max++; }
                        }

                        int min = 1;


                        var sbBatchJson = new StringBuilder();
                        string sOutput = "";
                        bool bOutput = true;
                        StringBuilder outputItems = new StringBuilder();

                        foreach (DataGridViewRow dr in imd.dgvItemList.Rows)
                        {
                            if (bool.Parse(dr.Cells[0].Value.ToString()))
                            {
                                var ItemCode = LibraryHelper.DataGridViewRowRet(dr, "ItemCode");
                                var sCodeBar = LibraryHelper.DataGridViewRowRet(dr, "BarCode");

                                var sPicture = LibraryHelper.DataGridViewRowRet(dr, "Path");

                                var sTo = "";

                                try
                                {
                                    FileAttributes attr = File.GetAttributes(sPicture);

                                    if (!attr.HasFlag(FileAttributes.Directory))
                                    {
                                        sTo = $"{helper.ReadDataRow(sapHana.Get("SELECT BitmapPath FROM OADP"), 0,"",0)}{ItemCode}{Path.GetExtension(sPicture)}";
                                        if (sPicture != sTo)
                                        { File.Copy(sPicture, sTo, true); }
                                    }
                                    else { sTo = ""; }
                                }
                                catch
                                { sPicture = ""; }

                                var sbJson = new StringBuilder();
                                var mode = string.Empty;

                                sbJson.AppendLine("{");

                                if (LibraryHelper.DataExist(sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_ItemCode), 1,"",0), ItemCode))))
                                {
                                    mode = "PATCH";
                                    sbJson.AppendLine($@"   ""U_Remarks"": ""Updated by EasySAP | Item Master Data : {SboCred.UserID} : {ItemCode} : {DateTime.Now} | Powered By : DIREC"",");
                                }
                                else
                                {
                                    mode = "POST";
                                    sbJson.AppendLine($@"   ""ItemCode"": ""{ItemCode}"",");
                                    sbJson.AppendLine($@"   ""U_Remarks"": ""Created by EasySAP | Item Master Data : {SboCred.UserID} : {ItemCode} : {DateTime.Now} | Powered By : DIREC"",");
                                }

                                var ItemName = LibraryHelper.DataGridViewRowRet(dr, "ItemName");
                                sbJson.AppendLine($@"   ""ItemName"": ""{(ItemName.Length <= 100 ? ItemName : ItemName.Substring(0, 100)).ToUpper()}"",");

                                var InventoryItem = imd.InvntItem.Checked ? "Y" : "N";
                                var SalesItem = imd.SellItem.Checked ? "Y" : "N";
                                var PurchaseItem = imd.PrchseItem.Checked ? "Y" : "N";

                                var ColorCode = LibraryHelper.DataGridViewRowRet(dr, "Color");
                                var SizeCode = LibraryHelper.DataGridViewRowRet(dr, "Size");
                                var ItmsGrpCod = imd.ItmsGrpCod.SelectedValue.ToString();

                                sbJson.AppendLine($@"   ""ItemsGroupCode"": ""{ItmsGrpCod}"",");

                                //if (string.IsNullOrEmpty(sCodeBar) == false)
                                //{ sbJson.AppendLine($@"   ""BarCode"": ""{sCodeBar}"","); }

                                sbJson.AppendLine($@"   ""BarCode"": ""{(string.IsNullOrEmpty(sCodeBar) ? ItemCode : sCodeBar)}"",");

                                sbJson.AppendLine($@"   ""InventoryItem"": ""{InventoryItem}"",");
                                sbJson.AppendLine($@"   ""DefaultCountingUnit"": ""{InventoryItem}"",");
                                sbJson.AppendLine($@"   ""SalesItem"": ""{SalesItem}"",");
                                sbJson.AppendLine($@"   ""PurchaseItem"": ""{PurchaseItem}"",");

                                var InvnTryUOM = GetUomID(imd.InvntryUom.Text);
                                if (string.IsNullOrEmpty(InvnTryUOM) == false)
                                {
                                    sbJson.AppendLine($@"   ""InventoryUoMEntry"": {InvnTryUOM},");
                                    sbJson.AppendLine($@"   ""DefaultCountingUoMEntry"": {InvnTryUOM},");
                                }

                                var SalUnitMsr = GetUomID(imd.SalUnitMsr.Text);
                                if (string.IsNullOrEmpty(SalUnitMsr) == false)
                                { sbJson.AppendLine($@"   ""DefaultSalesUoMEntry"": {SalUnitMsr},"); }

                                var BuyUnitMsr = GetUomID(imd.BuyUnitMsr.Text);
                                if (string.IsNullOrEmpty(BuyUnitMsr) == false)
                                { sbJson.AppendLine($@"   ""DefaultPurchasingUoMEntry"": {BuyUnitMsr},"); }


                                sbJson.AppendLine($@"   ""Mainsupplier"": ""{imd.CardCode.Text}"",");

                                var sGLMethod = GetItemGrpDetails(ItmsGrpCod, 0);
                                if (string.IsNullOrEmpty(sGLMethod) == false)
                                { sbJson.AppendLine($@"   ""GLMethod"": ""{sGLMethod}"","); }

                                var sEvalSys = GetItemGrpDetails(ItmsGrpCod, 1);
                                if (string.IsNullOrEmpty(sEvalSys) == false)
                                { sbJson.AppendLine($@"   ""CostAccountingMethod"": ""{sEvalSys}"","); }

                                var sManageBy = GetItemGrpDetails(ItmsGrpCod, 2);
                                if (string.IsNullOrEmpty(sManageBy) == false && sManageBy != "0")
                                { sbJson.AppendLine(GetManageBy(sManageBy)); }
                                
                                if (!string.IsNullOrEmpty(sTo))
                                { sbJson.AppendLine($@"   ""Picture"": ""{Path.GetFileName(sTo)}"","); }

                                sbJson.AppendLine($@"   ""Properties2"": ""Y"",");
                                sbJson.AppendLine($@"   ""IssueMethod"": ""B"",");

                                if (string.IsNullOrEmpty(imd.UserText.Text) == false)
                                { sbJson.AppendLine($@"   ""User_Text"": ""{imd.UserText.Text}"","); }

                                //Issue Logs - Moved here 01/14/2020
                                if (string.IsNullOrEmpty(imd.BuyQPC.Text) == false)
                                { sbJson.AppendLine($@"       ""OrderMultiple"": {imd.BuyQPC.Text},"); }

                                sbJson.AppendLine($@"   ""U_ID001"": ""{Brand}"",");
                                sbJson.AppendLine($@"   ""U_ID002"": ""{imd.U_ID002.Text}"",");
                                sbJson.AppendLine($@"   ""U_ID003"": ""{imd.U_ID003.Text}"",");
                                sbJson.AppendLine($@"   ""U_ID004"": ""{imd.U_ID004.Text}"",");
                                sbJson.AppendLine($@"   ""U_ID005"": ""{imd.U_ID005.Text}"",");

                                if (imd.U_ID006.SelectedValue != null)
                                {
                                    sbJson.AppendLine($@"   ""U_ID006"": ""{imd.U_ID006.SelectedValue.ToString()}"",");
                                }

                                sbJson.AppendLine($@"   ""U_ID007"": ""{LibraryHelper.DataGridViewRowRet(dr, "SizeName")}"",");
                                sbJson.AppendLine($@"   ""U_ID008"": ""{SizeCode}"",");
                                sbJson.AppendLine($@"   ""U_ID009"": ""{LibraryHelper.DataGridViewRowRet(dr, "ParentCode")}"",");
                                sbJson.AppendLine($@"   ""U_ID010"": ""{ColorCode}"",");
                                sbJson.AppendLine($@"   ""U_ID011"": ""{LibraryHelper.DataGridViewRowRet(dr, "ChildName")}"",");

                                sbJson.AppendLine($@"   ""U_ID012"": ""{Style}"",");
                                sbJson.AppendLine($@"   ""U_ID013"": ""{imd.U_ID013.Text}"",");
                                sbJson.AppendLine($@"   ""U_ID014"": ""{imd.U_ID014.Text}"",");
                                sbJson.AppendLine($@"   ""U_ID015"": ""{imd.U_ID015.Text}"",");
                                sbJson.AppendLine($@"   ""U_ID016"": ""{imd.U_ID016.Text}"",");
                                sbJson.AppendLine($@"   ""U_ID017"": ""{imd.U_ID017.Text}"",");

                                sbJson.AppendLine($@"   ""U_ID019"": ""{Brand}"",");
                                sbJson.AppendLine($@"   ""U_ID020"": ""{Category}"",");
                                sbJson.AppendLine($@"   ""U_ID021"": ""{SubCategory}"",");
                                sbJson.AppendLine($@"   ""U_ID022"": ""{ColorCode}"",");
                                sbJson.AppendLine($@"   ""U_ID023"": ""{Company}{Brand}{Style}{Category}{SubCategory}{ColorCode}{SizeCode}"",");
                                sbJson.AppendLine($@"   ""U_ID025"": ""{imd.U_ID025.Text}"",");

                                var dgvUDF = imd.form.frmUDF.dgvUDF;

                                foreach (DataGridViewRow drUDF in dgvUDF.Rows)
                                {
                                    string code = LibraryHelper.DataGridViewRowRet(drUDF, "Code");
                                    string val = LibraryHelper.DataGridViewRowRet(drUDF, "Field");

                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        if (code.Contains("Date"))
                                        {
                                            string date = Convert.ToDateTime(val).ToString("yyyy-MM-dd");
                                            sbJson.AppendLine($@"   ""{code}"": ""{date}"",");
                                        }
                                        else
                                        {
                                            sbJson.AppendLine($@"   ""{code}"": ""{val}"",");
                                        }
                                    }
                                }

                                sbJson.AppendLine($@"   ""ItemPreferredVendors"": [");
                                sbJson.AppendLine("     {");
                                sbJson.AppendLine($@"       ""BPCode"": ""{imd.CardCode.Text}""");
                                sbJson.AppendLine("     }");
                                sbJson.AppendLine(" ],");

                                if (ItmsGrpCod.Equals("100"))
                                {
                                    DataTable dt = sapHana.Get(helper.ReadDataRow(sapHana.Get(SP.ITM_UoMConcession), 1,"",0));

                                    int whscnt = 0;

                                    sbJson.AppendLine($@"   ""ItemWarehouseInfoCollection"": [");
                                    foreach (DataRow drwhs in dt.Rows)
                                    {
                                        whscnt++;
                                        sbJson.AppendLine("     {");
                                        sbJson.AppendLine($@"       ""MinimalStock"": ""-9999"",");
                                        sbJson.AppendLine($@"       ""WarehouseCode"": ""{LibraryHelper.DataRowRet(drwhs, "WhsCode","01")}""");

                                        string sWhsCode = LibraryHelper.DataRowRet(drwhs, "WhsCode", "");
                                        if (GetMaxQuantity(sWhsCode) != "")
                                        {
                                            sbJson.AppendLine($@",       ""U_MaxROQuantity"": ""{GetMaxQuantity(sWhsCode)}""");
                                        }

                                        if (whscnt < dt.Rows.Count)
                                        { sbJson.AppendLine("     },"); }
                                    }

                                    sbJson.AppendLine("     }");
                                    sbJson.AppendLine(" ],");
                                }

                                if (string.IsNullOrEmpty(imd.SalQPC.Text) == false || string.IsNullOrEmpty(imd.BuyQPC.Text) == false)
                                {

                                    sbJson.AppendLine($@"   ""ItemUnitOfMeasurementCollection"": [");
                                    string GetUom, GetUomType, GetUomTypeSL, GetQPC;

                                    for (int x = 0; x < 2; x++)
                                    {

                                        if (string.IsNullOrEmpty(imd.SalQPC.Text))
                                        {
                                            continue;
                                        }

                                        GetUomTypeSL = x == 0 ? "iutSales" : "iutPurchasing";
                                        GetUomType = x == 0 ? "S" : "P";
                                        GetUom = x == 0 ? imd.SalUnitMsr.Text : imd.BuyUnitMsr.Text;

                                        //Issue Logs - removed 01/14/2020
                                        //GetQPC = x == 0 ? imd.SalQPC.Text : imd.BuyQPC.Text;

                                        var dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_GetQuantityPerCarton), 1,"",0), GetUomType, ItemCode, GetUom));

                                        //int QPCcnt = 0;

                                        if (dt.Rows.Count > 0)
                                        {
                                            foreach (DataRow drQPC in dt.Rows)
                                            {
                                                string strOpen = x == 0 ? "      {" : ",      {";
                                                string strUomEntry = drQPC["UomEntry"].ToString();

                                                sbJson.AppendLine(strOpen);
                                                sbJson.AppendLine($@"       ""UoMType"": ""{GetUomTypeSL}"",");
                                                sbJson.AppendLine($@"       ""UoMEntry"": {strUomEntry},");
                                                sbJson.AppendLine($@"       ""ItemUoMPackageCollection"": [");
                                                sbJson.AppendLine("     {");
                                                sbJson.AppendLine($@"       ""UoMType"": ""{GetUomTypeSL}"",");
                                                sbJson.AppendLine($@"       ""UoMEntry"": {strUomEntry},");
                                                sbJson.AppendLine($@"       ""PackageTypeEntry"": 1,");
                                                //Issue Logs - Moved to upper part of code 01/14/2020
                                                //sbJson.AppendLine($@"       ""QuantityPerPackage"": {GetQPC} ");
                                                sbJson.AppendLine("     }");
                                                sbJson.AppendLine(" ] ");

                                                //QPCcnt++;
                                            }
                                            sbJson.AppendLine("     }");
                                        }
                                        else
                                        {
                                            string strOpen = x == 0 ? "      {" : ",      {";
                                            string strUomEntry = GetUomID(GetUom);

                                            sbJson.AppendLine(strOpen);
                                            sbJson.AppendLine($@"       ""UoMType"": ""{GetUomTypeSL}"",");
                                            sbJson.AppendLine($@"       ""UoMEntry"": {strUomEntry},");
                                            sbJson.AppendLine($@"       ""ItemUoMPackageCollection"": [");
                                            sbJson.AppendLine("     {");
                                            sbJson.AppendLine($@"       ""UoMType"": ""{GetUomTypeSL}"",");
                                            sbJson.AppendLine($@"       ""UoMEntry"": {strUomEntry},");
                                            sbJson.AppendLine($@"       ""PackageTypeEntry"": 1,");
                                            //Issue Logs 01/14/2020
                                            //sbJson.AppendLine($@"       ""QuantityPerPackage"": {GetQPC} ");
                                            sbJson.AppendLine("     }");
                                            sbJson.AppendLine(" ] ");
                                            sbJson.AppendLine("     }");
                                        }
                                    }
                                    sbJson.AppendLine(" ],");
                                }

                                var strSRP = LibraryHelper.DataGridViewRowRet(dr, "SRP");
                                var strOutrightPrice = LibraryHelper.DataGridViewRowRet(dr, "OutrightPrice");

                                sbJson.AppendLine($@"   ""ItemPrices"": [");
                                sbJson.AppendLine("     {");
                                sbJson.AppendLine($@"       ""PriceList"": 1,");
                                //sbJson.AppendLine($@"       ""Price"": ""{imd.Price.Text}""");
                                sbJson.AppendLine($@"       ""Price"": ""{strSRP}""");
                                sbJson.AppendLine("     },");
                                sbJson.AppendLine("     {");
                                sbJson.AppendLine($@"       ""PriceList"": 4,");
                                //sbJson.AppendLine($@"       ""Price"": ""{imd.OutrightPrice.Text}""");
                                sbJson.AppendLine($@"       ""Price"": ""{strOutrightPrice}""");
                                sbJson.AppendLine("     }");
                                sbJson.AppendLine(" ]");
                                sbJson.AppendLine("}");
                                //sbBatchJson.AppendLine(SAPHana.SL_BatchBody("ItemMasterDataUploading", mode, mode == SAPHana.SL_Mode.PATCH ? $"Items('{ItemCode}')" : "Items", sbJson.ToString()));

                                if (mode == "PATCH")
                                { sOutput = SAPHana.PostPatch_ItemMasterData(sbJson, ItemCode); }
                                else
                                { sOutput = SAPHana.PostPatch_ItemMasterData(sbJson); }

                                if (sOutput.Contains("error"))
                                {
                                    StaticHelper._MainForm.ShowMessage(sOutput, true);
                                    bOutput = false;
                                    break;
                                }
                                else
                                {
                                    outputItems.Append($"{ItemCode}^");
                                    StaticHelper._MainForm.Progress($"Please wait until all items are uploaded/updated. {min} out of {max}.", min++, max);
                                }
                            }
                        }


                        if (bOutput)
                        {
                            ITM_Preview prev = new ITM_Preview();
                            prev.imd = imd;
                            prev.Form_Load();
                            form.LoadForm();
                            form.ClearUDF();
                        }
                        else
                        {
                            foreach (var row in outputItems.ToString().Split('^'))
                            {
                                if (!string.IsNullOrEmpty(row))
                                { SAPHana.Delete_ItemMasterData(row); }
                            }
                        }

                        StaticHelper._MainForm.ProgressClear();
                    }
                }

            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
            imd.btnCommand.Enabled = true;
        }

        string GetUomID(string sUomCode)
        {
            string result = "";
            result = helper.ReadDataRow(sapHana.Get($"SELECT UomEntry FROM OUOM WHERE UomCode = '{sUomCode}'"), 0,"",0);
            return string.IsNullOrEmpty(result) ? "0" : result;
        }

        string GetItemGrpDetails(string ItmsGrpCod, int col)
        {
            string result = "";
            result = helper.ReadDataRow(sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_GetItemGrpDetail), 1,"",0), ItmsGrpCod)),col,"",0);
            return string.IsNullOrEmpty(result) ? "" : result;
        }

        string GetManageBy(string ManByValue)
        {
            string result = "";

            result = ManByValue == "1" ? $@"   ""ManageSerialNumbers"": ""N"", ""ManageBatchNumbers"": ""Y"", " : $@" ""ManageBatchNumbers"": ""N"", ""ManageSerialNumbers"": ""N"", ";

            return result;
        }

        string GetMaxQuantity(string sBPCode)
        {
            string result = "";
            sBPCode = sBPCode.Contains("C-") ? sBPCode : "C-" + sBPCode;
            result = helper.ReadDataRow(sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.ITM_GetMaxQuantity), 1,"",0)
                     , sBPCode, imd.U_ID001.Text, imd.U_ID020.Text, imd.U_ID021.Text, imd.U_ID002.Text
                     , imd.U_ID003.Text, imd.U_ID006.SelectedValue.ToString())),0,"",0);
            result = string.IsNullOrEmpty(result) ? "" : Convert.ToInt32(result).ToString();
            return result;
        }
    }
}

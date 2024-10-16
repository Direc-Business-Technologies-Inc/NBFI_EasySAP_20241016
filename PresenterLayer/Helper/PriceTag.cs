using Seagull.BarTender.Print;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using PresenterLayer.Views.Main;
using PresenterLayer.Helper;
using PresenterLayer.Services;

namespace DirecLayer._02_Form.MVP.Presenters.PriceTag
{
    public class PriceTag
    {
        private static bool CheckIfConnected(string path)
        {
            var isConnected = false;

            try
            {
                var sys = new SystemSettings();
                isConnected = sys.FolderExist(path);
            }
            catch
            {
            }
            return isConnected;
        }
        //public static void ImageFile(DataGridView DgvItem, string BPcode, string printer)
        public static void ImageFile(DataGridView DgvItem, string BPcode, string DeliveryDate, string DocEntry)
        {
            try
            {
                int iRow = 0;
                string iColumn;
                bool iCanceled;
                string tmpFile = Path.GetTempFileName();
                string path = "";
                int oCnt = 0;
                int iCntChainDesc = 0;
                int iCntSuccess = 0;
                int iCntCheckPath = 0;
                var oPath = ""; //@"E:\NEW BARBIZON FASHION INC\ALTURAS\Regular.btw";
                string strDeliveryDate2 = Convert.ToDateTime(DeliveryDate).ToString("yyyy/MM/dd");
                var helper = new DataHelper();
                var sapHana = new SAPHanaAccess();
                using (Engine btEngine = new Engine(true))
                {
                    // List<DataGridViewRow> list = DgvItem.Rows.Cast<DataGridViewRow>().Where(k => Convert.ToBoolean(k.Cells[0].Value) == true).ToList();
                    if (DgvItem.Columns.Count > 1)
                    {
                        oCnt = 0;
                        int min = 0;
                        int max = DgvItem.Rows.Count - 1;

                        foreach (DataGridViewRow row in DgvItem.Rows)
                        {
                            min++;
                            if (row.Cells["Item No."].Value != null && row.Cells["Item No."].Value.ToString() != "")
                            {
                                if (row.Cells["Chain Description"].Value != null && row.Cells["Chain Description"].Value.ToString() != "")
                                {
                                    string letters = "ABCDEFGHIJKL";
                                    string strChainDesc = row.Cells["Chain Description"].Value.ToString();
                                    //LabelFormatDocument btFormat = btEngine.Documents.Open(lb);

                                    string buf = BarcodeCtrl.GetBarcodeHeader(); // header

                                    string bufRH = buf, bufMH = buf;

                                    string oItemCode = row.Cells["Item No."].Value.ToString();
                                    //string oItemCode = "1150080103140001";
                                    string oItemDesc = row.Cells["Item Description"].Value.ToString();
                                    DataTable GetBarcodeFromOITMandPO = sapHana.Get(SP.BP_BarcodeAll_BarcodeFromOITMandPOforUPC);
                                    string qry = string.Format(helper.ReadDataRow(GetBarcodeFromOITMandPO, 1, "", 0), BPcode, "POR1", oItemCode, "OPOR", DocEntry);
                                    DataTable dtCampDetails = sapHana.Get(qry);//DataAccess.Select(DataAccess.conStr("HANA"), qry/*, StaticHelper._MainForm*/);

                                    if (helper.DataTableExist(dtCampDetails))
                                    {
                                        string sPrice = dtCampDetails.Rows[0]["RegularPrice"].ToString();
                                        Double SRP = Convert.ToDouble(sPrice);
                                        oPath = dtCampDetails.Rows[0]["CPN2_U_Path"].ToString();

                                        //MessageBox.Show($"{dtCampDetails.Rows.Count} - number of rows; {BPcode}, POR1,{oItemCode},OPOR,{DocEntry};");
                                        //Comment for testing
                                        //oPath = "D:" + oPath.Replace("\\NBFIHVMSVR01", "");

                                        if (CheckIfConnected(oPath))
                                        {
                                            string AG3 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMyy").Remove(2, 1);
                                            string SC1 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yyMM");
                                            string strSC2 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yyM").Substring(2);
                                            string SC2 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yyMM").Remove(2, 2) + letters.Substring(Convert.ToInt32(strSC2) - 1, 1);
                                            string SC3 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yMM").Remove(0, 1);
                                            string SC4 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MM/yy");
                                            //string SC5 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMy");
                                            string strColor1 = dtCampDetails.Rows[0]["Color"].ToString();
                                            string strColor2 = strColor1 == "" ? "" : strColor1.Length > 3 ? strColor1.Substring(0, 3) : strColor1;
                                            string strDept = dtCampDetails.Rows[0]["DeptCode"].ToString();
                                            string strSubDept = dtCampDetails.Rows[0]["SubDept"].ToString();
                                            int oItemCodeCnt = oItemCode.Count() - 4;
                                            string ItemType = dtCampDetails.Rows[0]["RegularPrice"].ToString().Contains(".00") ? "M" : "R";
                                            string strItemType2 = ItemType == "R" ? "REG" : "X";
                                            string MonthYear = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMMy");
                                            string ReceiveCode = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MM/y");
                                            string strSubClass = dtCampDetails.Rows[0]["SubClass"].ToString();
                                            string strDeliveryDate = dtCampDetails.Rows[0]["DeliveryDate"].ToString() != "" ? Convert.ToDateTime(dtCampDetails.Rows[0]["DeliveryDate"].ToString()).ToString("yyyyMMdd") : "";
                                            string SkuType = dtCampDetails.Rows[0]["SkuType"].ToString();
                                            string strSkuType = SkuType.Substring(0, 1);
                                            string strSKuType2 = "";
                                            string strItemDescOrig = dtCampDetails.Rows[0]["IMD_ItemDesc"].ToString();
                                            string strItemDesc = strItemDescOrig.Length > 15 ? strItemDescOrig.Substring(0, 15) : strItemDescOrig;
                                            string strUPC = dtCampDetails.Rows[0]["UPC"].ToString();
                                            string strCPN2_BarOption = strUPC == "" ? dtCampDetails.Rows[0]["CPN2_ItemCode"].ToString() : strUPC;
                                            string strSKU = dtCampDetails.Rows[0]["SKU"].ToString();
                                            string strCPN2_SKU = strSKU.Contains("-") ? strSKU.Remove(strSKU.IndexOf("-"), 2) : strSKU;
                                            string strItemDesc_Long = dtCampDetails.Rows[0]["ItemDesc"].ToString();
                                            string strItemDesc_Trim = strItemDesc_Long.Length > 20 ? strItemDesc_Long.Substring(0, 19) : strItemDesc_Long;
                                            string strIMDSubCat = dtCampDetails.Rows[0]["IMD_SubCat"].ToString();
                                            string strIMDCat = dtCampDetails.Rows[0]["IMD_Cat"].ToString();
                                            string strPriceFormatZero = SRP.ToString("000000.00").Replace(".", "");
                                            string strIMDCatCode = dtCampDetails.Rows[0]["OITM_CatCode"].ToString();
                                            string strIMDSubCatCode = dtCampDetails.Rows[0]["OITM_SubCatCode"].ToString();
                                            string strPromoIndi = ItemType == "R" ? "REGULAR" : "SALE";
                                            string strAG4 = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMyy");
                                            string strMonthValue = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("MMM").ToUpper();
                                            string strDateValue = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("dd");
                                            string strYearValue = Convert.ToDateTime(dtCampDetails.Rows[0]["AgeCode"].ToString()).ToString("yyyy");

                                            if (SkuType == "Outright")
                                            {
                                                strSKuType2 = "01";
                                            }
                                            else if (SkuType == "Concession")
                                            {
                                                strSKuType2 = "02";
                                            }
                                            else
                                            {
                                                strSKuType2 = "03";
                                            }

                                            string bufRow = $"\"0\"" +               //AgeCode1
                                                            $",\"0\"" +              //AgeCode2
                                                            $",\"{AG3}\"" +          //AgeCode3
                                                            $",\"{dtCampDetails.Rows[0]["BrandName"].ToString()}\"" +
                                                            $",\"{Convert.ToDouble(dtCampDetails.Rows[0]["BeforePrice"]).ToString("#,##0.00")}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["Class"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CncCode"].ToString()}\"" +             //CNCcode
                                                            $",\"{strColor2}\"" +                                               //Color
                                                            $",\"0\"" +                                                         //ConCode
                                                            $",\"{dtCampDetails.Rows[0]["DateFrom"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["DateTo"].ToString()}\"" +
                                                            $",\"{strDeliveryDate}\"" +                                         //DeliveryDate or DeliveryData
                                                            $",\"{strDept}\"" +                                                 //DeptCode
                                                            $",\"{dtCampDetails.Rows[0]["DeptName"].ToString()}\"" +
                                                            $",\"{strDept}-{strSubDept}\"" +                                    //Dept_SubDept
                                                            $",\"0\"" +                                                         //DiscountFreeText
                                                            $",\"{strDept}\"" +                                                 //Division
                                                            $",\"0\"" +                                                         //HierarchyCode
                                                            $",\"{oItemCode.Substring(oItemCodeCnt, 4)}\"" +                    //IntSKU
                                                            $",\"{strItemDesc_Long}\"" +
                                                            $",\"{ItemType}\"" +
                                                            $",\"{strItemType2}\"" +
                                                            $",\"{SRP.ToString("#,##0.00")}\"" +                        //MarkdownPrice
                                                            $",\"{MonthYear}\"" +                                       //Month_Year
                                                            $",\"{SRP.ToString("#,##0.00")}\"" +                        //Orig Price
                                                            $",\"{strSKU}\"" +                                          //ORIN
                                                            $",\"{SRP.ToString("#,##0.00")}\"" +                        //Price
                                                            $",\"{strSKU}\"" +                                          //ProductNo
                                                            $",\"{ItemType}\"" +                                        //PromoIndicator
                                                            $",\"{ReceiveCode}\"" +                                     //ReceiveCode
                                                            $",\"{SRP.ToString("#,##0.00")}\"" +                        //RegularPrice
                                                            $",\"{SRP.ToString("#,##0.00")}\"" +                        //RetailPrice
                                                            $",\"{SC1}\"" +                                             //SeasonCode1
                                                            $",\"{SC2}\"" +                                             //SeasonCode2
                                                            $",\"{SC3}\"" +                                             //SeasonCode3
                                                            $",\"{SC4}\"" +                                             //SeasonCode4
                                                            $",\"{dtCampDetails.Rows[0]["SalesUOM"].ToString()}\"" +    //SellingUOM
                                                            $",\"{dtCampDetails.Rows[0]["Size"].ToString()}\"" +
                                                            $",\"{strSKU}\"" +
                                                            $",\"{SkuType}\"" +
                                                            $",\"{strSKuType2}\"" +
                                                            $",\"0\"" +                                                   //StoreName
                                                            $",\"{dtCampDetails.Rows[0]["SubClass"].ToString()}\"" +
                                                            $",\"{strSubDept}\"" +                                          //SubDept
                                                            $",\"{strSubDept}{strSubClass}\"" +                             //SubDept_Class
                                                            $",\"{dtCampDetails.Rows[0]["SupplierCode"].ToString()}\"" +    //SupplierCode
                                                            $",\"{dtCampDetails.Rows[0]["SalesUOM"].ToString()}\"" +        //UOM
                                                            $",\"{strUPC}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["SupplierCode"].ToString()}\"" +      //VendorCode
                                                            $",\"{oItemCode}\"" +                                             //VendorPartNo
                                                            $",\"{strSkuType}\"" +                                            //VendorType
                                                            $",\"{dtCampDetails.Rows[0]["Style"].ToString()}\"" +             //VPN
                                                            $",\"{dtCampDetails.Rows[0]["IMD_CodeBars"].ToString()}\"" +      //IMD_CodeBars
                                                            $",\"{dtCampDetails.Rows[0]["IMD_Color"].ToString()}\"" +         //IMD_Color
                                                            $",\"{dtCampDetails.Rows[0]["IMD_Size"].ToString()}\"" +          //IMD_Size
                                                            $",\"{dtCampDetails.Rows[0]["IMD_Style"].ToString()}\"" +         //IMD_Style
                                                            $",\"{strItemDesc}\"" +                                           //IMD_ItemDesc
                                                            $",\"{dtCampDetails.Rows[0]["IMD_BrandName"].ToString()}\"" +     //IMD_BrandName
                                                            $",\"{strCPN2_BarOption}\"" +                                     //CPN2_BarOption
                                                            $",\"{strCPN2_SKU}\"" +                                           //CPN2_SKU
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_VPN"].ToString()}\"" +          //CPN2_VPN
                                                            $",\"{strIMDSubCat}\"" +                                          //IMD_SubCat
                                                            $",\"{strIMDCat}\"" +                                             //IMD_Cat
                                                            $",\"{strItemDesc_Trim}\"" +                                      //ItemDesc_Trim
                                                            $",\"{strIMDCat + strIMDSubCat}\"" +                              //IMD_CatSubCat
                                                            $",\"{dtCampDetails.Rows[0]["CPN3_BrandCode"].ToString()}\"" +    //CPN3_BrandCode
                                                            $",\"{dtCampDetails.Rows[0]["IMD_DeptName"].ToString()}\"" +      //IMD_DeptName
                                                            $",\"{dtCampDetails.Rows[0]["CPN3_DeptCode"].ToString()}\"" +     //CPN3_DeptCode
                                                            $",\"{strItemDescOrig}\"" +                                       //IMD_ItemDescFull
                                                            $",\"{strPriceFormatZero}\"" +                                    //PriceFormatZero
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_LongItemDesc"].ToString()}\"" +     //CPN2_LongItemDesc
                                                            $",\"{dtCampDetails.Rows[0]["OITM_SizeShortName"].ToString()}\"" +    //OITM_SizeShortName
                                                            $",\"{dtCampDetails.Rows[0]["OITM_CodeBars"].ToString()}\"" +         //OITM_CodeBars
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_Division"].ToString()}\"" +      //CPN2_Division
                                                            $",\"{strIMDCatCode}\"" +                                          //OITM_CatCode
                                                            $",\"{strIMDSubCatCode}\"" +                                       //OITM_SubCatCode
                                                            $",\"{strPromoIndi}\"" +                                           //PromoIndicator2
                                                            $",\"{strAG4}\"" +                                                 //AgeCode4
                                                            $",\"{dtCampDetails.Rows[0]["SampleCode"].ToString()}\"" +         //SampleCode
                                                            $",\"{strMonthValue}\"" +                                          //MonthValue
                                                            $",\"{strDateValue}\"" +                                           //DateValue
                                                            $",\"{strYearValue}\"" +                                           //YearValue
                                                            $",\"{dtCampDetails.Rows[0]["OITM_BrandName"].ToString()}\"" +     //OITM_BrandName
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Section"].ToString()}\"" +       //OITM_Section
                                                            $",\"{dtCampDetails.Rows[0]["CPN3_VendorID"].ToString()}\"" +    //CPN3_VendorID
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Department"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Brand2"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Dimension3"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Dimension4"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Dimension5"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_SubDepartment"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_SizeCategory"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Size2"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_ParentColor"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Class"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_SubClass"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Packaging"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Specification"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Collection"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_BrandCode"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_SortCode"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_ItemClass"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_StyleName"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Remarks"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OITM_Designer"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_AliasName"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_AddID"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_VatIdUnCmp"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_Notes"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_GlblLocNum"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_Series"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_GroupCode"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_Department"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_Dimension2"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_Dimension3"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_Dimension4"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_Dimension5"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_OrderClassification"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_StoreClassification"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_DeliveryClassification"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_Region"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["OCRD_ClassificationForSupplies"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["RDR1_U_SampleCode"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["RDR1_U_UnitPricePerPiece"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["ORDR_DocDueDate"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["ORDR_CancelDate"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_CpnLineNum"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_CpnNo"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_ItemCode"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_ItemGrp"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_ItemName"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_ItemType"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_LogIns"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_BarCodeColor"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Desc1"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Desc2"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc1"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc2"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc3"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc4"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Disc5"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_ItemType"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_MDPrice"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_OcpnArtNo"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_OldSKU"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_OldUPC"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_PartNo"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Path"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_PathMDPrice"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_PriceCat"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Pricetag"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_PricetagEquivalent"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_RPrice"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_SC"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_SeasonCode"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_Section"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_StyleCode"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_U_UPC"].ToString()}\"" +
                                                            $",\"{dtCampDetails.Rows[0]["CPN2_VisOrder"].ToString()}\"\r\n";


                                            //string sTempPath = oPath.Replace("\\NBFIHVMSVR01", "");
                                            //sTempPath = "D:" + sTempPath;

                                            string sTempPath = oPath;

                                            string sBarType = "";

                                            if (ItemType == "R")
                                            {
                                                sTempPath += "\\Regular.btw";
                                                sBarType = "Regular";
                                            }
                                            else
                                            {
                                                sTempPath += "\\Markdown.btw";
                                                sBarType = "Markdown";
                                            }

                                            StaticHelper._MainForm.Progress($"Data preparation {oCnt} out of {max}", oCnt, max);

                                            LabelFormatDocument btFormat = btEngine.Documents.Open(sTempPath);

                                            File.WriteAllText(tmpFile, $"{buf}{bufRow}");

                                            Seagull.BarTender.Print.Database.TextFile tf = new Seagull.BarTender.Print.Database.TextFile(btFormat.DatabaseConnections[0].Name);
                                            tf.FileName = tmpFile;
                                            btFormat.DatabaseConnections.SetDatabaseConnection(tf);
                                            btFormat.PrintSetup.ReloadTextDatabaseFields = true;
                                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                                            string sPath = oPath += $"\\Pricetag\\{DocEntry}\\";
                                            string sFileName = sBarType + "_" + oItemCode + ".jpg";

                                            if (!Directory.Exists(sPath))
                                            {
                                                Directory.CreateDirectory(sPath);
                                            }
                                            //MessageBox.Show($"Save Path - {sPath}");

                                            btFormat.ExportPrintPreviewToFile(sPath, sFileName, ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, new Resolution(300), System.Drawing.Color.White, OverwriteOptions.Overwrite, false, false, out Messages sample);
                                            CheckPath(sPath, sFileName);
                                            iCntSuccess++;
                                        }
                                        else
                                        {
                                            iCntCheckPath++;
                                        }
                                    }

                                }
                                else
                                {
                                    iCntChainDesc++;
                                }

                                StaticHelper._MainForm.Progress($"Please wait until all images are generated. {min} out of {max}", min, max);
                            }
                        }

                        btEngine.Dispose();
                        btEngine.Stop();

                        if (iCntCheckPath > 0)
                        {
                            StaticHelper._MainForm.ShowMessage($"{iCntCheckPath} image(s) did not generate. Please check shared Price Tag shared folder.", true);
                        }
                        if (iCntChainDesc > 0)
                        {
                            StaticHelper._MainForm.ShowMessage($"{iCntChainDesc} Chain Description(s) is/are blank. Unable to generate image(s).", true);
                        }
                        if (iCntSuccess > 0)
                        {
                            StaticHelper._MainForm.ShowMessage($"{iCntSuccess} items image(s) generated successfully.");
                        }
                        if (iCntSuccess == 0 && iCntCheckPath == 0)
                        {
                            StaticHelper._MainForm.ShowMessage($"No item(s) image(s) generated. Please check items if its existing in SAP - Campaign.", true);
                        }
                    }
                }
            }
            catch (FormatException ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }


        private static void CheckPath(string sPath, string sFileName)
        {
            List<string> sFileNameOrig = sFileName.Split('.').ToList<string>();
            string sNewFilePath = sPath + "\\" + sFileName;

            DirectoryInfo d = new DirectoryInfo(sPath);
            FileInfo[] Files = d.GetFiles("*.jpg");
            foreach (FileInfo file in Files)
            {
                string fileName = file.Name;

                List<string> sCheckFileName = fileName.Split('_').ToList<string>();
                string sLastDigit = "1";

                if (sCheckFileName[1].Replace(".jpg", "").Length == 17)
                {
                    sLastDigit = sCheckFileName[1].Substring(16, 1);
                }

                string sRenamedFile = sFileNameOrig[0] + sLastDigit + ".jpg";
                string sFilePath = sPath + "\\" + sRenamedFile;

                if (fileName.Contains(sRenamedFile))
                {
                    File.Copy(sFilePath, sNewFilePath, true);
                    File.Delete(sFilePath);
                }
            }

        }
    }
}

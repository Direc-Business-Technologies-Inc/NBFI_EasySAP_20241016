using System;
using PresenterLayer.Helper;
using DirecLayer;

namespace PresenterLayer.Services
{
    class BarcodeCtrl
    {

        public static bool CheckNoMaintenance(string BPCode)
        {
            bool result = false;

            try
            {
                var sapHana = new SAPHanaAccess();
                var helper = new DataHelper();

                var GetCheckBPCode = sapHana.Get(SP.BP_BarcodeAll_CheckBPCode);
                string sCheckBPCodeQry = string.Format(helper.ReadDataRow(GetCheckBPCode, 1, "", 0) , BPCode);

                if (sapHana.Get(sCheckBPCodeQry).Rows.Count == 0)
                {
                    var GetPath = sapHana.Get(SP.BP_BarcodeAll_GetPath);
                    string sCheckPathQry = string.Format(helper.ReadDataRow(GetPath, 1, "", 0), BPCode);

                    if (sapHana.Get(sCheckPathQry).Rows.Count == 1)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message);
                return result;
            }
            
        }

        //public BarcodePrintingService(IFrmSalesOrder view, ISalesOrderModel repository)
        //{
        //    _View = view;
        //    _View.Presenter = this;
        //    _repository = repository;

        //    Onload();
        //}

        public static string GetBarcodeHeader()
        {
            string value = "";

            string FirstHeaderValue = "\"AgeCode1\",\"AgeCode2\",\"AgeCode3\",\"BrandName\",\"BeforePrice\"" +
                                        ",\"Class\",\"CNCcode\",\"Color\",\"ConCode\",\"DateFrom\"" +
                                        ",\"DateTo\",\"DeliveryDate\",\"DeptCode\",\"DeptName\",\"Dept_SubDept\"" +
                                        ",\"DiscountFreeText\",\"Division\",\"HierarchyCode\",\"IntSKU\",\"ItemDesc\"" +
                                        ",\"ItemType\",\"ItemType2\",\"MarkdownPrice\",\"Month_Year\",\"Orig_Price\",\"ORIN\"" +
                                        ",\"Price\",\"ProductNo\",\"PromoIndicator\",\"ReceiveCode\",\"RegularPrice\"" +
                                        ",\"RetailPrice\",\"SeasonCode1\",\"SeasonCode2\",\"SeasonCode3\",\"SeasonCode4\"" +
                                        ",\"SellingUOM\",\"Size\",\"SKU\",\"SkuType\",\"SkuType2\",\"StoreName\"" +
                                        ",\"SubClass\",\"SubDept\",\"SubDept_Class\",\"SupplierCode\",\"UOM\"" +
                                        ",\"UPC\",\"VendorCode\",\"VendorPartNo\",\"VendorType\",\"VPN\",\"IMD_CodeBars\"" +
                                        ",\"OITM_Color\",\"OITM_Size\",\"OITM_Style\",\"OITM_ItemDesc\",\"OITM_Brand\"" +
                                        ",\"CPN2_BarOption\",\"CPN2_SKU\",\"CPN2_VendorPartNumber\",\"OITM_SubCat\"" +
                                        ",\"OITM_Category\",\"ItemDesc_Trim\",\"OITM_CatSubCat\",\"CPN3_BrandCode\"" +
                                        ",\"OITM_DeptName\",\"CPN3_DeptCode\",\"OITM_ItemDescFull\",\"PriceFormatZero\"" +
                                        ",\"CPN2_LongItemDesc\",\"OITM_SizeShortName\",\"OITM_CodeBars\",\"CPN2_Division\"" +
                                        ",\"OITM_CatCode\",\"OITM_SubCatCode\",\"PromoIndicator2\",\"AgeCode4\",\"SampleCode\"" +
                                        ",\"MonthValue\",\"DateValue\",\"YearValue\",\"OITM_BrandName\",\"OITM_Section\"" +
                                        ",\"CPN3_VendorID\"";

            string SecondHeaderValue_OITM = ",\"OITM_Department\"" +
                                            ",\"OITM_Brand2\"" +
                                            ",\"OITM_Dimension3\"" +
                                            ",\"OITM_Dimension4\"" +
                                            ",\"OITM_Dimension5\"" +
                                            ",\"OITM_SubDepartment\"" +
                                            ",\"OITM_SizeCategory\"" +
                                            ",\"OITM_Size2\"" +
                                            ",\"OITM_ParentColor\"" +
                                            ",\"OITM_Class\"" +
                                            ",\"OITM_SubClass\"" +
                                            ",\"OITM_Packaging\"" +
                                            ",\"OITM_Specification\"" +
                                            ",\"OITM_Collection\"" +
                                            ",\"OITM_BrandCode\"" +
                                            ",\"OITM_SortCode\"" +
                                            ",\"OITM_ItemClass\"" +
                                            ",\"OITM_StyleName\"" +
                                            ",\"OITM_Remarks\"" +
                                            ",\"OITM_Designer\"";

            string ThirdHeaderValue_OCRD = ",\"OCRD_AliasName\"" +
                                            ",\"OCRD_AddID\"" +
                                            ",\"OCRD_VatIdUnCmp\"" +
                                            ",\"OCRD_Notes\"" +
                                            ",\"OCRD_GlbLocNum\"" +
                                            ",\"OCRD_Series\"" +
                                            ",\"OCRD_GroupCode\"" +
                                            ",\"OCRD_Department\"" +
                                            ",\"OCRD_Dimension2\"" +
                                            ",\"OCRD_Dimension3\"" +
                                            ",\"OCRD_Dimension4\"" +
                                            ",\"OCRD_Dimension5\"" +
                                            ",\"OCRD_OrderClassification\"" +
                                            ",\"OCRD_StoreClassification\"" +
                                            ",\"OCRD_DeliveryClassification\"" +
                                            ",\"OCRD_Region\"" +
                                            ",\"OCRD_ClassificationForSupplies\"";

            string FourthHeaderValue_RDR = ",\"RDR1_U_SampleCode\"" +
                                            ",\"RDR1_U_UnitPricePerPiece\"" +
                                            ",\"ORDR_DocDueDate\"" +
                                            ",\"ORDR_CancelDate\"";

            //string FifthHeaderValue_CPN3 = ",\"CPN3_Memo\"" +
            //                                ",\"CPN3_U_Company\"" +
            //                                ",\"CPN3_U_Brand\"" +
            //                                ",\"CPN3_U_VendorID\"" +
            //                                ",\"CPN3_U_Series\"" +
            //                                ",\"CPN3_U_Category\"";

            string SixthHeaderValue_CPN2 = ",\"CPN2_CpnLineNum\"" +
                                            ",\"CPN2_CpnNo\"" +
                                            ",\"CPN2_ItemCode\"" +
                                            ",\"CPN2_ItemGrp\"" +
                                            ",\"CPN2_ItemName\"" +
                                            ",\"CPN2_ItemType\"" +
                                            ",\"CPN2_LogIns\"" +
                                            ",\"CPN2_U_BarCodeColor\"" +
                                            ",\"CPN2_U_Desc1\"" +
                                            ",\"CPN2_U_Desc2\"" +
                                            ",\"CPN2_U_Disc1\"" +
                                            ",\"CPN2_U_Disc2\"" +
                                            ",\"CPN2_U_Disc3\"" +
                                            ",\"CPN2_U_Disc4\"" +
                                            ",\"CPN2_U_Disc5\"" +
                                            ",\"CPN2_U_ItemType\"" +
                                            ",\"CPN2_U_MDPrice\"" +
                                            ",\"CPN2_U_OcpnArtNo\"" +
                                            ",\"CPN2_U_OldSKU\"" +
                                            ",\"CPN2_U_OldUPC\"" +
                                            ",\"CPN2_U_PartNo\"" +
                                            ",\"CPN2_U_Path\"" +
                                            ",\"CPN2_U_PathMDPrice\"" +
                                            ",\"CPN2_U_PriceCat\"" +
                                            ",\"CPN2_U_Pricetag\"" +
                                            ",\"CPN2_U_PricetagEquivalent\"" +
                                            ",\"CPN2_U_RPrice\"" +
                                            ",\"CPN2_U_SC\"" +
                                            ",\"CPN2_U_SeasonCode\"" +
                                            ",\"CPN2_U_Section\"" +
                                            ",\"CPN2_U_StyleCode\"" +
                                            ",\"CPN2_U_UPC\"" +
                                            ",\"CPN2_VisOrder\"";

            string SeventhHeaderValue_SpecialPrice = ",\"SP_Price\"";

            value = FirstHeaderValue + SecondHeaderValue_OITM + ThirdHeaderValue_OCRD + FourthHeaderValue_RDR + SixthHeaderValue_CPN2 + SeventhHeaderValue_SpecialPrice;
            value = value + "\r\n";

            return value;
        }
    }
}

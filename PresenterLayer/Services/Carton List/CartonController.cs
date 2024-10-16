using DirecLayer;
using DomainLayer;
using PresenterLayer.Helper;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace PresenterLayer.Services
{
    public class CartonController
    {
        DataContextList context = new DataContextList();
        private bool isSuccess;

        public delegate void DocEntry(int docEntry);

        public delegate void CartonListReturn(List<CartonListRow> CartonList);

        public delegate void ErrorString(string error);

        public delegate void ErrorId(string id);

        public delegate void GetErrorIds(List<ErrorIds> eIds);

        private SAPHanaAccess sapHana { get; set; }

        #region Carton Management

        //public bool uploadCartonManagementSL(ErrorString err, CartonListReturn cartonList, List<CartonManagement> cmHeader, List<CartonManagementRow> cmRow, [Optional] ErrorId errId)
        public bool uploadCartonManagementSL(ErrorString err, CartonListReturn cartonList, List<CartonManagement> cmHeader, List<CartonManagementRow> cmRow, [Optional] GetErrorIds eIds)
        {
            string id = "";
            int id2 = -1;

            try
            {
                context.cartonListRows.Clear();

                isSuccess = true;

                string message = string.Empty;

                var join = cmHeader.GroupJoin(cmRow, h => h.Id, i => i.Id, (h, i) => new
                {
                    h,
                    item = i.ToList()
                }).ToList();
                var cnt = 1;
                foreach (var header in join)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    List<Dictionary<string, object>> dictLines = new List<Dictionary<string, object>>();

                    id = header.h.VendorCode.ToString();
                    id2 = header.h.Id;

                    string vendorName = header.h.VendorName;

                    if (header.h.VendorName == null || header.h.VendorName == string.Empty)
                    {
                        vendorName = FetchVendorName(header.h.VendorCode);
                    }
                    dict.Add("U_CartonNo", header.h.CartonNo);
                    dict.Add("U_VendorCode", header.h.VendorCode);
                    dict.Add("U_VendorName", vendorName);
                    dict.Add("U_ChainName", header.h.ChainName);
                    dict.Add("U_DocRef", header.h.DocRef);
                    dict.Add("U_Ref1", header.h.Ref1);
                    dict.Add("U_Ref2", header.h.Ref2);
                    dict.Add("U_Status", header.h.Status);
                    dict.Add("Remark", (string.IsNullOrEmpty(header.h.Remark) ? $"Created By EasySAP | Data Transfer | {DateTime.Now.ToShortDateString()} | {DateTime.Now.ToShortTimeString()}" : header.h.Remark));
                    dict.Add("U_TransactionType", checkValue(header.h.TransactionType));
                    dict.Add("U_TargetWH", checkValue(header.h.TargetWH));
                    dict.Add("U_LastWH", checkValue(header.h.LastWH));
                    dict.Add("U_GroupCode", checkValue(header.h.GroupCode));

                    if (header.h.Date != null)
                    {
                        dict.Add("U_DateChecked", Convert.ToDateTime(checkDate(header.h.Date)));
                    }

                    foreach (var row in header.item)
                    {
                        var lines = new Dictionary<string, object>();

                        lines.Add("U_ItemNo", row.ItemNo);
                        lines.Add("U_Description", row.Description.Replace("\"", ""));
                        lines.Add("U_Quantity", Convert.ToDouble(row.Quantity));
                        lines.Add("U_QuantityInnerBox", Convert.ToDouble(checkValueD(row.QtyPerInnerBox)));

                        dictLines.Add(lines);
                    }

                    //oGeneralParams = oGeneralService.Add(oGeneralData);

                    //int documentEntry = oGeneralParams.GetProperty("DocEntry");
                    //DocEntry = documentEntry.ToString(),

                    var json = DataRepository.JsonBuilder(dict, dictLines, "CM_ROWSCollection");

                    //isSuccess = SAPHana.SL_Posting($"CartonMngt", SAPHana.SL_Mode.POST, json, "DocEntry", out message);
                    var serviceLayerAccess = new ServiceLayerAccess();
                    isSuccess = serviceLayerAccess.ServiceLayer_Posting(json, "POST", "CartonMngt", "DocEntry", out message, out string val);
                    if (isSuccess)
                    {
                        context.cartonListRows.Add(new CartonListRow
                        {
                            DocEntry = val,
                            CartonNo = header.h.CartonNo,
                            DocRef = header.h.DocRef,
                            Ref1 = header.h.Ref1,
                            Ref2 = header.h.Ref2,
                            VendorCode = header.h.VendorCode
                        });

                        cartonList(context.cartonListRows);

                        foreach (var row in header.item)
                        {
                            var cartonMItemExists = DataContextList.GetErrorIds.Any(x => x.DocEntry == val && x.UploadType == "Carton Management");
                            if (cartonMItemExists == false)
                            {
                                DataContextList.GetErrorIds.Add(new ErrorIds
                                {
                                    UploadType = "Carton Management",
                                    LineID = id2,
                                    DocEntry = val,
                                    CardCode = id,
                                    CardName = FetchVendorName(header.h.VendorCode),
                                    DocRef = header.h.DocRef,
                                    Ref1 = header.h.Ref1,
                                    Ref2 = header.h.Ref2,
                                    CartonNo = header.h.CartonNo,
                                    Remarks = header.h.Remark,
                                    RowCount = header.item.Count(),
                                    ItemCode = row.ItemNo,
                                    Quantity = Convert.ToDouble(row.Quantity),
                                    ErrMsg = "",
                                    Uploaded = "Yes"
                                });
                            }
                            eIds(DataContextList.GetErrorIds);
                        }
                    }
                    else
                    {
                        //Added this to get error id's by Cedi 062019

                        foreach (var row in header.item)
                        {
                            var cartonMItemExists = DataContextList.GetErrorIds.Any(x => x.DocEntry == val && x.UploadType == "Carton Management");
                            if (cartonMItemExists == false)
                            {
                                DataContextList.GetErrorIds.Add(new ErrorIds
                                {
                                    UploadType = "Carton Management",
                                    LineID = id2,
                                    CardCode = id,
                                    CardName = FetchVendorName(header.h.VendorCode),
                                    DocRef = header.h.DocRef,
                                    Ref1 = header.h.Ref1,
                                    Ref2 = header.h.Ref2,
                                    CartonNo = header.h.CartonNo,
                                    Remarks = header.h.Remark,
                                    RowCount = header.item.Count(),
                                    ItemCode = row.ItemNo,
                                    Quantity = Convert.ToDouble(row.Quantity),
                                    ErrMsg = message,
                                    Uploaded = "No"
                                });
                            }
                            eIds(DataContextList.GetErrorIds);
                        }

                    }
                    StaticHelper._MainForm.Invoke(new Action(() =>
                        StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {cnt++} out of {join.Count}", cnt, join.Count)
                    ));
                }

                //On Comment by Cedi on 052719 due to conflict on process
                //err(message);
                //context.cartonListRows.Clear();

                //Added due to conflict in logic 091319, 100219
                //isSuccess = true;
            }
            catch (Exception ex)
            {
                err(ex.Message);
                //errId(id);

                //Added this to get error id's by Cedi 062019
                DataContextList.GetErrorIds.Add(new ErrorIds
                {
                    UploadType = "Carton Management",
                    LineID = id2,
                    CardCode = id,
                    ErrMsg = ex.Message,
                    Uploaded = "No"
                });
                eIds(DataContextList.GetErrorIds);

                context.cartonListRows.Clear();
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool uploadCartonListSL(ErrorString errorstring, List<CartonList> clHeader, List<CartonListRow> clRow)
        {
            bool isSuccess = true;
            string message = string.Empty;
            sapHana = new SAPHanaAccess();

            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                List<Dictionary<string, object>> dictLines = new List<Dictionary<string, object>>();
                foreach (var header in clHeader)
                {
                    dict.Add("Remark", header.Remark);

                    foreach (var row in clRow)
                    {
                        var lines = new Dictionary<string, object>();

                        lines.Add("U_DocEntry", checkValue(row.DocEntry));
                        lines.Add("U_CartonNo", checkValue(row.CartonNo));
                        lines.Add("U_DocRef", checkValue(row.DocRef));
                        lines.Add("U_Ref1", checkValue(row.Ref1) == "-" ? row.VendorCode : checkValue(row.Ref1));
                        lines.Add("U_Ref2", checkValue(row.Ref2));
                        lines.Add("U_Remarks", checkValue(row.Remark));
                        dictLines.Add(lines);
                    }

                    var json = DataRepository.JsonBuilder(dict, dictLines, "CL_ROWSCollection");
                    var serviceLayerAccess = new ServiceLayerAccess();
                    isSuccess = serviceLayerAccess.ServiceLayer_Posting(json, "POST", "CartonList", "DocNum", out message, out string val);
                    //isSuccess = SAPHana.SL_Posting($"CartonList", SAPHana.SL_Mode.POST, json, "DocNum", out message);
                    if (isSuccess)
                    {
                        //Added this to get error id's by Cedi 062019
                        foreach (var row in clRow)
                        {
                            string SelQryForQty = $"select Count(LineId)[RowCount] from[@CL_ROWS] where DocEntry = '{val}'";
                            var cartonMItemExists = DataContextList.GetErrorIds.Any(x => x.DocEntry == val && x.UploadType == "Carton List");
                            if (cartonMItemExists == false)
                            {
                                DataContextList.GetErrorIds.Add(new ErrorIds
                                {
                                    UploadType = "Carton List",
                                    CardCode = row.VendorCode,
                                    CardName = FetchVendorName(row.VendorCode),
                                    RowCount = Convert.ToInt32(sapHana.Get(SelQryForQty).Rows.Count > 0 ? sapHana.Get(SelQryForQty).Rows[0]["RowCount"].ToString() : "0"),
                                    DocEntry = val,
                                    DocRef = row.DocRef,
                                    Ref1 = row.Ref1,
                                    Ref2 = row.Ref2,
                                    CartonNo = row.CartonNo,
                                    Remarks = header.Remark,
                                    URemarks = row.Remark,
                                    ErrMsg = message,
                                    Uploaded = "Yes"
                                });
                            }

                        }
                    }
                    else
                    {
                        foreach (var row in clRow)
                        {
                            DataContextList.GetErrorIds.Add(new ErrorIds
                            {
                                UploadType = "Carton List",
                                CardCode = row.VendorCode,
                                CardName = FetchVendorName(row.VendorCode),
                                RowCount = 0,
                                DocEntry = row.DocEntry,
                                DocRef = row.DocRef,
                                Ref1 = row.Ref1,
                                Ref2 = row.Ref2,
                                CartonNo = row.CartonNo,
                                Remarks = header.Remark,
                                URemarks = row.Remark,
                                ErrMsg = message,
                                Uploaded = "No"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorstring(ex.Message);

                DataContextList.GetErrorIds.Add(new ErrorIds
                {
                    UploadType = "Carton List",
                    ErrMsg = ex.Message,
                    Uploaded = "No"
                });

                return isSuccess = false;
            }

            return isSuccess;
        }

        #endregion



        #region FUNCTION
        public DataTable loadExistingCarton(string docEntry)
        {
            string query = $"select U_ItemNo, U_Description, U_Quantity FROM [@CM_ROWS] WHERE DocEntry = '{docEntry}'";

            return DataAccess.Select(DataAccess.conStr("HANA"), query);
        }

        public DataTable loadExistingCartonList(string docEntry)
        {
            string query = $"select * FROM [@CL_ROWS] WHERE DocEntry = '{docEntry}'";

            return DataAccess.Select(DataAccess.conStr("HANA"), query);
        }

        public string checkValue(string value)
        {
            return value == string.Empty || value == null ? "" : value;
        }

        public double checkValueD(string value)
        {
            return value == string.Empty || value == null ? 0D : Convert.ToDouble(value);
        }

        public string checkDate(string value)
        {
            return value == string.Empty || value == null ? DateTime.Now.ToString() : value;
        }

        string FetchVendorName(string vendorCode)
        {
            SAPHanaAccess hana = new SAPHanaAccess();
            //return DataAccess.SearchData(DataAccess.conStr("HANA"), $"SELECT CardName FROM OCRD WHERE CardCode = '{vendorCode}'", 0, "CardName");
            return hana.Get($"SELECT CardName FROM OCRD WHERE CardCode = '{vendorCode}'").Rows[0]["CardName"].ToString(); 
        }
        #endregion

    }
}

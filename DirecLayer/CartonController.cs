using Context;
using EasySAP._03_Repository;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace EasySAP
{
    public class CartonController
    {
        DataContextList context = new DataContextList();
        private bool isSuccess;
        private object _repository;

        GeneralService oGeneralService { get; set; } = null;
        GeneralData oGeneralData { get; set; } = null;
        GeneralDataParams oGeneralParams { get; set; } = null;
        CompanyService oCompanyService { get; set; } = null;
        GeneralDataCollection oChildren { get; set; } = null;
        GeneralData oChild { get; set; } = null;
        GeneralDataCollection oGeneralChildren { get; set; } = null;

        public delegate void DocEntry(int docEntry);

        public delegate void CartonListReturn(List<CartonListRow> CartonList);

        public delegate void ErrorString(string error);

        public delegate void ErrorId(string id);

        #region Carton Management

        public bool uploadCartonManagementSL(ErrorString err, CartonListReturn cartonList, List<CartonManagement> cmHeader, List<CartonManagementRow> cmRow, [Optional] ErrorId errId)
        {
            string id = "";

            try
            {
                context.cartonListRows.Clear();

                isSuccess = true;

                string message = string.Empty;

                if (!SAPAccess.oCompany.InTransaction)
                {
                    SAPAccess.oCompany.StartTransaction();
                }

                var join = cmHeader.GroupJoin(cmRow, h => h.Id, i => i.Id, (h, i) => new
                {
                    h,
                    item = i.ToList()
                }).ToList();
                    
                foreach (var header in join)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    List<Dictionary<string, object>> dictLines = new List<Dictionary<string, object>>();

                    id = header.h.VendorCode.ToString();

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
                    dict.Add("Remark", header.h.Remark);
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
                        lines.Add("U_Description", row.Description);
                        lines.Add("U_Quantity", Convert.ToDouble(row.Quantity));
                        lines.Add("U_QuantityInnerBox", Convert.ToDouble(checkValueD(row.QtyPerInnerBox)));

                        dictLines.Add(lines);
                    }

                    oGeneralParams = oGeneralService.Add(oGeneralData);

                    int documentEntry = oGeneralParams.GetProperty("DocEntry");

                    context.cartonListRows.Add(new CartonListRow
                    {
                        DocEntry = documentEntry.ToString(),
                        CartonNo = header.h.CartonNo,
                        DocRef = header.h.DocRef,
                        Ref1 = header.h.Ref1,
                        Ref2 = header.h.Ref2,
                        VendorCode = header.h.VendorCode
                    });

                    var json = DataRepository.JsonBuilder(dict, dictLines, "CM_ROWSCollection");
                    cartonList(context.cartonListRows);
                    isSuccess = SAPHana.SL_Posting($"CartonMngt", SAPHana.SL_Mode.POST, json, "DocEntry", out message);
                }
                
                err(message);
                context.cartonListRows.Clear();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                err(ex.Message);
                errId(id);
                context.cartonListRows.Clear();
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool uploadCartonListSL(ErrorString errorstring, List<CartonList> clHeader, List<CartonListRow> clRow)
        {
            bool isSuccess = true;
            string message = string.Empty;

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
                        lines.Add("U_Ref1", checkValue(row.Ref1));
                        lines.Add("U_Ref2", checkValue(row.Ref2));
                        lines.Add("U_Remarks", checkValue(row.Remark));
                        dictLines.Add(lines);
                    }

                    var json = DataRepository.JsonBuilder(dict, dictLines, "CL_ROWSCollection");
                    isSuccess = SAPHana.SL_Posting($"CartonList", SAPHana.SL_Mode.POST, json, "DocNum", out message);
                }
            }
            catch (Exception ex)
            {
                errorstring(ex.Message);
                return isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Carton Management 
        /// </summary>
        /// <param name="err"></param>
        /// <param name="cartonList"></param>
        /// <param name="cmHeader"></param>
        /// <param name="cmRow"></param>
        /// <returns></returns>
        public bool uploadCartonManagement(ErrorString err, CartonListReturn cartonList, List<CartonManagement> cmHeader, List<CartonManagementRow> cmRow, [Optional] ErrorId errId)
        {
            string id = "";

            try
            { 
                context.cartonListRows.Clear();

                isSuccess = true;


                if (!SAPAccess.oCompany.InTransaction)
                {
                    SAPAccess.oCompany.StartTransaction();
                }

                var join = cmHeader.GroupJoin(cmRow, h => h.Id, i => i.Id, (h, i) => new
                {
                    h,
                    item = i.ToList()
                }).ToList();

                foreach (var header in join)
                {
                    oCompanyService = SAPAccess.oCompany.GetCompanyService();
                    oGeneralService = oCompanyService.GetGeneralService("CartonMngt");
                    oGeneralData = ((GeneralData)(oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData)));
                    oChildren = oGeneralData.Child("CM_ROWS");

                    id = header.h.VendorCode.ToString();

                    string vendorName = header.h.VendorName;

                    if (header.h.VendorName == null || header.h.VendorName == string.Empty)
                    {
                        vendorName = FetchVendorName(header.h.VendorCode);
                    }

                    oGeneralData.SetProperty("U_CartonNo", header.h.CartonNo);
                    oGeneralData.SetProperty("U_VendorCode", header.h.VendorCode);
                    oGeneralData.SetProperty("U_VendorName", vendorName);
                    oGeneralData.SetProperty("U_ChainName", header.h.ChainName);
                    oGeneralData.SetProperty("U_DocRef", header.h.DocRef);
                    oGeneralData.SetProperty("U_Ref1", header.h.Ref1);
                    oGeneralData.SetProperty("U_Ref2", header.h.Ref2);
                    oGeneralData.SetProperty("U_Status", header.h.Status);
                    oGeneralData.SetProperty("Remark", header.h.Remark);
                    oGeneralData.SetProperty("U_TransactionType", checkValue(header.h.TransactionType));
                    oGeneralData.SetProperty("U_TargetWH", checkValue(header.h.TargetWH));
                    oGeneralData.SetProperty("U_LastWH", checkValue(header.h.LastWH));

                    if (header.h.Date != null)
                    {
                        oGeneralData.SetProperty("U_DateChecked", Convert.ToDateTime(checkDate(header.h.Date)));
                    }

                    oGeneralData.SetProperty("U_GroupCode", checkValue(header.h.GroupCode));

                    foreach (var row in header.item)
                    {
                        oChild = oChildren.Add();

                        oChild.SetProperty("U_ItemNo", row.ItemNo);
                        oChild.SetProperty("U_Description", row.Description);
                        oChild.SetProperty("U_Quantity", Convert.ToDouble(row.Quantity));
                        oChild.SetProperty("U_QuantityInnerBox", Convert.ToDouble(checkValueD(row.QtyPerInnerBox)));
                    }

                    oGeneralParams = oGeneralService.Add(oGeneralData);

                    int documentEntry = oGeneralParams.GetProperty("DocEntry");

                    context.cartonListRows.Add(new CartonListRow
                    {
                        DocEntry = documentEntry.ToString(),
                        CartonNo = header.h.CartonNo,
                        DocRef = header.h.DocRef,
                        Ref1 = header.h.Ref1,
                        Ref2 = header.h.Ref2,
                        VendorCode = header.h.VendorCode
                    });
                }

                SAPAccess.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                cartonList(context.cartonListRows);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                err(ex.Message);
                errId(id);
                context.cartonListRows.Clear();
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Update Carton Management
        /// </summary>
        /// <param name="cmHeader"></param>
        /// <param name="cmRow"></param>
        public void updateCartonManagement(List<CartonManagement> cmHeader, List<CartonManagementRow> cmRow)
        {
            oCompanyService = SAPAccess.oCompany.GetCompanyService();
            oGeneralService = oCompanyService.GetGeneralService("CartonMngt");
            oGeneralParams = ((GeneralDataParams)(oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)));

            foreach (var header in cmHeader)
            {
                oGeneralParams.SetProperty("DocEntry", header.DocEntry);

                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                oGeneralData.SetProperty("U_CartonNo", header.CartonNo);
                oGeneralData.SetProperty("U_VendorCode", header.VendorCode);
                oGeneralData.SetProperty("U_VendorName", header.VendorName);
                oGeneralData.SetProperty("U_ChainName", header.ChainName);
                oGeneralData.SetProperty("U_DocRef", header.DocRef);
                oGeneralData.SetProperty("U_Ref1", header.Ref1);
                oGeneralData.SetProperty("U_Ref2", header.Ref2);
                oGeneralData.SetProperty("U_Status", header.Status);
                oGeneralData.SetProperty("Remark", header.Remark);

                if (header.Date != null)
                {
                    oGeneralData.SetProperty("U_DateChecked", Convert.ToDateTime(header.Date));
                }

                oGeneralChildren = oGeneralData.Child("CM_ROWS");

                int count = loadExistingCarton(header.DocEntry).Rows.Count;
                int i = 0;
                foreach (var row in cmRow)
                {
                    oChild = count > i ? oGeneralChildren.Item(i) : oGeneralChildren.Add();

                    oChild.SetProperty("U_ItemNo", row.ItemNo);
                    oChild.SetProperty("U_Description", row.Description);
                    oChild.SetProperty("U_Quantity", Convert.ToDouble(row.Quantity));

                    i++;
                }

                oGeneralService.Update(oGeneralData);
            }
        }
        #endregion



        #region Carton List
        /// <summary>
        /// Upload Carton List
        /// </summary>
        /// <param name="errorstring"></param>
        /// <param name="clHeader"></param>
        /// <param name="clRow"></param>
        /// <returns></returns>
        public bool uploadCartonList(ErrorString errorstring, List<CartonList> clHeader, List<CartonListRow> clRow)
        {
            bool isSuccess = true;

            try
            {
                SAPAccess.oCompany.StartTransaction();

                oCompanyService = SAPAccess.oCompany.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("CartonList");
                oGeneralData = ((GeneralData)(oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData)));
                oChildren = oGeneralData.Child("CL_ROWS");

                foreach (var header in clHeader)
                {
                    oGeneralData.SetProperty("Remark", header.Remark);

                    foreach (var row in clRow)
                    {
                        oChild = oChildren.Add();
                        oChild.SetProperty("U_DocEntry", checkValue(row.DocEntry));
                        oChild.SetProperty("U_CartonNo", checkValue(row.CartonNo));
                        oChild.SetProperty("U_DocRef", checkValue(row.DocRef));
                        oChild.SetProperty("U_Ref1", checkValue(row.Ref1));
                        oChild.SetProperty("U_Ref2", checkValue(row.Ref2));
                        oChild.SetProperty("U_Remarks", checkValue(row.Remark));
                    }
                }

                oGeneralParams = oGeneralService.Add(oGeneralData);
                SAPAccess.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
            }
            catch (Exception ex)
            {
                errorstring(ex.Message);
                return isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// UPDATE CARTON LIST
        /// </summary>
        /// <param name="err"></param>
        /// <param name="clHeader"></param>
        /// <param name="clRow"></param>
        /// <returns></returns>
        public bool updateCartonList(ErrorString err, List<CartonList> clHeader, List<CartonListRow> clRow)
        {
            bool isSuccess = true;

            oCompanyService = SAPAccess.oCompany.GetCompanyService();
            oGeneralService = oCompanyService.GetGeneralService("CartonList");
            oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));

            foreach (var header in clHeader)
            {
                oGeneralParams.SetProperty("DocEntry", header.DocEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oGeneralData.SetProperty("Remark", header.Remark);
                oGeneralChildren = oGeneralData.Child("CL_ROWS");

                int count = loadExistingCartonList(header.DocEntry).Rows.Count;
                int i = 0;
                foreach (var row in clRow)
                {
                    oChild = count > i ? oGeneralChildren.Item(i) : oGeneralChildren.Add();

                    oChild.SetProperty("U_DocEntry", row.DocEntry);
                    oChild.SetProperty("U_CartonNo", row.CartonNo);
                    oChild.SetProperty("U_DocRef", row.DocRef);
                    oChild.SetProperty("U_Ref1", row.Ref1);
                    oChild.SetProperty("U_Ref2", row.Ref2);
                    oChild.SetProperty("U_Remarks", row.Remark);

                    i++;
                }

                try
                {
                    oGeneralService.Update(oGeneralData);
                }
                catch (Exception ex)
                {
                    err(ex.Message);
                    return isSuccess = false;
                }
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
            return DataAccess.SearchData(DataAccess.conStr("HANA"), $"SELECT CardName FROM OCRD WHERE CardCode = '{vendorCode}'", 0, "CardName");
        }
        #endregion

    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class MarketingDocumentHeaders
    {
        [Key]
        public int Id { get; set; }
        public string Session { get; set; }
        public string User { get; set; }
        public string rowLocation { get; set; }
        public string colLocation { get; set; }
        public string SapField { get; set; }
        public string Uploaded { get; set; } = "Yes";
        public string ErrorMessage { get; set; }
        public string ToWarehouse { get; set; }
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string DocType { get; set; }
        public string CANCELED { get; set; }
        public string Handwrtten { get; set; }
        public string Printed { get; set; }
        public string DocStatus { get; set; }
        public string InvntSttus { get; set; }
        public string FromWarehouse { get; set; }
        public string Transfered { get; set; }
        public string ObjType { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string DueDate { get; set; }
        public string CardCode { get; set; }
        public string CardCode2 { get; set; }
        public string CardName { get; set; }
        public string Address { get; set; }
        public string NumAtCard { get; set; }
        public string VatPercent { get; set; }
        public string VatSum { get; set; }
        public string VatSumFC { get; set; }
        public string DiscPrcnt { get; set; }
        public string DiscSum { get; set; }
        public string DiscSumFC { get; set; }
        public string DocCur { get; set; }
        public string DocRate { get; set; }
        public string DocTotal { get; set; }
        public string DocTotalFC { get; set; }
        public string PaidToDate { get; set; }
        public string PaidFC { get; set; }
        public string GrosProfit { get; set; }
        public string GrosProfFC { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Comments { get; set; }
        public string JrnlMemo { get; set; }
        public string TransId { get; set; }
        public string ReceiptNum { get; set; }
        public string GroupNum { get; set; }
        public string DocTime { get; set; }
        public string SlpCode { get; set; }
        public string TrnspCode { get; set; }
        public string PartSupply { get; set; }
        public string Confirmed { get; set; }
        public string GrossBase { get; set; }
        public string ImportEnt { get; set; }
        public string CreateTran { get; set; }
        public string SummryType { get; set; }
        public string UpdInvnt { get; set; }
        public string UpdCardBal { get; set; }
        public string Instance { get; set; }
        public string Flags { get; set; }
        public string InvntDirec { get; set; }
        public string CntctCode { get; set; }
        public string ShowSCN { get; set; }
        public string FatherCard { get; set; }
        public string SysRate { get; set; }
        public string CurSource { get; set; }
        public string VatSumSy { get; set; }
        public string DiscSumSy { get; set; }
        public string DocTotalSy { get; set; }
        public string PaidSys { get; set; }
        public string FatherType { get; set; }
        public string GrosProfSy { get; set; }
        public string UpdateDate { get; set; }
        public string IsICT { get; set; }
        public string CreateDate { get; set; }
        public string Volume { get; set; }
        public string VolUnit { get; set; }
        public string Weight { get; set; }
        public string WeightUnit { get; set; }
        public string Series { get; set; }
        public string TaxDate { get; set; }
        public string Filler { get; set; }
        public string DataSource { get; set; }
        public string StampNum { get; set; }
        public string isCrin { get; set; }
        public string FinncPriod { get; set; }
        public string UserSign { get; set; }
        public string selfInv { get; set; }
        public string VatPaid { get; set; }
        public string VatPaidFC { get; set; }
        public string VatPaidSys { get; set; }
        public string UserSign2 { get; set; }
        public string WddStatus { get; set; }
        public string draftKey { get; set; }
        public string TotalExpns { get; set; }
        public string TotalExpFC { get; set; }
        public string TotalExpSC { get; set; }
        public string DunnLevel { get; set; }
        public string Address2 { get; set; }
        public string LogInstanc { get; set; }
        public string Exported { get; set; }
        public string StationID { get; set; }
        public string Indicator { get; set; }
        public string NetProc { get; set; }
        public string AqcsTax { get; set; }
        public string AqcsTaxFC { get; set; }
        public string AqcsTaxSC { get; set; }
        public string CashDiscPr { get; set; }
        public string CashDiscnt { get; set; }
        public string CashDiscFC { get; set; }
        public string CashDiscSC { get; set; }
        public string ShipToCode { get; set; }
        public string LicTradNum { get; set; }
        public string PaymentRef { get; set; }
        public string WTSum { get; set; }
        public string WTSumFC { get; set; }
        public string WTSumSC { get; set; }
        public string RoundDif { get; set; }
        public string RoundDifFC { get; set; }
        public string RoundDifSy { get; set; }
        public string CheckDigit { get; set; }
        public string Form1099 { get; set; }
        public string Box1099 { get; set; }
        public string submitted { get; set; }
        public string PoPrss { get; set; }
        public string Rounding { get; set; }
        public string RevisionPo { get; set; }
        public string Segment { get; set; }
        public string ReqDate { get; set; }
        public string CancelDate { get; set; }
        public string PickStatus { get; set; }
        public string Pick { get; set; }
        public string BlockDunn { get; set; }
        public string PeyMethod { get; set; }
        public string PayBlock { get; set; }
        public string PayBlckRef { get; set; }
        public string MaxDscn { get; set; }
        public string Reserve { get; set; }
        public string Max1099 { get; set; }
        public string CntrlBnk { get; set; }
        public string PickRmrk { get; set; }
        public string ISRCodLine { get; set; }
        public string ExpAppl { get; set; }
        public string ExpApplFC { get; set; }
        public string ExpApplSC { get; set; }
        public string Project { get; set; }
        public string DeferrTax { get; set; }
        public string LetterNum { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string WTApplied { get; set; }
        public string WTAppliedF { get; set; }
        public string BoeReserev { get; set; }
        public string AgentCode { get; set; }
        public string WTAppliedS { get; set; }
        public string EquVatSum { get; set; }
        public string EquVatSumF { get; set; }
        public string EquVatSumS { get; set; }
        public string Installmnt { get; set; }
        public string VATFirst { get; set; }
        public string NnSbAmnt { get; set; }
        public string NnSbAmntSC { get; set; }
        public string NbSbAmntFC { get; set; }
        public string ExepAmnt { get; set; }
        public string ExepAmntSC { get; set; }
        public string ExepAmntFC { get; set; }
        public string VatDate { get; set; }
        public string CorrExt { get; set; }
        public string CorrInv { get; set; }
        public string NCorrInv { get; set; }
        public string CEECFlag { get; set; }
        public string BaseAmnt { get; set; }
        public string BaseAmntSC { get; set; }
        public string BaseAmntFC { get; set; }
        public string CtlAccount { get; set; }
        public string BPLId { get; set; }
        public string BPLName { get; set; }
        public string VATRegNum { get; set; }
        public string TxInvRptNo { get; set; }
        public string TxInvRptDt { get; set; }
        public string KVVATCode { get; set; }
        public string WTDetails { get; set; }
        public string SumAbsId { get; set; }
        public string SumRptDate { get; set; }
        public string PIndicator { get; set; }
        public string ManualNum { get; set; }
        public string UseShpdGd { get; set; }
        public string BaseVtAt { get; set; }
        public string BaseVtAtSC { get; set; }
        public string BaseVtAtFC { get; set; }
        public string NnSbVAt { get; set; }
        public string NnSbVAtSC { get; set; }
        public string NbSbVAtFC { get; set; }
        public string ExptVAt { get; set; }
        public string ExptVAtSC { get; set; }
        public string ExptVAtFC { get; set; }
        public string LYPmtAt { get; set; }
        public string LYPmtAtSC { get; set; }
        public string LYPmtAtFC { get; set; }
        public string ExpAnSum { get; set; }
        public string ExpAnSys { get; set; }
        public string ExpAnFrgn { get; set; }
        public string DocSubType { get; set; }
        public string DpmStatus { get; set; }
        public string DpmAmnt { get; set; }
        public string DpmAmntSC { get; set; }
        public string DpmAmntFC { get; set; }
        public string DpmDrawn { get; set; }
        public string DpmPrcnt { get; set; }
        public string PaidSum { get; set; }
        public string PaidSumFc { get; set; }
        public string PaidSumSc { get; set; }
        public string FolioPref { get; set; }
        public string FolioNum { get; set; }
        public string DpmAppl { get; set; }
        public string DpmApplFc { get; set; }
        public string DpmApplSc { get; set; }
        public string LPgFolioN { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string Posted { get; set; }
        public string OwnerCode { get; set; }
        public string BPChCode { get; set; }
        public string BPChCntc { get; set; }
        public string PayToCode { get; set; }
        public string IsPaytoBnk { get; set; }
        public string BnkCntry { get; set; }
        public string BankCode { get; set; }
        public string BnkAccount { get; set; }
        public string BnkBranch { get; set; }
        public string isIns { get; set; }
        public string TrackNo { get; set; }
        public string VersionNum { get; set; }
        public string LangCode { get; set; }
        public string BPNameOW { get; set; }
        public string BillToOW { get; set; }
        public string ShipToOW { get; set; }
        public string RetInvoice { get; set; }
        public string ClsDate { get; set; }
        public string MInvNum { get; set; }
        public string MInvDate { get; set; }
        public string SeqCode { get; set; }
        public string Serial { get; set; }
        public string SeriesStr { get; set; }
        public string SubStr { get; set; }
        public string Model { get; set; }
        public string TaxOnExp { get; set; }
        public string TaxOnExpFc { get; set; }
        public string TaxOnExpSc { get; set; }
        public string TaxOnExAp { get; set; }
        public string TaxOnExApF { get; set; }
        public string TaxOnExApS { get; set; }
        public string LastPmnTyp { get; set; }
        public string LndCstNum { get; set; }
        public string UseCorrVat { get; set; }
        public string BlkCredMmo { get; set; }
        public string OpenForLaC { get; set; }
        public string Excised { get; set; }
        public string ExcRefDate { get; set; }
        public string ExcRmvTime { get; set; }
        public string SrvGpPrcnt { get; set; }
        public string DepositNum { get; set; }
        public string CertNum { get; set; }
        public string DutyStatus { get; set; }
        public string AutoCrtFlw { get; set; }
        public string FlwRefDate { get; set; }
        public string FlwRefNum { get; set; }
        public string VatJENum { get; set; }
        public string DpmVat { get; set; }
        public string DpmVatFc { get; set; }
        public string DpmVatSc { get; set; }
        public string DpmAppVat { get; set; }
        public string DpmAppVatF { get; set; }
        public string DpmAppVatS { get; set; }
        public string InsurOp347 { get; set; }
        public string IgnRelDoc { get; set; }
        public string BuildDesc { get; set; }
        public string ResidenNum { get; set; }
        public string Checker { get; set; }
        public string Payee { get; set; }
        public string CopyNumber { get; set; }
        public string SSIExmpt { get; set; }
        public string PQTGrpSer { get; set; }
        public string PQTGrpNum { get; set; }
        public string PQTGrpHW { get; set; }
        public string ReopOriDoc { get; set; }
        public string ReopManCls { get; set; }
        public string DocManClsd { get; set; }
        public string ClosingOpt { get; set; }
        public string SpecDate { get; set; }
        public string Ordered { get; set; }
        public string NTSApprov { get; set; }
        public string NTSWebSite { get; set; }
        public string NTSeTaxNo { get; set; }
        public string NTSApprNo { get; set; }
        public string PayDuMonth { get; set; }
        public string ExtraMonth { get; set; }
        public string ExtraDays { get; set; }
        public string CdcOffset { get; set; }
        public string SignMsg { get; set; }
        public string SignDigest { get; set; }
        public string CertifNum { get; set; }
        public string KeyVersion { get; set; }
        public string EDocGenTyp { get; set; }
        public string ESeries { get; set; }
        public string EDocNum { get; set; }
        public string EDocExpFrm { get; set; }
        public string OnlineQuo { get; set; }
        public string POSEqNum { get; set; }
        public string POSManufSN { get; set; }
        public string POSCashN { get; set; }
        public string EDocStatus { get; set; }
        public string EDocCntnt { get; set; }
        public string EDocProces { get; set; }
        public string EDocErrCod { get; set; }
        public string EDocErrMsg { get; set; }
        public string EDocCancel { get; set; }
        public string EDocTest { get; set; }
        public string EDocPrefix { get; set; }
        public string CUP { get; set; }
        public string CIG { get; set; }
        public string DpmAsDscnt { get; set; }
        public string Attachment { get; set; }
        public string AtcEntry { get; set; }
        public string SupplCode { get; set; }
        public string GTSRlvnt { get; set; }
        public string BaseDisc { get; set; }
        public string BaseDiscSc { get; set; }
        public string BaseDiscFc { get; set; }
        public string BaseDiscPr { get; set; }
        public string CreateTS { get; set; }
        public string UpdateTS { get; set; }
        public string SrvTaxRule { get; set; }
        public string AnnInvDecR { get; set; }
        public string Supplier { get; set; }
        public string Releaser { get; set; }
        public string Receiver { get; set; }
        public string ToWhsCode { get; set; }
        public string AssetDate { get; set; }
        public string Requester { get; set; }
        public string ReqName { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Notify { get; set; }
        public string ReqType { get; set; }
        public string OriginType { get; set; }
        public string IsReuseNum { get; set; }
        public string IsReuseNFN { get; set; }
        public string DocDlvry { get; set; }
        public string PaidDpm { get; set; }
        public string PaidDpmF { get; set; }
        public string PaidDpmS { get; set; }
        public string EnvTypeNFe { get; set; }
        public string AgrNo { get; set; }
        public string IsAlt { get; set; }
        public string AltBaseTyp { get; set; }
        public string AltBaseEnt { get; set; }
        public string AuthCode { get; set; }
        public string StDlvDate { get; set; }
        public string StDlvTime { get; set; }
        public string EndDlvDate { get; set; }
        public string EndDlvTime { get; set; }
        public string VclPlate { get; set; }
        public string ElCoStatus { get; set; }
        public string AtDocType { get; set; }
        public string ElCoMsg { get; set; }
        public string PrintSEPA { get; set; }
        public string FreeChrg { get; set; }
        public string FreeChrgFC { get; set; }
        public string FreeChrgSC { get; set; }
        public string NfeValue { get; set; }
        public string FiscDocNum { get; set; }
        public string RelatedTyp { get; set; }
        public string RelatedEnt { get; set; }
        public string CCDEntry { get; set; }
        public string NfePrntFo { get; set; }
        public string ZrdAbs { get; set; }
        public string POSRcptNo { get; set; }
        public string FoCTax { get; set; }
        public string FoCTaxFC { get; set; }
        public string FoCTaxSC { get; set; }
        public string TpCusPres { get; set; }
        public string ExcDocDate { get; set; }
        public string FoCFrght { get; set; }
        public string FoCFrghtFC { get; set; }
        public string FoCFrghtSC { get; set; }
        public string InterimTyp { get; set; }
        public string PTICode { get; set; }
        public string Letter { get; set; }
        public string FolNumFrom { get; set; }
        public string FolNumTo { get; set; }
        public string FolSeries { get; set; }
        public string SplitTax { get; set; }
        public string SplitTaxFC { get; set; }
        public string SplitTaxSC { get; set; }
        public string ToBinCode { get; set; }
        public string PermitNo { get; set; }
        public string MYFtype { get; set; }
        public string PoDropPrss { get; set; }
        public string DocTaxID { get; set; }
        public string DateReport { get; set; }
        public string RepSection { get; set; }
        public string ExclTaxRep { get; set; }
        public string PosCashReg { get; set; }
        public string DmpTransID { get; set; }
        public string ECommerBP { get; set; }
        public string EComerGSTN { get; set; }
        public string Revision { get; set; }
        public string RevRefNo { get; set; }
        public string RevRefDate { get; set; }
        public string RevCreRefN { get; set; }
        public string RevCreRefD { get; set; }
        public string TaxInvNo { get; set; }
        public string FrmBpDate { get; set; }
        public string GSTTranTyp { get; set; }
        public string BaseType { get; set; }
        public string BaseEntry { get; set; }
        public string Remark { get; set; }
        public string U_TransCat { get; set; }
        public string U_DRNo { get; set; }
        public string U_SONo { get; set; }
        public string U_SINo { get; set; }
        public string U_PrepBy { get; set; }
        public string U_RevBy { get; set; }
        public string U_AppBy { get; set; }
        public string U_RetType { get; set; }
        public string U_Remarks { get; set; }
        public string U_PRNo { get; set; }
        public string U_CheckBy { get; set; }
        public string U_PurchaseType { get; set; }
        public string U_NotedBy { get; set; }
        public string U_ReqBy { get; set; }
        public string U_OrderStatus { get; set; }
        public string U_FormType { get; set; }
        public string U_TruckNo { get; set; }
        public string U_Driver { get; set; }
        public string U_Helper1 { get; set; }
        public string U_Helper2 { get; set; }
        public string U_Helper3 { get; set; }
        public string U_Status { get; set; }
        public string U_DelBy { get; set; }
        public string U_DateDel { get; set; }
        public string U_ReceivedBy { get; set; }
        public string U_PostRem { get; set; }
        public string U_SealNo { get; set; }
        public string U_ContainerNo { get; set; }
        public string U_TransType { get; set; }
        public string U_IsPrinted { get; set; }
        public string U_RRNo { get; set; }
        public string U_PLNo { get; set; }
        public string U_Whse { get; set; }
        public string U_Cashier_Codes { get; set; }
        public string U_TerminalID { get; set; }
        public string U_Cashier_Name { get; set; }
        public string U_DateRec { get; set; }
        public string U_Priority { get; set; }
        public string U_OR1 { get; set; }
        public string U_OR2 { get; set; }
        public string U_DelStatus { get; set; }
        public string U_AllocFrom { get; set; }
        public string U_AllocTo { get; set; }
        public string U_OrderType { get; set; }
        public string U_AllocPerc { get; set; }
        public string U_AllocFromName { get; set; }
        public string U_AllocToName { get; set; }
        public string U_Boxes { get; set; }
        public string U_TransactionType { get; set; }
        public string U_ShipmentDate { get; set; }
        public string U_Release_LaunchDate { get; set; }
        public string U_ProFormaInvoiceNo { get; set; }
        public string U_CredType { get; set; }
        public string U_TotalCBM { get; set; }
        public string U_TotalCartons { get; set; }
        public string U_LiquidationType { get; set; }
        public string U_ORNo { get; set; }
        public string U_EWTStatus { get; set; }
        public string U_EWTSubmitDate { get; set; }
        public string U_CheckStatus { get; set; }
        public string U_DateCleared { get; set; }
        public string U_Rating { get; set; }
        public string U_PONo { get; set; }
        public string U_GenAdd { get; set; }
        public string U_OrdrPromo { get; set; }
        public string U_OrdrRas { get; set; }
        public string U_TransfType { get; set; }
        public string U_SalesType { get; set; }
        public string U_Sent { get; set; }
        public string U_BudgetCode { get; set; }
        public string U_Release { get; set; }
        public string U_InspecDate { get; set; }
        public string U_OrderNo { get; set; }
        public string U_BudgetDesc { get; set; }
        public string U_CartonList { get; set; }
        public string U_Department { get; set; }
        public string U_OrRecDate { get; set; }
        public string U_AddID { get; set; }
        public string U_Reason { get; set; }
        public string U_VDesc { get; set; }
        public string U_VPla { get; set; }
        public string U_PaymentMode { get; set; }
        public string U_RFPType { get; set; }
        public string U_RequestType { get; set; }
        public string U_RetDate { get; set; }
        public string U_Purpose { get; set; }
        public string U_CancellationReason { get; set; }
        public string U_Designer { get; set; }
        public string U_MerchCoord { get; set; }
        public string U_TarRelease { get; set; }
        public string U_TargetWhs { get; set; }
        public string U_TrgetWhsName { get; set; }
        public string U_POUTNo { get; set; }
        public string U_ActualRecDate { get; set; }
        public string U_Backload { get; set; }
        public string U_Printed { get; set; }
        public string U_PlanDelDate { get; set; }
        public string U_SType { get; set; }
        public string U_GenChoose { get; set; }
        public string U_Allocated { get; set; }
        public string U_LiqRefNo { get; set; }
        public string U_PeriodFrom { get; set; }
        public string U_PeriodTo { get; set; }
        public string U_ShipmentNo { get; set; }
        public string U_CSTNo { get; set; }
        public string U_WaybillNo { get; set; }
        public string U_ItemRemarks { get; set; }
        public string U_DateTime { get; set; }
        public string U_CompanyTIN { get; set; }
        public string U_TransferType { get; set; }
        public string U_Area { get; set; }
        public string U_ForCancellation { get; set; }
        public string U_AppRemarks { get; set; }
        public string U_DoneeName { get; set; }
        public string U_DocType { get; set; }
        public string U_SuppAssessment { get; set; }
        public string U_BPSample { get; set; }
        public string U_ITNo { get; set; }
        public string U_Countered { get; set; }
        public string U_Barcode { get; set; }
        public string U_UPCTag { get; set; }
        public string U_PriceTag { get; set; }
        public string U_CancelDate { get; set; }
        public string U_CardCode { get; set; }
        public string U_CardName { get; set; }
        public string U_ETD { get; set; }
        public string U_ETA { get; set; }
        public string U_Season { get; set; }
        public string U_Brand { get; set; }
        public string U_PromoTotal { get; set; }
        public string U_IssueType { get; set; }
        public string U_CartonNo { get; set; }
        public string U_VendorCode { get; set; }
        public string U_VendorName { get; set; }
        public string U_ChainName { get; set; }
        public string U_DocRef { get; set; }
        public string U_Ref1 { get; set; }
        public string U_Ref2 { get; set; }
        public string U_GroupCode { get; set; }
        public string U_DRDesc { get; set; }
        public string U_PromoDate { get; set; }
        public string U_RunID { get; set; }
        public string U_RAS { get; set; }
        public string U_RegisteredName { get; set; }
        public string U_Discount { get; set; }
        public string U_CreateDate { get; set; }
        public string U_SentItemLogs { get; set; }
        public string U_PromoQuantity { get; set; }
        public string U_QuantityDifference { get; set; }
        public string U_TotalDifference { get; set; }
        public string U_DSRDate { get; set; }
        public string U_Drivers { get; set; }
        public string U_RunTotAmt { get; set; }
        public string U_RunQtyAmt { get; set; }
        public string U_AppTemplate { get; set; }
        public string U_ApprovalScenario { get; set; }
        public string U_BudgetEntry { get; set; }
        public string U_ApproverUserCode { get; set; }
        public string U_OrderTime { get; set; }
        public string U_OrderFlag { get; set; }
        public string U_ReceiverName { get; set; }
        public string U_BillingPeriod { get; set; }
        public string U_ShippingOption { get; set; }

    }
}
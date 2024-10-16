using System.Collections.Generic;

namespace PresenterLayer.Services
{
    class LinqAccess
    {
        public static List<gvSizes> frmSizes = new List<gvSizes>();
        public static List<gvITM_UDF> frmITM_UDF = new List<gvITM_UDF>();
        public static List<gvBarcodeItem> frmBarcodeItem = new List<gvBarcodeItem>();
        public static List<gvBarcodeBP> frmBarcodeBP = new List<gvBarcodeBP>();

        public static List<PrinterandLayout> PrinterLayout = new List<PrinterandLayout>();


        //CSV
        public static List<detailsCSV> _detailsCSV = new List<detailsCSV>();
        public static List<headerCSV> _headerCSV = new List<headerCSV>();
        public static List<docentryCSV> _docentryCSV = new List<docentryCSV>();
        public static List<sourceFields> _sourceFields = new List<sourceFields>();
        public static List<uploadSummary> _uploadSummary = new List<uploadSummary>();
        
        public class PrinterandLayout
        {
            public string DocNum { get; set; }
            public string Layout { get; set; }
            public double Printer { get; set; }
           
        }
        

        public class gvSizes
        {
            public string Code { get; set; }
            public string Sizes { get; set; }
            public bool Choose { get; set; }

        }
        public class gvITM_UDF
        {
            public string FieldCode { get; set; }
            public string FieldValue { get; set; }
        }
        public class gvBarcodeItem
        {
            public bool IsTick { get; set; }
            public string TableID { get; set; }
            public long DocEntry { get; set; }
            public string ItemCode { get; set; }
            public string Qty { get; set; }
            public string InCampaign { get; set; }
        }

        public class gvBarcodeBP
        {
            public bool IsTick { get; set; }
            public string ItemCode { get; set; }
        }

        public class detailsCSV
        {
            public string DocEntry { get; set; }
            public string CardCode { get; set; }
            public string CancelDate { get; set; }
            public string DocDueDate { get; set; }
            public string NumAtCard { get; set; }
            public string Location { get; set; }
            public string ItemCode { get; set; }
            public string Quantity { get; set; }
            public string UOM { get; set; }
            public string WhsCode { get; set; }
            public string Price { get; set; }
            public string Remarks { get; set; }
        }

        public class headerCSV
        {
            public string DocEntry { get; set; }
            public string CardCode { get; set; }
            public string CancelDate { get; set; }
            public string DocDueDate { get; set; }
            public string NumAtCard { get; set; }
            public string Location { get; set; }
            public string Remarks { get; set; }
        }
        public class docentryCSV
        {
            public string DocEntry { get; set; }
        }

        public class sourceFields
        {
            public string Num { get; set; }
            public string Id { get; set; }
            public string HeaderName { get; set; }
        }
        
        public class uploadSummary
        {
            public string Id { get; set; }
            public string DocEntry { get; set; }
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public string Status { get; set; }
            public string Description { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterLayer.Helper
{
    public class InvoiceHeaderModel
    {
        public static string oBPCode { get; set; }
        public static string oTaxGroup;
        public static string oPriceList;
        public static string oWhsCode;
        public static DateTime oDocDate;
        public static string oDocType;

        public static string selObjType = "";
        public static string oCode = "";
        public static string oDDW = "";
        public static string oSearchTable = "";
        public static string oLineNums = "";
        public static string oOrderEntry = "";
        public static string oSelectedDoc = "";

        public static List<DDWdocentryData> DDWdocentry { get; set; } = new List<DDWdocentryData>();
        public static List<InvoiceHeaderData> InvoiceHeader { get; set; } = new List<InvoiceHeaderData>();
        public class InvoiceHeaderData
        {

            public int Linenum { get; set; }
            public string ObjType { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public Double Quantity { get; set; }
            public Double Discount { get; set; }
            public Double Tax { get; set; }
            public Double LineTotal { get; set; }
            public Double Gross { get; set; }
        }

        public class DDWdocentryData
        {
            public int DocEntry { get; set; }
            public string BpCode { get; set; }
            public string OrderEntry { get; set; }
            public int LineEntry { get; set; }
        }
    }
}

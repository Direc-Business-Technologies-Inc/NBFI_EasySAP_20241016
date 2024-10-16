using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class SalesInvoiceHeaderModel
    {

        public static string oBPCode;
        public static string oTaxGroup;
        public static string oPriceList;
        public static string oWhsCode;
        public static DateTime oDocDate;

        public static string selObjType = "";
        public static string oCode = "";
        public static string oDDW = "";
        public static string oSearchTable = "";
        public static string oLineNums = "";
        public static string oOrderEntry = "";
        public static List<SalesInvoiceHeaderData> SalesInvoiceHeader { get; set; } = new List<SalesInvoiceHeaderData>();
        public class SalesInvoiceHeaderData
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

    }
}

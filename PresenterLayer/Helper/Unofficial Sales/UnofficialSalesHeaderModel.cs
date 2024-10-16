using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterLayer.Helper.Unofficial_Sales
{
    class UnofficialSalesHeaderModel
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
        public static string oDocType = "";

        public static List<UnofficialSalesHeaderData> UnofficialHeaderHeader { get; set; } = new List<UnofficialSalesHeaderData>();
        public class UnofficialSalesHeaderData
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

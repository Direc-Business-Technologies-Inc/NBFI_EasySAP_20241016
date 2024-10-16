using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterLayer.Helper
{
    public class SalesOrdersHeaderModel
    {
        public static string oBPCode { get; set; }
        public static string oTaxGroup;
        public static string oPriceList;
        public static string oWhsCode;
        public static DateTime oDocDate;
        public static string oDocType;

        public static List<SalesOrdersHeaderData> SalesOrderHeader { get; set; } = new List<SalesOrdersHeaderData>();
        public class SalesOrdersHeaderData
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

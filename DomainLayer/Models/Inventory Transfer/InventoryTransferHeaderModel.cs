using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class InventoryTransferHeaderModel
    {
        public static string oWhsCode;
        public static string oToWhsCode;
        public static string oAddressID;
        public static string selObjType = "";
        public static string oCode = "";
        public static string oDDW = "";
        public static string oSearchTable = "";
        public static string oLineNums = "";
        public static string oOrderEntry = "";
        public static string oBPCode = "";
        public static string oArea;
		public static string oSeries;
		public static string CardCode;

		public static List<xDocHeader> _DocHeader = new List<xDocHeader>();
        public static List<DDWdocentryData> DDWdocentry { get; set; } = new List<DDWdocentryData>();

        public class xDocHeader
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
            public string BpCode2 { get; set; }
        }
    }
}

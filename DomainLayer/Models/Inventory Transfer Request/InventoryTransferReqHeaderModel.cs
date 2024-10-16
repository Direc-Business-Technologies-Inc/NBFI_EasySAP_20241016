using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Inventory_Transfer_Request
{
    public class InventoryTransferReqHeaderModel
    {
        public static string oWhsCode;
        public static string oToWhsCode;
        public static string oAddressID;
        public static string oBPCode = "";
        public static string oTransferType;
        public static string oArea;

        public static List<xDocHeader> _DocHeader = new List<xDocHeader>();
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
    }
}

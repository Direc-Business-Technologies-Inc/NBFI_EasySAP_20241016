using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.OutgoingPackingList
{
    public class OutgoingPackingListModel
    {
        public static List<OutgoingPackingList> packinglist { get; set; } = new List<OutgoingPackingList>();

        public class OutgoingPackingList
        {
            public int Index { get; set; }
            public string SortCode { get; set; }
            public string DocoumnetNumber { get; set; }
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string Brand { get; set; }
            public string Size { get; set; }
            public string Color { get; set; }
            public string Barcode { get; set; }
            public string Cost { get; set; }
            public string Indication { get; set; }
            public string Data { get; set; }
            public string Department { get; set; }
            public string Status { get; set; }
            public string Available { get; set; }
        }

        public class OutgoingPackingList1
        {
            public int Index { get; set; }
            public string SortCode { get; set; }
            public string DocoumnetNumber { get; set; }
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string Brand { get; set; }
            public string Size { get; set; }
            public string Color { get; set; }
            public string Barcode { get; set; }
            public string Cost { get; set; }
            public string Indication { get; set; }
            public string Data { get; set; }
            public string Department { get; set; }
        }
    }
}

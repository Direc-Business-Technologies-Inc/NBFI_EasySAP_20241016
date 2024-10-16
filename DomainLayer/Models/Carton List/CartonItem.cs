using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class CartonItem
    {

        public static List<Item> items = new List<Item>();

        public class Item
        {
            public int Index { get; set; }

            public string ItemCode { get; set; }

            public string Description { get; set; }

            public string Quantity { get; set; }

            public string QuantityInnerBox { get; set; }

            public string BasedDocEntry { get; set; }

            public string BasedDocType { get; set; }
        }
    }
}

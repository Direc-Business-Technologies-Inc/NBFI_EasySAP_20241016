using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Inventory.Allocation_Wizard
{
    public class SelectionModel
    {
        public static List<SelectionData> Selection { get; set; } = new List<SelectionData>();
        public class SelectionData
        {
            public bool Choose { get; set; }
            public string TableID { get; set; }
            public string ID { get; set; }
            public string Type { get; set; }

        }

    }
}

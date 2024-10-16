using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class MappingList
    {
        public static List<MapField> fields = new List<MapField>();

        public class MapField
        {
            public string SapField { get; set; }
            public string Type { get; set; }
            public int RowStart { get; set; }
            public int ColumnStart { get; set; }
            public string Flow { get; set; }
            public int RowInterval { get; set; }
            public int ColumnInterval { get; set; }
        }
    }
}

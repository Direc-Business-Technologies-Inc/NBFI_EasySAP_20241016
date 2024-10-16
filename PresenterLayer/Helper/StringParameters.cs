using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer._03_Repository
{
    public class StringParameters
    {
        public List<ItemCondtionParameter> ItemCondition = new List<ItemCondtionParameter>();

        public class ItemCondtionParameter
        {
            public string Query { get; set; }

            public string Column { get; set; }

            public string SearchKeyword { get; set; }

            public string Brand { get; set; }

            public string Department { get; set; }

            public string SubDepartment { get; set; }

            public string Category { get; set; }

            public string SubCategory { get; set; }

            public string Style { get; set; }

            public string ParentColor { get; set; }

            public string Color { get; set; }

            public string ParentSize { get; set; }

            public string Size { get; set; }
        }
    }
}

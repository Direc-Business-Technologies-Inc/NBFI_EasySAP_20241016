using DomainLayer;
using System.Collections.Generic;

namespace DirecLayer
{
    public class DataContextList
    {
        public List<ColumnList> columnList { get; set; } = new List<ColumnList>();
        public List<TemplateFields> templateFields { get; set; } = new List<TemplateFields>();
        public List<Header> headers { get; set; } = new List<Header>();
        public List<Row> rows { get; set; } = new List<Row>();
        public List<CartonManagement> cartonManagement { get; set; } = new List<CartonManagement>();
        public List<CartonManagementRow> cartonManagementRow { get; set; } = new List<CartonManagementRow>();
        public List<CartonList> cartonLists { get; set; } = new List<CartonList>();
        public List<CartonListRow> cartonListRows { get; set; } = new List<CartonListRow>();
        public static List<ErrorIds> GetErrorIds { get; set; } = new List<ErrorIds>();
    }
}

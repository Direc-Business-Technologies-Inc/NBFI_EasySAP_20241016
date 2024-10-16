using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class CartonListRow
    {
        public string DocEntry { get; set; }
        public string CartonNo { get; set; }
        public string DocRef { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Remark { get; set; }
        public string VendorCode { get; set; }
    }

    public class ErrorIds
    {
        public string UploadType { get; set; }
        public int LineID { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CartonNo { get; set; }
        public int RowCount { get; set; }
        public string DocEntry { get; set; }
        public string DocRef { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Remarks { get; set; }
        public string URemarks { get; set; }
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public string ErrMsg { get; set; }
        public string Uploaded { get; set; }

    }
}

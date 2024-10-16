using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class CartonManagement
    {
        public string Uploaded { get; set; } = "Yes";
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public string DocEntry { get; set; }
        public string DocNo { get; set; }
        public string CartonNo { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ChainName { get; set; }
        public string DocRef { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string TransactionType { get; set; }
        public string TargetWH { get; set; }
        public string LastWH { get; set; }
        public string Date { get; set; }
        public string GroupCode { get; set; }
    }
}

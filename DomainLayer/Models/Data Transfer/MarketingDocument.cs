using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class MarketingDocument
    {
        public static List<MarketingDocumentHeaders> DocHeader = new List<MarketingDocumentHeaders>();
        public static List<MarketingDocumentLines> DocLines = new List<MarketingDocumentLines>();
        public static List<InventoryCounting> InventoryCounting = new List<InventoryCounting>();
        public static List<InventoryCountingRow> InventoryCountingRow = new List<InventoryCountingRow>();
    } 
}

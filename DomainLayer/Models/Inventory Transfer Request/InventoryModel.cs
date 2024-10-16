using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Inventory_Transfer_Request
{
    public class InventoryModel
    {
        public static List<InventoryTransferRequest> ITRdocument = new List<InventoryTransferRequest>();

        public static List<InventoryTransfer> ITdocument = new List<InventoryTransfer>();

        public class InventoryTransferRequest
        {
            public int Index { get; set; }
            public string ItemNo { get; set; }
            public string BarCode { get; set; }
            public string ItemDescription { get; set; }
            public string StyleCode { get; set; }
            public string Style { get; set; }
            public string ColorCode { get; set; }
            public string Color { get; set; }
            public string Size { get; set; }
            public double Quantity { get; set; }
            public int UomEntry { get; set; }
            public string UoM { get; set; }
            public string Warehouse { get; set; }
            public string GLAccount { get; set; }
            public string GLAccountName { get; set; }
            public string OpenQty { get; set; }
            public string ChainPricetag { get; set; }
            public string ChainDescription { get; set; }
            public string PricetagCount { get; set; }
            public double UnitPrice { get; set; }
            public double UnitPriceRaw { get; set; }
            public double UnitPricePerPCS { get; set; }
            public double GrossPricePerPCS { get; set; }
            public double DiscountPerc { get; set; }
            public double DiscAmount { get; set; }
            public string TaxCode { get; set; }
            public double TaxRate { get; set; }
            public double TaxAmount { get; set; }
            public double GrossPrice { get; set; }
            public double GrossPriceRaw { get; set; }
            public double TotalLC { get; set; }
            public double TotalLCRaw { get; set; }
            public double GrossTotalLC { get; set; }
            public string Project { get; set; }
            public string Department { get; set; }
            public string BrandCode { get; set; }
            public string Brand { get; set; }
            public string Remarks { get; set; }
        }

        public class InventoryTransfer
        {
            public int Index { get; set; }
            public string ItemNo { get; set; }
            public string BarCode { get; set; }
            public string ItemDescription { get; set; }
            public string BrandCode { get; set; }
            public string Brand { get; set; }
            public string StyleCode { get; set; }
            public string Style { get; set; }
            public string ColorCode { get; set; }
            public string Color { get; set; }
            public string Size { get; set; }
            public int PoQuantity { get; set; }
            public string TotalItemQuantityinCarton { get; set; }
            public string ReceivedQuantity { get; set; }
            public int UomEntry { get; set; }
            public string UoM { get; set; }
            public string Warehouse { get; set; }
            public string OpenQty { get; set; }
            public string ChainPricetag { get; set; }
            public string ChainDescription { get; set; }
            public string PricetagCount { get; set; }
            public string Project { get; set; }
            public string Remarks { get; set; }
            public double DiscountPerc { get; set; }
            public double DiscAmount { get; set; }
            public string TaxCode { get; set; }
            public double TaxRate { get; set; }
            public double TaxAmount { get; set; }
            public double UnitPrice { get; set; }
            public double GrossPrice { get; set; }
            public double GrossPriceRaw { get; set; }
            public double TotalLC { get; set; }
            public double GrossTotalLC { get; set; }
        }
        
    }
}

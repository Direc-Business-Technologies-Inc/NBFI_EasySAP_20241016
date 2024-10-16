using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class InventoryTransferItemsModel
    {
        public static List<ITitemsData> ITitems { get; set; } = new List<ITitemsData>();

        public class ITitemsData
        {
            public string SortCode { get; set; }
            public int Linenum { get; set; }
            public string ObjType { get; set; }
            public string BaseEntry { get; set; }
            public string BaseLine { get; set; }
            public string BaseType { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string BrandCode { get; set; }
            public string Brand { get; set; }
            public string StyleCode { get; set; }
            public string Style { get; set; }
            public string ColorCode { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public string Size { get; set; }
            public string BarCode { get; set; }
            public Double Quantity { get; set; }
            public Double UnitPrice { get; set; } //VAT EX
            public Double GrossPrice { get; set; } //VAT INC
            public Double DiscountPerc { get; set; }
            public Double DiscountAmount { get; set; }
            public string TaxCode { get; set; }
            public Double TaxRate { get; set; }
            public Double TaxAmount { get; set; }
            public string FWhsCode { get; set; }
            public string TWhsCode { get; set; }
            public Double LineTotal { get; set; }
            public Double GrossTotal { get; set; }
            public Double Available { get; set; }
            public string Company { get; set; }
            public string CompanyCode { get; set; }
            public string SKU { get; set; }
            public Double LineTotalManual => Quantity * UnitPrice;
            public Double VatAmountManual => LineTotalManual * (TaxRate / 100) - ((LineTotal * (TaxRate / 100)) * (0 / 100));
            public Double GrossTotalManual => (LineTotal + VatAmountManual) - 0;
            public string InventoryUOM { get; set; }

            public string Chain { get; set; }
            public string ChainDescription {  get; set; }
        }
    }
}

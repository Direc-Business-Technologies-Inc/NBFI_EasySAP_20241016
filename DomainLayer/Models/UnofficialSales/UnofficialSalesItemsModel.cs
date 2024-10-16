using System;
using System.Drawing;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DomainLayer
{
    public class UnofficialSalesItemsModel
    {
        public static List<DeliveryItemsData> DeliveryItems { get; set; } = new List<DeliveryItemsData>();
        public static List<UnofficialSalesItemsData> UnofficialItems { get; set; } = new List<UnofficialSalesItemsData>();

        public class UnofficialSalesItemsData
        {
            public string SortCode { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public double Price { get; set; }
            public double DiscPerc { get; set; }
            public double DiscAmount { get; set; }
            public double Quantity { get; set; }
            public double Total { get; set; }
            public int index { get; set; }
            public string disc { get; set; }
            public bool positive { get; set; }
            public double GrossPrice { get; set; }
            public double PriceBeforeDiscount { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public string Size { get; set; }
            public string ItemProperty { get; set; }
        }

        public class DeliveryItemsData
        {
            public int Linenum { get; set; }
            public string ObjType { get; set; }
            public string BaseEntry { get; set; }
            public string BaseLine { get; set; }
            public string BaseType { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string Style { get; set; }
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
            public Double LineTotalManual => Quantity * UnitPrice;
            public Double VatAmountManual => LineTotalManual * (TaxRate / 100) - ((LineTotal * (TaxRate / 100)) * (0 / 100));
            public Double GrossTotalManual => (LineTotal + VatAmountManual) - 0;
            public Double PriceAfterDisc { get; set; }
            public bool Selected { get; set; }
        }

    }
}

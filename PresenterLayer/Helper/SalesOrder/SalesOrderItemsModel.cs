using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterLayer.Helper.SalesOrder
{
    public class SalesOrderItemsModel
    {
        public static List<SalesOrderItemsData> SalesOrderItems { get; set; } = new List<SalesOrderItemsData>();

        public class SalesOrderItemsData
        {

            public int Linenum { get; set; }
            public string ObjType { get; set; }
            public string BaseEntry { get; set; }
            public string BaseLine { get; set; }
            public string BaseType { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string Brand { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public string Size { get; set; }
            public string BarCode { get; set; }
            public double Quantity { get; set; }
            public double UnitPrice { get; set; } //VAT EX
            public double GrossPrice { get; set; } //VAT INC
            public double EffectivePrice { get; set; }
            public double EmpDiscountPerc { get; set; }
            public double DiscountPerc { get; set; } = 0;
            public double DiscountAmount { get; set; }
            public string TaxCode { get; set; }
            public double TaxRate { get; set; }
            public double TaxAmount { get; set; }
			public double TotalLCRaw { get; set; }
            public string FWhsCode { get; set; }
            public string TWhsCode { get; set; }
            public double LineTotal { get; set; }
            public double GrossTotal { get; set; }
            public double Available { get; set; }
            public double LineTotalManual => Quantity * UnitPrice;
            public double VatAmountManual => LineTotalManual * (TaxRate / 100) - ((LineTotal * (TaxRate / 100)) * (0 / 100));
            public double GrossTotalManual => (LineTotal + VatAmountManual) - 0;
            public double PriceAfterDisc { get; set; }
            public bool Selected { get; set; }
            public string SKU { get; set; }
            public double OrderedQty { get; set; }
        }
    }
}

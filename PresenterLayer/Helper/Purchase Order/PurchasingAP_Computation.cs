using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class PurchasingAP_Computation
    {
        public double PercentConverter(object value)
        {
            double a = 0D;

            if (value != null)
            {
                value = value.ToString() == "" ? 0 : value;

                a = Convert.ToDouble(value) / 100;
            }

            return a;
        }

        public double ComputeUnitPrice(double grossPrice, double tax, string taxGroup)
        {
            double result = 0;

            if (taxGroup != "OT0" && tax != 0)
            {
                result = grossPrice / tax;
            }
            else
            {
                result = grossPrice;
            }

            return Convert.ToDouble(Math.Round(result, 3).ToString("0.00"));
        }

        public double ComputeUnitPriceRaw(double grossPrice, double tax, string taxGroup)
        {
            double result = 0;

            if (taxGroup != "OT0" && tax != 0)
            {
                result = grossPrice / tax;
            }
            else
            {
                result = grossPrice;
            }

            return result;
        }

        public double MultipleByQty(double price, double qty)
        {
            double result = price * qty;

            return Convert.ToDouble(Math.Round(result, 3).ToString("0.00"));
        }

        public double ComputeGrossPrice(double unitPrice, double tax, string taxGroup)
        {
            double result = 0;

            if (taxGroup != "OT0" && tax != 0)
            {
                result = unitPrice * tax;
            }
            else
            {
                result = unitPrice;
            }

            result = Convert.ToDouble(Math.Round(result, 3).ToString("0.00"));

            return result;
        }

        public double ComputeTax(double totalLC, double taxRate, double discountAmount)
        {
            double result = 0;
            
            result = totalLC * taxRate;

            result = (totalLC - discountAmount - result) * -1;

            return result;
        }

        public double ComputeDiscountAmount(double discountValue, double unitPrice, double quantity)
        {
            double result = 0;

            result = discountValue * (unitPrice * quantity);

            return Convert.ToDouble(Math.Round(result, 3).ToString("0.00"));
        }

        public double ComputeTotalLc(double quantity, double unitprice, double discount)
        {
            double result = 0;

            result = (quantity * unitprice) - discount;

            return result;
        }

        public double ComputeRowTotals(double quantity, double price, double discount)
        {
            double result = 0;

            result = (quantity * price) - discount;

            return result;
        }

        public double ComputeGrossTotalLc(double totalLc, double tax)
        {
            double result = 0;

            result = totalLc + tax;
            
            return Convert.ToDouble(Math.Round(result, 3).ToString("0.00"));
        }

        // footer

        public double ComputeFooterDiscount(double totalBefDis, double discount)
        {
            double result = 0;

            result = totalBefDis * discount;

            return result;
        }

        public double ComputeFooterTax(double totalBefDis, double discount, double tax)
        {
            double result = 0;

            result = (totalBefDis - discount) * tax;

            if (result == 0)
            {
                result = (totalBefDis - discount);
            }
            return result;
        }

        public double ComputeTotalPaymentDue(double totalBefDi, double discount, double tax)
        {
            double result = 0;

            result = (totalBefDi - discount) + tax;

            if (result == 0)
            {
                result = (totalBefDi - discount);
            }

            return result;
        }
    }
}

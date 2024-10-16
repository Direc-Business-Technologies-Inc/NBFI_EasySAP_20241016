using System;
using System.Collections.Generic;
//using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
//using System.Data;
using DirecLayer;
using System.Data;
using System.Windows.Forms;
using DirecLayer;

namespace zDeclare
{
    public class DECLARE
    {
        //TEMP DOCUMENT
        public static List<xDocHeader> _DocHeader { get; set; } = new List<xDocHeader>();
        public static List<xDocItems> _DocItems = new List<xDocItems>();
        public static List<xConcession> _DocConcession = new List<xConcession>();
        public static List<items> _items = new List<items>();
        public static List<warehouse> _warehouse = new List<warehouse>();
        public static List<ErrorLogs> _error = new List<ErrorLogs>();
        //public static List<CartonList> getCarton = new List<CartonList>();
        //Declare LIST<> Sales Order Items (BARCODE)
        public static List<SalesOrderItems> so_items = new List<SalesOrderItems>();
        //Declare LIST<> Items for Printing
        public static List<Print> print_items = new List<Print>();

        #region INVENTORY
        //Declare LIST<> Items for ITR Parent Items
        public static List<ITR_ParentItems> itr_parent_items = new List<ITR_ParentItems>();
        //Declare LIST<> Items for ITR
        public static List<ITR_Items> itr_items = new List<ITR_Items>();

        #endregion
        #region MARKETING_DOCS
        //public static List<Marketing_Parent> marketing_parent = new List<Marketing_Parent>();
        //public static List<Marketing_Items> marketing_items = new List<Marketing_Items>();
        #endregion

        public static List<UDF> udf = new List<UDF>();
        public static List<MultipleSelection> _multipleSelection = new List<MultipleSelection>();

        public static void GetSapUserID(string ES_UserID, string ES_Password, out string SAP_UserID, out string SAP_Password, out bool AccessGranted)
        {
            try
            {
                //MessageBox.Show(DataAccess.conStr("HANA"));
                var q = $"SELECT U_UserID [SapUser],U_SapPassword [SapPassword],(LEFT(firstName,1) + '. ' + lastName) [Name],(firstName + ' ' + lastName) [CompleteName] FROM OHEM Where U_User = '{ES_UserID}' and U_Password = '{ES_Password}'";
                //var q = $"SELECT U_User [SapUser],U_SapPassword [SapPassword],(LEFT(firstName,1) + '. ' + lastName) [Name] FROM OHEM Where U_User = '{ES_UserID}' and U_Password = '{ES_Password}'";
                var dt = DataAccess.Select(DataAccess.conStr("HANA"), q);

                if (dt.Rows.Count > 0)
                {
                    if (dtNull(dt, 0, "SapUser", "") != "" && dtNull(dt, 0, "SapPassword", "") != "")
                    {
                        SAP_UserID = dt.Rows[0][0].ToString();
                        SAP_Password = dt.Rows[0][1].ToString();
                        PublicStatic.oEmployeeName = dt.Rows[0][2].ToString();
                        PublicStatic.oEmployeeCompleteName = dt.Rows[0][3].ToString();
                        AccessGranted = true;
                        //ver 1.4.1.25
                        PublicStatic.oLicenseID = SAP_UserID;
                    }
                    else
                    {
                        SAP_UserID = "";
                        SAP_Password = "";
                        PublicStatic.oEmployeeName = "";
                        AccessGranted = false;
                    }
                }
                else
                {
                    AccessGranted = false;
                    SAP_UserID = "";
                    SAP_Password = "";
                    PublicStatic.oEmployeeName = "";
                }
            }
            catch (Exception ex)
            {
                AccessGranted = false;
                SAP_UserID = "";
                SAP_Password = "";
                PublicStatic.oEmployeeName = "";
            }
        }

        public static double Discount(string ItemCode, string CardCode, DateTime DocDate)
        {
            try
            {
                string query = $"SELECT * FROM ITEM_PRICE2('{ItemCode}','{CardCode.Replace("'", "'''")}','{DocDate.ToString("MM/dd/yyyy")}')";

                DataTable dt = DataAccess.SelectNoConvert(DataAccess.conStr("HANA"), query);

                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDouble(DECLARE.dtNull(dt, 0, "Discount", "0"));
                }
                else
                    return 0;
            }
            catch (Exception ex)
            { return 0; }
        }
        public static double Price(string ItemCode, string CardCode, DateTime DocDate)
        {
            try
            {
                string query = $"SELECT * FROM ITEM_PRICE2('{ItemCode}','{CardCode.Replace("'", "'''")}','{DocDate.ToString("yyyyMMdd")}')";

                DataTable dt = DataAccess.SelectNoConvert(DataAccess.conStr("HANA"), query);

                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDouble(DECLARE.dtNull(dt, 0, "Price", "0"));
                }
                else
                    return 0;
            }
            catch (Exception ex)
            { return 0; }
        }
        public static void GetEmployeeCode(string user)
        {
            try
            {
                var q = $"SELECT empID FROM OHEM Where U_UserID = '{user}'";
                var dt = DataAccess.Select(DataAccess.conStr("HANA"), q);

                if (dtNull(dt, 0, "empID", "") != "")
                {
                    PublicStatic.oEmployeeCode = dt.Rows[0][0].ToString();
                }
                else
                {
                    MessageBox.Show("User ID not registered to the Database.", "EasySAP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("User ID not registered to the Database.", "EasySAP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        public int CountCharacter(string value, char ch)
        {
            int count = 0;
            foreach (Char c in value)
            {
                if (c == ch)
                {
                    count += 1;
                }
            }
            return count;
        }
        public class MultipleSelection
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
        //FOR NEW VERSION
        #region DOCUMENT
        public class xDocHeader
        {
            public int Linenum { get; set; }
            public string ObjType { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public Double Quantity { get; set; }
            public Double Discount { get; set; }
            public Double Tax { get; set; }
            public Double LineTotal { get; set; }
            public Double Gross { get; set; }
        }

        public class xConcession
        {
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public string ConCerti { get; set; }
        }

        public class xDocItems
        {
            public int Linenum { get; set; }
            public string ObjType { get; set; }
            public string BaseEntry { get; set; }
            public string BaseLine { get; set; }
            public string BaseType { get; set; }
            public string ItemCode { get; set; }
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
            public bool Selected { get; set; }
        }

        #endregion 
        #region MARKETING DOCS
        public class Marketing_Parent
        {
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public int Quantity { get; set; }
            public Double LineTotal { get; set; }
            public Double Discount { get; set; }
            public Double Tax { get; set; }
            public Double Gross { get; set; }
        }
        public class Marketing_Items
        {
            public string ItemCode { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public int Quantity { get; set; }
            public string WhsCode { get; set; }
            public string TaxCode { get; set; }
            public Double TaxAmount { get; set; }
            public Double TaxRate { get; set; }
            public Double Discount { get; set; }
            public Double UnitPrice { get; set; }
            public Double LineTotal { get; set; }
            public Double GrossTotal { get; set; }
        }
        #endregion
        #region BARCODE
        //TEMP CONTAINER FOR ITEMS IN SALES ORDER
        public class SalesOrderItems
        {
            public string BaseLine { get; set; }
            public string BaseEntry { get; set; }
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
            public string DelDate { get; set; }
        }
        //TEMP CONTAINER FOR ITEMS TO BE PRINTED
        public class Print
        {

            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string FrgnName { get; set; }
            public string Price { get; set; }
            public int Quantity { get; set; }
            public int Percentage { get; set; }
        }
        #endregion
        #region INVENTORY
        //TEMP CONTAINER FOR ITR ITEMS
        public class StockTransfer
        {
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public int Quantity { get; set; }
        }
        public class StockTransfer_Items
        {
            public string ItemCode { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public string BarCode { get; set; }
            public Double Quantity { get; set; }
            public Double PriceAfVat { get; set; }
            public Double PriceBefDi { get; set; }
            public Double NetDisc { get; set; }
            public Double LineTotal { get; set; }
            public Double VatAmount { get; set; }
            public Double Total { get; set; }
            public string WhsCode { get; set; }
            public string ToWhsCode { get; set; }
        }


        public class ITR_Items
        {
            public string ItemCode { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public int Quantity { get; set; }
            public string WhsCode { get; set; }
            public string ToWhsCode { get; set; }
        }
        public class ITR_ParentItems
        {
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public int Quantity { get; set; }
        }
        public class Parent_IT
        {
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public int Quantity { get; set; }
        }
        public class IT_Items
        {
            public string ItemCode { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public string Section { get; set; }
            public int Quantity { get; set; }
            public string WhsCode { get; set; }
            public string ToWhsCode { get; set; }
        }
        #endregion
        #region OTHERS
        public class UDF
        {
            public string ObjCode { get; set; }
            public string FieldCode { get; set; }
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
        }
        public class items
        {
            public bool selected { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
        }
        public class warehouse
        {
            public string WhsCode { get; set; }
            public string WhsName { get; set; }
        }
        public class ErrorLogs
        {
            public string ErrorCode { get; set; }
            public string ErrorDate { get; set; }
            public string ErrorMessage { get; set; }
        }
        #endregion
        #region OINC

        public static List<xOINC> _OINC = new List<xOINC>();
        internal static object oPurchase;

        public static object Properties { get; private set; }

        public class xOINC
        {
            public string oDocEntry { get; set; }
            public string oWhsCode { get; set; }
            public string oItemCode { get; set; }
            public string oQty { get; set; }
        }


        #endregion
        public static string HanaQuery(string query)
        {
            string hdb_query = "";
            hdb_query = query.Replace("%", "\"");
            return hdb_query;
        }
        public static string Replace(DataGridViewRow row, string ColName, string newvalue)
        {
           
            double x;
            var value = row.Cells[ColName].Value;
           
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                value = newvalue;
            }

            if (double.TryParse(value.ToString(), out x) != false)
            {
                //is numeric
                return value.ToString();
            }
            else
            {
                return newvalue;
            }
        }
        public static string DataGridReplace(DataGridView gv, int row, string ColName, string newvalue)
        {
            int n;
            double x;
            bool isNumeric = int.TryParse(ColName, out n);
            object value;
            if (isNumeric == false)
            {
                value = gv.Rows[row].Cells[ColName].Value;
            }
            else
            {
                value = gv.Rows[row].Cells[n].Value;
            }

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                value = newvalue;
            }
            if (double.TryParse(value.ToString(), out x) != false)
            {
                return value.ToString();
            }
            else
            {
                return newvalue.ToString();
            }

        }
        public static string _DataGridReplace(DataGridView gv, int row, string ColName, string newvalue)
        {
            int n;
            double x;
            bool isNumeric = int.TryParse(ColName, out n);
            object value;
            if (isNumeric == false)
            {
                value = gv.Rows[row].Cells[ColName].Value;
            }
            else
            {
                value = gv.Rows[row].Cells[n].Value;
            }

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                value = newvalue;
            }

            return value.ToString();

        }
        public static string dtNull(DataTable dt, int row, string ColName, string newvalue)
        {
            var value = dt.Rows[row][ColName];
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                value = newvalue;
            }
            return value.ToString();
        }
        /// <summary>
        /// Layout for datagridview
        /// </summary>
        /// <param name="dgv"></param>
        public static void dataGridLayout(DataGridView dgv)
        {

            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.MultiSelect = false;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
        }

        //HEADER COMPUTATION
        public static void ComputeTotal( DataGridView dgv, [Optional] TextBox txtDiscountPercent, [Optional] out string TotalBefDisc, [Optional] out string TotalQty, 
                                         [Optional] out string TotalDisc, [Optional] out string TotalAftDisc, [Optional] out string TotalTax)
        {
            double net = 0;
            double disc = 0;
            double disc2 = 0;
            double tax = 0;
            double total = 0;

            double disc_total = 0;
            double disc_tax = 0;
            double disc_net = 0;

            double _total = 0;
            double _tax = 0;
            double _disc = 0;
            double _totalqty = 0;

            //IF DISCOUNT APPLIES TO WHOLE DOCUMENT
            if (txtDiscountPercent != null)
            {
                if (txtDiscountPercent.Text != "")
                {
                    disc2 = Convert.ToDouble(txtDiscountPercent.Text);
                }
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                net += Convert.ToDouble(Replace(row, "Line Total", "0.00"));
                //disc += Convert.ToDouble(DECLARE.Replace(row, "Discount", "0.00"));
                tax += Convert.ToDouble(Replace(row, "Tax", "0.00"));
                total += Convert.ToDouble(Replace(row, "Gross Total", "0.00"));
                _totalqty += Convert.ToDouble(Replace(row, "Quantity", "0.00"));
            }

            if (disc != 0)
            {
                disc_total = (total * (disc2 / 100));
                disc_tax = (tax * (disc2 / 100));
                disc_net = (net * (disc2 / 100));
            }
            else
            {
                disc_total = 0;
                disc_tax = 0;
                disc_net = (net * (disc2 / 100));
            }

            _total = total - disc_total;
            _tax = tax - disc_tax;
            _disc = disc_net + disc;


            //OUT PARAMETERS
            TotalBefDisc = net.ToString("#,##0.##");
            TotalDisc = _disc.ToString("#,##0.##");
            TotalTax = _tax.ToString("#,##0.##");
            TotalAftDisc = _total.ToString("#,##0.##");
            TotalQty = _totalqty.ToString("#,##0.##");
        }
        
        public static string ArrangeUDF(string UDFvalues)
        {
            string ArrangedUDF = "";

            var ArrayUDF = UDFvalues.Split(',');
            int intCnt = 1;

            foreach (string i in ArrayUDF)
            {
                ArrangedUDF += $"WHEN {i} THEN {intCnt} ";
                intCnt++;
            }

            return ArrangedUDF;
        }
    }
}
using DirecLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PresenterLayer
{
    class OutGoingRepository
    {
        public DataGridView dgv { get; set; }
        SAPHanaAccess hana { get; set; }
        public OutGoingRepository()
        {
            hana = new SAPHanaAccess();
        }

        public void LinesColumn ()
        {
            var SortCode = new DataGridViewTextBoxColumn();
            SortCode.Name = "Sort Code";
            dgv.Columns.Add(SortCode);

            var DocNum = new DataGridViewTextBoxColumn();
            DocNum.Name = "Document No.";
            dgv.Columns.Add(DocNum);

            var ItemCode = new DataGridViewTextBoxColumn();
            ItemCode.Name = "Item Code";
            dgv.Columns.Add(ItemCode);

            var Description = new DataGridViewTextBoxColumn();
            Description.Name = "Description";
            dgv.Columns.Add(Description);

            var Quantity = new DataGridViewTextBoxColumn();
            Quantity.Name = "Quantity";
            dgv.Columns.Add(Quantity);

            var Brand = new DataGridViewTextBoxColumn();
            Brand.Name = "Brand";
            dgv.Columns.Add(Brand);

            var Color = new DataGridViewTextBoxColumn();
            Color.Name = "Color";
            dgv.Columns.Add(Color);

            var Size = new DataGridViewTextBoxColumn();
            Size.Name = "Size";
            dgv.Columns.Add(Size);

            var Barcode = new DataGridViewTextBoxColumn();
            Barcode.Name = "Barcode";
            dgv.Columns.Add(Barcode);
            
            var Cost = new DataGridViewTextBoxColumn();
            Cost.Name = "Cost";
            dgv.Columns.Add(Cost);

            var Indication = new DataGridViewTextBoxColumn();
            Indication.Name = "Indication";
            dgv.Columns.Add(Indication);

            var Data = new DataGridViewTextBoxColumn();
            Data.Name = "Data";
            dgv.Columns.Add(Data);
            
            var Department = new DataGridViewTextBoxColumn();
            Department.Name = "Department";
            dgv.Columns.Add(Department);

            var Index = new DataGridViewTextBoxColumn();
            Index.Name = "Index";
            Index.Visible = false;
            dgv.Columns.Add(Index);

        }

        public List<string> ModalShow(string parameter, string parameter1, string title)
        {
            List<string> list = new List<string>();

            var fS = new frmSearch2();
            fS.oSearchMode = parameter;
            fS.oFormTitle = title;

            if (parameter1 != string.Empty)
            {
                frmSearch2.Param1 = parameter1;
            }

            fS.ShowDialog();

            if (fS.oCode != null)
            {
                list.Add(fS.oCode);
                list.Add(fS.oName);
            }

            return list;
        }

        public DataTable Select(string query)
        {
            var dt = hana.Get(query);
            return dt;
        }

        public string GenerateDocNum()
        {
            var row = Select("SELECT TOP 1 (DocNum + 1) [DocNum] FROM [@OPKL] Order By DocNum DESC").Rows;

            return row.Count > 0 ? row[0]["DocNum"].ToString() : "1";
        }

        public DataTable GetBpInfo(string supplierCode)
        {
            return Select("SELECT A.CardCode, A.CardName, A.Address" +
                         ", (SELECT min(Y.Street) [Address] FROM CRD1 Y Where Y.CardCode = A.CardCode And AdresType = 'S') [Address2] " +
                         $" FROM OCRD A WHERE A.CardCode = '{supplierCode}' ");
            //", (SELECT min(Y.Address) [Address] FROM CRD1 Y Where Y.CardCode = A.CardCode And AdresType = 'S') [Address2] " +
        }

        public DataTable GetItem(string docNum, string series, string table)
        {

            var query =  "SELECT T0.DocNum [Document No.], T2.ItemCode, T2.ItemName [Description], T2.CodeBars [Barcode],  " +
            
                            "T1.Quantity, " +
                            
                            $"T1.Quantity - ISNULL((SELECT SUM(Z.U_Quantity) FROM [@PKL1] Z WHERE Z.U_BaseType = '{table}' AND Z.U_BaseRef = '{series}' AND Z.U_DocNum = '{docNum}' AND Z.U_ItemCode = T2.ItemCode),0) [Available], " +
                            
                            "0.000000 [Cost], " +
                            
                            "T2.ItemName [Indication], '' [Data], " +
                
                            "(SELECT Name FROM [@OBND] Where Code = T2.U_ID001) [Brand], T2.U_ID002 [Department], T2.U_ID007 [Size], T2.U_ID011 [Color], T2.U_ID023 [SortCode] " +
                            
                            $"FROM  O{table} T0 " +
                            
                            $"INNER JOIN {table}1 T1 ON T1.DocEntry = T0.DocEntry " +
                            
                            "INNER JOIN OITM T2 ON T2.ItemCode = T1.ItemCode " +
                            
                            $"WHERE T0.DocNum = '{docNum}' AND T0.Series = '{series}' AND " +
                            
                            $"(T1.Quantity - ISNULL((SELECT SUM(Z.U_Quantity) FROM [@PKL1] Z WHERE Z.U_BaseType = '{table}' AND Z.U_BaseRef = '{series}' AND Z.U_DocNum = '{docNum}' AND Z.U_ItemCode = T2.ItemCode),0)) > 0" +
                            
                            "Order By DocNum";

            return Select(query);
        }

        public DataTable GetDrItem(string Dr, string table)
        {
            var query = "SELECT T0.DocNum [Document No.], T2.ItemCode, T2.ItemName [Description], T2.CodeBars [Barcode],  T1.Quantity, " +

                            $"T1.Quantity - ISNULL((SELECT SUM(Z.U_Quantity) FROM [@PKL1] Z WHERE Z.U_BaseType = '{table}' AND Z.U_BaseRef = 'DR' AND Z.U_BaseLine = '{Dr}' AND Z.U_ItemCode = T2.ItemCode),0) [Available], " +

                            "0.000000 [Cost], T2.ItemName [Indication], '' [Data], " +

                            "(SELECT Name FROM [@OBND] Where Code = T2.U_ID001)  [Brand], T2.U_ID002 [Department], T2.U_ID007 [Size], T2.U_ID011 [Color], T2.U_ID023 [SortCode] " +

                            $"FROM O{table} T0 INNER JOIN {table}1 T1 ON T1.DocEntry = T0.DocEntry " +

                            $"INNER JOIN OITM T2 ON T2.ItemCode = T1.ItemCode WHERE T0.U_DRNo = '{Dr}' AND " +

                             $"(T1.Quantity - ISNULL((SELECT SUM(Z.U_Quantity) FROM [@PKL1] Z WHERE Z.U_BaseType = '{table}' AND Z.U_BaseRef = 'DR' AND Z.U_BaseLine = '{Dr}' AND Z.U_ItemCode = T2.ItemCode),0)) > 0";

            return Select(query);
        }
        
        public DataTable loadHeader(string docEntry)
        {
            var query = $"SELECT Remark, U_PONo, U_CardName, U_ShipTo, U_Box, U_Length, U_Height, U_BranchCode, U_Shipper, U_SIDRNo," +

                            $" U_Department, U_CardCode, U_Weight, U_TotalBox, U_Width, U_Type, U_Date, U_DocRef, U_Brand FROM [@OPKL] WHERE DocEntry = '{docEntry}'";

            return Select(query);
        }

        public DataTable loadItems(string docEntry)
        {
            var query =  "select U_DocNum [Document No.], U_ItemCode [ItemCode], U_ItemName [Description], U_Barcode [Barcode], " +
                
                            "U_Quantity [Quantity], U_Cost [Cost],  U_Indication [Indication], U_Data [Data], U_Brand [Brand], U_Department [Department], " +
                            
                            $"'' [Color], '' [Size], '' [SortCode],U_Quantity [Available] FROM [@PKL1] WHERE DocEntry = '{docEntry}'";

            return Select(query);
        }

        public string AutomateShipTo(string dr, string table)
        {
            var query = $"SELECT TOP 1 Address FROM O{table} where U_DRNo = '{dr}'";

            return Select(query).Rows.Count > 0 ? Select(query).Rows[0][0].ToString() : "";
        }

        public string AutomateShipper(string bpcode, string brand)
        {
            var query = "SELECT T3.Name FROM OCPN T0 INNER JOIN CPN1 T1 ON T0.CpnNo = T1.CpnNo INNER JOIN CPN3 T2 ON T0.CpnNo = T2.CpnNo " +

                            "INNER JOIN [@CMP_INFO] T3 ON T2.U_Company = T3.Code INNER JOIN [@OBND] T4 ON T2.U_Brand = T4.Code " +

                            $"WHERE T0.U_CType = 'VC' AND T1.BpCode = '{bpcode}' AND T4.Name = '{brand}'";

            return Select(query).Rows.Count > 0 ? Select(query).Rows[0][0].ToString() : "";
        }
        
        public string AutomateBox (string docNum, string bpcode)
        {
            var query = $"SELECT Count(DocEntry) + 1  [Count] FROM [@OPKL] WHERE U_DocRef = '{docNum}' AND U_CardCode = '{bpcode}'";

            return Select(query).Rows.Count > 0 ? Select(query).Rows[0][0].ToString() : "";
        }
    }
}

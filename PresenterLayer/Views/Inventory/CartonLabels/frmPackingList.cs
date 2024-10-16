using MetroFramework.Forms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zDeclare;
using DirecLayer;
using PresenterLayer.Helper;
using MetroFramework;
using System.Text;
using ServiceLayer.Services;

namespace PresenterLayer
{
    public partial class frmPackingList : MetroForm
    {
        frmpackinglist_udf frmUDF;
        DataHelper helper { get; set; }
        SAPHanaAccess hana { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        public frmPackingList()
        {
            InitializeComponent();
            helper = new DataHelper();
            hana = new SAPHanaAccess();
            msSql = new SAPMsSqlAccess();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void frmPackingList_Load(object sender, EventArgs e)
        {
            btnViewReport.Enabled = false;
            ReadSeries();
        }

        void GetDetails(string Type, string DocNum)
        {
            try
            {
                var query = $"SELECT TOP 1 CAST(U_Length as Decimal(18,2)) as U_Length " +
                                ",CAST(U_Width as Decimal(18,2)) as U_Width " +
                                ",CAST(U_Height as Decimal(18,2)) as U_Height " +
                                ",CAST(U_Weight as Decimal(18,2)) as U_Weight " +
                                ",CAST(U_TotalBox as Decimal(18,2)) as U_TotalBox " +
                                $"FROM [@OPKL] Where U_Type = '{Type}' And U_SIDRNo = '{DocNum}' Order By DocEntry DESC";
                
                var dt1 = hana.Get(query);

                if (dt1.Rows.Count > 0)
                {
                    txtLength.Text = String.Format(Convert.ToDouble(dt1.Rows[0]["U_Length"]).ToString(), "#,##0.##");
                    txtShipper.Text = String.Format(Convert.ToDouble(dt1.Rows[0]["U_Width"]).ToString(), "#,##0.##");
                    txtWeight.Text = String.Format(Convert.ToDouble(dt1.Rows[0]["U_Weight"]).ToString(), "#,##0.##");
                    txtHeight.Text = String.Format(Convert.ToDouble(dt1.Rows[0]["U_Height"]).ToString(), "#,##0.##");
                    txtBoxof.Text = String.Format(Convert.ToDouble(dt1.Rows[0]["U_TotalBox"]).ToString(), "#,##0.##");
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void refresh(string oMode)
        {
            string sdata;
            string oTempType = txtTransType.Text;
            DataTable dt = null;

            if (txtTransType.Text == "SI")
            {
                string docentry; var fS = new frmSearchPackingList();

                switch (oMode)
                {

                    case "1":
                        CleanForm();
                        txtTransType.Text = oTempType.ToString();
                        fS.oSearchMode = "INVOICE_PICKLIST";
                        txtTransType.Text = oTempType.ToString();
                        fS.ShowDialog();

                        if (fS.oCode != null)
                        {
                            txtDocNum.Text = fS.oCode;
                            sdata = " select T0.U_PONo,T0.DocEntry,T0.CardCode,T0.CardName,T0.ShipToCode,T0.Address2  ";
                            sdata = sdata + " ,ISNULL((Select AliasName FROM OCRD WHERE CardCode = T0.CardCode),'') as wbvendercode ";
                            sdata = sdata + " from OINV T0 Where T0.U_SINo = '" + fS.oCode + "'";
                            dt = hana.Get(sdata);
                            
                            docentry = helper.ReadDataRow(dt, "DocEntry", "", 0);
                            txtPONum.Text = helper.ReadDataRow(dt, "U_PONo", "", 0);
                            txtShipto.Text = helper.ReadDataRow(dt, "Address2", "", 0);

                            GetDetails("SI", fS.oCode);


                            sdata = $" SELECT COUNT(BaseCard) as BaseCard FROM (SELECT COUNT(BaseCard) as BaseCard FROM (SELECT T1.BaseCard as BaseCard FROM INV1 T1 WHERE T1.DocEntry =  ";
                            sdata += $" (SELECT TOP 1 T0.DocEntry FROM OINV T0 WHERE T0.U_SINo = '{txtDocNum.Text}') GROUP BY T1.BaseCard) T0 GROUP BY T0.BaseCard) T0 ";
                            
                            dt = hana.Get(sdata);
                            
                            sdata = helper.ReadDataRow(dt, "BaseCard", "", 0);

                            if (helper.DataTableExist(dt))
                            {
                                if (Convert.ToInt64(sdata) == 1)
                                {
                                    sdata = "SELECT T1.\"BaseCard\"  \"BaseCard\" " +
                                        ", (SELECT MAX(\"CardName\") FROM OCRD WHERE \"CardCode\" in (T1.\"BaseCard\"))  \"CardName\" " +
                                        ", IFNULL((SELECT \"AddID\" FROM OCRD WHERE \"CardCode\" in (T1.\"BaseCard\")), '')  \"wbvendercode\" " +
                                        ", LTRIM((SELECT DISTINCT B.\"U_Dept\"  " +
                                         "FROM CPN1 A " +
                                          "INNER JOIN CPN2 B ON A.\"CpnNo\" = B.\"CpnNo\" " +
                                         "WHERE A.\"BpCode\" in (T1.\"BaseCard\") " +
                                         "AND B.\"ItemCode\" in (T1.\"ItemCode\")),'0') || '-' || (Select \"U_Section\" || ' ' || \"U_FootWearType\" FROM OITM WHERE \"ItemCode\" in (T1.\"ItemCode\")) department " +
                                         $"FROM INV1 T1 WHERE T1.\"DocEntry\" in (SELECT  T0.\"DocEntry\" FROM OINV T0 WHERE T0.\"U_SINo\" = '{txtDocNum.Text}' and  \"CANCELED\" = 'N' )";
                                    
                                    dt = hana.Get(sdata);

                                    txtCardCode.Text = helper.ReadDataRow(dt, "BaseCard", "", 0);
                                    txtCardName.Text = helper.ReadDataRow(dt, "CardName", "", 0);
                                    txtBranchCode.Text = helper.ReadDataRow(dt, "wbvendercode", "", 0);
                                    txtDepartment.Text = helper.ReadDataRow(dt, "department", "", 0);
                                }
                                else
                                { txtCardCode.Text = ""; txtCardName.Text = ""; txtBranchCode.Text = ""; txtDepartment.Text = ""; }
                            }

                            ConsolidatedBP(fS.oCode);

                        }
                        break;


                    case "3":
                        fS.oSearchMode = "BP_Consolidated";
                        fS.oCode = txtDocNum.Text;
                        fS.ShowDialog();

                        try
                        {
                            if (fS.oCode != null)
                            {
                                txtCardCode.Text = fS.oCode;

                                sdata = "SELECT T1.\"BaseCard\" " +
                                        ", (SELECT MAX(\"CardName\") FROM OCRD WHERE \"CardCode\" in (T1.\"BaseCard\"))  \"CardName\" " +
                                        ", IFNULL((SELECT \"AddID\" FROM OCRD WHERE \"CardCode\" in (T1.\"BaseCard\")), '') \"wbvendercode\" " +
                                        ", LTRIM((SELECT DISTINCT B.\"U_Dept\" || '-' || B.\"U_Section\" " +
                                        "FROM CPN1 A " +
                                        "INNER JOIN CPN2 B ON A.\"CpnNo\" = B.\"CpnNo\" " +
                                        "WHERE A.\"BpCode\" in (T1.\"BaseCard\") " +
                                        "AND B.\"ItemCode\" in (T1.\"ItemCode\")),'0') \"department\" " +
                                        $"FROM INV1 T1 WHERE T1.\"DocEntry\" in (SELECT  T0.\"DocEntry\" FROM OINV T0 WHERE T0.\"U_SINo\" = '{txtDocNum.Text}' and  \"CANCELED\" = 'N') and t1.\"BaseCard\" = '{fS.oCode}'";
                                
                                dt = hana.Get(sdata);
                                txtCardName.Text = helper.ReadDataRow(dt, "CardName", "", 0);
                                txtBranchCode.Text = helper.ReadDataRow(dt, "wbvendercode", "", 0);
                                txtDepartment.Text = helper.ReadDataRow(dt, "department", "", 0);

                                var sdata1 = $"SELECT COUNT(T0.U_SIDRNo)+1 as count FROM [@OPKL] T0 WHERE T0.U_Type = 'SI' AND T0.U_SIDRNo = '{fS.oCode}'";                                
                                var dt1 = hana.Get(sdata1);

                                if (helper.DataTableExist(dt1))
                                { txtBox.Text = helper.ReadDataRow(dt, "count", "", 0); }
                                else { txtBox.Text = "1"; }
                                ConsolidatedBP(txtDocNum.Text);
                            }
                        }
                        catch (Exception ex)
                        { txtCardCode.Text = ""; }
                        break;

                }

            }
            else if (txtTransType.Text == "DR")
            {
                string docentry;
                var fS = new frmSearchPackingList();
                fS.oSearchMode = "DELIVERY_PICKLIST";
                fS.ShowDialog();
                txtDocNum.Text = fS.oCode;
                sdata = " SELECT  T0.U_PONo,T0.DocEntry,T0.CardCode,T0.CardName,T0.Address  ";
                sdata = sdata + " ,ISNULL((Select AliasName FROM OCRD WHERE CardCode = T0.CardCode),'') as wbvendercode ";
                sdata = sdata + " ,ISNULL((Select AddID FROM OCRD WHERE CardCode = T0.CardCode),'') as BranchCode ";
                sdata = sdata + " from OWTR T0 where T0.U_DRNo = '" + fS.oCode + "'";

                dt = hana.Get(sdata);
                docentry = helper.ReadDataRow(dt, "DocEntry", "", 0);

                txtPONum.Text = helper.ReadDataRow(dt, "U_PONo", "", 0);
                txtCardCode.Text = helper.ReadDataRow(dt, "CardCode", "", 0);
                txtCardName.Text = helper.ReadDataRow(dt, "CardName", "", 0);
                txtBranchCode.Text = helper.ReadDataRow(dt, "BranchCode", "", 0);
                txtShipto.Text = helper.ReadDataRow(dt, "Address", "", 0);

                GetDetails("DR", fS.oCode);

                sdata = " SELECT A.ItemCode,A.StockName,A.BarCode,A.Ordered_Qty,(A.Actual_Qty - ISNULL(B.qty,0)) as Actual_Qty,A.Cost FROM  ";
                sdata = sdata + " (SELECT T0.U_DRNo,T0.U_PONo,T0.DocEntry,T1.ItemCode,T1.Dscription as StockName,T1.CodeBars as BarCode,0 as Ordered_Qty,SUM(T1.Quantity)  as Actual_Qty ";
                sdata = sdata + " ,T1.Price as Cost FROM OWTR T0 INNER JOIN WTR1 T1 ON T0.DocEntry = T1.DocEntry ";
                sdata = sdata + " GROUP BY T0.U_DRNo,T0.U_PONo,T0.DocEntry,T1.ItemCode,T1.Dscription,T1.CodeBars,T1.Price) A ";
                sdata = sdata + " LEFT JOIN ";
                sdata = sdata + " (SELECT T0.U_Type,T0.U_SIDRNo,T0.U_PONo,T0.U_BranchCode,T1.U_ItemCode,SUM(T1.U_Quantity) as qty,T1.U_Cost  FROM [@OPKL] T0 INNER JOIN [@PKL1] T1 ON T0.DocEntry = T1.DocEntry  ";
                sdata = sdata + " WHERE T0.U_Type = 'DR' GROUP BY T0.U_Type,T0.U_SIDRNo,T0.U_PONo,T0.U_BranchCode,T1.U_ItemCode,T1.U_Cost) B ";
                sdata = sdata + " ON A.U_DRNo = B.U_SIDRNo AND A.ItemCode = B.U_ItemCode ";
                sdata = sdata + " WHERE (A.Actual_Qty - ISNULL(B.qty,0)) <> 0 AND A.U_DRNo = '" + fS.oCode + "' ";


                dataGridView1.ReadOnly = false;
                dataGridView1.DataSource = hana.Get(sdata);
                ReadOnly();

                sdata = $"SELECT COUNT(T0.U_SIDRNo)+1 as count FROM [@OPKL] T0 WHERE T0.U_Type = 'DR' AND T0.U_SIDRNo = '{txtDocNum.Text}' AND U_CardCode = '{txtCardCode.Text}'";
                dt = hana.Get(sdata);

                if (helper.DataTableExist(dt))
                {
                    txtBox.Text = helper.ReadDataRow(dt, "count", "", 0);
                }
                else { txtBox.Text = "1"; }


            }
            else if (txtTransType.Text == "CST")
            {

                var fS = new frmSearchPackingList();
                fS.oSearchMode = "INVENTORY_TRANSFER";
                fS.ShowDialog();

                if (fS.oCode != null)
                {
                    var GetBPdetails = $"SELECT DocEntry, DocNum ,CardCode, CardName, Address FROM OWTR where DocNum = '{fS.oCode}'";
                    
                    foreach (DataRow row in hana.Get(GetBPdetails).Rows)
                    {
                        txtDocNum.Text = row["DocNum"].ToString();
                        txtDocEntry.Text = row["DocEntry"].ToString();
                        txtCardCode.Text = row["CardCode"].ToString();
                        txtCardName.Text = row["CardName"].ToString();
                        txtShipto.Text = row["Address"].ToString();
                    }
                    
                    var GetItems = $"SELECT ItemCode[Item Code], Dscription[Stock Name], CodeBars[Bar Code], OrderedQty [Ordered Qty], Quantity [Actual Qty], PriceBefDi [Cost] FROM WTR1 where DocEntry = '{txtDocEntry.Text}'";
                    dataGridView1.DataSource = hana.Get(GetItems);
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("NO DATA FOUND!", true);
            }
        }

        void ConsolidatedBP(string oBPCode)
        {
            string sdata;
            DataTable dt;

            sdata = $" SELECT A.ItemCode [Item Code] ,A.StockName [Stock Name]";
            sdata += $",A.BarCode [Bar Code]";
            sdata += ",A.Ordered_Qty [Ordered Qty],(CAST(A.Actual_Qty as Decimal(18,2)) - ISNULL(CAST(B.qty as Decimal(18,2)),0)) [Actual Qty],CAST(A.Cost as Decimal(18,4)) as Cost  ";
            sdata += $" FROM (SELECT T0.U_SINo,T1.ItemCode,T1.Dscription as StockName,(SELECT DISTINCT Y.U_SKU FROM CPN1 X INNER JOIN CPN2 Y ON X.CpnNo =  Y.CpnNo Where X.BpCode = T0.CardCode And Y.ItemCode = T1.ItemCode) as BarCode,0 as Ordered_Qty,SUM(T1.Quantity) as Actual_Qty ";
            sdata += " ,(Case when (SELECT Z.QryGroup9 FROM OITM Z Where Z.ItemCode = T1.ItemCode) = 'Y' THEN (SELECT DISTINCT Y.U_RPrice FROM CPN1 X INNER JOIN CPN2 Y ON X.CpnNo =  Y.CpnNo Where X.BpCode = T0.CardCode And Y.ItemCode = T1.ItemCode) ELSE T1.PriceAfVAT END) as Cost";
            sdata += " ,T1.BaseCard FROM OINV T0 INNER JOIN INV1 T1 ON T0.DocEntry = T1.DocEntry GROUP BY  ";
            sdata += $" T0.U_SINo,T1.ItemCode,T1.Dscription,T1.PriceAfVAT,T1.BaseCard,T0.CardCode) A LEFT JOIN (SELECT T0.U_Type,T0.U_SIDRNo,T0.U_PONo,T0.U_BranchCode,T1.U_ItemCode,SUM(T1.U_Quantity) as qty,T1.U_Cost FROM [@OPKL] T0 INNER JOIN [@PKL1] T1 ON T0.DocEntry = T1.DocEntry WHERE  ";
            sdata += $" T0.U_Type = 'SI' AND T0.U_CardCode = '{txtCardCode.Text}' GROUP BY T0.U_Type,T0.U_SIDRNo,T0.U_PONo,T0.U_BranchCode,T1.U_ItemCode,T1.U_Cost) B ON A.U_SINo = B.U_SIDRNo AND A.ItemCode = B.U_ItemCode WHERE (A.Actual_Qty - ISNULL(B.qty,0)) <> 0   AND A.BaseCard = '{txtCardCode.Text}' AND A.U_SINo = '{oBPCode}' ";

            dt = hana.Get(sdata);

            if (helper.DataTableExist(dt))
            {
                dataGridView1.ReadOnly = false;
                dataGridView1.DataSource = hana.Get(sdata);
                ReadOnly();

                sdata = $"SELECT COUNT(T0.U_SIDRNo)+1 as count FROM [@OPKL] T0 WHERE T0.U_Type = 'SI' AND T0.U_SIDRNo = '{txtDocNum.Text}' AND U_CardCode = '{txtCardCode.Text}'";
                dt = hana.Get(sdata);

                if (helper.DataTableExist(dt))
                {
                    txtBox.Text = helper.ReadDataRow(dt, "count", "", 0);
                }
                else { txtBox.Text = "1"; }

            }
            else
            {
                //CleanForm();
                dataGridView1.DataSource = null;
            }
        }

        void ReadOnly()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].ReadOnly = true;
                row.Cells[1].ReadOnly = true;
                row.Cells[2].ReadOnly = true;
                row.Cells[3].ReadOnly = false;
                row.Cells[4].ReadOnly = true;
                row.Cells[5].ReadOnly = true;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            refresh("1");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            var fS = new frmSearchPackingList();
            fS.oSearchMode = "PiclistType";
            CleanForm();
            fS.ShowDialog();
            txtTransType.Text = fS.oCode;
            txtDocNum.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            refresh("2");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var val = true;
                double adder = 0;
                int encoder = 0;
                string sdata = "";

                // Consolidating all the blockings and make error message starts here
                foreach (DataGridViewRow dt in dataGridView1.Rows)
                {
                    if (Convert.ToDouble(dt.Cells["Actual Qty"].Value.ToString()) < Convert.ToDouble(dt.Cells["Ordered Qty"].Value.ToString()))
                    { val = false; }
                    if (Convert.ToDouble(dt.Cells["Ordered Qty"].Value.ToString()) != 0) { encoder = encoder + 1; }
                    if (val == true) { adder = adder + Convert.ToDouble(dt.Cells["Ordered Qty"].Value.ToString()); }
                    else { adder = -1; val = false; encoder = 0; }
                }
                // Consolidating all the blockings and make error message ends here

                if (adder != 0 && adder != -1 && val == true)
                {
                    try
                    {

                        if (txtLength.Text == "" || txtShipper.Text == "" || txtHeight.Text == "" || txtWeight.Text == "" || txtBox.Text == "" || txtBoxof.Text == "") { StaticHelper._MainForm.ShowMessage("Please fill up all mandatory fields", true); }
                        else
                        {
                            if (Convert.ToDouble(txtBox.Text) <= Convert.ToDouble(txtBoxof.Text))
                            {
                                var sbJson = new StringBuilder("{");
                                sbJson.AppendLine($@" ""U_PONo"": ""{txtPONum.Text}"",");
                                sbJson.AppendLine($@" ""U_Type"": ""{txtTransType.Text}"",");
                                sbJson.AppendLine($@" ""U_SIDRNo"": ""{txtDocNum.Text}"",");
                                sbJson.AppendLine($@" ""U_BranchCode"": ""{txtBranchCode.Text}"",");
                                sbJson.AppendLine($@" ""U_CardCode"": ""{txtCardCode.Text}"",");
                                sbJson.AppendLine($@" ""U_CardName"": ""{txtCardName.Text}"",");
                                sbJson.AppendLine($@" ""U_ShipTo"": ""{txtShipto.Text}"",");
                                sbJson.AppendLine($@" ""U_Department"": ""{txtDepartment.Text}"",");
                                sbJson.AppendLine($@" ""U_Shipper"": ""{txtShipper.Text}"",");
                                sbJson.AppendLine($@" ""U_Date"": ""{dtDate.Value.ToString("YYYY/mm/dd")}"",");
                               
                                if (string.IsNullOrEmpty(txtLength.Text) == false)
                                {
                                    sbJson.AppendLine($@" ""U_Length"": ""{txtLength.Text}"",");
                                }
                                if (string.IsNullOrEmpty(txtWidth.Text) == false)
                                {
                                    sbJson.AppendLine($@" ""U_Width"": ""{txtWidth.Text}"",");
                                }
                                if (string.IsNullOrEmpty(txtHeight.Text) == false)
                                {
                                    sbJson.AppendLine($@" ""U_Height"": ""{txtHeight.Text}"",");
                                }
                                if (string.IsNullOrEmpty(txtWeight.Text) == false)
                                {
                                    sbJson.AppendLine($@" ""U_Weight"": ""{txtWeight.Text}"",");
                                }
                                if (string.IsNullOrEmpty(txtBox.Text) == false)
                                {
                                    sbJson.AppendLine($@" ""U_Box"": ""{txtBox.Text}"",");
                                }

                                if (string.IsNullOrEmpty(txtBoxof.Text) == false)
                                {
                                    sbJson.AppendLine($@" ""U_TotalBox"": ""{txtBoxof.Text}"",");
                                }

                                int count = 0;
                                sdata = "";
                                decimal oQty, oCost;
                                sbJson.AppendLine(@" ""PKL1Collection"": [");
                                var cnt = 0;
                                foreach (DataGridViewRow dt in dataGridView1.Rows)
                                {
                                    if (Convert.ToDouble(dt.Cells["Ordered Qty"].Value.ToString()) != 0)
                                    {
                                        string _indication;
                                        string _data;

                                        oQty = Convert.ToDecimal(dt.Cells["Ordered Qty"].Value.ToString());
                                        oCost = Convert.ToDecimal(dt.Cells["Cost"].Value.ToString());
                                        oQty = Decimal.Parse(oQty.ToString("0.00"));
                                        oCost = Decimal.Parse(oCost.ToString("0.0000"));
                                        count = count + 1;
                                        sbJson.AppendLine(@"   {");
                                        sbJson.AppendLine($@"   ""U_ItemCode"": ""{dt.Cells["Item Code"].Value.ToString()}"",");
                                        sbJson.AppendLine($@"   ""U_ItemName"": ""{dt.Cells["Stock Name"].Value.ToString()}"",");
                                        sbJson.AppendLine($@"   ""U_Barcode"": ""{dt.Cells["Bar Code"].Value.ToString()}"",");
                                        sbJson.AppendLine($@"   ""U_Quantity"": ""{dt.Cells["Ordered Qty"].Value.ToString()}"",");
                                        sbJson.AppendLine($@"   ""U_Cost"": ""{dt.Cells["Cost"].Value.ToString()}"",");

                                        if (count != encoder) { _indication = "SM"; } else { _indication = "SMEND"; }
                                        sbJson.AppendLine($@"   ""U_Indication"": ""{_indication}"",");

                                        _data = txtPONum.Text + "," + txtDocNum.Text + "," + dt.Cells["Bar Code"].Value.ToString();
                                        _data = _data + "," + oQty.ToString() + "," + oCost.ToString() + ",";
                                        _data = _data + txtBoxof.Text + "," + txtBox.Text + ",";
                                        _data = _data + txtBranchCode.Text + "," + txtLength.Text + "," + txtShipper.Text + "," + txtHeight.Text + "," + txtWeight.Text;
                                        _data = _data + "," + _indication;

                                        sbJson.AppendLine($@"   ""U_Data"": ""{_data}""");
                                        
                                        sdata = sdata + _data;
                                        cnt++;
                                        if (cnt == dataGridView1.Rows.Count)
                                        { sbJson.AppendLine(@"   }"); }
                                        else
                                        { sbJson.AppendLine(@"   },"); }
                                    }
                                }
                                sbJson.AppendLine(@" ]");
                                sbJson.AppendLine($@" ""Remark"": ""{sdata}"",");

                                StaticHelper._MainForm.ShowMessage("Packing List Added Successfully");


                                var serviceLayerAccess = new ServiceLayerAccess();
                                
                                if (!serviceLayerAccess.ServiceLayer_Posting(sbJson, "PATCH", $"OPKL({txtDocEntry.Text})", "DocEntry", out string output, out string value))
                                { StaticHelper._MainForm.ShowMessage(output, true); }
                                else
                                { CleanForm(); }
                            }
                            else
                            { StaticHelper._MainForm.ShowMessage("Boxof Less than the box", true); }
                        }
                    }
                    catch (Exception ex)
                    {
                        StaticHelper._MainForm.ShowMessage(ex.Message, true);
                    }
                }
                else if (adder == -1) { StaticHelper._MainForm.ShowMessage("Quantity is greater than actual quantity.", true); }
                else { StaticHelper._MainForm.ShowMessage("Invalid Quantity", true); }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }

        }
        void ClearForm2()
        {
            txtBoxof.Text = "";
            txtTransType.Text = "";
            txtDocNum.Text = "";
            txtPONum.Text = "";
            txtBranchCode.Text = "";
            txtCardCode.Text = "";
            txtCardName.Text = "";
            txtDepartment.Text = "";
            txtShipto.Text = "";
            txtFinalData.Text = "";
            txtDocEntry.Text = "";

            dataGridView1.DataSource = null;
            ReadSeries();
            this.Refresh();
        }
        private void CleanForm()
        {
            txtLength.Text = "";
            txtShipper.Text = "";
            txtHeight.Text = "";
            txtWeight.Text = "";
            txtBoxof.Text = "";
            txtTransType.Text = "";
            txtDocNum.Text = "";
            txtPONum.Text = "";
            txtBranchCode.Text = "";
            txtCardCode.Text = "";
            txtCardName.Text = "";
            txtDepartment.Text = "";
            txtShipto.Text = "";
            txtFinalData.Text = "";
            txtDocEntry.Text = "";

            // Clear All textbox 
            foreach (var c in this.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = String.Empty;

                }
            }
            dataGridView1.DataSource = null;
            ReadSeries();
            this.Refresh();
        }

        private void txtLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBoxof_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        void ShowUDF()
        {
            Form fc = Application.OpenForms["frmpackinglist_udf"];
            if (fc == null)
            {
                frmpackinglist_udf.oTableName = "OPKL";
                frmUDF = new frmpackinglist_udf();
                frmUDF.StartPosition = FormStartPosition.Manual;
                frmUDF.Location = new Point(this.Right, this.Top);
                frmUDF.Height = this.Height;
                frmUDF.MdiParent = StaticHelper._MainForm;
                frmUDF.Show();
            }
        }

        private void ReadSeries()
        {
            DataTable dta = null;
            string sdata;
            sdata = "select MAX(DocEntry) + 1 as DocEntry FROM [@OPKL] ";
            dta = hana.Get(sdata);
            txtDocEntry.Text = helper.ReadDataRow(dta, "DocEntry", "", 0);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && dataGridView1.CurrentCell.ColumnIndex == 1)
            {
                e.Handled = true;
                DataGridViewCell cell = dataGridView1.Rows[0].Cells[0];
                dataGridView1.CurrentCell = cell;
                dataGridView1.BeginEdit(true);
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                var readOnly = (sender as DataGridView).SelectedCells[0].ReadOnly;
                return;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            string sdata, docentry;
            DataTable dt = null;
            var fS = new frmSearchPackingList();
            fS.oSearchMode = "FINDPICKLIST";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                picbtn1.Enabled = false;
                picbtn2.Enabled = false;
                picbtn3.Enabled = false;
                
                sdata = "  SELECT T0.DocEntry,T0.U_Type,T0.U_SIDRNo,T0.U_PONo,T0.U_BranchCode,T0.U_Department,T0.U_CardCode ";
                sdata = sdata + " ,T0.U_CardName,T0.U_ShipTo,T0.U_Length,T0.U_Width,T0.U_Height,T0.U_Weight,T0.U_Box,T0.U_TotalBox ";
                sdata = sdata + " ,T1.U_ItemCode,T1.U_ItemName,T1.U_Barcode,T1.U_Cost,T1.U_Indication,T1.U_Data,T0.Remark ";
                sdata = sdata + " FROM [@OPKL] T0 INNER JOIN [@PKL1] T1 ON T0.DocEntry = T1.DocEntry WHERE T0.DocEntry = '" + fS.oCode + "' ";
                
                dt = hana.Get(sdata);

                txtDocEntry.Text = fS.oCode;
               
                txtTransType.Text = helper.ReadDataRow(dt, "U_Type", "", 0);
                txtDocNum.Text = helper.ReadDataRow(dt, "U_SIDRNo", "", 0);
                txtPONum.Text = helper.ReadDataRow(dt, "U_PONo", "", 0);
                txtCardCode.Text = helper.ReadDataRow(dt, "U_CardCode", "", 0);
                txtCardName.Text = helper.ReadDataRow(dt, "U_CardName", "", 0);
                txtDepartment.Text = helper.ReadDataRow(dt, "U_Department", "", 0);
                txtBranchCode.Text = helper.ReadDataRow(dt, "U_BranchCode", "", 0);
                txtShipto.Text = helper.ReadDataRow(dt, "U_ShipTo", "", 0);
                txtLength.Text = Convert.ToDouble(helper.ReadDataRow(dt, "U_Length", "", 0)).ToString("#,##0.##");
                txtShipper.Text = Convert.ToDouble(helper.ReadDataRow(dt, "U_Width", "", 0)).ToString("#,##0.##");
                txtHeight.Text = Convert.ToDouble(helper.ReadDataRow(dt, "U_Height", "", 0)).ToString("#,##0.##");
                txtWeight.Text = Convert.ToDouble(helper.ReadDataRow(dt, "U_Weight", "", 0)).ToString("#,##0.##");
                txtBox.Text = helper.ReadDataRow(dt, "U_Box", "", 0);
                txtBoxof.Text = helper.ReadDataRow(dt, "U_TotalBox", "", 0);
                txtFinalData.Text = helper.ReadDataRow(dt, "Remark", "", 0);


                sdata = "SELECT ";
                sdata = sdata + $" T1.U_ItemCode as ItemCode,T1.U_ItemName as StockName,(SELECT DISTINCT Y.U_SKU FROM CPN1 X INNER JOIN CPN2 Y ON X.CpnNo =  Y.CpnNo Where X.BpCode = '{helper.ReadDataRow(dt, "U_CardCode", "",0)}' And Y.ItemCode = T1.U_ItemCode) as BarCode,T1.U_Quantity AS Ordered_Qty,T1.U_Cost as Cost ";
                sdata = sdata + " ,T1.U_Indication as Indication,T1.U_Data as Data ";
                sdata = sdata + " FROM [@OPKL] T0 INNER JOIN [@PKL1] T1 ON T0.DocEntry = T1.DocEntry WHERE T0.DocEntry = '" + fS.oCode + "' and T1.U_ItemCode <> '' ";

                dataGridView1.ReadOnly = true;
                dataGridView1.DataSource = hana.Get(sdata);
                btnAdd.Enabled = false;
                btnViewReport.Enabled = true;
            }

            //btnAdd.Text = "Update";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            picbtn1.Enabled = true;
            picbtn2.Enabled = true;
            picbtn3.Enabled = true;
            btnViewReport.Enabled = false;
            CleanForm();
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            var a = new frmCrystalReports();
            a.type = "outright";
            a.oDocKey = txtDocEntry.Text;
            a.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            refresh("3");
        }

        private void txtHeight_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtWidth_KeyPress_1(object sender, KeyPressEventArgs e)
        {

        }

        private void frmPackingList_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DECLARE.udf.RemoveAll(x => x.ObjCode == "OPKL");
                Form frm = Application.OpenForms["frmpackinglist_udf"];
                if (frm != null)
                {
                    frmUDF.Dispose();
                }

                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void frmPackingList_LocationChanged(object sender, EventArgs e)
        {
            if (frmUDF != null)
            {
                frmUDF.Location = new Point(this.Right, this.Top);
                frmUDF.Height = Height;
            }
        }

        private void frmPackingList_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void frmPackingList_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close this Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            { Dispose(); }
            else
            { e.Cancel = true; }
        }
    }
}

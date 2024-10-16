using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sap.Data.Hana;
using zDeclare;
using System.Diagnostics;
using System.Threading;
using MetroFramework.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using PresenterLayer.Views.Main;
using PresenterLayer.Views.Inventory.Inventory_Transfer;
using PresenterLayer.Services.Inventory.Inventory_Transfer;
using DirecLayer;
using DomainLayer.Models;
using PresenterLayer.Helper;

namespace PresenterLayer
{
    public partial class frmIT_DrawDoumentWizard : MetroForm
    {
        MainForm frmMain;
        InventoryTransfer frmIT;
        private InventoryTransferItemsController itic = new InventoryTransferItemsController();
        SAPHanaAccess Hana = new SAPHanaAccess();
        DataAccess da = new DataAccess();
        Thread gvItThread;
        DataTable dtGCR = new DataTable();


        private string oCode,_StyleCode,oName;
        private static int defaultColumn = 1, _rowIndex = 0;
        private long CurrentPageIndex = 1;
        public static string oStyleCode, oColorCode, oSection, oFWhs, oTWhs, oBPCode;
        public static DateTime oDocDate;

        Boolean DrawAll = false;

        bool isError = false;

        public frmIT_DrawDoumentWizard(InventoryTransfer frmIT)
        {
            InitializeComponent();
            this.frmIT = frmIT;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (DrawAll == true && btnAdd.Text == "Finish" && gvITddw.Visible == false)
                {
                    InventoryTransferHeaderModel.oDDW = "DrawAll";
                    this.Close();
                }
                else if (btnAdd.Text == "Next")
                {
                    InventoryTransferHeaderModel.oDDW = "Customize";
                    gvITddw.Visible = true;
                    rbtDrawAll.Visible = false;
                    rbtCustomize.Visible = false;
                    btnBack.Enabled = true;
                    btnAdd.Text = "Finish";

                    LoadCurrentRecords();
                }
                else if (btnAdd.Text == "Finish" && gvITddw.Visible == true)
                {
                    if (gvITddw.SelectedRows.Count > 0)
                    {
                        if (AddItem() == true)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Please select an item(s) first before adding.",true);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            InventoryTransferHeaderModel.oDDW = "";
            gvITddw.Visible = false;
            rbtDrawAll.Visible = true;
            rbtCustomize.Visible = true;
            btnBack.Enabled = false;
            btnAdd.Text = "Finish";
            rbtDrawAll.Checked = true;
        }

        public void LoadCurrentRecords()
        {
            dtGCR = GetCurrentRecords();

            try
            {
                gvItThread = new Thread(new ThreadStart(LoadDataWithThreading));
                gvItThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DataTable GetCurrentRecords()
        {

            DataTable dt = new DataTable();
            string query = "";
            string oSearchTB = InventoryTransferHeaderModel.oSearchTable;
            string oDocEntries = "";
            string Selqry1 = "";

            foreach (var x in InventoryTransferHeaderModel.DDWdocentry.Where(x => x.BpCode != ""))
            {
                if (oDocEntries == "")
                {
                    oDocEntries = "'" + x.DocEntry.ToString() + "'";
                }
                else
                {
                    oDocEntries += ",'" + x.DocEntry.ToString() + "'";
                }
            }

            if (oSearchTB == "OWTQ_NOBP" || oSearchTB == "OWTQ")
            {
                query = "SELECT " +
                    " T0.LineNum [Line No.]" +
                    ", T0.DocEntry [BaseDocument]" +
                    ", T0.ItemCode [Item No.]" +
                    ", T0.Dscription [Item Description]" +
                    ", T0.OpenQty [Quantity]" +
                    ", T0.Price" +
                    ", T0.DiscPrcnt" +
                    ", T0.LineTotal" +
                    ", T0.WhsCode [To Warehouse]" +
                    ", T0.FromWhsCod [From Warehouse]" +
                    ", T0.SlpCode" +
                    ", T0.PriceBefDi" +
                    ", T0.Project" +
                    ", T0.VatGroup" +
                    ", T0.VatPrcnt" +
                    ", T0.CodeBars" +
                    ", T0.PriceAfVAT" +
                    ", T0.TaxCode" +
                    ", T0.VatAppld" +
                    ", T0.LineVat" +
                    ", T1.U_ID001 [BrandCode] " +
                    ", (SELECT Name FROM [@OBND] WHERE Code = T1.U_ID001) [BrandName] " +
                    ", T1.U_ID012 [U_StyleCode]" +
                    ", (SELECT Name FROM [@PRSTYLE] WHERE Code = T1.U_ID012) [StyleName] " +
                    ", T1.U_ID011 [U_Color]" +
                    ", (SELECT Name FROM[@OCLC] Z Where Z.Code = T1.U_ID011) [ColorName] " +
                    ", T1.U_ID018 [U_Section]" +
                    ", T1.U_ID007 [U_Size]" +
                    ", (select Name from [@CMP_INFO] where Code = T0.U_Company) [U_Company] " +
                    ", T0.U_SKU" +
                    ", '' [OrderEntry] " +
                    ", (select CardCode from OWTQ where DocEntry = T0.DocEntry) [BPCode]" +
                    " FROM WTQ1 T0 INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode" +
                    $" Where T0.OpenQty > 0 AND T0.DocEntry IN ({oDocEntries}) ORDER BY T0.DocEntry asc, T0.U_Company asc, T1.U_ID023 asc";
            }
            else
            {
                Selqry1 = "SELECT " +
                        " A.AbsEntry [BaseDocument]" +
                        ", A.OrderEntry " +
                        ", A.OrderLine [Line No.]" +
                        ", A.PickQtty [Quantity]" +
                        ", A.PickStatus" +
                        ", A.RelQtty " +
                        ", A.PrevReleas" +
                        ", A.BaseObject" +
                        ", C.U_ID001 [BrandCode] " +
                        ", (SELECT Name FROM [@OBND] WHERE Code = C.U_ID001) [BrandName] " +
                        ", C.U_ID012 [U_StyleCode]" +
                        ", (SELECT Name FROM [@PRSTYLE] WHERE Code = C.U_ID012) [StyleName] " +
                        ", C.U_ID011 [U_Color]" +
                        ", (SELECT Name FROM[@OCLC] Z Where Z.Code = C.U_ID011) [ColorName] " +
                        ", C.U_ID018 [U_Section]" +
                        ", C.U_ID007 [U_Size]" +
                        ", B.ItemCode [Item No.]" +
                        ", B.Dscription [Item Description]" +
                        ", (A.PickQtty + A.RelQtty)[Quantity]" +
                        ", B.Price" +
                        ", ISNULL(B.DiscPrcnt, 0)[DiscPrcnt]" +
                        ", B.LineTotal" +
                        ", B.WhsCode [To Warehouse]" +
                        ", B.FromWhsCod [From Warehouse]" +
                        ", B.SlpCode" +
                        ", B.PriceBefDi" +
                        ", B.Project" +
                        ", B.VatGroup" +
                        ", B.VatPrcnt" +
                        ", B.CodeBars" +
                        ", B.PriceAfVAT" +
                        ", B.TaxCode" +
                        ", B.VatAppld" +
                        ", B.LineVat " +
                        ", (select Name from [@CMP_INFO] where Code = B.U_Company) [U_Company] " +
                        ", B.U_SKU " +
                        ", C.U_ID023 " +
                        ", (select CardCode from OWTQ where DocEntry = B.DocEntry) [BPCode]" +
                        " FROM PKL1 A " +
                        " LEFT JOIN WTQ1 B ON A.OrderEntry = B.DocEntry and A.OrderLine = B.LineNum";

                foreach (var x in InventoryTransferHeaderModel.DDWdocentry.Where(x => x.BpCode != ""))
                {
                    if (query == "")
                    {
                        query = Selqry1 +
                                $" INNER JOIN OWTQ T2 on B.DocEntry = T2.DocEntry and T2.CardCode = '{x.BpCode}' " +
                                $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickQtty > 0 and A.PickStatus != 'C' and A.AbsEntry = '{x.DocEntry}' " +
                                $" and A.OrderEntry = '{x.OrderEntry}' ";
                    }
                    else
                    {
                        query += " UNION ALL " +
                                 Selqry1 +
                                 $" INNER JOIN OWTQ T2 on B.DocEntry = T2.DocEntry and T2.CardCode = '{x.BpCode}' " +
                                 $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickQtty > 0 and A.PickStatus != 'C' and A.AbsEntry = '{x.DocEntry}' " +
                                 $" and A.OrderEntry = '{x.OrderEntry}' ";
                    }
                }

                query = $" SELECT * from ({ query }) MT1 " +
                        "ORDER BY MT1.BaseDocument asc, MT1.U_Company asc, MT1.U_ID023 asc";

                //$" INNER JOIN OWTQ T2 on B.DocEntry = T2.DocEntry and T2.CardCode = '{InventoryTransferHeaderModel.oBPCode}' " +
                //$" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickStatus != 'C' and A.AbsEntry = '{InventoryTransferHeaderModel.oCode}' ";

            }

            dt = Hana.Get(query);

            gvITddw.Rows.Clear();
            itic.dgvSetup(gvITddw);

            return dt;

        }

        public void LoadDataWithThreading()
        {
            foreach (DataRow row in dtGCR.Rows)
            {

                object[] lineitem = { row["BaseDocument"].ToString(), row["Line No."].ToString(), row["Item No."].ToString(), row["Item Description"].ToString(), row["Quantity"].ToString()
                                      , row["From Warehouse"].ToString(), row["To Warehouse"].ToString(), row["BrandName"].ToString(), row["StyleName"].ToString()
                                      , row["ColorName"].ToString(), row["OrderEntry"].ToString(), row["U_Company"].ToString(), row["BPCode"].ToString()
                                    };
                try
                {
                    gvITddw.Invoke(new Action(() => { gvITddw.Rows.Add(lineitem); }));
                }
                catch { }

            }

            Thread.Sleep(5000);
        }

        private Boolean AddItem()
        {
            bool result = false;
            try
            {
                InventoryTransferHeaderModel.DDWdocentry.RemoveAll(z => z.BpCode != "");
                foreach (DataGridViewRow dr in gvITddw.SelectedRows)
                {                  
                    InventoryTransferHeaderModel.DDWdocentry.Add(new InventoryTransferHeaderModel.DDWdocentryData
                    {
                        DocEntry = Convert.ToInt32(dr.Cells[0].Value),
                        BpCode = dr.Cells[12].Value.ToString(),
                        OrderEntry = dr.Cells[10].Value.ToString(),
                        LineEntry = Convert.ToInt32(dr.Cells[1].Value)
                    });
                }

                //InventoryTransferHeaderModel.oLineNums = "";
                //InventoryTransferHeaderModel.oOrderEntry = "";

                //foreach (DataGridViewRow dr in gvITddw.SelectedRows)
                //{
                //    if (InventoryTransferHeaderModel.oLineNums == "")
                //    {
                //        InventoryTransferHeaderModel.oLineNums = dr.Cells[1].Value.ToString();
                //    }
                //    else
                //    {
                //        InventoryTransferHeaderModel.oLineNums += "," + dr.Cells[1].Value.ToString();
                //    }
                //    InventoryTransferHeaderModel.oOrderEntry = dr.Cells[10].Value.ToString();
                //}       

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
                return result;
            }

        }

        private void rdbCustomize_CheckedChanged(object sender, EventArgs e)
        {
            DrawAll = false;
            btnAdd.Text = "Next";
        }

        private void rbtDrawAll_CheckedChanged(object sender, EventArgs e)
        {
            DrawAll = true;
            btnAdd.Text = "Finish";
        }

        


        private void btnCancel_Click(object sender, EventArgs e)
        {
            InventoryTransferHeaderModel.oCode = "";
            Dispose();
        }

        private void frmIT_Items_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

    }
}

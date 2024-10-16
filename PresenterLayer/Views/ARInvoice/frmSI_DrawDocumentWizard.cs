using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sap.Data.Hana;
using zDeclare;
using System.Diagnostics;
using System.Threading;
using MetroFramework.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using PresenterLayer.Views;
using PresenterLayer.Helper;
using DirecLayer;

namespace EasySAP
{
    public partial class frmSI_DrawDoumentWizard : MetroForm
    {
        //frmMain frmMain;
        FrmARInvoice SalesInvoice;
        private SalesInvoiceItemsController siic = new SalesInvoiceItemsController();

        //DataAccess da = new DataAccess();
        Thread gvItThread;
        DataTable dtGCR = new DataTable();

        private string oCode,_StyleCode,oName;
        private static int defaultColumn = 1, _rowIndex = 0;
        private long CurrentPageIndex = 1;
        public static string oStyleCode, oColorCode, oSection, oFWhs, oTWhs, oBPCode;
        public static DateTime oDocDate;

        Boolean DrawAll = false;

        bool isError = false;

        private SAPHanaAccess sapHana { get; set; }

        public frmSI_DrawDoumentWizard(FrmARInvoice SalesInvoice)
        {
            InitializeComponent();
            this.SalesInvoice = SalesInvoice;
            sapHana = new SAPHanaAccess();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (DrawAll == true && btnAdd.Text == "Finish" && gvSIddw.Visible == false)
                {
                    InvoiceHeaderModel.oDDW = "DrawAll";
                    this.Close();
                }
                else if (btnAdd.Text == "Next")
                {
                    InvoiceHeaderModel.oDDW = "Customize";
                    gvSIddw.Visible = true;
                    rbtDrawAll.Visible = false;
                    rbtCustomize.Visible = false;
                    btnBack.Enabled = true;
                    btnAdd.Text = "Finish";

                    LoadCurrentRecords();
                }
                else if (btnAdd.Text == "Finish" && gvSIddw.Visible == true)
                {
                    if (gvSIddw.SelectedRows.Count > 0)
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
            InvoiceHeaderModel.oDDW = "";
            gvSIddw.Visible = false;
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
            string oSearchTB = InvoiceHeaderModel.oSearchTable;
            string oDocEntries = "";
            string Selqry1 = "";

            foreach (var x in InvoiceHeaderModel.DDWdocentry.Where(x => x.BpCode != ""))
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

            if (oSearchTB == "ORDR_BP" || oSearchTB == "ORDR")
            {
                query = "SELECT " +
                        " T0.LineNum [Line No.] " +
                        ", T0.DocEntry [BaseDocument] " +
                        ", T0.ItemCode [Item No.] " +
                        ", T0.Dscription [Item Description] " +
                        ", T0.OpenQty [Quantity] " +
                        ", T0.Price [Price]" +
                        ", T0.DiscPrcnt" +
                        ", T0.LineTotal [Line Total]" +
                        ", T0.WhsCode [Warehouse]" +
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
                        ", T1.U_ID012 [U_StyleCode]" +
                        ", T1.U_ID011 [U_Color]" +
                        ", T1.U_ID018 [U_Section]" +
                        ", T1.U_ID007 [U_Size]" +
                        ", T1.CodeBars " +
                        " FROM RDR1 T0 " +
                        " INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode " +
                        $" Where T0.DocEntry IN({oDocEntries}) ORDER BY T0.DocEntry asc, T0.U_Company asc, T1.U_ID023 asc";
                //" Where T0.DocEntry = '" + InvoiceHeaderModel.oCode + "'";
            }
            else
            {
                Selqry1 = "SELECT " +
                        "A.AbsEntry [BaseDocument] " +
                        ", A.OrderEntry" +
                        ", A.OrderLine [Line No.] " +
                        ", A.PickQtty" +
                        ", A.PickStatus" +
                        ", A.RelQtty" +
                        ", A.PrevReleas" +
                        ", A.BaseObject " +
                        ",C.U_ID025 [U_StyleCode]" +
                        ",C.U_ID011 [U_Color]" +
                        ",C.U_ID018 [U_Section]" +
                        ",C.U_ID007 [U_Size]" +
                        ", B.ItemCode [Item No.] " +
                        ", B.Dscription [Item Description] " +
                        ", (A.RelQtty + A.PickQtty) [Quantity]" +
                        ", B.Price [Price]" +
                        ", B.DiscPrcnt" +
                        ", B.LineTotal [Line Total]" +
                        ", B.WhsCode [Warehouse]" +
                        ", B.SlpCode" +
                        ", B.PriceBefDi" +
                        ", B.Project" +
                        ", B.VatGroup" +
                        ", B.VatPrcnt" +
                        ", B.CodeBars" +
                        ", B.PriceAfVAT" +
                        ", B.TaxCode" +
                        ", B.VatAppld" +
                        ", B.LineVat" +
                        ", (select CardCode from ORDR where DocEntry = B.DocEntry) [BPCode]" +
                        " FROM PKL1 A " +
                        " LEFT JOIN RDR1 B ON A.OrderEntry = B.DocEntry and A.OrderLine = B.LineNum "; 
                        
                        //" LEFT JOIN RDR1 B ON A.OrderEntry = B.DocEntry and A.OrderLine = B.LineNum and A.BaseObject = B.ObjType " +
                        //" LEFT JOIN OITM C on C.ItemCode = B.ItemCode " +
                        //" Where A.AbsEntry = '" + InvoiceHeaderModel.oCode + "'";


                foreach (var x in InvoiceHeaderModel.DDWdocentry.Where(x => x.BpCode != ""))
                {
                    if (query == "")
                    {
                        query = Selqry1 +
                                $" INNER JOIN ORDR T2 on B.DocEntry = T2.DocEntry and T2.CardCode = '{x.BpCode}' " +
                                $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickQtty > 0 and A.PickStatus != 'C' and A.AbsEntry = '{x.DocEntry}' " +
                                $" and A.OrderEntry = '{x.OrderEntry}' ";
                    }
                    else
                    {
                        query += " UNION ALL " +
                                 Selqry1 +
                                 $" INNER JOIN ORDR T2 on B.DocEntry = T2.DocEntry and T2.CardCode = '{x.BpCode}' " +
                                 $" LEFT JOIN OITM C on C.ItemCode = B.ItemCode Where A.PickQtty > 0 and A.PickStatus != 'C' and A.AbsEntry = '{x.DocEntry}' " +
                                 $" and A.OrderEntry = '{x.OrderEntry}' ";
                    }
                }

                query = $" SELECT * from ({ query }) MT1 " +
                        "ORDER BY MT1.BaseDocument asc ";
            }

            dt = sapHana.Get(query);

            gvSIddw.Rows.Clear();
            siic.dgvSetup(gvSIddw);

            return dt;

        }

        public void LoadDataWithThreading()
        {
            foreach (DataRow row in dtGCR.Rows)
            {

                object[] lineitem = { row["BaseDocument"].ToString(), row["Line No."].ToString(), row["Item No."].ToString(), row["Item Description"].ToString()
                                      , row["Quantity"].ToString(), row["Price"].ToString(), row["Line Total"].ToString(), row["Warehouse"].ToString(), row["BPCode"].ToString(), row["OrderEntry"].ToString()
                                    };
                try
                {
                    gvSIddw.Invoke(new Action(() => { gvSIddw.Rows.Add(lineitem); }));
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
                InvoiceHeaderModel.DDWdocentry.RemoveAll(z => z.BpCode != "");

                foreach (DataGridViewRow dr in gvSIddw.SelectedRows)
                {
                    InvoiceHeaderModel.DDWdocentry.Add(new InvoiceHeaderModel.DDWdocentryData
                    {
                        DocEntry = Convert.ToInt32(dr.Cells[0].Value),
                        BpCode = dr.Cells[8].Value.ToString(),
                        OrderEntry = dr.Cells[9].Value.ToString(),
                        LineEntry = Convert.ToInt32(dr.Cells[1].Value)
                    });
                }

                //InvoiceHeaderModel.oLineNums = "";

                //foreach (DataGridViewRow dr in gvSIddw.SelectedRows)
                //{
                //    if (InvoiceHeaderModel.oLineNums == "")
                //    {
                //        InvoiceHeaderModel.oLineNums = dr.Cells[1].Value.ToString();
                //    }
                //    else
                //    {
                //        InvoiceHeaderModel.oLineNums += "," + dr.Cells[1].Value.ToString();
                //    }
                //}

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
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
            Dispose();
        }

        private void frmIT_Items_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

    }
}

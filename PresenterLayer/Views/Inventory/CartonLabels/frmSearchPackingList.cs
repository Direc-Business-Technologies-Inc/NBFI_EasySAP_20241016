using DirecLayer;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PresenterLayer
{
    public partial class frmSearchPackingList : MetroForm
    {
        private string search;
        private string code;
        private string DocType;
        int defaultColumn = 1, _rowIndex = 0;
        SAPHanaAccess hana { get; set; }
        public string oSearchMode { get { return search; } set { search = value; } }
        public string @Param1, @Param2, @Param3, @Param4, _title;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _rowIndex = e.RowIndex;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    switch (oSearchMode)
                    {
                        case "PiclistType": { oCode = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); break; }
                        case "INVOICE_PICKLIST": { oCode = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); break; }
                        case "DELIVERY_PICKLIST": { oCode = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); break; }
                        case "FINDPICKLIST": { oCode = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); break; }
                        case "BP_Consolidated": { oCode = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); break; }
                        case "INVENTORY_TRANSFER": { oCode = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); break; }
                    }
                    this.Close();
                }

            }
            catch (Exception ex) { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[defaultColumn].Value.ToString().ToUpper().StartsWith(txtSearch.Text.ToUpper()))
                    {
                        row.Selected = true;
                        _rowIndex = row.Index;
                        dataGridView1.FirstDisplayedScrollingRowIndex = _rowIndex;
                        break;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == Keys.Enter)
                {

                    oCode = dataGridView1.Rows[_rowIndex].Cells[1].Value.ToString();

                    this.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            _rowIndex = e.RowIndex;
        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    switch (oSearchMode)
                    {
                        case "PiclistType": { oCode = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); break; }
                        case "INVOICE_PICKLIST": { oCode = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); break; }
                        case "DELIVERY_PICKLIST": { oCode = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); break; }
                        case "FINDPICKLIST": { oCode = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); break; }
                        case "BP_Consolidated": { oCode = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); break; }
                    }
                    this.Close();
                }

            }
            catch (Exception ex) { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            _rowIndex = e.RowIndex;
        }

        public string oCode { get { return code; } set { code = value; } }

        public frmSearchPackingList()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
        }

        private void frmSearchPackingList_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
            lblTitle.Text = _title;

            DataTable dt = null;
            string query = "";

            switch (oSearchMode)
            {
                case "PiclistType":
                    {
                        dataGridView1.DataSource = GetTable();
                        break;
                    }

                case "INVOICE_PICKLIST":
                    {
                        query = $" select 'SI' as TYPE,U_SINo as REFNO,CardCode,CardName  from OINV A WHERE CANCELED = 'N' AND U_SINo <> '' AND U_SINo NOT IN (select T0.U_SINo  from OINV T0 WHERE CANCELED = 'N' AND U_SINo <> ''  ";
                        query += $" AND (SELECT SUM(Quantity) FROM INV1 WHERE DocEntry = (SELECT  MAX(DocEntry) FROM OINV WHERE U_SINo = T0.U_SINo)) <= (SELECT SUM(U_Quantity) FROM [@PKL1] WHERE DocEntry IN (SELECT DocEntry FROM [@OPKL] WHERE U_Type = 'SI' AND U_SIDRNo = T0.U_SINo)))";
                        dt = hana.Get(query);
                        dataGridView1.DataSource = dt;
                        break;
                    }
                case "BP_Consolidated":
                    {
                        query = $"  SELECT T1.BaseCard as CardCode,(SELECT CardName FROM OCRD WHERE CardCode = T1.BaseCard) as CardName FROM INV1 T1 WHERE T1.DocEntry =  ";
                        query += $" (SELECT TOP 1 T0.DocEntry FROM OINV T0 WHERE T0.U_SINo = '{oCode}') GROUP BY T1.BaseCard) T0 GROUP BY T0.BaseCard) T0 ";
                        dt = hana.Get(query);
                        dataGridView1.DataSource = dt;
                        break;
                    }
                case "DELIVERY_PICKLIST":
                    {
                        //query = "SELECT 'DR' AS TYPE,U_DRNo AS REFNO,CardCode,CardName FROM OWTR WHERE ISNULL(U_DRNo,'-') <> '-'";
                        query = $" select 'DR' as TYPE,U_DRNo as REFNO,CardCode,CardName  from OWTR A WHERE CANCELED = 'N' AND U_DRNo <> '' AND U_DRNo NOT IN (select T0.U_DRNo  from OWTR T0 WHERE CANCELED = 'N' AND U_DRNo <> ''  ";
                        query += $" AND (SELECT SUM(Quantity) FROM WTR1 WHERE DocEntry = (SELECT  MAX(DocEntry) FROM OWTR WHERE U_DRNo = T0.U_DRNo)) <= (SELECT SUM(U_Quantity) FROM [@PKL1] WHERE DocEntry IN (SELECT DocEntry FROM [@OPKL] WHERE U_Type = 'DR' AND U_SIDRNo = T0.U_DRNo))) AND (SELECT GroupCode FROM OCRD Z Where Z.CardCode = A.CardCode) = '109'";

                        dt = hana.Get(query);
                        dataGridView1.DataSource = dt;
                        break;
                    }
                case "FINDPICKLIST":
                    {
                        query = "SELECT T0.DocEntry as docentry,T0.U_Type as Type,T0.U_SIDRNo as DocNum,U_BranchCode as BranchCode,U_CardCode as CardCode,U_CardName AS CardName,U_ShipTo as ShipTo  FROM [@OPKL] T0";
                        dt = hana.Get(query);
                        dataGridView1.DataSource = dt;
                        break;
                    }

                case "INVENTORY_TRANSFER":
                    query = "SELECT T0.DocNum, T0.DocDate, T0.Filler, T0.Comments, (SELECT Z.SeriesName FROM NNM1 Z where Z.Series = T0.Series) [Series] FROM OWTR T0 WHERE T0.Series = 21";
                    dt = hana.Get(query);
                    dataGridView1.DataSource = dt;
                    break;
            }
        }




        static DataTable GetTable()
        {
            // BINDING MANUAL data
            var table = new DataTable();
            table.Columns.Add("Doc Entry", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Rows.Add("SI", "SI");
            table.Rows.Add("DR", "DR");
            table.Rows.Add("CST", "CST");
            return table;
        }
    }
}

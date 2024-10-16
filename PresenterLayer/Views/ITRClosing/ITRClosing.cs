using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;
using zDeclare;
using DirecLayer;
using MetroFramework;
using PresenterLayer.Helper;
using ServiceLayer.Services;

namespace PresenterLayer.Views
{
    public partial class frmDelivery : MetroForm
    {
        DECLARE dc = new DECLARE();
        private string oGroupCode = "0";
        private string oAreaSupervisor = "0";
        private string oSeries = "";
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        bool SelectAll = false;
        public frmDelivery()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            helper = new DataHelper();
        }
        void CreateGrid()
        {
            //DocEntry,Doc No.,CardCode,Card Name,DocDate,Remarks

            var col0 = new DataGridViewCheckBoxColumn();
            var col1 = new DataGridViewTextBoxColumn();
            var col2 = new DataGridViewTextBoxColumn();
            var col3 = new DataGridViewTextBoxColumn();
            var col4 = new DataGridViewTextBoxColumn();
            var col5 = new DataGridViewTextBoxColumn();
            var col6 = new DataGridViewTextBoxColumn();
            var col7 = new DataGridViewTextBoxColumn();
            var col8 = new DataGridViewTextBoxColumn();
            var col9 = new DataGridViewTextBoxColumn();
            var col10 = new DataGridViewTextBoxColumn();

            col0.Width = 20;
            col0.Frozen = true;

            col1.Name = "DocEntry";
            col1.HeaderText = "DocEntry";
            col1.Width = 100;
            col1.Visible = false;
            col1.Frozen = true;

            col2.Name = "DocNo";
            col2.HeaderText = "Doc No.";
            col2.Width = 100;
            col2.Frozen = true;

            col3.Name = "CardCode";
            col3.HeaderText = "Bp Code";
            col3.Width = 150;

            col4.Name = "CardName";
            col4.HeaderText = "Bp Name";
            col4.Width = 300;

            col5.Name = "DocDate";
            col5.HeaderText = "DocDate";
            col5.Width = 100;

            col6.Name = "Remarks";
            col6.HeaderText = "Remarks";
            col6.Width = 400;

            col7.Name = "Series";
            col7.HeaderText = "Series";
            col7.Width = 100;
            col7.Frozen = true;

            col8.Name = "DevDate";
            col8.HeaderText = "Delivery Date";
            col8.Width = 100;

            col9.Name = "PO";
            col9.HeaderText = "PO No.";
            col9.Width = 100;

            col10.Name = "DR";
            col10.HeaderText = "DR No.";
            col10.Width = 100;


            dgvItems.Columns.Add(col0);
            dgvItems.Columns.Add(col1);
            dgvItems.Columns.Add(col7);
            dgvItems.Columns.Add(col2);
            dgvItems.Columns.Add(col3);
            dgvItems.Columns.Add(col4);
            dgvItems.Columns.Add(col5);
            dgvItems.Columns.Add(col8);
            dgvItems.Columns.Add(col6);
            dgvItems.Columns.Add(col9);
            dgvItems.Columns.Add(col10);

            dataGridLayout(dgvItems);
        }
        public static void dataGridLayout(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.MultiSelect = true;
            dgv.ReadOnly = false;
            foreach(DataGridViewColumn col in dgv.Columns)
            {
                if (col.Index != 0) { col.ReadOnly = true; }
            }
        }

        private void frmCloseDocument_Load(object sender, EventArgs e)
        {
            CreateGrid();
        }

        void loadDR(string DFrom,string DTo,string AreaVisor,string Group)
        {
            dgvItems.Rows.Clear();
            string query = "";
            if (!string.IsNullOrEmpty(txtBPCode.Text) && string.IsNullOrEmpty(oSeries))
            {
                query = $"SELECT A.DocEntry,(SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum,A.CardCode,A.CardName,A.DocDate,A.DocDueDate,A.Comments,A.U_PONo,A.U_DRNo " +
                         $"FROM OWTQ A INNER JOIN OCRD B ON A.CardCode = B.CardCode " +
                         $"Where A.DocDate between '{DFrom}' and '{DTo}' And B.GroupCode = '{Group}' and A.CardCode = '{txtBPCode.Text}' and DocStatus = 'O'";
                //$"Where A.DocDate between '{DFrom}' and '{DTo}' And B.GroupCode = '{Group}' And B.DfTcnician = '{AreaVisor}' and A.CardCode = '{txtBPCode.Text}' and DocStatus = 'O'";
            }
            else if (!string.IsNullOrEmpty(oSeries))
            {
                query = $"SELECT A.DocEntry,(SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum,A.CardCode,A.CardName,A.DocDate,A.DocDueDate,A.Comments,A.U_PONo,A.U_DRNo " +
                       $"FROM OWTQ A INNER JOIN OCRD B ON A.CardCode = B.CardCode " +
                       $"Where A.DocDate between '{DFrom}' and '{DTo}' And B.GroupCode = '{Group}' and A.CardCode = '{txtBPCode.Text}' and A.Series = '{oSeries}' and DocStatus = 'O'";
               // $"Where A.DocDate between '{DFrom}' and '{DTo}' And B.GroupCode = '{Group}' And B.DfTcnician = '{AreaVisor}' and A.CardCode = '{txtBPCode.Text}' and A.Series = '{oSeries}' and DocStatus = 'O'";
            }
            else 
            {
                query = $"SELECT A.DocEntry,(SELECT T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001 and T0.Series = A.Series) Series,A.DocNum,A.CardCode,A.CardName,A.DocDate,A.DocDueDate,A.Comments,A.U_PONo,A.U_DRNo " +
                         $"FROM OWTQ A INNER JOIN OCRD B ON A.CardCode = B.CardCode " +
                         $"Where A.DocDate between '{DFrom}' and '{DTo}' And B.GroupCode = '{Group}' and DocStatus = 'O'";
                //$"Where A.DocDate between '{DFrom}' and '{DTo}' And B.GroupCode = '{Group}' And B.DfTcnician = '{AreaVisor}' and DocStatus = 'O'";
            }
            
            var dt = hana.Get(query);

            foreach(DataRow row in dt.Rows)
            {
                var docEntry = row["DocEntry"];
                var docNum = row["DocNum"];
                var series = row["Series"];
                var cardcode = row["CardCode"];
                var cardname = row["CardName"];
                var docdate = row["DocDate"];
                var duedate = row["DocDueDate"];
                var remarks = row["Comments"];

                object[] _row = { false,docEntry,series,docNum,cardcode,cardname,Convert.ToDateTime(docdate).ToString("MM/dd/yyyy"), Convert.ToDateTime(duedate).ToString("MM/dd/yyyy"), remarks };

                dgvItems.Rows.Add(_row);
            }
        }

        private void pbList1_Click(object sender, EventArgs e)
        {
            //SELECT empID,lastName,position FROM OHEM
            //SELECT posID,name FROM OHPS

            //string q = "SELECT A.empID,(A.lastName + A.firstName) [Name],B.name FROM OHEM A Inner Join OHPS B ON A.position = B.posID";

            var fS = new frmSearch2();
            fS.oSearchMode = "OHEM";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                oAreaSupervisor = fS.oCode;
                txtSupervisor.Text = fS.oName;

                loadDR(dtFrom.Value.ToString("yyyy-MM-dd"), dtTo.Value.ToString("yyyy-MM-dd"), oAreaSupervisor, oGroupCode);
            }
        }

        private void frmDelivery_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the Document? Unsaved data will be lost.", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void pbList2_Click(object sender, EventArgs e)
        {
            var fS = new frmSearch2();
            fS.oSearchMode = "OCRG";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                oGroupCode = fS.oCode;
                txtBPGroup.Text = fS.oName;

                loadDR(dtFrom.Value.ToString("yyyy-MM-dd"), dtTo.Value.ToString("yyyy-MM-dd"), oAreaSupervisor, oGroupCode);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close selected documents?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                CloseDoc();
            }
        }


        void CloseDoc()
        {
            bool _Error = false;
            int ctr = 0;
            int _counter = 1;
            foreach (DataGridViewRow a in dgvItems.Rows)
            {
                if ((bool)a.Cells[0].Value == true)
                {
                    ctr++;
                }
            }

            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if ((bool)row.Cells[0].Value == true)
                {
                    var oItr = $"InventoryTransferRequests({row.Cells[1].Value})/Close";

                    var serviceLayerAccess = new ServiceLayerAccess();
                    
                    StaticHelper._MainForm.Progress($"Closing Delivery Doc No. {row.Cells[3].Value} | {_counter} of {ctr}",ctr  , ctr);

                    if (serviceLayerAccess.ServiceLayer_Posting(null, "POST", oItr, "DocEntry", out string output, out string val))
                    {
                        StaticHelper._MainForm.ShowMessage($"Doc No. {row.Cells[3].Value} successfully closed.");
                    }
                    else
                    {
                        //ERROR
                        _Error = true;
                        StaticHelper._MainForm.ShowMessage(output, true);
                    }
                    _counter++;
                }
            }

            loadDR(dtFrom.Value.ToString("yyyy-MM-dd"), dtTo.Value.ToString("yyyy-MM-dd"), oAreaSupervisor, oGroupCode);

        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            loadDR(dtFrom.Value.ToString("yyyy-MM-dd"), dtTo.Value.ToString("yyyy-MM-dd"), oAreaSupervisor, oGroupCode);
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            loadDR(dtFrom.Value.ToString("yyyy-MM-dd"), dtTo.Value.ToString("yyyy-MM-dd"), oAreaSupervisor, oGroupCode);
        }
        void New()
        {
            txtBPGroup.Text = "";
            txtSupervisor.Text = "";

            dgvItems.Rows.Clear();
        }
        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvItems.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(SelectAll == false)
            {
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (SelectAll == false)
                    {
                        row.Cells[0].Value = true;
                    }
                }
                SelectAll = true;
            }
            else
            {
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (SelectAll == true)
                    {
                        row.Cells[0].Value = false;
                    }
                }
                SelectAll = false;

            }
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var fS = new frmSearch2();
            fS.oSearchMode = "OCRD-ITRC";
            frmSearch2.Param1 = oGroupCode;
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                txtBPCode.Text = fS.oCode;
                //txtBPGroup.Text = fS.oName;

                loadDR(dtFrom.Value.ToString("yyyy-MM-dd"), dtTo.Value.ToString("yyyy-MM-dd"), oAreaSupervisor, oGroupCode);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var fS = new frmSearch2();
            fS.oSearchMode = "SERIES-ITRC";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                oSeries = fS.oCode;
                txtSeries.Text = fS.oName;
                //txtBPGroup.Text = fS.oName;

                loadDR(dtFrom.Value.ToString("yyyy-MM-dd"), dtTo.Value.ToString("yyyy-MM-dd"), oAreaSupervisor, oGroupCode);
            }
            //SELECT T0.SeriesCode,T0.SeriesName FROM NNM1 T0 Where T0.ObjectCode = 1250000001
        }

        private void frmDelivery_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }
    }
}

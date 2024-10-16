using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirecLayer;
using PresenterLayer.Helper;

namespace PresenterLayer.Views
{
    public partial class UcUDF_itr : UserControl
    {
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        public UcUDF_itr()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            helper = new DataHelper();
            LoadTransferType();
        }
        void LoadTransferType()
        {
            var query = "";
            var queryResult = new DataTable();
            query = GetTransferTypeQuery();
            queryResult = hana.Get(query);
            cbTransferType.DisplayMember = "Name";
            cbTransferType.ValueMember = "Code";
            cbTransferType.DataSource = queryResult;
        }

        string GetTransferTypeQuery()
        {
            return $"SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [@TRANSFER_TYPE] Order by Name";
        }

        private void dtPostingDate_ValueChanged(object sender, EventArgs e)
        {
            txtPostingDate.Text = dtPostingDate.Text;
            frmDT_UDF.DocDate = dtPostingDate.Text;
        }

        private void dtDocDate_ValueChanged(object sender, EventArgs e)
        {
            txtDeliveryDate.Text = dtDocDate.Text;
            frmDT_UDF.DocDueDate = dtDocDate.Text;
        }

        private void dtDueDate_ValueChanged(object sender, EventArgs e)
        {
            txtDueDate.Text = dtDueDate.Text;
            frmDT_UDF.TaxDate = dtDueDate.Text;
        }

        private void pbFromWhsList_Click(object sender, EventArgs e)
        {
            frmSearch2 fS = new frmSearch2();
            fS.oSearchMode = "OWHS";
            frmSearch2.Param1 = "C";
            frmSearch2._title = "List of Warehouses";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                txtFWhsCode.Text = fS.oCode;
                frmDT_UDF.frmWhs = fS.oCode;
            }
        }

        
        //public void NumSeries()
        //{
        //    try
        //    {
        //        string sdata = "SELECT SeriesName FROM NNM1 Where ObjectCode = 1250000001";
        //        DataTable dt = hana.Get(sdata); //DataAccess.Select(DataAccess.conStr("HANA"), sdata);

        //        Application.DoEvents();
        //        CmbSeries.DisplayMember = "SeriesName";
        //        CmbSeries.DataSource = dt;
        //    }
        //    catch (Exception ex) { }
        //}

        private void PbVatGroup_Click(object sender, EventArgs e)
        {
            frmSearch2 fS = new frmSearch2();
            fS.oSearchMode = "OVTG";
            frmSearch2.Param1 = "O";
            frmSearch2._title = "Tax Code";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                TxtVat.Text = fS.oCode;
                frmDT_UDF.Vat = fS.oCode;
            }
        }

        private void CbTransferType_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmDT_UDF.TransactionType = cbTransferType.Text;
        }
    }
}

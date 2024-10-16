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

namespace PresenterLayer.Views
{
    public partial class UcUDF_delivery : UserControl
    {
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        public UcUDF_delivery()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            helper = new DataHelper();
        }
        
        private void UcUDF_delivery_Load(object sender, EventArgs e)
        {
            var dt = new DataTable();
            var query = "SELECT '' [Code], '' [Name] UNION SELECT Code, Name FROM [@DOC_TYPE]";
            dt = hana.Get(query);

            CmbDocumentType.DisplayMember = "Name";
            CmbDocumentType.ValueMember = "Code";
            CmbDocumentType.DataSource = dt;
        }

        private void pbVatGroup_Click(object sender, EventArgs e)
        {
            var fS = new frmSearch2();
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

        private void dtDocDate_ValueChanged(object sender, EventArgs e)
        {
            txtDeliveryDate.Text = dtDocDate.Text;
            frmDT_UDF.DocumentDate = dtDocDate.Text;
        }

        private void CmbDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmDT_UDF.DocumentType = CmbDocumentType.SelectedValue.ToString();
        }

        private void DtDocumentDate_ValueChanged(object sender, EventArgs e)
        {
            TxtDocumentDate.Text = DtDocumentDate.Text;
            frmDT_UDF.DocumentDate2 = DtDocumentDate.Text;
        }

        private void DtPostingDate_ValueChanged(object sender, EventArgs e)
        {
            TxtPostingDate.Text = DtPostingDate.Text;
            frmDT_UDF.PostingDate = DtPostingDate.Text;

            txtDeliveryDate.Text = DtPostingDate.Text;
            frmDT_UDF.DocumentDate = DtPostingDate.Text;
        }
    }
}

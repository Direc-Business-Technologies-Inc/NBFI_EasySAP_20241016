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
    public partial class UcUDF_salesorder : UserControl
    {
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        public UcUDF_salesorder()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            helper = new DataHelper();
            LoadTransferType();
        }
        void LoadTransferType()
        {
            var dt = new DataTable();
            var query = "SELECT '' [Code], '' [Name] UNION SELECT Code, Name FROM [@DOC_TYPE]";
            dt = hana.Get(query);
            cbDocumentType.DisplayMember = "Name";
            cbDocumentType.ValueMember = "Code";
            cbDocumentType.DataSource = dt;
        }

        string GetTransferTypeQuery()
        {
            return $"SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [@TRANSFER_TYPE] Order by Name";
        }
        private void CbTransferType_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmDT_UDF.DocumentType = cbDocumentType.Text;
        }
    }
}

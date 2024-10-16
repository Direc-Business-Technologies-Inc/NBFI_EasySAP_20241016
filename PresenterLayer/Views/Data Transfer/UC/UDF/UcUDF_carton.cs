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
    public partial class UcUDF_carton : UserControl
    {
        public UcUDF_carton()
        {
            InitializeComponent();
        }

        private void pbTransactionType_Click(object sender, EventArgs e)
        {
            frmSearch2 fS = new frmSearch2();
            fS.oSearchMode = "@CM - Get Transaction Type";
            fS.oFormTitle = "List of Transaction Type";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                frmDT_UDF.TransactionType = fS.oCode;
                txtTransactionType.Text = fS.oCode;
            }
        }

        private void pbTargetWarehouse_Click(object sender, EventArgs e)
        {
            frmSearch2 fS = new frmSearch2();
            fS.oSearchMode = "@CM - Get target warehouse";
            fS.oFormTitle = "List of Target Warehouse";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                frmDT_UDF.TargetWarehouse = fS.oCode;
                txtTargetWarehouse.Text = fS.oCode;
            }
        }

        private void pbLastWarehouse_Click(object sender, EventArgs e)
        {
            frmSearch2 fS = new frmSearch2();
            fS.oSearchMode = "@CM - Get target warehouse";
            fS.oFormTitle = "List of Target Warehouse";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                frmDT_UDF.LastWarehouse = fS.oCode;
                txtLastWarehouse.Text = fS.oCode;
            }
        }
    }
}

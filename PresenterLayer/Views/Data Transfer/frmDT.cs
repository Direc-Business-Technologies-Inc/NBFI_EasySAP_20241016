using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.IO;
using System.Linq;
using PresenterLayer.Helper;

namespace PresenterLayer.Views
{
    public partial class frmDT : MetroForm
    {
        frmDT_UDF frmUDF;
        

        public frmDT()
        {
            InitializeComponent();
        }
        
        private void frmDT_Load(object sender, EventArgs e)
        {
            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add(new UcMainMenu(this,StaticHelper._MainForm));
        }
        
        private void frmDT_LocationChanged(object sender, EventArgs e)
        {
            if (frmUDF != null)
            {
                frmUDF.Location = new Point(Right, Top);
                frmUDF.Height = Height;
            }
        }

        public void DisplayUdf (string obj)
        {
            Form fc = Application.OpenForms["frmDT_UDF"];
            frmUDF = new frmDT_UDF();
            frmUDF.StartPosition = FormStartPosition.Manual;
            frmUDF.Location = new Point(Right, Top);
            frmUDF.Height = Height;
            frmUDF.MdiParent = StaticHelper._MainForm;
            frmUDF.objType = obj;
            frmUDF.Show();
            
            if (fc != null)
            {
                closeUDF();
            }
        }

        public void closeUDF ()
        {
            try
            {
                frmUDF.Close();
            }
            catch { }
        }

        private void frmDT_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (PublicStatic.DtRunID != null)
            //{
            //    var context = new SAOContext();

            //    var header = context.DocumentHeaders.SingleOrDefault(x => x.Session == PublicStatic.DtRunID && x.User == Environment.MachineName);
            //    context.DocumentHeaders.Remove(header);

            //    var row = context.DocumentLines.SingleOrDefault(x => x.Session == PublicStatic.DtRunID && x.User == Environment.MachineName);
            //    context.DocumentLines.Remove(row);

            //    context.SaveChanges();

            //}

            Form fc = Application.OpenForms["frmDT_UDF"];

            if (fc != null)
            {
                closeUDF();
            }
        }

        private void frmDT_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }
    }
}

using MetroFramework.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PresenterLayer.Views.Main;
using PresenterLayer.Helper;
using DirecLayer;

namespace DirecLayer
{
    public partial class frmQueryGenerator : MetroForm
    {
        MainForm frmMain;
        public frmQueryGenerator()
        {
            InitializeComponent();
            frmMain = StaticHelper._MainForm;
            ActiveControl = QString;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            { ExecuteQuery(); }
            else if (keyData == Keys.Escape)
            { Dispose(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void ExecuteQuery()
        { gvDataTable.DataSource = DataAccess.Select(DataAccess.conStr("HANA"), QString.Text/*, frmMain*/); }

        private void btnCommand_Click(object sender, System.EventArgs e)
        {
             ExecuteQuery(); 
            //gvDataTable.DataSource = DataAccessSQL.Select(DataAccess.conStr("SQL"), QString.Text, frmMain);
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        { Dispose(); }

        private void frmQueryGenerator_Resize(object sender, System.EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                if (Size == new Size(MinimumSize.Width, MinimumSize.Height))
                {
                    Size = new Size(Screen.PrimaryScreen.Bounds.Width - 50, Screen.PrimaryScreen.Bounds.Height - 220);
                    Location = new Point(25, 25);
                }
                else { Size = new Size(MinimumSize.Width, MinimumSize.Height); }
            }
        }
    }
}

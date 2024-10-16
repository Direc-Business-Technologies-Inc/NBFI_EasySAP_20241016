using MetroFramework.Forms;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PresenterLayer.Views.Main;

namespace DirecLayer
{
    public partial class frmShortcut : MetroForm
    {

        private MainForm frmMain;
        public frmShortcut(MainForm frmMain)
        {
            InitializeComponent();
            this.frmMain = frmMain;
        }

        private void frmShortcut_Load(object sender, EventArgs e)
        {

        }
    }
}

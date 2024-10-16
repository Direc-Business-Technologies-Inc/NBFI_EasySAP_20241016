using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresenterLayer
{
    public partial class frmPackingListConcessionaire_Search : MetroForm
    {
        int max_width = Screen.PrimaryScreen.Bounds.Width - 220;
        int max_height = Screen.PrimaryScreen.Bounds.Height - 200;
        int iColumn = 1, iRow = 0;

        private string search;
        private string code;
        private string DocType;
        public string oSearchMode { get { return search; } set { search = value; } }
        public string @Param1, @Param2, @Param3, @Param4, _title;


        public frmPackingListConcessionaire_Search()
        {
            InitializeComponent();
        }

        private void frmPackingListConcessionaire_Search_Load(object sender, EventArgs e)
        {
            MaximumSize = new Size(max_width, max_height);
        }
    }
}

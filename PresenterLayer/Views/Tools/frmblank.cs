using PresenterLayer.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PresenterLayer.Views.Main;
using DirecLayer;

namespace DirecLayer
{
    public partial class frmblank : MetroFramework.Forms.MetroForm
    {
        public string title { get; set; } = "";
        public string query { get; set; } = "";
        public string value { get; set; }

        public Form frmName { get; set; }
        public MainForm frmMain { get; set; }
        
        public frmblank()
        {
            InitializeComponent();
        }

        private void frmblank_Load(object sender, EventArgs e)
        {
            Text = title;
            var dt = DataAccess.Select(DataAccess.conStr("HANA"), query);
            dgvBlank.DataSource = dt;
            dgvBlank.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgvBlank_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            value = dgvBlank.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            Dispose();
        }
    }
}

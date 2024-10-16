using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MetroFramework.Forms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PresenterLayer.Views.Main;

namespace DirecLayer
{ 
    public partial class frmReportList : MetroForm
    {
        public string xcode, xcardcode,oDocKey;
        public DataTable dt;
        MainForm frmMain;
        public frmReportList(MainForm frmMain)
        {
            InitializeComponent();
            this.frmMain = frmMain;
        }

        private void frmReportList_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dt;
            dataGridLayout(dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            string name = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();


            var a = new frmCrystalReports2();
            a.zPath = path;
            a.zReportName = name;
            a.oDocKey = oDocKey;
            a.fromReportList = true;
            a.MdiParent = frmMain;
            a.Show();
        }

        private void dataGridLayout(DataGridView dgv)
        {
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;

            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Width = 200;
        }
    }
}

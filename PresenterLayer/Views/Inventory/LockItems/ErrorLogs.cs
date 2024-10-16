using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sap.Data.Hana;
using zDeclare;
using System.Diagnostics;
using System.Threading;
using MetroFramework.Forms;
using System.Runtime.InteropServices;
using PresenterLayer.Helper;

namespace PresenterLayer
{
    public partial class ErrorLogs : MetroForm
    {
        public ErrorLogs()
        {
            InitializeComponent();
        }

        private void ErrorLogs_Load(object sender, EventArgs e)
        {
            //CreateGrid();

            dataGridView1.DataSource = DECLARE._error;
        }
        void CreateGrid()
        {
            var col1 = new DataGridViewTextBoxColumn();
            var col2 = new DataGridViewTextBoxColumn();
            var col3 = new DataGridViewTextBoxColumn();


            col1.Name = "ErrorCode";
            col1.HeaderText = "Code";
            col1.Width = 150;
            col1.Frozen = true;

            col2.Name = "ErrorDate";
            col2.HeaderText = "Data";
            col2.Width = 150;
            col2.Frozen = true;

            col3.Name = "ErrorMessage";
            col3.HeaderText = "Error Message";
            col3.Width = 300;
            col3.ReadOnly = true;

            dataGridView1.Columns.Add(col1);
            dataGridView1.Columns.Add(col2);
            dataGridView1.Columns.Add(col3);


            dataGridLayout(dataGridView1);
        }
        public static void dataGridLayout(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.MultiSelect = true;
            dgv.ReadOnly = true;
        }

        private void copyTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            Clipboard.SetDataObject(dataObj, true);

            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            StaticHelper._MainForm.ShowMessage("Grid view data has been copied to clipboard");
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var mousePosition = dataGridView1.PointToClient(Cursor.Position);
                msMenu.Show(dataGridView1, mousePosition);
            }
        }
    }
}

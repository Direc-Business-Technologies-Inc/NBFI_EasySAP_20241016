using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using zDeclare;
using PresenterLayer.Helper;
using DirecLayer;

namespace PresenterLayer
{
    public partial class frmpackinglist_udf : MetroForm
    {
        DataTable dtIT_Items = new DataTable();
        DataTable dtIT = new DataTable();
        DataTable dtUDF = new DataTable();
        DECLARE dc = new DECLARE();
        SAPHanaAccess hana { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        DataHelper helper { get; set; }
        public static string oTableName;

        public frmpackinglist_udf()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            msSql = new SAPMsSqlAccess();
            helper = new DataHelper();
        }

        private void frmpackinglist_udf_Load(object sender, EventArgs e)
        {
            dataGridLayout(gvUDF);
            _LoadData();
        }

        public static void dataGridLayout(DataGridView dgv)
        {
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgv.ReadOnly = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.MultiSelect = false;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersVisible = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            
            dgv.ColumnCount = 3;
            dgv.Columns[0].Name = "Code";
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[1].Name = "Name";
            dgv.Columns[2].Name = "Field";
            dgv.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(191, 235, 245);
        }

        public void _LoadData()
        {
            string UDF = SP.UDF("[@OPKL]");

            dtUDF = hana.Get(UDF);
            gvUDF.Rows.Clear();
            int i = 0;
            foreach (DataRow dr in dtUDF.Rows)
            {
                string[] row = new string[] { dr["AliasID"].ToString(), dr["Descr"].ToString() };
                gvUDF.Rows.Add(row);
                var dt1 = new DataTable();
                var cmb = new DataGridViewComboBoxCell();
                if (dr["Table"].ToString() == "")
                {
                    dt1 = hana.Get($"SELECT '' [Code] ,'' [Name] UNION SELECT FldValue, Descr FROM UFD1 WHERE TableID ='OPOR' AND FieldID = { dr["FieldID"].ToString()}");
                    if (dt1.Rows.Count > 1)
                    {
                        cmb.DisplayMember = "Name";
                        cmb.ValueMember = "Code";
                        cmb.DataSource = dt1;

                        gvUDF.Rows[i].Cells["Field"] = cmb;
                    }
                    else
                    {
                        gvUDF.Rows[i].Cells["Field"] = new DataGridViewTextBoxCell();
                    }
                }
                else
                {
                    dt1 = hana.Get($"SELECT '' [Code] ,'' [Name] UNION SELECT Code, Name FROM [@{ dr["Table"].ToString() }]");
                    cmb.DisplayMember = "Name";
                    cmb.ValueMember = "Code";
                    cmb.DataSource = dt1;

                    gvUDF.Rows[i].Cells["Field"] = cmb;
                }

                if (DECLARE.udf.Exists(x => x.FieldCode == gvUDF.Rows[i].Cells["Code"].Value.ToString()))
                {
                    var data = (from x in DECLARE.udf where x.FieldCode == dr["AliasID"].ToString() select new { FieldValue = x.FieldValue });
                    foreach (var x in data)
                    {
                        string val = x.FieldValue;
                        if (val == "0")
                        {
                            val = "";
                        }
                        gvUDF.Rows[i].Cells["Field"].Value = val;
                    }
                }

                i++;
            }
        }

    }
}

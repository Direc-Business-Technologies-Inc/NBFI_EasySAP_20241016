using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using zDeclare;
using MetroFramework.Forms;
using System.Runtime.InteropServices;
using DirecLayer;
using PresenterLayer.Helper;

namespace PresenterLayer
{
    public partial class frmItemSelection : MetroForm
    {
        private string oCode, _StyleCode;
        
        frmFreezeItems frmFreezeitems;
        SAPHanaAccess hana { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        DataHelper helper { get; set; }
        //Public Var
        public static string oStyleCode, oColorCode, oSection, oBPCode;
        public static DateTime oDocDate;
        public frmItemSelection(frmFreezeItems frmFreezeitems)
        {
            InitializeComponent();
            this.frmFreezeitems = frmFreezeitems;
            helper = new DataHelper();
            hana = new SAPHanaAccess();
            msSql = new SAPMsSqlAccess();
        }
        

        private void pbColorList_Click(object sender, EventArgs e)
        {
            ViewList("@PRCOLOR", out oCode, "Lists of Color", txtStyleCode.Text);
            txtColorCode.Text = oCode;

            string query = "SELECT Code,Name FROM [@PRCOLOR] WHERE Code = '" + oCode + "'";
            
            var dt = hana.Get(query);
            if (helper.DataTableExist(dt))
            {
                txtColorDesc.Text = helper.ReadDataRow(dt, "Name", "", 0);
                //Check Section
                CheckSection(_StyleCode, oCode);
            }
            else
            {
                txtColorDesc.Text = "";
            }

            //GET ITEM LIST
            if (txtSection.Text != "" && txtStyleCode.Text != "")
            {
                LoadItemsSize();
            }
        }

        private void pbSection_Click(object sender, EventArgs e)
        {
            ViewList("@Section_S", out oCode, "Lists of Section", txtStyleCode.Text, txtColorCode.Text);
            txtSection.Text = oCode;
            LoadItemsSize();
        }
        private CheckBox ColumnCheckbox(DataGridView dataGridView)
        {
            var checkBox = new CheckBox();
            checkBox.Size = new Size(15, 15);
            checkBox.BackColor = Color.Transparent;

            // Reset properties
            checkBox.Padding = new Padding(0);
            checkBox.Margin = new Padding(0);
            checkBox.Text = "";

            // Add checkbox to datagrid cell
            dataGridView.Controls.Add(checkBox);
            DataGridViewHeaderCell header = dataGridView.Columns[0].HeaderCell;
            checkBox.Location = new Point(
                (header.ContentBounds.Left +
                 (header.ContentBounds.Right - header.ContentBounds.Left + checkBox.Size.Width)
                 / 2) - 2,
                (header.ContentBounds.Top +
                 (header.ContentBounds.Bottom - header.ContentBounds.Top + checkBox.Size.Height)
                 / 2) - 2);
            return checkBox;
        }

       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool isSelected;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value == null) { isSelected = false; } else isSelected = (bool)row.Cells[0].Value;

                if (DECLARE._items.Exists(a => a.ItemCode == row.Cells[1].Value.ToString()))
                {
                    //If exist
                    foreach (var x in DECLARE._items.Where(a => a.ItemCode == row.Cells[1].Value.ToString()))
                    {
                        x.selected = isSelected;
                    }
                }
                else
                {
                    DECLARE._items.Add(new DECLARE.items
                    {
                        selected = isSelected,
                        ItemCode = row.Cells[1].Value.ToString(),
                        ItemName = row.Cells[2].Value.ToString()
                    });

                    StaticHelper._MainForm.ShowMessage("Item(s) added successfully.");
                }
            }

            frmFreezeitems.RefreshData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void frmItemSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        private void ViewList(string SearchTable
                          , out string Code
                          , string title
                          , [Optional] string Param1
                          , [Optional] string Param2
                          , [Optional] string Param3
                          , [Optional] string Param4
                          )
        {
            var fS = new frmSearch2();
            fS.oSearchMode = SearchTable;
            //Set Parameter 1
            frmSearch2.Param1 = Param1;
            //Set Parameter 2
            frmSearch2.Param2 = Param2;
            //Set Parameter 3
            frmSearch2.Param3 = Param3;
            //Set Parameter 4
            frmSearch2.Param3 = Param4;
            //Set Title
            frmSearch2._title = title;
            fS.ShowDialog();

            Code = fS.oCode;
        }

        private void pbBPList_Click(object sender, EventArgs e)
        {
            ViewList("@PRSTYLE", out oCode, "Lists of Styles");
            txtStyleCode.Text = oCode;

            string query = "SELECT Code,Name FROM [@PRSTYLE] WHERE Code = '" + oCode + "'";
            
            var dt = hana.Get(query);
            if (helper.DataTableExist(dt))
            {
                _StyleCode = oCode;
                txtStyleDesc.Text = helper.ReadDataRow(dt,"Name","",0);
                txtColorCode.Text = "";
                txtSection.Text = "";
                txtColorDesc.Text = "";
                //Check Color and Section
                CheckColor(oCode);
            }
            else
            {
                txtStyleDesc.Text = "";
            }

            //GET ITEM LIST
            if (txtSection.Text != "" && txtColorCode.Text != "")
            {
                LoadItemsSize();
            }
        }
        void CheckColor(string StyleCode)
        {
            string query = query = $"SELECT DISTINCT A.U_Color,(SELECT Name FROM [@PRCOLOR] Z Where Z.Code = A.U_Color) [Name] FROM OITM A Where A.U_StyleCode = '{StyleCode.Replace("'", "''")}'";
            
            var dtColor = hana.Get(query);

            if (dtColor.Rows.Count == 1)
            {
                string ColorCode = dtColor.Rows[0][0].ToString();
                txtColorCode.Text = ColorCode; //Color Code
                txtColorDesc.Text = dtColor.Rows[0]["Name"].ToString();

                //Check if section has 1 value
                var query2 = $"SELECT DISTINCT Replace(U_Section,'''','''') Section FROM OITM WHERE U_StyleCode = '{StyleCode.Replace("'", "''")}' and U_Color = '{ColorCode.Replace("'", "''")}'";
                
                var dtSection = hana.Get(query2);

                if (dtSection.Rows.Count == 1)
                {
                    txtSection.Text = dtSection.Rows[0][0].ToString();
                }
            }
        }
        void CheckSection(string StyleCode, string ColorCode)
        {
            //Check if section has 1 value
            var query = $"SELECT DISTINCT Replace(U_Section,'''','''') Section FROM OITM WHERE U_StyleCode = '{StyleCode.Replace("'", "''")}' and U_Color = '{ColorCode.Replace("'", "''")}'";
            var dtSection = hana.Get(query);

            if (dtSection.Rows.Count == 1)
            {
                txtSection.Text = dtSection.Rows[0][0].ToString();
            }
        }
        private void LoadItemsSize()
        {
            try
            {
                dataGridView1.Columns.Clear();

                var dt = new DataTable();
                dt = hana.Get("SELECT A.ItemCode,A.ItemName,A.FrgnName [Style],(SELECT Name FROM [@PRCOLOR] Z Where Z.Code = A.U_Color) [Color],U_Size [Size]" +
                                                                     " FROM OITM A Where A.U_Color = '" + txtColorCode.Text + "' And A.U_StyleCode = '" + txtStyleCode.Text + "'" +
                                                                     " And A.U_Section = '" + txtSection.Text.Replace("'", "''") + "' And A.frozenFor = 'N' Order By U_Size");
                //check box column
                var chk = new DataGridViewCheckBoxColumn();
                dataGridView1.Columns.Add(chk);
                //bind data
                dataGridView1.DataSource = dt;
                DECLARE.dataGridLayout(dataGridView1);

                //disable column sort
                dataGridView1.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
                dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }
            catch (Exception ex)
            {

            }

        }
    }
}

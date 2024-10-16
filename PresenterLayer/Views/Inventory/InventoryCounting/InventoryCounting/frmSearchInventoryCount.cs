using DirecLayer;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using zDeclare;

namespace PresenterLayer
{


    public partial class frmSearchInventoryCount : MetroForm
    {
        SAPHanaAccess hana { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        DECLARE dc = new DECLARE();
        private string search;
        private string DocType;
        private string code;
        private string name;
        public string oSearchMode { get { return search; } set { search = value; } }
        public string oCode { get { return code; } set { code = value; } }
        public string oName { get { return name; } set { name = value; } }

        private static int defaultColumn = 1, _rowIndex = 0;

        public static string @Param1, @Param2, @Param3, @Param4, _title;

        private void dgvSearchList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        private void dgvSearchList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                oCode = dgvSearchList.Rows[e.RowIndex].Cells[0].Value.ToString();
                if (dgvSearchList.Columns.Count > 1)
                {
                    oName = dgvSearchList.Rows[e.RowIndex].Cells[1].Value.ToString();
                }

                this.Close();
            }
        }

        private void dgvSearchList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        private void dgvSearchList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvSearchList.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            Choose();
        }

        private void Search()
        {
            DataTable dt = null;
            string query = "";

            if (search == "OCRD")
            {
                query = "select cardcode,cardname from ocrd where cardtype = '" + @Param1 + "' and cardname like '" + txtSearch.Text + "%' order by cardcode";
                dt = hana.Get(query);
            }
            else if (search == "@PRSTYLE")
            {
                query = "SELECT Code ,Name FROM [@PRSTYLE] Where Name like '" + txtSearch.Text + "%'";
            }
            else if (search == "@PRCOLOR")
            {
                // query = "SELECT Code ,Name FROM [@PRCOLOR]";

                query = "SELECT DISTINCT A.U_Color,B.Name FROM OITM A Left Join [@PRCOLOR] B " +
                        " On A.U_Color = B.Code  Where A.U_StyleCode = '" + @Param1 + "' and B.Name like '" + txtSearch.Text + "%'";
            }
            else if (search == "@Section")
            {
                query = "SELECT DISTINCT U_Section FROM OITM WHERE U_StyleCode = '" + @Param1 + "' and U_Color = '" + @Param2 + "' and U_Section like '" + txtSearch.Text + "%'";
            }

            //Bind Data
            
            dt = hana.Get(query);
            dgvSearchList.DataSource = dt;

            //DECLARE.dataGridLayout(dgvSearchList);
            dataGridLayout(dgvSearchList);
        }

        void Choose()
        {
            if (allowMultiple == false)
            {
                int rowindex = dgvSearchList.CurrentCell.RowIndex;
                int colindex = dgvSearchList.CurrentCell.ColumnIndex;

                if (rowindex != -1)
                {
                    oCode = dgvSearchList.Rows[rowindex].Cells[0].Value.ToString();
                    if (dgvSearchList.Columns.Count > 1)
                    {
                        oName = dgvSearchList.Rows[rowindex].Cells[1].Value.ToString();
                    }

                    this.Dispose();
                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvSearchList.Rows)
                {
                    if (row.Selected == true)
                    {
                        DECLARE._multipleSelection.Add(new DECLARE.MultipleSelection { Code = row.Cells[0].Value.ToString(), Name = row.Cells[1].Value.ToString() });
                    }
                }
                this.Dispose();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvSearchList.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgvSearchList.Rows)
                {
                    if (row.Cells[defaultColumn].Value.ToString().ToUpper().Contains(txtSearch.Text.ToUpper()))
                    {
                        row.Selected = true;
                        _rowIndex = row.Index;
                        dgvSearchList.FirstDisplayedScrollingRowIndex = _rowIndex;
                        break;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }
        }

        private void frmSearchInventoryCount_Load(object sender, EventArgs e)
        {
            DECLARE._multipleSelection.Clear();

            txtSearch.Focus();
            lblTitle.Text = _title;

            DataTable dt = null;
            string query = "";

            if (search == "OCRD")
            {
                query = "select CardCode,CardName from OCRD where CardType = '" + @Param1 + "' Order by CardCode";
            }
            else if (search == "OCRD*")
            {
                query = "select CardCode,CardName from OCRD Order by CardCode";
            }
            else if (search == "OITM")
            {
                query = "SELECT  ItemCode" +
                                      ",ItemName" +
                                      ",FrgnName" +
                                      ",CodeBars" +
                               " FROM OITM Where InvntItem = 'Y' and frozenFor = 'N' ORDER BY ItemName ASC";
            }

            else if (search == "OWTQ")
            {
                query = "SELECT DocEntry,DocNum,CardCode" +
                                      ",CardName" +
                                      ",DocDate" +
                                      ",DocDueDate" +
                               " FROM OWTQ Where DocStatus = 'O' And CardCode = '" + @Param1 + "'";
            }
            else if (search == "OWTQ_NOBP")
            {
                query = "SELECT DocEntry,DocNum,CardCode" +
                                      ",CardName" +
                                      ",DocDate" +
                                      ",DocDueDate" +
                               " FROM OWTQ Where DocStatus = 'O'";
            }
            else if (search == "OPOR_BP")
            {
                query = "SELECT DocEntry,CardCode" +
                                      ",CardName" +
                                      ",DocDate" +
                                      ",DocDueDate" +
                                      ",TaxDate" +
                               " FROM OPOR Where CardCode = '" + @Param1 + "' and DocStatus = 'O'";
            }
            else if (search == "ORDR_BP")
            {
                query = "SELECT DocEntry,DocNum,CardCode" +
                                      ",CardName" +
                                      ",DocDate" +
                                      ",DocDueDate" +
                                      ",TaxDate" +
                               " FROM ORDR Where CardCode = '" + @Param1 + "'  and DocStatus = 'O'";
            }
            else if (search == "OWHS")
            {
                query = "SELECT WhsCode [Warehouse Code],WhsName [Warehouse Name] FROM OWHS";
            }
            else if (search == "OITW")
            {
                query = "SELECT WhsCode [Warehouse Code],WhsCode [Warehouse Code],OnHand [InStock] FROM OITW Where ItemCode = '" + @Param1 + "' And OnHand > 0";
            }
            else if (search == "@PRSTYLE")
            {
                query = "SELECT Code ,Name FROM [@PRSTYLE]";
            }
            else if (search == "@PRCOLOR")
            {
                query = "SELECT DISTINCT A.U_Color,(SELECT Name FROM [@PRCOLOR] Z Where Z.Code = A.U_Color) [Name] FROM OITM A Where A.U_StyleCode = '" + @Param1 + "'";
            }
            else if (search == "@Section")
            {
                query = "SELECT DISTINCT U_Section FROM OITM WHERE U_StyleCode = '" + @Param1 + "' and U_Color = '" + @Param2 + "'";
            }
            else if (search == "OVTG")
            {
                query = " select Code,Name,Rate from OVTG Where Category = '" + @Param1 + "' and Inactive = 'N'";
            }
            else if (search == "OVTG_VatEx")
            {
                query = " select Code,Name,Rate from OVTG where Inactive = 'N' and Rate = 0";
            }
            else if (search == "CRD1")
            {
                query = " select Address as AddressCode,Street,Zipcode,City from CRD1 Where AardCode = '" + @Param1 + "' and Adrestype = '" + @Param2 + "'   ";
            }
            else if (search == "@ItemType")
            {
                query = "SELECT DISTINCT U_ItemType FROM [@SECCAT]";
            }
            else if (search == "@AgeGroup")
            {
                query = "SELECT DISTINCT U_AgeGroup FROM [@SECCAT] Where U_ItemType = '" + @Param1 + "'";
            }
            else if (search == "OPLN")
            {
                query = "SELECT DISTINCT ListNum,ListName FROM OPLN";
            }
            else if (search == "OCTG")
            {
                query = "select GroupNum,PymntGroup  from OCTG";
            }
            else if (search == "OPKL_I")
            {
                query = "SELECT A.AbsEntry, A.Name, A.PickDate, A.Status, A.Remarks  FROM OPKL A LEFT JOIN PKL1 B ON A.AbsEntry = B.AbsEntry Where A.Canceled = 'N' and Status <> 'C' and B.BaseObject = '1250000001'";
            }
            else if (search == "OPKL_S")
            {
                query = "SELECT A.AbsEntry, A.Name, A.PickDate, A.Status, A.Remarks  FROM OPKL A LEFT JOIN PKL1 B ON A.AbsEntry = B.AbsEntry Where A.Canceled = 'N' and Status <> 'C' and B.BaseObject = '17'";
            }
            else if (search == "OSLP")
            {
                query = "SELECT SlpCode,SlpName FROM OSLP";
            }
            else if (search == "OINV")
            {
                query = "SELECT DocEntry,DocNum [Doc No.],CardCode [BP Code],CardName [BP Name],DocDate [Doc Date],DocStatus [Status] FROM OINV";
            }
            else if (search == "ORDR")
            {
                query = "SELECT DocEntry,DocNum [Doc No.],CardCode [BP Code],CardName [BP Name],DocDate [Doc Date],DocStatus [Status] FROM ORDR";
            }
            else if (search == "OPDN")
            {
                query = "SELECT DocEntry,DocNum [Doc No.],CardCode [BP Code],CardName [BP Name],DocDate [Doc Date],DocStatus [Status] FROM OPDN";
            }
            else if (search == "OWTR")
            {
                query = "SELECT DocEntry,DocNum [Doc No.],CardCode [BP Code],CardName [BP Name],DocDate [Doc Date],DocStatus [Status] FROM OWTR";
            }
            else if (search == "OINC")
            {
                query = "SELECT DocEntry,DocNum [Doc No.],CountDate [Count Date],Time [Time],Remarks FROM OINC Where Status = 'O'";
            }
            else if (search == "?")
            {
                query = "SELECT '' [Code], '' [Name]";
            }


            
            dt = hana.Get(query);
            dgvSearchList.DataSource = dt;
            //if (search != "OITM")
            //{
            //    //Bind Data
            //    dgvSearchList.DataSource = dt;
            //}
            //else
            //{
            //    //manual add rows
            //    CreateGrid();
            //    foreach(DataRow row in dt.Rows)
            //    {
            //        object[] x = { row[0].ToString()
            //                     , row[1].ToString()
            //                     , row[2].ToString()
            //                     , row[3].ToString()
            //        };

            //        dgvSearchList.Rows.Add(x);
            //    }   

            //}

            //DECLARE.dataGridLayout(dgvSearchList);
            dataGridLayout(dgvSearchList);
        }
        void CreateGrid()
        {
            dgvSearchList.Columns.Clear();
            dgvSearchList.Rows.Clear();

            var col0 = new DataGridViewTextBoxColumn();
            var col1 = new DataGridViewTextBoxColumn();
            var col2 = new DataGridViewTextBoxColumn();
            var col3 = new DataGridViewTextBoxColumn();

            col0.Name = "ItemCode";
            col0.HeaderText = "ItemCode";
            col0.Frozen = true;
            col0.ReadOnly = true;

            col1.Name = "ItemName";
            col1.HeaderText = "Item Description";
            col1.Frozen = true;
            col1.ReadOnly = true;

            col2.Name = "FrgnName";
            col2.HeaderText = "Foreign Name";
            col2.Width = 200;
            col2.ReadOnly = true;

            col2.Name = "CodeBars";
            col2.HeaderText = "CodeBars";
            col2.Width = 200;
            col2.ReadOnly = true;

            dgvSearchList.Columns.Add(col0);
            dgvSearchList.Columns.Add(col1);
            dgvSearchList.Columns.Add(col2);
            dgvSearchList.Columns.Add(col3);

            dataGridLayout(dgvSearchList);
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

            if (allowMultiple == false)
            {
                dgv.MultiSelect = false;
            }
            else
            {
                dgv.MultiSelect = true;

            }
        }

        public bool allowMultiple = false;

        public frmSearchInventoryCount([Optional]frmInventoryCount frmMain)
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            msSql = new SAPMsSqlAccess();
        }
    }
}

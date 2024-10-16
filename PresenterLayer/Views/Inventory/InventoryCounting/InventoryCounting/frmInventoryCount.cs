using DirecLayer;
using DomainLayer.Models;
using MetroFramework;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using PresenterLayer.Views.Main;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using zDeclare;

namespace PresenterLayer
{
    public partial class frmInventoryCount : MetroForm
    {

        public string oMode, oDocEntry;

        // oMode = A then ADD
        // oMode = U then UPDATE
        SAPMsSqlAccess msSql { get; set; }
        DataHelper helper { get; set; }
        SAPHanaAccess hana { get; set; }
        
        string Code, Name, oWHS, oWHSName, activeSearch;
        int activeColumn, currentRow, currentCol, defaultColumn;

        int max_height = Screen.PrimaryScreen.Bounds.Height - 200;
        int max_width = Screen.PrimaryScreen.Bounds.Width;

        int iColumn = 1, iRow = 0;

        public DataTable dtItems, dtWhs, dtActive;
        DateTime localDate = DateTime.Now;

        IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
        string userName = WindowsIdentity.GetCurrent().Name;
        //string compname = (System.Security.Principal.WindowsIdentity.GetCurrent().Name + Properties.Settings.Default.oLoginUser);
        string compname = (WindowsIdentity.GetCurrent().Name + SboCred.UserID);
        private void dgvItems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private string NextKey()
        {
            try
            {
                
                var dt = msSql.Get("SBO_GetNextAutoKey '1','Y'");
                var autokey = helper.ReadDataRow(dt, "AutoKey", "",0);

                return autokey;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvItems.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void dgvItems_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        private void dgvItems_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["WhsCode"].Value = oWHS;
            e.Row.Cells["WhsName"].Value = oWHSName;
        }

        private void dgvItems_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    dgvItems.CurrentCell = dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex + 1];
                    dgvItems.Rows[e.RowIndex].Selected = true;
                    dgvItems.Focus();

                    var mousePosition = dgvItems.PointToClient(Cursor.Position);

                    msItemsList.Show(dgvItems, mousePosition);
                }
            }
        }

        private void txtBarcode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barcode_string, query, itemCode, itemName;
                int _exist = 0;
                //Split string into array
                barcode_string = txtBarcode.Text;
                List<string> barcode = barcode_string.Split('|').ToList<string>();

                //Search itemCode
                query = $"SELECT TOP 1 A.ItemCode,A.ItemName FROM OITM A WHERE A.CodeBars = '{ barcode[0] } ' OR A.ItemCode = '{ barcode[0] }' ";

                var dt = new DataTable();
                
                dt = hana.Get(query);

                if (dt.Rows.Count > 0)
                {
                    itemCode = dt.Rows[0]["ItemCode"].ToString();
                    itemName = dt.Rows[0]["ItemName"].ToString();


                    if (itemCode != "")
                    {
                        foreach (DataGridViewRow row in dgvItems.Rows)
                        {
                            if (row.Cells[0].Value != null)
                            {
                                //check if exists
                                if (row.Cells[0].Value.ToString() == itemCode)
                                {
                                    try
                                    {
                                        //update
                                        row.Cells[4].Value = Convert.ToDouble(row.Cells[4].Value) + 1;
                                        txtBarcode.Text = "";
                                        txtBarcode.Focus();
                                        _exist = 1;
                                    }
                                    catch (Exception ex)
                                    { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
                                }
                            }
                        }

                        //if not exist add item
                        if (_exist == 0)
                        {
                            object[] a = { itemCode, itemName, oWHS, oWHSName, "1" };
                            dgvItems.Rows.Insert(0, a);

                            txtBarcode.Text = "";
                            txtBarcode.Focus();
                        }
                    }
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Item not found",true);
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                }
            }

            TotCount();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            MultiItemSelect();
        }

        void MultiItemSelect()
        {

            var fS = new frmSearchInventoryCount();
            fS.oSearchMode = "OITM";
            fS.allowMultiple = true;
            fS.ShowDialog();

            if (DECLARE._multipleSelection.Count > 0)
            {
                foreach (var x in DECLARE._multipleSelection)
                {
                    object[] a = { x.Code, x.Name, oWHS, oWHSName, "" };
                    dgvItems.Rows.Add(a);
                }

            }
        }



        private void frmInventoryCount_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        void CheckWhs()
        {
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[2].Value != null)
                {
                    //dt = DataAccess.Select(DataAccess.conStr("HANA"), query);
                    
                    var dt1 = hana.Get($"SELECT WhsCode FROM OWHS Where WhsCode ='{row.Cells[2].Value}'");
                    if (dt1.Rows.Count == 0)
                    {
                        //row.DefaultCellStyle.BackColor = Color.Red;
                        row.Cells[2].Style.BackColor = Color.Red;
                        dgvItems.Rows[row.Index].Cells[3].Value = "";
                    }
                    else
                    {
                        //row.DefaultCellStyle.BackColor = Color.White;

                        row.Cells[2].Style.BackColor = Color.White;
                       
                        var dt2 = hana.Get($"SELECT WhsName FROM OWHS Where WhsCode ='{row.Cells[2].Value}'");
                        dgvItems.Rows[row.Index].Cells[3].Value = dt2.Rows[0]["WhsName"];
                    }
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        void CheckItem()
        {
            //dt = DataAccess.Select(DataAccess.conStr("HANA"), query);

            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    //dt = DataAccess.Select(DataAccess.conStr("HANA"), $"SELECT ItemName FROM OITM Where ItemCode ='{row.Cells[0].Value}'");
                   
                    var dt1 = hana.Get($"SELECT ItemName FROM OITM Where ItemCode ='{row.Cells[0].Value}'");
                    if (dt1.Rows.Count == 0)
                    {
                        //row.DefaultCellStyle.BackColor = Color.Red;
                        row.Cells[0].Style.BackColor = Color.Red;
                        dgvItems.Rows[row.Index].Cells[1].Value = "";
                    }
                    else
                    {
                        //row.DefaultCellStyle.BackColor = Color.White;
                        //DataAccess.Select(DataAccess.conStr("HANA"), $"SELECT ItemName FROM OITM Where ItemCode ='{row.Cells[0].Value}'");
                        row.Cells[0].Style.BackColor = Color.White;
                        
                        var dt2 = hana.Get($"SELECT ItemName FROM OITM Where ItemCode ='{row.Cells[0].Value}'");
                        dgvItems.Rows[row.Index].Cells[1].Value = dt2.Rows[0]["ItemName"];
                    }
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        void ChangeCurrentCell(int rowIndex, int ColNum)
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    dgvItems.CurrentCell = dgvItems.Rows[rowIndex].Cells[ColNum];
                }));
            }
            catch (Exception ex) { }
        }
        private void dgvItems_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //change active column
            activeColumn = e.ColumnIndex;

            //check if item code or whs is missing
            if (e.ColumnIndex == 2)
            {
                if (dgvItems.Rows[e.RowIndex].Cells[0].Value == null)
                {
                    StaticHelper._MainForm.ShowMessage($"Specify Item code for Line {e.RowIndex + 1}", true);
                    ChangeCurrentCell(e.RowIndex, 0);
                }
            }
            else if (e.ColumnIndex == 4)
            {
                if (dgvItems.Rows[e.RowIndex].Cells[0].Value == null)
                {
                    StaticHelper._MainForm.ShowMessage($"Specify Item code for Line {e.RowIndex + 1}", true);
                    ChangeCurrentCell(e.RowIndex, 0);
                }
                else if (dgvItems.Rows[e.RowIndex].Cells[2].Value == null)
                {
                    StaticHelper._MainForm.ShowMessage($"Specify Warehouse code for Line {e.RowIndex + 1}", true);
                    ChangeCurrentCell(e.RowIndex, 2);
                }
            }
        }

        void DgvCellSearch(string Table)
        {
            var fS = new frmSearchInventoryCount();

            int col_index = dgvItems.CurrentCell.ColumnIndex;
            int row_index = dgvItems.CurrentCell.RowIndex;

            fS.oSearchMode = Table;
            fS.ShowDialog();

            dgvItems.Rows[row_index].Cells[col_index - 1].Value = fS.oCode;
            dgvItems.Rows[row_index].Cells[col_index + 1].Value = fS.oName;
        }

        void DeleteTemp()
        {
            try
            {
                msSql.Execute($"DELETE FROM tmpINC Where CompName ='{compname}'");
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {

            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                DeleteTemp();
                Dispose();
            }
        }

        private void pbFromWhsList_Click(object sender, EventArgs e)
        {
            var fS = new frmSearchInventoryCount();
            fS.oSearchMode = "OWHS";
            fS.ShowDialog();

            if (fS.oCode != null)
            {
                txtWhse.Text = fS.oCode;
                oWHS = fS.oCode;
                oWHSName = fS.oName;

                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    row.Cells["WhsCode"].Value = fS.oCode;
                    row.Cells["WhsName"].Value = fS.oName;
                }
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (txtRefNo.Text != "" && txtPrepared.Text != "" && txtNoted.Text != "" && txtRemarks.Text != "" && txtCheck.Text != "")
            {
                // ARJAYS BLOCKING STARTS HERE
                try
                {
                    Boolean val = false;
                    // Consolidating all the blockings and make error message starts here
                    foreach (DataGridViewRow dt in dgvItems.Rows)
                    {
                        if (dt.IsNewRow == false || dt.Cells["ItemCode"].Value != null)
                        {
                            if (dt.Cells["Quantity"].Value.ToString() != "")
                            {
                                if (Convert.ToDouble(dt.Cells["Quantity"].Value.ToString()) > 0)
                                { val = true; }
                                else { val = false; break; }
                            }else { val = false; break; }
                        }
                        else
                        { break; }

                    }
                    // Consolidating all the blockings and make error message ends here
                    
                    var dt1 = msSql.Get("SELECT RefNo FROM OINC WHERE RefNo = '" + txtRefNo.Text + "' AND Canceled = 'N'");
                    
                    if (helper.DataTableExist(dt1) && btnAdd.Text == "ADD")
                    {
                        StaticHelper._MainForm.ShowMessage("Reference Number Already Exists", true);
                    }
                    else
                    {
                        if (btnAdd.Text == "ADD")
                        {
                            if (val == true)
                            {
                                if (InsertTemp() == false)
                                {
                                    oMode = "A";
                                    Insert();
                                }
                                else { StaticHelper._MainForm.ShowMessage("Error in saving documents", true); }

                            }
                            else { StaticHelper._MainForm.ShowMessage("Dont Leave Empty Spaces", true); }

                        }

                        if (btnAdd.Text == "UPDATE")
                        {
                            if (val == true)
                            {
                                if (InsertTemp() == false)
                                { Insert(); }
                                else { StaticHelper._MainForm.ShowMessage("Error in saving documents", true); }

                            }
                            else { StaticHelper._MainForm.ShowMessage("Dont Leave Empty Spaces", true); }

                        }

                        TotCount();
                    }
                }
                catch (Exception ex)
                {
                    StaticHelper._MainForm.ShowMessage(ex.Message, true);
                    // ARJAYS BLOCKING ENDS HERE    
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Please fill-up mandatory fields", true);
            }

        }

        private bool InsertTemp()
        {
            //insert temp
            //@CompName nvarchar(50),
            //@DocDate datetime,
            //@ItemCode nvarchar(50),
            //@ItemName nvarchar(254),
            //@WhsCode nvarchar(50),
            //@Quantity numeric(18, 4)
            var isError = false;


            //string a = dtPostingDate.Text;
            //DateTime txtMyDate = Convert.ToDateTime(a);
            //a = txtMyDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            int linenum = 1;
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    //var q = $"[sp_InsertTemp] '{compname}','{a}','{row.Cells[0].Value}','{row.Cells[1].Value}','{row.Cells[2].Value}','{row.Cells[4].Value}'";
                    //var q = $"[sp_InsertTemp] '{compname}','{linenum}',CAST(GETDATE() as date),'{row.Cells[0].Value}','{row.Cells[1].Value}','{row.Cells[2].Value}','{row.Cells[4].Value}'";
                    var q = $"[sp_InsertTemp] '{compname}','{linenum}','','{row.Cells[0].Value}','{row.Cells[1].Value.ToString().Replace("'","")}','{row.Cells[2].Value}','{row.Cells[4].Value}'";
                    //using (var con = new SqlConnection(DataAccess.conStr("SAO")))
                    //{
                    //    SqlCommand cmd = new SqlCommand();
                    //    cmd = con.CreateCommand();
                    //    con.Open();
                    //    cmd.CommandText = q;
                    //    cmd.ExecuteNonQuery();
                    //    //_bool = true;
                    //}
                    msSql.Execute(q);
                    linenum = linenum + 1;


                    //if (DataAccessSQL.Execute(DataAccess.conStr("SQL"), q) != true)
                    //{
                    //    isError = true;
                    //}
                }
            }
            return isError;
        }

        private void Insert()
        {
            switch (oMode)
            {
                case "A":

                    string _sapuser = "";
                     _sapuser = EasySAPCredentialsModel.EmployeeName;
                    //var q = $"[sp_InsertCount] '{NextKey()}','','','O','{txtCounter.Text}','','{txtWhse.Text}','{DataAccess.replace(txtRemarks.Text)}','{compname}','{txtRefNo.Text}','{txtPrepared.Text}','{txtNoted.Text}','{_sapuser}','{oSettings.oLoginUser}','ADD','{txtCheck.Text.Trim()}'";
                    var q = $"[sp_InsertCount] '{NextKey()}','','','O','{txtCounter.Text}','','{txtWhse.Text}','{txtRemarks.Text.Replace("'","''")}','{compname}','{txtRefNo.Text}','{txtPrepared.Text}','{txtNoted.Text}','{_sapuser}','{SboCred.UserID}','ADD','{txtCheck.Text.Trim()}'";
                    try
                    {
                        msSql.Execute(q);
                        DeleteTemp();
                        ClearData();
                        //txtRefNo.Text = ""; txtPrepared.Text = ""; txtNoted.Text = ""; txtWhse.Text = ""; txtRemarks.Text = "";
                        StaticHelper._MainForm.ShowMessage("Operation completed successfully");
                    }
                    catch (Exception ex)
                    {
                        DeleteTemp();
                        StaticHelper._MainForm.ShowMessage(ex.Message, true);
                    }

                    break;

                case "U":
                    string _squery = "SELECT TOP 1 sapcode,RefNo FROM OINC WHERE DocEntry = '" + oDocEntry + "' AND Canceled = 'N' AND DocStatus IN ('O','R')";
                    var dt = msSql.Get(_squery);
                    
                    _squery = helper.ReadDataRow(dt, "sapcode", "", 0);
                    
                    _sapuser = EasySAPCredentialsModel.EmployeeName;

                    //dtuser = DataAccess.Select(DataAccess.conStr("HANA"), $"SELECT USER_CODE, U_NAME FROM OUSR Where USER_CODE = '" + _squery + "'", frmMain);
                    //_sapuser = DataAccess.Search(dtuser, 0, "U_NAME", frmMain);
                    var b = $"[sp_InsertCount] '{oDocEntry}','{DateTime.Now}','{DateTime.Now}','O','{txtCounter.Text}','{DateTime.Now}','{txtWhse.Text}','{DataAccess.replace(txtRemarks.Text)}','{compname}','{txtRefNo.Text}','{txtPrepared.Text}','{txtNoted.Text}','{_sapuser}','{_squery}','UPDATE','{txtCheck.Text.Trim()}'";

                    try
                    {
                        msSql.Execute(b);
                        DeleteTemp();
                        ClearData();
                        //txtRefNo.Text = ""; txtPrepared.Text = ""; txtNoted.Text = ""; txtWhse.Text = ""; txtRemarks.Text = "";
                        StaticHelper._MainForm.ShowMessage("Operation completed successfully");
                        Dispose();
                    }
                    catch (Exception ex)
                    {
                        DeleteTemp();
                        StaticHelper._MainForm.ShowMessage(ex.Message, true);
                    }

                    break;
            }



        }

        void ClearData()
        {
            txtSearch.Text = "";
            txtCheck.Text = "";
            txtRemarks.Text = "";
            txtNoted.Text = "";
            txtRefNo.Text = "";
            txtWhse.Text = "";
            txtBarcode.Text = "";
            txtPrepared.Clear();
            dgvItems.Rows.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cbDocList.DroppedDown = true;
        }

        private void frmInventoryCount_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (oMode == "A")
            {
                if (dgvItems.Rows.Count > 0)
                {
                    var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        DeleteTemp();
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to remove item?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var index = dgvItems.CurrentCell.RowIndex;
                    dgvItems.Rows.RemoveAt(index);
                }
                TotCount();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchRow(txtSearch.Text, defaultColumn);
        }

        void SearchRow(string value, int Column)
        {
            int _rowIndex;
            if (dgvItems.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row.Cells[0].Value != null) //check if row has data
                    {
                        if (row.Cells[Column].Value.ToString().ToUpper().Contains(value.ToUpper()))
                        {
                            row.Selected = true;
                            _rowIndex = row.Index;
                            dgvItems.FirstDisplayedScrollingRowIndex = _rowIndex;
                            break;
                        }
                        else
                        {
                            row.Selected = false;
                        }
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }
        }
        

        void CopyFrom()
        {
            var fS = new frmSearchInventoryCount();

            fS.oSearchMode = "OINC";
            fS.ShowDialog();


            if (fS.oCode != null)
            {
                var query = $"SELECT ItemCode,ItemDesc,WhsCode,CountQty FROM INC1 Where DocEntry ='{fS.oCode}'";
                //var dt2 = DataAccess.Select(DataAccess.conStr("HANA"), query);
                var dt = hana.Get(query);

                dgvItems.Columns.Clear();
                dgvItems.DataSource = dt;
            }
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            activeColumn = e.ColumnIndex;
        }
        
        private void txtRefNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                if (e.KeyChar == ')' && !txtRefNo.Text.Contains(")"))
                {
                    e.Handled = false; //Do not reject the input
                }
                else if (e.KeyChar == '(' && !txtRefNo.Text.Contains("("))
                {
                    e.Handled = false; //Do not reject the input
                }
                else if (e.KeyChar == '-' && !txtRefNo.Text.Contains("-"))
                {
                    e.Handled = false; //Do not reject the input
                }
                else if (e.KeyChar == ' ' && !txtRefNo.Text.Contains(" "))
                {
                    e.Handled = false; //Do not reject the input
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void dgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.ColumnIndex == 0)
                {
                    if (dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        var dt1 = hana.Get($"SELECT ItemName FROM OITM Where ItemCode ='{dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}'");
                        if (dt1.Rows.Count == 0)
                        {
                            StaticHelper._MainForm.ShowMessage("Item code not found", true);
                            ChangeCurrentCell(e.RowIndex, 0);
                        }
                        else
                        {
                            var dt2 = hana.Get($"SELECT ItemName,ItemCode FROM OITM Where ItemCode ='{dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}'");
                            dgvItems.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.White;
                            dgvItems.Rows[e.RowIndex].Cells[1].Value = dt2.Rows[0]["ItemName"];
                            dgvItems.Rows[e.RowIndex].Cells[0].Value = dt2.Rows[0]["ItemCode"];
                        }
                    }
                    else
                    {
                        dgvItems.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.White;
                        dgvItems.Rows[e.RowIndex].Cells[1].Value = "";
                    }
                }
                else if (e.ColumnIndex == 2)
                {
                    if (dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        var dt1 = hana.Get($"SELECT WhsCode FROM OWHS Where WhsCode ='{dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}'");
                        if (dt1.Rows.Count == 0)
                        {
                            StaticHelper._MainForm.ShowMessage("Warehouse code not found", true);
                            ChangeCurrentCell(e.RowIndex, 2);
                        }
                        else
                        {
                            var dt2 = hana.Get($"SELECT WhsName FROM OWHS Where WhsCode ='{dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}'");
                            dgvItems.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.White;
                            dgvItems.Rows[e.RowIndex].Cells[3].Value = dt2.Rows[0]["WhsName"];
                        }
                    }
                    else
                    {
                        dgvItems.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.White;
                        dgvItems.Rows[e.RowIndex].Cells[3].Value = "";
                    }
                }

                TotCount();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
            
        }

        private void frmInventoryCount_Load(object sender, EventArgs e)
        {
            switch (oMode)
            {
                case "A":
                    btnAdd.Text = "ADD";
                    txtRefNo.ReadOnly = false;
                    //txtTime.Text = localDate.ToString("hh:mmtt", CultureInfo.InvariantCulture).ToString();
                    break;
                case "U":
                    var oCheck = $"SELECT TOP 1 U_User,U_UpdateCount FROM OHEM WHERE empID = '{EasySAPCredentialsModel.GetEmployeeCode()}' AND U_UpdateCount = 'Y'";
                    var dt1 = hana.Get(oCheck);
                    
                    if (helper.DataTableExist(dt1))
                    { btnAdd.Enabled = true; }
                    else
                    {
                        oCheck = $"SELECT DocStatus,DocEntry FROM OINC WHERE DocEntry = '{ oDocEntry }' AND DocStatus = 'R'";
                        dt1 = msSql.Get(oCheck);

                        if (helper.DataTableExist(dt1))
                        { btnAdd.Enabled = false; }
                    }

                    btnAdd.Text = "UPDATE";
                    txtRefNo.ReadOnly = true;
                    string sdata;
                    sdata = "SELECT *,CAST(Time as nvarchar(10)),CAST(DocDate as date) as docdate2 FROM OINC WHERE DocEntry = '" + oDocEntry + "' AND Canceled = 'N'  AND DocStatus IN ('O','R')";

                    var dt = msSql.Get(sdata);
                    
                    if (helper.DataTableExist(dt))
                    {
                        
                        txtRefNo.Text = helper.ReadDataRow(dt, "RefNo", "", 0);
                        txtPrepared.Text = helper.ReadDataRow(dt, "PreparedBy", "", 0);
                        txtNoted.Text = helper.ReadDataRow(dt, "NotedBy", "", 0);
                        txtRemarks.Text = helper.ReadDataRow(dt, "Remarks", "", 0);
                        txtWhse.Text = helper.ReadDataRow(dt, "WhsCode", "", 0);
                        txtCheck.Text = helper.ReadDataRow(dt, "CheckedBy", "", 0).Trim();

                        DateTime date;
                        
                        var dateformat = helper.ReadDataRow(dt, "countdate", "", 0);
                        
                        sdata = " SELECT ItemCode,ItemName,WhsCode,Quantity FROM INC1 WHERE HeaderId = '" + oDocEntry + "' ";
                        //sdata = sdata + "  ORDER BY linenum ";

                        dt = msSql.Get(sdata);
                        string _oWhsName;

                        foreach (DataRow row in dt.Rows)
                        {
                            _oWhsName = "select WhsCode,WhsName from OWHS WHERE WhsCode = '" + row["WhsCode"].ToString() + "'";
                            dt1 = hana.Get(_oWhsName);
                            _oWhsName = helper.ReadDataRow(dt1,"WhsName","",0);
                            object[] a = { row["ItemCode"].ToString(), row["ItemName"].ToString(), row["WhsCode"].ToString(), _oWhsName, Convert.ToDouble(row["Quantity"].ToString()) };
                            dgvItems.Rows.Add(a);
                        }
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Docnument has been updated please refresh and reopen again", true);
                        //this.Close();
                    }

                    break;
            }

            MaximumSize = new Size(max_width, max_height);
            PreloadData();
            TotCount();

        }

        private void TotCount()
        {
            try
            {
                int n;
                //bool isNumeric = int.TryParse("123", out n);
                double _TotalQty = 0;
                foreach (DataGridViewRow dt in dgvItems.Rows)
                {

                    try
                    {

                        if (dt.Cells[4].Value != null)
                        {
                            //dt.IsNewRow == false ||
                       
                            if (dt.Cells["ItemCode"].Value != null && string.IsNullOrEmpty(dt.Cells["Quantity"].Value.ToString()) == false)
                            {
                                if (Convert.ToDouble(dt.Cells["Quantity"].Value.ToString()) > 0)
                                {
                                    _TotalQty = _TotalQty + Convert.ToDouble(dt.Cells[4].Value.ToString());
                                    //if ((Regex.IsMatch(dt.Cells[4].Value.ToString(), @"^\d+$")) == true)
                                    //{ _TotalQty = _TotalQty + Convert.ToDouble(dt.Cells[4].Value.ToString()); }
                                }
                                else { break; }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //dt.IsNewRow == false ||
                        if (dt.Cells["ItemCode"].Value != null || dt.Cells["Quantity"].Value.ToString() != "")
                        {
                            if (Convert.ToDouble(dt.Cells["Quantity"].Value.ToString()) > 0)
                            {
                                _TotalQty = _TotalQty + Convert.ToDouble(dt.Cells[4].Value.ToString());
                                //if ((Regex.IsMatch(dt.Cells[4].Value.ToString(), @"^\d+$")) == true)
                                //{ _TotalQty = _TotalQty + Convert.ToDouble(dt.Cells[4].Value.ToString()); }
                            }
                            else { break; }
                        }
                        else
                        { break; }
                    }
                }

                lblTotalCount.Text = _TotalQty.ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        void PreloadData()
        {
            
            dtItems = hana.Get("SELECT ItemCode,ItemName From OITM");
            dtWhs = hana.Get("SELECT WhsCode,WhsName From OWHS");
        }

        public frmInventoryCount()
        {
            InitializeComponent();
            msSql = new SAPMsSqlAccess();
            helper = new DataHelper();
            hana = new SAPHanaAccess();
            //Create table for datagridview
            CreateTable();
        }

        private void Search(string CellName)
        {
            int col_index = dgvItems.CurrentCell.ColumnIndex;
            int row_index = dgvItems.CurrentCell.RowIndex;


            if (CellName == "ItemCode")
            {
                ViewList("OITM", "Lists of Items", out Code, out Name);
                if (Code != null)
                {
                    dgvItems.Rows[row_index].Cells[col_index + 1].Value = Name;
                }

            }
            else if (CellName == "Whs")
            {
                ViewList("OWHS", "Lists of Warehouses", out Code, out Name);
                if (Code != null)
                {
                    dgvItems.Rows[row_index].Cells[col_index + 1].Value = Name;
                }
            }
            else
            {
                ViewList("?", "", out Code, out Name);
            }

            //display value
            if (Code != null)
            {
                dgvItems.Rows[row_index].Cells[col_index].Value = Code;
            }

            //dgvItems.CurrentCell = dgvItems.Rows[row_index].Cells[col_index + 1];


            ChangeCurrentCell(row_index, col_index + 1);
        }

        private void ViewList(string SearchTable
                         , string Title
                         , out string Code
                         , out string Name
                         , [Optional] string Param1
                         , [Optional] string Param2
                         , [Optional] string Param3
                         , [Optional] string Param4
                         )
        {
            frmSearchInventoryCount fS = new frmSearchInventoryCount();
            frmSearchInventoryCount._title = Title;
            fS.oSearchMode = SearchTable;
            //Set Parameter 1
            frmSearchInventoryCount.Param1 = Param1;
            //Set Parameter 2
            frmSearchInventoryCount.Param2 = Param2;
            //Set Parameter 3
            frmSearchInventoryCount.Param3 = Param3;
            //Set Parameter 4
            frmSearchInventoryCount.Param3 = Param4;
            fS.ShowDialog();

            Code = fS.oCode;
            Name = fS.oName;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //datagridview
            if (keyData == Keys.Tab && dgvItems.IsCurrentCellInEditMode == true)
            {

                if (activeColumn == 0) //ItemCode
                {
                    Search("ItemCode");
                }
                else if (activeColumn == 2) //Warehouse
                {
                    Search("Whs");
                }
                return true;
            }
            else if (keyData == (Keys.Control | Keys.F))
            {
                MultiItemSelect();
                return true;
            }
            else if (keyData == Keys.Tab && txtWhse.Focused == true)
            {
                var fS = new frmSearchInventoryCount();
                fS.oSearchMode = "OWHS";
                fS.ShowDialog();

                if (fS.oCode != null)
                {
                    txtWhse.Text = fS.oCode;
                    oWHS = fS.oCode;
                    oWHSName = fS.oName;

                    foreach (DataGridViewRow row in dgvItems.Rows)
                    {
                        row.Cells["WhsCode"].Value = fS.oCode;
                        row.Cells["WhsName"].Value = fS.oName;
                    }
                }
                return true;
            }
            else if (keyData == Keys.Escape)
            { Close(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CreateTable()
        {
            var col1 = new DataGridViewTextBoxColumn();
            var btn1 = new DataGridViewButtonColumn();
            var btn2 = new DataGridViewButtonColumn();
            var col2 = new DataGridViewTextBoxColumn();
            var col3 = new DataGridViewTextBoxColumn();
            var col4 = new DataGridViewTextBoxColumn();
            var col5 = new DataGridViewTextBoxColumn();

            col1.Name = "ItemCode";
            col1.HeaderText = "Item No.";
            col1.Frozen = true;

            col2.Name = "ItemName";
            col2.HeaderText = "Item Description";
            col2.Width = 200;
            col2.ReadOnly = true;
            //col2.DefaultCellStyle.BackColor = Color.LightGray;

            col3.Name = "WhsCode";
            col3.HeaderText = "Whse Code";
            col3.ReadOnly = true;
            //col3.DefaultCellStyle.BackColor = Color.LightGray;

            col4.Name = "WhsName";
            col4.HeaderText = "Whse Name";
            col4.Width = 150;
            col4.ReadOnly = true;
            //col4.DefaultCellStyle.BackColor = Color.LightGray;

            col5.Name = "Quantity";
            col5.HeaderText = "Counted Qty";

            //item
            btn1.UseColumnTextForButtonValue = true;
            btn1.Text = "...";
            btn1.Name = "btn1";
            btn1.HeaderText = "";
            btn1.Width = 20;
            //warehouse
            btn2.UseColumnTextForButtonValue = true;
            btn2.Text = "...";
            btn2.Name = "btn2";
            btn2.HeaderText = "";
            btn2.Width = 20;

            dgvItems.Columns.Add(col1);
            //dgvItems.Columns.Add(btn1);
            dgvItems.Columns.Add(col2);
            dgvItems.Columns.Add(col3);
            //dgvItems.Columns.Add(btn2);
            dgvItems.Columns.Add(col4);
            dgvItems.Columns.Add(col5);

            dataGridLayout(dgvItems);

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
        }


    }
}

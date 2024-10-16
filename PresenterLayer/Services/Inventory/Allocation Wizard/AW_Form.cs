using Context;
using DirecLayer;
using DomainLayer.Models.Inventory.Allocation_Wizard;
using MetroFramework;
using PresenterLayer.Helper;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PresenterLayer
{
    public class AW_Form
    {
        // ########## Static Tab ##########
        public static TabPage tabWelcome { get; set; }
        public static TabPage tabScenario { get; set; }
        public static TabPage tabAllocWizRuns { get; set; }
        public static TabPage tabItemSelection { get; set; }
        public static TabPage tabAllocationBase { get; set; }
        public static TabPage tabStoreSelection { get; set; }
        public static TabPage tabRanking { get; set; }
        public static TabPage tabSummary { get; set; }


        public frmAllocationWizard frmAW { get; set; }
        public int max_width { get; set; } = Screen.PrimaryScreen.Bounds.Width;
        public int max_height { get; set; } = Screen.PrimaryScreen.Bounds.Height - 200;
        string iColumn { get; set; } = "Run ID";
        SAPHanaAccess sapHana { get; set; }
        DataHelper helper { get; set; }
        public AW_Form()
        {
            helper = new DataHelper();
            sapHana = new SAPHanaAccess();
        }
        public void Form_Load()
        {
            LoadStaticTabs();
            RemoveAllTabs();
            SetOneMonthDate();
        }

        void LoadStaticTabs()
        {
            tabWelcome = frmAW.MetroTabControl.TabPages[0];
            tabScenario = frmAW.MetroTabControl.TabPages[1];
            tabAllocWizRuns = frmAW.MetroTabControl.TabPages[2];
            tabItemSelection = frmAW.MetroTabControl.TabPages[3];
            tabAllocationBase = frmAW.MetroTabControl.TabPages[4];
            tabStoreSelection = frmAW.MetroTabControl.TabPages[5];
            tabRanking = frmAW.MetroTabControl.TabPages[6];
            tabSummary = frmAW.MetroTabControl.TabPages[7];
        }

        void RemoveAllTabs()
        {
            foreach (TabPage tab in frmAW.MetroTabControl.TabPages)
            { frmAW.MetroTabControl.TabPages.Remove(tab); }

            //frmAW.tabWIzardRuns.TabPages.Remove(frmAW.tabWIzardRuns.TabPages[1]);

            frmAW.MetroTabControl.TabPages.Add(tabWelcome);
        }

        public void SetOneMonthDate()
        {
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);
            var last = month.AddDays(-1);

            frmAW.dtpApprovedFrom.Value = first;
            frmAW.dtpApprovedTo.Value = last;
        }
        
        public void Form_Close(FormClosingEventArgs e)
        {
            if (!frmAW.btnFinish.Enabled)
            {
                if (MetroMessageBox.Show(StaticHelper._MainForm, $"Unsaved data will be lost. Continue?", LibraryHelper.AssemblyInfo.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            PublicStatic.sbCardCode.Clear();
            SelectionModel.Selection.Clear();
            StaticHelper._MainForm.ProgressClear();

            foreach (Form frm in StaticHelper._MainForm.MdiChildren)
            {
                if (frm.Name.Contains("frmAW_"))
                {
                    frm.Dispose();
                    frm.Close();
                }
            }

            frmAW.Dispose();

        }

        public void EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            e.Control.KeyPress -= new KeyPressEventHandler(AllowNumericFormatOnly);
            TextBox tb = e.Control as TextBox;
            if (tb != null)
            { tb.KeyPress += new KeyPressEventHandler(AllowNumericFormatOnly); }
        }

        void AllowNumericFormatOnly(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            //{ e.Handled = true; }

            //if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            //{ e.Handled = true; }

            //if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            //{ e.Handled = true; }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            { e.Handled = true; }


            //if (dgv.Name == "dgvItemSelected")
            //{
            //    var val = e.KeyChar.ToString();
            //}
        }

        public void dgvCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (e.RowIndex >= 0)
            {
                int col_index = e.ColumnIndex;
                int row_index = dgv.CurrentCell.RowIndex;

                if (dgv.Columns[col_index].Name == "")
                {
                    var frm = new frmAW_Filter();
                    frm.Text = dgv.Rows[row_index].Cells["Name"].Value.ToString();
                    frm.TableID = dgv.Rows[row_index].Cells["ID"].Value.ToString();
                    frm.Type = dgv.Tag.ToString();
                    //AW_Filter_Form.dt = SAPHana.Get(dgv.Rows[row_index].Cells["Query"].Value.ToString());
                    frm.MdiParent = StaticHelper._MainForm;
                    frm.Show();
                }
            }
        }

        public void dgvButtonCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;
            int col_index = e.ColumnIndex;
            int row_index = e.RowIndex;
            if (row_index >= 0 && dgv.Columns[col_index].Name.Equals("btn"))
            {
                frmAW_Filter frm = new frmAW_Filter();
                frm.Text = dgv.Rows[row_index].Cells["Name"].Value.ToString();
                frm.TableID = dgv.Rows[row_index].Cells["Code"].Value.ToString();
                frm.Type = dgv.Tag.ToString();
                frm.sQuery = dgv.Rows[row_index].Cells["Query"].Value.ToString();
                frm.MdiParent = StaticHelper._MainForm;
                frm.Show();
                frm.LoadData();
            }
        }

        public void dgvItemOtherParam_CellContentClick(DataGridViewCellEventArgs e)
        {
            var dgv = frmAW.dgvItemOtherParam;
            int col_index = e.ColumnIndex;
            int row_index = e.RowIndex;
            if (row_index >= 0 && dgv.Columns[col_index].Name.Equals("btn"))
            {
                DataGridViewRow dgvdr = dgv.Rows[row_index];
                if (row_index > 5)
                {
                    frmAW_Filter frm = new frmAW_Filter();
                    frm.Text = dgv.Rows[row_index].Cells["Name"].Value.ToString();
                    frm.TableID = dgv.Rows[row_index].Cells[0].Value.ToString();
                    frm.Type = "IOP";
                    frm.sQuery = dgv.Rows[row_index].Cells["Query"].Value.ToString();
                    frm.MdiParent = StaticHelper._MainForm;
                    frm.dgvdr = dgvdr;
                    frm.Show();
                }
                else
                {
                    // 1. Brand
                    string sBrand = dgv.Rows[0].Cells["Value"].Value.ToString();
                    // 2. Department
                    string sDept = dgv.Rows[1].Cells["Value"].Value.ToString();
                    // 3. Sub-Department
                    string sSubDept = dgv.Rows[2].Cells["Value"].Value.ToString();
                    // 4. Category
                    string sCat = dgv.Rows[3].Cells["Value"].Value.ToString();
                    // 5. Sub-Category
                    string sSubCat = dgv.Rows[4].Cells["Value"].Value.ToString();

                    string sSubCode = $"{sBrand}{sCat}{sSubCat}";

                    if (dgv.Rows[row_index].Cells["Type"].Value.ToString() == "Many")
                    {
                        if (string.IsNullOrEmpty(sBrand) || string.IsNullOrEmpty(sCat) || string.IsNullOrEmpty(sSubCat))
                        {
                            StaticHelper._MainForm.ShowMessage("Please choose brand/category/sub-category first!", true);
                            return;
                        }
                        var frm = new frmAW_Filter();
                        frm.Text = dgv.Rows[row_index].Cells["Name"].Value.ToString();
                        frm.TableID = dgv.Rows[row_index].Cells[0].Value.ToString();
                        frm.Type = "IOP";
                        frm.sQuery = string.Format(dgv.Rows[row_index].Cells["Query"].Value.ToString(), sSubCode);
                        frm.dgvdr = dgvdr;
                        frm.MdiParent = StaticHelper._MainForm;
                        frm.Show();
                        frm.LoadData();
                    }
                    else
                    {
                        frmAW_Find frm = new frmAW_Find();
                        frm.form = frmAW;
                        frm.dgvdr = dgvdr;
                        frm.Type = "IOP";
                        frm.TableID = dgv.Rows[row_index].Cells[0].Value.ToString();
                        switch (row_index)
                        {
                            // 1. Brand
                            case 0:
                                frm.sQuery = dgv.Rows[row_index].Cells["Query"].Value.ToString();
                                break;
                            // 2. Department
                            case 1:
                                if (string.IsNullOrEmpty(sBrand))
                                {
                                    StaticHelper._MainForm.ShowMessage("Please choose brand first!", true);
                                    return;
                                }
                                frm.sQuery = string.Format(dgv.Rows[row_index].Cells["Query"].Value.ToString(), sBrand);
                                break;
                            // 3. Sub-Department
                            case 2:
                                if (string.IsNullOrEmpty(sDept))
                                {
                                    StaticHelper._MainForm.ShowMessage("Please choose department first!",true);
                                    return;
                                }
                                frm.sQuery = string.Format(dgv.Rows[row_index].Cells["Query"].Value.ToString(), sBrand, sDept);
                                break;
                            // 4. Category
                            case 3:
                                if (string.IsNullOrEmpty(sSubDept))
                                {
                                    StaticHelper._MainForm.ShowMessage("Please choose sub-department first!", true);
                                    return;
                                }
                                frm.sQuery = string.Format(dgv.Rows[row_index].Cells["Query"].Value.ToString(), sBrand, sDept, sSubDept);
                                break;
                            // 4. Sub-Category
                            case 4:
                                if (string.IsNullOrEmpty(sCat))
                                {
                                    StaticHelper._MainForm.ShowMessage("Please choose category first!", true);
                                    return;
                                }
                                frm.sQuery = string.Format(dgv.Rows[row_index].Cells["Query"].Value.ToString(), sBrand, sCat);
                                break;
                            default:
                                break;
                        }
                        frm.Text = dgv.Rows[row_index].Cells["Name"].Value.ToString();
                        frm.MdiParent = StaticHelper._MainForm;
                        frm.Show();
                    }
                }
            }
        }

        public void dgvCustOtherParam_CellContentClick(DataGridViewCellEventArgs e)
        {
            var dgv = frmAW.dgvCustOtherParam;
            int col_index = e.ColumnIndex;
            int row_index = e.RowIndex;
            if (row_index >= 0 && dgv.Columns[col_index].Name.Equals("btn"))
            {
                DataGridViewRow row = dgv.Rows[row_index];

                var frm = new frmAW_Filter();
                frm.Text = dgv.Rows[row_index].Cells["Name"].Value.ToString();
                frm.TableID = dgv.Rows[row_index].Cells[0].Value.ToString();
                frm.Type = "COP";
                frm.sQuery = dgv.Rows[row_index].Cells["Query"].Value.ToString();
                frm.MdiParent = StaticHelper._MainForm;
                frm.dgvdr = row;
                frm.Show();
            }
        }

        public void dgvLevels(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;
            dgv.EndEdit();
            if (e.RowIndex >= 0)
            {
                var val = dgv.Rows[e.RowIndex].Cells["Field"].Value;
                if (val != null)
                {
                    if (!string.IsNullOrEmpty(val.ToString()))
                    {
                        int checker = 0;

                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            var dr = dgv.Rows[i];
                            if (val.Equals(dr.Cells[0].Value.ToString()))
                            { checker++; }

                            if (checker == 2)
                            {
                                StaticHelper._MainForm.ShowMessage("Please choose other level", true);
                                dgv.Rows[e.RowIndex].Cells["Field"].Value = "";
                            }
                        }
                        
                        var dt = new DataTable();
                        

                        dt = sapHana.Get(string.Format(helper.ReadDataRow(sapHana.Get(SP.AW_LVL2), 1, "", 0), val));

                        dgv.Rows[e.RowIndex].Cells["Code"].Value = val;
                        dgv.Rows[e.RowIndex].Cells["Query"].Value = dt.Rows[0][0].ToString();

                        dt = sapHana.Get(SP.AW_LVL);

                        if (dt.Rows.Count - 1 != dgv.Rows.Count)
                        {
                            string[] row = new string[] { "", "" };
                            dgv.Rows.Add(row);

                            var cmb = new DataGridViewComboBoxCell();

                            cmb.DisplayMember = "Name";
                            cmb.ValueMember = "Code";
                            cmb.DataSource = dt;
                            dgv.Rows[dgv.Rows.Count - 1].Cells["Field"] = cmb;
                        }
                    }
                    else
                    { dgv.Rows[e.RowIndex].Cells["Code"].Value = ""; }
                }
                else if (dgv.Rows.Count > 1)
                { dgv.Rows.RemoveAt(e.RowIndex); }
            }
        }

        public void dgvSalesCritera(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var dgv = (DataGridView)sender;
                var sBrand = dgv.Rows[0].Cells[e.ColumnIndex].Value.ToString();
                var cmb = (DataGridViewComboBoxCell)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var sQuery = dgv.Rows[e.RowIndex].Cells["Query"].Value.ToString();
                if (e.RowIndex == 1)
                { cmb.DataSource = sapHana.Get(string.Format(sQuery, sBrand)); }
                else if (e.RowIndex == 2)
                {
                    var sDept = dgv.Rows[1].Cells["Field"].Value;
                    if (sDept != null)
                    { cmb.DataSource = sapHana.Get(string.Format(sQuery, sBrand, sDept.ToString())); }
                }
                else if (e.RowIndex == 3)
                {
                    var sDept = dgv.Rows[1].Cells["Field"].Value;
                    if (sDept != null)
                    {
                        var sSubDept = dgv.Rows[2].Cells["Field"].Value;
                        if (sSubDept != null)
                        { cmb.DataSource = sapHana.Get(string.Format(sQuery, sBrand, sDept.ToString(), sSubDept.ToString())); }
                    }
                }
                else if (e.RowIndex == 4)
                {
                    var sCategory = dgv.Rows[3].Cells["Field"].Value;
                    if (sCategory != null)
                    { cmb.DataSource = sapHana.Get(string.Format(sQuery, sBrand, sCategory.ToString())); }
                }
            }
        }

        public void rowChangedValue(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;

            if (dgv.Rows.Count > 4 && dgv.Name == "dgvSalesCritera")
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    var dr = dgv.Rows[i];
                    if (dr.Index > e.RowIndex)
                    {
                        var cmb = new DataGridViewComboBoxCell();
                        cmb.DisplayMember = "Name";
                        cmb.ValueMember = "Code";
                        cmb.DataSource = sapHana.Get("SELECT '' [Code], '' [Name]");
                        dgv.Rows[dr.Index].Cells["Field"] = cmb;
                        dgv.Rows[dr.Index].Cells["Field"].Value = "";
                    }
                }
            }
            else if (dgv.Name == "dgvItemSelected" && e.ColumnIndex == 14)
            {
                var curRow = dgv.Rows[e.RowIndex].Cells[14];

                int row = curRow.RowIndex;

                var currVal = curRow.Value != null ? double.Parse(curRow.Value.ToString()) : 0;

                if (currVal > 100 || currVal < 0)
                { curRow.Value = 0; }
                currVal = int.Parse(curRow.Value.ToString());

                var getVal = dgv.Rows[row].Cells["Qty"].Value != null ? double.Parse(dgv.Rows[row].Cells["Qty"].Value.ToString()) : 0;

                if (currVal == 0)
                { dgv.Rows[row].Cells["Allocate"].Value = 0; return; }
                else
                {
                    double sNumber = double.Parse((getVal == 0 ? 0 : getVal * (currVal / 100)).ToString());
                    string ret = string.Format("{0:#,0}", sNumber);
                    dgv.Rows[row].Cells["Allocate"].Value = ret;
                }
                if (row == 0 && dgv.Rows.Count > 2)
                {
                    if (MetroMessageBox.Show(StaticHelper._MainForm, $"Do you want to apply it to all rows?", LibraryHelper.AssemblyInfo.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        var max = dgv.Rows.Count;
                        for (int i = 0; i < max; i++)
                        {
                            StaticHelper._MainForm.Progress($"Please wait until all data are loaded. {i + 1} out of {max}", i + 1, max);
                            var dr = dgv.Rows[i];
                            if (dr.Index != 0)
                            {
                                dr.Cells["%"].Value = currVal;
                            }
                        }
                    }
                }

            }
        }

        public void dgvSearch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var dgv = (DataGridView)sender;

            if (dgv.Rows.Count > 0)
            { iColumn = dgv.Columns[e.ColumnIndex].Name; }

        }

        public void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var txt = (TextBox)sender;

            var dgv = txt.Name.Equals("txtSearchApprovalRuns") ? frmAW.dgvAllocWizRuns : frmAW.dgvApprovedRuns;

            if (dgv.Columns.Count > 1)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    var row = dgv.Rows[i];
                    if (string.IsNullOrEmpty(iColumn))
                    { iColumn = "Code"; }

                    if (row.Cells[iColumn].Value.ToString().ToUpper().Contains(txt.Text.ToUpper()))
                    {
                        row.Selected = true;
                        dgv.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                    else { row.Selected = false; }
                }
            }
        }

        public void AutoEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dgv = (DataGridView)sender;
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dgv.BeginEdit(true);

            }
            catch { }
        }

        public void RowsAdd()
        {
            try
            {
                for (int i = 0; i < frmAW.dgvCustSelected.Rows.Count; i++)
                {
                    var dr = frmAW.dgvCustSelected.Rows[i];

                    var val = dr.Cells["Classification"].Value;
                    if (val == null)
                    { dr.DefaultCellStyle.BackColor = Color.Red; }
                    else if (string.IsNullOrEmpty(val.ToString()))
                    { dr.DefaultCellStyle.BackColor = Color.Red; }
                    else
                    { dr.DefaultCellStyle.BackColor = Color.White; }
                }
            }
            catch (Exception ex) { }
        }

        public void dgvColorFormat(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;

            if ((e.ColumnIndex == 20 && frmAW.rbRepeatOrder.Checked) || (e.ColumnIndex == 18 && frmAW.rbCreateNewAlloc.Checked))
            {

                for (int i = 0; i < frmAW.dgvItemSelected.Rows.Count; i++)
                {
                    var item = frmAW.dgvItemSelected.Rows[i];
                    var sItemCode = item.Cells[0].Value.ToString();
                    var iTotal = double.Parse(item.Cells["Allocate"].Value == null ? "0" : string.IsNullOrEmpty(item.Cells["Allocate"].Value.ToString()) ? "0" : item.Cells["Allocate"].Value.ToString());
                    double iSum = 0;

                    for (int iRow = 0; iRow < dgv.Rows.Count; iRow++)
                    {
                        var dr = dgv.Rows[iRow];
                        if (dr.Cells["ItemCode"].Value.ToString() == sItemCode)
                        {
                            iSum += double.Parse((dr.Cells["Allocated"].Value == null ? "0" : dr.Cells["Allocated"].Value.ToString()));
                            dr.Cells["Allocation"].Value = iTotal - iSum;
                        }
                    }

                    for (int iSummary = 0; iSummary < dgv.Rows.Count; iSummary++)
                    {
                        var dr = dgv.Rows[iSummary];
                        if (dr.Cells["ItemCode"].Value.ToString() == sItemCode)
                        {
                            if (iTotal < iSum)
                            { dr.DefaultCellStyle.BackColor = Color.Red; }
                            else if (iTotal == iSum)
                            { dr.DefaultCellStyle.BackColor = Color.LightBlue; }
                            else { dr.DefaultCellStyle.BackColor = Color.White; }
                        }
                    }
                }
            }
            else if (e.ColumnIndex == 7)
            {
                if (frmAW.rbAllocationApproval.Checked)
                {
                    for (int i = 0; i < frmAW.dgvAllocation.Rows.Count; i++)
                    {
                        var dr = frmAW.dgvAllocation.Rows[i];
                        var sApproved = dr.Cells["Allocated"].Value;
                        if (int.Parse(dr.Cells["Quantity"].Value.ToString()) < int.Parse(sApproved == null ? "0" : sApproved.ToString()))
                        { dr.DefaultCellStyle.BackColor = Color.Red; }
                        else { dr.DefaultCellStyle.BackColor = Color.White; }
                    }
                    Application.DoEvents();
                }

            }
        }

        public void AverageCheckedChange(bool bVal)
        {
            frmAW.cbAverage.Enabled = bVal;
            frmAW.cbAverage.SelectedIndex = 0;
        }

        void navEnable(bool val)
        {
            frmAW.navItemGet.Enabled = val;
            frmAW.navItemGetAll.Enabled = val;
            frmAW.navItemBackAll.Enabled = val;
            frmAW.navItemBack.Enabled = val;

            frmAW.navCustBack.Enabled = val;
            frmAW.navCustBackAll.Enabled = val;
            frmAW.navCustGet.Enabled = val;
            frmAW.navCustGetAll.Enabled = val;

            Application.DoEvents();
        }

        public void navClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                navEnable(false);
                switch (btn.Name)
                {
                    case "navItemGet":
                        GetSelectedItems(frmAW.dgvItemSelection, frmAW.dgvItemSelected);
                        break;
                    case "navItemGetAll":
                        GetAllSelectedItems(frmAW.dgvItemSelection, frmAW.dgvItemSelected);
                        break;
                    case "navItemBackAll":
                        GetAllSelectedItems(frmAW.dgvItemSelected, frmAW.dgvItemSelection);
                        break;
                    case "navItemBack":
                        GetSelectedItems(frmAW.dgvItemSelected, frmAW.dgvItemSelection);
                        break;

                    case "navCustGet":
                        GetSelectedItems(frmAW.dgvCustSelection, frmAW.dgvCustSelected);
                        break;
                    case "navCustGetAll":
                        GetAllSelectedItems(frmAW.dgvCustSelection, frmAW.dgvCustSelected);
                        break;
                    case "navCustBackAll":
                        GetAllSelectedItems(frmAW.dgvCustSelected, frmAW.dgvCustSelection);
                        break;
                    case "navCustBack":
                        GetSelectedItems(frmAW.dgvCustSelected, frmAW.dgvCustSelection);
                        break;
                }
                navEnable(true);
            }
            catch (Exception ex)
            {
            }

        }

        void GetAllSelectedItems(DataGridView dgvget, DataGridView dgvpost)
        {

            var max = dgvget.Rows.Count;
            StaticHelper._MainForm.ProgressClear();

            if (dgvget.Rows.Count <= 0)
            {
                return;
            }

            StaticHelper._MainForm.Progress("Please wait...", 1, max);

            for (int i = 0; i < dgvget.Rows.Count; i++)
            {
                var dr = dgvget.Rows[i];
                if (dgvpost.Name.Contains("dgvCustSelect"))
                {
                    dgvpost.Rows.Add(dr.Cells[0].Value,
                                     dr.Cells[1].Value,
                                     dr.Cells[2].Value);
                    //row.Cells[2].Value = dr.Cells[2].Value;
                }
                else if (dgvpost.Name.Equals("dgvItemSelection"))
                {
                    dgvpost.Rows.Add(dr.Cells[0].Value,
                                    dr.Cells[1].Value,
                                    dr.Cells[2].Value,
                                    dr.Cells[3].Value,
                                    dr.Cells[4].Value,
                                    dr.Cells[5].Value,
                                    dr.Cells[6].Value,
                                    dr.Cells[7].Value,
                                    dr.Cells[8].Value,
                                    dr.Cells[9].Value,
                                    dr.Cells[10].Value,
                                    dr.Cells[11].Value,
                                    dr.Cells[12].Value,
                                    dr.Cells[13].Value);
                }
                else if (dgvpost.Name.Equals("dgvItemSelected"))
                {
                    dgvpost.Rows.Add(dr.Cells[0].Value,
                                    dr.Cells[1].Value,
                                    dr.Cells[2].Value,
                                    dr.Cells[3].Value,
                                    dr.Cells[4].Value,
                                    dr.Cells[5].Value,
                                    dr.Cells[6].Value,
                                    dr.Cells[7].Value,
                                    dr.Cells[8].Value,
                                    dr.Cells[9].Value,
                                    dr.Cells[10].Value,
                                    dr.Cells[11].Value,
                                    dr.Cells[12].Value,
                                    dr.Cells[13].Value,
                                    0,
                                    frmAW.rbRepeatOrder.Checked ? dr.Cells["Qty"].Value : 0);
                }
                
                if (i >= 20)
                {
                    StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {i} out of {max}", (i + 1), max);
                }
                else if (max > 20 && (i+1) == max)
                {
                    Application.DoEvents();
                }
               
            }

            if (dgvpost.Name == "dgvItemSelected")
            {
                if (dgvpost.Rows[0].Cells["Qty"].Value != dgvpost.Rows[0].Cells["Allocate"].Value)
                { dgvpost.Rows[0].Cells["Allocate"].Value = dgvpost.Rows[0].Cells["Qty"].Value; }
            }

            if (dgvpost.Name.Contains("dgvCustSelect"))
            { dgvpost.Sort(dgvpost.Columns[2], ListSortDirection.Ascending); }
            else
            { dgvpost.Sort(dgvpost.Columns["Sort"], ListSortDirection.Ascending); }

            dgvget.Rows.Clear();
        }

        void GetSelectedItems(DataGridView dgvget, DataGridView dgvpost)
        {
            var max = dgvget.SelectedRows.Count;
            var min = 1;
            StaticHelper._MainForm.ProgressClear();

            if (dgvget.Rows.Count <= 0)
            {
                return;
            }

            StaticHelper._MainForm.Progress("Please wait...", 1, max);

            for (int i = 0; i < dgvget.SelectedRows.Count; i++)
            {

                var dr = dgvget.SelectedRows[i];
                if (dgvpost.Name.Contains("dgvCustSelect"))
                {
                    dgvpost.Rows.Add(dr.Cells[0].Value,
                                     dr.Cells[1].Value,
                                     dr.Cells[2].Value);
                }
                else if (dgvpost.Name.Equals("dgvItemSelection"))
                {
                    dgvpost.Rows.Add(dr.Cells[0].Value,
                                   dr.Cells[1].Value,
                                   dr.Cells[2].Value,
                                   dr.Cells[3].Value,
                                   dr.Cells[4].Value,
                                   dr.Cells[5].Value,
                                   dr.Cells[6].Value,
                                   dr.Cells[7].Value,
                                   dr.Cells[8].Value,
                                   dr.Cells[9].Value,
                                   dr.Cells[10].Value,
                                   dr.Cells[11].Value,
                                   dr.Cells[12].Value,
                                   dr.Cells[13].Value);

                }
                else if (dgvpost.Name.Equals("dgvItemSelected"))
                {
                    dgvpost.Rows.Add(dr.Cells[0].Value,
                                    dr.Cells[1].Value,
                                    dr.Cells[2].Value,
                                    dr.Cells[3].Value,
                                    dr.Cells[4].Value,
                                    dr.Cells[5].Value,
                                    dr.Cells[6].Value,
                                    dr.Cells[7].Value,
                                    dr.Cells[8].Value,
                                    dr.Cells[9].Value,
                                    dr.Cells[10].Value,
                                    dr.Cells[11].Value,
                                    dr.Cells[12].Value,
                                    dr.Cells[13].Value,
                                    0,
                                    frmAW.rbRepeatOrder.Checked ? dr.Cells["Qty"].Value : 0);
                }
                min++;
                if (min >= 20)
                {
                   StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {min} out of {max}", min, max);
                }
                else if (max > 20 && min == max)
                {
                    Application.DoEvents();
                }
                
            }
   
            if (dgvpost.Name == "dgvItemSelected")
            {
                if (dgvpost.Rows[0].Cells["Qty"].Value != dgvpost.Rows[0].Cells["Allocate"].Value)
                { dgvpost.Rows[0].Cells["Allocate"].Value = dgvpost.Rows[0].Cells["Qty"].Value; }
            }

            if (dgvpost.Name.Contains("dgvCustSelect"))
            { dgvpost.Sort(dgvpost.Columns[2], ListSortDirection.Ascending); }
            else
            { dgvpost.Sort(dgvpost.Columns["Sort"], ListSortDirection.Ascending); }
            
            foreach (DataGridViewRow dr in dgvget.SelectedRows)
            { dgvget.Rows.Remove(dr); }
        }

        public void dgvCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var dgv = (DataGridView)sender;
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
            { return; }

            if (IsTheSameCellValue(dgv, e.ColumnIndex, e.RowIndex))
            { e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None; }
            else
            { e.AdvancedBorderStyle.Top = dgv.AdvancedCellBorderStyle.Top; }
        }

        public void dgvMergeFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (e.RowIndex == 0)
            { return; }

            if (IsTheSameCellValue(dgv, e.ColumnIndex, e.RowIndex))
            {
                e.Value = "";
                e.FormattingApplied = true;
            }
        }

        public void dgvNumberCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            { return; }

            DataGridView dgv = (DataGridView)sender;
            var row = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            if (IsNumeric(row))
            {
                double sNumber = double.Parse(row.ToString());
                string ret = string.Format("{0:#,0}", sNumber);
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ret;
            }
        }

        bool IsNumeric(object e)
        {
            bool result = false;
            if (e != null)
            { result = int.TryParse(e.ToString(), out int n); }

            return result;
        }

        bool IsTheSameCellValue(DataGridView dgv, int column, int row)
        {
            if (column < 2)
            {
                DataGridViewCell cell1 = dgv[column, row];
                DataGridViewCell cell2 = dgv[column, row - 1];
                if (cell1.Value == null || cell2.Value == null)
                { return false; }
                if (cell1.Value.ToString() == cell2.Value.ToString())
                { return true; }
                else
                { return false; }
            }
            else { return false; }
        }

        public void dgvRowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var dgv = (DataGridView)sender;
            using (SolidBrush b = new SolidBrush(dgv.RowHeadersDefaultCellStyle.ForeColor))
            { e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4); }
        }
    }
}

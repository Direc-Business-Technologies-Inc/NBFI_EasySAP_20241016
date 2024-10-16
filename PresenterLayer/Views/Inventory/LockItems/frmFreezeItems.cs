using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using zDeclare;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using DirecLayer;
using MetroFramework;
using DomainLayer.Helper;
using System.Text;
using ServiceLayer.Services;

namespace PresenterLayer
{
    public partial class frmFreezeItems : MetroForm
    {
        SAPHanaAccess hana { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        DataHelper helper { get; set; }
        int max_width = Screen.PrimaryScreen.Bounds.Width;
        int max_height = Screen.PrimaryScreen.Bounds.Height - 200;
        int _column = 1;
        bool UpdateActiveError = false;
        bool UpdateInActiveError = false;
        bool findMode = false;
        bool isClosedOrCanceled = false;
        int oCnt { get; set; } = 0;
        public frmFreezeItems()
        {
            InitializeComponent();
            helper = new DataHelper();
            hana = new SAPHanaAccess();
            msSql = new SAPMsSqlAccess();
        }
        private void frmFreezeItems_Load(object sender, EventArgs e)
        {
            MaximumSize = new Size(max_width, max_height);

            CreateGridItems();
            CreateGridWhs();

            txtDocEntry.Text = GetKey(2);

            StaticHelper._MainForm.ProgressClear();
            txtDocStatus.Text = "Open";
            txtDocDate.Text = DateTime.Now.ToShortDateString();
        }

        void CreateGridItems()
        {
            var col1 = new DataGridViewTextBoxColumn();
            var col2 = new DataGridViewTextBoxColumn();

            col1.Name = "ItemCode";
            col1.HeaderText = "Item No.";
            col1.Width = 150;
            col1.Frozen = true;

            col2.Name = "ItemName";
            col2.HeaderText = "Item Description";
            col2.Width = 300;
            col2.ReadOnly = true;

            dgvItems.Columns.Add(col1);
            dgvItems.Columns.Add(col2);


            dataGridLayout(dgvItems);
        }
        void CreateGridWhs()
        {

            var col1 = new DataGridViewTextBoxColumn();
            var col2 = new DataGridViewTextBoxColumn();

            col1.Name = "WhsCode";
            col1.HeaderText = "Warehouse Code";
            col1.Width = 150;
            col1.Frozen = true;

            col2.Name = "WhsName";
            col2.HeaderText = "Warehouse Name";
            col2.Width = 300;
            col2.ReadOnly = true;

            dgvWhs.Columns.Add(col1);
            dgvWhs.Columns.Add(col2);

            dataGridLayout(dgvWhs);
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

        public void RefreshData()
        {
            try
            {
                dgvItems.Rows.Clear();

                foreach (var x in DECLARE._items.Where(x => x.selected == true))
                {
                    object[] a = { x.ItemCode, x.ItemName };
                    dgvItems.Rows.Add(a);
                }

                dataGridLayout(dgvItems);
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }

        }
        public void RefreshWhs()
        {
            try
            {
                dgvWhs.Rows.Clear();

                foreach (var x in DECLARE._warehouse)
                {
                    object[] a = { x.WhsCode, x.WhsName };
                    dgvWhs.Rows.Add(a);
                }

                dataGridLayout(dgvWhs);

                foreach (DataGridViewRow row in dgvWhs.Rows)
                {
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                }
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        void LoadWhs()
        {
            dgvWhs.Rows.Clear();

            string query = "SELECT WhsCode,WhsName From OWHS Where WhsCode not in ('02-WHS1','02-WHS2','02-WHS3','02-WHS4')";

            var dt = hana.Get(query);

            //Config.oCnt = 1;
            foreach (DataRow row in dt.Rows)
            {
                var WhsCode = row["WhsCode"].ToString();
                var WhsName = row["WhsName"].ToString();

                object[] x = { false, WhsCode, WhsName };

                dgvWhs.Rows.Add(x);

                // frmMain.Progress(String.Format("Loading Warehouse {0} of {1}", Config.oCnt, dt.Rows.Count), dt.Rows.Count);
                //Config.oCnt++;
            }
        }
        void LoadItems()
        {
            dgvItems.Rows.Clear();

            var query = "SELECT ItemCode,ItemName,validFor [Active],validFrom [Active From],validTo [Active To],frozenFor [Inactive],frozenFrom [Inactive From],frozenTo [Inactive To] FROM OITM Where InvntItem = 'Y' Order by ItemCode";
            var dt = hana.Get(query);

            //Config.oCnt = 1;
            foreach (DataRow row in dt.Rows)
            {
                var ItemCode = row["ItemCode"].ToString();
                var ItemName = row["ItemName"].ToString();
                var Active = row["Active"].ToString();
                var AFrom = row["Active From"].ToString();
                var ATo = row["Active To"].ToString();
                var Inactive = row["Inactive"].ToString();
                var IFrom = row["Inactive From"].ToString();
                var ITo = row["Inactive To"].ToString();

                object[] x = { false, ItemCode, ItemName, Active, AFrom, ATo, Inactive, IFrom, ITo };

                dgvItems.Rows.Add(x);
            }
        }

        #region SAPB1
        //private void ActivateItems(string oItemCode, int min, int max)
        //{
        //    Items oItems;
        //    int lRetCode;
        //    try
        //    {
        //        oItems = (Items)SAPAccess.oCompany.GetBusinessObject(BoObjectTypes.oItems);

        //        oItems.GetByKey(oItemCode);

        //        oItems.Valid = BoYesNoEnum.tYES;
        //        oItems.ValidFrom = dtFrom.Value;
        //        oItems.ValidTo = dtTo.Value;

        //        oCnt = 1;
        //        DateTime startTime = DateTime.Now;
        //        for (int i = 0; i < oItems.WhsInfo.Count; i++)
        //        {
        //            foreach (DataGridViewRow x in dgvWhs.Rows)
        //            {
        //                oItems.WhsInfo.SetCurrentLine(i);
        //                double count = oItems.WhsInfo.Count;
        //                double percent = (i / count) * 100;
        //                if (oItems.WhsInfo.WarehouseCode == x.Cells[1].Value.ToString())
        //                {
        //                    // Calculate the time remaining:
        //                    TimeSpan timeRemaining = TimeSpan.FromTicks(DateTime.Now.Subtract(startTime).Ticks * ((int)count - (i + 1)) / (i + 1));

        //                    frmMain.Progress(String.Format("{2}% {3:hh\\:mm\\:ss}s remaining - Updating Item:{0} [{4} of {5}] | Warehouse: {1}", oItemCode, x.Cells[1].Value, percent.ToString("#0.#"), timeRemaining, min, max), oItems.WhsInfo.Count);

        //                    oItems.WhsInfo.Locked = SAPbobsCOM.BoYesNoEnum.tYES;
        //                }
        //            }

        //            oCnt++;
        //        }

        //        lRetCode = oItems.Update();

        //        if (lRetCode == 0)
        //        {
        //            //frmMain.NotiMsg("Operation completed successfully", Color.Green);
        //            UpdateActiveError = false;
        //        }
        //        else
        //        {
        //            UpdateActiveError = true;
        //            //MessageBox.Show(SAPAccess.oCompany.GetLastErrorDescription());
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateActiveError = true;
        //    }
        //}
        //private void InactiveItems(string oItemCode)
        //{
        //    Items oItems;
        //    int lRetCode;
        //    try
        //    {
        //        oItems = (Items)SAPAccess.oCompany.GetBusinessObject(BoObjectTypes.oItems);

        //        oItems.GetByKey(oItemCode);

        //        oItems.Frozen = BoYesNoEnum.tYES;
        //        oItems.FrozenFrom = dtFrom.Value;
        //        oItems.FrozenTo = dtTo.Value;

        //        lRetCode = oItems.Update();

        //        if (lRetCode != 0)
        //        {
        //            UpdateInActiveError = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion


        private string NextKey(int Object)
        {
            try
            {
                var dt = msSql.Get($"SBO_GetNextAutoKey '{Object}','Y'");
                var autokey = dt.Rows[0]["AutoKey"].ToString();
                return autokey;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string GetKey(int Object)
        {
            try
            {
                var dt = msSql.Get($"SBO_GetNextAutoKey '{Object}','N'");
                var autokey = dt.Rows[0]["AutoKey"].ToString();
                return autokey;
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                return "";
            }

        }


        private void frmFreezeItems_MaximumSizeChanged(object sender, EventArgs e)
        {
            Size = new Size(MdiParent.ClientSize.Width, max_height);
            WindowState = FormWindowState.Normal;
            Location = new Point(0, 0);
        }

        #region SaveDocument
        //void SaveDocument()
        //{
        //    try
        //    {
        //        if (InsertTempItems() != true && InsertTempWhs() != true)
        //        {
        //            //@Id nvarchar(50),
        //            //@UserId nvarchar(50),
        //            //@DocEntry int,
        //            //@DocDate datetime,
        //            //@Canceled char(1),
        //            //@DateFrom datetime,
        //            //@DateTo datetime,
        //            //@Remarks nvarchar(254)

        //            //Add docs
        //            string query = $"[sp_InsertWHL] '{SystemSettings.GetLogonSid.getLogonSid()}'" + //Uid
        //                           $",'{SboCred.UserID}'" + //UserId
        //                           $",'{NextKey(2)}'" +          //Next Key
        //                           $",'{DateTime.Now}','N'" +    //DocDate
        //                           $",'{dtFrom.Value}'" +        //Date From
        //                           $",'{dtTo.Value}'" +          //Date To
        //                           $",'{txtRemarks.Text}'";      //Remarks

        //            if (DataAccessSQL.Execute(DataAccess.conStr("SQL"), query, frmMain) == false)
        //            {
        //                //Error
        //                DeleteTemp(SystemSettings.GetLogonSid.getLogonSid(), SboCred.UserID);
        //            }
        //            else
        //            {
        //                //Success
        //                frmMain.NotiMsg("Operation completed successfully", Color.Green);
        //                //Delete Temp
        //                DeleteTemp(SystemSettings.GetLogonSid.getLogonSid(), SboCred.UserID);
        //                //Clear Form
        //                New();
        //            }
        //        }
        //        else
        //        {
        //            frmMain.NotiMsg("Error in saving data.", Color.Red);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        frmMain.NotiMsg(ex.Message, Color.Red);
        //    }

        //}

        //private bool InsertTempItems()
        //{
        //    bool isError = false;

        //    foreach (DataGridViewRow row in dgvItems.Rows)
        //    {
        //        if (row.Cells[0].Value != null)
        //        {
        //            //INSERT ITEMS TO TEMP
        //            var query = $"[sp_InsertTemp2] '{SystemSettings.GetLogonSid.getLogonSid()}','{SboCred.UserID}','{DateTime.Now}','{row.Cells[0].Value}','{row.Cells[1].Value}','',''";

        //            if (DataAccessSQL.Execute(DataAccess.conStr("SQL"), query, frmMain) == false)
        //            {
        //                isError = true;
        //            }
        //        }

        //    }

        //    if (isError == true)
        //    {
        //        //delete temp
        //        DeleteTemp(SystemSettings.GetLogonSid.getLogonSid(), SboCred.UserID);
        //    }

        //    return isError;
        //}
        //private bool InsertTempWhs()
        //{
        //    bool isError = false;

        //    foreach (DataGridViewRow row in dgvWhs.Rows)
        //    {
        //        if (row.Cells[0].Value != null)
        //        {
        //            //INSERT ITEMS TO TEMP
        //            var query = $"[sp_InsertTemp2] '{SystemSettings.GetLogonSid.getLogonSid()}','{SboCred.UserID}','{DateTime.Now}','','','{row.Cells[0].Value}','{row.Cells[1].Value}'";

        //            if (DataAccessSQL.Execute(DataAccess.conStr("SQL"), query, frmMain) == false)
        //            {
        //                isError = true;
        //            }
        //        }

        //    }

        //    if (isError == true)
        //    {
        //        //delete temp
        //        DeleteTemp(SystemSettings.GetLogonSid.getLogonSid(), SboCred.UserID);
        //    }

        //    return isError;
        //}
        #endregion

        void DeleteTemp(string Sid, string UserId)
        {
            try
            {
                string q = $"DELETE FROM tmpWHL Where Id = '{Sid}' and UserId = '{UserId}'";
                msSql.Execute(q);
            }
            catch (Exception ex)
            {
               StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Add")
            {
                Add();
            }
            else
            {
                Update(txtDocEntry.Text);
            }
            //CHECK IF ITEM ALREADY EXIST IN OPEN DOC

        }
        void Update(string DocEntry)
        {
            var Error = false;
            var query = $"UPDATE OWHL SET DateFrom = '{dtFrom.Value}',DateTo = '{dtTo.Value}' Where DocEntry = '{DocEntry}'";

            try
            {
                if (hana.Execute(query) > 0)
                {
                    //insert items
                    
                    if (hana.Execute($"DELETE FROM WHL1 Where DocEntry = '{DocEntry}'") > 0)
                    {
                        foreach (DataGridViewRow row1 in dgvItems.Rows)
                        {
                            
                            if (hana.Execute($"INSERT INTO WHL1 VALUES ('{DocEntry}','{row1.Cells[0].Value}','{row1.Cells[1].Value}')") < 0)
                                Error = true;
                        }
                    }
                    else Error = true;

                    //insert whs
                    if (hana.Execute($"DELETE FROM WHL2 Where DocEntry = '{DocEntry}'") > 0)
                    {
                        foreach (DataGridViewRow row2 in dgvWhs.Rows)
                        {
                            
                            if (hana.Execute($"INSERT INTO WHL2 VALUES ('{DocEntry}','{row2.Cells[0].Value}','{row2.Cells[1].Value}')") < 0)
                                Error = true;
                        }
                    }
                    else Error = true;
                }
                else
                {
                    Error = true;
                }

                if (Error != true)
                {
                    StaticHelper._MainForm.ShowMessage("Operation completed successfully.");
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Error encountered upon updating Documents.", true);
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        void Add()
        {
            string _item = "";
            string _whs = "";
            string _docno = "";
            bool blockFlag = false;

            foreach (DataGridViewRow x in dgvItems.Rows)
            {
                string itemCode = x.Cells[0].Value.ToString();
                foreach (DataGridViewRow y in dgvWhs.Rows)
                {
                    string whsCode = y.Cells[0].Value.ToString();

                    if (BlockExistingItems(itemCode, whsCode, out _item, out _whs, out _docno) == true)
                    {
                        MetroMessageBox.Show(StaticHelper._MainForm, $"Item: {_item} in Warehouse: {_whs} already exists in Document No. {_docno}", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        blockFlag = true;
                    }
                }
            }

            if (blockFlag == false)
            {
                //SaveDocument();
            }
        }

        void New()
        {
            isClosedOrCanceled = false;
            findMode = false;

            foreach (Control c in Controls) { if (c is TextBox) c.Text = ""; } //Clear TextBox

            txtDocEntry.Text = GetKey(2);
            txtDocStatus.Text = "Open";
            txtDocDate.Text = DateTime.Now.ToShortDateString();

            tabControl1.Enabled = true;
            txtItemSearch.Enabled = true;
            txtSearchWhs.Enabled = true;

            dgvItems.Enabled = true;
            dgvWhs.Enabled = true;
            dgvItems.Rows.Clear();
            dgvWhs.Rows.Clear();
            btnAddItems.Enabled = true;
            btnAddWhs.Enabled = true;

            //Clear LIST<>
            DECLARE._items.Clear();
            DECLARE._warehouse.Clear();
            DECLARE._error.Clear();
            var sboCred = new SboCredentials();
            DeleteTemp(SystemSettings.GetLogonSid.getLogonSid(), sboCred.UserId);

            //btn Add
            btnAdd.Enabled = true;

            //foreach (Control c in tabControl1.Controls) c.Enabled = true;
            txtRemarks.Enabled = true;
            btnAdd.Enabled = true;

        }

        public bool BlockExistingItems(string itemCode, string whsCode, out string _item, out string _whs, out string docEntry)
        {
            bool blocked = false;
            _item = "";
            _whs = "";
            docEntry = "";
            try
            {
                var query = "SELECT DISTINCT A.DocEntry FROM OWHL A " +
                               "INNER JOIN WHL1 B ON A.DocEntry = B.DocEntry " +
                               "INNER JOIN WHL2 C ON A.DocEntry = C.DocEntry " +
                              $"WHERE B.ItemCode = '{itemCode}' And C.WhsCode = '{whsCode}' " +
                              $"And (CAST(A.DateFrom AS DATE) <= '{dtTo.Value}' And CAST(A.DateTo AS DATE) >= '{dtFrom.Value}') " +
                               "And A.Canceled = 'N'";

                var dt = msSql.Get(query);

                if (dt.Rows.Count > 0)
                {
                    blocked = true;
                    _item = itemCode;
                    _whs = whsCode;
                    docEntry = dt.Rows[0]["DocEntry"].ToString();
                }

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
            }
            return blocked;
        }
        
        void ReloadItems()
        {
            LoadItems();
            LoadWhs();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void txtItemSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvItems.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row.Cells[_column].Value.ToString().ToUpper().StartsWith(txtItemSearch.Text.ToUpper()))
                    {
                        row.Selected = true;
                        dgvItems.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            var iSelection = new frmItemSelection(this);
            iSelection.ShowDialog();
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to remove selected item?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row.Selected == true)
                    {
                        //Remove selected items
                        DECLARE._items.RemoveAll(x => x.ItemCode == row.Cells[0].Value.ToString());
                    }
                }
                //Rebind data
                RefreshData();
            }
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

                    msItems.Show(dgvItems, mousePosition);
                }
            }
        }
        
        private void dgvItems_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _column = e.ColumnIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fS = new frmSearch2();
            fS.oSearchMode = "OWHS";
            fS.fromWhsLock = true;
            frmSearch2._title = "Lists of Warehouse";
            fS.allowMultiple = true;
            fS.ShowDialog();

            dgvWhs.Rows.Clear();
            foreach (var x in DECLARE._multipleSelection)
            {
                //check if exist
                if (DECLARE._warehouse.Exists(a => a.WhsCode == x.Code))
                {
                    ////If exist
                    //foreach (var z in DECLARE._warehouse.Where(a => a.WhsCode == x.Code))
                    //{
                    //    z.WhsCode 
                    //}
                }
                else
                {
                    //add selected whs to temp list<>
                    DECLARE._warehouse.Add(new DECLARE.warehouse { WhsCode = x.Code, WhsName = x.Name });
                }

            }
            //clear selection
            DECLARE._multipleSelection.Clear();

            //add rows
            foreach (var x in DECLARE._warehouse)
            {
                //add selected whs to temp list<>
                dgvWhs.Rows.Add(x.WhsCode, x.WhsName);
            }

            foreach (DataGridViewRow row in dgvWhs.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to remove selected warehouse?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvWhs.Rows)
                {
                    if (row.Selected == true)
                    {
                        //Remove selected items
                        DECLARE._warehouse.RemoveAll(x => x.WhsCode == row.Cells[0].Value.ToString());
                    }
                }
                //Rebind data
                RefreshWhs();
            }
        }

        private void dgvWhs_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    dgvWhs.CurrentCell = dgvWhs.Rows[e.RowIndex].Cells[e.ColumnIndex + 1];
                    dgvWhs.Rows[e.RowIndex].Selected = true;
                    dgvWhs.Focus();

                    var mousePosition = dgvItems.PointToClient(Cursor.Position);

                    msWhs.Show(dgvItems, mousePosition);
                }
            }
        }

        private void frmFreezeItems_MouseClick(object sender, MouseEventArgs e)
        {

        }


        /// <summary>
        /// Activate Items
        /// </summary>
        private void ActivateItems(bool isActive = true)
        {
            int lRetCode;
            try
            {
                //Item Details
                var dtHeader = msSql.Get($"SELECT [DocEntry],[DocDate],[Canceled],[DateCanceled],[DateFrom],[DateTo],[Remarks] " +
                                                            $"FROM OWHL Where DateTo < '{DateTime.Now}' And DocStatus = 'O' And Status2 = 'O'");

                foreach (DataRow row_Header in dtHeader.Rows)
                {
                    oCnt = 1;
                    var docEntry = row_Header["DocEntry"].ToString();
                    //GET ITEMS
                    
                    var dtItems = msSql.Get($"SELECT ItemCode,ItemName FROM WHL1 Where DocEntry = '{docEntry}'");
                    foreach (DataRow row_Items in dtItems.Rows)
                    {
                        var itemCode = row_Items["ItemCode"].ToString();
                        var itemName = row_Items["ItemName"].ToString();
                        var oItems = $"Items('{itemCode}')";
                        var dtWhsInfo = hana.Get("SELECT COUNT(WhsCode) FROM OWHS");
                        //GET WAREHOUSE
                        var sbJson = new StringBuilder("{");
                        var cntRows = dtWhsInfo.Rows.Count;
                        for (int i = 0; i < cntRows; i++)
                        {
                            var dtWhs = msSql.Get($"SELECT WhsCode,WhsName FROM WHL2 Where DocEntry = '{docEntry}'");
                            sbJson.AppendLine(@" ""ItemWarehouseInfoCollection"": [");
                            var whsCode = row_Items["WhsCode"].ToString();
                            sbJson.AppendLine(@"    {");
                            sbJson.AppendLine($@"       ""WarehouseCode"": ""{whsCode}"",");
                            if (isActive)
                            {
                                sbJson.AppendLine($@"       ""Locked"": ""tNo""");
                            }
                            else
                            {
                                sbJson.AppendLine($@"       ""Locked"": ""tYes""");
                            }

                            if (i < cntRows)
                            { sbJson.AppendLine(@"    },"); }
                            else
                            { sbJson.AppendLine(@"    }"); }

                            double total = cntRows * dtItems.Rows.Count;
                            double perc = (Convert.ToDouble(oCnt) / total) * 100;
                            //Progress Bar
                            StaticHelper._MainForm.Progress($"[{Math.Round(perc, 0)}%] Uploading Doc No. {docEntry} - Item: {itemName}",1, Convert.ToInt32(total));
                            oCnt++;
                        }

                        //Update Item
                        var serviceLayerAccess = new ServiceLayerAccess();
                        

                        if (serviceLayerAccess.ServiceLayer_Posting(sbJson, "UPDATE", oItems, "ItemCode", out string output, out string val))
                        {
                            StaticHelper._MainForm.ShowMessage($"Successfully uploaded Item: {itemCode} - {itemName}");
                            Application.DoEvents();
                            StaticHelper._MainForm.ProgressClear();
                            //UPDATE STATUS OF ITEM - SQL
                            UpdateStatus2(docEntry);
                        }
                        else
                        {
                            DECLARE._error.Add(new DECLARE.ErrorLogs
                            {
                                ErrorCode = "-1",
                                ErrorDate = DateTime.Now.ToString(),
                                ErrorMessage = $"Doc No. {docEntry} - ItemCode: {itemCode} | {output}"
                            });
                        }
                    }
                    oCnt++;
                }

                //Error
                if (DECLARE._error.Count > 0)
                {
                    var el = new ErrorLogs();
                    el.ShowDialog();
                    StaticHelper._MainForm.ProgressClear();

                    DECLARE._error.Clear();
                }
            }
            catch (Exception ex)
            {
                DECLARE._error.Add(new DECLARE.ErrorLogs
                {
                    ErrorCode = "-1",
                    ErrorDate = DateTime.Now.ToString(),
                    ErrorMessage = $"{ex.Message}"
                });
            }
        }
        /// <summary>
        /// Inactive Items
        /// </summary>
        #region InActiveItems
        //private void InActivateItems()
        //{
        //    int lRetCode;
        //    try
        //    {
        //        //Item Details
        //        var dtHeader = msSql.Get($"SELECT [DocEntry],[DocDate],[Canceled],[DateCanceled],[DateFrom],[DateTo],[Remarks] " +
        //                                                    $"FROM OWHL Where CAST(DateFrom AS DATE) = '{DateTime.Now.ToShortDateString()}' And DocStatus = 'O' And Status1 = 'O'");

        //        foreach (DataRow row_Header in dtHeader.Rows)
        //        {
        //            oCnt = 1;
        //            string docEntry = row_Header["DocEntry"].ToString();
        //            //GET ITEMS
        //            var dtItems = msSql.Get($"SELECT ItemCode,ItemName FROM WHL1 Where DocEntry = '{docEntry}'");
        //            foreach (DataRow row_Items in dtItems.Rows)
        //            {
        //                var itemCode = row_Items["ItemCode"].ToString();
        //                var itemName = row_Items["ItemName"].ToString();
        //                var oItems = $"Items('{itemCode}')";

        //                //GET WAREHOUSE
        //                for (int i = 0; i < oItems.WhsInfo.Count; i++)
        //                {
        //                    var dtWhs = msSql.Get($"SELECT WhsCode,WhsName FROM WHL2 Where DocEntry = '{docEntry}'");
        //                    foreach (DataRow row_Whs in dtWhs.Rows)
        //                    {
        //                        var whsCode = row_Whs["WhsCode"].ToString();

        //                        oItems.WhsInfo.SetCurrentLine(i);
        //                        if (oItems.WhsInfo.WarehouseCode == whsCode)
        //                        {
        //                            oItems.WhsInfo.Locked = SAPbobsCOM.BoYesNoEnum.tYES;

        //                        }
        //                    }
        //                    double total = oItems.WhsInfo.Count * dtItems.Rows.Count;
        //                    double perc = (Convert.ToDouble(oCnt) / total) * 100;
        //                    //Progress Bar
        //                    StaticHelper._MainForm.Progress($"[{Math.Round(perc, 0)}%] Uploading Doc No. {docEntry} - Item: {itemName}", 1, Convert.ToInt32(total));
        //                    oCnt++;
        //                }
        //                //Update Item
        //                lRetCode = oItems.Update();

        //                if (lRetCode != 0)
        //                {
        //                    DECLARE._error.Add(new DECLARE.ErrorLogs
        //                    {
        //                        ErrorCode = SAPAccess.oCompany.GetLastErrorCode().ToString(),
        //                        ErrorDate = DateTime.Now.ToString(),
        //                        ErrorMessage = $"Doc No. {docEntry} - ItemCode: {itemCode} | {SAPAccess.oCompany.GetLastErrorDescription()}"
        //                    });
        //                }
        //                else
        //                {
        //                    StaticHelper._MainForm.ShowMessage($"Successfully uploaded Item: {itemCode} - {itemName}");
        //                    Application.DoEvents();
        //                    StaticHelper._MainForm.ProgressClear();
        //                    //UPDATE STATUS OF ITEM - SQL
        //                    UpdateStatus1(docEntry);
        //                }
        //                oCnt++;
        //            } //end of item loop

        //            Application.DoEvents();
        //        }


        //        //Error
        //        if (DECLARE._error.Count > 0)
        //        {
        //            ErrorLogs el = new ErrorLogs();
        //            el.ShowDialog();

        //            StaticHelper._MainForm.ProgressClear();
        //            DECLARE._error.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DECLARE._error.Add(new DECLARE.ErrorLogs
        //        {
        //            ErrorCode = SAPAccess.oCompany.GetLastErrorCode().ToString(),
        //            ErrorDate = DateTime.Now.ToString(),
        //            ErrorMessage = $"{ex.Message}"
        //        });
        //    }
        //}
        #endregion

        void UpdateStatus1(string docEntry)
        {
            try
            {
                var query = $"Update OWHL SET Status1 = 'C' Where DocEntry = '{docEntry}'";
                msSql.Execute(query);

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message,true);
            }
        }
        void UpdateStatus2(string docEntry)
        {
            try
            {
                var query = $"Update OWHL SET DocStatus = 'C',Status2 = 'C' Where DocEntry = '{docEntry}'";
                msSql.Execute(query);

            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }
        void CancelDoc(string docEntry)
        {
            try
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to cancel the document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var query = $"Update OWHL SET Canceled = 'Y' Where DocEntry = '{docEntry}'";
                    if (msSql.Execute(query) < 0)
                    {
                        //error
                        StaticHelper._MainForm.ShowMessage("Cancellation of document failed.", true);
                    }
                    else
                    {
                        //successful
                        StaticHelper._MainForm.ShowMessage("Cancellation of document successful.");
                        New();
                    }
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ActivateItems(false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ActivateItems();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            findMode = false;
            if (dgvItems.Rows.Count > 0)
            {
                var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    New();
                }
            }
            else
            {
                New();
            }
        }
        

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CancelDoc(txtDocEntry.Text);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            findMode = true;
            Find();
        }

        void Find()
        {
            var status1 = "";
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var fS = new frmSearch2();
                fS.fromSQL = true;
                fS.oSearchMode = "OWHL";
                fS.ShowDialog();

                if (fS.oCode != null)
                {
                    //load Data
                    var query = $"SELECT [DocEntry],CAST([DocDate] AS DATE) DocDate,[Canceled],ISNULL([DateFrom],'') DateFrom ,ISNULL([DateTo],'') DateTo  " +
                                   $",(CASE WHEN [DocStatus] = 'O' AND Canceled = 'N' THEN 'Open' WHEN [DocStatus] = 'C' AND Canceled = 'N' THEN 'Closed' WHEN Canceled = 'Y' THEN 'Cancelled' END) [DocStatus]" +
                                   $",[Remarks],[Status1] FROM OWHL Where [DocEntry] = {fS.oCode}";                    
                    var dt = msSql.Get(query);

                    if (dt.Rows.Count > 0)
                    {
                        txtDocDate.Text = Convert.ToDateTime(dt.Rows[0]["DocDate"]).ToShortDateString();
                        txtDocStatus.Text = dt.Rows[0]["DocStatus"].ToString();
                        txtDocEntry.Text = dt.Rows[0]["DocEntry"].ToString();
                        status1 = dt.Rows[0]["Status1"].ToString();
                        dtFrom.Text = dt.Rows[0]["DateFrom"].ToString();
                        dtTo.Text = dt.Rows[0]["DateTo"].ToString();

                        if (txtDocStatus.Text != "Open") { isClosedOrCanceled = true; } else { isClosedOrCanceled = false; }

                        //Load Items
                        var iquery = $"SELECT ItemCode,ItemName From WHL1 Where DocEntry = {fS.oCode}";
                        var dtItems = msSql.Get(query);

                        if (dtItems.Rows.Count > 0)
                        {
                            dgvItems.Rows.Clear();
                            foreach (DataRow row in dtItems.Rows)
                            {
                                dgvItems.Rows.Add(row["ItemCode"], row["ItemName"]);

                                DECLARE._items.Add(new DECLARE.items
                                {
                                    ItemCode = row["ItemCode"].ToString()
                                    ,
                                    ItemName = row["ItemName"].ToString()
                                    ,
                                    selected = true
                                });
                            }
                        }

                        //Load Whs
                        var wquery = $"SELECT WhsCode,WhsName From WHL2 Where DocEntry = {fS.oCode}";
                        var dtWhs = msSql.Get(wquery);
                        if (dtWhs.Rows.Count > 0)
                        {
                            dgvWhs.Rows.Clear();
                            foreach (DataRow row in dtWhs.Rows)
                            {
                                dgvWhs.Rows.Add(row["WhsCode"], row["WhsName"]);

                                DECLARE._warehouse.Add(new DECLARE.warehouse
                                {
                                    WhsCode = row["WhsCode"].ToString()
                                    ,
                                    WhsName = row["WhsName"].ToString()
                                });
                            }
                        }

                        //if doc is cancelled or closed
                        if (txtDocStatus.Text == "Cancelled" || txtDocStatus.Text == "Closed")
                        {
                            //foreach (Control ctl in tabControl1.Controls) ctl.Enabled = false;
                            dgvWhs.Enabled = false;
                            txtSearchWhs.Enabled = false;
                            btnAddWhs.Enabled = false;
                            dgvItems.Enabled = false;
                            txtItemSearch.Enabled = false;
                            btnAddItems.Enabled = false;
                            btnAddWhs.Enabled = false;
                            txtRemarks.Enabled = false;
                            btnAdd.Enabled = false;
                        }
                        else
                        {
                            dgvWhs.Enabled = true;
                            txtSearchWhs.Enabled = true;
                            btnAddWhs.Enabled = true;
                            dgvItems.Enabled = true;
                            txtItemSearch.Enabled = true;
                            btnAddItems.Enabled = true;
                            btnAddWhs.Enabled = true;
                            tabControl1.Enabled = true;
                            txtRemarks.Enabled = true;
                            btnAdd.Enabled = true;
                            //if open
                            if (status1 == "O")
                            {
                                btnAdd.Enabled = true;
                                dtFrom.Enabled = true;
                                dtTo.Enabled = true;
                                btnAdd.Text = "Update";
                            }
                            else if (status1 == "C")
                            {
                                btnAdd.Enabled = false;
                                dtFrom.Enabled = false;
                                dtTo.Enabled = false;
                                tabControl1.Enabled = false;
                                btnAdd.Text = "Add";
                            }

                        }

                        foreach (DataGridViewRow row in dgvItems.Rows)
                        {
                            row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            row.HeaderCell.Value = string.Format("{0}", row.Index + 1);
                        }

                        foreach (DataGridViewRow row in dgvWhs.Rows)
                        {
                            row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            row.HeaderCell.Value = string.Format("{0}", row.Index + 1);
                        }
                    }
                }
            }
        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (btnAdd.Enabled == false || findMode == true)
                {
                    if (isClosedOrCanceled == false)
                    {
                        var mousePosition = panel2.PointToClient(Cursor.Position);
                        msCancel.Show(panel2, mousePosition);
                    }
                }
            }
        }

        private void frmFreezeItems_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }
    }
}



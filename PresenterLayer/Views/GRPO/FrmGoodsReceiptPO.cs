using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DirecLayer._02_Form.MVP.Presenters;
using PresenterLayer.Helper;
using PresenterLayer;
using DirecLayer._02_Form.MVP.Presenters.PriceTag;
using zDeclare;
using DomainLayer;
using MetroFramework;
using DirecLayer;
using DomainLayer.Helper;
//using EasySAP._01_Models.Context;
using PresenterLayer.Services.Security;

namespace PresenterLayer
{
    public partial class FrmGoodsReceiptPO : MetroForm, IFrmGoodsReceiptPO
    {
        public FrmGoodsReceiptPO()
        {
            InitializeComponent();
        }

        private bool FindMode = false;
        private int findDocSearch;

        public static int datetimeCount = 0;

        private string Vat { get; set; }

        private double VatRate { get; set; }

        private string DefaultWarehouse { get; set; }

        private string table { get; set; }

        private string RawCurr { get; set; }

        private DataTable CurrentDataSet { get; set; }

        public bool IsFindMode
        {
            get => FindMode;
            set => FindMode = value;
        }

        public Panel UdfPanel
        {
            get => panel1;
        }

        public string DocEntry
        {
            get => TxtDocEntry.Text;
            set => TxtDocEntry.Text = value;
        }

        public string DocNum
        {
            get => txtDocNum.Text;
            set => txtDocNum.Text = value;
        }

        public string Series
        {
            get => CmbSeries.SelectedValue.ToString();
            set => CmbSeries.Text = value;
        }

        public string SuppCode
        {
            get => TxtVendorCode.Text;
            set => TxtVendorCode.Text = value;
        }

        public string SuppName
        {
            get => TxtVendorName.Text;
            set => TxtVendorName.Text = value;
        }

        public string ContactPerson
        {
            get => TxtContactPerson.Text;
            set => TxtContactPerson.Text = value;
        }

        public string Company
        {
            get => Convert.ToString(CmbCompany.SelectedValue);
            set => CmbCompany.Text = value;
        }

        public string Department
        {
            get => TxtDepartment.Text;
            set => TxtDepartment.Text = value;
        }

        public string BpCurrency
        {
            get => CmbCurrency.Text;
            set => CmbCurrency.Text = value;
        }

        public string BpRate
        {
            get => TxtBpRate.Text;
            set => TxtBpRate.Text = value;
        }

        public string Status
        {
            get => CmbStatus.Text;
            set => CmbStatus.Text = value;
        }

        public string PostingDate
        {
            get => TxtPostingDate.Text;
            set => TxtPostingDate.Text = value;
        }

        public string DocumentDate
        {
            get => TxtDocumentDate.Text;
            set => TxtDocumentDate.Text = value;
        }

        public string DeliveryDate
        {
            get => TxtDeliveryDate.Text;
            set => TxtDeliveryDate.Text = value;
        }

        public string CancellationDate
        {
            //get => TxtCancellationDate.Text;
            //set => TxtCancellationDate.Text = value;
            get;set;
        }

        public DataGridView Table
        {
            get => DgvItem;
        }

        public DataGridView Udf
        {
            get => DgvUdf;
        }

        public DataGridView TablePreview
        {
            get => DgvPreviewItem;
        }

        public string Remark
        {
            get => TxtRemark.Text;
            set => TxtRemark.Text = value;
        }

        public GoodReceiptService Presenter
        {
            private get;
            set;
        }

        public string VatGroup
        {
            get => Vat;
            set => Vat = value;
        }

        public double VatGroupRate
        {
            get => VatRate;
            set => VatRate = value;
        }


        public string Warehouse
        {
            get => DefaultWarehouse;
            set => DefaultWarehouse = value;
        }
        public string Service
        {
            get => CmbServiceType.Text;
            set => CmbServiceType.Text = value;
        }

        public string TotalBeforeDiscount
        {
            get => TxtTotalBefDisc.Text;
            set => TxtTotalBefDisc.Text = value;
        }

        public string RefNo
        {
            get => txtRefNo.Text;
            set => txtRefNo.Text = value;
        }


        public string Tax
        {
            get => TxtTax.Text;
            set => TxtTax.Text = value;
        }

        public string DiscountInput
        {
            get => TxtDiscountInput.Text;
            set => TxtDiscountInput.Text = value.ToString();
        }

        public string DiscountAmount
        {
            get => TxtDiscAmount.Text;
            set => TxtDiscAmount.Text = value;
        }

        public string Total
        {
            get => TxtTotal.Text;
            set => TxtTotal.Text = value;
        }

        public ContextMenuStrip MsItems
        {
            get => msItems;
        }

        public string RawCurrency
        {
            get => RawCurr;
            set => RawCurr = value;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.D1:
                    TabPO.SelectedIndex = 0;
                    break;

                case Keys.Control | Keys.D2:
                    TabPO.SelectedIndex = 1;
                    break;

                case Keys.Control | Keys.D3:
                    TabPO.SelectedIndex = 2;
                    break;

                case Keys.Control | Keys.D4:
                    TabPO.SelectedIndex = 3;
                    break;

                case Keys.Control | Keys.D5:
                    TabPO.SelectedIndex = 4;
                    break;

                case Keys.Control | Keys.B:
                    Presenter.GetSupplierInformation();
                    break;

                case Keys.Control | Keys.M:
                    CmbCompany.Focus();
                    break;

                case Keys.Control | Keys.D:
                    Presenter.GetDepartment();
                    break;

                case Keys.Control | Keys.E:
                    CmbSeries.Focus();
                    break;

                case Keys.Control | Keys.S:
                    CmbStatus.Focus();
                    break;

                case Keys.Control | Keys.I:
                    Presenter.DisplayItemList(CmbServiceType.Text);
                    break;

                case Keys.Escape:
                    Close();
                    break;

                case Keys.Control | Keys.N:
                    var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Presenter.ClearField(true);
                        TabPO.SelectedIndex = 0;
                        BtnRequest.Text = "Add";
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FrmPurchaseOrder_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            CmbCompany.DisplayMember = "Name";
            CmbCompany.ValueMember = "Code";
            CmbCompany.DataSource = Presenter.GetCompany();

            CmbSeries.DisplayMember = "Name";
            CmbSeries.ValueMember = "Code";
            CmbSeries.DataSource = Presenter.GetDocumentSeries();

            CmbServiceType.Text = "Item";

            var files = Presenter.GetDocumentCrystalForms();

            TxtPostingDate.Text = DtPostingDate.Value.ToShortDateString();
            TxtDocumentDate.Text = DtDocumentDate.Value.ToShortDateString();

            foreach (FileInfo file in files)
            {
                CmbPrintPreview.Items.Add(file.Name);
            }

            var currs = Presenter.GetCurrencies();
            CmbCurrency.Items.Clear();
            foreach (DataRow curr in currs.Rows)
            {
                CmbCurrency.Items.Add(curr[0].ToString());
            }

            DgvFindDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DgvUdf.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.Resizable = DataGridViewTriState.False);
            foreach (DataGridViewRow row in DgvUdf.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals("Prepared By"))
                {
                    DgvUdf.Rows[row.Index].Cells["Field"].Value = DomainLayer.Models.EasySAPCredentialsModel.ESUserId;
                    DgvUdf.Rows[row.Index].Cells["Field"].ReadOnly = true;
                    break;
                }
            }
        }

        private void PbSuppCode_Click(object sender, EventArgs e)
        {
            Presenter.GetSupplierInformation();

            CmbCurrency.Enabled = RawCurrency == "##" ? true : false;
        }

        private void PbDepartment_Click(object sender, EventArgs e)
        {
            Presenter.GetDepartment();
        }

        private void CmbCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            Presenter.GetCurrencyRate();
        }

        private void CmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Presenter.ChangeDocumentStatus())
            {
                BtnRequest.Text = "Add";
            }
        }

        private void CmbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            Presenter.ChangeDocumentNumber();
        }

        private void CmbServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Presenter.ChangeServiceType();
        }

        private void DtPostingDate_ValueChanged(object sender, EventArgs e)
        {
            TxtPostingDate.Text = DtPostingDate.Value.ToShortDateString();
        }

        private void DtPostingDate_Enter(object sender, EventArgs e)
        {
            TxtPostingDate.Text = DtPostingDate.Value.ToShortDateString();
        }

        private void DtDocumentDate_ValueChanged(object sender, EventArgs e)
        {
            TxtDocumentDate.Text = DtDocumentDate.Value.ToShortDateString();
        }

        private void DtDocumentDate_Enter(object sender, EventArgs e)
        {
            TxtDocumentDate.Text = DtDocumentDate.Value.ToShortDateString();
        }

        private void DtDeliveryDate_ValueChanged(object sender, EventArgs e)
        {
            TxtDeliveryDate.Text = DtDeliveryDate.Value.ToShortDateString();
        }

        private void DtDeliveryDate_Enter(object sender, EventArgs e)
        {
            TxtDeliveryDate.Text = DtDeliveryDate.Value.ToShortDateString();
        }

        //private void DtCancellationDate_ValueChanged(object sender, EventArgs e)
        //{
        //    TxtCancellationDate.Text = DtCancellationDate.Value.ToShortDateString();
        //}

        //private void DtCancellationDate_Enter(object sender, EventArgs e)
        //{
        //    TxtCancellationDate.Text = DtCancellationDate.Value.ToShortDateString();
        //}

        private void BtnItem_Click(object sender, EventArgs e)
        {
            Presenter.DisplayItemList(CmbServiceType.Text);
        }

        private void DgvItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex - 1;
            colIndex = colIndex < 0 ? 0 : colIndex;

            string columnName = DgvItem.Columns[colIndex].Name;
            int index = DgvItem.Focus() ? Convert.ToInt32(DgvItem.CurrentRow.Cells["Index"].Value) : Convert.ToInt32(DgvPreviewItem.CurrentRow.Cells["Index"].Value);

            switch (columnName)
            {
                case "Warehouse":

                    Presenter.GetWarehouse(colIndex, index);
                    break;

                case "Tax Code":

                    Presenter.GetTaxCode(e);
                    break;

                case "Department":

                    Presenter.GetDepartment(colIndex, index);
                    break;

                case "Chain Description":

                    Presenter.GetChain(colIndex, index);
                    break;

                case "UoM":

                    Presenter.GetUom(colIndex, index);
                    break;

                case "Project":

                    Presenter.GetProject(colIndex, index);
                    break;

                case "G/L Account Name":

                    Presenter.GetGlAccount(colIndex, index);
                    break;
            }
        }

        private void DgvItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Presenter.CurrentRowComputation(e);

            DataGridView dgv = (DataGridView)sender;
            var row = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            DataGridViewColumn cell = dgv.Columns[e.ColumnIndex];

            if (cell.Name.Equals("Unit Price") ||
                cell.Name.Equals("Disc Amount") ||
                cell.Name.Equals("Total(LC)") ||
                cell.Name.Equals("Gross Total (LC)") ||
                cell.Name.Equals("Unit Price per piece") ||
                cell.Name.Equals("Gross Price per piece") ||
                cell.Name.Equals("Gross Price"))
            {
                if (IsNumeric(row))
                {
                    double sNumber = double.Parse(row.ToString());
                    string ret = string.Format("{0:#,0.00}", Math.Round(sNumber, 2));
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ret;
                }
            }
        }

        bool IsNumeric(object e)
        {
            bool result = false;
            if (e != null)
            { result = double.TryParse(e.ToString(), out double n); }

            return result;
        }

        private void BtnRequest_Click(object sender, EventArgs e)
        {
            var trans = new object() ;
            foreach (DataGridViewRow row in DgvUdf.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals("Trans. Category"))
                {
                    trans = DgvUdf.Rows[row.Index].Cells["Field"].Value;
                    break;
                }
            }
            var isZeroQuantity = PurchasingModel.GRPOdocument.Where(x => x.Quantity == 0).Any();
            if (TxtVendorCode.Text != string.Empty && trans != null && isZeroQuantity is false)
            {
                BtnRequest.Enabled = false;
                if (Presenter.ExecuteRequest(BtnRequest.Text))
                {
                    BtnRequest.Text = BtnRequest.Text == "Update" ? "Add" : "Add";
                    EnableControls();
                }
                BtnRequest.Enabled = true;
            }
            else
            {
                var msg = trans == null ? "Select Transaction Category" : isZeroQuantity ? "Please Input Quantity" : "Enter Bp Code";
                StaticHelper._MainForm.ShowMessage($"Warning: {msg}", true);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BtnCopyFrom_Click(object sender, EventArgs e)
        {
            CmbCopyFromOption.DroppedDown = CmbCopyFromOption.DroppedDown == false ? true : false;
        }

      //need to change to Purchase Order
        private void TabPO_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TabPO.SelectedIndex)
            {
                case 0:
                    Presenter.LoadData(DgvItem);

                    if (CmbServiceType.Text == "Service" && BtnRequest.Text == "Update")
                    {
                        DgvItem.Rows.RemoveAt(0);
                    }
                    break;

                case 1:

                    Presenter.LoadData(DgvPreviewItem);

                    if (CmbServiceType.Text == "Service" && BtnRequest.Text == "Update")
                    {
                        DgvPreviewItem.Rows.RemoveAt(0);
                    }
                    break;

                case 2:
                    Presenter.GetExistingDocument(DgvFindDocument);
                    CurrentDataSet = Presenter.ListDocuments();
                    CmbFilterDocument.Text = "All";
                    break;

                case 3:
                    if (TxtDocEntry.Text == string.Empty)
                    {
                        StaticHelper._MainForm.ShowMessage("Warning: Please select document first",true);
                    }
                    else
                    {
                        TabPO.SelectedIndex = 3;
                        TxtPrintDocNo.Text = TxtDocEntry.Text;
                    }
                    break;

                default:
                    break;
            }
        }

        private void DgvFindDocument_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvFindDocument.CurrentRow.Selected = true;
        }

        private void BtnNewDocument_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Presenter.ClearField(true);
                TabPO.SelectedIndex = 0;
                BtnRequest.Text = "Add";
                EnableControls();
            }
        }

        private void SelectDocument_Event(object sender, DataGridViewCellEventArgs e)
        {
            ChooseDocument();
        }

        private void SelectDocument_Event(object sender, EventArgs e)
        {
            ChooseDocument();
        }

        private void ChooseDocument()
        {
            var itemNo = DgvFindDocument.CurrentRow.Cells[0].Value;

            if (itemNo != null)
            {
                string status = DgvFindDocument.CurrentRow.Cells[1].Value.ToString();
                Presenter.ClearField(true);
                Presenter.GetSelectedDocument("PDN", itemNo.ToString(), status);

                TabPO.SelectedIndex = 0;

                BtnRequest.Text = "Update";
                DisableControls();
                if (CmbServiceType.Text == "Service")
                {

                    var qwe = DgvItem.Rows[0].Cells["G/L Account"].Value;

                    if (qwe == null)
                    {
                        DgvItem.Rows.RemoveAt(0);
                    }
                }
            }
        }

        void DisableControls()
        {
            PbSuppCode.Visible = false;
            PbDepartment.Visible = false;
            BtnItem.Enabled = false;
            DgvItem.Enabled = false;
            CmbServiceType.Enabled = false;
        }

        void EnableControls()
        {
            PbSuppCode.Visible = true;
            PbDepartment.Visible = true;
            BtnItem.Enabled = true;
            DgvItem.Enabled = true;
            CmbServiceType.Enabled = true;
        }

        private void DgvUdf_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Presenter.UdfRequest();
            //TxtDocEntry.Focus();
        }

        private void DgvItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Presenter.DeleteItem(e.RowIndex, e.ColumnIndex);
            }
        }

        private void DgvPreviewItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Presenter.DeleteItem(e.RowIndex, e.ColumnIndex);
            }
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.ExecuteDeleteItem();
        }

        private void TxtDiscountInput_Leave(object sender, EventArgs e)
        {
            Presenter.ComputeInputDiscount();
        }

        private void TxtDiscAmount_TextChanged(object sender, EventArgs e)
        {
            Presenter.ComputeAmountDiscount();
        }

        private void CmbCopyFromOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtVendorCode.Text))
            {
                StaticHelper._MainForm.ShowMessage("Please fillup supplier first.",true);
            }
            else
            {
                Presenter.ExecuteCopyDocument(CmbCopyFromOption.Text);
                DisableControls();
            }
        }
        
        private void Dgv_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var table = DgvPreviewItem.Focus() ? DgvPreviewItem : DgvItem;
            
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            {
                string clipBoard = Clipboard.GetText();

                foreach (DataGridViewCell cell in table.SelectedCells)
                {
                    cell.Value = clipBoard;

                    Presenter.CurrentRowComputation(null, cell.RowIndex);
                }
            }
        }

        private void CmbFilterDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            DgvFindDocument.DataSource = null;
            
            try
            {
                switch (CmbFilterDocument.Text)
                {
                    case "All":
                        DgvFindDocument.DataSource = CurrentDataSet;
                        break;

                    case "Open":
                        var dt = CurrentDataSet.Select("Status = 'Open'").CopyToDataTable();
                        DgvFindDocument.DataSource = dt;
                        break;

                    case "Canceled":
                        var dtc = CurrentDataSet.Select("Status = 'Canceled'").CopyToDataTable();
                        DgvFindDocument.DataSource = dtc;
                        break;

                    case "Closed":
                        var dtd = CurrentDataSet.Select("Status = 'Closed'").CopyToDataTable();
                        DgvFindDocument.DataSource = dtd;
                        break;
                        
                    case "Draft Pending":

                        if (!Presenter.CheckUser())
                        {
                            var dt1 = CurrentDataSet.Select("Status = 'Draft-Pending'").CopyToDataTable();
                            DgvFindDocument.DataSource = dt1;
                        }
                        break;

                    case "Draft Approved":
                        if (!Presenter.CheckUser())
                        {
                            var dt2 = CurrentDataSet.Select("Status = 'Draft-Approved'").CopyToDataTable();
                            DgvFindDocument.DataSource = dt2;
                        }
                        break;
                }
            }
            catch
            {
                DgvFindDocument.DataSource = null;
            }
        }

        private void DtFrom_Leave(object sender, EventArgs e)
        {
            var dt2 = CurrentDataSet.Select("Status = 'D-A'").CopyToDataTable();
            DgvFindDocument.DataSource = dt2;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (TxtDocEntry.Text == string.Empty)
            {
                StaticHelper._MainForm.ShowMessage("Warning: Please select document first",true);
            }
            else
            {
                TabPO.SelectedIndex = 3;
                TxtPrintDocNo.Text = TxtDocEntry.Text;
            }
        }

        private void CmbPrintPreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ReportDocument cryRpt = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                var sboCred = new SboCredentials();
                //string path = $"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\PO\\{CmbPrintPreview.Text}";
                var _settingsService = new SettingsService();

                string path = $"{_settingsService.GetReportPath()}Purchasing\\{CmbPrintPreview.Text}";

                cryRpt.Load(path);
                cryRpt.SetParameterValue("DocKey@", TxtDocEntry.Text);
                cryRpt.SetParameterValue("UserCode@", sboCred.UserId);

                string constring = $"DRIVER=HDBODBC32;SERVERNODE=HANASERVERNBFI:30015;DATABASE={SboCred.Database}";

                //crConnectionInfo.IntegratedSecurity = false;

                //#############################################################################
                //Added by Cedi 070119
                var logonProperties = new DbConnectionAttributes();
                logonProperties.Collection.Set("Connection String", @"DRIVER={HDBODBC32};SERVERNODE=" + sboCred.DbServer + "; UID=" + sboCred.DbUserId + ";PWD=" + sboCred.DbPassword + ";CS=" + sboCred.Database + "; ");
                //logonProperties.Collection.Set("Connection String", @"DRIVER ={B1CRHPROXY32}; SERVERNODE = HANASERVERNBFI:30015; DATABASE = " + sboCred.Database + "");
                logonProperties.Collection.Set("UseDSNProperties", false);
                var connectionAttributes = new DbConnectionAttributes();
                connectionAttributes.Collection.Set("Database DLL", "crdb_odbc.dll");
                connectionAttributes.Collection.Set("QE_DatabaseName", String.Empty);
                connectionAttributes.Collection.Set("QE_DatabaseType", "ODBC (RDO)");
                connectionAttributes.Collection.Set("QE_LogonProperties", logonProperties);
                connectionAttributes.Collection.Set("QE_ServerDescription", sboCred.DbServer);
                connectionAttributes.Collection.Set("QE_SQLDB", false);
                connectionAttributes.Collection.Set("SSO Enabled", false);
                //#############################################################################

                //crConnectionInfo.ServerName = sboCred.DbServer;
                //crConnectionInfo.DatabaseName = sboCred.Database;
                //crConnectionInfo.UserID = sboCred.DbUserId;
                //crConnectionInfo.Password = sboCred.DbPassword;

                crConnectionInfo.Attributes = connectionAttributes;
                crConnectionInfo.Type = ConnectionInfoType.CRQE;
                crConnectionInfo.IntegratedSecurity = false;
                //crConnectionInfo.Type = ConnectionInfoType.SQL;

                foreach (Table CrTable in cryRpt.Database.Tables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;

                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                crystalReportViewer1.ReportSource = cryRpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtSearchDocument_TextChanged(object sender, EventArgs e)
        {
            DataRepository.RowSearch(DgvFindDocument, TxtSearchDocument.Text, findDocSearch);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            DataRepository.RowSearch(DgvPreviewItem, TxtSearch.Text, 0);
        }

        private void FrmPurchaseOrder_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void DgvItem_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (CmbServiceType.Text == "Service")
            {
                //Presenter.NewRowGetDefaultValue();
            }
        }

        private void DgvItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rowCount = DgvItem.RowCount;
            int currentRow = e.RowIndex + 1;

            if (rowCount == currentRow)
            {
                DgvItem.Rows[e.RowIndex].ReadOnly = true;
            }
        }

        private void DgvItem_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(DgvItem.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }
        
        private void DgvUdf_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DgvUdf.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                DgvUdf.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvUdf_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Presenter.AutomateGlAccountName();
        }

        private void DgvPreviewItem_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(DgvPreviewItem.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void DgvFindDocument_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           findDocSearch = e.ColumnIndex;
        }

        private void pbSuppRef_Click(object sender, EventArgs e)
        {
            if (DECLARE.udf.Exists(x => x.ObjCode == "OPDN" && x.FieldCode == "U_DRNo"))
            {
                txtRefNo.Text = DECLARE.udf.Find(x => x.ObjCode == "OPDN" && x.FieldCode == "U_DRNo").FieldValue.ToString();
            }
        }

        private void DgvItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (CmbServiceType.Text == "Item")
            {
                e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
                if (DgvItem.CurrentCell.ColumnIndex == 19 || DgvItem.CurrentCell.ColumnIndex == 8 ||
                    DgvItem.CurrentCell.ColumnIndex == 18 || DgvItem.CurrentCell.ColumnIndex == 24) //Desired Column
                {
                    TextBox tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    }
                }
            }
            else
            {
                e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
                if (DgvItem.CurrentCell.ColumnIndex == 8 || DgvItem.CurrentCell.ColumnIndex == 16 ||
                    DgvItem.CurrentCell.ColumnIndex == 21 || DgvItem.CurrentCell.ColumnIndex == 22 ||
                    DgvItem.CurrentCell.ColumnIndex == 23 || DgvItem.CurrentCell.ColumnIndex == 27 ||
                    DgvItem.CurrentCell.ColumnIndex == 28 || DgvItem.CurrentCell.ColumnIndex == 29) //Desired Column
                {
                    TextBox tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    }
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true;
            //}
            if (DgvItem.CurrentCell.ColumnIndex != 8)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
            }
            else if (DgvItem.CurrentCell.ColumnIndex == 8)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        private void txtRefNo_Leave(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DgvUdf.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals("DR No."))
                {
                    DgvUdf.Rows[row.Index].Cells["Field"].Value = txtRefNo.Text;
                    break;
                }
            }
        }

        private void DgvUdf_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (DgvUdf.CurrentRow.Cells[0].Value.ToString() == "U_DRNo")
                {
                    txtRefNo.Text = DgvUdf.CurrentRow.Cells["Field"].Value.ToString();
                }
            }
            catch
            {

            }
        }

        private void FrmGoodsReceiptPO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close this document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) != DialogResult.Yes)
            { e.Cancel = true; }
            else { Dispose(); }
        }
    }
}
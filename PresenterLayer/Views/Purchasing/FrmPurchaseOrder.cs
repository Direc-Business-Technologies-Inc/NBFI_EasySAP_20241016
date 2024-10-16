using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DirecLayer._02_Form.MVP.Presenters;
using DirecLayer._02_Form.MVP.Presenters.PriceTag;
using DomainLayer;
using DomainLayer.Helper;
using MetroFramework;
using MetroFramework.Forms;
using PresenterLayer;
using PresenterLayer.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PresenterLayer.Services.Security;

//using EasySAP._01_Models.Context;

namespace DirecLayer._02_Form.MVP.Views
{
    public partial class FrmPurchaseOrder : MetroForm, IPurchaseOrder
    {
        public FrmPurchaseOrder()
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
            get => TxtDocNum.Text;
            set => TxtDocNum.Text = value;
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
            get => TxtCancellationDate.Text;
            set => TxtCancellationDate.Text = value;
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

        public PurchaseOrderPresenter Presenter
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
                case Keys.Escape:
                    Close();
                    break;
                case Keys.Control | Keys.I:
                    Presenter.DisplayItemList(CmbServiceType.Text);
                    break;

                //case Keys.Control | Keys.A:

                //    if (TxtVendorCode.Text != string.Empty)
                //    {
                //        if (Presenter.ExecuteRequest(BtnRequest.Text))
                //        {
                //            BtnRequest.Text = BtnRequest.Text == "Update" ? "Add" : "Add";
                //        }
                //    }
                //    else
                //    {
                //        PublicStatic.frmMain.NotiMsg("Warning: Enter BP Code", Color.Red);
                //    }

                //    break;

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
            var r = Presenter.GetCompany();
            CmbCompany.DataSource = Presenter.GetCompany();

            CmbSeries.DisplayMember = "Name";
            CmbSeries.ValueMember = "Code";
            CmbSeries.DataSource = Presenter.GetDocumentSeries();

            CmbServiceType.Text = "Item";

            var files = Presenter.GetDocumentCrystalForms();

            TxtPostingDate.Text = DtPostingDate.Value.ToShortDateString();
            TxtDocumentDate.Text = DtDocumentDate.Value.ToShortDateString();

            //NNED TO ADD COMMENT BECAUSE NAG E ERROR WHILE DEVELOPING - Andric
            if (files!=null)
            {

                foreach (FileInfo file in files)
                {
                    CmbPrintPreview.Items.Add(file.Name);
                }

            }

            var currs = Presenter.GetCurrencies();
            CmbCurrency.Items.Clear();
            foreach (DataRow curr in currs.Rows)
            {
                CmbCurrency.Items.Add(curr[0].ToString());
            }

            DgvFindDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (DataGridViewRow row in DgvUdf.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals("Prepared By"))
                {
                    DgvUdf.Rows[row.Index].Cells["Field"].Value = DomainLayer.Models.EasySAPCredentialsModel.EmployeeCompleteName;
                    DgvUdf.Rows[row.Index].Cells["Field"].ReadOnly = true;
                    break;
                }
            }
            //DgvUdf.Rows[4].Cells["Field"].Value = DomainLayer.Models.EasySAPCredentialsModel.EmployeeCompleteName;
            //DgvUdf.Rows[4].Cells["Field"].ReadOnly = true;
            DgvUdf.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.Resizable = DataGridViewTriState.False);

        }

        private void PbSuppCode_Click(object sender, EventArgs e)
        {
            Presenter.GetSupplierInformation();

            CmbCurrency.Enabled = RawCurrency == "##" ? true : false;

            //using (var db = new LSContext())
            //{
            //    var qwe = db.items.ToList();
            //}
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

        private void DtCancellationDate_ValueChanged(object sender, EventArgs e)
        {
            TxtCancellationDate.Text = DtCancellationDate.Value.ToShortDateString();
        }

        private void DtCancellationDate_Enter(object sender, EventArgs e)
        {
            TxtCancellationDate.Text = DtCancellationDate.Value.ToShortDateString();
        }

        private void BtnItem_Click(object sender, EventArgs e)
        {
            Presenter.DisplayItemList(CmbServiceType.Text);
        }

        List<string> GetJobOrderGL()
        {
            var dt = DataRepositoryForInvoice.GetData("SELECT AcctCode [Code], AcctName [Name] FROM OACT Where Postable = 'Y' AND AcctCode = '200-197'");

            List<string> result = new List<string>();

            if (dt == null)
            {
                result.Add("");
                result.Add("");

                return result;
            }
            else if (dt.Rows.Count <= 0)
            {
                result.Add("");
                result.Add("");

                return result;
            }

            result.Add(ValidateInput.String(dt.Rows[0][0]));
            result.Add(ValidateInput.String(dt.Rows[0][1]));

            return result;
        }

        private void DgvItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex - 1;
            colIndex = colIndex < 0 ? 0 : colIndex;

            string columnName = DgvItem.Columns[colIndex].Name;
            //int index = DgvItem.Focus() ? Convert.ToInt32(DgvItem.CurrentRow.Cells["Index"].Value) : Convert.ToInt32(DgvPreviewItem.CurrentRow.Cells["Index"].Value);

            int CurrentRowIndex = DgvItem.CurrentRow.Cells["Index"].Value == null ? -1 : Convert.ToInt32((string.IsNullOrEmpty(DgvItem.CurrentRow.Cells["Index"].Value.ToString()) ? -1 : DgvItem.CurrentRow.Cells["Index"].Value));
            int intGenIndex = PurchasingAP_generics.index + 1;
            int index = CurrentRowIndex;
            if (columnName == "G/L Account Name") 
            { 
                index = (CmbServiceType.SelectedItem.ToString() == "Service" && DgvItem.Rows.Count == 2 && CurrentRowIndex == 0) ? CurrentRowIndex : (DgvItem.Rows.Count > intGenIndex ? intGenIndex : PurchasingAP_generics.index); 
            };
            index = index == CurrentRowIndex ? CurrentRowIndex : index;
            //Check on this
            switch (columnName)
            {
                case "Warehouse":
                    if (Service == "Service" && (DgvItem.CurrentRow.Cells["Item No."].Value == null || string.IsNullOrEmpty(DgvItem.CurrentRow.Cells["Item No."].Value.ToString())))
                    {
                        break;
                    }
                    Presenter.GetWarehouse(colIndex, index);

                    if (Service == "Service" )
                    {
                        var isJobOrder = false;
                        foreach (DataGridViewRow dgvRow in DgvUdf.Rows)
                        {
                            if (ValidateInput.String(dgvRow.Cells[0].Value) == "U_TransactionType" && ValidateInput.String(dgvRow.Cells[2].Value) == "Job Order")
                            {
                                var val = GetJobOrderGL();

                                for (int i = 0; i < DgvItem.RowCount - 1; i++)
                                {
                                    DgvItem.Rows[i].Cells["G/L Account"].Value = val[0];
                                    DgvItem.Rows[i].Cells["G/L Account Name"].Value = val[1];

                                    int indx = ValidateInput.Int(DgvItem.Rows[0].Cells["Index"].Value);

                                    PurchasingModel.PurchaseOrderDocument.Where(a => a.Index == index)
                                    .ToList().ForEach(a =>
                                    {
                                        a.GLAccount = val[0];
                                        a.GLAccountName = val[1];
                                    });
                                }
                                isJobOrder = true;
                                break;
                            }
                        }

                        if (!isJobOrder)
                        {
                            var dt = Presenter.GLwarehouse(DgvItem.CurrentRow.Cells["Item No."].Value.ToString(), DgvItem.CurrentRow.Cells["Warehouse"].Value.ToString());
                            int indexx = Convert.ToInt32(DgvItem.CurrentRow.Cells["Index"].Value);
                            var item = PurchasingModel.PurchaseOrderDocument.Find(x => x.Index == index);
                            item.GLAccount = dt.Rows.Count == 0 ? item.GLAccount : dt.Rows[0].ItemArray[0].ToString();

                            item.GLAccountName = dt.Rows.Count == 0 ? item.GLAccountName : dt.Rows[0].ItemArray[1].ToString();
                            //Presenter.LoadData(DgvItem);
                            Presenter.LoadData(DgvItem, true);
                        }

                    }
                    break;

                case "Tax Code":

                    Presenter.GetTaxCode(e);
                    break;

                case "Department":

                    //Presenter.GetDepartment(colIndex, index);
                    break;

                case "Chain Description":

                    Presenter.GetChain(colIndex, index);
                    break;

                case "UoM":

                    //Presenter.GetUom(colIndex, index);
                    if (DgvItem.CurrentRow.Cells["Item No."].Value != null && string.IsNullOrEmpty(DgvItem.CurrentRow.Cells["Item No."].Value.ToString()) == false)
                    {
                        Presenter.GetUom(colIndex, index, DgvItem.CurrentRow.Cells["Item No."].Value.ToString());
                    }

                    break;

                case "Project":

                    Presenter.GetProject(colIndex, index);
                    break;

                case "G/L Account Name":

                    Presenter.GetGlAccount(colIndex, index);

                    //DgvItem.CurrentRow.Cells["G/L Account"].Value = dt.Rows.Count == 0 ? DgvItem.CurrentRow.Cells["G/L Account"].Value.ToString() : dt.Rows[0].ItemArray[0].ToString();


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

            if (cell.Name.Equals("Tax Code"))
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = row.ToString();
            }

            //Presenter.LoadData(DgvPreviewItem.Focus() ? DgvPreviewItem : DgvItem);
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
            try
            {
                var isZeroQuantity = PurchasingModel.PurchaseOrderDocument.Where(x => x.Quantity == 0).Any();
                //MessageBox.Show("Try");

                //if (TxtVendorCode.Text != string.Empty && DgvUdf.Rows[0].Cells["Field"].Value != null && isZeroQuantity is false)
                if (TxtVendorCode.Text != string.Empty && isZeroQuantity is false)
                {
                   // MessageBox.Show("If - TxtVendorCode.Text");
                    BtnRequest.Enabled = false;
                    if (Presenter.ExecuteRequest(BtnRequest.Text))
                    {

                        BtnRequest.Text = BtnRequest.Text == "Update" ? "Add" : "Add";
                       // MessageBox.Show("If - TxtVendorCode.Text");

                    }
                    BtnRequest.Enabled = true;

                    //MessageBox.Show("after if Presenter");
                }
                else
                {
                    //MessageBox.Show("else - TxtVendorCode.Text");
                    //var msg = DgvUdf.Rows[0].Cells["Field"].Value.ToString() == string.Empty ? "Select Transaction Category" : isZeroQuantity ? "Please Input Quantity" : "Enter Bp Code";
                    var msg = isZeroQuantity ? "Please Input Quantity" : "Enter Bp Code";
                    StaticHelper._MainForm.ShowMessage($"Warning: {msg}", true);
                }

                //MessageBox.Show("Done sa Try");
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                BtnRequest.Enabled = true;
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

        private void TabPO_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TabPO.SelectedIndex)
            {
                case 0:

                    FindMode = BtnRequest.Text == "Update" ? true : false;
                    Presenter.LoadData(DgvItem, FindMode);

                    if (CmbServiceType.Text == "Service" && BtnRequest.Text == "Update")
                    {
                        //DgvItem.Rows.RemoveAt(0);
                    }
                    break;

                case 1:

                    Presenter.LoadData(DgvPreviewItem);

                    if (CmbServiceType.Text == "Service" && BtnRequest.Text == "Update")
                    {
                        //DgvPreviewItem.Rows.RemoveAt(0);
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
                        //PublicStatic.frmMain.NotiMsg("Warning: Please select document first", Color.Red);
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
                Presenter.GetSelectedDocument(table, itemNo.ToString(), status);

                BtnRequest.Text = "Update";
                TabPO.SelectedIndex = 0;

                if (CmbServiceType.Text == "Service")
                {
                    var GLval = DgvItem.Rows[0].Cells["G/L Account"].Value;

                    if (GLval == null)
                    {
                        DgvItem.Rows.RemoveAt(0);
                    }
                }
            }
        }

        private void DgvUdf_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Presenter.UdfRequest();
            TxtDocEntry.Focus();
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
            Presenter.ExecuteCopyDocument(CmbCopyFromOption.Text);
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
                StaticHelper._MainForm.ShowMessage("Warning: Please select document first", true);
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
                var cryRpt = new ReportDocument();
                var crtableLogoninfos = new TableLogOnInfos();
                var crtableLogoninfo = new TableLogOnInfo();
                var crConnectionInfo = new ConnectionInfo();
                var sboCred = new SboCredentials();

               
                //string path = $"\\\\HANASERVERNBFI\\b1_shf\\AttachmentsPath\\Extensions\\PO\\{CmbPrintPreview.Text}";
                var _settingsService = new SettingsService();

                string path = $"{_settingsService.GetReportPath()}Purchasing\\{CmbPrintPreview.Text}";
   
                cryRpt.Load(path);
                cryRpt.SetParameterValue("DocKey@", TxtDocEntry.Text);
                cryRpt.SetParameterValue("UserCode@", sboCred.UserId);
                
                //string constring = $"DRIVER=HDBODBC32;SERVERNODE=HANASERVERNBFI:30015;DATABASE={SboCred.Database}";
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

        private void btnBarcode_Click(object sender, EventArgs e)
        {
            PriceTag.ImageFile(DgvItem, TxtVendorCode.Text, TxtDeliveryDate.Text, DocEntry);
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

        private void TxtDiscountInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void DgvItem_KeyPress(object sender, KeyPressEventArgs e)
        {

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


        private void FrmPurchaseOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close this Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Presenter.ClearField(true);
                Dispose();
            }
            else
            { e.Cancel = true; }
        }

        private void DgvItem_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int CurrentRowIndex = DgvItem.CurrentRow.Cells["Index"].Value == null ? -1 : Convert.ToInt32((string.IsNullOrEmpty(DgvItem.CurrentRow.Cells["Index"].Value.ToString()) ? -1 : DgvItem.CurrentRow.Cells["Index"].Value));
            int index = CurrentRowIndex;
            index = index == CurrentRowIndex ? CurrentRowIndex : index;

            int colIndex = e.ColumnIndex;
            colIndex = colIndex < 0 ? 0 : colIndex;

            string columnName = DgvItem.Columns[colIndex].Name;
            
            switch (columnName)
            {
                case "Item Description":
                    PurchasingModel.PurchaseOrderDocument.Find(x => x.Index == index).ItemDescription = ValidateInput.String(DgvItem.CurrentRow.Cells[columnName].Value);
                    break;
                default:
                    break;
            }
        }
    }
}
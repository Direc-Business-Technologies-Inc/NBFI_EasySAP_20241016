using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using MetroFramework;
using DirecLayer;
using PresenterLayer.Services;
using PresenterLayer.Views.Main;

namespace PresenterLayer.Views
{
    public partial class FrmCartonManagement : MetroForm, ICartonManagement
    {

        private readonly string DocumentEntry = "";

        private readonly string DocumentEntryList = "";

        private bool _isOnSearch = false;
        
        public FrmCartonManagement(bool isOnSearch, string _docEntryList)
        {
            InitializeComponent();

            _isOnSearch = isOnSearch;

            DocumentEntryList = _docEntryList;
        }

        public FrmCartonManagement(string _docEntry)
        {
            InitializeComponent();

            DocumentEntry = _docEntry;

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            { Close(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public List<string> DocumentEntries = new List<string>();

        private string tableName = string.Empty;

        private int ColumnIndex = 0;

        private int ColumnIndex2 = 0;

        public static int ItemIndex { get; set; }

        public string DocEntry
        {
            get => TxtDocEntry.Text; set => TxtDocEntry.Text = value;
        }

        public string DocNo
        {
            get => TxtDocNum.Text; set => TxtDocNum.Text = value;
        }

        public string CartonNo
        {
            get => TxtCartonNumber.Text; set => TxtCartonNumber.Text = value;
        }

        public string SuppCode
        {
            get => TxtVendorCode.Text; set => TxtVendorCode.Text = value;
        }

        public string SuppName
        {
            get => TxtVendorName.Text; set => TxtVendorName.Text = value;
        }

        public string Chain
        {
            get => TxtChainName.Text; set => TxtChainName.Text = value;
        }

        public string GroupCode
        {
            get => TxtGroupCode.Text; set => TxtGroupCode.Text = value;
        }

        public string DocRef
        {
            get => TxtDocRef.Text; set => TxtDocRef.Text = value;
        }

        public string OrderNo
        {
            get => TxtRef2.Text; set => TxtRef2.Text = value;
        }

        public string ShipmentNo
        {
            get => TxtRef.Text; set => TxtRef.Text = value;
        }

        public string TransType
        {
            get => TxtTransactionType.Text; set => TxtTransactionType.Text = value;
        }

        public string TransferType
        {
            get => CmbTransferType.Text; set => CmbTransferType.Text = value;
        }

        public string TargetWhse
        {
            get => TxtTargetWH.Text; set => TxtTargetWH.Text = value;
        }

        public string LastWhse
        {
            get => TxtLastWH.Text; set => TxtLastWH.Text = value;
        }

        public string Status
        {
            get => CmbStatus.Text; set => CmbStatus.Text = value;
        }

        public string DateChecked
        {
            get => TxtDateChecked.Text;
            set => TxtDateChecked.Text = value;
        }

        public DateTimePicker DTPDateChecked
        {
            get => DtCheck;
            set => DtCheck = value;
        }

        public string BasedDocEntry
        {
            get => TxtBasedDoEntry.Text; set => TxtBasedDoEntry.Text = value;
        }

        public string Remarks
        {
            get => TxtRemarks.Text; set => TxtRemarks.Text = value;
        }

        public DataGridView Table
        {
            get => DgvItem;
        }

        public FrmCartonManagement Form
        {
            get => this;
        }

        public ContextMenuStrip MouseSelect
        {
            get => msItems;
        }

        public DataTable TableFindDocument
        {
            set => DgvFindDocument.DataSource = value;
        }

        public CartonManagementPresenter Presenter
        {
            private get;
            set;
        }
        public string TotalQuantity
        {
            get => TxtQty.Text; set => TxtQty.Text = value;
        }
        public string BasedDocument
        {
            get
            {
                string tableName = "";

                switch (CmbBasedDocument.Text)
                {
                    case "ITR":
                        tableName = "WTQ";
                        break;

                    case "IT":
                        tableName = "WTR";
                        break;

                    default:
                        tableName = "PDN";
                        break;
                }

                return tableName;
            }
            set => CmbBasedDocument.Text = value;
        }

        private void FrmCartonManagement_Load(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Maximized;
            //DgvFindDocument.MultiSelect = true;

            if (DocumentEntry == null || DocumentEntry == string.Empty)
            {
                if (!_isOnSearch)
                {

                    CmbStatus.DisplayMember = "Value";
                    CmbStatus.DataSource = Presenter.StatusDataSource();

                    if (DocumentEntry != string.Empty)
                    {
                        if (Presenter.GetDocument(DocumentEntry))
                        {
                            TabPreviewItem.SelectedIndex = 0;

                            BtnRequest.Text = "Update";
                        }
                    }
                }
                else
                {
                    TabPreviewItem.TabPages.Remove(TabNewDocs);
                    TabPreviewItem.TabPages.Remove(TabPreviewItems);

                    Presenter.GetExistingDocument(DocumentEntryList);
                }
            }
            else
            {
                TabPreviewItem.TabPages.Remove(TabFindDocument);

                if (Presenter.GetDocument(DocumentEntry))
                {
                    BtnRequest.Text = "Update";
                }
            }

            CmbTransferType.DisplayMember = "Name";
            CmbTransferType.DataSource = Presenter.GetTransferType();
        }

        private void PbSuppCode_Click(object sender, EventArgs e)
        {
            Presenter.GetSupplier();
        }

        private void PbChain_Click(object sender, EventArgs e)
        {
            Presenter.GetChain();
        }

        private void PbTransType_Click(object sender, EventArgs e)
        {
            Presenter.GetTransactionType();
        }

        private void PbTargetWhs_Click(object sender, EventArgs e)
        {
            Presenter.GetTargetWarehouse();
        }

        private void PbLastWhs_Click(object sender, EventArgs e)
        {
            Presenter.GetLastWarehouse();
        }

        private void DtCheck_ValueChanged(object sender, EventArgs e)
        {
            TxtDateChecked.Text = DtCheck.Value.ToShortDateString();
            DtCheck.CustomFormat = " ";
            DtCheck.Format = DateTimePickerFormat.Custom;
            DtCheck.Format = DateTimePickerFormat.Short;
            DtCheck.Select();
        }

        private void CmbBasedDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (TxtVendorCode.Text != string.Empty)
            //{
            //    if (Presenter.GetBasedDocumentEntry() == false)
            //    {
            //        tableName = string.Empty;
            //    }
            //    else
            //    {
            //        tableName = BasedDocument;
            //    }
            //}
            //else
            //{
            //   StaticHelper._MainForm.ShowMessage("Warning: No selected Business Partner",true);
            //}
            if (Presenter.GetBasedDocumentEntry() == false)
            {
                tableName = string.Empty;
            }
            else
            {
                tableName = BasedDocument;
            }
        }

        private void BtnItem_Click(object sender, EventArgs e)
        {
            Presenter.GetItems(tableName);
        }

        private void BtnRequest_Click(object sender, EventArgs e)
        {
            BtnRequest.Enabled = false;
            if (string.IsNullOrEmpty(TxtDateChecked.Text))
            {
                StaticHelper._MainForm.ShowMessage("Warning: Please set the date checked.", true);
                BtnRequest.Enabled = true;
                return;
            }
            if (Presenter.ExecuteRequest(BtnRequest.Text))
            {
                if (BtnRequest.Text == "Update")
                {
                    BtnRequest.Text = "Add";
                    CmbBasedDocument.Text = "";
                    TxtBasedDoEntry.Text = "";
                }
            }
            BtnRequest.Enabled = true;
        }

        private void TabPreviewItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabPreviewItem.SelectedIndex == 0)
            {
                Presenter.DataGridViewColumns(DgvItem);
                Presenter.LoadItems(DgvItem);
            }
            else if (TabPreviewItem.SelectedIndex == 1)
            {
                Presenter.DataGridViewColumns(DgvPreviewItem);
                Presenter.LoadItems(DgvPreviewItem);
            }
            else
            {
                Presenter.GetExistingDocument("");
            }
        }

        private void DgvPreviewItem_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex = e.ColumnIndex;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            DataRepository.RowSearch(DgvPreviewItem, TxtSearch.Text, ColumnIndex);
        }

        private void DgvPreviewItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                    Presenter.DeleteItem(DgvPreviewItem, e.ColumnIndex);
            }
        }

        private void DgvItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                    Presenter.DeleteItem(DgvItem, e.ColumnIndex);
            }
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dgv = DgvPreviewItem.Focus() ? DgvPreviewItem : DgvItem;

            Presenter.CommitDelete(dgv);
        }

        private void DgvFindDocument_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnIndex2 = e.ColumnIndex;
        }

        private void TxtSearchDocument_TextChanged(object sender, EventArgs e)
        {
            DataRepository.RowSearch(DgvFindDocument, TxtSearchDocument.Text, ColumnIndex2);
        }

        private void BtnNewDocument_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Presenter.ClearField();
                TabPreviewItem.SelectedIndex = 0;
                BtnRequest.Text = "Add";
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void DgvFindDocument_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvFindDocument.CurrentRow.Selected = true;
        }

        private void BtnChoose_Click(object sender, EventArgs e)
        {
            EnterSelectedRows();
        }

        private void DgvFindDocument_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EnterSelectedRows();
            }
        }

        private void EnterSelectedRows()
        {
            if (!_isOnSearch)
            {
                Presenter.ClearField();
                if (DgvFindDocument.CurrentRow.Cells[0].Value != null)
                {
                    var docEntry = DgvFindDocument.CurrentRow.Cells[0].Value.ToString();

                    if (Presenter.GetDocument(docEntry))
                    {
                        TabPreviewItem.SelectedIndex = 0;

                        BtnRequest.Text = "Update";
                    }
                }
            }
            else
            {
                if(DgvFindDocument.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in DgvFindDocument.SelectedRows)
                    {
                        if (row.Cells[0].Value != null)
                        {
                            DocumentEntries.Add(row.Cells[0].Value.ToString());
                        }
                    }
                }
                else
                {
                    var DocNo = DgvFindDocument.CurrentRow.Cells[0].Value;
                    DocumentEntries.Add(DocNo.ToString());
                }
               

                Dispose();
            }
        }

        private void TxtDocEntry_Click(object sender, EventArgs e)
        {
            TxtCartonNumber.Focus();
        }

        private void TxtDocNum_Click(object sender, EventArgs e)
        {
            TxtCartonNumber.Focus();
        }

        private void FrmCartonManagement_Resize(object sender, EventArgs e)
        {
            //this.MdiParent = StaticHelper._MainForm;
            //FormHelper.ResizeForm(this);
        }

        private void FrmCartonManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MetroMessageBox.Show(StaticHelper._MainForm, "Unsaved data will be lost. Continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) != DialogResult.Yes)
            { e.Cancel = true; }
            else { Dispose(); }
        }

        private void DgvFindDocument_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EnterSelectedRows();
        }

        private void DgvItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string sEditField = DgvItem.Columns[e.ColumnIndex].Name;
            if (DgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && (sEditField == "Quantity" || sEditField == "Quantity Per Inner Box"))
            {
                bool CheckIfNoChar = true;
                string sQty = DgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                char[] StrToChar = sQty.ToCharArray();

                foreach (char a in StrToChar)
                {
                    if (!char.IsControl(a) && !char.IsDigit(a))
                    {
                        CheckIfNoChar = false;
                    }
                }

                if (!CheckIfNoChar)
                {
                    DgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "1";
                    StaticHelper._MainForm.ShowMessage("Input format is number only.");
                }
                Presenter.ComputeTotal();
            }
        }

    }
}

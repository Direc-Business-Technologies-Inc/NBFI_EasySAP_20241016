using Context;
using DirecLayer;
using DomainLayer.Helper;
using MetroFramework;
using MetroFramework.Forms;
using PresenterLayer.Helper;
using PresenterLayer.Services.Main;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresenterLayer.Views.Main
{
    public partial class MainForm : MetroForm
    {
        MainService _mainService { get; set; }


        public MainForm()
        {
            InitializeComponent();
            _mainService = new MainService();
            foreach (Control ctl in Controls)
            {
                try
                {
                    MdiClient ctlMDI = (MdiClient)ctl;
                    ctlMDI.BackColor = Color.FromArgb(191, 235, 245);
                }
                catch
                { }
            }
            menuStrip.Enabled = false;
            RegisterObjects();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Q))
            { Close(); }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RegisterObjects()
        {
            IMainForm._MenuStrip = menuStrip;
        }

        public void ShowMessage(string sMessage, bool isError = false)
        {
            StaticHelper.MessageCountdown = 0;
            lblNotification.Text = sMessage;
            lblNotification.BackColor = isError ? Color.Red : Color.Green;
            tmrMsg.Start();
            Application.DoEvents();
        }

        public void Connected()
        {
            try
            {
                var sapHana = new SAPHanaAccess();
                var helper = new DataHelper();
                var sapCred = new SboCredentials();

                var Version = $"{LibraryHelper.AssemblyInfo.Trademark} {LibraryHelper.AssemblyInfo.AssemblyVersion}";
                var dbname = helper.ReadDataRow(sapHana.Get("SELECT CompnyName FROM OADM"), 0, "", 0);
                FormHelper.GetBackground();
                lblConnected.Text = $"CONNECTED TO : {sapCred.Database} - {dbname} {Version}";
                lblConnected.BackColor = Color.FromArgb(191, 235, 245);
            }
            catch (Exception ex)
            { ShowMessage(ex.Message, true); }
        }

        public void Progress(string Message, int Min, int max)
        {
            pbStatus.Minimum = 0;
            pbStatus.Maximum = max;
            lblStatus.Text = Message;
            pbStatus.Value = Min;
            if (Min == max)
            { ProgressClear(); }
            Application.DoEvents();
        }

        public void ProgressClear()
        {
            lblStatus.Text = "";
            pbStatus.Value = 0;
            Application.DoEvents();
        }
        private void tmrTime_Tick(object sender, EventArgs e)
        {
            _mainService.TimeTick();
            lblDate.Text = IMainForm.Date;
            lblTime.Text = IMainForm.Time;
        }

        void ClearProgress()
        {
            lblStatus.Text = null;
            pbStatus.Value = 0;
        }


        void ClearNotification()
        {
            lblNotification.Text = null;
            lblNotification.BackColor = Color.Transparent;
        }

        private void tmrMsg_Tick(object sender, EventArgs e)
        {
            if (_mainService.WaitTenSecond())
            {
                ClearProgress();
                ClearNotification();
                tmrMsg.Stop();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _mainService.FormLoad();
        }

        void closeForm()
        {
            foreach (Form frm in MdiChildren)
            {
                frm.Dispose();
            }
        }

        private void lblNotification_MouseHover(object sender, EventArgs e)
        {
            tmrMsg.Stop();
        }

        private void lblNotification_MouseLeave(object sender, EventArgs e)
        {
            ClearProgress();
            ClearNotification();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (menuStrip.Enabled == true)
            {
                if (MetroMessageBox.Show(this, $"Closing {SystemSettings.Info.Title} will stop all running processes and close all open windows. Do you want to continue?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                {
                    Dispose();
                }
            }
        }

        private void purchaseOrderPrototypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowPurchaseOrder();
        }

        private void goodsReceiptPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowGoodReceipt();
        }

        private void salesOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            _mainService.ShowSalesOrder();
        }

        private void MenuItemItemMasterData_Click(object sender, EventArgs e)
        {
            _mainService.ShowItemMasterData();
        }
        private void inventoryTransferRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowInventoryTransferRequest();
        }

        private void chooseCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ChooseCompany();
        }

        private void allocationWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowAllocationWizard();
        }

        private void salesOrderToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            _mainService.ShowSalesOrder();
        }

        private void inventoryAutoReplenishmentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void inventoryCountingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowInventoryCounting();
        }

        private void inventoryCountingPostingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowInventoryPosting();
        }

        private void concessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowPackingListConcession();
        }

        private void outgoingPackingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowOutgoingPackingList();
        }

        private void outrightToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _mainService.ShowOutright();
        }

        private void concessionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _mainService.ShowConcession();
        }

        private void freezeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowFreezeItem();
        }

        private void aRInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowArInvoice();
        }

        private void unofficialSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowUnofficialSales("Unofficial Sales/Warehouse Sales");
        }

        private void deliveryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _mainService.ShowUnofficialSales("Delivery");
        }

        private void inventoryTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowInventoryTransfer();
        }

        private void cartonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowCartonManagement();
        }

        private void printLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowPrintLayout();
        }

        private void cartonListPrototypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowCartonList();
        }

        private void dataTransferBetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowDataTransfer();
        }

        private void barcodePrintingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowBarcodePrintingPerDoc();
        }

        private void barcodePerBPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainService.ShowBarcodePrintingPerBP();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
using PresenterLayer.Helper;
using PresenterLayer.Views;
using PresenterLayer.Services.Inventory;
using PresenterLayer.Views.Inventory.Inventory_Transfer_Request;
using System;
using PresenterLayer.Views.Main;
using PresenterLayer.Views.Security;
using DirecLayer._02_Form.MVP.Views;
using InfrastructureLayer.InventoryRepository;
using DirecLayer;
using PresenterLayer.Views.Inventory.Inventory_Transfer;
using System.Windows.Forms;

namespace PresenterLayer.Services.Main
{
    public class MainService
    {
        static MainForm frmMain { get; set; }
        public MainService()
        {

        }

        public void FormLoad()
        {
            FormHelper.ShowForm(new LoginForm());
        }


        public bool WaitTenSecond()
        {
            var output = false;
            try
            {
                if (StaticHelper.MessageCountdown != 10)
                {
                    StaticHelper.MessageCountdown++;
                }
                else
                {
                    output = true;
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
            return output;
        }

        public void TimeTick()
        {
            IMainForm.Date = DateTime.Now.ToString("MM-dd-yyyy");
            IMainForm.Time = DateTime.Now.ToString("hh:mmtt");
        }

        public void ShowPurchaseOrder()
        {
            if (!CheckFormIfOpen("FrmPurchaseOrder"))
            {
                FormHelper.ShowPOForm(new FrmPurchaseOrder());
            }
        }

        public void ShowGoodReceipt()
        {
            if (!CheckFormIfOpen("FrmGoodsReceiptPO"))
            {
                FormHelper.ShowGRPOForm(new FrmGoodsReceiptPO());
            }
        }

        public void ShowAllocationWizard()
        {
            FormHelper.ShowAllocationWizard(new frmAllocationWizard());
        }

        public void ShowInventoryCounting()
        {
            if (!CheckFormIfOpen("frmInventoryCount"))
            {
                FormHelper.ShowForm(new frmInventoryCount());
            }
        }

        public void ShowInventoryPosting()
        {
            if (!CheckFormIfOpen("InventoryCount_Posting"))
            {
                FormHelper.ShowForm(new InventoryCount_Posting());
            }
        }

        public void ShowOutgoingPackingList()
        {
            if (!CheckFormIfOpen("frmOutGoingPacklist"))
            {
                FormHelper.ShowForm(new frmOutGoingPacklist());
            }
        }

        public void ShowConcession()
        {
            var cr = new frmCrystalReports();
            cr.type = "asn";
            FormHelper.ShowForm(cr);
        }

        public void ShowOutright()
        {
            var cr = new frmCrystalReports();
            cr.type = "outright";
            FormHelper.ShowForm(cr);
        }

        public void ShowPackingListConcession()
        {
            if (!CheckFormIfOpen("PackingList_Conssionaire"))
            {
                FormHelper.ShowForm(new PackingList_Conssionaire());
            }
        }

        public void ShowAutoReflenish()
        {
            //FormHelper.ShowAllocationWizard(new frmAutoReplenish());
        }

        public void ShowItemMasterData()
        {
            if (!CheckFormIfOpen("frmItemMasterData"))
            {
                FormHelper.ShowForm(new frmItemMasterData());
            }
        }

        public void ShowInventoryTransferRequest()
        {
            if (!CheckFormIfOpen("FrmInventoryTransferRequest"))
            {
                FormHelper.ShowInventoryTransferRequest(new FrmInventoryTransferRequest());
            }
        }

        public void ShowInventoryTransfer()
        {
            if (!CheckFormIfOpen("InventoryTransfer"))
            {
                FormHelper.ShowInventoryTransfer(new InventoryTransfer());
            }
        }

        public void ShowPrintLayout()
        {
            FormHelper.ShowForm(new frmMultiplePrint());
        }

        public void ShowCartonManagement()
        {
            if (!CheckFormIfOpen("FrmCartonManagement"))
            {
                FormHelper.ShowCartonManagement(new FrmCartonManagement(false, ""));
            }
        }

        public void ShowCartonList()
        {
            if (!CheckFormIfOpen("FrmCartonList"))
            {
                FormHelper.ShowCartonList(new FrmCartonList());
            }
        }
        public void ShowDataTransfer()
        {
            FormHelper.ShowForm(new frmDT());
        }

        public void ShowBarcodePrintingPerDoc()
        {
            if (!CheckFormIfOpen("frmBarcodeAll"))
            {
                FormHelper.ShowForm(new frmBarcodeAll());
            }
        }

        public void ShowBarcodePrintingPerBP()
        {
            if (!CheckFormIfOpen("frmBarcodePerBP"))
            {
                FormHelper.ShowForm(new frmBarcodePerBP());
            }
        }
        public void ChooseCompany()
        {
            if (!CheckFormIfOpen("ChooseCompanyForm"))
            {
                FormHelper.ShowForm(new ChooseCompanyForm());
            }
        }

        public void ShowUnofficialSales(string sModule)
        {
            if (!CheckFormIfOpen("FrmUnofficialSales"))
            {
                var form = new FrmUnofficialSales();
                form.Text = sModule;
                FormHelper.ShowUnofficialSalesForm(form);
            }
        }

        public void ShowSalesOrder()
        {
            if (!CheckFormIfOpen("FrmSalesOrder"))
            {
                FormHelper.ShowSalesOrderForm(new FrmSalesOrder());
            }
        }

        public void ShowArInvoice()
        {
            if (!CheckFormIfOpen("FrmARInvoice"))
            {
                FormHelper.ShowInvoiceForm(new FrmARInvoice());
            }
        }

        public void ShowFreezeItem()
        {
            if (!CheckFormIfOpen("frmFreezeItems"))
            {
                FormHelper.ShowForm(new frmFreezeItems());
            }
        }

        public bool CheckFormIfOpen(string FormName)
        {
            bool stat = false;

            Form fc = Application.OpenForms[FormName];
            if (fc != null)
            {
                stat = true;
                StaticHelper._MainForm.ShowMessage("Form already open.", true);
            }
            return stat;
        }
    }
}

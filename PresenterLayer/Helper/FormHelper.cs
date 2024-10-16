using DirecLayer._02_Form.MVP.Presenters;
using DirecLayer._02_Form.MVP.Views;
using DomainLayer.Models;
using InfrastructureLayer.InventoryRepository;
using MetroFramework.Forms;
using PresenterLayer.Services;
using PresenterLayer.Services.Inventory;
using PresenterLayer.Services.Inventory.Inventory_Transfer;
using PresenterLayer.Views;
using PresenterLayer.Views.Inventory.Inventory_Transfer;
using PresenterLayer.Views.Inventory.Inventory_Transfer_Request;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PresenterLayer.Helper
{
    public class FormHelper
    {
        public static void Close()
        {
            Application.Exit();
        }

        public static void ResizeForm(MetroForm form)
        {
            int max_height = Screen.PrimaryScreen.Bounds.Height - 180;
            if (form.WindowState == FormWindowState.Maximized)
            {
                form.Size = new Size(form.MdiParent.ClientSize.Width - 4, max_height);
                form.Location = new Point(0, 0);
                form.WindowState = FormWindowState.Normal;
            }
            else if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Minimized;
            }
        }

        public static void ShowForm(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                fForm.MdiParent = StaticHelper._MainForm;
                fForm.Show();
            }
        }

        public static void ShowPOForm(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                var model = new PurchaseOrderModel();
                fForm.MdiParent = StaticHelper._MainForm;
                var presenter = new PurchaseOrderPresenter((FrmPurchaseOrder)fForm, model);
                fForm.Show();
            }
        }

        public static void ShowGRPOForm(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                var model = new GoodRecieptModel();
                fForm.MdiParent = StaticHelper._MainForm;
                var presenter = new GoodReceiptService((FrmGoodsReceiptPO)fForm, model);
                fForm.Show();
            }
        }
        public static void ShowInvoiceForm(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                var model = new ARInvoiceModel();
                fForm.MdiParent = StaticHelper._MainForm;
                var presenter = new InvoiceService((FrmARInvoice)fForm, model);
                fForm.Show();
            }
        }
        public static void ShowAllocationWizard(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                Form frm = Application.OpenForms["frmAllocationWizard"];
                if (frm == null)
                {
                    fForm.MdiParent = StaticHelper._MainForm;
                    fForm.Show();
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Allocation wizard is already open.", true);
                }
            }
        }

        public static void ShowUnofficialSalesForm(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                var model = new UnofficialSalesModel();
                fForm.MdiParent = StaticHelper._MainForm;
                var presenter = new UnofficialSalesService((FrmUnofficialSales)fForm, model);
                fForm.Show();
            }
        }

        public static void ShowSalesOrderForm(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                var model = new SalesOrderModel();
                fForm.MdiParent = StaticHelper._MainForm;
                var presenter = new SalesOrderService((FrmSalesOrder)fForm, model);
                fForm.Show();
            }
        }

        public static void ShowCartonManagement(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                var model = new CartonManagementModel();
                fForm.MdiParent = StaticHelper._MainForm;
                var presenter = new CartonManagementPresenter((FrmCartonManagement)fForm, model);
                fForm.Show();
            }
        }

        public static void ShowCartonList(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                var model = new CartonListModel();
                fForm.MdiParent = StaticHelper._MainForm;
                var presenter = new CartonListPresenter((FrmCartonList)fForm, model);
                fForm.Show();
            }
        }

        public static void ShowInventoryTransferRequest(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                var model = new QueryRepository();
                fForm.MdiParent = StaticHelper._MainForm;
                var presenter = new InventoryTransferRequestService((FrmInventoryTransferRequest)fForm, model);
                fForm.Show();
            }
        }

        public static void ShowInventoryTransfer(MetroForm fForm, bool isShowDialog = false)
        {
            if (isShowDialog)
            { fForm.ShowDialog(); }
            else
            {
                var model = new QueryRepository();
                fForm.MdiParent = StaticHelper._MainForm;
                var presenter = new InventoryTransferService((InventoryTransfer)fForm, model);
                fForm.Show();
            }
        }

        public static void GetBackground()
        {
            try
            {
                string GetExePath = Path.GetDirectoryName(Application.ExecutablePath);
                string BgImagePath = EasySAPCredentialsModel.ESDatabase.Contains("NBFI") ? "\\Background\\NBFI\\BG_NBFI.jpg" : "\\Background\\EPC\\BG_EPC.jpg";
                //string BgImagePath = "\\Background\\EasySAPLogo.png";
                if (File.Exists(GetExePath + BgImagePath))
                {
                    Image BgImage = new Bitmap(GetExePath + BgImagePath);
                    //StaticHelper._MainForm.BackgroundImage = BgImage;
                    //StaticHelper._MainForm.BackgroundImageLayout = ImageLayout.Stretch;
                    StaticHelper._MainForm.pictureBox2.BackgroundImage = BgImage;
                    StaticHelper._MainForm.pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Background image does not exist.", true);
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }
    }
}


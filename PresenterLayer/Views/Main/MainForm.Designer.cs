using MetroFramework.Controls;
using System.ComponentModel;
using System.Windows.Forms;

namespace PresenterLayer.Views.Main
{
    partial class MainForm
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseCompanyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aRInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unofficialSalesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deliveryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.purchasingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.purchaseOrderPrototypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goodsReceiptPOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.purchaseOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemItemMasterData = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryTransferRequestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryTransferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryAutoReplenishmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryCountingTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryCountingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryCountingPostingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.concessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outgoingPackingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dBarcodePrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outrightToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.concessionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.freezeItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cartonsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cartonListPrototypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allocationWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barcodePrintingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barcodePerBPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deliveryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataTransferBetaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrMsg = new System.Windows.Forms.Timer(this.components);
            this.tmrTime = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip3 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbStatus = new MetroFramework.Controls.MetroProgressBar();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTime = new MetroFramework.Controls.MetroLabel();
            this.lblDate = new MetroFramework.Controls.MetroLabel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.lblNotification = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblConnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.menuStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.statusStrip3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.moduleToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 30);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(132, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // moduleToolStripMenuItem
            // 
            this.moduleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.administratorToolStripMenuItem,
            this.salesToolStripMenuItem,
            this.purchasingToolStripMenuItem,
            this.inventoryToolStripMenuItem,
            this.barCodeToolStripMenuItem,
            this.deliveryToolStripMenuItem,
            this.dataTransferBetaToolStripMenuItem});
            this.moduleToolStripMenuItem.Name = "moduleToolStripMenuItem";
            this.moduleToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.moduleToolStripMenuItem.Text = "&Module";
            // 
            // administratorToolStripMenuItem
            // 
            this.administratorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chooseCompanyToolStripMenuItem});
            this.administratorToolStripMenuItem.Name = "administratorToolStripMenuItem";
            this.administratorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.administratorToolStripMenuItem.Text = "&Administrator";
            // 
            // chooseCompanyToolStripMenuItem
            // 
            this.chooseCompanyToolStripMenuItem.Name = "chooseCompanyToolStripMenuItem";
            this.chooseCompanyToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.chooseCompanyToolStripMenuItem.Text = "&Choose Company";
            this.chooseCompanyToolStripMenuItem.Click += new System.EventHandler(this.chooseCompanyToolStripMenuItem_Click);
            // 
            // salesToolStripMenuItem
            // 
            this.salesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salesOrderToolStripMenuItem,
            this.aRInvoiceToolStripMenuItem,
            this.unofficialSalesToolStripMenuItem,
            this.deliveryToolStripMenuItem1});
            this.salesToolStripMenuItem.Name = "salesToolStripMenuItem";
            this.salesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.salesToolStripMenuItem.Text = "&Sales - A/R";
            // 
            // salesOrderToolStripMenuItem
            // 
            this.salesOrderToolStripMenuItem.Name = "salesOrderToolStripMenuItem";
            this.salesOrderToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.salesOrderToolStripMenuItem.Text = "&Sales Order";
            this.salesOrderToolStripMenuItem.Click += new System.EventHandler(this.salesOrderToolStripMenuItem_Click_1);
            // 
            // aRInvoiceToolStripMenuItem
            // 
            this.aRInvoiceToolStripMenuItem.Name = "aRInvoiceToolStripMenuItem";
            this.aRInvoiceToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.aRInvoiceToolStripMenuItem.Text = "A/R &Invoice";
            this.aRInvoiceToolStripMenuItem.Click += new System.EventHandler(this.aRInvoiceToolStripMenuItem_Click);
            // 
            // unofficialSalesToolStripMenuItem
            // 
            this.unofficialSalesToolStripMenuItem.Name = "unofficialSalesToolStripMenuItem";
            this.unofficialSalesToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.unofficialSalesToolStripMenuItem.Text = "Unofficial Sales/Warehouse Sales";
            this.unofficialSalesToolStripMenuItem.Click += new System.EventHandler(this.unofficialSalesToolStripMenuItem_Click);
            // 
            // deliveryToolStripMenuItem1
            // 
            this.deliveryToolStripMenuItem1.Name = "deliveryToolStripMenuItem1";
            this.deliveryToolStripMenuItem1.Size = new System.Drawing.Size(247, 22);
            this.deliveryToolStripMenuItem1.Text = "Delivery";
            this.deliveryToolStripMenuItem1.Click += new System.EventHandler(this.deliveryToolStripMenuItem1_Click);
            // 
            // purchasingToolStripMenuItem
            // 
            this.purchasingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.purchaseOrderPrototypeToolStripMenuItem,
            this.goodsReceiptPOToolStripMenuItem,
            this.purchaseOrderToolStripMenuItem});
            this.purchasingToolStripMenuItem.Name = "purchasingToolStripMenuItem";
            this.purchasingToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.purchasingToolStripMenuItem.Text = "&Purchasing - A/P";
            // 
            // purchaseOrderPrototypeToolStripMenuItem
            // 
            this.purchaseOrderPrototypeToolStripMenuItem.Name = "purchaseOrderPrototypeToolStripMenuItem";
            this.purchaseOrderPrototypeToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.purchaseOrderPrototypeToolStripMenuItem.Text = "Purchase Order";
            this.purchaseOrderPrototypeToolStripMenuItem.Click += new System.EventHandler(this.purchaseOrderPrototypeToolStripMenuItem_Click);
            // 
            // goodsReceiptPOToolStripMenuItem
            // 
            this.goodsReceiptPOToolStripMenuItem.Name = "goodsReceiptPOToolStripMenuItem";
            this.goodsReceiptPOToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.goodsReceiptPOToolStripMenuItem.Text = "&Goods Receipt PO";
            this.goodsReceiptPOToolStripMenuItem.Click += new System.EventHandler(this.goodsReceiptPOToolStripMenuItem_Click);
            // 
            // purchaseOrderToolStripMenuItem
            // 
            this.purchaseOrderToolStripMenuItem.Name = "purchaseOrderToolStripMenuItem";
            this.purchaseOrderToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.purchaseOrderToolStripMenuItem.Text = "&Purchase Order (MERCH)";
            this.purchaseOrderToolStripMenuItem.Visible = false;
            // 
            // inventoryToolStripMenuItem
            // 
            this.inventoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemItemMasterData,
            this.inventoryTransactionToolStripMenuItem,
            this.freezeItemsToolStripMenuItem,
            this.printLayoutToolStripMenuItem,
            this.cartonsToolStripMenuItem,
            this.cartonListPrototypeToolStripMenuItem,
            this.allocationWizardToolStripMenuItem});
            this.inventoryToolStripMenuItem.Name = "inventoryToolStripMenuItem";
            this.inventoryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.inventoryToolStripMenuItem.Text = "&Inventory";
            // 
            // MenuItemItemMasterData
            // 
            this.MenuItemItemMasterData.Name = "MenuItemItemMasterData";
            this.MenuItemItemMasterData.Size = new System.Drawing.Size(188, 22);
            this.MenuItemItemMasterData.Text = "Item &Master Data";
            this.MenuItemItemMasterData.Click += new System.EventHandler(this.MenuItemItemMasterData_Click);
            // 
            // inventoryTransactionToolStripMenuItem
            // 
            this.inventoryTransactionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventoryTransferRequestToolStripMenuItem,
            this.inventoryTransferToolStripMenuItem,
            this.inventoryAutoReplenishmentToolStripMenuItem,
            this.inventoryCountingTransactionsToolStripMenuItem,
            this.packingListToolStripMenuItem,
            this.dBarcodePrintToolStripMenuItem});
            this.inventoryTransactionToolStripMenuItem.Name = "inventoryTransactionToolStripMenuItem";
            this.inventoryTransactionToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.inventoryTransactionToolStripMenuItem.Text = "&Inventory Transaction";
            // 
            // inventoryTransferRequestToolStripMenuItem
            // 
            this.inventoryTransferRequestToolStripMenuItem.Name = "inventoryTransferRequestToolStripMenuItem";
            this.inventoryTransferRequestToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.inventoryTransferRequestToolStripMenuItem.Text = "Inventory Transfer &Request";
            this.inventoryTransferRequestToolStripMenuItem.Click += new System.EventHandler(this.inventoryTransferRequestToolStripMenuItem_Click);
            // 
            // inventoryTransferToolStripMenuItem
            // 
            this.inventoryTransferToolStripMenuItem.Name = "inventoryTransferToolStripMenuItem";
            this.inventoryTransferToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.inventoryTransferToolStripMenuItem.Text = "Inventory &Transfer";
            this.inventoryTransferToolStripMenuItem.Click += new System.EventHandler(this.inventoryTransferToolStripMenuItem_Click);
            // 
            // inventoryAutoReplenishmentToolStripMenuItem
            // 
            this.inventoryAutoReplenishmentToolStripMenuItem.Name = "inventoryAutoReplenishmentToolStripMenuItem";
            this.inventoryAutoReplenishmentToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.inventoryAutoReplenishmentToolStripMenuItem.Text = "Inventory Auto Replenishment";
            this.inventoryAutoReplenishmentToolStripMenuItem.Visible = false;
            this.inventoryAutoReplenishmentToolStripMenuItem.Click += new System.EventHandler(this.inventoryAutoReplenishmentToolStripMenuItem_Click);
            // 
            // inventoryCountingTransactionsToolStripMenuItem
            // 
            this.inventoryCountingTransactionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventoryCountingToolStripMenuItem,
            this.inventoryCountingPostingToolStripMenuItem});
            this.inventoryCountingTransactionsToolStripMenuItem.Name = "inventoryCountingTransactionsToolStripMenuItem";
            this.inventoryCountingTransactionsToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.inventoryCountingTransactionsToolStripMenuItem.Text = "&Inventory Counting Transactions";
            // 
            // inventoryCountingToolStripMenuItem
            // 
            this.inventoryCountingToolStripMenuItem.Name = "inventoryCountingToolStripMenuItem";
            this.inventoryCountingToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.inventoryCountingToolStripMenuItem.Text = "Inventory &Counting";
            this.inventoryCountingToolStripMenuItem.Click += new System.EventHandler(this.inventoryCountingToolStripMenuItem_Click);
            // 
            // inventoryCountingPostingToolStripMenuItem
            // 
            this.inventoryCountingPostingToolStripMenuItem.Name = "inventoryCountingPostingToolStripMenuItem";
            this.inventoryCountingPostingToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.inventoryCountingPostingToolStripMenuItem.Text = "Inventory &Counting Posting";
            this.inventoryCountingPostingToolStripMenuItem.Click += new System.EventHandler(this.inventoryCountingPostingToolStripMenuItem_Click);
            // 
            // packingListToolStripMenuItem
            // 
            this.packingListToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.concessionToolStripMenuItem,
            this.outgoingPackingListToolStripMenuItem});
            this.packingListToolStripMenuItem.Name = "packingListToolStripMenuItem";
            this.packingListToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.packingListToolStripMenuItem.Text = "Carton Labels";
            // 
            // concessionToolStripMenuItem
            // 
            this.concessionToolStripMenuItem.Name = "concessionToolStripMenuItem";
            this.concessionToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.concessionToolStripMenuItem.Text = "2D Barcode";
            this.concessionToolStripMenuItem.Click += new System.EventHandler(this.concessionToolStripMenuItem_Click);
            // 
            // outgoingPackingListToolStripMenuItem
            // 
            this.outgoingPackingListToolStripMenuItem.Name = "outgoingPackingListToolStripMenuItem";
            this.outgoingPackingListToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.outgoingPackingListToolStripMenuItem.Text = "Outgoing Packing List";
            this.outgoingPackingListToolStripMenuItem.Click += new System.EventHandler(this.outgoingPackingListToolStripMenuItem_Click);
            // 
            // dBarcodePrintToolStripMenuItem
            // 
            this.dBarcodePrintToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outrightToolStripMenuItem1,
            this.concessionToolStripMenuItem1});
            this.dBarcodePrintToolStripMenuItem.Name = "dBarcodePrintToolStripMenuItem";
            this.dBarcodePrintToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.dBarcodePrintToolStripMenuItem.Text = "2D Barcode Print";
            this.dBarcodePrintToolStripMenuItem.Visible = false;
            // 
            // outrightToolStripMenuItem1
            // 
            this.outrightToolStripMenuItem1.Name = "outrightToolStripMenuItem1";
            this.outrightToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.outrightToolStripMenuItem1.Text = "Outright";
            this.outrightToolStripMenuItem1.Click += new System.EventHandler(this.outrightToolStripMenuItem1_Click);
            // 
            // concessionToolStripMenuItem1
            // 
            this.concessionToolStripMenuItem1.Name = "concessionToolStripMenuItem1";
            this.concessionToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.concessionToolStripMenuItem1.Text = "Concession";
            this.concessionToolStripMenuItem1.Click += new System.EventHandler(this.concessionToolStripMenuItem1_Click);
            // 
            // freezeItemsToolStripMenuItem
            // 
            this.freezeItemsToolStripMenuItem.Name = "freezeItemsToolStripMenuItem";
            this.freezeItemsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.freezeItemsToolStripMenuItem.Text = "Warehouse Lock";
            this.freezeItemsToolStripMenuItem.Click += new System.EventHandler(this.freezeItemsToolStripMenuItem_Click);
            // 
            // printLayoutToolStripMenuItem
            // 
            this.printLayoutToolStripMenuItem.Name = "printLayoutToolStripMenuItem";
            this.printLayoutToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.printLayoutToolStripMenuItem.Text = "Print Layout";
            this.printLayoutToolStripMenuItem.Visible = false;
            this.printLayoutToolStripMenuItem.Click += new System.EventHandler(this.printLayoutToolStripMenuItem_Click);
            // 
            // cartonsToolStripMenuItem
            // 
            this.cartonsToolStripMenuItem.Name = "cartonsToolStripMenuItem";
            this.cartonsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.cartonsToolStripMenuItem.Text = "Carton Management";
            this.cartonsToolStripMenuItem.Click += new System.EventHandler(this.cartonsToolStripMenuItem_Click);
            // 
            // cartonListPrototypeToolStripMenuItem
            // 
            this.cartonListPrototypeToolStripMenuItem.Name = "cartonListPrototypeToolStripMenuItem";
            this.cartonListPrototypeToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.cartonListPrototypeToolStripMenuItem.Text = "Carton List ";
            this.cartonListPrototypeToolStripMenuItem.Click += new System.EventHandler(this.cartonListPrototypeToolStripMenuItem_Click);
            // 
            // allocationWizardToolStripMenuItem
            // 
            this.allocationWizardToolStripMenuItem.Name = "allocationWizardToolStripMenuItem";
            this.allocationWizardToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.allocationWizardToolStripMenuItem.Text = "&Allocation Wizard";
            this.allocationWizardToolStripMenuItem.Click += new System.EventHandler(this.allocationWizardToolStripMenuItem_Click);
            // 
            // barCodeToolStripMenuItem
            // 
            this.barCodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barcodePrintingToolStripMenuItem,
            this.barcodePerBPToolStripMenuItem});
            this.barCodeToolStripMenuItem.Name = "barCodeToolStripMenuItem";
            this.barCodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.barCodeToolStripMenuItem.Text = "Price &Tag";
            // 
            // barcodePrintingToolStripMenuItem
            // 
            this.barcodePrintingToolStripMenuItem.Name = "barcodePrintingToolStripMenuItem";
            this.barcodePrintingToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.barcodePrintingToolStripMenuItem.Text = "&Price Tag Printing";
            this.barcodePrintingToolStripMenuItem.Click += new System.EventHandler(this.barcodePrintingToolStripMenuItem_Click);
            // 
            // barcodePerBPToolStripMenuItem
            // 
            this.barcodePerBPToolStripMenuItem.Name = "barcodePerBPToolStripMenuItem";
            this.barcodePerBPToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.barcodePerBPToolStripMenuItem.Text = "Pr&ice Tag Per Document";
            this.barcodePerBPToolStripMenuItem.Click += new System.EventHandler(this.barcodePerBPToolStripMenuItem_Click);
            // 
            // deliveryToolStripMenuItem
            // 
            this.deliveryToolStripMenuItem.Name = "deliveryToolStripMenuItem";
            this.deliveryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deliveryToolStripMenuItem.Text = "ITR Closing";
            // 
            // dataTransferBetaToolStripMenuItem
            // 
            this.dataTransferBetaToolStripMenuItem.Name = "dataTransferBetaToolStripMenuItem";
            this.dataTransferBetaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dataTransferBetaToolStripMenuItem.Text = "Data Transfer";
            this.dataTransferBetaToolStripMenuItem.Click += new System.EventHandler(this.dataTransferBetaToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queryToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            this.toolsToolStripMenuItem.Visible = false;
            // 
            // queryToolStripMenuItem
            // 
            this.queryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queryGeneratorToolStripMenuItem});
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            this.queryToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.queryToolStripMenuItem.Text = "&Queries";
            // 
            // queryGeneratorToolStripMenuItem
            // 
            this.queryGeneratorToolStripMenuItem.Name = "queryGeneratorToolStripMenuItem";
            this.queryGeneratorToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.queryGeneratorToolStripMenuItem.Text = "Q&uery Generator";
            // 
            // tmrMsg
            // 
            this.tmrMsg.Interval = 300;
            this.tmrMsg.Tick += new System.EventHandler(this.tmrMsg_Tick);
            // 
            // tmrTime
            // 
            this.tmrTime.Enabled = true;
            this.tmrTime.Tick += new System.EventHandler(this.tmrTime_Tick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 401);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 39);
            this.panel2.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.85535F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.301887F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(800, 39);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::PresenterLayer.Properties.Resources.DirecLogo;
            this.pictureBox1.Location = new System.Drawing.Point(737, 5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(58, 29);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.statusStrip3, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.pbStatus, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(438, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(294, 39);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // statusStrip3
            // 
            this.statusStrip3.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip3.Location = new System.Drawing.Point(0, 0);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(294, 19);
            this.statusStrip3.SizingGrip = false;
            this.statusStrip3.TabIndex = 2;
            this.statusStrip3.Text = "statusStrip3";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = false;
            this.lblStatus.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(10, 1, 0, 1);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(269, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbStatus
            // 
            this.pbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbStatus.Location = new System.Drawing.Point(10, 20);
            this.pbStatus.Margin = new System.Windows.Forms.Padding(10, 1, 15, 1);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(269, 18);
            this.pbStatus.TabIndex = 3;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.lblTime, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblDate, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(359, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(79, 39);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime.Location = new System.Drawing.Point(3, 19);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(73, 20);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "06:50PM";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDate.Location = new System.Drawing.Point(3, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(73, 19);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "05-16-2016";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.statusStrip2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.statusStrip1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(359, 39);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // statusStrip2
            // 
            this.statusStrip2.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblNotification});
            this.statusStrip2.Location = new System.Drawing.Point(0, 19);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(359, 20);
            this.statusStrip2.SizingGrip = false;
            this.statusStrip2.TabIndex = 1;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = false;
            this.lblNotification.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblNotification.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblNotification.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblNotification.ForeColor = System.Drawing.Color.White;
            this.lblNotification.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(344, 18);
            this.lblNotification.Spring = true;
            this.lblNotification.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblNotification.MouseLeave += new System.EventHandler(this.lblNotification_MouseLeave);
            this.lblNotification.MouseHover += new System.EventHandler(this.lblNotification_MouseHover);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblConnected});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(359, 19);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblConnected
            // 
            this.lblConnected.AutoSize = false;
            this.lblConnected.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblConnected.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblConnected.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblConnected.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(344, 17);
            this.lblConnected.Spring = true;
            this.lblConnected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Location = new System.Drawing.Point(0, 54);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(800, 25);
            this.toolStrip.TabIndex = 13;
            this.toolStrip.Text = "toolStrip1";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Location = new System.Drawing.Point(680, 30);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(120, 43);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip);
            this.DisplayHeader = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MinimumSize = new System.Drawing.Size(795, 450);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 10);
            this.Text = "MainForm";
            this.TransparencyKey = System.Drawing.Color.Empty;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.statusStrip3.ResumeLayout(false);
            this.statusStrip3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem moduleToolStripMenuItem;
        private ToolStripMenuItem administratorToolStripMenuItem;
        private ToolStripMenuItem chooseCompanyToolStripMenuItem;
        private ToolStripMenuItem salesToolStripMenuItem;
        private ToolStripMenuItem salesOrderToolStripMenuItem;
        private ToolStripMenuItem aRInvoiceToolStripMenuItem;
        private ToolStripMenuItem unofficialSalesToolStripMenuItem;
        private ToolStripMenuItem deliveryToolStripMenuItem1;
        private ToolStripMenuItem purchasingToolStripMenuItem;
        private ToolStripMenuItem purchaseOrderPrototypeToolStripMenuItem;
        private ToolStripMenuItem goodsReceiptPOToolStripMenuItem;
        private ToolStripMenuItem purchaseOrderToolStripMenuItem;
        private ToolStripMenuItem inventoryToolStripMenuItem;
        private ToolStripMenuItem MenuItemItemMasterData;
        private ToolStripMenuItem inventoryTransactionToolStripMenuItem;
        private ToolStripMenuItem inventoryTransferRequestToolStripMenuItem;
        private ToolStripMenuItem inventoryTransferToolStripMenuItem;
        private ToolStripMenuItem inventoryAutoReplenishmentToolStripMenuItem;
        private ToolStripMenuItem inventoryCountingTransactionsToolStripMenuItem;
        private ToolStripMenuItem inventoryCountingToolStripMenuItem;
        private ToolStripMenuItem inventoryCountingPostingToolStripMenuItem;
        private ToolStripMenuItem packingListToolStripMenuItem;
        private ToolStripMenuItem concessionToolStripMenuItem;
        private ToolStripMenuItem outgoingPackingListToolStripMenuItem;
        private ToolStripMenuItem dBarcodePrintToolStripMenuItem;
        private ToolStripMenuItem outrightToolStripMenuItem1;
        private ToolStripMenuItem concessionToolStripMenuItem1;
        private ToolStripMenuItem freezeItemsToolStripMenuItem;
        private ToolStripMenuItem printLayoutToolStripMenuItem;
        private ToolStripMenuItem cartonsToolStripMenuItem;
        private ToolStripMenuItem cartonListPrototypeToolStripMenuItem;
        private ToolStripMenuItem allocationWizardToolStripMenuItem;
        private ToolStripMenuItem barCodeToolStripMenuItem;
        private ToolStripMenuItem barcodePrintingToolStripMenuItem;
        private ToolStripMenuItem barcodePerBPToolStripMenuItem;
        private ToolStripMenuItem deliveryToolStripMenuItem;
        private ToolStripMenuItem dataTransferBetaToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem queryToolStripMenuItem;
        private ToolStripMenuItem queryGeneratorToolStripMenuItem;
        private Timer tmrMsg;
        private Timer tmrTime;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel2;
        private PictureBox pictureBox1;
        private TableLayoutPanel tableLayoutPanel5;
        private StatusStrip statusStrip3;
        public ToolStripStatusLabel lblStatus;
        public MetroProgressBar pbStatus;
        private TableLayoutPanel tableLayoutPanel4;
        public MetroLabel lblTime;
        public MetroLabel lblDate;
        private TableLayoutPanel tableLayoutPanel3;
        private StatusStrip statusStrip2;
        public ToolStripStatusLabel lblNotification;
        private StatusStrip statusStrip1;
        public ToolStripStatusLabel lblConnected;
        private ToolStrip toolStrip;
        public PictureBox pictureBox2;
    }
}
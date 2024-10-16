using sys = System.Windows.Forms;
using MetroFramework.Forms;
using System;
using PresenterLayer.Services;

namespace PresenterLayer.Views
{
    public partial class frmItemMasterData : MetroForm
    {
        public ITM_Form form { get; set; }
        ITM_ChooseFromList cfl { get; set; }
       
        ITM_Size size { get; set; }
        ITM_Color color { get; set; }

        ITM_Price price { get; set; }
        ITM_SL sl { get; set; }

        ITM_Preview preview { get; set; }

        ITM_QuantityPerCarton QPC;

        string sColName = "";
        
        protected override bool ProcessCmdKey(ref sys.Message msg, sys.Keys keyData)
        {
            if (keyData == sys.Keys.Escape)
            { Close(); }
            else if (keyData == (sys.Keys.Control | sys.Keys.Shift | sys.Keys.U))
            { form.UDFShow(); }
            else if ((U_ID001.Focused || U_Name_ID001.Focused) && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID001.Text))
            { cfl.GetBrands(); }
            else if (U_ID002.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID002.Text))
            { cfl.GetDepartments(); }
            else if (U_ID003.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID003.Text))
            { cfl.GetSubDepartments(); }
            else if ((U_ID020.Focused || U_ID004.Focused) && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID020.Text))
            { cfl.GetCategories(); }
            else if ((CardCode.Focused || CardName.Focused) && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(CardCode.Text))
            { cfl.GetSuppliers(); }
            else if ((U_ID021.Focused || U_ID005.Focused) && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID021.Text))
            { cfl.GetSubCategories(); }
            else if ((U_ID012.Focused || U_ID025.Focused) && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID012.Text))
            { cfl.GetStyles(); }
            else if (U_ID013.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID013.Text))
            { cfl.GetClass(); }
            else if (U_ID014.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID014.Text))
            { cfl.GetSubClass(); }
            else if (U_ID015.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID015.Text))
            { cfl.GetPackaging(); }
            else if (U_ID016.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID016.Text))
            { cfl.GetSpecifications(); }
            else if (U_ID017.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(U_ID017.Text))
            { cfl.GetColections(); }
            else if (InvntryUom.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(InvntryUom.Text))
            { cfl.GetInvntryUom(ItmsGrpCod.SelectedValue.ToString()); }
            else if (SalUnitMsr.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(SalUnitMsr.Text))
            { cfl.GetSalUnitMsr(ItmsGrpCod.SelectedValue.ToString()); }
            else if (BuyUnitMsr.Focused && keyData == sys.Keys.Oem3 && string.IsNullOrEmpty(BuyUnitMsr.Text))
            { cfl.GetBuyUnitMsr(ItmsGrpCod.SelectedValue.ToString()); }

            else if (keyData == (sys.Keys.Control | sys.Keys.D1))
            { tabControl.SelectTab(tabColor); }
            else if (keyData == (sys.Keys.Control | sys.Keys.D2))
            { tabControl.SelectTab(tabSizes); }
            else if (keyData == (sys.Keys.Control | sys.Keys.D3))
            { tabControl.SelectTab(tabRemarks); }
            else if (keyData == (sys.Keys.Control | sys.Keys.D4))
            { tabControl.SelectTab(tabItemList); }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public frmItemMasterData()
        {
            InitializeComponent();

            ITM_Size size = new ITM_Size();
            size.imd = this;
            this.size = size;

            ITM_Color color = new ITM_Color();
            color.imd = this;
            this.color = color;

            ITM_Preview preview = new ITM_Preview();
            this.preview = preview;
            preview.imd = this;

            ITM_Form form = new ITM_Form();
            form.frmItemMasterData = this;
            form.color = color;
            form.size = size;
            this.form = form;

            ITM_ChooseFromList cfl = new ITM_ChooseFromList();
            cfl.frmItemMasterData = this;
            this.cfl = cfl;

            ITM_Price price = new ITM_Price();
            this.price = price;

            ITM_SL sl = new ITM_SL();
            sl.imd = this;
            sl.form = form;
            this.sl = sl;

            ITM_QuantityPerCarton QPC = new ITM_QuantityPerCarton();
            this.QPC = QPC;
            QPC.imd = this;
        }

        private void p_ID001_Click(object sender, EventArgs e)
        { cfl.GetBrands(); }

        private void frmItemMasterData_Load(object sender, EventArgs e)
        {
            form.LoadForm();
            preview.Form_Load();
        }

        private void btnCommand_Click(object sender, EventArgs e)
        { sl.Post(); }

        private void frmItemMasterData_Resize(object sender, EventArgs e)
        { form.frmResize(); }

        private void dgvColor_CellContentClick(object sender, sys.DataGridViewCellEventArgs e)
        { color.CellContentClick(sender, e); }

        private void SearchColor_TextChanged(object sender, EventArgs e)
        { color.SearchColor(dgvColor,sColName,SearchColor); }

        private void p_ID002_Click(object sender, EventArgs e)
        { cfl.GetDepartments(); }

        private void p_ID003_Click(object sender, EventArgs e)
        { cfl.GetSubDepartments(); }

        private void p_ID004_Click(object sender, EventArgs e)
        { cfl.GetCategories(); }

        private void Price_KeyPress(object sender, sys.KeyPressEventArgs e)
        { price.Price_KeyPress(sender, e); }

        private void pbSupplier_Click(object sender, EventArgs e)
        { cfl.GetSuppliers(); }

        private void p_ID005_Click(object sender, EventArgs e)
        { cfl.GetSubCategories(); }

        private void InvntItem_CheckedChanged(object sender, EventArgs e)
        { form.CheckInvntItem(); }

        private void SellItem_CheckedChanged(object sender, EventArgs e)
        { form.CheckSellItem(); }

        private void PrchseItem_CheckedChanged(object sender, EventArgs e)
        { form.CheckPrchseItem(); }

        private void U_ID006_SelectedIndexChanged(object sender, EventArgs e)
        { size.SelectedIndexChanged(); }

        private void btnCancel_Click(object sender, EventArgs e)
        { Close(); }

        private void frmItemMasterData_FormClosing(object sender, sys.FormClosingEventArgs e)
        { form.ClosingForm(e); }

        private void p_ID012_Click(object sender, EventArgs e)
        { cfl.GetStyles(); }

        private void p_ID013_Click(object sender, EventArgs e)
        { cfl.GetClass(); }

        private void p_ID014_Click(object sender, EventArgs e)
        { cfl.GetSubClass(); }

        private void p_ID015_Click(object sender, EventArgs e)
        { cfl.GetPackaging(); }

        private void frmItemMasterData_LocationChanged(object sender, EventArgs e)
        { form.LocationChanged(); }
        
        private void p_InvntryUom_Click(object sender, EventArgs e)
        { cfl.GetInvntryUom(ItmsGrpCod.SelectedValue.ToString()); }

        private void p_SalUnitMsr_Click(object sender, EventArgs e)
        { cfl.GetSalUnitMsr(ItmsGrpCod.SelectedValue.ToString()); }

        private void p_BuyUnitMsr_Click(object sender, EventArgs e)
        { cfl.GetBuyUnitMsr(ItmsGrpCod.SelectedValue.ToString()); }

        private void p_ID016_Click(object sender, EventArgs e)
        { cfl.GetSpecifications(); }

        private void p_ID017_Click(object sender, EventArgs e)
        { cfl.GetColections(); }

        private void btnAddSize_Click(object sender, EventArgs e)
        { cfl.GetSize(); }

        private void btnAddColor_Click(object sender, EventArgs e)
        { cfl.GetColors(); }

        private void ColumnHeaderMouseClick(object sender, sys.DataGridViewCellMouseEventArgs e)
        { form.ColumnHeaderMouseClick(sender, e,out sColName); }

        private void btnPreview_Click(object sender, EventArgs e)
        { preview.LoadData(); }

        private void U_ID021_TextChanged(object sender, EventArgs e)
        { size.SubClass_TextChanged(); }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        { preview.SearchEngine(dgvItemList, txtSearch); }

        private void dgvItemList_ColumnHeaderMouseClick(object sender, sys.DataGridViewCellMouseEventArgs e)
        { preview.ColumnHeaderMouseClick(sender,e); }
        
        private void dgvColor_CellContentClick(object sender, sys.DataGridViewCellCancelEventArgs e)
        { color.dgvColor_CellValueChanged(e); }

        private void SalUnitMsr_TextChanged(object sender, EventArgs e)
        {
            QPC.SetQtyPerCarton("S");
        }

        private void BuyUnitMsr_TextChanged(object sender, EventArgs e)
        {
            QPC.SetQtyPerCarton("P");
        }

        private void dgvItemList_CellEnter(object sender, sys.DataGridViewCellEventArgs e)
        { form.AutoEdit(sender, e); }

        private void dgvItemList_CellEndEdit(object sender, sys.DataGridViewCellEventArgs e)
        {
            preview.IsInputtedValueNumber(e);
        }
    }
}

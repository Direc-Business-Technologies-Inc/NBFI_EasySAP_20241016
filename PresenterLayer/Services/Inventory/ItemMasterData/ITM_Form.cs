using Context;
using DirecLayer;
using MetroFramework;
using PresenterLayer.Helper;
using PresenterLayer.Tools;
using PresenterLayer.Views;
using System.Drawing;
using System.Windows.Forms;

namespace PresenterLayer.Services
{
    public class ITM_Form
    {
        int max_width = Screen.PrimaryScreen.Bounds.Width - 220;
        int max_height = Screen.PrimaryScreen.Bounds.Height - 200;
        public frmItemMasterData frmItemMasterData { get; set; }
        public ITM_Color color { get; set; }
        public ITM_Size size { get; set; }
        public frm_UDF frmUDF { get; set; }
        public UDF_Form udf { get; set; }
        public void LoadForm()
        {
            ClearAll();
            UDFShow();
            frmItemMasterData.WindowState = FormWindowState.Maximized;
            color.LoadGridView();
            LoadItmsGrpCod();
            size.LoadSizeCategories();
            LoadPrice();
        }

        void ClearAll()
        {
            var frm = frmItemMasterData;

            foreach (var c in frmItemMasterData.panel1.Controls)
            {
                if (c is TextBox)
                {
                    try
                    { ((TextBox)c).Clear(); }
                    catch { }
                }
            }
            frm.UserText.Clear();
            frm.InvntItem.Checked = false;
            frm.SellItem.Checked = false;
            frm.PrchseItem.Checked = false;
        }

        void LoadPrice()
        {
            frmItemMasterData.Price.Text = "0.00";
            frmItemMasterData.OutrightPrice.Text = "0.00";
        }

        void LoadItmsGrpCod()
        {
            var frm = frmItemMasterData;
            frm.ItmsGrpCod.DataSource = null;
            var sapHana = new SAPHanaAccess();
            frm.ItmsGrpCod.DataSource = sapHana.Get(SP.ITM_ItmsGrpCod);

            frm.ItmsGrpCod.ValueMember = "Code";
            frm.ItmsGrpCod.DisplayMember = "Name";
            var helper = new DataHelper();
            frm.ItmsGrpCod.SelectedValue = helper.ReadDataRow(sapHana.Get(SP.ITM_DeftItemGrp), 0, "", 0);
        }
        
        public void frmResize()
        {
            var frmITM = frmItemMasterData;

            if (frmUDF != null)
            {
                if (frmITM.WindowState == FormWindowState.Maximized)
                {
                    int udf_width = frmUDF.Width;
                    frmITM.Size = new Size(frmITM.MdiParent.ClientSize.Width - udf_width - 10, max_height - 5);
                    frmITM.WindowState = FormWindowState.Normal;
                    frmITM.Location = new Point(0, 0);
                }
            }
            else
            { frmITM.WindowState = FormWindowState.Normal; }
            frmUDF.Location = new Point(frmITM.Right, frmITM.Top);
            frmUDF.MinimumSize = new Size(0, frmITM.Height);
            frmUDF.MaximumSize = new Size(frmITM.Width, frmITM.Height);
            frmUDF.Height = frmITM.Height;
        }
        
        public void ClearUDF()
        {
            foreach (DataGridViewRow dr in udf.frmUDF.dgvUDF.Rows)
            {
                dr.Cells["Field"].Value = "";
            }
        }

        public void UDFShow()
        {
            var frmITM = frmItemMasterData;

            if (udf == null)
            {
                udf = new UDF_Form();
                udf.LoadUDF("OITM","150");
            }

            frmUDF = udf.frmUDF;
            if (frmITM.Size == new Size(max_width, max_height))
            {
                int udf_width = frmUDF.Width;
                frmITM.Size = new Size(max_width - udf_width, max_height);
                frmITM.StartPosition = FormStartPosition.CenterScreen;
                frmITM.MaximumSize = new Size(max_width - udf_width, max_height);
            }
            else
            {
                frmUDF.StartPosition = FormStartPosition.Manual;
                frmITM.MaximumSize = new Size(max_width, max_height);
            }
            frmITM.WindowState = FormWindowState.Normal;
            frmUDF.Location = new Point(frmITM.Right, frmITM.Top);
            frmUDF.Show();
        }

        public void ClosingForm(FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(frmItemMasterData.U_ID001.Text))
            {
                if (MetroMessageBox.Show(StaticHelper._MainForm, $"Unsaved data will be lost. Continue?", LibraryHelper.AssemblyInfo.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
                { CloseForm(); }
                else
                { e.Cancel = true; }
            }
            else { CloseForm(); }
        }

        void CloseForm()
        {
            if (frmUDF != null)
            { frmUDF.Dispose(); }
            frmItemMasterData.Dispose();
        }

        public void CheckInvntItem()
        {
            frmItemMasterData.p_InvntryUom.Enabled = frmItemMasterData.InvntItem.Checked;
            frmItemMasterData.InvntryUom.Clear();
        }

        public void CheckSellItem()
        {
            frmItemMasterData.p_SalUnitMsr.Enabled = frmItemMasterData.SellItem.Checked;
            frmItemMasterData.SalUnitMsr.Clear();
        }

        public void CheckPrchseItem()
        {
            frmItemMasterData.p_BuyUnitMsr.Enabled = frmItemMasterData.PrchseItem.Checked;
            frmItemMasterData.BuyUnitMsr.Clear();
        }

        public void LocationChanged()
        {
            if (frmUDF != null)
            {
                frmUDF.Location = new Point(frmItemMasterData.Right, frmItemMasterData.Top);
                frmUDF.Height = frmItemMasterData.Height;
            }
        }
        
        public void ClearUOMDetails()
        {
            frmItemMasterData.InvntryUom.Clear();
            frmItemMasterData.SalUnitMsr.Clear();
            frmItemMasterData.BuyUnitMsr.Clear();
        }

        public void ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e, out string sColName)
        {
            var dgv = (DataGridView)sender;
            if (dgv.Rows.Count > 0)
            { sColName = dgv.Columns[e.ColumnIndex].Name; }
            else
            { sColName = ""; }
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

    }
}

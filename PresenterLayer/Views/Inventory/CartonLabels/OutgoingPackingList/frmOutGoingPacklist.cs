using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using DirecLayer;
using MetroFramework;
using DomainLayer.Models.OutgoingPackingList;
using PresenterLayer.Helper;
using ServiceLayer.Services;
using PresenterLayer.Views;

namespace PresenterLayer
{
    public partial class frmOutGoingPacklist : MetroForm
    {
        SAPHanaAccess hana { get; set; }
        SAPMsSqlAccess msSql { get; set; }
        DataHelper helper { get; set; }
        OutGoingRepository repository = new OutGoingRepository();
        DataTable dt = new DataTable();
        private string DocEntry { get; set; }

        private string ObjectName = string.Empty;

        private string OtherParams = string.Empty;

        string oDocNum;

        int max_width = Screen.PrimaryScreen.Bounds.Width - 220;
        int max_height = Screen.PrimaryScreen.Bounds.Height - 200;
        static int index = 0;

        public frmOutGoingPacklist()
        {
            InitializeComponent();
            hana = new SAPHanaAccess();
            msSql = new SAPMsSqlAccess();
            helper = new DataHelper();
            repository.dgv = dgvItems;
        }

        private void frmOutGoingPacklist_Load(object sender, EventArgs e)
        {
            txtDocNum.Text = repository.GenerateDocNum();
            repository.LinesColumn();

            CmbCompanyTIN.DisplayMember = "Name";
            CmbCompanyTIN.ValueMember = "Code";

            CmbCompanyTIN.DataSource = hana.Get("SELECT Name, Code FROM [@CMP_INFO]");
        }

        private void frmOutGoingPacklist_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            { Dispose(); }
            else
            { e.Cancel = true; }
        }

        private void frmOutGoingPacklist_Resize(object sender, EventArgs e)
        {
            FormHelper.ResizeForm(this);
        }

        private void pbSearchCardCode_Click(object sender, EventArgs e)
        {
            var bp = repository.ModalShow("OCRD***", "", "List of Business Partner");

            if (bp.Count > 0)
            {
                var dt = repository.GetBpInfo(bp[0].ToString());

                foreach (DataRow row in dt.Rows)
                {
                    OutgoingPackingListModel.packinglist.Clear();
                    txtCardCode.Text = row["CardCode"].ToString();
                    txtCardName.Text = row["CardName"].ToString();
                    txtAddress.Text = row["Address2"].ToString();
                }
                loadLines();
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            var form = new frmOutGoingPackingList_ItemList();
            form.ShowDialog();

            loadLines();
        }

        private void pbDepartment_Click(object sender, EventArgs e)
        {
            var dept = repository.ModalShow("dept*", "", "List of Department");

            txtDepartment.Text = dept.Count > 0 ? dept[0].ToString() : txtDepartment.Text;

            if (dgvItems.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    row.Cells[9].Value = txtDepartment.Text;
                }
            }
        }

        private void pbBrand_Click(object sender, EventArgs e)
        {
            var brand = repository.ModalShow("@OPKL - Automate Brand", txtCardCode.Text, "List of Brands");

            txtBrand.Text = brand.Count > 0 ? brand[0].ToString() : txtBrand.Text;
            //txtShipper.Text = repository.AutomateShipper(txtCardCode.Text, txtBrand.Text);
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowIndex = dgvItems.CurrentRow.Index;
            int index = Convert.ToInt32(dgvItems.CurrentRow.Cells["Index"].Value);

            dgvItems.Rows.RemoveAt(rowIndex);
            //OutgoingPackingListModel.packinglist.Find(x => x.Index == index).Status = "N";
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

        private void pbFindDocument_Click(object sender, EventArgs e)
        {
            var dtRows = new DataTable();

            DocEntry = null;

            if (cmbDocumentType.Text != string.Empty && txtCardCode.Text != string.Empty)
            {
                switch (cmbDocumentType.Text)
                {
                    case "SI":

                        var si = repository.ModalShow("SearchArInvoiceOutright", txtCardCode.Text, "List of Documents");

                        if (si.Count > 0)
                        {
                            oDocNum = si[0].ToString();
                            txtDocRef.Text = si[0].ToString();
                            txtDocumentNo.Text = si[0].ToString();

                            ObjectName = "INV";

                            dtRows = repository.GetItem(oDocNum, "98", "INV");
                        }
                        break;

                    case "DR":

                        var dr = repository.ModalShow("Outgoing Packinglist - Get DR no.", txtCardCode.Text, "List of Documents");

                        if (dr.Count > 0)
                        {
                            DocEntry = dr[0].ToString();
                            txtDocRef.Text = dr[0].ToString();
                            txtDocumentNo.Text = dr[0].ToString();

                            string table = dr[1].ToString() == "IT" ? "WTR" : "INV";
                            txtAddress.Text = repository.AutomateShipTo(DocEntry, table);

                            ObjectName = table;

                            oDocNum = DocEntry;

                            OtherParams = "Dr";

                            dtRows = repository.GetDrItem(DocEntry, table);
                        }
                        break;

                    case "CST":

                        var cst = repository.ModalShow("SearchDrConcession", txtCardCode.Text, "List of Documents");

                        if (cst.Count > 0)
                        {
                            oDocNum = cst[0].ToString();
                            txtDocRef.Text = oDocNum;
                            txtDocumentNo.Text = oDocNum;
                            string strSeries = cst[1].ToString();

                            OtherParams = strSeries;

                            ObjectName = "WTR";

                            dtRows = repository.GetItem(oDocNum, strSeries, "WTR");
                        }
                        break;
                }

                if (dtRows.Rows.Count > 0)
                {
                    txtBox.Text = repository.AutomateBox(txtDocRef.Text, txtCardCode.Text);
                    PopulatePackingList(dtRows, "N");
                }
                else
                {
                    OutgoingPackingListModel.packinglist.Clear();
                }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Kindly Select a Business Partner or check your document type", true);
            }
        }

        private void btnFindPurchaseOrder_Click(object sender, EventArgs e)
        {
            ClearFields();
            btnAdd.Text = "Update";
            index = 0;

            var findList = repository.ModalShow("findPackinglist", txtCardCode.Text, "List of Business Partner");

            if (findList.Count > 0)
            {
                txtDocEntry.Text = findList[0].ToString();
                txtDocNum.Text = findList[1].ToString();

                var header = repository.loadHeader(txtDocEntry.Text);

                foreach (DataRow head in header.Rows)
                {
                    txtRemark.Text = head[0].ToString();
                    txtPONo.Text = head[1].ToString();
                    txtCardName.Text = head[2].ToString();
                    txtAddress.Text = head[3].ToString();
                    txtBox.Text = head[4].ToString();
                    txtLength.Text = head[5].ToString();
                    txtHeight.Text = head[6].ToString();
                    txtBranchCode.Text = head[7].ToString();
                    CmbCompanyTIN.Text = head[8].ToString();
                    txtDocumentNo.Text = head[9].ToString();
                    txtDepartment.Text = head[10].ToString();
                    txtCardCode.Text = head[11].ToString();
                    txtWeight.Text = head[12].ToString();
                    txtTotalBox.Text = head[13].ToString();
                    txtWidth.Text = head[14].ToString();
                    cmbDocumentType.Text = head[15].ToString();
                    dtpDocument.Text = head[16].ToString();
                    txtDocRef.Text = head[17].ToString();
                    txtBrand.Text = head[18].ToString();
                }

                dt = repository.loadItems(txtDocEntry.Text);

                PopulatePackingList(dt, "Y");

                loadLines();
                OutgoingPackingListModel.packinglist.Clear();
            }
        }

        private void DigitOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var head = new Dictionary<string, string>();

            head.Add("Remark", txtRemark.Text);
            head.Add("U_PONo", txtPONo.Text);
            head.Add("U_CardName", txtCardName.Text);
            head.Add("U_ShipTo", txtAddress.Text);
            head.Add("U_Box", txtBox.Text);
            head.Add("U_Length", ValidateInput.Double(txtLength.Text).ToString());
            head.Add("U_Height", ValidateInput.Double(txtHeight.Text).ToString());
            head.Add("U_BranchCode", txtBranchCode.Text);
            head.Add("U_Shipper", CmbCompanyTIN.Text);
            head.Add("U_SIDRNo", txtDocumentNo.Text);
            head.Add("U_Department", txtDepartment.Text);
            head.Add("U_CardCode", txtCardCode.Text);
            head.Add("U_Weight", ValidateInput.Double(txtWeight.Text).ToString());
            head.Add("U_TotalBox", txtTotalBox.Text);
            head.Add("U_Width", ValidateInput.Double(txtWidth.Text).ToString());
            head.Add("U_Type", cmbDocumentType.Text);
            head.Add("U_Date", dtpDocument.Value.ToString("yyyyMMdd"));
            head.Add("U_DocRef", txtDocRef.Text);
            head.Add("U_Brand", txtBrand.Text);
            head.Add("U_DRDesc", CmbDrDescription.Text);

            var dictLines = new List<Dictionary<string, object>>();

            foreach (DataGridViewRow dt in dgvItems.Rows)
            {
                var lines = new Dictionary<string, object>();
                lines.Add("LineId", dt.Index);
                lines.Add("U_DocNum", ValidateInput.Int(dt.Cells["Document No."].Value));
                lines.Add("U_ItemCode", ValidateInput.String(dt.Cells["Item Code"].Value));
                lines.Add("U_ItemName", ValidateInput.String(dt.Cells["Description"].Value));
                lines.Add("U_Barcode", ValidateInput.String(dt.Cells["Barcode"].Value));
                lines.Add("U_Quantity", ValidateInput.String(dt.Cells["Quantity"].Value));
                lines.Add("U_Cost", ValidateInput.String(dt.Cells["Cost"].Value));
                lines.Add("U_Indication", ValidateInput.String(dt.Cells["Indication"].Value));
                lines.Add("U_Data", ValidateInput.String(dt.Cells["Data"].Value));
                lines.Add("U_Brand", ValidateInput.String(dt.Cells["Brand"].Value));
                lines.Add("U_Department", ValidateInput.String(dt.Cells["Department"].Value));

                // fk's
                lines.Add("U_BaseType", ObjectName);
                lines.Add("U_BaseRef", OtherParams); // series if cst or dr
                lines.Add("U_BaseLine", oDocNum);

                dictLines.Add(lines);
            }

            var json = DataRepository.JsonBuilder(head, dictLines, "PKL1Collection");

            var returnvalue = string.Empty;

            var isPosted = false;

            var serviceLayerAccess = new ServiceLayerAccess();

            if (btnAdd.Text == "Add")
            {
                string url = $"OPKL";
                isPosted = serviceLayerAccess.ServiceLayer_Posting(json, "POST", $"OPKL", "DocEntry", out returnvalue, out string value);
            }
            else
            {
                string url = $"OPKL({txtDocEntry.Text})";
                isPosted = serviceLayerAccess.ServiceLayer_Posting(json, "PATCH", url, "DocEntry", out returnvalue, out string value);
            }

            if (isPosted)
            {
                ClearFields();
                StaticHelper._MainForm.ShowMessage("Operation completed successfully");
            }
            else
            {
                StaticHelper._MainForm.ShowMessage(returnvalue, true);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Are you sure you want to close the Document?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Dispose();
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (dgvItems.RowCount > 0)
            {
                var list = OutgoingPackingListModel.packinglist
                        .Where(x => x.Status == "Y")
                        .OrderBy(x => x.SortCode)
                        .Select(x => new
                        {
                            x.SortCode,
                            DocumentNo = x.DocoumnetNumber,
                            x.ItemCode,
                            x.Description,
                            x.Quantity,
                            x.Brand,
                            x.Color,
                            x.Size,
                            x.Barcode,
                            x.Cost,
                            x.Indication,
                            x.Data,
                            x.Department,
                            x.Index
                        }).ToList();

                var preview = new FrmPreviewItenlist(DataRepository.ToDataTable(list));
                preview.ShowDialog();

                loadLines();
            }
        }

        private void btnNewDocument_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            var a = new frmCrystalReports();
            a.type = "Outright";
            a.oDocKey = txtDocEntry.Text;
            a.ShowDialog();
        }

        // private functions 

        void ClearFields()
        {
            foreach (Control c in panel2.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
            }

            if (dgvItems.RowCount > 0)
            {
                dgvItems.Rows.Clear();
            }

            txtDocNum.Text = repository.GenerateDocNum();
            cmbDocumentType.Text = "";
            txtLength.Text = "0";
            txtHeight.Text = "0";
            txtWeight.Text = "0";
            txtWidth.Text = "0";
            txtBox.Text = "1";
            txtTotalBox.Text = "1";
            btnAdd.Text = "Add";

            OutgoingPackingListModel.packinglist.Clear();
        }

        void PopulatePackingList(DataTable dt, string status)
        {
            OutgoingPackingListModel.packinglist.Clear();

            foreach (DataRow row in dt.Rows)
            {
                OutgoingPackingListModel.packinglist.Add(new OutgoingPackingListModel.OutgoingPackingList
                {
                    Index = index++,
                    DocoumnetNumber = row["Document No."].ToString(),
                    ItemCode = row["ItemCode"].ToString(),
                    Description = row["Description"].ToString(),
                    Barcode = row["Barcode"].ToString(),
                    Cost = row["Cost"].ToString(),
                    Indication = row["Indication"].ToString(),
                    Quantity = row["Quantity"].ToString(),
                    Data = row["Data"].ToString(),
                    Brand = row["Brand"].ToString(),
                    Department = row["Department"].ToString(),
                    Color = row["Color"].ToString(),
                    Size = row["Size"].ToString(),
                    Status = status,
                    SortCode = row["SortCode"].ToString(),
                    Available = row["Available"].ToString(),
                });
            }
        }

        void loadLines()
        {
            //if (dgvItems.RowCount > 0)
            //{
            //    dgvItems.Rows.Clear();
            //}

            foreach (var row in OutgoingPackingListModel.packinglist.Where(x => x.Status == "Y").OrderBy(x => x.SortCode).ToList())
            {
                int index = row.Index;
                string sortCode = row.SortCode;
                string docNum = row.DocoumnetNumber;
                string itemcode = row.ItemCode;
                string itemName = row.Description;
                string barCode = row.Barcode;
                string qty = row.Available;
                string cost = row.Cost;
                string indication = row.Indication;
                string data = row.Data;
                string brand = row.Brand;
                string dept = row.Department;
                string color = row.Color;
                string size = row.Size;
                var isUpdate = false;

                foreach (DataGridViewRow item in dgvItems.Rows)
                {
                    if (item.Cells[2].Value != null)
                    {
                        if (item.Cells[2].Value.ToString() == itemcode)
                        {
                            if (item.Cells[4].Value != null)
                            {
                                //On Comment, conflict on process by Darrel 080819
                                //item.Cells[4].Value = double.Parse(qty) + double.Parse(string.IsNullOrEmpty(item.Cells[4].Value.ToString()) ? "0" : item.Cells[4].Value.ToString());
                                item.Cells[4].Value = double.Parse(string.IsNullOrEmpty(item.Cells[4].Value.ToString()) ? "0" : item.Cells[4].Value.ToString());
                            }
                            
                            isUpdate = true;
                            break;
                        }                   
                    }
                }

                if (isUpdate == false)
                {
                    dgvItems.Rows.Add(sortCode, docNum, itemcode, itemName, qty, brand, color, size, barCode, cost, indication, data, dept, index);
                }                
            }
        }
    }
}

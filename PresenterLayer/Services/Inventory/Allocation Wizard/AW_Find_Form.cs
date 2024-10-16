using Context;
using DirecLayer;
using DomainLayer.Models.Inventory.Allocation_Wizard;
using PresenterLayer;
using PresenterLayer.Helper;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PresenterLayer
{
    public class AW_Find_Form
    {
        public frmAW_Find find { get; set; }
        public frmAllocationWizard form { get; set; }
        public string sQuery { get; set; }
        public DataGridViewRow dgvdr { get; set; }
        public string TableID { get; set; }
        public string Type { get; set; }
        string iColumn { get; set; } = "Code";

        public void dgvSetup(DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.RowTemplate.Resizable = DataGridViewTriState.False;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.MultiSelect = false;

            var dt = new DataTable();
            var sapHana = new SAPHanaAccess();
            dt = sapHana.Get(sQuery);

            dgv.DataSource = dt;
        }

        public void Choose()
        {
            int i = find.dgvSearch.SelectedRows[0].Index;
            if (i < 0)
            { StaticHelper._MainForm.ShowMessage("No matching records found", true); }
            else
            {
                string newValue = find.dgvSearch.Rows[i].Cells[0].Value.ToString();
                if (dgvdr.Cells["Value"].Value.ToString() != newValue)
                {
                    dgvdr.Cells["Value"].Value = newValue;

                    var dgv = form.dgvItemOtherParam;
                    switch (dgvdr.Index)
                    {
                        case 0:
                            dgv.Rows[1].Cells["Value"].Value = string.Empty;
                            dgv.Rows[2].Cells["Value"].Value = string.Empty;
                            dgv.Rows[3].Cells["Value"].Value = string.Empty;
                            dgv.Rows[4].Cells["Value"].Value = string.Empty;
                            dgv.Rows[5].Cells["Value"].Value = string.Empty;
                            break;
                        case 1:
                            dgv.Rows[2].Cells["Value"].Value = string.Empty;
                            dgv.Rows[3].Cells["Value"].Value = string.Empty;
                            dgv.Rows[4].Cells["Value"].Value = string.Empty;
                            dgv.Rows[5].Cells["Value"].Value = string.Empty;
                            break;
                        case 2:
                            dgv.Rows[3].Cells["Value"].Value = string.Empty;
                            dgv.Rows[4].Cells["Value"].Value = string.Empty;
                            dgv.Rows[5].Cells["Value"].Value = string.Empty;
                            break;
                        case 3:
                            dgv.Rows[4].Cells["Value"].Value = string.Empty;
                            dgv.Rows[5].Cells["Value"].Value = string.Empty;
                            var modelIOP = SelectionModel.Selection.Where(x => x.Type == "IOP");
                            if (modelIOP.Any())
                            {
                                foreach (var item in modelIOP.ToList())
                                { SelectionModel.Selection.Remove(item); }

                            }
                            // Clear Data
                            break;
                        case 4:
                            dgv.Rows[5].Cells["Value"].Value = string.Empty;
                            break;
                        default:
                            break;
                    }

                }
                var dr = SelectionModel.Selection.Where(x => x.TableID == TableID && x.Type == Type).FirstOrDefault();
                if (dr != null)
                {
                    dgvTick(TableID, Type, dr.Type, false);
                }
                dgvTick(TableID, Type, newValue, true);
                find.Dispose();
            }
        }

        public void dgvTick(string TableID, string Type, string ID, bool val)
        {
            var model = SelectionModel.Selection.Where(x => x.TableID == TableID && x.ID == ID && x.Type == Type);

            if (model.Count() > 0)
            {
                if (val)
                { model.FirstOrDefault().Choose = val; }
                else { SelectionModel.Selection.Remove(model.FirstOrDefault()); }
            }
            else
            {
                if (val)
                {
                    SelectionModel.Selection.Add(new SelectionModel.SelectionData
                    {
                        Choose = val,
                        TableID = TableID,
                        ID = ID,
                        Type = Type
                    });
                }
            }
        }

        public void txtSearch_TextChanged()
        {
            for (int i = 0; i < find.dgvSearch.Rows.Count; i++)
            {
                var row = find.dgvSearch.Rows[i];
                if (string.IsNullOrEmpty(iColumn))
                { iColumn = "Code"; }

                if (row.Cells[iColumn].Value.ToString().ToUpper().Contains(find.txtSearch.Text.ToUpper()))
                {
                    row.Selected = true;
                    find.dgvSearch.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
                else
                { row.Selected = false; }
            }
            
        }

        public void dgvSearch_ColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (find.dgvSearch.Rows.Count > 0)
            { iColumn = find.dgvSearch.Columns[e.ColumnIndex].Name; }
        }
    }
}
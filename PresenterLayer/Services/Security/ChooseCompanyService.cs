using InfrastructureLayer.Repository;
using PresenterLayer.Helper;
using PresenterLayer.Views.Security;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresenterLayer.Services.Security
{
    public class ChooseCompanyService
    {
        public ChooseCompanyService()
        {

        }

        public void SelectionCompanyChange()
        {
            var dgv = IChooseCompany._CompaniesDataGridView;

            if (dgv.CurrentRow.Index != -1)
            { IChooseCompany.CompanyName = dgv.Rows[dgv.CurrentRow.Index].Cells["Database"].Value.ToString(); }
        }

        public void DataGridViewSetup()
        {
            var output = IChooseCompany._CompaniesDataGridView;
            try
            {
                output.ColumnCount = 4;
                output.Columns[0].Name = "Company Name";
                output.Columns[1].Name = "Database";
                output.Columns[2].Name = "Localization";
                output.Columns[3].Name = "Version";
                output.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                output.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                output.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                output.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                output.ReadOnly = true;
                output.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                output.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                output.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                output.MultiSelect = false;
                output.RowTemplate.Resizable = DataGridViewTriState.False;
                output.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                output.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                output.RowHeadersVisible = false;
                output.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8);
                output.DefaultCellStyle.Font = new Font("Tahoma", 7, GraphicsUnit.Point);
                output.DefaultCellStyle.SelectionBackColor = Color.FromArgb(181, 213, 253);
                output.DefaultCellStyle.SelectionForeColor = Color.Black;
                output.DefaultCellStyle.BackColor = Color.FromArgb(231, 231, 231);
                output.DefaultCellStyle.ForeColor = Color.Black;

                output.Rows.Clear();
                
                var db = new ChooseCompanyRepository();

                foreach (var dr in db.GetCompanies())
                {
                    string[] row = new string[] { dr.CompanyName, dr.Database, dr.Localization, dr.Version };
                    output.Rows.Add(row);
                }


            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        public bool CancelClick()
        {
            var output = false;
            try
            {
                if (StaticHelper._MainForm.menuStrip.Enabled == false)
                {
                    FormHelper.ShowForm(new LoginForm());
                    output = true;
                }
                else { output = true; }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
            return output;
        }

        public void SearchCompany()
        {
            if (IChooseCompany.Search.Length >= 3)
            {
                var dgv = IChooseCompany._CompaniesDataGridView;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[IChooseCompany.ColumnSearch].Value.ToString().Contains(IChooseCompany.Search))
                    {
                        row.Selected = true;
                        IChooseCompany.CompanyName = dgv.Rows[row.Index].Cells["Database"].Value.ToString();
                        break;
                    }
                    else
                    { row.Selected = false; }
                }
            }
        }
    }
}

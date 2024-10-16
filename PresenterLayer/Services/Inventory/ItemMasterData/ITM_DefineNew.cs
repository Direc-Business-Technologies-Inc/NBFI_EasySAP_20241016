using Context;
using MetroFramework;
using PresenterLayer;
using PresenterLayer.Helper;
using PresenterLayer.Views;
using ServiceLayer.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PresenterLayer.Services
{
    class ITM_DefineNew
    {
        public frmITM_DefineNew frmITM_DefineNew { get; set; }
        public ITM_ChooseFromList cfl { get; set; }
        private ServiceLayerAccess slAccess = new ServiceLayerAccess();
        public void Form_Load()
        {
            dgvSetup();
            frmITM_DefineNew.dgvDefine.Focus();
        }

        public void Form_Cancel()
        { frmITM_DefineNew.Dispose(); }

        void dgvSetup()
        {
            try
            {
                var dt = frmITM_DefineNew.dgvDefine;
                dt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dt.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dt.MultiSelect = false;
                dt.RowTemplate.Resizable = DataGridViewTriState.False;
                dt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dt.RowHeadersVisible = false;
                dt.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
                dt.DefaultCellStyle.Font = new Font("Arial", 7, GraphicsUnit.Point);

                DataGridViewTextBoxColumn col0 = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
                DataGridViewCheckBoxColumn col2 = new DataGridViewCheckBoxColumn();

                dt.Columns.Add(col0);
                dt.Columns.Add(col1);
                dt.Columns.Add(col2);

                dt.Columns[0].ReadOnly = true;
                dt.Columns[1].ReadOnly = true;
                if (frmITM_DefineNew.Text == "Classes" || frmITM_DefineNew.Text == "Sub-Classes")
                {
                    dt.Columns[0].Name = "Class";
                    dt.Columns[1].Name = "Subclass";
                }
                else
                {
                    dt.Columns[0].Name = "Code";
                    dt.Columns[1].Name = "Name";
                }
                dt.Columns[2].Name = "Tag";
                dt.Columns[2].Visible = false;

                dt.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
            }
            catch (Exception ex)
            { StaticHelper._MainForm.ShowMessage(ex.Message, true); }
        }

        public void DefindNewSearch(string sCode, string sName)
        {
            var dgv = frmITM_DefineNew.dgvDefine;
            if (dgv.Rows.Count >= 1)
            {
                int iRow = dgv.Rows.Count - 1;
                dgv.CurrentCell = dgv.Rows[iRow].Cells[1];
                dgv.FirstDisplayedScrollingRowIndex = iRow;

                if (frmITM_DefineNew.Text == "Styles" || frmITM_DefineNew.Text == "Packaging" || frmITM_DefineNew.Text == "Specifications" || frmITM_DefineNew.Text == "Collections")
                {
                    if (frmITM_DefineNew.Text != "Styles")
                    { dgv.Columns[0].Visible = false; }
                    else
                    { dgv.Rows[iRow].Cells[0].ReadOnly = false; }

                    dgv.Rows[iRow].Cells[1].ReadOnly = false;
                }
                else
                {
                    dgv.Rows[iRow].Cells[0].ReadOnly = false;
                    dgv.Rows[iRow].Cells[1].ReadOnly = false;
                }

                dgv.Rows[iRow].Cells[0].Value = sCode;
                dgv.Rows[iRow].Cells[1].Value = sName;

                if (frmITM_DefineNew.Text == "Classes" || frmITM_DefineNew.Text == "Styles")
                { dgv.CurrentCell = dgv.Rows[iRow].Cells[0]; }
                else
                { dgv.CurrentCell = dgv.Rows[iRow].Cells[1]; }
                dgv.BeginEdit(true);
            }

        }

        public void dgv_CellEndEdit()
        {
            var dgv = frmITM_DefineNew.dgvDefine;
            frmITM_DefineNew.btnCommand.Text = "&Update";
            dgv.Rows[dgv.CurrentCell.RowIndex].Cells["Tag"].Value = true;
            int iRow = dgv.Rows.Count - 1;

            if (frmITM_DefineNew.Text == "Packaging" || frmITM_DefineNew.Text == "Specifications" || frmITM_DefineNew.Text == "Collections")
            {
                dgv.Rows[iRow].Cells[0].Value = dgv.Rows[iRow].Cells[1].Value;
            }
        }

        public void AddValue()
        {
            if (frmITM_DefineNew.btnCommand.Text == "&OK")
            { frmITM_DefineNew.Dispose(); }
            else
            {
                bool ret = false;
                string sErr = "";
                StaticHelper._MainForm.lblStatus.Text = "Please wait...";
                try
                {
                    var dr = frmITM_DefineNew.dgvDefine.Rows[frmITM_DefineNew.dgvDefine.Rows.Count - 1];
                    if (bool.Parse(dr.Cells["Tag"].Value.ToString()))
                    {
                        if (!string.IsNullOrEmpty(dr.Cells[0].Value.ToString()))
                        {
                            try
                            {
                                string Code = dr.Cells[0].Value.ToString();
                                string Name = dr.Cells[1].Value.ToString();

                                var sbJson = new StringBuilder();
                                sbJson.AppendLine("{");

                                if (frmITM_DefineNew.Text == "Styles")
                                {
                                    sbJson.AppendLine(@" ""OSTLCollection"": [");
                                    sbJson.AppendLine("   {");
                                    sbJson.AppendLine($@"     ""U_Code"": ""{Code}"",");
                                    sbJson.AppendLine($@"     ""U_Style"": ""{Name}"",");
                                    sbJson.AppendLine($@"     ""U_Series"": ""{int.Parse(Code)}""");
                                }
                                else if (frmITM_DefineNew.Text == "Classes" || frmITM_DefineNew.Text == "Sub-Classes")
                                {
                                    sbJson.AppendLine(@" ""OCLSCollection"": [");
                                    sbJson.AppendLine("   {");
                                    sbJson.AppendLine($@"     ""U_Class"": ""{Code}"",");
                                    sbJson.AppendLine($@"     ""U_SubClass"": ""{Name}""");
                                }
                                else if (frmITM_DefineNew.Text == "Packaging")
                                {
                                    sbJson.AppendLine(@" ""OPCKCollection"": [");
                                    sbJson.AppendLine("   {");
                                    sbJson.AppendLine($@"     ""U_PackBund"": ""{Code}""");
                                }
                                else if (frmITM_DefineNew.Text == "Specifications")
                                {
                                    sbJson.AppendLine(@" ""OSPCCollection"": [");
                                    sbJson.AppendLine("   {");
                                    sbJson.AppendLine($@"     ""U_Spec"": ""{Code}""");
                                }
                                else if (frmITM_DefineNew.Text == "Collections")
                                {
                                    sbJson.AppendLine(@" ""OCLTCollection"": [");
                                    sbJson.AppendLine("   {");
                                    sbJson.AppendLine($@"     ""U_Col"": ""{Code}""");
                                }

                                sbJson.AppendLine("   }");
                                sbJson.AppendLine("  ]");
                                sbJson.AppendLine("}");

                                var serviceLayerAccess = new ServiceLayerAccess();
                               ret = serviceLayerAccess.ServiceLayer_Posting(sbJson, "PATCH", $"OSBC('{cfl.SubCategoryCode}')", "Code", out sErr, out string val);
                            }
                            catch (Exception ex)
                            { StaticHelper._MainForm.ShowMessage(sErr = ex.Message, true); }
                        }
                        else
                        {
                            sErr = "Please include code column.";
                        }
                    }
                }
                catch (Exception ex)
                { StaticHelper._MainForm.ShowMessage(sErr = ex.Message, true); }

                if (ret)
                {
                    cfl.frmCFL.Dispose();
                    switch (frmITM_DefineNew.Text)
                    {
                        case "Styles":
                            cfl.GetStyles();
                            break;
                        case "Classes":
                            cfl.GetClass();
                            break;
                        case "Sub-Classes":
                            cfl.GetSubClass();
                            break;
                        case "Packaging":
                            cfl.GetPackaging();
                            break;
                        case "Specifications":
                            cfl.GetSpecifications();
                            break;
                        case "Collections":
                            cfl.GetColections();
                            break;
                    }
                    cfl.GetLastRecord();
                    frmITM_DefineNew.Dispose();

                    frmITM_DefineNew.btnCommand.Text = "&OK";
                }
                else
                { StaticHelper._MainForm.ShowMessage(sErr, true); }

                StaticHelper._MainForm.lblStatus.Text = string.Empty;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using DirecLayer;
using PresenterLayer.Views.Main;
using DomainLayer;
using PresenterLayer.Services;
using PresenterLayer.Helper;

namespace PresenterLayer.Views
{
    public partial class UcTemplateCreation : UserControl
    {
        frmDT frmDt { get; set; }
        MainForm frmMain { get; set; }

        SAOContext model = new SAOContext();
        DtDgvController dgvSetup = new DtDgvController();
        Excel ex = new Excel();

        DataSet DatasetResult { get; set; }

        public UcTemplateCreation(frmDT frmDt, MainForm frmMain)
        {
            InitializeComponent();

            this.frmDt = frmDt;
            this.frmMain = frmMain;
        }

        private void UcTemplateCreation_Load(object sender, EventArgs e)
        {
            dgvSetup.DataGridViewMapColumns(DgvMap);
        }

        private void BtnFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog odf = new OpenFileDialog())
            {
                if (odf.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fs = new FileStream(odf.FileName, FileMode.Open, FileAccess.Read))
                        {
                            TxtTemplate.Text = odf.FileName;

                            if (ex.ReadExcel((ds) => DatasetResult = ds, fs, Path.GetExtension(odf.FileName)))
                            {
                                ex.CloseExcel();
                            }

                            CmbWorkSheet.Items.Clear();
                            CmbWorkSheet.Text = string.Empty;

                            foreach (DataTable dt in DatasetResult.Tables)
                            {
                                CmbWorkSheet.Items.Add(dt.TableName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void CmbWorkSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = DatasetResult.Tables[CmbWorkSheet.SelectedIndex];
            DgvExcel.DataSource = null;
            DgvExcel.DataSource = dt;
        }

        private void DgvMap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && CmbUploadType.Text != string.Empty)
            {
                frmColumnSearch form = new frmColumnSearch();

                form.UploadType = CmbUploadType.Text;
                form.ShowDialog();

                DgvMap.Rows[e.RowIndex].Cells[0].Value = form.FieldName;
                DgvMap.Rows[e.RowIndex].Cells[2].Value = form.Type;
            }
        }

        private void btnFindPurchaseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtMapName.Text != string.Empty)
                {
                    var parent = new DTheader
                    {
                        MapCode = TxtMapName.Text,
                        MapDescription = TxtMapDescription.Text,
                        UploadType = CmbUploadType.Text,
                        Dtrows = new List<DTrow>()
                    };

                    int count = 1;
                    var isCardCode = false;
                    foreach (DataGridViewRow gvRow in DgvMap.Rows)
                    {
                        if (DgvMap.RowCount != count)
                        {
                            if (gvRow.Cells[0].Value.ToString() == "CardCode")
                            {
                                isCardCode = true;
                            }
                            parent.Dtrows.Add(new DTrow
                            {
                                SapField = gvRow.Cells[0].Value.ToString(),
                                Type = gvRow.Cells[2].Value.ToString(),
                                RowStart = Convert.ToInt32(gvRow.Cells[3].Value),
                                ColumnStart = Convert.ToInt32(gvRow.Cells[4].Value),
                                Flow = gvRow.Cells[5].Value.ToString(),
                                RowInterval = Convert.ToInt32(gvRow.Cells[6].Value),
                                ColumnInterval = Convert.ToInt32(gvRow.Cells[7].Value),
                            });
                        }

                        count++;
                    }

                    if (CmbUploadType.Text.Equals("Carton Packing List") == true || CmbUploadType.Text.Equals("A/R Credit Memo") == true)
                    {
                        model.dtheader.Add(parent);
                        model.SaveChanges();

                        ClearFields();
                    }
                    else if (isCardCode)
                    {
                        model.dtheader.Add(parent);
                        model.SaveChanges();

                        ClearFields();
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage("Please do not save mappings without cardcode.");
                    }
                    //PublicStatic.frmMain.NotiMsg($"New Mapping has been successfully created", System.Drawing.Color.Green);
                }
                else
                {
                    MessageBox.Show("Please Enter Template Name");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            frmDt.pnlContainer.Controls.Clear();
            frmDt.pnlContainer.Controls.Add(new UcMainMenu(frmDt, frmMain));
        }

        private void ClearFields()
        {
            foreach (Control c in panel1.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
                else if (c is ComboBox)
                {
                    c.Text = "";
                }
            }

            DgvExcel.DataSource = null;
            DgvMap.Rows.Clear();
            dgvSetup.DataGridViewMapColumns(DgvMap);
        }

        private void DgvMap_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int lastRow = DgvMap.RowCount;

            if (e.Button == MouseButtons.Right && lastRow != 1)
            {
                if (e.RowIndex != -1)
                {
                    DgvMap.Rows[e.RowIndex].Selected = true;
                    DgvMap.Focus();

                    var mousePosition = DgvMap.PointToClient(Cursor.Position);

                    msItems.Show(DgvMap, mousePosition);
                }
            }
        }

        private void deleteItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = DgvMap.RowCount - 1;

            if (DgvMap.CurrentRow.Index != count)
            {
                DgvMap.Rows.RemoveAt(DgvMap.CurrentRow.Index);
            }
            else
            {
                DgvMap.CurrentRow.Cells[0].Value = "";
                DgvMap.CurrentRow.Cells[2].Value = "";
                DgvMap.CurrentRow.Cells[3].Value = "";
                DgvMap.CurrentRow.Cells[4].Value = "";
                DgvMap.CurrentRow.Cells[5].Value = "";
                DgvMap.CurrentRow.Cells[6].Value = "";
            }
        }

        private void DgvExcel_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvMap.SelectedRows.Count > 0)
            {
                DgvMap.SelectedRows[0].Cells["Row Start"].Value = e.RowIndex + 1;
                DgvMap.SelectedRows[0].Cells["Column Start"].Value = e.ColumnIndex + 1;
            }
        }

        private void TxtMapName_TextChanged(object sender, EventArgs e)
        {
            if (TxtMapName.TextLength < 20)
            {
                TxtMapName.Text = TxtMapName.Text;
            }
            else
            {
                string s = TxtMapName.Text.Substring(0, TxtMapName.Text.Length - 1);

                TxtMapName.Text = s;
            }
        }

        private void DgvExcel_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(DgvExcel.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

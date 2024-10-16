using System;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using PresenterLayer.Services;

namespace PresenterLayer.Views
{
    public partial class frmColumnSearch : MetroForm
    {
        public MainMenu frmMain { get; set; }
        //public frmDataConfig frmdataConfig { get; set; }
        public string UploadType { get; set; }
        public string FieldName { get; set; }
        public string Type { get; set; }
        private string[] OptionType { get; set; } = { "Header", "Row" };
        private static int defaultColumn { get; set; } = 1;
        private static int _rowIndex {get;set;} = 0;

        UploadController controller = new UploadController();
         
        public frmColumnSearch()
        {
            InitializeComponent();

            cmbOptionType.Text = "Header";
            OptionType.ToList().ForEach(x => {

                cmbOptionType.Items.Add(x);
            });
        }

        private void frmSearchColumns_Load(object sender, EventArgs e)
        {
            dgvSapFields.DataSource = null;
            controller.tableName = UploadType;
            dgvSapFields.DataSource = controller.GetUploadType();
            dgvSapFields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgvSapFields_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            defaultColumn = e.ColumnIndex;
        }

        private void dgvSapFields_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        
        private void txtExcelName_TextChanged(object sender, EventArgs e)
        {
            if (dgvSapFields.Columns.Count > 1)
            {
                foreach (DataGridViewRow row in dgvSapFields.Rows)
                {
                    try
                    {
                        if (row.Cells[defaultColumn].Value.ToString().ToUpper().Contains(txtSearch.Text.ToUpper()))
                        {
                            row.Selected = true;
                            _rowIndex = row.Index;
                            dgvSapFields.FirstDisplayedScrollingRowIndex = _rowIndex;
                            break;
                        }
                        else
                        {
                            row.Selected = false;
                        }
                    }
                    catch { }
                }
            }
        }
        
        private void cmbOptionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvSapFields.DataSource = null;
            controller.tableName = UploadType;
            
            switch (cmbOptionType.Text)
            {
                case "Header":
                    dgvSapFields.DataSource = controller.GetUploadType();
                    break;

                case "Row":
                    dgvSapFields.DataSource = controller.GetUploadRowType();
                    break;
            }

            dgvSapFields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgvSapFields_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                FieldName = dgvSapFields.Rows[e.RowIndex].Cells[1].Value.ToString();
                Type = cmbOptionType.Text;
                Close();
            }
        }

        private void dgvSapFields_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            FieldName = dgvSapFields.CurrentRow.Cells[1].Value.ToString();
            Type = cmbOptionType.Text;
            Close();
        }
    }
}

using System;
using System.Data;
using System.Linq;
using MetroFramework.Forms;
using PresenterLayer.Views.Main;
using PresenterLayer.Helper;
using DomainLayer;

namespace PresenterLayer.Views
{
    public partial class FrmDt_Modal : MetroForm
    {
        SAOContext model = new SAOContext();

        public frmDT frmDt { get; set; }
        public MainForm frmMain { get; set; }
        public int Id { get; set; }

        public FrmDt_Modal()
        {
            InitializeComponent();
        }

        private void FrmDt_Modal_Load(object sender, EventArgs e)
        {
            TxtMapCode.Text = model.dtheader.ToList().Find(x => x.MapID == Id).MapCode;
            TxtMapDescription.Text = model.dtheader.ToList().Find(x => x.MapID == Id).MapDescription;
            TxtUploadType.Text = model.dtheader.ToList().Find(x => x.MapID == Id).UploadType;

            var data = model.dtrows.Where(x => x.HeaderID == Id).Select(x => new { x.SapField, x.Type, x.RowStart, x.ColumnStart, x.Flow, x.RowInterval, x.ColumnInterval }).ToList();

            DgvMap.DataSource = null;
            DgvMap.DataSource = data;
        }

        private void BtnProceed_Click(object sender, EventArgs e)
        {
            UcGeneric sadsa = new UcGeneric();

            sadsa.frmDt = frmDt;
            sadsa.frmMain = StaticHelper._MainForm;
            sadsa.MapCode = Id.ToString();

            frmDt.pnlContainer.Controls.Clear();
            frmDt.pnlContainer.Controls.Add(sadsa);

            Close();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

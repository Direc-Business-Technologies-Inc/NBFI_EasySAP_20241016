using Context;
using DirecLayer;
using PresenterLayer;
using PresenterLayer.Helper;
using PresenterLayer.Views;
using ServiceLayer.Services;
using System.Drawing;
using System.Text;

namespace PresenterLayer.Services
{
    class ITM_DefineNewCustom
    {
        public frmITM_DefineNewCustom definenew { get; set; }
        public frmItemMasterData imd { get; set; }
        SAPHanaAccess hana { get; set; }
        DataHelper helper { get; set; }
        public ITM_DefineNewCustom()
        {
            hana = new SAPHanaAccess();
            helper = new DataHelper();
        }

        public void Form_Close()
        { definenew.Dispose(); }

        public void Form_Load()
        {
            switch (definenew.Text)
            {
                case "Define New Color":
                    GetColors();
                    break;
                default:
                    GetSize();
                    break;
            }

            definenew.btnCommand.Focus();
        }

        public void Form_Add()
        {
            var sErr = "";
            var sModule = "";
            var sValue = "";
            var sbJson = new StringBuilder();
            sbJson.AppendLine("{");
            switch (definenew.Text)
            {
                case "Define New Color":
                    sModule = "OCLC";
                    sbJson.AppendLine(@" ""OCLRCollection"": [");
                    sbJson.AppendLine("   {");
                    sbJson.AppendLine($@"     ""U_Color"": ""{definenew.txtChildName.Text}""");
                    sValue = definenew.txtParentCode.Text;
                    break;
                default:
                    sModule = "OSZC";
                    sbJson.AppendLine(@" ""OSZSCollection"": [");
                    sbJson.AppendLine("   {");
                    sbJson.AppendLine($@"     ""U_Code"": ""{definenew.txtParentCode.Text}"",");
                    sbJson.AppendLine($@"     ""U_SizeID"": ""{definenew.txtParentCode.Text}"",");
                    sbJson.AppendLine($@"     ""U_ShortDesc"": ""{definenew.cbParentCode.SelectedText.ToString()}"",");
                    sbJson.AppendLine($@"     ""U_Size"": ""{definenew.txtChildName.Text}""");
                    sValue = imd.U_ID006.SelectedValue.ToString();
                    break;
            }
            sbJson.AppendLine("   }");
            sbJson.AppendLine("  ]");
            sbJson.AppendLine("}");

            var serviceLayerAccess = new ServiceLayerAccess();
            if (!serviceLayerAccess.ServiceLayer_Posting(sbJson, "PATCH", $"{sModule}('{sValue}')", "Code", out sErr, out string val))
            {
                StaticHelper._MainForm.ShowMessage(sErr, true);
            }
            else
            {
                switch (definenew.Text)
                {
                    case "Define New Color":
                        imd.form.color.LoadGridView();
                        break;
                    default:
                        imd.form.size.LoadSizeCategories();
                        break;
                }
                definenew.Dispose();
            }
        }


        public void Form_SelectedIndexChanged()
        {
            definenew.txtParentCode.Text = definenew.cbParentCode.SelectedValue.ToString();
        }

        public void GetColors()
        {
            var frm = definenew;
            frm.cbParentCode.DataSource = null;
            frm.cbParentCode.DataSource = hana.Get(SP.ITM_NewColor);
            frm.cbParentCode.ValueMember = "Code";
            frm.cbParentCode.DisplayMember = "Name";
        }

        public void GetSize()
        {
            var frm = definenew;
            frm.cbParentCode.DataSource = null;
            var query = helper.ReadDataRow(hana.Get(SP.ITM_NewSize), 1, "", 0);
            frm.cbParentCode.DataSource = hana.Get(string.Format(query, imd.U_ID006.SelectedValue.ToString()));
            frm.cbParentCode.ValueMember = "Code";
            frm.cbParentCode.DisplayMember = "Name";
        }

    }
}

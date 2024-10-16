using Context;
using PresenterLayer.Helper;
using PresenterLayer.Views;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PresenterLayer.Services
{
    class ITM_QuantityPerCarton
    {
        public frmItemMasterData imd { get; set; }

        public void SetQtyPerCarton(string UomType)
        {
            try
            {
                //if (UomType == "S")
                //{
                //    imd.SalQPC.ReadOnly = false;
                //}
                //else
                //{
                //    imd.BuyQPC.ReadOnly = false;
                //}
                if (UomType != "")
                {
                    imd.BuyQPC.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        public static int GetQuantityPerCarton(string UomType, string ItemCode, string UOM)
        {
            int QPC = 0;

            //QPC = SAPHana.GetQuery(string.Format(SAPHana.GetHanaQuery(SP.ITM_GetQuantityPerCarton, 1), UomType, ItemCode, UOM), 0);

            return QPC;
        } 
    }
}

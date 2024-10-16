using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DirecLayer;

namespace PresenterLayer.Helper
{
    public class SalesAR_generics
    {
        public static int index = 0;
        public static SAPHanaAccess Hana = new SAPHanaAccess();
        public List<string> ModalShow(string parameter, string parameter1, string title)
        {
            List<string> modalValue = new List<string>();

            frmSearch2 fS = new frmSearch2()
            {
                oSearchMode = parameter,
                oFormTitle = title
            };

            if (parameter1 != string.Empty)
            {
                frmSearch2.Param1 = parameter1;
            }

          

            if (fS.oCode != null)
            {
                modalValue.Add(fS.oCode);
                modalValue.Add(fS.oName);
            }

            return modalValue;
            fS.ShowDialog();
        }

        public List<string> ModalShow(string searchKey, List<string> Parameters, string title)
        {
            List<string> modalValue = new List<string>();

            frmSearch2 fS = new frmSearch2()
            {
                oSearchMode = searchKey,
                oFormTitle = title
            };

            if (Parameters.Count > 0)
            {
                for (int i = 0; Parameters.Count > i; i++)
                {
                    switch (i)
                    {
                        case 0:
                            frmSearch2.Param1 = Parameters[i].ToString();
                            break;

                        case 1:
                            frmSearch2.Param2 = Parameters[i].ToString();
                            break;

                        case 2:
                            frmSearch2.Param3 = Parameters[i].ToString();
                            break;

                        case 3:
                            frmSearch2.Param4 = Parameters[i].ToString();
                            break;

                        case 4:
                            frmSearch2.Param5 = Parameters[i].ToString();
                            break;
                    }
                }
            }

            fS.ShowDialog();

            if (fS.oCode != null)
            {
                modalValue.Add(fS.oCode);

                if (fS.oName != null)
                {
                    modalValue.Add(fS.oName);
                }
            }

            return modalValue;
        }

        public DataTable Select(string query)
        {
            DataTable dt = Hana.Get(query);

            return dt;
        }



        public void PreLoading(int NoOfItems, string Action)
        {
            int max = NoOfItems;
            int min = 0;

            for (int i = 0; max > i; i++)
            {
                min++;
                Thread.Sleep(10);
               // PublicStatic.frmMain.Progress2($"Please wait until all data are uploaded. {min} out of {max}", min, max);
            }

           // PublicStatic.frmMain.Progress($"Document is now being {Action}. Please wait.", 100);
        }
    }
}

using Context;
using DirecLayer._03_Repository;
using PresenterLayer;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterLayer.Services
{
    public class CartonListModel : ICartonListModel
    {
        StringQueryRepository _Repo = new StringQueryRepository();

        public bool ActivateService(Func<string, string> returnMessage, string method,
            string docEntry, StringBuilder json)
        {
            //string message = "";
            string sMsg = "";
            var serviceLayer = new ServiceLayerAccess();
            bool isPosted = false;

            if (method == "Add")
            {
                //isPosted = SAPHana.SL_Posting($"CartonList", SAPHana.SL_Mode.POST, json,
                //    "DocNum", out message);
                isPosted = serviceLayer.ServiceLayer_Posting(json, "POST", $"CartonList", "DocEntry", out sMsg, out string val);
            }

            else
            {
                //isPosted = SAPHana.SL_Posting($"CartonList", SAPHana.SL_Mode.PATCH, json,
                //            Convert.ToInt32(docEntry), out message);
                
                isPosted = serviceLayer.ServiceLayer_Posting(json, "PATCH", $"CartonList({docEntry})", "DocEntry", out sMsg, out string val);
            }

            returnMessage(sMsg);
            return isPosted;
        }

        public DataTable DisplayExistingCartonList()
        {
            return DataRepository.GetData(_Repo.ExistingCartonList());
        }

        public DataTable SelectCarton(List<string> documentEntries)
        {
            return DataRepository.GetData(_Repo.CartonMngtHeader(documentEntries));
        }

        public DataTable SelectCartonListHeader(string docEntry)
        {
            return DataRepository.GetData(_Repo.CartonListHeader(docEntry));
        }

        public DataTable SelectCartonListRows(string docEntry)
        {
            return DataRepository.GetData(_Repo.CartonListLines(docEntry));
        }
    }
}

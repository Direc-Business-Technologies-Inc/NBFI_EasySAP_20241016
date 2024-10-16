using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PresenterLayer.Services
{
    public interface ICartonListModel
    {
        DataTable DisplayExistingCartonList();
        DataTable SelectCarton(List<string> documentEntries);
        DataTable SelectCartonListHeader(string docEntry);
        DataTable SelectCartonListRows(string docEntry);
        bool ActivateService(Func<string, string> returnMessage, string method, string docEntry, StringBuilder json);
    }
}

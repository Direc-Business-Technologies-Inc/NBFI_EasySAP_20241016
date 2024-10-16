using DirecLayer;
using System.Data;

namespace DomainLayer.Models
{
    public class EasySAPCredentialsModel
    {
        public static string ESUserId { get; set; }
        public static string ESPassword { get; set; }
        public static string ESDatabase { get; set; }
        public static string EmployeeName { get; set; }
        public static string EmployeeCompleteName { get; set; }

        public static string GetEmployeeCode()
        {
            var hana = new SAPHanaAccess();
            var helper = new DataHelper();
            var result = "";
            var query = $"SELECT empID FROM OHEM Where U_UserID = '{ESUserId}'";
            var dt = hana.Get(query);

            if (helper.DataTableExist(dt))
            {
                if (string.IsNullOrEmpty(helper.ReadDataRow(dt, "empID", "",0)) == false)
                {
                    result = dt.Rows[0][0].ToString();
                }
                else
                { result = ""; }
            }
            else
            { result = ""; }
            return result;
        }
    }
}

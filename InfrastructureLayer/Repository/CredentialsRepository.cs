using DirecLayer;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repository
{
    public class CredentialsRepository
    {
        public bool UpdateDatabase(string sDatabase)
        {
            var output = false;
            try
            {
                var list = new List<ConfigModel>();
                list.Add(new ConfigModel { Code = "Database", Value = sDatabase });
                var repo = new SettingsRepository();
                output = repo.UpdateSettings(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return output;
        }
        public bool GetSAPUser(string sUserId, string sPassword)
        {
            var output = false;
            try
            {
                var sapHana = new SAPHanaAccess();

                var dt = sapHana.Get($"SELECT U_UserID [SapUser],U_SapPassword [SapPassword],(LEFT(firstName,1) + '. ' + lastName) [Name],(firstName + ' ' + lastName) [CompleteName] FROM OHEM Where U_User = '{sUserId}' and U_Password = '{sPassword}'");

                var data = new DataHelper();
                output = data.DataTableExist(dt);

                if (output)
                {
                    var list = new List<ConfigModel>();
                    list.Add(new ConfigModel { Code = "SAPUserId", Value = data.ReadDataRow(dt, "SapUser", "", 0) });
                    list.Add(new ConfigModel { Code = "SAPPassword", Value = data.ReadDataRow(dt, "SapPassword", "", 0) });

                    var repo = new SettingsRepository();
                    repo.UpdateSettings(list);

                    EasySAPCredentialsModel.EmployeeName = data.ReadDataRow(dt, "Name", "", 0);
                    EasySAPCredentialsModel.EmployeeCompleteName = data.ReadDataRow(dt, "CompleteName", "", 0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return output;
        }
    }
}

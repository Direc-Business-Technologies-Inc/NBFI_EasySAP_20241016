using DomainLayer.Models;
using InfrastructureLayer.Repository;
using System;

namespace ServiceLayer.Services
{
    public class CredentialService
    {
        bool IsValid(string sUserId,string sPassword,string sDatabase)
        { return (!string.IsNullOrEmpty(sUserId) && !string.IsNullOrEmpty(sPassword) && !string.IsNullOrEmpty(sDatabase)); }

        public bool Login(out string sMessage)
        {
            var output = false;

            try
            {
                var UserId = EasySAPCredentialsModel.ESUserId;
                var Password = EasySAPCredentialsModel.ESPassword;
                var Database = EasySAPCredentialsModel.ESDatabase;

                bool isValid = IsValid(UserId, Password, Database);

                if (isValid)
                {   
                    CredentialsRepository cred = new CredentialsRepository();
                    if (cred.UpdateDatabase(Database))
                    {
                        if (cred.GetSAPUser(UserId, Password))
                        {
                            ServiceLayerAccess sl = new ServiceLayerAccess();
                            output = sl.Login(out sMessage);
                        }
                        else
                        {
                            sMessage = "Make sure correct user name, password and company database provided";
                        }
                    }
                    else
                    {
                        sMessage = "Cannot update app config";
                    }
                }
                else
                {
                    sMessage = "Incorrect user/password or User has not been registered to the database";
                }
            }
            catch (Exception ex)
            {
                sMessage = $"Error : Credential Service Return Login {ex.Message} in Line {ex.StackTrace} ";
                throw new Exception(sMessage);
            }

            return output;
        }
    }
}

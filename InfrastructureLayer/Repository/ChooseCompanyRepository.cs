using DirecLayer;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace InfrastructureLayer.Repository
{
    public class ChooseCompanyRepository
    {
        public List<ChooseCompanyModel> GetCompanies()
        {
            var output = new List<ChooseCompanyModel>();
            try
            {
                var dt = new DataTable();
                var sapHana = new SAPHanaAccess();

                dt = sapHana.Get(@"SELECT ""cmpName"",""dbName"",""LOC"",""versStr"" FROM SBOCOMMON.SRGC");

                foreach (DataRow dr in dt.Rows)
                {
                    string[] row = new string[] { dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString() };

                    output.Add(new ChooseCompanyModel {
                        CompanyName = dr[0].ToString(),
                        Database = dr[1].ToString(),
                        Localization = dr[2].ToString(),
                        Version = dr[3].ToString()
                    });
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

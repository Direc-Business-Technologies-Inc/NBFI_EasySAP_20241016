using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repository
{
    public class QueryRepository
    {
        public string SeriesCode(string objectCode)
        {
            string query = "SELECT T0.Series [Code], T0.SeriesName [Name]" +

                "FROM NNM1 T0 " +

                $"Where T0.ObjectCode = '{objectCode}'";

            return query;
        }
    }
}

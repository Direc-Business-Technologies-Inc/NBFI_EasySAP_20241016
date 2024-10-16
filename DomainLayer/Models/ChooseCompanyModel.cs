using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class ChooseCompanyModel
    {
        public string CompanyName { get; set; }
        public string Database { get; set; }
        public string Localization { get; set; }
        public string Version { get; set; }
    }
}

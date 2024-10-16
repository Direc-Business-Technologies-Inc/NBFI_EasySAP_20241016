using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class Messages
    {
        public static List<Msg> Msgs = new List<Msg>();

        public class Msg
        {
            public string Id { get; set; }
            public string Message { get; set; }
            public string Date { get; set; }
        }
    }
}

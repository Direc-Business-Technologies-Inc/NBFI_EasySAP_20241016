using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    [Table("INC1")]

    public class InventoryCountingRow
    {
        [Key]
        public int Id { get; set; }

        public int HeaderId { get; set; }

        [StringLength(50)]
        public string  CompName { get; set; }

        public int linenumber { get; set; }

        public string DocDate { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        [StringLength(50)]
        public string WhsCode { get; set; }
        
        public int Quantity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer.Models
{
    [Table("DTINC1")]
    public class DTInventoryCountingRow
    {
        [Key]
        public int Id { get; set; }

        public int HeaderId { get; set; }

        [StringLength(50)]
        public string CompName { get; set; }

        public int linenumber { get; set; }

        public string DocDate { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        [StringLength(50)]
        public string WhsCode { get; set; }

        public int Quantity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    [Table("DTrows")]
    public class DTrow
    {
        [Key]
        public int RowId { get; set; }

        public int HeaderID { get; set; }

        [StringLength(50)]
        public string SapField { get; set; }

        [StringLength(50)]
        public string TemplateField { get; set; }

        [StringLength(20)]
        public string Type { get; set; }

        [StringLength(20)]
        public string Flow { get; set; }

        public int RowStart { get; set; }

        public int ColumnStart { get; set; }

        public int ColumnInterval { get; set; }

        public int RowInterval { get; set; }
    }
}

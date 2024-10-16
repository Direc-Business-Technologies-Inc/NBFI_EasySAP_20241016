using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    [Table("OINC")]
    public class InventoryCounting
    {
        [Key]
        public int id { get; set; }

        public int DocEntry { get; set; }

        public string DocDate { get; set; }

        [StringLength(50)]
        public string DocStatus { get; set; }

        public string CountDate { get; set; }

        [StringLength(50)]
        public string Counter { get; set; }

        public string Time { get; set; }

        [StringLength(50)]
        public string WhsCode { get; set; }

        public string Remarks { get; set; }

        [StringLength(50)]
        public string RefNo { get; set; }

        [StringLength(50)]
        public string PreparedBy { get; set; }

        [StringLength(254)]
        public string NotedBy { get; set; }

        [StringLength(1)]
        public string Canceled { get; set; }

        [StringLength(50)]
        public string Comments { get; set; }

        [StringLength(50)]
        public string SapUsername { get; set; }

        [StringLength(50)] 
        public string SapCode { get; set; }

        [StringLength(50)]
        public string RowIndex { get; set; }

        [StringLength(50)]
        public string CheckedBy { get; set; }

        [ForeignKey("HeaderId")]
        public virtual ICollection<InventoryCountingRow> HeaderRow { get; set; }
    }
}

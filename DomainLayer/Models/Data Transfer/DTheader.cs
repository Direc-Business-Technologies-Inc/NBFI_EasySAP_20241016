using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    [Table("DTheaders")]
    public class DTheader
    {
        [Key]
        public int MapID { get; set; }

        [StringLength(20)] 
        public string MapCode { get; set; }

        [StringLength(100)]
        public string MapDescription { get; set; }

        [StringLength(50)]
        public string UploadType { get; set; }

        [ForeignKey("HeaderID")]
        public virtual ICollection<DTrow> Dtrows { get; set; }
    }
}

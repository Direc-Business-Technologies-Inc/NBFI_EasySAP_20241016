using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirecLayer
{
    [Table("OISM")]
    public class ItemSelectionModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string ItemCode { get; set; }

        [StringLength(100)]
        public string ItemName { get; set; }
        public float Quantity { get; set; }
        public float UnAppOrder { get; set; }
        public string UserCode { get; set; }
    }
}

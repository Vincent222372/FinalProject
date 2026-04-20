using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    [Table("tb_OrderDetails")]
    public class OrderDetails
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? ShopId { get; set; }
       

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Added to match CartController usage
        public string? Size { get; set; } 

        // Navigation
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; } = null!;

        [ForeignKey("ShopId")]
        public virtual Shop? Shop { get; set; } = null!;
    }
}

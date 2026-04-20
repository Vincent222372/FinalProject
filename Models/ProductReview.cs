using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    [Table("tb_ProductReview")]
    public class ProductReview
    {
        [Key]
        public int ReviewId { get; set; }

        // Liên kết với Sản phẩm
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
        public virtual Product Product { get; set; }

        // Người đánh giá
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } // Số sao cho sản phẩm này

        [StringLength(1000)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

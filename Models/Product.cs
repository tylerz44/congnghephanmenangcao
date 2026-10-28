using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }
        [Required]
        public string ProductName { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        public string? Brand { get; set; }
        public string? Size { get; set; }
        public string? Gender { get; set; }
        public string? Color { get; set; }
        public ICollection<ProductVariant> Variants { get; set; }
     = new List<ProductVariant>();
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}
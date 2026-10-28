using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
<<<<<<< HEAD
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }
        [Required]
        public string ProductName { get; set; }
=======

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;
>>>>>>> 3dadca78ada52871d4e9b445449b617792ef3cc4

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

<<<<<<< HEAD
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
=======
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }

        public int Quantity { get; set; }

        // Không bắt buộc có ảnh
        public string? ImageUrl { get; set; }

        public string? Brand { get; set; }

        public string? Size { get; set; }

        public string? Color { get; set; }

        public string? Gender { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        public ICollection<ProductVariant> Variants { get; set; }
            = new List<ProductVariant>();
>>>>>>> 3dadca78ada52871d4e9b445449b617792ef3cc4
    }
}
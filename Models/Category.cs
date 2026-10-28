using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
            = new List<Product>();
    }
}
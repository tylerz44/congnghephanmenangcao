using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
<<<<<<< HEAD

=======
        public string? ImageUrl { get; set; }
>>>>>>> 3dadca78ada52871d4e9b445449b617792ef3cc4
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
            = new List<Product>();
    }
}
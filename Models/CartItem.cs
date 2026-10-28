using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public string Size { get; set; } = "";
        public string Color { get; set; } = "";

        public string? ImageUrl { get; set; }

        public decimal Total
        {
            get { return Price * Quantity; }
        }
    }
}
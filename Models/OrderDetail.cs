using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? Size { get; set; }

        public string? Color { get; set; }

        public Order? Order { get; set; }

        public Product? Product { get; set; }
    }
}
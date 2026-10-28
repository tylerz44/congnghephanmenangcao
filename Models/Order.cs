using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int UserId { get; set; } // Add this property

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string ReceiverName { get; set; }

        public string ReceiverPhone { get; set; }

        public string ShippingAddress { get; set; }

        public string? Note { get; set; }

        public string PaymentMethod { get; set; }

        public string ShippingStatus { get; set; } = "Chờ xác nhận";

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public UserAccount? User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
     = new List<OrderDetail>();
    }
}
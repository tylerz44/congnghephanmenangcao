using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string ReceiverName { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string ReceiverPhone { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string ShippingAddress { get; set; } = "";

        public string? Note { get; set; }

        public string PaymentMethod { get; set; } = "COD";
    }
}
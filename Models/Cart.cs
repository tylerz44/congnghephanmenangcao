namespace FashionStore.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new();

        public decimal TotalPrice => Items.Sum(i => i.Total);
    }
}
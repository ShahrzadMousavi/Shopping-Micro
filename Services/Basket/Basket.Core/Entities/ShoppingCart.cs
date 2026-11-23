namespace Basket.Core.Entities
{
    public class ShoppingCart(string userName, string userId)
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = userName;
        public string UserId { get; set; } = userId;
        public List<ShoppingCartItem> Items { get; set; } = [];

        public decimal CalculateOriginalPrice()
        {
            return Items.Sum(item => item.Quantity * item.Price);
        }
    }
}

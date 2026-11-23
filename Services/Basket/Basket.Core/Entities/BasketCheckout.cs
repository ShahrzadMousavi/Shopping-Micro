namespace Basket.Core.Entities
{
    public class BasketCheckout
    {
        //Identity
        public Guid Guid { get; set; } = Guid.NewGuid();
        //Customer
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? BuyerPhoneNumber { get; set; }
        //Address
        public string? AddressLine { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        //Payment
        public decimal TotalPrice { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Online;
        public string? PaymentTrackingCode { get; set; }//For Bank
        //Shipping
        public string? TrackingCode { get; set; }
        public string? ShippingTrackingCode { get; set; }//For Post Office
    }

    public enum PaymentMethod
    {
        Online=1,
        Wallet=2,
        CashOnDelivery=3
    }
}

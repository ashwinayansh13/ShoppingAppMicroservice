using SharedModels;

namespace CheckoutService.Services
{
    public interface ICheckoutService
    {
        Task<OrderResult> ProcessPaymentAsync(List<GalleryItem> items);
        OrderResult GetOrderStatus(string orderId);
    }
}

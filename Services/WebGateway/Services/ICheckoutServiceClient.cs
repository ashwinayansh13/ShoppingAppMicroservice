using SharedModels;

namespace WebGateway.Services
{
    public interface ICheckoutServiceClient
    {
        Task<OrderResult> ProcessPaymentAsync(List<GalleryItem> items);
        Task<OrderResult> GetOrderStatusAsync(string orderId);
    }
}

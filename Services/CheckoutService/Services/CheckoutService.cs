using SharedModels;

namespace CheckoutService.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly Dictionary<string, OrderResult> _orders = new();

        public async Task<OrderResult> ProcessPaymentAsync(List<GalleryItem> items)
        {
            // Simulate async payment processing
            await Task.Delay(100);

            var orderId = Guid.NewGuid().ToString();
            var totalAmount = items.Sum(i => i.Price);

            var result = new OrderResult
            {
                OrderId = orderId,
                Success = true,
                Message = "Payment processed successfully",
                TotalAmount = totalAmount
            };

            _orders[orderId] = result;
            return result;
        }

        public OrderResult GetOrderStatus(string orderId)
        {
            if (_orders.TryGetValue(orderId, out var order))
            {
                return order;
            }
            return new OrderResult
            {
                Success = false,
                Message = "Order not found"
            };
        }
    }
}

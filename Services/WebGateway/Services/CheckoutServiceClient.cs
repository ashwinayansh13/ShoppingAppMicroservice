using System.Text.Json;
using SharedModels;

namespace WebGateway.Services
{
    public class CheckoutServiceClient : ICheckoutServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CheckoutServiceClient> _logger;

        public CheckoutServiceClient(HttpClient httpClient, ILogger<CheckoutServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<OrderResult> ProcessPaymentAsync(List<GalleryItem> items)
        {
            try
            {
                var json = JsonSerializer.Serialize(items);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/checkout/pay", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderResult>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new OrderResult { Success = false, Message = "Invalid response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling CheckoutService");
                throw;
            }
        }

        public async Task<OrderResult> GetOrderStatusAsync(string orderId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/checkout/order/{orderId}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new OrderResult { Success = false, Message = "Order not found" };
                }
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderResult>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new OrderResult { Success = false, Message = "Invalid response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling CheckoutService for order {OrderId}", orderId);
                throw;
            }
        }
    }
}

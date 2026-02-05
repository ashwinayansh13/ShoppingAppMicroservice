using System.Text.Json;
using SharedModels;

namespace WebGateway.Services
{
    public class GalleryServiceClient : IGalleryServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GalleryServiceClient> _logger;

        public GalleryServiceClient(HttpClient httpClient, ILogger<GalleryServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<GalleryItem>> GetAllItemsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/gallery");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<GalleryItem>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<GalleryItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling GalleryService");
                throw;
            }
        }

        public async Task<GalleryItem?> GetItemByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/gallery/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GalleryItem>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling GalleryService for item {Id}", id);
                throw;
            }
        }

        public async Task<List<GalleryItem>> GetItemsByIdsAsync(int[] ids)
        {
            try
            {
                var json = JsonSerializer.Serialize(ids);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/gallery/items", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<GalleryItem>>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<GalleryItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling GalleryService for items");
                throw;
            }
        }
    }
}

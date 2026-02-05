using SharedModels;

namespace WebGateway.Services
{
    public interface IGalleryServiceClient
    {
        Task<List<GalleryItem>> GetAllItemsAsync();
        Task<GalleryItem?> GetItemByIdAsync(int id);
        Task<List<GalleryItem>> GetItemsByIdsAsync(int[] ids);
    }
}

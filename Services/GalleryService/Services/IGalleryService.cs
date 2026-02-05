using SharedModels;

namespace GalleryService.Services
{
    public interface IGalleryService
    {
        List<GalleryItem> GetAllItems();
        GalleryItem? GetItemById(int id);
    }
}

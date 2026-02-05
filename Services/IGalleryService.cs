using SimpleProject.Models;

namespace SimpleProject.Services
{
    public interface IGalleryService
    {
        List<GalleryItem> GetAllItems();
        GalleryItem? GetItemById(int id);
    }
}


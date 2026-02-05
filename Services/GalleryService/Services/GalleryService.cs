using SharedModels;

namespace GalleryService.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly List<GalleryItem> _items = new()
        {
            new GalleryItem
            {
                Id = 1,
                Title = "Mountain Landscape-Shashidhar Talluri.",
                Description = "Beautiful mountain view at sunset",
                ImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=800",
                Category = "Nature",
                Price = 49.99m
            },
            new GalleryItem
            {
                Id = 2,
                Title = "Ocean Waves-slots",
                Description = "Peaceful ocean waves crashing on the shore",
                ImageUrl = "https://images.unsplash.com/photo-1505142468610-359e7d316be0?w=800",
                Category = "Nature",
                Price = 39.99m
            },
            new GalleryItem
            {
                Id = 3,
                Title = "City Skyline",
                Description = "Modern city skyline at night",
                ImageUrl = "https://images.unsplash.com/photo-1477959858617-67f85cf4f1df?w=800",
                Category = "Urban",
                Price = 59.99m
            },
            new GalleryItem
            {
                Id = 4,
                Title = "Forest Path",
                Description = "Serene forest path through the trees",
                ImageUrl = "https://images.unsplash.com/photo-1448375240586-882707db888b?w=800",
                Category = "Nature",
                Price = 44.99m
            },
            new GalleryItem
            {
                Id = 5,
                Title = "Desert Dunes",
                Description = "Golden sand dunes in the desert",
                ImageUrl = "https://images.unsplash.com/photo-1509316785289-025f5b846b35?w=800",
                Category = "Nature",
                Price = 54.99m
            },
            new GalleryItem
            {
                Id = 6,
                Title = "Abstract Art",
                Description = "Colorful abstract painting",
                ImageUrl = "https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800",
                Category = "Art",
                Price = 69.99m
            }
        };

        public List<GalleryItem> GetAllItems()
        {
            return _items;
        }

        public GalleryItem? GetItemById(int id)
        {
            return _items.FirstOrDefault(item => item.Id == id);
        }
    }
}

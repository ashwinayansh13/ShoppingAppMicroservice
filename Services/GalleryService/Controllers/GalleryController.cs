using Microsoft.AspNetCore.Mvc;
using GalleryService.Services;
using SharedModels;

namespace GalleryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        [HttpGet]
        public ActionResult<List<GalleryItem>> GetAllItems()
        {
            return Ok(_galleryService.GetAllItems());
        }

        [HttpGet("{id}")]
        public ActionResult<GalleryItem> GetItemById(int id)
        {
            var item = _galleryService.GetItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost("items")]
        public ActionResult<List<GalleryItem>> GetItemsByIds([FromBody] int[] ids)
        {
            var allItems = _galleryService.GetAllItems();
            var selectedItems = allItems.Where(i => ids.Contains(i.Id)).ToList();
            return Ok(selectedItems);
        }
    }
}

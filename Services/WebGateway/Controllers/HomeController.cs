using Microsoft.AspNetCore.Mvc;
using WebGateway.Services;

namespace WebGateway.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGalleryServiceClient _galleryServiceClient;

        public HomeController(IGalleryServiceClient galleryServiceClient)
        {
            _galleryServiceClient = galleryServiceClient;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _galleryServiceClient.GetAllItemsAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _galleryServiceClient.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
    }
}

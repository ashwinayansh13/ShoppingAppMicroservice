using Microsoft.AspNetCore.Mvc;
using SimpleProject.Services;

namespace SimpleProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGalleryService _galleryService;

        public HomeController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        public IActionResult Index()
        {
            var items = _galleryService.GetAllItems();
            return View(items);
        }

        public IActionResult Details(int id)
        {
            var item = _galleryService.GetItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
    }
}






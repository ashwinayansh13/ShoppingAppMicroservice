using Microsoft.AspNetCore.Mvc;
using SimpleProject.Models;
using SimpleProject.Services;

namespace SimpleProject.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IGalleryService _galleryService;

        public CheckoutController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        // List all selected items with price tag
        // selectedIds comes from checkboxes on the gallery page
        public IActionResult List(int[]? selectedIds)
        {
            var allItems = _galleryService.GetAllItems();

            List<GalleryItem> selectedItems;
            if (selectedIds != null && selectedIds.Length > 0)
            {
                selectedItems = allItems
                    .Where(i => selectedIds.Contains(i.Id))
                    .ToList();
            }
            else
            {
                // If nothing selected, show all items as a fallback
                selectedItems = allItems;
            }

            return View(selectedItems);
        }

        // Simulate payment and redirect to thank you page
        [HttpPost]
        public IActionResult Pay()
        {
            // In a real app you would process payment here
            return RedirectToAction(nameof(ThankYou));
        }

        // Simple thank you page with a way back to the list
        public IActionResult ThankYou()
        {
            return View();
        }

        // Verify page listing selected items again
        public IActionResult Verify(int[]? selectedIds)
        {
            var allItems = _galleryService.GetAllItems();

            List<GalleryItem> selectedItems;
            if (selectedIds != null && selectedIds.Length > 0)
            {
                selectedItems = allItems
                    .Where(i => selectedIds.Contains(i.Id))
                    .ToList();
            }
            else
            {
                selectedItems = allItems;
            }

            return View(selectedItems);
        }
    }
}


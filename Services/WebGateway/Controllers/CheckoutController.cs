using Microsoft.AspNetCore.Mvc;
using WebGateway.Services;

namespace WebGateway.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IGalleryServiceClient _galleryServiceClient;
        private readonly ICheckoutServiceClient _checkoutServiceClient;

        public CheckoutController(
            IGalleryServiceClient galleryServiceClient,
            ICheckoutServiceClient checkoutServiceClient)
        {
            _galleryServiceClient = galleryServiceClient;
            _checkoutServiceClient = checkoutServiceClient;
        }

        public async Task<IActionResult> List(int[]? selectedIds)
        {
            List<SharedModels.GalleryItem> selectedItems;
            
            if (selectedIds != null && selectedIds.Length > 0)
            {
                selectedItems = await _galleryServiceClient.GetItemsByIdsAsync(selectedIds);
            }
            else
            {
                // If nothing selected, show all items as a fallback
                selectedItems = await _galleryServiceClient.GetAllItemsAsync();
            }

            return View(selectedItems);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(int[]? selectedIds)
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                return RedirectToAction(nameof(List));
            }

            var items = await _galleryServiceClient.GetItemsByIdsAsync(selectedIds);
            var result = await _checkoutServiceClient.ProcessPaymentAsync(items);
            
            if (result.Success)
            {
                return RedirectToAction(nameof(ThankYou), new { orderId = result.OrderId });
            }

            return RedirectToAction(nameof(List), new { selectedIds });
        }

        public IActionResult ThankYou(string? orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        public async Task<IActionResult> Verify(int[]? selectedIds)
        {
            List<SharedModels.GalleryItem> selectedItems;
            
            if (selectedIds != null && selectedIds.Length > 0)
            {
                selectedItems = await _galleryServiceClient.GetItemsByIdsAsync(selectedIds);
            }
            else
            {
                selectedItems = await _galleryServiceClient.GetAllItemsAsync();
            }

            return View(selectedItems);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using CheckoutService.Services;
using SharedModels;

namespace CheckoutService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("pay")]
        public async Task<ActionResult<OrderResult>> ProcessPayment([FromBody] List<GalleryItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return BadRequest("No items provided");
            }

            var result = await _checkoutService.ProcessPaymentAsync(items);
            return Ok(result);
        }

        [HttpGet("order/{orderId}")]
        public ActionResult<OrderResult> GetOrderStatus(string orderId)
        {
            var result = _checkoutService.GetOrderStatus(orderId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}

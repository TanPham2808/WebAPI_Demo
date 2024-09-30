using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Demo.Models;
using WebAPI_Demo.ServicesCondition;
using WebAPI_Demo.ServicesCondition.IServiceCondition;

namespace WebAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;

        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        // Tạo API endpoint để gọi Checkout
        [HttpPost("checkout")]
        public IActionResult Checkout(UserLocations userLocation)
        {
            // Gọi phương thức Checkout của PurchaseService
            var total = _purchaseService.Checkout(userLocation);

            // Trả về kết quả
            return Ok(total);
        }
    }
}

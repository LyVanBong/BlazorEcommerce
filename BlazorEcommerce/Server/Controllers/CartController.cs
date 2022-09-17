using System.Security.Claims;
using BlazorEcommerce.Server.Services.CartService;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("products")]
        public async Task<IActionResult> GetCartProducts(List<CartItem> cartItems)
        {
            var result = await _cartService.GetCartProductsAsync(cartItems);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> StoreCartItem(List<CartItem> cartItems)
        {
            var result = await _cartService.StoreCartItemsAsync(cartItems);
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCartItemCountAsync()
        {
            return Ok(await _cartService.GetCartItemsCountAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetDbCartProductsAsync()
        {
            return Ok(await _cartService.GetDbCartProductsAsync());
        }
    }
}
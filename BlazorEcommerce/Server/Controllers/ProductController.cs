namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductAsync(int productId)
        {
            var data = await _productService.GetProductAsync(productId);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            try
            {
                var data = await _productService.GetProducstAsync();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(new MessageResponse<string>("error", false, "error"));
            }
        }
    }
}

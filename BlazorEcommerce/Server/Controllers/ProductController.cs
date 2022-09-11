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

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProductsAsync()
        {
            var data = await _productService.GetFeaturedProductsAsync();
            return Ok(data);
        }

        [HttpGet("search-suggerstions/{searchText}")]
        public async Task<IActionResult> GetProductSearchSuggestionsAsync(string searchText)
        {
            var data = await _productService.GetProductSearchSuggestionsAsync(searchText);
            return Ok(data);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<IActionResult> SearchProductsAsync(string searchText, int page = 1)
        {
            var data = await _productService.SearchProductsAsync(searchText, page);
            return Ok(data);
        }

        [HttpGet("Category/{categoryUrl}")]
        public async Task<IActionResult> GetProductsByCategory(string categoryUrl)
        {
            var data = await _productService.GetProductsByCategoryAsync(categoryUrl);
            return Ok(data);
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
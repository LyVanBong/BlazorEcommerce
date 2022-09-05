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

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProductAsync()
        {
            try
            {
                var data = await _productService.GetProductAsync();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(new MessageResponse<string>("error", false, "error"));
            }
        }
    }
}

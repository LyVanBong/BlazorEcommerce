namespace BlazorEcommerce.Server.Services.ProductService;

public interface IProductService
{
    Task<MessageResponse<List<Product>>> GetProducstAsync();
    Task<MessageResponse<Product>> GetProductAsync(int productId);
}
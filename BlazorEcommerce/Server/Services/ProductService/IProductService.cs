namespace BlazorEcommerce.Server.Services.ProductService;

public interface IProductService
{
    Task<MessageResponse<List<Shared.Product>>> GetProductAsync();
}
using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.ProductService;

public interface IProductService
{
    List<Product> Products { get; set; }
    Task GetProducts();
    Task<MessageResponse<Product>> GetProduct(int productId);
}
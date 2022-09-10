namespace BlazorEcommerce.Server.Services.ProductService;

public interface IProductService
{
    Task<MessageResponse<List<Product>>> GetProducstAsync();
    Task<MessageResponse<Product>> GetProductAsync(int productId);
    Task<MessageResponse<ProductSearchResult>> SearchProductsAsync(string searchText, int page);
    Task<MessageResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchText);
    Task<MessageResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);
    Task<MessageResponse<List<Product>>> GetFeaturedProductsAsync();
}
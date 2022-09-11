using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.ProductService;

public interface IProductService
{
    event Action ProductsChanged;

    List<Product> Products { get; set; }

    Task GetProducts(string? categoryUrl = null);

    Task<MessageResponse<Product>> GetProduct(int productId);

    string Message { get; set; }

    Task<List<string>> GetProductsSearchSuggestionAsync(string searchText);

    Task SearchProductsAsync(string searchText, int page);

    int CurrentPage { get; set; }
    int PageCount { get; set; }
    string LastSearchText { get; set; }
}
using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.ProductService;

public class ProductService : IProductService
{
    public event Action? ProductsChanged;
    public List<Product> Products { get; set; }
    private HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task GetProducts(string? categoryUrl = null)
    {
        var result = categoryUrl == null ? await _httpClient.GetFromJsonAsync<MessageResponse<List<Product>>>("api/Product") : await _httpClient.GetFromJsonAsync<MessageResponse<List<Product>>>($"api/Product/Category/{categoryUrl}");
        if (result != null && result.Success)
        {
            Products = result.Data;
        }
        ProductsChanged.Invoke();
    }

    public async Task<MessageResponse<Product>> GetProduct(int productId)
    {
        var result = await _httpClient.GetFromJsonAsync<MessageResponse<Product>>($"api/Product/{productId}");
        return result;
    }

    public string Message { get; set; }
    public async Task<List<string>> GetProductsSearchSuggestionAsync(string searchText)
    {
        var result =
            await _httpClient.GetFromJsonAsync<MessageResponse<List<string>>>(
                $"api/Product/search-suggerstions/{searchText}");
        return result?.Data;
    }

    public async Task SearchProductsAsync(string searchText)
    {
        var result =
            await _httpClient.GetFromJsonAsync<MessageResponse<List<Product>>>($"api/Product/search/{searchText}");
        if (result != null && result.Data != null)
        {
            Products = result.Data;
        }

        if (Products.Count == 0)
        {
            Message = "No products found.";
        }
        ProductsChanged.Invoke();
    }
}
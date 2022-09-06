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
}
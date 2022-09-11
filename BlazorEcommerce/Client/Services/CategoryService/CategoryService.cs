using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CategoryService;

public class CategoryService : ICategoryService
{
    public List<Category> Categories { get; set; } = new();
    private HttpClient _httpClient;

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task GetCategoriesAsync()
    {
        var data = await _httpClient.GetFromJsonAsync<MessageResponse<List<Category>>>("api/Category");
        Categories = data?.Data;
    }

    public async Task<Category> GetCategoryAsync(int id)
    {
        var data = await _httpClient.GetFromJsonAsync<MessageResponse<Category>>($"api/Category/{id}");
        return data?.Data;
    }
}
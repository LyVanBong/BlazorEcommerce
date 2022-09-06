using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.CategoryService;

public interface ICategoryService
{
    public List<Category> Categories { get; set; }
    Task GetCategoriesAsync();
    Task<Category> GetCategoryAsync(int id);
}
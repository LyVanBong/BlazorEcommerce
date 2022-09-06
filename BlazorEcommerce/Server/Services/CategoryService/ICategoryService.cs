namespace BlazorEcommerce.Server.Services.CategoryService;

public interface ICategoryService
{
    Task<MessageResponse<List<Category>>> GetCategoriesAsync();
    Task<MessageResponse<Category>> GetCategoryAsync(int id);
}
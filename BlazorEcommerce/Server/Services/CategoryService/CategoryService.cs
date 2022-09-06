namespace BlazorEcommerce.Server.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly DataContext _dataContext;

    public CategoryService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<MessageResponse<List<Category>>> GetCategoriesAsync()
    {
        var data = await _dataContext.Categories.ToListAsync();
        return new MessageResponse<List<Category>>(data, true, "done");
    }

    public async Task<MessageResponse<Category>> GetCategoryAsync(int id)
    {
        var data = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        return new MessageResponse<Category>(data, true, "done");
    }
}
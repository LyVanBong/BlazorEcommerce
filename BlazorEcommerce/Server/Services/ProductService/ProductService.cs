namespace BlazorEcommerce.Server.Services.ProductService;

public class ProductService : IProductService
{
    private readonly DataContext _dataContext;

    public ProductService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<MessageResponse<List<Shared.Product>>> GetProductAsync()
    {
        var data = await _dataContext.Products.ToListAsync();
        return new MessageResponse<List<Shared.Product>>(data, true, "done");
    }
}
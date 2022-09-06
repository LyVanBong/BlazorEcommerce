namespace BlazorEcommerce.Server.Services.ProductService;

public class ProductService : IProductService
{
    private readonly DataContext _dataContext;

    public ProductService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<MessageResponse<List<Shared.Product>>> GetProducstAsync()
    {
        var data = await _dataContext.Products.ToListAsync();
        return new MessageResponse<List<Product>>(data, true, "done");
    }

    public async Task<MessageResponse<Product>> GetProductAsync(int productId)
    {
        var data = await _dataContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
        return new MessageResponse<Product>(data, true, "done");
    }
}
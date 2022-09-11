namespace BlazorEcommerce.Server.Services.ProductService;

public class ProductService : IProductService
{
    private readonly DataContext _dataContext;

    public ProductService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<MessageResponse<List<Product>>> GetProducstAsync()
    {
        var data = await _dataContext.Products
            .Include(p => p.ProductVariants)
            .ToListAsync();
        return new MessageResponse<List<Product>>(data, true, "done");
    }

    public async Task<MessageResponse<Product>> GetProductAsync(int productId)
    {
        var data = await _dataContext.Products
            .Include(p => p.ProductVariants)
            .ThenInclude(v => v.ProductType)
            .FirstOrDefaultAsync(x => x.Id == productId);
        return new MessageResponse<Product>(data, true, "done");
    }

    public async Task<MessageResponse<ProductSearchResult>> SearchProductsAsync(string searchText, int page)
    {
        var pageResults = 2f;
        var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count / pageResults);
        var products = await _dataContext
            .Products
            .Where(p =>
                p.Title.ToUpper().Contains(searchText.ToUpper()) ||
                p.Description.ToUpper().Contains(searchText.ToUpper()))
            .Include(p => p.ProductVariants).Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();


        var data = await FindProductsBySearchText(searchText);
        return new MessageResponse<ProductSearchResult>(new ProductSearchResult()
        {
            Products = products,
            Pages = (int)pageCount,
            CurrentPage = page
        }, true, "done");
    }

    private async Task<List<Product>> FindProductsBySearchText(string searchText)
    {
        var data = await _dataContext
            .Products
            .Where(p =>
                p.Title.ToUpper().Contains(searchText.ToUpper()) ||
                p.Description.ToUpper().Contains(searchText.ToUpper()))
            .Include(p => p.ProductVariants)
            .ToListAsync();
        return data;
    }

    public async Task<MessageResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchText)
    {
        var data = await FindProductsBySearchText(searchText);
        var suggestions = data.Where(p => p.Title.ToUpper().Contains(searchText.ToUpper())).Select(p => p.Title).ToList();
        return new MessageResponse<List<string>>(suggestions, true, "done");
    }

    public async Task<MessageResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
    {
        var data = await _dataContext.Products
            .Where(x => x.Category.Url.ToUpper().Equals(categoryUrl.ToUpper()))
            .Include(p => p.ProductVariants)
            .ToListAsync();
        return new MessageResponse<List<Product>>(data, true, "done");
    }

    public async Task<MessageResponse<List<Product>>> GetFeaturedProductsAsync()
    {
        var data = await _dataContext.Products.Where(p => p.Featured).Include(p => p.ProductVariants).ToListAsync();
        return new MessageResponse<List<Product>>(data, true, "done");
    }
}
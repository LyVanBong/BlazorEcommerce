using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.CartService;

public class CartService : ICartService
{
    private readonly DataContext _dataContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
    {
        _dataContext = dataContext;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() =>
        int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    public async Task<MessageResponse<List<CartProductResponse>>> GetCartProductsAsync(List<CartItem> cartItems)
    {
        var result = new MessageResponse<List<CartProductResponse>>(new List<CartProductResponse>(), true, "Done");
        foreach (var cartItem in cartItems)
        {
            var product = await _dataContext.Products.Where(p => p.Id == cartItem.ProductId).FirstOrDefaultAsync();
            if (product == null)
            {
                continue;
            }

            var productVAriant = await _dataContext.ProductVariants
                .Where(p => p.ProductId == cartItem.ProductId && p.ProductTypeId == cartItem.ProductTypeId)
                .Include(p => p.ProductType).FirstOrDefaultAsync();
            if (productVAriant == null)
            {
                continue;
            }

            var cartProduct = new CartProductResponse()
            {
                ProductId = product.Id,
                Title = product.Title,
                ImageUrl = product.ImageUrl,
                Price = productVAriant.Price,
                ProductType = productVAriant.ProductType.Name,
                ProductTypeId = productVAriant.ProductTypeId,
                Quantity = cartItem.Quantity
            };
            result.Data.Add(cartProduct);
        }

        return result;
    }

    public async Task<MessageResponse<List<CartProductResponse>>> StoreCartItemsAsync(List<CartItem> cartItems)
    {
        cartItems.ForEach(cartItem => cartItem.UserId = GetUserId());
        _dataContext.CartItems.AddRange(cartItems);
        await _dataContext.SaveChangesAsync();
        return await GetDbCartProductsAsync();
    }

    public async Task<MessageResponse<int>> GetCartItemsCountAsync()
    {
        var count = (await _dataContext.CartItems.Where(p => p.UserId == GetUserId()).ToListAsync()).Count;
        return new MessageResponse<int>(count, true, "done");
    }

    public async Task<MessageResponse<List<CartProductResponse>>> GetDbCartProductsAsync()
    {
        return await GetCartProductsAsync(
            await _dataContext.CartItems.Where(p => p.UserId == GetUserId()).ToListAsync());
    }
}
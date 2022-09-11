namespace BlazorEcommerce.Server.Services.CartService;

public class CartService : ICartService
{
    private readonly DataContext _dataContext;

    public CartService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

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
}
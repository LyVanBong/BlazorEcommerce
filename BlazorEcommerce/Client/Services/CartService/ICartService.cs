using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.CartService;

public interface ICartService
{
    event Action OnChange;

    Task AddToCart(CartItem cartItem);

    Task<List<CartProductResponse>> GetCartProductsAsync();

    Task RemoveProductFromCart(int productId, int productTypeId);

    Task UpdateQuantityAsync(CartProductResponse cartProductResponse);
    Task StoreCartItemsAsync(bool emptyLocalCart);
    Task GetCartItemsCountAsync(); 
}
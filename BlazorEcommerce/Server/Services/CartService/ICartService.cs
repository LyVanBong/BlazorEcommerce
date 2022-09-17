namespace BlazorEcommerce.Server.Services.CartService;

public interface ICartService
{
    Task<MessageResponse<List<CartProductResponse>>> GetCartProductsAsync(List<CartItem> cartItems);
    Task<MessageResponse<List<CartProductResponse>>> StoreCartItemsAsync(List<CartItem> cartItems);
    Task<MessageResponse<int>> GetCartItemsCountAsync();
    Task<MessageResponse<List<CartProductResponse>>> GetDbCartProductsAsync();
}
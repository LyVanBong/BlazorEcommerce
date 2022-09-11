using BlazorEcommerce.Shared;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CartService;

public class CartService : ICartService
{
    public event Action? OnChange;

    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _httpClient;

    public CartService(ILocalStorageService localStorageService, HttpClient httpClient)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClient;
    }

    public async Task AddToCart(CartItem cartItem)
    {
        var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
        if (cart == null)
        {
            cart = new List<CartItem>();
        }

        var sameItem = cart.Find(p => p.ProductTypeId == cartItem.ProductTypeId && p.ProductId == cartItem.ProductId);
        if (sameItem == null)
        {
            cart.Add(item: cartItem);
        }
        else
        {
            sameItem.Quantity += cartItem.Quantity;
        }

        await _localStorageService.SetItemAsync("cart", cart);
        OnChange.Invoke();
    }

    public async Task<List<CartItem>> GetCartItems()
    {
        var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
        if (cart == null)
        {
            cart = new List<CartItem>();
        }
        return cart;
    }

    public async Task<List<CartProductResponse>> GetCartProductsAsync()
    {
        var cartItems = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
        var responese =
            await _httpClient.PostAsJsonAsync("api/cart/products", cartItems);
        var cartProducts = await responese.Content.ReadFromJsonAsync<MessageResponse<List<CartProductResponse>>>();
        return cartProducts?.Data;
    }

    public async Task RemoveProductFromCart(int productId, int productTypeId)
    {
        var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
        if (cart == null)
        {
            return;
        }

        var cartItem = cart.Find(p => p.ProductId == productId && p.ProductTypeId == productTypeId);
        if (cartItem != null)
        {
            cart.Remove(cartItem);
            await _localStorageService.SetItemAsync("cart", cart);
            OnChange.Invoke();
        }
    }

    public async Task UpdateQuantityAsync(CartProductResponse cartProductResponse)
    {
        var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
        if (cart == null)
        {
            return;
        }

        var cartItem = cart.Find(p => p.ProductId == cartProductResponse.ProductId && p.ProductTypeId == cartProductResponse.ProductTypeId);
        if (cartItem != null)
        {
            cartItem.Quantity = cartProductResponse.Quantity;
            await _localStorageService.SetItemAsync("cart", cart);
        }
    }
}
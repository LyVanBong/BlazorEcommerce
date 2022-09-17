using System.IO.Compression;
using BlazorEcommerce.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CartService;

public class CartService : ICartService
{
    public event Action? OnChange;

    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public CartService(ILocalStorageService localStorageService, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task AddToCart(CartItem cartItem)
    {
        if (await GetIsAuthenticated())
        {
            Console.WriteLine("da dang nhap");
        }
        else
        {
            Console.WriteLine("chua dang nhap");
        }

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
        await GetCartItemsCountAsync();
    }

    private async Task<bool> GetIsAuthenticated()
    {
        return (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
    }

    public async Task<List<CartProductResponse>> GetCartProductsAsync()
    {
        if (await GetIsAuthenticated())
        {
            var responese =
                await _httpClient.GetFromJsonAsync<MessageResponse<List<CartProductResponse>>>("api/cart");
            return responese?.Data;
        }
        else
        {
            var cartItems = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cartItems == null)
                return new List<CartProductResponse>();
            var responese =
                await _httpClient.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = await responese.Content.ReadFromJsonAsync<MessageResponse<List<CartProductResponse>>>();
            return cartProducts?.Data;
        }
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
            await GetCartItemsCountAsync();
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

    public async Task StoreCartItemsAsync(bool emptyLocalCart = false)
    {
        var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
        if (cart == null)
        {
            return;
        }

        await _httpClient.PostAsJsonAsync("api/cart", cart);
        if (emptyLocalCart)
        {
            await _localStorageService.RemoveItemAsync("cart");
        }
    }

    public async Task GetCartItemsCountAsync()
    {
        if (await GetIsAuthenticated())
        {
            var result = await _httpClient.GetFromJsonAsync<MessageResponse<int>>("api/Cart/count");
            var count = result.Data;
            await _localStorageService.SetItemAsync<int>("cartItemsCount", count);
        }
        else
        {
            var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
            await _localStorageService.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
        }
        OnChange?.Invoke();
    }
}
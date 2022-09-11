﻿namespace BlazorEcommerce.Server.Services.CartService;

public interface ICartService
{
    Task<MessageResponse<List<CartProductResponse>>> GetCartProductsAsync(List<CartItem> cartItems);
}
﻿using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.ProductService;

public class ProductService : IProductService
{
    public List<Product> Products { get; set; }
    private HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task GetProduct()
    {
        var result = await _httpClient.GetFromJsonAsync<MessageResponse<List<Product>>>("api/Product");
        if (result != null && result.Success)
        {
            Products = result.Data;
        }
    }
}
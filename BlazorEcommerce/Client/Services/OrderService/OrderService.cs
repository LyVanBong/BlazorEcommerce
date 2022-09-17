namespace BlazorEcommerce.Client.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> PlaceOrder()
    {
        var result = await _httpClient.PostAsync("api/payment/checkout", null);
        var url = await result.Content.ReadAsStringAsync();
        return url;
    }
}
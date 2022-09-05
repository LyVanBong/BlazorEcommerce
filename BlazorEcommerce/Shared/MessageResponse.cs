namespace BlazorEcommerce.Shared;

public class MessageResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; }

    public MessageResponse(T? data, bool success, string message)
    {
        Data = data;
        Success = success;
        Message = message;
    }
}
namespace Gateway.Api.Services;

public class OrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<OrderDto>> GetOrderAsync()
    {
        var orders = await _httpClient.GetFromJsonAsync<List<OrderDto>>("/orders");
        return orders ?? new List<OrderDto>();
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Realiza una petición GET liviana al endpoint o al health check del servicio
            var response = await _httpClient.GetAsync("/orders", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
public record OrderDto
{
    int Id;
    int ProductId;
    int Quantity;
    decimal Total;
}
namespace Gateway.Api.Services;

public class CatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        var products = await _httpClient.GetFromJsonAsync<List<ProductDto>>("/products");
        return products ?? new List<ProductDto>();
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Realiza una petición GET liviana al endpoint o al health check del servicio
            var response = await _httpClient.GetAsync("/products", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}

public record ProductDto(
    int Id,
    string Name,
    decimal Price
);
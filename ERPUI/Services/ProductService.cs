using System.Net.Http.Json;

namespace ERPUI.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResponse> GetProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedResponse>($"products?pageNumber={pageNumber}&pageSize={pageSize}");
        }
    }
}

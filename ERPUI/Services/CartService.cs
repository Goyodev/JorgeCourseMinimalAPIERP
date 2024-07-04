using Domain.Dtos;
using System.Net.Http.Json;

namespace ERPUI.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IList<CartItemResponse>> GetCartItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IList<CartItemResponse>>("cartitems");
        }

        public async Task<CartItemRequest> AddCartItemAsync(CartItemRequest cartItemDto)
        {
            var response = await _httpClient.PostAsJsonAsync("cartitems", cartItemDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartItemRequest>();
        }

        public async Task DeleteCartItemAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"cartitems/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

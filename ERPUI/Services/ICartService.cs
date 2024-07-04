using Domain.Dtos;

namespace ERPUI.Services
{
    public interface ICartService
    {
        Task<IList<CartItemResponse>> GetCartItemsAsync();
        Task<CartItemRequest> AddCartItemAsync(CartItemRequest cartItemDto);
        Task DeleteCartItemAsync(Guid id);
    }
}

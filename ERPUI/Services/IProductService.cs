using Domain.Dtos;

namespace ERPUI.Services
{
    public interface IProductService
    {
        Task<PaginatedResponse> GetProductsAsync(int pageNumber = 1, int pageSize = 10);
    }

    public record PaginatedResponse(int PageNumber, int PageSize, int TotalProducts, IList<ProductDto> Products);
}

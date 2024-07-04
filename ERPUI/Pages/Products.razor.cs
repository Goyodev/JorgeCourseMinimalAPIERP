
using Domain.Dtos;
using ERPUI.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ERPUI.Pages
{
    public partial class Products
    {
        #region Vars
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected NotificationService NotificationService { get; set; } = default!;
        [Inject] protected IProductService ProductService { get; set; } = default!;
        [Inject] protected ICartService CartService { get; set; } = default!;

        string pagingSummaryFormat = "Mostrando página {0} of {1} (total {2} registros)";
        int pageSize = 6;
        int count;
        IEnumerable<ProductDto> products;
        #endregion

        #region Methods
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            try
            {
                var response = await GetProducts(1, 6);
                count = response.TotalProducts;
                products = response.Products;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "No se pudieron cargar los productos.",
                    Duration = 4000
                });
            }
        }

        void AddToCart(ProductDto productDto)
        {
            try
            {
                CartItemRequest cartItemDto = new CartItemRequest();
                cartItemDto.ProductId = productDto.ProductId;
                cartItemDto.CartItemGuid = productDto.ProductGuid;
                cartItemDto.Count = 1;
                CartService.AddCartItemAsync(cartItemDto);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "No se pudo añadir el producto al carrito.",
                    Duration = 4000
                });
            }
        }

        async Task PageChanged(PagerEventArgs args)
        {
            var response = await GetProducts(args.PageIndex, args.Top);
            products = response.Products;
        }

        private async Task<PaginatedResponse> GetProducts(int pageNum, int take)
        {
            try
            {
                PaginatedResponse products = await ProductService.GetProductsAsync(pageNum, take);
                return products;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "No se pudieron cargar los productos.",
                    Duration = 4000
                });
                return null;
            }
        }
        #endregion


    }
}

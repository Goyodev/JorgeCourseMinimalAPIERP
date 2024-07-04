using Domain.Dtos;
using ERPUI.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ERPUI.Pages
{
    public partial class Basket
    {
        #region Vars
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected ICartService CartService { get; set; } = default!;
        [Inject] protected NotificationService NotificationService { get; set; } = default!;
        #endregion

        #region Methods
        private IList<CartItemResponse> cartItems = new List<CartItemResponse>();
        bool DeleteIsBusy = false;


        protected override async Task OnInitializedAsync()
        {
            await LoadCartItems();
        }

        private async Task LoadCartItems()
        {
            try
            {
                cartItems = await CartService.GetCartItemsAsync();

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

        private async Task RemoveFromCart(CartItemResponse item)
        {
            try
            {
                DeleteIsBusy = true;
                await CartService.DeleteCartItemAsync(item.CartItemGuid);
                await LoadCartItems();
                DeleteIsBusy = false;

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Correcto",
                    Detail = "Producto eliminado correctamente.",
                    Duration = 4000
                });
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "No se pudo eliminar el producto del carrito.",
                    Duration = 4000
                });
            }
        }

        private void ProceedToCheckout()
        {
            NavigationManager.NavigateTo("/checkout");
        }
        #endregion


    }
}

using Domain.Dtos;
using ERP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPIERP.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Api;

internal static class CartItemApi
{
    public static RouteGroupBuilder MapCartItemApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/erp")
            .WithTags("Cart Item Api");

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        group.MapGet("/cartitems", async ([AsParameters] ApiDependencies dep) =>
        await dep.Context.CartItems.Include(ci => ci.Product).ToListAsync() is IList<CartItem> cartItems
        ? Results.Json(dep.Mapper.Map<IList<CartItemDto>>(cartItems), options)
        : Results.NotFound())
    .WithOpenApi();

        group.MapGet("/cartitems/{id}", async ([AsParameters] ApiDependencies dep, Guid id) =>
            await dep.Context.CartItems.Include(ci => ci.Product).SingleOrDefaultAsync(ci => ci.CartItemGuid == id) is CartItem cartItem
                ? Results.Json(dep.Mapper.Map<CartItemDto>(cartItem), options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapPost("/cartitems", async ([AsParameters] ApiDependencies dep, CartItemDto cartItemDto) =>
        {
            var cartItem = dep.Mapper.Map<CartItem>(cartItemDto);
            cartItem.CartItemGuid = Guid.NewGuid();
            dep.Context.CartItems.Add(cartItem);
            await dep.Context.SaveChangesAsync();

            var resultDto = dep.Mapper.Map<CartItemDto>(cartItem);
            return Results.Created($"/cartitems/{resultDto.CartItemId}", resultDto);
        }).WithOpenApi();

        group.MapPut("/cartitems/{id}", async ([AsParameters] ApiDependencies dep, Guid id, CartItemDto updatedCartItemDto) =>
        {
            var cartItem = await dep.Context.CartItems.Include(ci => ci.Product).SingleOrDefaultAsync(ci => ci.CartItemGuid == id);
            if (cartItem is null) return Results.NotFound();

            dep.Mapper.Map(updatedCartItemDto, cartItem);
            await dep.Context.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        group.MapDelete("/cartitems/{id}", async ([AsParameters] ApiDependencies dep, Guid id) =>
        {
            var cartItem = await dep.Context.CartItems.SingleOrDefaultAsync(ci => ci.CartItemGuid == id);
            if (cartItem is null) return Results.NotFound();

            dep.Context.CartItems.Remove(cartItem);
            await dep.Context.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        return group;
    }
}

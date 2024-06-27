using ERP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MinimalAPIERP.Dtos;
using Newtonsoft.Json;
using System.Security.Claims;
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

        group.MapGet("/cartitems", async (AppDbContext db) =>
            await db.CartItems.ToListAsync() is IList<CartItem> cartItems
                ? Results.Json(cartItems, options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapGet("/cartitems/{id}", async (AppDbContext db, Guid id) =>
            await db.CartItems.FindAsync(id) is CartItem cartItem
                ? Results.Json(cartItem, options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapPost("/cartitems", async (AppDbContext db, CartItem cartItem) =>
        {
            cartItem.Guid = Guid.NewGuid();
            db.CartItems.Add(cartItem);
            await db.SaveChangesAsync();
            return Results.Created($"/cartitems/{cartItem.CartItemId}", cartItem);
        }).WithOpenApi();

        group.MapPut("/cartitems/{id}", async (AppDbContext db, Guid id, CartItem updatedCartItem) =>
        {
            var cartItem = await db.CartItems.FindAsync(id);
            if (cartItem is null) return Results.NotFound();

            cartItem.CartId = updatedCartItem.CartId;
            cartItem.ProductId = updatedCartItem.ProductId;
            cartItem.Count = updatedCartItem.Count;
            cartItem.DateCreated = updatedCartItem.DateCreated;
            

            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        group.MapDelete("/cartitems/{id}", async (AppDbContext db, Guid id) =>
        {
            var cartItem = await db.CartItems.FindAsync(id);
            if (cartItem is null) return Results.NotFound();

            db.CartItems.Remove(cartItem);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        return group;
    }
}

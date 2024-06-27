using ERP.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Api;

internal static class OrderApi
{
    public static RouteGroupBuilder MapOrderApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/erp")
            .WithTags("Order Api");

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        group.MapGet("/orders", async (AppDbContext db) =>
     await db.Orders.ToListAsync() is IList<Order> orders
         ? Results.Json(orders, options)
         : Results.NotFound())
     .WithOpenApi();

        group.MapGet("/orders/{id}", async (AppDbContext db, Guid id) =>
            await db.Orders.FindAsync(id) is Order order
                ? Results.Json(order, options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapPost("/orders", async (AppDbContext db, Order order) =>
        {
            order.OrderGuid = Guid.NewGuid();
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            return Results.Created($"/orders/{order.OrderId}", order);
        }).WithOpenApi();

        group.MapPut("/orders/{id}", async (AppDbContext db, Guid id, Order updatedOrder) =>
        {
            var order = await db.Orders.FindAsync(id);
            if (order is null) return Results.NotFound();

            order.OrderDate = updatedOrder.OrderDate;
            order.Username = updatedOrder.Username;
            order.Name = updatedOrder.Name;
            order.Address = updatedOrder.Address;
            order.City = updatedOrder.City;
            order.State = updatedOrder.State;
            order.PostalCode = updatedOrder.PostalCode;
            order.Country = updatedOrder.Country;
            order.Phone = updatedOrder.Phone;
            order.Email = updatedOrder.Email;
            order.Total = updatedOrder.Total;
            order.OrderDetails = updatedOrder.OrderDetails;

            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        group.MapDelete("/orders/{id}", async (AppDbContext db, Guid id) =>
        {
            var order = await db.Orders.FindAsync(id);
            if (order is null) return Results.NotFound();

            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        return group;
    }
}

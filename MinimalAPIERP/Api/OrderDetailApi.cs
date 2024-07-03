using ERP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Domain.Dtos;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Api;

internal static class OrderDetailApi
{
    public static RouteGroupBuilder MapOrderDetailApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/erp")
            .WithTags("Order Detail Api");

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };


        group.MapGet("/orderdetails", async (AppDbContext db) =>
            await db.OrderDetails.ToListAsync() is IList<OrderDetail> orderDetails
                ? Results.Json(orderDetails, options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapGet("/orderdetails/{id}", async (AppDbContext db, Guid id) =>
            await db.OrderDetails.FindAsync(id) is OrderDetail orderDetail
                ? Results.Json(orderDetail, options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapPost("/orderdetails", async (AppDbContext db, OrderDetail orderDetail) =>
        {
            orderDetail.OrderDetailGuid = Guid.NewGuid();
            db.OrderDetails.Add(orderDetail);
            await db.SaveChangesAsync();
            return Results.Created($"/orderdetails/{orderDetail.OrderDetailId}", orderDetail);
        }).WithOpenApi();

        group.MapPut("/orderdetails/{id}", async (AppDbContext db, Guid id, OrderDetail updatedOrderDetail) =>
        {
            var orderDetail = await db.OrderDetails.FindAsync(id);
            if (orderDetail is null) return Results.NotFound();

            orderDetail.OrderId = updatedOrderDetail.OrderId;
            orderDetail.ProductId = updatedOrderDetail.ProductId;
            orderDetail.Count = updatedOrderDetail.Count;
            orderDetail.UnitPrice = updatedOrderDetail.UnitPrice;
            orderDetail.Order = updatedOrderDetail.Order;
            orderDetail.Product = updatedOrderDetail.Product;

            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        group.MapDelete("/orderdetails/{id}", async (AppDbContext db, Guid id) =>
        {
            var orderDetail = await db.OrderDetails.FindAsync(id);
            if (orderDetail is null) return Results.NotFound();

            db.OrderDetails.Remove(orderDetail);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();


        return group;
    }
}

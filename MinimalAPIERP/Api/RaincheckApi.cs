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

internal static class RaincheckApi
{
    public static RouteGroupBuilder MapRaincheckApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/erp")
            .WithTags("Raincheck Api");

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        group.MapGet("/rainchecks", async (AppDbContext db) =>
            await db.Rainchecks.ToListAsync() is IList<Raincheck> rainchecks
                ? Results.Json(rainchecks, options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapGet("/rainchecks/{id}", async (AppDbContext db, Guid id) =>
            await db.Rainchecks.FindAsync(id) is Raincheck raincheck
                ? Results.Json(raincheck, options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapPost("/rainchecks", async (AppDbContext db, Raincheck raincheck) =>
        {
            raincheck.RaincheckGuid = Guid.NewGuid();
            db.Rainchecks.Add(raincheck);
            await db.SaveChangesAsync();
            return Results.Created($"/rainchecks/{raincheck.RaincheckId}", raincheck);
        }).WithOpenApi();

        group.MapPut("/rainchecks/{id}", async (AppDbContext db, Guid id, Raincheck updatedRaincheck) =>
        {
            var raincheck = await db.Rainchecks.FindAsync(id);
            if (raincheck is null) return Results.NotFound();

            raincheck.Name = updatedRaincheck.Name;
            raincheck.ProductId = updatedRaincheck.ProductId;
            raincheck.Count = updatedRaincheck.Count;
            raincheck.SalePrice = updatedRaincheck.SalePrice;
            raincheck.StoreId = updatedRaincheck.StoreId;
            raincheck.Product = updatedRaincheck.Product;
            raincheck.Store = updatedRaincheck.Store;

            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        group.MapDelete("/rainchecks/{id}", async (AppDbContext db, Guid id) =>
        {
            var raincheck = await db.Rainchecks.FindAsync(id);
            if (raincheck is null) return Results.NotFound();

            db.Rainchecks.Remove(raincheck);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();


        return group;
    }
}

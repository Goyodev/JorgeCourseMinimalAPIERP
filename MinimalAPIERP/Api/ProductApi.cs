using Domain.Dtos;
using ERP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MinimalAPIERP.Extensions;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Api;

internal static class ProductApi
{
    public static RouteGroupBuilder MapProductApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/erp")
            .WithTags("Product Api");


        // TODO: Mover a config
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            //PropertyNameCaseInsensitive = false,
            //PropertyNamingPolicy = null,
            WriteIndented = true,
            //IncludeFields = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            //ReferenceHandler = ReferenceHandler.Preserve
        };

        group.MapGet("/products", async ([AsParameters] ApiDependencies dep) =>
    await dep.Context.Products.ToListAsync() is IList<Product> products
        ? Results.Json(dep.Mapper.Map<IList<ProductDto>>(products), options)
        : Results.NotFound())
    .WithOpenApi();

        group.MapGet("/products/{id}", async ([AsParameters] ApiDependencies dep, Guid id) =>
     await dep.Context.Products.SingleOrDefaultAsync(p => p.ProductGuid == id) is Product product
         ? Results.Json(dep.Mapper.Map<ProductDto>(product), options)
         : Results.NotFound())
     .WithOpenApi();

        // Endpoint para crear un producto
        group.MapPost("/products", async ([AsParameters] ApiDependencies dep, ProductDto productDto) =>
        {
            var product = dep.Mapper.Map<Product>(productDto);
            dep.Context.Products.Add(product);
            await dep.Context.SaveChangesAsync();

            var resultDto = dep.Mapper.Map<ProductDto>(product);
            return Results.Created($"/products/{resultDto.ProductId}", resultDto);
        }).WithOpenApi();

        // Endpoint para actualizar un producto
        group.MapPut("/products/{id}", async ([AsParameters] ApiDependencies dep, Guid id, ProductDto updatedProductDto) =>
        {
            var product = await dep.Context.Products.SingleOrDefaultAsync(p => p.ProductGuid == id);
            if (product is null) return Results.NotFound();

            dep.Mapper.Map(updatedProductDto, product);
            await dep.Context.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        // Endpoint para eliminar un producto
        group.MapDelete("/products/{id}", async ([AsParameters] ApiDependencies dep, Guid id) =>
        {
            var product = await dep.Context.Products.SingleOrDefaultAsync(p => p.ProductGuid == id);
            if (product is null) return Results.NotFound();

            dep.Context.Products.Remove(product);
            await dep.Context.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        return group;
    }
}

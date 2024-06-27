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

internal static class CategoryApi
{
    public static RouteGroupBuilder MapCategoryApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/erp")
            .WithTags("Category Api");

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        group.MapGet("/categories", async (AppDbContext db) =>
            await db.Categories.ToListAsync() is IList<Category> categories
                ? Results.Json(categories, options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapGet("/categories/{id}", async (AppDbContext db, Guid id) =>
            await db.Categories.FindAsync(id) is Category category
                ? Results.Json(category, options)
                : Results.NotFound())
            .WithOpenApi();

        group.MapPost("/categories", async (AppDbContext db, Category category) =>
        {
            category.CategoryGuid = Guid.NewGuid();
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return Results.Created($"/categories/{category.CategoryId}", category);
        }).WithOpenApi();

        group.MapPut("/categories/{id}", async (AppDbContext db, Guid id, Category updatedCategory) =>
        {
            var category = await db.Categories.FindAsync(id);
            if (category is null) return Results.NotFound();

            category.Name = updatedCategory.Name;
            category.Description = updatedCategory.Description;
            category.ImageUrl = updatedCategory.ImageUrl;
            category.Products = updatedCategory.Products;

            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        group.MapDelete("/categories/{id}", async (AppDbContext db, Guid id) =>
        {
            var category = await db.Categories.FindAsync(id);
            if (category is null) return Results.NotFound();

            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        return group;
    }
}

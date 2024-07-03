using ERP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Domain.Dtos;
using MinimalAPIERP.Extensions;
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

        group.MapGet("/categories", async ([AsParameters] ApiDependencies dep) =>
     await dep.Context.Categories.ToListAsync() is IList<Category> categories
         ? Results.Json(categories, options)
         : Results.NotFound())
     .WithOpenApi();

        // Endpoint para obtener una categoría por ID
        group.MapGet("/categories/{id}", async ([AsParameters] ApiDependencies dep, Guid id) =>
            await dep.Context.Categories.SingleOrDefaultAsync(c => c.CategoryGuid == id) is Category category
                ? Results.Json(category, options)
                : Results.NotFound())
            .WithOpenApi();

        // Endpoint para crear una categoría
        group.MapPost("/categories", async ([AsParameters] ApiDependencies dep, CategoryDto categoryDto) =>
        {
            var category = dep.Mapper.Map<Category>(categoryDto);
            category.CategoryGuid = Guid.NewGuid();
            dep.Context.Categories.Add(category);
            await dep.Context.SaveChangesAsync();

            var resultDto = dep.Mapper.Map<CategoryDto>(category);
            return Results.Created($"/categories/{resultDto.CategoryGuid}", resultDto);
        }).WithOpenApi();

        // Endpoint para actualizar una categoría
        group.MapPut("/categories/{id}", async ([AsParameters] ApiDependencies dep, Guid id, CategoryDto updatedCategoryDto) =>
        {
            var category = await dep.Context.Categories.SingleOrDefaultAsync(c => c.CategoryGuid == id);
            if (category is null) return Results.NotFound();

            dep.Mapper.Map(updatedCategoryDto, category);

            await dep.Context.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        // Endpoint para eliminar una categoría
        group.MapDelete("/categories/{id}", async ([AsParameters] ApiDependencies dep, Guid id) =>
        {
            var category = await dep.Context.Categories.SingleOrDefaultAsync(c => c.CategoryGuid == id);
            if (category is null) return Results.NotFound();

            dep.Context.Categories.Remove(category);
            await dep.Context.SaveChangesAsync();
            return Results.NoContent();
        }).WithOpenApi();

        return group;
    }
}

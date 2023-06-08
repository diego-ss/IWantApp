using IWantApp.Domain.Products;
using IWantApp.Endpoints.Extensions;
using IWantApp.Infra.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IWantApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action([FromRoute] Guid id, HttpContext httpContext, CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        if (category == null)
            return Results.NotFound();

        category.EditInfo(categoryRequest.Name, categoryRequest.Active, userId);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

        context.SaveChanges();

        return Results.Ok();
    }
}

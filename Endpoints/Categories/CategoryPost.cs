using IWantApp.Domain.Products;
using IWantApp.Endpoints.Extensions;
using IWantApp.Infra.Database;
using System.Security.Claims;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(CategoryRequest categoryRequest, HttpContext httpContext, ApplicationDbContext context)
    {
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var category = new Category(categoryRequest.Name, userId, userId);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}

using IWantApp.Domain.Products;
using IWantApp.Infra.Database;
using System.Xml.Linq;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = new Category(categoryRequest.Name)
        {
            CreatedBy = "Test",
            CreatedDate = DateTime.Now,
            EditedBy = "Test",
            EditedDate = DateTime.Now
        };

        if(!category.IsValid)
            return Results.BadRequest(category.Notifications);

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}

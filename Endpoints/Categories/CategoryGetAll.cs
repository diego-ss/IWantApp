using IWantApp.Infra.Database;

namespace IWantApp.Endpoints.Categories;

public class CategoryGetAll
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(ApplicationDbContext context)
    {
        var categories = context.Categories.Where(x=>x.Active).ToList();
        var response = categories.Select(c => new CategoryResponse 
        { 
            Id = c.Id,
            Name = c.Name, 
            Active = c.Active 
        });
        return Results.Ok(response);
    }
}

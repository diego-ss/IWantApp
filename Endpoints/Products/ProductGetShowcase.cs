using IWantApp.Infra.Database;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Products;

public class ProductGetShowcase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context)
    {
        var products = context.Products.Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active)
            .OrderBy(p => p.Name).ToList();

        var results = products.Select(p => new ProductsResponse(p.Name, p.Category.Name, p.Description, p.Price, p.HasStock, p.Active));
        return Results.Ok(results);
    }
}

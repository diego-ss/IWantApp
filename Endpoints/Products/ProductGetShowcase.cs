using IWantApp.Infra.Database;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Products;

public class ProductGetShowcase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, int page = 1, int rows = 10, string orderBy = "name")
    {
        if (rows > 10)
            return Results.Problem(title: "Invalid number of rows", detail: "The max rows number is 10", statusCode: 400);

        var queryBase = context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active);

        if (orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Name);
        else if (orderBy == "price")
            queryBase = queryBase.OrderBy(p => p.Price);
        else
            return Results.Problem(title: "Invalid order criteria", detail: "Only 'name' and 'price' are accepted like order criteria", statusCode: 400);

        var queryFilter = queryBase.Skip((page - 1) * rows).Take(rows);
  
        var products = queryFilter.OrderBy(p => p.Name).ToList();

        var results = products.Select(p => new ProductsResponse(p.Id, p.Name, p.Category.Name, p.Description, p.Price, p.HasStock, p.Active));
        return Results.Ok(results);
    }
}

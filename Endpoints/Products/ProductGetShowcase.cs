using IWantApp.Infra.Database;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Products;

public class ProductGetShowcase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(int? page, int? rows, string? orderBy, ApplicationDbContext context)
    {
        if (page == null)
            page = 1;

        if (rows == null)
            rows = 10;

        if (string.IsNullOrEmpty(orderBy))
            orderBy = "name";

        var queryBase = context.Products.Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active);

        if (orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Name);
        else
            queryBase = queryBase.OrderBy(p => p.Price);

        var queryFilter = queryBase.Skip((page.Value - 1) * rows.Value).Take(rows.Value);
  
        var products = queryFilter.OrderBy(p => p.Name).ToList();

        var results = products.Select(p => new ProductsResponse(p.Name, p.Category.Name, p.Description, p.Price, p.HasStock, p.Active));
        return Results.Ok(results);
    }
}

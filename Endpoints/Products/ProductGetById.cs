using IWantApp.Infra.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Products;

public class ProductGetById
{
    public static string Template => "/products/{productId:guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid productId, ApplicationDbContext context)
    {
        var product = await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
            return Results.NotFound("Failed to find product with given id");

        var result = new ProductsResponse(product.Name, product.Category.Name, product.Description, product.Price, product.HasStock, product.Active);
        return Results.Ok(result);
    }
}

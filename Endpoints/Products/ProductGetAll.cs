﻿using IWantApp.Infra.Database;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Products;

public class ProductGetAll
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ApplicationDbContext context)
    {
        var products = context.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
        var results = products.Select(p => new ProductsResponse(p.Name, p.Category.Name, p.Description, p.HasStock, p.Active));
        return Results.Ok(results);
    }
}

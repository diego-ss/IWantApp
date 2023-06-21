using IWantApp.Domain.Orders;
using IWantApp.Infra.Database;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IWantApp.Endpoints.Orders;

public class OrderPost
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [Authorize]
    public static async Task<IResult> Action(OrderRequest orderRequest, HttpContext httpContext, ApplicationDbContext applicationDbContext)
    {
        var clientId = httpContext.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var clientName = httpContext.User.Claims
            .First(c => c.Type == "Name").Value;

        // apenas uma consulta no banco de dados
        var productsFound = applicationDbContext.Products.Where(p => orderRequest.ProductIds.Contains(p.Id)).ToList();

        var order = new Order(clientId, clientName, productsFound, orderRequest.DeliveryAddress);
        await applicationDbContext.Orders.AddAsync(order);
        await applicationDbContext.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order.Id);
    }
}

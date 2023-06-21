using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IWantApp.Endpoints.Clients;

public class ClientGet
{
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(HttpContext httpContext)
    {
        var user = httpContext.User;
        var result = new
        {
            Id = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            Email = user.Claims.First(c => c.Type == ClaimTypes.Email).Value,
            Name = user.Claims.First(c => c.Type == "Name").Value,
            Cpf = user.Claims.First(c => c.Type == "Cpf").Value,
        };

        return Results.Ok(result);
    }
}

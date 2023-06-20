using IWantApp.Endpoints.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IWantApp.Endpoints.Clients;

public class ClientPost
{
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ClientRequest clientRequest, HttpContext httpContext, UserManager<IdentityUser> userManager)
    {
        // criando novo usuário
        var client = new IdentityUser { UserName = clientRequest.Name, Email = clientRequest.Email };
        var result = await userManager.CreateAsync(client, clientRequest.Password);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var claimResult = await userManager.AddClaimsAsync(client,
            new Claim[] {
                new Claim("Cpf", clientRequest.Cpf),
                new Claim("Name", clientRequest.Name),
            });

        if (!claimResult.Succeeded)
            return Results.BadRequest(claimResult.Errors);

        return Results.Created($"/clients/{client.Id}", client.Id);
    }
}

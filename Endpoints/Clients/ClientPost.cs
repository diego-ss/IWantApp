using IWantApp.Domain.Users;
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
    public static async Task<IResult> Action(ClientRequest clientRequest, UserService userService)
    {
        var claims = new List<Claim> {
                new Claim("Cpf", clientRequest.Cpf),
                new Claim("Name", clientRequest.Name) };

        (IdentityResult identity, string userId) result = await userService.Create(clientRequest.Name, clientRequest.Email, clientRequest.Password, claims);

        if (!result.identity.Succeeded)
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());

        return Results.Created($"/clients/{result.userId}", result.userId);
    }
}

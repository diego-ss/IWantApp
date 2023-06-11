using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IWantApp.Endpoints.Security;

public class TokenPost
{
    public static string Template => "/token";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static IResult Action(
        LoginRequest loginRequest, 
        IConfiguration configuration, 
        UserManager<IdentityUser> userManager, 
        ILogger<TokenPost> logger)
    {
        logger.LogInformation("Finding user...");
        var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
        if (user == null || !userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
        {
            logger.LogError($"User with email {loginRequest.Email} not registered");
            return Results.BadRequest();
        }

        var claims = userManager.GetClaimsAsync(user).Result;
        var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, loginRequest.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            });
        subject.AddClaims(claims);

        logger.LogInformation("Getting token...");
        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
            Expires = DateTime.UtcNow.AddSeconds(int.Parse(configuration["JwtBearerTokenSettings:ExpiryTimeInSeconds"]))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        logger.LogInformation("Token generated successfully...");

        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token)
        });
    }
}

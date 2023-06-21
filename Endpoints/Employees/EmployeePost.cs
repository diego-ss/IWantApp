using IWantApp.Domain.Users;
using IWantApp.Endpoints.Extensions;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    public static async Task<IResult> Action(EmployeeRequest employeeRequest, HttpContext httpContext, UserService userService)
    {
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var claims = new List <Claim> {
                new Claim("EmployeeCode", employeeRequest.EmployeeCode),
                new Claim("Name", employeeRequest.Name),
                new Claim("CreateBy", userId)
            };

        (IdentityResult identity, string userId) result = await userService.Create(employeeRequest.Name, employeeRequest.Email, employeeRequest.Password, claims);

        if (!result.identity.Succeeded)
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());

        return Results.Created($"/employees/{result.userId}", result.userId);
    }
}

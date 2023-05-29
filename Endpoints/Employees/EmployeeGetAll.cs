using Microsoft.AspNetCore.Identity;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(int page, int rows, UserManager<IdentityUser> userManager)
    {
        var users = userManager.Users
            .Skip((page - 1) * rows)
            .Take(rows).ToList();
        var employees = new List<EmployeeResponse>();

        users.ForEach(u =>
        {
            var claims = userManager.GetClaimsAsync(u).Result;
            var username = claims.FirstOrDefault(c => c.Type == "Name");
            employees.Add(new EmployeeResponse(u.Email, username != null ? username.Value : ""));
        });

        return Results.Ok(employees);
    }
}

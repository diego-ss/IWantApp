using Dapper;
using IWantApp.Infra.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(int? page, int? rows, QueryAllUsersWithClaimName queryAllUsersWithClaimName)
    {
        if (page == null)
            return Results.BadRequest("Page is required");

        if (rows == null)
            return Results.BadRequest("Rows is required");


        return Results.Ok(queryAllUsersWithClaimName.Execute(page.Value, rows.Value));
    }
}

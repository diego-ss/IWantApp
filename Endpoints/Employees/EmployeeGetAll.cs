using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(int? page, int? rows, IConfiguration configuration)
    {
        if (page == null)
            return Results.BadRequest("Page is required");

        if (rows == null)
            return Results.BadRequest("Rows is required");

        var db = new SqlConnection(configuration.GetConnectionString("SqlServer"));
        var employees = db.Query<EmployeeResponse>(
            @"SELECT EMAIL, CLAIMVALUE AS NAME 
            FROM AspNetUsers A 
            INNER JOIN AspNetUserClaims B
            ON A.ID = B.USERID AND B.CLAIMTYPE = 'NAME' 
            ORDER BY NAME
            OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY", 
            new { page, rows } );

        return Results.Ok(employees);
    }
}

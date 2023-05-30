using Dapper;
using IWantApp.Endpoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Database;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IEnumerable<EmployeeResponse> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration.GetConnectionString("SqlServer"));
        var employees = db.Query<EmployeeResponse>(
            @"SELECT EMAIL, CLAIMVALUE AS NAME 
            FROM AspNetUsers A 
            INNER JOIN AspNetUserClaims B
            ON A.ID = B.USERID AND B.CLAIMTYPE = 'NAME' 
            ORDER BY NAME
            OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY",
            new { page, rows });

        return employees;
    }
}

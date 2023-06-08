﻿using IWantApp.Endpoints.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(EmployeeRequest employeeRequest, HttpContext httpContext, UserManager<IdentityUser> userManager)
    {
        // criando novo usuário
        var employee = new IdentityUser();
        employee.Email = employeeRequest.Email;
        employee.UserName = employeeRequest.Name;
        var result = userManager.CreateAsync(employee, employeeRequest.Password).Result;
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;


        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var claimResult = userManager.AddClaimsAsync(employee,
            new Claim[] {
                new Claim("EmployeeCode", employeeRequest.EmployeeCode),
                new Claim("Name", employeeRequest.Name),
                new Claim("CreateBy", userId)
            }).Result;

        if (!claimResult.Succeeded)
            return Results.BadRequest(claimResult.Errors);

        return Results.Created($"/employees/{employee.Id}", employee.Id);
    }
}

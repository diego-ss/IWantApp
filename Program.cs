using IWantApp.Endpoints.Categories;
using IWantApp.Endpoints.Employees;
using IWantApp.Endpoints.Security;
using IWantApp.Infra.Database;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
// servi�o de banco de dados
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration.GetConnectionString("SqlServer"));
// servi�o do identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
// servi�o da query do dapper
builder.Services.AddScoped<QueryAllUsersWithClaimName>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handler);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handler);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handler);
app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handler);
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handler);
app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handler);

app.Run();



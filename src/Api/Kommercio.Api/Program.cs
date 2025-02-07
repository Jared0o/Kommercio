using Kommercio.Modules.Users.Api;
using Kommercio.Modules.Users.Api.Endpoints;
using Kommercio.Modules.Users.Core.Entities.Enums;
using Kommercio.Modules.Users.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddUsersModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.MapGet("/", () => "It's Kommercio API!").WithName("Kommercio").WithTags("Kommercio Home");
//users
app.MapGroup("/users").WithTags("users").MapUserRoutes();
    

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UserIdentityContext>();
    await context.Database.MigrateAsync();
    
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var roles = new[] { Roles.Member.ToString(), Roles.Moderator.ToString(), Roles.Admin.ToString() };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }
}
await app.RunAsync();

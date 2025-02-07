using Kommercio.Modules.Users.Core.Dtos;
using Kommercio.Modules.Users.Core.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Kommercio.Modules.Users.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/register", async ([FromBody]RegisterUser user, [FromServices]RegisterUserUseCase userCase) =>
        {
            try
            {
                await userCase.Execute(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        });
        
        group.MapPost("/login", async ([FromBody]LoginUser user, [FromServices]LoginUserUseCase userCase) =>
        {
            try
            {
                var token = await userCase.Execute(user);
                return Results.Ok(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }
}
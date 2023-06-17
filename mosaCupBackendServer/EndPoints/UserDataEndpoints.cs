using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.Models.DbModels;

namespace mosaCupBackendServer.EndPoints;

public static class UserDataEndpoints
{
    public static void MapUserDataEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/UserData").WithTags(nameof(UserData));

        group.MapGet("/", async (mosaCupBackendServerContext db) =>
        {
            return await db.UserData.ToListAsync();
        })
        .WithName("GetAllUserData")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<UserData>, NotFound>> (string uid, mosaCupBackendServerContext db) =>
        {
            return await db.UserData.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Uid == uid)
                is UserData model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserDataById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (string uid, UserData userData, mosaCupBackendServerContext db) =>
        {
            var affected = await db.UserData
                .Where(model => model.Uid == uid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Uid, userData.Uid)
                  .SetProperty(m => m.DisplayName, userData.DisplayName)
                  .SetProperty(m => m.Name, userData.Name)
                  .SetProperty(m => m.Description, userData.Description)
                  .SetProperty(m => m.DeletedAt, userData.DeletedAt)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUserData")
        .WithOpenApi();

        group.MapPost("/", async (UserData userData, mosaCupBackendServerContext db) =>
        {
            db.UserData.Add(userData);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/UserData/{userData.Uid}", userData);
        })
        .WithName("CreateUserData")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (string uid, mosaCupBackendServerContext db) =>
        {
            var affected = await db.UserData
                .Where(model => model.Uid == uid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUserData")
        .WithOpenApi();
    }
}

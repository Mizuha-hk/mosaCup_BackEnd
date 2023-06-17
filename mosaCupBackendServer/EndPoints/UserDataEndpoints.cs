using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.Models.DbModels;
using mosaCupBackendServer.Models.ReqModels;

namespace mosaCupBackendServer.EndPoints;

public static class UserDataEndpoints
{
    public static void MapUserDataEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/UserData").WithTags(nameof(UserData));

        //Load user info
        group.MapGet("/{id}", async Task<Results<Ok<UserData>, NotFound>> (string uid, mosaCupBackendServerContext db) =>
        {
            return await db.UserData.AsNoTracking()
                .FirstOrDefaultAsync(model => model.DeletedAt == null && model.Uid == uid)
                is UserData model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserDataById")
        .WithOpenApi();

        //Search user
        group.MapGet("/search/{name}", async Task<Results<Ok<List<UserData>>, NotFound>> (string name, mosaCupBackendServerContext db) =>
        {
            if (!name.StartsWith("@"))
            {
                var userList = await db.UserData
                    .Where(model => model.DeletedAt == null && model.DisplayName.IndexOf(name) != -1)
                    .ToListAsync();
                return userList != null ? TypedResults.Ok(userList) : TypedResults.NotFound();
            }
            else
            {
                var userList = await db.UserData
                    .Where(model => model.Name.IndexOf(name) != -1)
                    .ToListAsync();
                return userList != null ? TypedResults.Ok(userList) : TypedResults.NotFound();
            }
        })
        .WithName("SearchUserAsName")
        .WithOpenApi();

        //Judge Name is Available (Available -> 1/ Not available -> 0)
        group.MapGet("/JudgeAvailable/{name}", async Task<Results<Ok<int>, NotFound>> (string userName, mosaCupBackendServerContext db) =>
        {
            return await db.UserData
                .FirstOrDefaultAsync(model => model.Name == userName) 
                is UserData model
                    ? TypedResults.Ok(0)
                    : TypedResults.Ok(1);
        });

        //Edit profile
        group.MapPut("/EditProfile", async Task<Results<Ok, NotFound>> (EditProfile userData, mosaCupBackendServerContext db) =>
        {
            var affected = await db.UserData
                .Where(model => model.Uid == userData.Uid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.DisplayName, userData.DisplayName)
                  .SetProperty(m => m.Description, userData.Description)
                );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("EditProfile")
        .WithOpenApi();

        //Create user
        group.MapPost("/", async (UserData userData, mosaCupBackendServerContext db) =>
        {
            db.UserData.Add(userData);
            await db.SaveChangesAsync();
            return TypedResults.Ok();
        })
        .WithName("CreateUser")
        .WithOpenApi();

        //Delete user
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (string uid, mosaCupBackendServerContext db) =>
        {
            var affected = await db.UserData
                .Where(model => model.Uid == uid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.DeletedAt, DateTime.UtcNow)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUser")
        .WithOpenApi();
    }
}

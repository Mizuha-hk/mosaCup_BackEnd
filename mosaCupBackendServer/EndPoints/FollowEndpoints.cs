using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.Models.DbModels;
namespace mosaCupBackendServer.EndPoints;

public static class FollowEndpoints
{
    public static void MapFollowEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Follow").WithTags(nameof(Follow));

        group.MapGet("/", async (mosaCupBackendServerContext db) =>
        {
            return await db.Follow.ToListAsync();
        })
        .WithName("GetAllFollows")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Follow>, NotFound>> (int id, mosaCupBackendServerContext db) =>
        {
            return await db.Follow.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Follow model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetFollowById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Follow follow, mosaCupBackendServerContext db) =>
        {
            var affected = await db.Follow
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, follow.Id)
                  .SetProperty(m => m.Uid, follow.Uid)
                  .SetProperty(m => m.FollowedUid, follow.FollowedUid)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateFollow")
        .WithOpenApi();

        group.MapPost("/", async (Follow follow, mosaCupBackendServerContext db) =>
        {
            db.Follow.Add(follow);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Follow/{follow.Id}",follow);
        })
        .WithName("CreateFollow")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, mosaCupBackendServerContext db) =>
        {
            var affected = await db.Follow
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteFollow")
        .WithOpenApi();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.Models.DbModels;
namespace mosaCupBackendServer.EndPoints;

public static class LikeEndpoints
{
    public static void MapLikeEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Like").WithTags(nameof(Like));

        group.MapGet("/", async (mosaCupBackendServerContext db) =>
        {
            return await db.Like.ToListAsync();
        })
        .WithName("GetAllLikes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Like>, NotFound>> (int id, mosaCupBackendServerContext db) =>
        {
            return await db.Like.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Like model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetLikeById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Like like, mosaCupBackendServerContext db) =>
        {
            var affected = await db.Like
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, like.Id)
                  .SetProperty(m => m.PostId, like.PostId)
                  .SetProperty(m => m.Uid, like.Uid)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateLike")
        .WithOpenApi();

        group.MapPost("/", async (Like like, mosaCupBackendServerContext db) =>
        {
            db.Like.Add(like);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Like/{like.Id}",like);
        })
        .WithName("CreateLike")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, mosaCupBackendServerContext db) =>
        {
            var affected = await db.Like
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteLike")
        .WithOpenApi();
    }
}

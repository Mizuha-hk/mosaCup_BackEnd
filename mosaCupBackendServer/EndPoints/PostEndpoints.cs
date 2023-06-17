using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.Models.DbModels;
namespace mosaCupBackendServer.EndPoints;

public static class PostEndpoints
{
    public static void MapPostEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Post").WithTags(nameof(Post));

        group.MapGet("/", async (mosaCupBackendServerContext db) =>
        {
            return await db.Post.ToListAsync();
        })
        .WithName("GetAllPosts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Post>, NotFound>> (int id, mosaCupBackendServerContext db) =>
        {
            return await db.Post.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Post model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPostById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Post post, mosaCupBackendServerContext db) =>
        {
            var affected = await db.Post
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, post.Id)
                  .SetProperty(m => m.Uid, post.Uid)
                  .SetProperty(m => m.Content, post.Content)
                  .SetProperty(m => m.PostedDate, post.PostedDate)
                  .SetProperty(m => m.ReplyId, post.ReplyId)
                  .SetProperty(m => m.JoyLevel, post.JoyLevel)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePost")
        .WithOpenApi();

        group.MapPost("/", async (Post post, mosaCupBackendServerContext db) =>
        {
            db.Post.Add(post);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Post/{post.Id}",post);
        })
        .WithName("CreatePost")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, mosaCupBackendServerContext db) =>
        {
            var affected = await db.Post
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePost")
        .WithOpenApi();
    }
}

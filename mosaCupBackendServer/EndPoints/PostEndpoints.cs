using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.Models.DbModels;
using mosaCupBackendServer.Models.ReqModels;
using System.Collections.Immutable;

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

        //add Post / return JoyLevel
        group.MapPost("/", async (PostReq reqData, mosaCupBackendServerContext db) =>
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Uid = reqData.Uid,
                Content = reqData.Content,
                PostedDate = DateTime.UtcNow,
                ReplyId = reqData.ReplyId,
                //JoyLevel = UseJoyModel.JudgeJoyLevel(reqData.Content)
            };
            db.Post.Add(post);
            await db.SaveChangesAsync();
            return Results.Ok(0);
        })
        .WithName("CreatePost")
        .WithOpenApi();

        //Delete Post
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, mosaCupBackendServerContext db) =>
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

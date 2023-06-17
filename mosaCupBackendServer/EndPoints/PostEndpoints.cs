using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.Models.DbModels;
using mosaCupBackendServer.Models.ReqModels;
using System.Collections.Immutable;
using JoyLevelMLModel;

namespace mosaCupBackendServer.EndPoints;

public static class PostEndpoints
{
    public static void MapPostEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Post").WithTags(nameof(Post));

        //get all
        group.MapGet("/", async (mosaCupBackendServerContext db) =>
        {
            return await db.Post
                .OrderByDescending(m => m.PostedDate)
                .ToListAsync();
        })
        .WithName("GetAllPosts")
        .WithOpenApi();

        //get 10 posts
        group.MapGet("/{page}", async (int page, mosaCupBackendServerContext db) =>
        {
            return await db.Post
                .OrderByDescending (m => m.PostedDate)
                .Skip(page * 10)
                .Take(10)
                .ToListAsync();
        })
        .WithName("GetPostsByPage")
        .WithOpenApi();

        //add Post / return JoyLevel
        group.MapPost("/", async (PostReq reqData, mosaCupBackendServerContext db) =>
        {
            var model = new JoyLevelModel.ModelInput();
            model.Sentence = reqData.Content;
            var result = JoyLevelModel.Predict(model);

            var post = new Post
            {
                Id = Guid.NewGuid(),
                Uid = reqData.Uid,
                Content = reqData.Content,
                PostedDate = DateTime.UtcNow,
                ReplyId = reqData.ReplyId,
                JoyLevel = Convert.ToInt32(result.Avg__Readers_Joy)
            };
            db.Post.Add(post);
            await db.SaveChangesAsync();
            return Results.Ok(post.JoyLevel);
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

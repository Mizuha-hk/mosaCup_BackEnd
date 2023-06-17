using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.Models.DbModels;
using mosaCupBackendServer.Models.ReqModels;
namespace mosaCupBackendServer.EndPoints;

public static class LikeEndpoints
{
    public static void MapLikeEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Like").WithTags(nameof(Like));

        //Get Like User
        group.MapGet("/{pid}", async Task<Results<Ok<List<Like>>, NotFound>> (Guid Pid, mosaCupBackendServerContext db) =>
        {
            var likeList = await db.Like
                .Where(model => model.PostId == Pid)
                .ToListAsync();

            return likeList != null ? TypedResults.Ok(likeList) : TypedResults.NotFound();
        })
        .WithName("GetLikeById")
        .WithOpenApi();

        //Like
        group.MapPost("/", async (LikeReq reqData, mosaCupBackendServerContext db) =>
        {
            var like = new Like { Id = Guid.NewGuid(), PostId = reqData.PostId, Uid = reqData.Uid };
            db.Like.Add(like);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Like/{like.Id}",like);
        })
        .WithName("CreateLike")
        .WithOpenApi();

        //Delete Like
        group.MapPost("/Delete", async Task<Results<Ok, NotFound>> (LikeReq reqData, mosaCupBackendServerContext db) =>
        {
            var likeData = await db.Like
                .FirstOrDefaultAsync(model => model.PostId == reqData.PostId && model.Uid == reqData.Uid);
            if(likeData == null)
            {
                return TypedResults.NotFound();
            }

            var affected = await db.Like
                .Where(model => model.Id == likeData.Id)
                .ExecuteDeleteAsync();
               
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteLike")
        .WithOpenApi();
    }
}

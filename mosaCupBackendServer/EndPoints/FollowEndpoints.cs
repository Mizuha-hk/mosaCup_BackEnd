using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.Models.DbModels;
using mosaCupBackendServer.Models.ReqModels;

namespace mosaCupBackendServer.EndPoints;

public static class FollowEndpoints
{
    public static void MapFollowEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Follow").WithTags(nameof(Follow));

        //Get following users
        group.MapGet("/{uid}", async Task<Results<Ok<List<Follow>>, NotFound>> (string uid, mosaCupBackendServerContext db) =>
        {
            var followUser = await db.Follow
                .Where(model => model.Uid == uid)
                .ToListAsync();
            return followUser != null ? TypedResults.Ok(followUser) : TypedResults.NotFound();
        })
        .WithName("GetFollowingUsers")
        .WithOpenApi();

        //Judge Follow or UnFollow(following -> 1/ unfollowing -> 0)
        group.MapPost("/JudgeFollow", async Task<Results<Ok<int>, NotFound>> (FollowReq reqData, mosaCupBackendServerContext db) =>
        {
            return await db.Follow
                .Where(m => m.Uid == reqData.Uid)
                .FirstOrDefaultAsync(m => m.FollowedUid == reqData.FollowedUid) 
                    is Follow m
                    ? TypedResults.Ok(1)
                    : TypedResults.Ok(0);
        })
        .WithName("JudgeFollow")
        .WithOpenApi();

        //Follow user
        group.MapPost("/", async (FollowReq reqData, mosaCupBackendServerContext db) =>
        {
            var id = Guid.NewGuid();
            var follow = new Follow { Id = id, Uid = reqData.Uid, FollowedUid = reqData.FollowedUid };
            db.Follow.Add(follow);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Follow/{follow.Id}",follow);
        })
        .WithName("FollowUser")
        .WithOpenApi();

        //UnFollow user
        group.MapPost("/Unfollow", async Task<Results<Ok, NotFound>> (FollowReq reqData, mosaCupBackendServerContext db) =>
        {
            var follow = await db.Follow
                .Where(model => model.Uid == reqData.Uid)
                .FirstOrDefaultAsync(model => model.FollowedUid == reqData.FollowedUid);
            if(follow == null)
            {
                return TypedResults.NotFound();
            }

            var affected = await db.Follow
                .Where(model => model.Id == follow.Id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UnFollowUser")
        .WithOpenApi();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mosaCupBackendServer.Data;
using mosaCupBackendServer.EndPoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<mosaCupBackendServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mosaCupBackendServerContext") ?? throw new InvalidOperationException("Connection string 'mosaCupBackendServerContext' not found.")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapUserDataEndpoints();

app.MapFollowEndpoints();

app.MapPostEndpoints();

app.MapLikeEndpoints();

app.Run();
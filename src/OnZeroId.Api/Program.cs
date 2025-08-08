using OnZeroId.Infrastructure;
using OnZeroId.Application;
using Wolverine;
using OnZeroId.Application.Features.Users.Commands.GenerateTotp;
using OnZeroId.Application.Features.Users.Commands.ValidateTotp;
using OnZeroId.Application.Features.Users.Commands.RegisterUser;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseWolverine(opt =>
{
    opt.Discovery.IncludeType<GenerateTotpCommandHandler>();
    opt.Discovery.IncludeType<ValidateTotpCommandHandler>();
    opt.Discovery.IncludeType<RegisterUserCommandHandler>();
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

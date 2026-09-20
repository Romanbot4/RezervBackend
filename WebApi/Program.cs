using Application;
using Infrastructure;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddPersistence(builder.Configuration)
    .AddInfrastructure()
    .AddApplication(builder.Configuration);

var app = builder.Build();

app.Run();

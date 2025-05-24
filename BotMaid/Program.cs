using Microsoft.Extensions.Hosting;

using NetCord;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Rest;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddDiscordGateway()
    .AddApplicationCommands();

var host = builder.Build();

// Add commands from modules
host.AddModules(typeof(Program).Assembly);

// Add handlers to handle the commands
host.UseGatewayEventHandlers();

await host.RunAsync();
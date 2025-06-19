using Microsoft.Extensions.Hosting;

using NetCord;
using NetCord.Rest;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using Lavalink4NET.NetCord;
using Lavalink4NET.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .ConfigureLavalink(config =>
    {
        config.BaseAddress = new Uri("http://lavalink:2333");
    })
    .AddDiscordGateway()
    .AddLavalink()
    .AddApplicationCommands();


var host = builder.Build();

// Add commands from modules
host.AddModules(typeof(Program).Assembly);

// Add handlers to handle the commands
host.UseGatewayEventHandlers();

await host.RunAsync();
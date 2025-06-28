using Microsoft.Extensions.Hosting;

using NetCord;
using NetCord.Rest;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using Lavalink4NET.InactivityTracking.Extensions;
using Lavalink4NET.InactivityTracking.Trackers.Users;
using Lavalink4NET.NetCord;
using Lavalink4NET.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .ConfigureLavalink(config =>
    {
        config.BaseAddress = new Uri("http://lavalink:2333");
    })
    .Configure<UsersInactivityTrackerOptions>(options =>
    {
        options.Threshold = 1;
        options.Timeout = TimeSpan.FromSeconds(30);
        options.ExcludeBots = true;
    })
    .AddDiscordGateway()
    .AddLavalink()
    .AddInactivityTracking()
    .AddApplicationCommands();

var host = builder.Build();

// Add commands from modules
host.AddModules(typeof(Program).Assembly);

// Add handlers to handle the commands
host.UseGatewayEventHandlers();

await host.RunAsync();
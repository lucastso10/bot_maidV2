namespace BotMaid;

using Lavalink4NET;
using Lavalink4NET.NetCord;
using Lavalink4NET.Players;
using Lavalink4NET.Rest.Entities.Tracks;
using NetCord.Services.ApplicationCommands;

public class MusicModule(IAudioService audioService) : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("play", "Toca uma música!")]
    public async Task<string> PlayAsync([SlashCommandParameter(Description = "Url ou pesquisa")] string query)
    {
        var retrieveOptions = new PlayerRetrieveOptions(ChannelBehavior: PlayerChannelBehavior.Join);

        var result = await audioService.Players
            .RetrieveAsync(Context, playerFactory: PlayerFactory.Queued, retrieveOptions);

        if (!result.IsSuccess)
        {
            return GetErrorMessage(result.Status);
        }

        var player = result.Player;

        var track = await audioService.Tracks
            .LoadTrackAsync(query, TrackSearchMode.YouTube);

        if (track is null)
        {
            return "No tracks found.";
        }

        await player.PlayAsync(track);

        return $"Now playing: {track.Title}";
    }

    private static string GetErrorMessage(PlayerRetrieveStatus retrieveStatus) => retrieveStatus switch
    {
        PlayerRetrieveStatus.UserNotInVoiceChannel => "Tu nem tá em um canal de voz :skull:.",
        PlayerRetrieveStatus.VoiceChannelMismatch => "Você não está no mesmo canal de voz que eu!",
        PlayerRetrieveStatus.BotNotConnected => "O bot não está conectado.",
        _ => "Unknown error.",
    };
}
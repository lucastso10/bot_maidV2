namespace BotMaid;

using System.Data.Common;
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
            return "Não achei nada!";
        }

        await player.PlayAsync(track);

        if (player.Queue.IsEmpty)
        {
            return $"Tocando agora: {track.Title}";
        }
        else
        {
            return $"{track.Title} colocado na fila em posição {player.Queue.Count}";
        }
    }


    [SlashCommand("skip", "Pula a música atual!")]
    public async Task<string> SkipAsync([SlashCommandParameter(Description = "Quantidade de músicas pra pular")] int quant = 1)
    {
        var retrieveOptions = new PlayerRetrieveOptions(ChannelBehavior: PlayerChannelBehavior.Join);

        var result = await audioService.Players
            .RetrieveAsync(Context, playerFactory: PlayerFactory.Queued, retrieveOptions);

        if (!result.IsSuccess)
        {
            return GetErrorMessage(result.Status);
        }

        var player = result.Player;

        if (player.CurrentItem is null)
        {
            return "Nenhuma música tocando!";
        }

        await player.SkipAsync(quant);

        var track = player.CurrentItem;

        if (track is null)
        {
            return "Música pulada!";
        }
        else
        {
            return $"Música pulada! Agora tocando: {track}";
        }
    }

    [SlashCommand("seek", "Pula para o tempo especificado da música!")]
    public async Task<string> SeekAsync(
        [SlashCommandParameter(Description = "segundos")] int segundos = 0,
        [SlashCommandParameter(Description = "minutos")] int minutos = 0,
        [SlashCommandParameter(Description = "horas")] int horas = 0
    )
    {
        if (segundos + minutos + horas <= 0) return "Horário inválido";

        var retrieveOptions = new PlayerRetrieveOptions(ChannelBehavior: PlayerChannelBehavior.Join);

        var result = await audioService.Players
            .RetrieveAsync(Context, playerFactory: PlayerFactory.Queued, retrieveOptions);

        if (!result.IsSuccess)
        {
            return GetErrorMessage(result.Status);
        }

        var player = result.Player;

        if (player.CurrentItem is null)
        {
            return "Não estou tocando nada!";
        }

        TimeSpan ts = TimeSpan.FromSeconds(segundos) + TimeSpan.FromMinutes(minutos) + TimeSpan.FromMinutes(horas);

        await player.SeekAsync(ts);

        if (horas > 0)
        {
            return $"Pulei para horas  {horas}:{minutos}:{segundos}";
        }
        else if (minutos > 0)
        {
            return $"Pulei para minuto {minutos}:{segundos}";
        }
        else
        {
            return $"Pulei para segundo {segundos}";
        }
    }


    private static string GetErrorMessage(PlayerRetrieveStatus retrieveStatus) => retrieveStatus switch
    {
        PlayerRetrieveStatus.UserNotInVoiceChannel => "Tu nem tá em um canal de voz :skull:.",
        PlayerRetrieveStatus.VoiceChannelMismatch => "Você não está no mesmo canal de voz que eu!",
        PlayerRetrieveStatus.BotNotConnected => "O bot não está conectado.",
        _ => "Erro desconhecido.",
    };
}
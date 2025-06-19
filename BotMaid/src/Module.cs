namespace BotMaid;

using NetCord.Services.ApplicationCommands;

public class ExampleModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("escolha", "Escolhe entre 2 opções!")]
    public static string Escolha(
        [SlashCommandParameter(Description = "Escolha 1")] string @escolha1,
        [SlashCommandParameter(Description = "Escolha 2")] string @escolha2
    )
    {
        var random = new Random();

        var falas = new List<string>
        {
            "Eu prefiro esse aqui: ",
            "Esse aqui é mais mandrak: ",
            "Esse aqui é poggers: ",
            "Esse aqui é bruh moment: ",
            "Esse aqui é melhor do que sexo: ",
            ":face_vomiting: : ",
            "Não gostei desse: ",
            "bruh :skull:: "
        };

        int falaRandom = random.Next(falas.Count);

        if (random.Next(2) == 0)
        {
            return $"{falas[falaRandom]}{escolha1}";
        }
        else
        {
            return $"{falas[falaRandom]}{escolha2}";
        }
    }

    [SlashCommand("pong", "Pong!")]
    public static string Pong() => "Ping!";
}
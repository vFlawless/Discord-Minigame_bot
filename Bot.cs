using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using wordle_bot.Commands;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using TicTacToeDiscordBot.Models;
using TicTacToeDiscordBot.TicTacToeGame;
using System;
using Minigame_bot.Commands.VierGewinnt;
using Minigame_bot.Commands.RockPaperScissors;

namespace wordle_bot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {

            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            Client = new DiscordClient(config);

            Client.Ready += Client_Ready;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(5)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<Wordle_CMD>();
            Commands.RegisterCommands<Hangman>();
            Commands.RegisterCommands<TicTacToe>();
            Commands.RegisterCommands<Viergewinnt>();
            Commands.RegisterCommands<SchereSteinPapier>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {

            return Task.CompletedTask;
        }

        public static string ReadFromJson(string jsonProperty)
        {
            using FileStream fs = File.OpenRead("bottoken.json");
            using StreamReader sr = new StreamReader(fs, new UTF8Encoding(false));
            string json = sr.ReadToEnd();

            JsonModel configJson = JsonConvert.DeserializeObject<JsonModel>(json);

            switch (jsonProperty)
            {
                case "release":
                    return configJson.ReleaseToken;
                case "debug":
                    return configJson.DebugToken;
                case "spreadsheetId":
                    return configJson.SpreadsheetId;
                case "connectionString":
                    return configJson.ConnectionString;
                default:
                    return "Nothing was found.";
            }

        }
    }
}

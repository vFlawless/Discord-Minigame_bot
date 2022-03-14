using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System.Threading.Tasks;

namespace TicTacToeDiscordBot.TicTacToeGame
{
    public class SingleplayerGame : GameElements
    {
        private Player p1;
        private AI ai;
        private Grid grid;
        private CommandContext ctx;
        private AILogic aiLogic;

        public SingleplayerGame(CommandContext context, DiscordMember aiBot, string difficulty)
        {
            ctx = context;
            p1 = new Player(ctx.Member.Id, ctx.Member.DisplayName, GameEmoji.X);
            ai = new AI(aiBot.Id, aiBot.DisplayName, GameEmoji.O, difficulty.ToLower());
            grid = new Grid(GameEmoji.Field);
            aiLogic = new AILogic();
        }

        // Initiates a single player game.
        public async Task BeginSingleplayerGame()
        {
            Multiplayer = false;
            EmbedDefaults.SetEmbedDefaultsSP(p1, ai);

            ActivePlayer = p1.Name;

            // Creates and sends the embed that is used as the gameboard
            DiscordEmbedBuilder gameBoard = CreatePlayField(grid.GameGrid);

            DiscordMessage embedMessage = await ctx.Channel.SendMessageAsync(embed: gameBoard).ConfigureAwait(false);
            await AddReactions(embedMessage);

            // Loops the game while the game isn't ended
            while (GameActive)
            {
                await MakeMove(embedMessage);
                Turn++;
            }
            SetWinnerStatus(p1, ai);
            RegisterInDatabase(p1, null, ai);
            await embedMessage.ModifyAsync(embed: new Optional<DiscordEmbed>(CreatePlayField(grid.GameGrid))).ConfigureAwait(false);
            await embedMessage.DeleteAllReactionsAsync();
        }

        // Takes turns to wait for the player and ai to make a move.
        public async Task MakeMove(DiscordMessage embed)
        {
            if (GameActive)
            {
                InteractivityResult<MessageReactionAddEventArgs> reactionResultp1 = await InteractivityResult(p1.Id, embed);

                ActivePlayer = ai.Name;
                await RemoveChoice(embed, reactionResultp1.Result.Emoji);
                await UpdateField(grid.GameGrid, embed, reactionResultp1.Result.Emoji, p1.PlayerEmoji, 1);

                if (Turn >= 2)
                    CheckWinCondition(p1.Name, grid.GameGrid);
            }

            if (GameActive)
            {
                await Task.Delay(1000);
                ActivePlayer = p1.Name;

                DiscordEmoji aiMove = ConvertAiMove();

                await RemoveChoice(embed, aiMove);
                await UpdateField(grid.GameGrid, embed, aiMove, ai.AiEmoji, 2);
                
                if (Turn >= 2)
                    CheckWinCondition(ai.Name, grid.GameGrid);
            }
        }

        // Captures the users selected emoji returns it.
        private async Task<InteractivityResult<MessageReactionAddEventArgs>> InteractivityResult(ulong id, DiscordMessage embed)
        {
            var interactivity = ctx.Client.GetInteractivity();
            return await interactivity.WaitForReactionAsync(
                x => x.Message == embed &&
                     x.User.Id == id && GameEmoji.OneThroughNine().Contains(x.Emoji)).ConfigureAwait(false);
        }

        // Converts the bots move to an emoji and returns it.
        private DiscordEmoji ConvertAiMove()
        {
            int aiMove = aiLogic.MakeMove(grid.GameGrid, ai.Difficulty, Turn);

            if (aiMove == -1)
            {
                ctx.Channel.SendMessageAsync("The bot couldn't figure out a move.");
                GameActive = false;
            }

            return GameEmoji.OneThroughNine()[aiMove];
        }

        private void SetWinnerStatus(Player player, AI aiPlayer)
        {
            if (Winner == player.Name)
            {
                player.SetGameResult("win");
                aiPlayer.SetGameResult("loss");
            }
            else if (Winner == aiPlayer.Name)
            {
                aiPlayer.SetGameResult("win");
                player.SetGameResult("loss");
            }
            else
            {
                player.SetGameResult("tie");
                aiPlayer.SetGameResult("tie");
            }
        }
    }
}

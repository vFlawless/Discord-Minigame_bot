using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;

namespace TicTacToeDiscordBot.TicTacToeGame
{
    public class MultiplayerGame : GameElements
    {
        private Player p1;
        private Player p2;
        private Grid grid;
        private CommandContext ctx;

        public MultiplayerGame(CommandContext context, DiscordMember playerTwo)
        {
            ctx = context;
            p1 = new Player(ctx.Member.Id, ctx.Member.DisplayName, GameEmoji.X);
            p2 = new Player(playerTwo.Id, playerTwo.DisplayName, GameEmoji.O);
            grid = new Grid(GameEmoji.Field);
        }
        
        // Initates multiplayer game
        public async Task BeginMultiplayerGame()
        {
            Multiplayer = true;
            EmbedDefaults.SetEmbedDefaultsMP(p1, p2);

            ActivePlayer = p1.Name;

            // Creates and sends the embed that is used as the gameboard
            DiscordEmbedBuilder gameBoard = CreatePlayField(grid.GameGrid);

            DiscordMessage embedMessage = await ctx.Channel.SendMessageAsync(embed: gameBoard).ConfigureAwait(false);
            await AddReactions(embedMessage);

            // Loops the game until the game is over.
            while (GameActive)
            {
                await MakeMove(embedMessage);
                Turn++;
            }

            SetWinnerStatus(p1, p2);
            RegisterInDatabase(p1, p2);
            await embedMessage.ModifyAsync(embed: new Optional<DiscordEmbed>(CreatePlayField(grid.GameGrid))).ConfigureAwait(false);
            await embedMessage.DeleteAllReactionsAsync();
        }

        // Waits for the player1 to make a move - after it will wait for player 2
        public async Task MakeMove(DiscordMessage embed)
        {
            if (GameActive)
            {
                InteractivityResult<MessageReactionAddEventArgs> reactionResultp1 = await InteractivityResult(p1.Id, embed);

                ActivePlayer = p2.Name;
                await RemoveChoice(embed, reactionResultp1.Result.Emoji);
                await UpdateField(grid.GameGrid, embed, reactionResultp1.Result.Emoji, p1.PlayerEmoji, 1);
                if (Turn >= 2)
                    CheckWinCondition(p1.Name, grid.GameGrid);
            }

            if (GameActive)
            {
                InteractivityResult<MessageReactionAddEventArgs> reactionResultp2 = await InteractivityResult(p2.Id, embed);

                ActivePlayer = p1.Name;
                await RemoveChoice(embed, reactionResultp2.Result.Emoji);
                await UpdateField(grid.GameGrid, embed, reactionResultp2.Result.Emoji, p2.PlayerEmoji, 2);
                if (Turn >= 2)
                    CheckWinCondition(p2.Name, grid.GameGrid);
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

        private void SetWinnerStatus(Player player1, Player player2)
        {
            if (Winner == player1.Name)
            {
                player1.SetGameResult("win");
                player2.SetGameResult("loss");
            }
            else if (Winner == player2.Name)
            {
                player2.SetGameResult("win");
                player1.SetGameResult("loss");
            }
            else
            {
                player1.SetGameResult("tie");
                player2.SetGameResult("tie");
            }
        }
    }
}

using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToeDiscordBot.External_Dependencies;

namespace TicTacToeDiscordBot.TicTacToeGame
{
    public class GameElements
    {
        ScoreDatabase sdb = new ScoreDatabase();
        public bool GameActive = true;
        public string ActivePlayer;
        public bool Multiplayer;
        public string Winner;
        public int Turn;
        
        // Creates an embedded message with the game board. 
        public DiscordEmbedBuilder CreatePlayField(List<Field> g)
        {
            string descriptionString = $"";
            string bottomMessage = $"\n\n";
            DiscordEmbedBuilder newEmbed;

            for (int i = 0; i < g.Count; i++)
            {
                descriptionString += $"{i + 1} " + g[i].FieldEmoji + "    ";
                if (i == 2 || i == 5)
                    descriptionString += "\n\n";
            }

            // Result message
            if (GameActive)
                bottomMessage += $"Warten auf {ActivePlayer}'s Zug";

            else if (GameActive == false && !string.IsNullOrEmpty(Winner))
                bottomMessage += $"{Winner} hat das Spiel gewonnen.";

            else if (GameActive == false && string.IsNullOrEmpty(Winner))
            {
                Winner = "tie";
                bottomMessage += $"Das Spiel ist en Unentscheiden";
            }

            // Displays embed based on MP or SP
            if (Multiplayer)
            { 
                newEmbed = new DiscordEmbedBuilder
                {
                    Title = EmbedDefaults.Title,
                    Description = EmbedDefaults.PlayerAndEmoji + "\n\n" +
                                  descriptionString + bottomMessage,
                    Color = DiscordColor.Green
                };
            }
            else
            {
                newEmbed = new DiscordEmbedBuilder
                {
                    Title = EmbedDefaults.Title,
                    Description = EmbedDefaults.PlayerAndEmoji + "\n\n" +
                                  descriptionString + bottomMessage,
                    Color = DiscordColor.Green
                };
            }

            return newEmbed;
        }

        // Adds prefixed reactions to the embed.
        public async Task AddReactions(DiscordMessage embed)
        {
            foreach(DiscordEmoji discordEmoji in GameEmoji.OneThroughNine())
                await embed.CreateReactionAsync(discordEmoji);
        }

        // Removes the emoji that has been recently selected by a player or AI
        public async Task RemoveChoice(DiscordMessage embed, DiscordEmoji demoji)
        {
            await embed.DeleteReactionsEmojiAsync(demoji).ConfigureAwait(false);
        }

        // Updates the grid with Emoji and value.
        public async Task UpdateField(List<Field> grid, DiscordMessage embed, DiscordEmoji demoji, DiscordEmoji playerEmoji, int playerValue)
        {
            for (int i = 0; i < 9; i++)
            {
                if (demoji == GameEmoji.OneThroughNine()[i])
                {
                    grid[i].FieldEmoji = playerEmoji;
                    grid[i].FieldValue = playerValue;
                }
            }

            await embed.ModifyAsync(embed: new Optional<DiscordEmbed>(CreatePlayField(grid))).ConfigureAwait(false);
        }

        // Checks if the player or AI has met the win conditions
        public void CheckWinCondition(string playerName, List<Field> grid)
        {
            // Horizontal row 1
            if (grid[0].FieldValue == grid[1].FieldValue && grid[1].FieldValue == grid[2].FieldValue && grid[0].FieldValue != 0)
            {
                Winner = playerName;
                GameActive = false;
            }

            // Horizontal row 2
            if (grid[3].FieldValue == grid[4].FieldValue && grid[4].FieldValue == grid[5].FieldValue && grid[3].FieldValue != 0)
            {
                Winner = playerName;
                GameActive = false;
            }

            // Horizontal row 3
            if (grid[6].FieldValue == grid[7].FieldValue && grid[7].FieldValue == grid[8].FieldValue && grid[6].FieldValue != 0)
            {
                Winner = playerName;
                GameActive = false;
            }

            // Vertical row 1
            if (grid[0].FieldValue == grid[3].FieldValue && grid[3].FieldValue == grid[6].FieldValue && grid[0].FieldValue != 0)
            {
                Winner = playerName;
                GameActive = false;
            }

            // Vertical row 2
            if (grid[1].FieldValue == grid[4].FieldValue && grid[4].FieldValue == grid[7].FieldValue && grid[1].FieldValue != 0)
            {
                Winner = playerName;
                GameActive = false;
            }

            // Vertical row 3
            if (grid[2].FieldValue == grid[5].FieldValue && grid[5].FieldValue == grid[8].FieldValue && grid[2].FieldValue != 0)
            {
                Winner = playerName;
                GameActive = false;
            }

            // Diagonal row 1
            if (grid[0].FieldValue == grid[4].FieldValue && grid[4].FieldValue == grid[8].FieldValue && grid[0].FieldValue != 0)
            {
                Winner = playerName;
                GameActive = false;
            }

            // Diagonal row 2
            if (grid[2].FieldValue == grid[4].FieldValue && grid[4].FieldValue == grid[6].FieldValue && grid[2].FieldValue != 0)
            {
                Winner = playerName;
                GameActive = false;
            }

            if (grid[0].FieldValue != 0 && grid[1].FieldValue != 0 && grid[2].FieldValue != 0 &&
                grid[3].FieldValue != 0 && grid[4].FieldValue != 0 && grid[5].FieldValue != 0 &&
                grid[6].FieldValue != 0 && grid[7].FieldValue != 0 && grid[8].FieldValue != 0)
            {
                GameActive = false;
            }
        }

        public void RegisterInDatabase(Player p1, Player p2 = null, AI ai = null)
        {
            if (p2 == null)
            {
                sdb.RegisterGameData(p1, null);
                sdb.RegisterGameData(null, ai);
            }
            else
            {
                sdb.RegisterGameData(p1, null);
                sdb.RegisterGameData(p2, null);
            }
        }
    }
}

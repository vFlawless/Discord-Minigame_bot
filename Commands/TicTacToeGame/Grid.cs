using DSharpPlus.Entities;
using System.Collections.Generic;

namespace TicTacToeDiscordBot.TicTacToeGame
{
    // Class for the grid Model.
    public class Grid
    {
        public List<Field> GameGrid = new List<Field>();

        public Grid(DiscordEmoji fieldEmoji)
        {

            GameGrid.AddRange(new List<Field>
            {
                new Field(fieldEmoji, 0), new Field(fieldEmoji, 0), new Field(fieldEmoji, 0),
                new Field(fieldEmoji, 0), new Field(fieldEmoji, 0), new Field(fieldEmoji, 0),
                new Field(fieldEmoji, 0), new Field(fieldEmoji, 0), new Field(fieldEmoji, 0)
            });
        }
    }
}

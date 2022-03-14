using DSharpPlus.Entities;

namespace TicTacToeDiscordBot.TicTacToeGame
{
    // Model for the Field class 
    public class Field
    {
        public DiscordEmoji FieldEmoji { get; set; }
        public int FieldValue { get; set; }

        public Field(DiscordEmoji fieldEmoji, int fieldValue)
        {
            FieldEmoji = fieldEmoji;
            FieldValue = fieldValue;
        }
    }
}

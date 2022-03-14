using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TicTacToeDiscordBot.TicTacToeGame
{
    // Defaults for the embed in both MP and SP
    public static class EmbedDefaults
    {
        public static string Title;
        public static string PlayerAndEmoji;

        public static void SetEmbedDefaultsMP(Player p1, Player p2)
        {
            Title = $"{p1.Name} hat dich zu einem Spiel Tic Tac Toe herausgefordert!";
            PlayerAndEmoji = $"{p1.Name}: {p1.PlayerEmoji}\n" +
                             $"{p2.Name}: {p2.PlayerEmoji}";
        }

        public static void SetEmbedDefaultsSP(Player p1, AI ai)
        {
            Title = $"{p1.Name} hat den Bot zu einem Spiel Tic Tac Toe herausgefordert!";
            PlayerAndEmoji = $"{p1.Name}: {p1.PlayerEmoji}\n" +
                             $"{ai.Name}: {ai.AiEmoji}\nDifficulty: {char.ToUpper(ai.Difficulty[0]) + ai.Difficulty.Substring(1).ToLower()}";
        }
    }
}

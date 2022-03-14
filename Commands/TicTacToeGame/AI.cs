using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;

namespace TicTacToeDiscordBot.TicTacToeGame
{
    // Model for the bot AI
    public class AI
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public DiscordEmoji AiEmoji { get; set; }
        public string Difficulty { get; set; }
        public string GameResult { get; set; }

        public AI(ulong id, string name, DiscordEmoji aiEmoji, string difficulty)
        {
            Id = id;
            Name = name;
            AiEmoji = aiEmoji;

            if (difficulty.Contains("easy"))
                Difficulty = "easy";
            else if (difficulty.Contains("medium"))
                Difficulty = "medium";
            else
                Difficulty = "hard";
        }

        public void SetGameResult(string result)
        {
            GameResult = result;
        }
    }
}

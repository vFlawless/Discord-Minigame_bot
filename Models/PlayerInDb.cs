using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeDiscordBot.Models
{
    public class PlayerInDb
    {
        public string UserId { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }

        public PlayerInDb(string userId, int wins, int losses, int ties)
        {
            UserId = userId;
            Wins = wins;
            Losses = losses;
            Ties = ties;
        }
    }
}

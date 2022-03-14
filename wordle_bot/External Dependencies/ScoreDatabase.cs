using System;
using TicTacToeDiscordBot.Models;
using TicTacToeDiscordBot.TicTacToeGame;

namespace TicTacToeDiscordBot.External_Dependencies
{
    public class ScoreDatabase : Database
    {
        public void RegisterGameData(Player player = null, AI ai = null)
        {
            if (player != null)
            {
                string playerId = player.Id.ToString();
                string playerResult = player.GameResult;
                bool isRegistered = IsUserDataRegistered(playerId);

                if(isRegistered)
                    UpdateUserData(playerId, playerResult);
                else
                {
                    CreateUserData(playerId);
                    UpdateUserData(playerId, playerResult);
                }
            }

            if (ai != null)
            {
                string aiId = ai.Id.ToString();
                string aiResult = ai.GameResult;
                bool isRegistered = IsUserDataRegistered(aiId);

                if (isRegistered)
                    UpdateUserData(aiId, aiResult);
                else
                {
                    CreateUserData(aiId);
                    UpdateUserData(aiId, aiResult);
                }
            }
        }

        private void CreateUserData(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                string sqlString = $"insert into ScoreList(userId, wins, losses, ties)" +
                                   $"values('{userId}', 0, 0, 0)";

                ExecuteSql(sqlString);
            }
            else
            {
                Console.WriteLine("Something went wrong. Player data could not be created in database.");
            }
        }

        private void UpdateUserData(string userId, string result)
        {
            PlayerInDb member = FetchPreviousResults(userId);

            if (member != null)
            {
                int wins = member.Wins;
                int losses = member.Losses;
                int ties = member.Ties;

                if (result == "win")
                    wins++;
                else if (result == "loss")
                    losses++;
                else
                    ties++;

                string sqlUpdate = $"update ScoreList set wins = {wins}, losses = {losses}, ties = {ties}" +
                                   $"where userId = {member.UserId}";

                ExecuteSql(sqlUpdate);
            }
            else
            {
                Console.WriteLine("Something went wrong. Player data could not be updated in database.");
            }
        }

        private bool IsUserDataRegistered(string userId)
        {
            if (string.IsNullOrEmpty(GetId("select * from ScoreList where userId = " + userId)))
                return false;

            return true;
        }

    }
}

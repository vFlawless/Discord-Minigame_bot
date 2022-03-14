using System;
using System.Data.SqlClient;
using TicTacToeDiscordBot.Models;
using wordle_bot;

namespace TicTacToeDiscordBot.External_Dependencies
{
    public class Database
    {
        public SqlConnection connection;
        public SqlCommand command;

        public void PrepareSql(string sqlString)
        {
            connection = new SqlConnection(Bot.ReadFromJson("connectionString"));

            try
            {
                connection.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = sqlString;
        }

        public void ExecuteSql(string sqlString)
        {
            PrepareSql(sqlString);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public string GetId(string sqlSelect)
        {
            PrepareSql(sqlSelect);
            SqlDataReader sqldr = command.ExecuteReader();

            if (sqldr.HasRows)
                while (sqldr.Read())
                {
                    string id = sqldr["userId"].ToString();
                    connection.Close();
                    return id;
                }

            connection.Close();
            return null;
        }

        public PlayerInDb FetchPreviousResults(string memberId)
        {
            string sqlSelect = $"select * from ScoreList where userId = " + memberId;
            PrepareSql(sqlSelect);
            SqlDataReader sqldr = command.ExecuteReader();

            if (sqldr.HasRows)
                while (sqldr.Read())
                {
                    string userId = sqldr["userId"].ToString();
                    int wins = Convert.ToInt32(sqldr["wins"].ToString());
                    int losses = Convert.ToInt32(sqldr["losses"].ToString());
                    int ties = Convert.ToInt32(sqldr["ties"].ToString());
                    connection.Close();
                    return new PlayerInDb(userId, wins, losses, ties);
                }

            return new PlayerInDb("", 0, 0, 0);
        }
    }
}

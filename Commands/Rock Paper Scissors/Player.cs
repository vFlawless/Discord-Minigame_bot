using DSharpPlus.Entities;

namespace Minigame_bot.Commands.RockPaperScissors
{
    public class Player
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        public Player(ulong id, string name)
        {
            Id = id;
            Name = name;
        }

    }

}
using System;
namespace Minigame_bot.Commands.RockPaperScissors
{
    public class AI
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        public AI(ulong id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

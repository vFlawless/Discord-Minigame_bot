﻿using System;
using DSharpPlus.Entities;

namespace Minigame_bot.Commands.VierGewinnt
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
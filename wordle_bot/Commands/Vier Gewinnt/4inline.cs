using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;


namespace Minigame_bot.Commands.VierGewinnt
{
    public class Viergewinnt : BaseCommandModule
    {
        [Command("4inline")]
        [Aliases("4il")]
        [Description("")]
        public async Task Testing(CommandContext ctx, string userMention)
        {
            DiscordMember playerTwo = null;

            List<DiscordMember> dmList = await GetMemberList(ctx);

            await Task.WhenAny(GetMemberList(ctx));
            
            // Checks if the member that has been @'d is a member of the channel and if it is a bot. 
            foreach (DiscordMember discordMember in dmList)
            {
                if (userMention.Contains(discordMember.Id.ToString()) && discordMember.IsBot)
                {
                    playerTwo = discordMember;
                    SinglePlayer4g sp = new SinglePlayer4g(ctx, playerTwo);
                    await sp.StartSinglePlayer();
                }
                else if (userMention.Contains(discordMember.Id.ToString()))
                {
                    playerTwo = discordMember;
                    Multiplayer4g mp = new Multiplayer4g(ctx, playerTwo);
                    await mp.StartMultiplayerGame();
                }
            }

            if (playerTwo == null)
                await ctx.Channel.SendMessageAsync("The Player couldn't be found.");
        }

        // Returns the list of all members
        public async Task<List<DiscordMember>> GetMemberList(CommandContext ctx)
        {
            List<DiscordMember> memberList = await Task.Run(() => ctx.Channel.Users.ToList());

            return new List<DiscordMember>(memberList);
        }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Minigame_bot.Commands.RockPaperScissors
{
    public class SchereSteinPapier : BaseCommandModule
    {
        [Command("SchereSteinPapier")]
        [Aliases("RockPaperScissors", "SSP", "RPS")]
        [Description("")]
        public async Task RockPaperScissors(CommandContext ctx, string userMention)
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
                    SinglePlayerRPS sp = new SinglePlayerRPS(ctx, playerTwo);
                    await sp.StartSinglePlayer();
                }
                else if (userMention.Contains(discordMember.Id.ToString()))
                {
                    playerTwo = discordMember;
                    MultiplayerRPS mp = new MultiplayerRPS(ctx, playerTwo);
                    await mp.StartMultiplayerGame();
                }
            }

            if (playerTwo == null)
                await ctx.Channel.SendMessageAsync("Der gefragte Spieler konnte nicht gefunden werden.");
        }

        // Returns the list of all members
        public async Task<List<DiscordMember>> GetMemberList(CommandContext ctx)
        {
            List<DiscordMember> memberList = await Task.Run(() => ctx.Channel.Users.ToList());

            return new List<DiscordMember>(memberList);
        }
    }
}

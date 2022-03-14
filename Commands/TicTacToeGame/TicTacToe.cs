using System;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeDiscordBot.TicTacToeGame
{
    public class TicTacToe : BaseCommandModule
    {
        [Command("tictactoe")]
        [Aliases("ttt")]
        [Description("Spiele TicTacToe mit einem Freund oder gegen die AI \n um gegen einen Spieler zu spielen - schreibe: \"!ttt @DiscordUser\"\nUm gegen die AI zu spielen - schreibe: \"!ttt @DiscordBot difficulty\"")]
        public async Task TicTacToeGame(CommandContext ctx, string userMention, string difficulty = "")
        {
            DiscordMember playerTwo = null;

            List<DiscordMember> dmList = await GetMemberList(ctx);

            await Task.WhenAny(GetMemberList(ctx));
            GameEmoji.InitEmoji(ctx);

            // Checks if the member that has been @'d is a member of the channel and if it is a bot. 
            foreach (DiscordMember discordMember in dmList)
            {
                if (userMention.Contains(discordMember.Id.ToString()) && discordMember.IsBot)
                {
                    playerTwo = discordMember;

                    if (string.IsNullOrEmpty(difficulty))
                        await ctx.Channel.SendMessageAsync(
                            "Probierst du gegen die AI zu spielen? Sei dir sicher eine Schwierigkeit dazu zu schrieben. \nFormat: \"!ttt @DiscordBot difficulty\"\n Es gibt momentan 3 Schwierigkeiten: Easy, Medium & Hard");
                    else
                    {
                        difficulty = difficulty.ToLower();
                        SingleplayerGame sp = new SingleplayerGame(ctx, playerTwo, difficulty);
                        await sp.BeginSingleplayerGame();
                    }
                }

                else if (userMention.Contains(discordMember.Id.ToString()))
                {
                    playerTwo = discordMember;
                    MultiplayerGame mp = new MultiplayerGame(ctx, playerTwo);
                    await mp.BeginMultiplayerGame();
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

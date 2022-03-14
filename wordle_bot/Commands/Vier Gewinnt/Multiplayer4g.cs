using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace Minigame_bot.Commands.VierGewinnt
{
    public class Multiplayer4g
    {
        private Player p1;
        private Player p2;
        private CommandContext ctx;
        private DiscordUser user;
        private const string empty = ":white_large_square:";
        private const string p1emote = ":green_square:";
        private const string p2emote = ":red_square:";

        public Multiplayer4g(CommandContext context, DiscordMember playerTwo)
        {
            ctx = context;
            p1 = new Player(ctx.Member.Id, ctx.Member.DisplayName);
            p2 = new Player(playerTwo.Id, playerTwo.DisplayName);
            user = playerTwo;
        }


        public async Task StartMultiplayerGame()
        {
            var interactivity = ctx.Client.GetInteractivity();
            string Playeractive = null;

            var Row1Emoji = DiscordEmoji.FromName(ctx.Client, ":one:");
            var Row2Emoji = DiscordEmoji.FromName(ctx.Client, ":two:");
            var Row3Emoji = DiscordEmoji.FromName(ctx.Client, ":three:");
            var Row4Emoji = DiscordEmoji.FromName(ctx.Client, ":four:");
            var Row5Emoji = DiscordEmoji.FromName(ctx.Client, ":five:");
            var Row6Emoji = DiscordEmoji.FromName(ctx.Client, ":six:");
            var Row7Emoji = DiscordEmoji.FromName(ctx.Client, ":seven:");

            var R1 = new DiscordComponentEmoji(Row1Emoji);
            var R2 = new DiscordComponentEmoji(Row2Emoji);
            var R3 = new DiscordComponentEmoji(Row3Emoji);
            var R4 = new DiscordComponentEmoji(Row4Emoji);
            var R5 = new DiscordComponentEmoji(Row5Emoji);
            var R6 = new DiscordComponentEmoji(Row6Emoji);
            var R7 = new DiscordComponentEmoji(Row7Emoji);

            var Row1Button = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "1", null, false, R1);
            var Row2Button = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "2", null, false, R2);
            var Row3Button = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "3", null, false, R3);
            var Row4Button = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "4", null, false, R4);
            var Row5Button = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "5", null, false, R5);
            var Row6Button = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "6", null, false, R6);
            var Row7Button = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "7", null, false, R7);

            string[,] Spielfeld = new string[6, 7];
            Spielfeld = Fill(Spielfeld);
            string Desc = Concat(Spielfeld);

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{p1.Name} challenges {p2.Name} to FourInLine",
                Color = DiscordColor.Yellow,
                Description = Desc
            };

            var msg = await new DiscordMessageBuilder()
                .WithEmbed(embed)
                .AddComponents(Row1Button, Row2Button, Row3Button, Row4Button, Row5Button)
                .AddComponents(Row6Button, Row7Button)
                .SendAsync(ctx.Channel);

            string Title = $"{p1.Name} challenges {p2.Name} to FourInLine";
            int Moves = 0;

            while (true)
            {
                await msg.ModifyAsync(x =>
                {
                    DiscordEmbedBuilder embed = new();
                    embed.WithTitle(Title);
                    embed.WithDescription(Desc);
                    embed.WithColor(DiscordColor.Yellow);
                    x.Embed = embed.Build();
                    x.AddComponents(Row1Button, Row2Button, Row3Button, Row4Button, Row5Button);
                    x.AddComponents(Row6Button, Row7Button);
                });

                if (isWin(Spielfeld))
                {
                    break;
                }

                if (Moves == 42)    //Check for draw
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle("It's a draw");
                        embed.WithDescription(Desc);
                        embed.WithColor(DiscordColor.Yellow);
                        x.Embed = embed.Build();
                    });
                    return;
                }

                var reactionResult = await interactivity.WaitForButtonAsync(msg, ctx.User, null);
                Playeractive = "Player 1";

                if (Check_if_full(Spielfeld, int.Parse(reactionResult.Result.Id) - 1))
                {
                    Title = "This row is allready full!";
                    continue;
                }

                int row = GetPos(Spielfeld, int.Parse(reactionResult.Result.Id) - 1); //Get Position to enter
                Spielfeld[row, int.Parse(reactionResult.Result.Id) - 1] = p1emote;
                Moves++;
                Title = $"{p1.Name} challenges {p2.Name} to FourInLine";
                Desc = Concat(Spielfeld);

                await msg.ModifyAsync(x =>
                {
                    DiscordEmbedBuilder embed = new();
                    embed.WithTitle(Title);
                    embed.WithDescription(Desc);
                    embed.WithColor(DiscordColor.Yellow);
                    x.Embed = embed.Build();
                    x.AddComponents(Row1Button, Row2Button, Row3Button, Row4Button, Row5Button);
                    x.AddComponents(Row6Button, Row7Button);
                });

                if (isWin(Spielfeld))
                {
                    break;
                }

                if (Moves == 42)    //Check for draw 
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle(Title);
                        embed.WithDescription(Desc);
                        embed.WithColor(DiscordColor.Yellow);
                        x.Embed = embed.Build();
                    });
                    return;
                }

                Playeractive = "Player 2";
                reactionResult = await interactivity.WaitForButtonAsync(msg, user, null);
                
                if (Check_if_full(Spielfeld, int.Parse(reactionResult.Result.Id) - 1))
                {
                    Title = "This row is allready full!";
                    continue;
                }

                row = GetPos(Spielfeld, int.Parse(reactionResult.Result.Id) - 1); //Get Position to enter
                Spielfeld[row, int.Parse(reactionResult.Result.Id) - 1] = p2emote;
                Moves++;
                Desc = Concat(Spielfeld);
            }

            Playeractive = Playeractive == "Player 1" ? p1.Name : p2.Name;
            await msg.ModifyAsync(x =>
            {
                DiscordEmbedBuilder embed = new();
                embed.WithTitle($"The winner is {Playeractive}");
                embed.WithDescription(Desc);
                embed.WithColor(DiscordColor.Yellow);
                x.Embed = embed.Build();
            });
            return;
        }


        private static string Concat(string[,] Spielfeld)
        {
            string concat = "";
            for (int i = 0; i < Spielfeld.GetLength(0); i++)
            {
                for (int j = 0; j < Spielfeld.GetLength(1); j++)
                {
                    concat += Spielfeld[i, j] + " ";
                }
                concat += "\n";
            }
            return concat;
        }

        private static string[,] Fill(string[,] Spielfeld)
        {
            for (int i = 0; i < Spielfeld.GetLength(0); i++)
            {
                for (int j = 0; j < Spielfeld.GetLength(1); j++)
                {
                    Spielfeld[i, j] = empty;
                }
            }
            return Spielfeld;
        }

        private static bool isWin(string[,] Spielfeld)
        {
            //Check Horizontally for win    

            for (int i = 0; i < Spielfeld.GetLength(0); i++)    //Height
            {
                for (int k = 0; k < 4; k++) //Width
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (Spielfeld[i, k + j] == empty)
                            break;
                        if (j != 0 && Spielfeld[i, k + j] != Spielfeld[i, (k + j) - 1])
                            break;
                        if (j == 3)
                            return true;
                    }
                }
            }

            //Check Vertically for win
            for (int i = 0; i < Spielfeld.GetLength(1); i++)    //Width 
            {
                for (int k = 0; k < 3; k++) //Height
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (Spielfeld[k + j, i] == empty)
                            break;
                        if (j != 0 && Spielfeld[k + j, i] != Spielfeld[(k + j) - 1, i])
                            break;
                        if (j == 3)
                            return true;
                    }
                }
            }

            //Check cross (up left to down right)   
            for (int i = 0; i < 3; i++) //Height
            {
                for (int j = 0; j < 4; j++) //Width
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (Spielfeld[i + k, j + k] == empty)
                            break;
                        if ((i != 0 && k != 0) && Spielfeld[i + k, j + k] != Spielfeld[(i + k) - 1, (j + k) - 1])
                            break;
                        if (k == 3)
                            return true;
                    }
                }
            }

            //Check cross (down left to up right)
            for (int i = Spielfeld.GetLength(0) - 1; i > 2; i--)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (Spielfeld[i - k, j + k] == empty)
                            break;
                        if ((i != Spielfeld.GetLength(0) - 1 && k != 0) && Spielfeld[i - k, j + k] != Spielfeld[(i - k) + 1, (j + k) - 1])
                            break;
                        if (k == 3)
                            return true;
                    }
                }
            }
            return false;
        }

        private static bool Check_if_full(string[,] Spielfeld, int row)
        {
            return Spielfeld[0, row] != empty;
        }

        private static int GetPos(string[,] Spielfeld, int row)
        {
            for (int i = 0; i < 6; i++) // 6 = Height 
            {
                if (Spielfeld[i, row] != empty)
                {
                    return i - 1;
                }
            }
            return 5; //If empty
        }
    }
}

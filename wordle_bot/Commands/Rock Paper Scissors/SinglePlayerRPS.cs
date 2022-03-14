using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace Minigame_bot.Commands.RockPaperScissors
{
    public class SinglePlayerRPS
    {
        private Player p1;
        private AI ai;
        private CommandContext ctx;
        
        public SinglePlayerRPS(CommandContext context, DiscordMember aiBot)
        {
            ctx = context;
            p1 = new Player(ctx.Member.Id, ctx.Member.DisplayName);
            ai = new AI(aiBot.Id, aiBot.DisplayName);
        }

        public async Task StartSinglePlayer()
        {
            var RockEmoji = DiscordEmoji.FromName(ctx.Client, ":rock:");
            var PaperEmoji = DiscordEmoji.FromName(ctx.Client, ":page_facing_up:");
            var ScissorsEmoji = DiscordEmoji.FromName(ctx.Client, ":scissors:");
            var RE = new DiscordComponentEmoji(RockEmoji);
            var PE = new DiscordComponentEmoji(PaperEmoji);
            var SE = new DiscordComponentEmoji(ScissorsEmoji);

            var RockButton = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "Rock", null, false, RE);
            var PaperButton = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "Paper", null ,false, PE);
            var ScissorsButton = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "Scissors", null, false, SE);

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{p1.Name} fordert den Bot zu Schere Stein Papier heraus",
                Color = DiscordColor.Lilac,
                Description = $"{ p1.Name }: \n { ai.Name}: \n\n\nAuf {ctx.Member.DisplayName}'s Entscheidung warten"
            };

            var msg = await new DiscordMessageBuilder()
                .WithEmbed(embed)
                .AddComponents(RockButton, PaperButton, ScissorsButton)
                .SendAsync(ctx.Channel);
            
            var interactivity = ctx.Client.GetInteractivity();

            var reactionResult = await interactivity.WaitForButtonAsync(msg, ctx.User, null);

            List<string> possibilitys = new List<string> { "Rock", "Paper", "Scissors" };
            Random rnd = new();
            var Player2 = possibilitys[rnd.Next(3)]; //Get AI's Guess (random)
            
            if (reactionResult.Result.Id == RockButton.CustomId)
            {
                switch (Winner("Rock", Player2))
                {
                    case "Unentschieden":
                        {
                            await msg.ModifyAsync(x =>
                            {
                                DiscordEmbedBuilder embed = new();
                                embed.WithTitle("Es ist Unentschieden");
                                embed.WithDescription($"{p1.Name}: {RockEmoji} \n {ai.Name}: {RockEmoji}");
                                embed.WithColor(DiscordColor.Lilac);
                                x.Embed = embed.Build();
                            });
                            break;
                        }

                    case "Gewonnen":
                        {
                            await msg.ModifyAsync(x =>
                            {
                                DiscordEmbedBuilder embed = new();
                                embed.WithTitle($"{p1.Name} hat gewonnen!");
                                embed.WithDescription($"{p1.Name}: {RockEmoji} \n {ai.Name}: {ScissorsEmoji}");
                                embed.WithColor(DiscordColor.Lilac);
                                x.Embed = embed.Build();
                            });
                            break;
                        }

                    case "Verloren":
                        {
                            await msg.ModifyAsync(x =>
                            {
                                DiscordEmbedBuilder embed = new();
                                embed.WithTitle($"{p1.Name} hat verloren!");
                                embed.WithDescription($"{p1.Name}: {RockEmoji} \n {ai.Name}: {PaperEmoji}");
                                embed.WithColor(DiscordColor.Lilac);
                                x.Embed = embed.Build();
                            });
                            break;
                        }
                }
            }
            else if (reactionResult.Result.Id == PaperButton.CustomId)  ////////////
            {
                switch (Winner("Paper", Player2))
                {
                    case "Unentschieden":
                        {
                            await msg.ModifyAsync(x =>
                            {
                                DiscordEmbedBuilder embed = new();
                                embed.WithTitle("Es ist Unentschieden");
                                embed.WithDescription($"{p1.Name}: {PaperEmoji} \n {ai.Name}: {PaperEmoji}");
                                embed.WithColor(DiscordColor.Lilac);
                                x.Embed = embed.Build();
                            });
                            break;
                        }

                    case "Gewonnen":
                        {
                            await msg.ModifyAsync(x =>
                            {
                                DiscordEmbedBuilder embed = new();
                                embed.WithTitle($"{p1.Name} hat gewonnen!");
                                embed.WithDescription($"{p1.Name}: {PaperEmoji} \n {ai.Name}: {RockEmoji}");
                                embed.WithColor(DiscordColor.Lilac);
                                x.Embed = embed.Build();
                            });
                            break;
                        }

                    case "Verloren":
                        {
                            await msg.ModifyAsync(x =>
                            {
                                DiscordEmbedBuilder embed = new();
                                embed.WithTitle($"{p1.Name} hat verloren!");
                                embed.WithDescription($"{p1.Name}: {PaperEmoji} \n {ai.Name}: {ScissorsEmoji}");
                                embed.WithColor(DiscordColor.Lilac);
                                x.Embed = embed.Build();
                            });
                            break;
                        }
                }
            }
            else if (reactionResult.Result.Id == ScissorsButton.CustomId)  ///////////////
            {
                switch (Winner("Paper", Player2))
                {
                    case "Unentschieden":
                        {
                            await msg.ModifyAsync(x =>
                            {
                                DiscordEmbedBuilder embed = new();
                                embed.WithTitle("Es ist Unentschieden");
                                embed.WithDescription($"{p1.Name}: {ScissorsEmoji} \n {ai.Name}: {ScissorsEmoji}");
                                embed.WithColor(DiscordColor.Lilac);
                                x.Embed = embed.Build();
                            });
                            break;
                        }

                    case "Gewonnen":
                        {
                            await msg.ModifyAsync(x =>
                            {
                                DiscordEmbedBuilder embed = new();
                                embed.WithTitle($"{p1.Name} hat gewonnen!");
                                embed.WithDescription($"{p1.Name}: {ScissorsEmoji} \n {ai.Name}: {PaperEmoji}");
                                embed.WithColor(DiscordColor.Lilac);
                                x.Embed = embed.Build();
                            });
                            break;
                        }

                    case "Verloren":
                        {
                            await msg.ModifyAsync(x =>
                            {
                                DiscordEmbedBuilder embed = new();
                                embed.WithTitle($"{p1.Name} hat verloren!");
                                embed.WithDescription($"{p1.Name}: {ScissorsEmoji} \n {ai.Name}: {RockEmoji}");
                                embed.WithColor(DiscordColor.Lilac);
                                x.Embed = embed.Build();
                            });
                            break;
                        }
                }
            }

        }

        private static string Winner(string Player1, string Player2)
        {
            string Unentschieden = "Unentschieden";
            string Gewonnen = "Gewonnen";
            string Verloren = "Verloren";

            switch (Player1)
            {
                case "Rock":
                    if (Player2 == "Rock")
                        return Unentschieden;
                    else if (Player2 == "Scissors")
                        return Gewonnen;
                    else if (Player2 == "Paper")
                        return Verloren;
                    break;
                case "Paper":
                    if (Player2 == "Paper")
                        return Unentschieden;
                    else if (Player2 == "Rock")
                        return Gewonnen;
                    else if (Player2 == "Scissors")
                        return Verloren;
                    break;
                case "Scissors":
                    if (Player2 == "Scissors")
                        return Unentschieden;
                    else if (Player2 == "Paper")
                        return Gewonnen;
                    else if (Player2 == "Rock")
                        return Verloren;
                    break;
            }
            return String.Empty;
        }
    }
}

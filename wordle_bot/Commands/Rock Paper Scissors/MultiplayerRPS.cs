using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace Minigame_bot.Commands.RockPaperScissors
{
    public class MultiplayerRPS
    {
        private Player p1;
        private Player p2;
        private DiscordUser user;
        private CommandContext ctx;

        public MultiplayerRPS(CommandContext context, DiscordMember playerTwo)
        {
            ctx = context;
            p1 = new Player(ctx.Member.Id, ctx.Member.DisplayName);
            p2 = new Player(playerTwo.Id, playerTwo.DisplayName);
            user = playerTwo;
        }

        public async Task StartMultiplayerGame()
        { 
            var RockEmoji = DiscordEmoji.FromName(ctx.Client, ":rock:");
            var PaperEmoji = DiscordEmoji.FromName(ctx.Client, ":page_facing_up:");
            var ScissorsEmoji = DiscordEmoji.FromName(ctx.Client, ":scissors:");
            var RE = new DiscordComponentEmoji(RockEmoji);
            var PE = new DiscordComponentEmoji(PaperEmoji);
            var SE = new DiscordComponentEmoji(ScissorsEmoji);

            var RockButton = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "Rock", null, false, RE);
            var PaperButton = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "Paper", null, false, PE);
            var ScissorsButton = new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "Scissors", null, false, SE);

            string Player2 = String.Empty;
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{p1.Name} fordert {p2.Name} zu Schere Stein Papier heraus",
                Color = DiscordColor.Lilac,
                Description = $"{ p1.Name }: \n { p2.Name}: \n\n\nAuf {p1.Name}'s und {p2.Name} Entscheidung warten",
            };

            var msg = await new DiscordMessageBuilder()     
                .WithEmbed(embed)
                .AddComponents(RockButton, PaperButton, ScissorsButton)
                .SendAsync(ctx.Channel);

            var interactivity = ctx.Client.GetInteractivity();

            var reactionResultP1 = await interactivity.WaitForButtonAsync(msg, ctx.User, null);
            var reactionResultP2 = await interactivity.WaitForButtonAsync(msg, user, null);
            
            if (reactionResultP2.Result.Id == RockButton.CustomId)
                Player2 = "Rock";
            else if (reactionResultP2.Result.Id == PaperButton.CustomId)
                Player2 = "Paper";
            else if (reactionResultP2.Result.Id == ScissorsButton.CustomId)
                Player2 = "Scissors";


            if (reactionResultP1.Result.Id == RockButton.CustomId)
            {
                if (Winner("Rock", Player2) == "Unentschieden")
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle("Es ist Unentschieden");
                        embed.WithDescription($"{p1.Name}: {RockEmoji} \n {p2.Name}: {RockEmoji}");
                        embed.WithColor(DiscordColor.Lilac);
                        x.Embed = embed.Build();
                    });
                }
                else if (Winner("Rock", Player2) == "Gewonnen")
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle($"{p1.Name} hat gewonnen!");
                        embed.WithDescription($"{p1.Name}: {RockEmoji} \n {p2.Name}: {ScissorsEmoji}");
                        embed.WithColor(DiscordColor.Lilac);
                        x.Embed = embed.Build();
                    });
                }
                else if (Winner("Rock", Player2) == "Verloren")
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle($"{p2.Name} hat gewonnen!");
                        embed.WithDescription($"{p1.Name}: {RockEmoji} \n {p2.Name}: {PaperEmoji}");
                        embed.WithColor(DiscordColor.Lilac);
                        x.Embed = embed.Build();
                    });
                }
            }
            else if (reactionResultP1.Result.Id == PaperButton.CustomId)  ////////////
            {
                if (Winner("Paper", Player2) == "Unentschieden")
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle("Es ist Unentschieden");
                        embed.WithDescription($"{p1.Name}: {PaperEmoji} \n {p2.Name}: {PaperEmoji}");
                        embed.WithColor(DiscordColor.Lilac);
                        x.Embed = embed.Build();
                    });
                }
                else if (Winner("Paper", Player2) == "Gewonnen")
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle($"{p1.Name} hat gewonnen!");
                        embed.WithDescription($"{p1.Name}: {PaperEmoji} \n {p2.Name}: {RockEmoji}");
                        embed.WithColor(DiscordColor.Lilac);
                        x.Embed = embed.Build();
                    });
                }
                else if (Winner("Paper", Player2) == "Verloren")
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle($"{p2.Name} hat gewonnen!");
                        embed.WithDescription($"{p1.Name}: {PaperEmoji} \n {p2.Name}: {ScissorsEmoji}");
                        embed.WithColor(DiscordColor.Lilac);
                        x.Embed = embed.Build();
                    });
                }
            }
            else if (reactionResultP1.Result.Id == ScissorsButton.CustomId)  ///////////////
            {
                if (Winner("Paper", Player2) == "Unentschieden")
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle("Es ist Unentschieden");
                        embed.WithDescription($"{p1.Name}: {ScissorsEmoji} \n {p2.Name}: {ScissorsEmoji}");
                        embed.WithColor(DiscordColor.Lilac);
                        x.Embed = embed.Build();
                    });
                }
                else if (Winner("Paper", Player2) == "Gewonnen")
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle($"{p1.Name} hat gewonnen!");
                        embed.WithDescription($"{p1.Name}: {ScissorsEmoji} \n {p2.Name}: {PaperEmoji}");
                        embed.WithColor(DiscordColor.Lilac);
                        x.Embed = embed.Build();
                    });
                }
                else if (Winner("Paper", Player2) == "Verloren")
                {
                    await msg.ModifyAsync(x =>
                    {
                        DiscordEmbedBuilder embed = new();
                        embed.WithTitle($"{p2.Name} hat gewonnen!");
                        embed.WithDescription($"{p1.Name}: {ScissorsEmoji} \n {p2.Name}: {RockEmoji}");
                        embed.WithColor(DiscordColor.Lilac);
                        x.Embed = embed.Build();
                    });
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
                    switch (Player2)
                    {
                        case "Rock":
                            return Unentschieden;
                        case "Scissors":
                            return Gewonnen;
                        case "Paper":
                            return Verloren;
                    }
                    break;
                case "Paper":
                    switch (Player2)
                    {
                        case "Paper":
                            return Unentschieden;
                        case "Rock":
                            return Gewonnen;
                        case "Scissors":
                            return Verloren;
                    }
                    break;
                case "Scissors":
                    switch (Player2)
                    {
                        case "Scissors":
                            return Unentschieden;
                        case "Paper":
                            return Gewonnen;
                        case "Rock":
                            return Verloren;
                    }
                    break;
            }
            return String.Empty;
        }
    }

}

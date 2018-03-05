using Discord.Commands;
using Discord.WebSocket;
using PhantomsForever_GRP.Core.Data;
using PhantomsForever_GRP.Core.Discord.ConditionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Discord.Commands
{
    [Group("hiring")]
    public class HiringModule : ModuleBase<SocketCommandContext>
    {
        [Command("apply")]
        public async Task ApplyAsync([Summary("The position the user is applying for")] string position)
        {
            var guild = PhantomsForeverBot.Instance._client.GetGuild(Settings.Guild);
            if(!guild.Users.Contains(Context.User))
            {
                await ReplyAsync("You are not in the server");
                var invites = await guild.GetInvitesAsync();
                var invite = invites.Where(x => x.ChannelName == "general").FirstOrDefault();
                await ReplyAsync(invite.Url);
                return;
            }
            var user = guild.GetUser(Context.User.Id);
            if(user.Roles.Where(x => x.Name == "Muted").Count() != 0)
            {
                await ReplyAsync("You are muted, please wait untill your mute has ended");
                return;
            }
            await user.AddRoleAsync(guild.Roles.Where(x => x.Name == "maybe-soon-teammember").FirstOrDefault());
            await guild.GetTextChannel(guild.Channels.Where(x => x.Name == "get-to-know-us").FirstOrDefault().Id).SendMessageAsync(user.Mention + " you have been summoned");
        }
        [RequireTeam]
        [Command("decide")]
        public async Task DecideAsync([Summary("The user on who the decision is made")] string user, string answer)
        {
            var guild = PhantomsForeverBot.Instance._client.GetGuild(Settings.Guild);
            if (!guild.Users.Contains(Context.User))
            {
                await ReplyAsync("You are not in the server");
                var invites = await guild.GetInvitesAsync();
                var invite = invites.Where(x => x.ChannelName == "general").FirstOrDefault();
                await ReplyAsync(invite.Url);
                return;
            }
            var u = guild.GetUser(Context.User.Id);
            if (u.Roles.Where(x => x.Name == "Muted").Count() != 0)
            {
                await ReplyAsync("You are muted, please wait untill your mute has ended");
                return;
            }
            await u.AddRoleAsync(guild.Roles.Where(x => x.Name == "maybe-soon-teammember").FirstOrDefault());
            await guild.GetTextChannel(guild.Channels.Where(x => x.Name == "get-to-know-us").FirstOrDefault().Id).SendMessageAsync(u.Mention + " you have been summoned");
        }
    }
}
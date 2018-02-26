using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = Discord;

namespace PhantomsForever_GRP.Core.Discord.Commands
{
    [Group("mod")]
    public class ModeratorCommands : ModuleBase<SocketCommandContext>
    {
        [Command("mute")]
        [Summary("Mutes an idiot")]
        public async Task MuteAsync([Summary("The user to mute")] SocketUser user)
        {
            var c = Context.Guild.GetUser(Context.User.Id);
            if (c.Roles.Where(x => x.Name == "Moderators" || x.Name == "Administrators").Count() != 1)
            {
                await ReplyAsync("I'm afraid I can't let you do that, " + Context.User.Mention);
                return;
            }
            var u = Context.Guild.GetUser(user.Id);
            var roles = u.Roles;
            await u.RemoveRolesAsync(roles.Where(x => x.IsEveryone != true));
            await u.AddRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == "Muted"));
            await ReplyAsync("Muted " + user.Username);
        }
    }
}
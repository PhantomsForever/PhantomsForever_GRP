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
    [RequireUserPermission(D.GuildPermission.DeafenMembers)]
    public class ModeratorCommands : ModuleBase<SocketCommandContext>
    {
        [Command("mute")]
        [Summary("Mutes an idiot")]
        public async Task MuteAsync([Summary("The user to mute")] SocketUser user)
        {
            var u = Context.Guild.GetUser(user.Id);
            var roles = u.Roles;
            await u.RemoveRolesAsync(roles.Where(x => x.IsEveryone != true));
            await u.AddRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == "Muted"));
            await ReplyAsync("Muted " + user.Username);
        }
    }
}
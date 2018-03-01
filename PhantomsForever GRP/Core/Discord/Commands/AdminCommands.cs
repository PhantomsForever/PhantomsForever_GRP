using Discord.Commands;
using Discord.WebSocket;
using PhantomsForever_GRP.Core.Discord.ConditionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Discord.Commands
{
    [Group("admin")]
    [RequireAdministrator]
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ban")]
        [Summary("Bans an idiot")]
        public async Task BanAsync([Summary("The user to ban")] SocketUser user)
        {
            await Context.Guild.AddBanAsync(user, 0, "Admin request");
            await ReplyAsync("Banned " + user.Username);
        }
    }
}
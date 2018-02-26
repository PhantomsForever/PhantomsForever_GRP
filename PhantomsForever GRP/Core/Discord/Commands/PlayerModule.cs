using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Discord.Commands
{
    [Group("player")]
    public class PlayerModule : ModuleBase<SocketCommandContext>
    {
        [Command("stats")]
        [Summary("View own stats")]
        public async Task ShowStatsAsync()
        {
            var c = Context.Guild.GetUser(Context.User.Id);
            if (c.Roles.Where(x => x.Name == "Player").Count() != 1)
            {
                await ReplyAsync("I'm afraid I can't let you do that, " + Context.User.Mention);
                return;
            }

            await ReplyAsync("Not yet fully implemented");
        }
        [Command("stats")]
        [Summary("Show a users stats")]
        public async Task ShowStatsAsync([Summary("The user to view stats from")] SocketUser user)
        {
            var c = Context.Guild.GetUser(Context.User.Id);
            if (c.Roles.Where(x => x.Name == "Player").Count() != 1)
            {
                await ReplyAsync("I'm afraid I can't let you do that, " + Context.User.Mention);
                return;
            }
            
            await ReplyAsync("Not yet fully implemented");
        }
    }
}
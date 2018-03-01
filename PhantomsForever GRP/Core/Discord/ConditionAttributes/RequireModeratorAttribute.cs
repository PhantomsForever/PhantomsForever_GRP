using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PhantomsForever_GRP.Core.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Discord.ConditionAttributes
{
    public class RequireModeratorAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var modId = services.GetService<DiscordSocketClient>().GetGuild(Settings.Guild).GetRole(Settings.ModeratorRole).Id;
            var user = services.GetService<DiscordSocketClient>().GetGuild(Settings.Guild).GetUser(context.User.Id);
            if (user.Roles.Where(x => x.Id == modId).Count() != 1)
                return PreconditionResult.FromError("You need to be in the moderators group");
            else
                return PreconditionResult.FromSuccess();
        }
    }
}
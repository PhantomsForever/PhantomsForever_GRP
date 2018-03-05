using Discord.Commands;
using Discord.WebSocket;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhantomsForever_GRP.Core.Data;

namespace PhantomsForever_GRP.Core.Discord.ConditionAttributes
{
    public class RequireTeamAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var user = services.GetService<DiscordSocketClient>().GetGuild(Settings.Guild).GetUser(context.User.Id);
            if (user.Roles.Where(x => x.Id == Settings.TeamRole).Count() != 1)
            {
                PhantomsForeverBot.Instance.Log(user.Username + " tried executing " + command.ToString());
                return PreconditionResult.FromError("You need to be a team member");
            }
            else
                return PreconditionResult.FromSuccess();
        }
    }
}
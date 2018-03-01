using Discord.Commands;
using Discord.WebSocket;
using PhantomsForever_GRP.Core.Data;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Discord.ConditionAttributes
{
    public class RequireAdministratorAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var user = services.GetService<DiscordSocketClient>().GetGuild(Settings.Guild).GetUser(context.User.Id);
            if (user.Roles.Where(x => x.Id == Settings.AdministratorRole).Count() != 1)
            {
                PhantomsForeverBot.Instance.Log(user.Username + " tried executing " + command.ToString());
                return PreconditionResult.FromError("You need to be in the administrators group");
            }
            else
                return PreconditionResult.FromSuccess();
        }
    }
}
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PhantomsForever_GRP.Core.Data;
using PhantomsForever_GRP.Core.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace PhantomsForever_GRP.Core.Discord
{
    public class PhantomsForeverBot
    {
        internal static PhantomsForeverBot Instance;
        internal DiscordSocketClient _client { get; private set; }
        private IServiceProvider _services;
        private CommandService _commands;
        private System.Timers.Timer _updateTimer;
        public PhantomsForeverBot()
        {
            if (!File.Exists(Path.Combine(Application.StartupPath, "discord.sqlite")))
                DiscordDatabaseHandler.Install();
            Instance = this;
            _updateTimer = new System.Timers.Timer(60000);
            _updateTimer.Elapsed += _updateTimer_Elapsed;
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();
            _client.Log += _client_Log;
            _client.MessageReceived += MessageReceived;
        }

        private void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _updateTimer.Stop();
            DiscordDatabaseHandler.CheckTimers();
            _updateTimer.Start();
        }

        public async Task Start()
        {
            _updateTimer.Start();
            Console.WriteLine("Registering Commands...");
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            Console.WriteLine("Done");
            if(string.IsNullOrEmpty(Settings.DiscordBotToken))
                Settings.DiscordBotToken = Microsoft.VisualBasic.Interaction.InputBox("Please enter a bot id for discord");
            await _client.LoginAsync(TokenType.Bot, Settings.DiscordBotToken);
            await _client.StartAsync();
            await Task.Delay(-1);
        }
        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg.Message);
            return Task.CompletedTask;
        }
        private async Task MessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
        internal void UnmuteUser(string id, string roles)
        {
            var user = _client.GetGuild(Settings.Guild).GetUser(Convert.ToUInt64(id));
            user.RemoveRoleAsync(user.Roles.Where(x => x.Name == "Muted").First());
            Console.WriteLine("Auto unmuting " + user.Username);
            Log("Auto unmuting " + user.Username);
            foreach(var role in roles.Split(';'))
            {
                if (string.IsNullOrEmpty(role))
                    continue;
                var guildrole = _client.GetGuild(Settings.Guild).Roles.Where(x => x.Name == role).First();
                user.AddRoleAsync(guildrole);
            }
        }
        internal void Log(string msg)
        {
            var channel = _client.GetGuild(Settings.Guild).GetTextChannel(Settings.LogChannel);
            channel.SendMessageAsync(DateTime.Now + ": " + msg);
        }
    }
}
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PhantomsForever_GRP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Discord
{
    public class PhantomsForeverBot
    {
        private DiscordSocketClient _client;
        private IServiceProvider _services;
        private CommandService _commands;
        public PhantomsForeverBot()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();
            _client.Log += _client_Log;
            _client.MessageReceived += MessageReceived;
        }

        public async Task Start()
        {
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
    }
}
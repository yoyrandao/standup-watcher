using System;
using System.Linq;

using Telegram.Bot;


namespace UsernameFiller
{
	class Program
	{
		static void Main(string[] args)
		{
			if (!args.Any())
			{
				Console.WriteLine("No parameters provided. Usage: UsernameFiller.exe <TELEGRAM_BOT_TOKEN> \"<COMMA_SPLITTED_CHAT_IDS>\"");
			}

			var token = args[0];
			var chatIds = args[1].Split(",").Select(x => long.Parse(x));

			var bot = new TelegramBotClient(token);

			foreach (var chatId in chatIds)
			{
				var chat = bot.GetChatAsync(chatId).GetAwaiter().GetResult();
				Console.WriteLine($@"{chat.Id} - {chat.Username}");
			}
		}
	}
}
using System;
using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace StandupWatcher.Processing.Notifying
{
	public interface IBotFacade
	{
		public Task HandleMessages(ITelegramBotClient client, Update update, CancellationToken cancellationToken);

		public Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken);

		public void SendMessage(long chatId, string message);

		public void SendMessageWithPhoto(long chatId, string imageUrl, string message);
	}
}
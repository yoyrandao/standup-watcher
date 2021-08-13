using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using StandupWatcher.Common.Types;
using StandupWatcher.DataAccess.Models;
using StandupWatcher.DataAccess.Repositories;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StandupWatcher.Processing.Notifying
{
	public class BotFacade : IBotFacade
	{
		public BotFacade(ITelegramBotClient client, IGenericRepository<Subscriber> subscribersRepository)
		{
			_client = client;
			_subscribersRepository = subscribersRepository;
		}

		#region Implementation of IBotFacade

		public async Task HandleMessages(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
		{
			var handler = update.Type switch
			{
				UpdateType.Message => OnMessageRecieved(client, update.Message),

				_ => OnUnknownUpdateRecieved()
			};

			try
			{
				await handler;
			}
			catch (Exception e)
			{
				await HandleError(client, e, cancellationToken);
			}
		}

		public Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
		{
			var errorMessage = exception switch
			{
				ApiRequestException e => $"Telegram API error [{e.ErrorCode}]",

				_ => exception.ToString()
			};

			// logging
			return Task.CompletedTask;
		}

		public void SendMessage(long chatId, string message)
		{
			_client.SendTextMessageAsync(chatId, message, ParseMode.Html).GetAwaiter().GetResult();
		}

		public void SendMessageWithPhoto(long chatId, string imageUrl, string message)
		{
			_client.SendPhotoAsync(chatId, imageUrl, message, ParseMode.Html).GetAwaiter().GetResult();
		}

		#endregion

		private async Task OnMessageRecieved(ITelegramBotClient client, Message message)
		{
			var action = message.Text.Split(' ').First() switch
			{
				"/start" => () =>
				{
					RegisterSubscriber(message.Chat.Id);
					SendMessage(message.Chat.Id, Messages.SuccessfulOperationMessage);
				},
				"/stop"  => () =>
				{
					DeleteSubscriber(message.Chat.Id);
					SendMessage(message.Chat.Id, Messages.SuccessfulOperationMessage);
				},

				_ => (Action) (() => ReplyUnknownAction(message.Chat.Id))
			};

			try
			{
				action();
			}
			catch (Exception e)
			{
				await HandleError(client, e, CancellationToken.None);
			}
		}

		private void ReplyUnknownAction(long chatId)
		{
			_client.SendTextMessageAsync(chatId, Messages.UnknownMessage).GetAwaiter().GetResult();
		}

		private void DeleteSubscriber(long chatId)
		{
			var existingSubscriber = _subscribersRepository.Get(x => x.ChatId.Equals(chatId)).Single();

			_subscribersRepository.Delete(existingSubscriber);
			_subscribersRepository.Save();
		}

		private void RegisterSubscriber(long chatId)
		{
			var now = DateTime.Now;

			_subscribersRepository.Add(new Subscriber
			{
				ChatId = chatId,
				CreationTimestamp = now,
				ModificationTimestamp = now
			});

			_subscribersRepository.Save();
		}

		private Task OnUnknownUpdateRecieved()
		{
			// logging
			return Task.CompletedTask;
		}

		private readonly ITelegramBotClient _client;
		private readonly IGenericRepository<Subscriber> _subscribersRepository;
	}
}
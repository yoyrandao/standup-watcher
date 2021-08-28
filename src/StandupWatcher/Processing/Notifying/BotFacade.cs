using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

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
		public BotFacade(ITelegramBotClient client, IGenericRepository<Subscriber> subscribersRepository, ILogger<BotFacade> logger, IGenericRepository<SubscribedAuthors> subscribedAuthorsRepository)
		{
			_client = client;
			_logger = logger;
			_subscribersRepository = subscribersRepository;
			_subscribedAuthorsRepository = subscribedAuthorsRepository;
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
				ApiRequestException e => $"Recieved error with code {e.ErrorCode} from Telegram API.",

				_ => exception.ToString()
			};

			_logger.LogError(errorMessage);

			return Task.CompletedTask;
		}

		public void SendMessage(long chatId, string message)
		{
			try
			{
				_client.SendTextMessageAsync(chatId, message, ParseMode.Html).GetAwaiter().GetResult();
			}
			catch (Exception e)
			{
				HandleError(_client, e, CancellationToken.None).GetAwaiter().GetResult();
			}
		}

		public void SendMessageWithPhoto(long chatId, string imageUrl, string message)
		{
			try
			{
				_client.SendPhotoAsync(chatId, imageUrl, message, ParseMode.Html).GetAwaiter().GetResult();
			}
			catch (Exception e)
			{
				HandleError(_client, e, CancellationToken.None).GetAwaiter().GetResult();
			}
		}

		#endregion

		private async Task OnMessageRecieved(ITelegramBotClient client, Message message)
		{
			var action = message.Text.Split(' ').First() switch
			{
				"/start" => () =>
				{
					RegisterSubscriber(message.Chat.Id, message.Chat.Username);
					SendMessage(message.Chat.Id, Messages.SuccessfulOperationMessage);
				}
				,
				"/stop" => () =>
				{
					DeleteSubscriber(message.Chat.Id);
					SendMessage(message.Chat.Id, Messages.SuccessfulOperationMessage);
				}
				,
				"/status" => () =>
				{
					var isSubscribed = IsSubscribed(message.Chat.Id);

					SendMessage(message.Chat.Id, isSubscribed ? Messages.SubscribedStatusMessage : Messages.NotSubscribedStatusMessage);
				}
				,
				"/add" => (Action)(() =>
				{
					var isSubscribed = IsSubscribed(message.Chat.Id);

					if (isSubscribed)
					{
						var requestedAuthor = message.Text.Split("/add").ToList().FirstOrDefault().Trim();
						if (requestedAuthor is not null)
						{
							_subscribedAuthorsRepository.Add(newAuthor(message.Chat.Id, requestedAuthor));
							_subscribedAuthorsRepository.Save();

							SendMessage(message.Chat.Id, $"{requestedAuthor} {Messages.AddedToFavoriteMessage}");
						}
						else
						{
							SendMessage(message.Chat.Id, $"{requestedAuthor} {Messages.EmptyAddedToFavoriteMessage}");
						}
					}
				})
				,
				"/remove" => (Action)(() =>
				{
					var isSubscribed = IsSubscribed(message.Chat.Id);

					if (isSubscribed)
					{
						var requestedAuthor = message.Text.Split("/remove").ToList().FirstOrDefault().Trim();

						if (requestedAuthor is not null)
						{
							var searchedAuthors = _subscribedAuthorsRepository.Get(x => x.ChatId.Equals(message.Chat.Id) && x.StanduperName.Equals(requestedAuthor));

							if (searchedAuthors.Any())
							{
								var subscribed = searchedAuthors.First();
								_subscribedAuthorsRepository.Delete(subscribed);
								_subscribedAuthorsRepository.Save();
								SendMessage(message.Chat.Id, $"{requestedAuthor} {Messages.RemovedFromFavoriteMessage}");
							}
							else
							{
								SendMessage(message.Chat.Id, $"{requestedAuthor} {Messages.NotFoundInFavoriteMessage}");
							}
						}
						else
						{
							SendMessage(message.Chat.Id, Messages.EmptyAddedToFavoriteMessage);
						}
					}
				})
				,
				"/list" => (Action)(() =>
				{
					var isSubscribed = IsSubscribed(message.Chat.Id);

					if (isSubscribed)
					{
						var searchedAuthors = _subscribedAuthorsRepository.Get(x => x.ChatId.Equals(message.Chat.Id));

						if (searchedAuthors.Any())
						{
							var subscribedAuthors = searchedAuthors.Select(x => x.StanduperName);
							var joinedSubscribedAuthors = string.Join(", ", subscribedAuthors);

							SendMessage(message.Chat.Id, $"{Messages.FollowingStandupersMessage} : {joinedSubscribedAuthors}");
						}
						else
						{
							SendMessage(message.Chat.Id, $"{Messages.EmptyFollowingStandupersMessage}");
						}
					}
				})
				,
				_ => (Action)(() => ReplyUnknownAction(message.Chat.Id))
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

		private SubscribedAuthors newAuthor(long chatId, string standuperName)
		{
			var author = new SubscribedAuthors()
			{
				CreationTimestamp = DateTime.Now,
				ModificationTimestamp = DateTime.Now,
				ChatId = chatId,
				StanduperName = standuperName
			};

			return author;
		}
		private bool IsSubscribed(long chatId)
		{
			return _subscribersRepository.Get(x => x.ChatId.Equals(chatId)).Any();
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

		private void RegisterSubscriber(long chatId, string username)
		{
			if (IsSubscribed(chatId))
				return;

			var now = DateTime.Now;

			_subscribersRepository.Add(new Subscriber
			{
				ChatId = chatId,
				Username = username,
				CreationTimestamp = now,
				ModificationTimestamp = now
			});

			_subscribersRepository.Save();
		}

		private Task OnUnknownUpdateRecieved()
		{
			_logger.LogWarning("Unknown update recieved.");

			return Task.CompletedTask;
		}

		private readonly ITelegramBotClient _client;
		private readonly IGenericRepository<Subscriber> _subscribersRepository;
		private readonly ILogger<BotFacade> _logger;
		private readonly IGenericRepository<SubscribedAuthors> _subscribedAuthorsRepository;
	}
}
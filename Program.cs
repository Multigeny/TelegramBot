using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace TelegramBot
{ 

    class Program
    {
        static async Task Main()
        {
            var botClient = new TelegramBotClient("7620060922:AAGdQfv02CCDfIsZYObn9Xi4E7uozi59JlM"); // ← замени на свой токен

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // Получать все типы обновлений
            };

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandlePollingErrorAsync,
                receiverOptions,
                cts.Token
            );

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Бот @{me.Username} запущен");
            Console.ReadLine();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { Text: { } messageText }) return;

            var chatId = update.Message.Chat.Id;
            var text = messageText.ToLower();

            string response = text switch
            {
                var t when t.Contains("привет") => "Здорова! Как ты?",
                var t when t.Contains("как дела") => "Отлично! А у тебя как?",
                var t when t.Contains("пока") => "Увидимся!",
                var t when t.Contains("что делаешь") => "Болтаю с тобой :)",
                var t when t.Contains("ты кто") => "Я простой Telegram-бот на C#!",
                _ => "Команда не распознана"
            };

            await botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
        }

        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }


}

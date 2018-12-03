using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using CarPupsTelegramBot.Models.ReturnModels.MessageReturnModels;
using CarPupsTelegramBot.Utilities;

namespace CarPupsTelegramBot.Api
{
    class MessageApi
    {
        public static async void SendTextMessage(string message, ITelegramBotClient botClient, MessageEventArgs telegramMessageEvent)
        {
            try {
                await botClient.SendTextMessageAsync(
                    chatId: telegramMessageEvent.Message.Chat,
                    text: message,
                    parseMode: ParseMode.Html,
                    disableWebPagePreview: true
                );
            } catch (Exception e) {
                ConsoleOutputUtilities.ErrorConsoleMessage(e.ToString());
            }

            ConsoleOutputUtilities.MessageOutConsoleMessage(message, telegramMessageEvent);
        }

        public static async void SendTextMessage(TextMessageReturnModel messageReturnModel, ITelegramBotClient botClient, MessageEventArgs telegramMessageEvent)
        {
            try {
                await botClient.SendTextMessageAsync(
                    text: messageReturnModel.Text,
                    chatId: telegramMessageEvent.Message.Chat,
                    parseMode: ParseMode.Html,
                    disableWebPagePreview: true
                );
            } catch (Exception e) {
                ConsoleOutputUtilities.ErrorConsoleMessage(e.ToString());
            }
        }

        public static async void SendPhotoMessage(ImageMessageReturnModel messageReturnModel, ITelegramBotClient botClient, MessageEventArgs telegramMessageEvent) {
            try {
                await botClient.SendPhotoAsync(
                    chatId: telegramMessageEvent.Message.Chat,
                    parseMode: ParseMode.Html,
                    caption: messageReturnModel.Caption,
                    photo: messageReturnModel.PhotoUrl
                );
            } catch (Exception e) {
                ConsoleOutputUtilities.ErrorConsoleMessage(e.ToString());
            }
        }
    }
}

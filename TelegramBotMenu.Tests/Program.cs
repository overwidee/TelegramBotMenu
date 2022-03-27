using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotMenu.Extensions;
using TelegramBotMenu.Models;

List<MenuButtonModel> buttonModels = new()
{
    new MenuButtonModel("Button1", "Button1Value"),
    new MenuButtonModel("Button2", "Button2Value"),
    new MenuButtonModel("Button3", "Button3Value"),
    new MenuButtonModel("Button4", "Button3Value"),
    new MenuButtonModel("Button5", "Button3Value"),
    new MenuButtonModel("Button6", "Button3Value"),
    new MenuButtonModel("Button7", "Button3Value"),
    new MenuButtonModel("Button8", "Button3Value"),
    new MenuButtonModel("Button9", "Button3Value"),
};

async Task Test1(ITelegramBotClient client, long chatId)
{
    var replyMenu = client.GetPaginationInlineKeyboard(buttonModels, 2, 2);
    await client.SendTextMessageAsync(chatId, "Test1", replyMarkup: replyMenu);
}

async Task Test2(ITelegramBotClient client, long chatId, int messageId, string? clickedNavigation)
{
    var replyMenu = client.GetPaginationInlineKeyboard(buttonModels, 2, 2, clickedNavigation);
    await client.EditMessageReplyMarkupAsync(chatId, messageId, replyMenu);
}

#region Initialize bot

const string token = "5134852992:AAH0qis0h9FYgKfy7GfuiYGQgEswfO-wMPs";
var botClient = new TelegramBotClient(token);
using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions();
botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
{
    var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;

    if (chatId != null)
        switch (update.Type)
        {
            case UpdateType.Message:
                await Test1(client, chatId.Value);
                break;
            case UpdateType.CallbackQuery:
                await Test2(client, chatId.Value, update.CallbackQuery!.Message!.MessageId,
                    update.CallbackQuery?.Data ?? string.Empty);
                break;
        }
}

Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
{
    var errorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(errorMessage);
    return Task.CompletedTask;
}

#endregion

Console.ReadKey();
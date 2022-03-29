using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotMenu.Extensions;
using TelegramBotMenu.Models;

List<MenuButtonModel> buttonModels = new()
{
    new MenuButtonModel("Button1", "Button1Value", new[] { new MenuButtonModel("InsideButton1", "InsideValue1") }),
    new MenuButtonModel("Button2", "Button2Value"),
    new MenuButtonModel("Button3", "Button3Value"),
    new MenuButtonModel("Button4", "Button4Value"),
    new MenuButtonModel("Button5", "Button5Value", 
        new[]
        {
            new MenuButtonModel("InsideButton5", "InsideValue5",
                new[] { new MenuButtonModel("InsideInsideButton5", "InsideValue55") }),
            new MenuButtonModel("InsideButton5_1", "InsideButton5_1"),
            new MenuButtonModel("InsideButton5_2", "InsideButton5_2"),
            new MenuButtonModel("InsideButton5_3", "InsideButton5_3"),
            new MenuButtonModel("InsideButton5_4", "InsideButton5_4"),
            new MenuButtonModel("InsideButton5_5", "InsideButton5_5"),
        }),
    new MenuButtonModel("Button6", "Button6Value"),
    new MenuButtonModel("Button7", "Button7Value"),
    new MenuButtonModel("Button8", "Button8Value"),
    new MenuButtonModel("Button9", "Button9Value"),
};

async Task Test1(ITelegramBotClient client, long chatId)
{
    var replyMenu = client.GetPaginationInlineKeyboard(buttonModels, 2, 2);
    if (replyMenu != null)
        await client.SendTextMessageAsync(chatId, "Test1", replyMarkup: replyMenu);
}

async Task Test2(ITelegramBotClient client, long chatId, int messageId, string? clickedNavigation)
{
    var replyMenu = client.GetPaginationInlineKeyboard(buttonModels, 2, 2, clickedNavigation);
    if (replyMenu != null)
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
    try
    {
        var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;

        if (chatId != null)
            switch (update.Type)
            {
                case UpdateType.Message:
                    await Test1(client, chatId.Value);
                    break;
                case UpdateType.CallbackQuery:
                    await client.AnswerCallbackQueryAsync(update.CallbackQuery!.Id, cancellationToken: cancellationToken);
                    await Test2(client, chatId.Value, update.CallbackQuery!.Message!.MessageId,
                        update.CallbackQuery?.Data ?? string.Empty);
                    break;
            }
    }
    catch (Exception)
    {
        //ignored
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
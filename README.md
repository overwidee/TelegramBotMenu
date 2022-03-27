## How to use Menu-Extension

### Pagination

* Initialize your list of objects

```C#
List<MenuButtonModel> buttonModels = new()
{
    new MenuButtonModel("Button1", "Button1Value"),
    new MenuButtonModel("Button2", "Button2Value"),
    new MenuButtonModel("Button3", "Button3Value"),
    new MenuButtonModel("Button4", "Button4Value"),
    new MenuButtonModel("Button5", "Button5Value"),
    new MenuButtonModel("Button6", "Button6Value"),
    new MenuButtonModel("Button7", "Button7Value"),
    new MenuButtonModel("Button8", "Button8Value"),
    new MenuButtonModel("Button9", "Button9Value"),
};
```
* Initialize and start Telegram Bot
```C#
var botClient = new TelegramBotClient(token);
...
var me = await botClient.GetMeAsync();
```
* Use `GetPaginationInlineKeyboard` method for get InlineKeyboardMarkup and send telegram message

```C#
const int columnCount = 2;
const int rowCount = 3;
var replyMenu = botClient.GetPaginationInlineKeyboard(buttonModels, columnCount, rowCount);
await botClient.SendTextMessageAsync(chatId, "YourMessage", replyMarkup: replyMenu);
```

### Click button `NEXT/PREV`

* You have to send `clickedNavigation`
```C#
// clickedNavigation - Data from CallbackQuery
var replyMenu = client.GetPaginationInlineKeyboard(buttonModels, columnCount, rowCount, clickedNavigation);
await botClient.EditMessageReplyMarkupAsync(chatId, messageId, replyMenu);
```

### Example

![image](https://user-images.githubusercontent.com/36662441/160297013-e9d6a0e8-b169-456f-8afe-99b97be2580e.png)
![image](https://user-images.githubusercontent.com/36662441/160297046-aac479f9-45a1-419e-a569-0455b12c3454.png)



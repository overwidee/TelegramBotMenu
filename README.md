# How to use Menu-Extension

First of all, you need to initialize the object of TelegramBotClient and run.
```C#
var botClient = new TelegramBotClient(token);
...
var me = await botClient.GetMeAsync();
```

## ➡Pagination

Initialize your list of objects

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

Use `GetPaginationInlineKeyboard` method for get InlineKeyboardMarkup and send telegram message

```C#
const int columnCount = 2;
const int rowCount = 3;
var replyMenu = botClient.GetPaginationInlineKeyboard(buttonModels, columnCount, rowCount);
await botClient.SendTextMessageAsync(chatId, "YourMessage", replyMarkup: replyMenu);
```

## 👆Click button `NEXT/PREV`

You have to send `clickedNavigation`
```C#
// clickedNavigation - Data from CallbackQuery
var replyMenu = client.GetPaginationInlineKeyboard(buttonModels, columnCount, rowCount, clickedNavigation);
await botClient.EditMessageReplyMarkupAsync(chatId, messageId, replyMenu);
```

## 🌳Tree architecture (since version 0.0.2)

In addition to navigation, you can send child elements to the constructor for a tree-like architecture.
```C#
var childrenItems = new List<MenuButtonModel>() { new MenuButtonModel("ButtonInside", "InsideData") };
var item = new MenuButtonModel("Button1", "Button1Value", childrenItems);
```

## 🔗Static settings
You can change the text of the navigation buttons.
```C#
// Default values
public static class Settings
{
    public static string ButtonNextText = "»";
    public static string ButtonPrevText = "«";
    public static string ButtonUpText = "« up";
    public const string PaginationData = "toPage";
}
...
// Custom values
Settings.ButtonNextText = "👉";
Settings.ButtonPrevText = "👈";
```

## 👀Example of pagination
![image](https://user-images.githubusercontent.com/36662441/160777044-90bfedee-9a00-4e88-ada2-a49f2b240e45.png)

-> Click Button5 -> 

![image](https://user-images.githubusercontent.com/36662441/160777220-8e10e0bf-ffe2-48bb-a9c0-337787c8a0f2.png)

For a more detailed analysis, see the ```TelegramBotMenu.Tests```

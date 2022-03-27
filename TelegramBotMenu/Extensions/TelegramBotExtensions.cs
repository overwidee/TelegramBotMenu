using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotMenu.Models;

namespace TelegramBotMenu.Extensions;

public static class TelegramBotExtensions
{
    public static InlineKeyboardMarkup GetPaginationInlineKeyboard(this ITelegramBotClient sourceClient,
        IEnumerable<MenuButtonModel> data, int columns, int rows, string? clickedNavigationData = null)
    {
        // init page number in list
        var pagingList = data.Chunk(columns * rows).SelectMany((row, page) =>
            row.Select(newItem => new MenuButtonNumberModel(newItem.Text, page, newItem.Value))).ToList();

        // max count page
        var pageCount = pagingList.Last().NumberPage;

        var currentPageIndex = clickedNavigationData != null
            ? int.Parse(clickedNavigationData.Replace(Settings.PaginationData, string.Empty))
            : 0;

        var showList = pagingList.Where(x => x.NumberPage == currentPageIndex).ToList();

        // add prev button
        if (currentPageIndex > 0)
            showList.Add(new MenuButtonNumberModel(Settings.ButtonPrevText, currentPageIndex,
                $"{Settings.PaginationData}{currentPageIndex - 1}"));

        // add next button
        if (currentPageIndex < pageCount)
            showList.Add(new MenuButtonNumberModel(Settings.ButtonNextText, currentPageIndex,
                $"{Settings.PaginationData}{currentPageIndex + 1}"));

        return new InlineKeyboardMarkup(showList.Chunk(columns).Select(row => row.Select(button =>
            new InlineKeyboardButton(button.Text)
            {
                CallbackData = button.Value
            })));
    }
}
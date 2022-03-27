namespace TelegramBotMenu.Models;

public class MenuButtonModel
{
    public MenuButtonModel(string text, string? value = null)
    {
        Text = text;
        Value = value;
    }

    public string Text { get; }
    public string? Value { get; }
}

internal class MenuButtonNumberModel : MenuButtonModel
{
    public MenuButtonNumberModel(string text, int numberPage, string? value = null) : base(text, value)
    {
        NumberPage = numberPage;
    }
    
    public int NumberPage { get; }
}
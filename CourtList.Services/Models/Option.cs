namespace CourtList.Services.Models;

internal class Option
{
    public Option(string value, string text)
    {
        Value = value;
        Text = text;
    }

    public string Value { get; }
    public string Text { get; }
}
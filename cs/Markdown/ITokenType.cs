namespace Markdown
{
    public interface ITokenType
    {
        string ToHtml(string text, bool opened, bool closed);
        bool CheckIfOpen(char symbol, char left, string right);
        bool CheckIfClosing(char symbol, char left, string right);
        ITokenType[] SupportedInnerTypes();
        string GetMarker();
    }
}
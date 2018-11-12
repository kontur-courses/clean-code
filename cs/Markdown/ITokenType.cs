using System;

namespace Markdown
{
    public interface ITokenType
    {
        string ToHtml(string text);
        bool CheckIfOpen(char symbol, char left, char right);
        bool CheckIfClosing(char symbol, char left, char right);
        Type[] SupportedInnerTypes();
        string GetMarker();
    }
}
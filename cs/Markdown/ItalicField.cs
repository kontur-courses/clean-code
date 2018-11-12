using System;

namespace Markdown
{
    public class ItalicField : ITokenType
    {
        public readonly string Marker = "_";

        public string ToHtml(string text)
        {
            return $"<em>{text}</em>";
        }

        public bool CheckIfOpen(char symbol, char left, char right)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfClosing(char symbol, char left, char right)
        {
            throw new NotImplementedException();
        }

        public Type[] SupportedInnerTypes() => new Type[0];

        public string GetMarker()
        {
            return Marker;
        }
    }
}
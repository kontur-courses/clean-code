using System;

namespace Markdown
{
    public class StrongField : ITokenType
    {
        public readonly string Marker = "__";

        public string ToHtml(string text) => $"<strong>{text}</strong>";

        public bool CheckIfOpen(char symbol, char left, char right)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckIfClosing(char symbol, char left, char right)
        {
            throw new System.NotImplementedException();
        }

        public Type[] SupportedInnerTypes() => new[] { typeof(ItalicField) };

        public string GetMarker() => Marker;
    }
}
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
            return SpecChecks(symbol, left, right) && !char.IsWhiteSpace(right);
        }
        
        public bool CheckIfClosing(char symbol, char left, char right)
        {
            return SpecChecks(symbol, left, right) && !char.IsWhiteSpace(left);
        }

        private bool SpecChecks(char symbol, char left, char right)
        {
            if (symbol != Marker[0])
                return false;
            if (left == '\\')
                return false;
            if (char.IsDigit(left) || char.IsDigit(right))
                return false;
            return right != '_';
        }

        public Type[] SupportedInnerTypes() => new Type[0];

        public string GetMarker()
        {
            return Marker;
        }
    }
}
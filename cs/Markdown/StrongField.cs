namespace Markdown
{
    public class StrongField : ITokenType
    {
        public readonly string Marker = "__";

        public string ToHtml(string text, bool opened, bool closed)
        {
            var html = text;
            if (opened)
                html = $"<strong>{html}";
            if (closed)
                html = $"{html}</strong>";
            return html;
        }

        public bool CheckIfOpen(char symbol, char left, string right)
        {
            return SpecificationCheck(symbol, left, right) && !char.IsWhiteSpace(right[1]);
        }

        public bool CheckIfClosing(char symbol, char left, string right)
        {
            return SpecificationCheck(symbol, left, right) && !char.IsWhiteSpace(left);
        }

        private bool SpecificationCheck(char symbol, char left, string right)
        {
            if (symbol != Marker[0])
                return false;
            if (left == '\\')
                return false;
            if (char.IsDigit(left) || char.IsDigit(right[1]))
                return false;
            return right[0] == '_';
        }

        public ITokenType[] SupportedInnerTypes() => new ITokenType[] { new ItalicField() };

        public string GetMarker() => Marker;
    }
}
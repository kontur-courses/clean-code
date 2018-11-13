namespace Markdown
{
    public class UnderscoreRule : ILexerRule
    {
        public Delimiter ProcessIncomingChar(int position, Delimiter previousDelimiter, out bool shouldRemovePrevious)
        {
            shouldRemovePrevious = false;
            if (previousDelimiter != null && previousDelimiter.Position + 1 == position && previousDelimiter.Value == "_")
            {
                shouldRemovePrevious = true;
                return new Delimiter("__", position - 1);
            }
            return new Delimiter("_", position);
        }

        public  string[] PossibleDelimitersStrings => new[] {"_", "__"};

        public bool Check(char symbol) => symbol == '_';
        public bool Check(Delimiter delimiter) => delimiter.Value == "_" || delimiter.Value == "__";
        public Delimiter Escape(Delimiter delimiter, string text)
        {
            if (delimiter.Position == 0 || text[delimiter.Position - 1] != '\\')
                return delimiter;
            if (delimiter.Value == "__")
                return new Delimiter("_", delimiter.Position + 1);

            if (delimiter.Value == "_")
                return null;
            return null;
        }

        public bool IsValid(Delimiter delimiter, string text)
        {
            if (delimiter.Position == 0 || (delimiter.Value == "_" && delimiter.Position == text.Length - 1) || (delimiter.Value == "__" && delimiter.Position == text.Length - 2))
                return true;
            var previousSymbol = text[delimiter.Position - 1];

            char nextSymbol;
            switch (delimiter.Value)
            {
                case "__":
                    nextSymbol = text[delimiter.Position + 2];
                    break;
                case "_":
                    nextSymbol = text[delimiter.Position + 1];
                    break;
                default:
                    nextSymbol = '-';
                    break;
            }
            return !((char.IsLetterOrDigit(previousSymbol)||previousSymbol == '_') && (char.IsLetterOrDigit(nextSymbol) || nextSymbol == '_'));

        }

        public Token GetToken(Delimiter delimiter, string text)
        {
            if (delimiter.IsFirst)
            {
                var second = delimiter.Partner;
                if (delimiter.Value == "_")
                {
                    var length = second.Position - delimiter.Position + 1;
                    return new UnderscoreToken(delimiter.Position, length, text.Substring(delimiter.Position, length));
                }

                if (delimiter.Value == "__")
                {
                    var length = second.Position - delimiter.Position + 2;
                    return new UnderscoreToken(delimiter.Position, length, text.Substring(delimiter.Position, length));

                }
            }

            if (delimiter.IsLast)
            {
                return null;
            }

            return null;
        }
    }
}
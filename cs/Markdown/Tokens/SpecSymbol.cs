namespace Markdown.Tokens
{
    internal abstract class SpecSymbol : Token
    {
        public SpecSymbol(char symbol)
        {
            Symbol = symbol;
        }

        public char Symbol { get; private set; }
        
        public static bool IsSpecSymbol(char ch, out SpecSymbol specSymbol)
        {
            if (ch == '\\')
            {
                specSymbol = new Backslash();
                return true;
            }

            if (ch == ' ')
            {
                specSymbol = new Space();
                return true;
            }

            if (ch == '_')
            {
                specSymbol = new Underline();
                return true;
            }

            specSymbol = default;
            return false;
        }
    }
}

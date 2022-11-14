namespace Markdown;

public class Symbols
    {
        public static readonly string ItalicSymbol = "_";
        public static readonly string EscapeSymbol = @"\";
        public static readonly string BoldSymbol = "__";
        public static readonly string HeaderSymbol = "#";

        public static bool IsMarkdownSymbol(string symbol)
        {
            return symbol == ItalicSymbol || symbol == BoldSymbol || symbol == HeaderSymbol || symbol == EscapeSymbol;
        }

        public static bool IsEscapeSymbol(string symbol)
        {
            return symbol == EscapeSymbol;
        }
    }

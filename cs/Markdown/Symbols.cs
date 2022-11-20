namespace Markdown;

public class Symbols
{
        private const string EscapeSymbol = @"\";
        public static readonly Symbol EmptySymbol = new Symbol("", "", "");
        public static readonly Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>()
        {
            {"", new Symbol("", "", "")},
            {"_", new Symbol("_", "<em>", "</em>")},
            {"__", new Symbol("__", "<strong>", "</strong>")},
            {"#", new Symbol("#", "<h1>", "</h1>\n")},
        };
        
        public static Symbol? IsMarkdownSymbol(string text, int indexOfSymbol)
        {
            Symbol symbol = null;
            var possibleMarkdownSymbols = symbols.Keys.ToHashSet();
            
            for (int i = indexOfSymbol; i < text.Length; i++)
            {
                var segmentToCheck = text.Substring(indexOfSymbol, i - indexOfSymbol + 1);
                possibleMarkdownSymbols.RemoveWhere(markdownSymbol => markdownSymbol.StartsWith(segmentToCheck) == false);
                if (possibleMarkdownSymbols.Count == 0)
                    return symbol;
                symbol = symbols[segmentToCheck];
            }

            return symbol;
        }

        public static string? IsEscapeSymbol(string text, int indexOfSymbol)
        {
            for (var i = indexOfSymbol; i < text.Length; i++)
            {
                var segmentToCheck = text.Substring(indexOfSymbol, i - indexOfSymbol + 1);
                if (EscapeSymbol.StartsWith(segmentToCheck) == false)
                    return null;
                if (segmentToCheck is EscapeSymbol)
                    return EscapeSymbol;
            }

            return null;
        }
    }

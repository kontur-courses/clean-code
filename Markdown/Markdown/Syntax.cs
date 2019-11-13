using System.Collections.Generic;

namespace Markdown
{
    public class Syntax
    {
        public readonly Dictionary<char, AttributeType> TypeDictionary;

        private static readonly HashSet<char> specialSymbols = new HashSet<char>
        {
            '\\', '`', '*', '_', '{', '}', '[', ']', '(', ')', '#', '+', '-', '.', '!', '?', '|', '$', '^', '/', '>',
            '<', '&'
        };

        public Syntax(Dictionary<char, AttributeType> typeDictionary)
        {
            TypeDictionary = typeDictionary;
        }

        public static bool CharCanBeEscaped(char ch)
        {
            return specialSymbols.Contains(ch);
        }

        public static Syntax InitializeDefaultSyntax()
        {
            var typeDictionary = new Dictionary<char, AttributeType>
            {
                {'_', AttributeType.Emphasis},
                {'\\', AttributeType.Escape}
            };
            return new Syntax(typeDictionary);
        }

        public static bool IsValidEmphasisDelimiter(int charPosition, string source)
        {
            return IsOpeningDelimiter(charPosition, source) || IsClosingDelimiter(charPosition, source);
        }

        public static bool IsOpeningDelimiter(int charPosition, string source)
        {
            return (charPosition == 0 || char.IsWhiteSpace(source[charPosition - 1]))
                   && charPosition != source.Length - 1 && !char.IsWhiteSpace(source[charPosition + 1]);
        }

        public static bool IsClosingDelimiter(int charPosition, string source)
        {
            return (charPosition == source.Length - 1 || char.IsWhiteSpace(source[charPosition + 1]))
                   && charPosition != 0 && !char.IsWhiteSpace(source[charPosition - 1]);
        }
    }
}
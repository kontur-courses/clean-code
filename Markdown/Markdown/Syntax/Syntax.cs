using System.Collections.Generic;

namespace Markdown
{
    public class Syntax : ISyntax
    {
        private static readonly HashSet<char> SpecialSymbols = new HashSet<char>
        {
            '\\', '`', '*', '_', '{', '}', '[', ']', '(', ')', '#', '+', '-', '.', '!', '?', '|', '$', '^', '/', '>',
            '<', '&'
        };

        private static readonly HashSet<char> ClosingBrackets = new HashSet<char> {')', ']', '>', '}'};

        private static readonly HashSet<char> OpeningBrackets = new HashSet<char> {'(', '[', '<', '{'};

        private readonly Dictionary<char, Attribute> charAttributes;

        public Syntax(Dictionary<char, Attribute> charAttributes)
        {
            this.charAttributes = charAttributes;
        }

        public static Syntax InitializeDefaultSyntax()
        {
            var escape = new Attribute(AttributeType.Escape, IsValidEscapeAttribute);

            var emphasis = new PairAttribute(
                AttributeType.Emphasis,
                IsValidPairCharAttribute,
                IsClosingDelimiter
            );

            var linkHeader = new PairAttribute(
                AttributeType.LinkHeader,
                IsValidBracketAttribute,
                IsClosingBracket
            );

            var linkDescription = new PairAttribute(
                AttributeType.LinkDescription,
                IsValidBracketAttribute,
                IsClosingBracket
            );

            var charAttributes = new Dictionary<char, Attribute>
            {
                {'_', emphasis},
                {'\\', escape},
                {'[', linkHeader},
                {']', linkHeader},
                {'(', linkDescription},
                {')', linkDescription}
            };
            return new Syntax(charAttributes);
        }


        public bool TryGetCharAttribute(string source, int charPosition, out Attribute charAttribute)
        {
            charAttribute = null;
            var ch = source[charPosition];
            if (!charAttributes.ContainsKey(ch))
                return false;

            var attribute = charAttributes[ch];
            if (!attribute.IsCharValid(source, charPosition))
                return false;

            charAttribute = attribute;
            return true;
        }

        private static bool IsValidPairCharAttribute(string source, int charPosition)
        {
            return IsOpeningDelimiter(source, charPosition) ^ IsClosingDelimiter(source, charPosition);
        }

        private static bool IsValidEscapeAttribute(string source, int charPosition)
        {
            return charPosition < source.Length - 1 && SpecialSymbols.Contains(source[charPosition + 1]);
        }

        private static bool IsOpeningDelimiter(string source, int charPosition)
        {
            if (StringHasGivenCharacterAtPosition(source, source[charPosition], charPosition - 1))
                return IsOpeningDelimiter(source, charPosition - 1);

            return (charPosition == 0 || char.IsWhiteSpace(source[charPosition - 1])
                                      || SpecialSymbols.Contains(source[charPosition - 1]))
                   && charPosition != source.Length - 1 && !char.IsWhiteSpace(source[charPosition + 1]);
        }

        private static bool IsClosingDelimiter(string source, int charPosition)
        {
            if (StringHasGivenCharacterAtPosition(source, source[charPosition], charPosition + 1))
                return IsClosingDelimiter(source, charPosition + 1);

            return (charPosition == source.Length - 1 || char.IsWhiteSpace(source[charPosition + 1]) ||
                    SpecialSymbols.Contains(source[charPosition + 1]))
                   && charPosition != 0 && !char.IsWhiteSpace(source[charPosition - 1]);
        }

        private static bool IsValidBracketAttribute(string source, int charPosition)
        {
            return IsOpeningBracket(source, charPosition) || IsClosingBracket(source, charPosition);
        }

        private static bool IsClosingBracket(string source, int charPosition)
        {
            return ClosingBrackets.Contains(source[charPosition]);
        }

        private static bool IsOpeningBracket(string source, int charPosition)
        {
            return OpeningBrackets.Contains(source[charPosition]);
        }

        private static bool StringHasGivenCharacterAtPosition(string source, char ch, int position)
        {
            return position >= 0 && position <= source.Length - 1 && source[position] == ch;
        }
    }
}
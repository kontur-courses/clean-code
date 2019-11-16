using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Syntax
    {
        private static readonly HashSet<char> SpecialSymbols = new HashSet<char>
        {
            '\\', '`', '*', '_', '{', '}', '[', ']', '(', ')', '#', '+', '-', '.', '!', '?', '|', '$', '^', '/', '>',
            '<', '&'
        };

        private readonly Dictionary<char, AttributeType> typeOfCharDictionary;


        private readonly Dictionary<AttributeType, Func<string, int, bool>> ValidateMethodDictionary;

        public Syntax(Dictionary<char, AttributeType> typeOfCharDictionary)
        {
            this.typeOfCharDictionary = typeOfCharDictionary;

            ValidateMethodDictionary =
                new Dictionary<AttributeType, Func<string, int, bool>>
                {
                    {AttributeType.Emphasis, IsValidPairCharAttribute},
                    {AttributeType.Escape, IsValidEscapeAttribute}
                };
        }

        public static Syntax InitializeDefaultSyntax()
        {
            var charTypes = new Dictionary<char, AttributeType>
            {
                {'_', AttributeType.Emphasis},
                {'\\', AttributeType.Escape}
            };
            return new Syntax(charTypes);
        }


        public bool TryGetCharAttribute(string source, int charPosition, out AttributeType type)
        {
            type = AttributeType.None;
            var ch = source[charPosition];
            if (!typeOfCharDictionary.ContainsKey(ch)) return false;

            var attribute = typeOfCharDictionary[ch];
            if (!IsValidCharAttribute(attribute, source, charPosition)) return false;

            type = attribute;
            return true;
        }

        private bool IsValidCharAttribute(AttributeType type, string source, int charPosition)
        {
            return ValidateMethodDictionary[type](source, charPosition);
        }

        private bool IsValidPairCharAttribute(string source, int charPosition)
        {
            return IsOpeningDelimiter(source, charPosition) ^ IsClosingDelimiter(source, charPosition);
        }

        private bool IsValidEscapeAttribute(string source, int charPosition)
        {
            return charPosition < source.Length - 1 && SpecialSymbols.Contains(source[charPosition + 1]);
        }

        public bool IsOpeningDelimiter(string source, int charPosition)
        {
            if (charPosition > 0
                && typeOfCharDictionary.ContainsKey(source[charPosition - 1])
                && typeOfCharDictionary[source[charPosition]] == typeOfCharDictionary[source[charPosition - 1]])
                return IsOpeningDelimiter(source, charPosition - 1);

            return (charPosition == 0 || char.IsWhiteSpace(source[charPosition - 1]) ||
                    source[charPosition - 1] == '\\')
                   && charPosition != source.Length - 1 && !char.IsWhiteSpace(source[charPosition + 1]);
        }

        public bool IsClosingDelimiter(string source, int charPosition)
        {
            if (charPosition < source.Length - 1
                && typeOfCharDictionary.ContainsKey(source[charPosition + 1])
                && typeOfCharDictionary[source[charPosition]] == typeOfCharDictionary[source[charPosition + 1]])
                return IsClosingDelimiter(source, charPosition + 1);

            return (charPosition == source.Length - 1 || char.IsWhiteSpace(source[charPosition + 1]))
                   && charPosition != 0 && !char.IsWhiteSpace(source[charPosition - 1]);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Syntax
    {
        private static readonly HashSet<char> SpecialSymbols = new HashSet<char>
        {
            '\\', '`', '*', '_', '{', '}', '[', ']', '(', ')', '#', '+', '-', '.', '!', '?', '|', '$', '^', '/', '>',
            '<', '&'
        };

        public readonly Dictionary<char, AttributeType> TypeDictionary;

        private readonly Dictionary<AttributeType, char> TextViewDictionary;

        private readonly Dictionary<AttributeType, Func<string, int, bool>> ValidateMethodDictionary;

        public Syntax(Dictionary<char, AttributeType> typeDictionary)
        {
            TypeDictionary = typeDictionary;
            TextViewDictionary = typeDictionary.ToDictionary(x => x.Value, x => x.Key);

            ValidateMethodDictionary =
                new Dictionary<AttributeType, Func<string, int, bool>>
                {
                    {AttributeType.Emphasis, IsValidPairAttribute},
                    {AttributeType.Escape, IsValidEscapeAttribute}
                };
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


        public bool TryGetAttribute(string source, int charPosition, out AttributeType type)
        {
            type = AttributeType.None;
            var ch = source[charPosition];
            if (!TypeDictionary.ContainsKey(ch)) return false;

            var attribute = TypeDictionary[ch];
            if (!IsValidAttribute(attribute, source, charPosition)) return false;

            type = attribute;
            return true;
        }

        private bool IsValidAttribute(AttributeType type, string source, int charPosition)
        {
            return ValidateMethodDictionary[type](source, charPosition);
        }

        private bool IsValidPairAttribute(string source, int charPosition)
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
                && TypeDictionary.ContainsKey(source[charPosition - 1])
                && TypeDictionary[source[charPosition]] == TypeDictionary[source[charPosition - 1]])
                return IsOpeningDelimiter(source, charPosition - 1);

            return (charPosition == 0 || char.IsWhiteSpace(source[charPosition - 1]) || source[charPosition - 1] == '\\')
                   && charPosition != source.Length - 1 && !char.IsWhiteSpace(source[charPosition + 1]);
        }

        public bool IsClosingDelimiter(string source, int charPosition)
        {
            if (charPosition < source.Length - 1 
                && TypeDictionary.ContainsKey(source[charPosition + 1]) 
                && TypeDictionary[source[charPosition]] == TypeDictionary[source[charPosition + 1]])
                return IsClosingDelimiter(source, charPosition + 1);

            return (charPosition == source.Length - 1 || char.IsWhiteSpace(source[charPosition + 1]))
                    && charPosition != 0 && (!char.IsWhiteSpace(source[charPosition - 1]));
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Syntax
    {
        public readonly Dictionary<char, AttributeType> TypeDictionary;
        public readonly Dictionary<AttributeType, char> CharDictionary;

        private static readonly HashSet<char> specialSymbols = new HashSet<char>() 
            { '\\', '`', '*', '_', '{', '}', '[', ']', '(', ')', '#', '+', '-', '.', '!','?', '|','$','^','/','>','<', '&'};

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
            var typeDictionary = new Dictionary<char, AttributeType>()
            {
                {'_', AttributeType.Emphasis},
                {'\\',AttributeType.Escape}
            };
            return new Syntax(typeDictionary);
        }
    }
}
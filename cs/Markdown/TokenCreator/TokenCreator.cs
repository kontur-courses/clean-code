using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Interfaces;

namespace Markdown
{
    public class TokenCreator : ITokenCreator
    {
        private readonly IEnumerable<IToken> subClasses = GetAllInheritors();
        private static readonly List<Type> ListOfDuplicateCreators = new() { typeof(TokenItalics), typeof(TokenStrong) };

        public IEnumerable<IToken> Create(string text)
        {
            text = text ?? throw new ArgumentException("Text must be not null");
            var index = 0;
            var tokens = new List<IToken>();
            var stringArray = GetStringArrayFromText(text);
            while (index < text.Length)
            {
                var token = GetToken(stringArray, index);
                index += token.Value.Length;
                tokens.Add(token);
            }
 
            return tokens;
        }
        
        private string[] GetStringArrayFromText(string text) => text.Select(x => x.ToString()).ToArray();

        private IToken GetToken(string[] text, int index)
        {
            var symbol = GetSymbol(text, index);
            var token = CreateToken(symbol).Create(text, index); 
            return token;
        }

        private string GetSymbol(string[] text, int index)
        {
            var symbol = text[index];
            var nextPosition = index + 1;
            if (symbol == "#")
                return nextPosition < text.Length && text[nextPosition] == " " ? "# " : "#";
            return symbol;
        }

        private IToken CreateToken(string current)
        {
            foreach (var subClass in subClasses)
            {
                if (!subClass.CanParse(current)) 
                    continue;
                return subClass;
            }

            return new TokenText();
        }

        private static IEnumerable<IToken> GetAllInheritors()
        {
            return typeof(IToken)
                .Assembly.GetTypes()
                .Where(type => ImplementInterfaceAndIsClass(type) && NotDefaultAndInDuplicate(type))
                .Select(type => (IToken)Activator.CreateInstance(type));
        }

        private static bool ImplementInterfaceAndIsClass(Type type)
            => typeof(IToken).IsAssignableFrom(type) && type.IsClass;
        
        private static bool NotDefaultAndInDuplicate(Type type) 
            => type != DefaultCreator() && !ListOfDuplicateCreators.Contains(type);
        
        private static Type DefaultCreator() => typeof(TokenText);
    }
}
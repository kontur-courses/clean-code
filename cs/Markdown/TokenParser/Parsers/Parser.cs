using System;
using System.Collections.Generic;
using System.Linq;
using Markdow.Interfaces;

namespace Markdown.TokenParser.Parsers
{
    public class Parser
    {
        protected readonly IReadOnlyList<IToken> Tokens;
        private readonly IEnumerable<IConcreteParser> concreteParsers;

        public Parser(IEnumerable<IToken> tokens)
        {
            Tokens = tokens.ToList();
            concreteParsers = GetAllInheritors();
        }
        
        public TokenTree[] Parse()
        {
            var components = new List<TokenTree>();
            var position = 0;
            while (position < Tokens.Count)
            {
                var component = ParseToken(position);
                position += component.Count;
                components.Add(component);
            }

            return components.ToArray();
        }

        public virtual TokenTree ParseToken(int position)
        {
            var token = Tokens[position];
            var parser = DefineParser(token.TokenType);
            return parser.ParseToken(position);
        }

        private IConcreteParser DefineParser(TokenType tokenType)
        {
            foreach (var parser in concreteParsers)
            {
                if (parser.CanParse(tokenType))
                    return parser;
            }

            throw new ArgumentOutOfRangeException($"Unsupported tokenType: {tokenType}");
        }
        
        protected IToken NextToken(int position) => GetTokenWithOffset(position, 1);
        
        protected IToken PreviousToken(int position) => GetTokenWithOffset(position, -1);
        
        protected bool HasCloseTokenInLine(TokenType tokenType, int position) => 
            TryGetFirstIndexOfTokenInLine(tokenType, position, out _);

        protected bool TryGetFirstIndexOfTokenInLine(TokenType tokenType, int position, out int index, int offset = 1)
        {
            index = offset  + position;
            do
            {
                var currentToken = GetTokenWithOffset(index, 0);
                if (currentToken.TokenType == tokenType)
                    return true;
                index++;
            } while (index < Tokens.Count && Tokens[index].TokenType != TokenType.NewLine);

            return false;
        }

        private IToken GetTokenWithOffset(int currentPosition, int offset)
        {
            var index = currentPosition + offset;
            if (index >= Tokens.Count)
                return Tokens.Last();
            if (index <= 0)
                return Tokens.First();
            return Tokens[index];
        }

        private IEnumerable<IConcreteParser> GetAllInheritors()
        {
            return typeof(IConcreteParser)
                .Assembly.GetTypes()
                .Where(ImplementInterfaceAndIsClass)
                .Select(type => (IConcreteParser) Activator.CreateInstance(type, Tokens));
        }
        
        private static bool ImplementInterfaceAndIsClass(Type type)
            => typeof(IConcreteParser).IsAssignableFrom(type) && type.IsClass;
    }
}
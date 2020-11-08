using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Abstract;
using MarkdownParser.Infrastructure.Models;

namespace MarkdownParser
{
    public class MarkdownDocumentParser
    {
        private readonly IMarkdownElementProvider[] providers;
        private readonly TokenParser tokenParser;

        public MarkdownDocumentParser(IEnumerable<IMarkdownElementProvider> providers, TokenParser tokenParser)
        {
            this.tokenParser = tokenParser;
            this.providers = providers.ToArray();
        }

        public MarkdownDocument Parse(string rawMarkdown)
        {
            var tokens = tokenParser.Tokenize(rawMarkdown).ToArray();
            return CreateDocumentFrom(tokens);
        }

        private MarkdownDocument CreateDocumentFrom(IReadOnlyList<Token> tokens)
        {
            var document = MarkdownDocument.Empty;
            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                var currentContext = new MarkdownElementContext(i, tokens);
                var element = CreateElementFrom(currentContext, token);

                document.Add(element);
                i = element.LastTokenIndex + 1;
            }

            return document;
        }

        private MarkdownElement CreateElementFrom(MarkdownElementContext currentContext, Token token)
        {
            var markdownElements = providers.Select(mp => new
                {
                    IsSuccessful = mp.TryParse(currentContext, out var elem),
                    Element = elem
                })
                .Where(x => x.IsSuccessful)
                .Select(x => x.Element)
                .ToArray();

            var element = markdownElements.Length switch
            {
                0 => throw new InvalidOperationException($"Cannot create markdown from token {token.GetType()}"),
                1 => markdownElements[0],
                _ => throw new InvalidOperationException($"Multiple markdown parser matched for {token.GetType()}"),
            };
            return element;
        }
    }
    
    
    public class TokenParser
    {
        private readonly ICollection<ITokenCreator> tokenCreators;

        public TokenParser(ICollection<ITokenCreator> tokenCreators)
        {
            this.tokenCreators = tokenCreators;
        }

        public ICollection<Token> Tokenize(string rawInput)
        {
            throw new NotImplementedException(); //TODO implement
        }
    }

    public interface ITokenCreator
    {
        string TokenSymbol { get; }
        Token TokenFrom(string text);
    }

    public abstract class TokenCreator<TToken> : ITokenCreator where TToken : Token
    {
        public abstract string TokenSymbol { get; }
        public abstract TToken TokenFrom(string text);

        Token ITokenCreator.TokenFrom(string text) => TokenFrom(text);
    }

}
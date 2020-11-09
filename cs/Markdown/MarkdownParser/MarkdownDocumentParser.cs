using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure;
using MarkdownParser.Infrastructure.Abstract;
using MarkdownParser.Infrastructure.Models;

namespace MarkdownParser
{
    public class MarkdownDocumentParser
    {
        private readonly MarkdownCollector collector;
        private readonly TokenParser tokenParser;

        public MarkdownDocumentParser(TokenParser tokenParser, MarkdownCollector markdownCollector)
        {
            this.tokenParser = tokenParser;
            collector = markdownCollector;
        }

        public MarkdownDocument Parse(string rawMarkdown)
        {
            var tokens = tokenParser.Tokenize(rawMarkdown).ToArray();
            return new MarkdownDocument(collector.ParseElementsFrom(tokens));
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
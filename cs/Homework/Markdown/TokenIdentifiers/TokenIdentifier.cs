using System;
using Markdown.Parser;
using Markdown.Tokens;

namespace Markdown.TokenIdentifiers
{
    public abstract class TokenIdentifier<TToken>
    where TToken: Token
    {
        protected TokenIdentifier(IParser<TToken> parser, string selector)
        {
            Parser = parser;
            Selector = selector;
        }

        protected IParser<TToken> Parser;
        protected string Selector { get; }

        public bool Identify(string[] paragraphs, TemporaryToken temporaryToken, out TToken identifiedToken)
        {
            if (IsValid(paragraphs, temporaryToken))
            {
                identifiedToken = CreateToken(temporaryToken);
                return true;
            }

            identifiedToken = null;
            return false;
        }

        public abstract TToken CreateToken(TemporaryToken temporaryToken);
        protected abstract bool IsValid(string[] paragraphs, TemporaryToken temporaryToken);

    }
}
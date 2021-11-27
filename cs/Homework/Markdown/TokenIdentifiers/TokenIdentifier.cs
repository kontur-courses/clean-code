using System;
using Markdown.Tokens;

namespace Markdown.TokenIdentifiers
{
    public abstract class TokenIdentifier
    {
        protected TokenIdentifier(string tag, Func<TemporaryToken, Token> tokenCreator)
        {
            Tag = tag;
            TokenCreator = tokenCreator;
        }

        protected string Tag { get; }
        protected Func<TemporaryToken, Token> TokenCreator { get; }

        public bool Identify(string[] paragraphs, TemporaryToken temporaryToken, out Token identifiedToken)
        {
            if (IsValid(paragraphs, temporaryToken))
            {
                identifiedToken = CreateToken(temporaryToken);
                return true;
            }

            identifiedToken = null;
            return false;
        }

        protected virtual Token CreateToken(TemporaryToken temporaryToken) => TokenCreator(temporaryToken); 
        protected abstract bool IsValid(string[] paragraphs, TemporaryToken temporaryToken);

    }
}
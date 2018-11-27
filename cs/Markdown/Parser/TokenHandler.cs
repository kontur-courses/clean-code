using System.Collections.Immutable;

namespace Markdown.Md.TagHandlers
{
    public abstract class TokenHandler
    {
        protected TokenHandler Successor;

        public TokenHandler SetSuccessor(TokenHandler successor)
        {
            Successor = successor;

            return this;
        }

        public abstract Token Handle(string str, int position, ImmutableStack<Token> openingTokens);
    }
}
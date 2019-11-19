using Markdown.Tokens;
using System.Collections.Generic;

namespace Markdown.Styles
{
    internal abstract class Style
    {
        private readonly Token[] beginTokens;
        private readonly Token[] endTokens;

        public Style(Token[] beginTokens, Token[] endTokens)
        {
            this.beginTokens = beginTokens;
            this.endTokens = endTokens;
        }

        public
        public List<Token> ChildTokens = new List<Token>();

        public abstract bool CanBeApplied(Token firstToken, out Token lastToken);
    }
}

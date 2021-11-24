using System.Collections.Generic;

namespace Markdown.Tokens
{
    public abstract class Token
    {
        protected List<Token> _inners;
        protected string Content { get;}

        protected abstract bool AllowInners { get; }

        public Token(string content)
        {
            Content = content;
            _inners = new List<Token>();
        }

        public abstract string Render();

        public void BuildTokenTree(IMdParser parser)
        {
            _inners = parser.ParseToTokens(Content);
            foreach (var inner in _inners)
                if (inner.AllowInners)
                    inner.BuildTokenTree(parser);
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class Token
    {
        protected List<Token> _inners;
        public virtual string Content { get; protected set; }

        public bool HasParent { get; protected set; }
        public abstract bool AllowInners { get; }
        public List<Token> Inners => _inners.ToList();

        public Token(string content)
        {
            Content = content;
            _inners = new List<Token>();
            HasParent = false;
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

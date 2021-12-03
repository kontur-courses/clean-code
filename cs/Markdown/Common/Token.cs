using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Common
{
    public class Token
    {
        private readonly List<Token> childTokens = new List<Token>();

        public string Value { get; }
        public int Position { get; private set; }
        public BaseMdTag MdTag { get; }


        public Token(string value)
            : this(value, 0, new BlockMdTag())
        {
        }
        
        public Token(int position, BaseMdTag mdTag)
            : this(mdTag.MdTag, 0, new BlockMdTag())
        {
        }

        public Token(string value, int position, BaseMdTag tag)
        {
            Value = value;
            Position = position;
            MdTag = tag;
        }


        public void AddToken(Token child)
        {
            var parent = childTokens.FirstOrDefault(token => token.IsChild(child));
            if (parent != null)
            {
                parent.AddToken(child);
                child.Position -= parent.Position + parent.MdTag.MdTag.Length;
            }
            else
                childTokens.Add(child);
        }

        public string Render()
        {
            var render = new StringBuilder(MdTag.RemoveMdTags(Value));
            foreach (var childToken in childTokens.OrderByDescending(token => token.Position))
            {
                render.Remove(childToken.Position, childToken.Value.Length);
                render.Insert(childToken.Position, childToken.Render());
            }

            return MdTag.InsertHtmlTags(render.ToString());
        }

        public bool IsChild(Token child)
        {
            return child.Position >= Position &&
                   child.Position < Position + Value.Length &&
                   child.Position + child.Value.Length <= Position + Value.Length;
        }
    }
}
using System.Collections.Generic;
using System.Text;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TagToken : IToken
    {
        public readonly List<IToken> ChildrenTokens;
        public readonly string MdTag;

        public TagToken(string text, string mdTag, List<IToken> childrenTokens, int position)
        {
            MdTag = mdTag;
            Text = text;
            ChildrenTokens = childrenTokens;
            Position = position;
        }

        public string Text { get; }
        public int Position { get; }

        public void Translate(ITranslator translator, StringBuilder stringBuilder)
        {
            translator.VisitTag(this, stringBuilder);
        }
    }
}
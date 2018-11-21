using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TagToken : IToken
    {
        public readonly List<IToken> TokensInnerTag;
        public readonly string MdTag;

        public TagToken(string text, string mdTag, List<IToken> tokensInnerTag, int position)
        {
            MdTag = mdTag;
            Text = text;
            TokensInnerTag = tokensInnerTag;
            Position = position;
        }

        public string Text { get; }
        public int Position { get; }

        public string Translate(ITranslator translator)
        {
            return translator.VisitTag(this);
        }
    }
}
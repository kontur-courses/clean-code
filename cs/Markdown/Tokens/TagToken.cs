using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TagToken : IToken
    {
        public readonly List<IToken> Tokens;
        public readonly string EmTag;

        public TagToken(string text, string emTag, List<IToken> tokens, int position)
        {
            EmTag = emTag;
            Text = text;
            Tokens = tokens;
            Position = position;
        }

        public string Text { get; }
        public int Position { get; }

        public string Accept(ITranslator translator)
        {
//            var result = string.Concat(tokens.Aggregate(htmlTag, (current, token) => current + token.ToHtml()), closingHtmlTag);
//            return result;
            return translator.VisitTag(this);
        }
    }
}
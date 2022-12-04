using System;
using Markdown.Extensions;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdCommentTag : Tag
    {
        private static readonly string commentText = "\\";
        public MdCommentTag() : base(commentText)
        {
            
        }

        public override IToken ToHtml() => new TextToken(Text);

        public override bool IsValidTag(string currentLine, int position) => true;

        public static bool IsCommented(string text, int beforePosition)
        {
            var startPosition = beforePosition - commentText.Length - 1;
            return text.IsInside(startPosition)
                   && text.AsSpan(startPosition).StartsWith(commentText);
        }
    }
}
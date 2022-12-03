using System;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdCommentTag : Tag
    {
        private static readonly string commentText = "\\";
        public MdCommentTag() : base(commentText)
        {
            
        }

        public override IToken ToHtml() => new TextToken(text);

        public override bool IsValidTag(string currentLine, int position) => true;

        public static bool IsCommented(string text, int beforePosition)
        {
            var startPosition = beforePosition - commentText.Length - 1;
            return startPosition >= 0
                   && text.AsSpan(startPosition).StartsWith(commentText);
        }
    }
}
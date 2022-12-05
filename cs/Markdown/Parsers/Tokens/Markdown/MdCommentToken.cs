using System;
using Markdown.Extensions;

namespace Markdown.Parsers.Tokens.Markdown
{
    public class MdCommentToken : TextToken
    {
        private static readonly string commentText = "\\";
        public MdCommentToken() : base(commentText)
        {
            
        }

        public override IToken ToHtml() => new TextToken(Text);

        public static bool IsStart(char symbol) => symbol == commentText[0];

        public static bool IsCommented(string text, int textStartPosition)
        {
            var startPosition = textStartPosition - commentText.Length - 1;
            return text.IsInside(startPosition)
                   && text.AsSpan(startPosition).StartsWith(commentText);
        }
    }
}
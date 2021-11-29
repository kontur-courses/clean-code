using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class FirstHeaderTaggedNode : TaggedNode
    {
        private const string HtmlTag = "h1";
        private const string MarkdownTag = "#";
        
        public FirstHeaderTaggedNode() : base(HtmlTag, MarkdownTag) {}

        public override bool TryOpen(List<IToken> tokens, ref int parentTokenPosition)
        {
            var isOpened = PreviousTokenIsParagraphEndToken(tokens, parentTokenPosition) &&
                   NextTokenIsWhiteSpace(tokens, parentTokenPosition);
            if (isOpened)
            {
                parentTokenPosition += 2;
            }

            return isOpened;
        }

        public override bool ShouldBeClosedByNewToken(List<IToken> tokens, int anotherTokenPosition)
        {
            return tokens[anotherTokenPosition] is ParagraphEndToken;
        }

        public override bool CannotBeClosed(List<IToken> tokens, int anotherTokenPosition)
        {
            return tokens[anotherTokenPosition] is Header1Token;
        }

        public override bool ShouldBeClosedWhenParagraphEnds()
        {
            return true;
        }

        private bool PreviousTokenIsParagraphEndToken(List<IToken> tokens, int parentTokenPosition)
        {
            return !tokens.InBorders(parentTokenPosition - 1) ||
                   tokens[parentTokenPosition - 1] is ParagraphEndToken;
        }

        private bool NextTokenIsWhiteSpace(List<IToken> tokens, int parentTokenPosition)
        {
            return tokens.InBorders(parentTokenPosition + 1) &&
                   tokens[parentTokenPosition + 1] is SpaceToken;
        }
    }
}
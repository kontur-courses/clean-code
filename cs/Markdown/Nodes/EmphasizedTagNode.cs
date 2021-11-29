using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class EmphasizedTaggedNode: TaggedNode
    {
        private const string HtmlTag = "em";
        private const string MarkdownTag = "_";
        private bool openedInsideWord;
        
        public EmphasizedTaggedNode() : base(HtmlTag, MarkdownTag) {}

        public override bool TryOpen(List<IToken> tokens, ref int parentTokenPosition)
        {
            var isOpened = tokens.InBorders(parentTokenPosition + 1) &&
                           tokens[parentTokenPosition + 1] is not SpaceToken;
            if (isOpened)
            {
                openedInsideWord = tokens.InBorders(parentTokenPosition - 1) &&
                                   tokens[parentTokenPosition - 1] is WordToken;
                parentTokenPosition += 1;
            }

            return isOpened;
        }

        public override bool ShouldBeClosedByNewToken(List<IToken> tokens, int anotherTokenPosition)
        {
            return tokens[anotherTokenPosition] is ItalicToken;
        }

        public override bool CannotBeClosed(List<IToken> tokens, int anotherTokenPosition)
        {
            return tokens[anotherTokenPosition] is ParagraphEndToken ||
                   tokens[anotherTokenPosition] is BoldToken ||
                   tokens[anotherTokenPosition] is WordToken {ContainsDigits: true} ||
                   openedInsideWord && tokens[anotherTokenPosition] is SpaceToken;
        }

        public override bool ShouldBeClosedWhenParagraphEnds()
        {
            return false;
        }
    }
}
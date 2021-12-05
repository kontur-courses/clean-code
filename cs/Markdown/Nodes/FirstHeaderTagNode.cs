using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class FirstHeaderTaggedNode : TaggedNode
    {
        private const string HtmlTag = "h1";
        private const string MarkdownTag = "#";
        
        public FirstHeaderTaggedNode() : base(HtmlTag, MarkdownTag) {}

        //FIXME
        public override bool TryOpen(Stack<INode> openedNodes, List<IToken> tokens, ref int parentTokenPosition)
        {
            var isOpened = PreviousTokenIsParagraphEndToken(tokens, parentTokenPosition) &&
                   NextTokenIsWhiteSpace(tokens, parentTokenPosition);
            if (isOpened)
            {
                parentTokenPosition += 2;
            }

            return isOpened;
        }

        public override void UpdateCondition(IToken newToken)
        {
            if (newToken is ParagraphEndToken)
            {
                this.Condition = NodeCondition.Closed;
            }

            if (newToken is Header1Token)
            {
                this.Condition = NodeCondition.ImpossibleToClose;
            }
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
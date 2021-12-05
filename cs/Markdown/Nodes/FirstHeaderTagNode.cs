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
        public override bool TryOpen(Stack<INode> openedNodes, CollectionIterator<IToken> tokensIterator)
        {
            var isOpened = PreviousTokenIsParagraphEndToken(tokensIterator) &&
                   NextTokenIsWhiteSpace(tokensIterator);
            if (isOpened)
            {
                tokensIterator.Move(2);
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

        private bool PreviousTokenIsParagraphEndToken(CollectionIterator<IToken> tokensIterator)
        {
            return !tokensIterator.TryGet(-1, out var prevToken) ||
                   prevToken is ParagraphEndToken;
        }

        private bool NextTokenIsWhiteSpace(CollectionIterator<IToken> tokensIterator)
        {
            return tokensIterator.TryGet(1, out var nextToken) &&
                   nextToken is SpaceToken;
        }
    }
}
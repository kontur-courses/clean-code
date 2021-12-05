using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class FirstHeaderTaggedNode : TaggedNode
    {
        private const string HtmlTag = "h1";
        private const string MarkdownTag = "#";
        
        public FirstHeaderTaggedNode() : base(HtmlTag, MarkdownTag) {}

        public override bool TryOpen(Stack<INode> openedNodes, CollectionIterator<IToken> tokensIterator)
        {
            if (IsOpening(tokensIterator))
            {
                tokensIterator.Move(2);
                Condition = NodeCondition.Opened;
                return true;
            }

            Condition = NodeCondition.ImpossibleToClose;
            return false;
        }

        public override void UpdateCondition(IToken newToken)
        {
            if (newToken is ParagraphEndToken)
            {
                this.Condition = NodeCondition.Closed;
            }
        }

        private bool PreviousTokenIsParagraphEndToken(CollectionIterator<IToken> tokensIterator)
        {
            return !tokensIterator.TryGet(-1, out var prevToken) ||
                   prevToken is ParagraphEndToken;
        }

        private bool IsOpening(CollectionIterator<IToken> tokensIterator)
        {
            return PreviousTokenIsParagraphEndToken(tokensIterator) &&
                   NextTokenIsWhiteSpace(tokensIterator);
        }

        private bool NextTokenIsWhiteSpace(CollectionIterator<IToken> tokensIterator)
        {
            return tokensIterator.TryGet(1, out var nextToken) &&
                   nextToken is SpaceToken;
        }
    }
}
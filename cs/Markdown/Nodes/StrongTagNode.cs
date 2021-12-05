using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class StrongTaggedNode: TaggedNode
    {
        private bool openedInsideWord;
        private bool prevTokenWasSpace;
        private const string HtmlTag = "strong";
        private const string MarkdownTag = "__";
        
        public StrongTaggedNode() : base(HtmlTag, MarkdownTag) {}
        
        public override bool TryOpen(Stack<INode> parentNodes, CollectionIterator<IToken> tokensIterator)
        {
            var isOpening =
                !ParentNodeWasEmphasized(parentNodes) &&
                tokensIterator.TryGet(1, out var nextToken) &&
                nextToken is not SpaceToken;

            if (isOpening)
            {
                openedInsideWord = tokensIterator.TryGet(-1, out var prevToken) &&
                                   prevToken is WordToken;
                tokensIterator.Move(1);
                Condition = NodeCondition.Opened;
                return true;
            }

            Condition = NodeCondition.ImpossibleToClose;
            return false;
        }

        public override void UpdateCondition(IToken newToken)
        {
            if (newToken is ParagraphEndToken or WordToken {ContainsDigits: true} ||
                openedInsideWord && newToken is SpaceToken)
            {
                Condition = NodeCondition.ImpossibleToClose;
            }
            else if (newToken is BoldToken && !prevTokenWasSpace)
            {
                Condition = Children.Any() ? NodeCondition.Closed : NodeCondition.ImpossibleToClose;
            }

            prevTokenWasSpace = newToken is SpaceToken;
        }

        private bool ParentNodeWasEmphasized(Stack<INode> openedNodes)
        {
            return openedNodes.Any(parent => parent is EmphasizedTaggedNode);
        }
    }
}
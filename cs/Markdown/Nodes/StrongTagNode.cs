using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class StrongTaggedNode: TaggedNode
    {
        private bool openedInsideWord;
        private const string HtmlTag = "strong";
        private const string MarkdownTag = "__";
        
        public StrongTaggedNode() : base(HtmlTag, MarkdownTag) {}

        
        //FIXME не проверяешь на цифры
        public override bool TryOpen(Stack<INode> openedNodes, List<IToken> tokens, ref int parentTokenPosition)
        {
            var isOpened =
                !ParentNodeWasEmphasized(openedNodes) &&
                tokens.InBorders(parentTokenPosition + 1) &&
                tokens[parentTokenPosition + 1] is not SpaceToken;

            if (isOpened)
            {
                openedInsideWord = tokens.InBorders(parentTokenPosition - 1) &&
                                   tokens[parentTokenPosition - 1] is WordToken;
                parentTokenPosition += 1;
            }

            return isOpened;
        }

        public override void UpdateCondition(IToken newToken)
        {
            if (newToken is BoldToken)
            {
                Condition = NodeCondition.Closed;
            }
            else if (newToken is ParagraphEndToken ||
                     newToken is WordToken {ContainsDigits: true} ||
                     openedInsideWord && newToken is SpaceToken)
            {
                Condition = NodeCondition.ImpossibleToClose;
            }
        }

        private bool ParentNodeWasEmphasized(Stack<INode> openedNodes)
        {
            return openedNodes.Any(parent => parent is EmphasizedTaggedNode);
        }
    }
}
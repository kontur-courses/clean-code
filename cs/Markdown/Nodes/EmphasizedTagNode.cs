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

        //FIXME
        public override bool TryOpen(Stack<INode> openedNodes, List<IToken> tokens, ref int parentTokenPosition)
        {
            var isOpened = tokens.InBorders(parentTokenPosition + 1) &&
                           tokens[parentTokenPosition + 1] is not SpaceToken;
            if (isOpened)
            {
                openedInsideWord = tokens.InBorders(parentTokenPosition - 1) &&
                                   tokens[parentTokenPosition - 1] is WordToken;
                parentTokenPosition += 1;
            }

            //FIXME
            Condition = NodeCondition.Opened;
            return isOpened;
        }

        public override void UpdateCondition(IToken newToken)
        {
            if (newToken is WordToken {ContainsDigits: true} ||
                openedInsideWord && newToken is SpaceToken)
            {
                Condition = NodeCondition.ImpossibleToClose;
            }
            else if (newToken is ItalicToken)
            {
                //ЧЕГО???
                Condition = NodeCondition.Closed;
            }
        }
    }
}
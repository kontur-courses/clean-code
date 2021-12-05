using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class EmphasizedTaggedNode: TaggedNode
    {
        private const string HtmlTag = "em";
        private const string MarkdownTag = "_";
        private bool openedInsideWord;
        private bool prevTokenWasSpace;
        
        public EmphasizedTaggedNode() : base(HtmlTag, MarkdownTag) {}

        //FIXME
        public override bool TryOpen(Stack<INode> openedNodes, CollectionIterator<IToken> tokensIterator)
        {
            var isOpened = tokensIterator.TryGet(1, out var nextToken) &&
                           nextToken is not SpaceToken;
            if (isOpened)
            {
                openedInsideWord = tokensIterator.TryGet(-1, out var prevToken) &&
                                   prevToken is WordToken;
                tokensIterator.Move(1);
            }

            //FIXME
            Condition = NodeCondition.Opened;
            return isOpened;
        }

        public override void UpdateCondition(IToken newToken)
        {
            if (newToken is ParagraphEndToken or WordToken {ContainsDigits: true} ||
                openedInsideWord && newToken is SpaceToken)
            {
                Condition = NodeCondition.ImpossibleToClose;
            }
            else if (newToken is ItalicToken && !prevTokenWasSpace)
            {
                //ЧЕГО???
                Condition = NodeCondition.Closed;
            }

            prevTokenWasSpace = newToken is SpaceToken;
        }
    }
}
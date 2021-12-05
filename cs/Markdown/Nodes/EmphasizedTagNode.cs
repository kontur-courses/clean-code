using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Nodes
{
    public class EmphasizedTaggedNode : TaggedNode
    {
        private const string HtmlTag = "em";
        private const string MarkdownTag = "_";
        private bool openedInsideWord;
        private bool prevTokenWasSpace;
        private bool prevTokenWasStrongTag;

        public EmphasizedTaggedNode() : base(HtmlTag, MarkdownTag) {}

        public override bool TryOpen(Stack<INode> openedNodes, CollectionIterator<IToken> tokensIterator)
        {
            if (tokensIterator.TryGet(1, out var nextToken) && nextToken is not SpaceToken)
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
                newToken is SpaceToken && (openedInsideWord || prevTokenWasStrongTag))
            {
                Condition = NodeCondition.ImpossibleToClose;
            }
            else if (newToken is ItalicToken && !prevTokenWasSpace)
            {
                Condition = NodeCondition.Closed;
            }

            prevTokenWasStrongTag = newToken is BoldToken;
            prevTokenWasSpace = newToken is SpaceToken;
        }
    }
}
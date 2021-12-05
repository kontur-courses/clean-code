using System.Collections.Generic;
using Markdown.Nodes;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkingTreeBuilder
    {
        public INode BuildTree(List<IToken> tokens)
        {
            var openedNodes = new Stack<INode>();
            openedNodes.Push(new StringNode(string.Empty));
            var tokensIterator = new CollectionIterator<IToken>(tokens);
            while (!tokensIterator.IsFinished)
            {
                if (tokensIterator.GetCurrent() is ParagraphEndToken paragraphEndToken)
                {
                    CloseParagraph(openedNodes, paragraphEndToken);
                    tokensIterator.Move(1);
                } else if (!TryCloseLastNode(openedNodes, tokensIterator))
                {
                    AddNewNode(openedNodes, tokensIterator);
                }
            }

            return openedNodes.Pop();
        }

        private bool TryCloseLastNode(Stack<INode> openedNodes, CollectionIterator<IToken> tokensIterator)
        {
            var parentNode = openedNodes.Peek();
            parentNode.UpdateCondition(tokensIterator.GetCurrent());
            var parentNodeCondition = parentNode.Condition;
            if (parentNodeCondition == NodeCondition.ImpossibleToClose)
            {
                openedNodes.Pop();
                openedNodes.Peek().AddChild(parentNode);
                return false;
            }
            if (parentNodeCondition == NodeCondition.Closed)
            {
                tokensIterator.Move(1);  
                openedNodes.Pop();
                openedNodes.Peek().AddChild(parentNode);

                return true;
            }

            return false;
        }

        private void AddNewNode(Stack<INode> openedNodes, CollectionIterator<IToken> tokensIterator)
        {
            var token = tokensIterator.GetCurrent();
            var node = token.ToNode();
            if (node.TryOpen(openedNodes, tokensIterator))
            {
                openedNodes.Push(node);
            }
            else
            {
                tokensIterator.Move(1);
                openedNodes.Peek().AddChild(node);
            }
        }

        private void CloseParagraph(Stack<INode> openedNodes, ParagraphEndToken paragraphEndToken)
        {
            var topNode = default(INode);
            while (openedNodes.TryPop(out var node))
            {
                if (topNode != null)
                    node.AddChild(topNode);
                
                node.UpdateCondition(paragraphEndToken);
                topNode = node;
            }
            
            openedNodes.Push(topNode);
        }
    }
}
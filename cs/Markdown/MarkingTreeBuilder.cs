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
            var tokenIndex = 0;
            while (tokenIndex < tokens.Count)
            {
                if (tokens[tokenIndex] is ParagraphEndToken paragraphEndToken)
                {
                    CloseParagraph(openedNodes, paragraphEndToken);
                    tokenIndex++;
                    continue;
                }

                if (!TryCloseLastNode(openedNodes, tokens, ref tokenIndex))
                {
                    AddNewNode(openedNodes, tokens, ref tokenIndex);
                }
            }

            return openedNodes.Pop();
        }

        private bool TryCloseLastNode(Stack<INode> openedNodes, List<IToken> tokens, ref int tokenId)
        {
            var parentNode = openedNodes.Peek();
            parentNode.UpdateCondition(tokens[tokenId]);
            var parentNodeCondition = parentNode.Condition;
            if (parentNodeCondition == NodeCondition.ImpossibleToClose)
            {
                openedNodes.Pop();
                openedNodes.Peek().AddChild(parentNode);
                return false;
            }
            if (parentNodeCondition == NodeCondition.Closed)
            {
                tokenId++;  
                openedNodes.Pop();
                openedNodes.Peek().AddChild(parentNode);

                return true;
            }

            return false;
        }

        private void AddNewNode(Stack<INode> openedNodes, List<IToken> tokens, ref int tokenId)
        {
            var token = tokens[tokenId];
            var node = token.ToNode();
            //FIXME
            if (node.TryOpen(openedNodes, tokens, ref tokenId))
            {
                openedNodes.Push(node);
            }
            else
            {
                tokenId++;
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
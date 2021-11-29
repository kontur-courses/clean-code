using System.Collections.Generic;
using Markdown.Nodes;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkingTreeBuilder
    {
        public INode BuiltTree(List<IToken> tokens)
        {
            var openedNodes = new Stack<INode>();
            openedNodes.Push(new StringNode(string.Empty));
            var tokenId = 0;
            while (tokenId < tokens.Count)
            {
                if (tokens[tokenId] is ParagraphEndToken)
                {
                    CloseParagraph(openedNodes);
                    tokenId++;
                    continue;
                }

                if (!TryCloseLastNode(openedNodes, tokens, ref tokenId))
                {
                    AddNewNode(openedNodes, tokens, ref tokenId);
                }
            }

            CloseParagraph(openedNodes);
            return openedNodes.Pop();
        }

        private bool TryCloseLastNode(Stack<INode> openedNodes, List<IToken> tokens, ref int tokenId)
        {
            var parentNode = openedNodes.Peek();
            if (parentNode.CannotBeClosed(tokens, tokenId))
            {
                openedNodes.Pop();
                openedNodes.Peek().AddChild(parentNode);
                return false;
            }
            if (parentNode.ShouldBeClosedByNewToken(tokens, tokenId))
            {
                tokenId++;
                parentNode.Close();
                openedNodes.Pop();
                openedNodes.Peek().AddChild(parentNode);

                return true;
            }

            return false;
        }

        private void AddNewNode(Stack<INode> stack, List<IToken> tokens, ref int tokenId)
        {
            var token = tokens[tokenId];
            var node = token.ToNode();
            if (node.TryOpen(tokens, ref tokenId))
            {
                stack.Push(node);
            }
            else
            {
                tokenId++;
                stack.Peek().AddChild(node);
            }
        }

        private void CloseParagraph(Stack<INode> stack)
        {
            var topNode = default(INode);
            while (stack.TryPop(out var node))
            {
                if (topNode != null)
                    node.AddChild(topNode);

                if (node.ShouldBeClosedWhenParagraphEnds())
                {
                    node.Close();
                }

                topNode = node;
            }
            
            stack.Push(topNode);
        }
    }
}
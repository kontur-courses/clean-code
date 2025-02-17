using System.Collections.Immutable;
using Markdown.treeVisitor;

namespace Markdown.Converter;

internal class ConvertTree : IConvertTree
{
    public INode Convert(IImmutableList<Token> tokens)
    {
        var start = -1;
        var tree = CreateNode(tokens, new LineNode(), false, ref start);


        return tree;
    }

    private TNode CreateNode<TNode>(IImmutableList<Token> tokens, TNode node, bool haveOuterToken, ref int j)
        where TNode : INode
    {
        for (j++; j < tokens.Count; j++)
        {
            if (!tokens[j].IsTag)
            {
                node.InnerNode.Add(new TextNode(tokens[j].Value));
            }

            else if (tokens[j].Type is TokenType.Header)
            {
                var header = CreateNode(tokens, new HeaderNode(), false, ref j);
                node.NextNode = header;
            }

            else if (tokens[j].Type is TokenType.Marker)
            {
                var marker = CreateNode(tokens, new MarkerNode(), false, ref j);
                node.InnerNode.Add(marker);
            }

            else if (tokens[j].Type is TokenType.MarkerRange)
            {
                if (node is MarkerRangeNode)
                {
                    node.NextNode = CreateNode(tokens, new NewLineNode(), false, ref j);
                    break;
                }

                var markerRange = CreateNode(tokens, new MarkerRangeNode(), false, ref j);
                node.NextNode = markerRange;
            }
            else if (tokens[j].Type is TokenType.Italic)
            {
                if (node is ItalicNode)
                {
                    if (!haveOuterToken) node.NextNode = CreateNode(tokens, new LineNode(), false, ref j);

                    break;
                }

                if (node is HeaderNode or BoldNode or MarkerNode)
                {
                    node.InnerNode.Add(CreateNode(tokens, new ItalicNode(), true, ref j));
                }

                else
                {
                    var italic = CreateNode(tokens, new ItalicNode(), false, ref j);
                    node.NextNode = italic;
                }
            }

            else if (tokens[j].Type is TokenType.Bold)
            {
                if (node is BoldNode)
                {
                    if (!haveOuterToken) node.NextNode = CreateNode(tokens, new LineNode(), false, ref j);

                    break;
                }

                if (node is HeaderNode or MarkerNode)
                {
                    node.InnerNode.Add(CreateNode(tokens, new BoldNode(), true, ref j));
                }

                else
                {
                    var bold = CreateNode(tokens, new BoldNode(), false, ref j);

                    node.NextNode = bold;
                }
            }

            else if (tokens[j].Type is TokenType.NewLine)
            {
                if (node is HeaderNode)
                {
                    node.NextNode = CreateNode(tokens, new NewLineNode(), false, ref j);
                    break;
                }

                if (node is MarkerRangeNode)
                {
                    node.InnerNode.Add(new NewLineNode());
                }

                if (node is MarkerNode)
                {
                    j--;
                    break;
                }
            }
        }

        return node;
    }
}
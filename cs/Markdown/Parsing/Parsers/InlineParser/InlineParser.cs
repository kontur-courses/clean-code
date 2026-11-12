using Markdown.Nodes;
using Markdown.Nodes.HtmlTags;

namespace Markdown.Parsing.Parsers.InlineParser;

public class InlineParser : ParserBase
{
    private readonly Lexer lexer = new();
    private readonly PairFinder pairFinder = new();

    protected override List<INode> ParseText(string text)
    {
        var tokens = lexer.Tokenize(text);
        var validPairs = pairFinder.FindValidPairs(tokens, text);
        return BuildTree(tokens, validPairs);
    }

    private List<INode> BuildTree(List<Token> tokens, HashSet<(int Index, bool IsOpening)> validPairs)
    {
        var rootNodes = new List<INode>();
        var currentNodes = rootNodes;
        var stack = new Stack<(HtmlTag Tag, List<INode> ParentNodes)>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (token.Type == TokenType.Text)
            {
                currentNodes.Add(new TextNode(token.Value));
                continue;
            }

            if (validPairs.Contains((i, true)))
            {
                var tag = token.Type.ToHtmlTag();
                stack.Push((tag, currentNodes));
                currentNodes = new();
            }
            else if (validPairs.Contains((i, false)))
            {
                var (tag, parentNodes) = stack.Pop();
                var container = new ContainerNode(tag, currentNodes);
                currentNodes = parentNodes;
                currentNodes.Add(container);
            }
            else
            {
                currentNodes.Add(new TextNode(token.Value));
            }
        }

        return rootNodes;
    }
}
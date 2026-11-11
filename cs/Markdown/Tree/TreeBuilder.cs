using Markdown.Tokens;

namespace Markdown.Tree;

public class TreeBuilder
{
    public IReadOnlyList<INode> Build(IReadOnlyList<Token> tokens)
    {
        var nodes = new List<INode>();
        BuildRange(tokens, 0, tokens.Count, nodes);
        return nodes;
    }

    private static void BuildRange(IReadOnlyList<Token> tokens, int start, int end, List<INode> nodes)
    {
        var index = start;

        while (index < end)
        {
            var token = tokens[index];

            if (token.IsMarkup && token.Role == TokenRole.Opening)
            {
                var closeIndex = token.PairIndex;
                var children = new List<INode>();

                BuildRange(tokens, index + 1, closeIndex, children);
                nodes.Add(CreateNode(tokens, token, closeIndex, children));

                index = GetNextIndexAfter(token, closeIndex);
                continue;
            }

            nodes.Add(new TextNode(token.Value));
            index++;
        }
    }

    private static INode CreateNode(IReadOnlyList<Token> tokens, Token opener, int closeIndex, List<INode> children)
    {
        return opener.Kind switch
        {
            TokenKind.Underscore => new EmNode(children),
            TokenKind.DoubleUnderscore => new StrongNode(children),
            TokenKind.LeftBracket => new LinkNode(ExtractUrl(tokens, closeIndex), children),
            _ => throw new InvalidOperationException($"Открывающей лексемой не может быть {opener.Kind}.")
        };
    }

    private static string ExtractUrl(IReadOnlyList<Token> tokens, int closeBracketIndex)
    {
        var rawUrlWithParens = tokens[closeBracketIndex + 1].Value;
        return rawUrlWithParens[1..^1];
    }

    private static int GetNextIndexAfter(Token opener, int closeIndex)
    {
        return opener.Kind == TokenKind.LeftBracket
            ? closeIndex + 2
            : closeIndex + 1;
    }
}
using System.Diagnostics;
using System.Text;
using Markdown.Syntax;

namespace Markdown.Parsing;

/// <summary>
/// строит дерево документа, делит токены на блоки,
/// + определяет вид каждого блока и собирает inline-узлы по парам маркеров от InlineDelimiterMatcher
/// </summary>
internal static class MarkdownParser
{
    public static DocumentNode Parse(IReadOnlyList<Token> tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);

        var blocks = new List<BlockNode>();

        foreach (var blockTokens in SplitIntoBlocks(tokens))
        {
            blocks.Add(ParseBlock(blockTokens));
        }

        return new DocumentNode(blocks);
    }

    /// <summary>
    /// блок - это подряд идущие непустые строки
    /// переносы строк внутри блока сохраняются как токены
    /// </summary>
    private static List<List<Token>> SplitIntoBlocks(
        IReadOnlyList<Token> tokens)
    {
        var blocks = new List<List<Token>>();
        List<Token>? currentBlock = null;

        foreach (var line in SplitIntoLines(tokens))
        {
            if (IsBlank(line))
            {
                currentBlock = null;
                continue;
            }

            if (currentBlock is null)
            {
                currentBlock = new List<Token>();
                blocks.Add(currentBlock);
            }
            else
            {
                currentBlock.Add(Token.LineBreak);
            }

            currentBlock.AddRange(line);
        }

        return blocks;
    }

    private static List<List<Token>> SplitIntoLines(
        IReadOnlyList<Token> tokens)
    {
        var lines = new List<List<Token>>();
        var currentLine = new List<Token>();

        foreach (var token in tokens)
        {
            if (token.Kind != TokenKind.LineBreak)
            {
                currentLine.Add(token);
                continue;
            }

            lines.Add(currentLine);
            currentLine = new List<Token>();
        }

        lines.Add(currentLine);

        return lines;
    }

    private static bool IsBlank(List<Token> line)
    {
        return line.TrueForAll(IsWhitespaceText);
    }

    private static bool IsWhitespaceText(Token token)
    {
        return token.Kind == TokenKind.Text &&
               string.IsNullOrWhiteSpace(token.Value);
    }

    private static BlockNode ParseBlock(List<Token> block)
    {
        if (StartsWith(block, TokenKind.HeadingMarker))
        {
            return new HeadingNode(
                ParseInlineNodes(WithoutMarker(block)));
        }

        if (StartsWith(block, TokenKind.ListMarker) &&
            TryParseUnorderedList(block) is { } list)
        {
            return list;
        }

        return new ParagraphNode(ParseInlineNodes(block));
    }

    /// <summary>
    /// список - блок, в котором каждая строка начинается с маркера
    /// иначе блок остаётся обычным абзацем
    /// </summary>
    private static UnorderedListNode? TryParseUnorderedList(List<Token> block)
    {
        var lines = SplitIntoLines(block);

        if (!lines.TrueForAll(
                line => StartsWith(line, TokenKind.ListMarker)))
        {
            return null;
        }

        var items = new List<ListItemNode>(lines.Count);

        foreach (var line in lines)
        {
            items.Add(new ListItemNode(
                ParseInlineNodes(WithoutMarker(line))));
        }

        return new UnorderedListNode(items);
    }

    private static bool StartsWith(List<Token> tokens, TokenKind kind)
    {
        return tokens.Count > 0 && tokens[0].Kind == kind;
    }

    private static List<Token> WithoutMarker(List<Token> tokens)
    {
        return tokens.GetRange(1, tokens.Count - 1);
    }

    private static List<InlineNode> ParseInlineNodes(
        IReadOnlyList<Token> tokens)
    {
        var pairs = InlineDelimiterMatcher.FindPairs(tokens);

        return BuildInlineNodes(tokens, pairs, 0, tokens.Count);
    }

    private static List<InlineNode> BuildInlineNodes(
        IReadOnlyList<Token> tokens,
        DelimiterPair?[] pairs,
        int startIndex,
        int endIndex)
    {
        var nodes = new List<InlineNode>();
        var textStart = startIndex;

        for (var i = startIndex; i < endIndex; i++)
        {
            if (pairs[i] is not { } pair)
                continue;

            AddTextNode(tokens, textStart, i, nodes);

            var children = BuildInlineNodes(
                tokens,
                pairs,
                i + 1,
                pair.ClosingIndex);

            nodes.Add(CreateNode(pair.Type, children));

            i = pair.ClosingIndex;
            textStart = i + 1;
        }

        AddTextNode(tokens, textStart, endIndex, nodes);

        return nodes;
    }

    private static InlineNode CreateNode(
        DelimiterType type,
        IReadOnlyList<InlineNode> children)
    {
        return type switch
        {
            DelimiterType.Emphasis => new EmphasisNode(children),
            DelimiterType.Strong => new StrongNode(children),
            _ => throw new UnreachableException()
        };
    }

    private static void AddTextNode(
        IReadOnlyList<Token> tokens,
        int startIndex,
        int endIndex,
        List<InlineNode> nodes)
    {
        if (startIndex >= endIndex)
            return;

        nodes.Add(new TextNode(
            ConcatenateValues(tokens, startIndex, endIndex)));
    }

    private static string ConcatenateValues(
        IReadOnlyList<Token> tokens,
        int startIndex,
        int endIndex)
    {
        if (endIndex - startIndex == 1)
            return tokens[startIndex].Value;

        var builder = new StringBuilder();

        for (var i = startIndex; i < endIndex; i++)
        {
            builder.Append(tokens[i].Value);
        }

        return builder.ToString();
    }
}

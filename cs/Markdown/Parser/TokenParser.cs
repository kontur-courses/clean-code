using Markdown.Domains;
using Markdown.Domains.Nodes;

namespace Markdown.Parser;

/// <summary>
///     Парсит список токенов Markdown в синтаксическое дерево.
/// </summary>
public class TokenParser(List<MdToken> tokens, int index = 0)
{
    private readonly List<MdToken> tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
    private int index = index;

    private static readonly HashSet<TokenType> EscapableTokenTypes =
    [
        TokenType.Grid,
        TokenType.Underscore,
        TokenType.LeftSquareBracket,
        TokenType.RightSquareBracket,
        TokenType.LeftParenthesis,
        TokenType.RightParenthesis,
        TokenType.Slash
    ];

    private bool IsEnd => index >= tokens.Count;
    private bool HasNext => index + 1 < tokens.Count;
    private MdToken Current => tokens[index];

    public RootNode Parse(NodeContext context = NodeContext.None)
    {
        var rootChildren = new List<Node>();

        while (!IsEnd)
        {
            var token = Current;

            switch (token.Type)
            {
                case TokenType.Grid:
                    HandleHeader(rootChildren);
                    break;

                case TokenType.Escape:
                    HandleNewLine(rootChildren);
                    break;

                case TokenType.Slash:
                    HandleSlashCharacter(rootChildren);
                    break;

                case TokenType.Underscore:
                    HandleUnderscore(rootChildren, context);
                    break;

                case TokenType.LeftSquareBracket:
                    HandleLeftSquareBracket(rootChildren, context);
                    break;

                case TokenType.LeftParenthesis:
                case TokenType.RightParenthesis:
                case TokenType.Word:
                case TokenType.Number:
                case TokenType.Space:
                case TokenType.RightSquareBracket:
                default:
                    HandleText(token, rootChildren);
                    break;
            }
        }

        return new RootNode(rootChildren);
    }

    private static TokenParser CreateParserFor(List<MdToken> inputTokens) =>
        new(inputTokens);

    #region Header

    private void HandleHeader(List<Node> rootChildren)
    {
        var headerStart = index;
        var level = GetHeaderLevel();

        var atLineStart = IsAtStartOfLine(tokens, headerStart);
        var headerLevelOk = level <= HeaderNode.MaxHeaderLevel;
        var afterHeaderIsWhitespaceOrEol = IsEnd || IsWhitespaceToken(Current.Type);

        var isHeader = atLineStart && headerLevelOk && afterHeaderIsWhitespaceOrEol;

        if (isHeader)
        {
            ParseHeaderContent(level, rootChildren);
        }
        else
        {
            rootChildren.Add(new TextNode(new string('#', level)));
        }
    }

    private int GetHeaderLevel()
    {
        var level = 0;
        while (!IsEnd && Current.Type == TokenType.Grid)
        {
            level++;
            MoveIndex();
        }

        return level;
    }

    private void ParseHeaderContent(int level, List<Node> rootChildren)
    {
        var whitespaceAfter = CountWhitespaceAfter(index);
        MoveIndex(whitespaceAfter);

        var countUntilEol = CountTokensUntilEndOfLine();
        var innerTokens = tokens.GetRange(index, countUntilEol);
        MoveIndex(countUntilEol);

        var innerBlock = CreateParserFor(innerTokens).Parse().Children;
        rootChildren.Add(new HeaderNode(level, innerBlock));
    }

    #endregion

    #region New line

    private void HandleNewLine(List<Node> rootChildren)
    {
        rootChildren.Add(new NewLineNode());
        MoveIndex();
    }

    #endregion

    #region Slash

    private void HandleSlashCharacter(List<Node> rootChildren)
    {
        if (!HasNext)
        {
            rootChildren.Add(new TextNode("\\"));
            MoveIndex();
            return;
        }

        var next = tokens[index + 1];

        if (EscapableTokenTypes.Contains(next.Type))
        {
            rootChildren.Add(new TextNode(next.Value));
            MoveIndex(2);
        }
        else
        {
            rootChildren.Add(new TextNode("\\"));
            MoveIndex();
        }
    }

    #endregion

    #region Text

    private void HandleText(MdToken token, List<Node> rootChildren)
    {
        rootChildren.Add(new TextNode(token.Value));
        MoveIndex();
    }

    #endregion

    #region Underscore

    private void HandleUnderscore(List<Node> rootChildren, NodeContext context)
    {
        var underscoreCount = tokens.GetTokensCountAfter(index, TokenType.Underscore);
        MoveIndex(underscoreCount);

        var whitespaceStart = index;
        var whitespaceAfter = CountWhitespaceAfter(index);
        MoveIndex(whitespaceAfter);

        if (whitespaceAfter != 0)
        {
            rootChildren.AddSymbol("_", underscoreCount);
            AddWhitespaceNodes(rootChildren, whitespaceStart, whitespaceAfter);
            return;
        }

        if (underscoreCount is 1 or 2)
        {
            index += HandleUnderscoresAndReturnShift(underscoreCount, rootChildren, context);
        }
        else
        {
            rootChildren.AddSymbol("_", underscoreCount);
        }
    }

    private int HandleUnderscoresAndReturnShift(
        int underscoreCount,
        List<Node> rootChildren,
        NodeContext context = NodeContext.None)
    {
        var closeIndex = FindClosing(tokens, index, underscoreCount, TokenType.Underscore);

        if (closeIndex == -1)
        {
            rootChildren.AddSymbol("_", underscoreCount);
            return 0;
        }

        if (IsEscaped(tokens, closeIndex))
        {
            var nextIndex = closeIndex + 1;
            if (nextIndex >= tokens.Count)
            {
                rootChildren.AddSymbol("_", underscoreCount);
                return 0;
            }

            var extraUnderscores = tokens.GetTokensCountAfter(nextIndex, TokenType.Underscore);
            if (extraUnderscores >= underscoreCount)
            {
                closeIndex = nextIndex;
            }
            else
            {
                rootChildren.AddSymbol("_", underscoreCount);
                return 0;
            }
        }

        var whitespaceBefore = CountWhitespaceBefore(closeIndex);

        if (whitespaceBefore > 0)
            return HandleNonFormattingUnderscore(closeIndex, underscoreCount, whitespaceBefore, rootChildren);

        if (ShouldHandleAsNonFormattingUnderscore(context, closeIndex, underscoreCount))
            return HandleNonFormattingUnderscore(closeIndex, underscoreCount, 0, rootChildren, context);

        var shift = CreateFormattingNode(underscoreCount, closeIndex, rootChildren);
        return shift;
    }

    private int HandleNonFormattingUnderscore(
        int closeIndex,
        int underscoreCount,
        int whitespaceBefore,
        List<Node> rootChildren,
        NodeContext context = NodeContext.None)
    {
        var innerLength = closeIndex - index - whitespaceBefore;
        var tokensInsideUnderscores = tokens.GetRange(index, innerLength);

        var innerBlock = CreateParserFor(tokensInsideUnderscores)
            .Parse(context)
            .Children;

        rootChildren.AddSymbol("_", underscoreCount);
        rootChildren.AddRange(innerBlock);

        if (whitespaceBefore > 0)
            rootChildren.AddSymbol(" ", whitespaceBefore);

        rootChildren.AddSymbol("_", underscoreCount);
        return closeIndex - index + underscoreCount;
    }

    private int CreateFormattingNode(int underscoreCount, int closeIndex, List<Node> rootChildren)
    {
        var innerTokens = tokens.GetRange(index, closeIndex - index);

        var innerContext = underscoreCount == 1 ? NodeContext.Italic : NodeContext.None;

        var parsedInnerBlock = CreateParserFor(innerTokens)
            .Parse(innerContext)
            .Children;

        Node formattingNode = underscoreCount switch
        {
            1 => new ItalicNode(parsedInnerBlock),
            2 => new BoldNode(parsedInnerBlock),
            _ => throw new InvalidOperationException("Unexpected underscore count for formatting node")
        };

        rootChildren.Add(formattingNode);

        var shift = closeIndex - index + underscoreCount;
        return shift;
    }

    private bool ShouldHandleAsNonFormattingUnderscore(
        NodeContext context,
        int closeIndex,
        int underscoreCount)
    {
        if (context == NodeContext.Italic
            || tokens.IsUnderscoreInDifferentWord(index - 1, closeIndex, underscoreCount)
            || tokens.IsUnderscoreInWordWithNumbers(index - 1, closeIndex, underscoreCount))
        {
            return true;
        }

        for (var i = index; i < closeIndex; i++)
        {
            if (tokens[i].Type == TokenType.Underscore && !IsEscaped(tokens, i))
            {
                return tokens
                    .GetRange(index, closeIndex - index)
                    .HaveNotPairedUnderscore();
            }
        }

        return false;
    }

    #endregion

    #region Link

    private void HandleLeftSquareBracket(List<Node> rootChildren, NodeContext context)
    {
        if (TryParseLink(context, out var linkNode))
        {
            rootChildren.Add(linkNode ?? throw new InvalidOperationException());
        }
        else
        {
            rootChildren.AddSymbol("[", 1);
            MoveIndex();
        }
    }

    private bool TryParseLink(NodeContext context, out LinkNode? node)
    {
        node = null;

        var bracketsLength = tokens.GetTokensCountAfter(index, TokenType.LeftSquareBracket);
        var meaningTextCloseIndex = FindClosing(tokens, index, bracketsLength, TokenType.RightSquareBracket);

        if (meaningTextCloseIndex == -1
            || IsEscaped(tokens, meaningTextCloseIndex)
            || !IsValidLinkSyntax(meaningTextCloseIndex))
            return false;

        var linkTextCloseIndex =
            FindClosing(tokens, meaningTextCloseIndex + 2, 1, TokenType.RightParenthesis);

        if (linkTextCloseIndex == -1 || IsEscaped(tokens, linkTextCloseIndex))
            return false;

        node = BuildLinkNode(
            meaningStart: index + 1,
            meaningEnd: meaningTextCloseIndex,
            linkStart: meaningTextCloseIndex + 2,
            linkEnd: linkTextCloseIndex,
            context: context
        );

        MoveIndex(linkTextCloseIndex + 1 - index);

        return true;
    }

    private LinkNode BuildLinkNode(int meaningStart, int meaningEnd,
        int linkStart, int linkEnd, NodeContext context)
    {
        var meaningTokens = tokens.GetRange(meaningStart, meaningEnd - meaningStart);
        var linkTokens = tokens.GetRange(linkStart, linkEnd - linkStart);

        var meaningText = CreateParserFor(meaningTokens).Parse(context).Children;
        var linkText = CreateParserFor(linkTokens).Parse(context).Children;

        return new LinkNode(
            LinkNodeType.LinkRoot,
            [
                new LinkNode(LinkNodeType.MeaningText, meaningText),
                new LinkNode(LinkNodeType.LinkText, linkText)
            ]);
    }

    private bool IsValidLinkSyntax(int meaningTextCloseIndex)
    {
        return meaningTextCloseIndex < tokens.Count - 1
               && tokens[meaningTextCloseIndex + 1].Type == TokenType.LeftParenthesis
               && HasPlaceForLinkText(meaningTextCloseIndex);
    }

    private bool HasPlaceForLinkText(int meaningTextCloseIndex) =>
        meaningTextCloseIndex + 2 < tokens.Count;

    #endregion

    #region Helpers

    /// <summary>
    ///     Поиск закрывающего индекса закрывающего токена
    /// </summary>
    /// <param name="tokens">Список токенов для анализа.</param>
    /// <param name="startIndex">Индекс первого токена после конца открывающей цепочки токенов</param>
    /// <param name="patternLen">Длина цепочки токенов, соответствующих шаблону</param>
    /// <param name="tokenType">Тип токена, который ищется</param>
    /// <returns>
    ///     Индекс первого токена в закрывающей цепочке, либо -1, если закрывающая цепочка не найдена.
    /// </returns>
    public static int FindClosing(List<MdToken> tokens, int startIndex, int patternLen, TokenType tokenType)
    {
        ArgumentNullException.ThrowIfNull(tokens);

        if (startIndex < 0 || startIndex >= tokens.Count)
            return -1;

        if (patternLen <= 0)
            return -1;

        var maxStart = tokens.Count - patternLen;

        for (var j = startIndex; j <= maxStart; j++)
        {
            if (!RunMatches(tokens, j, patternLen, tokenType))
                continue;

            var prevIndex = j - 1;
            var prevSame = prevIndex >= 0
                           && tokens[prevIndex].Type == tokenType
                           && !IsEscaped(tokens, prevIndex);

            var nextIndex = j + patternLen;
            var nextSame = nextIndex < tokens.Count
                           && tokens[nextIndex].Type == tokenType
                           && !IsEscaped(tokens, nextIndex);

            if (!prevSame && !nextSame)
                return j;
        }

        return -1;
    }

    private void MoveIndex(int shift = 1)
    {
        index += shift;
    }

    private int CountTokensUntilEndOfLine()
    {
        var count = 0;
        var position = index;

        while (position < tokens.Count &&
               tokens[position].Type != TokenType.Escape)
        {
            count++;
            position++;
        }

        return count;
    }

    private static bool IsAtStartOfLine(List<MdToken> tokens, int position) =>
        position == 0 || tokens[position - 1].Type == TokenType.Escape;

    private static bool IsWhitespaceToken(TokenType type) =>
        type is TokenType.Space;

    private int CountWhitespaceAfter(int startIndex)
    {
        var count = 0;
        var i = startIndex;
        while (i < tokens.Count && IsWhitespaceToken(tokens[i].Type))
        {
            count++;
            i++;
        }

        return count;
    }

    private int CountWhitespaceBefore(int position)
    {
        var count = 0;
        var i = position - 1;
        while (i >= 0 && IsWhitespaceToken(tokens[i].Type))
        {
            count++;
            i--;
        }

        return count;
    }

    private static bool IsEscaped(List<MdToken> tokens, int position)
    {
        var backslashCount = 0;
        var i = position - 1;

        while (i >= 0 && tokens[i].Type == TokenType.Slash)
        {
            backslashCount++;
            i--;
        }

        return backslashCount % 2 == 1;
    }

    private static bool RunMatches(List<MdToken> tokens, int start, int length, TokenType type)
    {
        for (var k = 0; k < length; k++)
        {
            var index = start + k;
            if (index >= tokens.Count)
                return false;

            if (tokens[index].Type != type)
                return false;

            if (IsEscaped(tokens, index))
                return false;
        }

        return true;
    }

    private void AddWhitespaceNodes(List<Node> rootChildren, int startIndex, int count)
    {
        var end = Math.Min(startIndex + count, tokens.Count);

        for (var i = startIndex; i < end; i++)
        {
            var t = tokens[i];

            if (!IsWhitespaceToken(t.Type))
                break;

            rootChildren.Add(new TextNode(t.Value));
        }
    }

    #endregion
}
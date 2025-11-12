using Markdown.Tokenizing;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing;

public static class UnderscoreHandler
{
    public static void HandleUnderscores(
        List<InlineTypeNode> children,
        List<(TokenType Type, int ChildrenIndex)> underscores,
        ParserCursor cursor,
        IList<Token> tokens)
    {
        var currentToken = cursor.Current;
        var previousToken = tokens.ElementAtOrDefault(cursor.Index - 1);
        var nextToken = tokens.ElementAtOrDefault(cursor.Index + 1);

        if (IsUnderscoreBetweenDigits(previousToken, nextToken))
        {
            AddUnderscoreAsLiteral(children, currentToken.Value, cursor);
            return;
        }

        if (!IsTokenWhitespaceLike(previousToken))
        {
            if (TryCloseExistingHighlight(underscores, currentToken, children, cursor))
                return;
        }

        if (IsTokenWhitespaceLike(nextToken) ||
            (currentToken.Type == TokenType.DoubleUnderscore && DoesDoubleUnderscoreBreak(cursor, tokens)))
        {
            AddUnderscoreAsLiteral(children, currentToken.Value, cursor);
        }
        else
        {
            underscores.Add((currentToken.Type, children.Count));
            cursor.MoveNext();
        }
    }

    public static void InsertUnmatchedUnderscores(List<InlineTypeNode> children,
        List<(TokenType Type, int ChildrenIndex)> underscores)
    {
        for (var i = underscores.Count - 1; i >= 0; i--)
        {
            var (type, index) = underscores[i];
            var literal = type == TokenType.DoubleUnderscore ? "__" : "_";
            children.Insert(index, new TextNode(literal));
        }
    }

    private static bool TryCloseExistingHighlight(
        List<(TokenType Type, int ChildrenIndex)> underscores,
        Token currentToken,
        List<InlineTypeNode> children,
        ParserCursor cursor)
    {
        var highlightingType = currentToken.Type;

        var openerIndex = FindMatchingOpenerIndex(underscores, highlightingType);
        if (openerIndex < 0) return false;

        var openerUnderscore = underscores[openerIndex];
        var startIndex = openerUnderscore.ChildrenIndex;
        var innerTokensCount = children.Count - startIndex;

        if (innerTokensCount == 0) return false;

        var innerTokens = children.GetRange(startIndex, innerTokensCount);

        if (!IsValidHighlighting(underscores, highlightingType, openerUnderscore, innerTokens)) return false;

        if (HasIntersection(underscores, openerUnderscore, highlightingType, children, openerIndex, out var innerIndex))
        {
            InsertIntersection(children, underscores, openerUnderscore, underscores[innerIndex], openerIndex, innerIndex);
            AddUnderscoreAsLiteral(children, currentToken.Value, cursor);
            return true;
        }

        CloseHighlight(children, underscores, openerIndex, highlightingType, innerTokens, startIndex);
        cursor.MoveNext();
        return true;
    }

    private static int FindMatchingOpenerIndex(List<(TokenType Type, int ChildrenIndex)> underscores, TokenType type)
    {
        for (var i = underscores.Count - 1; i >= 0; i--)
        {
            if (underscores[i].Type == type) return i;
        }

        return -1;
    }

    private static bool IsValidHighlighting(
        List<(TokenType Type, int ChildrenIndex)> underscores,
        TokenType highlightingType,
        (TokenType Type, int ChildrenIndex) openerUnderscore,
        List<InlineTypeNode> innerTokens)
    {
        if (highlightingType != TokenType.Underscore) return true;

        var highlightingIsInsideDoubleUnderscore = underscores.Any(underscore =>
            underscore.Type == TokenType.DoubleUnderscore &&
            underscore.ChildrenIndex < openerUnderscore.ChildrenIndex);

        var areThereAnyWhitespaces = innerTokens.Any(t => t is TextNode tn && tn.Text.Any(char.IsWhiteSpace));

        return highlightingIsInsideDoubleUnderscore || !areThereAnyWhitespaces;
    }

    private static bool HasIntersection(
        List<(TokenType Type, int ChildrenIndex)> underscores,
        (TokenType Type, int ChildrenIndex) opener,
        TokenType highlightingType,
        List<InlineTypeNode> children,
        int openerIndex,
        out int intersectionIndex)
    {
        for (var i = openerIndex + 1; i < underscores.Count; i++)
        {
            if (underscores[i].Type == highlightingType ||
                underscores[i].ChildrenIndex <= opener.ChildrenIndex ||
                underscores[i].ChildrenIndex >= children.Count)
                continue;

            intersectionIndex = i;
            return true;
        }

        intersectionIndex = -1;
        return false;
    }

    private static void CloseHighlight(
        List<InlineTypeNode> children,
        List<(TokenType Type, int ChildrenIndex)> underscores,
        int openerIndex,
        TokenType highlightingType,
        List<InlineTypeNode> innerTokens,
        int startIndex)
    {
        children.RemoveRange(startIndex, innerTokens.Count);

        InlineTypeNode node = highlightingType == TokenType.DoubleUnderscore
            ? new StrongNode(innerTokens)
            : new EmNode(innerTokens);

        children.Add(node);
        underscores.RemoveAt(openerIndex);
    }

    private static void InsertIntersection(
        List<InlineTypeNode> children,
        List<(TokenType Type, int ChildrenIndex)> underscores,
        (TokenType Type, int ChildrenIndex) opener,
        (TokenType Type, int ChildrenIndex) inner,
        int openerIndex, int innerIndex)
    {
        var innerLiteral = inner.Type == TokenType.DoubleUnderscore ? "__" : "_";
        var openerLiteral = opener.Type == TokenType.DoubleUnderscore ? "__" : "_";

        children.Insert(inner.ChildrenIndex, new TextNode(innerLiteral));
        children.Insert(opener.ChildrenIndex, new TextNode(openerLiteral));

        underscores.RemoveAt(innerIndex);
        underscores.RemoveAt(openerIndex);
    }

    private static void AddUnderscoreAsLiteral(List<InlineTypeNode> children, string value, ParserCursor cursor)
    {
        children.Add(new TextNode(value));
        cursor.MoveNext();
    }

    private static bool IsTokenWhitespaceLike(Token? token)
    {
        return token == null ||
               token.Type == TokenType.Whitespace ||
               token.Type == TokenType.EndOfLine ||
               token.Type == TokenType.EndOfFile;
    }

    private static bool IsUnderscoreBetweenDigits(Token? previousToken, Token? nextToken)
    {
        var leftTokenIsDigit = previousToken is { Type: TokenType.Text, Value.Length: > 0 } &&
                               char.IsDigit(previousToken.Value.Last());
        var rightTokenIsDigit = nextToken is { Type: TokenType.Text, Value.Length: > 0 } &&
                                char.IsDigit(nextToken.Value.First());
        return leftTokenIsDigit && rightTokenIsDigit;
    }

    private static bool DoesDoubleUnderscoreBreak(ParserCursor cursor, IList<Token> tokens)
    {
        var singleUnderscoresCount = 0;

        for (var i = cursor.Index + 1; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];

            if (currentToken.Type is TokenType.EndOfLine or TokenType.EndOfFile) return false;
            if (currentToken.Type == TokenType.DoubleUnderscore) return (singleUnderscoresCount % 2) == 1;
            if (currentToken.Type != TokenType.Underscore) continue;

            var previousToken = tokens.ElementAtOrDefault(i - 1);
            if (previousToken is { Type: TokenType.Escape }) continue;

            singleUnderscoresCount++;
        }

        return false;
    }
}
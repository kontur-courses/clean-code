using System.Text;
using Markdown.Parsing.Nodes;
using Markdown.Tokenizing;

namespace Markdown.Parsing;

public static class LinkHandler
{
    public static void HandleLink(List<InlineTypeNode> children, ParserCursor cursor, IList<Token> tokens)
    {
        var link = TryParseLink(cursor, tokens);
        if (link != null) children.Add(link);
        else
        {
            children.Add(new TextNode("["));
            cursor.MoveNext();
        }
    }

    private static LinkNode? TryParseLink(ParserCursor cursor, IList<Token> tokens)
    {
        var rightBracket = FindTokenBeforeEol(tokens, cursor.Index + 1, TokenType.RightBracket);
        if (rightBracket == -1) return null;

        if (rightBracket + 1 >= tokens.Count || tokens[rightBracket + 1].Type != TokenType.LeftParen) return null;

        var rightParen = FindTokenBeforeEol(tokens, rightBracket + 2, TokenType.RightParen);
        if (rightParen == -1) return null;

        var saved = cursor.Index;
        cursor.MoveNext();

        var innerText = HandleLinkText(cursor, cursor.Index, rightBracket);

        cursor.IndexJumpTo(rightBracket + 2);

        var urlBuilder = new StringBuilder();
        while (!cursor.End && cursor.Index < rightParen)
        {
            urlBuilder.Append(cursor.Current.Value);
            cursor.MoveNext();
        }

        if (cursor.Current.Type != TokenType.RightParen)
        {
            cursor.IndexJumpTo(saved);
            return null;
        }

        cursor.MoveNext();
        return new LinkNode(innerText, urlBuilder.ToString());
    }

    private static List<InlineTypeNode> HandleLinkText(
        ParserCursor cursor, int from, int to)
    {
        var saved = cursor.Index;
        cursor.IndexJumpTo(from);

        var children = new List<InlineTypeNode>();

        while (cursor.Index < to && !cursor.End)
        {
            var token = cursor.Current;

            if (token.Type is TokenType.Text or TokenType.Whitespace)
            {
                children.Add(new TextNode(token.Value));
                cursor.MoveNext();
            }
            else if (token.Type == TokenType.Escape)
            {
                cursor.MoveNext();
                if (!cursor.End &&
                    cursor.Current.Type != TokenType.EndOfLine &&
                    cursor.Current.Type != TokenType.EndOfFile)
                {
                    children.Add(new TextNode(cursor.Current.Value));
                    cursor.MoveNext();
                }
                else
                {
                    children.Add(new TextNode("\\"));
                }
            }
            else
            {
                children.Add(new TextNode(token.Value));
                cursor.MoveNext();
            }
        }

        cursor.IndexJumpTo(saved);
        return children;
    }

    private static int FindTokenBeforeEol(IList<Token> tokens, int start, TokenType type)
    {
        for (var i = start; i < tokens.Count; i++)
        {
            var tok = tokens[i];
            if (tok.Type == type) return i;
            if (tok.Type is TokenType.EndOfLine or TokenType.EndOfFile)
                return -1;
        }

        return -1;
    }
}
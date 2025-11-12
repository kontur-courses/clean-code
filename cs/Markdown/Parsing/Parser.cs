using Markdown.Tokenizing;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing;

public class Parser
{
    public static RootNode Parse(List<Token> tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);

        var cursor = new ParserCursor(tokens);
        var blocks = new List<BlockTypeNode>();

        while (!cursor.End)
        {
            if (cursor.Current.Type == TokenType.EndOfLine)
            {
                cursor.MoveNext();
                continue;
            }

            blocks.Add(ParseBlock(cursor, tokens));
        }

        return new RootNode(blocks);
    }

    private static BlockTypeNode ParseBlock(ParserCursor cursor, IList<Token> tokens)
    {
        if (cursor.Current.Type == TokenType.Hash &&
            cursor.Peek().Type == TokenType.Whitespace)
        {
            cursor.MoveNext();
            cursor.MoveNext();

            var inlines = ParseInlinesInBlock(cursor, tokens);
            if (cursor.Current.Type == TokenType.EndOfLine)
                cursor.MoveNext();

            return new HeaderNode(inlines);
        }

        var parsedInlines = ParseInlinesInBlock(cursor, tokens);
        if (cursor.Current.Type == TokenType.EndOfLine) cursor.MoveNext();

        return new ParagraphNode(parsedInlines);
    }

    private static List<InlineTypeNode> ParseInlinesInBlock(ParserCursor cursor, IList<Token> tokens)
    {
        var children = new List<InlineTypeNode>();
        var underscores = new List<(TokenType Type, int ChildrenIndex)>();

        while (!cursor.End &&
               cursor.Current.Type != TokenType.EndOfLine &&
               cursor.Current.Type != TokenType.EndOfFile)
        {
            var token = cursor.Current;

            switch (token.Type)
            {
                case TokenType.Text:
                case TokenType.Whitespace:
                    AddLiteral(children, token.Value, cursor);
                    break;

                case TokenType.Escape:
                    EscapeHandler.HandleEscape(children, cursor, tokens);
                    break;

                case TokenType.LeftBracket:
                    LinkHandler.HandleLink(children, cursor, tokens);
                    break;

                case TokenType.Underscore:
                case TokenType.DoubleUnderscore:
                    UnderscoreHandler.HandleUnderscores(children, underscores, cursor, tokens);
                    break;

                default:
                    AddLiteral(children, token.Value, cursor);
                    break;
            }
        }

        UnderscoreHandler.InsertUnmatchedUnderscores(children, underscores);
        return children;
    }

    private static void AddLiteral(List<InlineTypeNode> children, string value, ParserCursor cursor)
    {
        children.Add(new TextNode(value));
        cursor.MoveNext();
    }
}
using Markdown.Parsing.Nodes;
using Markdown.Tokenizing;

namespace Markdown.Parsing;

public static class EscapeHandler
{
    public static void HandleEscape(List<InlineTypeNode> children, ParserCursor cursor, IList<Token> tokens)
    {
        cursor.MoveNext(); // skip '\'
        var next = cursor.Current;

        // Lone backslash — keep it
        if (next.Type is TokenType.EndOfLine or TokenType.EndOfFile)
        {
            children.Add(new TextNode("\\"));
            return;
        }

        // Escaped backslash
        if (next.Type is TokenType.Escape)
        {
            children.Add(new TextNode("\\"));
            cursor.MoveNext();
            return;
        }
        
        if (next.Type is TokenType.Text)
        {
            children.Add(new TextNode("\\" + next.Value));
            cursor.MoveNext();
            return;
        }
        
        children.Add(new TextNode(next.Value));
        cursor.MoveNext();
    }
}
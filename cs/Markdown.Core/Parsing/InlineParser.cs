using System.Text;
using Markdown.Core.Lexing;
using Markdown.Core.Parsing.Nodes;

namespace Markdown.Core.Parsing;

public class InlineParser(
    IReadOnlyList<Token> tokens,
    Func<Token> moveNext,
    Func<Token> currentToken,
    Func<int> currentIndex,
    Func<bool> isEndOfLine,
    InlineValidator validator)
{
    public InlineNode? ParseInline()
    {
        var token = currentToken();
        if (token.Kind == TokenKind.Eof)
            return null;

        return token.Kind switch
        {
            TokenKind.Text => ParseText(),
            TokenKind.Underscore => ParseEmphasis(),
            TokenKind.DoubleUnderscore => ParseStrong(),
            TokenKind.LeftBracket => ParseLink(),
            _ => ParseText()
        };
    }

    private TextNode ParseText()
    {
        var node = new TextNode(currentToken().Slice.ToString());
        moveNext();
        return node;
    }

    private InlineNode ParseEmphasis()
    {
        var startIndex = currentIndex() - 1;
        moveNext();

        if (IsInvalidEmphasisStart())
            return CreateTextNode("_");

        var emphasis = new EmphasisNode();
        return ParseEmphasisContent(emphasis, startIndex);
    }
    
    private InlineNode ParseStrong()
    {
        var startIndex = currentIndex() - 1;
        moveNext();

        if (IsInvalidStrongStart())
            return ConvertToTextNode(new StrongNode(), "__");

        var strong = new StrongNode();
        return ParseStrongContent(strong, startIndex);
    }
    
    private InlineNode ParseLink()
    {
        var linkTextNodes = new List<InlineNode>();
        moveNext();

        while (!isEndOfLine() && currentToken().Kind != TokenKind.RightBracket)
        {
            var inline = ParseInline();
            if (inline != null)
                linkTextNodes.Add(inline);
        }

        if (currentToken().Kind != TokenKind.RightBracket)
            return RestoreAsText("[", linkTextNodes);

        moveNext();

        if (currentToken().Kind != TokenKind.LeftParen)
            return RestoreAsText("[", linkTextNodes, "]");

        moveNext(); 

        var hrefBuilder = new StringBuilder();
        while (currentToken().Kind != TokenKind.Eof &&
               currentToken().Kind != TokenKind.RightParen &&
               currentToken().Kind != TokenKind.NewLine)
        {
            hrefBuilder.Append(currentToken().Slice.ToString());
            moveNext();
        }

        if (currentToken().Kind != TokenKind.RightParen)
            return RestoreAsText("[", linkTextNodes, "](" + hrefBuilder);

        moveNext(); 

        var href = hrefBuilder.ToString();
        return new LinkNode(href, linkTextNodes);
    }

    private bool IsInvalidEmphasisStart() =>
        currentToken().Kind is TokenKind.Space or TokenKind.NewLine or TokenKind.Eof;

    private InlineNode ParseEmphasisContent(EmphasisNode emphasis, int startIndex)
    {
        while (!IsEndOfContent())
        {
            if (currentToken().Kind == TokenKind.Underscore)
            {
                if (TryCloseEmphasis(emphasis, startIndex, out var node))
                    return node!;
            }
            else if (currentToken().Kind == TokenKind.DoubleUnderscore)
            {
                emphasis.Inlines.Add(CreateTextNode("__"));
                moveNext();
            }
            else
            {
                var inline = ParseInline();
                if (inline != null)
                    emphasis.Inlines.Add(inline);
            }
        }
        return ConvertToTextNode(emphasis, "_");
    }

    private bool IsInvalidStrongStart() => 
        currentToken().Kind is TokenKind.Space or TokenKind.NewLine or TokenKind.Eof;

    private InlineNode ParseStrongContent(StrongNode strong, int startIndex)
    {
        while (!IsEndOfContent())
        {
            if (currentToken().Kind == TokenKind.DoubleUnderscore)
            {
                if (TryCloseStrong(strong, startIndex, out var node))
                    return node!;
            }
            else if (currentToken().Kind == TokenKind.Underscore)
            {
                var emphasis = ParseEmphasis();
                strong.Inlines.Add(emphasis);
            }
            else
            {
                var inline = ParseInline();
                if (inline != null)
                    strong.Inlines.Add(inline);
            }
        }
        return ConvertToTextNode(strong, "__");
    }

    private bool IsEndOfContent() => currentToken().Kind is TokenKind.NewLine or TokenKind.Eof;
    
    private bool TryCloseEmphasis(EmphasisNode emphasis, int startIndex, out InlineNode? node)
    {
        var closeIndex = currentIndex() - 1;
        if (validator.IsValidEmphasisClose(tokens, startIndex, closeIndex))
        {
            moveNext();
            node = emphasis;
            return true;
        }
        emphasis.Inlines.Add(CreateTextNode("_"));
        moveNext();
        node = null;
        return false;
    }

    private bool TryCloseStrong(StrongNode strong, int startIndex, out InlineNode? node)
    {
        var closeIndex = currentIndex() - 1;
        if (validator.IsValidStrongClose(tokens, startIndex, closeIndex))
        {
            moveNext();
            node = strong;
            return true;
        }

        strong.Inlines.Add(CreateTextNode("__"));
        moveNext();
        node = null;
        return false;
    }
    
    private static TextNode CreateTextNode(string text) => new(text);

    private static TextNode RestoreAsText(string prefix, IList<InlineNode> nodes, string suffix = "")
    {
        var builder = new StringBuilder(prefix);
        foreach (var node in nodes)
            builder.Append(ExtractTextFromNode(node));
        builder.Append(suffix);
        return new TextNode(builder.ToString());
    }

    private static TextNode ConvertToTextNode(InlineNode node, string prefix)
    {
        var textContent = new StringBuilder();
        textContent.Append(prefix);
        textContent.Append(ExtractTextFromNode(node));
        return new TextNode(textContent.ToString());
    }

    private static string ExtractTextFromNode(InlineNode node)
    {
        var result = new StringBuilder();

        switch (node)
        {
            case EmphasisNode emphasis:
                foreach (var inline in emphasis.Inlines)
                    result.Append(ExtractTextFromNode(inline));
                break;
            case StrongNode strong:
                foreach (var inline in strong.Inlines)
                    result.Append(ExtractTextFromNode(inline));
                break;
            case TextNode text:
                result.Append(text.Text);
                break;
            case LinkNode link:
                foreach (var inline in link.Inlines)
                    result.Append(ExtractTextFromNode(inline));
                break;
        }
        return result.ToString();
    }
}

using System.Text;
using Markdown.Nodes.Interfaces;
using Markdown.Nodes.Internal;
using Markdown.Nodes.Leaf;
using Markdown.Parsers.Interfaces;
using Markdown.Tokenizer;

namespace Markdown.Parsers;

public class ImageParser : IParser
{
    private readonly MarkdownParser parser;
    private readonly HashSet<TokenType> altRollbackSymbols = [TokenType.NewLine, TokenType.Hash];
    private readonly HashSet<TokenType> altExpectedStopSymbols = [TokenType.RBracket];

    public ImageParser(MarkdownParser parser)
    {
        this.parser = parser;
    }

    public ParseStatus TryParse(out MarkdownNode node)
    {
        node = new TextNode("!");
        if (parser.CurrentToken.Type != TokenType.Exclamation)
            return ParseStatus.Fail();

        parser.MoveNext();

        parser.ParentStack.Push(parser.CurrentParent);
        node = new ImageNode("!");
        parser.CurrentParent = node;
        if (TryReadAlt(out var altNode))
        {
            node.AddChild(altNode);
            if (TryReadUrl(out var urlNode))
            {
                node.AddChild(urlNode);
                parser.CurrentParent = parser.ParentStack.Pop();
                return ParseStatus.Ok();
            }
        }

        var prevParent = parser.ParentStack.Peek();
        prevParent.AddChildren(MarkdownParser.Rollback(node));
        parser.CurrentParent = prevParent;
        
        return ParseStatus.Fail();
    }

    private bool TryReadAlt(out MarkdownNode node)
    {
        node = new TextNode("[");
        if (parser.CurrentToken.Type != TokenType.LBracket) 
            return false;
        
        parser.ParentStack.Push(parser.CurrentParent);
        node = new AltNode("[");
        parser.CurrentParent = node;
        parser.MoveNext();
        parser.ParseTokens(altRollbackSymbols, altExpectedStopSymbols);
        
        return parser.CurrentToken.Type == TokenType.RBracket;
    }

    private bool TryReadUrl(out MarkdownNode node)
    {
        var urlBuilder = new StringBuilder();
        node = new TextNode("");
        parser.MoveNext();

        if (parser.CurrentToken.Type != TokenType.LParenthesis)
            return false;

        parser.MoveNext();
        while (parser.CurrentToken.Type != TokenType.RParenthesis
               && parser.CurrentToken.Type != TokenType.NewLine
               && parser.CurrentToken.Type != TokenType.Carriage
               && parser.CurrentToken.Type != TokenType.Eof)
        {
            urlBuilder.Append(parser.CurrentToken.Value);
            parser.MoveNext();
        }

        if (parser.CurrentToken.Type != TokenType.RParenthesis)
        {
            node = new TextNode(urlBuilder.ToString());
            return false;
        }
        parser.MoveNext();
        node = new UrlNode(urlBuilder.ToString());
        
        return true;
    }
}
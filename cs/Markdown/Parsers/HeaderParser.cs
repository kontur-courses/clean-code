using Markdown.Nodes.Interfaces;
using Markdown.Nodes.Internal;
using Markdown.Nodes.Leaf;
using Markdown.Parsers.Interfaces;
using Markdown.Tokenizer;

namespace Markdown.Parsers;

public class HeaderParser : IParser
{
    private readonly MarkdownParser parser;
    private readonly HashSet<TokenType> expectedStopSymbols = [TokenType.NewLine, TokenType.Carriage];

    public HeaderParser(MarkdownParser parser)
    {
        this.parser = parser;
    }

    public ParseStatus TryParse(out MarkdownNode node)
    {
        if (parser.NextToken != null
            && (parser.CurrentToken.Type != TokenType.Hash
                || (parser.CurrentToken.Type == TokenType.Hash && parser.NextToken.Type != TokenType.Space)))
        {
            node = new TextNode("#");
            return ParseStatus.Fail();
        }
        parser.MoveNext();

        node = new HeaderNode("#");
        parser.ParentStack.Push(parser.CurrentParent);
        parser.CurrentParent = node;
        parser.MoveNext();
        parser.ParseTokens(null, expectedStopSymbols);
        
        if (parser.CurrentToken.Type != TokenType.Eof)
            node.AddChild(new TextNode($"{parser.CurrentToken.Value}"));
        
        if (!expectedStopSymbols.Contains(parser.CurrentToken.Type))
            parser.CurrentParent = parser.ParentStack.Pop();

        return ParseStatus.Ok();
    }
}
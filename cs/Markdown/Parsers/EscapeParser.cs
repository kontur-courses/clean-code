using Markdown.Nodes.Interfaces;
using Markdown.Nodes.Leaf;
using Markdown.Parsers.Interfaces;
using Markdown.Tokenizer;

namespace Markdown.Parsers;

public class EscapeParser : IParser
{
    private readonly MarkdownParser parser;

    public EscapeParser(MarkdownParser parser)
    {
        this.parser = parser;
    }

    public ParseStatus TryParse(out MarkdownNode node)
    {
        node = new TextNode(@"\");
        if (parser.CurrentToken.Type != TokenType.Escape)
            return ParseStatus.Fail();

        parser.MoveNext();
        node = new TextNode($"{parser.CurrentToken.Value}");
        
        return ParseStatus.Ok();
    }
}
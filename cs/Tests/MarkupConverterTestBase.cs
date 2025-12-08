using MarkupConverter.Parsers;
using MarkupConverter.Parsers.BlockParsers;
using MarkupConverter.Parsers.InlineParsers;
using MarkupConverter.Renderers;

namespace Tests;

public class MarkupConverterTestBase
{
    protected MarkupConverter.MarkupConverter Converter;
    
    [SetUp]
    public void Setup()
    {
        var parser = new Parser(
            [new HeaderParser(), new ParagraphParser()],
            new InlineParser()
        );
        var renderer = new HtmlRenderer();
        
        Converter = new MarkupConverter.MarkupConverter(parser ,renderer);
    }
}
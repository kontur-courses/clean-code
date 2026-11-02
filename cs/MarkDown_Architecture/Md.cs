using System.Text;
using Markdown.Parsing;

namespace Markdown;

public class Md
{
    private readonly BlockParser parser;

    public Md()
    {
        var inlineParser = new InlineParser();
        parser = new BlockParser(inlineParser);
    }

    public string Render(string text)
    {
        throw new NotImplementedException();
    }
}
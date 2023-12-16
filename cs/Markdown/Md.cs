using System.Text;

namespace Markdown;

public class Md
{

    public string Render(string text)
    {
        var tagDict = Tags.TagDict;
        var parser = new Parser(tagDict);
        var renderer = new Renderer();
        return Builder.Build(renderer.HandleTokens(parser.Parse(text)));
    }
}
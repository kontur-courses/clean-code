using System.Text;
using Markdown.Extensions;

namespace Markdown;

public class Md
{
    private Dictionary<string, TagType> tagDictionary;

    public Md(Dictionary<string, TagType> tagDictionary)
    {
        this.tagDictionary = tagDictionary;
    }

    public string Render(string text)
    {
        var lines = text.Split(new[] {"\r\n", "\n",  }, StringSplitOptions.None);
        var result = new StringBuilder();
        foreach (var line in lines)
        {
            var parser = new Parser(tagDictionary).Parse(line);
            var Renderer = new Renderer();
            result.Append(Renderer.HandleTokens(parser).ConcatenateToString());
        }

        return result.ToString().Trim('\n');
    }
}
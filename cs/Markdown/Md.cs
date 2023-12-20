using System.Text;

namespace Markdown;

public class Md
{
    Dictionary<string, TagType> tagDictionary = new Dictionary<string, TagType>()
    {
        {"_", TagType.Italic},
        {"__", TagType.Bold},
        {"# ", TagType.Heading},
        {"\n", TagType.LineBreaker},
        {"\r\n", TagType.LineBreaker},
        {"* ", TagType.Bulleted}
            
    };
    public string Render(string text)
    {
        var lines = text.Split(new[] {"\r\n", "\n",  }, StringSplitOptions.None);
        var result = new StringBuilder();
        foreach (var line in lines)
        {
            var parser = new Parser(tagDictionary).Parse(line);
            var Renderer = new Renderer();
            result.Append(Builder.Build(Renderer.HandleTokens(parser)));
        }

        return result.ToString().Trim('\n');
    }
}
using System.Collections.Immutable;

namespace Markdown;

public class Program
{
    public static void Main(string[] args)
    {
        var tagDictionary = new Dictionary<string, TagType>()
        {
            {"_", TagType.Italic},
            {"__", TagType.Bold},
            {"# ", TagType.Heading},
            {"\n", TagType.LineBreaker},
            {"\r\n", TagType.LineBreaker}
            
        };
        var text = "* _text_\n";
        var sut = new Md(tagDictionary);
        var t= sut.Render(text);
        Console.WriteLine(t);
    }
}
namespace Markdown;

public class Program
{
    public static void Main(string[] args)
    {
        var tagDictionary = new Dictionary<string?, TagType>
        {
            { "_", TagType.Italic },
            { "__", TagType.Bold },
            { "# ", TagType.Heading },
            { "## ", TagType.Heading },
            { "### ", TagType.Heading },
            { "* ", TagType.Bulleted},
        };
        const string text = "* # _text_\r\n";
        var md = new Md(tagDictionary);
        var newText = md.Render(text);
        Console.WriteLine(newText);
    }
}
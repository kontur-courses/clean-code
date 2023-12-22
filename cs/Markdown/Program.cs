namespace Markdown;

public class Program
{
    public static void Main(string[] args)
    {
        var tagDictionary = new Dictionary<string, TagType>
        {
            { "_", TagType.Italic },
            { "__", TagType.Bold },
            { "# ", TagType.Heading },
            { "## ", TagType.Heading },
            { "### ", TagType.Heading },
            { "* ", TagType.Bulleted},
            { "\n", TagType.LineBreaker },
            { "\r\n", TagType.LineBreaker }
        };
        const string text = " _а djdjd __ала__а_";
        var sut = new Md(tagDictionary);
        var t = sut.Render(text);
        Console.WriteLine(t);
    }
}
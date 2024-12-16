using Markdown.Markdown;

namespace Markdown;

class Program
{
    public static void Main()
    {
        var mdFile = File.ReadAllText("Markdown.md");
        var md = new Md();
        //write to file
        File.WriteAllText("Markdown.html", md.Render(mdFile));
    }
}
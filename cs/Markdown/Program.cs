namespace Markdown;

public class Program
{
    public static void Main(string[] args)
    {
        var md = new Md(new Parser());
        md.Render("_Hello world_");
    }
}
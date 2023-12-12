public class Program
{
    public static void Main(string[] args)
    {
        var text = "# Заголовок __с _разными_ символами__";
        var htmlText = Markdown.Markdown.Render(text);
        Console.WriteLine(htmlText);
    }
}
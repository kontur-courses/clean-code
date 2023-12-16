using MarkdownTask;
using System;
public static class Program
{
    public static void Main()
    {
        Markdown parser = new Markdown();
        string markdownText = "# This is a header\n_This is italic_\n __This is bold__\n this is text";
        string htmlText = parser.Render(markdownText);
        Console.WriteLine(htmlText);
    }
}

namespace Markdown;

public static class Program
{
    public static void Main()
    {
        var input = "# __TEST__";
        Console.WriteLine(Md.Render(input));
    }
}
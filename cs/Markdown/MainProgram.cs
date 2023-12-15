using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;

namespace Markdown;

public class MainProgram
{
    public static void Main(string[] args)
    {
        var text = Console.ReadLine();
        var sut = new Md();
        var parsedLine = sut.Render(text);
        Console.WriteLine(parsedLine);
    }
}
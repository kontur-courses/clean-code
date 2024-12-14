using System.Text;
using Markdown;

namespace MarkdownTests;

public class MdAcceptanceTest
{
    private const string TestInputFilename = "MdAcceptanceTest.txt";
    private const string TestOutputFilename = "MdAcceptanceResult.html";
    private static readonly string? TestDirectory = Directory.GetParent(".")?.Parent?.Parent?.FullName;
    private readonly string testInputPath = Path.Combine(
        TestDirectory ?? throw new InvalidOperationException(), TestInputFilename);
    private readonly string testOutputPath = Path.Combine(
        TestDirectory ?? throw new InvalidOperationException(), TestOutputFilename);
    
    [Test]
    public async Task Render_ShouldReturnCorrectHtml()
    {
        string html;
        await using (var fstream = new FileStream(testInputPath, FileMode.Open))
        {
            var buffer = new byte[fstream.Length];
            _ = await fstream.ReadAsync(buffer);
            var markdown = Encoding.UTF8.GetString(buffer);
            Console.WriteLine(markdown);

            html = Md.Render(markdown);
        }
        Console.WriteLine(html);
        await using (var fstream = new FileStream(testOutputPath, FileMode.Create))
        {
            Console.WriteLine(fstream.Name);
            var buffer = Encoding.UTF8.GetBytes(html);
            await fstream.WriteAsync(buffer);
        } 
    }
}
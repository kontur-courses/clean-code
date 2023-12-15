using Markdown;

namespace MarkdownTest;

public class ParserTest
{
    [Test]
    public void Test()
    {
        var lexer = new Lexer("__a _b_ c__ _d_ __e__");
        var parser = new Parser(lexer.GetTokens().ToArray());
        var t = parser.Parse();
        var r = 2;
        return;
    }
}
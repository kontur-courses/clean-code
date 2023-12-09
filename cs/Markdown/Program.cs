using Markdown.Lexer;
using Markdown.Tokens.Types;

namespace Markdown;

public static class Program
{
    public static void Main()
    {
        //Пример построения лексера
        var lexer = new LexerBuilder()
            .WithTokenType("_", new EmphasisToken())
            .WithTokenType("__", new StrongToken())
            .WithTokenType("# ", new HeaderToken())
            .WithTokenType(@"\", new EscapeToken())
            .Build();
    }
}
using Markdown.Tokens;

namespace Markdown.Transformers;

public static class StringTokenizer
{
    public static IReadOnlyList<Token> Tokenize(string text)
        => new Scanner(text).Run();

    private sealed class Scanner(string text)
    {
        private int _cursor = 0;
        private readonly List<Token> _tokens = [];

        public IReadOnlyList<Token> Run() => throw new NotImplementedException();
    }
}
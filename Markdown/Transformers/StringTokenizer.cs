using Markdown.Tokens;

namespace Markdown.Transformers;

public static class StringTokenizer
{
    public static IReadOnlyList<Token> Tokenize(string text)
        => new Scanner(text).Run();

    private sealed class Scanner(string text)
    {
        
        private readonly List<Token> _tokens = [];

        public IReadOnlyList<Token> Run()
        {
            var paragraphs = text.Split("\n");

            foreach (var line in paragraphs)
            {
                _tokens.Add(Token.Text(line));
            }


            return _tokens;
        }
    }
}
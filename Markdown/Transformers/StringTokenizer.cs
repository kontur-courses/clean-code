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
                var start = 0;

                if (line.StartsWith("# "))
                {
                    _tokens.Add(Token.HeaderMarker());
                    start = 2; 
                }

                for (var i = start; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        
                    }
                }
                
                
                _tokens.Add(Token.Text(line[start..]));
            }
            

            return _tokens;
        }
    }
}
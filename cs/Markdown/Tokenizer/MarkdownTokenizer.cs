using Markdown.Tokenizer.Scanners;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer;

public class MarkdownTokenizer
{
    private readonly ITokenScanner[] scanners = [
        new SpecScanner(), new NumberScanner(), new WordScanner()
    ];

    public List<Token> Tokenize(string markdown)
    {
        var begin = 0;
        var tokenList = new List<Token>();
        var memoryText = new Memory<char>(markdown.ToCharArray());
        
        while (begin < markdown.Length)
        {
            var textSlice = memoryText[begin..];
            
            var token = scanners
                .Select(sc => sc.Scan(textSlice))
                .First(token => token is not null);
            begin += token!.Length; tokenList.Add(token);
        }
        return tokenList;
    }
}
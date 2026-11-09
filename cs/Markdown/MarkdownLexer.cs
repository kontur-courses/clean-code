using System.Text;

namespace Markdown;

public class MarkdownLexer : ILexer
{
    private readonly Dictionary<string, TokenType> tokenSymbols = new()
    {
        ["__"] = TokenType.BoldSymbol,
        ["_"] = TokenType.ItalicSymbol,
        ["#"] = TokenType.Header,
        [" "] = TokenType.Space
    };
    
    public IEnumerable<Token> GetTokens(string text)
    {
        var lines = text.Split(Environment.NewLine);
        var result = new List<Token>();
        for (var i = 0; i< lines.Length; i++)
        {
            var line = lines[i];
            var index = 0;
            var word = new StringBuilder();
            while (index < line.Length)
            {
                SkipExtraSpaces(line, ref index);

                if (line[index] == '\\' )
                {
                    var typeNextToken = GetTypeNextToken(line, index);
                    if (typeNextToken != TokenType.Word)
                    {
                        word.Append(line[index + 1]);
                        index += 2;
                        continue;
                    }
                }
                
                var (tokenType, length) = ScanFragment(line, index);

                if (tokenType != TokenType.Word)
                {
                    if (word.Length > 0)
                    {
                        result.Add(new Token(TokenType.Word, word.ToString()));
                        word.Clear();
                    }
                    
                    result.Add(new Token(tokenType));
                }
                
                else word.Append(line[index]);
                
                index += length;
            }
            
            if (word.Length > 0) result.Add(new Token(TokenType.Word, word.ToString()));
            
            if (i != lines.Length - 1) 
                result.Add(new Token(TokenType.NewLine));
        }

        return result;
    }

    private void SkipExtraSpaces(string line, ref int index)
    {
        while (index < line.Length - 1 && line[index] == ' ' && line[index + 1] == ' ')
            index ++;
    }

    private (TokenType tokenType, int length) ScanFragment(string line, int index)
    {
        foreach (var key in tokenSymbols.Keys)
        {
            if (line[index..].StartsWith(key)) 
                return (tokenSymbols[key], key.Length);
        }

        return (TokenType.Word, 1);
    }
    
    private TokenType GetTypeNextToken(string line, int currentIndex)
    {
        if (currentIndex >= line.Length - 1) return TokenType.Word;
        currentIndex++;
        foreach (var key in tokenSymbols.Keys)
        {
            if (line[currentIndex..].StartsWith(key)) 
                return tokenSymbols[key];
        }

        return TokenType.Word;
    }
}

using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class TextScanner : ITokenScanner
{
    private readonly EscapingScanner escapingScanner = new();
    private readonly NewlineScanner newlineScanner = new();
    private readonly SimpleScanner simpleScanner = new();
    private readonly NumberScanner numberScanner = new();
    
    public bool TryGetToken(string markdown, int index, out Token? token)
    {
        token = default;
        
        try
        {
            var text = ExtractText(markdown, ref index);

            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            token = new Token(TypeOfToken.Word, text);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private string ExtractText(string markdown, ref int index)
    {
        var result = new List<char>();

        while (index < markdown.Length &&
               !escapingScanner.TryGetToken(markdown, index, out _) &&
               !newlineScanner.TryGetToken(markdown, index, out _) &&
               !simpleScanner.TryGetToken(markdown, index, out _) &&
               !numberScanner.TryGetToken(markdown, index, out _))
        {
            result.Add(markdown[index]);
            index++;
        }

        return new string(result.ToArray());
    }
}
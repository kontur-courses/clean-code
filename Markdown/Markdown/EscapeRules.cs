using System.Text;

namespace Markdown;

public static class EscapeRules
{
    public const char Character = '\\';

    public static bool IsNotEscaped(string text, int index)
    {
        return !text.IsInBound(index) || !text.IsInBound(index - 1) || text[index - 1] != Character || !IsNotEscaped(text, index - 1);
    }
    
    public static string RemoveEscapes(string line)
    {
        var sb = new StringBuilder();
        var length = line.Length;
        var newStart = 0;
        var tokensToCheck = TokenSelector.Tokens;
        
        for (var i = 0; i < length; i++)
        {
            var next = i + 1;
            if (line[i] != Character)
                continue;
            
            var isEscaped = !IsNotEscaped(line, i);
            if (i  > 0 && isEscaped)
            {
                var substrLength = next - newStart -1;
                if (substrLength == 0)
                    continue;
                sb.Append(line.AsSpan(newStart, substrLength));
                newStart = next;
            }
            else if (!isEscaped && i == line.Length - 1)
                continue;

            if (tokensToCheck.Any(token => token.Opening.IsSubstringAt(line, next) || token.Ending.IsSubstringAt(line, next)))
            {
                sb.Append(line.AsSpan(newStart, i - newStart));
                newStart = next;
            }
        }
        if (newStart != line.Length)
            sb.Append(line.AsSpan(newStart, line.Length - newStart));
        return sb.ToString();
    }
}
using Markdown.Tokens;
using Markdown.Tokens.Tags;

namespace Markdown.Parsers;

public class BoldParser : IParser
{
    public bool CanParse(char symbol, string text, int index)
    {
        if (symbol != '_')
            return false;
        
        if (index + 1 >= text.Length || text[index + 1] != '_') 
            return false;
        
        if (index + 2 >= text.Length)
            return false;
        
        if (IsEscaped(text, index))
            return false;
        
        return true;
    }

    public Token Parse(string text, int index)
    {
        var closeIndex = text.IndexOf("__", index + 2, StringComparison.Ordinal);
        
        while (closeIndex != -1 && IsEscaped(text, closeIndex))
            closeIndex = text.IndexOf("__", closeIndex + 2, StringComparison.Ordinal);

        if (!CheckForCloseSymbol(closeIndex, index, text))
            return GetSimpleTextToken(index);

        var innerPart = text.Substring(index + 2, closeIndex - (index + 2));
        
        if (innerPart.Length == 0)
            return GetSimpleTextToken(index);
        
        if (char.IsWhiteSpace(innerPart[^1]))
            return GetSimpleTextToken(index);
        
        if (innerPart.All(char.IsDigit))
            return GetSimpleTextToken(index);
        
        if (!CheckIfInsideWord(closeIndex, index, text, innerPart))
            return GetSimpleTextToken(index);

        if (HasUnclosedSingle(innerPart))
            return GetSimpleTextToken(index);
        
        return new Token(new DoubleUnderscoreTag(), innerPart, closeIndex + 1);
    }

    private bool CheckIfInsideWord(int closeIndex, int textIndex, string text, string innerPart)
    {
        var openInsideWord = textIndex - 1 >= 0 && char.IsLetterOrDigit(text[textIndex - 1]);
        var closeInsideWord = closeIndex + 2 < text.Length && char.IsLetterOrDigit(text[closeIndex + 2]);
        var insideWord = openInsideWord || closeInsideWord;

        if (insideWord && innerPart.Any(char.IsWhiteSpace))
            return false;
        return true;
    }
    
    private bool CheckForCloseSymbol(int closeIndex, int textIndex, string text)
    {
        if (closeIndex == -1)
            return false;

        if (char.IsWhiteSpace(text[textIndex + 2]))
            return false;

        return true;
    }
    
    private Token GetSimpleTextToken(int index)
    {
        return new Token(new TextTag(), "__", index + 1);
    }
    
    private bool HasUnclosedSingle(string innerPart)
    {
        var count = 0;
        for (var i = 0; i < innerPart.Length - 1; i++)
        {
            if (innerPart[i] != '_' || innerPart[i + 1] == '_')
                continue;
            count++;
        }
        
        return count % 2 == 1;
    }
    
    private bool IsEscaped(string text, int pos)
    {
        var backSlashes = 0;
        for (var i = pos - 1; i >= 0; i--)
        {
            if (text[i] != '\\')
                break;
            backSlashes++;
        }

        return backSlashes % 2 == 1;
    }
}
using Markdown.Tokens;
using Markdown.Tokens.Tags;

namespace Markdown.Parsers;

public class ItalicsParser : IParser
{
    public bool CanParse(char symbol, string text, int index)
    {
        if (symbol != '_')
            return false;
        
        if (index + 1 >= text.Length) 
            return false;
        
        if (text[index + 1] == '_')
            return false;

        if (IsEscaped(text, index))
            return false;
        
        return true;
    }

    public Token Parse(string text, int index)
    {
        var closeIndex = FindSingleClosingUnderscoreIndex(text, index + 1);

        if (!CheckForCloseSymbol(closeIndex, index, text))
            return GetSimpleTextToken(index);
        
        var innerPart = text.Substring(index + 1, closeIndex - index - 1);
        
        if (innerPart.Length == 0)
            return GetSimpleTextToken(index);
        
        if (char.IsWhiteSpace(innerPart[^1]))
            return GetSimpleTextToken(index);
        
        if (innerPart.All(char.IsDigit))
            return GetSimpleTextToken(index);
        
        if (!CheckIfInsideWord(closeIndex, index, text, innerPart))
            return GetSimpleTextToken(index);
        
        if (HasUnclosedDouble(innerPart))
            return GetSimpleTextToken(index);
        
        return new Token(new UnderscoreTag(), innerPart, closeIndex);
    }

    private bool CheckIfInsideWord(int closeIndex, int textIndex, string text, string innerPart)
    {
        var openInsideWord = textIndex - 1 >= 0 && char.IsLetterOrDigit(text[textIndex - 1]);
        var closeInsideWord = closeIndex + 1 < text.Length && char.IsLetterOrDigit(text[closeIndex + 1]);
        var insideWord = openInsideWord || closeInsideWord;

        if (insideWord && innerPart.Any(char.IsWhiteSpace))
            return false;
        return true;
    }

    private bool CheckForCloseSymbol(int closeIndex, int textIndex, string text)
    {
        if (closeIndex == -1)
            return false;
        
        if (char.IsWhiteSpace(text[textIndex + 1]))
            return false;
        
        return true;
    }

    private Token GetSimpleTextToken(int index)
    {
        return new Token(new TextTag(), "_", index);
    }
    
    private bool HasUnclosedDouble(string innerPart)
    {
        var count = 0;
        for (var i = 0; i < innerPart.Length - 1; i++)
        {
            if (innerPart[i] != '_' || innerPart[i + 1] != '_')
                continue;
            count++;
            i++;
        }
        
        return count % 2 == 1;
    }
    
    private int FindSingleClosingUnderscoreIndex(string text, int start)
    {
        var position = text.IndexOf('_', start);
        while (position != -1)
        {
            if (IsEscaped(text, position))
            {
                start = position + 1;
                position = text.IndexOf('_', start);
                continue;
            }
            
            if (position + 1 >= text.Length || text[position + 1] != '_')
                return position;
            
            start = position + 2;
            position = text.IndexOf('_', start);
        }
        return -1;
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
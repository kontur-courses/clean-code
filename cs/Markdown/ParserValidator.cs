namespace Markdown;
using Markdown.Data;

public class ParserValidator
{
    private string _text;

    public ParserValidator(string input)
    {
        _text = input;
    }
    
    public bool IsMarkCorrect(int startIndex, bool isOpening, int markLength = 1)
    {
        var isScreened = IsScreened(startIndex);
        if (isOpening)
        {
            return !isScreened 
                   && startIndex + markLength < _text.Length 
                   && !Char.IsWhiteSpace(_text[startIndex + markLength]);
        }
        return !isScreened 
               && startIndex > 0
               && !Char.IsWhiteSpace(_text[startIndex - 1]);
    }

    public bool IsScreened(int index)
    {
        return (index > 0 && _text[index - 1] == '\\') 
            && (index > 1 && _text[index - 2] != '\\' || index == 1);
    }

    public bool IsDoubleUnderscore(int index)
    {
        return index < _text.Length - 1 && _text[index + 1] == '_'
               || index > 0 && _text[index - 1] == '_';
    }

    public bool IsContentAcceptable(string content, string mark)
    {
        return !string.IsNullOrEmpty(content) && (HasNoDigits(content) || mark == Marks.Header || mark == Marks.List);
    }

    public bool IsSplittingWords(int start, int end)
    {
        return start > 0 && _text[start - 1] != ' '
                && end < _text.Length - 1 &&  _text[end + 1] != ' '
                && _text.Substring(start, end - start).Any(Char.IsWhiteSpace)
                && _text[end] != '\n';
    }
    
    private bool HasNoDigits(string content)
    {
        foreach (char c in content)
        {
            if (char.IsDigit(c))
                return false;
        }
        return true;
    }
    
    
}
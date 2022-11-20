using System.Text;

namespace Markdown;

public class ReplaceTagInspector
{
    private StringBuilder _text;
    private int _index;
    private string _currentSymbol;
    private Dictionary<string, bool> _needCloseTag;
    
    public bool CanReplaceTag(StringBuilder text, int index, string currentSymbol, Dictionary<string, bool> needCloseTag)
    {
        _text = text;
        _index = index;
        _needCloseTag = needCloseTag;
        _currentSymbol = currentSymbol;

        if (IsNumericWord() 
            || IsReplaceInDifferentWords() 
            || IsLonelySymbol() 
            || IsIncorrectCloseTagSymbol() 
            || IsIncorrectOpenTagSymbol()
            || IsIntersectsReplaceSymbols()
            || IsInReplaceSymbolsEmptyString())
            return false;

        return true;
    }

    private bool IsIncorrectOpenTagSymbol()
    {
        return !_needCloseTag[_currentSymbol] && _text[_index + 1].Equals(' ');
    }
    
    private bool IsIncorrectCloseTagSymbol()
    {
        return _needCloseTag[_currentSymbol] && _text[_index - 1].Equals(' '); 
    }

    private bool IsIntersectsReplaceSymbols()
    {
        return false;
    }

    private bool IsInReplaceSymbolsEmptyString()
    {
        return false;
    }
    
    private bool IsReplaceInDifferentWords()
    {
        return false;
    }

    private bool IsLonelySymbol()
    {
        return false;
    }

    private bool IsDoubleUnderLineInSingleUnderline(int hasNeighboors)
    {
        return _needCloseTag["_"] && hasNeighboors != 0;
    }

    private bool IsNumericWord()
    {
        var isNumeric = false;
        for (var i = _index; i < _text.Length - 1; i++)
        {
            if (_text[i].Equals(' '))
                return false;
            if (Char.IsNumber(_text[i]))
                return true;
        }
        return false;
    }
}


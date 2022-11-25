<<<<<<< HEAD
﻿namespace Markdown;

public class ReplaceTagInspector
{
    private ReplaceTagInspectorConfig _config;
    
    public bool CanReplaceTag(ReplaceTagInspectorConfig config)
    {
        _config = config;

        var customConditionsResult = config.CustomConditions.Any(condition => condition(config.Index));
        
        return !(IsNumericWord() 
            || IsIncorrectCloseTagSymbol(_config.NeedClosure, _config.Index) 
            || IsIncorrectOpenTagSymbol()
            || IsEmptyStringInReplaceSymbols()
            || customConditionsResult);
    }

    private bool IsIncorrectOpenTagSymbol()
    {
        return !_config.NeedClosure && _config.Text[_config.Index + 1].Equals(' ');
    }
    
    private bool IsIncorrectCloseTagSymbol(bool needClosure, int index)
    {
        return needClosure && _config.Text[index - 1].Equals(' '); 
    }

    private bool IsEmptyStringInReplaceSymbols()
    {
        var currentSymbol = _config.TagConfiguration.Symbol;
        var currentIndex = _config.Index;

        if (currentIndex + 1 < _config.Text.Length - 1)
        {
            var indexWithOffset = currentIndex + _config.TagConfiguration.Symbol.Length - 1;
            
            var firstCondition = currentIndex + 3 < _config.Text.Length &&
                                 currentSymbol.Equals("" + _config.Text[indexWithOffset + 1] +
                                                      _config.Text[indexWithOffset + 2]);
            var secondCondition = currentSymbol.Equals("" + _config.Text[indexWithOffset + 1]) && !_config.NeedClosure;
            
            if (firstCondition || secondCondition)
            {
                return true;
            } 
        }
            
        return false;
    }

    private bool IsNumericWord()
    {
        for (var i = _config.Index; i < _config.Text.Length - 1; i++)
        {
            if (_config.Text[i].Equals(' ') || _config.Text[i].Equals('\\'))
                return false;
            if (Char.IsNumber(_config.Text[i]))
                return true;
        }
        return false;
    }
}

=======
﻿using System.Text;

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

>>>>>>> origin/master

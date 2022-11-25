namespace Markdown;

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


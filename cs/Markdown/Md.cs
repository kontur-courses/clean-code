using System.Text;

namespace Markdown;

public class Md
{
    private StringBuilder _html;
    private ReplaceTagInspector _inspector;
    private Dictionary<string, bool> _needCloseTag = new Dictionary<string, bool>()
    {
        { "_", false },
        { "__", false }
    };
    private HashSet<string> _symbolsToReplace = new HashSet<string>() { "__", "_" };
    private Dictionary<string, string> _symbolsOpenTagPairs = new Dictionary<string, string>()
    {
        { "#", TagNames.TitleOpen },
        { "_", TagNames.ItalicOpen },
        { "__", TagNames.BoldOpen }
    };
    
    private Dictionary<string, string> _symbolsCloseTagPairs = new Dictionary<string, string>()
    {
        { "#", TagNames.TitleClose },
        { "_", TagNames.ItalicClose },
        { "__", TagNames.BoldClose }
    };
    
    private string _currentSymbol;

    public Md()
    {
        _html = new StringBuilder();
        _inspector = new ReplaceTagInspector();
    }
    
    public string Render(string markdown)
    {
        _html.Append(markdown);
        _html.Append(" ");

        for (var i = 0; i < _html.Length - 1; i++)
        {
            DefineCurrentReplaceSymbol(i);
            
            if (_symbolsToReplace.Contains(_currentSymbol) && _inspector.CanReplaceTag(_html, i, _currentSymbol, _needCloseTag))
            {
                if (_needCloseTag["_"] && HasEqualNeighbors(i) != 0)
                    continue;
                
                var tagToReplace = _needCloseTag[_currentSymbol]
                    ? _symbolsCloseTagPairs[_currentSymbol]
                    : _symbolsOpenTagPairs[_currentSymbol];
                
                ReplaceSymbol(i, tagToReplace);
                i += tagToReplace.Length - 1;
            }
        }

        _html.Remove(_html.Length - 1, 1);
        return _html.ToString();
    }

    private void ReplaceSymbol(int index, string replaceTag)
    {
        _html.Remove(index, _currentSymbol.Length); 
        _html.Insert(index, replaceTag); 
        ChangeNeedCloseTagFlag();
    } 
    
    private void DefineCurrentReplaceSymbol(int index) { 
        _currentSymbol = "" + _html[index];
        if (HasEqualNeighbors(index) != 0)
            _currentSymbol += _html[index];
    }

    private int HasEqualNeighbors(int index)
    {
        for (var i = -1; i <= 1; i += 2)
        {
            if (index + i < 0 || i == 0) continue;
            if (_html[index + i].Equals(_html[index]))
                return i;
        }

        return 0;
    }
    
    private void ChangeNeedCloseTagFlag()
    {
        _needCloseTag[_currentSymbol] = !_needCloseTag[_currentSymbol];
    }
    
}
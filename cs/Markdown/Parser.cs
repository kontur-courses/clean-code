using System.Text;

namespace Markdown;

public class Parser
{
    private readonly HashSet<string> _doubleTags = new();

    private readonly Stack<(string, int)> _memory = new();
    private readonly HashSet<string> _specialSymbols = new();

    private readonly Dictionary<string, string> _symbToTag = new();
    private readonly Dictionary<string, string> _tagToSymb = new();

    private bool _hasOpened;

    public Parser()
    {
        _doubleTags.Add("strong");
        _doubleTags.Add("em");

        _specialSymbols.Add("/");
        _specialSymbols.Add("_");
        _specialSymbols.Add("__");
        _specialSymbols.Add("#");

        _symbToTag["_"] = "em";
        _symbToTag["__"] = "strong";

        _tagToSymb["em"] = "_";
        _tagToSymb["strong"] = "__";
    }

    public string Parse(string text)
    {
        _hasOpened = false;
        var result = new StringBuilder();
        var offset = 1;
        for (var i = 0; i < text.Length; i += offset)
        {
            var symbol = DetermineSymbol(text, i);
            offset = symbol.Length;
            if (_symbToTag.TryGetValue(symbol, out var tag))
                result.Append(GetFormedTag(tag, i, result.Length));

            else
                switch (symbol)
                {
                    case "#":
                        result.Append("<h1>");
                        _hasOpened = true;
                        break;
                    case "/":
                    {
                        var next = text[i + 1].ToString();

                        if (_specialSymbols.Contains(next))
                            result.Append(next);
                        else
                            result.Append("/" + next);

                        i += 1;
                        break;
                    }
                    case " ":
                    {
                        result.Append(' ');
                        if (_memory.Count == 0)
                            continue;

                        var tagData = _memory.Pop();
                        result.ReplaceAt(tagData.Item2, tagData.Item1.Length + 2, _tagToSymb[tagData.Item1]);
                        break;
                    }
                    default:
                        result.Append(symbol);
                        break;
                }
        }

        while (_memory.Count != 0)
        {
            var tagData = _memory.Pop();
            result.ReplaceAt(tagData.Item2, tagData.Item1.Length + 2, _tagToSymb[tagData.Item1]);
        }

        if (_hasOpened)
            result.Append("</h1>");

        return result.ToString();
    }

    private string DetermineSymbol(string text, int index)
    {
        switch (text[index])
        {
            case '_':
            {
                if (index + 1 >= text.Length || !text[index + 1].Equals('_'))
                    return "_";
                return "__";
            }
            default: return text[index].ToString();
        }
    }

    private string GetFormedTag(string tag, int index, int resultHeader)
    {
        if (_memory.Count != 0)
            if (_memory.Peek().Item1 == tag)
            {
                if (_memory.Peek().Item2 + _tagToSymb[tag].Length == index)
                    return _tagToSymb[tag];

                _memory.Pop();
                return $"</{tag}>";
            }

        _memory.Push((tag, resultHeader));
        return $"<{tag}>";
    }
}
using System.Text;
using Markdown.Extensions;

namespace Markdown
{
    /// <summary>
    /// Класс, который в зависимости от типа символа будет выбирать тот или иной парсеры
    /// </summary>
    public class ParsersHandler
    {
        // Везде string а не char, потому что пока не придумал, как адекватно __ обрабатывать
        private SymbolType _curSymbolType;
        private readonly Dictionary<string, SymbolType> _states;
        private readonly Dictionary<SymbolType, string> _tags;
        private readonly HashSet<string> _specialSymbols;
        private int _curIndex;

        public ParsersHandler()
        {
            _curSymbolType = SymbolType.Default;

            _states = new();
            _states["_"] = SymbolType.Underscore;
            _states["#"] = SymbolType.HashSymbol;
            _states["/"] = SymbolType.Slash;

            _tags = new();
            _tags[SymbolType.Underscore] = "em";
            _tags[SymbolType.HashSymbol] = "h1";
            _tags[SymbolType.DoubleUnderscore] = "strong";

            _specialSymbols = new();
            _specialSymbols.Add("_");
            _specialSymbols.Add("#");
            _specialSymbols.Add("/");
        }
        
        public string Handle(string text)
        {
            var result = new StringBuilder();
            foreach (var part in GetNextTextPart(text))
                result.Append(part);
            return result.ToString();
        }

        private IEnumerable<StringBuilder> GetNextTextPart(string text)
        {
            _curIndex = 0;
            while (_curIndex < text.Length)
            {
                CheckState(text);
                yield return ParsePart(text);
            }
        }

        private void CheckState(string text)
        {
            var curSymb = text[_curIndex];
            _curSymbolType = _states.GetOrDefault(curSymb.ToString(), SymbolType.Default);
        }

        private StringBuilder ParsePart(string text)
        {
            var result = new StringBuilder();
            var tag = _tags.GetOrDefault(_curSymbolType, text[_curIndex].ToString());

            if (_curSymbolType != SymbolType.Default)
            {
                result.Append($"<{tag}>");
                _curIndex++;
            }
            
            while (_curIndex < text.Length && !text[_curIndex].Equals((char)_curSymbolType) && !_specialSymbols.Contains(text[_curIndex].ToString())) 
            { 
                result.Append(text[_curIndex]);
                _curIndex++;
            }

            if (_curSymbolType != SymbolType.Default)
            {
                result.Append($"</{tag}>");
                _curIndex++;
            }
            
            return result;
        }
    }
}


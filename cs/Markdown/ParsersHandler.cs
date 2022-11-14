namespace Markdown
{
    /// <summary>
    /// Класс, который в зависимости от типа символа будет выбирать тот или иной парсеры
    /// </summary>
    public class ParsersHandler
    {
        private SymbolType _curSymbolType;
        private readonly  Dictionary<SymbolType, IParser> _parsers;
        
        public ParsersHandler()
        {
            _curSymbolType = SymbolType.Default;
            
            _parsers[SymbolType.Underscore] = new UnderscoreParser();
            _parsers[SymbolType.DoubleUnderscore] = new DoubleUnderscoreParser();
            _parsers[SymbolType.Slash] = new SlashParser();
            _parsers[SymbolType.HashSymbol] = new HashSymbolParser();
        }
        
        public string Handle(string text)
        {
            throw new NotImplementedException();
        }

        private SymbolType ChangeStateForSymbol(string text)
        {
            throw new NotImplementedException();
        }
    }
}


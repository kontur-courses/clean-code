using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlTokenAnalyzer
    {
        private readonly LinkedList<IToken> _tokens = new LinkedList<IToken>();

        private StringBuilder _currentBuilder = new StringBuilder();

        public string AnalyzeLine(string line)
        {

            for (var i = 0; i < line.Length; i++)
            {
                var currentSymbol = line[i];
                var parsersTable = ParsersStorage.ParsersTable;
                var currentAndNextSymbols = (i< line.Length - 1) ? currentSymbol.ToString() +line[i+1] : null; 
                IToken token;
                StringBuilder builder;
                
                if (currentAndNextSymbols != null && parsersTable.ContainsKey(currentAndNextSymbols))
                {
                    token = parsersTable[currentAndNextSymbols]
                        .TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);
                    builder = _currentBuilder;
                }
                else if (parsersTable.ContainsKey(currentSymbol.ToString()))
                {
                    token = parsersTable[currentSymbol.ToString()]
                        .TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);
                    builder = (currentSymbol == '\\') ? null : _currentBuilder;
                }
                else
                {
                    _currentBuilder.Append(currentSymbol);
                    continue;
                }
                if (token == null)
                    continue;
                AddWordToken(builder);
                AddToken(token);
            }
            AddWordToken(_currentBuilder);
            return MakeHtml();
        }

        private void AddToken(IToken token)
        {
            if (token is ITag tag)
            {
                if (_tokens.Count > 1 && _tokens.Last.Value is TokenSpace ||
                    _tokens.Count == 0)
                    tag.IsAtTheBeginning = true;
                _tokens.AddLast(token);
                var currentToken = _tokens.Last.Previous;
                tag.FindPairToken(currentToken);
            }
            else
            {
                _tokens.AddLast(token);
            }
        }

        private void AddWordToken(StringBuilder word)
        {
            if (word == null || word.Length == 0)
                return;

            var wordToken = new TokenWord(word.ToString());
            _tokens.AddLast(wordToken);
            _currentBuilder = new StringBuilder();
        }

        private string MakeHtml()
        {
            return string.Concat(_tokens.Select(token =>
            {
                if (token is ITag tag && tag.IsClosed)
                    return tag.HtmlTagAnalog;
                return token.Content;
            }));
        }

        public static void MakePair(ITag opener, ITag endTag)
        {
            opener.IsClosed = true;
            opener.IsStartTag = true;
            endTag.IsClosed = true;
        }
    }
}

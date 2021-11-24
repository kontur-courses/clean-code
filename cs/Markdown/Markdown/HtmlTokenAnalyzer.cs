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
                var parsersList = ParsersStorage.ParserList;
                IToken token = null;

                foreach (var parser in parsersList)
                {
                    token = parser.TryGetToken(ref i, ref _currentBuilder, ref line);
                    if (token != null)
                        break;
                }

                if (token == null)
                {
                    _currentBuilder.Append(currentSymbol);
                    continue;
                }
                AddWordToken(_currentBuilder);
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

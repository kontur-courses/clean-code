using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parsers;

namespace Markdown
{
    public class HtmlTokenAnalyzer
    {
        private readonly LinkedList<IToken> _tokens = new LinkedList<IToken>();

        private StringBuilder _currentBuilder = new StringBuilder();

        public string AnalyzeLine(string line)
        {

            for (int i = 0; i < line.Length; i++)
            {
                var currentSymbol = line[i];
                var parsersTable = ParsersStorage.ParsersTable;
                var currentAndNextSymbols = (i< line.Length - 1) ? currentSymbol.ToString() +line[i+1] : null; 
                IToken token;

                
                if (currentAndNextSymbols != null && parsersTable.ContainsKey(currentAndNextSymbols))
                {
                    token = parsersTable[currentAndNextSymbols]
                        .TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);
                    AddWordToken(_currentBuilder);
                    AddToken(token);
                }
                else if (parsersTable.ContainsKey(currentSymbol.ToString()))
                {
                    token = parsersTable[currentSymbol.ToString()]
                        .TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);
                    if (token == null)
                        continue;
                    if (currentSymbol == '\\')
                        AddWordToken(null);
                    else
                        AddWordToken(_currentBuilder);
                    AddToken(token);
                }
                else
                {
                    _currentBuilder.Append(currentSymbol);
                    //continue;
                }
                


                
                /*
                switch (currentSymbol)
                {
                    case '\\':
                        token = new ShieldingTagParser().TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);

                        AddWordToken(null);
                        AddToken(token);
                        break;

                    case '_':
                        if (i != line.Length - 1 && line[i + 1] == '_')
                        {
                            token = new DoubleUnderliningParser().TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);

                            AddWordToken(_currentBuilder);
                            AddToken(token);
                        }
                        else
                        {
                            token = new SingleUnderliningParser().TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);
                            if (token == null)
                                break;

                            AddWordToken(_currentBuilder);
                            AddToken(token);
                        }
                        break;

                    case '#':
                        token = new HeaderParser().TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);

                        AddWordToken(_currentBuilder);
                        AddToken(token);
                        break;

                    case ' ':
                        token = new SpaceParser().TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);

                        AddWordToken(_currentBuilder);
                        AddToken(token);
                        break;


                    case '[':
                        token = new StartLinkParser().TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);

                        AddWordToken(_currentBuilder);
                        AddToken(token);
                        break;

                    case ']':
                        token = new EndLinkParser().TryGetToken(ref i, ref _currentBuilder, ref line, currentSymbol);

                        AddWordToken(_currentBuilder);
                        AddToken(token);
                        break;

                    default:
                        _currentBuilder.Append(currentSymbol);
                        break;
                }
                */
                
            }
            AddWordToken(_currentBuilder);
            return MakeHtml();
        }

        private void AddToken(IToken token)
        {
            if (token == null)
                return;

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
            if (word == null)
                return;

            if (word.Length == 0)
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

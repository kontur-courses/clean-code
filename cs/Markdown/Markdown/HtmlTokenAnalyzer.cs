using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parsers;

namespace Markdown
{
    public class HtmlTokenAnalyzer
    {
        private readonly LinkedList<IToken> _tokens = new LinkedList<IToken>();

        private readonly HashSet<char> _specialSymbols = new HashSet<char> { '_', '#', '[', ']' };

        private StringBuilder _currentBuilder = new StringBuilder();

        public string AnalyzeLine(string line)
        {
            //var currentBuilder = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                var currentSymbol = line[i];
                switch (currentSymbol)
                {
                    case '\\':
                        if (_specialSymbols.Contains(line[i + 1]) ||
                            line[i + 1] == '\\')
                        {
                            _currentBuilder.Append(line[i + 1]);
                            i++;
                        }
                        else
                        {
                            _currentBuilder.Append(line[i]);
                        }
                        break;

                    case '_':
                        //ITag token;
                        if (i != line.Length - 1 && line[i + 1] == '_')
                        {
                            
                            var token = new DoubleUnderliningParser().TryGetToken(ref i);

                            AddWordToken(_currentBuilder);
                            AddToken(token);
                            //token = new TagBold();

                            //i++;

                        }
                        else
                        {
                            var token = new SingleUnderliningParser().TryGetToken(i, line, _currentBuilder, currentSymbol);

                            if (token == null)
                                break;

                            AddWordToken(_currentBuilder);
                            AddToken(token);
                            


                            /*

                            if (i < line.Length - 1 && char.IsDigit(line[i + 1]) ||
                                i > 0 && char.IsDigit(line[i - 1]))
                            {
                                _currentBuilder.Append(currentSymbol);
                                break;
                            }

                            */

                            //token = new TagItalic();
                        }
                        //AddWordToken(_currentBuilder);
                        //_currentBuilder = new StringBuilder();

                        //AddToken(token);

                        break;

                    case '#':
                        AddWordToken(_currentBuilder);
                        //_currentBuilder = new StringBuilder();

                        AddToken(new HeaderParser().TryGetToken());
                        //AddToken(new TagHeader());

                        break;

                    case ' ':
                        AddWordToken(_currentBuilder);
                        //_currentBuilder = new StringBuilder();

                        AddToken(new SpaceParser().TryGetToken());
                        //AddSpaceToken();

                        break;


                    case '[':
                        AddWordToken(_currentBuilder);
                        //_currentBuilder = new StringBuilder();

                        AddToken(new StartLinkParser().TryGetToken());
                        //AddToken(new TagLink(null));

                        break;

                    case ']':
                        AddWordToken(_currentBuilder);
                        //_currentBuilder = new StringBuilder();

                        /*
                        var substring = line.Substring(i + 1, line.Length - i-1);
                        string address;
                        if (line[i + 1] == '(' && substring.Contains(')'))
                        {

                            var start = substring.IndexOf('(');
                            var finish = substring.IndexOf(')');
                            address = line.Substring(i + start + 2, finish -1 - start);
                            line = line.Remove(start, finish - start + 1);
                        }
                        else
                        {
                            address = null;
                        }
                        */
                        AddToken(new EndLinkParser().TryGetToken(ref line, i));
                        //AddToken(new TagLink(address));

                        break;

                    default:
                        _currentBuilder.Append(currentSymbol);
                        break;
                }
            }
            AddWordToken(_currentBuilder);
            return MakeHtml();
        }

        private void AddSpaceToken()
        {
            _tokens.AddLast(new TokenSpace());
        }

        private void AddToken(IToken token)
        {
            //var tokenAsTag = token as ITag;
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

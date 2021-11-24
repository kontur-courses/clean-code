using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlTokenAnalyzer
    {
        private readonly LinkedList<IToken> _tokens = new LinkedList<IToken>();

        private readonly HashSet<char> _specialSymbols = new HashSet<char> { '_', '#', '[', ']' };

        public string AnalyzeLine(string line)
        {
            var currentBuilder = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                var currentSymbol = line[i];
                switch (currentSymbol)
                {
                    case '\\':
                        if (_specialSymbols.Contains(line[i + 1]) ||
                            line[i + 1] == '\\')
                        {
                            currentBuilder.Append(line[i + 1]);
                            i++;
                        }
                        else
                        {
                            currentBuilder.Append(line[i]);
                        }
                        break;

                    case '_':
                        ITag token;
                        if (i != line.Length - 1 && line[i + 1] == '_')
                        {
                            token = new TagBold();
                            i++;
                        }
                        else
                        {

                            if (i < line.Length - 1 && char.IsDigit(line[i + 1]) ||
                                i > 0 && char.IsDigit(line[i - 1]))
                            {
                                currentBuilder.Append(currentSymbol);
                                break;
                            }

                            token = new TagItalic();
                        }
                        AddWordToken(currentBuilder);
                        currentBuilder = new StringBuilder();
                        AddToken(token);
                        break;

                    case '#':
                        AddWordToken(currentBuilder);
                        currentBuilder = new StringBuilder();

                        AddToken(new HeaderParser().TryGetToken());
                        //AddToken(new TagHeader());

                        break;

                    case ' ':
                        AddWordToken(currentBuilder);
                        currentBuilder = new StringBuilder();

                        AddToken(new SpaceParser().TryGetToken());
                        //AddSpaceToken();

                        break;


                    case '[':
                        AddWordToken(currentBuilder);
                        currentBuilder = new StringBuilder();
                        AddToken(new TagLink(null));
                        break;

                    case ']':
                        AddWordToken(currentBuilder);
                        currentBuilder = new StringBuilder();
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
                        AddToken(new TagLink(address));
                        break;

                    default:
                        currentBuilder.Append(currentSymbol);
                        break;
                }
            }
            AddWordToken(currentBuilder);
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

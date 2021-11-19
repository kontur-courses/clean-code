using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlTokenAnalyzer
    {
        private readonly LinkedList<IToken> _tokens = new LinkedList<IToken>();

        private readonly HashSet<char> _specialSymbols = new HashSet<char> {'_'};

        public string AnalyzeLine(string line)
        {
            var currentBuilder = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                var symbol = line[i];
                switch (symbol)
                {
                    case '\\':
                        if (_specialSymbols.Contains(line[i + 1]) || line[i + 1] == '\\')
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

                        

                        if (i != line.Length - 1 && line[i + 1] == '_')
                        {
                            AddWordToken(currentBuilder);
                            currentBuilder = new StringBuilder();
                            AddBoldToken();
                            i++;
                        }

                        else
                        {
                            if (i < line.Length - 1 && char.IsDigit(line[i + 1]) || i > 0 && char.IsDigit(line[i - 1]))
                            {
                                currentBuilder.Append(symbol);
                                break;
                            }
                            AddWordToken(currentBuilder);
                            currentBuilder = new StringBuilder();
                            AddItalicToken();
                        }
                        break;

                    case ' ':

                        AddWordToken(currentBuilder);
                        currentBuilder = new StringBuilder();

                        AddSpaceToken();
                        break;

                    default:
                        currentBuilder.Append(symbol);
                        break;
                }
            }
            AddWordToken(currentBuilder);
            return MakeHtml();
        }

        private void AddSpaceToken()
        {
            _tokens.AddLast(new TagSpace());
        }

        private void AddItalicToken()
        {
            var italicToken = new TagItalic();

            if (_tokens.Count > 0 && _tokens.Last.Value is TagSpace || _tokens.Count == 0)
                italicToken.IsAtTheBeginning = true;

            _tokens.AddLast(italicToken);
            var currentToken = _tokens.Last.Previous;

            var spacesCnt = 0;
            var boldsCnt = 0;
            var onlyEmptyStrings = true;
            while (currentToken != null)
            {
                if (currentToken.Value is TagSpace)
                    spacesCnt++;
                if (currentToken.Value is TagBold)
                    boldsCnt++;
                if (currentToken.Value is TagBold && currentToken.Value.IsPrevent && boldsCnt%2!=0)
                    break;
                if (currentToken.Value is TagItalic && !currentToken.Value.IsPrevent)
                {
                    var starter = currentToken.Value as ITag;
                    if ((!starter.IsAtTheBeginning && spacesCnt == 0 || starter.IsAtTheBeginning && !italicToken.IsAtTheBeginning) && !onlyEmptyStrings)
                        MakePair(starter, italicToken);
                }
                if (!(currentToken.Value is TagWord && currentToken.Value.Content.Length == 0))
                    onlyEmptyStrings = false;
                currentToken = currentToken.Previous;
            }
        }

        private void AddBoldToken()
        {
            var boldToken = new TagBold();

            if (_tokens.Count > 1 && _tokens.Last.Value is TagSpace || _tokens.Count == 0)
                boldToken.IsAtTheBeginning = true;

            _tokens.AddLast(boldToken);
            var currentToken = _tokens.Last.Previous;
            var spacesCnt = 0;
            var onlyEmptyStrings = true;

            while (currentToken != null)
            {
                if (currentToken.Value is TagSpace)
                    spacesCnt++;


                if (currentToken.Value.IsPrevent)
                    break;

                if (currentToken.Value is TagItalic && !currentToken.Value.IsPrevent && (currentToken.Value is ITag) && !(currentToken.Value as ITag).IsClosed)
                {
                    boldToken.IsPrevent = true;
                    break;
                }

                if ((currentToken.Value is TagBold) && !currentToken.Value.IsPrevent)
                {
                    var starter = currentToken.Value as ITag;
                    if ((!starter.IsAtTheBeginning && spacesCnt == 0 || starter.IsAtTheBeginning) && !onlyEmptyStrings)
                        MakePair(starter, boldToken);
                }
                if (!(currentToken.Value is TagWord && currentToken.Value.Content.Length == 0))
                    onlyEmptyStrings = false;

                currentToken = currentToken.Previous;
            }
        }

        private void AddWordToken(StringBuilder word)
        {
            if (word.Length != 0)
            {
                var wordToken = new TagWord(word.ToString());
                _tokens.AddLast(wordToken);
            }
        }

        private string MakeHtml()
        {
            return string.Concat(_tokens.Select(token => token.Content));
        }
        private void MakePair(ITag opener, ITag endTag)
        {

            opener.IsClosed = true;
            opener.IsStartTag = true;
            endTag.IsClosed = true;
        }
    }
}

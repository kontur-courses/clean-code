using Markdown.Abstractions;
using Markdown.MDNodes;

namespace Markdown.MDParsing
{
    public sealed class InlineParser
    {
        private const char UnderscoreChar = '_';
        private const int ItalicLength = 1;
        private const int StrongLength = 2;
        private const int StrongWithItalicLength = 3;

        private readonly IReadOnlyList<Token> _tokens;
        private readonly bool[] _italicCloserAhead; 
        private readonly List<InlineNode> _output = new();

        private OpenDelimiter? _strong;
        private OpenDelimiter? _italic;
        private int _spaceTokens;

        private int _doubleTokens;

        public InlineParser(IReadOnlyList<Token> tokens)
        {
            _tokens = tokens;
            _italicCloserAhead = BuildItalicCloserAhead(tokens);
        }

        public List<InlineNode> ParseAll()
        {
            for (int i = 0; i < _tokens.Count; i++)
            {
                var token = _tokens[i];
                if (token.IsUnderscores)
                    ProcessUnderscores(i, token.UnderscoreCount);
                else
                    AddText(token.Text);

            }
            return _output;
        }

        private void ProcessUnderscores(int index, int count)
        {
            switch (count)
            {
                case ItalicLength:
                case StrongLength:
                    ProcessDelimiter(index, count);
                    break;
                case StrongWithItalicLength:
                    if (_strong != null || _italic != null)
                    {
                        ProcessDelimiter(index, ItalicLength);
                        ProcessDelimiter(index, StrongLength);
                    }
                    else
                    {
                        ProcessDelimiter(index, StrongLength);
                        ProcessDelimiter(index, ItalicLength);
                    }
                    break;
                default:
                    AddUnderscores(count);
                    break;
            }

            if (count == StrongLength) _doubleTokens++;
        }

        private void ProcessDelimiter(int index, int length)
        {
            if (InlineTokenValidator.CanClose(_tokens, index) && TryClose(index, length)) return;
            if (InlineTokenValidator.CanOpen(_tokens, index) && TryOpen(index, length)) return;
            AddUnderscores(length);
        }

        private bool TryOpen(int index, int length)
        {
            if (length == StrongLength && _italic != null)
                return false; 

            var opener = new OpenDelimiter(
                _output.Count,
                _spaceTokens,
                InlineTokenValidator.IsDigitBefore(_tokens, index),
                InlineTokenValidator.HasTextBefore(_tokens, index),
                _doubleTokens);

            if (length == StrongLength) _strong = opener;
            else _italic = opener;

            AddUnderscores(length); 
            return true;
        }

        private bool TryClose(int index, int length)
        {
            if (length == ItalicLength)
                return TryCloseItalic(index);
            return TryCloseStrong(index);
        }

        private bool TryCloseItalic(int index)
        {
            if (_italic == null || !IsValidPair(_italic, index, ItalicLength))
                return false;

            Wrap(_italic, children => new ItalicNode(children));
            _italic = null;
            return true;
        }

        private bool TryCloseStrong(int index)
        {
            if (_strong == null) return false;

            if (_italic != null)
            {
                if (_italicCloserAhead[index + 1])
                {
                    _strong = null;
                    _italic = null;
                    AddUnderscores(StrongLength);
                    return true;
                }
                _italic = null; 
            }

            if (!IsValidPair(_strong, index, StrongLength)) return false;

            Wrap(_strong, children => new StrongNode(children));
            _strong = null;
            return true;
        }

        private bool IsValidPair(OpenDelimiter opener, int closeIndex, int length)
        {
            bool isEmpty = _output.Count == opener.OutputIndex + 1;

            bool boundedByDigits = opener.IsDigitBefore || InlineTokenValidator.IsDigitAfter(_tokens, closeIndex);

            bool acrossWords = length == ItalicLength
                               && opener.HasTextBefore
                               && InlineTokenValidator.HasTextAfter(_tokens, closeIndex)
                               && _spaceTokens > opener.SpaceTokensBefore;

            bool crossesStrong = length == ItalicLength
                         && (_doubleTokens - opener.DoubleTokensBefore) % 2 == 1;

            return !isEmpty && !boundedByDigits && !acrossWords && !crossesStrong;
        }

        private void Wrap(OpenDelimiter opener, Func<List<InlineNode>, InlineNode> factory)
        {
            int start = opener.OutputIndex;
            var children = _output.GetRange(start + 1, _output.Count - start - 1);
            _output.RemoveRange(start, _output.Count - start);
            _output.Add(factory(children));
        }

        private void AddText(string text)
        {
            _output.Add(new TextNode(text));
            if (text.Contains(' ')) _spaceTokens++;
        }

        private void AddUnderscores(int count) =>
            _output.Add(new TextNode(new string(UnderscoreChar, count)));

        private static bool[] BuildItalicCloserAhead(IReadOnlyList<Token> tokens)
        {
            var ahead = new bool[tokens.Count + 1];
            for (int i = tokens.Count - 1; i >= 0; i--)
            {
                bool isItalicCloser = tokens[i].IsUnderscores
                      && tokens[i].UnderscoreCount == ItalicLength
                      && InlineTokenValidator.CanClose(tokens, i);
                ahead[i] = ahead[i + 1] || isItalicCloser;
            }
            return ahead;
        }
    }
}
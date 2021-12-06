using Markdown.TagEvents;
using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Taginizer
    {
        private readonly List<TagEvent> _tokens;
        private readonly string _input;

        public Taginizer(string input)
        {
            _tokens = new List<TagEvent>();
            _input = input;
        }

        public List<TagEvent> Taginize()
        {
            for (var index = 0; index < _input.Length; index++)
            {
                var symbol = _input[index];
                if (char.IsLetter(symbol))
                {
                    var contentLength = GetTagContentLength(index, char.IsLetter);
                    var content = _input.Substring(index, contentLength);
                    _tokens.Add(new TagEvent(TagSide.None, TagName.Word, content));
                    index = index + contentLength - 1;
                }
                else if (char.IsDigit(symbol))
                {
                    var contentLength = GetTagContentLength(index, char.IsDigit);
                    var content = _input.Substring(index, contentLength);
                    _tokens.Add(new TagEvent(TagSide.None, TagName.Number, content));
                    index = index + contentLength - 1;
                }
                switch (symbol)
                {
                    case '_':
                        index = ProcessUnderlineAndGetNewIndex(index);
                        break;
                    case '#':
                        index = ProcessHashtagAndGetNewIndex(index);
                        break;
                    case '\n':
                        _tokens.Add(new TagEvent(TagSide.Right, TagName.NewLine, symbol.ToString()));
                        break;
                    case '\\':
                        _tokens.Add(new TagEvent(TagSide.None, TagName.Escape, symbol.ToString()));
                        break;
                    case ' ':
                        _tokens.Add(new TagEvent(TagSide.None, TagName.Whitespace, symbol.ToString()));
                        break;
                }
            }
            _tokens.Add(new TagEvent(TagSide.None, TagName.Eof, ""));
            return _tokens;
        }

        private int ProcessHashtagAndGetNewIndex(int index)
        {
            var prevIndex = index - 1;
            var nextIndex = index + 1;
            if (index == 0 || _input[prevIndex] == '\n')
            {
                _tokens.Add(new TagEvent(TagSide.Left, TagName.Header, _input[index].ToString()));
                return index;
            }

            var contentLength = GetTagContentLength(index, s => s == '#');
            var content = _input.Substring(index, contentLength);
            _tokens.Add(new TagEvent(TagSide.None, TagName.Word, content));
            return index = index + contentLength - 1;
        }

        private int ProcessUnderlineAndGetNewIndex(int index)
        {
            var nextIndex = index + 1;
            if (IsDoubleUnderlineDetected(nextIndex))
            {
                _tokens.Add(new TagEvent(TagSide.None, TagName.DoubleUnderliner, "__"));
                index = nextIndex;
            }
            else
                _tokens.Add(new TagEvent(TagSide.None, TagName.Underliner, "_"));

            return index;
        }

        private bool IsDoubleUnderlineDetected(int nextIndex)
        {
            return nextIndex < _input.Length && _input[nextIndex] == '_';
        }

        private int GetTagContentLength(int index, Func<char, bool> predicate)
        {
            var length = 0;
            while (index < _input.Length && predicate(_input[index]))
            {
                length++;
                index++;
            }
            return length;
        }
    }
}

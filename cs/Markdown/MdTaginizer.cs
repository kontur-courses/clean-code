using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tag_Classes;

namespace Markdown
{
    internal class MdTaginizer : ITaginizer
    {
        private readonly string _input;
        private readonly HashSet<char> _tagMarks;

        private readonly HashSet<char> _marksToSkipIfRepeated
            = new HashSet<char>() {'_', '\\'};

        public MdTaginizer(string input, HashSet<char> tagMarks)
        {
            _input = input;
            _tagMarks = tagMarks;
        }

        public List<TagEvent> GetTagEvents()
        {
            var tagEvents = GetSimpleTagEvents();
            SetSideToSidefreeTags(tagEvents);
            return tagEvents;
        }

        private List<TagEvent> GetSimpleTagEvents()
        {
            var tagEvents = new List<TagEvent>();
            var plainText = new StringBuilder();
            for (var index = 0; index < _input.Length; index++)
            {
                var symbol = _input[index];
                if (IsSymbolAlreadyProcessed(index, symbol)) continue;
                if (_tagMarks.Contains(symbol))
                {
                    AddTextTagEventIfNotEmpty(plainText, tagEvents);
                    tagEvents.Add(GetTagEventFrom(symbol, index));
                }
                else
                {
                    plainText.Append(symbol);
                }
            }

            AddTextTagEventIfNotEmpty(plainText, tagEvents);
            return tagEvents;
        }

        private TagEvent GetTagEventFrom(char symbol, int index)
        {
            if (IsSymbolHeader(symbol))
                return GetTagEventFromHeader(index);
            if (IsSymbolUnderliner(symbol))
                return GetTagEventFromUnderliners(index);
            return new TagEvent(Side.None, Mark.Text, _input[index].ToString());
        }

        private void AddTextTagEventIfNotEmpty(StringBuilder plainText, List<TagEvent> tagEvents)
        {
            if (plainText.Length > 0)
            {
                tagEvents.Add(new TagEvent(Side.None, Mark.Text, plainText.ToString()));
                plainText.Clear();
            }
        }

        private void SetSideToSidefreeTags(List<TagEvent> tagEvents)
        {
            TagEvent possibleLeft = new TagEvent();
            for (var index = 0; index < tagEvents.Count; index++)
            {
                var tagEvent = tagEvents[index];
                if (tagEvent.IsSideUnknown())
                {
                    possibleLeft = ProcessPreviousUnknownTag(possibleLeft, tagEvent);
                    continue;
                }
                if (tagEvent.IsPlainText() && !possibleLeft.IsEmpty())
                {
                    if (tagEvent.TagContent.Contains(" "))
                    {
                        possibleLeft.ChangeMarkAndSideTo(Mark.Text, Side.None);
                        possibleLeft = new TagEvent();
                    }
                }
            }
            possibleLeft.ChangeMarkAndSideTo(Mark.Text, Side.None);
        }

        private TagEvent ProcessPreviousUnknownTag(TagEvent possibleLeft, TagEvent tagEvent)
        {
            if (possibleLeft.IsEmpty())
                return tagEvent;

            possibleLeft.Side = Side.Left;
            tagEvent.Side = Side.Right;
            return new TagEvent();
        }

        private TagEvent GetTagEventFromUnderliners(int index)
        {
            Mark underlinerKind;
            string content;
            int nextIndex;
            var prevIndex = index - 1;
            (underlinerKind, content, nextIndex) = GetUnderlinerOptions(index);

            if (IsInsideWord(prevIndex, nextIndex))
            {
                return IsBetweenDigits(prevIndex, nextIndex)
                    ? new TagEvent(Side.None, Mark.Text, content)
                    : new TagEvent(Side.Unknown, underlinerKind, content);
            }
            if (IsExistsAndNotWhitespace(nextIndex))
                return new TagEvent(Side.Left, underlinerKind, content);
            if (IsExistsAndNotWhitespace(prevIndex))
                return new TagEvent(Side.Right, underlinerKind, content);
            return new TagEvent(Side.None, underlinerKind, content);
        }

        private bool IsBetweenDigits(int prevIndex, int nextIndex)
        {
            return char.IsDigit(_input[prevIndex]) & char.IsDigit(_input[nextIndex]);
        }

        private TagEvent GetTagEventFromHeader(int index)
        {
            var previousIndex = index - 1;
            var mark = _input[index].ToString();
            if (IsLeftHeaderFrom(index, previousIndex))
                return new TagEvent(Side.Left, Mark.Header, mark);
            if (IsRightHeaderFrom(index))
                return new TagEvent(Side.Right, Mark.Header, mark);
            return new TagEvent(Side.None, Mark.Text, mark);
        }

        private (Mark, string, int) GetUnderlinerOptions(int index)
        {
            return IsDoubleUnderlinerFrom(index)
                ? (Mark.DoubleUnderliner, _input[index].ToString() + _input[index + 1].ToString(), index + 2)
                : (Mark.Underliner, _input[index].ToString(), index + 1);
        }

        private bool IsDoubleUnderlinerFrom(int index)
        {
            var nextIndex = index + 1;
            return nextIndex < _input.Length && _input[nextIndex] == '_';
        }

        private bool IsRightHeaderFrom(int index)
        {
            return _input[index] == '\n';
        }

        private bool IsLeftHeaderFrom(int index, int previousIndex)
        {
            return _input[index] == '#' && (index == 0 || _input[previousIndex] == '\n');
        }

        private bool IsExistsAndNotWhitespace(int index)
        {
            return index >= 0 && index < _input.Length && _input[index] != ' ';
        }

        private bool IsSymbolUnderliner(char symbol)
        {
            return symbol == '_';
        }

        private bool IsSymbolHeader(char symbol)
        {
            return symbol == '#' || symbol == '\n';
        }


        private bool IsInsideWord(int prevIndex, int nextIndex)
        {
            return IsExistsAndNotWhitespace(prevIndex) && IsExistsAndNotWhitespace(nextIndex);
        }

        private bool IsSymbolAlreadyProcessed(int index, char symbol)
        {
            return index > 0 && _input[index - 1] == symbol && _marksToSkipIfRepeated.Contains(symbol);
        }
    }
}

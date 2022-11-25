using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Markdown
{
    public class MarkdownParser : ITagsParser<MdTagWithIndex>
    {
        private List<MdTag> _tags;
        private List<MdTagWithIndex> _foundTags;

        public MarkdownParser(IEnumerable<MdTag> tags)
        {
            _tags = new List<MdTag>(tags);
        }

        public IEnumerable<MdTagWithIndex> GetIndexesTags(string text)
        {
            _foundTags = new List<MdTagWithIndex>();
            
            for (var i = 0; i < text.Length; i++)
            {
                foreach (var tag in _tags)
                {
                    var openTagIndex = TryGetTagIndex(tag, text, i, true);
                    var closeTagIndex = TryGetTagIndex(tag, text, openTagIndex + tag.OpenTag.Length + 1, false);
                    if (tag.OpenTag is "#" && openTagIndex != -1)
                        closeTagIndex = text.IndexOf('\n', openTagIndex) == -1 ? text.Length : text.IndexOf('\n', openTagIndex);
                    if (CheckCorrectnessIndexes(tag, openTagIndex, closeTagIndex, text))
                        _foundTags.Add(new MdTagWithIndex(tag, openTagIndex, closeTagIndex));
                }
            }

            CheckIntersection();

            return _foundTags.OrderBy(x => x.OpenTagIndex);
        }

        private int TryGetTagIndex(MdTag tag, string text, int start, bool isOpenTag)
        {
            if (start == -1)
                return -1;
            
            var index = text.IndexOf(tag.OpenTag, start, StringComparison.Ordinal);
            var count = index + 1;
            while (IsIndexAlreadyUse(index))
                index = text.IndexOf(tag.OpenTag, count++, StringComparison.Ordinal);
            if (isOpenTag)
                return index + tag.OpenTag.Length + 1 > text.Length ? -1 : index;
            return index;
        }

        private bool CheckCorrectnessIndexes(MdTag tag, int openTagIndex, int closeTagIndex, string text)
        {
            if (!GeneralCheck(tag, openTagIndex, closeTagIndex))
                return false;
            
            if (CheckPreviousEscapeSymbol(text, openTagIndex) ||
                CheckPreviousEscapeSymbol(text, closeTagIndex))
                return false;
            
            if (tag.OpenTag is "#")
                return CheckForHeaderTag(text, openTagIndex);

            var isValidItalicTag = true;
            if (tag.OpenTag is "_")
                isValidItalicTag = CheckForItalicTag(tag, text, openTagIndex, closeTagIndex);
            
            if (tag.OpenTag is "_" or "__")
                return isValidItalicTag && 
                       СheckForItalicAndBoldTag(tag, text, openTagIndex, closeTagIndex);

            return true;
        }

        private bool GeneralCheck(MdTag tag, int openTagIndex, int closeTagIndex)
        {
            if (openTagIndex == -1 || closeTagIndex == -1)
                return false;

            return openTagIndex + tag.OpenTag.Length != closeTagIndex;
        }

        private bool CheckForHeaderTag(string text, int openTagIndex)
        {
            return text[0] == '#' || 
                   IsValidIndex(text, openTagIndex - 1) &&
                   text[openTagIndex - 1] != '\n';
        }

        private bool CheckForItalicTag(MdTag tag, string text, int openTagIndex, int closeTagIndex)
        {
            return !HasEqualNeighbors(tag, openTagIndex, text) && !HasEqualNeighbors(tag, closeTagIndex, text);
        }
        
        private bool СheckForItalicAndBoldTag(MdTag tag, string text, int openTagIndex, int closeTagIndex)
        {
            if (IsValidIndex(text, openTagIndex + 1) &&
                text[openTagIndex + 1] == ' ')
                return false;

            if (text[closeTagIndex - 1] == ' ')
                return false;

            if (!IsIndexInsideAWord(openTagIndex, text) || !IsIndexInsideAWord(closeTagIndex, text)) return true;
            
            var strBetweenTags = text.Substring(openTagIndex + tag.OpenTag.Length,
                closeTagIndex - (openTagIndex + tag.OpenTag.Length));
            
            if (strBetweenTags.Contains(' '))
                return false;
            
            return !int.TryParse(strBetweenTags, out _);
        }
        
        private bool CheckPreviousEscapeSymbol(string text, int index)
        {
            var temp = index - 1;
            var isEscapeSymbol = false;
            while (IsValidIndex(text, temp) &&
                   text[temp] == '\\')
            {
                isEscapeSymbol = !isEscapeSymbol;
                temp -= 1;
            }

            return isEscapeSymbol;
        }

        private bool IsIndexAlreadyUse(int index)
        {
            return _foundTags.Any(x => x.OpenTagIndex + x.OpenTag.Length - 1 == index ||
                                        x.CloseTagIndex + x.OpenTag.Length - 1 == index ||
                                        x.OpenTagIndex == index ||
                                        x.CloseTagIndex == index);
        }

        private bool IsValidIndex(string text, int index)
        {
            return index >= 0 && index < text.Length;
        }

        private bool HasEqualNeighbors(MdTag tag, int index, string text)
        {
            for (var i = -tag.OpenTag.Length; i <= tag.OpenTag.Length; i += tag.OpenTag.Length * 2)
            {
                if (!IsValidIndex(text, index + i) || !IsValidIndex(text, index))
                    continue;
                if (index + i < 0 || i == 0)
                    continue;
                if (text[index + i] == text[index])
                    return true;
            }

            return false;
        }

        private bool IsIndexInsideAWord(int index, string text)
        {
            var isInsideAWord = true;
            for (var i = -1; i < 2; i += 2)
            {
                if (!IsValidIndex(text, index + i) || !IsValidIndex(text, index))
                    continue;
                if (index + i < 0 || i == 0)
                    continue;
                if (!Char.IsLetterOrDigit(text[index + i]))
                    isInsideAWord = false;
            }

            return isInsideAWord;
        }
        
        private void CheckIntersection()
        {
            var tagsWithoutHeaderTag = _foundTags
                .Select(x => x)
                .Where(x => x.OpenTag[0] == '_')
                .ToList();

            var toRemoveTags = new List<MdTagWithIndex>();
            foreach (var tag in tagsWithoutHeaderTag)
            {
                foreach (var tag1 in tagsWithoutHeaderTag)
                {
                    if (tag == tag1)
                        continue;

                    if (tag.OpenTag != "_" || tag1.OpenTag != "__")
                        continue;

                    if ((tag.OpenTagIndex > tag1.OpenTagIndex &&
                         tag1.CloseTagIndex < tag.CloseTagIndex) ||
                        (tag1.OpenTagIndex > tag.OpenTagIndex &&
                         tag.CloseTagIndex < tag1.CloseTagIndex))
                    {
                        toRemoveTags.Add(tag);
                        toRemoveTags.Add(tag1);
                    }

                    if (tag.OpenTagIndex < tag1.OpenTagIndex &&
                        tag.CloseTagIndex > tag1.CloseTagIndex)
                    {
                        toRemoveTags.Add(tag1);
                    }
                }
            }

            foreach (var toRemove in toRemoveTags)
                _foundTags.Remove(toRemove);
        }
    }
}
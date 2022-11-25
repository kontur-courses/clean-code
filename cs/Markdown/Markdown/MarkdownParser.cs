using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkdownParser : ITagsParser<MdTagWithIndex>
    {
        private List<MdTag> _tags;
        private List<MdTagWithIndex> _findedTags;

        public MarkdownParser(IEnumerable<MdTag> tags)
        {
            _tags = new List<MdTag>(tags);
            _findedTags = new List<MdTagWithIndex>();
        }

        public IEnumerable<MdTagWithIndex> GetIndexesTags(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                foreach (var tag in _tags)
                {
                    var openTagIndex = TryGetOpenTagIndex(tag, text, i);
                    var closeTagIndex = TryGetCloseTagIndex(tag, text, openTagIndex);
                    if (IsTagIndexes(tag, openTagIndex, closeTagIndex, text))
                        _findedTags.Add(new MdTagWithIndex(tag, openTagIndex, closeTagIndex));
                }
            }

            CheckIntersection();

            return _findedTags.OrderBy(x => x.OpenTagIndex);
        }

        private int TryGetOpenTagIndex(MdTag tag, string text, int start)
        {
            var openTagIndex =
                text.IndexOf(tag.OpenTag, start, StringComparison.Ordinal);

            while (IsIndexAlreadyUse(openTagIndex))
                openTagIndex = text.IndexOf(tag.OpenTag, openTagIndex + 1, StringComparison.Ordinal);

            return openTagIndex;
        }

        private int TryGetCloseTagIndex(MdTag tag, string text, int openTagIndex)
        {
            if (openTagIndex == -1)
                return -1;

            if (!tag.HasCloseTag)
            {
                if (text.IndexOf('\n', openTagIndex) == -1)
                    return text.Length;
                return text.IndexOf('\n', openTagIndex);
            }

            //Следующий индекс элемента после тега
            if (openTagIndex + tag.OpenTag.Length + 1 > text.Length)
                return -1;

            var closeTagIndex =
                text.IndexOf(tag.OpenTag, openTagIndex + tag.OpenTag.Length + 1, StringComparison.Ordinal);

            while (IsIndexAlreadyUse(closeTagIndex))
                closeTagIndex = text.IndexOf(tag.OpenTag, closeTagIndex + 1, StringComparison.Ordinal);

            return closeTagIndex;
        }

        private void CheckIntersection()
        {
            var tagsWithoutHeaderTag = _findedTags
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
                _findedTags.Remove(toRemove);
        }

        private bool IsTagIndexes(MdTag tag, int openTagIndex, int closeTagIndex, string text)
        {
            if (openTagIndex == -1 || closeTagIndex == -1)
                return false;

            // #
            if (!tag.HasCloseTag && text[0] != tag.OpenTag[0])
                return false;

            // Если после тега нет символов, то это не открывающий тег
            if (text.Length < openTagIndex + 1)
                return false;

            //Если дальше закрывающий тег, то это не считается тегом
            if (openTagIndex + tag.OpenTag.Length == closeTagIndex)
                return false;

            if (tag.OpenTag.Length == 1 && (HasEqualNeighbors(tag, openTagIndex, text) ||
                                            HasEqualNeighbors(tag, closeTagIndex, text)))
                return false;

            //За подчерками, начинающими выделение, должен следовать непробельный символ
            if (tag.HasCloseTag &&
                IsValidIndex(text, openTagIndex + 1) &&
                text[openTagIndex + 1] == ' ')
                return false;

            //Подчерки, заканчивающие выделение, должны следовать за непробельным символом
            if (tag.HasCloseTag &&
                text[closeTagIndex - 1] == ' ')
                return false;

            //Выделение в разных словах не работает
            if (IsIndexInsideAWord(openTagIndex, text) &&
                IsIndexInsideAWord(closeTagIndex, text)
                && text.Substring(openTagIndex, closeTagIndex - openTagIndex).Contains(' '))
                return false;

            //Подчерки внутри текста c цифрами не считаются выделением и должны оставаться символами подчерка.
            if (IsIndexInsideAWord(openTagIndex, text) &&
                IsIndexInsideAWord(closeTagIndex, text)
                && int.TryParse(
                    text.Substring(openTagIndex + tag.OpenTag.Length,
                        closeTagIndex - (openTagIndex + tag.OpenTag.Length)), out _))
                return false;

            if (IsPreviousSlash(openTagIndex, closeTagIndex, text))
                return false;

            return true;
        }


        //Проверка на экранирующий символ
        private bool IsPreviousSlash(int openTagIndex, int closeTagIndex, string text)
        {
            var temp = openTagIndex - 1;
            var isSlashOpenTag = false;
            while (IsValidIndex(text, temp) &&
                   text[temp] == '\\')
            {
                isSlashOpenTag = !isSlashOpenTag;
                temp -= 1;
            }

            temp = closeTagIndex - 1;
            var isSlashCloseTag = false;
            while (IsValidIndex(text, temp) &&
                   text[temp] == '\\')
            {
                isSlashCloseTag = !isSlashCloseTag;
                temp -= 1;
            }

            return isSlashOpenTag || isSlashCloseTag;
        }

        //Найден ли уже тег с таким индексом
        private bool IsIndexAlreadyUse(int index)
        {
            return _findedTags.Any(x => x.OpenTagIndex + x.OpenTag.Length - 1 == index ||
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
                if (IsIndexAlreadyUse(index + i))
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
    }
}
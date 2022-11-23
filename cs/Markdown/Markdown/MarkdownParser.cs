using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkdownParser : ITagsParser<MdTag>
    {
        private List<MdTag> _tags;
        private string _text;
        
        public MarkdownParser(List<MdTag> tags)
        {
            _tags = new List<MdTag>();
            _tags.AddRange(tags);
        }
        
        public List<MdTag> GetIndexesTags(string text)
        {
            var findedTags = new List<MdTag>();
            
            for (int i = 0; i < text.Length; i++)
            {
                foreach (var tag in _tags)
                {
                    var openTagIndex = text.IndexOf(tag.OpenTag, i, StringComparison.Ordinal);
                    var closeTagIndex = TryGetCloseTagIndex(tag, text, openTagIndex);
                    if (AreIndexesAlreadyUse(findedTags, (openTagIndex, closeTagIndex)))
                        continue;
                    if (IsTagIndexes(tag, openTagIndex, closeTagIndex, text))
                        findedTags.Add(new MdTag(tag.OpenTag, tag.HasCloseTag, openTagIndex, closeTagIndex));
                }
            }
            return findedTags;
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
            
            return text.IndexOf(tag.OpenTag, openTagIndex + tag.OpenTag.Length + 1, StringComparison.Ordinal);
        }
        
        private bool IsTagIndexes(MdTag tag, int openTagIndex, int closeTagIndex ,string text)
        {
            if(openTagIndex == -1 || closeTagIndex == -1)
                return false;
            
            // #
            if (!tag.HasCloseTag && text[0] != tag.OpenTag[0])
                return false;
            
            // Если после тега нет символов, то это не открывающий тег
            if (text.Length < openTagIndex + 1)
                return false;

            //Если дальше такой же тег, то это не открывающий тег
            if (openTagIndex + tag.OpenTag.Length == closeTagIndex) 
                return false;

            if (tag.IsSimpleTag() && HasEqualNeighbors(tag, closeTagIndex, text) &&
                HasEqualNeighbors(tag, openTagIndex, text))
                return false;

            if (IsValidIndex(text, openTagIndex - 1) &&
                text[openTagIndex - 1] == '\\')
                return false;

            if (IsValidIndex(text, closeTagIndex - 1) &&
                text[closeTagIndex - 1] == '\\')
                return false;

            return true;
        }

        private bool AreIndexesAlreadyUse(List<MdTag> findedTags, (int openTagIndex, int closeTagIndex) tags)
        {
            return findedTags.Any(x => x.OpenTagIndex + x.OpenTag.Length - 1 == tags.openTagIndex ||
                                x.CloseTagIndex + x.OpenTag.Length - 1 == tags.closeTagIndex ||
                                x.OpenTagIndex + x.OpenTag.Length - 1 == tags.closeTagIndex ||
                                x.CloseTagIndex + x.OpenTag.Length - 1 == tags.openTagIndex || 
                                x.OpenTagIndex == tags.openTagIndex ||
                                x.CloseTagIndex == tags.closeTagIndex ||
                                x.OpenTagIndex == tags.closeTagIndex ||
                                x.CloseTagIndex  == tags.openTagIndex);
        }

        private bool IsValidIndex(string text, int index)
        {
            if (index < 0 || index >= text.Length)
                return false;
            return true;
        }

        private bool HasEqualNeighbors(MdTag tag, int index, string text)
        {
            for (var i = -tag.OpenTag.Length; i <= tag.OpenTag.Length; i += tag.OpenTag.Length * 2)
            {
                if (index + i >= text.Length || index + i < 0
                || index >= text.Length || index < 0)
                    continue;
                if (index + i < 0 || i == 0)
                    continue;
                if (text[index + i] == text[index])
                    return true;
            }
        
            return false;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownParser : ITagsParser<MdTag>
    {
        private List<MdTag> _tags;
        public MarkdownParser(List<MdTag> tags)
        {
            _tags = new List<MdTag>();
            _tags.AddRange(tags);
        }
        
        public List<MdTag> GetIndexesTags(string text)
        {
            foreach (var tag in _tags)
            {
                while(text.Contains(tag.OpenTag))
                    TagIsFind(tag, ref text);
            }
            
            return _tags;
        }

        private void TagIsFind(MdTag tag, ref string text)
        {
            tag.OpenTagIndex = text.IndexOf(tag.OpenTag, StringComparison.Ordinal);
            text = text.Substring(tag.OpenTagIndex + tag.OpenTag.Length);
            if (tag.HasCloseTag)
                tag.CloseTagIndex = text.IndexOf(tag.CloseTag, StringComparison.Ordinal);
            else
                tag.CloseTagIndex = text.IndexOf('\n');
            
            text = text.Substring(tag.CloseTagIndex + tag.CloseTag.Length);
        }
    }
}
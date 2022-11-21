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
            if (tag.HasCloseTag)
                tag.CloseTagIndex = text.LastIndexOf(tag.CloseTag, StringComparison.Ordinal);
            else
            {
                if (text.IndexOf('\n') == -1)
                    tag.CloseTagIndex = text.Length;
                else
                    tag.CloseTagIndex = text.IndexOf('\n');
            }
            
            text = text.Substring(tag.CloseTagIndex + tag.CloseTag.Length);
        }
    }
}
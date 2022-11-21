using System;
using System.Collections.Generic;
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
            _text = text;
            foreach (var tag in _tags)
            {
                var index = TryGetOpenTagIndex(tag);
                while(index != -1)
                {
                    TagIsFind(tag, index);
                    index = TryGetOpenTagIndex(tag);
                }
            }
            
            return _tags;
        }

        private int TryGetOpenTagIndex(MdTag tag)
        {
            if (!_text.Contains(tag.OpenTag))
                return -1;
            var index = -1;
            for (int i = 0; i < _text.Length; i++)
            {
                if (_text[i] != tag.OpenTag[0])
                    continue;
                if (tag.OpenTag.Length > _text.Length - i)
                    break;

                if (_text[i + 1] == tag.OpenTag[0] && tag.OpenTag.Length == 1)
                {
                    i++;
                    if (i == _text.Length - 1)
                        return -1;
                    continue;
                }

                index = i;
                for (int j = 0; j < tag.OpenTag.Length; j++)
                {
                    if (_text[i + j] != tag.OpenTag[j])
                        index = -1;
                }
                break;
            }
            return index;
        }

        private void TagIsFind(MdTag tag, int openTagIndex)
        {
            tag.OpenTagIndex = openTagIndex;
            if (!tag.HasCloseTag)
            {
                if (_text.IndexOf('\n') == -1)
                    tag.CloseTagIndex = _text.Length;
                else
                    tag.CloseTagIndex = _text.IndexOf('\n');
            }
            else
            {
                int i = openTagIndex + 1;
                if (tag.OpenTag.Length > 1)
                    i++;
                for (; i < _text.Length; i++)
                {
                    if (_text[i] != tag.OpenTag[0])
                        continue;
                    tag.CloseTagIndex = i;
                    break;
                }
            }
            RefactorString(tag);
        }

        private void RefactorString(MdTag tag)
        {
            var oldLength = _text.Length;
            _text = _text.Substring(0, tag.OpenTagIndex) + _text.Substring(tag.CloseTagIndex + tag.CloseTag.Length);

            var builder = new StringBuilder();
            for (int i = 0; i < oldLength - _text.Length; i++)
                builder.Append('a');
            _text = _text.Insert(tag.OpenTagIndex, builder.ToString());
        }
    }
}
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TagsReplacer<T, H> 
        where T: Tag
        where H: Tag
    {
        private readonly Dictionary<T, H> _tagToTag;
        public TagsReplacer(Dictionary<T, H> tagToTag)
        {
            _tagToTag = tagToTag;
        }

        public string ReplaceTag(List<T> tags, string text)
        {
            var actions = new Dictionary<Type, Func<string>>
            {
                {typeof(MdTag), () => 
                    ReplaceMdTag(tags, text)}
            };
            
            return actions[typeof(T)]();
        }
        
        private string ReplaceMdTag(List<T> tags, string text)
        {
            var builder = new StringBuilder();
            foreach (var tag in tags)
            {
                var strWithoutTag = 
                    text.Substring(tag.OpenTagIndex + tag.OpenTag.Length, 
                        tag.CloseTagIndex - (tag.OpenTagIndex + tag.OpenTag.Length));

                var openAndCloseTag = GetOpenAndCloseTag(tag);
                
                builder.Append(openAndCloseTag.Item1);
                builder.Append(strWithoutTag);
                builder.Append(openAndCloseTag.Item2);

                text = text.Substring(0, tag.OpenTagIndex) + 
                       builder + text.Substring(tag.CloseTagIndex + tag.CloseTag.Length);
                
                ShiftTeg(tags, tag, openAndCloseTag.Item1.Length, openAndCloseTag.Item2.Length);
                builder.Clear();
            }

            text = RemoveSlash(text);
            
            return text;
        }

        private (string, string) GetOpenAndCloseTag(T a)
        {
            foreach (var tag in _tagToTag)
            {
                if (tag.Key.OpenTag == a.OpenTag)
                    return (tag.Value.OpenTag, tag.Value.CloseTag);
            }

            throw new ArgumentException("Tag not found");
        }

        private void ShiftTeg(List<T> tags, T currentTag, int openTagLength, int closeTagLength)
        {
            foreach (var tag in tags)
            {
                if (tag == currentTag)
                    continue;
                var openTagIndex = tag.OpenTagIndex;
                var closeTagIndex = tag.CloseTagIndex;
                
                if (openTagIndex > currentTag.OpenTagIndex)
                    tag.OpenTagIndex += openTagLength - currentTag.OpenTag.Length;
                
                if(closeTagIndex > currentTag.OpenTagIndex)
                    tag.CloseTagIndex += openTagLength - currentTag.OpenTag.Length;

                if (openTagIndex > currentTag.CloseTagIndex)
                    tag.OpenTagIndex += closeTagLength - currentTag.CloseTag.Length;
                
                if(closeTagIndex > currentTag.CloseTagIndex)
                    tag.CloseTagIndex += closeTagLength - currentTag.CloseTag.Length;
            }
        }

        private string RemoveSlash(string text)
        {
            var builder = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\\' && i + 1 < text.Length &&
                    IsTagSymbol(text[i + 1]))
                {
                    var temp = i + 1;
                    while (temp < text.Length && text[temp] == '\\')
                        temp++;
                    if ((temp - i + 1) % 2 != 0)
                        builder.Append('\\');
                    i = temp - 1;
                }
                else
                    builder.Append(text[i]);
            }
            return builder.ToString();
        }

        private bool IsTagSymbol(char symbol)
        {
            return _tagToTag
                .Any(x => x.Key.OpenTag[0] == symbol ||
                                      x.Value.OpenTag[0] == symbol ||
                                      symbol == '\\');
        }
    }
}

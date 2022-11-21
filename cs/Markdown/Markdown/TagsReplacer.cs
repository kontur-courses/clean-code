using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            // var actions = new Dictionary<Type, Func<string>>
            // {
            //     {typeof(MdTag), () => 
            //         ReplaceMdTag(tags, text)}
            // };
            
            // return actions[typeof(T)]();
            return ReplaceMdTag(tags, text);
        }
        
        private string ReplaceMdTag(IEnumerable<T> tags, string text)
        {
            var builder = new StringBuilder();
            int index = 0;
            foreach (var tag in tags)
            {
                if (index == text.Length)
                    break;
                if(!text.Contains(tag.OpenTag))
                    continue;
                
                var strWithoutTag = 
                    text.Substring(tag.OpenTagIndex + tag.OpenTag.Length, 
                        tag.CloseTagIndex - (tag.OpenTagIndex + tag.OpenTagIndex + tag.OpenTag.Length));
                
                builder.Append(_tagToTag[tag].OpenTag);
                builder.Append(strWithoutTag);
                builder.Append(_tagToTag[tag].CloseTag);

                text = text.Substring(0, tag.OpenTagIndex) + 
                       builder + text.Substring(tag.CloseTagIndex + tag.CloseTag.Length);
                builder.Clear();
            }

            return text;
        }
    }
}

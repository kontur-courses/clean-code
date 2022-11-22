using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class HtmlTagStorage : TagStorage
    {
        public HtmlTagStorage()
        {
            Tags = new List<ITag>()
            {
                new Tag(TagType.Header, "<h1>", "</h1>"),
                new Tag(TagType.Italic, "<em>", "</em>"),
                new Tag(TagType.Strong, "<strong>", "</strong>")
            };
        }
    }
}

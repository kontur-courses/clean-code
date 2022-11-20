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
                new Token(TagType.Header, "<h1>", "</h1>"),
                new Token(TagType.Italic, "<em>", "</em>"),
                new Token(TagType.Strong, "<strong>", "</strong>")
            };
        }
    }
}

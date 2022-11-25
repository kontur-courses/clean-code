using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.TagValidator;

namespace Markdown.Tags
{
    public class HtmlTagStorage : TagStorage
    {
        public HtmlTagStorage()
        {
            Tags = new List<ITag>()
            {
                new Tag(TagType.Header, "<h1>", "</h1>", new HeaderTagValidator()),
                new Tag(TagType.Italic, "<em>", "</em>", new InlineTagValidator()),
                new Tag(TagType.Strong, "<strong>", "</strong>", new InlineTagValidator())
            };
        }
    }
}

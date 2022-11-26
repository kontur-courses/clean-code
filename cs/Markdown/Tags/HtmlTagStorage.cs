using System.Collections.Generic;
using Markdown.TagValidator;

namespace Markdown.Tags
{
    public class HtmlTagStorage : TagStorage
    {
        public HtmlTagStorage()
        {
            Tags = new List<ITag>()
            {
                new Tag(TagType.Header, "<h1>", "</h1>", new LineTagValidator()),
                new Tag(TagType.Italic, "<em>", "</em>", new InlineTagValidator()),
                new Tag(TagType.Strong, "<strong>", "</strong>", new InlineTagValidator()),
                new Tag(TagType.UnorderedList, "<ul>", "</ul>", new ListTagValidator()),
                new Tag(TagType.UnorderedListItem, "<li>", "</li>", new ListTagValidator())
            };
        }
    }
}
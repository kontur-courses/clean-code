using System.Collections.Generic;
using Markdown.TagValidator;

namespace Markdown.Tags
{
    public class MdTagStorage : TagStorage
    {
        public MdTagStorage()
        {
            Tags = new List<ITag>
            {
                new Tag(TagType.Header, "# ", "\n", new LineTagValidator()),
                new Tag(TagType.Italic, "_", "_", new InlineTagValidator()),
                new Tag(TagType.Strong, "__", "__", new InlineTagValidator()), 
                new Tag(TagType.UnorderedList, "", "", new ListTagValidator()),
                new Tag(TagType.UnorderedListItem, "* ", "\n", new LineTagValidator())
            };

            ForbiddenTagNestings = new Dictionary<TagType, HashSet<TagType>>
            {
                { TagType.Italic, new HashSet<TagType> { TagType.Strong } }
            };
        }
    }
}
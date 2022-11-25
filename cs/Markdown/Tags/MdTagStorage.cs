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
                new Tag(TagType.Header, "# ", "\n", new HeaderTagValidator()),
                new Tag(TagType.Italic, "_", "_", new InlineTagValidator()),
                new Tag(TagType.Strong, "__", "__", new InlineTagValidator())
            };

            ForbiddenTagNestings = new Dictionary<TagType, HashSet<TagType>>
            {
                { TagType.Italic, new HashSet<TagType> { TagType.Strong } }
            };
        }
    }
}
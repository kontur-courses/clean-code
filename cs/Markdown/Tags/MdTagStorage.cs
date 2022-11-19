using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class MdTagStorage : TagStorage
    {
        private static readonly List<ITag> Tags = new List<ITag>()
        {
            new Tag(TagType.Header, "*", "\n"),
            new Tag(TagType.Italic, "_", "_"),
            new Tag(TagType.Strong, "__", "__")
        };
    }
}

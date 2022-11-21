using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class MdTagStorage : TagStorage
    {
        public MdTagStorage()
        {
            Tags = new List<ITag>()
            {
                new Token(TagType.Header, "#", "\n"),
                new Token(TagType.Italic, "_", "_"),
                new Token(TagType.Strong, "__", "__")
            };
        }
    }
}

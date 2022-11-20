using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public abstract class TagStorage
    {
        public List<ITag> Tags { get; protected set; }


        public ITag GetTagByType(TagType tagType)
        {
            foreach (var tag in Tags)
            {
                if (tag.Type == tagType)
                {
                    return tag;
                }
            }

            throw new Exception($"Token with type {nameof(tagType)} isn't found");
        }
    }
}

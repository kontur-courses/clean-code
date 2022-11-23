using System.Collections.Generic;

namespace Markdown.Tags
{
    public abstract class TagStorage
    {
        public List<ITag> Tags { get; protected set; }

        public string EscapeCharacter => "\\" ;

        public string GetSubTag(TagType tagType, SubTagOrder order)
        {
            var tag = Tags.Find(t => t.Type == tagType);

            return order == SubTagOrder.Opening ? tag.OpeningSubTag : tag.ClosingSubTag;
        }
    }
}

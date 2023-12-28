namespace Markdown.Tokens
{
    public class NestedTag : MdTag
    {
        public List<MdTag> Tags { get; set; } = new List<MdTag>();

        public NestedTag(TagType type) : base(type)
        {

        }
    }
}
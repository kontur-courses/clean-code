namespace Markdown.Tokens
{
    public abstract class MdTag
    {
        public TagType Type { get; protected set; }

        protected MdTag(TagType type)
        {
            Type = type;
        }
    }
}
namespace Markdown
{
    public class TagToken
    {
        public TagType TagType { get; }

        public TagToken(TagType operatorType)
        {
            this.TagType = operatorType;
        }
    }
}

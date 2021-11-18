namespace Markdown
{
    public class TagToken
    {
        public TagType TagType { get; private set; }

        public TagToken(TagType operatorType)
        {
            this.TagType = operatorType;
        }
    }
}

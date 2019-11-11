namespace Markdown
{
    public class EmTagType : TagType
    {
        private EmTagType() : base("<em>", "</em>", "_", "_")
        {
        }
    }
}
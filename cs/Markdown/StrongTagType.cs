namespace Markdown
{
    public class StrongTagType : TagType
    {
        public StrongTagType() : base("<strong>", "</strong>", "__", "__")
        {
        }
    }
}
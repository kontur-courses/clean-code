namespace MarkDown.TagTypes
{
    public class StrongTag : TagType
    {
        public StrongTag() : base("__", "strong", new []{new EmTag()}) { }
    }
}
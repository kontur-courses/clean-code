namespace MarkDown.TagTypes
{
    public class ATag : TagType
    {
        public ATag() : base("(", ")", "a", new []{typeof(ATag)}, new Parameter("[", "]", "href")){}
    }
}
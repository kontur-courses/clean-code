namespace Markdown
{
    public class TagSymbolSpecification
    {
        public readonly string RawView;
        public readonly string TagBody;
        public readonly bool WorksInsideOthers;

        public TagSymbolSpecification(string rawView, string tagBody, bool worksInsideOthers)
        {
            RawView = rawView;
            TagBody = tagBody;
            WorksInsideOthers = worksInsideOthers;
        }
    }
}
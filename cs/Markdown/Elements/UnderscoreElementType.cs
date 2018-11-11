namespace Markdown.Elements
{
    public class UnderscoreElementType : IElementType
    {
        private static readonly UnderscoreElementType Instance = new UnderscoreElementType();

        private UnderscoreElementType()
        { }

        public static UnderscoreElementType Create()
        {
            return Instance;
        }

        public string Indicator => "_";

        public bool CanContainElement(IElementType elementType)
        {
            return false;
        }
    }
}

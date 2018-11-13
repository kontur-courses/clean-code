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

        public bool IsIndicatorAt(string markdown, int position)
        {
            if (markdown.Substring(position, 1) != Indicator)
                return false;
            return position + 1 >= markdown.Length ||
                   markdown.Substring(position + 1, 1) != Indicator;
        }
    }
}

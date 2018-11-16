namespace Markdown.Elements
{
    public class SingleUnderscoreElementType : UnderscoreElementTypeBase
    {
        private static readonly SingleUnderscoreElementType Instance = new SingleUnderscoreElementType();

        private SingleUnderscoreElementType()
        { }

        public static SingleUnderscoreElementType Create()
        {
            return Instance;
        }

        public override string Indicator => "_";

        public override bool CanContainElement(IElementType elementType)
        {
            return false;
        }
    }
}

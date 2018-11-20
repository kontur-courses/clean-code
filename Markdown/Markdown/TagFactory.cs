using Markdown.Tag;

namespace Markdown
{
    public class TagFactory
    {
        public static ITag Create(MdType type)
        {
            switch (type)
            {
                case MdType.DoubleUnderLine:
                    return new DoubleUnderLineTag();
                case MdType.SingleUnderLine:
                    return new SingleUnderLineTag();
                case MdType.Sharp:
                    return new SharpTag();
                case MdType.TripleGraveAccent:
                    return new TrippleGraveAccentTag();
            }

            return new TextTag();
        }
    }
}
using Markdown.Tag;

namespace Markdown
{
    public static class MdTagCreator
    {
        public static ITag Create(string symbol)
        {
            if (symbol == "__")
                return new DoubleUnderLineTag();
            if (symbol == "_")
                return new SingleUnderLineTag();
            if (symbol == "#")
                return new SharpTag();
            if (symbol == "```")
                return new TrippleGraveAccentTag();
            return new TextTag();
        }
    }
}
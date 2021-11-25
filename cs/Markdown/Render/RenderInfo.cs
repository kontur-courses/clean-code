namespace Markdown.Render
{
    public class RenderInfo
    {
        public readonly TextType Type;
        public readonly string Prefix;
        public readonly string Suffix;

        public RenderInfo(TextType type, string prefix, string suffix)
        {
            Type = type;
            Prefix = prefix;
            Suffix = suffix;
        }
    }
}
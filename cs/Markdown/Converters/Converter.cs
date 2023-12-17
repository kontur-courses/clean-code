using Markdown.Tags;

namespace Markdown
{
    public interface Converter
    {
        public static extern string InsertTags(ParsedText paragraphInfo);
    }
}

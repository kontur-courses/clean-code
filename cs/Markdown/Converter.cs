using Markdown.Tags;

namespace Markdown
{
    public interface Converter
    {
        public static extern string InsertNewTags((string paragraph, List<ITag> tags) paragraphInfo);
    }
}

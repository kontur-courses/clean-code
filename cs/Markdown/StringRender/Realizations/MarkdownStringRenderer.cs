using Markdown.Realizations;

namespace Markdown
{
    public class MarkdownStringRenderer : StringRenderer
    {
        public MarkdownStringRenderer() : base(new MarkdownTokenSelector())
        {
            var emTag = new Tag("em");
            var strongTag = new Tag("strong");
            strongTag.TagDependencies.Add(upperTags => !upperTags.Contains(emTag));
            Tags.Add("__", strongTag);
            Tags.Add("_", emTag);
        }
    }
}
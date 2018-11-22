namespace Markdown.Realizations
{
    public class MarkdownTokenSelector : TokenSelector
    {
        public MarkdownTokenSelector()
        {
            AddDependency(x =>
                !(x.Next != null && x.Next.Value.HasNumber && x.Previous != null && x.Previous.Value.HasNumber));
        }
    }
}
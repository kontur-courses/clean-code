namespace Markdown
{
    public class ParagraphRenderer
    {
        private static readonly BlockTag[] BlockTags = [BlockTag.Header, BlockTag.ListItem];

        private readonly InlineRenderer inlineRenderer = new ();

        public string Render(string paragraph)
        {
            throw new NotImplementedException();
        }

        public bool IsListItem(string paragraph)
        {
            throw new NotImplementedException();
        }

        private static bool TryFindBlockTag(string paragraph, out BlockTag blockTag)
        {
            throw new NotImplementedException();
        }

        private static bool IsBlockTagEscaped(string paragraph)
        {
            throw new NotImplementedException();
        }
    }
}
